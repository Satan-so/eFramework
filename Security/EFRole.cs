using System.ComponentModel.DataAnnotations;

namespace GHY.EF.Security
{
    /// <summary>
    /// EF角色。
    /// </summary>
    [Table("EF_Roles")]
    public class EFRole
    {
        /// <summary>
        /// 获取或设置角色Id。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置角色名。
        /// </summary>
        public string RoleName { get; set; }
    }
}
