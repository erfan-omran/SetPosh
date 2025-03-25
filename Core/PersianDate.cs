using System;
using System.Globalization;
using System.IO;

namespace Core
{
    public class PersianDate : IComparable<PersianDate>, IEquatable<PersianDate>
    {
        public string Year { get; private set; } = "0001";
        public string Month { get; private set; } = "01";
        public string Day { get; private set; } = "01";

        public static PersianDate Now { get { return new PersianDate(DateTime.Now); } }
        private PersianCalendar _persianCalendar = new PersianCalendar();

        public PersianDate() { }
        public PersianDate(DateTime dateTime)
        {
            InitModel(
                _persianCalendar.GetYear(dateTime).ConvertToString(),
                _persianCalendar.GetMonth(dateTime).ConvertToString(),
                _persianCalendar.GetDayOfMonth(dateTime).ConvertToString()
            );
        }
        public PersianDate(string Date)
        {
            if (string.IsNullOrEmpty(Date))
                return;

            string[] parts = Date.Trim().Split('/');
            string year = parts[0];
            string month = parts[1];
            string day = parts[2];

            if (parts.Length != 3)
                throw new ArgumentException("فرمت تاریخ اشتباه است");

            InitModel(year, month, day);
        }
        private void InitModel(string year, string month, string day)
        {
            if (year.Length != 4 || month.Length != 2 || day.Length != 2)
                throw new ArgumentException("فرمت تاریخ اشتباه است");

            int Year = year.ConvertToInt();
            int Month = month.ConvertToInt();
            int Day = day.ConvertToInt();

            if (Year < 0 || Year > int.MaxValue)
                throw new ArgumentException("سال وارد شده درست نمی باشد");
            if (Month < 1 || Month > 12)
                throw new ArgumentException("ماه باید بین 1 تا 12 باشد.");

            int DaysInMonth = _persianCalendar.GetDaysInMonth(Year, Month);
            if (Day < 1 || Day > DaysInMonth)
                throw new ArgumentException($"روز باید بین 1 تا {DaysInMonth} باشد.");

            this.Year = year;
            this.Month = month;
            this.Day = day;
        }
        //--------------------------------------
        public DateTime ToGregorian()
        {
            return _persianCalendar.ToDateTime(Year.ConvertToInt(), Month.ConvertToInt(), Day.ConvertToInt(), 0, 0, 0, 0);
        }
        public override string ToString() => $"{Year}/{Month:D2}/{Day:D2}";

        public static int DaysDifference(PersianDate date1, PersianDate date2)
        {
            DateTime date1Gregorian = date1.ToGregorian();
            DateTime date2Gregorian = date2.ToGregorian();
            return (int)(date1Gregorian - date2Gregorian).TotalDays;
        }
        public static int MonthsDifference(PersianDate date1, PersianDate date2)
        {
            DateTime date1Gregorian = date1.ToGregorian();
            DateTime date2Gregorian = date2.ToGregorian();
            int monthsDifference = ((date1Gregorian.Year - date2Gregorian.Year) * 12) + date1Gregorian.Month - date2Gregorian.Month;
            return monthsDifference;
        }
        public static int YearsDifference(PersianDate date1, PersianDate date2)
        {
            DateTime date1Gregorian = date1.ToGregorian();
            DateTime date2Gregorian = date2.ToGregorian();
            return date1Gregorian.Year - date2Gregorian.Year;
        }
        public static PersianDate DateBetween(PersianDate date1, PersianDate date2)
        {
            int totalYears = DaysDifference(date1, date2);
            int totalMonths = DaysDifference(date1, date2);
            int totalDays = DaysDifference(date1, date2);
            return new PersianDate($"{totalYears}/{totalMonths}/{totalDays}");
        }
        //--------------------------------------
        public int CompareTo(PersianDate date)
        {
            if (date == null) return 1;
            return ToGregorian().CompareTo(date.ToGregorian());
        }
        public bool Equals(PersianDate date)
        {
            if (date == null) return false;
            bool Ans = Year.ConvertToInt() == date.Year.ConvertToInt() &&
                       Month.ConvertToInt() == date.Month.ConvertToInt() &&
                       Day.ConvertToInt() == date.Day.ConvertToInt();
            return Ans;
        }
        public override bool Equals(object obj) => Equals(obj as PersianDate);
        public override int GetHashCode() => ToGregorian().GetHashCode();

        public static bool operator ==(PersianDate left, PersianDate right) => Equals(left, right);
        public static bool operator !=(PersianDate left, PersianDate right) => !Equals(left, right);
        public static bool operator <(PersianDate left, PersianDate right) => left.CompareTo(right) < 0;
        public static bool operator >(PersianDate left, PersianDate right) => left.CompareTo(right) > 0;
        public static bool operator <=(PersianDate left, PersianDate right) => left.CompareTo(right) <= 0;
        public static bool operator >=(PersianDate left, PersianDate right) => left.CompareTo(right) >= 0;
    }
}
