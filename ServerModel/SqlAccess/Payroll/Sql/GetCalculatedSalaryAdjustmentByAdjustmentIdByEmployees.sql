

----GetCalculatedSalaryAdjustmentByAdjustmentIdByEmployees.Sql

--DECLARE    @EmpIds NVARCHAR(MAX) = 'C7716840-5EEE-4EC7-8E10-2AAF274252AD,5DE2FE64-4D6B-4E75-BA75-B43A143BC4BD';
--DECLARE    @AdjustmentTypeId INT = 1;

BEGIN
    SET NOCOUNT ON;

    -- Split EmpIds string into table
    ;WITH EmpList AS (
        SELECT TRIM(value) AS EMP_Info_Id 
        FROM STRING_SPLIT(@EmpIds, ',')
    )
    SELECT 
        e.EMP_Info_Id,
        emp.FullName,
        adj.AdjustmentType,
        adj.IsRuleBased,
        adj.PercentageAmt,
        sl.SalaryHeadName,

        CASE 
            WHEN adj.IsRuleBased = 1 THEN 
                ISNULL(h.Amount, 0)
            ELSE 
                ISNULL(sladj.Amount, 0)
        END AS SalaryHeadAmount,

        CASE 
            WHEN adj.IsRuleBased = 1 THEN 
                ROUND((adj.PercentageAmt / 100.0) * h.Amount / 12, 2)
            ELSE 
                ISNULL(sladj.Amount, 0)
        END AS CalculatedAdjustmentAmount
		,sladj.Status

    FROM EmpList e
        INNER JOIN EMP_Info emp 
            ON emp.Id = e.EMP_Info_Id
        INNER JOIN MS_Payroll_AdjustType adj 
            ON adj.Id = @AdjustmentTypeId
            AND adj.CompId = emp.CompId
        LEFT JOIN MS_SLHeads sl 
            ON adj.MS_SLHeads_Id = sl.Id
        LEFT JOIN EMP_SLHeads h 
            ON h.EMP_Info_Id = e.EMP_Info_Id
            AND h.MS_SLHeads_Id = adj.MS_SLHeads_Id
            AND h.IsAmmend = 0
        LEFT JOIN PR_EMP_SL_Adjustment sladj
            ON sladj.EMP_Info_Id = e.EMP_Info_Id
            AND sladj.MS_Payroll_AdjstType_Id = adj.Id
            --AND sladj.Status = 'Pending'  -- optional filter

    WHERE adj.Id = @AdjustmentTypeId;
END

