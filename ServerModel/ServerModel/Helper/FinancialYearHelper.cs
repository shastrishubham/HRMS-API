using System;

namespace ServerModel.ServerModel.Helper
{
    public static class FinancialYearHelper
    {
        public static DateTime GetFinancialYearStart(DateTime? input = null)
        {
            if (input.HasValue) return input.Value;

            var today = DateTime.Today;
            int year = today.Month >= 4 ? today.Year : today.Year - 1;
            return new DateTime(year, 4, 1); // 1 April
        }

        public static DateTime GetFinancialYearEnd(DateTime? input = null)
        {
            if (input.HasValue) return input.Value;

            var today = DateTime.Today;
            int year = today.Month >= 4 ? today.Year + 1 : today.Year;
            return new DateTime(year, 3, 31); // 31 March
        }
    }
}
