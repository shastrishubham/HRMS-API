using ServerModel.Model.Login;

namespace ServerModel.SqlAccess.Login
{
    public interface ILoginSetupAccess
    {
        int UpsertLogInLogout(LogInOut logInOutdetail);
    }
}
