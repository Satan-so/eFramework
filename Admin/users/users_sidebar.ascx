<%@ Control Language="C#" AutoEventWireup="true" Inherits="GHY.EF.Admin.users.users_sidebar" %>
<asp:Menu ID="sidebarMenu" runat="server" CssClass="sidebar" IncludeStyleBlock="false">
    <Items>
        <asp:MenuItem NavigateUrl="add_user.aspx" Text="添加成员"></asp:MenuItem>
        <asp:MenuItem NavigateUrl="user_list.aspx" Text="成员管理"></asp:MenuItem>
        <asp:MenuItem NavigateUrl="#" Text="角色管理"></asp:MenuItem>
    </Items>
</asp:Menu>