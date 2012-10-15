using System;

namespace GHY.EF.Core.Info
{
    /// <summary>
    /// 内容实体类。
    /// EF中的内容类均应继承自该类。
    /// </summary>
    public class InfoEntity
    {
        /// <summary>
        /// 获取或设置内容的Id。
        /// </summary>
        public int InfoId { get; set; }

        /// <summary>
        /// 获取或设置内容的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置内容的正文。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置内容的作者。
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// 获取或设置内容的审核人。
        /// </summary>
        public string AuditName { get; set; }

        /// <summary>
        /// 获取或设置内容的来源。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 获取或设置内容的链接地址。
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 获取或设置内容的封面图片。
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 获取或设置内容是否置顶。
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 获取或设置所属节点的Id。
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 获取或设置所属节点的完全路径。
        /// </summary>
        public string FullNodeIds { get; set; }

        /// <summary>
        /// 获取或设置内容的创建时间。
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// 获取或设置内容的更新时间。
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 获取或设置内容的状态。
        /// </summary>
        public InfoState State { get; set; }
    }
}