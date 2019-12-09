<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_MasterPage.Master" AutoEventWireup="true" CodeBehind="RequerimientoCargaMasiva.aspx.cs" Inherits="UI.Paginas.RequerimientoCargaMasiva" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/RequerimientoCargaMasiva.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
</asp:Content>

<%@ Register Src="~/Controls/SelectorMotivo.ascx" TagName="SelectorMotivo" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorDomicilio.ascx" TagName="SelectorDomicilio" TagPrefix="Controles" %>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div id="cardAlerta" class="card contenedor ">
        <div class="contenedor-header">
            <i class="material-icons">check_circle</i>
            <label class="titulo">Requerimiento registrado</label>
            <a id="btn_CerrarAlerta" class="btn-flat btn-redondo waves-effect"><i class="material-icons">clear</i></a>
        </div>
        <div class="contenedor-main">
            <label class="numero link">XXX123/2017</label>
            <div class="contenedor_Mail">
                <i class="material-icons">mail</i>
                <label class="mail"></label>
            </div>
        </div>
        <div class="contenedor-footer separador">
            <div class="contenedor-botones">
                <a id="btn_ReenviarComprobante" class="btn btn-cuadrado"><i class="material-icons">mail</i></a>
                <a id="btn_Imprimir" class="btn btn-cuadrado"><i class="material-icons">print</i></a>
            </div>
        </div>
    </div>
    <div id="cardFormulario" class="card contenedor">
        <%--Content--%>
        <div class="contenedor-main padding">
            <form id="formRequerimiento">

                <!-- Seccion Area -->
                <div class="row">
                    <div class="col s12 m3 l3 col-seccion">
                        <div class="header">
                            <label class="titulo no-select">Área</label>
                            <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        </div>
                        <div class="contenedor-mantener">
                            <input type="checkbox" id="check_MantenerArea" checked />
                            <label for="check_MantenerArea">Mantener</label>
                        </div>

                        <div style="display: none">
                            1- Seleccione su método de ingreso 'Rapido' o 'Manual'.<br />
                            2- Tipifique el motivo.<br />
                            3- Describa libremente el motivo
                        </div>
                    </div>
                    <div class="col s12 m9 l9">
                        <div class="row">
                            <div class="col s12 m6 l3 mi-input-field">
                                <select id="selectFormulario_Area" style="width: 100%" name="select_Area"></select>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-separador"></div>

                <!-- Seccion Motivo -->
                <div class="row">
                    <div class="col s12 m3 l3 col-seccion">
                        <div class="header">
                            <label class="titulo no-select">Motivo y Servicio</label>
                            <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        </div>
                        <div class="contenedor-mantener">
                            <input type="checkbox" id="check_MantenerMotivo" checked />
                            <label for="check_MantenerMotivo">Mantener</label>
                        </div>

                        <div style="display: none">
                            1- Seleccione su método de ingreso 'Rapido' o 'Manual'.<br />
                            2- Tipifique el motivo.<br />
                            3- Describa libremente el motivo
                        </div>
                    </div>
                    <div class="col s12 m9 l9">
                        <div class="row">
                            <div class="col s12 no-margin no-padding">
                                <Controles:SelectorMotivo runat="server" />
                                <label id="errorFormulario_Motivo" class="control-observacion colorTextoError no-select" style="display: none;"></label>
                            </div>
                        </div>

                        <div class="row margin-top" id="contenedorDescripcionMotivo">
                            <!-- Descripcion -->
                            <div class="col s12">
                                <div class="input-field">
                                    <textarea id="inputFormulario_Descripcion" class="materialize-textarea" name="input_Descripcion"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-separador"></div>

                <!-- Seccion Ubicacion -->
                <div class="row">
                    <div class="col s12 m3 l3 col-seccion">
                        <div class="header">
                            <label class="titulo no-select">Ubicación</label>
                            <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        </div>
                        <div class="contenedor-mantener">
                            <input type="checkbox" id="check_MantenerUbicacion" />
                            <label for="check_MantenerUbicacion">Mantener</label>
                        </div>

                        <div style="display: none">
                            1- Use calle y número si tiene datos precisos.<br />
                            2- Use barrio si lo conoce y tiene algún dato de la dirección.<br />
                            3- Use otro cuando no tenga precisión de calle y barrio.
                        </div>
                    </div>
                    <div class="col s12 m9 l9">
                        <div class="row">
                            <div class="col s12 no-margin no-padding">
                                <div id="contenedor_DomicilioSeleccionado" style="display: none">
                                    <div class="mapa">
                                    </div>
                                    <div class="contenido">
                                        <label id="texto_DomicilioTitulo"></label>
                                        <label id="texto_DomicilioDescripcion"></label>
                                        <label id="texto_DomicilioBarrio"></label>
                                        <label id="texto_DomicilioCpc"></label>
                                        <a id="btn_CancelarDomicilio" class="btn waves-effect"><i class="btn-icono material-icons">clear</i>Cancelar</a>
                                    </div>

                                </div>
                                <div id="contenedor_DomicilioNoSeleccionado">
                                    <label>No selecciono ninguna ubicación</label>
                                    <a id="btn_SeleccionarDomicilio" class="btn waves-effect colorExito"><i class="btn-icono material-icons">location_on</i>Definir ubicación</a>
                                </div>
                                <label id="errorFormulario_Domicilio" class="control-observacion colorTextoError no-select" style="display: none;"></label>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-separador"></div>

                <!-- Seccion Estado -->
                <div class="row ">
                    <div class="col s12 m3 col-seccion">
                        <div class="header">
                            <label class="titulo no-select">Estado</label>
                            <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        </div>
                        <div class="contenedor-mantener">
                            <input type="checkbox" id="check_MantenerEstado" />
                            <label for="check_MantenerEstado">Mantener</label>
                        </div>
                        <div style="display: none">
                        </div>
                    </div>

                    <div class="col s12 m9">
                        <div class="row">
                            <div class="col s12 m6 l3 mi-input-field">
                                <select id="selectFormulario_Estado" style="width: 100%" name="select_Estado"></select>
                            </div>
                        </div>
                        <div class="row" id="contenedorFormulario_EstadoDescripcion">
                            <div class="col s12 m6 l6 input-field">
                                <textarea id="inputFormulario_EstadoDescripcion" class="materialize-textarea" name="input_DescripcionEstado"></textarea>
                                <label for="inputFormulario_EstadoDescripcion" class=" no-select textarea">Motivo del cambio de estado</label>
                            </div>

                        </div>
                    </div>
                </div>

            </form>

        </div>

        <!-- Footer -->
        <div class="contenedor-footer separador">
            <div class="contenedor-botones  row">
                <a id="btn_Limpiar" class="btn waves-effect waves-light"><i class="material-icons btn-icono">clear</i>Limpiar</a>
                <a id="btn_Registrar" class="btn waves-effect waves-light colorExito"><i class="material-icons btn-icono">save</i>Registrar</a>
            </div>
        </div>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/RequerimientoCargaMasiva.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>


</asp:Content>
