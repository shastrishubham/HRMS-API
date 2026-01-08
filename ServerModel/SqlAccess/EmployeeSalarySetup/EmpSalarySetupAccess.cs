using ServerModel.Data;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSalarySetup
{
    public class EmpSalarySetupAccess
    {
    }

    public static class EmpSalarySetupAccess<T> where T : EmployeeSalarySetupDetails
    {
        public static int AddUpdateEmployeeSalarySetup(EmployeeSalarySetupDetails employeeSalarySetupDetails)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(employeeSalarySetupDetails);
            var jsonString1 = Newtonsoft.Json.JsonConvert
                .SerializeObject(employeeSalarySetupDetails.employeeSalaryHeadsSetupDetails);

            string sql = EmployeeSalarySetup.Sql.AddUpdateEmpSalarySetup;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@json", jsonString);
                cmd.Parameters.AddWithValue("@json1", jsonString1);

                return cmd.ExecuteNonQuery();
            }
        }

        public static List<EmployeeSalarySetupDetails> GetSalarySetupByCompId(Guid companyId)
        {
            List<EmployeeSalarySetupDetails> employeesSalarySetupDetails = new List<EmployeeSalarySetupDetails>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeSalarySetup.Sql.GetSalarySetupByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var salarySetup = new EmployeeSalarySetupDetails
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                EmpId = reader["EMP_Info_Id"]  != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,


                                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : "",
                                TotalEarningAmt = reader["TotalEarningAmt"] != DBNull.Value ? Convert.ToDecimal(reader["TotalEarningAmt"]) : 0,
                                TotalDeductionAmt = reader["TotalDeductionAmt"] != DBNull.Value ? Convert.ToDecimal(reader["TotalDeductionAmt"]) : 0,

                                MS_PayMode_Id = reader["MS_PayMode_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_PayMode_Id"]) : 0,
                            };
                        
                            employeesSalarySetupDetails.Add(salarySetup);
                        }
                    }
                }
            }

            return employeesSalarySetupDetails;
        }

        public static List<EmployeeSalaryHeadsSetupDetails> GetEmpSalarySetupDetails(Guid companyId, Guid employeeId)
        {
            List<EmployeeSalaryHeadsSetupDetails> empSalarySetupDetails = new List<EmployeeSalaryHeadsSetupDetails>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeSalarySetup.Sql.GetEmpSalarySetupDetails;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", companyId);
                    command.Parameters.AddWithValue("@empId", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empSalarySetup = new EmployeeSalaryHeadsSetupDetails
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                EmpId = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                MS_SLHeads_Id = reader["MS_SLHeads_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHeads_Id"]) : 0,

                                SalaryHeadName = reader["SalaryHeadName"] != DBNull.Value ? reader["SalaryHeadName"].ToString() : "",
                                IsEarningComponent = reader["IsEarningComponent"] != DBNull.Value ? Convert.ToBoolean(reader["IsEarningComponent"].ToString()) : true,
                                Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0,
                                MS_PayMode_Id = reader["MS_PayMode_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_PayMode_Id"]) : 0,
                                SalaryHeadOrder = reader["SalaryHeadOrder"] != DBNull.Value ? Convert.ToInt32(reader["SalaryHeadOrder"]) : 999,

                                EMP_SLSetup_Id = reader["EMP_SLSetup_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_SLSetup_Id"].ToString()) : Guid.Empty,
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                            };

                            empSalarySetupDetails.Add(empSalarySetup);
                        }
                    }
                }
            }

            return empSalarySetupDetails;
        }


        public static List<EmployeeSalarySetupDetails> GetUnSetupSalaryEmployeesByDesignationId(Guid compId, int designationId)
        {
            List<EmployeeSalarySetupDetails> empDetails = new List<EmployeeSalarySetupDetails>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeSalarySetup.Sql.GetUnSetupSalaryEmployeesByDesignationId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@designationId", designationId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var emp = new EmployeeSalarySetupDetails
                            {
                                Id = Guid.Parse(reader["EmpId"].ToString()),
                                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty
                            };

                            empDetails.Add(emp);
                        }
                    }
                }
            }

            return empDetails;
        }



    }

}
