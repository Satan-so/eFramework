using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using GHY.EF.Security;

namespace GHY.EF.Admin.users
{
    /// <summary>
    /// 用户列表页
    /// </summary>
    public partial class user_list : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int userCount;
            MembershipUserCollection usersCollection;

            if (string.IsNullOrWhiteSpace(this.userName.Text))
            {
                usersCollection = Membership.GetAllUsers(int.Parse(this.pageIndex.Value), int.Parse(this.pageSize.Value), out userCount);
            }
            else
            {
                usersCollection = Membership.FindUsersByName(this.userName.Text.Trim(), int.Parse(this.pageIndex.Value), int.Parse(this.pageSize.Value), out userCount);
            }

            List<EFUser> users = new List<EFUser>(usersCollection.Count);

            foreach (MembershipUser user in usersCollection)
            {
                users.Add(user as EFUser);
            }

            this.infoCount.Value = userCount.ToString();
            this.usersView.DataSource = users;
            this.usersView.DataBind();
        }
    }
}