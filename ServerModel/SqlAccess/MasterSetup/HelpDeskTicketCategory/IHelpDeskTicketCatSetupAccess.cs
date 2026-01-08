using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.HelpDeskTicketCategory
{
    public interface IHelpDeskTicketCatSetupAccess
    {
        int UpsertTicketCategory(TicketCategoryInformation ticketCategoryInformation);

        List<TicketCategoryInformation> GetTicketCategoriesByCompId(Guid compId);
    }
}
