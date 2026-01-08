using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.ReimbursementTypes
{
    public class ReimbursementTypesAccessWrapper : IReimbursementTypesAccess
    {
        public List<Model.Masters.ReimbursementTypes> GetReimbursementTypesByCompId(Guid compId)
        {
            return ReimbursementTypesAccess.GetReimbursementTypesByCompId(compId);
        }

        public int UpsertReimbursementTypes(Model.Masters.ReimbursementTypes reimbursement)
        {
            return ReimbursementTypesAccess.UpsertReimbursementTypes(reimbursement);
        }
    }
}
