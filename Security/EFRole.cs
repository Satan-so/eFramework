using System.ComponentModel.DataAnnotations;

namespace GHY.EF.Security
{
    /// <summary>
    /// EF��ɫ��
    /// </summary>
    [Table("EF_Roles")]
    public class EFRole
    {
        /// <summary>
        /// ��ȡ�����ý�ɫId��
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ��ȡ�����ý�ɫ����
        /// </summary>
        public string RoleName { get; set; }
    }
}
