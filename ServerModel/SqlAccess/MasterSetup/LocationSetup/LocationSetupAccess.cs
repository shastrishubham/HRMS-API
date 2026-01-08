using ServerModel.Data;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.LocationSetup
{
    public static class LocationSetupAccess
    {
        public static LocationRegistration GetLocationDetails(int locationId)
        {
            LocationRegistration locationRegistration = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetLocationDetails;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", locationId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            locationRegistration = new LocationRegistration
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                BranchName = reader["BranchName"].ToString(),
                                MailAlias = reader["MailAlias"].ToString(),
                                TimeZone_Id = Convert.ToInt32(reader["TimeZone_Id"].ToString()),
                                BranchCode = reader["BranchCode"].ToString(),
                                BranchAddress = reader["BranchAddress"].ToString(),
                                CountryId = Convert.ToInt32(reader["CountryId"].ToString()),
                                StateId = Convert.ToInt32(reader["StateId"].ToString()),
                                CityId =  Convert.ToInt32(reader["CityId"].ToString()),
                                PostalCode = reader["PostalCode"].ToString(),
                                IsMainBranch = Convert.ToBoolean(reader["IsMainBranch"].ToString()),
                                BranchHead_Id = Guid.Parse(reader["BranchHead_Id"].ToString()),
                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"].ToString()),
                                CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                                ModifiedOn = Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                                ModifiedBy = Guid.Parse(reader["ModifiedBy"].ToString()),
                            };

                        }
                    }
                }
            }

            return locationRegistration;
        }

        public static List<LocationRegistration> GetLocationsByCompId(Guid companyId)
        {
            List<LocationRegistration> locations = new List<LocationRegistration>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = MasterSetupSqls.GetLocationDetailsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", companyId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var locationRegistration = new LocationRegistration
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                BranchName = reader["BranchName"].ToString(),
                                MailAlias = reader["MailAlias"].ToString(),
                                TimeZone_Id = Convert.ToInt32(reader["TimeZone_Id"].ToString()),
                                BranchCode = reader["BranchCode"].ToString(),
                                BranchAddress = reader["BranchAddress"].ToString(),
                                CountryId = Convert.ToInt32(reader["CountryId"].ToString()),
                                StateId = Convert.ToInt32(reader["StateId"].ToString()),
                                CityId = Convert.ToInt32(reader["CityId"].ToString()),
                                PostalCode = reader["PostalCode"].ToString(),
                                IsMainBranch = Convert.ToBoolean(reader["IsMainBranch"].ToString()),
                                BranchHead_Id = Guid.Parse(reader["BranchHead_Id"].ToString()),
                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"].ToString()),
                                CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                                ModifiedOn = Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                                ModifiedBy = Guid.Parse(reader["ModifiedBy"].ToString()),
                                Active = Convert.ToBoolean(reader["Active"].ToString())
                            };
                            locations.Add(locationRegistration);
                        }
                    }
                }
            }

            return locations;
        }

        public static int UpsertLocationSetup(LocationRegistration locationRegistration)
        {
            try
            {
                string sql = MasterSetupSqls.UpsertBranch;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", locationRegistration.Id);
                    cmd.Parameters.AddWithValue("@formdate", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@MachineIp", locationRegistration.MachineIp == null ? "" : locationRegistration.MachineIp);
                    cmd.Parameters.AddWithValue("@MachineId", locationRegistration.MachineId == null ? "" : locationRegistration.MachineId);
                    cmd.Parameters.AddWithValue("@CompId", locationRegistration.CompId);
                    cmd.Parameters.AddWithValue("@BranchName", locationRegistration.BranchName);
                    cmd.Parameters.AddWithValue("@MailAlias", locationRegistration.MailAlias);
                    cmd.Parameters.AddWithValue("@TimeZone_Id", locationRegistration.TimeZone_Id);
                    cmd.Parameters.AddWithValue("@BranchCode", locationRegistration.BranchCode);
                    cmd.Parameters.AddWithValue("@BranchAddress", locationRegistration.BranchAddress);
                    cmd.Parameters.AddWithValue("@CountryId", locationRegistration.CountryId);
                    cmd.Parameters.AddWithValue("@StateId", locationRegistration.StateId);
                    cmd.Parameters.AddWithValue("@CityId", locationRegistration.CityId);
                    cmd.Parameters.AddWithValue("@PostalCode", locationRegistration.PostalCode);
                    cmd.Parameters.AddWithValue("@IsMainBranch", locationRegistration.IsMainBranch);
                    cmd.Parameters.AddWithValue("@BranchHead_Id", locationRegistration.BranchHead_Id);
                    cmd.Parameters.AddWithValue("@CreatedOn", locationRegistration.CreatedOn == DateTime.MinValue ? DateTime.Now.Date : locationRegistration.CreatedOn.Date);
                    cmd.Parameters.AddWithValue("@CreatedBy", locationRegistration.CreatedBy);
                    cmd.Parameters.AddWithValue("@ModifiedOn", locationRegistration.ModifiedOn == DateTime.MinValue ? DateTime.Now.Date :
                        locationRegistration.ModifiedOn.Date);
                    cmd.Parameters.AddWithValue("@ModifiedBy", locationRegistration.ModifiedBy);
                    cmd.Parameters.AddWithValue("@Active", locationRegistration.Active);

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
