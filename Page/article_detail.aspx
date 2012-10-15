<%@ Page Title="文章内容页Demo" Language="C#" MasterPageFile="~/GHY.Master" AutoEventWireup="true" Inherits="GHY.EF.Article.ArticleDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">
    <asp:HiddenField ID="connectionStringName" runat="server" Value="EFConnectionString" />

    <h1><% =info.Title %></h1>
    <p><% =info.Content %></p>
    <p>作者：<% =info.AuthorName %></p>
    <p>责任编辑：<% =info.AuditName %></p>
    <p>来源：<% =info.Source %></p>
    <p>创建时间：<% =info.CreationDate %></p>
    <p>更新时间：<% =info.UpdateDate %></p>

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