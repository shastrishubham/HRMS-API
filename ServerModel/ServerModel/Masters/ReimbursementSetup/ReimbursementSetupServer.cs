using ServerModel.SqlAccess.MasterSetup.ReimbursementTypes;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Masters.ReimbursementSetup
{
    public class ReimbursementSetupServer
    {
        #region Properties Interface

        public static IReimbursementTypesAccess mReimbursementSetupAccessT
            = new ReimbursementTypesAccessWrapper();

        #endregion


        public static List<Model.Masters.ReimbursementTypes> GetReimbursementTypesByCompId(Guid companyId)
        {
            return mReimbursementSetupAccessT.GetReimbursementTypesByCompId(companyId);
        }

        public static int UpsertReimbursementTypes(Model.Masters.ReimbursementTypes reimbursementTypes)
        {
            return mReimbursementSetupAccessT.UpsertReimbursementTypes(reimbursementTypes);
        }
    }
}
