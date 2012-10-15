using System;
using System.Web.Security;
using System.Web.UI;
using GHY.EF.Security;

namespace GHY.EF.Admin.users
{
    /// <summary>
    /// 编辑成员信息。
    /// </summary>
    public partial class update_user : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Request.QueryString["username"]))
            {
                this.Response.Write("<script>alert('参数错误！');</script>");
                this.Response.Write("<script>window.location = 'user_list.aspx';</script>");
                return;
            }

            EFUser user = Membership.GetUser(this.Request.QueryString["username"]) as EFUser;

            this.userName.Text = user.UserName;
            this.realName.Text = user.RealName;
            this.email.Text = user.Email;
            this.telNumber.Text = user.TelNumber;
            this.isApproved.Checked = user.IsApproved;
            this.comment.Text = user.Comment;
        }

        protected void updateUserButton_Click(object sender, EventArgs e)
        {
            EFUser user = Membership.GetUser(this.Request.QueryString["username"]) as EFUser;

            user.Password = string.IsNullOrWhiteSpace(this.password.Text) ? string.Empty : this.password.Text;
            user.RealName = this.realName.Text;
            user.Email = this.email.Text;
            user.TelNumber = this.telNumber.Text;
            user.IsApproved = this.isApproved.Checked;
            user.Comment = this.comment.Text;
            Membership.UpdateUser(user);

            this.Response.Redirect("user_list.aspx");
        }
    }
}