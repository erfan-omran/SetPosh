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
        //public void SetTable(TableEnum table)
        //{
        //    this.TableName = '[' + table.ToString() + ']';
        //}
        public void AddWith(string Query, string WithName)
        {
            WithList.Add($"{WithName} AS ({Query})");
        }
        public void AddWith(QueryBuilder qb, string WithName)
        {
            string query = qb.CreateQuery();
            WithList.Add($"{WithName} AS ({query})");
        }
        public void SetTop(long count)
        {
            TopCount = count;
        }
        public void AddColumn(string columnName, string aliasName = "")
        {
            if (aliasName == "")
                ColumnList.Add(columnName);
            else
                ColumnList.Add(columnName + " AS " + aliasName);
        }
        public void AddColumns(params string[] columnNames)
        {
            ColumnList.AddRange(columnNames);
        }
        public void AddColumns(List<string> columnNames)
        {
            ColumnList.AddRange(columnNames);
        }

        public void AddJoin(string joinType, string table, string onCondition, string aliasName = "")
        {
            if (aliasName == "")
                JoinClauseList.Add($"{joinType} JOIN {table} ON {onCondition}");
            else
                JoinClauseList.Add($"{joinType} JOIN {table} AS {aliasName} ON {onCondition}");
        }
        public void AddJoin(string joinType, string table, Action<QueryBuilder> configureQB, string aliasName = "")
        {
            QueryBuilder tempQueryBuilder = new QueryBuilder();
            configureQB(tempQueryBuilder);
            if (aliasName == "")
                JoinClauseList.Add($"{joinType} JOIN {table} ON ({string.Join(" AND ", tempQueryBuilder.ConditionList)})");
            else
                JoinClauseList.Add($"{joinType} JOIN {table} AS {aliasName} ON ({string.Join(" AND ", tempQueryBuilder.ConditionList)})");
        }

        public void AddInnerJoin(string table, string onCondition, string aliasName = "") => AddJoin("INNER", table, onCondition, aliasName);
        public void AddInnerJoin(string table, Action<QueryBuilder> qb, string aliasName = "") => AddJoin("INNER", table, qb, aliasName);

        public void AddLeftJoin(string table, string onCondition, string aliasName = "") => AddJoin("LEFT", table, onCondition, aliasName);
        public void AddLeftJoin(string table, Action<QueryBuilder> qb, string aliasName = "") => AddJoin("LEFT", table, qb, aliasName);

        public void AddRightJoin(string table, string onCondition, string aliasName = "") => AddJoin("RIGHT", table, onCondition, aliasName);
        public void AddRightJoin(string table, Action<QueryBuilder> qb, string aliasName = "") => AddJoin("RIGHT", table, qb, aliasName);

        public void AddFullJoin(string table, string onCondition, string aliasName = "") => AddJoin("Full", table, onCondition, aliasName);
        public void AddFullJoin(string table, Action<QueryBuilder> qb, string aliasName = "") => AddJoin("FULL", table, qb, aliasName);

        public void AddCondition(string condition)
        {
            ConditionList.Add(condition);
        }
        public void AddEqualCondition(string columnName, object value)
        {
            ConditionList.Add($"{columnName} = {value}");
        }
        public void AddGreaterThanCondition(string columnName, object value)
        {
            ConditionList.Add($"{columnName} > {value}");
        }
        public void AddLessThanCondition(string columnName, object value)
        {
            ConditionList.Add($"{columnName} < {value}");
        }
        public void AddGreaterThanOrEqualCondition(string columnName, object value)
        {
            ConditionList.Add($"{columnName} >= '{value}'");
        }
        public void AddLessThanOrEqualCondition(string columnName, object value)
        {
            ConditionList.Add($"{columnName} <= '{value}'");
        }
        public void AddNotEqualCondition(string columnName, object value)
        {
            ConditionList.Add($"{columnName} != '{value}'");
        }
        public void AddLikeCondition(string columnName, object value)
        {
            ConditionList.Add($"{columnName} LIKE '%{value}%'");
        }
        public void AddLikeConditionStartsWith(string columnName, object value)
        {
            ConditionList.Add($"{columnName} LIKE '{value}%'");
        }
        public void AddLikeConditionEndsWith(string columnName, object value)
        {
            ConditionList.Add($"{columnName} LIKE '%{value}'");
        }

        public void AddInCondition(string columnName, IEnumerable<object> values)
        {
            ConditionList.Add($"{columnName} IN ({string.Join(",", values)})");
        }
        public void AddInCondition(string columnName, params object[] values)
        {
            ConditionList.Add($"{columnName} IN ({string.Join(",", values)})");
        }
        public void AddInCondition(string columnName, string valueString)
        {
            string[] values = valueString.Split(',');
            AddInCondition(columnName, values);
        }
        public void AddBetweenCondition(string columnName, object value1, object value2)
        {
            ConditionList.Add($"{columnName} BETWEEN {value1} AND {value2}");
        }

        public void AddGroupBy(string columnName)
        {
            GroupByList.Add(columnName);
        }
        public void AddGroupBy(params string[] columnNames)
        {
            GroupByList.AddRange(columnNames);
        }
        public void AddGroupBy(List<string> columnNames)
        {
            GroupByList.AddRange(columnNames);
        }
        public void AddHaving(string condition)
        {
            HavingList.Add(condition);
        }
        public void AddOrderBy(string column, bool IsASC = true)
        {
            string direction = IsASC ? "ASC" : "DESC";
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
