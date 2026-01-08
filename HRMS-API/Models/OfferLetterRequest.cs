using System.Collections.Generic;

namespace HRMS_API.Models
{
    public class OfferLetterRequest
    {
        // Company
        public string CompanyName { get; set; } = "";
        public string CompanyAddressLine1 { get; set; } = "";
        public string CompanyAddressLine2 { get; set; } = "";
        public string CityStatePin { get; set; } = "";

        // Candidate
        public string CandidateFullName { get; set; } = "";
        public string CandidateAddress { get; set; } = "";

        // Job
        public string JobTitle { get; set; } = "";
        public string Location { get; set; } = "";
        public string ReportingManager { get; set; } = "";
        public string DateOfJoining { get; set; } = "";
        public string OfferDate { get; set; } = "";

        // Offer Specific
        public string ProbationPeriod { get; set; } = "";
        public string ProbationPeriodWords { get; set; } = "";
        public string AnnualCtcFigures { get; set; } = "";
        public string AnnualCtcWords { get; set; } = "";
        public string NoticePeriod { get; set; } = "";
        public string WorkingHours { get; set; } = "";

        // Annexure A
        public List<SalaryComponentDto> Components { get; set; } = new List<SalaryComponentDto>();
        public string TotalCtcMonthly { get; set; } = "";
        public string TotalCtcAnnual { get; set; } = "";
    }
}