<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_Pagina.Master" AutoEventWireup="true" CodeBehind="MisRequerimientos.aspx.cs" Inherits="Internet_UI.Paginas.MisRequerimientos" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/MisRequerimientos.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/MisRequerimientos.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <label id="texto_Titulo">Mis requerimientos</label>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="contenedor_Filtros" class="row" style="display: none">
        <div class="filtros_Estados col s8" style="display: flex"></div>
        <div class="filtros_Numero col s4" style="display: flex">
            <div class="input-field">
                <i class="material-icons prefix">search</i>
                <input id="inputBusqueda" type="text" />
                <label for="inputBusqueda" class="no-select">Buscar por número</label>
            </div>
        </div>
    </div>

    <div id="content_SinRequerimientos" class="row" >
        <img src="<%=ResolveUrl("~/Resources/Imagenes/sin-requerimientos.png")%>" />

        <label>No posee ningún requerimiento para mostrar...</label>

        <a class="btn waves-effect">
            <label>Agregar requerimiento</label>
        </a>
    </div>

    <div id="content_SinRequerimientosFiltrados" class="row" style="    display: none;"> 
        <label>No hay requerimientos que cumplan con los filtros...</label>
    </div>

    <%--    <div id="content_SinRequerimientosFiltrados" style="display:none">
        <img src="<%=ResolveUrl("~/Resources/Imagenes/sin-requerimientos.png")%>" />

        <label>No hay ningún requerimiento que cumpla con los filtros...</label>

       <%-- <a class="btn waves-effect">
            <label>Agregar requerimiento</label>
        </a>
    </div>--%>

    <div id="contenedor_Requerimientos" class="row">
    </div>

    <a id="fab" class="tooltipped btn-floating btn-large waves-effect waves-light" data-tooltip="Nuevo requerimiento" data-position="left">
        <i class="material-icons">add</i>
    </a>

    <%-- Templantes --%>

    <div id="template_Estado" style="display: none">
        <div class="contenedor_estado">
            <div class="estado">
                <i class="indicador mdi mdi-circle"></i>
                <label class="texto">NUEVO</label>
            </div>
        </div>
    </div>

    <div id="template_Requerimiento" style="display: none">
        <div class="col s12 m6 l4">
            <div class="requerimiento">
                <div class="contenedor_Encabezado">
                    <div class="contenedor_Numero">
                        <label class="numero"></label>
                        <label class="fecha"></label>
                    </div>

                    <div class="contenedor_Estado">
                        <i class="indicador mdi mdi-circle"></i>
                        <label class="nombre"></label>
                    </div>
                </div>

                <div class="separador">
                </div>

                <div class="contenedor_Contenido">
                    <div class="contenedor_Servicio contenedor_Detalle">
                        <label class="titulo">Servicio</label>
                        <label class="nombre"></label>
                    </div>

                    <div class="contenedor_Motivo contenedor_Detalle">
                        <label class="titulo">Motivo</label>
                        <label class="nombre"></label>
                    </div>

                    <div class="contenedor_Cpc contenedor_Detalle">
                        <label class="titulo">Cpc</label>
                        <label class="nombre"></label>
                    </div>

                    <div class="contenedor_Barrio contenedor_Detalle">
                        <label class="titulo">Barrio</label>
                        <label class="nombre"></label>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
