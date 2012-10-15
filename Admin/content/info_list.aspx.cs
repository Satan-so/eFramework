using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.UI;
using GHY.EF.Core.Info;
using GHY.EF.Core.Node;
using GHY.EF.Core.Security;

namespace GHY.EF.Admin.content
{
    /// <summary>
    /// 内容列表页。
    /// </summary>
    public partial class info_list : Page
    {
        const string updatePageString = "Application{0}UpdatePage";

        protected void Page_Load(object sender, EventArgs e)
        {
            int id;

            if (int.TryParse(this.Request.QueryString["nodeid"], out id))
            {
                NodeManager nodeManager = new NodeManager("EFConnectionString");
                NodeEntity node = nodeManager.Get(id);
                StringDictionary roles = (this.Master as AdminLayout).UserRoles;

                if (!nodeManager.CheckNodeRole(node, roles, ActionType.ManageNode))
                {
                    this.Response.Write("<script>alert('无权限！');</script>");
                    this.Response.Write("<script>window.location = 'info_manager.aspx';</script>");
                    return;
                }

                int infoCount;

                if (node.ApplicationId == 3)
                {
                    InfoManager infoManager = new InfoManager("EFConnectionString");
                    this.infosView.DataSource = infoManager.GetByNodeId(id, true, int.Parse(this.pageIndex.Value), int.Parse(this.pageSize.Value), out infoCount);
                }
                else
                {
                    infoCount = 0;
                }

                this.infoCount.Value = infoCount.ToString();
                this.infosView.DataBind();
            }
            else if (int.TryParse(this.Request.QueryString["infoid"], out id))
            {
                InfoManager infoManager = new InfoManager("EFConnectionString");
                InfoEntity info = infoManager.Get(id);
                int nodeId = info.NodeId;

                NodeManager nodeManager = new NodeManager("EFConnectionString");
                NodeEntity node = nodeManager.Get(nodeId);

                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[string.Format(updatePageString, node.ApplicationId.ToString())]))
                {
                    this.Response.Write("<script>alert('配置出错！请联系程序猿！');</script>");
                    this.Response.Write("<script>window.location = 'info_manager.aspx';</script>");
                }
                else
                {
                    this.Response.Redirect(ConfigurationManager.AppSettings[string.Format(updatePageString, node.ApplicationId.ToString())] + "?infoid=" + id);
                }
            }
            else
            {
                this.Response.Write("<script>alert('参数错误！');</script>");
                this.Response.Write("<script>window.location = 'info_manager.aspx';</script>");
            }
        }
    }
}