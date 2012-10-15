<%@ Page Title="登录EF" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.users.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>Login</h2>
    <p>Please enter your username and password!</p>
    <asp:Login ID="loginUser" runat="server" RenderOuterTable="false">
        <LayoutTemplate>
            <div class="account-info">
                <fieldset class="login">
                    <legend>Account Info</legend>
                    <p>
                        <asp:Label ID="userNameLabel" runat="server" AssociatedControlID="userName">User Name:</asp:Label>
                        <asp:TextBox ID="userName" runat="server" CssClass="text-entry"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="userNameRequired" runat="server" ControlToValidate="userName" 
                             CssClass="failure-notification" ErrorMessage="必须填写“User Name”。" ToolTip="必须填写“User Name”。" 
                             ValidationGroup="loginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="passwordLabel" runat="server" AssociatedControlID="password">Password:</asp:Label>
                        <asp:TextBox ID="password" runat="server" CssClass="password-entry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="password" 
                             CssClass="failure-notification" ErrorMessage="必须填写“Password”。" ToolTip="必须填写“Password”。" 
                             ValidationGroup="loginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <span class="failure-notification">
                        <asp:Literal ID="failureText" runat="server"></asp:Literal>
                    </span>
                    <asp:ValidationSummary ID="loginUserValidationSummary" runat="server" CssClass="failure-notification" ValidationGroup="loginUserValidationGroup"/>
                </fieldset>
                <p class="submit-button">
                    <asp:Button ID="loginButton" runat="server" CssClass="input-button" CommandName="Login" Text="Login" ValidationGroup="loginUserValidationGroup"/>
                </p>
            </div>
        </LayoutTemplate>
    </asp:Login>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
</asp:Content>