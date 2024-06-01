using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace Diploma.Server.DataAccess
{
    public abstract class DataController
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// The table name
        /// </summary>
        private string _tableName;

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        protected string ConnectionString => _configuration.GetConnectionString("DefaultConnection");

        /// <summary>
        /// Initializes a new instance of the <see cref="DataController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="tableName">Name of the table.</param>
        protected DataController(IConfiguration configuration, string tableName)
        {
            _configuration = configuration;
            _tableName = tableName;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected async Task<SqlConnection> GetConnectionAsync()
        {
            var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<(SqlConnection, SqlTransaction)> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var connection = await GetConnectionAsync();
            return (connection, connection.BeginTransaction(IsolationLevel.ReadUncommitted));
        }

        /// <summary>
        /// Prepares the name of the stored procedure.
        /// </summary>
        /// <param name="storedProcedureEnding">The stored procedure ending.</param>
        /// <returns></returns>
        protected string PrepareStoredProcedureName(string storedProcedureEnding)
        {
            return $"sp_{_tableName}_{storedProcedureEnding}";
        }

        /// <summary>
        /// Performs the non query.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="customName">if set to <c>true</c> [custom name].</param>
        /// <returns></returns>
        protected async Task<bool> PerformNonQuery(DynamicParameters parameters, string storedProcedureName, bool customName = false, SqlTransaction transaction = null)
        {
            string storedProcedure = customName ? storedProcedureName : PrepareStoredProcedureName(storedProcedureName);

            if (transaction != null)
            {
                var rows = await transaction.Connection.ExecuteAsync(storedProcedure, parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                return rows > 0;
            }
            else
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                    return rows > 0;
                }
            }
        }

        /// <summary>
        /// Gets the many asynchronous.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="customName">if set to <c>true</c> [custom name].</param>
        /// <returns></returns>
        protected async Task<IEnumerable<TModel>> GetManyAsync<TModel>(string storedProcedureName, DynamicParameters parameters = null, bool customName = false, SqlTransaction transaction = null) where TModel : class
        {
            string storedProcedure = customName ? storedProcedureName : PrepareStoredProcedureName(storedProcedureName);

            if (transaction != null)
            {
                var rows = await transaction.Connection.QueryAsync<TModel>(storedProcedure, parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                return rows;
            }
            else
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.QueryAsync<TModel>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                    return rows;
                }
            }
        }

        /// <summary>
        /// Gets the by parameters asynchronous.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="customName">if set to <c>true</c> [custom name].</param>
        /// <returns></returns>
        protected async Task<TModel> GetByParamsAsync<TModel>(string storedProcedureName, DynamicParameters parameters = null, bool customName = false, SqlTransaction transaction = null)
        {
            string storedProcedure = customName ? storedProcedureName : PrepareStoredProcedureName(storedProcedureName);

            if (transaction != null)
            {
                var row = await transaction.Connection.QueryFirstOrDefaultAsync<TModel>(storedProcedure, parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                return row;
            }
            else
            {
                using (var connection = await GetConnectionAsync())
                {
                    var row = await connection.QueryFirstOrDefaultAsync<TModel>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                    return row;
                }
            }
        }

        protected async Task<TModel> GetByParamsFromJsonAsync<TModel>(string storedProcedureName, DynamicParameters parameters = null, bool customName = false, SqlTransaction transaction = null)
        {
            string storedProcedure = customName ? storedProcedureName : PrepareStoredProcedureName(storedProcedureName);

            if (transaction != null)
            {
                var rows = await transaction.Connection.QueryAsync<string>(storedProcedure, parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                string tempResult = string.Empty;

                foreach (var item in rows)
                    tempResult = tempResult + item;

                var res = JsonConvert.DeserializeObject<List<TModel>>(tempResult);
                return res != null ? res.FirstOrDefault() : default(TModel);
            }
            else
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.QueryAsync<string>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                    string tempResult = string.Empty;

                    foreach (var item in rows)
                        tempResult = tempResult + item;

                    var res = JsonConvert.DeserializeObject<List<TModel>>(tempResult);
                    return res != null ? res.FirstOrDefault() : default(TModel);
                }
            }
        }

        protected async Task<IEnumerable<TModel>> GetManyFromJsonAsync<TModel>(string storedProcedureName, DynamicParameters parameters = null, bool customName = false, SqlTransaction transaction = null)
        {
            string storedProcedure = customName ? storedProcedureName : PrepareStoredProcedureName(storedProcedureName);

            if (transaction != null)
            {
                var rows = await transaction.Connection.QueryAsync<string>(storedProcedure, parameters, transaction: transaction, commandType: CommandType.StoredProcedure);
                string tempResult = string.Empty;

                foreach (var item in rows)
                    tempResult = tempResult + item;

                var res = JsonConvert.DeserializeObject<IEnumerable<TModel>>(tempResult);
                return res;
            }
            else
            {
                using (var connection = await GetConnectionAsync())
                {
                    var rows = await connection.QueryAsync<string>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                    string tempResult = string.Empty;

                    foreach (var item in rows)
                        tempResult = tempResult + item;

                    var res = JsonConvert.DeserializeObject<IEnumerable<TModel>>(tempResult);
                    return res;
                }
            }
        }

    }
}
