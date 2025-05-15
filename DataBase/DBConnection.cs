using System.Data;
using System.Data.SqlClient;

namespace DataBase
{
    public static class DBConnection
    {
        private static string _setPoshConnectionString = "";

        //public DBConnection(string configuration)
        //{
        //    _setPoshConnectionString = configuration;
        //}
        //public DBConnection(IConfiguration configuration)
        //{
        //    _setPoshConnectionString = configuration.GetConnectionString("SetPoshConnection");
        //}
        public static void InitConnectionStr(string configuration)
        {
            _setPoshConnectionString = configuration ?? "";
        }
        public static async Task<DataTable> GetDataTableAsync(string query, List<SqlParameter>? parameters = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_setPoshConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.Text;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }
                        await sqlConnection.OpenAsync();
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
                        {
                            sqlDataAdapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex, query, parameters);
            }
            return dataTable;
        }
        public static async Task<DataRow?> GetDataRowAsync(QueryBuilder qb, List<SqlParameter>? parameters = null)
        {
            qb.SetTop(1);
            string query = qb.CreateQuery();
            DataRow? row = await GetDataRowAsync(query, parameters);
            return row;
        }
        public static async Task<DataRow?> GetDataRowAsync(string query, List<SqlParameter>? parameters = null)
        {
            DataTable dataTable = await GetDataTableAsync(query, parameters);
            return dataTable.Rows.Count > 0 ? dataTable.Rows[0] : null;
        }
        public static async Task<T?> GetFirstValueAsync<T>(string query, List<SqlParameter>? parameters = null)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_setPoshConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.Text;
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }
                        await sqlConnection.OpenAsync();
                        object? result = await cmd.ExecuteScalarAsync();
                        return /*result is null || result is DBNull ? default :*/ (T?)result;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex, query, parameters);
                //return default;
                throw new Exception(ex.Message);
            }
        }
        //-----------------------------------------------
        public static async Task<bool> ExecProcedureAsync(string procedureName, List<SqlParameter> parameters)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_setPoshConnectionString))
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandTimeout = 300,
                        Connection = sqlConnection,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = procedureName
                    };

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    if (sqlConnection.State == ConnectionState.Closed)
                        await sqlConnection.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<bool> ExecTransactionProcedureAsync(string procedureName, List<SqlParameter> parameters)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_setPoshConnectionString))
                {
                    await sqlConnection.OpenAsync();

                    using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(procedureName, sqlConnection, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 300;

                                if (parameters != null && parameters.Count > 0)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddRange(parameters.ToArray());
                                }

                                await cmd.ExecuteNonQueryAsync();
                            }

                            await transaction.CommitAsync();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            await LogException(ex, "ExecTransactionProcedureAsync (Single Procedure)", null);
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await LogException(ex, "ExecTransactionProcedureAsync (Connection Error - Single Procedure)", null);
                throw new Exception(ex.Message);
            }
        }

        public static async Task<T> ExecProcedureAsync<T>(string procedureName, List<SqlParameter> parameters, string OutputParamName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_setPoshConnectionString))
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandTimeout = 300,
                        Connection = sqlConnection,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = procedureName
                    };

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    SqlParameter outputParam = new SqlParameter("@" + OutputParamName, typeof(T).ToSqlDbType())
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    if (sqlConnection.State == ConnectionState.Closed)
                        await sqlConnection.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();
                    return (T)Convert.ChangeType(outputParam.Value, typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<T> ExecTransactionProcedureAsync<T>(string procedureName, List<SqlParameter> parameters, string OutputParamName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_setPoshConnectionString))
                {
                    await sqlConnection.OpenAsync();

                    using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(procedureName, sqlConnection, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 300;

                                if (parameters != null && parameters.Count > 0)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddRange(parameters.ToArray());
                                }
                                SqlParameter outputParam = new SqlParameter("@" + OutputParamName, typeof(T).ToSqlDbType())
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(outputParam);

                                await cmd.ExecuteNonQueryAsync();

                                await transaction.CommitAsync();
                                return (T)Convert.ChangeType(outputParam.Value, typeof(T));
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            await LogException(ex, "ExecTransactionProcedureAsync (Single Procedure)", null);
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await LogException(ex, "ExecTransactionProcedureAsync (Connection Error - Single Procedure)", null);
                throw new Exception(ex.Message);
            }
        }

        //public static async Task<bool> ExecTransactionMultiProcedureAsync(List<(string ProcedureName, List<SqlParameter> Parameters)> procedures)
        //{
        //    try
        //    {
        //        using (SqlConnection sqlConnection = new SqlConnection(_setPoshConnectionString))
        //        {
        //            await sqlConnection.OpenAsync();

        //            using (SqlTransaction transaction = sqlConnection.BeginTransaction())
        //            {
        //                try
        //                {
        //                    foreach (var procedure in procedures)
        //                    {
        //                        using (SqlCommand cmd = new SqlCommand(procedure.ProcedureName, sqlConnection, transaction))
        //                        {
        //                            cmd.CommandType = CommandType.StoredProcedure;
        //                            cmd.CommandTimeout = 300;

        //                            if (procedure.Parameters != null && procedure.Parameters.Count > 0)
        //                            {
        //                                cmd.Parameters.AddRange(procedure.Parameters.ToArray());
        //                            }
        //                            await cmd.ExecuteNonQueryAsync();
        //                        }
        //                    }
        //                    await transaction.CommitAsync();
        //                    return true;
        //                }
        //                catch (Exception ex)
        //                {
        //                    await transaction.RollbackAsync();
        //                    await LogException(ex, "ExecuteTransactionAsync (Procedures)", null);
        //                    return false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await LogException(ex, "ExecuteTransactionAsync (Connection Error - Procedures)", null);
        //        return false;
        //    }
        //}
        public static async Task<bool> ExecTransactionMultiProcedureAsync(List<(string ProcedureName, List<SqlParameter> Parameters, bool ReturnsValue)> procedures)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_setPoshConnectionString))
                {
                    await sqlConnection.OpenAsync();

                    using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            long NewID = 0;

                            foreach (var procedure in procedures)
                            {
                                using (SqlCommand cmd = new SqlCommand(procedure.ProcedureName, sqlConnection, transaction))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandTimeout = 300;

                                    if (procedure.Parameters != null && procedure.Parameters.Count > 0)
                                    {
                                        if (NewID > 0)
                                        {
                                            foreach (var param in procedure.Parameters)
                                            {
                                                if (param.Value.ToString() == SqlDbType.BigInt.ToString())
                                                {
                                                    param.Value = NewID;
                                                }
                                            }
                                        }
                                        cmd.Parameters.AddRange(procedure.Parameters.ToArray());
                                    }

                                    if (procedure.ReturnsValue)
                                    {
                                        using (var reader = await cmd.ExecuteReaderAsync())
                                        {
                                            if (reader.Read())
                                            {
                                                NewID = reader.GetInt64(0);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await cmd.ExecuteNonQueryAsync();
                                    }
                                }
                            }

                            await transaction.CommitAsync();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            await LogException(ex, "ExecTransactionMultiProcedureAsync", null);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await LogException(ex, "ExecTransactionMultiProcedureAsync (Connection Error)", null);
                return false;
            }
        }

        public static async Task<bool> LogException(Exception ex, string query, List<SqlParameter>? parameters = null)
        {
            return await LogException(ex.Message, query, parameters);
        }
        public static async Task<bool> LogException(string errorMessage, string query, List<SqlParameter>? parameters = null)
        {
            string parameterDetails = "";
            if (parameters != null)
                parameterDetails = string.Join(", ", parameters.Select(p => $"{p.ParameterName}={p.Value}"));
            else
                parameterDetails = "No parameters";

            List<SqlParameter> logParams = new List<SqlParameter>
            {
                new SqlParameter("@Query", query + " | Parameters: " + parameterDetails),
                new SqlParameter("@Exception", errorMessage)
            };
            bool result = await ExecProcedureAsync("[Log_Exception.Add]", logParams);

            return result;
        }

        //---------------------
        private static SqlDbType ToSqlDbType(this Type type)
        {
            if (type == typeof(int)) return SqlDbType.Int;
            if (type == typeof(long)) return SqlDbType.BigInt;
            if (type == typeof(string)) return SqlDbType.NVarChar;
            if (type == typeof(bool)) return SqlDbType.Bit;
            if (type == typeof(DateTime)) return SqlDbType.DateTime;
            if (type == typeof(float)) return SqlDbType.Float;
            if (type == typeof(double)) return SqlDbType.Float;
            if (type == typeof(decimal)) return SqlDbType.Decimal;
            throw new ArgumentException("Unsupported type");
        }
    }
}

