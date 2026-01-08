using ServerModel.Data;
using ServerModel.Model.HelpDesk;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.HelpDesk
{
    public class HelpDeskSetupAccess
    {
        public static List<HelpDeskTicketInformation> GetHelpDeskTicketInformationByCompId(Guid compId)
        {
            List<HelpDeskTicketInformation> helpDeskTickets = new List<HelpDeskTicketInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = HelpDeskSqls.GetHelpDeskTicketInformationByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HelpDeskTicketInformation helpDeskTicket = GetHelpDeskTicketInformationObject(reader);
                            helpDeskTickets.Add(helpDeskTicket);
                        }
                    }
                }
            }

            return helpDeskTickets;
        }

        public static List<HelpDeskTckReplies> GetHelpDeskTckRepliesByTicketId(int ticketId)
        {
            List<HelpDeskTckReplies> helpDeskTicketReplies = new List<HelpDeskTckReplies>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = HelpDeskSqls.GetHelpDeskTckRepliesByTicketId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ticketId", ticketId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var helpDeskTicketReply = new HelpDeskTckReplies
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                HR_HelpDesk_Id = reader["HR_HelpDesk_Id"] != DBNull.Value ? Convert.ToInt32(reader["HR_HelpDesk_Id"].ToString()) : 0,
                                RepliedByEmpId = reader["RepliedByEmpId"] != DBNull.Value ? Guid.Parse(reader["RepliedByEmpId"].ToString()) : Guid.Empty,
                                RepliesEmployeeName = reader["RepliesEmployeeName"] != DBNull.Value ? reader["RepliesEmployeeName"].ToString() : string.Empty,
                                Message = reader["Message"] != DBNull.Value ? reader["Message"].ToString() : string.Empty,
                                ReplyDate = reader["ReplyDate"] != DBNull.Value ? Convert.ToDateTime(reader["ReplyDate"]) : (DateTime?)null
                          
                            };
                            helpDeskTicketReplies.Add(helpDeskTicketReply);
                        }
                    }
                }
            }

            return helpDeskTicketReplies;
        }

        private static HelpDeskTicketInformation GetHelpDeskTicketInformationObject(SqlDataReader reader)
        {
            return new HelpDeskTicketInformation
            {
                Id = Convert.ToInt32(reader["Id"].ToString()),
                CompId = Guid.Parse(reader["CompId"].ToString()),
                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                EmployeeName = reader["EmployeeName"] != DBNull.Value ? reader["EmployeeName"].ToString() : string.Empty,
                MS_TicketCat_Id = reader["MS_TicketCat_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_TicketCat_Id"].ToString()) : 0,
                TicketCategory = reader["TicketCategory"] != DBNull.Value ? reader["TicketCategory"].ToString() : string.Empty,
                TicketName = reader["TicketName"] != DBNull.Value ? reader["TicketName"].ToString() : string.Empty,
                Subject = reader["Subject"] != DBNull.Value ? reader["Subject"].ToString() : string.Empty,
                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                Severity = reader["Severity"] != DBNull.Value ? Convert.ToInt32(reader["Severity"].ToString()) : 0,
                AttachmentPath = reader["AttachmentPath"] != DBNull.Value ? reader["AttachmentPath"].ToString() : string.Empty,
                AssignEmpId = reader["AssignEmpId"] != DBNull.Value ? Guid.Parse(reader["AssignEmpId"].ToString()) : Guid.Empty,
                AssignEmployeeName = reader["AssignEmployeeName"] != DBNull.Value ? reader["AssignEmployeeName"].ToString() : string.Empty,
                TicketStatus = reader["TicketStatus"] != DBNull.Value ? reader["TicketStatus"].ToString() : "Open",
                TicketResolvedDt = reader["TicketResolvedDt"] != DBNull.Value ? Convert.ToDateTime(reader["TicketResolvedDt"]) : (DateTime?)null
            };
        }

        public static int UpsertHelpDeskTicketCreation(HelpDeskTicketInformation helpDeskTicket)
        {
            try
            {
                string sql = HelpDeskSqls.UpsertHelpDeskTicketCreation;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", helpDeskTicket.Id);
                    cmd.Parameters.AddWithValue("@CompId", helpDeskTicket.CompId);
                    cmd.Parameters.AddWithValue("@EMP_Info_Id", helpDeskTicket.EMP_Info_Id);
                    cmd.Parameters.AddWithValue("@MS_TicketCat_Id", helpDeskTicket.MS_TicketCat_Id);
                    cmd.Parameters.AddWithValue("@TicketName", helpDeskTicket.TicketName);
                    cmd.Parameters.AddWithValue("@Subject", (object)helpDeskTicket.Subject ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Description", (object)helpDeskTicket.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Severity", (object)helpDeskTicket.Severity ?? 0);
                    cmd.Parameters.AddWithValue("@AttachmentPath", (object)helpDeskTicket.AttachmentPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AssignEmpId", (object)helpDeskTicket.AssignEmpId ?? Guid.Empty);
                    cmd.Parameters.AddWithValue("@TicketStatus", (object)helpDeskTicket.TicketStatus ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TicketResolvedDt", (object)helpDeskTicket.TicketResolvedDt ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", (object)helpDeskTicket.Active != null ? helpDeskTicket.Active : true);

                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        int id = int.Parse(returnObj.ToString());
                        return id;
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static int UpsertHelpDeskTicketReply(HelpDeskTckReplies helpDeskTckReply)
        {
            try
            {
                string sql = HelpDeskSqls.UpsertHelpDeskTicketReplies;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = HelpDeskSqls.UpsertHelpDeskTicketReplies;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", helpDeskTckReply.Id);
                    cmd.Parameters.AddWithValue("@CompId", helpDeskTckReply.CompId);
                    cmd.Parameters.AddWithValue("@HR_HelpDesk_Id", helpDeskTckReply.HR_HelpDesk_Id);
                    cmd.Parameters.AddWithValue("@RepliedByEmpId", helpDeskTckReply.RepliedByEmpId);
                    cmd.Parameters.AddWithValue("@Message", helpDeskTckReply.Message);
                    cmd.Parameters.AddWithValue("@InterviewStatus", helpDeskTckReply.InterviewStatus);

                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        int id = int.Parse(returnObj.ToString());
                        return id;
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
