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
        public int UserId { get; set; }

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
        /// 构造函数。
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="userName"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="comment"></param>
        /// <param name="isApproved"></param>
        /// <param name="isLockedOut"></param>
        /// <param name="creationDate"></param>
        /// <param name="lastLoginDate"></param>
        /// <param name="lastActivityDate"></param>
        /// <param name="lastPasswordChangedDate"></param>
        /// <param name="lastLockedOutDate"></param>
        public EFUser(string providerName, string userName, Object providerUserKey, string email, string passwordQuestion, string comment, bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockedOutDate)
            : base(providerName, userName, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockedOutDate)
        { }
    }
}
