
/*


---GetPayrollAdjustmentTypesByCompId.Sql

DECLARE @compId UNIQUEIDENTIFIER = 'C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4'

*/



SELECT
	 ad.Id
	,ad.CompId
	,ad.IsEarningHead
	,ad.AdjustmentType
	,ad.PercentageAmt
	,ad.IsRuleBased
	,ad.MS_SLHeads_Id
	,sl.SalaryHeadName
	FROM MS_Payroll_AdjustType AS ad
		LEFT JOIN MS_SLHeads AS sl ON sl.Id = ad.MS_SLHeads_Id
		WHERE ad.CompId = @compId