using System;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI;
using GHY.EF.Article;
using GHY.EF.Core.Node;
using GHY.EF.Core.Security;

namespace GHY.EF.Admin.content
{
    /// <summary>
    /// 添加文章页。
    /// </summary>
    public partial class add_article : Page
    {
        int nodeId;
        NodeEntity node;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Request.QueryString["nodeid"], out nodeId))
            {
                this.Response.Write("<script>alert('参数错误！');</script>");
                this.Response.Write("<script>window.location = 'add_info.aspx';</script>");
                return;
            }

            NodeManager nodeManager = new NodeManager("EFConnectionString");
            this.node = nodeManager.Get(this.nodeId);
            StringDictionary roles = (this.Master as AdminLayout).UserRoles;

            if (!nodeManager.CheckNodeRole(node, roles, ActionType.AddInfo))
            {
                this.Response.Write("<script>alert('无权限！');</script>");
                this.Response.Write("<script>window.location = 'add_info.aspx';</script>");
                return;
            }
        }

        protected void addArticleButton_Click(object sender, EventArgs e)
        {
            ArticleEntity article = new ArticleEntity()
            {
                Title = this.title.Text,
                Content = this.content.Text,
                AuthorName = Membership.GetUser().UserName,
                Source = this.source.Text,
                Link = this.link.Text.Trim(),
                Image = this.image.Text.Trim(),
                NodeId = this.nodeId,
                FullNodeIds = node.FullIdsStringType
            };

            ArticleManager articleManager = new ArticleManager("EFConnectionString");
            articleManager.Add(article);

            this.Response.Write("<script>alert('添加文章成功！');</script>");
            this.Response.Write("<script>window.location = 'add_info.aspx';</script>");
        }
    }
}