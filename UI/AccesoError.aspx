<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccesoError.aspx.cs" Inherits="UI.AccesoError" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="Styles/Libs/materialize.min.css" media="screen,projection" />
    <link href="<%=ResolveUrl("~/Styles/AccesoError.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />

    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery-3.0.0.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/materialize.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery.waitforimages.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/AccesoError.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</head>
<body>

    <script type="text/javascript">
        var baseUrl = "<%= ResolveUrl("~/") %>";
    </script>

    <form if="form1" runat="server">
        <asp:ScriptManager runat="server" EnablePartialRendering="True" EnablePageMethods="true" ID="scriptManager" />
    </form>

    <div id="fondo"></div>

    <div id="contenedor_Error" >
        <label class="titulo"><b>Se ha producido un error al iniciar sesión.</b></label>
        <label class="detalle"></label>
        <a class="btn btn-flat">Volver</a>
    </div>

</body>
</html>
