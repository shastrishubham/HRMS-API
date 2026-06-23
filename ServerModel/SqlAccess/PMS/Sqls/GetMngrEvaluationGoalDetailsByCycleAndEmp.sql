
--Sql.GetMngrEvaluationGoalDetailsByCycleAndEmp

--DECLARE @empId UNIQUEIDENTIFIER = '5DE2FE64-4D6B-4E75-BA75-B43A143BC4BD'
--DECLARE @cycleId INT = 3


SELECT 
	   pmsSelfDetail.PMS_EmployeeGoals_GoalId
	  ,msPmsCycle.CycleName
      ,pmsSelfDetail.SelfRating
	  ,pmsSelfEmp.FullName
      ,pmsSelfDetail.Comments
	  ,empGoal.GoalDescription
	  ,empGoal.GoalTitle
	  ,pmsSelfEval.OverallComments
	  ,mngrEval.Id AS 'PMS_ManagerEvaluations_Id'
	  ,mngrEval.ManagerId
	  ,mngrEval.OverallRating AS 'MngrOverallRating'
	  ,mngrEval.OverallComments AS 'MngrOverallComments'
	  ,mngrEvalDetail.Id AS 'MngrEvalDetailId'
	  ,mngrEvalDetail.PMS_EmployeeGoals_GoalId AS 'MngrGoalId'
	  ,mngrEvalDetail.ManagerRating As 'MngrGoalRating'
	  ,mngrEvalDetail.ManagerComments AS 'MngrGoalComment'
  FROM PMS_EmployeeGoals AS empGoal
	INNER JOIN PMS_SelfEvaluationDetails AS pmsSelfDetail ON pmsSelfDetail.PMS_EmployeeGoals_GoalId = empGoal.GoalId AND pmsSelfDetail.Active = 1
	INNER JOIN PMS_SelfEvaluations AS pmsSelfEval ON pmsSelfEval.SelfEvaluationId = pmsSelfDetail.PMS_SelfEvaluations_SelfEvaluationId
	LEFT JOIN PMS_ManagerEvaluationDetails AS mngrEvalDetail ON mngrEvalDetail.PMS_EmployeeGoals_GoalId = empGoal.GoalId AND mngrEvalDetail.Active = 1
	LEFT JOIN PMS_ManagerEvaluations AS mngrEval ON mngrEval.Id = mngrEvalDetail.PMS_ManagerEvaluations_Id
	INNER JOIN EMP_Info AS pmsSelfEmp ON empGoal.EMP_Info_Id = pmsSelfEmp.Id AND pmsSelfEmp.IsActive = 1
	INNER JOIN MS_PerformanceCycles AS msPmsCycle ON msPmsCycle.CycleId = empGoal.MS_PerformanceCycles_Id  AND msPmsCycle.Active = 1
 WHERE empGoal.EMP_Info_Id = @empId
	AND empGoal.MS_PerformanceCycles_Id = @cycleId
	AND empGoal.Active = 1 AND msPmsCycle.Active = 1