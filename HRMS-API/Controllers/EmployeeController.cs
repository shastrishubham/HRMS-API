using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using ServerModel.Data;
using ServerModel.Database;
using ServerModel.Employee;
using ServerModel.Masters;
using ServerModel.Model;
using ServerModel.Model.Base;
using ServerModel.Model.Dashboard;
using ServerModel.Model.Employee;
using ServerModel.Model.ESS;
using ServerModel.Model.Masters;
using ServerModel.Model.Training;
using ServerModel.Repository;
using ServerModel.ServerModel.Dashboard;
using ServerModel.ServerModel.EmployeeSalarySetup;
using ServerModel.ServerModel.Ess;
using ServerModel.ServerModel.Masters.SalaryHeads;
using ServerModel.ServerModel.Training;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeController : ApiController
    {
        EmployeeRespository employeeRespository;
        EmployeeShiftSetupRepository employeeShiftSetupRepository;
        PunchHelper punchHelper;
        EmployeeShiftHelper employeeShiftHelper;
        EmployeeHelper employeeHelper;
        LeaveHelper leaveHelper;
        TrainingInfoServer trainingInfoServer;

        bool isStorageTypeBlob = false;
        private readonly string _blobConnectionString;
        private readonly string _empImgContainerName;

        private readonly string _flaskAPIUrl;

        private static string EMP_IMAGE_UPLOAD_FILE_PATH = System.Web.Hosting.HostingEnvironment.MapPath("~/Images");

        public EmployeeController()
        {
            isStorageTypeBlob = Convert.ToBoolean(ConfigurationManager.AppSettings["IsStorageTypeBlob"].ToString());

            _blobConnectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
            _empImgContainerName = ConfigurationManager.AppSettings["EmployeeImagesContainer"];

            _flaskAPIUrl = ConfigurationManager.AppSettings["FlaskApiUrl"].ToString();

            DbContext.ConnectionString = WebApiConfig.ConnectionString;
            employeeRespository = new EmployeeRespository();
            employeeShiftSetupRepository = new EmployeeShiftSetupRepository();

            punchHelper = new PunchHelper();
            employeeShiftHelper = new EmployeeShiftHelper();
            employeeHelper = new EmployeeHelper();
            leaveHelper = new LeaveHelper();
            trainingInfoServer = new TrainingInfoServer();
        }

        [AllowAnonymous]
        [Route("dashboard")]
        [HttpGet]
        public IHttpActionResult GetDashboard(Guid compId)
        {
            DashboardResponseDto responseDto = DashboardSetupServer.GetDashboard(compId);

            return Ok(responseDto);
        }



        [Route("api/Employee/GetAllEmployees")]
        [HttpGet]
        public IEnumerable<EMP_Info> GetAllEmployee()
        {
            return employeeRespository.GetAll();
        }

        [Route("api/Employee/GetAllEmployeesByCompanyBranch")]
        [HttpGet]
        public List<EmployeeInformation> GetAllEmployeesByCompanyBranch(Guid compId, int branchId)
        {
            return employeeRespository.GetAllEmployeesByCompanyBranch(compId, branchId);
        }

        #region Employee General/Info

        [Route("api/Employee/GetNextEmpId")]
        [HttpGet]
        public int GetNextEmpId(Guid compId)
        {
            return EmployeePersonalInformationServer.GetNextEmpId(compId);
        }

        [Route("api/Employee/AddUpdateEmployee")]
        [HttpPost]
        public DataResult AddUpdateEmployeeInformation(EmployeeInformation employeeInformation)
        {
            return employeeRespository.AddUpdateEmployee(employeeInformation);
        }

        [Route("api/Employee/GetEmployeesByCompanyId")]
        [HttpGet]
        public IEnumerable<EmployeeInformation> GetEmployeesByCompanyId(Guid compId)
        {
            return employeeHelper.GetEmployeesByCompanyId(compId);
        }

        [Route("api/Employee/GetEmployeeInformationById")]
        [HttpGet]
        public EmployeeInformation GetEmployeeInformationById(Guid employeeId)
        {
            return employeeHelper.GetEmployeeInformationById(employeeId);
        }

        [Route("api/Employee/GetEmployeeListByCompanyId")]
        [HttpGet]
        public IEnumerable<EmployeeInformation> GetEmployeeListByCompanyId(Guid compId)
        {
            return employeeHelper.GetEmployeesByCompanyId(compId);
        }

        [Route("api/Employee/GetEmployeesByDesignationAndCompanyId")]
        [HttpGet]
        public IEnumerable<EmployeeInformation> GetEmployeesByDesignationAndCompanyId(int designationId, Guid compId)
        {
            return employeeRespository.GetEmployeesByDesignationAndCompanyId(designationId, compId);
        }

        #endregion

        #region Employee PersonalInfo
        [Route("api/Employee/AddUpdateEmployeePersonalInfo")]
        [HttpPost]
        public DataResult UpsertEmployeePersonalInfo(EmployeePersonalInformation employeePersonalInformation)
        {
            return EmployeePersonalInformationServer.UpsertEmployeePersonalInfo(employeePersonalInformation);
        }

        [Route("api/Employee/GetEmployeePersonalInfoById")]
        [HttpPost]
        public IEnumerable<EmployeePersonalInformation> GetEmployeePersonalInfoById(Guid employeeId)
        {
            return EmployeePersonalInformationServer.GetEmployeePersonalInfoById(employeeId);
        }
        #endregion

        #region Employee AddressInfo
        [Route("api/Employee/UpsertEmployeeAddresses")]
        [HttpPost]
        public async Task<IHttpActionResult> UpsertEmployeeAddresses()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Expected multipart/form-data");

            var provider = await Request.Content.ReadAsMultipartAsync();

            // Get JSON payload
            var jsonPart = provider.Contents.FirstOrDefault(c =>
                c.Headers.ContentDisposition.Name.Trim('"') == "employeeAddressesJson");
            if (jsonPart == null)
                return BadRequest("Missing employeeAddressesJson");

            var jsonString = await jsonPart.ReadAsStringAsync();
            var employeeAddresses = JsonConvert.DeserializeObject<List<EmployeeAddresses>>(jsonString);

            // Get file(s)
            var fileParts = provider.Contents.Where(c => c.Headers.ContentDisposition.FileName != null);

            foreach (var file in fileParts)
            {
                // ContentDisposition.Name is like "file_123"
                var name = file.Headers.ContentDisposition.Name.Trim('"');
                if (name.StartsWith("file_"))
                {
                    var typeIdStr = name.Substring(5); // after "file_"
                    if (int.TryParse(typeIdStr, out int addressTypeId))
                    {
                        var target = employeeAddresses.FirstOrDefault(e => e.AddressTypeId == addressTypeId && e.Active == true);
                        if (target != null)
                        {
                            var fileName = Path.GetFileName(file.Headers.ContentDisposition.FileName.Trim('"'));
                            var root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["EmployeeDocuments"]);
                            if (!Directory.Exists(root))
                                Directory.CreateDirectory(root);

                            string addressProofPath = ConfigurationManager.AppSettings["EmployeeAddressProofDocuments"].ToString();
                            var newFilePath = Path.Combine(root, target.EMP_Info_Id.ToString(), addressProofPath, fileName);
                            // Get the directory part of the path
                            var directoryPath = Path.GetDirectoryName(newFilePath);

                            // Ensure directory exists
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            using (var stream = File.Create(newFilePath))
                            {
                                await file.CopyToAsync(stream);
                            }

                            target.AddressProofDoc = newFilePath;
                        }
                    }
                }
            }


            var result = EmployeeAddressInformationServer.UpsertEmployeeAddresses(employeeAddresses);
            return Ok(result);
        }

        [Route("api/Employee/GetEmployeeAddressesById")]
        [HttpPost]
        public IEnumerable<EmployeeAddresses> GetEmployeeAddressesById(Guid employeeId)
        {
            return EmployeeAddressInformationServer.GetEmployeeAddressesById(employeeId);
        }

        [Route("api/Employee/DownloadAddressProofDocument")]
        [HttpGet]
        public IHttpActionResult DownloadAddressProofDocument(Guid empAddressId)
        {
            // get emp address by id
            EmployeeAddresses employeeAddress = EmployeeAddressInformationServer.GetEmployeeAddressByAddressId(empAddressId);
            if (employeeAddress == null)
                return NotFound();

            if (!System.IO.File.Exists(employeeAddress.AddressProofDoc))
                return NotFound();

            string filePath = employeeAddress.AddressProofDoc;

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string mimeType = MimeMapping.GetMimeMapping(filePath); // auto detect type

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
                {
                    Headers =
            {
                ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                },
                ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType)
            }
                }
            });
        }

        #endregion

        #region Employee Qualification
        [Route("api/Employee/AddUpdateEmployeeQualificationInfo")]
        [HttpPost]
        public DataResult AddUpdateEmployeeQualificationInfo(List<EmployeeQualificationInformation> employeeQualificationInformations)
        {
            return EmployeeQualificationInformationServer.UpsertEmployeeQualificationInfo(employeeQualificationInformations);
        }

        [Route("api/Employee/GetEmployeeQualificationInfoById")]
        [HttpGet]
        public IEnumerable<EmployeeQualificationInformation> GetEmployeeQualificationInfoById(Guid employeeId)
        {
            return EmployeeQualificationInformationServer.GetEmployeeQualificationInfoById(employeeId);
        }
        #endregion

        #region Employee WorkExperience
        [Route("api/Employee/AddUpdateEmployeeWorkExperienceInfo")]
        [HttpPost]
        public DataResult AddUpdateEmployeeWorkExperienceInfo(List<EmployeeWorkExperienceInformation> employeeWorkExperienceInformations)
        {
            return EmployeeWorkExperienceInformationServer.UpsertEmployeeWorkExpInfo(employeeWorkExperienceInformations);
        }

        [Route("api/Employee/GetEmployeeWorkExpInfoById")]
        [HttpGet]
        public IEnumerable<EmployeeWorkExperienceInformation> GetEmployeeWorkExpInfoById(Guid employeeId)
        {
            return EmployeeWorkExperienceInformationServer.GetEmployeeWorkExpInfoById(employeeId);
        }
        #endregion

        #region Employee FamilyInformation
        [Route("api/Employee/AddUpdateEmployeeFamilyInfo")]
        [HttpPost]
        public DataResult AddUpdateEmployeeFamilyInfo(List<EmployeeFamilyInformation> employeeFamilyInformations)
        {
            return EmployeeFamilyInformationServer.UpsertEmployeeFamilyInfo(employeeFamilyInformations);
        }

        [Route("api/Employee/GetEmployeeFamilyInfoById")]
        [HttpGet]
        public IEnumerable<EmployeeFamilyInformation> GetEmployeeFamilyInfoById(Guid employeeId)
        {
            return EmployeeFamilyInformationServer.GetEmployeeFamilyInfoById(employeeId);
        }
        #endregion

        #region Upload Employee Document

        [Route("api/Employee/AddUpdateEmployeeDocument")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUpdateEmployeeDocument()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Unsupported media type.");

            var root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["EmployeeDocuments"]);
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            var provider = new MultipartFormDataStreamProvider(root);
            await Request.Content.ReadAsMultipartAsync(provider);

            // 1️⃣ Get JSON object (your "emp-doc-form")
            var empDocJson = provider.FormData["emp-doc-form"];
            if (string.IsNullOrEmpty(empDocJson))
                return BadRequest("Employee document data missing.");

            EmployeeDocument employeeDoc = JsonConvert.DeserializeObject<EmployeeDocument>(empDocJson);

            // 2️⃣ Get File (your "doc")
            var fileData = provider.FileData.FirstOrDefault();
            if (fileData != null)
            {
                var originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('"');
                var newFilePath = Path.Combine(root, employeeDoc.EMP_Info_Id.ToString(), employeeDoc.DocsKey, originalFileName);
                // Get the directory part of the path
                var directoryPath = Path.GetDirectoryName(newFilePath);

                // Ensure directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.Copy(fileData.LocalFileName, newFilePath, true); // overwrite = true
                employeeDoc.DocsValue = newFilePath;
            }

            EmployeeDocument existingEmpDoc = null;
            List<EmployeeDocument> existingEmpDocs = EmployeeDocumentServer.GetEmployeeDocuments(employeeDoc.EMP_Info_Id ?? Guid.Empty);
            if (existingEmpDocs != null && existingEmpDocs.Count > 0)
            {
                existingEmpDoc = existingEmpDocs.FirstOrDefault(x => x.DocsKey.Equals(employeeDoc.DocsKey, StringComparison.CurrentCultureIgnoreCase));
                if (existingEmpDoc != null)
                    employeeDoc = existingEmpDoc;
            }

            bool resp = EmployeeDocumentServer.UpsertEmployeeDocument(employeeDoc);
            return Ok(resp);
        }

        [Route("api/Employee/DownloadEmployeeDocument")]
        [HttpGet]
        public IHttpActionResult DownloadDocument(Guid id)
        {
            // get emp docs
            EmployeeDocument employeeDocument = EmployeeDocumentServer.GetEmployeeDocument(id);
            if (employeeDocument == null)
                return NotFound();

            if (!System.IO.File.Exists(employeeDocument.DocsValue))
                return NotFound();

            string filePath = employeeDocument.DocsValue;

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string mimeType = MimeMapping.GetMimeMapping(filePath); // auto detect type

            return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
                {
                    Headers =
            {
                ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                },
                ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType)
            }
                }
            });
        }

        [Route("api/Employee/GetEmployeeDocuments")]
        [HttpGet]
        public List<EmployeeDocument> GetEmployeeDocuments(Guid employeeId)
        {
            return EmployeeDocumentServer.GetEmployeeDocuments(employeeId);
        }

        #endregion

        #region Employee BankInformation
        [Route("api/Employee/AddUpdateEmployeeBankInfo")]
        [HttpPost]
        public int AddUpdateEmployeeBankInfo(EmployeeBankInformation employeeBankInformation)
        {
            return EmployeeBankInformationServer.UpsertEmployeeBankInfo(employeeBankInformation);
        }

        [Route("api/Employee/GetEmployeeBankInfoById")]
        [HttpGet]
        public EmployeeBankInformation GetEmployeeBankInfoById(Guid employeeId, Guid compId)
        {
            return EmployeeBankInformationServer.GetEmployeeBankInfoById(employeeId, compId);
        }
        #endregion

        #region Employee Shift Information
        [Route("api/Employee/AddUpdateEmployeeShiftInformation")]
        [HttpPost]
        public void AddUpdateEmployeeShiftInformation(EmployeeShiftInformation employeeShiftInformation)
        {
            employeeShiftSetupRepository.AddUpdateEmployeeShiftInformation(employeeShiftInformation);
        }

        [Route("api/Employee/GetEmployeeShiftDetailByBranchId")]
        [HttpGet]
        public List<EmployeeShiftInformation> GetEmployeeShiftDetailByBranchId(Guid compId, int branchId)
        {
            return employeeShiftHelper.GetEmployeeShiftDetailByBranchId(compId, branchId);
        }

        [Route("api/Employee/GetEmployeeShiftInformation")]
        [HttpGet]
        public EmployeeShiftInformation GetEmployeeShiftInformation(Guid compId, Guid empId)
        {
            return employeeShiftHelper.GetEmployeeShiftInformation(compId, empId);
        }

        [Route("api/Employee/GetEmlployeeShiftByBranchAndShift")]
        [HttpGet]
        public List<EmployeeShiftInformation> GetEmlployeeShiftByBranchAndShift(int branchId, int shiftId)
        {
            return employeeShiftHelper.GetEmlployeeShiftByBranchAndShift(branchId, shiftId);
        }


        #endregion

        #region Employee Leave Setup
        [Route("api/Employee/AddUpdateEmployeeLeaveSetup")]
        [HttpPost]
        public bool AddUpdateEmployeeLeaveSetup(EmployeeLeaves employeeLeave)
        {
            return EssSetupServer.AddUpdateEmployeeLeaves(employeeLeave);
        }

        [Route("api/Employee/GetEmployeesLeaveDetails")]
        [HttpGet]
        public IEnumerable<EmployeeLeaveSetupInformation> GetEmployeesLeaveDetails(Guid compId)
        {
            return employeeHelper.GetEmployeesLeaveDetails(compId);
        }

        [Route("api/Employee/DeleteEmployeeLeaveSetup")]
        [HttpPost]
        public DataResult DeleteEmployeeLeaveSetup(EmployeeLeaveSetupInformation employeeLeaveSetupInformation)
        {
            return employeeHelper.DeleteEmployeeLeaveSetup(employeeLeaveSetupInformation);
        }
        #endregion

        #region Employee Leave Request
        [Route("api/Employee/AddUpdateCreateEmployeeLeaveReqest")]
        [HttpPost]
        public IHttpActionResult AddUpdateCreateEmployeeLeaveReqest()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                // 1. Get uploaded file (from key: "resume")
                HttpPostedFile postedFile = httpRequest.Files["doc"];

                // 2. Get model from form-data
                string empLeaveReqlJson = httpRequest.Form["emp-leave-req-form"];
                if (string.IsNullOrEmpty(empLeaveReqlJson))
                    return BadRequest("Model is missing.");

                EmployeeLeaveRequestInformation employeeLeaveRequestInformation = JsonConvert.DeserializeObject<EmployeeLeaveRequestInformation>(empLeaveReqlJson);
                string filePath = null;
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    // 3. Save file
                    string relativePath = ConfigurationManager.AppSettings["LeaveDocuments"];
                    string rootPath = HttpContext.Current.Server.MapPath(relativePath);
                    string userFolder = Path.Combine(rootPath, employeeLeaveRequestInformation.EMP_Info_Id.ToString());
                    if (Directory.Exists(userFolder))
                    {
                        // Directory already exists
                    }
                    else
                    {
                        // Directory does not exist, create it
                        Directory.CreateDirectory(userFolder);
                    }

                    filePath = Path.Combine(userFolder, Path.GetFileName(postedFile.FileName));
                    postedFile.SaveAs(filePath);

                    employeeLeaveRequestInformation.Docs = filePath;
                }
                else
                {
                    // get existing doc path
                    List<EmployeeLeaveRequestInformation> empLeaves = leaveHelper.GetEmployeeLeaveReqByLeaveStatusAndCompId(employeeLeaveRequestInformation.CompId ?? Guid.Empty, LeaveStatusType.Pending).ToList();
                    if (empLeaves != null && empLeaves.Any() && (employeeLeaveRequestInformation.Id != null || employeeLeaveRequestInformation.Id != Guid.Empty))
                    {
                        EmployeeLeaveRequestInformation employeeLeaveRequest = empLeaves.FirstOrDefault(x => x.Id == employeeLeaveRequestInformation.Id);
                        if (employeeLeaveRequest != null)
                        {
                            employeeLeaveRequestInformation.Docs = employeeLeaveRequest.Docs;
                        }
                    }
                }

                // 4. Call your business logic
                DataResult result = leaveHelper.AddUpdateCreateEmployeeLeaveReqest(employeeLeaveRequestInformation);

                return Ok(new { DataResult = result });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Employee/DeleteEmployeeLeaveRequest")]
        [HttpPost]
        public DataResult DeleteEmployeeLeaveRequest(EmployeeLeaveRequestInformation employeeLeaveRequestInformation)
        {
            return leaveHelper.DeleteEmployeeLeaveRequest(employeeLeaveRequestInformation);
        }

        [Route("api/Employee/GetEmployeeLeaveReqByLeaveStatusAndCompId")]
        [HttpGet]
        public IEnumerable<EmployeeLeaveRequestInformation> GetEmployeeLeaveReqByLeaveStatusAndCompId(Guid compId, LeaveStatusType leaveStatusType)
        {
            return leaveHelper.GetEmployeeLeaveReqByLeaveStatusAndCompId(compId, leaveStatusType);
        }

        [Route("api/Employee/GetEmployeeApprovedLeaveRequests")]
        [HttpGet]
        public IEnumerable<EmployeeLeaveRequestInformation> GetEmployeeApprovedLeaveRequests(Guid compId)
        {
            return leaveHelper.GetEmployeeApprovedLeaveRequests(compId);
        }

        [Route("api/Employee/GetAvailableLeavesForEmployee")]
        [HttpGet]
        public decimal GetAvailableLeavesForEmployee(Guid employeeId)
        {
            return leaveHelper.GetAvailableLeavesForEmployee(employeeId);
        }

        [Route("api/Employee/UpdateEmployeeLeaveRequestStatusByIdAndStatus")]
        [HttpPost]
        public DataResult UpdateEmployeeLeaveRequestStatusByIdAndStatus(Guid approverEmpId, Guid requestId, LeaveStatusType leaveStatusType)
        {
            return EssSetupServer.UpdateEmployeeLeaveRequestStatusByIdAndStatus(approverEmpId, requestId, leaveStatusType);
        }

        [Route("api/Employee/GetEmployeeLeaveRequestsByCompId")]
        [HttpGet]
        public IEnumerable<EmployeeLeaveRequestInformation> GetEmployeeLeaveRequestsByCompId(Guid compId)
        {
            return EssSetupServer.GetEmployeeLeaveRequestsByCompId(compId);
        }

        #endregion

        #region Employee Reimbursement

        [Route("api/Employee/GetReimbursementClaimsByCompId")]
        [HttpGet]
        public List<ReimbursementClaims> GetReimbursementClaimsByCompId(Guid compId)
        {
            return EssSetupServer.GetReimbursementClaimsByCompId(compId);
        }

        [Route("api/Employee/GetReimbursementClaimsByCompIdAndBranchId")]
        [HttpGet]
        public List<ReimbursementClaims> GetReimbursementClaimsByCompIdAndBranchId(Guid compId, int branchId)
        {
            return EssSetupServer.GetReimbursementClaimsByCompIdAndBranchId(compId, branchId);
        }

        [Route("api/Employee/UpsertReimbursementClaims")]
        [HttpPost]
        public IHttpActionResult UpsertReimbursementClaims()
        {
            var httpRequest = HttpContext.Current.Request;

            // 1. Get uploaded file (from key: "reimbursement doc")
            HttpPostedFile postedFile = httpRequest.Files["doc"];

            // 2. Get model from form-data
            string reimbursementlJson = httpRequest.Form["reimbursement-form"];
            if (string.IsNullOrEmpty(reimbursementlJson))
                return BadRequest("Model is missing.");

            ReimbursementClaims reimbursementClaim = JsonConvert.DeserializeObject<ReimbursementClaims>(reimbursementlJson);
            string filePath = null;
            if (postedFile != null && postedFile.ContentLength > 0)
            {
                string relativePath = ConfigurationManager.AppSettings["ReimbursementDocuments"];
                string rootPath = HttpContext.Current.Server.MapPath(relativePath);
                string userFolder = Path.Combine(rootPath, reimbursementClaim.EMP_Info_Id.ToString());
                if (Directory.Exists(userFolder))
                {
                    // Directory already exists
                }
                else
                {
                    // Directory does not exist, create it
                    Directory.CreateDirectory(userFolder);
                }

                filePath = Path.Combine(userFolder, Path.GetFileName(postedFile.FileName));
                postedFile.SaveAs(filePath);

                reimbursementClaim.BillPath = filePath;
            }
            else
            {
                // get existing doc path
                List<ReimbursementClaims> empReimbursements = EssSetupServer.GetReimbursementClaimsByCompId(reimbursementClaim.CompId.Value);
                if (empReimbursements != null && empReimbursements.Any())
                {
                    ReimbursementClaims empReimbursement = empReimbursements.FirstOrDefault(x => x.Id == reimbursementClaim.Id);
                    if (empReimbursement != null)
                    {
                        reimbursementClaim.BillPath = empReimbursement.BillPath;
                    }
                }
            }


            DataResult res = EssSetupServer.UpsertReimbursementClaims(reimbursementClaim);

            return Ok(new { DataResult = res });
        }

        [Route("api/Employee/ApprovedReimbursementClaim")]
        [HttpPost]
        public DataResult ApprovedReimbursementClaim(ReimbursementApproverModel reimbursementApproverModel)
        {
            return EssSetupServer.ApprovedReimbursementClaim(reimbursementApproverModel);
        }

        #endregion

        #region EmployeeSalarySetup

        [Route("api/Employee/AddUpdateEmployeeSalarySetup")]
        [HttpPost]
        public int AddUpdateEmployeeSalarySetup(List<EmployeeSalarySetupDetails> employeeSalarySetupDetails)
        {
            return EmployeeSalarySetupServer.AddUpdateEmployeeSalarySetup(employeeSalarySetupDetails);
        }

        [Route("api/Employee/GetSalarySetupByCompId")]
        [HttpGet]
        public List<EmployeeSalarySetupDetails> GetSalarySetupByCompId(Guid compId)
        {
            return EmployeeSalarySetupServer.GetSalarySetupByCompId(compId);
        }


        [Route("api/Employee/GetEmployeeSalarySetupDetails")]
        [HttpGet]
        public List<EmployeeSalaryHeadsSetupDetails> GetEmployeeSalarySetupDetails(Guid compId, Guid empId)
        {
            return EmployeeSalarySetupServer.GetEmployeeSalarySetupDetails(compId, empId);
        }

        [Route("api/Employee/GetUnSetupSalaryEmployeesByDesignationId")]
        [HttpGet]
        public IEnumerable<EmployeeSalarySetupDetails> GetUnSetupSalaryEmployeesByDesignationId(Guid compId, int designationId)
        {
            return EmployeeSalarySetupServer.GetUnSetupSalaryEmployeesByDesignationId(compId, designationId);
        }

        #endregion

        #region Employee Image 
        [HttpPost]
        [Route("api/Employee/UploadEmployeeImage1")]
        public async Task<HttpResponseMessage> UploadEmployeeImage1()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Message = "Unsupported media type."
                });
            }

            try
            {

                // Temporary storage for uploaded files
                var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());
                await Request.Content.ReadAsMultipartAsync(provider);


                //// ✅ Define upload path (for local storage)
                //var uploadPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads");
                //if (!Directory.Exists(uploadPath))
                //    Directory.CreateDirectory(uploadPath);

                //var provider = new MultipartFormDataStreamProvider(uploadPath);

                //// ✅ Parse multipart request
                //await Request.Content.ReadAsMultipartAsync(provider);

                // ✅ Extract the JSON data (Angular side sends emp-photo-upload)
                var jsonData = provider.FormData["emp-photo-upload"];
                if (string.IsNullOrEmpty(jsonData))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Employee model missing");

                // ✅ Extract flag (optional)
                bool isStorageTypeBlob = false;
                if (provider.FormData.AllKeys.Contains("isStorageTypeBlob"))
                    bool.TryParse(provider.FormData["isStorageTypeBlob"], out isStorageTypeBlob);

                // ✅ Deserialize model
                EmployeeImages empImg = JsonConvert.DeserializeObject<EmployeeImages>(jsonData);

                // ✅ Check file
                var fileData = provider.FileData.FirstOrDefault();
                if (fileData == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "No file uploaded");

                string originalFileName = fileData.Headers.ContentDisposition.FileName?.Trim('"');
                string extension = Path.GetExtension(originalFileName);
                string newFileName = $"Emp_{empImg.EMP_Info_Id}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                string fileUrl = string.Empty;

                // ✅ If upload to Azure Blob
                if (isStorageTypeBlob)
                {
                    fileUrl = await UploadToAzureBlobAsync(fileData);

                    bool isSuccess = employeeHelper.UpsertEmployeeImage(empImg);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        Message = isSuccess ? "Image uploaded successfully (Blob)" : "DB update failed",
                        FileName = newFileName,
                        FileUrl = fileUrl,
                        IsSuccess = isSuccess,
                        IsBlobStorage = true
                    });
                }
                else
                {
                    // ✅ Local storage
                    fileUrl = SaveToLocalPath(fileData.LocalFileName, newFileName);

                    // ✅ Generate embedding (if applicable)
                    string imgEmbedding = await getImageEmbedding(fileUrl, empImg.Photo);

                    if (string.IsNullOrEmpty(imgEmbedding))
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Embedding not generated");

                    EmplmgEmbedding embedding = new EmplmgEmbedding()
                    {
                        CompId = empImg.CompId,
                        EMP_Info_Id = empImg.EMP_Info_Id,
                        Embedding = imgEmbedding
                    };

                    bool isUpsert = employeeHelper.UpsertEmpImageEmbeddings(embedding);
                    bool isSuccess = employeeHelper.UpsertEmployeeImage(empImg);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        Message = (isUpsert && isSuccess)
                            ? "Image uploaded successfully (Local)"
                            : "Failed to save embedding or image info",
                        FileName = newFileName,
                        FileUrl = fileUrl,
                        IsSuccess = (isUpsert && isSuccess),
                        IsBlobStorage = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost]
        [Route("api/Employee/UploadImageFromFile")]
        public async Task<IHttpActionResult> UploadEmployeeImage()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    return BadRequest("Unsupported media type.");

                // Temporary storage for uploaded files
                var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());
                await Request.Content.ReadAsMultipartAsync(provider);

                // 1️⃣ Get JSON object
                var jsonData = provider.FormData["emp-photo-upload"];
                if (string.IsNullOrEmpty(jsonData))
                    return BadRequest("Employee document data missing.");

                EmployeeImages empImg = JsonConvert.DeserializeObject<EmployeeImages>(jsonData);

                // 2️⃣ Get uploaded file
                var fileData = provider.FileData.FirstOrDefault();
                if (fileData == null)
                    return Content(HttpStatusCode.BadRequest, "No file uploaded");

                string originalFileName = fileData.Headers.ContentDisposition.FileName?.Trim('"');
                string extension = Path.GetExtension(originalFileName);
                string newFileName = $"Emp_{empImg.EMP_Info_Id}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                string fileUrl = string.Empty;

                // ✅ If upload to Azure Blob
                if (isStorageTypeBlob)
                {
                    fileUrl = await UploadToAzureBlobAsync(fileData);

                    // ✅ Generate embedding (if applicable)
                    string imgEmbedding = await getImageEmbedding(fileUrl, empImg.Photo);

                    if (string.IsNullOrEmpty(imgEmbedding))
                        return Content(HttpStatusCode.InternalServerError, "Embedding not generated");

                    bool isUpsert = UpsertEmpImgEmbedding(empImg, imgEmbedding);
                    bool isSuccess = employeeHelper.UpsertEmployeeImage(empImg);

                    return Content(HttpStatusCode.OK, new
                    {
                        Message = (isUpsert && isSuccess)
                            ? "Image uploaded successfully (Blob)"
                            : "Failed to save embedding or image info",
                        FileName = newFileName,
                        FileUrl = fileUrl,
                        IsSuccess = (isUpsert && isSuccess),
                        IsBlobStorage = true
                    });
                }
                else
                {
                    // ✅ Local storage
                    fileUrl = SaveToLocalPath(fileData.LocalFileName, newFileName);

                    // ✅ Generate embedding (if applicable)
                    string imgEmbedding = await getImageEmbedding(fileUrl, empImg.Photo);

                    if (string.IsNullOrEmpty(imgEmbedding))
                        return Content(HttpStatusCode.InternalServerError, "Embedding not generated");

                    bool isUpsert = UpsertEmpImgEmbedding(empImg, imgEmbedding);
                    bool isSuccess = employeeHelper.UpsertEmployeeImage(empImg);

                    return Content(HttpStatusCode.OK, new
                    {
                        Message = (isUpsert && isSuccess)
                            ? "Image uploaded successfully (Local)"
                            : "Failed to save embedding or image info",
                        FileName = newFileName,
                        FileUrl = fileUrl,
                        IsSuccess = (isUpsert && isSuccess),
                        IsBlobStorage = false
                    });
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // Provides full exception details for debugging
            }
        }

        // Save locally (existing behavior)
        private string SaveToLocalPath(string tempFilePath, string newFileName)
        {
            string uploadSetting = ConfigurationManager.AppSettings["EmployeeImageUploadPath"];
            string uploadRoot = uploadSetting.StartsWith("~")
                ? HttpContext.Current.Server.MapPath(uploadSetting)
                : uploadSetting;

            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            string newFilePath = Path.Combine(uploadRoot, newFileName);
            File.Move(tempFilePath, newFilePath);

            // Return file path or URL (depending on need)
            return newFilePath;
        }

        // Upload to Azure Blob
        private async Task<string> UploadToAzureBlobAsync(MultipartFileData fileData)
        {
            // 🔷 Azure Blob Storage Upload
            string connectionString = _blobConnectionString;
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_empImgContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            string originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('"');

            string blobName = $"{originalFileName}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            using (var fileStream = File.OpenRead(fileData.LocalFileName))
            {
                await blobClient.UploadAsync(fileStream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }

        private async Task<string> getImageEmbedding(string filePath, byte[] imageBytes)
        {
            string base64String = Convert.ToBase64String(imageBytes);
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            using (var httpClient = new HttpClient())
            {
                //var jsonData = new
                //{
                //    image_path = filePath // local image path
                //};
                var jsonData = new
                {
                    image_base64 = base64String
                };

                var jsonContent = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(jsonData),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.PostAsync(_flaskAPIUrl, jsonContent);
                var result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Flask API should return JSON like: { "embedding": [0.123, 0.456, ...] }
                    dynamic jsonResponse = JsonConvert.DeserializeObject(result);
                    string embeddingJson = JsonConvert.SerializeObject(jsonResponse.embedding);
                    return embeddingJson;
                }
                else
                {
                    return string.Empty;
                }

                return string.Empty;
            }
        }

        private bool UpsertEmpImgEmbedding(EmployeeImages empImg, string imgEmbedding)
        {
            EmplmgEmbedding embedding = new EmplmgEmbedding()
            {
                CompId = empImg.CompId,
                EMP_Info_Id = empImg.EMP_Info_Id,
                Embedding = imgEmbedding
            };

            bool isUpsert = employeeHelper.UpsertEmpImageEmbeddings(embedding);
            return isUpsert;
        }

        [Route("api/Employee/UploadImageFromFile1")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUpdateOnlineFormApplication()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    return BadRequest("Unsupported media type.");

                // Temporary storage for uploaded files
                var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());
                await Request.Content.ReadAsMultipartAsync(provider);

                // 1️⃣ Get JSON object
                var empDocJson = provider.FormData["job-app-form"];
                if (string.IsNullOrEmpty(empDocJson))
                    return BadRequest("Employee document data missing.");

                EmployeeImages employeeImage = JsonConvert.DeserializeObject<EmployeeImages>(empDocJson);

                // 2️⃣ Get uploaded file
                var fileData = provider.FileData.FirstOrDefault();
                if (fileData != null)
                {
                    string originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('"');

                    // Generate unique filename to avoid overwrite
                    // string uniqueFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_{DateTime.Now.Ticks}{Path.GetExtension(originalFileName)}";

                    // Organize files by designation
                    //DesignationInfo designation = DesignationSetupServer
                    //    .GetDesignationsByCompId(employeeDoc.CompId ?? Guid.Empty)
                    //    .FirstOrDefault(x => x.Id == employeeDoc.MS_Designation_Id);

                    //if (designation == null)
                    //    return BadRequest("Designation not found for the given employee.");

                    if (isStorageTypeBlob)
                    {
                        //// 🔷 Azure Blob Storage Upload
                        //string connectionString = _blobConnectionString;
                        //BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                        //BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_resumeContainerName);
                        //await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                        //string blobName = $"{designation.DesignationName}/{originalFileName}";
                        //BlobClient blobClient = containerClient.GetBlobClient(blobName);

                        //using (var fileStream = File.OpenRead(fileData.LocalFileName))
                        //{
                        //    await blobClient.UploadAsync(fileStream, overwrite: true);
                        //}

                        //employeeDoc.Resume = blobClient.Uri.ToString();
                    }
                    else
                    {
                        // 🔷 Local File System Upload
                        string relativePath = ConfigurationManager.AppSettings["EmployeeImageUploadPath"]; // e.g. "~/UploadedFiles/Employee Images"
                        string physicalBasePath = HostingEnvironment.MapPath(relativePath);

                        // Create folder path: {BasePath}/{DesignationName}
                       // string destinationFolder = Path.Combine(physicalBasePath, designation.DesignationName);
                        Directory.CreateDirectory(physicalBasePath);

                        // Full destination file path
                        string destinationFilePath = Path.Combine(physicalBasePath, originalFileName);

                        // Copy file from temp folder to destination
                        File.Copy(fileData.LocalFileName, destinationFilePath, true);

                        // Save path in model
                       // employeeDoc.Resume = destinationFilePath;
                    }
                }

                // 3️⃣ Preserve existing resume if updating and no new file uploaded
                //if (employeeDoc.Id != Guid.Empty && string.IsNullOrEmpty(employeeDoc.Resume))
                //{
                //    var existingApplications = recruitmentHelper
                //        .GetJobVacanciesApplicationsByCompId(employeeDoc.CompId ?? Guid.Empty)
                //        .ToList();

                //    var existingEmpDoc = existingApplications?.FirstOrDefault(x => x.Id == employeeDoc.Id);
                //    if (!string.IsNullOrEmpty(existingEmpDoc?.Resume))
                //        employeeDoc.Resume = existingEmpDoc.Resume;
                //}

                // 4️⃣ Save application
                //DataResult result = recruitmentHelper.AddUpdateOnlineFormApplication(employeeDoc);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // Provides full exception details for debugging
            }
        }

        #endregion

        #region PunchEntry
        //[Route("api/Employee/AddUpdateEmployeePunch")]
        //[HttpPost]
        //public void AddUpdateEmployeePunch(EmployeePunchInformation employeePunchInformation)
        //{
        //    punchHelper.AddUpdateEmployeePunch(employeePunchInformation);
        //}


        #endregion

        #region Employee Training Setup

        [Route("api/Employee/UpsertEmployeeTraining")]
        [HttpPost]
        public Guid UpsertEmployeeTraining(EmployeeTrainingInfo employeeTraining)
        {
            return trainingInfoServer.UpsertEmployeeTraining(employeeTraining);
        }

        [Route("api/Employee/GetEmployeeTrainingsByCompId")]
        [HttpGet]
        public List<EmployeeTrainingInfo> GetEmployeeTrainingsByCompId(Guid compId)
        {
            return trainingInfoServer.GetEmployeeTrainingsByCompId(compId);
        }

        #endregion


        #region Face Detection



        #endregion Face Detection


        #region Employee Loan Request
        [HttpPost]
        [Route("api/Employee/UpsertEmployeeLoanRequest")]
        public Guid UpsertEmployeeLoanRequest(EmpLoanRequest empLoanRequest)
        {
            return EssSetupServer.UpsertEmployeeLoanRequest(empLoanRequest);
        }

        [HttpGet]
        [Route("api/Employee/GetEmployeeLoanRequestsByCompId")]
        public List<EmpLoanRequest> GetEmployeeLoanRequestsByCompId(Guid compId)
        {
            return EssSetupServer.GetEmployeeLoanRequestsByCompId(compId);
        }

        #endregion
    }
}
