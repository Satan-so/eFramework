using System;
using System.Collections.Specialized;
using System.Web.UI;
using GHY.EF.Core.Node;
using GHY.EF.Core.Security;

namespace GHY.EF.Admin.content
{
    /// <summary>
    /// 增加节点页面。
    /// </summary>
    public partial class add_node : Page
    {
        protected int parentId;
        NodeManager nodeManager;
        StringDictionary roles;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Request.QueryString["parentid"], out parentId))
            {
                this.Response.Write("<script>alert('参数错误！');</script>");
                this.Response.Write("<script>window.location = 'node_manager.aspx';</script>");
                return;
            }

            this.nodeManager = new NodeManager("EFConnectionString");
            NodeEntity parentNode = this.nodeManager.Get(this.parentId);
            this.roles = (this.Master as AdminLayout).UserRoles;

            if (!nodeManager.CheckNodeRole(parentNode, this.roles, ActionType.ManageNode))
            {
                this.Response.Write("<script>alert('无权限！');</script>");
                this.Response.Write("<script>window.location = 'node_manager.aspx';</script>");
                return;
            }
        }

        protected void addNodeButton_Click(object sender, EventArgs e)
        {
            NodeEntity node = new NodeEntity()
            {
                ParentId = this.parentId,
                NodeName = this.nodeName.Text,
                ImagePath = this.imagePath.Text.Trim(),
                NeedAudit = this.needAudit.Checked,
                Comment = this.comment.Text,
                Enable = this.enable.Checked,
                ApplicationId = 3
            };

            this.nodeManager.Add(node);
            this.Response.Redirect("node_manager.aspx");
        }
    }
}