using ServerModel.Data;
using ServerModel.Model.Login;
using System;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.Login
{
    public class LoginSetupAccess
    {
        public static int UpsertLogInLogout(LogInOut logInOutdetail)
        {
            try
            {
                string sql = LoginSql.UpsertLogInOutUser;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", logInOutdetail.Id);
                    cmd.Parameters.AddWithValue("@OAuthUserId", logInOutdetail.OAuthUserId);
                    cmd.Parameters.AddWithValue("@UserId", logInOutdetail.UserId);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        int id = int.Parse(returnObj.ToString());
                        return id;
                    }

                    return 0;

                    // var ddd = employeeBankInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
