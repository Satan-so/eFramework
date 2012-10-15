using System;
using System.Web.UI;
using GHY.EF.Core.Node;
using GHY.EF.Core.Security;

namespace GHY.EF.Admin.content
{
    /// <summary>
    /// 信息管理页面。
    /// </summary>
    public partial class info_manager : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NodeManager nodeManager = new NodeManager("EFConnectionString");
            nodeManager.FillTreeView(nodeTreeView, (this.Master as AdminLayout).UserRoles, ActionType.ManageInfo, "info_list.aspx?nodeid=");
        }
    }
}