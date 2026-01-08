using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.MasterSetup.TrainingSetup
{
    public class TrainingSetupAccess
    {
        public static List<TrainingInfo> GetTrainingsByCompId(Guid compId)
        {
            List<TrainingInfo> trainings = new List<TrainingInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetTrainingsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var training = new TrainingInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                TrainingName = reader["TrainingName"] != DBNull.Value ? reader["TrainingName"].ToString() : "",
                                TrainingShortName = reader["TrainingShortName"] != DBNull.Value ? reader["TrainingShortName"].ToString() : "",
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "",
                                IsTrainingMandatory = reader["IsTrainingMandatory"] != DBNull.Value ? Convert.ToBoolean(reader["IsTrainingMandatory"].ToString()) : false,
                                MS_Designation_Id = reader["MS_Designation_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Designation_Id"].ToString()) : 0,
                                Designation = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : null
                            };
                            trainings.Add(training);
                        }
                    }
                }
            }

            return trainings;
        }

        public static List<TrainingInfo> GetTrainingsByDesignationIdAndCompId(Guid compId, int designationId)
        {
            List<TrainingInfo> trainings = new List<TrainingInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetTrainingsByDesignationIdAndCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);
                    command.Parameters.AddWithValue("@designationId", designationId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var training = new TrainingInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                TrainingName = reader["TrainingName"] != DBNull.Value ? reader["TrainingName"].ToString() : "",
                                TrainingShortName = reader["TrainingShortName"] != DBNull.Value ? reader["TrainingShortName"].ToString() : "",
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : "",
                                IsTrainingMandatory = reader["IsTrainingMandatory"] != DBNull.Value ? Convert.ToBoolean(reader["IsTrainingMandatory"].ToString()) : false,
                                MS_Designation_Id = reader["MS_Designation_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Designation_Id"].ToString()) : 0,
                                Designation = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : null
                            };
                            trainings.Add(training);
                        }
                    }
                }
            }

            return trainings;
        }

        public static int UpsertTrainingSetup(TrainingInfo trainingInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertTraining;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", trainingInfo.Id);
                    cmd.Parameters.AddWithValue("@CompId", trainingInfo.CompId);
                    cmd.Parameters.AddWithValue("@TrainingName", trainingInfo.TrainingName);
                    cmd.Parameters.AddWithValue("@TrainingShortName", trainingInfo.TrainingShortName);
                    cmd.Parameters.AddWithValue("@Description", trainingInfo.Description);
                    cmd.Parameters.AddWithValue("@IsTrainingMandatory", trainingInfo.IsTrainingMandatory);
                    cmd.Parameters.AddWithValue("@MS_Designation_Id", trainingInfo.MS_Designation_Id);

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
