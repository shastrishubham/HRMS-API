

BEGIN

    BEGIN TRY

--DECLARE @Json NVARCHAR(MAX) = 
--N'
--[
--  {
--    "Id": 0,
--    "FormDate": "2026-06-01",
--    "MachineId": "Id",
--    "MachineIp": "192.168.1.1",
--    "CompId": "C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4",
--    "EMP_Info_Id": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "MS_PerformanceCycles_CycleId": 4,
--	"PMS_CalibrationReviews_Id": 8,
--    "FinalRating": 4.5155,
--    "MS_PerformanceBand_Id": 2,
--    "BonusEligible": true,
--    "PromotionRecommendation": true,
--    "PromotionEligible": false,
--    "BonusAmount": 50000,
--    "IncrementPercentage": 12,
--    "DevelopmentPlan": "Improve leadership--1111",
--    "PublishedDate": "2026-06-10",
--    "OutcomeStatus": "Published",
--    "ApprovedBy": "C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4",
--    "ApprovedDate": "2026-06-12",
--    "AppraisalLetterGenerated": true,
--    "AppraisalLetterGeneratedDate": "2026-06-13",
--    "PromotionEffectiveDate": "2026-07-01",
--    "IncrementEffectiveDate": "2026-07-01",
--    "Remarks": "Good performance",
--    "IsAcknowledgedByEmployee": true,
--    "AcknowledgedDate": "2026-06-14",
--    "AmmendId": 0,
--    "IsAmmend": false,
--    "CreatedBy": "C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4",
--    "CreatedOn": "2026-06-01",
--    "ModifiedBy": "C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4",
--    "ModifiedOn": "2026-06-15"
--  }
--]
--';

------------------------------------------------------------
-- STEP 1: JSON → TEMP TABLE
------------------------------------------------------------
IF OBJECT_ID('tempdb..#SourceData') IS NOT NULL DROP TABLE #SourceData;

SELECT *
INTO #SourceData
FROM OPENJSON(@Json)
WITH (
    Id INT,
    FormDate DATETIME2,
    MachineId NVARCHAR(50),
    MachineIp NVARCHAR(50),
    CompId UNIQUEIDENTIFIER,
    EMP_Info_Id UNIQUEIDENTIFIER,
    MS_PerformanceCycles_CycleId INT,
	PMS_CalibrationReviews_Id INT,
    FinalRating DECIMAL(5,2),   -- FIXED
    MS_PerformanceBand_Id INT,
    BonusEligible BIT,
    PromotionRecommendation BIT,
    PromotionEligible BIT,
    BonusAmount DECIMAL(18,2),  -- FIXED (VERY IMPORTANT)
    IncrementPercentage DECIMAL(18,2),
    DevelopmentPlan NVARCHAR(MAX),
    PublishedDate DATETIME2,
    OutcomeStatus NVARCHAR(50),
    ApprovedBy UNIQUEIDENTIFIER,
    ApprovedDate DATETIME2,
    AppraisalLetterGenerated BIT,
    AppraisalLetterGeneratedDate DATETIME2,
    PromotionEffectiveDate DATETIME2,
    IncrementEffectiveDate DATETIME2,
    Remarks NVARCHAR(MAX),
    IsAcknowledgedByEmployee BIT,
    AcknowledgedDate DATETIME2,
    AmmendId INT,
    IsAmmend BIT,
    CreatedBy UNIQUEIDENTIFIER,
    CreatedOn DATETIME2,
    ModifiedBy UNIQUEIDENTIFIER,
    ModifiedOn DATETIME2
);

------------------------------------------------------------
-- STEP 2: Mark old records
------------------------------------------------------------
UPDATE T
SET
    T.IsAmmend = 1,
    T.AmmendId = S.Id,
    T.ModifiedOn = GETDATE(),
    T.ModifiedBy = S.ModifiedBy
FROM HRMS.dbo.PMS_PerformanceOutcomes T
INNER JOIN #SourceData S
    ON T.Id = S.Id
WHERE ISNULL(T.IsAmmend, 0) = 0;

------------------------------------------------------------
-- STEP 3: INSERT NEW VERSION (FIXED ALIGNMENT)
------------------------------------------------------------
INSERT INTO HRMS.dbo.PMS_PerformanceOutcomes
(
    FormDate,
    MachineId,
    MachineIp,
    CompId,
    EMP_Info_Id,
    MS_PerformanceCycles_CycleId,
	PMS_CalibrationReviews_Id,
    FinalRating,
    MS_PerformanceBand_Id,
    BonusEligible,
    PromotionRecommendation,
    PromotionEligible,
    BonusAmount,
    IncrementPercentage,
    DevelopmentPlan,
    PublishedDate,
    OutcomeStatus,
    ApprovedBy,
    ApprovedDate,
    AppraisalLetterGenerated,
    AppraisalLetterGeneratedDate,
    PromotionEffectiveDate,
    IncrementEffectiveDate,
    Remarks,
    IsAcknowledgedByEmployee,
    AcknowledgedDate,
    AmmendId,
    IsAmmend,
    CreatedBy,
    CreatedOn,
    ModifiedBy,
    ModifiedOn
)
SELECT
    GETDATE(),
    ISNULL(S.MachineId, ''),
    ISNULL(S.MachineIp, ''),
    S.CompId,
    S.EMP_Info_Id,
    S.MS_PerformanceCycles_CycleId,
	S.PMS_CalibrationReviews_Id,
    S.FinalRating,
    S.MS_PerformanceBand_Id,
    S.BonusEligible,
    S.PromotionRecommendation,
    S.PromotionEligible,
    S.BonusAmount,
    S.IncrementPercentage,
    S.DevelopmentPlan,
    S.PublishedDate,
    S.OutcomeStatus,
    S.ApprovedBy,
    S.ApprovedDate,
    S.AppraisalLetterGenerated,
    S.AppraisalLetterGeneratedDate,
    S.PromotionEffectiveDate,
    S.IncrementEffectiveDate,
    S.Remarks,
    S.IsAcknowledgedByEmployee,
    S.AcknowledgedDate,
    S.Id AS AmmendId,
    0 AS IsAmmend,
    S.CreatedBy,
    S.CreatedOn,
    S.ModifiedBy,
    S.ModifiedOn
FROM #SourceData S;

 SELECT CAST(1 AS BIT) AS IsSuccess;
END TRY
BEGIN CATCH
    SELECT CAST(0 AS BIT) AS IsSuccess;
END CATCH
END