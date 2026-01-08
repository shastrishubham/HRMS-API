
/*


---GetDepatmentDetails

DECLARE @Id int = 2;
*/



SELECT Id
      ,MachineId
      ,MachineIp
      ,CompId
      ,DepartmentName
      ,DepartmentCode
      ,DepartmentShortName
      ,MailAlias
      ,DepartmentLead_Id
      ,ParentDepartment_Id
      ,CreatedOn
      ,CreatedBy
      ,ModifiedOn
      ,ModifiedBy
      ,Active
  FROM MS_Dept
  WHERE Id = @Id




