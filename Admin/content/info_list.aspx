<%@ Page Title="内容列表" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.content.info_list" %>
<%@ Register Src="~/content/content_sidebar.ascx" TagName="SideBar" TagPrefix="EF" %>
<%@ Register Src="~/controls/pager.ascx" TagName="Pager" TagPrefix="EF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" src="/js/jq.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:HiddenField ID="pageIndex" runat="server" Value="1" ClientIDMode="Static" />
    <asp:HiddenField ID="pageSize" runat="server" Value="20" ClientIDMode="Static" />
    <asp:HiddenField ID="infoCount" runat="server" ClientIDMode="Static" />

    <asp:GridView ID="infosView" runat="server" AutoGenerateColumns="False"
        CellPadding="4" ForeColor="#000333" GridLines="None" Width="100%">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="标题" ReadOnly="True" 
                SortExpression="Title" />
            <asp:BoundField DataField="AuthorName" HeaderText="作者" ReadOnly="True" 
                SortExpression="AuthorName" />
            <asp:CheckBoxField DataField="IsTop" HeaderText="是否置顶" ReadOnly="True" />
            <asp:HyperLinkField DataNavigateUrlFields="InfoId" 
                DataNavigateUrlFormatString="info_list.aspx?infoid={0}" Text="编辑" />
        </Columns>
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>

    <EF:Pager ID="pager" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
    <EF:SideBar ID="sideBar" runat="server" />
</asp:Content>