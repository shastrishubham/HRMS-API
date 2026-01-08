

--Sql.GetBankBranchesByCompId
/*
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'

*/

SELECT BB.Id
      ,BB.FormDate
      ,BB.CompId
      ,BB.MS_Bank_Id
	  ,B.BankName
      ,BB.BranchName
      ,BB.BranchAddress
      ,BB.BrachCode
      ,BB.IFSC
      ,BB.MICR
      ,BB.BranchContact
  FROM MS_Bank_Branch as BB
	INNER JOIN MS_Bank AS b ON b.Id = BB.MS_Bank_Id
  WHERE BB.CompId = @compId