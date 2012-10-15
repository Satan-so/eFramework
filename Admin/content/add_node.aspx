<%@ Page Title="增加节点" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.content.add_node" %>
<%@ Register Src="~/content/content_sidebar.ascx" TagName="SideBar" TagPrefix="EF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>编辑节点信息</h2>
    <div class="node-info">
        <fieldset class="add-node">
            <legend>节点信息</legend>
            <p>
                <asp:Label ID="nodeNameLabel" runat="server" AssociatedControlID="nodeName">节点名:</asp:Label>
                <asp:TextBox ID="nodeName" runat="server" CssClass="text-entry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="nodeNameRequired" runat="server" ControlToValidate="nodeName" 
                        CssClass="failure-notification" ErrorMessage="必须填写“节点名”。" ToolTip="必须填写“节点名”。" 
                        ValidationGroup="addNodeValidationGroup">*</asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="imagePathLabel" runat="server" AssociatedControlID="imagePath">图片保存路径:</asp:Label>
                <asp:TextBox ID="imagePath" runat="server" CssClass="text-entry"></asp:TextBox>
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
            <span class="failureNotification">
                <asp:Literal ID="errorMessage" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="addNodeValidationSummary" runat="server" CssClass="failure-notification" 
                    ValidationGroup="addNodeValidationGroup"/>
        </fieldset>
        <p class="submit-button">
            <asp:Button ID="addNodeButton" runat="server" Text="添加节点" CssClass="input-button"
                ValidationGroup="addNodeValidationGroup" onclick="addNodeButton_Click"/>
        </p>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
    <EF:SideBar ID="sideBar" runat="server" />
</asp:Content>