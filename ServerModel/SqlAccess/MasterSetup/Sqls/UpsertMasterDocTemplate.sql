



--UpsertMasterDocTemplate.Sql

--select * from MS_DocList
/*
DECLARE @Id int = 1;
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @DocName nvarchar(MAX) = 'name23';
DECLARE @DocPath nvarchar(MAX) = 'name23';

**/

MERGE INTO MS_DocList as Target
USING (select @Id as Id, @DocName as DocName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 DocName = @DocName 
		,DocPath = @DocPath

WHEN NOT MATCHED THEN
        INSERT (
			 MachineId
			,MachineIp
            ,CompId
			,DocName
			,DocPath)
        VALUES
           (
		    'AJINKYA-PC'
		   ,'1.1.1.1'
		   ,@CompId
		   ,@DocName
		   ,@DocPath)

OUTPUT INSERTED.Id;


