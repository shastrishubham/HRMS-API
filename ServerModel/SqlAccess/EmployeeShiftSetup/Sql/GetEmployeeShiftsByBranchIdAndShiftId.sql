
--GetEmployeeShiftsByBranchIdAndShiftId

--DECLARE @branchId INT = 4002
--DECLARE @shiftId INT = 3004

select 
	empShift.Id,
	empShift.FormDate,
	empShift.CompId,
	empShift.EMP_Info_Id,
	emp.FullName AS EmployeeFullName,
	empShift.MS_Shift_Id,
	shiftMs.ShiftName,
	empShift.StartFrom,
	empShift.EndTo,
	empShift.IsPermanentShift,
	branchMs.BranchName,
	branchMs.Id AS EmployeeBranchId
	from EMP_Shift AS empShift
		INNER JOIN EMP_Info as emp ON emp.Id = empShift.EMP_Info_Id
		INNER JOIN MS_Shift as shiftMs ON shiftMs.Id = empShift.MS_Shift_Id
		INNER JOIN MS_Branch as branchMs ON branchMs.Id = shiftMs.MS_Branch_Id
	WHERE emp.IsActive = 1
		AND branchMs.Id = @branchId
		AND empShift.IsAmmend = 0
		AND empShift.MS_Shift_Id = @shiftId