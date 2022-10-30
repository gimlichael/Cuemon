using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Cuemon.Extensions.Globalization
{
    [Serializable]
    internal class NumberFormatInfoSurrogate : ISerializable
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

        internal NumberFormatInfoSurrogate(SerializationInfo info, StreamingContext context)
        {
            CurrencyDecimalDigits = (int)info.GetValue(nameof(CurrencyDecimalDigits), typeof(int));
            CurrencyNegativePattern = (int)info.GetValue(nameof(CurrencyNegativePattern), typeof(int));
            CurrencyPositivePattern = (int)info.GetValue(nameof(CurrencyPositivePattern), typeof(int));
            DigitSubstitution = (DigitShapes)info.GetValue(nameof(DigitSubstitution), typeof(DigitShapes));
            NumberDecimalDigits = (int)info.GetValue(nameof(NumberDecimalDigits), typeof(int));
            NumberNegativePattern = (int)info.GetValue(nameof(NumberNegativePattern), typeof(int));
            PercentDecimalDigits = (int)info.GetValue(nameof(PercentDecimalDigits), typeof(int));
            PercentNegativePattern = (int)info.GetValue(nameof(PercentNegativePattern), typeof(int));
            PercentPositivePattern = (int)info.GetValue(nameof(PercentPositivePattern), typeof(int));
            CurrencyDecimalSeparator = (string)info.GetValue(nameof(CurrencyDecimalSeparator), typeof(string));
            CurrencyGroupSeparator = (string)info.GetValue(nameof(CurrencyGroupSeparator), typeof(string));
            CurrencySymbol = (string)info.GetValue(nameof(CurrencySymbol), typeof(string));
            NaNSymbol = (string)info.GetValue(nameof(NaNSymbol), typeof(string));
            NegativeInfinitySymbol = (string)info.GetValue(nameof(NegativeInfinitySymbol), typeof(string));
            NegativeSign = (string)info.GetValue(nameof(NegativeSign), typeof(string));
            NumberDecimalSeparator = (string)info.GetValue(nameof(NumberDecimalSeparator), typeof(string));
            NumberGroupSeparator = (string)info.GetValue(nameof(NumberGroupSeparator), typeof(string));
            PercentDecimalSeparator = (string)info.GetValue(nameof(PercentDecimalSeparator), typeof(string));
            PercentGroupSeparator = (string)info.GetValue(nameof(PercentGroupSeparator), typeof(string));
            PercentSymbol = (string)info.GetValue(nameof(PercentSymbol), typeof(string));
            PerMilleSymbol = (string)info.GetValue(nameof(PerMilleSymbol), typeof(string));
            PositiveInfinitySymbol = (string)info.GetValue(nameof(PositiveInfinitySymbol), typeof(string));
            PositiveSign = (string)info.GetValue(nameof(PositiveSign), typeof(string));
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(DigitSubstitution), DigitSubstitution);
            info.AddValue(nameof(CurrencyDecimalDigits), CurrencyDecimalDigits);
            info.AddValue(nameof(CurrencyNegativePattern), CurrencyNegativePattern);
            info.AddValue(nameof(CurrencyPositivePattern), CurrencyPositivePattern);
            info.AddValue(nameof(NumberDecimalDigits), NumberDecimalDigits);
            info.AddValue(nameof(NumberNegativePattern), NumberNegativePattern);
            info.AddValue(nameof(PercentDecimalDigits), PercentDecimalDigits);
            info.AddValue(nameof(PercentNegativePattern), PercentNegativePattern);
            info.AddValue(nameof(PercentPositivePattern), PercentPositivePattern);
            info.AddValue(nameof(CurrencyDecimalSeparator), CurrencyDecimalSeparator);
            info.AddValue(nameof(CurrencyGroupSeparator), CurrencyGroupSeparator);
            info.AddValue(nameof(CurrencySymbol), CurrencySymbol);
            info.AddValue(nameof(NaNSymbol), NaNSymbol);
            info.AddValue(nameof(NegativeInfinitySymbol), NegativeInfinitySymbol);
            info.AddValue(nameof(NegativeSign), NegativeSign);
            info.AddValue(nameof(NumberDecimalSeparator), NumberDecimalSeparator);
            info.AddValue(nameof(NumberGroupSeparator), NumberGroupSeparator);
            info.AddValue(nameof(PercentDecimalSeparator), PercentDecimalSeparator);
            info.AddValue(nameof(PercentGroupSeparator), PercentGroupSeparator);
            info.AddValue(nameof(PercentSymbol), PercentSymbol);
            info.AddValue(nameof(PerMilleSymbol), PerMilleSymbol);
            info.AddValue(nameof(PositiveInfinitySymbol), PositiveInfinitySymbol);
            info.AddValue(nameof(PositiveSign), PositiveSign);
        }
    }
}
