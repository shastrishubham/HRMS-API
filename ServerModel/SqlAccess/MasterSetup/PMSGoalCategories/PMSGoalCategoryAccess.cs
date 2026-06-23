using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.PMSGoalCategories
{
    public class PMSGoalCategoryAccess
    {
        public static List<PMSGoalCategory> GetGoalCategoriesByCompId(Guid compId)
        {
            List<PMSGoalCategory> goalCategories = new List<PMSGoalCategory>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetGoalCategoriesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var goalCat = new PMSGoalCategory
                            {
                                CategoryId = Convert.ToInt32(reader["CategoryId"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                CategoryName = reader["CategoryName"].ToString()
                            };

                            goalCategories.Add(goalCat);
                        }
                    }
                }
            }

            return goalCategories;
        }

        public static PMSGoalCategory GetGoalCategoryById(int categoryId)
        {
            PMSGoalCategory goalCategory = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetGoalCategoriesById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", categoryId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            goalCategory = new PMSGoalCategory
                            {
                                CategoryId = Convert.ToInt32(reader["CategoryId"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                CategoryName = reader["CategoryName"].ToString()
                            };

                        }
                    }
                }
            }

            return goalCategory;
        }

        public static int UpsertPMSGoalCategory(PMSGoalCategory category)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertPMSGoalCategory;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                    cmd.Parameters.AddWithValue("@MachineId", category.MachineId == null ? "" : category.MachineId);
                    cmd.Parameters.AddWithValue("@MachineIp", category.MachineIp == null ? "" : category.MachineIp);
                    cmd.Parameters.AddWithValue("@CompId", category.CompId);
                    cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    cmd.Parameters.AddWithValue("@CreatedBy", category.CreatedBy);
                    cmd.Parameters.AddWithValue("@ModifiedBy", category.ModifiedBy);
                    cmd.Parameters.AddWithValue("@Active", category.Active);

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
