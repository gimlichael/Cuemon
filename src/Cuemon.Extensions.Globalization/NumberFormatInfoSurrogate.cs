using System.Globalization;

namespace Cuemon.Extensions.Globalization
{
    internal class NumberFormatInfoSurrogate
    {
        internal NumberFormatInfoSurrogate()
        {
        }

        internal NumberFormatInfoSurrogate(NumberFormatInfo numberFormatInfo)
        {
            CurrencyDecimalDigits = numberFormatInfo.CurrencyDecimalDigits;
            CurrencyDecimalSeparator = numberFormatInfo.CurrencyDecimalSeparator;
            CurrencyGroupSeparator = numberFormatInfo.CurrencyGroupSeparator;
            CurrencyNegativePattern = numberFormatInfo.CurrencyNegativePattern;
            CurrencyPositivePattern = numberFormatInfo.CurrencyPositivePattern;
            CurrencySymbol = numberFormatInfo.CurrencySymbol;
            DigitSubstitution = numberFormatInfo.DigitSubstitution;
            NaNSymbol = numberFormatInfo.NaNSymbol;
            NegativeInfinitySymbol= numberFormatInfo.NegativeInfinitySymbol;
            NegativeSign = numberFormatInfo.NegativeSign;
            NumberDecimalDigits= numberFormatInfo.NumberDecimalDigits;
            NumberDecimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            NumberGroupSeparator = numberFormatInfo.NumberGroupSeparator;
            NumberNegativePattern = numberFormatInfo.NumberNegativePattern;
            PerMilleSymbol= numberFormatInfo.PerMilleSymbol;
            PercentDecimalDigits = numberFormatInfo.PercentDecimalDigits;
            PercentDecimalSeparator = numberFormatInfo.PercentDecimalSeparator;
            PercentGroupSeparator = numberFormatInfo.PercentGroupSeparator;
            PercentNegativePattern = numberFormatInfo.PercentNegativePattern;
            PercentPositivePattern = numberFormatInfo.PercentPositivePattern;
            PercentSymbol = numberFormatInfo.PercentSymbol;
            PositiveInfinitySymbol = numberFormatInfo.PositiveInfinitySymbol;
            PositiveSign = numberFormatInfo.PositiveSign;
        }

        public int CurrencyDecimalDigits { get; set; }
        
        public int CurrencyNegativePattern { get; set; }
        
        public int CurrencyPositivePattern { get; set; }

        public DigitShapes DigitSubstitution { get; set; }
        
        public int NumberDecimalDigits { get; set; }
        
        public int NumberNegativePattern { get; set; }
        
        public int PercentDecimalDigits { get; set; }
        
        public int PercentNegativePattern { get; set; }
        
        public int PercentPositivePattern { get; set; }
        
        public string CurrencyDecimalSeparator { get; set; }
        
        public string CurrencyGroupSeparator { get; set; }
        
        public string CurrencySymbol { get; set; }
        
        public string NaNSymbol { get; set; }
        
        public string NegativeInfinitySymbol { get; set; }
        
        public string NegativeSign { get; set; }
        
        public string NumberDecimalSeparator { get; set; }
        
        public string NumberGroupSeparator { get; set; }
        
        public string PercentDecimalSeparator { get; set; }
        
        public string PercentGroupSeparator { get; set; }
        
        public string PercentSymbol { get; set; }
        
        public string PerMilleSymbol { get; set; }
        
        public string PositiveInfinitySymbol { get; set; }
        
        public string PositiveSign { get; set; }
    }
}
