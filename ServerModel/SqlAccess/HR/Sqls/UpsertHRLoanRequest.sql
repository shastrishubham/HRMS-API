

--UpsertHRLoanRequest.Sql

/*
--select * from HR_LNReq

DECLARE @Id uniqueidentifier = 'A07A840F-B53B-472B-910F-558C5958E9A6';
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @EMP_LNReq_Id uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @ApproverId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @Status NVARCHAR(50) = 'Approved'
DECLARE @Remark nvarchar(MAX) = 'remark';

*/

MERGE INTO HR_LNReq as Target
USING (select @Id as Id) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 EMP_LNReq_Id = @EMP_LNReq_Id
		,ApproverId = @ApproverId
		,Status = @Status
		,Remark = @Remark

WHEN NOT MATCHED THEN
        INSERT (
			 Id
			,FormDate
			,MachineId
			,MachineIp
			,CompId
            ,EMP_LNReq_Id
			,ApproverId
			,Status
			,Remark)
        VALUES
           (
		     NEWID()
		    ,GETDATE()
			,'MachineId'
			,'MachineIp'
			,@CompId
            ,@EMP_LNReq_Id
			,@ApproverId
			,@Status
			,@Remark)

OUTPUT INSERTED.Id;

-------------- check if others approvers are pending if not then mark the empLoanRequest status = Hrleave req

IF NOT EXISTS (SELECT 1 FROM HR_LNReq WHERE EMP_LNReq_Id = @EMP_LNReq_Id AND Status = 'Pending')
BEGIN
	UPDATE EMP_LNReq SET Status = @Status WHERE Id = @EMP_LNReq_Id AND Ammend = 0
END

