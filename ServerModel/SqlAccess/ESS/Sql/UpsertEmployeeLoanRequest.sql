

--UpsetLoanTypes.Sql

/*
--select * from EMP_LNReq

DECLARE @Id uniqueidentifier = 'F155BDB3-1DCD-448C-9972-44D1F4623421';
DECLARE @FormDate DATETIME2 = GETDATE();
DECLARE @MachineId NVARCHAR(50) = '';
DECLARE @MachineIp NVARCHAR(50) = '';
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @EMP_Info_Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @MS_LN_Id INT = 1;
DECLARE @ReqAmt DECIMAL(18, 4) = 100;
DECLARE @ApprAmt DECIMAL(18, 4) = 0;
DECLARE @TenureMonths DECIMAL(18, 0) = 1;
DECLARE @Status NVARCHAR(100) = 'Pending';
DECLARE @Remark NVARCHAR(MAX) = 'Remark111';
DECLARE @Ammend BIT = 0;
DECLARE @Active BIT = 1;

*/

IF(@Id = '00000000-0000-0000-0000-000000000000')
BEGIN
		
		SET @Id = NEWID();
		
		INSERT INTO EMP_LNReq([Id], [FormDate], [MachineId], [MachineIp], [CompId], 
			[EMP_Info_Id], [MS_LN_Id], [ReqAmt], [ApprAmt], [TenureMonths], [Status],
			[Remark], [Ammend], [Active])
		VALUES (@Id, GETDATE(), @MachineId, @MachineIp, @CompId, @EMP_Info_Id,
			@MS_LN_Id, @ReqAmt, @ApprAmt, @TenureMonths, @Status, @Remark, 0, 1)
END
ELSE
BEGIN
	
		UPDATE EMP_LNReq SET Ammend = 1 WHERE Id = @Id

		SET @Id = NEWID();

		INSERT INTO EMP_LNReq([Id], [FormDate], [MachineId], [MachineIp], [CompId], 
			[EMP_Info_Id], [MS_LN_Id], [ReqAmt], [ApprAmt], [TenureMonths], [Status],
			[Remark], [Ammend], [Active])
		VALUES (@Id, GETDATE(), @MachineId, @MachineIp, @CompId, @EMP_Info_Id,
			@MS_LN_Id, @ReqAmt, @ApprAmt, @TenureMonths, @Status, @Remark, 0, 1)

END

SELECT @Id;