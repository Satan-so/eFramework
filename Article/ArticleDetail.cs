using GHY.EF.Core.Info;

namespace GHY.EF.Article
{
    /// <summary>
    /// 文章详情页。
    /// </summary>
    public class ArticleDetail : InfoDetail
    {
        protected override InfoEntity GetInfo(int infoId)
        {
            ArticleManager articleManager = new ArticleManager(this.connectionStringName.Value);
            return articleManager.Get(infoId);
        }
    }
}