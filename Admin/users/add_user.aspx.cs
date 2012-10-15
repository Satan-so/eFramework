using System;
using System.Web.Security;
using System.Web.UI;
using GHY.EF.Security;

namespace GHY.EF.Admin.users
{
    /// <summary>
    /// 添加成员页。
    /// </summary>
    public partial class add_user : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void addUserButton_Click(object sender, EventArgs e)
        {
            EFUser user = Membership.CreateUser(this.userName.Text.Trim(), this.password.Text.Trim(), this.email.Text) as EFUser;
            
            user.RealName = this.realName.Text;
            user.TelNumber = this.telNumber.Text;
            user.Comment = this.comment.Text;
            Membership.UpdateUser(user);

            this.Response.Redirect("user_list.aspx");
        }
    }
}