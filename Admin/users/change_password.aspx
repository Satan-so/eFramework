<%@ Page Title="更改密码" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.users.change_password" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>更改密码</h2>
    <p>请使用以下表单更改密码。</p>
    <asp:ChangePassword ID="changeUserPassword" runat="server" CancelDestinationPageUrl="~/" RenderOuterTable="false" SuccessPageUrl="change_password_success.aspx">
        <ChangePasswordTemplate>
            <div class="account-info">
                <fieldset class="change-password">
                    <legend>帐户信息</legend>
                    <p>
                        <asp:Label ID="currentPasswordLabel" runat="server" AssociatedControlID="currentPassword">旧密码:</asp:Label>
                        <asp:TextBox ID="currentPassword" runat="server" CssClass="password-entry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="currentPasswordRequired" runat="server" ControlToValidate="currentPassword" 
                             CssClass="failure-notification" ErrorMessage="必须填写“密码”。" ToolTip="必须填写“旧密码”。" 
                             ValidationGroup="changeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="newPasswordLabel" runat="server" AssociatedControlID="newPassword">新密码:</asp:Label>
                        <asp:TextBox ID="newPassword" runat="server" CssClass="password-entry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="newPasswordRequired" runat="server" ControlToValidate="newPassword" 
                             CssClass="failure-notification" ErrorMessage="必须填写“新密码”。" ToolTip="必须填写“新密码”。" 
                             ValidationGroup="changeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="confirmNewPasswordLabel" runat="server" AssociatedControlID="confirmNewPassword">确认新密码:</asp:Label>
                        <asp:TextBox ID="confirmNewPassword" runat="server" CssClass="password-entry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="confirmNewPasswordRequired" runat="server" ControlToValidate="confirmNewPassword" 
                             CssClass="failure-notification" Display="Dynamic" ErrorMessage="必须填写“确认新密码”。"
                             ToolTip="必须填写“确认新密码”。" ValidationGroup="changeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="newPasswordCompare" runat="server" ControlToCompare="newPassword" ControlToValidate="confirmNewPassword" 
                             CssClass="failure-notification" Display="Dynamic" ErrorMessage="“确认新密码”与“新密码”项必须匹配。"
                             ValidationGroup="changeUserPasswordValidationGroup">*</asp:CompareValidator>
                    </p>
                    <span class="failure-notification">
                        <asp:Literal ID="failureText" runat="server"></asp:Literal>
                    </span>
                    <asp:ValidationSummary ID="changeUserPasswordValidationSummary" runat="server" CssClass="failure-notification" 
                         ValidationGroup="changeUserPasswordValidationGroup"/>
                </fieldset>
                <p class="submit-button">
                    <asp:Button ID="cancelPushButton" runat="server" CssClass="input-button" CausesValidation="False" CommandName="Cancel" Text="取消"/>
                    <asp:Button ID="changePasswordPushButton" runat="server" CssClass="input-button" CommandName="ChangePassword" Text="更改密码" 
                         ValidationGroup="changeUserPasswordValidationGroup"/>
                </p>
            </div>
        </ChangePasswordTemplate>
    </asp:ChangePassword>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
</asp:Content>