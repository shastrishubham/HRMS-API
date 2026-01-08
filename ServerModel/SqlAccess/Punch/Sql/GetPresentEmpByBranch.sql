
--GetPresentEmpByBranch

--DECLARE @attendaceDate DATE = GETDATE()
--DECLARE @branchId INT = 1

SELECT 
		 attd.EMP_Info_Id
		,emp.FullName
		,desg.DesignationName
		,branch.BranchName
		,shiftMS.ShiftName
		,attd.Date AS AttendanceDate
		,MIN(attd.Intime) AS PunchIn
		,MAX(attd.Outtime) AS PunchOut 
	FROM EMP_Punch AS attd 
		INNER JOIN EMP_Info AS emp ON emp.Id = attd.EMP_Info_Id
		INNER JOIN MS_Designation AS desg ON desg.Id = emp.MS_Designation_Id
		INNER JOIN MS_Branch AS branch ON branch.Id = emp.MS_Branch_Id
		INNER JOIN EMP_Shift AS eShift ON eShift.EMP_Info_Id = attd.EMP_Info_Id AND eShift.IsAmmend = 0
		INNER JOIN MS_Shift as shiftMS ON eShift.MS_Shift_Id = shiftMS.Id
	WHERE attd.PunchDate = @attendaceDate
		AND emp.MS_Branch_Id = @branchId
		GROUP BY attd.EMP_Info_Id, emp.FullName,desg.DesignationName, branch.BranchName,shiftMS.ShiftName,attd.PunchDate 

