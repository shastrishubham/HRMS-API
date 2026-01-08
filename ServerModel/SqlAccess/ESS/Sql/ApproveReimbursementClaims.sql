


--ApproveReimbursementClaims.Sql

--select * from Reim_Appro_Claims

--DECLARE @Id uniqueidentifier = 'A07A840F-B53B-472B-910F-558C5958E9A6';
--DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
--DECLARE @Reim_Claims_Id uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
--DECLARE @Approver_Emp_Id UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
--DECLARE @Status nvarchar(50) = 'Monthly';
--DECLARE @MachineId nvarchar(50) = 'AJINKYA-PC';
--DECLARE @MachineIp nvarchar(50) = '1.1.1.1';
--DECLARE @Comment nvarchar(MAX) = 'name23'; 

MERGE INTO Reim_Appro_Claims as Target
USING (select @Id as Id) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 Reim_Claims_Id = @Reim_Claims_Id
		,Approver_Emp_Id = @Approver_Emp_Id
		,Status = @Status
		,Comment = @Comment

WHEN NOT MATCHED THEN
        INSERT (
			 Id
			,FormDate
			,MachineId
			,MachineIp
			,CompId
            ,Reim_Claims_Id
			,Approver_Emp_Id
			,Status
			,Comment
			)
        VALUES
           (
		     NEWID()
		    ,GETDATE()
			,@MachineId
			,@MachineIp
			,@CompId
            ,@Reim_Claims_Id
			,@Approver_Emp_Id
			,@Status
			,@Comment
			)

OUTPUT INSERTED.Id;


-- Now update the Reim_Claims table based on @Reim_Claims_Id
UPDATE Reim_Claims
SET Status = @Status,
    ApprovedDate = GETDATE()
WHERE Id = @Reim_Claims_Id;


