
/*

---GetShiftDetailsByBranchIdAndCompId


DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @BranchId INT = 1;

*/

SELECT MS_Shift.Id
      ,MS_Shift.CompId
      ,MS_Shift.MS_Branch_Id
      ,MS_Shift.ShiftName
      ,MS_Shift.ShiftCode
      ,MS_Shift.ShiftShortName
      ,MS_Shift.StartTime
      ,MS_Shift.EndTime
      ,MS_Shift.TotalHrs
      ,MS_Shift.WeeklyOffDay
	  ,MS_Shift.IsShiftAllowance
	  ,MS_Shift.ShiftAllowanceAmtPerDay
	  ,MS_Shift.IsOTApplicable
	  ,MS_Shift.OTAmtPerHrs
	  ,MS_Shift.OTApplicableAfterEndTime
	  ,MS_Shift.EMP_Info_Id
      ,MS_Shift.IsLateMarkApplicable
	  ,EMP_Info_Id AS 'SupervisorId'
	  ,emp.FullName AS 'Supervisor'
      ,MS_Shift.LateMarkAfterOn
      ,MS_Shift.Active
  FROM MS_Shift
  LEFT JOIN EMP_Info AS emp ON emp.Id =MS_Shift.EMP_Info_Id
	AND emp.IsActive = 1
  WHERE MS_Shift.MS_Branch_Id = @BranchId 
	AND MS_Shift.CompId = @CompId
	AND MS_Shift.Active = 1