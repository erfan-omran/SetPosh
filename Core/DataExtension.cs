namespace Core
{
    public static class DataExtension
    {
        public static string ConvertToString(this object value, string defaultValue = "")
        {
            if (value != null && value != DBNull.Value)
            {
                return value.ToString();
            }
            return defaultValue;
        }
        public static short ConvertToShort(this object value, short defaultValue = 0)
        {
            if (value != null && value != DBNull.Value && short.TryParse(value.ToString(), out short result))
            {
                return result;
            }
            return defaultValue;
        }
        public static int ConvertToInt(this object value, int defaultValue = 0)
        {
            if (value != null && value != DBNull.Value && int.TryParse(value.ToString(), out int result))
            {
                return result;
            }
            return defaultValue;
        }
        public static long ConvertToLong(this object value, long defaultValue = 0)
        {
            if (value != null && value != DBNull.Value && long.TryParse(value.ToString(), out long result))
            {
                return result;
            }
            return defaultValue;
        }
        public static float ConvertToFloat(this object value, float defaultValue = 0f)
        {
            if (value != null && value != DBNull.Value && float.TryParse(value.ToString(), out float result))
            {
                return result;
            }
            return defaultValue;
        }
        public static double ConvertToDouble(this object value, double defaultValue = 0.0)
        {
            if (value != null && value != DBNull.Value && double.TryParse(value.ToString(), out double result))
            {
                return result;
            }
            return defaultValue;
        }
        public static decimal ConvertToDecimal(this object value, decimal defaultValue = 0.0M)
        {
            if (value != null && value != DBNull.Value && decimal.TryParse(value.ToString(), out decimal result))
            {
                return result;
            }
            return defaultValue;
        }
        public static bool ConvertToBool(this object value, bool defaultValue = false)
        {
            if (value != null && value != DBNull.Value && bool.TryParse(value.ToString(), out bool result))
            {
                return result;
            }
            return defaultValue;
        }
        public static DateTime ConvertToDateTime(this object value, DateTime defaultValue = default)
        {
            if (value != null && value != DBNull.Value && DateTime.TryParse(value.ToString(), out DateTime result))
            {
                return result;
            }
            return defaultValue;
        }
        public static Guid ConvertToGuid(this object value, Guid defaultValue = default)
        {
            if (value != null && value != DBNull.Value && Guid.TryParse(value.ToString(), out Guid result))
            {
                return result;
            }
            return defaultValue;
        }

    }

}