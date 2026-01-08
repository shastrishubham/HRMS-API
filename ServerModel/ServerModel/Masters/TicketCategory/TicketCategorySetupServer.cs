using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.HelpDeskTicketCategory;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Masters.TicketCategory
{
    public class TicketCategorySetupServer
    {
        #region Properties Interface

        public static IHelpDeskTicketCatSetupAccess mHelpDeskTicketCatSetupAccessT
            = new HelpDeskTicketCatSetupAccessWrapper();

        #endregion

        public static List<TicketCategoryInformation> GetTicketCategoriesByCompId(Guid companyId)
        {
            List<TicketCategoryInformation> ticketCategories = mHelpDeskTicketCatSetupAccessT.GetTicketCategoriesByCompId(companyId);
            
            return ticketCategories;
        }

        public static int UpsertTicketCategory(TicketCategoryInformation ticketCategoryInformation)
        {
            return mHelpDeskTicketCatSetupAccessT.UpsertTicketCategory(ticketCategoryInformation);
        }
    }
}
