using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    public class QueryBuilder
    {
        private string TableName = string.Empty;
        private List<string> WithList = new List<string>();
        private List<string> ColumnList = new List<string>();
        private List<string> JoinClauseList = new List<string>();
        private List<string> ConditionList = new List<string>();
        private List<string> GroupByList = new List<string>();
        private List<string> HavingList = new List<string>();
        private List<string> OrderByList = new List<string>();
        private long? TopCount = null;

        public void SetTable(string tableName)
        {
            this.TableName = tableName;
        }
        public void AddWith(string WithName, string Query)
        {
            WithList.Add($"{WithName} AS ({Query})");
        }
        public void SetTop(long count)
        {
            TopCount = count;
        }
        public void AddColumn(string columnName)
        {
            ColumnList.Add(columnName);
        }

        public void AddJoin(string joinType, string table, string onCondition)
        {
            JoinClauseList.Add($"{joinType} JOIN {table} ON {onCondition}");
        }
        public void AddInnerJoin(string table, string onCondition) => AddJoin("INNER", table, onCondition);
        public void AddLeftJoin(string table, string onCondition) => AddJoin("LEFT", table, onCondition);
        public void AddRightJoin(string table, string onCondition) => AddJoin("RIGHT", table, onCondition);
        public void AddFullJoin(string table, string onCondition) => AddJoin("Full", table, onCondition);

        public void AddCondition(string condition)
        {
            ConditionList.Add(condition);
        }
        public void AddEqualCondition(string columnName, string value)
        {
            ConditionList.Add($"{columnName} = '{value}'");
        }
        public void AddGreaterThanCondition(string columnName, string value)
        {
            ConditionList.Add($"{columnName} > '{value}'");
        }
        public void AddLessThanCondition(string columnName, string value)
        {
            ConditionList.Add($"{columnName} < '{value}'");
        }
        public void AddGreaterThanOrEqualCondition(string columnName, string value)
        {
            ConditionList.Add($"{columnName} >= '{value}'");
        }
        public void AddLessThanOrEqualCondition(string columnName, string value)
        {
            ConditionList.Add($"{columnName} <= '{value}'");
        }
        public void AddNotEqualCondition(string columnName, string value)
        {
            ConditionList.Add($"{columnName} != '{value}'");
        }
        public void AddLikeConditionContains(string columnName, string value)
        {
            ConditionList.Add($"{columnName} LIKE '%{value}%'");
        }
        public void AddLikeConditionStartsWith(string columnName, string value)
        {
            ConditionList.Add($"{columnName} LIKE '{value}%'");
        }
        public void AddLikeConditionEndsWith(string columnName, string value)
        {
            ConditionList.Add($"{columnName} LIKE '%{value}'");
        }
        public void AddInCondition(string columnName, IEnumerable<object> values)
        {
            var valueString = string.Join(",", values.Select(v =>
                v is string ? $"'{v}'" : v.ToString()));  // برای stringها تک کوتیشن و برای بقیه مقادیر مستقیم
            ConditionList.Add($"{columnName} IN ({valueString})");
        }
        public void AddInCondition(string columnName, string valueString)
        {
            string[] values = valueString.Split(',');
            AddInCondition(columnName, values);
        }
        public void AddBetweenCondition(string columnName, string value1, string value2)
        {
            ConditionList.Add($"{columnName} BETWEEN '{value1}' AND '{value2}'");
        }

        public void AddGroupBy(string column)
        {
            GroupByList.Add(column);
        }
        public void AddHaving(string condition)
        {
            HavingList.Add(condition);
        }
        public void AddOrderBy(string column, string direction = "ASC")
        {
            OrderByList.Add($"{column} {direction}");
        }

        public string CreateQuery()
        {
            if (string.IsNullOrEmpty(TableName))
                throw new InvalidOperationException("Table name is not set.");

            StringBuilder Query = new StringBuilder();

            if (WithList.Count > 0)
            {
                Query.Append("WITH ");
                Query.Append(string.Join(",\n\t", WithList));
                Query.Append(" \n");
            }

            Query.Append("SELECT ");
            if (TopCount.HasValue)
                Query.Append($"TOP {TopCount.Value} ");

            Query.Append(ColumnList.Count > 0 ? string.Join(",\n\t", ColumnList) : "*");
            Query.Append($"\nFROM {TableName} \n");

            if (JoinClauseList.Count > 0)
                Query.Append(string.Join("\n\t", JoinClauseList) + " \n");
            if (ConditionList.Count > 0)
                Query.Append($"WHERE {string.Join(" AND \n\t", ConditionList)} \n");
            if (GroupByList.Count > 0)
                Query.Append($"GROUP BY {string.Join(", \n\t", GroupByList)} \n");
            if (HavingList.Count > 0)
                Query.Append($"HAVING {string.Join(" AND \n\t", HavingList)} \n");
            if (OrderByList.Count > 0)
                Query.Append($"ORDER BY {string.Join(", \n\t", OrderByList)} \n");

            return Query.ToString().Trim();
        }
    }
}
