<%@ Page Title="成员管理" Language="C#" MasterPageFile="~/AdminLayout.Master" AutoEventWireup="true" Inherits="GHY.EF.Admin.users.user_list" %>
<%@ Register Src="~/users/users_sidebar.ascx" TagName="SideBar" TagPrefix="EF" %>
<%@ Register Src="~/controls/pager.ascx" TagName="Pager" TagPrefix="EF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" src="/js/jq.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <asp:HiddenField ID="pageIndex" runat="server" Value="1" ClientIDMode="Static" />
    <asp:HiddenField ID="pageSize" runat="server" Value="20" ClientIDMode="Static" />
    <asp:HiddenField ID="infoCount" runat="server" ClientIDMode="Static" />

    <fieldset class="search">
        <legend>搜索成员</legend>
        <asp:Label ID="userNameLabel" runat="server" AssociatedControlID="userName">用户名或真名:</asp:Label>
        <asp:TextBox ID="userName" runat="server" CssClass="text-entry"></asp:TextBox>
        <input type="button" value="搜索" class="input-button" id="search-btn" />
        <script type="text/javascript">
            $('#search-btn').click(function () {
                $('#pageIndex')[0].value = 1;
                $('form')[0].submit();
            });
        </script>
    </fieldset>

    <asp:GridView ID="usersView" runat="server" AutoGenerateColumns="False"
        CellPadding="4" ForeColor="#000333" GridLines="None" Width="100%">
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="用户名" ReadOnly="True" 
                SortExpression="UserName" />
            <asp:BoundField DataField="RealName" HeaderText="真名" ReadOnly="True" 
                SortExpression="RealName" />
            <asp:BoundField DataField="TelNumber" HeaderText="电话" ReadOnly="True" 
                SortExpression="TelNumber" />
            <asp:BoundField DataField="Email" HeaderText="邮箱" ReadOnly="True" 
                SortExpression="Email" />
            <asp:CheckBoxField DataField="IsApproved" HeaderText="是否允许登录" ReadOnly="True" 
                SortExpression="IsApproved" />
            <asp:BoundField DataField="LastLoginDate" HeaderText="最近登录时间" 
                SortExpression="LastLoginDate" />
            <asp:HyperLinkField DataNavigateUrlFields="UserName" 
                DataNavigateUrlFormatString="update_user.aspx?username={0}" Text="编辑" />
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