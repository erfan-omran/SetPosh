using System;
using System.Globalization;
using System.IO;

namespace Core
{
    public class PersianTime : IComparable<PersianTime>, IEquatable<PersianTime>
    {
        public string Hour { get; private set; } = "00";
        public string Minute { get; private set; } = "00";
        public string Second { get; private set; } = "00";
        public static PersianTime Now { get { return new PersianTime(DateTime.Now); } }

        public PersianTime() { }
        public PersianTime(DateTime dateTime)
        {
            InitModel(
                dateTime.Hour.ConvertToString(),
                dateTime.Minute.ConvertToString(),
                dateTime.Second.ConvertToString()
            );
        }
        public PersianTime(string time)
        {
            if (string.IsNullOrEmpty(time))
                return;

            string[] parts = time.Trim().Split(':');
            string Hour = parts[0];
            string Minute = parts[1];
            string Second = parts[2];

            if (parts.Length != 3)
                throw new ArgumentException("فرمت زمان اشتباه است");

            InitModel(Hour, Minute, Second);
        }
        private void InitModel(string hour, string minute, string second)
        {
            if (hour.Length > 2 || minute.Length > 2 || second.Length > 2)
                throw new ArgumentException("فرمت زمان اشتباه است");

            int Hour = hour.ConvertToInt();
            int Minute = minute.ConvertToInt();
            int Second = second.ConvertToInt();

            if (Hour < 0 || Hour > 23) throw new ArgumentException("ساعت باید بین 0 تا 23 باشد.");
            if (Minute < 0 || Minute > 59) throw new ArgumentException("دقیقه باید بین 0 تا 59 باشد.");
            if (Second < 0 || Second > 59) throw new ArgumentException("ثانیه باید بین 0 تا 59 باشد.");

            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }
        //--------------------------------------
        public DateTime ToGregorian()
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.ToDateTime(0, 0, 0, Hour.ConvertToInt(), Minute.ConvertToInt(), Second.ConvertToInt(), 0);
        }
        public override string ToString() => $"{Hour:D2}:{Minute:D2}:{Second:D2}";

        public static int SecondsDifference(PersianTime Time1, PersianTime Time2)
        {
            DateTime DateTime1 = DateTime.Today.AddHours(Time1.Hour.ConvertToInt()).AddMinutes(Time1.Minute.ConvertToInt()).AddSeconds(Time1.Second.ConvertToInt());
            DateTime DateTime2 = DateTime.Today.AddHours(Time2.Hour.ConvertToInt()).AddMinutes(Time2.Minute.ConvertToInt()).AddSeconds(Time2.Second.ConvertToInt());
            return (int)(DateTime1 - DateTime2).TotalSeconds;
        }
        public static int MinutesDifference(PersianTime Time1, PersianTime Time2)
        {
            DateTime DateTime1 = DateTime.Today.AddHours(Time1.Hour.ConvertToInt()).AddMinutes(Time1.Minute.ConvertToInt()).AddSeconds(Time1.Second.ConvertToInt());
            DateTime DateTime2 = DateTime.Today.AddHours(Time2.Hour.ConvertToInt()).AddMinutes(Time2.Minute.ConvertToInt()).AddSeconds(Time2.Second.ConvertToInt());
            return (int)(DateTime1 - DateTime2).TotalMinutes;
        }
        public static int HoursDifference(PersianTime Time1, PersianTime Time2)
        {
            DateTime DateTime1 = DateTime.Today.AddHours(Time1.Hour.ConvertToInt()).AddMinutes(Time1.Minute.ConvertToInt()).AddSeconds(Time1.Second.ConvertToInt());
            DateTime DateTime2 = DateTime.Today.AddHours(Time2.Hour.ConvertToInt()).AddMinutes(Time2.Minute.ConvertToInt()).AddSeconds(Time2.Second.ConvertToInt());
            return (int)(DateTime1 - DateTime2).TotalHours;
        }
        public static PersianTime TimeBetween(PersianTime Time1, PersianTime Time2)
        {
            int totalHours = HoursDifference(Time1, Time2);
            int totalMinutes = MinutesDifference(Time1, Time2);
            int totalSeconds = SecondsDifference(Time1, Time2);
            return new PersianTime($"{totalHours:D2}:{totalMinutes:D2}:{totalSeconds:D2}");
        }
        //--------------------------------------
        //public int CompareTo(PersianTime other)
        //{
        //    if (other == null) return 1;
        //    return (Hour, Minute, Second).CompareTo((other.Hour, other.Minute, other.Second));
        //}
        public int CompareTo(PersianTime time)
        {
            if (time == null) return 1;
            return ToGregorian().CompareTo(time.ToGregorian());
        }
        public bool Equals(PersianTime time)
        {
            if (time == null) return false;
            bool Ans = Hour.ConvertToInt() == time.Hour.ConvertToInt() &&
                       Minute.ConvertToInt() == time.Minute.ConvertToInt() &&
                       Second.ConvertToInt() == time.Second.ConvertToInt();
            return Ans;
        }
        public override bool Equals(object obj) => Equals(obj as PersianTime);
        public override int GetHashCode() => (Hour, Minute, Second).GetHashCode();

        public static bool operator ==(PersianTime left, PersianTime right) => Equals(left, right);
        public static bool operator !=(PersianTime left, PersianTime right) => !Equals(left, right);
        public static bool operator <(PersianTime left, PersianTime right) => left.CompareTo(right) < 0;
        public static bool operator >(PersianTime left, PersianTime right) => left.CompareTo(right) > 0;
        public static bool operator <=(PersianTime left, PersianTime right) => left.CompareTo(right) <= 0;
        public static bool operator >=(PersianTime left, PersianTime right) => left.CompareTo(right) >= 0;
    }
}
