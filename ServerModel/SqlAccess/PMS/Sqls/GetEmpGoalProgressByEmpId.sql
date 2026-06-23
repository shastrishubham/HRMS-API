

--select * from PMS_PerformanceCheckIns
--select * from PMS_EmployeeGoals

--DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
--DECLARE @EmpId UNIQUEIDENTIFIER = 'EA186978-58F9-4153-9863-DCD4E7AF4F18';

SELECT 
		 empPerfCheckIns.CheckInId
		,empGoalId.EMP_Info_Id
		,empGoalId.GoalTitle
		,empGoalId.GoalDescription
		,empGoalId.Weightage
		,empGoalId.TargetValue
		,empPerfCheckIns.ProgressPercentage
		,empPerfCheckIns.Comments
  FROM PMS_PerformanceCheckIns AS empPerfCheckIns
	INNER JOIN PMS_EmployeeGoals AS empGoalId ON empPerfCheckIns.PMS_EmployeeGoals_GoalId = empGoalId.GoalId
	INNER JOIN PMS_GoalApprovals AS pmsGoalApp ON pmsGoalApp.PMS_EmployeeGoals_GoalId = empGoalId.GoalId
  WHERE 
		empPerfCheckIns.CompId = @CompId
			AND empGoalId.EMP_Info_Id = @EmpId
		AND empPerfCheckIns.Active = 1 AND empGoalId.Active = 1

