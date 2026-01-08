
/*
--GetLocationDetails.Sql

DECLARE @Id int = 2

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
	WHERE Id = @Id
