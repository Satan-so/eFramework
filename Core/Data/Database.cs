using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;

namespace GHY.EF.Core.Data
{
    /// <summary>
    /// 关系数据库操作工具类。
    /// </summary>
    public class Database
    {
        #region 私有字段
        string connectionStringName;
        DbProviderFactory factory;
        DbCommand command;
        #endregion

        #region 私有方法
        void PreparingCommand(DbConnection connection, CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(commandText));

            connection.ConnectionString = ConfigurationManager.ConnectionStrings[this.connectionStringName].ConnectionString;

            this.command = this.factory.CreateCommand();

            this.command.Connection = connection;
            this.command.CommandType = commandType;
            this.command.CommandText = commandText;
            this.command.Parameters.AddRange(commandParameters);
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 使用数据库链接字符串名初始化数据库工具类。
        /// </summary>
        /// <param name="connectionStringName">数据库链接字符串名。</param>
        public Database(string connectionStringName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(connectionStringName));
            Contract.Requires(ConfigurationManager.ConnectionStrings[connectionStringName] != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString));
            Contract.Requires(!string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName));

            this.connectionStringName = connectionStringName;
            this.factory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName);
        }

        /// <summary>
        /// 新建一个查询参数。
        /// </summary>
        /// <returns>查询参数。</returns>
        public IDataParameter NewDataParameter()
        {
            return this.factory.CreateParameter();
        }

        /// <summary>
        /// 新建一个查询参数。
        /// </summary>
        /// <param name="name">新查询参数的名字。</param>
        /// <param name="value">新查询参数的值。</param>
        /// <returns>查询参数。</returns>
        public IDataParameter NewDataParameter(string name, object value)
        {
            IDataParameter parameter = this.factory.CreateParameter();

            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// 新建一个输出参数。
        /// </summary>
        /// <param name="name">新输出参数的名字。</param>
        /// <returns>输出参数。</returns>
        public IDataParameter NewOutputDataParameter(string name)
        {
            IDataParameter parameter = this.factory.CreateParameter();

            parameter.ParameterName = name;
            parameter.Direction = ParameterDirection.Output;

            return parameter;
        }

        /// <summary>
        /// 获取指定的输出参数。
        /// </summary>
        /// <param name="name">输出参数的名字。</param>
        /// <returns>输出参数。</returns>
        public IDataParameter GetOutputDataParameter(string name)
        {
            return this.command.Parameters[name];
        }

        /// <summary>
        /// 执行一个查询，返回受影响行数。
        /// </summary>
        /// <param name="commandType">查询类型。</param>
        /// <param name="commandText">查询文本。</param>
        /// <param name="commandParameters">查询参数。</param>
        /// <returns>受影响行数。</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            using (DbConnection conn = this.factory.CreateConnection())
            {
                this.PreparingCommand(conn, commandType, commandText, commandParameters);

                conn.Open();
                return this.command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 执行一个查询，返回结果第一行第一列。
        /// </summary>
        /// <param name="commandType">查询类型。</param>
        /// <param name="commandText">查询文本。</param>
        /// <param name="commandParameters">查询参数。</param>
        /// <returns>结果第一行第一列。</returns>
        public object ExecuteScalar(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            using (DbConnection conn = this.factory.CreateConnection())
            {
                this.PreparingCommand(conn, commandType, commandText, commandParameters);

                conn.Open();
                return this.command.ExecuteScalar();
            }
        }

        /// <summary>
        /// 执行一个查询，返回一个DataReader。
        /// </summary>
        /// <param name="commandType">查询类型。</param>
        /// <param name="commandText">查询文本。</param>
        /// <param name="commandParameters">查询参数。</param>
        /// <returns>返回DataReader。务必使用using体保证资源销毁。</returns>
        public IDataReader ExcuteReader(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            DbConnection conn = this.factory.CreateConnection();
            this.PreparingCommand(conn, commandType, commandText, commandParameters);
            
            conn.Open();
            return this.command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行一个查询，返回一个DataTable。
        /// </summary>
        /// <param name="commandType">查询类型。</param>
        /// <param name="commandText">查询文本。</param>
        /// <param name="commandParameters">查询参数。</param>
        /// <returns>返回DataTable。务必使用using体保证资源销毁。</returns>
        public DataTable ExcuteDataTable(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            using (DbConnection conn = this.factory.CreateConnection())
            {
                this.PreparingCommand(conn, commandType, commandText, commandParameters);

                DbDataAdapter da = this.factory.CreateDataAdapter();
                da.SelectCommand = this.command;

                DataTable dt = new DataTable();

                conn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// 执行一个查询，返回一个DataSet。
        /// </summary>
        /// <param name="commandType">查询类型。</param>
        /// <param name="commandText">查询文本。</param>
        /// <param name="commandParameters">查询参数。</param>
        /// <returns>返回DataSet。务必使用using体保证资源销毁。</returns>
        public DataSet ExcuteDataSet(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            using (DbConnection conn = this.factory.CreateConnection())
            {
                this.PreparingCommand(conn, commandType, commandText, commandParameters);

                DbDataAdapter da = this.factory.CreateDataAdapter();
                da.SelectCommand = this.command;

                DataSet ds = new DataSet();

                conn.Open();
                da.Fill(ds);
                return ds;
            }
        }
        #endregion
    }
}