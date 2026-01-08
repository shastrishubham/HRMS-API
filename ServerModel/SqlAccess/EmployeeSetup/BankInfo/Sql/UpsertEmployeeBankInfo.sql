

---Sql.UpsertEmployeeBank

/*
DECLARE @id INT = 1
DECLARE @formdate DATE = '2023-04-15'
DECLARE @machineIp nvarchar(50) = '1.1.1.1'
DECLARE @machineId nvarchar(50) = 'Machine1'
DECLARE @compId uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @createdby uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @EMP_Info_Id uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @MS_Bank_Id INT = 0
DECLARE @MS_Bank_Branch_Id INT = 0
DECLARE @AccountNo NVARCHAR(50) = '11111111111111'
DECLARE @MS_AccountType_Id INT = 1
DECLARE @MS_PaymentType_Id INT = 1
DECLARE @DDPayableAt NVARCHAR(max) = 'qwqwqwqwqwqwq'
DECLARE @NameAsPerBank NVARCHAR(max) = 'qwqwqwqwqwqwq'
DECLARE @UAN NVARCHAR(50) = 'qwqwqwqwqwqwq'
DECLARE @PFNo NVARCHAR(50) = 'qwqwqwqwqwqwq'
DECLARE @IsCoveredESI bit = 1
DECLARE @ESINo NVARCHAR(50) = 'qwqwqwqwqwqwq'
DECLARE @IsCoveredLWF bit = 1
DECLARE @IsAmend BIT = 0

*/


DECLARE @OutputTable TABLE (ID int)
IF (@id = 0)
BEGIN
    SET @id = ISNULL((SELECT MAX(Id) FROM EMP_AcctInfo), 0) + 1
END

IF(@id <> 0)
BEGIN
	UPDATE EMP_AcctInfo SET IsAmend = CAST (1 AS BIT) WHERE Id = @id


   INSERT INTO EMP_AcctInfo
		(
			Id, FormDate, MachineId, MachineIp, CompId, CreatedBy, EMP_Info_Id, MS_Bank_Id, MS_Bank_Branch_Id, 
			AccountNo, MS_AccountType_Id, MS_PaymentType_Id, DDPayableAt, NameAsPerBank,
			UAN, PFNo, IsCoveredESI, ESINo, IsCoveredLWF, AmendId, IsAmend
		)
		OUTPUT INSERTED.Id INTO @OutputTable
		VALUES
		(
			@id, GETDATE(), @machineId, @machineIp, @compId, @createdby, @EMP_Info_Id, @MS_Bank_Id, @MS_Bank_Branch_Id,
			@AccountNo, @MS_AccountType_Id, @MS_PaymentType_Id, @DDPayableAt, @NameAsPerBank,
			@UAN, @PFNo, @IsCoveredESI, @ESINo, @IsCoveredLWF, @id, CAST(0 AS BIT)
		)

	END

-- Return the value of @id
SELECT @id AS Id;

