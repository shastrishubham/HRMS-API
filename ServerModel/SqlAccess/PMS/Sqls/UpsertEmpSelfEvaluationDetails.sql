/******************************************************************
MULTIPLE ROW UPSERT USING OPENJSON
TABLE: [HRMS].[dbo].[PMS_SelfEvaluationDetails]
******************************************************************/
--select * from PMS_SelfEvaluationDetails

--DECLARE @Json NVARCHAR(MAX) = N'
--[
--    {
--        "Id": 10,
--        "FormDate": "2026-05-28",
--        "MachineId": "Id",
--        "MachineIp": "192.168.1.10",
--        "CompId": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--        "PMS_SelfEvaluations_SelfEvaluationId": 1,
--        "PMS_EmployeeGoals_GoalId": 5,
--        "SelfRating": 4.2,
--        "Comments": "Good progress",
--        "Active": true,
--        "CreatedBy": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--        "CreatedOn": "2026-05-28T10:00:00",
--        "ModifiedBy": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--        "ModifiedOn": "2026-05-28T10:00:00"
--    }
--]';

MERGE PMS_SelfEvaluationDetails AS TARGET
USING
(
    SELECT *
    FROM OPENJSON(@Json)
    WITH
    (
         Id                                          INT
        ,FormDate                                    DATETIME
        ,MachineId                                   NVARCHAR(50)
        ,MachineIp                                   NVARCHAR(50)
        ,CompId                                      UNIQUEIDENTIFIER
        ,PMS_SelfEvaluations_SelfEvaluationId        INT
        ,PMS_EmployeeGoals_GoalId                    INT
        ,SelfRating                                  DECIMAL(18,2)
        ,Comments                                    NVARCHAR(MAX)
        ,Active                                      BIT
        ,CreatedBy                                   NVARCHAR(100)
        ,CreatedOn                                   DATETIME2
        ,ModifiedBy                                  NVARCHAR(100)
        ,ModifiedOn                                  DATETIME2
    )
) AS SOURCE

ON TARGET.Id = SOURCE.Id

WHEN MATCHED THEN
    UPDATE SET
         TARGET.PMS_EmployeeGoals_GoalId             = SOURCE.PMS_EmployeeGoals_GoalId
        ,TARGET.SelfRating                           = SOURCE.SelfRating
        ,TARGET.Comments                             = SOURCE.Comments
        ,TARGET.Active                               = SOURCE.Active
        ,TARGET.ModifiedBy                           = SOURCE.ModifiedBy
        ,TARGET.ModifiedOn                           = SOURCE.ModifiedOn

WHEN NOT MATCHED THEN
    INSERT
    (
         [FormDate]
        ,[MachineId]
        ,[MachineIp]
        ,[CompId]
        ,[PMS_SelfEvaluations_SelfEvaluationId]
        ,[PMS_EmployeeGoals_GoalId]
        ,[SelfRating]
        ,[Comments]
        ,[Active]
        ,[CreatedBy]
        ,[CreatedOn]
        ,[ModifiedBy]
        ,[ModifiedOn]
    )
    VALUES
    (
         GETDATE()
        ,SOURCE.MachineId
        ,SOURCE.MachineIp
        ,SOURCE.CompId
        ,SOURCE.PMS_SelfEvaluations_SelfEvaluationId
        ,SOURCE.PMS_EmployeeGoals_GoalId
        ,SOURCE.SelfRating
        ,SOURCE.Comments
        ,SOURCE.Active
        ,SOURCE.CreatedBy
        ,GETDATE()
        ,SOURCE.ModifiedBy
        ,GETDATE()
    );

-- OPTIONAL:
-- OUTPUT $action, inserted.*;