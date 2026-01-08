using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.ReimbursementTypes
{
    public interface IReimbursementTypesAccess
    {
        int UpsertReimbursementTypes(Model.Masters.ReimbursementTypes reimbursement);

        List<Model.Masters.ReimbursementTypes> GetReimbursementTypesByCompId(Guid compId);
    }
}
