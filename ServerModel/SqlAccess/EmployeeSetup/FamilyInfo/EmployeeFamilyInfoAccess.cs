using ServerModel.Data;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.FamilyInfo
{
    public class EmployeeFamilyInfoAccess
    {
    }

    public static class EmployeeFamilyInfoAccess<T> where T : EmployeeFamilyInformation
    {
        public static bool UpsertEmployeeFamilyInfo(List<EmployeeFamilyInformation> employeeFamilyInformations)
        {
            try
            {
                string sql = EmployeeSetup.FamilyInfo.Sql.Sql.UpsertEmployeeFamilyInfo;
                string strConString = DbContext.ConnectionString;

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(employeeFamilyInformations);
                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    if (employeeFamilyInformations.Count > 0)
                    {
                        cmd.Parameters.AddWithValue("@empId", employeeFamilyInformations[0].EMP_Info_Id);
                        cmd.Parameters.AddWithValue("@json", jsonString);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public static IEnumerable<EmployeeFamilyInformation> GetEmployeeFamilyInfoById(Guid employeeId)
        {
            return null;
        }



    }
}
