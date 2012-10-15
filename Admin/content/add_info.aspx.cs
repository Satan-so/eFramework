using System;
using System.Configuration;
using System.Web.UI;
using GHY.EF.Core.Node;
using GHY.EF.Core.Security;

namespace GHY.EF.Admin.content
{
    /// <summary>
    /// 添加信息页面。
    /// </summary>
    public partial class add_info : Page
    {
        const string addPageString = "Application{0}AddPage";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Request.QueryString["nodeid"]))
            {
                NodeManager nodeManager = new NodeManager("EFConnectionString");
                nodeManager.FillTreeView(nodeTreeView, (this.Master as AdminLayout).UserRoles, ActionType.ManageInfo, "add_info.aspx?nodeid=");
            }
            else
            {
                int nodeId;

                if (!int.TryParse(this.Request.QueryString["nodeid"], out nodeId))
                {
                    this.Response.Write("<script>alert('参数错误！');</script>");
                    this.Response.Write("<script>window.location = 'add_info.aspx';</script>");
                    return;
                }

                NodeManager nodeManager = new NodeManager("EFConnectionString");
                NodeEntity node = nodeManager.Get(nodeId);

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[string.Format(addPageString, node.ApplicationId.ToString())]))
                {
                    this.Response.Write("<script>alert('配置出错！请联系程序猿！');</script>");
                    this.Response.Write("<script>window.location = 'add_info.aspx';</script>");
                }
                else
                {
                    this.Response.Redirect(ConfigurationManager.AppSettings[string.Format(addPageString, node.ApplicationId.ToString())] + "?nodeid=" + nodeId);
                }
            }
        }
    }
}