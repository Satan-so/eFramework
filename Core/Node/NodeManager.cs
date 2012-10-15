using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics.Contracts;
using System.Web.UI.WebControls;
using GHY.EF.Core.Data;
using GHY.EF.Core.Security;

namespace GHY.EF.Core.Node
{
    /// <summary>
    /// 节点管理类。
    /// </summary>
    public class NodeManager
    {
        #region 私有字段
        Database db;
        #endregion

        #region 私有方法
        NodeEntity PopulateNode(IDataReader reader)
        {
            NodeEntity node = new NodeEntity()
            {
                NodeId = (int)reader["NodeId"],
                NodeName = reader["NodeName"].ToString(),
                ParentId = (int)reader["ParentId"],
                FullIdsStringType = reader["FullIds"].ToString(),
                ChildIdsStringType = reader["ChildIds"].ToString(),
                ApplicationId = (int)reader["ApplicationId"],
                ImagePath = reader["ImagePath"].ToString(),
                NeedAudit = (bool)reader["NeedAudit"],
                Comment = reader["Comment"].ToString(),
                Enable = (bool)reader["Enable"]
            };

            return node;
        }

        void FillTreeNode(TreeNode treeNode, StringDictionary roles, ActionType actionType, string url, Dictionary<int, NodeEntity> nodes, int nodeId, bool needCheckRole)
        {
            if (nodes[nodeId].ChildIds == null || nodes[nodeId].ChildIds.Length == 0)
            {
                return;
            }

            for (int i = 0; i < nodes[nodeId].ChildIds.Length; i++)
            {
                if (needCheckRole)
                {
                    if (this.CheckNodeRole(nodes[nodes[nodeId].ChildIds[i]], roles, actionType))
                    {
                        needCheckRole = false;
                    }
                    else
                    {
                        break;
                    }
                }

                TreeNode currentNode = new TreeNode(nodes[nodes[nodeId].ChildIds[i]].NodeName) { NavigateUrl = url + nodes[nodeId].ChildIds[i].ToString() };
                this.FillTreeNode(currentNode, roles, actionType, url, nodes, nodes[nodeId].ChildIds[i], needCheckRole);
                treeNode.ChildNodes.Add(currentNode);
            }
        }

        bool CheckNodeAncestorsRole(NodeEntity node, StringDictionary roles, string roleName)
        {
            for (int i = 0; i < node.FullIds.Length; i++)
            {
                if (roles.ContainsKey(node.FullIds[i].ToString() + roleName))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 用连接字符串构造节点管理类。
        /// </summary>
        /// <param name="connectionStringName">连接字符串。</param>
        public NodeManager(string connectionStringName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(connectionStringName));

            this.db = new Database(connectionStringName);
        }

        /// <summary>
        /// 添加一个节点。
        /// </summary>
        /// <param name="node">待添加的节点。</param>
        public void Add(NodeEntity node)
        {
            Contract.Requires(node != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(node.NodeName));
            Contract.Requires(node.ParentId > 0);
            Contract.Requires(node.ApplicationId > 0);

            IDataParameter[] parameters = new IDataParameter[7];

            parameters[0] = this.db.NewDataParameter("@NodeName", node.NodeName);
            parameters[1] = this.db.NewDataParameter("@ParentId", node.ParentId);
            parameters[2] = this.db.NewDataParameter("@ApplicationId", node.ApplicationId);
            parameters[3] = this.db.NewDataParameter("@ImagePath", node.ImagePath);
            parameters[4] = this.db.NewDataParameter("@NeedAudit", node.NeedAudit);
            parameters[5] = this.db.NewDataParameter("@Comment", node.Comment);
            parameters[6] = this.db.NewDataParameter("@Enable", node.Enable);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Node_Add", parameters);

            if (result == 0)
            {
                throw new Exception("新建节点失败！");
            }
        }

        /// <summary>
        /// 更新一个节点。
        /// </summary>
        /// <param name="node">待更新的节点。</param>
        public void Update(NodeEntity node)
        {
            Contract.Requires(node != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(node.NodeName));
            Contract.Requires(node.ParentId > 0);
            Contract.Requires(node.ApplicationId > 0);

            IDataParameter[] parameters = new IDataParameter[7];

            parameters[0] = this.db.NewDataParameter("@NodeId", node.NodeId);
            parameters[1] = this.db.NewDataParameter("@NodeName", node.NodeName);
            parameters[2] = this.db.NewDataParameter("@ApplicationId", node.ApplicationId);
            parameters[3] = this.db.NewDataParameter("@ImagePath", node.ImagePath);
            parameters[4] = this.db.NewDataParameter("@NeedAudit", node.NeedAudit);
            parameters[5] = this.db.NewDataParameter("@Comment", node.Comment);
            parameters[6] = this.db.NewDataParameter("@Enable", node.Enable);

            int result = this.db.ExecuteNonQuery(CommandType.StoredProcedure, "EF_Node_Update", parameters);

            if (result == 0)
            {
                throw new Exception("更新节点失败！");
            }
        }

        /// <summary>
        /// 获取一个节点。
        /// </summary>
        /// <param name="nodeId">节点Id。</param>
        /// <returns>欲获取的节点。</returns>
        public NodeEntity Get(int nodeId)
        {
            Contract.Requires(nodeId > 0);

            IDataParameter[] parameters = new IDataParameter[1];
            parameters[0] = this.db.NewDataParameter("@NodeId", nodeId);

            NodeEntity node;

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Node_Get", parameters))
            {
                if (reader.Read())
                {
                    node = this.PopulateNode(reader);
                }
                else
                {
                    node = new NodeEntity();
                }
            }

            return node;
        }

        /// <summary>
        /// 获取全部节点的KV集合。
        /// </summary>
        /// <returns>节点的KV集合。</returns>
        public Dictionary<int, NodeEntity> GetAll()
        {
            Dictionary<int, NodeEntity> nodes = new Dictionary<int, NodeEntity>();

            using (IDataReader reader = this.db.ExcuteReader(CommandType.StoredProcedure, "EF_Node_GetAll"))
            {
                while (reader.Read())
                {
                    NodeEntity node = this.PopulateNode(reader);
                    nodes.Add(node.NodeId, node);
                }
            }

            return nodes;
        }

        /// <summary>
        /// 填充一个树状视图控件。
        /// </summary>
        /// <param name="treeView">欲填充的视图控件。</param>
        /// <param name="roles">权限集合。</param>
        /// <param name="actionType">动作的类型。</param>
        /// <param name="url">点击节点后跳转的URL。</param>
        public void FillTreeView(TreeView treeView, StringDictionary roles, ActionType actionType, string url)
        {
            Dictionary<int, NodeEntity> nodes = this.GetAll();

            TreeNode treeNode = new TreeNode(nodes[1].NodeName) { NavigateUrl = url + "1" };
            this.FillTreeNode(treeNode, roles, actionType, url, nodes, 1, true);
            treeView.Nodes.Add(treeNode);
        }

        /// <summary>
        /// 检验是否具有指定节点的指定动作的权限。
        /// </summary>
        /// <param name="node">待检查节点。</param>
        /// <param name="roles">权限字典。</param>
        /// <param name="actionType">动作的类型。</param>
        /// <returns>是否具有权限。</returns>
        public bool CheckNodeRole(NodeEntity node, StringDictionary roles, ActionType actionType)
        {
            if (roles.ContainsKey("Administrator"))
            {
                return true;
            }
            else if (actionType == ActionType.AddInfo)
            {
                if (roles.ContainsKey("ContentAdmin") || this.CheckNodeAncestorsRole(node, roles, "Admin") || this.CheckNodeAncestorsRole(node, roles, "Author"))
                {
                    return true;
                }
            }
            else if (actionType == ActionType.ManageInfo)
            {
                if (roles.ContainsKey("ContentAdmin") || this.CheckNodeAncestorsRole(node, roles, "Admin"))
                {
                    return true;
                }
            }
            else if (actionType == ActionType.ManageRole)
            {
                if (roles.ContainsKey("MemberAdmin"))
                {
                    return true;
                }
            }
            else if (actionType == ActionType.ManageNode)
            {
                if (roles.ContainsKey("ContentAdmin") || this.CheckNodeAncestorsRole(node, roles, "Admin"))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}