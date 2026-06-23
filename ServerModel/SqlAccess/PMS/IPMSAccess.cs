using ServerModel.Entity.PMS;
using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using ServerModel.Model.PMS;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace ServerModel.SqlAccess.PMS
{
    public interface IPMSAccess
    {
        #region Employee Goals
        List<PMSEmpGoals> GetEmployeesGoalsByCompId(Guid compId, int cycleId); // return distinct employees by cycle id
        List<PMSEmpGoals> GetEmployeeGoalsByEmpId(Guid empId, int cycleId);
        bool UpsertEmployeeGoals(List<PMSEmployeeGoals> employeeGoals);
        #endregion

        #region Approved/Reject Employee Goals

        List<PMSEmpGoalApproval> GetEmpGoalsApprovalStatusByCycleId(Guid compId, int cycleId);
        GoalDetails GetGoalDetailsByGoadId(Guid compId, int goalId);
        bool UpsertPMSGoalApprovals(List<PMSGoalApprovals> goalApprovals);
        List<PMSEmpGoalApproval> GetGoalApprovals(Guid compId, DateTime fromDt, DateTime toDt);

        #endregion

        #region EmpUpdateGoal Progress

        List<EmployeeInformation> GetEmployeesByCycleIdAndStatus(int cycleId, string status);
        int UpsertPMSCheckIns(PMSPerformanceCheckIns performanceCheckIns);
        List<PMSEmpCheckIns> GetEmpGoalProgressByGoalId(Guid compId, int goalId);
        List<PMSEmpCheckIns> GetEmpGoalCheckIns(Guid compId, Guid empId);

        #endregion

        #region Self Evaluations
        
        bool UpsertSelfEvaluations(EmpSelfEvaluations selfEvaluation);

        List<EmpSelfEvaluations> GetEmployeesSelfEvaluationsByStatus(Guid compId, int cycleId, string status);

        List<EmpSelfEvaluationDetails> GetEmpSelfEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId);

        #endregion

        #region Manager Evaluations

        List<MngrEvaluationGoalDetail> GetMngrEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId);

        bool UpsertManagerEvaluations(MngrEvaluation mngrEvaluation);

        List<MngrEvaluation> GetMngrEvaluationsByManagerAndStatus(Guid compId, string status);

        List<MngrEvaluation> GetMngrEvaluationsByDates(Guid compId, DateTime frmdt, DateTime todt);

        #endregion

        #region HR Calibration Review

        List<HRCalibrationReviewDetail> GetMngrEvaluatedEmpByCycleId(int cycleId, string status);

        bool UpsertHRCalibrationReview(List<HRCalibrationReview> calibrationReviews);

        List<HRCalibrationReviewDetail> GetHRCalibrationReviewsByCycleId(Guid compId, int cycleId);

        #endregion

        #region Published Outcomes

        List<PMSPerformanceOutcomesDetail> GetPerformanceOutcomesByCycleId(int cycleId);
        bool UpsertPerformanceOutcomes(List<PMSPerformanceOutcomes> performanceOutcomes);


        #endregion

        #region FeedBack

        int UpsertEmployeeFeedback(PMSFeedbacks feedback);

        List<PMSFeedbackDetails> GetPMSFeedbackDetailsByCycleId(int cycleId);

        #endregion
    }
}
