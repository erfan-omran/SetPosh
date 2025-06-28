using System.Data;

namespace DataBase
{
    public static class SqlFunction
    {
        #region Aggregation Functions
        public static string Count(string expression)
        {
            return $"COUNT({expression})";
        }
        public static string Sum(string expression)
        {
            return $"SUM({expression})";
        }
        public static string Avg(string expression)
        {
            return $"AVG({expression})";
        }
        public static string Min(string expression)
        {
            return $"MIN({expression})";
        }
        public static string Max(string expression)
        {
            return $"MAX({expression})";
        }
        public static string Stdev(string expression)
        {
            return $"STDEV({expression})";
        }
        public static string Var(string expression)
        {
            return $"VAR({expression})";
        }
        #endregion

        #region Condition Functions
        public static string EqualCondition(string columnName, string value)
        {
            return $"{columnName} = '{value}'";
        }

        public static string GreaterThanCondition(string columnName, string value)
        {
            return $"{columnName} > '{value}'";
        }

        public static string LessThanCondition(string columnName, string value)
        {
            return $"{columnName} < '{value}'";
        }

        public static string GreaterThanOrEqualCondition(string columnName, string value)
        {
            return $"{columnName} >= '{value}'";
        }

        public static string LessThanOrEqualCondition(string columnName, string value)
        {
            return $"{columnName} <= '{value}'";
        }

        public static string NotEqualCondition(string columnName, string value)
        {
            return $"{columnName} != '{value}'";
        }

        public static string LikeConditionContains(string columnName, string value)
        {
            return $"{columnName} LIKE '%{value}%'";
        }

        public static string LikeConditionStartsWith(string columnName, string value)
        {
            return $"{columnName} LIKE '{value}%'";
        }

        public static string LikeConditionEndsWith(string columnName, string value)
        {
            return $"{columnName} LIKE '%{value}'";
        }

        public static string InCondition(string columnName, params string[] values)
        {
            return $"{columnName} IN ({string.Join(", ", values.Select(v => $"'{v}'"))})";
        }

        public static string BetweenCondition(string columnName, string startValue, string endValue)
        {
            return $"{columnName} BETWEEN '{startValue}' AND '{endValue}'";
        }
        #endregion

        #region Mathematical Functions
        public static string Abs(string expression)
        {
            return $"ABS({expression})";
        }
        public static string Ceiling(string expression)
        {
            return $"CEILING({expression})";
        }
        public static string Floor(string expression)
        {
            return $"FLOOR({expression})";
        }
        public static string Round(string expression, int decimalPlaces)
        {
            return $"ROUND({expression}, {decimalPlaces})";
        }
        public static string Power(string expression, double exponent)
        {
            return $"POWER({expression}, {exponent})";
        }
        public static string Sqrt(string expression)
        {
            return $"SQRT({expression})";
        }
        public static string Pi()
        {
            return "PI()";
        }
        public static string Exp(string expression)
        {
            return $"EXP({expression})";
        }
        public static string Log(string expression)
        {
            return $"LOG({expression})";
        }
        public static string Log10(string expression)
        {
            return $"LOG10({expression})";
        }
        public static string Random()
        {
            return "RAND()";
        }
        #endregion

        #region String Functions
        public static string Len(string expression)
        {
            return $"LEN({expression})";
        }
        public static string LTrim(string expression)
        {
            return $"LTRIM({expression})";
        }
        public static string RTrim(string expression)
        {
            return $"RTRIM({expression})";
        }
        public static string Upper(string expression)
        {
            return $"UPPER({expression})";
        }
        public static string Lower(string expression)
        {
            return $"LOWER({expression})";
        }
        public static string Concat(params string[] expressions)
        {
            return $"CONCAT({string.Join(", ", expressions)})";
        }
        public static string Replace(string expression, string oldValue, string newValue)
        {
            return $"REPLACE({expression}, {oldValue}, {newValue})";
        }
        public static string Substring(string expression, int startIndex, int length)
        {
            return $"SUBSTRING({expression}, {startIndex}, {length})";
        }
        public static string CharIndex(string substring, string expression)
        {
            return $"CHARINDEX({substring}, {expression})";
        }
        public static string PatIndex(string pattern, string expression)
        {
            return $"PATINDEX({pattern}, {expression})";
        }
        public static string Left(string expression, int length)
        {
            return $"LEFT({expression}, {length})";
        }
        public static string Right(string expression, int length)
        {
            return $"RIGHT({expression}, {length})";
        }
        public static string Format(string expression, string format)
        {
            return $"FORMAT({expression}, {format})";
        }
        #endregion

        #region Date and Time Functions
        public static string GetDate()
        {
            return "GETDATE()";
        }
        public static string CurrentTimestamp()
        {
            return "CURRENT_TIMESTAMP";
        }
        public static string SysDateTime()
        {
            return "SYSDATETIME()";
        }
        public static string DatePart(string datePart, string date)
        {
            return $"DATEPART({datePart}, {date})";
        }
        public static string DateDiff(string datePart, string startDate, string endDate)
        {
            return $"DATEDIFF({datePart}, {startDate}, {endDate})";
        }
        public static string DateAdd(string datePart, int number, string date)
        {
            return $"DATEADD({datePart}, {number}, {date})";
        }
        public static string Year(string date)
        {
            return $"YEAR({date})";
        }
        public static string Month(string date)
        {
            return $"MONTH({date})";
        }
        public static string Day(string date)
        {
            return $"DAY({date})";
        }
        public static string DateFromParts(int year, int month, int day)
        {
            return $"DATEFROMPARTS({year}, {month}, {day})";
        }
        public static string TimeFromParts(int hour, int minute, int second)
        {
            return $"TIMEFROMPARTS({hour}, {minute}, {second})";
        }
        public static string DateTimeFromParts(int year, int month, int day, int hour, int minute, int second)
        {
            return $"DATETIMEFROMPARTS({year}, {month}, {day}, {hour}, {minute}, {second})";
        }
        public static string DateName(string datePart, string date)
        {
            return $"DATENAME({datePart}, {date})";
        }
        #endregion

        #region Conversion Functions
        public static string Cast(string expression, string targetType)
        {
            return $"CAST({expression} AS {targetType})";
        }
        public static string Convert(string expression, string targetType)
        {
            return $"CONVERT({targetType}, {expression})";
        }
        public static string Parse(string expression, string targetType)
        {
            return $"PARSE({expression} AS {targetType})";
        }
        public static string TryCast(string expression, string targetType)
        {
            return $"TRY_CAST({expression} AS {targetType})";
        }
        public static string TryConvert(string expression, string targetType)
        {
            return $"TRY_CONVERT({targetType}, {expression})";
        }
        public static string TryParse(string expression, string targetType)
        {
            return $"TRY_PARSE({expression} AS {targetType})";
        }
        #endregion

        #region Logical Functions
        public static string Coalesce(params string[] expressions)
        {
            return $"COALESCE({string.Join(", ", expressions)})";
        }
        public static string IsNull(string expression1, object expression2)
        {
            return $"IsNull({expression1}, {expression2})";
        }
        public static string NullIf(string expression1, string expression2)
        {
            return $"NULLIF({expression1}, {expression2})";
        }
        public static string IIf(string condition, string truePart, string falsePart)
        {
            return $"IIF({condition}, {truePart}, {falsePart})";
        }
        public static string Case(string condition, string truePart, string falsePart)
        {
            return $"CASE WHEN {condition} THEN {truePart} ELSE {falsePart} END";
        }
        #endregion

        #region Ranking Functions
        public static string RowNumber()
        {
            return "ROW_NUMBER()";
        }
        public static string Rank()
        {
            return "RANK()";
        }
        public static string DenseRank()
        {
            return "DENSE_RANK()";
        }
        public static string NTile(int n)
        {
            return $"NTILE({n})";
        }
        #endregion

        #region Security Functions
        public static string HashBytes(string algorithm, string expression)
        {
            return $"HASHBYTES({algorithm}, {expression})";
        }
        public static string NewId()
        {
            return "NEWID()";
        }
        #endregion

        #region System Functions
        public static string ColumnProperty(string tableName, string columnName, string property)
        {
            return $"COLUMNPROPERTY({tableName}, {columnName}, {property})";
        }
        public static string DatabasePropertyEx(string property)
        {
            return $"DATABASEPROPERTYEX({property})";
        }
        public static string ObjectProperty(string objectName, string property)
        {
            return $"OBJECTPROPERTY({objectName}, {property})";
        }
        public static string ServerProperty(string property)
        {
            return $"SERVERPROPERTY({property})";
        }
        #endregion

        //-----------------------
        public static bool HasColumn(IDataRecord reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool HasColumn(DataRow dataRow, string columnName)
        {
            return dataRow.Table.Columns.Contains(columnName);
        }
    }
}
