

/*
---GetEmployeeAvailableLeavesById.Sql

DECLARE @empId UNIQUEIDENTIFIER = '90DAA615-055D-44B1-84F7-905C9E904791'

*/

select empLeaves.Id,
		empLeaves.CompId,
		empLeaves.EMP_Info_Id, 
		emp.FullName, 
		leave.LeaveName, 
		empLeaves.MS_Leave_Id, 
		TotalLeaves, 
		AvailableLeaves
	from EMP_Leaves as empLeaves
		INNER JOIN MS_Leave as leave on leave.Id = empLeaves.MS_Leave_Id
		INNER JOIN EMP_Info As emp on emp.Id = empLeaves.EMP_Info_Id
		where empLeaves.EMP_Info_Id = @empId
			and empLeaves.Active = 1
			and emp.IsActive = 1
   group by empLeaves.Id,
		empLeaves.CompId,
		empLeaves.EMP_Info_Id, 
		emp.FullName, 
		leave.LeaveName, 
		empLeaves.MS_Leave_Id, 
		TotalLeaves, 
		AvailableLeaves