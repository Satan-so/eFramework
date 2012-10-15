using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Web.Caching;
using System.Web.UI.WebControls;

namespace GHY.EF.Core.Info
{
    /// <summary>
    /// 内容列表显示控件。
    /// </summary>
    public class InfoRepeater : Repeater
    {
        #region 私有静态成员
        const string nodeCountString = "Node{0}Count";
        const string nodeListString = "Node{0}List";
        #endregion

        #region 保护方法
        protected override void OnDataBinding(EventArgs e)
        {
            if (this.NodeId < 1 || this.PageIndex < 1 || this.PageSize < 1)
            {
                throw new Exception("参数配置错误！");
            }

            if (this.Count <= 0)
            {
                if (string.IsNullOrWhiteSpace(this.ConnectionStringName))
                {
                    throw new Exception("连接字符串未定义或为空！");
                }

                List<InfoEntity> infos;

                if (this.PageIndex == 1 && this.CacheTime > 0 && this.Page.Cache[string.Format(nodeCountString, this.NodeId.ToString())] != null && this.Page.Cache[string.Format(nodeListString, this.NodeId.ToString())] != null)
                {
                    infos = (List<InfoEntity>)this.Page.Cache[string.Format(nodeListString, this.NodeId.ToString())];
                    this.Count = (int)this.Page.Cache[string.Format(nodeCountString, this.NodeId.ToString())];
                }
                else
                {
                    int count;
                    infos = this.GetInfos(out count);
                    this.Count = count;

                    if (this.PageIndex == 1 && this.CacheTime > 0)
                    {
                        this.Page.Cache.Insert(string.Format(nodeListString, this.NodeId.ToString()), infos, null, DateTime.Now.AddMinutes(this.CacheTime), Cache.NoSlidingExpiration);
                        this.Page.Cache.Insert(string.Format(nodeCountString, this.NodeId.ToString()), count, null, DateTime.Now.AddMinutes(this.CacheTime), Cache.NoSlidingExpiration);
                    }
                }

                this.DataSource = infos;
            }

            base.OnDataBinding(e);
        }

        /// <summary>
        /// 获取信息集合。各种具体类型的信息Repeater重载该方法以绑定具体的信息类型。
        /// </summary>
        /// <param name="count">信息总数。</param>
        /// <returns>信息集合。</returns>
        protected virtual List<InfoEntity> GetInfos(out int count)
        {
            InfoManager infoManager = new InfoManager(this.ConnectionStringName);
            return infoManager.GetByNodeId(this.NodeId, false, this.PageIndex, this.PageSize, out count);
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 获取或设置连接字符串名。
        /// </summary>
        [Bindable(true), Category("Data"), Description("数据库连接字符串名。")]
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// 获取或设置节点Id。
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue("1"), Description("节点Id。")]
        public int NodeId { get; set; }

        /// <summary>
        /// 获取或设置每页大小。
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue("20"), Description("每页大小。")]
        public int PageSize { get; set; }

        /// <summary>
        /// 获取或设置缓存时间。以分钟为单位。小于0时不缓存。
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue("0"), Description("缓存时间。")]
        public int CacheTime { get; set; }

        /// <summary>
        /// 获取或设置当前页索引。
        /// </summary>
        [Bindable(true), Category("Data"), DefaultValue("1"), Description("当前页索引。")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 获取或设置信息总数。
        /// </summary>
        public int Count { get; protected set; }
        #endregion
    }
}