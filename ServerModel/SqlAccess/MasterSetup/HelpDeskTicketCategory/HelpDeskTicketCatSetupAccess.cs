using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.MasterSetup.HelpDeskTicketCategory
{
    public class HelpDeskTicketCatSetupAccess
    {
        public static List<TicketCategoryInformation> GetTicketCategoriesByCompId(Guid compId)
        {
            List<TicketCategoryInformation> ticketCategories = new List<TicketCategoryInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetTicketCategoriesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ticketCategory = new TicketCategoryInformation
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                TicketCategory = reader["TicketCategory"] != DBNull.Value ? reader["TicketCategory"].ToString() : string.Empty
                            };
                            ticketCategories.Add(ticketCategory);
                        }
                    }
                }
            }

            return ticketCategories;
        }

        public static int UpsertTicketCategory(TicketCategoryInformation ticketCategoryInformation)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertTicketCategory;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", ticketCategoryInformation.Id);
                    cmd.Parameters.AddWithValue("@CompId", ticketCategoryInformation.CompId);
                    cmd.Parameters.AddWithValue("@TicketCategory", ticketCategoryInformation.TicketCategory);

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
