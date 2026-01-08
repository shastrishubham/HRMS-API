
/*
--UpsertCompanyDocument.Sql

--select * from MS_CompDocs

DECLARE @Id int = 0;
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @DocumentName nvarchar(MAX) = 'name23';
DECLARE @DocumentPath nvarchar(MAX) = 'name23';
DECLARE @Active bit = 1;

*/

MERGE INTO MS_CompDocs as Target
USING (select @Id as Id, @DocumentName as DocumentName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 DocumentName = @DocumentName
		,DocumentPath = @DocumentPath
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
			 FormDate
            ,CompId
			,DocumentName
			,DocumentPath
			,Active)
        VALUES
           (
		    GETDATE()
           ,@CompId
		   ,@DocumentName
		   ,@DocumentPath
		   ,ISNULL(@Active, CAST(1 AS BIT)))

OUTPUT INSERTED.Id;


