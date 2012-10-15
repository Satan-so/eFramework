<%@ Control Language="C#" AutoEventWireup="true" Inherits="GHY.EF.Admin.content.content_sidebar" %>
<asp:Menu ID="sidebarMenu" runat="server" CssClass="sidebar" IncludeStyleBlock="false">
    <Items>
        <asp:MenuItem NavigateUrl="add_info.aspx" Text="添加内容"></asp:MenuItem>
        <asp:MenuItem NavigateUrl="info_manager.aspx" Text="管理内容"></asp:MenuItem>
        <asp:MenuItem NavigateUrl="node_manager.aspx" Text="管理节点"></asp:MenuItem>
    </Items>
</asp:Menu>