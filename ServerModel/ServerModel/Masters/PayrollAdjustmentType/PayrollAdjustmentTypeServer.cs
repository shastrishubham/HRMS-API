using ServerModel.SqlAccess.MasterSetup.PayrollAdjustmentTypes;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Masters.PayrollAdjustmentType
{
    public class PayrollAdjustmentTypeServer
    {
        #region Properties Interface

        public static IPayrollAdjustmentsAccess mPayrollAdjustmentsAccessT
            = new PayrollAdjustmentsAccessWrapper();

        #endregion


        public static List<Model.Masters.PayrollAdjustmentType> GetPayrollAdjustmentTypes(Guid companyId)
        {
            return mPayrollAdjustmentsAccessT.GetPayrollAdjustmentTypes(companyId);
        }

        public static int UpsertPayrollAdjustmentType(Model.Masters.PayrollAdjustmentType adjustmentType)
        {
            return mPayrollAdjustmentsAccessT.UpsertPayrollAdjustmentType(adjustmentType);
        }
    }
}
