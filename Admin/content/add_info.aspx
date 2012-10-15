<%@ Page Title="添加信息" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" CodeBehind="add_info.aspx.cs" Inherits="GHY.EF.Admin.content.add_info" %>
<%@ Register Src="~/content/content_sidebar.ascx" TagName="SideBar" TagPrefix="EF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <h2>添加信息</h2>
    <p>请选择欲添加信息的节点</p>
    <asp:TreeView ID="nodeTreeView" runat="server" ExpandDepth="1" 
        ImageSet="Simple" ShowLines="True">
        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" 
            HorizontalPadding="0px" NodeSpacing="0px" VerticalPadding="0px" />
        <ParentNodeStyle Font-Bold="False" />
        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" 
            HorizontalPadding="0px" VerticalPadding="0px" />
    </asp:TreeView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sideBarContent" runat="server">
    <EF:SideBar ID="sideBar" runat="server" />
</asp:Content>
