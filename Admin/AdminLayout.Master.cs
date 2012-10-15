using System;
using System.Collections.Specialized;
using System.Web.Security;
using System.Web.UI;

namespace GHY.EF.Admin
{
    /// <summary>
    /// 管理后台模板。
    /// </summary>
    public partial class AdminLayout : MasterPage
    {
        StringDictionary userRoles;

        /// <summary>
        /// 获取或设置当前用户的权限字典。
        /// </summary>
        public StringDictionary UserRoles
        {
            get
            {
                if (this.userRoles != null)
                {
                }
                else if (this.Session["Roles"] != null)
                {
                    this.userRoles = this.Session["Roles"] as StringDictionary;
                }
                else
                {
                    this.userRoles = new StringDictionary();

                    if (this.Request.IsAuthenticated)
                    {
                        this.userRoles = new StringDictionary();
                        string[] rolesArray = Roles.GetRolesForUser();

                        for (int i = 0; i < rolesArray.Length; i++)
                        {
                            this.userRoles.Add(rolesArray[i], string.Empty);
                        }

                        this.Session["Roles"] = this.userRoles;
                    }
                }

                return this.userRoles;
            }

            private set
            {
                this.Session["Roles"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Request.IsAuthenticated)
            {
                this.navigationMenu.Visible = false;
            }
            else if (!this.UserRoles.ContainsKey("Administrator"))
            {
                if (!this.UserRoles.ContainsKey("SysAdmin"))
                {
                    navigationMenu.Items.RemoveAt(2);
                }

                if (!this.UserRoles.ContainsKey("MemberAdmin"))
                {
                    navigationMenu.Items.RemoveAt(1);
                }
            }
        }
    }
}