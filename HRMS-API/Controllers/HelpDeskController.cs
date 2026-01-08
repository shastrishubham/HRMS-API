using ServerModel.Data;
using ServerModel.Model.HelpDesk;
using ServerModel.ServerModel.HelpDesk;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HelpDeskController : ApiController
    {

        public HelpDeskController()
        {
            DbContext.ConnectionString = WebApiConfig.ConnectionString;
        }

        [AllowAnonymous]
        [Route("api/HelpDesk/UpsertHelpDeskTicketCreation")]
        [HttpPost]
        public int UpsertHelpDeskTicketCreation(HelpDeskTicketInformation helpDeskTicket)
        {
            return HelpDeskServer.UpsertHelpDeskTicketCreation(helpDeskTicket);
        }

        [AllowAnonymous]
        [Route("api/HelpDesk/GetHelpDeskTicketInformationByCompId")]
        [HttpGet]
        public List<HelpDeskTicketInformation> GetHelpDeskTicketInformationByCompId(Guid compId)
        {
            return HelpDeskServer.GetHelpDeskTicketInformationByCompId(compId);
        }

        [AllowAnonymous]
        [Route("api/HelpDesk/GetHelpDeskTckRepliesByTicketId")]
        [HttpGet]
        public List<HelpDeskTckReplies> GetHelpDeskTckRepliesByTicketId(int ticketId)
        {
            return HelpDeskServer.GetHelpDeskTckRepliesByTicketId(ticketId);
        }

        [AllowAnonymous]
        [Route("api/HelpDesk/UpsertHelpDeskTicketReplies")]
        [HttpPost]
        public int UpsertHelpDeskTicketReplies(HelpDeskTckReplies helpDeskTckReplies)
        {
            return HelpDeskServer.UpsertHelpDeskTicketReplies(helpDeskTckReplies);
        }
    }
}
