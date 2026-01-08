
/*
DECLARE @CompId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'

*/

SELECT ln.Id
      ,ln.CompId
      ,ln.LNTypeName
      ,ln.InterestRate
      ,ln.IsMaxAmtManual
      ,ln.MaxAmount
      ,ln.MS_SLHead_Id
	  ,sl.SalaryHeadName
      ,ln.Percentage
      ,ln.TenureMonths
      ,ln.AllowPartialPay
  FROM MS_LN AS ln
	LEFT JOIN MS_SLHeads AS sl ON ln.MS_SLHead_Id = sl.Id
  WHERE ln.CompId = @CompId