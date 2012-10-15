using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using GHY.EF.Core.Info;

namespace GHY.EF.Article
{
    /// <summary>
    /// 文章内容管理类。
    /// </summary>
    public class ArticleManager : InfoManager
    {
        #region 私有方法
        ArticleEntity PopulateArticle(IDataReader reader)
        {
            ArticleEntity info = new ArticleEntity()
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
        /// 用连接字符串构造文章内容管理类。
        /// </summary>
        /// <param name="connectionStringName">连接字符串。</param>
        public ArticleManager(string connectionStringName)
            : base(connectionStringName) { }

        /// <summary>
        /// 添加一篇文章。
        /// </summary>
        /// <param name="article">待添加的文章。</param>
        public void Add(ArticleEntity article)
        {
            Contract.Requires(article != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(article.Title));
            Contract.Requires(article.NodeId > 0);

            IDataParameter[] parameters = new IDataParameter[8];

            parameters[0] = this.db.NewDataParameter("@Title", article.Title);
            parameters[1] = this.db.NewDataParameter("@Content", article.Content);
            parameters[2] = this.db.NewDataParameter("@AuthorName", article.AuthorName);
            parameters[3] = this.db.NewDataParameter("@Source", article.Source);
            parameters[4] = this.db.NewDataParameter("@Link", article.Link);
            parameters[5] = this.db.NewDataParameter("@Image", article.Image);
            parameters[6] = this.db.NewDataParameter("@NodeId", article.NodeId);
            parameters[7] = this.db.NewDataParameter("@FullNodeIds", article.FullNodeIds);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Article_Add", parameters);

            if (result == 0)
            {
                throw new Exception("新建文章失败！");
            }
        }

        /// <summary>
        /// 更新一篇文章。
        /// </summary>
        /// <param name="article">待更新的文章。</param>
        public void Update(ArticleEntity article)
        {
            Contract.Requires(article != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(article.Title));
            Contract.Requires(article.NodeId > 0);

            IDataParameter[] parameters = new IDataParameter[9];

            parameters[0] = this.db.NewDataParameter("@InfoId", article.InfoId);
            parameters[1] = this.db.NewDataParameter("@Title", article.Title);
            parameters[2] = this.db.NewDataParameter("@Content", article.Content);
            parameters[3] = this.db.NewDataParameter("@AuditName", article.AuditName);
            parameters[4] = this.db.NewDataParameter("@Source", article.Source);
            parameters[5] = this.db.NewDataParameter("@Link", article.Link);
            parameters[6] = this.db.NewDataParameter("@Image", article.Image);
            parameters[7] = this.db.NewDataParameter("@IsTop", article.IsTop);
            parameters[8] = this.db.NewDataParameter("@State", (int)article.State);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Article_Update", parameters);

            if (result == 0)
            {
                throw new Exception("更新文章失败！");
            }
        }

        /// <summary>
        /// 获取一篇文章。
        /// </summary>
        /// <param name="articleId">欲获取文章的Id。</param>
        public override InfoEntity Get(int articleId)
        {
            Contract.Requires(articleId > 0);

            IDataParameter[] parameters = new IDataParameter[1];
            parameters[0] = this.db.NewDataParameter("@InfoId", articleId);

            ArticleEntity article;

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Info_Get", parameters))
            {
                if (reader.Read())
                {
                    article = this.PopulateArticle(reader);
                }
                else
                {
                    article = new ArticleEntity();
                }
            }

            return article;
        }

        /// <summary>
        /// 获取指定节点下的文章的集合，并显示在数据页中。
        /// </summary>
        /// <param name="nodeId">节点Id。</param>
        /// <param name="isAdmin">是否是管理后台。</param>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 从零开始。</param>
        /// <param name="pageSize">要返回的结果页的大小。</param>
        /// <param name="totalRecords">匹配的结果总数。</param>
        /// <returns>包含一页 pageSize ArticleEntity 对象的 List 集合，这些对象从 pageIndex 指定的页开始。</returns>
        public override List<InfoEntity> GetByNodeId(int articleId, bool isAdmin, int pageIndex, int pageSize, out int totalRecords)
        {
            Contract.Requires(articleId > 0);
            Contract.Requires(pageIndex > 0);
            Contract.Requires(pageSize > 0);

            IDataParameter[] parameters = new IDataParameter[5];

            parameters[0] = this.db.NewDataParameter("@NodeId", "%" + articleId.ToString() + "%");
            parameters[1] = this.db.NewDataParameter("@PageIndex", pageIndex);
            parameters[2] = this.db.NewDataParameter("@PageSize", pageSize);
            parameters[3] = this.db.NewDataParameter("@IsAdmin", isAdmin);
            parameters[4] = this.db.NewOutputDataParameter("@Count");
            parameters[4].DbType = DbType.Int32;

            List<InfoEntity> Articles = new List<InfoEntity>(pageSize);

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Info_GetByNodeId", parameters))
            {
                while (reader.Read())
                {
                    ArticleEntity article = this.PopulateArticle(reader);
                    Articles.Add(article);
                }
            }

            totalRecords = (int)this.db.GetOutputDataParameter("@Count").Value;

            return Articles;
        }
        #endregion
    }
}