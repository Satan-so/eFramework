using System;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Security;

namespace GHY.EF.Security
{
    /// <summary>
    /// EF成员资格提供程序。
    /// </summary>
    public class EFMembershipProvider : MembershipProvider
    {
        #region 私有字段
        string connStrName;
        #endregion

        #region 私有方法
        string EncodePassword(string password)
        {
            byte[] data;

            using (var md5Hasher = new MD5CryptoServiceProvider())
            {
                data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));
            }

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        SecurityEntities getDBContext()
        {
            return new SecurityEntities(this.connStrName);
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 使用成员资格提供程序的应用程序的名称。
        /// </summary>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// 获取成员资格提供程序是否配置为允许用户重置其密码。
        /// 暂不提供相关功能。
        /// </summary>
        public override bool EnablePasswordReset { get { return false; } }

        /// <summary>
        /// 获取成员资格提供程序是否配置为允许用户检索其密码。
        /// 因GHY的EF账号很多是提供给校内团体使用，故不允许个人检索其密码。
        /// </summary>
        public override bool EnablePasswordRetrieval { get { return false; } }

        /// <summary>
        /// 获取锁定成员资格用户前允许的无效密码或无效密码提示问题答案尝试次数。
        /// 因GHY的EF账号很多是提供给校内团体使用，故暂不提供相关功能。
        /// </summary>
        public override int MaxInvalidPasswordAttempts { get { return 0; } }

        /// <summary>
        /// 获取有效密码中必须包含的最少特殊字符数。
        /// 因GHY的EF账号很多是提供给校内团体使用，且兼容原有EF，故暂不提供相关功能。
        /// </summary>
        public override int MinRequiredNonAlphanumericCharacters { get { return 0; } }

        /// <summary>
        /// 获取密码所要求的最小长度。
        /// 因GHY的EF账号很多是提供给校内团体使用，且兼容原有EF，故暂不提供相关功能。
        /// </summary>
        public override int MinRequiredPasswordLength { get { return 0; } }

        /// <summary>
        /// 获取在锁定成员资格用户之前允许的最大无效密码或无效密码提示问题答案尝试次数的分钟数。
        /// 因GHY的EF账号很多是提供给校内团体使用，故暂不提供相关功能。
        /// </summary>
        public override int PasswordAttemptWindow { get { return 0; } }

        /// <summary>
        /// 获取一个值，该值指示在成员资格数据存储区中存储密码的格式。
        /// 基于GHY的EF的使用范围和需求，只提供Hash一种密码格式。
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat { get { return MembershipPasswordFormat.Hashed; } }

        /// <summary>
        /// 获取用于计算密码的正则表达式。
        /// 因GHY的EF账号很多是提供给校内团体使用，且要兼容原有密码，故暂不提供该功能。
        /// </summary>
        public override string PasswordStrengthRegularExpression { get { return string.Empty; } }

        /// <summary>
        /// 获取一个值，该值指示成员资格提供程序是否配置为要求用户在进行密码重置和检索时回答密码提示问题。
        /// 因GHY的EF账号很多是提供给校内团体使用，故暂不提供相关功能。
        /// </summary>
        public override bool RequiresQuestionAndAnswer { get { return false; } }

        /// <summary>
        /// 获取一个值，指示成员资格提供程序是否配置为要求每个用户名具有唯一的电子邮件地址。
        /// 因GHY的EF账号很多是提供给校内团体使用，故暂不提供相关功能。
        /// </summary>
        public override bool RequiresUniqueEmail { get { return false; } }
        #endregion

        #region 公共方法
        /// <summary>
        /// 处理更新成员资格用户密码的请求。
        /// </summary>
        /// <param name="userName">为其更新密码的用户。</param>
        /// <param name="oldPassword">指定的用户的当前密码。</param>
        /// <param name="newPassword">指定的用户的新密码。</param>
        /// <returns>如果密码更新成功，则为 true；否则为 false。</returns>
        public override bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(oldPassword));
            Contract.Requires(!string.IsNullOrWhiteSpace(newPassword));
            string encodePassword = this.EncodePassword(oldPassword);

            using (var db = this.getDBContext())
            {
                var user = db.Users.SingleOrDefault(u => u.UserName == userName && u.Password == encodePassword && u.IsApproved == true);

                if (user == null)
                {
                    return false;
                }

                user.Password = this.EncodePassword(newPassword);
                user.LastPasswordChangedDate = DateTime.Now;
                db.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// 处理更新成员资格用户的密码提示问题和答案的请求。
        /// 该方法暂不实现。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPasswordQuestion"></param>
        /// <param name="newPasswordAnswer"></param>
        /// <returns></returns>
        public override bool ChangePasswordQuestionAndAnswer(string userName, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        /// <summary>
        /// 将新的成员资格用户添加到数据源。
        /// </summary>
        /// <param name="userName">新用户的用户名。</param>
        /// <param name="password">新用户的密码。</param>
        /// <param name="email">新用户的电子邮件地址。</param>
        /// <param name="passwordQuestion">该参数暂不支持，请使用String.Empty。</param>
        /// <param name="passwordAnswer">该参数暂不支持，请使用String.Empty。</param>
        /// <param name="isApproved">是否允许验证新用户。</param>
        /// <param name="providerUserKey">该参数不支持指定，请使用null。</param>
        /// <param name="status">一个 MembershipCreateStatus 枚举值，指示是否已成功创建用户。</param>
        /// <returns>一个用新创建的用户的信息填充的 MembershipUser 对象。</returns>
        public override MembershipUser CreateUser(string userName, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));

            using (var db = this.getDBContext())
            {
                var user = db.Users.SingleOrDefault(u => u.UserName == userName);

                if (user != null)
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                    return new EFUser(user);
                }

                user = new EFUserPoco()
                {
                    UserName = userName,
                    Password = this.EncodePassword(password),
                    Email = email,
                    IsApproved = isApproved,
                    CreationDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    LastPasswordChangedDate = DateTime.Now
                };
                db.Users.Add(user);

                db.SaveChanges();

                status = MembershipCreateStatus.Success;
                return new EFUser(user);
            }
        }

        /// <summary>
        /// 从成员资格数据源删除一个用户。
        /// 根据GHY EF的实际需求，暂不提供该方法。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string userName, bool deleteAllRelatedData)
        {
            return false;
        }

        /// <summary>
        /// 获取一个成员资格用户的集合，其中的电子邮件地址包含要匹配的指定电子邮件地址。
        /// 该方法暂不支持。
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        /// <summary>
        /// 获取一个成员资格用户的集合，其中的用户名或真名包含要匹配的指定用户名。
        /// </summary>
        /// <param name="userNameToMatch">要搜索的用户名或真名。</param>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 从零开始。</param>
        /// <param name="pageSize">要返回的结果页的大小。</param>
        /// <param name="totalRecords">匹配用户的总数。</param>
        /// <returns>包含一页 pageSize 的 MembershipUser 对象的 MembershipUserCollection 集合，这些对象从 pageIndex 指定的页开始。</returns>
        public override MembershipUserCollection FindUsersByName(string userNameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userNameToMatch));
            Contract.Requires(pageIndex > 0);
            Contract.Requires(pageSize > 0);

            using (var db = this.getDBContext())
            {
                totalRecords = db.Users.Count(u => u.UserName.Contains(userNameToMatch) || u.RealName.Contains(userNameToMatch));
                var dbUsers = db.Users.Where(u => u.UserName.Contains(userNameToMatch) || u.RealName.Contains(userNameToMatch))
                    .OrderByDescending(u => u.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                var users = new MembershipUserCollection();

                foreach (var user in dbUsers)
                {
                    users.Add(new EFUser(user));
                }

                return users;
            }
        }

        /// <summary>
        /// 获取数据源中的所有用户的集合，并显示在数据页中。
        /// </summary>
        /// <param name="pageIndex">要返回的结果页的索引。pageIndex 从零开始。</param>
        /// <param name="pageSize">要返回的结果页的大小。</param>
        /// <param name="totalRecords">匹配用户的总数。</param>
        /// <returns>包含一页 pageSize 的 MembershipUser 对象的 MembershipUserCollection 集合，这些对象从 pageIndex 指定的页开始。</returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            Contract.Requires(pageIndex > 0);
            Contract.Requires(pageSize > 0);

            using (var db = this.getDBContext())
            {
                totalRecords = db.Users.Count();
                var dbUsers = db.Users.OrderByDescending(u => u.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);

                var users = new MembershipUserCollection();

                foreach (var user in dbUsers)
                {
                    users.Add(new EFUser(user));
                }

                return users;
            }
        }

        /// <summary>
        /// 获取当前访问该应用程序的用户数。
        /// 该方法暂不支持。
        /// </summary>
        /// <returns></returns>
        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        /// <summary>
        /// 从数据源获取指定用户名所对应的密码。
        /// 该方法在不实现。
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string GetPassword(string userName, string answer)
        {
            return string.Empty;
        }

        /// <summary>
        /// 从数据源获取用户的信息。
        /// </summary>
        /// <param name="userName">要获取其信息的用户名。</param>
        /// <param name="userIsOnline">该参数暂不支持。</param>
        /// <returns>用数据源中指定用户的信息填充的 MembershipUser 对象。</returns>
        public override MembershipUser GetUser(string userName, bool userIsOnline)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            using (var db = this.getDBContext())
            {
                return new EFUser(db.Users.SingleOrDefault(u => u.UserName == userName));
            }
        }

        /// <summary>
        /// 根据成员资格用户的唯一标识符从数据源获取用户信息。
        /// </summary>
        /// <param name="providerUserKey">要获取其信息的成员资格用户的唯一标识符。</param>
        /// <param name="userIsOnline">该参数暂不支持。</param>
        /// <returns>用数据源中指定用户的信息填充的 MembershipUser 对象。</returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            Contract.Requires(providerUserKey != null);
            int id = (int)providerUserKey;
            Contract.Requires(id > 0);

            using (var db = this.getDBContext())
            {
                return new EFUser(db.Users.Find(id));
            }
        }

        /// <summary>
        /// 获取与指定的电子邮件地址关联的用户名。
        /// 暂不支持该方法。
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override string GetUserNameByEmail(string email)
        {
            return string.Empty;
        }

        /// <summary>
        /// 初始化提供程序。
        /// </summary>
        /// <param name="name">该提供程序的友好名称。</param>
        /// <param name="config">名称/值对的集合，表示在配置中为该提供程序指定的、提供程序特定的属性。</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "EFMembershipProvider";
            }

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "GHY EF成员资格提供程序。");
            }

            base.Initialize(name, config);
            this.ApplicationName = string.IsNullOrEmpty(config["applicationName"]) ? HostingEnvironment.ApplicationVirtualPath : config["applicationName"];
            this.connStrName = config["connectionStringName"];
        }

        /// <summary>
        /// 将用户密码重置为一个自动生成的新密码。
        /// 暂不支持该方法。
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string ResetPassword(string username, string answer)
        {
            return string.Empty;
        }

        /// <summary>
        /// 清除锁定，以便可以验证该成员资格用户。
        /// 暂不支持该方法。
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override bool UnlockUser(string userName)
        {
            return false;
        }

        /// <summary>
        /// 更新数据源中有关用户的信息。
        /// </summary>
        /// <param name="user">一个 MembershipUser 对象，表示要更新的用户及其更新信息。</param>
        public override void UpdateUser(MembershipUser user)
        {
            Contract.Requires(user != null);
            Contract.Requires(user is EFUser);

            var efUser = user as EFUser;
            var poco = efUser.ToPoco();
            Contract.Requires(poco.Id > 0);

            using (var db = this.getDBContext())
            {
                var dbUser = db.Users.Find(poco.Id);

                if (dbUser == null)
                {
                    return;
                }

                if (!string.IsNullOrWhiteSpace(poco.Password))
                {
                    dbUser.Password = this.EncodePassword(poco.Password);
                }

                dbUser.RealName = poco.RealName;
                dbUser.Email = poco.Email;
                dbUser.TelNumber = poco.TelNumber;
                dbUser.Comment = poco.Comment;
                dbUser.IsApproved = poco.IsApproved;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 验证数据源中是否存在指定的用户名和密码。
        /// </summary>
        /// <param name="userName">要验证的用户的名称。</param>
        /// <param name="password">指定的用户的密码。</param>
        /// <returns>如果指定的用户名和密码有效，则为 true；否则为 false。</returns>
        public override bool ValidateUser(string userName, string password)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(password));
            string encodePassword = this.EncodePassword(password);

            using (var db = this.getDBContext())
            {
                var user = db.Users.SingleOrDefault(
                    u => u.UserName == userName && u.Password == encodePassword && u.IsApproved == true);

                if (user != null)
                {
                    user.LastLoginDate = DateTime.Now;
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }
        #endregion
    }
}