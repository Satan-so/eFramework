using System;

namespace GHY.EF.Core.Application
{
    /// <summary>
    /// 应用实体类。
    /// </summary>
    public class ApplicationEntity
    {
        /// <summary>
        /// 获取或设置应用的Id。
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 获取或设置应用名。
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// 获取或设置该应用的Owner。
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 获取或设置该应用的备注。
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置该应用的创建时间。
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 获取或设置该应用的更新时间。
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 获取或设置该应用是否启用。
        /// </summary>
        public bool Enable { get; set; }
    }
}
