using ServerModel.Masters;
using ServerModel.Model.Masters;
using ServerModel.Repository;
using ServerModel.ServerModel.Masters.BankSetup;
using ServerModel.ServerModel.Masters.CompanySetup;
using ServerModel.ServerModel.Masters.DepartmentSetup;
using ServerModel.ServerModel.Masters.DesignationSetup;
using ServerModel.ServerModel.Masters.LeaveSetup;
using ServerModel.ServerModel.Masters.LocationSetup;
using ServerModel.ServerModel.Masters.SalaryHeads;
using ServerModel.ServerModel.Masters.ShiftSetup;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using ServerModel.ServerModel.Masters.BankBranchSetup;
using ServerModel.ServerModel.Masters.InterviewRatingSetup;
using ServerModel.ServerModel.Masters.TrainingSetup;
using ServerModel.ServerModel.Masters.DocumentSetup;
using System.Configuration;
using System.Web.Hosting;
using System.Net;
using System.Web;
using ServerModel.ServerModel.Masters.AssetSetup;
using ServerModel.Data;
using Newtonsoft.Json;
using ServerModel.Model;
using ServerModel.ServerModel.Masters.ReimbursementSetup;
using ServerModel.ServerModel.Masters.TicketCategory;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Linq;
using ServerModel.ServerModel.Masters.LoanTypeSetup;
using ServerModel.ServerModel.Masters.PayrollAdjustmentType;
using ServerModel.ServerModel.Helper;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MastersSetupController : ApiController
    {
        SalaryHeadsRepository salaryHeadsRepository;
        DesignationSetupRepository designationSetupRepository;
        BranchSetupRespository branchSetupRespository;
        DepartmentSetupRepository departmentSetupRepository;
        LeaveTypeInformationRepository leaveTypeInformationRepository;

        BranchSetupServerModel branchSetupServerModel;

        private readonly string _blobConnectionString;
        private readonly string _CompContainerName;
        private readonly string _CompDocContainerName;
        private readonly string _CompMasterTempContainerName;
        bool isStorageTypeBlob = false;

        public MastersSetupController()
        {
            isStorageTypeBlob = Convert.ToBoolean(ConfigurationManager.AppSettings["IsStorageTypeBlob"].ToString());

            _blobConnectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
            _CompContainerName = ConfigurationManager.AppSettings["CompContainer"];
            _CompDocContainerName = ConfigurationManager.AppSettings["CompDocsContainer"];
            _CompMasterTempContainerName = ConfigurationManager.AppSettings["CompMasterTempContainer"];

            DbContext.ConnectionString = WebApiConfig.ConnectionString;
            branchSetupServerModel = new BranchSetupServerModel();
            salaryHeadsRepository = new SalaryHeadsRepository();
            designationSetupRepository = new DesignationSetupRepository();
            branchSetupRespository = new BranchSetupRespository();
            departmentSetupRepository = new DepartmentSetupRepository();
            leaveTypeInformationRepository = new LeaveTypeInformationRepository();
        }

        #region CompanySetup

        [Route("api/Masters/GetCountries")]
        [HttpGet]
        public List<Countries> GetCountries()
        {
            return CompanySetupServer.GetCountries();
        }

        [Route("api/Masters/GetStatesByCountryId")]
        [HttpGet]
        public List<States> GetStatesByCountryId(int countryId)
        {
            return CompanySetupServer.GetStatesByCountryId(countryId);
        }

        [Route("api/Masters/GetCitiesByStateId")]
        [HttpGet]
        public List<Cities> GetCitiesByStateId(int stateId)
        {
            return CompanySetupServer.GetCitiesByStateId(stateId);
        }


        [Route("api/Masters/UpsertCompanyRegistration")]
        [HttpPost]
        public Guid UpsertCompanyRegistration(CompanyRegistration companyRegistration)
        {
            return CompanySetupServer.UpsertCompanyRegistration(companyRegistration);
        }

        [Route("api/Masters/UploadCompanyDocument")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadCompanyDocument()
        {
            try
            {
                DataResult res = null;
                var httpRequest = System.Web.HttpContext.Current.Request;

                // 1. Get uploaded file (from key: "doc")
                HttpPostedFile postedFile = httpRequest.Files["doc"];

                // 2. Get model from form-data
                string companyDocumentJson = httpRequest.Form["comp-doc-form"];
                if (string.IsNullOrEmpty(companyDocumentJson))
                    return BadRequest("Model is missing.");

                CompanyDocument companyDocument = JsonConvert.DeserializeObject<CompanyDocument>(companyDocumentJson);
                string filePath = null;
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    List<CompanyRegistration> comps = CompanySetupServer.GetCompanyRegistrationDetailsById(companyDocument.CompId ?? Guid.Empty);
                    if (comps != null && comps.Count > 0)
                    {
                        CompanyRegistration compDetail = comps.FirstOrDefault();
                        // 3. Save file
                        if (isStorageTypeBlob)
                        {
                            // Setup Blob Storage
                            string connectionString = _blobConnectionString;
                            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_CompContainerName);
                            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                            string blobName = $"{companyDocument.CompId.ToString()}/{_CompDocContainerName}/{companyDocument.DocumentName.ToString()}/{Path.GetFileName(postedFile.FileName)}";
                            BlobClient blobClient = containerClient.GetBlobClient(blobName);

                            // Upload file to Blob
                            var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
                            {
                                ContentType = postedFile.ContentType
                            };

                            using (var stream = postedFile.InputStream)
                            {
                                await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
                                {
                                    HttpHeaders = blobHttpHeaders
                                });
                            }

                            companyDocument.DocumentPath = blobClient.Uri.ToString();
                        }
                        else
                        {
                            // 🔷 Local File System Upload
                            string relativePath = ConfigurationManager.AppSettings["CompanyDocuments"]; // e.g. "~/CompanyDocs"
                            string physicalBasePath = HostingEnvironment.MapPath(relativePath);

                            // Create folder path: {BasePath}/{CompId}/{DocumentName}
                            string destinationFolder = Path.Combine(physicalBasePath, companyDocument.CompId.ToString(), companyDocument.DocumentName.ToString());
                            Directory.CreateDirectory(destinationFolder);

                            // Full destination file path
                            string destinationFilePath = Path.Combine(destinationFolder, Path.GetFileName(postedFile.FileName));

                            // Save file
                            using (var memoryStream = new MemoryStream())
                            {
                                postedFile.InputStream.CopyTo(memoryStream);
                                File.WriteAllBytes(destinationFilePath, memoryStream.ToArray());
                            }

                            companyDocument.DocumentPath = destinationFilePath;
                        }

                        // 4. Call your business logic
                        bool isUpload = CompanySetupServer.UploadCompanyDocument(companyDocument);
                        if (isUpload)
                        {
                            res = new DataResult
                            {
                                IsSuccess = true
                            };
                            return Ok(new { DataResult = res });
                        }
                    }
                }
                res = new DataResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Upload Document Failed"
                };
                return Ok(new { DataResult = res });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Masters/UpsertCompanyPlanDetail")]
        [HttpPost]
        public bool UpsertCompanyPlanDetail(CompanyPlanDetail companyPlanDetail)
        {
            return CompanySetupServer.UpsertCompanyPlanDetail(companyPlanDetail);
        }

        [Route("api/Masters/GetCompanyRegistrationDetails")]
        [HttpGet]
        public List<CompanyRegistration> GetCompanyRegistrationDetails()
        {
            return CompanySetupServer.GetCompanyRegistrationDetails();
        }

        [Route("api/Masters/GetCompanyRegistrationDetailsById")]
        [HttpGet]
        public List<CompanyRegistration> GetCompanyRegistrationDetailsById(Guid companyId)
        {
            return CompanySetupServer.GetCompanyRegistrationDetailsById(companyId);
        }

        [Route("api/Masters/GetCompDocsByCompId")]
        [HttpGet]
        public List<CompanyDocument> GetCompDocsByCompId(Guid compId)
        {
            return CompanySetupServer.GetCompDocsByCompId(compId);
        }

        [Route("api/Masters/GetCompanyPlanDetailsByCompId")]
        [HttpGet]
        public List<CompanyPlanDetail> GetCompanyPlanDetailsByCompId(Guid compId)
        {
            return CompanySetupServer.GetCompanyPlanDetailsByCompId(compId);
        }

        #endregion

        #region BranchSetup


        [Route("api/Masters/UpsertLocationSetup")]
        [HttpPost]
        public int UpsertLocationSetup(LocationRegistration locationRegistration)
        {
            return LocationSetupServer.UpsertLocationSetup(locationRegistration);
        }

        [Route("api/Masters/GetLocationDetails")]
        [HttpGet]
        public LocationRegistration GetLocationDetails(int locationId)
        {
            return LocationSetupServer.GetLocationDetails(locationId);
        }

        [Route("api/Masters/GetLocationsByCompId")]
        [HttpGet]
        public List<LocationRegistration> GetLocationsByCompId(Guid companyId)
        {
            return LocationSetupServer.GetLocationsByCompId(companyId);
        }

        [Route("api/CreateBranch")]
        [HttpPost]
        public void CreateBranch(BranchInfo branchInfo)
        {
            branchSetupServerModel.AddUpdateBranch(branchInfo);
        }

        [Route("api/MastersSetup/GetAllBranchesByCompany")]
        [HttpGet]
        public IEnumerable<BranchInformation> GetAllBranchesByCompany(Guid compId)
        {
            return branchSetupRespository.GetAllBranchesByCompany(compId);
        }

        #endregion

        #region DepartmentSetup

        [Route("api/Masters/UpsertDepartmentSetup")]
        [HttpPost]
        public int UpsertDepartmentSetup(DepartmentRegistration departmentRegistration)
        {
            return DepartmentSetupServer.UpsertDepartmentSetup(departmentRegistration);
        }

        [Route("api/Masters/GetDepartmentDetails")]
        [HttpGet]
        public DepartmentRegistration GetDepartmentDetails(int locationId)
        {
            return DepartmentSetupServer.GetDepartmentDetails(locationId);
        }

        [Route("api/Masters/GetDepartmentsByCompId")]
        [HttpGet]
        public List<DepartmentRegistration> GetDepartmentsByCompId(Guid companyId)
        {
            return DepartmentSetupServer.GetDepartmentsByCompId(companyId);
        }

        #endregion

        #region SalaryHeadsSetup

        [Route("api/Masters/GetSalaryHeadsByCompId")]
        [HttpGet]
        public List<SalaryHeadsInfo> GetSalaryHeadsByCompId(Guid companyId)
        {
            return SalaryHeadsServer.GetSalaryHeadsByCompId(companyId);
        }


        [Route("api/Masters/UpsertSalaryHeads")]
        [HttpPost]
        public int UpsertSalaryHeads(SalaryHeadsInfo salaryHeadsInfo)
        {
            return SalaryHeadsServer.UpsertSalaryHeads(salaryHeadsInfo);
        }

        #endregion

        #region DesignationSetup

        [Route("api/Masters/GetDesignationsByCompId")]
        [HttpGet]
        public List<DesignationInfo> GetDesignationsByCompId(Guid companyId)
        {
            return DesignationSetupServer.GetDesignationsByCompId(companyId);
        }

        [Route("api/Masters/UpsertDesignation")]
        [HttpPost]
        public int UpsertDesignation(DesignationInfo designationInfo)
        {
            return DesignationSetupServer.UpsertDesignation(designationInfo);
        }

        [Route("api/MastersSetup/GetAllDesignationsByCompId")]
        [HttpGet]
        public IEnumerable<DesignationInfo> GetAllDesignationsByCompId(Guid compId)
        {
            return designationSetupRepository.GetAllDesignationByCompId(compId);
        }


        #endregion

        #region BankSetup

        [Route("api/Masters/GetBanksByCompId")]
        [HttpGet]
        public List<BankInfo> GetBanksByCompId(Guid companyId)
        {
            return BankSetupServer.GetBanksByCompId(companyId);
        }

        [Route("api/Masters/UpsertBank")]
        [HttpPost]
        public int UpsertBank(BankInfo bankInfo)
        {
            return BankSetupServer.UpsertBank(bankInfo);
        }

        #endregion

        #region BankBranchSetup

        [Route("api/Masters/GetBranchesByBankAndCompId")]
        [HttpGet]
        public List<BankBranches> GetBranchesByBankAndCompId(Guid companyId, int bankId)
        {
            return BankBranchServer.GetBranchesByBankAndCompId(companyId, bankId);
        }

        [Route("api/Masters/GetBankBranchesByCompId")]
        [HttpGet]
        public List<BankBranches> GetBankBranchesByCompId(Guid companyId)
        {
            return BankBranchServer.GetBankBranchesByCompId(companyId);
        }

        [Route("api/Masters/UpsertBankBranch")]
        [HttpPost]
        public int UpsertBankBranch(BankBranches bankBranch)
        {
            return BankBranchServer.UpsertBankBranch(bankBranch);
        }

        #endregion

        #region InterviewRatingSetup

        [Route("api/Masters/GetInterviewRating")]
        [HttpGet]
        public List<InterviewRatingInfo> GetInterviewRating(Guid companyId)
        {
            return InterviewRatingSetupServer.GetInterviewRating(companyId);
        }

        [Route("api/Masters/UpsertInterviewRating")]
        [HttpPost]
        public int UpsertInterviewRating(InterviewRatingInfo interviewRatingInfo)
        {
            return InterviewRatingSetupServer.UpsertInterviewRating(interviewRatingInfo);
        }

        #endregion

        #region TrainingSetup

        [Route("api/Masters/GetTrainingsByCompId")]
        [HttpGet]
        public List<TrainingInfo> GetTrainingsByCompId(Guid companyId)
        {
            return TrainingSetupServer.GetTrainingsByCompId(companyId);
        }

        [Route("api/Masters/GetTrainingsByDesignationIdAndCompId")]
        [HttpGet]
        public List<TrainingInfo> GetTrainingsByDesignationIdAndCompId(Guid compId, int designationId)
        {
            return TrainingSetupServer.GetTrainingsByDesignationIdAndCompId(compId, designationId);
        }

        [Route("api/Masters/UpsertTrainingSetup")]
        [HttpPost]
        public int UpsertTrainingSetup(TrainingInfo trainingInfo)
        {
            return TrainingSetupServer.UpsertTrainingSetup(trainingInfo);
        }

        #endregion

        #region MastersTemplates
        [HttpPost]
        [Route("UploadMasterTemplate1")]
        public async Task<IHttpActionResult> UploadMasterTemplate1()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest("Unsupported media type.");
            }

            var provider = new MultipartFormDataStreamProvider(System.Web.Hosting.HostingEnvironment.MapPath("~/UploadedFiles"));

            try
            {
                // Await the task to prevent deadlock
                await Request.Content.ReadAsMultipartAsync(provider);

                // Request.Content.ReadAsMultipartAsync(provider).Wait();
                var file = provider.FileData[0];
                if (file == null)
                {
                    return BadRequest("No file uploaded.");
                }

                // Read virtual path from config and convert to physical
                string relativePath = ConfigurationManager.AppSettings["MastersTamplatePath"];
                string physicalPath = HostingEnvironment.MapPath(relativePath);

                // Ensure directory exists
                Directory.CreateDirectory(physicalPath);

                string filePath = Path.Combine(physicalPath, file.Headers.ContentDisposition.FileName.Trim('\"'));
                File.Move(file.LocalFileName, filePath);

                var destinationPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/UploadedFiles"), file.Headers.ContentDisposition.FileName.Trim('\"'));
                File.Move(file.LocalFileName, destinationPath);

                DocumentUploadInfo documentUpload = new DocumentUploadInfo
                {
                    Id = 0,
                    CompId = Guid.Empty,
                    DocName = "name",
                    DocPath = destinationPath
                };

                int id = DocumentSetupServer.UpsertDocument(documentUpload);

                return Ok(new { FilePath = destinationPath });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("UploadMasterTemplate")]
        public async Task<HttpResponseMessage> UploadMasterTemplate()
        {
            if (!Request.Content.IsMimeMultipartContent())
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, "Invalid request format.");

            // Read multipart form data one time
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            DocumentUploadInfo documentUpload = new DocumentUploadInfo();
            HttpContent fileContent = null;

            // Extract form fields and file
            foreach (var content in provider.Contents)
            {
                var contentDisposition = content.Headers.ContentDisposition;
                var name = contentDisposition?.Name?.Trim('"');

                if (name == "file")
                {
                    fileContent = content;
                }
                else if (name == "DocName")
                {
                    documentUpload.DocName = await content.ReadAsStringAsync();
                }
                else if (name == "CompId")
                {
                    string compId = await content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(compId))
                    {
                        documentUpload.CompId = Guid.Parse(compId);
                    }
                }
            }

            if (fileContent == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No file received.");

            string fileName = fileContent.Headers.ContentDisposition.FileName?.Trim('"');
            var fileData = await fileContent.ReadAsByteArrayAsync();

            try
            {
                if (isStorageTypeBlob)
                {
                    // 🔷 Azure Blob Upload
                    string connectionString = _blobConnectionString;
                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_CompContainerName);

                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                    string blobPath = $"{documentUpload.CompId}/{_CompMasterTempContainerName}/{documentUpload.DocName}/{fileName}";
                    BlobClient blobClient = containerClient.GetBlobClient(blobPath);

                    using (var stream = await fileContent.ReadAsStreamAsync())
                    {
                        await blobClient.UploadAsync(stream, overwrite: true);
                    }

                    documentUpload.DocPath = blobClient.Uri.ToString();
                }
                else
                {
                    // 🔷 Local File System Upload
                    string relativePath = ConfigurationManager.AppSettings["MastersTamplatePath"]; // Example: "~/MastersTemplates"
                    string physicalBasePath = HostingEnvironment.MapPath(relativePath);

                    // Create folder path: {BasePath}/{CompId}/{DocName}
                    string destinationFolder = Path.Combine(physicalBasePath, documentUpload.CompId.ToString(), documentUpload.DocName.ToString());
                    Directory.CreateDirectory(destinationFolder);

                    // Full destination file path
                    string filePath = Path.Combine(destinationFolder, fileName);

                    // Overwrite if exists
                    File.WriteAllBytes(filePath, fileData);

                    documentUpload.DocPath = filePath;
                }

                // 🔷 Save document info to database
                int id = DocumentSetupServer.UpsertDocument(documentUpload);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Message = "File uploaded successfully",
                    FileName = fileName,
                    DocumentPath = documentUpload.DocPath,
                    DocumentId = id
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error uploading file: " + ex.Message);
            }
        }

        [Route("api/MastersSetup/GetMasterDocumentsByCompId")]
        [HttpGet]
        public List<DocumentUploadInfo> GetMasterDocumentsByCompId(Guid companyId)
        {
            return DocumentSetupServer.GetMasterDocumentsByCompId(companyId);
        }

        [HttpGet]
        [Route("api/Masters/GetFile")]
        public HttpResponseMessage GetFile(int documentId)
        {
            DocumentUploadInfo documentDetail = DocumentSetupServer.GetMasterDocumentById(documentId);
            if (documentDetail != null)
            {
                if (!File.Exists(documentDetail.DocPath))
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var stream = new FileStream(documentDetail.DocPath, FileMode.Open, FileAccess.Read);

                var mimeType = MimeMapping.GetMimeMapping(documentDetail.DocName); // Dynamically detect MIME type

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(stream)
                };

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
                {
                    FileName = documentDetail.DocName
                };

                return response;
            }
            return null;
        }

        #endregion

        #region ShiftSetup

        [Route("api/MastersSetup/GetShiftDetailsByBranchIdAndCompId")]
        [HttpGet]
        public List<ShiftInfo> GetShiftDetailsByBranchIdAndCompId(int branchId, Guid compId)
        {
            return ShiftSetupServer.GetShiftDetailsByBranchIdAndCompId(compId, branchId);
        }

        [Route("api/MastersSetup/GetShiftDetailsByCompId")]
        [HttpGet]
        public List<ShiftInfo> GetShiftDetailsByCompId(Guid compId)
        {
            return ShiftSetupServer.GetShiftDetailsByCompId(compId);
        }

        [Route("api/MastersSetup/UpsertShift")]
        [HttpPost]
        public int UpsertShift(ShiftInfo shiftInfo)
        {
            return ShiftSetupServer.UpsertShift(shiftInfo);
        }

        #endregion

        #region LeaveSetup

        [Route("api/Masters/GetLeavesByCompId")]
        [HttpGet]
        public List<LeaveInfo> GetLeavesByCompId(Guid companyId)
        {
            return LeaveSetupServer.GetLeavesByCompId(companyId);
        }

        [Route("api/Masters/UpsertLeave")]
        [HttpPost]
        public int UpsertLeave(LeaveInfo leaveInfo)
        {
            return LeaveSetupServer.UpsertLeave(leaveInfo);
        }


        [Route("api/MastersSetup/GetAllLeaveTypesDetailsByCompId")]
        [HttpGet]
        public IEnumerable<LeaveInfo> GetAllLeaveTypesDetailsByCompId(Guid compId)
        {
            return leaveTypeInformationRepository.GetAllLeaveTypesDetailsByCompId(compId);
        }
        #endregion

        #region AssetSetup

        [Route("api/Masters/GetAssetsByCompId")]
        [HttpGet]
        public List<AssetInfo> GetAssetsByCompId(Guid companyId)
        {
            return AssetSetupServer.GetAssetsByCompId(companyId);
        }

        [Route("api/Masters/UpsertAsset")]
        [HttpPost]
        public int UpsertAsset(AssetInfo assetInfo)
        {
            return AssetSetupServer.UpsertAsset(assetInfo);
        }

        #endregion

        #region ReimbursementTypesSetup

        [Route("api/Masters/GetReimbursementTypesByCompId")]
        [HttpGet]
        public List<ReimbursementTypes> GetReimbursementTypesByCompId(Guid companyId)
        {
            return ReimbursementSetupServer.GetReimbursementTypesByCompId(companyId);
        }

        [Route("api/Masters/UpsertReimbursementTypes")]
        [HttpPost]
        public int UpsertReimbursementTypes(ReimbursementTypes reimbursementTypes)
        {
            return ReimbursementSetupServer.UpsertReimbursementTypes(reimbursementTypes);
        }

        #endregion

        #region HelpDesk Ticket Category

        [AllowAnonymous]
        [Route("api/Masters/GetTicketCategoriesByCompId")]
        [HttpGet]
        public List<TicketCategoryInformation> GetTicketCategoriesByCompId(Guid companyId)
        {
            return TicketCategorySetupServer.GetTicketCategoriesByCompId(companyId);
        }

        [AllowAnonymous]
        [Route("api/Masters/UpsertTicketCategory")]
        [HttpPost]
        public int UpsertTicketCategory(TicketCategoryInformation ticketCategoryInformation)
        {
            return TicketCategorySetupServer.UpsertTicketCategory(ticketCategoryInformation);
        }

        #endregion

        #region Loan Type Setup

        [AllowAnonymous]
        [Route("api/Masters/GetLoanSetups")]
        [HttpGet]
        public List<LoanSetupInfo> GetLoanSetups(Guid companyId)
        {
            return LoanTypeSetupServer.GetLoanSetups(companyId);
        }

        [AllowAnonymous]
        [Route("api/Masters/UpsertLoanTypes")]
        [HttpPost]
        public int UpsertLoanTypes(LoanSetupInfo loanSetupInfo)
        {
            return LoanTypeSetupServer.UpsertLoanTypes(loanSetupInfo);
        }

        #endregion

        #region Payroll Adjustment Type

        [AllowAnonymous]
        [Route("api/Masters/GetPayrollAdjustmentTypes")]
        [HttpGet]
        public List<PayrollAdjustmentType> GetPayrollAdjustmentTypes(Guid companyId)
        {
            return PayrollAdjustmentTypeServer.GetPayrollAdjustmentTypes(companyId);
        }

        [AllowAnonymous]
        [Route("api/Masters/UpsertPayrollAdjustmentType")]
        [HttpPost]
        public int UpsertPayrollAdjustmentType(PayrollAdjustmentType adjustmentType)
        {
            return PayrollAdjustmentTypeServer.UpsertPayrollAdjustmentType(adjustmentType);
        }

        #endregion
    }
}
