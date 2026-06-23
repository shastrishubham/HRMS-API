using Microsoft.SqlServer.Server;
using ServerModel.Entity.PMS;
using ServerModel.Model.Employee;
using ServerModel.Model.PMS;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace ServerModel.SqlAccess.PMS
{
    public class PMSAccessWrapper : IPMSAccess
    {
        public List<PMSEmpGoals> GetEmployeeGoalsByEmpId(Guid empId, int cycleId)
        {
            return PMSAccess.GetEmployeeGoalsByEmpId(empId, cycleId);
        }

        public List<PMSEmpGoals> GetEmployeesGoalsByCompId(Guid compId, int cycleId)
        {
            return PMSAccess.GetEmployeesGoalsByCompId(compId, cycleId);
        }

        public bool UpsertEmployeeGoals(List<PMSEmployeeGoals> employeeGoals)
        {
           return PMSAccess.UpsertEmployeeGoals(employeeGoals);
        }

        public List<PMSEmpGoalApproval> GetEmpGoalsApprovalStatusByCycleId(Guid compId, int cycleId)
        {
            return PMSAccess.GetEmpGoalsApprovalStatusByCycleId(compId, cycleId);
        }

        public GoalDetails GetGoalDetailsByGoadId(Guid compId, int goalId)
        {
            return PMSAccess.GetGoalDetailsByGoadId(compId, goalId);
        }

        public List<PMSEmpGoalApproval> GetGoalApprovals(Guid compId, DateTime fromDt, DateTime toDt)
        {
            return PMSAccess.GetGoalApprovals(compId, fromDt, toDt);
        }

        public bool UpsertPMSGoalApprovals(List<PMSGoalApprovals> goalApprovals)
        {
            return PMSAccess.UpsertPMSGoalApprovals(goalApprovals);
        }

        public List<EmployeeInformation> GetEmployeesByCycleIdAndStatus(int cycleId, string status)
        {
            return PMSAccess.GetEmployeesByCycleIdAndStatus(cycleId, status);
        }

        public int UpsertPMSCheckIns(PMSPerformanceCheckIns performanceCheckIns)
        {
            return PMSAccess.UpsertPMSCheckIns(performanceCheckIns);
        }

        public List<PMSEmpCheckIns> GetEmpGoalProgressByGoalId(Guid compId, int goalId)
        {
            return PMSAccess.GetEmpGoalProgressByGoalId(compId, goalId);
        }

        public List<PMSEmpCheckIns> GetEmpGoalCheckIns(Guid compId, Guid empId)
        {
            return PMSAccess.GetEmpGoalCheckIns(compId, empId);
        }

        public bool UpsertSelfEvaluations(EmpSelfEvaluations empSelfEvaluations)
        {
            return PMSAccess.UpsertSelfEvaluations(empSelfEvaluations);
        }

        public List<EmpSelfEvaluations> GetEmployeesSelfEvaluationsByStatus(Guid compId, int cycleId, string status)
        {
            return PMSAccess.GetEmployeesSelfEvaluationsByStatus(compId, cycleId, status);
        }

        public bool UpsertManagerEvaluations(MngrEvaluation mngrEvaluation)
        {
            return PMSAccess.UpsertManagerEvaluations(mngrEvaluation);
        }

        public List<MngrEvaluation> GetMngrEvaluationsByManagerAndStatus(Guid compId, string status)
        {
            return PMSAccess.GetMngrEvaluationsByManagerAndStatus(compId, status);
        }

        public List<MngrEvaluation> GetMngrEvaluationsByDates(Guid compId, DateTime frmdt, DateTime todt)
        {
            return PMSAccess.GetMngrEvaluationsByDates(compId, frmdt, todt);
        }

        public List<HRCalibrationReviewDetail> GetMngrEvaluatedEmpByCycleId(int cycleId, string status)
        {
            return PMSAccess.GetMngrEvaluatedEmpByCycleId(cycleId, status);
        }

        public bool UpsertHRCalibrationReview(List<HRCalibrationReview> calibrationReviews)
        {
            return PMSAccess.UpsertHRCalibrationReview(calibrationReviews); 
        }

        public List<HRCalibrationReviewDetail> GetHRCalibrationReviewsByCycleId(Guid compId, int cycleId)
        {
            return PMSAccess.GetHRCalibrationReviewsByCycleId(compId, cycleId);
        }

        public List<EmpSelfEvaluationDetails> GetEmpSelfEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId)
        {
           return PMSAccess.GetEmpSelfEvaluationGoalDetailsByCycleAndEmp(empId, cycleId);
        }

        public List<MngrEvaluationGoalDetail> GetMngrEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId)
        {
            return PMSAccess.GetMngrEvaluationGoalDetailsByCycleAndEmp(empId, cycleId);
        }

        public List<PMSPerformanceOutcomesDetail> GetPerformanceOutcomesByCycleId(int cycleId)
        {
            return PMSAccess.GetPerformanceOutcomesByCycleId(cycleId);
        }

        public bool UpsertPerformanceOutcomes(List<PMSPerformanceOutcomes> performanceOutcomes)
        {
            return PMSAccess.UpsertPerformanceOutcomes(performanceOutcomes);
        }

        public int UpsertEmployeeFeedback(PMSFeedbacks feedback)
        {
            return PMSAccess.UpsertEmployeeFeedback(feedback);
        }

        public List<PMSFeedbackDetails> GetPMSFeedbackDetailsByCycleId(int cycleId)
        {
            return PMSAccess.GetPMSFeedbackDetailsByCycleId(cycleId);
        }
    }
}
