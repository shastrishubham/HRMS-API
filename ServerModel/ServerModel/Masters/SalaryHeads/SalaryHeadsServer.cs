using ServerModel.Model.Employee;
using ServerModel.Model.Masters;
using ServerModel.SqlAccess.SalaryHeads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.ServerModel.Masters.SalaryHeads
{
    public class SalaryHeadsServer : SalaryHeadsInfo
    {
        #region Properties Interface

        public static ISalaryHeadsAccess<SalaryHeadsInfo> mSalaryHeadsAccessT
            = new SalaryHeadsAccessWrapper<SalaryHeadsInfo>();

        #endregion

        public static List<SalaryHeadsInfo> GetSalaryHeadsByCompId(Guid compId)
        {
           return mSalaryHeadsAccessT.GetSalaryHeadsByCompId(compId);
        }

        public static int UpsertSalaryHeads(SalaryHeadsInfo salaryHeadsInfo)
        {
            return mSalaryHeadsAccessT.UpsertSalaryHeads(salaryHeadsInfo);
        }
    }
}
