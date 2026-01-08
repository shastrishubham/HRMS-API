
----- UpsertPayrollReimbursements.Sql

/*
DECLARE @json1 nvarchar(max) = 
'[
	{
		"Id": 0,
		"FormDate": "2023-01-25 23:33:32.017",
		"MachineId": "",
		"MachineIp": "",
		"CompId": "00000000-0000-0000-0000-000000000000",
		"CreatedBy": "00000000-0000-0000-0000-000000000000",
		"PayrollCreationDt": "2025-01-25 23:33:32.017",
		"Reim_Claims_Id": "00000000-0000-0000-0000-000000000000",
		"EMP_Info_Id": "566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE",
		"EMP_Info_MSBranchId": 2002,
		"Amount": 15,
		"CGST": 7750.0000000000,
		"SGST": 1966.6666666666,
		"NetPay": 1783.333333,
		"PayrollStatus": "Processed",
		"Comment": ""
	},
	{
		"Id": 2,
		"FormDate": "2023-01-25 23:33:32.017",
		"MachineId": "",
		"MachineIp": "",
		"CompId": "00000000-0000-0000-0000-000000000000",
		"CreatedBy": "00000000-0000-0000-0000-000000000000",
		"PayrollCreationDt": "2025-01-25 23:33:32.017",
		"Reim_Claims_Id": "00000000-0000-0000-0000-000000000000",
		"EMP_Info_Id": "566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE",
		"EMP_Info_MSBranchId": 2002,
		"Amount": 17,
		"CGST": 7750.0000000000,
		"SGST": 1966.6666666666,
		"NetPay": 1783.333333,
		"PayrollStatus": "Processed",
		"Comment": ""
	}
]';

*/

BEGIN TRY
    BEGIN TRANSACTION;

    -- Track inserted records
    DECLARE @OutputTable TABLE (
        Id INT,
        CreatedBy UNIQUEIDENTIFIER,
        PayrollCreationDt DATETIME,
        EMP_Info_Id UNIQUEIDENTIFIER
    );

    -- 1. Mark previous payroll records as amended
    UPDATE PR_Reimbursement
    SET IsAmmend = 1,
        AmmendDate = GETDATE()
    WHERE EXISTS (
        SELECT 1
        FROM OPENJSON(@json1)
        WITH (
            PayrollCreationDt DATETIME,
            EMP_Info_Id UNIQUEIDENTIFIER,
			Reim_Claims_Id UNIQUEIDENTIFIER
        ) j
        WHERE PR_Reimbursement.EMP_Info_Id = j.EMP_Info_Id
		  AND PR_Reimbursement.Reim_Claims_Id = j.Reim_Claims_Id
          AND MONTH(PR_Reimbursement.PayrollCreationDt) = MONTH(j.PayrollCreationDt)
          AND YEAR(PR_Reimbursement.PayrollCreationDt) = YEAR(j.PayrollCreationDt)
    );

    -- 2. Insert fresh payroll records and capture IDs
    INSERT INTO PR_Reimbursement (
       FormDate, MachineIp, MachineId, CompId, CreatedBy, PayrollCreationDt, Reim_Claims_Id,
            EMP_Info_Id, EMP_Info_MSBranchId, [Amount], [CGST], [SGST],[NetPay], PayrollStatus, Comment, [IsAmmend], [AmmendDate], [Active]
    )
    OUTPUT inserted.Id, inserted.CreatedBy, inserted.PayrollCreationDt, inserted.EMP_Info_Id
    INTO @OutputTable(Id, CreatedBy, PayrollCreationDt, EMP_Info_Id)
    SELECT
        GETDATE(),
        j.MachineId,
        j.MachineIp,
        j.CompId,
        j.CreatedBy,
        j.PayrollCreationDt,
		j.Reim_Claims_Id,
        j.EMP_Info_Id,
        j.EMP_Info_MSBranchId,
		j.Amount,
		j.CGST,
		j.SGST,
		j.Amount,
		j.PayrollStatus,
        j.Comment,
        0 AS IsAmmend,
        NULL AS AmmendDate,
        1 AS Active
    FROM OPENJSON(@json1)
    WITH (
        MachineId NVARCHAR(50),
        MachineIp NVARCHAR(50),
        CompId UNIQUEIDENTIFIER,
        CreatedBy UNIQUEIDENTIFIER,
        PayrollCreationDt DATETIME,
		Reim_Claims_Id UNIQUEIDENTIFIER,
        EMP_Info_Id UNIQUEIDENTIFIER,
        EMP_Info_MSBranchId INT,
		Amount DECIMAL(18,4),
		CGST DECIMAL(18,4),
		SGST DECIMAL(18,4),
		Amount DECIMAL(18,4),
		PayrollStatus NVARCHAR(50),
        Comment NVARCHAR(MAX)
    ) j;

	UPDATE RC
	SET RC.PaidDate = J.PayrollCreationDt, Status = 'Paid'
	FROM Reim_Claims RC
	JOIN OPENJSON(@json1)
	WITH (
	    Reim_Claims_Id UNIQUEIDENTIFIER,
	    PayrollCreationDt DATETIME
	) AS J ON RC.Id = J.Reim_Claims_Id;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();

    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;