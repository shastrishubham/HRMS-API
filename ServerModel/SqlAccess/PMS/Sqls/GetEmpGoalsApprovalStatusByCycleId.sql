
--Sql.GetEmpGoalsApprovalStatusByCycleId

/**

DECLARE @cycleId INT = 3;
DECLARE @compId UNIQUEIDENTIFIER = 'C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4'

**/

select 
		 empGoal.GoalId AS 'PMS_EmployeeGoals_GoalId'
		,empGoal.FormDate
		,empGoal.CompId
		,empGoal.EMP_Info_Id
		,emp.FullName AS 'EmployeeName'
		,empGoal.MS_PerformanceCycles_Id
		,pmsCycle.CycleName
		,empGoal.MS_GoalCategories_CategoryId
		,goalCat.CategoryName
		,empGoal.GoalTitle
		,empGoal.Status
		,goalApproval.ApproverId
		,apprEmp.FullName AS 'ApproverName'
		,goalApproval.ApprovalStatus
		,goalApproval.Comment
	from PMS_EmployeeGoals AS empGoal
		LEFT JOIN PMS_GoalApprovals AS goalApproval ON empGoal.GoalId = goalApproval.PMS_EmployeeGoals_GoalId
			AND goalApproval.Active = 1
		INNER JOIN EMP_Info AS emp ON empGoal.EMP_Info_Id = emp.Id
		INNER JOIN MS_PerformanceCycles AS pmsCycle ON pmsCycle.CycleId = empGoal.MS_PerformanceCycles_Id
		INNER JOIN MS_GoalCategories AS goalCat ON goalCat.CategoryId = empGoal.MS_GoalCategories_CategoryId
		LEFT JOIN EMP_Info AS apprEmp ON apprEmp.Id = goalApproval.ApproverId
	where empGoal.MS_PerformanceCycles_Id = @cycleId
		AND empGoal.Active = 1
		AND empGoal.CompId = @compId