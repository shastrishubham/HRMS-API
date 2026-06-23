using ServerModel.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.MasterSetup.PerformanceCycle
{
    public class PerformanceCycleAccess
    {
        public static Model.Masters.PerformanceCycle GetPerformanceCycleById(int cycleId)
        {
            Model.Masters.PerformanceCycle performanceCycle = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetPerformanceCycleById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", cycleId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            performanceCycle = new Model.Masters.PerformanceCycle
                            {
                                CycleId = Convert.ToInt32(reader["CycleId"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                StartDate = Convert.ToDateTime(reader["StartDate"].ToString()),
                                EndDate = Convert.ToDateTime(reader["EndDate"].ToString()),
                                ReviewType = reader["ReviewType"].ToString(),
                                Status = reader["Status"].ToString()
                            };

                        }
                    }
                }
            }

            return performanceCycle;
        }

        public static List<Model.Masters.PerformanceCycle> GetPerformanceCyclesByCompId(Guid compId)
        {
            List<Model.Masters.PerformanceCycle> performanceCycles = new List<Model.Masters.PerformanceCycle>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetPerformanceCyclesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var performanceCycle = new Model.Masters.PerformanceCycle
                            {
                                CycleId = Convert.ToInt32(reader["CycleId"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                StartDate = Convert.ToDateTime(reader["StartDate"].ToString()),
                                EndDate = Convert.ToDateTime(reader["EndDate"].ToString()),
                                ReviewType = reader["ReviewType"].ToString(),
                                Status = reader["Status"].ToString()
                            };

                            performanceCycles.Add(performanceCycle);
                        }
                    }
                }
            }

            return performanceCycles;
        }

        public static int UpsertPerformanceCycles(Model.Masters.PerformanceCycle performanceCycle)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertPerformanceCycles;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@CycleId", performanceCycle.CycleId);
                    cmd.Parameters.AddWithValue("@MachineId", performanceCycle.MachineId == null ? "" : performanceCycle.MachineId);
                    cmd.Parameters.AddWithValue("@MachineIp", performanceCycle.MachineIp == null ? "" : performanceCycle.MachineIp);
                    cmd.Parameters.AddWithValue("@CompId", performanceCycle.CompId);
                    cmd.Parameters.AddWithValue("@CycleName", performanceCycle.CycleName);
                    cmd.Parameters.AddWithValue("@StartDate", performanceCycle.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", performanceCycle.EndDate);
                    cmd.Parameters.AddWithValue("@ReviewType", performanceCycle.ReviewType);
                    cmd.Parameters.AddWithValue("@Status", performanceCycle.Status);
                    cmd.Parameters.AddWithValue("@CreatedBy", performanceCycle.CreatedBy);
                    cmd.Parameters.AddWithValue("@ModifiedBy", performanceCycle.ModifiedBy);
                    cmd.Parameters.AddWithValue("@Active", performanceCycle.Active);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Int32.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue;
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
