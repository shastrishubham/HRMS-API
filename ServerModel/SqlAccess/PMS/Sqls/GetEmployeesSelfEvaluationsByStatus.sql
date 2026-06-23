
--Sql.GetEmployeesSelfEvaluationsByStatus

--select * from PMS_SelfEvaluations
--select * from PMS_SelfEvaluationDetails

--DECLARE @compId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
--DECLARE @status NVARCHAR(50) = 'Draft';
--DECLARE @cycleId INT = 3


SELECT pmsSelf.[SelfEvaluationId]
      ,pmsSelf.[MS_PerformanceCycles_CycleId]
	  ,msPmsCycle.CycleName
      ,pmsSelf.[EMP_Info_Id]
	  ,pmsSelfEmp.FullName
      ,pmsSelf.[SubmittedDate]
      ,pmsSelf.[OverallComments]
      ,pmsSelf.[Status]
  FROM [PMS_SelfEvaluations] AS pmsSelf
	INNER JOIN EMP_Info AS pmsSelfEmp ON pmsSelf.EMP_Info_Id = pmsSelfEmp.Id AND pmsSelfEmp.IsActive = 1
	INNER JOIN MS_PerformanceCycles AS msPmsCycle ON msPmsCycle.CycleId = pmsSelf.MS_PerformanceCycles_CycleId  AND msPmsCycle.Active = 1
 WHERE pmsSelf.CompId = @compId
	AND pmsSelf.Status = @status
	AND pmsSelf.MS_PerformanceCycles_CycleId = @cycleId
	and pmsSelf.Active = 1
	and pmsSelfEmp.IsActive = 1
	and msPmsCycle.Active = 1