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

        public DataTable GetDataTable(string query, List<SqlParameter> parameters = null)
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
            catch (Exception ex)
            {
                // ثبت خطا و بازنویسی خطای بهتر
                string parameterDetails = string.Empty;
                if (parameters != null)
                    parameterDetails = string.Join(", ", parameters.Select(p => $"{p.ParameterName}={p.Value}"));
                else
                    parameterDetails = "No parameters";

                List<SqlParameter> logParams = new List<SqlParameter>
                {
                    new SqlParameter("@Query", query + " | Parameters: " + parameterDetails),
                    new SqlParameter("@Exception", ex.Message)
                };
                ExecProcedure("[Log_Exception.Add]", logParams);
            }
            return dataTable;
        }
        public DataRow GetDataRow(string Query, List<object> Parameters = null)
        {
            try
            {
                SqlConnection sqlconnection = new SqlConnection(ConnectionStr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Query;
                if (Parameters != null && Parameters.Count > 0)
                    for (int i = 0; i < Parameters.Count; i++)
                        cmd.Parameters.Add(Parameters[i]);
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
                List<SqlParameter> Params = new List<SqlParameter>();
                Params.Clear();
                string tmp = " \r\n";
                if (Parameters != null && Parameters.Count > 0)
                {
                    for (int i = 0; i < Parameters.Count; i++)
                        tmp += (Parameters[i] as SqlParameter).ParameterName + "= " + (Parameters[i] as SqlParameter).Value.ToString() + ", ";
                }
                Params.Add(new SqlParameter("@Query", @"/*GetDataRow*/ " + "\r\n" + Query + tmp));
                Params.Add(new SqlParameter("@Exception", ex.Message));
                ExecProcedure("[Log_Select.Add]", Params);

                throw;
            }
        }
        public string GetFirstValue(string Query, List<object> Parameters = null)
        {
            try
            {
                SqlConnection sqlconnection = new SqlConnection(ConnectionStr);
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Clear();
                cmd.CommandText = Query;
                if (Parameters != null && Parameters.Count > 0)
                    for (int i = 0; i < Parameters.Count; i++)
                        cmd.Parameters.Add(Parameters[i]);
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
            catch (Exception ex)
            {
                List<SqlParameter> Params = new List<SqlParameter>();
                Params.Clear();
                string tmp = " \r\n";
                if (Parameters != null && Parameters.Count > 0)
                {
                    for (int i = 0; i < Parameters.Count; i++)
                        tmp += (Parameters[i] as SqlParameter).ParameterName + "= " + (Parameters[i] as SqlParameter).Value.ToString() + ", ";
                }
                Params.Add(new SqlParameter("@Query", @"/*GetFirstValue*/ " + "\r\n" + Query + tmp));
                Params.Add(new SqlParameter("@Exception", ex.Message));
                ExecProcedure("[Log_Select.Add]", Params);

                return null;
            }
        }
        //----------------------------------------------------------------------------------------------
        public long ExecProcedure(string ProcedureName, List<SqlParameter> parameters)
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
                        CommandText = ProcedureName
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
                    if (ProcedureName.Contains(".Add") && !ProcedureName.Contains("Log_"))
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
                // ایجاد پیام خطا برای لاگ کردن
                string title = ProcedureName;
                if (parameters != null && parameters.Count > 0)
                {
                    title += ": ";
                    foreach (var param in parameters)
                    {
                        title += param.ToString() + ", ";
                    }
                }

                // لاگ کردن خطا به دیتابیس
                List<SqlParameter> logParams = new List<SqlParameter>
                {
                    new SqlParameter("@Query", title),
                    new SqlParameter("@Exception", ex.Message)
                };
                ExecProcedure("[Log_Exception.Add]", logParams);// فراخوانی تابع لاگ خطا
                throw;// پرتاب مجدد استثنا
            }
        }
    }
}
