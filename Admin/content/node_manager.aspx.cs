using System;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI;
using GHY.EF.Core.Node;
using GHY.EF.Core.Security;

namespace GHY.EF.Admin.content
{
    /// <summary>
    /// 节点管理页面。
    /// </summary>
    public partial class node_manager : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NodeManager nodeManager = new NodeManager("EFConnectionString");
            nodeManager.FillTreeView(nodeTreeView, (this.Master as AdminLayout).UserRoles, ActionType.ManageNode, "update_node.aspx?nodeid=");
        }
    }
}