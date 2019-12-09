<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="RequerimientoEmergenciaNueva.aspx.cs" Inherits="UI.Paginas.RequerimientoEmergenciaNueva" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoEmergenciaNueva.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<%@ Register Src="~/Controls/ControlDomicilioSelector.ascx" TagName="ControlDomicilioSelector" TagPrefix="Controles" %>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="contenedor-alertas">

        <!-- Alerta Ok -->
        <div id="alertaOk" class="container" style="display: none;">
            <div class="contenedor-alerta card">
                <i class="icono material-icons colorExitoTexto no-select">check_circle</i>
                <div class="textos">
                    <label class="titulo no-select">La operación se completo correctamente</label>
                    <div id="contenedorNumeroReclamo" class="flex direction-horizontal center-align">
                        <label class="titulo">Requerimiento N°</label>
                        <label id="textoNumeroReclamo" class="subtitulo"></label>
                    </div>
                </div>
                <div class="contenedor-botones">
                    <a class="btn waves-effect waves-light no-select" id="btnImprimirRequerimiento"><i class="btn-icono material-icons">print</i>Imprimir</a>
                    <a class="btn waves-effect waves-light no-select colorExito" id="btnNuevoRequerimiento"><i class="btn-icono material-icons">add</i>Nuevo</a>
                </div>
            </div>
        </div>
    </div>


    <div id="cardFormulario" class="card contenedor">

        <%--Content--%>
        <div class="contenedor-main" style="display: flex;">
            <div style="flex: 1" id="contenedor-formulario">
                <!-- Seccion Referente -->
                <div class="row">
                    <div class="seccionNombre">
                        <label class="titulo no-select">Referente</label>
                        <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        <div style="display: none">
                            - Ingrese los datos de la persona que se está comunicando.<br />
                            - El DNI y el género son datos obligatorios.<br />
                        </div>
                    </div>

                    <div id="contenedor_ReferenteProvisorio" class="visible">
                        <div class="row">
                            <!-- Telefono -->
                            <div class="col s12 m6">
                                <div class="input-field ">
                                    <input id="input_referenteTelefono" type="text" name="telefono" required="" aria-required="true" />
                                    <label for="input_referenteTelefono" class="no-select">Teléfono</label>
                                    <!-- <a id="errorFormulario_Modelo" class="control-observacion colorTextoError no-select"></a> -->
                                </div>
                            </div>


                            <!-- DNI -->
                            <div class="col s12 m4">
                                <div class="input-field ">
                                    <input id="input_referenteDni" name="DNI" type="number" required="" min="1" max="99999999" length="8" aria-required="true" />
                                    <label for="input_referenteDni" class="no-select">DNI</label>
                                    <a id="errorFormulario_Dni" class="control-observacion colorTextoError no-select"></a>
                                </div>
                            </div>

                            <div class="col s12 m1">
                                <a id="btn_BuscarUsuario" class="tooltipped btn btn-cuadrado " data-position="bottom" data-tooltip="Buscar"><i class="material-icons">search</i></a>
                            </div>
                        </div>

                        <div class="row" id="contenedor_DatosAdicionales">
                            <!-- Nombre -->
                            <div class="col s12 m4">
                                <div class="input-field ">
                                    <input id="input_referenteNombre" type="text" name="nombre" required="" aria-required="true" />
                                    <label for="input_referenteNombre" class="no-select">Nombre</label>
                                </div>
                            </div>

                            <!-- Apellido -->
                            <div class="col s12 m4">
                                <div class="input-field ">
                                    <input id="input_referenteApellido" type="text" name="apellido" required="" aria-required="true" />
                                    <label for="input_referenteApellido" class="no-select">Apellido</label>
                                    <!-- <a id="errorFormulario_Modelo" class="control-observacion colorTextoError no-select"></a> -->
                                </div>
                            </div>

                            <!--Género-->
                            <div class="col s12 m4">
                                <div class="mi-input-field">
                                    <label class="no-select">Género</label>
                                    <select id="select_referenteGenero" style="width: 100%"></select>
                                    <a id="errorFormulario_Genero" class="control-observacion colorTextoError no-select"></a>
                                </div>
                            </div>
                        </div>

                        <!-- Observaciones -->
                        <div class="row" id="contenedor_Observaciones">
                            <div class="col s12">
                                <div class="input-field ">
                                    <input id="input_referenteObservaciones" type="text" />
                                    <label for="input_referenteObservaciones" class="no-select">Observaciones</label>
                                    <!-- <a id="errorFormulario_Año" class="control-observacion colorTextoError no-select"></a> -->
                                </div>
                            </div>
                        </div>
                        <div style="display: flex; justify-content: center;">
                            <a id="btnDatosAdicionales" class="btn-flat btn-redondo waves-effect"><i class="material-icons">expand_more</i></a>
                        </div>
                    </div>

                    <div id="contenedor_UsuarioSeleccionado"></div>
                </div>

                <div class="form-separador"></div>

                <!-- Seccion Ubicacion -->
                <div class="row">
                    <div class="seccionNombre">
                        <label class="titulo no-select">Ubicación</label>
                        <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        <div style="display: none">
                            1- Use calle y número si tiene datos precisos.<br />
                            2- Use barrio si lo conoce y tiene algún dato de la dirección.<br />
                            3- Use otro cuando no tenga precisión de calle y barrio.
                        </div>
                    </div>

                    <Controles:ControlDomicilioSelector runat="server" />

                    <!-- Switch -->
                    <%--                        <div class="row" id="contenedorSwitchUbicacion">
                            <div class="flex direccion-horizontal right">
                                <div class="mi-input-field">
                                    <div class="switch mismo-color" id="Switch_Ubicacion">
                                        <div class="opcion opcion1">
                                            <i class="material-icons">search</i>
                                            <label>Edificios municipales</label>
                                        </div>

                                        <label>
                                            <input type="checkbox">
                                            <span class="lever"></span>
                                        </label>
                                        <div class="opcion opcion2">
                                            <i class="material-icons">touch_app</i>
                                            <label>Manual</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>--%>
                    <%--  <div class="row">
                            <div class="col s12 no-margin no-padding">
                                <div id="contenedor_DomicilioSeleccionado" style="display: none">
                                    <div class="contenido">
                                        <label id="texto_DomicilioTitulo"></label>
                                        <label id="texto_DomicilioDescripcion"></label>
                                        <label id="texto_DomicilioBarrio"></label>
                                        <label id="texto_DomicilioCpc"></label>
                                        <a id="btn_CancelarDomicilio" class="btn waves-effect"><i class="btn-icono material-icons">clear</i>Cancelar</a>
                                    </div>

                                </div>
                                <div id="contenedor_DomicilioNoSeleccionado_Manual" class="visible">
                                    <label>No selecciono ninguna ubicación</label>
                                    <a id="btn_SeleccionarDomicilio" class="btn waves-effect colorExito"><i class="btn-icono material-icons">location_on</i>Definir ubicación</a>
                                </div>--%>
                    <%--                  <div class="flex-main row" id="contenedor_DomicilioNoSeleccionado_Edificio">

                                    <!-- Categoria -->
                                    <div class="col s12 m6 l6">
                                        <div class="mi-input-field">
                                            <label class="no-select">Categoría</label>
                                            <select id="select_CategoriaEdificio" style="width: 100%">
                                            </select>
                                            <a class="control-observacion colorTextoError no-select"></a>
                                            <div class="input-error"><a></a></div>
                                        </div>
                                    </div>

                                    <!-- Edificio -->
                                    <div class="col s12 m6 l6">
                                        <div class="mi-input-field">
                                            <label class="no-select">Edificio Municipal</label>
                                            <select id="select_Edificio" style="width: 100%"></select>
                                            <div class="input-error"><a></a></div>
                                        </div>
                                    </div>
                                </div>--%>
                    <%--                            </div>
                        </div>--%>
                    <label id="errorFormulario_Domicilio" class="control-observacion colorTextoError no-select" style="display: none; margin-left: 12px !important;"></label>
                </div>

                <div class="form-separador"></div>

                <!--Seccion Motivo-->
                <div class="row">
                    <div class="seccionNombre">
                        <label class="titulo no-select">Motivo</label>
                        <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        <div style="display: none">
                            1- Seleccione la categoría del motivo según las especificaciones de la persona.<br />
                            2- Tipifique el motivo.<br />
                            3- Describa libremente el mismo
                        </div>
                    </div>

                    <div class="col s11 seccion-motivos">
                        <div class="row">
                            <div id="contenedor-filtrarMotivos" class="input-field ">
                                <input id="input_FiltrarMotivos" type="text" />
                                <label for="input_FiltrarMotivos" class="no-select">Filtrar motivos</label>
                            </div>
                            <a id="errorFormulario_Motivo" class="control-observacion colorTextoError  no-select" style="display: none"></a>
                            <div id="contenedor-motivos"></div>
                            <div id="contenedor-motivoSeleccionado"></div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="divisor"></div>
            <div id="contenedor-camposDinamicos">
            </div>
        </div>

        <!-- Footer -->
        <div class="contenedor-footer separador">
            <div class="contenedor-botones  row">
                <a id="btn_Limpiar" class="btn waves-effect waves-light"><i class="material-icons btn-icono">clear</i>Limpiar</a>
                <a id="btn_Registrar" class="btn waves-effect waves-light colorExito"><i class="material-icons btn-icono">save</i>Registrar</a>
            </div>
        </div>
    </div>

    <div>
    </div>

    <div id="template_Motivo" style="display: none">
        <div class="motivo waves-effect oculto">
            <label class="texto"></label>
            <a class="btn-flat chico waves-effect btn-redondo"><i class="material-icons">check</i></a>
        </div>
    </div>

    <div id="template_MotivoSeleccionado" style="display: none">
        <div class="motivoSeleccionado">
            <div class="flex">
                <label class="texto"></label>
                <a id="btnCancelarMotivoSeleccionado" class="btn-flat chico waves-effect btn-redondo"><i class="material-icons">clear</i></a>
            </div>
            <div class="input-field">
                <textarea id="inputFormulario_Descripcion" class="materialize-textarea"></textarea>
                <label for="inputFormulario_Descripcion" class=" no-select textarea">Detalle del motivo</label>
                <a id="errorFormulario_Descripcion" class="control-observacion colorTextoError  no-select" style="display: none"></a>
            </div>
        </div>
    </div>

    <div id="template_UsuarioSeleccionado" style="display: none">
        <div class="usuarioSeleccionado">
            <div class="flex">
                <div class="flex direction-vertical">
                    <label class="nombre"></label>
                    <label class="dni"></label>
                    <label class="telefono"></label>
                </div>
                <a id="btnCancelarUsuarioSeleccionado" class="btn-flat chico waves-effect btn-redondo"><i class="material-icons">clear</i></a>
            </div>
        </div>
    </div>

    <div id="template_Categoria" style="display: none">
        <a class="categoria btn waves-effect waves-light"></a>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientoEmergenciaNueva.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>


</asp:Content>
