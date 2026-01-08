using ServerModel.Data;
using ServerModel.Model.Punch;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.Punch
{
    public static class PunchSetupAccess
    {
        public static int AddPunchInTime(PunchInOut punchInOut)
        {
            string sql = PunchSql.AddPunchInOut;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@machineId", punchInOut.MachineId);
                cmd.Parameters.AddWithValue("@machineIp", punchInOut.MachineIp);
                cmd.Parameters.AddWithValue("@compId", punchInOut.CompId);
                cmd.Parameters.AddWithValue("@createdBy", punchInOut.CreatedBy);
                cmd.Parameters.AddWithValue("@date", punchInOut.Date.Date);
                cmd.Parameters.AddWithValue("@branchId", punchInOut.MS_Branch_Id);
                cmd.Parameters.AddWithValue("@empId", punchInOut.EMP_Info_Id);
                cmd.Parameters.AddWithValue("@inTime", punchInOut.Intime);
                cmd.Parameters.AddWithValue("@outTime", punchInOut.Outtime);

                return cmd.ExecuteNonQuery();
            }
        }
    }
        
}
