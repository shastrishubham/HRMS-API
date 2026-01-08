using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerModel.Data;
using ServerModel.Model.Employee;

namespace ServerModel.SqlAccess.EmployeeSetup.BankInfo
{
    public class EmployeeBankInfoAccess
    {
    }

    public class EmployeeBankInfoAccess<T> where T : EmployeeBankInformation
    {
        public static EmployeeBankInformation GetEmployeeBankInfoById(Guid employeeId, Guid companyId)
        {
            EmployeeBankInformation employeeBankInformation = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeSetup.BankInfo.Sql.Sql.GetEmployeeAccountDetailbyEmpId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", companyId);
                    command.Parameters.AddWithValue("@EMP_Info_Id", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeBankInformation = new EmployeeBankInformation
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                FullName = reader["FullName"].ToString(),
                                MS_Bank_Id = reader["MS_Bank_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Bank_Id"]) : 0,
                                BankName = reader["BankName"] != DBNull.Value ? reader["BankName"].ToString() : "",
                                MS_Bank_Branch_Id = reader["MS_Bank_Branch_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Bank_Branch_Id"]) : 0,
                                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                AccountNo = reader["AccountNo"] != DBNull.Value ? reader["AccountNo"].ToString() : "",
                                IFSC = reader["IFSC"] != DBNull.Value ? reader["IFSC"].ToString() : "",
                                MICR = reader["MICR"] != DBNull.Value ? reader["MICR"].ToString() : "",
                                MS_AccountType_Id = reader["MS_AccountType_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_AccountType_Id"]) : 0,
                                MS_PaymentType_Id = reader["MS_PaymentType_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_PaymentType_Id"]) : 0,
                                DDPayableAt = reader["DDPayableAt"] != DBNull.Value ? reader["DDPayableAt"].ToString() : "",
                                NameAsPerBank = reader["NameAsPerBank"] != DBNull.Value ? reader["NameAsPerBank"].ToString() : "",
                                UAN = reader["UAN"] != DBNull.Value ? reader["UAN"].ToString() : "",
                                PFNo = reader["PFNo"] != DBNull.Value ? reader["PFNo"].ToString() : "",
                                IsCoveredESI = reader["IsCoveredESI"] != DBNull.Value ? Convert.ToBoolean(reader["IsCoveredESI"]) : false,
                                ESINo = reader["ESINo"] != DBNull.Value ? reader["ESINo"].ToString() : "",
                                IsCoveredLWF = reader["IsCoveredLWF"] != DBNull.Value ? Convert.ToBoolean(reader["IsCoveredLWF"]) : false,
                                IsAmend = reader["IsAmend"] != DBNull.Value ? Convert.ToBoolean(reader["IsAmend"]) : false
                            };
                        }
                    }
                }
            }

            return employeeBankInformation;
        }

        public static int UpsertEmployeeBankInfo(EmployeeBankInformation employeeBankInformation)
        {
            try
            {
                string sql = EmployeeSetup.BankInfo.Sql.Sql.UpsertEmployeeBankInfo;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", employeeBankInformation.Id);
                    cmd.Parameters.AddWithValue("@formdate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@machineIp", !string.IsNullOrEmpty(employeeBankInformation.MachineIp) ? employeeBankInformation.MachineIp : "1.1.1.1");
                    cmd.Parameters.AddWithValue("@machineId", !string.IsNullOrEmpty(employeeBankInformation.MachineId) ? employeeBankInformation.MachineId : "Ajinkya-PC");
                    cmd.Parameters.AddWithValue("@compId", employeeBankInformation.CompId != null ? employeeBankInformation.CompId : Guid.Empty);
                    cmd.Parameters.AddWithValue("@createdby", employeeBankInformation.CreatedBy != null ? employeeBankInformation.CreatedBy : Guid.Empty);
                    cmd.Parameters.AddWithValue("@EMP_Info_Id", employeeBankInformation.EMP_Info_Id);
                    cmd.Parameters.AddWithValue("@MS_Bank_Id", employeeBankInformation.MS_Bank_Id > 0 ? employeeBankInformation.MS_Bank_Id : 0);
                    cmd.Parameters.AddWithValue("@MS_Bank_Branch_Id", employeeBankInformation.MS_Bank_Branch_Id > 0 ? employeeBankInformation.MS_Bank_Branch_Id : 0);
                    cmd.Parameters.AddWithValue("@AccountNo", !string.IsNullOrEmpty(employeeBankInformation.AccountNo) ? employeeBankInformation.AccountNo : "00000");
                    cmd.Parameters.AddWithValue("@MS_AccountType_Id", employeeBankInformation.MS_AccountType_Id > 0 ? employeeBankInformation.MS_AccountType_Id : 0);
                    cmd.Parameters.AddWithValue("@MS_PaymentType_Id", employeeBankInformation.MS_PaymentType_Id > 0 ? employeeBankInformation.MS_PaymentType_Id : 0);
                    cmd.Parameters.AddWithValue("@DDPayableAt", !string.IsNullOrEmpty(employeeBankInformation.DDPayableAt) ? employeeBankInformation.DDPayableAt : "");
                    cmd.Parameters.AddWithValue("@NameAsPerBank", !string.IsNullOrEmpty(employeeBankInformation.NameAsPerBank) ? employeeBankInformation.NameAsPerBank : "");
                    cmd.Parameters.AddWithValue("@UAN", !string.IsNullOrEmpty(employeeBankInformation.UAN) ? employeeBankInformation.UAN : "0");
                    cmd.Parameters.AddWithValue("@PFNo", !string.IsNullOrEmpty(employeeBankInformation.PFNo) ? employeeBankInformation.PFNo : "0");
                    cmd.Parameters.AddWithValue("@IsCoveredESI", employeeBankInformation.IsCoveredESI);
                    cmd.Parameters.AddWithValue("@ESINo", !string.IsNullOrEmpty(employeeBankInformation.ESINo) ? employeeBankInformation.ESINo : "0");
                    cmd.Parameters.AddWithValue("@IsCoveredLWF", employeeBankInformation.IsCoveredLWF);
                    cmd.Parameters.AddWithValue("@IsAmend", employeeBankInformation.IsAmend);


                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        return int.Parse(returnObj.ToString());
                    }

                    return 0;

                    // var ddd = employeeBankInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

