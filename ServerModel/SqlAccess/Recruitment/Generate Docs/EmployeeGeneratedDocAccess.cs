using ServerModel.Data;
using ServerModel.Model.Recruitment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;

namespace ServerModel.SqlAccess.Recruitment.Generate_Docs
{
    public class EmployeeGeneratedDocAccess
    {
        public static int UpsertEmployeeGeneratedOfferLetter(EmployeeGeneratedDocument employeeGeneratedDocument)
        {
            try
            {
                string _connectionString = DbContext.ConnectionString;

                using (var con = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("dbo.UpsertGenDocsWithOfferSL", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Build the payload object
                    var payload = new
                    {
                        EMP_GenDocs = new
                        {
                            Id = employeeGeneratedDocument.Id,
                            FormDate = employeeGeneratedDocument.FormDate,
                            MachineId = employeeGeneratedDocument.MachineId,
                            MachineIp = employeeGeneratedDocument.MachineIp,
                            CompId = employeeGeneratedDocument.CompId,
                            Req_JbForm_Id = employeeGeneratedDocument.Req_JbForm_Id,
                            CTC = employeeGeneratedDocument.CTC,
                            Status = employeeGeneratedDocument.Status,
                            CreatedBy = employeeGeneratedDocument.CreatedBy,
                            ProbationPeriod = employeeGeneratedDocument.ProbationPeriod,
                            NoticePeriod = employeeGeneratedDocument.NoticePeriod,
                            DateOfJoining = employeeGeneratedDocument.DateOfJoining,
                            MS_Doc_Id = employeeGeneratedDocument.MS_Doc_Id,
                            DocPath = employeeGeneratedDocument.DocPath,
                            DocCreatedOn = employeeGeneratedDocument.DocCreatedOn,
                            DocModifiedOn = employeeGeneratedDocument.DocModifiedOn,
                            DocExperiesOn = employeeGeneratedDocument.DocExperiesOn,
                            Ammend = employeeGeneratedDocument.Ammend
                        },
                        EMP_OfferSL = employeeGeneratedDocument.SalaryComponents.Select(sc => new
                        {
                            MS_SLHeads_Id = sc.MS_SLHeads_Id,
                            Amount = sc.Amount,
                            IsAmmend = sc.IsAmmend
                        }).ToList()
                    };

                    // Serialize to JSON
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

                    // Add JSON parameter
                    cmd.Parameters.AddWithValue("@json", json);

                    con.Open();

                    // Use ExecuteScalar to get the inserted/updated Id from SP
                    object res = cmd.ExecuteScalar();
                    int newOfferLetterId = Convert.ToInt32(res);

                    return newOfferLetterId;
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return 0;
            }
        }

        public static bool UpdateGeneratedDocStatusById(int documentId, string status)
        {
            try
            {
                string sql = Recruitment.Sql.UpdateGeneratedDocStatusById;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@docId", documentId);

                    object returnObj = cmd.ExecuteScalar();

                    bool result = returnObj != null && Convert.ToBoolean(returnObj);

                    return result;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static EmployeeGeneratedDocument GetEmployeeGeneratedDocDetail(Guid compId, Guid candidateId)
        {
            EmployeeGeneratedDocument empGeneratedDoc = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Recruitment.Sql.GetCandidateDetailsForGenerateDocById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@candidateId", candidateId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empGeneratedDoc = new EmployeeGeneratedDocument
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"].ToString()) : 0,
                                Req_JbForm_Id = reader["Req_JbForm_Id"] != DBNull.Value ? Guid.Parse(reader["Req_JbForm_Id"].ToString()) : Guid.Empty,
                                CompanyName = reader["CompanyName"] != DBNull.Value ? reader["CompanyName"].ToString() : string.Empty,
                                CompanyAddress = reader["CompanyAddress"] != DBNull.Value ? reader["CompanyAddress"].ToString() : string.Empty,
                                CompCity = reader["CompCity"] != DBNull.Value ? reader["CompCity"].ToString() : string.Empty,
                                CompState = reader["CompState"] != DBNull.Value ? reader["CompState"].ToString() : string.Empty,
                                CompCountry = reader["CompCountry"] != DBNull.Value ? reader["CompCountry"].ToString() : string.Empty,
                                Pincode = reader["Pincode"] != DBNull.Value ? reader["Pincode"].ToString() : string.Empty,
                                CandidateFullName = reader["CandidateFullName"] != DBNull.Value ? reader["CandidateFullName"].ToString() : string.Empty,
                                CandidateAddress = reader["CandidateAddress"] != DBNull.Value ? reader["CandidateAddress"].ToString() : string.Empty,
                                JobTitle = reader["Job Title"] != DBNull.Value ? reader["Job Title"].ToString() : string.Empty,
                                Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : string.Empty,
                                ReportingManager = reader["Reporting Manager"] != DBNull.Value ? reader["Reporting Manager"].ToString() : string.Empty,
                                CTC = reader["CTC"] != DBNull.Value ? Convert.ToDecimal(reader["CTC"].ToString()) : 0,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                                ProbationPeriod = reader["ProbationPeriod"] != DBNull.Value ? Convert.ToInt32(reader["ProbationPeriod"].ToString()) : 6,
                                NoticePeriod = reader["NoticePeriod"] != DBNull.Value ? Convert.ToInt32(reader["NoticePeriod"].ToString()) : 3,
                                DateOfJoining = reader["DateOfJoining"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfJoining"].ToString()) : DateTime.MinValue,
                                DocCreatedOn = reader["DocCreatedOn"] != DBNull.Value ? Convert.ToDateTime(reader["DocCreatedOn"].ToString()) : DateTime.MinValue,
                                DocExperiesOn = reader["DocExperiesOn"] != DBNull.Value ? Convert.ToDateTime(reader["DocExperiesOn"].ToString()) : DateTime.MinValue,
                                DocPath = reader["DocPath"] != DBNull.Value ? reader["DocPath"].ToString() : string.Empty,
                                MS_Doc_Id = reader["MS_Doc_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Doc_Id"].ToString()) : 0
                            };
                        }
                    }
                }
                return empGeneratedDoc;
            }
        }

        public static List<EmployeeOfferLetterSalaryInfo> GetEmployeeOfferSalaryInfo(Guid candidateId, int documentId)
        {
            List<EmployeeOfferLetterSalaryInfo> empOfferSalaryInfos = new List<EmployeeOfferLetterSalaryInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Recruitment.Sql.GetEmpOfferSalaryByDocId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@candidateId", candidateId);
                    command.Parameters.AddWithValue("@docId", documentId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeeOfferLetterSalaryInfo empOffersalary = new EmployeeOfferLetterSalaryInfo
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"].ToString()) : 0,
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                MS_SLHeads_Id = reader["MS_SLHeads_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHeads_Id"].ToString()) : 0,
                                IsEarningComponent = reader["IsEarningComponent"] != DBNull.Value ? Convert.ToBoolean(reader["IsEarningComponent"].ToString()) : false,
                                SLHeadName = reader["SalaryHeadName"] != DBNull.Value ? reader["SalaryHeadName"].ToString() : string.Empty,
                                Amount = reader["AnnualAmount"] != DBNull.Value ? Convert.ToDecimal(reader["AnnualAmount"].ToString()) : 0,
                            };
                            empOfferSalaryInfos.Add(empOffersalary);
                        }
                    }
                }
                return empOfferSalaryInfos;
            }
        }

        public static List<EmployeeGeneratedDocument> GetGeneratedDocumentsByCompId(Guid compId)
        {
            List<EmployeeGeneratedDocument> generatedDocuments = new List<EmployeeGeneratedDocument>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Recruitment.Sql.GetGeneratedDocumentsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeeGeneratedDocument generatedDocument = GetEmpGeneratedDocObject(reader);
                            generatedDocuments.Add(generatedDocument);
                        }
                    }
                }
                return generatedDocuments;
            }
        }

        public static EmployeeGeneratedDocument GetGeneratedDocById(int documentId)
        {
            EmployeeGeneratedDocument generatedDocument = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Recruitment.Sql.GetGeneratedDocById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@docId", documentId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            generatedDocument = GetEmpGeneratedDocObject(reader);
                        }
                    }
                }
                return generatedDocument;
            }
        }

        public static List<EmployeeGeneratedDocument> GetDocGeneratedCandidatesByCompId(Guid compId)
        {
            List<EmployeeGeneratedDocument> generatedDocuments = new List<EmployeeGeneratedDocument>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Recruitment.Sql.GetDocGeneratedCandidatesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeeGeneratedDocument generatedDocument = new EmployeeGeneratedDocument
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"].ToString()) : 0,
                                Req_JbForm_Id = reader["Req_JbForm_Id"] != DBNull.Value ? Guid.Parse(reader["Req_JbForm_Id"].ToString()) : Guid.Empty,
                                CandidateFullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                                DocCreatedOn = reader["DocCreatedOn"] != DBNull.Value ? Convert.ToDateTime(reader["DocCreatedOn"].ToString()) : DateTime.MinValue,
                                DateOfJoining = reader["DateOfJoining"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfJoining"].ToString()) : DateTime.MinValue,
                                DocExperiesOn = reader["DocExperiesOn"] != DBNull.Value ? Convert.ToDateTime(reader["DocExperiesOn"].ToString()) : DateTime.MinValue,
                                MS_Doc_Id  = reader["MS_Doc_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Doc_Id"].ToString()) : 0
                            };

                            generatedDocuments.Add(generatedDocument);
                        }
                    }
                }
                return generatedDocuments;
            }
        }

        public static List<EmployeeGeneratedDocument> GetConfirmedCandidatesByCompId(Guid compId, string status)
        {
            List<EmployeeGeneratedDocument> candidates = new List<EmployeeGeneratedDocument>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Recruitment.Sql.GetConfirmedCandidatesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@compId", compId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeeGeneratedDocument candidate = new EmployeeGeneratedDocument
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"].ToString()) : 0,
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                                CTC = reader["CTC"] != DBNull.Value ? Convert.ToDecimal(reader["CTC"].ToString()) : 0,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                                DateOfJoining = reader["DateOfJoining"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfJoining"].ToString()) : DateTime.MinValue,
                                DocExperiesOn = reader["DocExperiesOn"] != DBNull.Value ? Convert.ToDateTime(reader["DocExperiesOn"].ToString()) : DateTime.MinValue,
                                CandidateFullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                                Req_JbForm_Id = reader["Req_JbForm_Id"] != DBNull.Value ? Guid.Parse(reader["Req_JbForm_Id"].ToString()) : Guid.Empty,
                                Email = reader["Email"] != DBNull.Value ?reader["Email"].ToString() : string.Empty,
                                ContactNo = reader["ContactNo"] != DBNull.Value ? reader["ContactNo"].ToString() : string.Empty,
                                Location = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : string.Empty,
                                JobTitle = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : string.Empty,
                                ReportingManager = reader["ReportingManager"] != DBNull.Value ? reader["ReportingManager"].ToString() : string.Empty,
                                InterviewScheduledId = reader["InterviewScheduledId"] != DBNull.Value ? Guid.Parse(reader["InterviewScheduledId"].ToString()) : Guid.Empty,
                                InterviewStatusId = reader["InterviewStatus"] !=DBNull.Value ? Convert.ToInt32(reader["InterviewStatus"].ToString()) : 0
                            };

                            candidates.Add(candidate);
                        }
                    }
                }
                return candidates;
            }
        }

        private static EmployeeGeneratedDocument GetEmpGeneratedDocObject(SqlDataReader reader)
        {
            return new EmployeeGeneratedDocument
            {
                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"].ToString()) : 0,
                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : Guid.Empty,
                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                CTC = reader["CTC"] != DBNull.Value ? Convert.ToDecimal(reader["CTC"].ToString()) : 0,
                DateOfJoining = reader["DateOfJoining"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfJoining"].ToString()) : DateTime.MinValue,
                MS_Doc_Id = reader["MS_Doc_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Doc_Id"].ToString()) : 0,
                DocName = reader["DocName"] != DBNull.Value ? reader["DocName"].ToString() : string.Empty,
                DocPath = reader["DocPath"] != DBNull.Value ? reader["DocPath"].ToString() : string.Empty,
                CandidateFullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : string.Empty,
                ContactNo = reader["ContactNo"] != DBNull.Value ? reader["ContactNo"].ToString() : string.Empty,
                Location = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : string.Empty,
                JobTitle = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : string.Empty,
                ReportingManager = reader["HiringManager"] != DBNull.Value ? reader["HiringManager"].ToString() : string.Empty,
                InterviewerName = reader["InterviewerName"] != DBNull.Value ? reader["InterviewerName"].ToString() : string.Empty,
                CompanyName = reader["CompanyName"] != DBNull.Value ? reader["CompanyName"].ToString() : string.Empty,
                DocExperiesOn = reader["DocExperiesOn"] != DBNull.Value ? Convert.ToDateTime(reader["DocExperiesOn"].ToString()) : DateTime.MinValue,
                CompanyContact = reader["CompanyContact"] != DBNull.Value ? reader["CompanyContact"].ToString() : string.Empty,
                Website = reader["Website"] != DBNull.Value ? reader["Website"].ToString() : string.Empty,
                Req_JbForm_Id = reader["Req_JbForm_Id"] != DBNull.Value ? Guid.Parse(reader["Req_JbForm_Id"].ToString()) : Guid.Empty,
                ProbationPeriod = reader["ProbationPeriod"] != DBNull.Value ? Convert.ToInt32(reader["ProbationPeriod"].ToString()) : 0,
                NoticePeriod = reader["ProbationPeriod"] != DBNull.Value ? Convert.ToInt32(reader["ProbationPeriod"].ToString()) : 0,
            };
        }
    }
}
