
--Sql.GetEmployeesByCycleIdAndStatus

/**
DECLARE @cycleId int = 3;
DECLARE @status NVARCHAR(50) = 'Approved'
**/


select distinct empGoal.EMP_Info_Id, emp.FullName
	from PMS_GoalApprovals AS goalAppr
		inner join PMS_EmployeeGoals AS empGoal ON empGoal.GoalId = goalAppr.PMS_EmployeeGoals_GoalId
		inner join EMP_Info AS emp ON emp.Id = empGoal.EMP_Info_Id
	where goalAppr.ApprovalStatus = @status
		and goalAppr.MS_PerformanceCycles_Id = @cycleId