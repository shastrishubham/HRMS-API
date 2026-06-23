 

 ---UpsertEmpGoalApproval.Sql

-- DECLARE @json NVARCHAR(MAX) = N'
-- [
--  {
--    "ApprovalId": 0,
--    "FormDate": "2026-05-28T10:00:00",
--    "MachineId": "M1",
--    "MachineIp": "192.168.1.10",
--    "CompId": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--    "ApproverId": "C7716840-5EEE-4EC7-8E10-2AAF274252AD",
--    "MS_PerformanceCycles_Id": 1,
--    "MS_GoalCategories_CategoryId": 1,
--    "PMS_EmployeeGoals_GoalId": 5,
--    "ApprovalStatus": "Approved",
--    "Comment": "Good work",
--    "Active": 1,
--    "CreatedBy": "33333333-3333-3333-3333-333333333333",
--    "CreatedOn": "2026-05-28T10:00:00",
--    "ModifiedBy": "33333333-3333-3333-3333-333333333333",
--    "ModifiedOn": "2026-05-28T10:00:00"
--  }
--]'
 
 BEGIN TRY
 SET NOCOUNT ON;

    MERGE PMS_GoalApprovals AS Target
    USING
    (
        SELECT
            CAST(ApprovalId AS INT) AS ApprovalId,
            MachineId,
            MachineIp,
            CAST(CompId AS UNIQUEIDENTIFIER) AS CompId,
            CAST(ApproverId AS UNIQUEIDENTIFIER) AS ApproverId,
            CAST(MS_PerformanceCycles_Id AS INT) AS MS_PerformanceCycles_Id,
            CAST(MS_GoalCategories_CategoryId AS INT) AS MS_GoalCategories_CategoryId,
            CAST(PMS_EmployeeGoals_GoalId AS INT) AS PMS_EmployeeGoals_GoalId,
            ApprovalStatus,
            Comment,
            Active,
            CAST(CreatedBy AS UNIQUEIDENTIFIER) AS CreatedBy,
            CAST(ModifiedBy AS UNIQUEIDENTIFIER) AS ModifiedBy

        FROM OPENJSON(@Json)
        WITH
        (
            ApprovalId INT,
            MachineId NVARCHAR(255),
            MachineIp NVARCHAR(255),
            CompId UNIQUEIDENTIFIER,
            ApproverId UNIQUEIDENTIFIER,
            MS_PerformanceCycles_Id INT,
            MS_GoalCategories_CategoryId INT,
            PMS_EmployeeGoals_GoalId INT,
            ApprovalStatus NVARCHAR(50),
            Comment NVARCHAR(MAX),
            Active BIT,
            CreatedBy UNIQUEIDENTIFIER,
            ModifiedBy UNIQUEIDENTIFIER
        )
    ) AS Source

    ON Target.ApprovalId = Source.ApprovalId

    WHEN MATCHED THEN
        UPDATE SET
			Target.MS_PerformanceCycles_Id = Source.MS_PerformanceCycles_Id,
			Target.MS_GoalCategories_CategoryId = Source.MS_GoalCategories_CategoryId,
            Target.ApprovalStatus = Source.ApprovalStatus,
            Target.Comment = Source.Comment,
            Target.Active = Source.Active,
            Target.ModifiedBy = Source.ModifiedBy,
            Target.ModifiedOn = GETDATE()

    WHEN NOT MATCHED THEN
        INSERT
        (
            FormDate,
            MachineId,
            MachineIp,
            CompId,
            ApproverId,
            MS_PerformanceCycles_Id,
            MS_GoalCategories_CategoryId,
            PMS_EmployeeGoals_GoalId,
            ApprovalStatus,
            Comment,
            Active,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn
        )
        VALUES
        (
            GETDATE(),
            Source.MachineId,
            Source.MachineIp,
            Source.CompId,
            Source.ApproverId,
            Source.MS_PerformanceCycles_Id,
            Source.MS_GoalCategories_CategoryId,
            Source.PMS_EmployeeGoals_GoalId,
            Source.ApprovalStatus,
            Source.Comment,
            Source.Active,
            Source.CreatedBy,
            GETDATE(),
            Source.ModifiedBy,
            GETDATE()
        );

-- Now update PMS_EmployeeGoals based on GoalId from Source
UPDATE G
SET G.Status = S.ApprovalStatus
FROM PMS_EmployeeGoals AS G
INNER JOIN
(
    SELECT CAST(PMS_EmployeeGoals_GoalId AS INT) AS GoalId, 
			ApprovalStatus
    FROM OPENJSON(@Json)
    WITH (PMS_EmployeeGoals_GoalId INT, ApprovalStatus NVARCHAR(50))
) AS S
ON G.GoalId = S.GoalId;

 -- SUCCESS FLAG
 SELECT 1 AS Success;

END TRY
BEGIN CATCH
    -- FAILURE FLAG
    SELECT 0 AS Success;
END CATCH
