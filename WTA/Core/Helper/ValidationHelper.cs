using System;

namespace Core.Helper
{
    public static class ValidationHelper
    {
        public static bool ValidateDateTime(string input, out DateTime result)
        {
            return DateTime.TryParse(input, out result);
        }

        public static bool ValidateDecimal(string input, out decimal result)
        {
            return decimal.TryParse(input, out result);
        }
    }
}
