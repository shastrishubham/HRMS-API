using ServerModel.Masters;
using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.ShiftSetup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerModel.ServerModel.Masters.ShiftSetup
{
    public class ShiftSetupServer
    {
        #region Properties Interface

        public static IShiftSetupAccess mShiftSetupAccessT
            = new ShiftSetupAccessWrapper();

        #endregion

        public static List<ShiftInfo> GetShiftDetailsByBranchIdAndCompId(Guid companyId, int branchId)
        {
            List<ShiftInfo> shifts = mShiftSetupAccessT.GetShiftDetailsByBranchIdAndCompId(companyId, branchId);
            foreach (ShiftInfo shift in shifts)
            {
                if (!string.IsNullOrEmpty(shift.WeeklyOffDay))
                {
                    shift.WeeklyOffDays = LeaveHelper.ParseWeeklyOffDayIds(shift.WeeklyOffDay);
                }
            }
            return shifts;
        }

        public static List<ShiftInfo> GetShiftDetailsByCompId(Guid companyId)
        {
            List<ShiftInfo>  shifts = mShiftSetupAccessT.GetShiftDetailsByCompId(companyId);

            foreach(ShiftInfo shift in shifts)
            {
                if (!string.IsNullOrEmpty(shift.WeeklyOffDay))
                {
                    shift.WeeklyOffDays = LeaveHelper.ParseWeeklyOffDayIds(shift.WeeklyOffDay);
                }
            }
            return shifts;
        }

        public static int UpsertShift(ShiftInfo shiftInfo)
        {
            // -1 no weekly off - for this shift
            if (shiftInfo.WeeklyOffDays != null && shiftInfo.WeeklyOffDays.Any() && shiftInfo.WeeklyOffDays[0] != -1)
            {
                // Convert to comma-separated string with null check
                shiftInfo.WeeklyOffDay = shiftInfo.WeeklyOffDays != null ? string.Join(",", shiftInfo.WeeklyOffDays) : string.Empty;
            }
            else
            {
                shiftInfo.WeeklyOffDay = null;
            }
            return mShiftSetupAccessT.UpsertShift(shiftInfo);
        }
    }
}
