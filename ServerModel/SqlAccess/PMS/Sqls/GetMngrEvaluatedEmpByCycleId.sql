---GetMngrEvaluatedEmpByCycleId.Sql

--DECLARE @status NVARCHAR(50) = 'Submitted'
--DECLARE @cycleId INT = 4

SELECT 
	 mngrEval.EMP_Info_Id
	,emp.FullName AS 'EmployeeName'
	FROM PMS_ManagerEvaluations AS mngrEval
		INNER JOIN EMP_Info AS emp ON emp.Id = mngrEval.EMP_Info_Id AND emp.IsActive = 1
		INNER JOIN MS_PerformanceCycles AS cycle ON cycle.CycleId = mngrEval.MS_PerformanceCycles_CycleId
			AND cycle.Active = 1
	WHERE mngrEval.Active = 1
		AND mngrEval.MS_PerformanceCycles_CycleId = @cycleId
		AND mngrEval.Status = @status
