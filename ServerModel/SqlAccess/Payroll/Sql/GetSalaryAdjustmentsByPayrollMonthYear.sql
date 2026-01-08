

----GetSalaryAdjustmentsByPayrollMonthYear.Sql

--select * from PR_EMP_SL_Adjustment

/*
DECLARE @date DATE = '2025-09-29';
*/

SELECT 
			 sladj.Id
			,sladj.FormDate
			,sladj.CompId
			,sladj.EMP_Info_Id
			,emp.FullName
			,sladj.PayrollMonthYear
			,sladj.Amount
			,sladj.MS_Payroll_AdjstType_Id
			,adj.AdjustmentType
			,adj.IsRuleBased
			,CASE 
				WHEN adj.IsRuleBased = 1 THEN 
				    CAST(adj.PercentageAmt AS VARCHAR(10)) + ' % Of ' + head.SalaryHeadName
				ELSE 
				    ''
				END AS 'Rule'
			,sladj.Description
			,sladj.Status
		FROM PR_EMP_SL_Adjustment AS sladj
			INNER JOIN EMP_Info AS emp ON emp.Id = sladj.EMP_Info_Id
				AND emp.IsActive = 1
			INNER JOIN MS_Payroll_AdjustType AS adj ON adj.Id = sladj.MS_Payroll_AdjstType_Id
			LEFT JOIN MS_SLHeads AS head ON adj.MS_SLHeads_Id = head.Id
		WHERE YEAR(sladj.PayrollMonthYear) = YEAR(@date)
				AND MONTH(sladj.PayrollMonthYear) = MONTH(@date)