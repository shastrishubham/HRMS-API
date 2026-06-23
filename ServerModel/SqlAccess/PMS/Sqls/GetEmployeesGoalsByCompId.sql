


--DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
--DECLARE @cycleId INT = 1;


 SET NOCOUNT ON;

    SELECT DISTINCT
         empGoal.CompId
        ,empGoal.EMP_Info_Id
		,emp.FullName
    FROM PMS_EmployeeGoals as empGoal
		INNER JOIN EMP_Info AS emp ON empGoal.EMP_Info_Id = emp.Id
    WHERE 
        empGoal.CompId = @CompId
         AND empGoal.MS_PerformanceCycles_Id = @cycleId