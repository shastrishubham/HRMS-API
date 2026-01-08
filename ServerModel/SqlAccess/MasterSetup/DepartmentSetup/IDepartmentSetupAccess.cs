using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.DepartmentSetup
{
    public interface IDepartmentSetupAccess
    {
        int UpsertDepartmentSetup(DepartmentRegistration departmentRegistration);

        DepartmentRegistration GetDepartmentDetails(int departmentId);

        List<DepartmentRegistration> GetDepartmentsByCompId(Guid companyId);
    }
}
