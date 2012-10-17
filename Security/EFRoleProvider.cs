using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Hosting;
using System.Web.Security;

namespace GHY.EF.Security
{
    /// <summary>
    /// EF角色提供程序。
    /// </summary>
    public class EFRoleProvider : RoleProvider
    {
        #region 私有字段
        string connStrName;
        #endregion

        #region 私有方法
        SecurityEntities getDBContext()
        {
            return new SecurityEntities(this.connStrName);
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 获取或设置要存储和检索其角色信息的应用程序的名称。
        /// </summary>
        public override string ApplicationName { get; set; }
        #endregion

        #region 公共方法
        /// <summary>
        /// 将指定用户名添加到已配置的 applicationName 的指定角色名。
        /// 注意：根据EF实际需求，该方法实现只处理参数数组第一个值。
        /// </summary>
        /// <param name="userNames">一个字符串数组，其中包含要添加到指定角色的用户名。</param>
        /// <param name="roleNames">一个字符串数组，其中包含要将指定用户名添加到的角色的名称。</param>
        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            Contract.Requires(userNames.Length > 0);
            Contract.Requires(roleNames.Length > 0);

            using (var db = this.getDBContext())
            {
                foreach (string userName in userNames)
                {
                    foreach (string roleName in roleNames)
                    {
                        if (db.UserInRoles.SingleOrDefault(ur => ur.UserName == userName && ur.RoleName == roleName) != null)
                        {
                            break;
                        }

                        var userInRole = new EFUserInRole()
                        {
                            UserName = userName,
                            RoleName = roleName
                        };

                        db.UserInRoles.Add(userInRole);
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 在数据源中为已配置的 applicationName 添加一个新角色。
        /// </summary>
        /// <param name="roleName">要创建的角色的名称。</param>
        public override void CreateRole(string roleName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(roleName));

            using (var db = this.getDBContext())
            {
                var role = db.Roles.SingleOrDefault(r => r.RoleName == roleName);

                if (role != null)
                {
                    return;
                }

                role = new EFRole()
                {
                    RoleName = roleName
                };
                db.Roles.Add(role);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 从数据源中移除已配置的 applicationName 的角色。 
        /// </summary>
        /// <param name="roleName">要删除的角色的名称。</param>
        /// <param name="throwOnPopulatedRole">如果为 true，则在 roleName 具有一个或多个成员时引发异常，并且不删除 roleName。根据GHY实际需求，不提供指定该项，而强制为true,并且不引发异常。</param>
        /// <returns>如果成功删除角色，则为 true；否则为 false。</returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(roleName));
            throwOnPopulatedRole = true;

            using (var db = this.getDBContext())
            {
                if (throwOnPopulatedRole && db.UserInRoles.Count(ur => ur.RoleName == roleName) > 0)
                {
                    return false;
                }

                var role = db.Roles.SingleOrDefault(r => r.RoleName == roleName);
                db.Roles.Remove(role);

                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 获取属于某个角色且与指定的用户名相匹配的用户名的数组。
        /// 暂不提供该方法。
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="usernameToMatch"></param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return null;
        }

        /// <summary>
        /// 获取已配置的 applicationName 的所有角色的列表。
        /// </summary>
        /// <returns>一个字符串数组，包含在数据源中存储的已配置的 applicationName 的所有角色的名称。</returns>
        public override string[] GetAllRoles()
        {
            using (var db = this.getDBContext())
            {
                var dbRoles = db.Roles.OrderByDescending(r => r.Id).ToArray();
                string[] roles = new string[dbRoles.Length];

                for (int i = 0; i < roles.Length; i++)
                {
                    roles[i] = dbRoles[i].RoleName;
                }

                return roles;
            }
        }

        /// <summary>
        /// 获取指定用户对于已配置的 applicationName 所属于的角色的列表。
        /// </summary>
        /// <param name="userName">要为其返回角色列表的用户。</param>
        /// <returns>一个字符串数组，其中包含指定用户对于已配置的 applicationName 所属于的所有角色的名称。</returns>
        public override string[] GetRolesForUser(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            using (var db = this.getDBContext())
            {
                var dbRoles = db.UserInRoles.Where(ur => ur.UserName == userName).ToArray();
                string[] roles = new string[dbRoles.Length];

                for (int i = 0; i < roles.Length; i++)
                {
                    roles[i] = dbRoles[i].RoleName;
                }

                return roles;
            }
        }

        /// <summary>
        /// 获取属于已配置的 applicationName 的指定角色的用户的列表。
        /// </summary>
        /// <param name="roleName">一个角色名称，将获取该角色的用户列表。</param>
        /// <returns>一个字符串数组，其中包含属于已配置的 applicationName 的指定角色的成员的所有用户名。</returns>
        public override string[] GetUsersInRole(string roleName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(roleName));

            using (var db = this.getDBContext())
            {
                var dbUsers = db.UserInRoles.Where(ur => ur.RoleName == roleName).ToArray();
                string[] users = new string[dbUsers.Length];

                for (int i = 0; i < users.Length; i++)
                {
                    users[i] = dbUsers[i].UserName;
                }

                return users;
            }
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
                name = "EFRoleProvider";
            }

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "GHY EF角色提供程序。");
            }

            base.Initialize(name, config);
            this.ApplicationName = string.IsNullOrEmpty(config["applicationName"]) ? HostingEnvironment.ApplicationVirtualPath : config["applicationName"];

            this.connStrName = config["connectionStringName"];
        }

        /// <summary>
        /// 获取一个值，指示指定用户是否属于已配置的 applicationName 的指定角色。
        /// </summary>
        /// <param name="userName">要搜索的用户名。</param>
        /// <param name="roleName">作为搜索范围的角色。</param>
        /// <returns>如果指定用户属于已配置的 applicationName 的指定角色，则为 true；否则为 false。</returns>
        public override bool IsUserInRole(string userName, string roleName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            Contract.Requires(!string.IsNullOrWhiteSpace(roleName));

            using (var db = this.getDBContext())
            {
                return db.UserInRoles.SingleOrDefault(ur => ur.UserName == userName && ur.RoleName == roleName) != null;
            }
        }

        /// <summary>
        /// 移除已配置的 applicationName 的指定角色中的指定用户名。
        /// 基于安全考虑，该方法实现只处理参数数组第一个值。
        /// </summary>
        /// <param name="userNames">一个字符串数组，其中包含要从指定角色中移除的用户名。</param>
        /// <param name="roleNames">一个字符串数组，其中包含要将指定用户名从中移除的角色的名称。</param>
        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            Contract.Requires(userNames.Length > 0);
            Contract.Requires(roleNames.Length > 0);

            using (var db = this.getDBContext())
            {
                foreach (string userName in userNames)
                {
                    foreach (string roleName in roleNames)
                    {
                        var userInRole = db.UserInRoles.SingleOrDefault(ur => ur.UserName == userName && ur.RoleName == roleName);

                        if (userInRole != null)
                        {
                            db.UserInRoles.Remove(userInRole);
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 获取一个值，该值指示指定角色名是否已存在于已配置的 applicationName 的角色数据源中。
        /// </summary>
        /// <param name="roleName">要在数据源中搜索的角色的名称。</param>
        /// <returns>如果角色名已存在于已配置的 applicationName 的数据源中，则为 true；否则为 false。</returns>
        public override bool RoleExists(string roleName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(roleName));

            using (var db = this.getDBContext())
            {
                return db.Roles.SingleOrDefault(r => r.RoleName == roleName) != null;
            }
        }
        #endregion
    }
}