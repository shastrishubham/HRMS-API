
--select * from EMP_Info
--select * from MS_Designation

--select * from MS_Shift
/**
DECLARE @compId UNIQUEIDENTIFIER = 'C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4';
DECLARE @branchId INT = 6007;

*/

SELECT
	 empMain.Id
	,empMain.EmpId
	,empMain.EmpType
	,empMain.FullName
	,empMain.Gender
	,empMain.DOB
	,empMain.PANNo
	,empMain.AadharNo
	,empMain.MS_Designation_Id
	,desg.DesignationName
	,desg.DesignationCode
	,empMain.MS_Dept_Id
	,dept.DepartmentName
	,dept.DepartmentCode
	,empMain.MS_Branch_Id
	,brnch.BranchName
	,brnch.BranchCode
	,brnch.MailAlias
	,brnch.IsMainBranch
	,shft.ShiftName
	,shft.ShiftCode
	,shft.StartTime
	,shft.EndTime
	,shft.WeeklyOffDay
	FROM EMP_Info AS empMain
		INNER JOIN MS_Designation AS desg ON desg.Id = empMain.MS_Designation_Id
		INNER JOIN MS_Dept AS dept ON dept.Id = empMain.MS_Dept_Id
		INNER JOIN MS_Branch AS brnch ON brnch.Id = empMain.MS_Branch_Id
		INNER JOIN EMP_Shift AS empShift ON empShift.EMP_Info_Id = empMain.Id
			AND empShift.IsAmmend = 0
		INNER JOIN MS_Shift AS shft ON shft.Id = empShift.MS_Shift_Id
	WHERE empMain.CompId = @compId
		AND empMain.MS_Branch_Id = @branchId
