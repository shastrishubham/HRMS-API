

/*
---- Sql.GetEmployeeShiftDetailByBranchId
DECLARE @branchId INT = 5004
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'

*/

select	 
		 emp.Id AS EMP_Info_Id
		,emp.FullName AS EmployeeName
		,emp.MS_Branch_Id
		,branch.BranchName
		,emp.MS_Designation_Id
		,desg.DesignationName
		,emp.MS_Dept_Id
		,dept.DepartmentName
		,empShift.Id AS Id
		,sh.Id AS MS_Shift_Id
		,sh.ShiftName
		,empShift.StartFrom
		,empShift.EndTo
		,empShift.IsPermanentShift
	from EMP_Info AS emp
		LEFT JOIN EMP_Shift AS empShift ON empShift.EMP_Info_Id = emp.Id
			AND empShift.IsAmmend = 0
		LEFT JOIN MS_Shift AS sh ON sh.Id = empShift.MS_Shift_Id
		INNER JOIN MS_Branch AS branch ON branch.Id = emp.MS_Branch_Id
		INNER JOIN MS_Designation AS desg ON desg.Id = emp.MS_Designation_Id
		LEFT JOIN MS_Dept AS dept ON dept.Id = emp.MS_Dept_Id
	where emp.IsActive = 1
		AND emp.MS_Branch_Id = @branchId
		AND emp.CompId = @compId
	ORDER BY emp.FormDate, empShift.FormDate desc