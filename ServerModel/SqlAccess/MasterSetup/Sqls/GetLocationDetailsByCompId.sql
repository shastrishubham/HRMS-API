
/*
--GetLocationDetails.Sql

DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680'

*/

SELECT Id
      ,MachineId
      ,MachineIp
      ,CompId
      ,BranchName
      ,MailAlias
      ,TimeZone_Id
      ,BranchCode
      ,BranchAddress
      ,CountryId
      ,StateId
      ,CityId
      ,PostalCode
      ,IsMainBranch
      ,BranchHead_Id
      ,CreatedOn
      ,CreatedBy
      ,ModifiedOn
      ,ModifiedBy
      ,Active
  FROM MS_Branch
	WHERE CompId = @CompId



