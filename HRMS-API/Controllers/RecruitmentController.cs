using ServerModel.Masters.Recruitment;
using ServerModel.Model;
using ServerModel.Model.Base;
using ServerModel.Model.Recruitment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading.Tasks;
using ServerModel.Model.Masters;
using ServerModel.ServerModel.Masters.DocumentSetup;
using ServerModel.Masters;
using ServerModel.Model.Employee;
using System.Configuration;
using System.Web.Hosting;
using ServerModel.Data;
using System.Web;
using HRMS_API.Models;
using DocumentFormat.OpenXml;
using ServerModel.ServerModel.Recruitment;
using ServerModel.ServerModel.HelpDesk;
using System.Net.Mail;
using ServerModel.ServerModel.Masters.DesignationSetup;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RecruitmentController : ApiController
    {

        private readonly string _blobConnectionString;
        private readonly string _resumeContainerName;

        RecruitmentHelper recruitmentHelper;
        EmployeeHelper employeeHelper;
        bool isStorageTypeBlob = false;

        public RecruitmentController()
        {
            isStorageTypeBlob = Convert.ToBoolean(ConfigurationManager.AppSettings["IsStorageTypeBlob"].ToString());

            _blobConnectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
            _resumeContainerName = ConfigurationManager.AppSettings["ResumeContainer"];

            DbContext.ConnectionString = WebApiConfig.ConnectionString;
            recruitmentHelper = new RecruitmentHelper();
            employeeHelper = new EmployeeHelper();
        }

        #region Create Job Vacancy
        [Route("api/Recruitment/AddUpdateCreateJobVacancy")]
        [HttpPost]
        public DataResult AddUpdateCreateJobVacancy(JobVacancyForm jobVacancyForm)
        {
            return recruitmentHelper.AddUpdateCreateJobVacancy(jobVacancyForm);
        }

        [Route("api/Recruitment/GetJobVacanciesByCompId")]
        [HttpGet]
        public IEnumerable<JobVacancyForm> GetJobVacanciesByCompId(Guid companyId)
        {
            return recruitmentHelper.GetJobVacanciesByCompId(companyId);
        }

        [Route("api/Recruitment/DeleteJobVacancy")]
        [HttpPost]
        public DataResult DeleteJobVacancy(JobVacancyForm jobVacancyForm)
        {
            return recruitmentHelper.DeleteJobVacancy(jobVacancyForm);
        }
        #endregion

        #region Create Job Online Form
        [Route("api/Recruitment/AddUpdateOnlineFormApplication")]
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

                OnlineFormApplication employeeDoc = JsonConvert.DeserializeObject<OnlineFormApplication>(empDocJson);

                // 2️⃣ Get uploaded file
                var fileData = provider.FileData.FirstOrDefault();
                if (fileData != null)
                {
                    string originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('"');

                    // Generate unique filename to avoid overwrite
                   // string uniqueFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_{DateTime.Now.Ticks}{Path.GetExtension(originalFileName)}";

                    // Organize files by designation
                    DesignationInfo designation = DesignationSetupServer
                        .GetDesignationsByCompId(employeeDoc.CompId ?? Guid.Empty)
                        .FirstOrDefault(x => x.Id == employeeDoc.MS_Designation_Id);

                    if (designation == null)
                        return BadRequest("Designation not found for the given employee.");

                    if (isStorageTypeBlob)
                    {
                        // 🔷 Azure Blob Storage Upload
                        string connectionString = _blobConnectionString;
                        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_resumeContainerName);
                        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                        string blobName = $"{designation.DesignationName}/{originalFileName}";
                        BlobClient blobClient = containerClient.GetBlobClient(blobName);

                        using (var fileStream = File.OpenRead(fileData.LocalFileName))
                        {
                            await blobClient.UploadAsync(fileStream, overwrite: true);
                        }

                        employeeDoc.Resume = blobClient.Uri.ToString();
                    }
                    else
                    {
                        // 🔷 Local File System Upload
                        string relativePath = ConfigurationManager.AppSettings["CandidatesResume"]; // e.g. "~/Resumes"
                        string physicalBasePath = HostingEnvironment.MapPath(relativePath);

                        // Create folder path: {BasePath}/{DesignationName}
                        string destinationFolder = Path.Combine(physicalBasePath, designation.DesignationName);
                        Directory.CreateDirectory(destinationFolder);

                        // Full destination file path
                        string destinationFilePath = Path.Combine(destinationFolder, originalFileName);

                        // Copy file from temp folder to destination
                        File.Copy(fileData.LocalFileName, destinationFilePath, true);

                        // Save path in model
                        employeeDoc.Resume = destinationFilePath;
                    }
                }

                // 3️⃣ Preserve existing resume if updating and no new file uploaded
                if (employeeDoc.Id != Guid.Empty && string.IsNullOrEmpty(employeeDoc.Resume))
                {
                    var existingApplications = recruitmentHelper
                        .GetJobVacanciesApplicationsByCompId(employeeDoc.CompId ?? Guid.Empty)
                        .ToList();

                    var existingEmpDoc = existingApplications?.FirstOrDefault(x => x.Id == employeeDoc.Id);
                    if (!string.IsNullOrEmpty(existingEmpDoc?.Resume))
                        employeeDoc.Resume = existingEmpDoc.Resume;
                }

                // 4️⃣ Save application
                DataResult result = recruitmentHelper.AddUpdateOnlineFormApplication(employeeDoc);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // Provides full exception details for debugging
            }
        }


        [Route("api/Recruitment/AddUpdateOnlineFormApplication_LocalFileUpload")]
        [HttpPost]
        public async Task<IHttpActionResult> AddUpdateOnlineFormApplication_LocalFileUpload()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return BadRequest("Unsupported media type.");
                }

                // Get path from web.config
                var configuredPath = ConfigurationManager.AppSettings["CandidatesResume"]; // e.g. "~/UploadedFiles/Resume"

                // Normalize all slashes to OS-specific separator
                configuredPath = configuredPath
                    .Replace("/", Path.DirectorySeparatorChar.ToString())
                    .Replace("\\", Path.DirectorySeparatorChar.ToString());

                // Resolve Azure-safe root
                var homePath = Environment.GetEnvironmentVariable("HOME") ?? HttpContext.Current.Server.MapPath("~");

                // Combine HOME with configured path
                // Remove "~/" from config path if exists
                if (configuredPath.StartsWith("~"))
                    configuredPath = configuredPath.Substring(2);

                var root = Path.Combine(homePath, configuredPath);

                var provider = new MultipartFormDataStreamProvider(root);
                await Request.Content.ReadAsMultipartAsync(provider);

                // 1️⃣ Get JSON object (your "emp-doc-form")
                var empDocJson = provider.FormData["job-app-form"];
                if (string.IsNullOrEmpty(empDocJson))
                    return BadRequest("Employee document data missing.");

                OnlineFormApplication employeeDoc = JsonConvert.DeserializeObject<OnlineFormApplication>(empDocJson);

                // 2️⃣ Get File (your "doc")
                var fileData = provider.FileData.FirstOrDefault();
                if (fileData != null)
                {
                    DesignationInfo designation = DesignationSetupServer.GetDesignationsByCompId(employeeDoc.CompId ?? Guid.Empty)
                        .FirstOrDefault(x => x.Id == employeeDoc.MS_Designation_Id);

                    string originalFileName = fileData.Headers.ContentDisposition.FileName.Trim('"');
                    string newFilePath = Path.Combine(root, designation.DesignationName, originalFileName);
                    var directoryPath = Path.GetDirectoryName(newFilePath);
                    // Ensure directory exists
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    File.Copy(fileData.LocalFileName, newFilePath, true); // overwrite = true

                    employeeDoc.Resume = newFilePath;
                }

                if (employeeDoc.Id != Guid.Empty && string.IsNullOrEmpty(employeeDoc.Resume))
                {
                    List<OnlineFormApplication> existingjobApplications = recruitmentHelper.GetJobVacanciesApplicationsByCompId(employeeDoc.CompId ?? Guid.Empty).ToList();
                    if (existingjobApplications != null && existingjobApplications.Count > 0)
                    {
                        OnlineFormApplication existingEmpDoc = existingjobApplications.FirstOrDefault(x => x.Id == employeeDoc.Id);
                        if (existingEmpDoc != null && !string.IsNullOrEmpty(existingEmpDoc.Resume))
                            employeeDoc.Resume = existingEmpDoc.Resume;
                    }
                }

                // Save application using business logic
                DataResult result = recruitmentHelper.AddUpdateOnlineFormApplication(employeeDoc);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        [Route("api/Recruitment/GetJobVacanciesApplicationsByCompId")]
        [HttpGet]
        public IEnumerable<OnlineFormApplication> GetJobVacanciesApplicationsByCompId(Guid companyId)
        {
            return recruitmentHelper.GetJobVacanciesApplicationsByCompId(companyId);
        }

        [Route("api/Recruitment/DeleteOnlineFormApplication")]
        [HttpPost]
        public DataResult DeleteOnlineFormApplication(OnlineFormApplication onlineFormApplication)
        {
            return recruitmentHelper.DeleteOnlineFormApplication(onlineFormApplication);
        }
        #endregion

        #region Interview Schedule
        [Route("api/Recruitment/AddUpdateInterviewScheduleApplication")]
        [HttpPost]
        public DataResult AddUpdateInterviewScheduleApplication(InterviewPortalInformation interviewPortalInformation)
        {
            return recruitmentHelper.AddUpdateInterviewScheduleApplication(interviewPortalInformation);
        }

        [Route("api/Recruitment/GetScheduleInterviewsByCompId")]
        [HttpGet]
        public IEnumerable<InterviewPortalInformation> GetScheduleInterviewsByCompId(Guid companyId)
        {
            return recruitmentHelper.GetScheduleInterviewsByCompId(companyId);
        }


        [Route("api/Recruitment/GetJobApplicationsByVacancy")]
        [HttpGet]
        public IEnumerable<OnlineFormApplication> GetJobApplicationsByVacancy(Guid vacancyId)
        {
            return recruitmentHelper.GetJobApplicationsByVacancy(vacancyId);
        }

        [Route("api/Recruitment/GetApplicantsByStatus")]
        [HttpGet]
        public IEnumerable<InterviewPortalInformation> GetApplicantsByStatus(Guid companyId, int interviewStatusId)
        {
            return recruitmentHelper.GetApplicantsByStatus(companyId, interviewStatusId);
        }

        [Route("api/Recruitment/GetScheduleInterviewsByCompIdAndStatus")]
        [HttpGet]
        public List<InterviewPortalInformation> GetScheduleInterviewsByCompIdAndStatus(Guid compId, InterviewStatusTypes interviewStatus = InterviewStatusTypes.None)
        {
            return recruitmentHelper.GetScheduleInterviewsByCompId(compId, interviewStatus);
        }





        #endregion

        #region Interview Feedback

        [Route("api/Recruitment/AddUpdateInterviewFeedback")]
        [HttpPost]
        public DataResult AddUpdateInterviewFeedback(InterviewFeedback interviewFeedback)
        {
            return recruitmentHelper.AddUpdateInterviewFeedback(interviewFeedback);
        }

        [Route("api/Recruitment/GetInterviewFeedbackByCompId")]
        [HttpGet]
        public IEnumerable<InterviewFeedback> GetInterviewFeedbackByCompId(Guid companyId)
        {
            return recruitmentHelper.GetInterviewFeedbackByCompId(companyId);
        }

        [Route("api/Recruitment/GetInterviewFeedbacksByCompIdAndRating")]
        [HttpGet]
        public IEnumerable<InterviewFeedback> GetInterviewFeedbacksByCompIdAndRating(Guid compId, int interviewRateId = 0)
        {
            return recruitmentHelper.GetInterviewFeedbacksByCompIdAndRating(compId, interviewRateId);
        }

        // update Interview Status Based on Interview Feedback only HR will update this status, post interviewer discussion 
        // TODO : Make Provision to send mail with proper mail body and all..............
        [Route("api/Recruitment/UpdateInterviewStatus")]
        [HttpPost]
        public DataResult UpdateInterviewStatus(InterviewStatusTypes interviewStatusTypes, Guid candidateId)
        {
            return recruitmentHelper.UpdateInterviewStatus(interviewStatusTypes, candidateId);
        }

        #endregion

        #region Select/Reject Candidate

        [Route("api/Recruitment/AddUpdateCandidateStatus")]
        [HttpPost]
        public DataResult AddUpdateCandidateStatus(Guid schduledCandidateId, InterviewStatusTypes interviewStatusTypes)
        {
            return recruitmentHelper.AddUpdateCandidateStatus(schduledCandidateId, interviewStatusTypes);
        }

        #endregion

        [Route("api/Recruitment/GetConfirmedCandidatesByCompId")]
        [HttpGet]
        public List<EmployeeGeneratedDocument> GetConfirmedCandidatesByCompId(Guid compId, string status)
        {
            return RecruitmentServer.GetConfirmedCandidatesByCompId(compId, status);
        }


        [Route("api/Recruitment/DraftOfferLetter")]
        [HttpPost]
        public async Task<IHttpActionResult> DraftOfferLetter(EmployeeGeneratedDocument employeeGeneratedDocument)
        {
            // Get Generated Doc
            List<EmployeeGeneratedDocument> empGeneratedDocs = RecruitmentServer.GetGeneratedDocumentsByCompId(employeeGeneratedDocument.CompId.Value);

            EmployeeGeneratedDocument empGeneratedDocDetail = null;
            if (empGeneratedDocs.Any(x => x.Id == employeeGeneratedDocument.Id))
            {
                // Update
                employeeGeneratedDocument.Status = OfferLetterStatusTypes.Revise.ToString();
            }
            else
            {
                // Insert
                employeeGeneratedDocument.Status = OfferLetterStatusTypes.Draft.ToString();
            }

            empGeneratedDocDetail = RecruitmentServer
                  .GetEmployeeGeneratedDocDetail(employeeGeneratedDocument.CompId.Value, employeeGeneratedDocument.Req_JbForm_Id);

            empGeneratedDocDetail.CompId = employeeGeneratedDocument.CompId;
            empGeneratedDocDetail.Status = employeeGeneratedDocument.Status;
            empGeneratedDocDetail.MS_Doc_Id = employeeGeneratedDocument.MS_Doc_Id;
            empGeneratedDocDetail.Req_JbForm_Id = employeeGeneratedDocument.Req_JbForm_Id;
            empGeneratedDocDetail.DocCreatedOn = DateTime.Now;
            empGeneratedDocDetail.DocExperiesOn = employeeGeneratedDocument.DocExperiesOn;
            empGeneratedDocDetail.DateOfJoining = employeeGeneratedDocument.DateOfJoining;
            empGeneratedDocDetail.ProbationPeriod = employeeGeneratedDocument.ProbationPeriod;
            empGeneratedDocDetail.NoticePeriod = employeeGeneratedDocument.NoticePeriod;
            empGeneratedDocDetail.CTC = employeeGeneratedDocument.CTC;

            empGeneratedDocDetail.SalaryComponents = employeeGeneratedDocument.SalaryComponents;

            if (!empGeneratedDocDetail.MS_Doc_Id.HasValue)
                return Ok("Master Document File is not found");

            DocumentUploadInfo docInfo = DocumentSetupServer.GetMasterDocumentById(empGeneratedDocDetail.MS_Doc_Id.Value);
            if (docInfo == null)
                return Ok("Master Document File is not found");

            // 1. Resolve master template folder from config
            string physicalPath = docInfo.DocPath;
            if (string.IsNullOrEmpty(physicalPath))
                throw new Exception("MastersTemplatePath is invalid: " + physicalPath);

            // 2. Master template file
           // string templatePath = physicalPath;
            //if (!System.IO.File.Exists(templatePath))
            //    throw new Exception("Template file not found: " + templatePath);

            OnlineFormApplication candidateDetail = recruitmentHelper.GetJobApplicationDetailByApplicationId(empGeneratedDocDetail.Req_JbForm_Id);
            if (candidateDetail == null)
                return Ok("Candidate Detail Not Found");

            string extension = ".docx";
            string fileName = $"OfferLetter - {candidateDetail.FullName}_{DateTime.Now:yyyyMMdd_HHmmssfff}{extension}";

            if (isStorageTypeBlob)
            {
                string destBlobContainer = ConfigurationManager.AppSettings["EmployeeGeneratedDocumentContainer"];
                string copiedUrl = await CopyBlobFromUrlAsync(physicalPath, destBlobContainer, fileName);

                // Extract blob name from URL
                var blobServiceClient = new BlobServiceClient(_blobConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(destBlobContainer);

                Uri uri = new Uri(copiedUrl);
                string blobName = Path.GetFileName(uri.LocalPath);
                var blobClient = containerClient.GetBlobClient(blobName);

                // Download as stream → copy to MemoryStream for editing
                using (var downloadStream = new MemoryStream())
                {
                    await blobClient.DownloadToAsync(downloadStream);
                    downloadStream.Position = 0;

                    using (var wordDoc = WordprocessingDocument.Open(downloadStream, true))
                    {
                        ReplaceAllPlaceholders(wordDoc, empGeneratedDocDetail);
                        ReplaceAnnexureTable(wordDoc,
                            empGeneratedDocDetail.SalaryComponents,
                            (empGeneratedDocDetail.CTC / 12).ToString(),
                            empGeneratedDocDetail.CTC.ToString());

                        wordDoc.MainDocumentPart.Document.Save();
                    }

                    // Reset stream before upload
                    downloadStream.Position = 0;
                    await blobClient.UploadAsync(downloadStream, overwrite: true);
                }
                empGeneratedDocDetail.DocPath = copiedUrl;
            }
            else
            {
                // 🔷 Local File System Upload
                string relativePath = ConfigurationManager.AppSettings["EmployeeGeneratedDocumentPath"]; // e.g. "~/Generated Documents"
                string physicalBasePath = HostingEnvironment.MapPath(relativePath);

                // Create folder path: {BasePath}/{CandidateName}
                string destinationFolder = Path.Combine(physicalBasePath, empGeneratedDocDetail.CandidateFullName);
                Directory.CreateDirectory(destinationFolder);

                // Full destination file path
                string destinationFilePath = Path.Combine(destinationFolder, fileName);

                // 🔷 Copy file from master template path
                if (!System.IO.File.Exists(physicalPath))
                    return Ok("Master template file not found: " + physicalPath);

                System.IO.File.Copy(physicalPath, destinationFilePath, true);

                // 🔷 Open and update the DOCX file
                using (var wordDoc = WordprocessingDocument.Open(destinationFilePath, true))
                {
                    ReplaceAllPlaceholders(wordDoc, empGeneratedDocDetail);
                    ReplaceAnnexureTable(
                        wordDoc,
                        empGeneratedDocDetail.SalaryComponents,
                        (empGeneratedDocDetail.CTC / 12).ToString(),
                        empGeneratedDocDetail.CTC.ToString()
                    );

                    wordDoc.MainDocumentPart.Document.Save();
                }

                empGeneratedDocDetail.DocPath = destinationFilePath;
            }

            // 3. Output folder for generated documents
            //string relativePath = ConfigurationManager.AppSettings["EmployeeGeneratedDocumentPath"];
            //string generatedPhysicalPath = HostingEnvironment.MapPath(relativePath);
            //if (string.IsNullOrEmpty(generatedPhysicalPath))
            //{
            //    return BadRequest("❌ Invalid EmployeeGeneratedDocumentPath config value.");
            //}
            //string fileFolderPath = Path.Combine(generatedPhysicalPath, candidateDetail.FullName);
            //Directory.CreateDirectory(fileFolderPath);

            //// 4. Output file path
            //if (!empGeneratedDocDetail.Req_JbForm_Id.HasValue)
            //    return Ok("Candidate Detail Not Found");

            //string filePath = Path.Combine(fileFolderPath, fileName);

            //// 5. Copy template → new working file
            //System.IO.File.Copy(templatePath, filePath, true);

            // 6. Open the new file and replace placeholders
            //using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(localPath, true))
            //{
            //    // Replace all placeholders
            //    ReplaceAllPlaceholders(wordDoc, empGeneratedDocDetail);

            //    // Replace Annexure A dynamically
            //    ReplaceAnnexureTable(wordDoc, empGeneratedDocDetail.SalaryComponents, (empGeneratedDocDetail.CTC / 12).ToString(), empGeneratedDocDetail.CTC.ToString());

            //    wordDoc.MainDocumentPart.Document.Save();
            //}

            //// Upload back to blob (overwrite same blob)
            //using (var stream = File.OpenRead(localPath))
            //{
            //    await blobClient.UploadAsync(stream, overwrite: true);
            //}

          

            int result = RecruitmentServer.UpsertEmployeeGeneratedOfferLetter(empGeneratedDocDetail);

            return Ok(result);
        }

        public async Task<string> CopyBlobFromUrlAsync(string sourceBlobUrl, string destContainer, string destBlobName)
        {
            var blobServiceClient = new BlobServiceClient(_blobConnectionString);

            // Destination container
            var destContainerClient = blobServiceClient.GetBlobContainerClient(destContainer);
            await destContainerClient.CreateIfNotExistsAsync();

            // Destination blob
            var destBlobClient = destContainerClient.GetBlobClient(destBlobName);

            // Start copy (server-side, async)
            var operation = await destBlobClient.StartCopyFromUriAsync(new Uri(sourceBlobUrl));

            // Optional: wait for completion
            while (true)
            {
                var properties = await destBlobClient.GetPropertiesAsync();
                if (properties.Value.CopyStatus != CopyStatus.Pending)
                    break;

                await Task.Delay(500);
            }

            return destBlobClient.Uri.ToString();
        }


        [Route("api/Recruitment/GetGeneratedDocumentsByCompId")]
        [HttpGet]
        public List<EmployeeGeneratedDocument> GetGeneratedDocumentsByCompId(Guid compId)
        {
            return RecruitmentServer.GetGeneratedDocumentsByCompId(compId);
        }

        [Route("api/Recruitment/GetEmployeeOfferSalaryInfo")]
        [HttpGet]
        public List<EmployeeOfferLetterSalaryInfo> GetEmployeeOfferSalaryInfo(Guid candidateId, int documentId)
        {
            return RecruitmentServer.GetEmployeeOfferSalaryInfo(candidateId, documentId);
        }

        [Route("api/Recruitment/UpdateGeneratedDocStatusById")]
        [HttpPost]
        public bool UpdateGeneratedDocStatusById(int documentId, string status)
        {
            return RecruitmentServer.UpdateGeneratedDocStatusById(documentId, status);
        }

        [Route("api/Recruitment/SendLetterToUser")]
        [HttpPost]
        public async Task<IHttpActionResult> SendLetterToUser(int documentId)
        {
            string senderMail = ConfigurationManager.AppSettings["SenderMail"];
            string senderPasswd = ConfigurationManager.AppSettings["SenderPasswd"];

            string smtpServer = "smtp.office365.com";
            int smtpPort = 587;
            string smtpUser = senderMail;
            string smtpPass = senderPasswd;

            EmployeeGeneratedDocument empGeneratedDoc = RecruitmentServer.GetGeneratedDocById(documentId);
            if (empGeneratedDoc == null)
                return NotFound();

            // Auto-reply to USER
            var autoReply = new MailMessage();
            autoReply.From = new MailAddress(smtpUser, empGeneratedDoc.CompanyName);
            autoReply.To.Add(empGeneratedDoc.Email);
            autoReply.Subject = "Welcome to  " + empGeneratedDoc.CompanyName + "- Your Offer Letter";
            //autoReply.Body = $"Dear {request.Name},\n\nThanks for requesting a demo. Our team will contact you soon.\n\nBest Regards,\nYour Company";
            autoReply.Body = $"Dear {empGeneratedDoc.CandidateFullName},\n\nWe are delighted to welcome you to " + empGeneratedDoc.CompanyName + "!. " +
                "\n\nBased on our discussions, we are pleased to offer you the position of " + empGeneratedDoc.JobTitle + "with us. Attached, " +
                "you will find your official offer letter containing details regarding your role, compensation, benefits, and other terms of employment." +
                "\n\nPlease review the letter carefully and confirm your acceptance by replying to this email or signing and returning the attached document by " + empGeneratedDoc.DocExperiesOn + "." +
                "\n\nWe are excited about the skills and experience you bring to our team, and we look forward to working together." +
                "\n\nIf you have any questions, please don’t hesitate to reach out to us." +
                "\n\nWelcome aboard once again!" +
                "\n\nBest regards,\n" + empGeneratedDoc.CompanyName + ",\n" + empGeneratedDoc.CompanyContact + "\n" + empGeneratedDoc.Website + "";

            Uri blobUri = new Uri(empGeneratedDoc.DocPath);

            if (isStorageTypeBlob)
            {
                string containerName = ConfigurationManager.AppSettings["EmployeeGeneratedDocumentPath"];
                string blobName = Path.GetFileName(empGeneratedDoc.DocPath);

                var blobServiceClient = new BlobServiceClient(_blobConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Get file name
                ///string fileName = Path.GetFileName(containerClient.Name);

                // Get blob client
                BlobClient blobClient = containerClient.GetBlobClient(blobName);


                // Download blob into MemoryStream
                var ms = new MemoryStream();
                await blobClient.DownloadToAsync(ms);

                // Reset position so Attachment can read from start
                ms.Position = 0;

                // Get content type from blob properties (fallback to octet-stream if not set)
                var properties = await blobClient.GetPropertiesAsync();
                string contentType = !string.IsNullOrEmpty(properties.Value.ContentType)
                    ? properties.Value.ContentType
                    : "application/octet-stream";

                // ✅ For attachment name, use blobName (or Path.GetFileName(blobName))
                string fileName = Path.GetFileName(blobName);

                // Create attachment
                var attachment = new Attachment(ms, fileName, contentType);

                // Add to MailMessage
                //    var mail = new MailMessage();
                autoReply.Attachments.Add(attachment);

                using (var smtp = new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(autoReply);   // Send to USER

                    List<EmployeeGeneratedDocument> documents = RecruitmentServer.GetGeneratedDocumentsByCompId(empGeneratedDoc.CompId ?? Guid.Empty);
                    if (documents != null)
                    {
                        EmployeeGeneratedDocument generatedDocument = documents.FirstOrDefault(x => x.Id == documentId);
                        if (generatedDocument != null)
                        {
                            List<EmployeeOfferLetterSalaryInfo> salaryInfos = RecruitmentServer.GetEmployeeOfferSalaryInfo(generatedDocument.Req_JbForm_Id, generatedDocument.Id);

                            generatedDocument.Status = OfferLetterStatusTypes.NotConfirmed.ToString();
                            generatedDocument.SalaryComponents = salaryInfos;

                            int result = RecruitmentServer.UpsertEmployeeGeneratedOfferLetter(generatedDocument);
                        }
                    }

                }
            }
            else
            {
                // Local file system attachment
                string localFilePath = empGeneratedDoc.DocPath;

                if (!System.IO.File.Exists(localFilePath))
                {
                    return Ok("Offer letter file not found at local path: " + localFilePath);
                }

                // Read file into stream
                var ms = new MemoryStream();
                using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(ms);
                }

                // Reset position so email can read from the beginning
                ms.Position = 0;

                // Detect content type from file extension
                string extension = Path.GetExtension(localFilePath).ToLower();
                string contentType;

                switch (extension)
                {
                    case ".docx":
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".doc":
                        contentType = "application/msword";
                        break;
                    case ".pdf":
                        contentType = "application/pdf";
                        break;
                    default:
                        contentType = "application/octet-stream";
                        break;
                }

                string fileName = Path.GetFileName(localFilePath);


                // Attach
                var attachment = new Attachment(ms, fileName, contentType);
                autoReply.Attachments.Add(attachment);

                using (var smtp = new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(autoReply);

                    // Update status in DB
                    List<EmployeeGeneratedDocument> documents = RecruitmentServer.GetGeneratedDocumentsByCompId(empGeneratedDoc.CompId ?? Guid.Empty);
                    if (documents != null)
                    {
                        EmployeeGeneratedDocument generatedDocument = documents.FirstOrDefault(x => x.Id == documentId);
                        if (generatedDocument != null)
                        {
                            List<EmployeeOfferLetterSalaryInfo> salaryInfos = RecruitmentServer.GetEmployeeOfferSalaryInfo(generatedDocument.Req_JbForm_Id, generatedDocument.Id);

                            generatedDocument.Status = OfferLetterStatusTypes.NotConfirmed.ToString();
                            generatedDocument.SalaryComponents = salaryInfos;

                            int result = RecruitmentServer.UpsertEmployeeGeneratedOfferLetter(generatedDocument);
                        }
                    }
                }
            }
            return Ok();
        }

        public string GetFileExtension(string fileName)
        {
            string contentType;
            string extension = Path.GetExtension(fileName).ToLowerInvariant();

            switch (extension)
            {
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".doc":
                    contentType = "application/msword";
                    break;
                case ".xlsx":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".txt":
                    contentType = "text/plain";
                    break;
                default:
                    contentType = "application/octet-stream"; // safe fallback
                    break;
            }

            var ms = new MemoryStream();
            var attachment = new Attachment(ms, fileName, contentType);

            return extension;
        }

        [Route("api/Recruitment/GenerateDocument")]
        [HttpPost]
        public IHttpActionResult GenerateOfferLetter(Guid compId, Guid candidateId)
        {
            // Get Candidate Detail
            EmployeeGeneratedDocument empGeneratedDocDetail = RecruitmentServer.GetEmployeeGeneratedDocDetail(compId, candidateId);
            if (empGeneratedDocDetail == null)
                return NotFound();

            if (!empGeneratedDocDetail.MS_Doc_Id.HasValue)
                return Ok("Master Document File is not found");

            DocumentUploadInfo docInfo = DocumentSetupServer.GetMasterDocumentById(empGeneratedDocDetail.MS_Doc_Id.Value);
            if (docInfo == null)
                return Ok("Master Document File is not found");

            // 1. Resolve master template folder from config
            string savedLetterPath = docInfo.DocPath; //ConfigurationManager.AppSettings["MastersTamplatePath"];
            string physicalPath = HostingEnvironment.MapPath(savedLetterPath);
            if (string.IsNullOrEmpty(physicalPath))
                throw new Exception("MastersTemplatePath is invalid: " + savedLetterPath);

            // 2. Master template file
            string templatePath = Path.Combine(physicalPath, Path.GetFileName(docInfo.DocPath));
            if (!System.IO.File.Exists(templatePath))
                throw new Exception("Template file not found: " + templatePath);

            // 3. Output folder for generated documents
            string relativePath = ConfigurationManager.AppSettings["EmployeeGeneratedDocumentPath"];
            string generatedPhysicalPath = HostingEnvironment.MapPath(relativePath);
            if (string.IsNullOrEmpty(generatedPhysicalPath))
            {
                return BadRequest("❌ Invalid EmployeeGeneratedDocumentPath config value.");
            }
            Directory.CreateDirectory(generatedPhysicalPath);

            // 4. Output file path
            // Output file name
            string fileName = "OfferLetter - " + empGeneratedDocDetail.CandidateFullName + ".docx";
            string filePath = Path.Combine(generatedPhysicalPath, fileName);

            // 5. Copy template → new working file
            System.IO.File.Copy(templatePath, filePath, true);

            if (empGeneratedDocDetail.Req_JbForm_Id != Guid.Empty)
            {
                empGeneratedDocDetail.SalaryComponents = RecruitmentServer.GetEmployeeOfferSalaryInfo(empGeneratedDocDetail.Req_JbForm_Id, empGeneratedDocDetail.Id);
            }

            // 6. Open the new file and replace placeholders
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                // Replace all placeholders
                ReplaceAllPlaceholders(wordDoc, empGeneratedDocDetail);

                // Replace Annexure A dynamically
                ReplaceAnnexureTable(wordDoc, empGeneratedDocDetail.SalaryComponents, (empGeneratedDocDetail.CTC / 12).ToString(), empGeneratedDocDetail.CTC.ToString());

                wordDoc.MainDocumentPart.Document.Save();
            }


            // 7. SaveLetter in DB
            int result = RecruitmentServer.UpsertEmployeeGeneratedOfferLetter(empGeneratedDocDetail);

            // 7. Return file as download
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            // Return as download
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };

            response.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

            response.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            return ResponseMessage(response);
        }

        private void ReplaceAnnexureTable(WordprocessingDocument wordDoc, List<EmployeeOfferLetterSalaryInfo> components,
            string totalMonthly, string totalAnnual)
        {
            var body = wordDoc.MainDocumentPart.Document.Body;

            // Find the placeholder paragraph
            var placeholderParagraph = body.Descendants<Paragraph>()
                .FirstOrDefault(p => p.InnerText.Contains("[AnnexureTable]"));

            if (placeholderParagraph == null)
                throw new Exception("Placeholder [AnnexureTable] not found in template.");

            // Create the table
            Table table = new Table();

            // Table borders
            TableProperties tblProps = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 },
                    new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 12 }
                )
            );
            table.AppendChild(tblProps);

            // Header row
            TableRow headerRow = new TableRow();
            headerRow.Append(
                CreateCell("Component", true),
                CreateCell("Monthly (₹)", true),
                CreateCell("Annual (₹)", true)
            );
            table.Append(headerRow);

            // Data rows
            foreach (var comp in components)
            {
                if (comp.SLHeadName.Equals("CTC", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (comp.Amount == null || comp.Amount <= 0)
                    continue;

                TableRow row = new TableRow();
                row.Append(
                    CreateCell(comp.SLHeadName),
                    CreateCell((comp.Amount / 12).ToString()),
                    CreateCell(comp.Amount.ToString())
                );
                table.Append(row);
            }

            // Total row
            TableRow totalRow = new TableRow();
            totalRow.Append(
                CreateCell("Total CTC", true),
                CreateCell(totalMonthly, true),
                CreateCell(totalAnnual, true)
            );
            table.Append(totalRow);

            // Replace placeholder with table
            placeholderParagraph.Parent.InsertAfter(table, placeholderParagraph);
            placeholderParagraph.Remove();
        }

        // Helper to create a table cell
        private TableCell CreateCell(string text, bool bold = false)
        {
            Run run = new Run(new Text(text));
            if (bold) run.RunProperties = new RunProperties(new Bold());

            Paragraph para = new Paragraph(run);
            TableCell cell = new TableCell(para);

            TableCellProperties props = new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnitValues.Auto }
            );
            cell.Append(props);

            return cell;
        }


        private void ReplaceAllPlaceholders(WordprocessingDocument wordDoc, EmployeeGeneratedDocument req)
        {
            var body = wordDoc.MainDocumentPart.Document.Body;

            foreach (var paragraph in body.Descendants<Paragraph>())
            {
                ReplacePlaceholdersInParagraph(paragraph, "[Company Name]", req.CompanyName);
                ReplacePlaceholdersInParagraph(paragraph, "[Company Address Line 1]", req.CompanyAddress);
                //ReplacePlaceholdersInParagraph(paragraph, "[Company Address Line 2]", req.CompanyAddressLine2);
                ReplacePlaceholdersInParagraph(paragraph, "[City, State, PIN]", req.CompCity + ", " + req.CompState + ", " + req.Pincode + "\n " + req.CompCountry);

                ReplacePlaceholdersInParagraph(paragraph, "[Candidate’s Full Name]", req.CandidateFullName);
                ReplacePlaceholdersInParagraph(paragraph, "[Candidate’s Address]", req.CandidateAddress);

                ReplacePlaceholdersInParagraph(paragraph, "[Job Title]", req.JobTitle);
                ReplacePlaceholdersInParagraph(paragraph, "[Location]", req.Location);
                ReplacePlaceholdersInParagraph(paragraph, "[Reporting Manager’s Name/Designation]", req.ReportingManager);

                ReplacePlaceholdersInParagraph(paragraph, "[DD/MM/YYYY]", req.DateOfJoining?.ToString("dd/MM/yyyy"));
                ReplacePlaceholdersInParagraph(paragraph, "[date]", req.DocCreatedOn?.ToString("dd/MM/yyyy"));

                ReplacePlaceholdersInParagraph(paragraph, "[Probation Period]", req.ProbationPeriod.ToString());
                ReplacePlaceholdersInParagraph(paragraph, "(Probation Period in letters)", " (" + NumberToWordsConverter.ConvertAmountToWords((decimal)req.ProbationPeriod) + ") ");

                ReplacePlaceholdersInParagraph(paragraph, "[Amount in figures]", req.CTC.ToString());
                ReplacePlaceholdersInParagraph(paragraph, "[Amount in words]", NumberToWordsConverter.ConvertAmountToWords(req.CTC ?? 0, true));

                ReplacePlaceholdersInParagraph(paragraph, "[X months]", req.ProbationPeriod.ToString());
                ReplacePlaceholdersInParagraph(paragraph, "[X notice period]", req.NoticePeriod.ToString());

                //ReplacePlaceholdersInParagraph(paragraph, "[e.g., 9:30 AM to 6:30 PM, Monday to Friday]", );
            }
        }


        [Route("api/Recruitment/DownloadDocumentId")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult DownloadDocument(int documentId)
        {
            // get emp docs
            EmployeeGeneratedDocument employeeDocument = RecruitmentServer.GetGeneratedDocById(documentId);
            if (employeeDocument == null)
                return NotFound();

            if (!System.IO.File.Exists(employeeDocument.DocPath))
                return NotFound();

            string filePath = employeeDocument.DocPath;

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

        [Route("api/Recruitment/GetDocGeneratedCandidatesByCompId")]
        [HttpGet]
        public List<EmployeeGeneratedDocument> GetDocGeneratedCandidatesByCompId(Guid compId)
        {
            return RecruitmentServer.GetDocGeneratedCandidatesByCompId(compId);
        }

        [Route("api/Recruitment/GenerateDocument2")]
        [HttpGet]
        public void GenerateOfferLetter2(int documentId, Guid employeeId)
        {
            DocumentUploadInfo documentDetail = DocumentSetupServer.GetMasterDocumentById(documentId);

            EmployeeInformation employeeInformation = employeeHelper.GetEmployeeInformationById(employeeId);

            // Template path
            string templatePath = documentDetail.DocPath;
            string fileName = documentDetail.DocName + "-" + employeeInformation.FullName + ".docx";

            string relativePath = ConfigurationManager.AppSettings["EmployeeGeneratedDocumentPath"];
            string physicalPath = HostingEnvironment.MapPath(relativePath);

            // Ensure directory exists
            Directory.CreateDirectory(physicalPath);

            string filePath = Path.Combine(physicalPath, fileName);

            // Copy the template to create a new document
            File.Copy(templatePath, filePath, true);

            // Open the new document and replace placeholders
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;

                // Replace placeholders across Text nodes
                foreach (var paragraph in body.Descendants<Paragraph>())
                {
                    ReplacePlaceholdersInParagraph(paragraph, "[EmployeeName]", employeeInformation.FullName);
                    ReplacePlaceholdersInParagraph(paragraph, "[Position]", employeeInformation.DesignationName);
                }

                // Save changes
                wordDoc.MainDocumentPart.Document.Save();
            }
        }

        [Route("api/Recruitment/GenerateOfferLetter1")]
        [HttpGet]
        public void GenerateOfferLetter1()
        {
            string serverPath = @"C:\Templates";

            string employeeName = "Ajinkya";
            string position = "Developer";

            // Template path
            string templatePath = @"C:\Templates\OfferLetterTemplate.docx";

            // Create a new file name
            string fileName = $"OfferLetter_{employeeName}_{DateTime.Now:yyyyMMddHHmmss}.docx";
            string filePath = Path.Combine(serverPath, fileName);

            // Copy the template to create a new document
            File.Copy(templatePath, filePath, true);

            // Open the new document and replace placeholders
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, true))
            {
                var body = wordDoc.MainDocumentPart.Document.Body;

                // Replace placeholders across Text nodes
                foreach (var paragraph in body.Descendants<Paragraph>())
                {
                    ReplacePlaceholdersInParagraph(paragraph, "[EmployeeName]", employeeName);
                    ReplacePlaceholdersInParagraph(paragraph, "[Position]", position);
                }

                // Save changes
                wordDoc.MainDocumentPart.Document.Save();
            }


            // Console.WriteLine($"Offer letter generated and uploaded: {filePath}");
        }

        private void ReplacePlaceholdersInParagraph(Paragraph paragraph, string placeholder, string replacement)
        {
            // Combine all Text nodes within the paragraph
            var texts = paragraph.Descendants<Text>();
            string combinedText = string.Join("", texts.Select(t => t.Text));

            // Replace the placeholder if found
            if (combinedText.Contains(placeholder))
            {
                combinedText = combinedText.Replace(placeholder, replacement);

                // Clear existing Text nodes
                foreach (var text in texts)
                    text.Text = string.Empty;

                // Split the updated text into new Text nodes
                var firstText = texts.First();
                firstText.Text = combinedText;
            }
        }
    }
}
