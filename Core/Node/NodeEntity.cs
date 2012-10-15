namespace GHY.EF.Core.Node
{
    /// <summary>
    /// 节点实体类。
    /// </summary>
    public class NodeEntity
    {
        #region 私有方法
        int[] StringToIntArray(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            string[] strings = str.Split('|');
            int[] intArray = new int[strings.Length - 1];

            for (int i = 0; i < strings.Length - 1; i++)
            {
                intArray[i] = int.Parse(strings[i + 1]);
            }

            return intArray;
        }

        string IntArrayToString(int[] intArray)
        {
            string[] strings = new string[intArray.Length];

            for (int i = 0; i < intArray.Length; i++)
            {
                strings[i] = intArray[i].ToString();
            }

            return "|" + string.Join("|", strings);
        }
        #endregion

        /// <summary>
        /// 获取或设置节点的Id。
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 获取或设置节点名。
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 获取或设置父节点Id。
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 获取或设置完全路径。
        /// </summary>
        public int[] FullIds { get; set; }

        /// <summary>
        /// 获取或设置完全路径的字符串版本。该更改将会同步修改到FullIds字段。
        /// </summary>
        public string FullIdsStringType
        {
            get
            {
                return this.IntArrayToString(this.FullIds);
            }

            set
            {
                this.FullIds = this.StringToIntArray(value);
            }
        }

        /// <summary>
        /// 获取或设置该节点的子节点Id。
        /// </summary>
        public int[] ChildIds { get; set; }

        /// <summary>
        /// 获取或设置子节点Id的字符串版本。该更改将会同步修改到ChildIds字段。
        /// </summary>
        public string ChildIdsStringType
        {
            get
            {
                return this.IntArrayToString(this.ChildIds);
            }

            set
            {
                this.ChildIds = this.StringToIntArray(value);
            }
        }

        /// <summary>
        /// 获取或设置该节点所使用的Application的Id。
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// 获取或设置该节点的图片保存路径。
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 获取或设置该节点是否需要审核。
        /// </summary>
        public bool NeedAudit { get; set; }

        /// <summary>
        /// 获取或设置备注。
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置该节点是否启用。
        /// </summary>
        public bool Enable { get; set; }
    }
}