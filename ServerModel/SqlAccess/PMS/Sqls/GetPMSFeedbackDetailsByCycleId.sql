
-- GetPMSFeedbackDetailsByCycleId.Sql


/*
DECLARE @cycleId INT = 1

*/


SELECT pmsFedbck.Id
      ,pmsFedbck.FromEmployeeId
	  ,Fromemp.FullName AS 'FromEmployeeName'
      ,pmsFedbck.ToEmployeeId
	  ,Toemp.FullName AS 'ToEmployeeName'
      ,pmsFedbck.MS_PerformanceCycles_CycleId
	  ,cycle.CycleName
      ,pmsFedbck.FeedbackType
      ,pmsFedbck.FeedbackText
      ,pmsFedbck.Rating
      ,pmsFedbck.SubmittedDate
  FROM [PMS_Feedbacks] AS pmsFedbck
	INNER JOIN EMP_Info AS Fromemp ON pmsFedbck.FromEmployeeId = Fromemp.Id AND Fromemp.IsActive = 1
	INNER JOIN EMP_Info AS Toemp ON pmsFedbck.ToEmployeeId = Toemp.Id AND Toemp.IsActive = 1
	INNER JOIN MS_PerformanceCycles AS cycle ON pmsFedbck.MS_PerformanceCycles_CycleId = cycle.CycleId AND cycle.Active = 1
 WHERE pmsFedbck.MS_PerformanceCycles_CycleId = @cycleId