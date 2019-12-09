<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EstadisticasTV.aspx.cs" Inherits="Internet_UI.Paginas.EstadisticasTV" %>

<%@ Register Src="~/Controles/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>#CBA147 - Estadísticas</title>

    <link rel="shortcut icon" type="image/png" href="<%=ResolveUrl("~/Resources/Imagenes/Icons/apple-icon-144x144.png") %>">
    <link rel="manifest" href="/manifest.json">

    <!-- Materialize -->
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Libs/Styles/materialize.min.css") %>" />

    <!-- Utils -->
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Utils/Styles/utils.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Utils/Styles/utils_dialogo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Utils/Styles/utils_datatable.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" />

    <!-- Mi Estilo -->
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Styles/MasterPageBase.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Styles/MasterPage.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveUrl("~/Paginas/Styles/EstadisticasTV.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" />

    <!--JQuery-->
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/jquery-3.0.0.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/jquery.validate.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/jquery.validate.aditional.min.js") %>"></script>

    <!-- Materialize -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/materialize.min.js") %>"></script>

    <!-- Url -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/purl.js") %>"></script>

    <!--Datatables -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/jquery.dataTables.min.js") %>"></script>

    <!-- JAlert -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/jAlert.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/jAlert-functions.min.js") %>"></script>

    <!-- Moments -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Libs/Scripts/moment.js") %>"></script>

    <!-- Utils -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Utils/Scripts/utils.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Utils/Scripts/utils_datatable.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Utils/Scripts/utils_dialogo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.charts.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.theme.fint.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.widgets.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/fusioncharts.powercharts.js") %>"></script>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/MasterPageBase.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/MasterPage.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/EstadisticasTV.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</head>
<body>
    <!-- ScriptManager -->
    <form id="form2" runat="server" class="flex direction-vertical full-height no-scroll">
        <asp:ScriptManager runat="server" EnablePartialRendering="True" EnablePageMethods="true" ID="scriptManager" />
    </form>

    <script type="text/javascript">
        var baseUrl = "<%= ResolveUrl("~/") %>";
        var DEBUG = true;
    </script>

    <!-- Login -->
    <div id="login" class="visible">
        <div class="fondo"></div>
        <div class="contenido">
            <form id="formLogin">
                <label id="texto_IniciarSesion" class="titulo-login">Iniciar sesión</label>

                <div class="input">
                    <i class="material-icons">person</i>
                    <div class="input-field">
                        <input id="input_Username" type="text" placeholder="Nombre de usuario" name="username" required="" aria-required="true" />
                    </div>
                </div>

                <div class="input">
                    <i class="material-icons">vpn_key</i>
                    <div class="input-field">
                        <input id="input_Password" type="password" placeholder="Contraseña" name="password" required="" aria-required="true" />
                    </div>
                </div>

                <a id="btnLogin" class="btn waves-effect">Acceder</a>
            </form>
        </div>
    </div>

    <!-- Pagina -->
    <div id="contenido">
        <div id="encabezado">
            <div class="logo">
                <div class="logo-muni no-select"></div>
                <label class="no-select nombre-muni">
                    Municipalidad
                    <br>
                    de Córdoba
                </label>
                <label class="no-select nombre-sistema">#CBA147</label>
                <div class="periodos">
                    <label data-periodo="1" class="seleccionado">30 dias</label>
                    <label data-periodo="2">90 dias</label>
                    <label data-periodo="3">180 dias</label>
                </div>
                <a id="btnActualizar" class="btn-flat btn-redondo waves-effect"><i class="material-icons">refresh</i></a>
            </div>

            <div class="usuario">
                <div id="imagenUser" runat="server" class="imagen"></div>
                <div class="textos">
                    <label id="textoUser" runat="server" class="nombre"></label>
                    <label id="textoNivel" runat="server" class="nivel"></label>
                </div>
            </div>
        </div>

        <div id="estadisticas">
            <div class="contenedor-mapa">
                <div class="mapa">
                    <Controles:Mapa runat="server" ID="mapa" />
                </div>

                <!-- Cargando -->
                <div id="cargando_mapa" class="cargando" style="display: none">
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
            </div>

            <div class="paneles">
                <div class="row" id="primeraFila">
                    <div class="col s6 panel" id="atencion">
                        <div class="textos">
                            <label class="titulo">Atención</label>
                            <div class="contadores">
                                <div class="contador">
                                    <div class="titulo">Porcentaje</div>
                                    <div id="textoAtencionPorcentaje" class="valor">0%</div>
                                </div>
                                <div class="contador">
                                    <div class="titulo">Requerimientos</div>
                                    <div id="textoAtencionTotal" class="valor">0</div>
                                </div>
                            </div>
                        </div>
                        <div class="contenedor-grafico">
                            <div id="atencion-grafico" class="chart"></div>
                        </div>
                    </div>

                    <div class="col s6 panel" id="rankingMotivo">
                        <div class="textos">
                            <label id="tituloRankingMotivo" class="titulo click">Ranking de motivos</label>
                        </div>
                        <div class="contenedor tabla">
                            <div class="contenedor-main">
                                <div class="tabla-contenedor flex-main flex direction-vertical">
                                    <table id="tablaRankingMotivos"></table>
                                </div>
                            </div>
                            <div class="contenedor-footer">
                                <a class="anterior btn no-select waves-effect">Anterior</a>
                                <a class="siguiente btn no-select waves-effect">Siguiente</a>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" id="segundaFila">
                    <div class="col s6 panel" id="tiempoResolucion">
                        <div class="textos">
                            <label class="titulo">Tiempo de resolucion</label>
                            <div class="contadores">
                                <div class="contador">
                                    <div id="textoPromedioAtencion" class="valor">0 dias</div>
                                </div>
                            </div>
                        </div>
                        <div class="contenedor-grafico">
                            <div id="tiempoResolucion-grafico" class="chart"></div>
                        </div>
                    </div>

                    <div class="col s6 panel" id="areasOperativas">
                        <div class="textos">
                            <label id="tituloAreasOperativas" class="titulo click">Areas operativas</label>
                        </div>
                        <div class="contenedor tabla">
                            <div class="contenedor-main">
                                <div class="tabla-contenedor flex-main flex direction-vertical">
                                    <table id="tablaAreasOperativas"></table>
                                </div>
                            </div>
                            <div class="contenedor-footer">
                                <a class="anterior btn no-select waves-effect">Anterior</a>
                                <a class="siguiente btn no-select waves-effect">Siguiente</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Mensaje -->
    <div id="mensaje">
        <div>
            <i class="material-icons"></i>
            <div>
                <label class="texto"></label>
                <a id="btnMensaje" class="btn waves-effect">Aceptar</a>
            </div>
        </div>
    </div>

    <!-- Cargando -->
    <div id="indicador-cargando" class="cargando" style="display: none;">
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
        <label>Cargando...</label>
    </div>
</body>
</html>
