namespace GHY.EF.Security
{
    /// <summary>
    /// 用户角色关联类。
    /// </summary>
    public class EFUserInRole
    {
        /// <summary>
        /// 获取或设置Id。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置角色名。
        /// </summary>
        public string RoleName { get; set; }
    }
}
