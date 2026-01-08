using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.HelpDeskTicketCategory
{
    public class HelpDeskTicketCatSetupAccessWrapper : IHelpDeskTicketCatSetupAccess
    {
        public List<TicketCategoryInformation> GetTicketCategoriesByCompId(Guid compId)
        {
            return HelpDeskTicketCatSetupAccess.GetTicketCategoriesByCompId(compId);
        }

        public int UpsertTicketCategory(TicketCategoryInformation ticketCategoryInformation)
        {
            return HelpDeskTicketCatSetupAccess.UpsertTicketCategory(ticketCategoryInformation);
        }
    }
}
