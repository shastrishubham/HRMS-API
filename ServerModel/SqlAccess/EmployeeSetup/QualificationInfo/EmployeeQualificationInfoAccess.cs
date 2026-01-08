using ServerModel.Data;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.QualificationInfo
{
    public class EmployeeQualificationInfoAccess
    {
    }

    public static class EmployeeQualificationInfoAccess<T> where T : EmployeeQualificationInformation
    {
        public static bool UpsertEmployeeQualificationInfo(List<EmployeeQualificationInformation> employeeQualificationInformations)
        {
            try
            {
                string sql = EmployeeSetup.QualificationInfo.Sql.Sql.UpsertEmployeeQualificationInfo;
                string strConString = DbContext.ConnectionString;

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(employeeQualificationInformations);


                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    if (employeeQualificationInformations.Count > 0)
                    {
                        cmd.Parameters.AddWithValue("@empId", employeeQualificationInformations[0].EMP_Info_Id);
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



        public static IEnumerable<EmployeeQualificationInformation> GetEmployeeQualificationInfoById(Guid employeeId)
        {
            return null;
        }



    }
}
