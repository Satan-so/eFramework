using System;
using System.Collections.Specialized;
using System.Web.UI;
using GHY.EF.Core.Node;
using GHY.EF.Core.Security;

namespace GHY.EF.Admin.content
{
    /// <summary>
    /// 更新节点信息页。
    /// </summary>
    public partial class update_node : Page
    {
        protected int nodeId;
        protected NodeEntity node;
        NodeManager nodeManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Request.QueryString["nodeid"], out this.nodeId))
            {
                this.Response.Write("<script>alert('参数错误！');</script>");
                this.Response.Write("<script>window.location = 'node_manager.aspx';</script>");
                return;
            }

            this.nodeManager = new NodeManager("EFConnectionString");
            this.node = this.nodeManager.Get(this.nodeId);
            StringDictionary roles = (this.Master as AdminLayout).UserRoles;

            if (!nodeManager.CheckNodeRole(node, roles, ActionType.ManageNode))
            {
                this.Response.Write("<script>alert('无权限！');</script>");
                this.Response.Write("<script>window.location = 'node_manager.aspx';</script>");
                return;
            }

            if (!this.IsPostBack)
            {
                this.nodeName.Text = this.node.NodeName;
                this.imagePath.Text = this.node.ImagePath;
                this.needAudit.Checked = this.node.NeedAudit;
                this.comment.Text = this.node.Comment;
                this.enable.Checked = this.node.Enable;

                this.addNodeLink.NavigateUrl += this.nodeId.ToString();
            }
        }

        protected void updateNodeButton_Click(object sender, EventArgs e)
        {
            this.node.NeedAudit = this.needAudit.Checked;
            this.node.Comment = this.comment.Text;
            this.node.Enable = this.enable.Checked;

            this.nodeManager.Update(node);

            this.Response.Redirect("node_manager.aspx");
        }
    }
}