using System;

namespace ServerModel.Model.Masters
{
    public class PerformanceBand
    {
        public int Id { get; set; }
        public DateTime FormDate { get; set; }
        public Guid CompId { get; set; }
        public string BandCode { get; set; }
        public decimal MinRating { get; set; }
        public decimal MaxRating { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

    }
}
