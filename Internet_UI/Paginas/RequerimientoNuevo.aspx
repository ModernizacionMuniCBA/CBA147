<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_Pagina.Master" AutoEventWireup="true" CodeBehind="RequerimientoNuevo.aspx.cs" Inherits="Internet_UI.Paginas.RequerimientoNuevo" %>

<%@ Register Src="~/Controles/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>


<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" type="text/javascript"></script>

    <script src="<%=ResolveUrl("~/Libs/Scripts/lottie_light.min.js") %>" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~/Libs/Scripts/lottie_api.minified.js") %>" type="text/javascript"></script>

    <script src=' https://underscorejs.org/underscore-min.js' type="text/javascript"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderBoton" runat="server">
    <a class="btn btn-quadrate btn-transparent btn-volver waves-effect"><i class="material-icons">arrow_back</i></a>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <label id="texto_Titulo">Nuevo requerimiento</label>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Paso 1 --%>
    <div class="contenedor-detalle">
        <div id="paso1" class="card card-detalle seleccionada">
            <div class="content-header">
                <div class="indicador">
                    <label>1</label>
                </div>
                <label>Motivo</label>

                <div class="preloader-wrapper small active card-cargando">
                    <div class="spinner-layer spinner-green-only">
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

                <a class="btn btn-round btn-transparent btn-serviciosMotivos waves-effect">
                    <i class="mdi mdi-magnify"></i>
                </a>
            </div>

            <div class="contenedor">
                <%-- Servicios --%>
                <div id="contenedor_SeleccionarServicio">
                    <div class="info card-info">
                        <i class="material-icons">info_outline</i>
                        <label></label>
                    </div>

                    <div class="content-body">
                        <div class="contenedor_ServiciosPrincipales">
                            <div class="row">
                            </div>
                        </div>

                        <a class="btn btn-servicios waves-effect">Ver más servicios</a>
                    </div>
                </div>

                <%-- Motivos --%>
                <div id="contenedor_SeleccionarMotivo" style="display: none">
                    <div class="info card-info">
                        <i class="material-icons">info_outline</i>
                        <label></label>
                    </div>

                    <div class="content-body">
                        <div class="contenedor_ServicioSeleccionado">
                            <div class="item-detalle">
                                <div class="textos">
                                    <label class="titulo">Servicio seleccionado</label>
                                    <label class="nombre"></label>
                                </div>
                            </div>

                            <a class="btn-cancelarServicio link waves-effect">
                                <i class="mdi mdi mdi-delete"></i>
                                <label>Cancelar servicio</label>
                            </a>
                        </div>

                        <label class="texto_SeleccioneMotivo" style="display: none"></label>
                        <div class="contenedor_MotivosPrincipales" style="display: none">
                            <div class="row">
                            </div>
                        </div>

                        <a class="btn btn-motivos waves-effect">Seleccionar motivo</a>
                    </div>
                </div>

                <%-- Descripcion --%>
                <div id="contenedor_InsertarDescripcion" style="display: none">
                    <div class="info card-info">
                        <i class="material-icons">info_outline</i>
                        <label></label>
                    </div>

                    <div class="content-body">
                        <div class="item-detalle contenedor_ServicioSeleccionado">
                            <div class="textos">
                                <label class="titulo">Servicio seleccionado</label>
                                <label class="nombre"></label>
                            </div>
                        </div>

                        <div class="item-detalle contenedor_MotivoSeleccionado">
                            <div class="textos">
                                <label class="titulo">Motivo seleccionado</label>
                                <label class="nombre"></label>
                            </div>
                        </div>

                        <a class="btn-cancelarMotivo link waves-effect">
                            <i class="mdi mdi mdi-delete"></i>
                            <label>Cancelar selección</label>
                        </a>
                    </div>

                    <div class="contenedor-descripcion">
                        <label>Descripción</label>
                        <textarea id="input_Descripcion" placeholder="Ingresa una descripción..." autofocus="autofocus"></textarea>
                    </div>

                    <div class="content-footer">
                        <a class="btn btn-gris waves-effect btn-siguiente">Siguiente</a>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <%-- Paso 2 --%>
    <div class="contenedor-detalle">
        <div id="paso2" class="card card-detalle">
            <div class="content-header">
                <div class="indicador">
                    <label>2</label>
                </div>
                <label>Ubicación</label>

                <div class="preloader-wrapper small active card-cargando">
                    <div class="spinner-layer spinner-green-only">
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

            <div class="contenedor">
                <div id="contenedor_UbicacionNoSeleccionada">
                    <div class="card-info">
                        <i class="mdi mdi-help-box"></i>
                        <label></label>
                    </div>

                    <div class="content-body">
                        <a class="btn btn-ubicacion waves-effect">Seleccionar ubicación</a>
                    </div>
                </div>

                <div id="contenedor_UbicacionSeleccionada" style="display: none">
                    <div class="mapa waves-effect"></div>

                    <div class="content-body">
                        <div class="item-detalle">
                            <div class="textos">
                                <label class="titulo">Direccion</label>
                                <label class="detalle _direccion"></label>
                                <a class="link link-ubicacion waves-effect">
                                    <i class="mdi mdi-pencil"></i>
                                    <label>Modificar</label>
                                </a>
                            </div>
                        </div>

                        <div class="item-detalle">
                            <div class="textos">
                                <label class="titulo">Observaciones del domicilio</label>
                                <label class="detalle _observaciones"></label>
                                <a class="link link-ubicacionObservaciones waves-effect">
                                    <i class="mdi mdi-pencil"></i>
                                    <label>Modificar</label>
                                </a>
                            </div>
                        </div>

                        <div class="item-detalle">
                            <div class="textos">
                                <label class="titulo">Barrio</label>
                                <label class="detalle _barrio"></label>
                            </div>
                        </div>


                        <div class="item-detalle">
                            <div class="textos">
                                <label class="titulo">CPC</label>
                                <label class="detalle _CPC"></label>
                            </div>
                        </div>

                        <a class="link waves-effect btn-cancelarUbicacion">
                            <i class="mdi mdi mdi-delete"></i>
                            <label>Cancelar ubicación</label>
                        </a>
                    </div>

                    <div class="content-footer">
                        <a class="btn waves-effect btn-siguiente">Siguiente</a>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <%-- Paso 3 --%>
    <div class="contenedor-detalle">
        <div id="paso3" class="card card-detalle">
            <div class="content-header">
                <div class="indicador">
                    <label>3</label>
                </div>
                <label>Imagen (opcional)</label>

                <div class="preloader-wrapper small active card-cargando">
                    <div class="spinner-layer spinner-green-only">
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

            <div class="contenedor">
                <div id="contenedor_ImagenNoAgregada">
                    <div class="card-info">
                        <i class="mdi mdi-help-box"></i>
                        <label></label>
                    </div>

                    <div class="content-body">
                        <input type="file" id="input_Imagen" accept="image/*" style="display: none" />
                        <a class="btn btn-flat btn-imagen waves-effect">Agregar imágen</a>
                    </div>
                </div>

                <div id="contenedor_ImagenAgregada" style="display: none">
                    <div class="content-body">
                        <div class="content-imagen"></div>

                        <a class="link waves-effect btn-cancelarImagen">
                            <i class="mdi mdi mdi-delete"></i>
                            <label>Cancelar imagen</label>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="contenedor-botones">
        <div>
            <a class="btn-volver btn btn-transparent waves-effect">Volver</a>
            <a class="btn-finalizar btn btn-gris waves-effect">Finalizar</a>
        </div>
    </div>

    <%-- Templantes --%>
    <div id="template_ServicioPrincipal" style="display: none">
        <div class="servicio-principal col s6 m4">
            <i class="waves-effect"></i>
            <label></label>
        </div>
    </div>

    <div id="template_MotivoPrincipal" style="display: none">
        <div class="motivo waves-effect">
            <label class="nombre"></label>
        </div>
    </div>

    <div id="template_DialogoServicios" style="display: none">
        <div class="contenedor-busqueda">
            <a class="btn btn-quadrate btn-transparent btn-close waves-effect"><i class="mdi mdi-arrow-left"></i></a>
            <input type="text" placeholder="Buscar servicio..." autofocus="autofocus" />
        </div>

        <div class="contenedor-resultados"></div>
    </div>

    <div id="template_DialogoMotivos" style="display: none">
        <div class="contenedor-busqueda">
            <a class="btn btn-quadrate btn-transparent btn-close waves-effect"><i class="mdi mdi-arrow-left"></i></a>
            <input type="text" placeholder="Buscar motivo..." autofocus="autofocus" />
        </div>

        <div class="contenedor-resultados"></div>
    </div>

    <div id="template_DialogoServiciosMotivos" style="display: none">
        <div class="contenedor-busqueda">
            <a class="btn btn-quadrate btn-transparent btn-close waves-effect"><i class="mdi mdi-arrow-left"></i></a>
            <input type="text" placeholder="Ingresa tu busqueda..." autofocus="autofocus" />
        </div>

        <div class="contenedor-resultados"></div>
    </div>

    <div id="template_ServicioDialogo" style="display: none">
        <div class="item waves-effect">
            <label class="nombre"></label>
        </div>
    </div>

    <div id="template_MotivoDialogo" style="display: none">
        <div class="item waves-effect">
            <label class="nombre"></label>
        </div>
    </div>

    <div id="template_ServicioMotivoDialogo" style="display: none">
        <div class="item waves-effect">
            <label class="motivoNombre"></label>
            <label class="servicioNombre"></label>
        </div>
    </div>

    <div id="template_DialogoUbicacion" style="display: none">
        <div class="contenedor-mapa">
        </div>
    </div>

    <div id="template_DialogoConfirmacion" style="display: none">
        <div class="item-detalle">
            <div class="textos">
                <label class="titulo">Servicio</label>
                <label class="_servicio"></label>
            </div>
        </div>

        <div class="item-detalle">
            <div class="textos">
                <label class="titulo">Motivo</label>
                <label class="_motivo"></label>
            </div>
        </div>

        <div class="item-detalle">
            <div class="textos">
                <label class="titulo">Descripcion</label>
                <label class="_descripcion"></label>
            </div>
        </div>

        <div class="item-detalle">
            <div class="textos">
                <label class="titulo">Ubicacion</label>
                <label class="_ubicacion"></label>
            </div>
        </div>
    </div>


    <%-- Controles --%>
    <div id="contenedor-mapa" style="display: none">
        <div>
            <Controles:Mapa runat="server" ID="mapa" />
        </div>
    </div>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="panelExito">
        <div id="lottie"></div>

        <div>
            <label class="titulo">Requerimiento registrado correctamente</label>
            <label class="numero"></label>
            <label class="email"></label>
            <a class="btn btn-verDetalle waves-effect">Ver detalle</a>
            <a class="btn btn-gris btn-volver waves-effect">Volver</a>
        </div>
    </div>
</asp:Content>
