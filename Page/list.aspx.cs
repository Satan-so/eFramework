using System;
using GHY.EF.Core.Info;
using System.Web.UI;

namespace GHY.EF.Page
{
    /// <summary>
    /// 列表页。
    /// </summary>
    public partial class list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int nodeId;

            if (int.TryParse(this.Request.QueryString["nodeid"], out nodeId))
            {
                this.listRepeater.NodeId = nodeId;
            }

            int pageIndex;

            if (int.TryParse(this.Request.QueryString["pageindex"], out pageIndex))
            {
                this.listRepeater.PageIndex = pageIndex;
            }

            this.listRepeater.DataBind();
            this.infoCount.Value = listRepeater.Count.ToString();
        }
    }
}