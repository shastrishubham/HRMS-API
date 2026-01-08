using ServerModel.Data;
using ServerModel.Model.Base;
using ServerModel.Model.Recruitment;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.Recruitment.Interviews
{
    public static class InterviewSetupAccess
    {
        public static List<InterviewPortalInformation> GetScheduleInterviewsByCompId(Guid compId, InterviewStatusTypes interviewStatus = InterviewStatusTypes.None)
        {
            List<InterviewPortalInformation> scheduleInterviews = new List<InterviewPortalInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Recruitment.Sql.GetScheduleInterviewsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    if (interviewStatus != InterviewStatusTypes.None)
                    {
                        command.Parameters.AddWithValue("@compId", compId);
                        command.Parameters.AddWithValue("@InterviewStatus", interviewStatus);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@compId", compId);
                        command.Parameters.AddWithValue("@InterviewStatus", DBNull.Value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empSalarySetup = new InterviewPortalInformation
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.MinValue,
                                Req_JbVacancy_Id = reader["Req_JbVacancy_Id"] != DBNull.Value ? Guid.Parse(reader["Req_JbVacancy_Id"].ToString()) : Guid.Empty,
                                HiringManager_Id = reader["HiringManager_Id"] != DBNull.Value ? Guid.Parse(reader["HiringManager_Id"].ToString()) : Guid.Empty,
                                HiringManagerName = reader["HiringManagerName"] != DBNull.Value ? reader["HiringManagerName"].ToString() : "",
                                MS_Designation_Id = reader["MS_Designation_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Designation_Id"].ToString()) : 0,
                                DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : "",
                                Req_JbForm_Id = reader["Req_JbForm_Id"] != DBNull.Value ? Guid.Parse(reader["Req_JbForm_Id"].ToString()) : Guid.Empty,
                                CandidateName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                CandidateEmail = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                                CandidateContact = reader["ContactNo"] != DBNull.Value ? reader["ContactNo"].ToString() : "",
                                YearOfExp = reader["YearOfExp"] != DBNull.Value ? Convert.ToDecimal(reader["YearOfExp"]) : 0,
                                Resume = reader["Resume"] != DBNull.Value ? reader["Resume"].ToString() : "",
                                InterviewDateTime = reader["InterviewDateTime"] != DBNull.Value ? Convert.ToDateTime(reader["InterviewDateTime"]) : DateTime.MinValue,
                                Method = reader["Method"] != DBNull.Value ? Convert.ToInt32(reader["Method"]) : 0,
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                InterviewTakenEmp = reader["InterviewTakenEmp"] != DBNull.Value ? reader["InterviewTakenEmp"].ToString() : "",
                                InterviewStatus = reader["InterviewStatus"] != DBNull.Value ? Convert.ToInt32(reader["InterviewStatus"].ToString()) : 0,
                                Comments = reader["Comments"] != DBNull.Value ? reader["Comments"].ToString() : "",
                            };

                            scheduleInterviews.Add(empSalarySetup);
                        }
                    }
                }
            }

            return scheduleInterviews;
        }

        public static List<InterviewFeedback> GetInterviewFeedbacksByCompIdAndRating(Guid compId, int interviewRateId = 0)
        {
            List<InterviewFeedback> scheduleInterviews = new List<InterviewFeedback>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Recruitment.Sql.GetInterviewFeedbacksByCompIdAndRating;
                using (var command = new SqlCommand(sql, connection))
                {
                    if (interviewRateId != 0)
                    {
                        command.Parameters.AddWithValue("@compId", compId);
                        command.Parameters.AddWithValue("@interviewRateId", interviewRateId);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@compId", compId);
                        command.Parameters.AddWithValue("@interviewRateId", DBNull.Value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empSalarySetup = new InterviewFeedback
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                Req_InterviewSch_Id = reader["Req_InterviewSch_Id"] != DBNull.Value ? Guid.Parse(reader["Req_InterviewSch_Id"].ToString()) : Guid.Empty,
                                FeedBackGivenEmpId = reader["FeedBackGivenEmpId"] != DBNull.Value ? Guid.Parse(reader["FeedBackGivenEmpId"].ToString()) : Guid.Empty,
                                FeedBackGivenEmpName = reader["FeedBackGivenEmpName"] != DBNull.Value ? reader["FeedBackGivenEmpName"].ToString() : "",
                                InterviewDateTime = reader["InterviewDateTime"] != DBNull.Value ? Convert.ToDateTime(reader["InterviewDateTime"].ToString()) : DateTime.MinValue,
                                InterviewerComment = reader["InterviewerComment"] != DBNull.Value ? reader["InterviewerComment"].ToString() : "",
                                Method = reader["Method"] != DBNull.Value ? Convert.ToInt32(reader["Method"].ToString()) : 0,
                                InterviewStatus = reader["InterviewStatus"] != DBNull.Value ? Convert.ToInt32(reader["InterviewStatus"].ToString()) : 0,
                                InterviewerId = reader["InterviewerId"] != DBNull.Value ? Guid.Parse(reader["InterviewerId"].ToString()) : Guid.Empty,
                                InterviewerName = reader["InterviewerName"] != DBNull.Value ? reader["InterviewerName"].ToString() : "",
                                CandidateName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                CandidateContact = reader["ContactNo"] != DBNull.Value ? reader["ContactNo"].ToString() : "",
                                CandidateEmail = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "",
                                YearOfExp = reader["YearOfExp"] != DBNull.Value ? Convert.ToDecimal(reader["YearOfExp"]) : 0,
                                HiringManager_Id = reader["HiringManagerId"] != DBNull.Value ? Guid.Parse(reader["HiringManagerId"].ToString()) : Guid.Empty,
                                HiringManagerName = reader["HiringManager"] != DBNull.Value ? reader["HiringManager"].ToString() : "",
                                MS_Designation_Id = reader["MS_Designation_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Designation_Id"].ToString()) : 0,
                                DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : "",
                                MS_InterviewRate_Id = reader["MS_InterviewRate_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_InterviewRate_Id"].ToString()) : 0,
                                InterviewRate = reader["InterviewRate"] != DBNull.Value ? reader["InterviewRate"].ToString() : "",
                                Feedback = reader["Feedback"] != DBNull.Value ? reader["Feedback"].ToString() : "",
                            };

                            scheduleInterviews.Add(empSalarySetup);
                        }
                    }
                }
            }

            return scheduleInterviews;
        }
    }
}
