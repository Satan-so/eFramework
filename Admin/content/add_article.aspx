<%@ Page Title="添加文章" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.content.add_article" validateRequest="false" %>
<%@ Register Src="~/content/content_sidebar.ascx" TagName="SideBar" TagPrefix="EF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" src="/js/jq.js"></script>
    <script type="text/javascript" src="/js/xheditor.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>添加文章</h2>
    <div class="article-info">
        <fieldset class="add-article">
            <legend>文章内容</legend>
            <p>
                <asp:Label ID="titleLabel" runat="server" AssociatedControlID="title">标题:</asp:Label>
                <asp:TextBox ID="title" runat="server" CssClass="text-entry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="titleRequired" runat="server" ControlToValidate="title" 
                        CssClass="failure-notification" ErrorMessage="必须填写“标题”。" ToolTip="必须填写“标题”。" 
                        ValidationGroup="addArticleValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="contentLabel" runat="server" AssociatedControlID="content">内容:</asp:Label>
                <asp:TextBox ID="content" runat="server" CssClass="text-entry" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                <script type="text/javascript">
                    $('#content').xheditor({skin : 'nostyle'});
                </script>
                <asp:RequiredFieldValidator ID="contentRequired" runat="server" ControlToValidate="content" 
                        CssClass="failure-notification" ErrorMessage="必须填写“内容”。" ToolTip="必须填写“内容”。" 
                        ValidationGroup="addArticleValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="sourceLabel" runat="server" AssociatedControlID="source">来源:</asp:Label>
                <asp:TextBox ID="source" runat="server" CssClass="text-entry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="sourceRequired" runat="server" ControlToValidate="source" 
                        CssClass="failure-notification" ErrorMessage="必须填写“来源”。" ToolTip="必须填写“来源”。" 
                        ValidationGroup="addArticleValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="imageLabel" runat="server" AssociatedControlID="image">封面图片:</asp:Label>
                <asp:TextBox ID="image" runat="server" CssClass="text-entry"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="linkLabel" runat="server" AssociatedControlID="link">跳转链接:</asp:Label>
                <asp:TextBox ID="link" runat="server" CssClass="text-entry"></asp:TextBox>
            </p>
            <span class="failureNotification">
                <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="addArticleValidationSummary" runat="server" CssClass="failure-notification" 
                    ValidationGroup="addArticleValidationGroup"/>
        </fieldset>
        <p class="submit-button">
            <asp:Button ID="addArticleButton" runat="server" Text="添加文章" CssClass="input-button"
                ValidationGroup="addArticleValidationGroup" 
                onclick="addArticleButton_Click"/>
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
    <EF:SideBar ID="sideBar" runat="server" />
</asp:Content>