using ServerModel.Model.HelpDesk;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.HelpDesk
{
    public interface IHelpDeskSetupAccess
    {
        int UpsertHelpDeskTicketCreation(HelpDeskTicketInformation helpDeskTicket);

        List<HelpDeskTicketInformation> GetHelpDeskTicketInformationByCompId(Guid compId);

        int UpsertHelpDeskTicketReplies(HelpDeskTckReplies helpDeskTckReplies);

        List<HelpDeskTckReplies> GetHelpDeskTckRepliesByTicketId(int ticketId);
    }
}
