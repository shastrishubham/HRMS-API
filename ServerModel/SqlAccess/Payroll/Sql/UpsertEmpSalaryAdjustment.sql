
----- UpsertEmpSalaryAdjustment.Sql

-- MERGE query
MERGE PR_EMP_SL_Adjustment AS target
USING @Adjustments AS source
ON  MONTH(target.PayrollMonthYear) = MONTH(source.PayrollMonthYear)
    AND YEAR(target.PayrollMonthYear) = YEAR(source.PayrollMonthYear)
	AND target.EMP_Info_Id = source.EMP_Info_Id
	AND target.MS_Payroll_AdjstType_Id = source.MS_Payroll_AdjstType_Id
WHEN MATCHED THEN
    UPDATE SET 
        Amount = source.Amount,
        MS_Payroll_AdjstType_Id = source.MS_Payroll_AdjstType_Id,
        Description = source.Description,
        Status = source.Status  -- ✅ removed trailing comma
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, FormDate, CompId, CreatedBy, EMP_Info_Id, PayrollMonthYear, Amount, MS_Payroll_AdjstType_Id, Description, Status)
    VALUES (NEWID(), GETDATE(), source.CompId, source.CreatedBy, source.EMP_Info_Id, source.PayrollMonthYear, source.Amount, source.MS_Payroll_AdjstType_Id, source.Description, 'Pending');
