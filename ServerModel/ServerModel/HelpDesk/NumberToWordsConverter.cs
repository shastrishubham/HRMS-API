using System;

namespace ServerModel.ServerModel.HelpDesk
{
    public static class NumberToWordsConverter
    {
        private static readonly string[] units =
        {
            "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
            "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
         };

        private static readonly string[] tens =
        {
            "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
         };

        public static string ConvertAmountToWords(decimal amount, bool isAppendRs = false)
        {
            if (amount == 0) return "Zero Only";

            long rupees = (long)Math.Floor(amount);
            int paise = (int)((amount - rupees) * 100);

            if (isAppendRs)
            {
                string rupeesInWords = ConvertNumberToWords(rupees) + " Rupees";
                string paiseInWords = paise > 0 ? " and " + ConvertNumberToWords(paise) + " Paise" : "";
                return rupeesInWords + paiseInWords + " Only";
            }

            return ConvertNumberToWords(rupees);

        }

        private static string ConvertNumberToWords(long number)
        {
            if (number == 0) return "";

            if (number < 20) return units[number];
            if (number < 100) return tens[number / 10] + (number % 10 > 0 ? " " + units[number % 10] : "");
            if (number < 1000) return units[number / 100] + " Hundred" + (number % 100 > 0 ? " " + ConvertNumberToWords(number % 100) : "");

            if (number < 100000)
                return ConvertNumberToWords(number / 1000) + " Thousand " + ConvertNumberToWords(number % 1000);

            if (number < 10000000)
                return ConvertNumberToWords(number / 100000) + " Lakh " + ConvertNumberToWords(number % 100000);

            return ConvertNumberToWords(number / 10000000) + " Crore " + ConvertNumberToWords(number % 10000000);
        }
    }
}
