
--Sql.GetEmployeesSelfEvaluationsByMngrId

--DECLARE @compId UNIQUEIDENTIFIER  = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680'
--DECLARE @managerId UNIQUEIDENTIFIER  = 'EA186978-58F9-4153-9863-DCD4E7AF4F18'
--DECLARE @status NVARCHAR(50) = 'draft'

SELECT mngrEval.Id
      ,mngrEval.EMP_Info_Id
	  ,emp.FullName AS 'EmpName'
      ,mngrEval.ManagerId
	  ,empMngr.FullName AS 'ManagerName' 
      ,mngrEval.MS_PerformanceCycles_CycleId
	  ,permCycle.CycleName
      ,mngrEval.OverallRating
      ,mngrEval.OverallComments
      ,mngrEval.SubmittedDate
      ,mngrEval.Status
  FROM PMS_ManagerEvaluations AS mngrEval
	INNER JOIN MS_PerformanceCycles AS permCycle ON permCycle.CycleId = mngrEval.MS_PerformanceCycles_CycleId
	INNER JOIN EMP_Info AS emp ON emp.Id = mngrEval.EMP_Info_Id AND emp.IsActive = 1
	INNER JOIN EMP_Info AS empMngr ON empMngr.Id = mngrEval.ManagerId AND empMngr.IsActive = 1
 WHERE mngrEval.CompId = @compId
	AND mngrEval.Status = @status
	AND mngrEval.ManagerId = @managerId
    AND mngrEval.Active = 1
