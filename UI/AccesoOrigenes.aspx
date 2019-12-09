<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccesoOrigenes.aspx.cs" Inherits="UI.AccesoOrigenes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link type="text/css" rel="stylesheet" href="Styles/Libs/materialize.min.css" media="screen,projection" />
    <link href="<%=ResolveUrl("~/Styles/AccesoOrigenes.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />

    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery-3.0.0.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/materialize.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery.waitforimages.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/AccesoOrigenes.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</head>
<body>

    <script type="text/javascript">
        var baseUrl = "<%= ResolveUrl("~/") %>";
    </script>

    <form if="form1" runat="server">
        <asp:ScriptManager runat="server" EnablePartialRendering="True" EnablePageMethods="true" ID="scriptManager" />
    </form>

    <div id="fondo"></div>

    <div id="card">
        <div class="progress" id="indicador_Cargando">
            <div class="indeterminate"></div>
        </div>

        <label class="titulo">Seleccione un origen</label>
        <div id="contenedor_Origenes">
        </div>
    </div>

    <div id="template_Origen" style="display: none">
        <div class="origen waves-effect">
            <label class="nombre"></label>
        </div>
    </div>
</body>
</html>
