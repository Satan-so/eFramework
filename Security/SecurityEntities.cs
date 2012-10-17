using System.Configuration;
using System.Data.Entity;

namespace GHY.EF.Security
{
    /// <summary>
    /// EF安全数据实体集。
    /// </summary>
    class SecurityEntities : DbContext
    {
        /// <summary>
        /// 获取或设置用户数据集。
        /// </summary>
        public DbSet<EFUser> Users { get; set; }

        /// <summary>
        /// 获取或设置角色数据集。
        /// </summary>
        public DbSet<EFRole> Roles { get; set; }

        /// <summary>
        /// 获取或设置用户角色关联数据集。
        /// </summary>
        public DbSet<EFUserInRole> UserInRoles { get; set; }

        /// <summary>
        /// 使用指定连接字符串构造实例。
        /// </summary>
        /// <param name="connectionStringName">指定的连接字符串名。</param>
        public SecurityEntities(string connectionStringName)
            : base(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString) { }
    }
}
