﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace SIDCColaSyncer.Service
{
    internal class MySQLHelper : IDisposable
    {
        private System.ComponentModel.Component components = new System.ComponentModel.Component();
        private bool disposed = false;
        private Dictionary<string, object> _argSQLParam;
        private StringBuilder _argSQLCommand;

        /// <summary>
        /// MysqlTransaction
        /// </summary>
        private MySqlTransaction mysqlTrans;

        /// <summary>
        /// Mysqlconnection pre initialization
        /// </summary>
        private MySqlConnection cnn;

        /// <summary>
        /// Mysqlcommand pre initialization
        /// </summary>
        /// 
        //private MySqlCommand cmd
        //{
        //    get
        //    {
        //        var _cmd = new MySqlCommand(_argSQLCommand.ToString(), cnn);

        //        if (_argSQLParam != null)
        //        {
        //            foreach (var item in _argSQLParam)
        //            {
        //                _cmd.Parameters.AddWithValue(item.Key, item.Value);
        //            }
        //        }
        //        _cmd.CommandTimeout = 3000;
        //        return _cmd;
        //    }
        //}

        private MySqlCommand cmd
        {
            get
            {
                if (_argSQLCommand == null)
                    throw new InvalidOperationException("SQL Command is not initialized.");

                if (cnn == null)
                    throw new InvalidOperationException("Database connection is not initialized.");

                var _cmd = new MySqlCommand(_argSQLCommand.ToString(), cnn);

                if (_argSQLParam != null)
                {
                    foreach (var item in _argSQLParam)
                    {
                        _cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }
                _cmd.CommandTimeout = 3000;
                return _cmd;
            }
        }


        /// <summary>
        /// SQL Parameters
        /// </summary>
        public Dictionary<string, object> ArgSQLParam
        {
            get { return _argSQLParam; }
            set { _argSQLParam = value; }
        }

        /// <summary>
        /// SQL Command
        /// </summary>
        public StringBuilder ArgSQLCommand
        {
            get { return _argSQLCommand; }
            set { _argSQLCommand = value; }
        }

        /// <summary>
        /// Call this class to initialize Data Access Layer (You can use any of the following MySQL parameters of your like.)
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="argSQLCommand"></param>
        /// <param name="argSQLParam">Dictionary type of parameter (parametername,value)</param>
        public MySQLHelper(string dbSource, StringBuilder argSQLCommand = null, Dictionary<string, object> argSQLParam = null)
        {
            try
            {
                cnn = new MySqlConnection(dbSource);

                _argSQLCommand = argSQLCommand;
                _argSQLParam = argSQLParam;
            }
            catch
            {
                throw;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                mysqlTrans.Commit();
            }
            catch
            {
                mysqlTrans.Rollback();
                throw;
            }
            finally
            {
                cnn.Close();
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mysqlTrans.Rollback();
            }
            catch
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
        }

        public void BeginTransaction()
        {
            try
            {
                if (mysqlTrans == null)
                {
                    cnn.Open();
                    //mysqlTrans = cnn.BeginTransaction(IsolationLevel.Serializable);
                }
            }
            catch
            {
                throw;
            }
        }

        public void CloseConnection()
        {
            try
            {
                cnn.Close();

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Call this method to execute MySQL query (eg. Save,Update,Delete)
        /// </summary>
        public int ExecuteMySQL()
        {
            try
            {
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                    //mysqlTrans = cnn.BeginTransaction(IsolationLevel.Serializable);
                }

                int res = 0;

                res = cmd.ExecuteNonQuery();

                return res;
            }
            catch
            {
                //mysqlTrans.Rollback();
                throw;
            }
        }

        public MySqlDataReader MySQLReader()
        {
            cnn.Open();
            return cmd.ExecuteReader();
        }

        public MySqlDataReader MySQLExecuteReaderBeginTransaction()
        {
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Open();
                mysqlTrans = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            return cmd.ExecuteReader();
        }

        #region Disposing Interface

        /// <summary>
        /// Interface Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                try
                {
                    cmd.Dispose();
                    cnn.Dispose();

                    if (mysqlTrans != null)
                    {
                        mysqlTrans.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (cnn.State == ConnectionState.Open)
                    cnn.Close();

                if (cnn != null)
                    MySqlConnection.ClearPool(cnn);

                components.Dispose();
            }

            disposed = true;
        }

        /// <summary>
        /// Call this method function to return object from MySQL
        /// </summary>
        /// <returns></returns>
        public object GetMySQLScalar()
        {
            try
            {
                if (cnn.State == ConnectionState.Closed)
                {
                    cnn.Open();
                }

                return cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                cnn.Close();
            }
        }

        /// <summary>
        ///
        /// </summary>
        ~MySQLHelper()
        {
            Dispose(false);
        }

        #endregion Disposing Interface
    }
}
