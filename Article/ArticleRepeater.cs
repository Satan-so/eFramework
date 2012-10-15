using System.Collections.Generic;
using GHY.EF.Core.Info;

namespace GHY.EF.Article
{
    /// <summary>
    /// 文章型内容列表显示控件。
    /// </summary>
    public class ArticleRepeater : InfoRepeater
    {
        /// <summary>
        /// 获取文章信息集合。
        /// </summary>
        /// <param name="count">文章总数。</param>
        /// <returns>文章信息集合。</returns>
        protected override List<InfoEntity> GetInfos(out int count)
        {
            ArticleManager articleManger = new ArticleManager(this.ConnectionStringName);
            return articleManger.GetByNodeId(this.NodeId, false, this.PageIndex, this.PageSize, out count);
        }
    }
}