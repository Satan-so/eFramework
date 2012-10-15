using System;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI;
using GHY.EF.Article;
using GHY.EF.Core.Info;
using GHY.EF.Core.Node;
using GHY.EF.Core.Security;

namespace GHY.EF.Admin.content
{
    /// <summary>
    /// 更新文章页面。
    /// </summary>
    public partial class update_article : Page
    {
        int infoId;
        ArticleEntity article;
        ArticleManager articleManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Request.QueryString["infoid"], out this.infoId))
            {
                this.Response.Write("<script>alert('参数错误！');</script>");
                this.Response.Write("<script>window.location = 'info_manager.aspx';</script>");
                return;
            }

            this.articleManager = new ArticleManager("EFConnectionString");
            this.article = this.articleManager.Get(this.infoId) as ArticleEntity;

            NodeManager nodeManager = new NodeManager("EFConnectionString");
            NodeEntity node = nodeManager.Get(this.article.NodeId);
            StringDictionary roles = (this.Master as AdminLayout).UserRoles;

            if (!nodeManager.CheckNodeRole(node, roles, ActionType.ManageInfo))
            {
                this.Response.Write("<script>alert('无权限！');</script>");
                this.Response.Write("<script>window.location = 'info_manager.aspx';</script>");
                return;
            }

            if (!this.IsPostBack)
            {
                this.title.Text = this.article.Title;
                this.content.Text = this.article.Content;
                this.source.Text = this.article.Source;
                this.image.Text = this.article.Image;
                this.link.Text = this.article.Link;
                this.isTop.Checked = this.article.IsTop;
                this.stateList.SelectedIndex = (int)this.article.State;
            }
        }

        protected void updateArticleButton_Click(object sender, EventArgs e)
        {
            this.article.Title = this.title.Text;
            this.article.Content = this.content.Text;
            this.article.Source = this.source.Text;
            this.article.Image = this.image.Text;
            this.article.Link = this.link.Text;
            this.article.IsTop = this.isTop.Checked;

            if ((int)this.article.State != this.stateList.SelectedIndex)
            {
                this.article.State = (InfoState)this.stateList.SelectedIndex;
                this.article.AuditName = Membership.GetUser().UserName;
            }

            this.articleManager.Update(this.article);
            this.Response.Redirect("info_manager.aspx");
        }
    }
}