using ServerModel.Data;
using ServerModel.Model.Masters;
using ServerModel.Model.Training;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.EmployeeSetup.TrainingSetup
{
    public class TrainingSetupAccess
    {
        public static List<EmployeeTrainingInfo> GetEmployeeTrainingsByCompId(Guid compId)
        {
            List<EmployeeTrainingInfo> employeeTrainings = new List<EmployeeTrainingInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeSetup.TrainingSetup.Sql.GetEmployeeTrainingsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeeTraining = new EmployeeTrainingInfo
                            {
                                Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                EmployeeName = reader["EmployeeName"] != DBNull.Value ? reader["EmployeeName"].ToString() : null,
                                TrainingHead_EMP_Id = reader["TrainingHead_EMP_Id"] != DBNull.Value ? Guid.Parse(reader["TrainingHead_EMP_Id"].ToString()) : Guid.Empty,
                                TrainingHeadEmployeeName = reader["TrainingHeadEmployeeName"] != DBNull.Value ? reader["TrainingHeadEmployeeName"].ToString() : null,
                                MS_Training_Id = reader["MS_Training_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Training_Id"].ToString()) : 0,
                                TrainingName = reader["TrainingName"] != DBNull.Value ? reader["TrainingName"].ToString() : null,

                                StartOn = reader["StartOn"] != DBNull.Value ? Convert.ToDateTime(reader["StartOn"].ToString()) : DateTime.MinValue,
                                EndOn = reader["EndOn"] != DBNull.Value ? Convert.ToDateTime(reader["EndOn"].ToString()) : DateTime.MinValue,
                                Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"].ToString()) : 0,
                                DesignationId = reader["MS_Designation_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Designation_Id"].ToString()) : 0,
                                DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : null
                            };

                            employeeTrainings.Add(employeeTraining);
                        }
                    }
                }
            }

            return employeeTrainings;
        }

        public static Guid UpsertEmployeeTraining(EmployeeTrainingInfo employeeTraining)
        {
            try
            {
                string sql = TrainingSetup.Sql.UpsertEmployeeTraining;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", employeeTraining.Id);
                    cmd.Parameters.AddWithValue("@CompId", employeeTraining.CompId);
                    cmd.Parameters.AddWithValue("@EMP_Info_Id", employeeTraining.EMP_Info_Id);
                    cmd.Parameters.AddWithValue("@TrainingHead_EMP_Id", employeeTraining.TrainingHead_EMP_Id);
                    cmd.Parameters.AddWithValue("@MS_Training_Id", employeeTraining.MS_Training_Id);
                    cmd.Parameters.AddWithValue("@StartOn", employeeTraining.StartOn);
                    cmd.Parameters.AddWithValue("@EndOn", employeeTraining.EndOn);
                    cmd.Parameters.AddWithValue("@Status", employeeTraining.Status);
                    cmd.Parameters.AddWithValue("@Active", employeeTraining.Active);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Guid.TryParse(returnObj.ToString(), out Guid returnValue);
                        return returnValue;
                    }

                    return Guid.Empty;

                    // var ddd = employeeFamilyInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }
    }
}
