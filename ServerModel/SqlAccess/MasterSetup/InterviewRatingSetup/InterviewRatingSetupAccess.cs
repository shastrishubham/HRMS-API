using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.MasterSetup.InterviewRatingSetup
{
    public static class InterviewRatingSetupAccess
    {
        public static List<InterviewRatingInfo> GetInterviewRating(Guid compId)
        {
            List<InterviewRatingInfo> interviewRatings = new List<InterviewRatingInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetInterviewRating;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var interviewRate = new InterviewRatingInfo
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                InterviewRate = reader["InterviewRate"].ToString(),
                                Active = Convert.ToBoolean(reader["Active"].ToString())
                            };
                            interviewRatings.Add(interviewRate);
                        }
                    }
                }
            }

            return interviewRatings;
        }

        public static int UpsertInterviewRating(InterviewRatingInfo interviewRatingInfo)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertInterviewRating;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", interviewRatingInfo.Id);
                   cmd.Parameters.AddWithValue("@CompId", interviewRatingInfo.CompId);
                    cmd.Parameters.AddWithValue("@InterviewRate", interviewRatingInfo.InterviewRate);
                    cmd.Parameters.AddWithValue("@Active", interviewRatingInfo.Active);

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
