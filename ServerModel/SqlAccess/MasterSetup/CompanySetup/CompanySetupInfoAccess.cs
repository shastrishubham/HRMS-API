using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.CompanySetup
{
    public class CompanySetupInfoAccess
    {
        public static List<Countries> GetCountries()
        {
            List<Countries> countries = new List<Countries>();

            //string strConString = @"Data Source=DESKTOP-8G6MFH8\MSSQLSERVER01;Initial Catalog=HRMS;Integrated Security=True";
            //string strConString = DbContext.ConnectionString;

            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetCountries;
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var country = new Countries
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CountryName = reader["CountryName"].ToString(),
                                CountryCode = reader["CountryCode"].ToString()
                            };
                            countries.Add(country);
                        }
                    }
                }
            }

            return countries;
        }

        public static List<States> GetStatesByCountryId(int countryId)
        {
            List<States> states = new List<States>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetStatesByCountryId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@countryId", countryId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var state = new States
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CountryName = reader["CountryName"].ToString(),
                                CountryCode = reader["CountryCode"].ToString(),
                                CountryId = Convert.ToInt32(reader["MS_Country_Id"].ToString()),
                                StateName = reader["StateName"].ToString(),
                                StateCode = reader["StateCode"].ToString()
                            };
                            states.Add(state);
                        }
                    }
                }
            }

            return states;
        }

        public static List<Cities> GetCitiesByStateId(int stateId)
        {
            List<Cities> cities = new List<Cities>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetCitiesByStateId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@stateId", stateId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var city = new Cities
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CountryId = Convert.ToInt32(reader["MS_Country_id"].ToString()),
                                CountryName = reader["CountryName"].ToString(),
                                CountryCode = reader["CountryCode"].ToString(),
                                StateId = Convert.ToInt32(reader["MS_States_Id"].ToString()),
                                StateName = reader["StateName"].ToString(),
                                StateCode = reader["StateCode"].ToString(),
                                CityName = reader["CityName"].ToString(),
                                CityCode = reader["CityCode"].ToString(),
                            };
                            cities.Add(city);
                        }
                    }
                }
            }

            return cities;
        }

        public static bool UploadCompanyDocument(CompanyDocument companyDocument)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertCompanyDocument;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", companyDocument.Id);
                    cmd.Parameters.AddWithValue("@CompId", companyDocument.CompId);
                    cmd.Parameters.AddWithValue("@DocumentName", companyDocument.DocumentName);
                    cmd.Parameters.AddWithValue("@DocumentPath", companyDocument.DocumentPath);
                    cmd.Parameters.AddWithValue("@Active", companyDocument.Active == null ? true : companyDocument.Active);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        int.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue > 0 ? true : false;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<CompanyDocument> GetCompDocsByCompId(Guid companyId)
        {
            List<CompanyDocument> companyDocuments = new List<CompanyDocument>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetCompDocsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var companyDocument = new CompanyDocument
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                DocumentName = reader["DocumentName"].ToString(),
                                DocumentPath = reader["DocumentPath"].ToString(),
                                Active = Convert.ToBoolean(reader["Active"].ToString()),
                            };

                            companyDocuments.Add(companyDocument);
                        }
                    }
                }
            }

            return companyDocuments;
        }

        public static bool UpsertCompanyPlanDetail(CompanyPlanDetail companyPlanDetail)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertCompanyPlanDetail;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", companyPlanDetail.Id);
                    cmd.Parameters.AddWithValue("@CompId", companyPlanDetail.CompId);
                    cmd.Parameters.AddWithValue("@MS_Module_Id", companyPlanDetail.MS_Module_Id);
                    cmd.Parameters.AddWithValue("@ActiveFrom", companyPlanDetail.ActiveFrom);
                    cmd.Parameters.AddWithValue("@ActiveTo", companyPlanDetail.ActiveTo);
                    cmd.Parameters.AddWithValue("@NosOfUser", companyPlanDetail.NosOfUser);
                    cmd.Parameters.AddWithValue("@Active", companyPlanDetail.Active);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        int.TryParse(returnObj.ToString(), out int returnValue);
                        return returnValue > 0 ? true : false;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<CompanyPlanDetail> GetCompanyPlanDetailsByCompId(Guid companyId)
        {
            List<CompanyPlanDetail> companyPlanDetails = new List<CompanyPlanDetail>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetCompPlanDetailsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var companyPlanDetail = new CompanyPlanDetail
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                MS_Module_Id = reader["MS_Module_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Module_Id"].ToString()) : 0,
                                ActiveFrom = reader["ActiveFrom"] != DBNull.Value ? Convert.ToDateTime(reader["ActiveFrom"].ToString()) : DateTime.MinValue,
                                ActiveTo = reader["ActiveTo"] != DBNull.Value ? Convert.ToDateTime(reader["ActiveTo"].ToString()) : DateTime.MaxValue,
                                NosOfUser = reader["NosOfUser"] != DBNull.Value ? Convert.ToInt32(reader["NosOfUser"].ToString()) : 0,
                                Active = reader["Active"] != DBNull.Value ? Convert.ToBoolean(reader["Active"].ToString()) : true,
                            };

                            companyPlanDetails.Add(companyPlanDetail);
                        }
                    }
                }
            }

            return companyPlanDetails;
        }
    }

    public static class CompanySetupInfoAccess<T> where T : CompanyRegistration
    {
        public static Guid UpsertCompanyRegistration(CompanyRegistration companyRegistration)
        {
            try
            {
                string sql = MasterSetupSqls.CompanyRegistration;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", companyRegistration.Id);
                    cmd.Parameters.AddWithValue("@formdate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@MachineIp", companyRegistration.MachineIp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MachineId", companyRegistration.MachineId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CompanyName", companyRegistration.CompanyName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CompanyDomain", companyRegistration.CompanyDomain ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CompanyAddress", companyRegistration.CompanyAddress ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MS_Country_Id", companyRegistration.MS_Country_Id);
                    cmd.Parameters.AddWithValue("@MS_State_Id", companyRegistration.MS_State_Id);
                    cmd.Parameters.AddWithValue("@MS_City_Id", companyRegistration.MS_City_Id);
                    cmd.Parameters.AddWithValue("@Pincode", companyRegistration.Pincode ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@PFNo", companyRegistration.PFNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TANNo", companyRegistration.TANNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PANNo", companyRegistration.PANNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ESINo", companyRegistration.ESINo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LINNo", companyRegistration.LINNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GSTNo", companyRegistration.GSTNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FinancialYearFrom", companyRegistration.FinancialYearFrom );
                    cmd.Parameters.AddWithValue("@FinancialYearTo", companyRegistration.FinancialYearTo);
                    cmd.Parameters.AddWithValue("@RegCertNo", companyRegistration.RegCertNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IndustryType_Id", companyRegistration.IndustryType_Id);
                    cmd.Parameters.AddWithValue("@Website", companyRegistration.Website ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Timezone_Id", companyRegistration.Timezone_Id);
                    cmd.Parameters.AddWithValue("@ReportingCnt", companyRegistration.ReportingCnt ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReportingCntMail", companyRegistration.ReportingCntMail ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReportingCntDesg", companyRegistration.ReportingCntDesg ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", companyRegistration.Active ? companyRegistration.Active : false);

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

        public static List<CompanyRegistration> GetCompanyRegistrationDetailsById(Guid companyId)
        {
            List<CompanyRegistration> companyRegistration = new List<CompanyRegistration>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetCompanyRegistrationDetailsById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var company = new CompanyRegistration
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                CompanyName = reader["CompanyName"].ToString(),
                                CompanyDomain = reader["CompanyDomain"].ToString(),
                                CompanyAddress = reader["CompanyAddress"].ToString(),
                                MS_Country_Id = Convert.ToInt32(reader["MS_Country_Id"].ToString()),
                                MS_State_Id = Convert.ToInt32(reader["MS_State_Id"].ToString()),
                                MS_City_Id = Convert.ToInt32(reader["MS_City_Id"].ToString()),
                                Pincode = reader["Pincode"].ToString(),
                                PFNo = reader["PFNo"].ToString(),
                                TANNo = reader["TANNo"].ToString(),
                                PANNo = reader["PANNo"].ToString(),
                                ESINo = reader["ESINo"].ToString(),
                                LINNo = reader["LINNo"].ToString(),
                                GSTNo = reader["GSTNo"].ToString(),
                                FinancialYearFrom = Convert.ToDateTime(reader["FinancialYearFrom"].ToString()).Date,
                                FinancialYearTo = Convert.ToDateTime(reader["FinancialYearTo"].ToString()).Date,
                                RegCertNo = reader["RegCertNo"].ToString(),
                                IndustryType_Id = Convert.ToInt32(reader["IndustryType_Id"].ToString()),
                                Website = reader["Website"].ToString(),
                                Timezone_Id = Convert.ToInt32(reader["Timezone_Id"].ToString()),
                                ReportingCnt = reader["ReportingCnt"].ToString(),
                                ReportingCntMail = reader["ReportingCntMail"].ToString(),
                                ReportingCntDesg = reader["ReportingCntDesg"].ToString(),
                            };

                            companyRegistration.Add(company);
                        }
                    }
                }
            }

            return companyRegistration;
        }

        public static List<CompanyRegistration> GetCompanyRegistrationDetails()
        {
            try
            {
                List<CompanyRegistration> companyRegistrations = new List<CompanyRegistration>();
                string strConString = DbContext.ConnectionString;

                using (var connection = new SqlConnection(strConString))
                {
                    connection.Open();
                    string sql = MasterSetupSqls.GetCompanyRegistrationDetails;
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var companyRegistration = new CompanyRegistration
                                {
                                    Id = Guid.Parse(reader["Id"].ToString()),
                                    CompanyName = reader["CompanyName"].ToString(),
                                    CompanyDomain = reader["CompanyDomain"].ToString(),
                                    CompanyAddress = reader["CompanyAddress"].ToString(),
                                    MS_Country_Id = Convert.ToInt32(reader["MS_Country_Id"].ToString()),
                                    MS_State_Id = Convert.ToInt32(reader["MS_State_Id"].ToString()),
                                    MS_City_Id = Convert.ToInt32(reader["MS_City_Id"].ToString()),
                                    Pincode = reader["Pincode"].ToString(),
                                    PFNo = reader["PFNo"].ToString(),
                                    TANNo = reader["TANNo"].ToString(),
                                    PANNo = reader["PANNo"].ToString(),
                                    ESINo = reader["ESINo"].ToString(),
                                    LINNo = reader["LINNo"].ToString(),
                                    GSTNo = reader["GSTNo"].ToString(),
                                    FinancialYearFrom = Convert.ToDateTime(reader["FinancialYearFrom"].ToString()).Date,
                                    FinancialYearTo = Convert.ToDateTime(reader["FinancialYearTo"].ToString()).Date,
                                    RegCertNo = reader["RegCertNo"].ToString(),
                                    IndustryType_Id = Convert.ToInt32(reader["IndustryType_Id"].ToString()),
                                    Website = reader["Website"].ToString(),
                                    Timezone_Id = Convert.ToInt32(reader["Timezone_Id"].ToString()),
                                    ReportingCnt = reader["ReportingCnt"].ToString(),
                                    ReportingCntMail = reader["ReportingCntMail"].ToString(),
                                    ReportingCntDesg = reader["ReportingCntDesg"].ToString(),
                                    Active = Convert.ToBoolean(reader["Active"].ToString())
                                };
                                companyRegistrations.Add(companyRegistration);
                            }
                        }
                    }
                }

                return companyRegistrations;
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}
