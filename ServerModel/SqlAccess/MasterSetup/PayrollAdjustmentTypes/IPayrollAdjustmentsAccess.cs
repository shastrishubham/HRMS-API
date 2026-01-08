using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.MasterSetup.PayrollAdjustmentTypes
{
    public interface IPayrollAdjustmentsAccess
    {
        int UpsertPayrollAdjustmentType(PayrollAdjustmentType adjustmentType);

        List<PayrollAdjustmentType> GetPayrollAdjustmentTypes(Guid compId);
    }
}
