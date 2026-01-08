
---GetSalaryAdjustmentEmployeesByCompId.sql

/*
DECLARE @compId UNIQUEIDENTIFIER = 'C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4'
**/

SELECT 
	FORMAT(sladj.PayrollMonthYear, 'yyyy-MM') AS PayrollMonthYear   -- 👈 Month-Year only
	,sladj.Description
	--,sladj.Amount
	,sladj.MS_Payroll_AdjstType_Id
	,adj.AdjustmentType
	,adj.IsEarningHead
	,sladj.Status
	,SUM(sladj.Amount) AS TotalAmount  -- ✅ total per month
	FROM PR_EMP_SL_Adjustment AS sladj
		INNER JOIN EMP_Info AS emp ON emp.Id = sladj.EMP_Info_Id
		INNER JOIN MS_Payroll_AdjustType AS adj ON adj.Id = sladj.MS_Payroll_AdjstType_Id
	WHERE sladj.CompId = @compId
	GROUP BY FORMAT(sladj.PayrollMonthYear, 'yyyy-MM'), sladj.Description,sladj.MS_Payroll_AdjstType_Id,
		adj.IsEarningHead, sladj.Status, adj.AdjustmentType
	ORDER BY PayrollMonthYear DESC;