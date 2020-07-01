using Dapper;
using Dapper.Contrib.Extensions;
using HZY.Models.Common;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace HZY.DapperCore.Dapper
{
    public class DapperClient
    {
        public ConnectionConfig CurrentConnectionConfig { get; set; }

        public DapperClient(IOptionsMonitor<ConnectionConfig> config)
        {
            CurrentConnectionConfig = config.CurrentValue;
        }

        public DapperClient(ConnectionConfig config)
        {
            CurrentConnectionConfig = config;
        }

        private IDbConnection _connection = null;

        public IDbConnection Connection
        {
            get
            {
                switch (CurrentConnectionConfig.DbType)
                {
                    case DbStoreType.MySql:
                        //_connection = new MySqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    //case DbStoreType.Sqlite:
                    //    _connection = new SQLiteConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    case DbStoreType.SqlServer:
                        _connection = new SqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    //case DbStoreType.Oracle:
                    //    _connection = new Oracle.ManagedDataAccess.Client.OracleConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    default:
                        throw new Exception("未指定数据库类型！");
                }
                return _connection;
            }
        }

        public async Task<bool> InsertAsync<T>(T t) where T : class
        {
            using (IDbConnection connection = Connection)
            {
                return await connection.InsertAsync(t) > 0;
            }
        }

        public async Task<bool> DeleteAsync<T>(T t) where T : class
        {
            using (IDbConnection connection = Connection)
            {
                return await connection.DeleteAsync(t);
            }
        }

        public async Task<bool> UpdateAsync<T>(T t) where T : class
        {
            using (IDbConnection connection = Connection)
            {
                return await connection.UpdateAsync(t);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            using (IDbConnection connection = Connection)
            {
                return await connection.GetAllAsync<T>();
            }
        }

        public async Task<T> GetByIDAsync<T>(int id) where T : class
        {
            using (IDbConnection connection = Connection)
            {
                return await connection.GetAsync<T>(id);
            }
        }

        public async Task<int> ExecuteAsync(string path)
        {
            using (IDbConnection connection = Connection)
            {
                using (StreamReader streamReader = new StreamReader(path, System.Text.Encoding.UTF8))
                {
                    var script = await streamReader.ReadToEndAsync();
                    return await connection.ExecuteAsync(script);
                }
            }
        }

        public async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using (IDbConnection connection = Connection)
            {
                return await connection.ExecuteAsync(sql, param);
            }
        }

        public async Task<bool> ExecuteAsyncTransaction(List<string> list)
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();

                IDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (var sql in list)
                    {
                        await connection.ExecuteAsync(sql, null, transaction);
                    }

                    transaction.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    return false;
                }
            }
        }

        public async Task<bool> ExecuteAsyncTransaction(List<KeyValuePair<string, object>> list)
        {
            using (IDbConnection connection = Connection)
            {
                connection.Open();

                IDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (var item in list)
                    {
                        await connection.ExecuteAsync(item.Key, item.Value, transaction);
                    }

                    transaction.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    return false;
                }
            }
        }

        public async Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null)
        {
            using (IDbConnection connection = Connection)
            {
                return await connection.QueryAsync(sql, param);
            }
        }

        public async Task<dynamic> QueryFirstOrDefaultAsync(string sql, object param = null)
        {
            using (IDbConnection connection = Connection)
            {
                return await connection.QueryFirstOrDefaultAsync(sql, param);
            }
        }


        #region SELECT
        //public List<T> GetList<T>(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        //{
        //    var list = new List<T>();
        //    using (IDbConnection connection = Connection)
        //    {
        //        connection.Open();
        //        IEnumerable<T> ts = null;
        //        if (null == param)
        //        {
        //            connection.Query<T>(sqlString, null, null, true, commandTimeout, commandType);
        //        }
        //        else
        //        {
        //            ts = connection.Query<T>(sqlString, param, null, true, commandTimeout, commandType);
        //        }
        //        if (null != ts)
        //        {
        //            list = ts.AsList();
        //        }
        //    }
        //    return list;
        //}


        //public DataTable GetDataTable(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        //{
        //    var dt = new DataTable();
        //    using (IDbConnection connection = Connection)
        //    {
        //        connection.Open();
        //        if (null == param)
        //        {
        //            dt.Load(connection.ExecuteReader(sqlString, null, null, commandTimeout, commandType));
        //        }
        //        else
        //        {
        //            dt.Load(connection.ExecuteReader(sqlString, param, null, commandTimeout, commandType));
        //        }
        //    }

        //    return dt;
        //}


        //public GridReader GetMultiple(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        //{
        //    using (IDbConnection connection = Connection)
        //    {
        //        connection.Open();
        //        if (null == param)
        //        {
        //            var gr = connection.QueryMultiple(sqlString, null, null, commandTimeout, commandType);
        //            connection.Dispose();
        //            return gr;
        //        }
        //        else
        //        {
        //            var gr = connection.QueryMultiple(sqlString, param, null, commandTimeout, commandType);
        //            connection.Dispose();
        //            return gr;
        //        }
        //    }
        //}

        //public List<T> GetList<T>(string sqlString)
        //{
        //    return GetList<T>(sqlString, null, CommandType.Text);
        //}

        //public List<T> GetList<T>(string sqlString, object param)
        //{
        //    if (null == param)
        //    {
        //        return GetList<T>(sqlString);
        //    }

        //    return GetList<T>(sqlString, param, CommandType.Text);
        //}

        //public DataTable GetDataTable(string sqlString)
        //{
        //    return GetDataTable(sqlString, null, CommandType.Text);
        //}

        //public DataTable GetDataTable(string sqlString, object param)
        //{
        //    if (null == param)
        //    {
        //        return GetDataTable(sqlString);
        //    }

        //    return GetDataTable(sqlString, param, CommandType.Text);
        //}
        #endregion

        #region INSERT
        public bool Insert(string sqlString, object param = null, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }

        public bool Insert<T>(string sqlString, List<T> list, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            var intResult = 0;

            if (null != list && 0 < list.Count)
            {
                using (IDbConnection connection = Connection)
                {

                    intResult = connection.Execute(sqlString, list, null, commandTimeOut, commandType);
                }
            }

            return intResult > 0;
        }
        #endregion

        #region UPDATE
        public bool Update(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }
        #endregion

        #region DELETE
        public bool Delete(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }
        #endregion

        #region Private Methods
        private bool ExecuteNonQuery(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            var intResult = 0;
            using (IDbConnection connection = Connection)
            {


                if (null == param)
                {
                    intResult = connection.Execute(sqlString, null, null, commandTimeOut, commandType);

                }
                else
                {
                    intResult = connection.Execute(sqlString, param, null, commandTimeOut, commandType);
                }
            }

            return intResult > 0;
        }
        #endregion

        /// <summary>
        /// SQL 执行事务操作
        /// </summary>
        /// <param name="sqlStrings"></param>
        /// <returns></returns>
        public bool ExecuteTrans(List<string> sqlStrings, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {

            using (IDbConnection connection = Connection)
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (string sql in sqlStrings)
                    {
                        if (!string.IsNullOrWhiteSpace(sql))
                        {
                            connection.Execute(sql, null, transaction, commandTimeOut, commandType);
                        }

                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    return false;
                }

            }
        }


        /// <summary>
        /// SQL 执行事务操作
        /// </summary>
        /// <param name="sqlStrings"></param>
        /// <returns></returns>
        public bool ExecuteTrans(List<TransModel> list, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {

            using (IDbConnection connection = Connection)
            {
                connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    foreach (var item in list)
                    {
                        if (item.LisTModel != null && item.LisTModel.Count > 0)
                        {
                            connection.Execute(item.SQL, item.LisTModel, transaction, commandTimeOut, commandType);
                        }
                    }
                    transaction.Commit();
                    return true;

                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    return false;
                }
            }
        }




        public async Task<List<T>> GetList<T>(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        {
            var list = new List<T>();

            using (IDbConnection db = Connection)
            {
                db.Open();
                IEnumerable<T> ts = null;
                if (null == param)
                {
                    ts = db.Query<T>(sqlString, null, null, true, commandTimeout, commandType);
                }
                else
                {
                    ts = db.Query<T>(sqlString, param, null, true, commandTimeout, commandType);
                }
                if (null != ts)
                {
                    return ts.AsList();
                }
            }

            return list;
        }


        public DataTable GetDataTable(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        {
            var dt = new DataTable();
            using (IDbConnection db = Connection)
            {
                db.Open();
                if (null == param)
                {
                    dt.Load(db.ExecuteReader(sqlString, null, null, commandTimeout, commandType));
                }
                else
                {
                    dt.Load(db.ExecuteReader(sqlString, param, null, commandTimeout, commandType));
                }

            }

            return dt;
        }
        //


        public GridReader GetMultiple(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        {
            ///var gr =new GridReader();
            using (IDbConnection db = Connection)
            {
                db.Open();
                if (null == param)
                {
                    var gr = db.QueryMultiple(sqlString, null, null, commandTimeout, commandType);
                    db.Dispose();
                    return gr;
                }
                else
                {
                    var gr = db.QueryMultiple(sqlString, param, null, commandTimeout, commandType);
                    db.Dispose();
                    return gr;
                }

            }
        }


    }
}