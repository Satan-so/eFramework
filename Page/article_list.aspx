<%@ Page Title="文章列表页DEMO" Language="C#" MasterPageFile="~/GHY.Master" AutoEventWireup="true" Inherits="GHY.EF.Page.list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" src="/js/jq.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <asp:HiddenField ID="pageIndex" runat="server" Value="1" ClientIDMode="Static" />
    <asp:HiddenField ID="pageSize" runat="server" Value="20" ClientIDMode="Static" />
    <asp:HiddenField ID="infoCount" runat="server" ClientIDMode="Static" />

    <h1>文章列表页DEMO</h1>

    <EF:ArticleRepeater ID="listRepeater" runat="server" NodeId="1" PageSize="20" CacheTime="1" ConnectionStringName="EFConnectionString">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href='article_detail.aspx?infoid=<%# Eval("InfoId") %>' title='<%# Eval("Title")%>'><%# Eval("Title")%></a></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </EF:ArticleRepeater>

    <EF:Pager ID="pager" runat="server" />

    <h3>其它列表</h3>
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