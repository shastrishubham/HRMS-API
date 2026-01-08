using ServerModel.Model.HelpDesk;
using ServerModel.SqlAccess.HelpDesk;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.HelpDesk
{
    public class HelpDeskServer
    {
        #region Properties Interface

        public static IHelpDeskSetupAccess mHelpDeskSetupAccessT = new HelpDeskSetupAccessWrapper();

        #endregion


        public static int UpsertHelpDeskTicketCreation(HelpDeskTicketInformation helpDeskTicket)
        {
            return mHelpDeskSetupAccessT.UpsertHelpDeskTicketCreation(helpDeskTicket);
        }

        public static List<HelpDeskTicketInformation> GetHelpDeskTicketInformationByCompId(Guid compId)
        {
            return mHelpDeskSetupAccessT.GetHelpDeskTicketInformationByCompId(compId);
        }

        public static List<HelpDeskTckReplies> GetHelpDeskTckRepliesByTicketId(int ticketId)
        {
            return mHelpDeskSetupAccessT.GetHelpDeskTckRepliesByTicketId(ticketId);
        }

        public static int UpsertHelpDeskTicketReplies(HelpDeskTckReplies helpDeskTckReplies)
        {
            return mHelpDeskSetupAccessT.UpsertHelpDeskTicketReplies(helpDeskTckReplies);
        }
    }
}
