using ServerModel.Data;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.PersonalInfo
{
    public static class EmployeePersonalInfoAccess
    {
        public static EmployeeInformation GetEmployeeApproverByEmpId(Guid employeeId)
        {
            EmployeeInformation employeeInformation = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeSetup.PersonalInfo.Sql.Sql.GetEmployeeApproverByEmpId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@employeeId", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeInformation = new EmployeeInformation
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                FullName = reader["FullName"].ToString()
                            };
                        }
                    }
                }
            }

            return employeeInformation;
        }
    }

    public static class EmployeePersonalInfoAccess<T> where T : EmployeePersonalInformation
    {
        public static Guid UpsertEmployeePersonalInfo(EmployeePersonalInformation employeePersonalInformation)
        {
            try
            {
                string sql = EmployeeSetup.PersonalInfo.Sql.Sql.UpsertEmployeePersonalInfo;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    // cmd.Parameters.AddWithValue("@id", employeePersonalInformation.Id).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@id", employeePersonalInformation.Id);
                    cmd.Parameters.AddWithValue("@formdate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@machineId", !string.IsNullOrEmpty(employeePersonalInformation.MachineId) ? employeePersonalInformation.MachineId : "Ajinkya-PC");
                    cmd.Parameters.AddWithValue("@machineIp", !string.IsNullOrEmpty(employeePersonalInformation.MachineIp) ? employeePersonalInformation.MachineIp : "1.1.1.1");
                    cmd.Parameters.AddWithValue("@compId", employeePersonalInformation.CompId != null ? employeePersonalInformation.CompId : Guid.Empty);
                    cmd.Parameters.AddWithValue("@createdby", employeePersonalInformation.CreatedBy != null ? employeePersonalInformation.CreatedBy : Guid.Empty);
                    cmd.Parameters.AddWithValue("@empId", employeePersonalInformation.EMP_Info_Id);
                    cmd.Parameters.AddWithValue("@email", employeePersonalInformation.Email);
                    cmd.Parameters.AddWithValue("@mobile1", employeePersonalInformation.Mobile1);
                    cmd.Parameters.AddWithValue("@mobile2", employeePersonalInformation.Mobile2);
                    cmd.Parameters.AddWithValue("@maritialstatus", employeePersonalInformation.MaritialStatus);
                    cmd.Parameters.AddWithValue("@marriagedate", employeePersonalInformation.MarrigeDate.HasValue ? employeePersonalInformation.MarrigeDate.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nosofchild", employeePersonalInformation.NoOfChildren);
                    cmd.Parameters.AddWithValue("@birthplace", employeePersonalInformation.BirthPlace);
                    cmd.Parameters.AddWithValue("@religion", employeePersonalInformation.Religion);
                    cmd.Parameters.AddWithValue("@licno", employeePersonalInformation.LICNo);
                    cmd.Parameters.AddWithValue("@passportno", employeePersonalInformation.PassportNo);
                    cmd.Parameters.AddWithValue("@vehicleno", employeePersonalInformation.VehicleNo);
                    cmd.Parameters.AddWithValue("@bloodgrp", employeePersonalInformation.BloodGroup);
                    cmd.Parameters.AddWithValue("@hobbies", employeePersonalInformation.Hobbies);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Guid.TryParse(returnObj.ToString(), out Guid returnValue);
                        return returnValue;
                    }

                    return Guid.Empty;

                    // var ddd = employeePersonalInformation.Id;
                }
            }catch(Exception ex)
            {
                return Guid.Empty;
            }
        }

        public static EmployeePersonalInformation GetEmployeeDetailByEmailAddress(string emailAddress)
        {
            EmployeePersonalInformation employeePersonalInformation = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeSetup.PersonalInfo.Sql.Sql.GetEmployeeDetailByEmailAddress;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@emailAddress", emailAddress);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeePersonalInformation = new EmployeePersonalInformation
                            {
                                EMP_Info_Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                EmployeeName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                                ActiveFrom = reader["ActiveFrom"] != DBNull.Value ? Convert.ToDateTime(reader["ActiveFrom"].ToString()) : DateTime.MinValue,
                                ActiveTo = reader["ActiveTo"] != DBNull.Value ? Convert.ToDateTime(reader["ActiveTo"].ToString()) : DateTime.MaxValue
                            };
                        }
                    }
                }
                return employeePersonalInformation;
            }
        }

        public static int GetNextEmpId(Guid compId)
        {
            string connStr = DbContext.ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(TRY_CAST(EmpId AS INT)), 0) + 1 FROM EMP_Info WHERE CompId = '"+compId+"'", conn))
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 1;
            }
        }

        public static IEnumerable<EmployeePersonalInformation> GetEmployeePersonalInfoById(Guid employeeId)
        {
            return null;
        }
    }
}
