 

 ---Sql.UpsertHRCalibrationReviews

--select * from PMS_EmployeeGoals
--select * from PMS_SelfEvaluations
--select * from PMS_ManagerEvaluations
--select * from PMS_CalibrationReviews


--DECLARE @Json NVARCHAR(MAX) = N'[
--  {
--    "Id": 1,
--    "FormDate": "2025-01-15",
--    "MachineId": "PC001",
--    "MachineIp": "192.168.1.10",
--    "CompId": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--    "EMP_Info_Id": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "MS_PerformanceCycles_CycleId": 1,
--    "PMS_ManagerEvaluations_Id": 1001,
--    "ReviewerId": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "FinalRating": 4.5,
--    "Comments": "Excellent performance",
--    "ReviewDate": "2025-01-20",
--	"Active" : 1,
--    "CreatedBy": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "CreatedOn": "2025-01-20",
--    "ModifiedBy": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "ModifiedOn": "2025-01-20"
--  },
--  {
--    "Id": 2,
--    "FormDate": "2025-01-15",
--    "MachineId": "PC002",
--    "MachineIp": "192.168.1.11",
--    "CompId": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--    "EMP_Info_Id": "C7716840-5EEE-4EC7-8E10-2AAF274252AD",
--    "MS_PerformanceCycles_CycleId": 1,
--    "PMS_ManagerEvaluations_Id": 1002,
--    "ReviewerId": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "FinalRating": 3.8,
--    "Comments": "Good performance",
--    "ReviewDate": "2025-01-20",
--	"Active" : 1,
--    "CreatedBy": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "CreatedOn": "2025-01-20",
--    "ModifiedBy": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "ModifiedOn": "2025-01-20"
--  }
--]';


BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
MERGE HRMS.dbo.PMS_CalibrationReviews AS TARGET
USING
(
    SELECT
        Id,
        FormDate,
        MachineId,
        MachineIp,
        CompId,
        EMP_Info_Id,
        MS_PerformanceCycles_CycleId,
        PMS_ManagerEvaluations_Id,
        TRY_CONVERT(UNIQUEIDENTIFIER, NULLIF(ReviewerId, '00000000-0000-0000-0000-000000000000')) AS ReviewerId,
        FinalRating,
        Comments,
        ReviewDate,
        Active,
        TRY_CONVERT(UNIQUEIDENTIFIER, NULLIF(CreatedBy, '00000000-0000-0000-0000-000000000000')) AS CreatedBy,
        TRY_CONVERT(UNIQUEIDENTIFIER, NULLIF(ModifiedBy, '00000000-0000-0000-0000-000000000000')) AS ModifiedBy
    FROM OPENJSON(@Json)
    WITH
    (
        Id INT,
        FormDate DATETIME2,
        MachineId NVARCHAR(50),
        MachineIp NVARCHAR(50),
        CompId UNIQUEIDENTIFIER,
        EMP_Info_Id UNIQUEIDENTIFIER,
        MS_PerformanceCycles_CycleId INT,
        PMS_ManagerEvaluations_Id INT,
        ReviewerId NVARCHAR(100),
        FinalRating DECIMAL(3,2),
        Comments NVARCHAR(MAX),
        ReviewDate DATETIME2,
        Active BIT,
        CreatedBy NVARCHAR(100),
        ModifiedBy NVARCHAR(100)
    )
) AS SOURCE
ON 
    TARGET.EMP_Info_Id = SOURCE.EMP_Info_Id
    AND TARGET.MS_PerformanceCycles_CycleId = SOURCE.MS_PerformanceCycles_CycleId
    AND TARGET.PMS_ManagerEvaluations_Id = SOURCE.PMS_ManagerEvaluations_Id

WHEN MATCHED THEN
    UPDATE SET
        TARGET.FormDate                     = SOURCE.FormDate,
        TARGET.MachineId                    = SOURCE.MachineId,
        TARGET.MachineIp                    = SOURCE.MachineIp,
        TARGET.CompId                       = SOURCE.CompId,
        TARGET.EMP_Info_Id                  = SOURCE.EMP_Info_Id,
        TARGET.MS_PerformanceCycles_CycleId = SOURCE.MS_PerformanceCycles_CycleId,
        TARGET.PMS_ManagerEvaluations_Id    = SOURCE.PMS_ManagerEvaluations_Id,
        TARGET.ReviewerId                   = SOURCE.ReviewerId,
        TARGET.FinalRating                  = SOURCE.FinalRating,
        TARGET.Comments                     = SOURCE.Comments,
        TARGET.ReviewDate                   = SOURCE.ReviewDate,
        TARGET.Active                       = SOURCE.Active,
        TARGET.ModifiedBy                   = SOURCE.ModifiedBy,
        TARGET.ModifiedOn                   = GETDATE()

WHEN NOT MATCHED THEN
    INSERT
    (
        FormDate,
        MachineId,
        MachineIp,
        CompId,
        EMP_Info_Id,
        MS_PerformanceCycles_CycleId,
        PMS_ManagerEvaluations_Id,
        ReviewerId,
        FinalRating,
        Comments,
        ReviewDate,
        Active,
        CreatedBy,
        CreatedOn,
        ModifiedBy,
        ModifiedOn
    )
    VALUES
    (
        SOURCE.FormDate,
        SOURCE.MachineId,
        SOURCE.MachineIp,
        SOURCE.CompId,
        SOURCE.EMP_Info_Id,
        SOURCE.MS_PerformanceCycles_CycleId,
        SOURCE.PMS_ManagerEvaluations_Id,
        SOURCE.ReviewerId,
        SOURCE.FinalRating,
        SOURCE.Comments,
        SOURCE.ReviewDate,
        SOURCE.Active,
        SOURCE.CreatedBy,
        GETDATE(),
        SOURCE.ModifiedBy,
        GETDATE()
    );

	 -- ✅ SUCCESS RESPONSE
        SELECT CAST(1 AS BIT) AS IsSuccess;

	END TRY
    BEGIN CATCH

        -- ❌ FAILURE RESPONSE
        SELECT CAST(0 AS BIT) AS IsSuccess;

    END CATCH
END