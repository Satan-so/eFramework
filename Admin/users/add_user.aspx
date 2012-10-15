<%@ Page Title="添加成员" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.users.add_user" %>
<%@ Register Src="~/users/users_sidebar.ascx" TagName="SideBar" TagPrefix="EF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>添加新成员</h2>
    <div class="account-info">
        <fieldset class="add-user">
            <legend>帐户信息</legend>
            <p>
                <asp:Label ID="userNameLabel" runat="server" AssociatedControlID="userName">用户名:</asp:Label>
                <asp:TextBox ID="userName" runat="server" CssClass="text-entry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="userNameRequired" runat="server" ControlToValidate="userName" 
                        CssClass="failure-notification" ErrorMessage="必须填写“用户名”。" ToolTip="必须填写“用户名”。" 
                        ValidationGroup="addUserValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="passwordLabel" runat="server" AssociatedControlID="password">密码:</asp:Label>
                <asp:TextBox ID="password" runat="server" CssClass="password-entry" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="passwordRequired" runat="server" ControlToValidate="password" 
                        CssClass="failure-notification" ErrorMessage="必须填写“密码”。" ToolTip="必须填写“密码”。" 
                        ValidationGroup="addUserValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="confirmPasswordLabel" runat="server" AssociatedControlID="confirmPassword">确认密码:</asp:Label>
                <asp:TextBox ID="confirmPassword" runat="server" CssClass="password-entry" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="confirmPasswordRequired" runat="server" ControlToValidate="confirmPassword" 
                        CssClass="failure-notification" Display="Dynamic" ErrorMessage="必须填写“确认密码”。"
                        ToolTip="必须填写“确认密码”。" ValidationGroup="addUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="passwordCompare" runat="server" ControlToCompare="password" ControlToValidate="confirmPassword" 
                        CssClass="failure-notification" Display="Dynamic" ErrorMessage="“确认密码”与“密码”项必须匹配。"
                        ValidationGroup="addUserPasswordValidationGroup">*</asp:CompareValidator>
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
                <asp:Label ID="commentLabel" runat="server" AssociatedControlID="comment">备注:</asp:Label>
                <asp:TextBox ID="comment" runat="server" CssClass="text-entry" TextMode="MultiLine"></asp:TextBox>
            </p>
            <span class="failureNotification">
                <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="addUserValidationSummary" runat="server" CssClass="failure-notification" 
                    ValidationGroup="addUserValidationGroup"/>
        </fieldset>
        <p class="submit-button">
            <asp:Button ID="addUserButton" runat="server" Text="添加成员" CssClass="input-button"
                ValidationGroup="addUserValidationGroup" onclick="addUserButton_Click"/>
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
    <EF:SideBar ID="sideBar" runat="server" />
</asp:Content>
