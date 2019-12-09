<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="ServicioAreaMotivoConsulta.aspx.cs" Inherits="UI.ServicioAreaMotivoConsulta" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/ServicioAreaMotivoConsulta.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/MaterialDesign-Webfont/2.4.85/css/materialdesignicons.min.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="panel">
        <div class="card">
            <div id="contenedor_Servicios" class="panel">
                <div class="contenedor_Nombre">
                    <label>Servicios</label>
                    <a id="btn_ServiciosDestacados" class="waves-effect boton-filtro">Destacados</a>
                </div>
                <div class="contenedor_Busqueda">
                    <input type="text" placeholder="buscar" id="input_BuscarServicios" />
                    <i class="material-icons">search</i>
                </div>
                <div class="contenedor_Items"></div>
                <div class="contenedor_Botones">
                    <a id="btn_NuevoServicio" class="btn-flat waves-effect"><i class="material-icons btn-icono">add</i>Nuevo servicio</a>
                </div>
            </div>
            <div id="contenedor_Areas" class="panel">
                <div class="contenedor_Nombre">
                    <label>Areas</label>
                    <a class="waves-effect boton-filtro" style="opacity: 0; pointer-events: none">Destacados</a>
                </div>
                <div class="contenedor_Busqueda">
                    <input type="text" placeholder="buscar" id="input_BuscarAreas" />
                    <i class="material-icons">search</i>

                </div>
                <div class="contenedor_Items"></div>
            </div>
            <div id="contenedor_Motivos" class="panel">
                <div class="contenedor_Nombre">
                    <label>Motivos</label>
                    <a id="btn_MotivosDestacados" class="waves-effect boton-filtro">Destacados</a>
                    <a id="btn_MotivosUrgentes" class="waves-effect boton-filtro">Urgentes</a>
                    <a id="btn_MotivosInternos" class="waves-effect boton-filtro">Internos</a>
                </div>
                <div class="contenedor_Busqueda">
                    <input type="text" placeholder="buscar" id="input_BuscarMotivos" />
                    <i class="material-icons">search</i>

                </div>
                <div class="contenedor_Items"></div>
                <div class="contenedor_Botones">
                    <a id="btn_NuevoMotivo" class="btn-flat waves-effect"><i class="material-icons btn-icono">add</i>Nuevo motivo</a>
                </div>
            </div>
        </div>

    </div>


    <div id="template_Entity" style="display: none">
        <div class="entity waves-effect">
            <label class="nombre"></label>
            <div class="principal" style="display: none">
                <i class="mdi"></i>
            </div>

            <div class="principal2" style="display: none">
                <i class="mdi mdi-heart"></i>
            </div>
            <div class="urgente" style="display: none">
                <i class="mdi mdi-alert"></i>
            </div>
            <div class="interno" style="display: none">
                <i class="mdi mdi-home"></i>
            </div>
            <div class="prioridad" style="display: none">
                <i class="mdi mdi-flag"></i>
            </div>
            <a class="btn-flat btn-redondo chico waves-effect" style="display: none"><i class="material-icons">edit</i></a>
        </div>
    </div>



    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/underscore-min.js") %>"></script>

    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/ServicioAreaMotivoConsulta.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
