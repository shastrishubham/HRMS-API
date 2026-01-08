using ServerModel.Model.Employee;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.SalaryHeads
{
    public interface ISalaryHeadsAccess
    {
    }


    public interface ISalaryHeadsAccess<T> where T : SalaryHeadsInfo
    {
        List<SalaryHeadsInfo> GetSalaryHeadsByCompId(Guid compId);

        int UpsertSalaryHeads(SalaryHeadsInfo salaryHeadsInfo);
    }

}
