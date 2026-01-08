using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model;
using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using ServerModel.Model.Recruitment;
using ServerModel.Repository;
using ServerModel.Repository.Recruitment;
using ServerModel.SqlAccess.Recruitment.Interviews;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Masters.Recruitment
{
    public class RecruitmentHelper
    {
        IRespository<Req_JbVacancy> jobVacancyRespository = null;
        IRespository<Req_JbForm> jobApplicationRespository = null;
        IRespository<Req_InterviewSch> interviewScheduleRespository = null;
        IRespository<MS_Designation> designationRespository = null;
        IRespository<EMP_Info> employeeRespository = null;
        IRespository<MS_Branch> branchRespository = null;
        IRespository<Req_InterviewFedbck> intFeedbackRepository = null;

        JobVacancyFormRepository jobVacancyFormRepository;
        OnlineFormApplicationRepository onlineFormApplicationRepository;
        InterviewPortalRepository interviewPortalRepository;
        InterviewFeedbackRepository interviewFeedbackRepository;

        EmployeeRespository employeeInfoRespository;

        public RecruitmentHelper()
        {
            jobVacancyRespository = new Repository<Req_JbVacancy>();
            jobApplicationRespository = new Repository<Req_JbForm>();
            interviewScheduleRespository = new Repository<Req_InterviewSch>();
            designationRespository = new Repository<MS_Designation>();
            employeeRespository = new Repository<EMP_Info>();
            branchRespository = new Repository<MS_Branch>();
            intFeedbackRepository = new Repository<Req_InterviewFedbck>();

            jobVacancyFormRepository = new JobVacancyFormRepository();
            onlineFormApplicationRepository = new OnlineFormApplicationRepository();
            interviewPortalRepository = new InterviewPortalRepository();
            interviewFeedbackRepository = new InterviewFeedbackRepository();

            employeeInfoRespository = new EmployeeRespository();
        }

        #region Properties Interface

        public static IInterviewSetupAccess mInterviewSetupAccessT
            = new InterviewSetupAccessWrapper();

        #endregion

        public DataResult AddUpdateCreateJobVacancy(JobVacancyForm jobVacancyForm)
        {
            return jobVacancyFormRepository.AddUpdateCreateJobVacancy(jobVacancyForm);
        }

        public IEnumerable<JobVacancyForm> GetJobVacanciesByCompId(Guid companyId)
        {
            var result = (from jobvacancy in jobVacancyRespository.GetAll()
                          join designation in designationRespository.GetAll() on jobvacancy.MS_Designation_Id equals designation.Id
                          join empRepo in employeeRespository.GetAll() on jobvacancy.HiringManager_Id equals empRepo.Id
                          join branch in branchRespository.GetAll() on jobvacancy.MS_Branch_Id equals branch.Id
                          where jobvacancy.CompId == companyId && jobvacancy.Active == true
                         select new JobVacancyForm
                          {
                              Id = jobvacancy.Id,
                              MS_Designation_Id = jobvacancy.MS_Designation_Id,
                              DesignationName = designation.DesignationName,
                              MS_Branch_Id = jobvacancy.MS_Branch_Id,
                              BranchName = branch.BranchName,
                              HiringManager_Id = jobvacancy.HiringManager_Id,
                              HiringManagerEmployeeName = empRepo.FullName,
                              NosOfPosition = jobvacancy.NosOfPosition,
                              JobDesc = jobvacancy.JobDesc,
                              FormDate = jobvacancy.FormDate
                          }).OrderByDescending(x=>x.FormDate);
            return result;
        }

        public DataResult DeleteJobVacancy(JobVacancyForm jobVacancyForm)
        {
            return jobVacancyFormRepository.DeleteJobVacancy(jobVacancyForm);
        }

        public DataResult AddUpdateOnlineFormApplication(OnlineFormApplication onlineFormApplication)
        {
            return onlineFormApplicationRepository.AddUpdateOnlineFormApplication(onlineFormApplication);
        }

        public IEnumerable<OnlineFormApplication> GetJobVacanciesApplicationsByCompId(Guid companyId)
        {
            var result = (from jobapplication in jobApplicationRespository.GetAll()
                          join empRepo in employeeRespository.GetAll() on jobapplication.EMP_Info_Id equals empRepo.Id into g
                            from ct in g.DefaultIfEmpty()
                          join branch in branchRespository.GetAll() on jobapplication.MS_Branch_Id equals branch.Id
                          join jobvacancy in jobVacancyRespository.GetAll() on jobapplication.Req_JbVacancy_Id equals jobvacancy.Id
                          join designation in designationRespository.GetAll() on jobvacancy.MS_Designation_Id equals designation.Id
                          where jobapplication.CompId == companyId && jobapplication.Active == true
                          select new OnlineFormApplication
                          {
                              Id = jobapplication.Id,
                              EMP_Info_Id = jobapplication.EMP_Info_Id,
                              ReferredBy = ct?.FullName,
                              FullName = jobapplication.FullName,
                              Email = jobapplication.Email,
                              ContactNo = jobapplication.ContactNo,
                              YearOfExp = jobapplication.YearOfExp,
                              MS_Branch_Id = jobapplication.MS_Branch_Id,
                              PreferredJobLocation = branch.BranchName,
                              Req_JbVacancy_Id = jobapplication.Req_JbVacancy_Id,
                              DesignationName = designation.DesignationName,
                              Resume = jobapplication.Resume,
                              Comments = jobapplication.Comments,
                              FormDate = jobapplication.FormDate
                          }).OrderByDescending(x=>x.FormDate);
            return result;
        }

        public DataResult DeleteOnlineFormApplication(OnlineFormApplication onlineFormApplication)
        {
            return onlineFormApplicationRepository.DeleteOnlineFormApplication(onlineFormApplication);
        }

        public DataResult AddUpdateInterviewScheduleApplication(InterviewPortalInformation interviewPortalInformation)
        {
            Guid id = interviewPortalInformation.Id;

            bool isAlreadyScheduled = IsInterviewScheduleForCandidate(interviewPortalInformation.Req_JbForm_Id, ref id);

            if (isAlreadyScheduled)
                interviewPortalInformation.Id = id;

            return interviewPortalRepository.AddUpdateInterviewScheduleApplication(interviewPortalInformation);
        }

        public bool IsInterviewScheduleForCandidate(Guid? candidateId, ref Guid interviewScheduleId)
        {
            Guid interviewId = interviewScheduleId;
            var interviewScheduleDetails = this.interviewScheduleRespository.GetAll().
                Where(x => x.Req_JbForm_Id == candidateId && x.Active == true || x.Id == interviewId).FirstOrDefault();

            if (interviewScheduleDetails == null)
                return false;
            else
            {
                interviewScheduleId = interviewScheduleDetails.Id;
                return true;
            }
        }


        public IEnumerable<InterviewPortalInformation> GetScheduleInterviewsByCompId(Guid companyId)
        {
            DateTime todaysdate = DateTime.Now;

            var result = (from interviewSchedule in interviewScheduleRespository.GetAll()
                          join jobVacancy in jobVacancyRespository.GetAll() on interviewSchedule.Req_JbVacancy_Id equals jobVacancy.Id
                          join onlineApplication in jobApplicationRespository.GetAll() on interviewSchedule.Req_JbForm_Id equals onlineApplication.Id
                          join designation in designationRespository.GetAll() on jobVacancy.MS_Designation_Id equals designation.Id
                          join empRepo in employeeRespository.GetAll() on jobVacancy.HiringManager_Id equals empRepo.Id
                          join branch in branchRespository.GetAll() on jobVacancy.MS_Branch_Id equals branch.Id
                          join preferredbranch in branchRespository.GetAll() on onlineApplication.MS_Branch_Id equals preferredbranch.Id
                          join interviewtakenEmp in employeeRespository.GetAll() on interviewSchedule.EMP_Info_Id equals interviewtakenEmp.Id
                          join referredBy in employeeRespository.GetAll() on onlineApplication.EMP_Info_Id equals referredBy.Id
                                into _referredBy from referredBy in _referredBy.DefaultIfEmpty()
                          where interviewSchedule.CompId == companyId && interviewSchedule.Active == true
                               && interviewSchedule.InterviewDateTime >= todaysdate
                          select new InterviewPortalInformation
                          {
                              Id = interviewSchedule.Id,
                              Req_JbVacancy_Id = interviewSchedule.Req_JbVacancy_Id,
                              DesignationName = designation.DesignationName,
                              RequiredbranchName = branch.BranchName,
                              HiringManager_Id = jobVacancy.HiringManager_Id,
                              HiringManagerName = empRepo.FullName,
                              Req_JbForm_Id = interviewSchedule.Req_JbForm_Id,
                              ReferredById = onlineApplication.EMP_Info_Id,
                              ReferredByName = referredBy == null ? string.Empty : referredBy.FullName,
                              CandidateName = onlineApplication.FullName,
                              CandidateEmail = onlineApplication.Email,
                              CandidateContact = onlineApplication.ContactNo,
                              Resume = onlineApplication.Resume,
                              InterviewDateTime = interviewSchedule.InterviewDateTime,
                              Method = interviewSchedule.Method,
                              InterviewTakenById = interviewSchedule.EMP_Info_Id,
                              InterviewTakenByName = interviewtakenEmp.FullName,
                              InterviewStatus = interviewSchedule.InterviewStatus,
                              Comments = interviewSchedule.Comments,
                              FormDate = interviewSchedule.FormDate
                          }).OrderByDescending(x=>x.FormDate);

            return result;
        }

        public IEnumerable<OnlineFormApplication> GetJobApplicationsByVacancy(Guid vacancyId)
        {
            var result = (from jobApplication in jobApplicationRespository.GetAll()
                          where jobApplication.Req_JbVacancy_Id.Equals(vacancyId) && jobApplication.Active == true
                          select new OnlineFormApplication
                          {
                              Id = jobApplication.Id,
                              FullName = jobApplication.FullName
                          });
            return result;
        }

        public OnlineFormApplication GetJobApplicationDetailByApplicationId(Guid applicationId)
        {
            OnlineFormApplication result = (from jobApplication in jobApplicationRespository.GetAll()
                          where jobApplication.Id.Equals(applicationId) && jobApplication.Active == true
                          select new OnlineFormApplication
                          {
                              Id = jobApplication.Id,
                              FullName = jobApplication.FullName
                          }).FirstOrDefault();
            return result;
        }

        public DataResult AddUpdateInterviewFeedback(InterviewFeedback interviewFeedback)
        {
            /// Update Interview Status = FeedbackPending in Interview Schduled table
            var interviewDetails = interviewScheduleRespository.GetAll().Where(x => x.Id == interviewFeedback.Req_InterviewSch_Id && x.Active == true).FirstOrDefault();

            if (interviewDetails != null)
            {
                UpdateInterviewStatus(InterviewStatusTypes.FeedbackDone, interviewDetails.Req_JbForm_Id.Value);
            }

            return interviewFeedbackRepository.AddUpdateInterviewFeedback(interviewFeedback);
        }


        public IEnumerable<InterviewFeedback> GetInterviewFeedbackByCompId(Guid companyId)
        {
            int pendingFeedackApplicants = (int)InterviewStatusTypes.Scheduled;
            var result = (from interviewFeedback in intFeedbackRepository.GetAll()
                          join interviewSchedule in interviewScheduleRespository.GetAll() on interviewFeedback.Req_InterviewSch_Id equals interviewSchedule.Id
                          join jobVacancy in jobVacancyRespository.GetAll() on interviewSchedule.Req_JbVacancy_Id equals jobVacancy.Id
                          join onlineApplication in jobApplicationRespository.GetAll() on interviewSchedule.Req_JbForm_Id equals onlineApplication.Id
                          join designation in designationRespository.GetAll() on jobVacancy.MS_Designation_Id equals designation.Id
                          join empRepo in employeeRespository.GetAll() on jobVacancy.HiringManager_Id equals empRepo.Id
                          join branch in branchRespository.GetAll() on jobVacancy.MS_Branch_Id equals branch.Id
                          join preferredbranch in branchRespository.GetAll() on onlineApplication.MS_Branch_Id equals preferredbranch.Id
                          join interviewtakenEmp in employeeRespository.GetAll() on interviewSchedule.EMP_Info_Id equals interviewtakenEmp.Id
                          join referredBy in employeeRespository.GetAll() on onlineApplication.EMP_Info_Id equals referredBy.Id
                                into _referredBy
                          from referredBy in _referredBy.DefaultIfEmpty()
                          where interviewSchedule.CompId == companyId && interviewSchedule.Active == true
                            && interviewSchedule.InterviewStatus == pendingFeedackApplicants
                          select new InterviewFeedback
                          {
                              Id = interviewFeedback.Id,
                              FormDate = interviewFeedback.FormDate,
                              CandidateName = onlineApplication.FullName,
                              CandidateEmail = onlineApplication.Email,
                              CandidateContact = onlineApplication.ContactNo,
                              DesignationName = designation.DesignationName,
                              RequiredbranchName = branch.BranchName,
                              PreferredBranchName = preferredbranch.BranchName,
                              HiringManager_Id = empRepo.Id,
                              HiringManagerName = empRepo.FullName,
                              YearOfExp = onlineApplication.YearOfExp,
                              InterviewTakenById = interviewtakenEmp.Id,
                              InterviewTakenByName = interviewtakenEmp.FullName,
                              MS_InterviewRate_Id = interviewFeedback.MS_InterviewRate_Id.HasValue ? interviewFeedback.MS_InterviewRate_Id.Value : 0,
                              Feedback = interviewFeedback.Feedback,

                              Req_InterviewSch_Id = interviewSchedule.Id

                          }).OrderByDescending(x=>x.FormDate);
            return result;


        }

        public IEnumerable<InterviewPortalInformation> GetApplicantsByStatus(Guid companyId, int interviewStatusId)
        {
            var result = (from interviewSchedule in interviewScheduleRespository.GetAll()
                          join jobVacancy in jobVacancyRespository.GetAll() on interviewSchedule.Req_JbVacancy_Id equals jobVacancy.Id
                          join onlineApplication in jobApplicationRespository.GetAll() on interviewSchedule.Req_JbForm_Id equals onlineApplication.Id
                          join designation in designationRespository.GetAll() on jobVacancy.MS_Designation_Id equals designation.Id
                          join empRepo in employeeRespository.GetAll() on jobVacancy.HiringManager_Id equals empRepo.Id
                          join branch in branchRespository.GetAll() on jobVacancy.MS_Branch_Id equals branch.Id
                          join preferredbranch in branchRespository.GetAll() on onlineApplication.MS_Branch_Id equals preferredbranch.Id
                          join interviewtakenEmp in employeeRespository.GetAll() on interviewSchedule.EMP_Info_Id equals interviewtakenEmp.Id
                          join referredBy in employeeRespository.GetAll() on onlineApplication.EMP_Info_Id equals referredBy.Id
                                into _referredBy
                          from referredBy in _referredBy.DefaultIfEmpty()
                          where interviewSchedule.CompId == companyId && interviewSchedule.Active == true
                              && interviewSchedule.InterviewStatus == interviewStatusId
                          select new InterviewPortalInformation
                          {
                              Id = interviewSchedule.Id,
                              DesignationName = designation.DesignationName,
                              CandidateName = onlineApplication.FullName,
                          });

            return result;
        }

        public DataResult UpdateInterviewStatus(InterviewStatusTypes interviewStatusTypes, Guid candidateId)
        {
            try
            {
                var interviewScheduleDetails = interviewScheduleRespository.GetAll().Where(x => x.Req_JbForm_Id == candidateId && x.Active == true).FirstOrDefault();

                if (interviewScheduleDetails != null)
                {
                    interviewScheduleDetails.InterviewStatus = (int)interviewStatusTypes;
                    interviewScheduleRespository.Update(interviewScheduleDetails);
                    interviewScheduleRespository.Save();

                    return new DataResult { IsSuccess = true, ErrorMessage = "Interview Status Updated......" };
                }

                return new DataResult { IsSuccess = false, ErrorMessage = "Candidate Details are not find." };

            }
            catch (Exception e)
            {
                return new DataResult { IsSuccess = false, ErrorMessage = e.Message.ToString() };
            }
            
        }

        public DataResult AddUpdateCandidateStatus(Guid schduledCandidateId, InterviewStatusTypes interviewStatusTypes)
        {
            try
            {
                var interviewScheduleDetails = interviewScheduleRespository.GetAll().Where(x => x.Id == schduledCandidateId && x.Active == true).FirstOrDefault();

                if (interviewScheduleDetails != null)
                {
                    interviewScheduleDetails.InterviewStatus = (int)interviewStatusTypes;
                    interviewScheduleRespository.Update(interviewScheduleDetails);
                    interviewScheduleRespository.Save();

                    InterviewStatusTypes[] insertedInterviewStatusTypes = new InterviewStatusTypes[]
                    {
                        InterviewStatusTypes.Selected,
                        InterviewStatusTypes.Hired
                    };

                    if (insertedInterviewStatusTypes.Contains(interviewStatusTypes))
                    {
                        // Insert Entry into EMP_Info table

                      //  var candidateDetail = jobApplicationRespository.GetAll().FirstOrDefault(x => x.Id == interviewScheduleDetails.Req_JbForm_Id && x.Active == true);

                        var candidateDetail = (from jobApp in jobApplicationRespository.GetAll()
                                      join jobVacancy in jobVacancyRespository.GetAll() on jobApp.Req_JbVacancy_Id equals jobVacancy.Id
                                      where jobApp.Active == true && jobApp.Id == interviewScheduleDetails.Req_JbForm_Id
                                      select new EmployeeInformation
                                      {
                                         FullName = jobApp.FullName,
                                         MS_Branch_Id = jobApp.MS_Branch_Id,
                                         MS_Designation_Id = jobVacancy.MS_Designation_Id,
                                         CompId = jobApp.CompId
                                      }).FirstOrDefault();

                        if (candidateDetail != null)
                        {
                            if (!string.IsNullOrEmpty(candidateDetail.FullName))
                            {
                                string[] ssize = candidateDetail.FullName.Split(new char[0]);
                                EmployeeInformation employeeInformation = new EmployeeInformation()
                                {
                                    FirstName = ssize.Count() > 0 ? ssize[0] : string.Empty,
                                    MiddleName = ssize.Count() > 0 ? ssize.Count() > 1 ? ssize[1] : string.Empty : string.Empty,
                                    LastName = ssize.Count() > 0 ? ssize.Count() > 2 ? ssize[2] : string.Empty : string.Empty,

                                    FullName = candidateDetail.FullName,
                                    MS_Branch_Id = candidateDetail.MS_Branch_Id,
                                    MS_Designation_Id = candidateDetail.MS_Designation_Id,
                                    CompId = candidateDetail.CompId,
                                };

                                employeeInfoRespository.AddUpdateEmployee(employeeInformation);
                            }
                        }
                    }

                    return new DataResult { IsSuccess = true, ErrorMessage = "Interview Status Updated......" };
                }

                return new DataResult { IsSuccess = false, ErrorMessage = "Candidate Details are not find." };

            }
            catch (Exception e)
            {
                return new DataResult { IsSuccess = false, ErrorMessage = e.Message.ToString() };
            }
        }

        public List<InterviewPortalInformation> GetScheduleInterviewsByCompId(Guid compId, InterviewStatusTypes interviewStatus = InterviewStatusTypes.None)
        {
            return mInterviewSetupAccessT.GetScheduleInterviewsByCompId(compId, interviewStatus);
        }

        public List<InterviewFeedback> GetInterviewFeedbacksByCompIdAndRating(Guid compId, int interviewRateId = 0)
        {
            return mInterviewSetupAccessT.GetInterviewFeedbacksByCompIdAndRating(compId, interviewRateId);
        }
    }
}
