using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Cuemon.Extensions.Globalization
{
    internal class DateTimeFormatInfoSurrogate
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
            ShortestDayNames = dateTimeFormatInfo.ShortestDayNames;
            AbbreviatedDayNames = dateTimeFormatInfo.AbbreviatedDayNames;
            AbbreviatedMonthNames = dateTimeFormatInfo.AbbreviatedMonthNames;
            AbbreviatedMonthGenitiveNames = dateTimeFormatInfo.AbbreviatedMonthGenitiveNames;
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
        
        public string[] ShortestDayNames { get; set; }

        public string[] AbbreviatedDayNames { get; set; }

        public string[] AbbreviatedMonthNames { get; set; }

        public string[] AbbreviatedMonthGenitiveNames { get; set; }
    }
}
