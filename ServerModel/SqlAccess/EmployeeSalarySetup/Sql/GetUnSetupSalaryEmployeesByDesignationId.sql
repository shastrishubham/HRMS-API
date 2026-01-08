


----GetUnSetupSalaryEmployeesByDesignationId.Sql


--DECLARE @designationId INT = 1;
--DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'

select 
	empInfo.Id AS 'EmpId',
	empInfo.FullName
	from EMP_Info AS empInfo
		INNER JOIN MS_Designation AS desg ON desg.Id = empInfo.MS_Designation_Id
		LEFT JOIN EMP_SLSetup AS empSLSetup ON empSLSetup.EMP_Info_Id = empInfo.Id
			AND empInfo.IsActive = 1
	where empInfo.MS_Designation_Id = @designationId
		AND empInfo.CompId = @compId
		AND empSLSetup.Id IS NULL