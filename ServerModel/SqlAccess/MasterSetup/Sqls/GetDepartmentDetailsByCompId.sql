

/*

---GetDepatmentDetailsByCompId

DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';

*/


SELECT dept.Id
      ,dept.MachineId
      ,dept.MachineIp
      ,dept.CompId
      ,dept.DepartmentName
      ,dept.DepartmentCode
      ,dept.DepartmentShortName
      ,dept.MailAlias
      ,dept.DepartmentLead_Id
	  ,emp.FullName AS DepartmentLeadName
      ,dept.ParentDepartment_Id
	  ,parentDept.DepartmentName AS ParentDepartmentName
      ,dept.CreatedOn
      ,dept.CreatedBy
      ,dept.ModifiedOn
      ,dept.ModifiedBy
      ,dept.Active
  FROM MS_Dept AS dept
	LEFT JOIN EMP_Info AS emp on dept.DepartmentLead_Id = emp.Id
	LEFT JOIN MS_Dept AS parentDept on dept.ParentDepartment_Id = parentDept.Id
  WHERE dept.CompId = @CompId




