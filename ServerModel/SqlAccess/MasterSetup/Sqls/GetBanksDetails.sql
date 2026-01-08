


/*
---GetBanksDetails

DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
*/

SELECT Id
      ,CompId
      ,BankName
      ,Active
  FROM MS_Bank
  WHERE CompId = @CompId