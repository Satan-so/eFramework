using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using GHY.EF.Core.Data;

namespace GHY.EF.Core.Application
{
    /// <summary>
    /// 应用管理类。
    /// </summary>
    public class ApplicationManager
    {
        #region 私有字段
        Database db;
        #endregion

        #region 私有方法
        ApplicationEntity PopulateApplication(IDataReader reader)
        {
            ApplicationEntity application = new ApplicationEntity()
            {
                ApplicationId = (int)reader["ApplicationId"],
                ApplicationName = reader["ApplicationName"].ToString(),
                Owner = reader["Owner"].ToString(),
                Comment = reader["Comment"].ToString(),
                CreationDate = (DateTime)reader["CreationDate"],
                UpdateDate = (DateTime)reader["UpdateDate"],
                Enable = (bool)reader["Enable"]
            };

            return application;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 用连接字符串构造应用管理类。
        /// </summary>
        /// <param name="connectionStringName">连接字符串。</param>
        public ApplicationManager(string connectionStringName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(connectionStringName));

            this.db = new Database(connectionStringName);
        }

        /// <summary>
        /// 添加一个应用。
        /// </summary>
        /// <param name="application">待添加的应用。</param>
        public void Add(ApplicationEntity application)
        {
            Contract.Requires(application != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(application.ApplicationName));
            Contract.Requires(!string.IsNullOrWhiteSpace(application.Owner));

            IDataParameter[] parameters = new IDataParameter[4];

            parameters[0] = this.db.NewDataParameter("@ApplicationName", application.ApplicationName);
            parameters[1] = this.db.NewDataParameter("@Owner", application.Owner);
            parameters[2] = this.db.NewDataParameter("@Comment", application.Comment);
            parameters[3] = this.db.NewDataParameter("@Enable", application.Enable);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Application_Add", parameters);

            if (result == 0)
            {
                throw new Exception("新建应用失败！");
            }
        }

        /// <summary>
        /// 更新一个应用。
        /// </summary>
        /// <param name="application">待更新的应用。</param>
        public void Update(ApplicationEntity application)
        {
            Contract.Requires(application != null);
            Contract.Requires(application.ApplicationId > 0);
            Contract.Requires(!string.IsNullOrWhiteSpace(application.ApplicationName));
            Contract.Requires(!string.IsNullOrWhiteSpace(application.Owner));

            IDataParameter[] parameters = new IDataParameter[5];

            parameters[0] = this.db.NewDataParameter("@ApplicationId", application.ApplicationId);
            parameters[1] = this.db.NewDataParameter("@ApplicationName", application.ApplicationName);
            parameters[2] = this.db.NewDataParameter("@Owner", application.Owner);
            parameters[3] = this.db.NewDataParameter("@Comment", application.Comment);
            parameters[4] = this.db.NewDataParameter("@Enable", application.Enable);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Application_Update", parameters);

            if (result == 0)
            {
                throw new Exception("更新应用失败！");
            }
        }

        /// <summary>
        /// 获取一个应用。
        /// </summary>
        /// <param name="applicationId">应用的Id。</param>
        /// <returns>欲获取的应用。</returns>
        public ApplicationEntity Get(int applicationId)
        {
            Contract.Requires(applicationId > 0);

            IDataParameter[] parameters = new IDataParameter[1];
            parameters[0] = this.db.NewDataParameter("@ApplicationId", applicationId);

            ApplicationEntity application;

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Application_Get", parameters))
            {
                if (reader.Read())
                {
                    application = this.PopulateApplication(reader);
                }
                else
                {
                    application = new ApplicationEntity();
                }
            }

            return application;
        }

        /// <summary>
        /// 获取全部应用。
        /// </summary>
        /// <returns>全部应用。</returns>
        public List<ApplicationEntity> GetAll()
        {
            List<ApplicationEntity> applications = new List<ApplicationEntity>();

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Application_GetAll"))
            {
                while (reader.Read())
                {
                    applications.Add(this.PopulateApplication(reader));
                }
            }

            return applications;
        }
        #endregion
    }
}