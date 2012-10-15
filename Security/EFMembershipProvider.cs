using System;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;
using System.Web.Security;
using GHY.EF.Core.Data;

namespace GHY.EF.Security
{
    /// <summary>
    /// EF成员资格提供程序。
    /// </summary>
    public class EFMembershipProvider : MembershipProvider
    {
        #region 私有字段
        Database db;
        #endregion

        #region 私有方法
        string EncodePassword(string password)
        {
            byte[] data;

            using (MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider())
            {
                data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));
            }

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        EFUser PopulateUser(IDataReader reader)
        {
            EFUser user = new EFUser(this.Name, reader["UserName"].ToString(), null, reader["Email"].ToString(), string.Empty, reader["Comment"].ToString(), (bool)reader["IsApproved"], false, (DateTime)reader["CreationDate"], (DateTime)reader["LastLoginDate"], DateTime.Now, (DateTime)reader["LastPasswordChangedDate"], DateTime.Now)
            {
                UserId = (int)reader["UserId"],
                RealName = reader["RealName"].ToString(),
                TelNumber = reader["TelNumber"].ToString()
            };

            return user;
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

            if (!this.ValidateUser(userName, oldPassword))
            {
                return false;
            }

            IDataParameter[] parameters = new IDataParameter[3];

            parameters[0] = this.db.NewDataParameter("@UserName", userName);
            parameters[1] = this.db.NewDataParameter("@Password", this.EncodePassword(newPassword));
            parameters[2] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Membership_ChangePassword", parameters);

            if (result == 0)
            {
                return false;
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

            if (this.GetUser(userName, false) != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            IDataParameter[] parameters = new IDataParameter[5];

            parameters[0] = this.db.NewDataParameter("@UserName", userName);
            parameters[1] = this.db.NewDataParameter("@Password", this.EncodePassword(password));
            parameters[2] = this.db.NewDataParameter("@Email", email);
            parameters[3] = this.db.NewDataParameter("@IsApproved", isApproved);
            parameters[4] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Membership_Add", parameters);

            if (result == 0)
            {
                status = MembershipCreateStatus.ProviderError;
                return null;
            }

            status = MembershipCreateStatus.Success;
            return this.GetUser(userName, false);
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
            return new MembershipUserCollection();
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

            IDataParameter[] parameters = new IDataParameter[5];

            parameters[0] = this.db.NewDataParameter("@SearchName", "%" + userNameToMatch + "%");
            parameters[1] = this.db.NewDataParameter("@PageIndex", pageIndex);
            parameters[2] = this.db.NewDataParameter("@PageSize", pageSize);
            parameters[3] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);
            parameters[4] = this.db.NewOutputDataParameter("@Count");
            parameters[4].DbType = DbType.Int32;

            MembershipUserCollection users = new MembershipUserCollection();

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Membership_FindByName", parameters))
            {
                while (reader.Read())
                {
                    users.Add(this.PopulateUser(reader));
                }
            }

            totalRecords = (int)this.db.GetOutputDataParameter("@Count").Value;

            return users;
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

            IDataParameter[] parameters = new IDataParameter[4];

            parameters[0] = this.db.NewDataParameter("@PageIndex", pageIndex);
            parameters[1] = this.db.NewDataParameter("@PageSize", pageSize);
            parameters[2] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);
            parameters[3] = this.db.NewOutputDataParameter("@Count");
            parameters[3].DbType = DbType.Int32;

            MembershipUserCollection users = new MembershipUserCollection();

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Membership_GetAll", parameters))
            {
                while (reader.Read())
                {
                    users.Add(this.PopulateUser(reader));
                }
            }

            totalRecords = (int)this.db.GetOutputDataParameter("@Count").Value;

            return users;
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
        /// 从数据源获取用户的信息。提供一个更新用户最近一次活动的日期/时间戳的选项。
        /// </summary>
        /// <param name="userName">要获取其信息的用户名。</param>
        /// <param name="userIsOnline">该参数暂不支持。如果为 true，则更新用户最近一次活动的日期/时间戳；如果为 false，则返回用户信息，但不更新用户最近一次活动的日期/时间戳。</param>
        /// <returns>用数据源中指定用户的信息填充的 MembershipUser 对象。</returns>
        public override MembershipUser GetUser(string userName, bool userIsOnline)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            IDataParameter[] parameters = new IDataParameter[2];
            parameters[0] = this.db.NewDataParameter("@UserName", userName);
            parameters[1] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            EFUser user;

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Membership_Get", parameters))
            {
                if (reader.Read())
                {
                    user = this.PopulateUser(reader);
                }
                else
                {
                    user = null;
                }
            }

            return user;
        }

        /// <summary>
        /// 根据成员资格用户的唯一标识符从数据源获取用户信息。提供一个更新用户最近一次活动的日期/时间戳的选项。
        /// 暂不支持该方法。
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return null;
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

            this.db = new Database(config["connectionStringName"]);
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

            EFUser efUser = user as EFUser;

            Contract.Requires(efUser.UserId > 0);

            IDataParameter[] parameters = new IDataParameter[9];

            parameters[0] = this.db.NewDataParameter("@UserId", efUser.UserId);
            parameters[1] = this.db.NewDataParameter("@UserName", efUser.UserName);
            parameters[2] = this.db.NewDataParameter("@Password", string.IsNullOrWhiteSpace(efUser.Password) ? string.Empty : this.EncodePassword(efUser.Password));
            parameters[3] = this.db.NewDataParameter("@RealName", efUser.RealName);
            parameters[4] = this.db.NewDataParameter("@Email", efUser.Email);
            parameters[5] = this.db.NewDataParameter("@TelNumber", efUser.TelNumber);
            parameters[6] = this.db.NewDataParameter("@Comment", efUser.Comment);
            parameters[7] = this.db.NewDataParameter("@IsApproved", efUser.IsApproved);
            parameters[8] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Membership_Update", parameters);

            if (result == 0)
            {
                throw new Exception("更新应用失败！");
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

            IDataParameter[] parameters = new IDataParameter[3];
            parameters[0] = this.db.NewDataParameter("@UserName", userName);
            parameters[1] = this.db.NewDataParameter("@Password", this.EncodePassword(password));
            parameters[2] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Membership_Validate", parameters);

            if (result == 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}