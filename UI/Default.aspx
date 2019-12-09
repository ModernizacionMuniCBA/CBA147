<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UI.Default" %>

<!DOCTYPE html>
<%@ Register Src="Controls/Navigation/Header.ascx" TagName="Header" TagPrefix="Controles" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <!-- Materialize -->
    <link type="text/css" rel="stylesheet" href="Styles/Libs/materialize.min.css" media="screen,projection" />

    <!-- JAlert -->
    <link type="text/css" rel="stylesheet" href="Styles/Libs/jAlert.css" media="screen,projection" />

    <!-- Tooltip -->
    <link type="text/css" rel="stylesheet" href="Styles/Libs/jquery.qtip.min.css" />

    <!-- Master Page -->
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Styles/_MasterPageSinLogin.css?v=" + ConfigurationManager.AppSettings["VERSION"])%>" media="screen,projection" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Styles/Default.css?v=" + ConfigurationManager.AppSettings["VERSION"])%>" media="screen,projection" />

    <!-- DataTables -->
    <link type="text/css" rel="stylesheet" href="Styles/Libs/jquery.dataTables.min.css" media="screen,projection" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/buttons/1.5.1/css/buttons.dataTables.min.css" />

    <!--Select2 -->
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Styles/Libs/select2.min.css") %>" media="screen,projection" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Styles/Libs/select2.materialize.css") %>" media="screen,projection" />

    <!-- Dialogo -->
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Styles/Utils/utils_dialogo.css?v=" + ConfigurationManager.AppSettings["VERSION"])%>" media="screen,projection" />

    <!-- Let browser know website is optimized for mobile -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link rel="apple-touch-icon" sizes="57x57" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-57x57.png") %>" />
    <link rel="apple-touch-icon" sizes="60x60" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-60x60.png") %>" />
    <link rel="apple-touch-icon" sizes="72x72" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-72x72.png") %>" />
    <link rel="apple-touch-icon" sizes="76x76" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-76x76.png") %>" />
    <link rel="apple-touch-icon" sizes="114x114" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-114x114.png") %>" />
    <link rel="apple-touch-icon" sizes="120x120" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-120x120.png") %>" />
    <link rel="apple-touch-icon" sizes="144x144" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-144x144.png") %>" />
    <link rel="apple-touch-icon" sizes="152x152" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-152x152.png") %>" />
    <link rel="apple-touch-icon" sizes="180x180" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-180x180.png") %>" />
    <link rel="icon" type="image/png" sizes="192x192" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-192x192.png") %>" />
    <link rel="icon" type="image/png" sizes="32x32" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-32x32.png") %>" />
    <link rel="icon" type="image/png" sizes="96x96" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-96x96.png") %>" />
    <link rel="icon" type="image/png" sizes="16x16" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/favicon-16x16.png") %>" />
    <link rel="manifest" href="/manifest.json" />
    <meta name="msapplication-TileColor" content="#ffffff" />
    <meta name="msapplication-TileImage" content="<%=ResolveUrl("~/Resources/Imagenes/Icons//ms-icon-144x144.png") %>" />
    <meta name="theme-color" content="#ffffff" />
</head>
<body>
    <script type="text/javascript">
        var urlCordobaGeoApi = "<%= ConfigurationManager.AppSettings["URL_CORDOBA_GEO_API"] %>";
        var urlCordobaFiles = "<%= ConfigurationManager.AppSettings["URL_SERVER_ARCHIVO"] %>";
        var identificadorFotoUserMale = "<%= ConfigurationManager.AppSettings["IDENTIFICADOR_FOTO_USER_MALE"] %>";
        var identificadorFotoUserFemale = "<%= ConfigurationManager.AppSettings["IDENTIFICADOR_FOTO_USER_FEMALE"] %>";
        var baseUrl = "<%= ResolveUrl("~/") %>";
        const KEY_GOOGLE_MAPS = "<%= ConfigurationManager.AppSettings["KEY_GOOGLE_MAPS"] %>";
    </script>

    <!--JQuery-->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery-3.0.0.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery.validate.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery.validate.aditional.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/purl.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/underscore-min.js") %>"></script>

    <!-- Materialize -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/materialize.min.js") %>"></script>

    <!-- Mask -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery.mask.js") %>"></script>

    <!-- Select2 -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/select2.min.js") %>"></script>

    <!-- JAlert -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jAlert.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jAlert-functions.min.js") %>"></script>

    <!-- Moments -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/moment.js") %>"></script>

    <!-- Tooltip -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/jquery.qtip.min.js") %>"></script>

    <!-- Dialogo -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Utils/utils_dialogo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

    <form if="form1" runat="server">
        <!-- ScriptManager -->
        <asp:ScriptManager runat="server" EnablePartialRendering="True" EnablePageMethods="true" ID="scriptManager" />

    </form>

    <div id="contenedor-principal" class="flex direction-vertical full-height no-scroll">


        <!-- Header -->
        <Controles:Header runat="server" ID="header" />



        <div id="main" class="flex-main flex direction-vertical scroll">
            <!-- Body -->
            <div id="contenedor_IndicadorCargando">
                <div class="preloader-wrapper big active">
                    <div class="spinner-layer">
                        <div class="circle-clipper left">
                            <div class="circle"></div>
                        </div>
                        <div class="gap-patch">
                            <div class="circle"></div>
                        </div>
                        <div class="circle-clipper right">
                            <div class="circle"></div>
                        </div>
                    </div>
                </div>
            </div>

            <iframe id="iframePagina" style="height: 100%; border: none;"></iframe>
        </div>

    </div>

    <!-- Overlay -->
    <div id="overlay" style="display: none;">
        <div class="preloader-wrapper big active">
            <div class="spinner-layer">
                <div class="circle-clipper left">
                    <div class="circle"></div>
                </div>
                <div class="gap-patch">
                    <div class="circle"></div>
                </div>
                <div class="circle-clipper right">
                    <div class="circle"></div>
                </div>
            </div>
        </div>
        <label class="no-select"></label>
    </div>

    <div id="errorCritico">
        <i class="material-icons"></i>
        <div class="textos">
            <label class="titulo"></label>
            <label class="detalle"></label>
        </div>

    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Utils/js.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/_MasterPageSinLogin.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Default.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

    <script type="text/javascript">
        initJs();
    </script>

</body>
</html>
