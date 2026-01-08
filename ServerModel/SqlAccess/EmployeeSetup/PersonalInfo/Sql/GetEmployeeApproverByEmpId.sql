
/*

DECLARE @employeeId UNIQUEIDENTIFIER = 'FAC57D97-52DC-4325-9A30-5671CD8FA131'

*/


SELECT depHead.Id, depHead.FullName
	FROM EMP_Info AS emp
		LEFT JOIN MS_Dept AS dept ON dept.Id = emp.MS_Dept_Id
		LEFT JOIN EMP_Info AS depHead ON depHead.Id = dept.DepartmentLead_Id
	WHERE emp.Id = @employeeId
		AND emp.IsActive = 1