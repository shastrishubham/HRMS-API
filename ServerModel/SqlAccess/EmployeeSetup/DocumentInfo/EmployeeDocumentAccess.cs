using ServerModel.Data;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.EmployeeSetup.DocumentInfo
{
    public class EmployeeDocumentAccess
    {
        public static List<EmployeeDocument> GetEmployeeDocuments(Guid employeeId)
        {
            List<EmployeeDocument> employeeDocs = new List<EmployeeDocument>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = SqlAccess.EmployeeSetup.DocumentInfo.SqlDocumentUpload.GetEmployeeDocumentsByEmpId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@empId", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeeDoc = new EmployeeDocument
                            {
                                Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Guid.Parse(reader["CreatedBy"].ToString()) : Guid.Empty,
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                DocsKey = reader["DocsKey"] != DBNull.Value ? reader["DocsKey"].ToString() : null,
                                DocsValue = reader["DocsValue"] != DBNull.Value ? reader["DocsValue"].ToString() : null,
                            };
                            employeeDocs.Add(employeeDoc);
                        }
                    }
                }
            }

            return employeeDocs;
        }

        public static EmployeeDocument GetEmployeeDocument(Guid empDocId)
        {
            EmployeeDocument employeeDoc = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = SqlAccess.EmployeeSetup.DocumentInfo.SqlDocumentUpload.GetEmployeeDocumentsByDocId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@empDocId", empDocId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeDoc = new EmployeeDocument
                            {
                                Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Guid.Parse(reader["CreatedBy"].ToString()) : Guid.Empty,
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                DocsKey = reader["DocsKey"] != DBNull.Value ? reader["DocsKey"].ToString() : null,
                                DocsValue = reader["DocsValue"] != DBNull.Value ? reader["DocsValue"].ToString() : null,
                            };
                        }
                    }
                }
            }
            return employeeDoc;
        }

        public static bool UpsertEmployeeDocument(EmployeeDocument employeeDocument)
        {
            try
            {
                string sql = SqlAccess.EmployeeSetup.DocumentInfo.SqlDocumentUpload.UpsertEmployeeDocumentInfo;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", employeeDocument.Id);
                    cmd.Parameters.AddWithValue("@CompId", employeeDocument.CompId);
                    cmd.Parameters.AddWithValue("@EMP_Info_Id", employeeDocument.EMP_Info_Id);
                    cmd.Parameters.AddWithValue("@CreatedBy", employeeDocument.CreatedBy ?? Guid.Empty);
                    cmd.Parameters.AddWithValue("@DocsKey", employeeDocument.DocsKey);
                    cmd.Parameters.AddWithValue("@DocsValue", employeeDocument.DocsValue);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
