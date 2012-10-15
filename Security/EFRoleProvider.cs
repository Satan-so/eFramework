using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.Contracts;
using System.Web.Hosting;
using System.Web.Security;
using GHY.EF.Core.Data;

namespace GHY.EF.Security
{
    /// <summary>
    /// EF角色提供程序。
    /// </summary>
    public class EFRoleProvider : RoleProvider
    {
        #region 私有字段
        Database db;
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
            foreach (string userName in userNames)
            {
                foreach (string roleName in roleNames)
                {
                    if (IsUserInRole(userName, roleName))
                    {
                        return;
                    }

                    IDataParameter[] parameters = new IDataParameter[3];

                    parameters[0] = this.db.NewDataParameter("@UserName", userName);
                    parameters[1] = this.db.NewDataParameter("@RoleName", roleName);
                    parameters[2] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

                    this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_UsersInRoles_Add", parameters);
                }
            }
        }

        /// <summary>
        /// 在数据源中为已配置的 applicationName 添加一个新角色。
        /// </summary>
        /// <param name="roleName">要创建的角色的名称。</param>
        public override void CreateRole(string roleName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(roleName));

            if (this.RoleExists(roleName))
            {
                return;
            }

            IDataParameter[] parameters = new IDataParameter[2];

            parameters[0] = this.db.NewDataParameter("@RoleName", roleName);
            parameters[1] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Role_Add", parameters);
        }

        /// <summary>
        /// 从数据源中移除已配置的 applicationName 的角色。 
        /// </summary>
        /// <param name="roleName">要删除的角色的名称。</param>
        /// <param name="throwOnPopulatedRole">如果为 true，则在 roleName 具有一个或多个成员时引发异常，并且不删除 roleName。根据GHY实际需求，不提供指定该项，而强制为true,并且不引发异常。</param>
        /// <returns>如果成功删除角色，则为 true；否则为 false。</returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throwOnPopulatedRole = true;

            if (throwOnPopulatedRole && this.GetUsersInRole(roleName).Length > 0)
            {
                return false;
            }

            IDataParameter[] parameters = new IDataParameter[2];

            parameters[0] = this.db.NewDataParameter("@RoleName", roleName);
            parameters[1] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Role_Delete", parameters);

            if (result == 0)
            {
                return false;
            }

            return true;

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
            IDataParameter[] parameters = new IDataParameter[1];

            parameters[0] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            StringCollection rolesCollection = new StringCollection();

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Roles_GetAll", parameters))
            {
                while (reader.Read())
                {
                    rolesCollection.Add(reader["RoleName"].ToString());
                }
            }

            string[] roles = new string[rolesCollection.Count];
            rolesCollection.CopyTo(roles, 0);

            return roles;
        }

        /// <summary>
        /// 获取指定用户对于已配置的 applicationName 所属于的角色的列表。
        /// </summary>
        /// <param name="userName">要为其返回角色列表的用户。</param>
        /// <returns>一个字符串数组，其中包含指定用户对于已配置的 applicationName 所属于的所有角色的名称。</returns>
        public override string[] GetRolesForUser(string userName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));

            IDataParameter[] parameters = new IDataParameter[2];

            parameters[1] = this.db.NewDataParameter("@UserName", userName);
            parameters[0] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            StringCollection rolesCollection = new StringCollection();

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_UsersInRoles_GetRoles", parameters))
            {
                while (reader.Read())
                {
                    rolesCollection.Add(reader["RoleName"].ToString());
                }
            }

            string[] roles = new string[rolesCollection.Count];
            rolesCollection.CopyTo(roles, 0);

            return roles;
        }

        /// <summary>
        /// 获取属于已配置的 applicationName 的指定角色的用户的列表。
        /// </summary>
        /// <param name="roleName">一个角色名称，将获取该角色的用户列表。</param>
        /// <returns>一个字符串数组，其中包含属于已配置的 applicationName 的指定角色的成员的所有用户名。</returns>
        public override string[] GetUsersInRole(string roleName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(roleName));

            IDataParameter[] parameters = new IDataParameter[2];

            parameters[1] = this.db.NewDataParameter("@RoleName", roleName);
            parameters[0] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            StringCollection usersCollection = new StringCollection();

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_UsersInRoles_GetUsers", parameters))
            {
                while (reader.Read())
                {
                    usersCollection.Add(reader["UserName"].ToString());
                }
            }

            string[] users = new string[usersCollection.Count];
            usersCollection.CopyTo(users, 0);

            return users;
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

            this.db = new Database(config["connectionStringName"]);
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

            IDataParameter[] parameters = new IDataParameter[3];

            parameters[0] = this.db.NewDataParameter("@UserName", userName);
            parameters[1] = this.db.NewDataParameter("@RoleName", roleName);
            parameters[2] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            int result = (int)this.db.ExecuteScalar(CommandType.StoredProcedure, "EF_UsersInRoles_Exists", parameters);

            if (result > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 移除已配置的 applicationName 的指定角色中的指定用户名。
        /// 基于安全考虑，该方法实现只处理参数数组第一个值。
        /// </summary>
        /// <param name="userNames">一个字符串数组，其中包含要从指定角色中移除的用户名。</param>
        /// <param name="roleNames">一个字符串数组，其中包含要将指定用户名从中移除的角色的名称。</param>
        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            foreach (string userName in userNames)
            {
                foreach (string roleName in roleNames)
                {
                    IDataParameter[] parameters = new IDataParameter[3];

                    parameters[0] = this.db.NewDataParameter("@UserName", userName);
                    parameters[1] = this.db.NewDataParameter("@RoleName", roleName);
                    parameters[2] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

                    this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_UsersInRoles_Delete", parameters);
                }
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

            IDataParameter[] parameters = new IDataParameter[2];

            parameters[0] = this.db.NewDataParameter("@RoleName", roleName);
            parameters[1] = this.db.NewDataParameter("@ApplicationName", this.ApplicationName);

            int result = (int)this.db.ExecuteScalar(CommandType.StoredProcedure, "EF_Role_Exists", parameters);

            if (result > 0)
            {
                return true;
            }

            return false;

        }
        #endregion
    }
}