
--select * from PMS_ManagerEvaluations
--select * from PMS_ManagerEvaluationDetails

--DECLARE @Json NVARCHAR(MAX) = N'
--[
--  {
--    "Id": 1,
--    "FormDate": "2026-05-29",
--    "MachineId": "PC001",
--    "MachineIp": "192.168.1.1",
--    "CompId": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--    "PMS_ManagerEvaluations_Id": 1,
--    "PMS_EmployeeGoals_GoalId": 5,
--    "ManagerRating": 4,
--    "ManagerComments": "Excellent",
--    "Active": true,
--    "CreatedBy": "7EC2A3C8-554F-4F38-97A6-A5E89D431D29",
--    "ModifiedBy": "7EC2A3C8-554F-4F38-97A6-A5E89D431D29"
--  },
--  {
--    "Id": 0,
--    "FormDate": "2026-05-29",
--    "MachineId": "PC002",
--    "MachineIp": "192.168.1.2",
--    "CompId": "D33691FC-7EB9-4D1C-888C-4F4B6E204680",
--    "PMS_ManagerEvaluations_Id": 1,
--    "PMS_EmployeeGoals_GoalId": 6,
--    "ManagerRating": 3,
--    "ManagerComments": "Good",
--    "Active": true,
--    "CreatedBy": "7EC2A3C8-554F-4F38-97A6-A5E89D431D29",
--    "ModifiedBy": "7EC2A3C8-554F-4F38-97A6-A5E89D431D29"
--  }
--]
--'

    SET NOCOUNT ON;

    -- Parse JSON into temp table
    DECLARE @Temp TABLE
    (
        Id INT,
        FormDate DATETIME2,
        MachineId NVARCHAR(50),
        MachineIp NVARCHAR(50),
        CompId UNIQUEIDENTIFIER,
        PMS_ManagerEvaluations_Id INT,
        PMS_EmployeeGoals_GoalId INT,
        ManagerRating DECIMAL(3,2),
        ManagerComments NVARCHAR(MAX),
        Active BIT,
        CreatedBy UNIQUEIDENTIFIER,
		CreatedOn Datetime,
        ModifiedBy UNIQUEIDENTIFIER,
		ModifiedOn Datetime
    );

    INSERT INTO @Temp
    (
        Id,
        FormDate,
        MachineId,
        MachineIp,
        CompId,
        PMS_ManagerEvaluations_Id,
        PMS_EmployeeGoals_GoalId,
        ManagerRating,
        ManagerComments,
        Active,
        CreatedBy,
		CreatedOn,
        ModifiedBy,
		ModifiedOn
    )
    SELECT
        Id,
        GETDATE(),
        MachineId,
        MachineIp,
        CompId,
        PMS_ManagerEvaluations_Id,
        PMS_EmployeeGoals_GoalId,
        ManagerRating,
        ManagerComments,
        Active,
        CreatedBy,
		GETDATE(),
        ModifiedBy,
		GETDATE()
    FROM OPENJSON(@Json)
    WITH
    (
        Id INT,
        FormDate DATETIME,
        MachineId NVARCHAR(50),
        MachineIp NVARCHAR(50),
        CompId UNIQUEIDENTIFIER,
        PMS_ManagerEvaluations_Id INT,
        PMS_EmployeeGoals_GoalId INT,
        ManagerRating DECIMAL(3,2),
        ManagerComments NVARCHAR(MAX),
        Active BIT,
        CreatedBy UNIQUEIDENTIFIER,
		CreatedOn DATETIME,
        ModifiedBy UNIQUEIDENTIFIER,
		ModifiedOn DATETIME
    );

    -- UPSERT
    MERGE dbo.PMS_ManagerEvaluationDetails AS TARGET
    USING @Temp AS SOURCE
    ON TARGET.Id = SOURCE.Id

    WHEN MATCHED THEN
        UPDATE SET
            TARGET.PMS_ManagerEvaluations_Id =  SOURCE.PMS_ManagerEvaluations_Id,
            TARGET.PMS_EmployeeGoals_GoalId = SOURCE.PMS_EmployeeGoals_GoalId,
            TARGET.ManagerRating = SOURCE.ManagerRating,
            TARGET.ManagerComments = SOURCE.ManagerComments,
            TARGET.Active = SOURCE.Active,
            TARGET.ModifiedBy = SOURCE.ModifiedBy,
            TARGET.ModifiedOn = GETDATE()

    WHEN NOT MATCHED THEN
        INSERT
        (
            FormDate,
            MachineId,
            MachineIp,
            CompId,
            PMS_ManagerEvaluations_Id,
            PMS_EmployeeGoals_GoalId,
            ManagerRating,
            ManagerComments,
            Active,
            CreatedBy,
            CreatedOn,
			ModifiedBy,
			ModifiedOn
        )
        VALUES
        (
            GETDATE(),
            SOURCE.MachineId,
            SOURCE.MachineIp,
            SOURCE.CompId,
            SOURCE.PMS_ManagerEvaluations_Id,
            SOURCE.PMS_EmployeeGoals_GoalId,
            SOURCE.ManagerRating,
            SOURCE.ManagerComments,
            CAST(1 AS BIT),
            SOURCE.CreatedBy,
            GETDATE(),
			SOURCE.ModifiedBy,
            GETDATE()
        );
