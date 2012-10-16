using System;
using System.Web.Security;

namespace GHY.EF.Security
{
    /// <summary>
    /// 公开和更新成员资格数据存储区中的成员资格用户信息。
    /// </summary>
    public class EFUser : MembershipUser
    {
        /// <summary>
        /// 获取或设置用户唯一标识符。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public new string UserName { get; set; }

        /// <summary>
        /// 获取或设置用户密码。仅在管理员设置密码时使用。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置真名。
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 获取或设置电话号码。
        /// </summary>
        public string TelNumber { get; set; }

        /// <summary>
        /// 所属应用程序的名称。
        /// </summary>
        public string ApplicationName { get; set; }
    }
}
