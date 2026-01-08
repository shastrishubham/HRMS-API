using ServerModel.Data;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.EmployeeSetup.AddressesInfo
{
    public class EmployeeAddressSetupAccess
    {
        public static List<EmployeeAddresses> GetEmployeeAddressesById(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public static EmployeeAddresses GetEmployeeAddressByAddressId(Guid addressId)
        {
            EmployeeAddresses employeeAddress = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = SqlAccess.EmployeeSetup.AddressesInfo.SqlEmpAddresses.GetEmployeeAddressByAddressId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@empAddressId", addressId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeAddress = new EmployeeAddresses
                            {
                                Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                AddressProofDoc = reader["AddressProofDoc"] != DBNull.Value ? reader["AddressProofDoc"].ToString() : null
                            };
                        }
                    }
                }
            }
            return employeeAddress;
        }

        public static bool UpsertEmployeeAddresses(List<EmployeeAddresses> empAddresses)
        {
            try
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(empAddresses);

                string sql = SqlAccess.EmployeeSetup.AddressesInfo.SqlEmpAddresses.UpsertEmployeeAddresses;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@json", jsonString);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static DataTable BuildAddressDataTable(List<EmployeeAddresses> addresses)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("FormDate", typeof(DateTime));
            dt.Columns.Add("MachineId", typeof(int));
            dt.Columns.Add("MachineIp", typeof(string));
            dt.Columns.Add("CompId", typeof(int));
            dt.Columns.Add("EMP_Info_Id", typeof(int));
            dt.Columns.Add("AddressTypeId", typeof(int));
            dt.Columns.Add("AddressLine1", typeof(string));
            dt.Columns.Add("AddressLine2", typeof(string));
            dt.Columns.Add("FlatNo", typeof(string));
            dt.Columns.Add("Premise", typeof(string));
            dt.Columns.Add("Road", typeof(string));
            dt.Columns.Add("Area", typeof(string));
            dt.Columns.Add("Town", typeof(string));
            dt.Columns.Add("MS_State_Id", typeof(int));
            dt.Columns.Add("MS_Country_Id", typeof(int));
            dt.Columns.Add("PinCode", typeof(string));
            dt.Columns.Add("FullAddress", typeof(string));
            dt.Columns.Add("IsSameAsPresentAddress", typeof(bool));
            dt.Columns.Add("Active", typeof(bool));

            foreach (var addr in addresses)
            {
                dt.Rows.Add(
                    addr.Id,
                    addr.FormDate,
                    addr.MachineId,
                    addr.MachineIp,
                    addr.CompId,
                    addr.EMP_Info_Id,
                    addr.AddressTypeId,
                    addr.AddressLine1,
                    addr.AddressLine2,
                    addr.FlatNo,
                    addr.Premise,
                    addr.Road,
                    addr.Area,
                    addr.Town,
                    addr.MS_State_Id,
                    addr.MS_Country_Id,
                    addr.PinCode,
                    addr.FullAddress,
                    addr.IsSameAsPresentAddress,
                    addr.Active
                );
            }

            return dt;
        }

    }
}
