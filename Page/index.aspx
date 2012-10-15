<%@ Page Title="主页Demo" Language="C#" MasterPageFile="~/GHY.Master" AutoEventWireup="true" Inherits="GHY.EF.Page.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <h1>首页DEMO</h1>
    <p>
        <a href="list.aspx?nodeid=1">列表页DEMO</a>
    </p>
    <p>
        <a href="article_list.aspx?nodeid=1">文章列表页DEMO</a>
    </p>
    <h3>Detail页DEMO：</h3>
    <EF:InfoRepeater ID="ghyInfoRepeater" runat="server" NodeId="1" PageSize="20" CacheTime="1" ConnectionStringName="EFConnectionString">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href='detail.aspx?infoid=<%# Eval("InfoId") %>' title='<%# Eval("Title")%>'><%# Eval("Title")%></a></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </EF:InfoRepeater>
</asp:Content>