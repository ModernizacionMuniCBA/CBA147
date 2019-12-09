<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Acceso.aspx.cs" Inherits="UI.Acceso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="Styles/Libs/materialize.min.css" media="screen,projection" />
    <link href="<%=ResolveUrl("~/Styles/Acceso.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />

    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery-3.0.0.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/materialize.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Acceso.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</head>
<body>

    <script type="text/javascript">
        var baseUrl = "<%= ResolveUrl("~/") %>";
    </script>

    <iframe src="<%=ConfigurationManager.AppSettings["URL_LOGIN"] %>"></iframe>
</body>
</html>
