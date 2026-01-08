using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.DepartmentSetup
{
    public class DepartmentSetupAccess
    {
        public static DepartmentRegistration GetDepartmentDetails(int locationId)
        {
            DepartmentRegistration departmentRegistration = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetLocationDetails;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", locationId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departmentRegistration = new DepartmentRegistration
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : null,
                                DepartmentCode = reader["DepartmentCode"] != DBNull.Value ? reader["DepartmentCode"].ToString() : null,
                                DepartmentShortName = reader["DepartmentShortName"] != DBNull.Value ? reader["DepartmentShortName"].ToString() : null,
                                MailAlias = reader["MailAlias"] != DBNull.Value ? reader["MailAlias"].ToString(): null,
                                DepartmentLead_Id = reader["DepartmentLead_Id"] != DBNull.Value ? Guid.Parse(reader["DepartmentLead_Id"].ToString()) : Guid.Empty,
                                ParentDepartment_Id = reader["ParentDepartment_Id"] != DBNull.Value ? Convert.ToInt32(reader["ParentDepartment_Id"].ToString()) : 0,
                                CreatedOn = reader["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedOn"].ToString()) : DateTime.Now,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Guid.Parse(reader["CreatedBy"].ToString()) : Guid.Empty,
                                ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedOn"].ToString()) : DateTime.Now,
                                ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? Guid.Parse(reader["ModifiedBy"].ToString()) : Guid.Empty,
                            };

                        }
                    }
                }
            }

            return departmentRegistration;
        }

        public static List<DepartmentRegistration> GetDepartmentsByCompId(Guid companyId)
        {
            List<DepartmentRegistration> departments = new List<DepartmentRegistration>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetDepartmentDetailsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var departmentRegistration = new DepartmentRegistration
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : null,
                                DepartmentCode = reader["DepartmentCode"] != DBNull.Value ? reader["DepartmentCode"].ToString() : null,
                                DepartmentShortName = reader["DepartmentShortName"] != DBNull.Value ? reader["DepartmentShortName"].ToString() : null,
                                MailAlias = reader["MailAlias"] != DBNull.Value ? reader["MailAlias"].ToString() : null,
                                DepartmentLead_Id = reader["DepartmentLead_Id"] != DBNull.Value ? Guid.Parse(reader["DepartmentLead_Id"].ToString()) : Guid.Empty,
                                DepartmentLeadName = reader["DepartmentLeadName"] != DBNull.Value ? reader["DepartmentLeadName"].ToString() : null,
                                ParentDepartment_Id = reader["ParentDepartment_Id"] != DBNull.Value ? Convert.ToInt32(reader["ParentDepartment_Id"].ToString()) : 0,
                                ParentDepartmentName = reader["ParentDepartmentName"] != DBNull.Value ? reader["ParentDepartmentName"].ToString() : null,
                                CreatedOn = reader["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedOn"].ToString()) : DateTime.Now,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Guid.Parse(reader["CreatedBy"].ToString()) : Guid.Empty,
                                ModifiedOn = reader["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(reader["ModifiedOn"].ToString()) : DateTime.Now,
                                ModifiedBy = reader["ModifiedBy"] != DBNull.Value ? Guid.Parse(reader["ModifiedBy"].ToString()) : Guid.Empty,
                            };
                            departments.Add(departmentRegistration);
                        }
                    }
                }
            }

            return departments;
        }

        public static int UpsertDepartmentSetup(DepartmentRegistration departmentRegistration)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertDepartment;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", departmentRegistration.Id);
                    cmd.Parameters.AddWithValue("@MachineIp", departmentRegistration.MachineIp ?? "");
                    cmd.Parameters.AddWithValue("@MachineId", departmentRegistration.MachineId ?? "");
                    cmd.Parameters.AddWithValue("@CompId", departmentRegistration.CompId);
                    cmd.Parameters.AddWithValue("@DepartmentName", departmentRegistration.DepartmentName);
                    cmd.Parameters.AddWithValue("@DepartmentCode", departmentRegistration.DepartmentCode);
                    cmd.Parameters.AddWithValue("@DepartmentShortName", departmentRegistration.DepartmentShortName);
                    cmd.Parameters.AddWithValue("@MailAlias", departmentRegistration.MailAlias);
                    cmd.Parameters.AddWithValue("@DepartmentLead_Id", departmentRegistration.DepartmentLead_Id);
                    cmd.Parameters.AddWithValue("@ParentDepartment_Id", departmentRegistration.ParentDepartment_Id);
                    cmd.Parameters.AddWithValue("@CreatedOn", departmentRegistration.CreatedOn == DateTime.MinValue ? DateTime.Now : departmentRegistration.CreatedOn);
                    cmd.Parameters.AddWithValue("@CreatedBy", departmentRegistration.CreatedBy);
                    cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ModifiedBy", departmentRegistration.ModifiedBy);
                    cmd.Parameters.AddWithValue("@Active", departmentRegistration.Active);

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
