<%@ Page Title="更新成员信息" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.users.update_user" %>
<%@ Register Src="~/users/users_sidebar.ascx" TagName="SideBar" TagPrefix="EF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>编辑成员信息</h2>
    <div class="account-info">
        <fieldset class="update-user">
            <legend>帐户信息</legend>
            <p>
                <asp:Label ID="userNameLabel" runat="server" AssociatedControlID="userName">用户名:</asp:Label>
                <asp:TextBox ID="userName" runat="server" CssClass="text-entry" ReadOnly="True"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="passwordLabel" runat="server" AssociatedControlID="password">密码（不修改请留空）:</asp:Label>
                <asp:TextBox ID="password" runat="server" CssClass="password-entry" TextMode="Password"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="realNameLabel" runat="server" AssociatedControlID="realName">真名:</asp:Label>
                <asp:TextBox ID="realName" runat="server" CssClass="text-entry"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="emailLabel" runat="server" AssociatedControlID="email">电子邮件:</asp:Label>
                <asp:TextBox ID="email" runat="server" CssClass="text-entry"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="telNumberLabel" runat="server" AssociatedControlID="telNumber">电话:</asp:Label>
                <asp:TextBox ID="telNumber" runat="server" CssClass="text-entry"></asp:TextBox>
            </p>
            <p>
                <asp:CheckBox ID="isApproved" runat="server" Text="是否允许登录" />
            </p>
            <p>
                <asp:Label ID="commentLabel" runat="server" AssociatedControlID="comment">备注:</asp:Label>
                <asp:TextBox ID="comment" runat="server" CssClass="text-entry" TextMode="MultiLine"></asp:TextBox>
            </p>
        </fieldset>
        <p class="submit-button">
            <asp:Button ID="updateUserButton" runat="server" Text="编辑成员" CssClass="input-button" onclick="updateUserButton_Click"/>
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
    <EF:SideBar ID="sideBar" runat="server" />
</asp:Content>
