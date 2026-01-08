
/*

---GetShiftDetailsByBranchIdAndCompId


DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';

*/

SELECT shift.Id
      ,shift.CompId
      ,shift.MS_Branch_Id
	  ,branch.BranchName
      ,shift.ShiftName
      ,shift.ShiftCode
      ,shift.ShiftShortName
      ,shift.StartTime
      ,shift.EndTime
      ,shift.TotalHrs
      ,shift.WeeklyOffDay
	  ,shift.IsShiftAllowance
	  ,shift.ShiftAllowanceAmtPerDay
	  ,shift.IsOTApplicable
	  ,shift.OTAmtPerHrs
	  ,shift.OTApplicableAfterEndTime
	  ,shift.EMP_Info_Id AS 'SupervisorId'
	  ,emp.FullName AS 'Supervisor'
      ,shift.IsLateMarkApplicable
      ,shift.LateMarkAfterOn
      ,shift.Active
  FROM MS_Shift AS shift
	INNER JOIN MS_Branch AS branch ON shift.MS_Branch_Id = branch.Id
	LEFT JOIN EMP_Info AS emp ON emp.Id =shift.EMP_Info_Id
		AND emp.IsActive = 1
  WHERE shift.CompId = @CompId
		AND shift.Active = 1