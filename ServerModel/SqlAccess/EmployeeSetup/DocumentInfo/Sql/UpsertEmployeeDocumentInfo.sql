

/*
--UpsertEmployeeDocumentInfo.Sql

--select * from EMP_Docs

DECLARE @Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @CreatedBy uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @EMP_Info_Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @DocsKey NVARCHAR(100) = 'DocName';
DECLARE @DocsValue NVARCHAR(MAX) = 'DocPath';

*/

MERGE INTO EMP_Docs as Target
USING (select @Id as Id) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		 EMP_Info_Id = @EMP_Info_Id
		,DocsKey = @DocsKey
		,DocsValue = @DocsValue

WHEN NOT MATCHED THEN
        INSERT (
             Id
			,FormDate
			,CompId
			,CreatedBy
			,EMP_Info_Id
			,DocsKey
			,DocsValue)
        VALUES
           (
            NEWID()
           ,GETDATE()
           ,@CompId
		   ,@CreatedBy
		   ,@EMP_Info_Id
		   ,@DocsKey
		   ,@DocsValue)

OUTPUT INSERTED.Id;