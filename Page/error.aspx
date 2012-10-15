<%@ Page Language="C#" AutoEventWireup="true" Inherits="GHY.EF.Page.error" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Server Error</title>
</head>
<body>
    <form runat="server">
    <div>
        Error Message:<% =errorMessage %>
    </div>
    </form>
</body>
</html>
