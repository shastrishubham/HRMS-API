
----- UpsertPayrollCreation.Sql

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
		"EMP_Info_Id": "566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE",
		"EMP_Info_MSBranchId": 2002,
		"TotalPaidDays": 15,
		"TotalEarning": 7750.0000000000,
		"TotalDeduction": 1966.6666666666,
		"NetPay": 1783.333333,
		"PayrollStatus": "Processed",
		"Comment": ""
	},
	{
		"Id": 0,
		"FormDate": "2025-04-13 23:33:32.017",
		"MachineId": "",
		"MachineIp": "",
		"CompId": "00000000-0000-0000-0000-000000000000",
		"CreatedBy": "00000000-0000-0000-0000-000000000000",
		"PayrollCreationDt": "2025-01-25 23:33:32.017",
		"EMP_Info_Id": "473D7AB4-766E-4011-B4C2-8F8E8AF6EB6F",
		"EMP_Info_MSBranchId": 3,
		"TotalPaidDays": 1,
		"TotalEarning": 8.4166666666,
		"TotalDeduction": 0.0000,
		"NetPay": 0.271505,
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
    UPDATE PR_CRT
    SET IsAmmend = 1,
        AmmendDate = GETDATE()
    WHERE EXISTS (
        SELECT 1
        FROM OPENJSON(@json1)
        WITH (
            PayrollCreationDt DATETIME,
            EMP_Info_Id UNIQUEIDENTIFIER
        ) j
        WHERE PR_CRT.EMP_Info_Id = j.EMP_Info_Id
          AND MONTH(PR_CRT.PayrollCreationDt) = MONTH(j.PayrollCreationDt)
          AND YEAR(PR_CRT.PayrollCreationDt) = YEAR(j.PayrollCreationDt)
    );

    -- 2. Insert fresh payroll records and capture IDs
    INSERT INTO PR_CRT (
       FormDate, MachineIp, MachineId, CompId, CreatedBy, PayrollCreationDt,
            EMP_Info_Id, EMP_Info_MSBranchId, [TotalMonthDays], [PunchDays], [PaidLeaveDays],[UnpaidLeaveDays], [HolidayDays],TotalPaidDays, TotalEarning,TotalDeduction, 
			[ShiftAllowanceDays],[TotalShiftAllowance],[OTAllowanceHrs],[TotalOTAllowance],
			Netpay, PayrollStatus, Comment, [IsAmmend], [AmmendDate], [Active]
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
        j.EMP_Info_Id,
        j.EMP_Info_MSBranchId,
		j.TotalMonthDays,
		j.PunchDays,
		j.PaidLeaveDays,
		j.UnpaidLeaveDays,
		j.HolidayDays,
        j.TotalPaidDays,
        j.TotalEarning,
        j.TotalDeduction,
		j.ShiftAllowanceDays,
		j.TotalShiftAllowance,
		j.OTAllowanceHrs,
		j.TotalOTAllowance,
        j.NetPay,
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
        EMP_Info_Id UNIQUEIDENTIFIER,
        EMP_Info_MSBranchId INT,
		TotalMonthDays INT,
		PunchDays INT,
		PaidLeaveDays DECIMAL(18,4),
		UnpaidLeaveDays DECIMAL(18,4),
		HolidayDays DECIMAL(18,4),
        TotalPaidDays DECIMAL(18,10),
        TotalEarning DECIMAL(18,10),
        TotalDeduction DECIMAL(18,10),
		ShiftAllowanceDays DECIMAL(18,4),
		TotalShiftAllowance DECIMAL(18,4),
		OTAllowanceHrs DECIMAL(18,4),
		TotalOTAllowance DECIMAL(18,4),
        NetPay DECIMAL(18,10),
        PayrollStatus NVARCHAR(50),
        Comment NVARCHAR(MAX)
    ) j;

    -- 3. Delete PR_EMP_SL entries for updated months
    DELETE FROM PR_EMP_SL
    WHERE EXISTS (
        SELECT 1
        FROM @OutputTable ot
        WHERE PR_EMP_SL.EMP_Info_Id = ot.EMP_Info_Id
          AND MONTH(PR_EMP_SL.PayrollCreationDt) = MONTH(ot.PayrollCreationDt)
          AND YEAR(PR_EMP_SL.PayrollCreationDt) = YEAR(ot.PayrollCreationDt)
    );

    -- 4. Insert new PR_EMP_SL records from EMP_SLHeads (for all newly inserted PR_CRT rows)
    INSERT INTO PR_EMP_SL (
        FormDate, CompId, CreatedBy, PR_CRT_Id, EMP_Info_Id, PayrollCreationDt,
        MS_SLHeads_Id, Amount
    )
    SELECT
        GETDATE(),
        sl.CompId,
        ot.CreatedBy,
        ot.Id,
        ot.EMP_Info_Id,
        ot.PayrollCreationDt,
        sl.MS_SLHeads_Id,
        sl.Amount / 12
    FROM @OutputTable ot
    INNER JOIN EMP_SLHeads sl ON sl.EMP_Info_Id = ot.EMP_Info_Id
    WHERE sl.Active = 1
		AND sl.IsAmmend = 0;

	--5. Update PaidAmount and Status in LN_RepPaySch after payroll creation
	UPDATE rps
	SET 
	    rps.PaidAmount = rps.EMIAmt,      -- Mark full EMI as paid
	    rps.Status = 'Processed'
	FROM LN_RepPaySch rps
	INNER JOIN EMP_LNReq empLN
	    ON empLN.Id = rps.EMP_LNReq_Id
	INNER JOIN @OutputTable ot
	    ON ot.EMP_Info_Id = empLN.EMP_Info_Id
	WHERE rps.Status = 'Pending'
	  AND rps.DueDate BETWEEN DATEFROMPARTS(YEAR(ot.PayrollCreationDt), MONTH(ot.PayrollCreationDt), 1)
	                      AND EOMONTH(ot.PayrollCreationDt)
	  AND empLN.Status = 'Disbursed';

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