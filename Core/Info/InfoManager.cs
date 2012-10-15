using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using GHY.EF.Core.Data;

namespace GHY.EF.Core.Info
{
    /// <summary>
    /// 信息管理类。
    /// </summary>
    public class InfoManager
    {
        #region 保护字段
        protected Database db;
        #endregion

        #region 私有方法
        InfoEntity PopulateInfo(IDataReader reader)
        {
            InfoEntity info = new InfoEntity()
            {
                InfoId = (int)reader["InfoId"],
                Title = reader["Title"].ToString(),
                Content = reader["Content"].ToString(),
                AuthorName = reader["AuthorName"].ToString(),
                AuditName = reader["AuditName"].ToString(),
                Source = reader["Source"].ToString(),
                Link = reader["Link"].ToString(),
                Image = reader["Image"].ToString(),
                IsTop = (bool)reader["IsTop"],
                NodeId = (int)reader["NodeId"],
                FullNodeIds = reader["FullNodeIds"].ToString(),
                CreationDate = (DateTime)reader["CreationDate"],
                UpdateDate = (DateTime)reader["UpdateDate"],
                State = (InfoState)(int)reader["State"]
            };

            return info;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 用连接字符串构造信息管理类。
        /// </summary>
        /// <param name="connectionStringName">连接字符串。</param>
        public InfoManager(string connectionStringName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(connectionStringName));

            this.db = new Database(connectionStringName);
        }

        /// <summary>
        /// 获取一篇信息。
        /// </summary>
        /// <param name="infoId">内容Id。</param>
        /// <returns>欲获取的信息。</returns>
        public virtual InfoEntity Get(int infoId)
        {
            Contract.Requires(infoId > 0);

            IDataParameter[] parameters = new IDataParameter[1];
            parameters[0] = this.db.NewDataParameter("@InfoId", infoId);

            InfoEntity info;

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Info_Get", parameters))
            {
                if (reader.Read())
                {
                    info = this.PopulateInfo(reader);
                }
                else
                {
                    info = new InfoEntity();
                }
            }

            return info;
        }

        /// <summary>
        /// 获取指定节点下的信息的集合，并显示在数据页中。
        /// </summary>
        /// <param name="nodeId">节点Id。</param>
        /// <param name="isAdmin">是否是管理后台。</param>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 从零开始。</param>
        /// <param name="pageSize">要返回的结果页的大小。</param>
        /// <param name="totalRecords">匹配的结果总数。</param>
        /// <returns>包含一页 pageSize InfoEntity 对象的 List 集合，这些对象从 pageIndex 指定的页开始。</returns>
        public virtual List<InfoEntity> GetByNodeId(int nodeId, bool isAdmin, int pageIndex, int pageSize, out int totalRecords)
        {
            Contract.Requires(nodeId > 0);
            Contract.Requires(pageIndex > 0);
            Contract.Requires(pageSize > 0);

            IDataParameter[] parameters = new IDataParameter[5];

            parameters[0] = this.db.NewDataParameter("@NodeId", "%" + nodeId.ToString() + "%");
            parameters[1] = this.db.NewDataParameter("@PageIndex", pageIndex);
            parameters[2] = this.db.NewDataParameter("@PageSize", pageSize);
            parameters[3] = this.db.NewDataParameter("@IsAdmin", isAdmin);
            parameters[4] = this.db.NewOutputDataParameter("@Count");
            parameters[4].DbType = DbType.Int32;

            List<InfoEntity> infos = new List<InfoEntity>(pageSize);

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Info_GetByNodeId", parameters))
            {
                while (reader.Read())
                {
                    InfoEntity info = this.PopulateInfo(reader);
                    infos.Add(info);
                }
            }

            totalRecords = (int)this.db.GetOutputDataParameter("@Count").Value;

            return infos;
        }
        #endregion
    }
}