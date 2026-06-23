
--Sql.GetGoalDetailsByGoadId

/*
DECLARE @goalId INT = 1010;
DECLARE @compId UNIQUEIDENTIFIER = 'C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4'
*/

select 
		 empGoal.GoalId
		,empGoal.FormDate
		,empGoal.CompId
		,empGoal.EMP_Info_Id
		,emp.FullName AS 'EmployeeName'
		,emp.EmpId
		,dept.DepartmentName
		,desg.DesignationName
		,branch.BranchName
		,empGoal.MS_PerformanceCycles_Id
		,pmsCycle.StartDate
		,pmsCycle.EndDate
		,pmsCycle.CycleName
		,empGoal.MS_GoalCategories_CategoryId
		,goalCat.CategoryName
		,empGoal.GoalTitle
		,empGoal.GoalDescription
		,empGoal.TargetValue
		,empGoal.AchievementCriteria
		,empGoal.Weightage
		,empGoal.Status
		,goalApproval.ApproverId
		,apprEmp.FullName AS 'ApproverName'
		,goalApproval.ApprovalStatus
		,goalApproval.Comment
	from PMS_EmployeeGoals AS empGoal
		LEFT JOIN PMS_GoalApprovals AS goalApproval ON empGoal.GoalId = goalApproval.PMS_EmployeeGoals_GoalId
			AND goalApproval.Active = 1
		INNER JOIN EMP_Info AS emp ON empGoal.EMP_Info_Id = emp.Id AND emp.IsActive = 1
		INNER JOIN MS_Dept AS dept ON dept.Id = emp.MS_Dept_Id
		INNER JOIN MS_Designation AS desg ON desg.Id = emp.MS_Designation_Id
		INNER JOIN MS_Branch AS branch ON branch.Id = emp.MS_Branch_Id
		INNER JOIN MS_PerformanceCycles AS pmsCycle ON pmsCycle.CycleId = empGoal.MS_PerformanceCycles_Id
		INNER JOIN MS_GoalCategories AS goalCat ON goalCat.CategoryId = empGoal.MS_GoalCategories_CategoryId
		LEFT JOIN EMP_Info AS apprEmp ON apprEmp.Id = goalApproval.ApproverId
	where empGoal.GoalId = @goalId
		AND empGoal.Active = 1
		AND empGoal.CompId = @compId