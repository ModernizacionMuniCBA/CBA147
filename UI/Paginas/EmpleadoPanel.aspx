<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="EmpleadoPanel.aspx.cs" Inherits="UI.EmpleadoPanel" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/EmpleadoPanel.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <!-- Card Filtros -->
    <div id="cardFormularioFiltros" class="card contenedor">

        <div class="contenedor-main">

            <div class="row">
                <!-- Área -->
                <div class="col s12  m6 l3" id="contenedor-Area">
                    <div class="mi-input-field">
                        <label class="no-select">Área</label>
                        <select id="select_Area" style="width: 100%"></select>
                    </div>
                </div>

                <div class="col s12 m6 l3">
                    <div class="mi-input-field">
                        <label class="no-select">Estado</label>
                        <select id="select_Estado" style="width: 100%"></select>
                    </div>
                </div>

                <%--<div  class="col s12 m6 l2" id="contenedorCheckDeBaja">
                        <input class="with-gap" name="group1" type="checkbox" id="check_IncluirDeBaja" />
                        <label for="check_IncluirDeBaja">Incluir de baja</label>
                </div>--%>

                <div class="col s12 m6 l3">
                    <div class="mi-input-field">
                        <label class="no-select">Función</label>
                        <select id="select_Funcion" style="width: 100%"></select>
                    </div>
                </div>

                <!-- Busqueda -->
                <div class="col s12 m6 l3" id="contenedor-Busqueda">
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

        </div>
    </div>

    <div id="content_SinEmpleados" style="display: none">
        <label></label>

        <a class="btn waves-effect">
            <label>Gestionar empleados</label>
        </a>
    </div>

    <!-- Grd -->
    <div id="contenedor_Empleados" class="contenedor">
    </div>

    <%-- Templantes --%>
    <div id="template_Empleado" style="display: none">
        <div class="contenedor_empleado">
            <div class="empleado">
                <div class="contenedor_Encabezado ">
                    <div class="contenedor_Foto ">
                        <img class="foto" src=""></img>
                    </div>
                    <div class="contenedor_Informacion">
                        <div class="infoPrincipal">
                            <div class="contenedor_Nombre">
                                <label class="nombre link"></label>
                            </div>

                            <div class="contenedor_Estado">
                                <i class="indicador material-icons">swap_vertical_circle</i>
                                <label class="nombre"></label>
                            </div>
                        </div>
                        <div class="infoAdicional">
                            <div class="filaInfoAdicional primera">
                                <div class="contenedor_Dni">
                                    <label class="titulo">DNI</label>
                                    <label class="nombre"></label>
                                </div>
                                <div class=" contenedor_Cargo ">
                                    <label class="titulo">Cargo</label>
                                    <label class="nombre"></label>
                                </div>
                            </div>
                            <div class="linea" style="display: none">
                            </div>
                            <div class="filaInfoAdicional">
                                <div class="contenedor_Seccion">
                                    <label class="titulo">Sección</label>
                                    <label class="nombre"></label>
                                </div>
                                <div class=" contenedor_Funciones">
                                    <label class="titulo">Función</label>
                                    <label class="nombre"></label>
                                </div>
                            </div>
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
                            <label class="dias"></label>
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
            <div class="contenedor_empleado_cargando" style="display: none">
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
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/EmpleadoPanel.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
