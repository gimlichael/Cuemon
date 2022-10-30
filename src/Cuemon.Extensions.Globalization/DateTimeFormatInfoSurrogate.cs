using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Cuemon.Extensions.Globalization
{
    [Serializable]
    internal class DateTimeFormatInfoSurrogate : ISerializable
    {
        internal DateTimeFormatInfoSurrogate()
        {
        }

        internal DateTimeFormatInfoSurrogate(DateTimeFormatInfo dateTimeFormatInfo)
        {
            AMDesignator = dateTimeFormatInfo.AMDesignator;
            CalendarWeekRule = dateTimeFormatInfo.CalendarWeekRule;
            DateSeparator = dateTimeFormatInfo.DateSeparator;
            FirstDayOfWeek = dateTimeFormatInfo.FirstDayOfWeek;
            FullDateTimePattern = dateTimeFormatInfo.FullDateTimePattern;
            LongDatePattern = dateTimeFormatInfo.LongDatePattern;
            LongTimePattern = dateTimeFormatInfo.LongTimePattern;
            MonthDayPattern = dateTimeFormatInfo.MonthDayPattern;
            PMDesignator = dateTimeFormatInfo.PMDesignator;
            ShortDatePattern = dateTimeFormatInfo.ShortDatePattern;
            ShortTimePattern = dateTimeFormatInfo.ShortTimePattern;
            TimeSeparator = dateTimeFormatInfo.TimeSeparator;
            YearMonthPattern = dateTimeFormatInfo.YearMonthPattern;
        }

        internal DateTimeFormatInfoSurrogate(SerializationInfo info, StreamingContext context)
        {
            AMDesignator = (string)info.GetValue(nameof(AMDesignator), typeof(string));
            CalendarWeekRule = (CalendarWeekRule)info.GetValue(nameof(CalendarWeekRule), typeof(CalendarWeekRule));
            DateSeparator = (string)info.GetValue(nameof(DateSeparator), typeof(string));
            FirstDayOfWeek = (DayOfWeek)info.GetValue(nameof(FirstDayOfWeek), typeof(DayOfWeek));
            FullDateTimePattern = (string)info.GetValue(nameof(FullDateTimePattern), typeof(string));
            LongDatePattern = (string)info.GetValue(nameof(LongDatePattern), typeof(string));
            LongTimePattern = (string)info.GetValue(nameof(LongTimePattern), typeof(string));
            MonthDayPattern = (string)info.GetValue(nameof(MonthDayPattern), typeof(string));
            PMDesignator = (string)info.GetValue(nameof(PMDesignator), typeof(string));
            ShortDatePattern = (string)info.GetValue(nameof(ShortDatePattern), typeof(string));
            ShortTimePattern = (string)info.GetValue(nameof(ShortTimePattern), typeof(string));
            TimeSeparator = (string)info.GetValue(nameof(TimeSeparator), typeof(string));
            YearMonthPattern = (string)info.GetValue(nameof(YearMonthPattern), typeof(string));
        }
        
        public string AMDesignator { get; set; }

        public CalendarWeekRule CalendarWeekRule { get; set; }

        public string DateSeparator { get; set; }

        public DayOfWeek FirstDayOfWeek { get; set; }
        
        public string FullDateTimePattern { get; set; }
        
        public string LongDatePattern { get; set; }
        
        public string LongTimePattern { get; set; }
        
        public string MonthDayPattern { get; set; }
        
        public string PMDesignator { get; set; }
        
        public string ShortDatePattern { get; set; }
        
        public string ShortTimePattern { get; set; }
        
        public string TimeSeparator { get; set; }
        
        public string YearMonthPattern { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(AMDesignator), AMDesignator, typeof(string));
            info.AddValue(nameof(CalendarWeekRule), CalendarWeekRule, typeof(CalendarWeekRule));
            info.AddValue(nameof(DateSeparator), DateSeparator, typeof(string));
            info.AddValue(nameof(FirstDayOfWeek), FirstDayOfWeek, typeof(DayOfWeek));
            info.AddValue(nameof(FullDateTimePattern), FullDateTimePattern, typeof(string));
            info.AddValue(nameof(LongDatePattern), LongDatePattern, typeof(string));
            info.AddValue(nameof(LongTimePattern), LongTimePattern, typeof(string));
            info.AddValue(nameof(MonthDayPattern), MonthDayPattern, typeof(string));
            info.AddValue(nameof(PMDesignator), PMDesignator, typeof(string));
            info.AddValue(nameof(ShortDatePattern), ShortDatePattern, typeof(string));
            info.AddValue(nameof(ShortTimePattern), ShortTimePattern, typeof(string));
            info.AddValue(nameof(TimeSeparator), TimeSeparator, typeof(string));
            info.AddValue(nameof(YearMonthPattern), YearMonthPattern, typeof(string));
        }
    }
}
