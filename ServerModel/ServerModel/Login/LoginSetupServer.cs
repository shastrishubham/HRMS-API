using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model.Login;
using ServerModel.Repository;
using ServerModel.SqlAccess.Login;
using System;
using System.Linq;

namespace ServerModel.ServerModel.Login
{
    public class LoginSetupServer
    {
        private IRespository<MS_LogInOut> respository = null;

        public LoginSetupServer()
        {
            this.respository = new Repository<MS_LogInOut>();
        }

        #region Properties Interface

        public static ILoginSetupAccess mLoginSetupAccessT = new LoginSetupAccessWrapper();

        #endregion

        public static int UpsertLogInLogout(LogInOut logInOutDetail)
        {
            return mLoginSetupAccessT.UpsertLogInLogout(logInOutDetail);
        }

        public DateTime? GetFirstLogInDateById(string oAuthUserId)
        {
            var logInTime = this.respository.GetAll().Where(a => a.OAuthUserId.Equals(oAuthUserId, StringComparison.CurrentCultureIgnoreCase)).Min(a => a.LoginDateTime);
            return logInTime;
        }
    }
}
