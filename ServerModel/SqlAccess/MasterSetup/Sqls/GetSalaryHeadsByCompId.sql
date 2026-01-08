
/*
---GetSalaryHeadsByCompId.Sql

DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';

*/

SELECT sl1.Id
      ,sl1.CompId
      ,sl1.SalaryHeadName
      ,sl1.ShortSalaryHeadName
      ,sl1.IsEarningComponent
      ,sl1.IsFixedComponent
	  ,sl1.IsTaxableComponent
	  ,sl1.IsShowInSalarySlip
	  ,sl1.SalaryHeadOrder
	  ,sl1.MS_SLHeads_Id
	  ,sl2.SalaryHeadName AS 'HeadOf'
	  ,sl1.[Percentage]
      ,sl1.Active
  FROM MS_SLHeads AS sl1
	LEFT JOIN MS_SLHeads AS sl2 ON sl1.MS_SLHeads_Id = sl2.Id
  WHERE sl1.CompId = @CompId
  ORDER BY sl1.SalaryHeadOrder ASC