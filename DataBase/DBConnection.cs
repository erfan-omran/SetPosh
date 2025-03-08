using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DataBase
{
    public class DBConnection
    {
        private string ConnectionStr { get; set; } = string.Empty;
        public DBConnection()
        {
            // بارگذاری تنظیمات از appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // تنظیم مسیر
                .AddJsonFile("appsettings.json") // افزودن فایل appsettings.json
                .Build();

            // دسترسی به رشته اتصال
            ConnectionStr = configuration.GetConnectionString("SetPosh");
        }

        public DataTable GetDataTable(string query, List<SqlParameter>? parameters = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.Text;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }
                        // باز کردن اتصال
                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
                        {
                            sqlDataAdapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex) { ExecLogExceptionProcedure(ex, query, parameters); }

            return dataTable;
        }
        public DataRow? GetDataRow(string query, List<SqlParameter>? parameters = null)
        {
            try
            {
                SqlConnection sqlconnection = new SqlConnection(ConnectionStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                if (parameters != null && parameters.Count > 0)
                    for (int i = 0; i < parameters.Count; i++)
                        cmd.Parameters.Add(parameters[i]);
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlconnection;
                DataTable dataTable = new DataTable();
                if (sqlconnection.State == ConnectionState.Closed)
                    sqlconnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataTable);
                sqlDataAdapter.Dispose();
                sqlconnection.Close();
                if (dataTable.Rows.Count > 0)
                    return dataTable.Rows[0];
                return null;
            }
            catch (Exception ex)
            {
                ExecLogExceptionProcedure(ex, query, parameters);
                throw;
            }
        }
        public string? GetFirstValue(string query, List<SqlParameter>? parameters = null)
        {
            try
            {
                SqlConnection sqlconnection = new SqlConnection(ConnectionStr);
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Clear();
                cmd.CommandText = query;
                if (parameters != null && parameters.Count > 0)
                    for (int i = 0; i < parameters.Count; i++)
                        cmd.Parameters.Add(parameters[i]);
                cmd.CommandTimeout = 300;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlconnection;
                DataTable dataTable = new DataTable();
                if (sqlconnection.State == ConnectionState.Closed)
                    sqlconnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataTable);
                sqlDataAdapter.Dispose();
                sqlconnection.Close();
                if (dataTable.Rows.Count > 0)
                    return dataTable.Rows[0][0].ToString();
                return "";
            }
            catch (Exception ex) { ExecLogExceptionProcedure(ex, query, parameters); }

            return null;
        }
        public List<T> GetModels<T>(string query, List<SqlParameter>? parameters = null) where T : new()
        {
            List<T> result = new List<T>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.Text;

                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }

                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T obj = new T();
                                foreach (var property in typeof(T).GetProperties())
                                {
                                    if (HasColumn(reader, property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                    {
                                        property.SetValue(obj, reader[property.Name]);
                                    }
                                }
                                result.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { ExecLogExceptionProcedure(ex, query, parameters); }

            return result;
        }
        public T? GetModel<T>(string query, List<SqlParameter>? parameters = null) where T : new()
        {
            T result = new T();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.Text;

                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }

                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // فقط یک ردیف را پردازش می‌کند
                            {
                                T obj = new T();
                                foreach (var property in typeof(T).GetProperties())
                                {
                                    if (HasColumn(reader, property.Name) && !reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                    {
                                        property.SetValue(obj, reader[property.Name]);
                                    }
                                }
                                return obj;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { ExecLogExceptionProcedure(ex, query, parameters); }

            return default(T); // اگر هیچ داده‌ای یافت نشود، مقدار پیش‌فرض برمی‌گرداند
        }

        //----------------------------------------------------------------------------------------------
        public long ExecProcedure(string procedureName, List<SqlParameter> parameters)
        {
            try
            {
                // ایجاد اتصال به دیتابیس
                using (SqlConnection sqlConnection = new SqlConnection(ConnectionStr))
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandTimeout = 180, // تایم‌اوت 180 ثانیه
                        Connection = sqlConnection,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = procedureName
                    };

                    // افزودن پارامترها به فرمان SQL
                    if (parameters != null && parameters.Count > 0)
                    {
                        //cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }
                    // باز کردن اتصال
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    cmd.ExecuteNonQuery();

                    // اگر پروسیجر مربوط به افزودن است و نیاز به برگشتن SID دارد
                    if (procedureName.Contains(".Add") && !procedureName.Contains("Log_"))
                    {
                        // اطمینان از وجود پارامتر SID
                        if (cmd.Parameters.Contains("@SID"))
                        {
                            object SID = cmd.Parameters["@SID"].Value;
                            return SID is long ? (long)SID : -1; // برگشت SID یا -1 اگر SID به درستی دریافت نشد
                        }
                    }
                }
                return -1; // در صورتی که پروسیجر نتیجه‌ای نداشته باشد
            }
            catch (Exception ex)
            {
                ExecLogExceptionProcedure(ex, "ExecProcedure Func", parameters);
                throw;// پرتاب مجدد استثنا
            }
        }
        public bool ExecLogExceptionProcedure(Exception ex, string query, List<SqlParameter>? parameters = null)
        {
            string parameterDetails = parameters != null
                    ? string.Join(", ", parameters.Select(p => $"{p.ParameterName}={p.Value}"))
                    : "No parameters";

            List<SqlParameter> logParams = new List<SqlParameter>
                {
                    new SqlParameter("@Query", query + " | Parameters: " + parameterDetails),
                    new SqlParameter("@Exception", ex.Message)
                };
            long result = ExecProcedure("[Log_Exception.Add]", logParams);

            return result >= 0;
        }
        //----------------------------------------------------------------------------------------------
        private bool HasColumn(IDataRecord reader, string columnName)
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
    }
}
