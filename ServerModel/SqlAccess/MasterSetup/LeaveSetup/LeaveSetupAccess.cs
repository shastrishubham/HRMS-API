using ServerModel.Data;
using ServerModel.Model.Base;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.LeaveSetup
{
    public class LeaveSetupAccess
    {
        public static List<LeaveInfo> GetLeavesByCompId(Guid companyId)
        {
            List<LeaveInfo> leaves = new List<LeaveInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetLeaveDetails;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var leave = new LeaveInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                LeaveName = reader["LeaveName"].ToString(),
                                LeaveCode = reader["LeaveCode"].ToString(),
                                LeaveType = (LeaveTypes)reader["LeaveType"],
                                Unit = reader["Unit"].ToString(),
                                EffectiveAfterOnTypes = (LeaveEffectiveAfterOnTypes)reader["EffectiveAfterOnTypes"],
                                CarryForward = Convert.ToInt32(reader["CarryForward"].ToString()),
                                ApplicableFor = reader["ApplicableFor"].ToString(),
                                MS_Branch_Id = Convert.ToInt32(reader["MS_Branch_Id"].ToString()),
                                DurationAllowed = (LeaveDurationAllowedTypes)reader["DurationAllowed"],
                                Active = Convert.ToBoolean(reader["Active"].ToString())
                            };
                            leaves.Add(leave);
                        }
                    }
                }
            }

            return leaves;
        }

        public static int UpsertLeave(LeaveInfo leaveInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertLeave;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    //Enum.GetName(typeof(LeaveTypes), leaveInfo.LeaveType.ToString()
                    cmd.Parameters.AddWithValue("@Id", leaveInfo.Id);
                    cmd.Parameters.AddWithValue("@CompId", leaveInfo.CompId);
                    cmd.Parameters.AddWithValue("@LeaveName", leaveInfo.LeaveName);
                    cmd.Parameters.AddWithValue("@LeaveCode", leaveInfo.LeaveCode);
                    cmd.Parameters.AddWithValue("@LeaveType", leaveInfo.LeaveType);
                    cmd.Parameters.AddWithValue("@Unit", leaveInfo.Unit);
                    cmd.Parameters.AddWithValue("@EffectiveAfterOnTypes", leaveInfo.EffectiveAfterOnTypes);
                    cmd.Parameters.AddWithValue("@CarryForward", leaveInfo.CarryForward);
                    cmd.Parameters.AddWithValue("@ApplicableFor", leaveInfo.ApplicableFor);
                    cmd.Parameters.AddWithValue("@MS_Branch_Id", leaveInfo.MS_Branch_Id);
                    cmd.Parameters.AddWithValue("@DurationAllowed", leaveInfo.DurationAllowed);
                    cmd.Parameters.AddWithValue("@Active", leaveInfo.Active);

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
