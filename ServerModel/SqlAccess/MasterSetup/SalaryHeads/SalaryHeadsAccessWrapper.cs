using ServerModel.Model.Employee;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.SalaryHeads
{
    public class SalaryHeadsAccessWrapper : ISalaryHeadsAccess
    {
    }

    public class SalaryHeadsAccessWrapper<T> : ISalaryHeadsAccess<T> where T : SalaryHeadsInfo
    {
        public int UpsertSalaryHeads(SalaryHeadsInfo salaryHeadsInfo)
        {
            return SalaryHeadsAccess<T>.UpsertSalaryHeads(salaryHeadsInfo);
        }

        public List<SalaryHeadsInfo> GetSalaryHeadsByCompId(Guid compId)
        {
            return SalaryHeadsAccess<T>.GetSalaryHeadsByCompId(compId);
        }
    }

}
