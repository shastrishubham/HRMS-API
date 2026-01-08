using ServerModel.Model.Base;
using System;

namespace ServerModel.Model.Masters
{
    public class LeaveInfo
    {
        public int Id { get; set; }
        public Guid CompId { get; set; }
        public string LeaveName { get; set; }
        public string LeaveCode { get; set; }
        public LeaveTypes LeaveType { get; set; }
        public string Unit { get; set; }
        public LeaveEffectiveAfterOnTypes EffectiveAfterOnTypes { get; set; }
        public int CarryForward { get; set; }
        public string ApplicableFor { get; set; }
        public int MS_Branch_Id { get; set; }
        public LeaveDurationAllowedTypes DurationAllowed { get; set; }
        public bool Active { get; set; }
    }
}
