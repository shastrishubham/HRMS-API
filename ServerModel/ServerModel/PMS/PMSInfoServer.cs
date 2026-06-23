using ServerModel.Entity.PMS;
using ServerModel.Model.Employee;
using ServerModel.Model.PMS;
using ServerModel.SqlAccess.PMS;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace ServerModel.ServerModel.PMS
{
    public class PMSInfoServer
    {
        #region Properties Interface

        public static IPMSAccess mPMSAccess = new PMSAccessWrapper();

        #endregion

        public static List<PMSEmpGoals> GetEmployeeGoalsByEmpId(Guid empId, int cycleId)
        {
            return mPMSAccess.GetEmployeeGoalsByEmpId(empId, cycleId);
        }

        public static List<PMSEmpGoals> GetEmployeesGoalsByCompId(Guid compId, int cycleId)
        {
            return mPMSAccess.GetEmployeesGoalsByCompId(compId, cycleId);
        }

        public static bool UpsertEmployeeGoals(List<PMSEmployeeGoals> employeeGoals)
        {
            return mPMSAccess.UpsertEmployeeGoals(employeeGoals);
        }

        public static List<PMSEmpGoalApproval> GetGoalApprovals(Guid compId, DateTime fromDt, DateTime toDt)
        {
            return mPMSAccess.GetGoalApprovals(compId, fromDt, toDt);
        }

        public static List<PMSEmpGoalApproval> GetEmpGoalsApprovalStatusByCycleId(Guid compId, int cycleId)
        {
            return mPMSAccess.GetEmpGoalsApprovalStatusByCycleId(compId, cycleId);
        }

        public static GoalDetails GetGoalDetailsByGoadId(Guid compId, int goalId)
        {
            return mPMSAccess.GetGoalDetailsByGoadId(compId, goalId);
        }

        public static bool UpsertPMSGoalApprovals(List<PMSGoalApprovals> goalApprovals)
        {
            return mPMSAccess.UpsertPMSGoalApprovals(goalApprovals);
        }

        public static List<EmployeeInformation> GetEmployeesByCycleIdAndStatus(int cycleId, string status)
        {
            return mPMSAccess.GetEmployeesByCycleIdAndStatus(cycleId, status);
        }

        public static int UpsertPMSCheckIns(PMSPerformanceCheckIns performanceCheckIns)
        {
            return mPMSAccess.UpsertPMSCheckIns(performanceCheckIns);
        }

        public static List<PMSEmpCheckIns> GetEmpGoalProgressByGoalId(Guid compId, int goalId)
        {
            return mPMSAccess.GetEmpGoalProgressByGoalId(compId, goalId);
        }

        public static List<PMSEmpCheckIns> GetEmpGoalCheckIns(Guid compId, Guid empId)
        {
            return mPMSAccess.GetEmpGoalCheckIns(compId, empId);
        }

        public static bool UpsertSelfEvaluations(EmpSelfEvaluations empSelfEvaluations)
        {
            return mPMSAccess.UpsertSelfEvaluations(empSelfEvaluations);
        }


        public static List<EmpSelfEvaluations> GetEmployeesSelfEvaluationsByStatus(Guid compId, int cycleId, string status)
        {
            return mPMSAccess.GetEmployeesSelfEvaluationsByStatus(compId, cycleId, status);
        }

        public static List<EmpSelfEvaluationDetails> GetEmpSelfEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId)
        {
            return mPMSAccess.GetEmpSelfEvaluationGoalDetailsByCycleAndEmp(empId, cycleId);
        }

        public static List<MngrEvaluationGoalDetail> GetMngrEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId)
        {
            return mPMSAccess.GetMngrEvaluationGoalDetailsByCycleAndEmp(empId, cycleId);
        }

        public static bool UpsertManagerEvaluations(MngrEvaluation mngrEvaluation)
        {
            return mPMSAccess.UpsertManagerEvaluations(mngrEvaluation);
        }

        public static List<MngrEvaluation> GetMngrEvaluationsByManagerAndStatus(Guid compId, string status)
        {
            return mPMSAccess.GetMngrEvaluationsByManagerAndStatus(compId, status);
        }

        public static List<MngrEvaluation> GetMngrEvaluationsByDates(Guid compId, DateTime frmdt, DateTime todt)
        {
            return mPMSAccess.GetMngrEvaluationsByDates(compId, frmdt, todt);
        }

        public static List<HRCalibrationReviewDetail> GetMngrEvaluatedEmpByCycleId(int cycleId, string status)
        {
            return mPMSAccess.GetMngrEvaluatedEmpByCycleId(cycleId, status);
        }

        public static List<PMSPerformanceOutcomesDetail> GetPerformanceOutcomesByCycleId(int cycleId)
        {
            return mPMSAccess.GetPerformanceOutcomesByCycleId(cycleId);
        }

        public static bool UpsertPerformanceOutcomes(List<PMSPerformanceOutcomes> performanceOutcomes)
        {
            return mPMSAccess.UpsertPerformanceOutcomes(performanceOutcomes);
        }

        public static bool UpsertHRCalibrationReview(List<HRCalibrationReview> calibrationReviews)
        {
            return mPMSAccess.UpsertHRCalibrationReview(calibrationReviews);
        }

        public static List<HRCalibrationReviewDetail> GetHRCalibrationReviewsByCycleId(Guid compId, int cycleId)
        {
            return mPMSAccess.GetHRCalibrationReviewsByCycleId(compId, cycleId);
        }

        public static int UpsertEmployeeFeedback(PMSFeedbacks feedback)
        {
            return mPMSAccess.UpsertEmployeeFeedback(feedback);
        }

        public static List<PMSFeedbackDetails> GetPMSFeedbackDetailsByCycleId(int cycleId)
        {
            return mPMSAccess.GetPMSFeedbackDetailsByCycleId(cycleId);
        }
    }
}
