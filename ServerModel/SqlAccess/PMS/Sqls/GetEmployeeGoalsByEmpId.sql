

/*
DECLARE @EmpId UNIQUEIDENTIFIER = '5DE2FE64-4D6B-4E75-BA75-B43A143BC4BD';
DECLARE @cycleId INT = 3;
*/

 SET NOCOUNT ON;

    SELECT DISTINCT
         empGoal.GoalId
        ,empGoal.FormDate
        ,empGoal.MachineId
        ,empGoal.MachineIp
        ,empGoal.CompId
        ,empGoal.EMP_Info_Id
		,emp.FullName
		,empGoal.MS_PerformanceCycles_Id
		,pmsCycle.CycleName
		,empGoal.MS_GoalCategories_CategoryId
		,goalCat.CategoryName
		,empGoal.GoalTitle
		,empGoal.GoalDescription
		,empGoal.Weightage
		,empGoal.TargetValue
		,empGoal.AchievementCriteria
		,empGoal.Status
    FROM PMS_EmployeeGoals as empGoal
		INNER JOIN EMP_Info AS emp ON empGoal.EMP_Info_Id = emp.Id
		INNER JOIN MS_PerformanceCycles AS pmsCycle ON pmsCycle.CycleId = empGoal.MS_PerformanceCycles_Id
		INNER JOIN MS_GoalCategories AS goalCat ON goalCat.CategoryId= empGoal.MS_GoalCategories_CategoryId
    WHERE 
        empGoal.EMP_Info_Id = @EmpId
		AND empGoal.MS_PerformanceCycles_Id = @cycleId
    ORDER BY FormDate ASC;