 

 

 --select NEWID()
 --select * from PMS_EmployeeGoals
 --select top 100 * from EMP_Info order by FormDate desc

-- DECLARE @json NVARCHAR(MAX) = '[
--  {
--    "GoalId": 5,
--    "FormDate": "2026-05-28T00:00:00",
--    "MachineId": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--    "MachineIp": "192.168.1.10",
--    "CompId": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--    "EMP_Info_Id": "EA186978-58F9-4153-9863-DCD4E7AF4F18",
--    "MS_PerformanceCycles_Id": 1,
--    "MS_GoalCategories_CategoryId": 1,
--    "GoalTitle": "Goal 1",
--    "GoalDescription": "Description 1",
--    "Weightage": 10,
--    "TargetValue": "Need to complete it by 100% - 1",
--    "AchievementCriteria": "Criteria 1",
--    "Status": "Active",
--    "CreatedBy": "75B3016D-D4E3-4D48-927F-A11944D3134B",
--    "CreatedOn": "2026-05-28T00:00:00",
--    "ModifiedBy": "75B3016D-D4E3-4D48-927F-A11944D3134B",
--    "ModifiedOn": "2026-05-28T00:00:00",
--    "Active":1
--  },
--  {
--    "GoalId": 0,
--    "FormDate": "2026-05-28T00:00:00",
--    "MachineId": "D33691FC-7EB9-4D1C-888C-4F4B6E204681",
--    "MachineIp": "192.168.1.11",
--    "CompId": "D33691FC-7EB9-4D1C-888C-4F4B6E204681",
--    "EMP_Info_Id": "5DE2FE64-4D6B-4E75-BA75-B43A143BC4BD",
--    "MS_PerformanceCycles_Id": 1,
--    "MS_GoalCategories_CategoryId": 1,
--    "GoalTitle": "Goal 2",
--    "GoalDescription": "Description 2",
--    "Weightage": 15,
--    "TargetValue": "Need to complete it by atleast 50%",
--    "AchievementCriteria": "Criteria 2",
--    "Status": "Active",
--    "CreatedBy": "75B3016D-D4E3-4D48-927F-A11944D3134B",
--    "CreatedOn": "2026-05-28T00:00:00",
--    "ModifiedBy": "75B3016D-D4E3-4D48-927F-A11944D3134B",
--    "ModifiedOn": "2026-05-28T00:00:00"
--    "Active":1
--  }
--]'
 
 MERGE PMS_EmployeeGoals AS Target
    USING
    (
        SELECT
            CAST(GoalId AS INT) AS GoalId,
            MachineId,
            MachineIp,
            CAST(CompId AS UNIQUEIDENTIFIER) AS CompId,
            CAST(EMP_Info_Id AS UNIQUEIDENTIFIER) AS EMP_Info_Id,
            CAST(MS_PerformanceCycles_Id AS INT) AS MS_PerformanceCycles_Id,
            CAST(MS_GoalCategories_CategoryId AS INT) AS MS_GoalCategories_CategoryId,
            GoalTitle,
            GoalDescription,
            Weightage,
            TargetValue,
            AchievementCriteria,
            Status,
            CAST(CreatedBy AS UNIQUEIDENTIFIER) AS CreatedBy,
            CAST(CreatedOn AS DATETIME2) AS CreatedOn,
            CAST(ModifiedBy AS UNIQUEIDENTIFIER) AS ModifiedBy,
            CAST(ModifiedOn AS DATETIME2) AS ModifiedOn,
            Active

        FROM OPENJSON(@Json)
        WITH
        (
            GoalId INT,
            MachineId NVARCHAR(255),
            MachineIp NVARCHAR(255),
            CompId UNIQUEIDENTIFIER,
            EMP_Info_Id UNIQUEIDENTIFIER,
            MS_PerformanceCycles_Id INT,
            MS_GoalCategories_CategoryId INT,
            GoalTitle NVARCHAR(MAX),
            GoalDescription NVARCHAR(MAX),
            Weightage DECIMAL(5,2),
            TargetValue NVARCHAR(MAX),
            AchievementCriteria NVARCHAR(MAX),
            Status NVARCHAR(50),
            CreatedBy UNIQUEIDENTIFIER,
            CreatedOn DATETIME2,
            ModifiedBy UNIQUEIDENTIFIER,
            ModifiedOn DATETIME2,
            Active BIT
        )
    ) AS Source

    ON Target.GoalId = Source.GoalId

    WHEN MATCHED THEN
        UPDATE SET
            Target.EMP_Info_Id = Source.EMP_Info_Id,
            Target.MS_PerformanceCycles_Id = Source.MS_PerformanceCycles_Id,
            Target.MS_GoalCategories_CategoryId = Source.MS_GoalCategories_CategoryId,
            Target.GoalTitle = Source.GoalTitle,
            Target.GoalDescription = Source.GoalDescription,
            Target.Weightage = Source.Weightage,
            Target.TargetValue = Source.TargetValue,
            Target.AchievementCriteria = Source.AchievementCriteria,
            Target.Status = Source.Status,
            Target.ModifiedBy = Source.ModifiedBy,
            Target.ModifiedOn = GETDATE(),
            Target.Active = Source.Active

    WHEN NOT MATCHED THEN
        INSERT
        (
            FormDate,
            MachineId,
            MachineIp,
            CompId,
            EMP_Info_Id,
            MS_PerformanceCycles_Id,
            MS_GoalCategories_CategoryId,
            GoalTitle,
            GoalDescription,
            Weightage,
            TargetValue,
            AchievementCriteria,
            Status,
            CreatedBy,
            CreatedOn,
            ModifiedBy,
            ModifiedOn,
            Active
        )
        VALUES
        (
            GETDATE(),
            Source.MachineId,
            Source.MachineIp,
            Source.CompId,
            Source.EMP_Info_Id,
            Source.MS_PerformanceCycles_Id,
            Source.MS_GoalCategories_CategoryId,
            Source.GoalTitle,
            Source.GoalDescription,
            Source.Weightage,
            Source.TargetValue,
            Source.AchievementCriteria,
            Source.Status,
            Source.CreatedBy,
            GETDATE(),
            Source.ModifiedBy,
            GETDATE(),
            CAST(1 AS BIT)
        );
