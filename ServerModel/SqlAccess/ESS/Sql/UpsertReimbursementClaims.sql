
/*
--UpsertReimbursementClaims.Sql

--select * from Reim_Claims

DECLARE @Id uniqueidentifier = 'A07A840F-B53B-472B-910F-558C5958E9A6';
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @EMP_Info_Id uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @MS_Reim_Types_Id INT = 1;
DECLARE @ClaimDate DATETIME = GETDATE();
DECLARE @GSTRegNos NVARCHAR(100) = 'abc';
DECLARE @CGST DECIMAL(18, 4) = 1000;
DECLARE @SGST DECIMAL(18, 4) = 1000;
DECLARE @Amount DECIMAL(18, 4) = 1000;
DECLARE @Description nvarchar(MAX) = 'name23';
DECLARE @BillPath nvarchar(MAX) = 'name23'; 
DECLARE @Status nvarchar(50) = 'Monthly';
DECLARE @SubmittedDate DATETIME = GETDATE();
DECLARE @ApprovedDate DATETIME = NULL;
DECLARE @PaidDate DATETIME = NULL;
DECLARE @Active BIT = 0;

*/

MERGE INTO Reim_Claims as Target
USING (select @Id as Id) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 EMP_Info_Id = @EMP_Info_Id
		,MS_Reim_Types_Id = @MS_Reim_Types_Id
		,ClaimDate = @ClaimDate
		,GSTRegNos = @GSTRegNos
		,CGST = @CGST
		,SGST = @SGST
		,Amount = @Amount
		,Description = @Description
		,BillPath = @BillPath
		,Status = @Status
		,SubmittedDate = @SubmittedDate
		,ApprovedDate = @ApprovedDate
		,PaidDate = @PaidDate
		,Active = @Active


WHEN NOT MATCHED THEN
        INSERT (
			 Id
			,FormDate
			,CompId
            ,EMP_Info_Id
			,MS_Reim_Types_Id
			,ClaimDate
			,GSTRegNos
			,CGST
			,SGST
			,Amount
			,Description
			,BillPath
			,Status
			,SubmittedDate
			,ApprovedDate
			,PaidDate
			,Active)
        VALUES
           (
		     NEWID()
		    ,GETDATE()
			,@CompId
            ,@EMP_Info_Id
			,@MS_Reim_Types_Id
			,@ClaimDate
			,@GSTRegNos
			,@CGST
			,@SGST
			,@Amount
			,ISNULL(@Description, NULL)
			,ISNULL(@BillPath, NULL)
			,@Status
			,GETDATE()
			,ISNULL(@ApprovedDate, NULL)
			,ISNULL(@PaidDate, NULL)
			,CAST(1 AS BIT))

OUTPUT INSERTED.Id;


