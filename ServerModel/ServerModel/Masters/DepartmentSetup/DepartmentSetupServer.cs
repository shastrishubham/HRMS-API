using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.DepartmentSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.DepartmentSetup
{
    public class DepartmentSetupServer
    {
        #region Properties Interface

        public static IDepartmentSetupAccess mDepartmentSetupAccessT
            = new DepartmentSetupAccessWrapper();

        #endregion


        public static DepartmentRegistration GetDepartmentDetails(int locationId)
        {
            return mDepartmentSetupAccessT.GetDepartmentDetails(locationId);
        }

        public static List<DepartmentRegistration> GetDepartmentsByCompId(Guid companyId)
        {
            return mDepartmentSetupAccessT.GetDepartmentsByCompId(companyId);
        }

        public static int UpsertDepartmentSetup(DepartmentRegistration locationRegistration)
        {
            return mDepartmentSetupAccessT.UpsertDepartmentSetup(locationRegistration);
        }
    }
}
