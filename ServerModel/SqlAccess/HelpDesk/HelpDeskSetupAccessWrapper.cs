using ServerModel.Model.HelpDesk;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.HelpDesk
{
    public class HelpDeskSetupAccessWrapper : IHelpDeskSetupAccess
    {
        public List<HelpDeskTckReplies> GetHelpDeskTckRepliesByTicketId(int ticketId)
        {
            return HelpDeskSetupAccess.GetHelpDeskTckRepliesByTicketId(ticketId);
        }

        public List<HelpDeskTicketInformation> GetHelpDeskTicketInformationByCompId(Guid compId)
        {
            return HelpDeskSetupAccess.GetHelpDeskTicketInformationByCompId(compId);
        }

        public int UpsertHelpDeskTicketCreation(HelpDeskTicketInformation helpDeskTicket)
        {
            return HelpDeskSetupAccess.UpsertHelpDeskTicketCreation(helpDeskTicket);
        }

        public int UpsertHelpDeskTicketReplies(HelpDeskTckReplies helpDeskTckReplies)
        {
            return HelpDeskSetupAccess.UpsertHelpDeskTicketReply(helpDeskTckReplies);
        }
    }
}
