namespace SeerbitHackaton.Core.Extensions
{
    public static class DecimalExtension
    {
        public static string FormatAsMinorCurrency(this decimal value)
        {
            var numberFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
            numberFormat.CurrencyDecimalDigits = 2;
            numberFormat.CurrencyDecimalSeparator = ".";
            numberFormat.CurrencySymbol = "";
            numberFormat.CurrencyGroupSeparator = "";
            return value.ToString("c", numberFormat).Replace(".", "");
        }
    }
    
}
