
---Sql.GetPerformanceOutcomesByCycleId

--DECLARE @cycleId INT = 4;

SELECT 
		 calibration.Id AS 'PMS_CalibrationReviews_Id'
		,calibration.MS_PerformanceCycles_CycleId
		,calibration.FinalRating
		,calibration.EMP_Info_Id
		,emp.FullName AS 'EmployeeName'
		,perfOutcms.Id
		,perfOutcms.MS_PerformanceBand_Id
		,perfOutcms.BonusEligible
		,perfOutcms.PromotionRecommendation
		,perfOutcms.PromotionEligible
		,perfOutcms.BonusAmount
		,perfOutcms.IncrementPercentage
		,perfOutcms.DevelopmentPlan
		,perfOutcms.PublishedDate
		,perfOutcms.OutcomeStatus
		,perfOutcms.ApprovedBy
		,perfOutcms.ApprovedDate
		,perfOutcms.AppraisalLetterGenerated
		,perfOutcms.AppraisalLetterGeneratedDate
		,perfOutcms.PromotionEffectiveDate
		,perfOutcms.IncrementEffectiveDate
		,perfOutcms.Remarks
		,perfOutcms.IsAcknowledgedByEmployee
		,perfOutcms.AcknowledgedDate
		
	FROM PMS_CalibrationReviews AS calibration
		INNER JOIN PMS_ManagerEvaluations AS mngrEval ON mngrEval.Id = calibration.PMS_ManagerEvaluations_Id
		INNER JOIN EMP_Info AS emp ON emp.Id = calibration.EMP_Info_Id AND emp.IsActive = 1
		LEFT JOIN PMS_PerformanceOutcomes AS perfOutcms ON perfOutcms.PMS_CalibrationReviews_Id = calibration.Id AND perfOutcms.IsAmmend = 0
	WHERE calibration.MS_PerformanceCycles_CycleId = @cycleId
		AND calibration.Active = 1
		AND mngrEval.Active = 1