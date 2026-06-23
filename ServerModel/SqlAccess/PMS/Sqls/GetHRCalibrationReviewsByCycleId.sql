


--Sl.GetHRCalibrationReviewsByCycleId

--select * from PMS_EmployeeGoals
--select * from PMS_SelfEvaluations
--select * from PMS_ManagerEvaluations
--select * from PMS_CalibrationReviews
----UPDATE PMS_CalibrationReviews SET PMS_ManagerEvaluations_Id =1 where Id = 1


--DECLARE @compId UNIQUEIDENTIFIER = NEWID();
--DECLARE @cycleId INT = 1

SELECT 
		 review.Id
		,review.FormDate
		,review.EMP_Info_Id
		,emp.FullName AS 'EmployeeName'
		,review.MS_PerformanceCycles_CycleId
		,cycle.CycleName
		,review.PMS_ManagerEvaluations_Id
		,mngrEmp.FullName AS 'ManagerName'
		,review.ReviewerId
		,reviewer.FullName AS 'ReviewerName'
		,review.FinalRating
		,review.Comments
		,review.ReviewDate
	FROM PMS_CalibrationReviews AS review
		INNER JOIN EMP_INFo AS emp ON review.EMP_Info_Id = emp.Id AND emp.IsActive = 1
		INNER JOIN MS_PerformanceCycles AS cycle ON cycle.CycleId = review.MS_PerformanceCycles_CycleId AND cycle.Active = 1
		INNER JOIN PMS_ManagerEvaluations AS mngr ON mngr.Id = review.PMS_ManagerEvaluations_Id AND mngr.Active = 1
		INNER JOIN EMP_Info AS mngrEmp ON mngrEmp.Id = mngr.ManagerId
		INNER JOIN EMP_Info AS reviewer ON reviewer.Id = review.ReviewerId
	WHERE review.Active = 1
		AND review.CompId = @compId
		AND review.MS_PerformanceCycles_CycleId = @cycleId
	