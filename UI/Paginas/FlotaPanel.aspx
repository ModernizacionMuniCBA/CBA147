<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="FlotaPanel.aspx.cs" Inherits="UI.FlotaPanel" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/FlotaPanel.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <!-- Card Filtros -->
    <div id="cardAcciones" class="contenedor">

        <!-- <div class="contenedor-main"> -->

            <div class="contenedor_Botones">
                <a id="btnNuevaFlota" class="btn waves-effect colorExito no-select">Nuevo</a>
                <a id="btnTerminarTurno" style="display:none" class="btn waves-effect waves-light no-select">Terminar turno</a>
            </div>

            <div class="contenedor_Filtros">
                <!-- Área -->
                <div id="contenedor-Area">
                    <div class="mi-input-field">
                        <label class="no-select">Área</label>
                        <select id="select_Area" style="width: 100%"></select>
                    </div>
                </div>

                <!-- Busqueda -->
                <div  id="contenedor-Busqueda">
                    <div class="input-field">
                        <i class="material-icons prefix">search</i>
                        <input id="inputBusqueda" type="text" />
                        <label for="inputBusqueda" class="no-select">Buscar...</label>
                    </div>
                </div>
            </div>

            <!-- Cargando -->
            <div class="cargando" style="display: none">
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
        <!-- </div> -->
    </div>


    <div id="content_SinFlotas" style="display: none">
        <label>No posee ninguna flota configurada en el área</label>
    </div>

    <!-- Grd -->
    <div id="contenedor_Flotas" class="contenedor">
    </div>

    <%-- Templantes --%>
    <div id="template_Flota" style="display: none">

        <div class="flota">
            <div class="contenedor_Encabezado">
                <div class="contenedor_Nombre">
                    <label class="nombre titulo link"></label>
                </div>

                <div class="contenedor_Estado">
                    <i class="indicador material-icons">swap_vertical_circle</i>
                    <label class="nombre"></label>
                </div>
            </div>

            <div class="contenedor_Informacion">
                <!-- <div class="contenedor_Acciones">
                </div> -->

                <div class="contenedor_Movil">
                    <label class="titulo">Móvil</label>
                    <label class="numero"></label>
                    <label class="tipo"></label>
                    <label class="marca"></label>
                    <label class="btnVerDetalle link">Ver detalle</label>
                </div>

                <div class="contenedor_Personal">
                    <label class="titulo">Personal</label>
                    <div class="contenido">
                    </div>
                </div>
            </div>
            <div class="separador">
            </div>

            <div class="contenedor_Trabajo">
                <div class="contenedor_Trabajo_Resumen">
                    <div class="texto">
                        <label class="trabajandoEn">Trabajando en </label>
                        <label class="OTNumero link"></label>
                    </div>
                    <a class="btn-flat btn-redondo btnVerMasTrabajo">

                        <i class=" material-icons">keyboard_arrow_down</i>
                        <div class="contenedor-cargando">
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
                    </a>
                </div>
                <div class="contenedor_Trabajo_MasInfo">
                    <div class="contenido">
                        <label id="texto_Fecha"></label>
                        <label id="texto_CantidadRQ"></label>
                        <label id="texto_Zonas"></label>
                        <label id="texto_Moviles"></label>
                        <label id="btn_VerDetalle" class="link no-select">Ver detalle</label>
                    </div>
                </div>
            </div>
        </div>

        <div class="contenedor_flota_cargando" style="display: none">
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

    <div id="template_Accion" style="display: none">
        <div class="accion">
            <i class="material-icons icono"></i>
            <label class="texto"></label>
        </div>
    </div>

    <div id="template_Empleado" style="display: none">
        <div class="empleado">
            <div class="persona">
                <div class="foto"></div>
                <label class="link"></label>
                <label class="observaciones"></label>
            </div>
        </div>
    </div>

    <div id="template_MasEmpleados" style="display: none">
        <div class="empleado">
            <div class="persona">
                <div class="foto"></div>
                <label class="observaciones">...y 100 más</label>
            </div>
        </div>
    </div>
    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/FlotaPanel.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
