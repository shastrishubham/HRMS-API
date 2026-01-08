using ServerModel.Database;

namespace ServerModel.Model.HelpDesk
{
    public class HelpDeskTckReplies : HR_HelpDeskReplies
    {
        public string RepliesEmployeeName { get; set; }

        public string InterviewStatus { get; set; }
    }
}
