
/*
--UpsertBank.Sql

--select * from MS_Bank

DECLARE @Id int = 1;
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @BankName nvarchar(255) = 'name23';
DECLARE @Active bit = 1;



*/

MERGE INTO MS_Bank as Target
USING (select @Id as Id, @BankName as BankName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 CompId = @CompId
		,BankName = @BankName
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,BankName
			,Active)
        VALUES
           (
            @CompId
		   ,@BankName
		   ,@Active)

OUTPUT INSERTED.Id;


