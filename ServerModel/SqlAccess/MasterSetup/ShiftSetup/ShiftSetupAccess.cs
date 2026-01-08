using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.ShiftSetup
{
    public class ShiftSetupAccess
    {
        public static List<ShiftInfo> GetShiftDetailsByBranchIdAndCompId(Guid companyId, int branchId)
        {
            List<ShiftInfo> shifts = new List<ShiftInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetShiftDetailsByBranchIdAndCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);
                    command.Parameters.AddWithValue("@BranchId", branchId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var shift = new ShiftInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                MS_Branch_Id = Convert.ToInt32(reader["MS_Branch_Id"].ToString()),
                                ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString() : "",
                                ShiftCode = reader["ShiftCode"] != DBNull.Value ? reader["ShiftCode"].ToString() : "",
                                ShiftShortName = reader["ShiftShortName"] != DBNull.Value ? reader["ShiftShortName"].ToString() : "",
                                StartTime = reader["StartTime"] != DBNull.Value ? TimeSpan.Parse(reader["StartTime"].ToString()) : new TimeSpan(),
                                EndTime = reader["EndTime"] != DBNull.Value ? TimeSpan.Parse(reader["EndTime"].ToString()) : new TimeSpan(),
                                TotalHrs = reader["TotalHrs"] != DBNull.Value ? Convert.ToDecimal(reader["TotalHrs"].ToString()) : 0,
                                WeeklyOffDay = reader["WeeklyOffDay"] != DBNull.Value ? reader["WeeklyOffDay"].ToString() : string.Empty,
                                IsShiftAllowance = reader["IsShiftAllowance"] != DBNull.Value ? Convert.ToBoolean(reader["IsShiftAllowance"].ToString()) : false,
                                ShiftAllowanceAmtPerDay = reader["ShiftAllowanceAmtPerDay"] != DBNull.Value ? Convert.ToDecimal(reader["ShiftAllowanceAmtPerDay"].ToString()) : 0,
                                IsOTApplicable = reader["IsOTApplicable"] != DBNull.Value ? Convert.ToBoolean(reader["IsOTApplicable"].ToString()) : false,
                                OTAmtPerHrs = reader["OTAmtPerHrs"] != DBNull.Value ? Convert.ToDecimal(reader["OTAmtPerHrs"].ToString()) : 0,
                                OTApplicableAfterEndTime = reader["OTApplicableAfterEndTime"] != DBNull.Value ? TimeSpan.Parse(reader["OTApplicableAfterEndTime"].ToString()) : new TimeSpan(),
                                EMP_Info_Id = reader["SupervisorId"] != DBNull.Value ? Guid.Parse(reader["SupervisorId"].ToString()) : Guid.Empty,
                                SupervisorName = reader["Supervisor"] != DBNull.Value ? reader["Supervisor"].ToString() : "",
                                IsLateMarkApplicable = reader["IsLateMarkApplicable"] != DBNull.Value ? Convert.ToBoolean(reader["IsLateMarkApplicable"].ToString()) : false,
                                LateMarkAfterOn = reader["LateMarkAfterOn"] != DBNull.Value ? TimeSpan.Parse(reader["LateMarkAfterOn"].ToString()) : new TimeSpan(),
                                Active = reader["Active"] != DBNull.Value ? Convert.ToBoolean(reader["Active"].ToString()) : false
                            };
                            shifts.Add(shift);
                        }
                    }
                }
            }

            return shifts;
        }

        public static List<ShiftInfo> GetShiftDetailsByCompId(Guid companyId)
        {
            List<ShiftInfo> shifts = new List<ShiftInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetShiftDetailsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var shift = new ShiftInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                MS_Branch_Id = Convert.ToInt32(reader["MS_Branch_Id"].ToString()),
                                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString() : "",
                                ShiftCode = reader["ShiftCode"] != DBNull.Value ? reader["ShiftCode"].ToString() : "",
                                ShiftShortName = reader["ShiftShortName"] != DBNull.Value ? reader["ShiftShortName"].ToString() : "",
                                StartTime = reader["StartTime"] != DBNull.Value ? TimeSpan.Parse(reader["StartTime"].ToString()) : new TimeSpan(),
                                EndTime = reader["EndTime"] != DBNull.Value ? TimeSpan.Parse(reader["EndTime"].ToString()) : new TimeSpan(),
                                TotalHrs = reader["TotalHrs"] != DBNull.Value ? Convert.ToDecimal(reader["TotalHrs"].ToString()) : 0,
                                WeeklyOffDay = reader["WeeklyOffDay"] != DBNull.Value ? reader["WeeklyOffDay"].ToString() : string.Empty,
                                IsShiftAllowance = reader["IsShiftAllowance"] != DBNull.Value ? Convert.ToBoolean(reader["IsShiftAllowance"].ToString()) : false,
                                ShiftAllowanceAmtPerDay = reader["ShiftAllowanceAmtPerDay"] != DBNull.Value ? Convert.ToDecimal(reader["ShiftAllowanceAmtPerDay"].ToString()) : 0,
                                IsOTApplicable = reader["IsOTApplicable"] != DBNull.Value ? Convert.ToBoolean(reader["IsOTApplicable"].ToString()) : false,
                                OTAmtPerHrs = reader["OTAmtPerHrs"] != DBNull.Value ? Convert.ToDecimal(reader["OTAmtPerHrs"].ToString()) : 0,
                                OTApplicableAfterEndTime = reader["OTApplicableAfterEndTime"] != DBNull.Value ? TimeSpan.Parse(reader["OTApplicableAfterEndTime"].ToString()) : new TimeSpan(),
                                EMP_Info_Id = reader["SupervisorId"] != DBNull.Value ? Guid.Parse(reader["SupervisorId"].ToString()) : Guid.Empty,
                                SupervisorName = reader["Supervisor"] != DBNull.Value ? reader["Supervisor"].ToString() : "",
                                IsLateMarkApplicable = reader["IsLateMarkApplicable"] != DBNull.Value ? Convert.ToBoolean(reader["IsLateMarkApplicable"].ToString()) : false,
                                LateMarkAfterOn = reader["LateMarkAfterOn"] != DBNull.Value ? TimeSpan.Parse(reader["LateMarkAfterOn"].ToString()) : new TimeSpan(),
                                Active = reader["Active"] != DBNull.Value ? Convert.ToBoolean(reader["Active"].ToString()) : false
                            };
                            shifts.Add(shift);
                        }
                    }
                }
            }

            return shifts;
        }

        public static int UpsertShift(ShiftInfo shiftInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertShift;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", shiftInfo.Id);
                    cmd.Parameters.AddWithValue("@CompId", shiftInfo.CompId);
                    cmd.Parameters.AddWithValue("@MS_Branch_Id", shiftInfo.MS_Branch_Id);
                    cmd.Parameters.AddWithValue("@ShiftName", shiftInfo.ShiftName);
                    cmd.Parameters.AddWithValue("@ShiftCode", shiftInfo.ShiftCode);
                    cmd.Parameters.AddWithValue("@ShiftShortName", shiftInfo.ShiftShortName);
                    cmd.Parameters.AddWithValue("@StartTime", shiftInfo.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", shiftInfo.EndTime);
                    cmd.Parameters.AddWithValue("@TotalHrs", shiftInfo.TotalHrs);
                    cmd.Parameters.AddWithValue("@WeeklyOffDay", string.IsNullOrEmpty(shiftInfo.WeeklyOffDay) ? DBNull.Value.ToString() : shiftInfo.WeeklyOffDay);
                    cmd.Parameters.AddWithValue("@IsShiftAllowance", shiftInfo.IsShiftAllowance ?? false);
                    cmd.Parameters.AddWithValue("@ShiftAllowanceAmtPerDay", shiftInfo.ShiftAllowanceAmtPerDay ?? 0);
                    cmd.Parameters.AddWithValue("@IsOTApplicable", shiftInfo.IsOTApplicable ?? false);
                    cmd.Parameters.AddWithValue("@OTAmtPerHrs", shiftInfo.OTAmtPerHrs ?? 0);
                    cmd.Parameters.AddWithValue("@OTApplicableAfterEndTime", SqlDbType.Time).Value = shiftInfo.OTApplicableAfterEndTime.HasValue ? (object)shiftInfo.OTApplicableAfterEndTime.Value : DBNull.Value;
                    cmd.Parameters.AddWithValue("@EMP_Info_Id", shiftInfo.EMP_Info_Id ?? Guid.Empty);
                    cmd.Parameters.AddWithValue("@IsLateMarkApplicable", shiftInfo.IsLateMarkApplicable ?? false);
                    cmd.Parameters.AddWithValue("@LateMarkAfterOn", SqlDbType.Time).Value = shiftInfo.LateMarkAfterOn.HasValue ? (object)shiftInfo.LateMarkAfterOn.Value : DBNull.Value;
                    cmd.Parameters.AddWithValue("@Active", shiftInfo.Active ?? true);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }

                    return 0;

                    // var ddd = employeeFamilyInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
