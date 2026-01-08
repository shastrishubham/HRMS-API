using ServerModel.Database;

namespace ServerModel.Model.HelpDesk
{
    public class HelpDeskTicketInformation : HR_HelpDesk
    {
        public string EmployeeName { get; set; }
        public string TicketCategory { get; set; }
        public string AssignEmployeeName { get; set; }
    }
}
