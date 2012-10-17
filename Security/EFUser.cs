using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace GHY.EF.Security
{
    /// <summary>
    /// 公开和更新成员资格数据存储区中的成员资格用户信息。
    /// </summary>
    public class EFUser : MembershipUser
    {
        /// <summary>
        /// 获取或设置用户密码。
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
        /// 使用POCO初始化。
        /// </summary>
        /// <param name="poco">用来初始化的POCO对象。</param>
        public EFUser(EFUserPoco poco)
            : base("EFMembershipProvider", poco.UserName, poco.Id, poco.Email, string.Empty, poco.Comment, poco.IsApproved, false, poco.CreationDate, poco.LastLoginDate, DateTime.Now, poco.LastPasswordChangedDate, DateTime.MinValue)
        {
            if (poco == null)
            {
                poco = new EFUserPoco();
            }

            this.Password = poco.Password;
            this.RealName = poco.RealName;
            this.TelNumber = poco.TelNumber;
        }

        public EFUser(string providerName, string name, object providerUserKey, string email, string passwordQuestion, string comment, bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockoutDate)
            : base(providerName, name, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate) { }

        /// <summary>
        /// 返回该实例对应的Poco。
        /// </summary>
        /// <returns>该实例对应的Poco。</returns>
        public EFUserPoco ToPoco()
        {
            return new EFUserPoco()
            {
                Id = (int)this.ProviderUserKey,
                UserName = this.UserName,
                RealName = this.RealName,
                Email = this.Email,
                TelNumber = this.TelNumber,
                Comment = this.Comment,
                CreationDate = this.CreationDate,
                LastLoginDate = this.LastLoginDate,
                LastPasswordChangedDate = this.LastPasswordChangedDate,
                IsApproved = this.IsApproved
            };
        }
    }

    /// <summary>
    /// EFUser必须继承自MembershipUser，很多属性只读，无法作为POCO使用，无法使用Entity Framework自动Mapping，故使用此临时POCO转换。
    /// </summary>
    [Table("EF_Users")]
    public class EFUserPoco
    {
        /// <summary>
        /// 获取或设置用户唯一标识符。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置用户密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置真名。
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 获取或设置成员资格用户的电子邮件地址。
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置电话号码。
        /// </summary>
        public string TelNumber { get; set; }

        /// <summary>
        /// 获取或设置成员资格用户的特定于应用程序的信息。
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置将用户添加到成员资格数据存储区的日期和时间。
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 获取或设置用户上次进行身份验证的日期和时间。
        /// </summary>
        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 获取或设置上次更新成员资格用户的密码的日期和时间。
        /// </summary>
        public DateTime LastPasswordChangedDate { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示是否可以对成员资格用户进行身份验证。
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
