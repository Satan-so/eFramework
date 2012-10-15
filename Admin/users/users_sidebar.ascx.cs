using System;
using System.Web.Security;
using System.Web.UI;

namespace GHY.EF.Admin.users
{
    /// <summary>
    /// 成员管理侧边栏。
    /// </summary>
    public partial class users_sidebar : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("MemberAdmin")))
            {
                this.Response.Write("<script>alert('无权限！');</script>");
                this.Response.Write("<script>window.location = '../index.aspx';</script>");
            }
        }
    }
}