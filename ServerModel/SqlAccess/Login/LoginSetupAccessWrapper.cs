using ServerModel.Model.Login;

namespace ServerModel.SqlAccess.Login
{
    public class LoginSetupAccessWrapper : ILoginSetupAccess
    {
        public int UpsertLogInLogout(LogInOut logInOutdetail)
        {
            return LoginSetupAccess.UpsertLogInLogout(logInOutdetail);
        }
    }
}
