<%@ Page Title="编辑节点信息" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.content.update_node" %>
<%@ Register Src="~/content/content_sidebar.ascx" TagName="SideBar" TagPrefix="EF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>编辑节点信息</h2>
    <div class="node-info">
        <fieldset class="update-node">
            <legend>节点信息</legend>
            <p>
                <asp:Label ID="nodeNameLabel" runat="server" AssociatedControlID="nodeName">节点名:</asp:Label>
                <asp:TextBox ID="nodeName" runat="server" CssClass="text-entry" ReadOnly="True"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="imagePathLabel" runat="server" AssociatedControlID="imagePath">图片保存路径:</asp:Label>
                <asp:TextBox ID="imagePath" runat="server" CssClass="text-entry" ReadOnly="True"></asp:TextBox>
            </p>
            <p>
                <asp:CheckBox ID="needAudit" runat="server" Text="是否需要审核" />
            </p>
            <p>
                <asp:Label ID="commentLabel" runat="server" AssociatedControlID="comment">备注:</asp:Label>
                <asp:TextBox ID="comment" runat="server" CssClass="text-entry" TextMode="MultiLine"></asp:TextBox>
            </p>
            <p>
                <asp:CheckBox ID="enable" runat="server" Text="是否启用" />
            </p>
        </fieldset>
        <p>
            <asp:Button ID="updateNodeButton" runat="server" Text="保存" 
                CssClass="input-button" onclick="updateNodeButton_Click" />

            <asp:HyperLink ID="addNodeLink" runat="server" NavigateUrl="add_node.aspx?parentid=" CssClass="input-button">添加子节点</asp:HyperLink>
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
    <EF:SideBar ID="sideBar" runat="server" />
</asp:Content>
