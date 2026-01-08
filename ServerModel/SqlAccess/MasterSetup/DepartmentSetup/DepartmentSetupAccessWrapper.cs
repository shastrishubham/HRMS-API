using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.DepartmentSetup
{
    public class DepartmentSetupAccessWrapper : IDepartmentSetupAccess
    {
        public DepartmentRegistration GetDepartmentDetails(int departmentId)
        {
            return DepartmentSetupAccess.GetDepartmentDetails(departmentId);
        }

        public List<DepartmentRegistration> GetDepartmentsByCompId(Guid companyId)
        {
            return DepartmentSetupAccess.GetDepartmentsByCompId(companyId);
        }

        public int UpsertDepartmentSetup(DepartmentRegistration departmentRegistration)
        {
            return DepartmentSetupAccess.UpsertDepartmentSetup(departmentRegistration);
        }
    }
}
