

----UpsertEmployeeAssetDetails.Sql

--select * from EMP_Asset
--select * from EMP_AssetDetail

--truncate table EMP_Asset
--truncate table EMP_AssetDetail


--DECLARE @json1 NVARCHAR(MAX) = 
--'[{
--    "Id": "00000000-0000-0000-0000-000000000000",
--    "FormDate": "2023-01-25 23:33:32.017",
--    "CompId": "00000000-0000-0000-0000-000000000000",
--    "EMP_Info_Id": "566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE",
--    "CreatedBy": "00000000-0000-0000-0000-000000000000",
--    "Active": 1
--}]';

--DECLARE @json2 NVARCHAR(MAX) = 
--'[{
--    "Id": 1,
--    "EMP_Asset_Id": "566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE",
--    "MS_Asset_Id": 1,
--    "Active": 1,
--    "Description": "Laptop"
--}]';

BEGIN TRY
    BEGIN TRANSACTION;

    -- Table to capture inserted Asset Ids
    DECLARE @OutputTable TABLE (
        Id UNIQUEIDENTIFIER,
        EMP_Info_Id UNIQUEIDENTIFIER
    );

    -- Upsert into EMP_Asset
    MERGE EMP_Asset AS target
    USING (
        SELECT 
            Id,
            FormDate,
            CompId,
            EMP_Info_Id,
            CreatedBy,
            Active
        FROM OPENJSON(@json1)
        WITH (
            Id UNIQUEIDENTIFIER,
            FormDate DATETIME,
            CompId UNIQUEIDENTIFIER,
            EMP_Info_Id UNIQUEIDENTIFIER,
            CreatedBy UNIQUEIDENTIFIER,
            Active BIT
        )
    ) AS source
    ON target.EMP_Info_Id = source.EMP_Info_Id
    WHEN MATCHED THEN
        UPDATE SET
            target.Active = source.Active
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (Id, FormDate, CompId, EMP_Info_Id, CreatedBy, Active)
        VALUES (NEWID(), GETDATE(), source.CompId, source.EMP_Info_Id, source.CreatedBy, source.Active)
    OUTPUT inserted.Id, source.EMP_Info_Id INTO @OutputTable;

    -- Upsert into EMP_AssetDetail
    MERGE EMP_AssetDetail AS target
    USING (
        SELECT 
            Id,
            EMP_Asset_Id,
            MS_Asset_Id,
            [Description],
            Active
        FROM OPENJSON(@json2)
        WITH (
            Id INT,
            EMP_Asset_Id UNIQUEIDENTIFIER,
            MS_Asset_Id INT,
            [Description] NVARCHAR(MAX),
            Active BIT
        )
    ) AS source
    ON target.Id = source.Id
    WHEN MATCHED THEN
        UPDATE SET
            target.EMP_Asset_Id = source.EMP_Asset_Id,
            target.MS_Asset_Id = source.MS_Asset_Id,
            target.[Description] = source.[Description],
            target.Active = source.Active
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (EMP_Asset_Id, MS_Asset_Id, [Description], Active)
        VALUES ((SELECT Id FROM @OutputTable), source.MS_Asset_Id, source.[Description], source.Active);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();

    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;
