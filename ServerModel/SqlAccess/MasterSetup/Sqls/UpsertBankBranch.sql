
/*
--UpsertBank.Sql

--select * from MS_Bank_Branch

DECLARE @Id int = 1;
DECLARE @MS_Bank_Id int = 1;
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @BranchName nvarchar(max) = 'name23';
DECLARE @BranchAddress nvarchar(max) = 'address';
DECLARE @BrachCode nvarchar(50) = 'code';
DECLARE @IFSC nvarchar(50) = 'IFSC';
DECLARE @MICR nvarchar(50) = 'MICR'
DECLARE @BranchContact nvarchar(max) = 'address';



*/

MERGE INTO MS_Bank_Branch as Target
USING (select @Id as Id, @BranchName as BranchName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 CompId = @CompId
		,MS_Bank_Id = @MS_Bank_Id
		,BranchName = @BranchName
		,BranchAddress = @BranchAddress
		,BrachCode = @BrachCode
		,IFSC = @IFSC
		,MICR= @MICR
		,BranchContact = @BranchContact


WHEN NOT MATCHED THEN
        INSERT (
			FormDate
             ,CompId
			 ,MS_Bank_Id
			,BranchName
			,BranchAddress
			,BrachCode
			,IFSC
			,MICR
			,BranchContact)
        VALUES
           (
		   GETDATE()
            ,@CompId
		   ,@MS_Bank_Id
		   ,@BranchName
		   ,@BranchAddress
		   ,@BrachCode
		   ,@IFSC
		   ,@MICR
		   ,@BranchContact)

OUTPUT INSERTED.Id;


