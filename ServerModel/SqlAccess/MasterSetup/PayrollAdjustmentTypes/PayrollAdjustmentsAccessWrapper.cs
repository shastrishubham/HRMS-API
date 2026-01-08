using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.PayrollAdjustmentTypes
{
    public class PayrollAdjustmentsAccessWrapper : IPayrollAdjustmentsAccess
    {
        public List<PayrollAdjustmentType> GetPayrollAdjustmentTypes(Guid compId)
        {
            return PayrollAdjustmentsAccess.GetPayrollAdjustmentTypes(compId);
        }

        public int UpsertPayrollAdjustmentType(PayrollAdjustmentType adjustmentType)
        {
            return PayrollAdjustmentsAccess.UpsertPayrollAdjustmentType(adjustmentType);
        }
    }
}
