using ServerModel.Data;
using ServerModel.Entity.PMS;
using ServerModel.Model.Employee;
using ServerModel.Model.HR;
using ServerModel.Model.PMS;
using ServerModel.ServerModel.HR;
using ServerModel.ServerModel.PMS;
using ServerModel.SqlAccess.PMS;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PerformanceMgmtController : ApiController
    {
        public PerformanceMgmtController()
        {
            DbContext.ConnectionString = WebApiConfig.ConnectionString;
        }


        #region Performance Goals

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetEmployeeGoalsByEmpId")]
        public List<PMSEmpGoals> GetEmployeeGoalsByEmpId(Guid empId, int cycleId)
        {
            return PMSInfoServer.GetEmployeeGoalsByEmpId(empId, cycleId);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetEmployeesGoalsByCompId")]
        public List<PMSEmpGoals> GetEmployeesGoalsByCompId(Guid compId, int cycleId)
        {
            return PMSInfoServer.GetEmployeesGoalsByCompId(compId, cycleId);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Performance/UpsertEmployeeGoals")]
        public bool UpsertEmployeeGoals(List<PMSEmployeeGoals> employeeGoals)
        {
            return PMSInfoServer.UpsertEmployeeGoals(employeeGoals);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetEmpGoalsApprovalStatusByCycleId")]
        public List<PMSEmpGoalApproval> GetEmpGoalsApprovalStatusByCycleId(Guid compId, int cycleId)
        {
            return PMSInfoServer.GetEmpGoalsApprovalStatusByCycleId(compId, cycleId);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetGoalDetailsByGoadId")]
        public GoalDetails GetGoalDetailsByGoadId(Guid compId, int goalId)
        {
            return PMSInfoServer.GetGoalDetailsByGoadId(compId, goalId);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetGoalApprovals")]
        public List<PMSEmpGoalApproval> GetGoalApprovals(Guid compId, DateTime fromDt, DateTime toDt)
        {
            return PMSInfoServer.GetGoalApprovals(compId, fromDt, toDt);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetEmployeesByCycleIdAndStatus")]
        public List<EmployeeInformation> GetEmployeesByCycleIdAndStatus(int cycleId, string status)
        {
            return PMSInfoServer.GetEmployeesByCycleIdAndStatus(cycleId, status);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Performance/UpsertPMSGoalApprovals")]

        public bool UpsertPMSGoalApprovals(List<PMSGoalApprovals> goalApprovals)
        {
            return PMSInfoServer.UpsertPMSGoalApprovals(goalApprovals);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Performance/UpsertPMSCheckIns")]
        public int UpsertPMSCheckIns(PMSPerformanceCheckIns performanceCheckIns)
        {
            return PMSInfoServer.UpsertPMSCheckIns(performanceCheckIns);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetEmpGoalProgressByGoalId")]
        public List<PMSEmpCheckIns> GetEmpGoalProgressByGoalId(Guid compId, int goalId)
        {
            return PMSInfoServer.GetEmpGoalProgressByGoalId(compId, goalId);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetEmpGoalCheckIns")]
        public List<PMSEmpCheckIns> GetEmpGoalCheckIns(Guid compId, Guid empId)
        {
            return PMSInfoServer.GetEmpGoalCheckIns(compId, empId);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/Performance/UpsertSelfEvaluations")]
        public bool UpsertSelfEvaluations(EmpSelfEvaluations empSelfEvaluations)
        {
            return PMSInfoServer.UpsertSelfEvaluations(empSelfEvaluations);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetEmployeesSelfEvaluationsByStatus")]
        public List<EmpSelfEvaluations> GetEmployeesSelfEvaluationsByStatus(Guid compId, int cycleId, string status)
        {
            return PMSInfoServer.GetEmployeesSelfEvaluationsByStatus(compId, cycleId, status);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetEmpSelfEvaluationGoalDetailsByCycleAndEmp")]
        public List<EmpSelfEvaluationDetails> GetEmpSelfEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId)
        {
            return PMSInfoServer.GetEmpSelfEvaluationGoalDetailsByCycleAndEmp(empId, cycleId);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetMngrEvaluationGoalDetailsByCycleAndEmp")]
        public List<MngrEvaluationGoalDetail> GetMngrEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId)
        {
            return PMSInfoServer.GetMngrEvaluationGoalDetailsByCycleAndEmp(empId, cycleId);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Performance/UpsertManagerEvaluations")]
        public bool UpsertManagerEvaluations(MngrEvaluation mngrEvaluation)
        {
            return PMSInfoServer.UpsertManagerEvaluations(mngrEvaluation);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetMngrEvaluationsByManagerAndStatus")]
        public List<MngrEvaluation> GetMngrEvaluationsByManagerAndStatus(Guid compId, string status)
        {
            return PMSInfoServer.GetMngrEvaluationsByManagerAndStatus(compId, status);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetMngrEvaluationsByDates")]
        public List<MngrEvaluation> GetMngrEvaluationsByDates(Guid compId, DateTime frmdt, DateTime todt)
        {
            return PMSInfoServer.GetMngrEvaluationsByDates(compId, frmdt, todt);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetMngrEvaluatedEmpByCycleId")]
        public List<HRCalibrationReviewDetail> GetMngrEvaluatedEmpByCycleId(int cycleId, string status)
        {
            return PMSInfoServer.GetMngrEvaluatedEmpByCycleId(cycleId, status);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetPerformanceOutcomesByCycleId")]

        public List<PMSPerformanceOutcomesDetail> GetPerformanceOutcomesByCycleId(int cycleId)
        {
            return PMSInfoServer.GetPerformanceOutcomesByCycleId(cycleId);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Performance/UpsertHRCalibrationReview")]
        public bool UpsertHRCalibrationReview(List<HRCalibrationReview> calibrationReviews)
        {
            return PMSInfoServer.UpsertHRCalibrationReview(calibrationReviews);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetHRCalibrationReviewsByCycleId")]
        public List<HRCalibrationReviewDetail> GetHRCalibrationReviewsByCycleId(Guid compId, int cycleId)
        {
            return PMSInfoServer.GetHRCalibrationReviewsByCycleId(compId, cycleId);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Performance/UpsertPerformanceOutcomes")]
        public bool UpsertPerformanceOutcomes(List<PMSPerformanceOutcomes> performanceOutcomes)
        {
            return PMSInfoServer.UpsertPerformanceOutcomes(performanceOutcomes);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Performance/UpsertEmployeeFeedback")]
        public int UpsertEmployeeFeedback(PMSFeedbacks feedback)
        {
            return PMSInfoServer.UpsertEmployeeFeedback(feedback);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Performance/GetPMSFeedbackDetailsByCycleId")]
        public List<PMSFeedbackDetails> GetPMSFeedbackDetailsByCycleId(int cycleId)
        {
            return PMSInfoServer.GetPMSFeedbackDetailsByCycleId(cycleId);
        }

        #endregion

    }
}
