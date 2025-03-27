using System.Data;
using System.Globalization;

namespace Core
{
    public static class DataExtension
    {
        public static string ConvertToString(this object value, string defaultValue = "")
        {
            if (value != null && value != DBNull.Value)
                return value.ToString();
            return defaultValue;
        }
        public static short ConvertToShort(this object value, short defaultValue = 0)
        {
            if (value != null && value != DBNull.Value && short.TryParse(value.ToString(), out short result))
                return result;
            return defaultValue;
        }
        public static int ConvertToInt(this object value, int defaultValue = 0)
        {
            if (value != null && value != DBNull.Value && int.TryParse(value.ToString(), out int result))
                return result;
            return defaultValue;
        }
        public static long ConvertToLong(this object value, long defaultValue = 0)
        {
            if (value != null && value != DBNull.Value && long.TryParse(value.ToString(), out long result))
                return result;
            return defaultValue;
        }
        public static float ConvertToFloat(this object value, float defaultValue = 0f)
        {
            if (value != null && value != DBNull.Value && float.TryParse(value.ToString(), out float result))
                return result;
            return defaultValue;
        }
        public static double ConvertToDouble(this object value, double defaultValue = 0.0)
        {
            if (value != null && value != DBNull.Value && double.TryParse(value.ToString(), out double result))
                return result;
            return defaultValue;
        }
        public static decimal ConvertToDecimal(this object value, decimal defaultValue = 0.0M)
        {
            if (value != null && value != DBNull.Value && decimal.TryParse(value.ToString(), out decimal result))
                return result;
            return defaultValue;
        }
        public static bool ConvertToBool(this object value, bool defaultValue = false)
        {
            if (value != null && value != DBNull.Value && bool.TryParse(value.ToString(), out bool result))
                return result;
            return defaultValue;
        }
        public static DateTime ConvertToDateTime(this object value, DateTime defaultValue = default)
        {
            if (value != null && value != DBNull.Value && DateTime.TryParse(value.ToString(), out DateTime result))
                return result;
            return defaultValue;
        }
        //public static string ConvertToPersianDate(this DateTime date)
        //{
        //    var persianCalendar = new PersianCalendar();
        //    int year = persianCalendar.GetYear(date);
        //    int month = persianCalendar.GetMonth(date);
        //    int day = persianCalendar.GetDayOfMonth(date);

        //    return $"{year}/{month:D2}/{day:D2}";
        //}
        //public static string ConvertToDate()
        //{
        //}
        //public static string ConvertToTimes()
        //{
        //}
        public static Guid ConvertToGuid(this object value, Guid defaultValue = default)
        {
            if (value != null && value != DBNull.Value && Guid.TryParse(value.ToString(), out Guid result))
            {
                return result;
            }
            return defaultValue;
        }
        //----------------------
        public static string GetValueOfStringColumn(this DataRow dr, string columnName, string defaultValue = "")
        {
            if (dr.HasColumn(columnName))
                return dr[columnName].ConvertToString();
            return defaultValue;
        }
        public static short GetValueOfShortColumn(this DataRow dr, string columnName, short defaultValue = 0)
        {
            if (dr.HasColumn(columnName))
                return dr[columnName].ConvertToShort();
            return defaultValue;
        }
        public static int GetValueOfIntColumn(this DataRow dr, string columnName, int defaultValue = 0)
        {
            if (dr.HasColumn(columnName))
                return dr[columnName].ConvertToInt();
            return defaultValue;
        }
        public static long GetValueOfLongColumn(this DataRow dr, string columnName, long defaultValue = 0L)
        {
            if (dr.HasColumn(columnName))
                return dr[columnName].ConvertToLong();
            return defaultValue;
        }
        public static double GetValueOfDoubleColumn(this DataRow dr, string columnName, double defaultValue = 0.0)
        {
            if (dr.HasColumn(columnName))
                return dr[columnName].ConvertToDouble();
            return defaultValue;
        }
        public static decimal GetValueOfDecimalColumn(this DataRow dr, string columnName, decimal defaultValue = 0m)
        {
            if (dr.HasColumn(columnName))
                return dr[columnName].ConvertToDecimal();
            return defaultValue;
        }
        public static bool GetValueOfBoolColumn(this DataRow dr, string columnName, bool defaultValue = false)
        {
            if (dr.HasColumn(columnName))
                return dr[columnName].ConvertToBool();
            return defaultValue;
        }
        //public static DateTime GetValueOfDateTimeColumn(this DataRow dr, string columnName, DateTime defaultValue = default)
        //{
        //    if (dr.HasColumn(columnName))
        //        return dr[columnName].ConvertToDateTime();
        //    return defaultValue;
        //}
        public static PersianDate GetValueOfPersianDateColumn(this DataRow dr, string columnName, string defaultValue = "0000/00/00")
        {
            if (dr.HasColumn(columnName))
                return new PersianDate(dr[columnName].ConvertToString());
            return new PersianDate(defaultValue);
        }
        public static PersianTime GetValueOfPersianTimeColumn(this DataRow dr, string columnName, string defaultValue = "00:00:00")
        {
            if (dr.HasColumn(columnName))
                return new PersianTime(dr[columnName].ConvertToString());
            return new PersianTime(defaultValue);
        }
        //----------------------
        private static bool HasColumn(this DataRow dr, string columnName)
        {
            return dr.Table.Columns.Contains(columnName);
        }
        public static string SetSingleQuotes(this string value)
        {
            return $"'{value}'";
        }
    }

}