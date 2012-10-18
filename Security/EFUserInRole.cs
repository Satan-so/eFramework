using System.ComponentModel.DataAnnotations;

namespace GHY.EF.Security
{
    /// <summary>
    /// �û���ɫ�����ࡣ
    /// </summary>
    [Table("EF_UserInRoles")]
    public class EFUserInRole
    {
        /// <summary>
        /// ��ȡ������Id��
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ��ȡ�������û�����
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// ��ȡ�����ý�ɫ����
        /// </summary>
        public string RoleName { get; set; }
    }
}
