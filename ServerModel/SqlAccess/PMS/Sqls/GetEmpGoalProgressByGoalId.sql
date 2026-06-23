
--Sql.GetEmpGoalProgressByGoalId

/**

DECLARE @CompId UNIQUEIDENTIFIER = 'C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4';
DECLARE @goalId INT = 1010;

**/


SELECT DISTINCT
		 empPerfCheckIns.FormDate
		,empGoal.MS_PerformanceCycles_Id
		,cycle.CycleName
		,empGoal.GoalTitle
		,empGoal.MS_GoalCategories_CategoryId
		,goalCat.CategoryName
		,empPerfCheckIns.ProgressPercentage
		,empPerfCheckIns.Comments
		,empGoalId.EMP_Info_Id
        ,emp.FullName AS 'EmployeeName'
FROM PMS_PerformanceCheckIns AS empPerfCheckIns
INNER JOIN PMS_EmployeeGoals AS empGoalId
    ON empPerfCheckIns.PMS_EmployeeGoals_GoalId = empGoalId.GoalId
INNER JOIN EMP_Info AS emp ON emp.Id =empGoalId.EMP_Info_Id
INNER JOIN PMS_EmployeeGoals AS empGoal ON empGoal.GoalId = empPerfCheckIns.PMS_EmployeeGoals_GoalId
INNER JOIN MS_PerformanceCycles AS cycle ON empGoal.MS_PerformanceCycles_Id = cycle.CycleId
INNER JOIN MS_GoalCategories AS goalCat ON goalCat.CategoryId = empGoal.MS_GoalCategories_CategoryId
WHERE empPerfCheckIns.CompId = @CompId
	AND empPerfCheckIns.PMS_EmployeeGoals_GoalId = @goalId
    AND empPerfCheckIns.Active = 1
    AND empGoalId.Active = 1;