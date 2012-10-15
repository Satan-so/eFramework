using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GHY.EF.Core.Info
{
    /// <summary>
    /// 信息详情页。
    /// </summary>
    public class InfoDetail : Page
    {
        protected InfoEntity info;
        protected HiddenField connectionStringName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.connectionStringName.Value))
            {
                throw new Exception("连接字符串未定义或为空！");
            }

            int infoId;

            if (!int.TryParse(this.Request.QueryString["infoid"], out infoId))
            {
                this.Response.Write("<script>alert('参数错误！');</script>");
                this.Response.Write("<script>window.location = '/';</script>");
                return;
            }

            this.info = this.GetInfo(infoId);

            if (!string.IsNullOrWhiteSpace(info.Link))
            {
                this.Response.Redirect(info.Link);
            }
        }

        protected virtual InfoEntity GetInfo(int infoId)
        {
            InfoManager infoManager = new InfoManager(this.connectionStringName.Value);
            return infoManager.Get(infoId);
        }
    }
}