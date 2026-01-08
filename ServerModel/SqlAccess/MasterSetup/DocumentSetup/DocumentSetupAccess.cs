using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.MasterSetup.DocumentSetup
{
    public class DocumentSetupAccess
    {
        public static List<DocumentUploadInfo> GetMasterDocumentsByCompId(Guid compId)
        {
            List<DocumentUploadInfo> documents = new List<DocumentUploadInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetMastersDocsTemplates;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var document = new DocumentUploadInfo
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"].ToString()) : 0,
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                DocName = reader["DocName"] != DBNull.Value ? reader["DocName"].ToString() : null,
                                DocPath = reader["DocPath"] != DBNull.Value ? reader["DocPath"].ToString() : null
                            };
                            documents.Add(document);
                        }
                    }
                }
            }

            return documents;
        }

        public static DocumentUploadInfo GetMasterDocumentById(int documentId)
        {
            DocumentUploadInfo document = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetMastersDocTemplateById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@documentId", documentId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            document = new DocumentUploadInfo
                            {
                                Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"].ToString()) : 0,
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                DocName = reader["DocName"] != DBNull.Value ? reader["DocName"].ToString() : null,
                                DocPath = reader["DocPath"] != DBNull.Value ? reader["DocPath"].ToString() : null
                            };
                        }
                    }
                }
            }

            return document;
        }

        public static int UpsertDocument(DocumentUploadInfo documentUpload)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertMasterDocTemplate;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", documentUpload.Id);
                    cmd.Parameters.AddWithValue("@CompId", documentUpload.CompId);
                    cmd.Parameters.AddWithValue("@DocName", documentUpload.DocName);
                    cmd.Parameters.AddWithValue("@DocPath", documentUpload.DocPath);

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
