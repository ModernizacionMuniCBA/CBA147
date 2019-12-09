<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoNuevo.aspx.cs" Inherits="UI.IFrame.IRequerimientoNuevo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"])%>" media="screen,projection" />
</asp:Content>
<%@ Register Src="~/Controls/SelectorMotivo.ascx" TagName="SelectorMotivo" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorUsuario.ascx" TagName="SelectorUsuario" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/SelectorDomicilio.ascx" TagName="SelectorDomicilio" TagPrefix="Controles" %>
<%@ Register Src="~/Controls/ControlDomicilioSelector.ascx" TagName="ControlDomicilioSelector" TagPrefix="Controles" %>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <!-- PDF -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/pdf.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/pdf.worker.js") %>"></script>

    <div class="flex direction-vertical full-height">
        <div class="contenedorTabs">
            <ul class="tabs">
                <li class="tab col s3" id="pestañaDetalle"><a class="active" href="#tabDetalle">Detalle</a></li>
                <li class="tab col s3" id="pestañaNotas"><a href="#tabNotas">Notas</a></li>
                <li class="tab col s3" id="pestañaDocumentos"><a href="#tabDocumentos">Documentos</a></li>
                <li class="tab col s3" id="pestañaImagenes"><a href="#tabImagenes">Imagenes</a></li>
            </ul>
        </div>

        <div id="tabDetalle" class="flex-main scroll padding">
            <!-- Seccion Identificacion -->
            <div class="row">

                <div class="col s12 m2 no-padding no-margin">
                    <div class="row">
                        <div class="col s12 flex">
                            <label class="titulo no-select" style="flex: 1;">Motivo y Servicio</label>
                            <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                            <div style="display: none">
                                1- Seleccione su método de ingreso 'Rapido' o 'Manual'.<br />
                                2- Tipifique el motivo.<br />
                                3- Describa libremente el motivo
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col s12 m10">
                    <div class="row">
                        <div class="col s12 no-margin no-padding">
                            <Controles:SelectorMotivo id="selectorMotivo" runat="server" />
                            <label id="errorFormulario_Motivo" class="control-observacion colorTextoError no-select" style="display: none; margin-left: 12px !important;"></label>
                        </div>
                    </div>

                    <div class="row margin-top" id="contenedorDescripcionMotivo" style="display: none">

                        <!-- Descripcion -->
                        <div class="col s12">
                            <div class="input-field">
                                <textarea id="inputFormulario_Descripcion" class="materialize-textarea"></textarea>
                                <label for="inputFormulario_Descripcion" class=" no-select textarea">Detalle del motivo</label>
                                <a id="errorFormulario_Descripcion" class="control-observacion colorTextoError  no-select" style="display: none"></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-separador"></div>

            <!-- Seccion Ubicacion -->
            <div class="row">
                <div class="col s12 m2 flex">
                    <label class="titulo no-select" style="flex: 1;">Ubicación</label>
                    <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                    <div style="display: none">
                        1- Use calle y número si tiene datos precisos.<br />
                        2- Use barrio si lo conoce y tiene algún dato de la dirección.<br />
                        3- Use otro cuando no tenga precisión de calle y barrio.
                    </div>
                </div>
                <div class="col s12 m10">
                    <div class="row">
                        <Controles:ControlDomicilioSelector runat="server" />
                    </div>
                    <label id="errorFormulario_Domicilio" class="control-observacion colorTextoError no-select" style="display: none; margin-left: 12px !important;"></label>

                </div>
            </div>

            <div id="contenedor_Iniciador" style="display: none">
                <div class="form-separador"></div>


                <!-- Seccion iniciador -->
                <div class="row">
                    <div class="col s12 m2 no-padding no-margin">
                        <div class="row">
                            <div class="col s12 flex">
                                <label class="titulo no-select" style="flex: 1;">Datos de contacto</label>
                                <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                                <div style="display: none">
                                    1- Escriba el número sin importar el tipo de documento.<br />
                                    2- No es obligatorio pero si carga el mail se le envía el comprobante de requerimiento.
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col s12 m10">
                        <div class="row">
                            <div class="col s12 no-padding">
                                <Controles:SelectorUsuario runat="server" />
                                <label id="errorFormulario_Usuario" class="margin-left control-observacion colorTextoError no-select" style="display: none; margin-left: 12px !important"></label>
                            </div>
                        </div>
                        <div class="row" id="contenedorMail" style="display: none">

                            <!-- Main -->
                            <div class="col s12">
                                <div class="input-field">
                                    <textarea id="inputFormulario_Mail" class="materialize-textarea"></textarea>
                                    <label for="inputFormulario_Mail" class=" no-select textarea">E-Mail</label>
                                    <a id="errorFormulario_Mail" class="control-observacion colorTextoError  no-select" style="display: none"></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--            <div class="form-separador"></div>--%>

            <%--            <!-- Seccion otros -->
            <div class="row ">
                <div class="col s12 m3 no-padding no-margin">
                    <div class="row flex">
                        <label class="titulo no-select" style="flex: 1;">Otra informacion</label>
                        <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                        <div style="display: none">
                            1- Marque relevamiento de oficio cuando sea informado por un empleado o funcionario.    
                        </div>
                    </div>
                </div>

                <div class="col s12 m9">
                    <div class="row">
                        <div class="col s4 mi-input-field no-padding">
                            <div class="checkboxs" id="contenedorCheckboxInicio">
                                <div>
                                    <input type="checkbox" id="check_RelevamientoInterno" />
                                    <label class="colorRelevamiento" for="check_RelevamientoInterno">Relevamiento De Oficio</label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>

            <div id="contenedor_CamposDinamicos"></div>
        </div>

        <!-- Pestaña Notas -->
        <div id="tabNotas" class="flex-main no-scroll flex direction-vertical">
            <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                <table id="tablaNotas"></table>
            </div>
            <div class="flex direction-horizontal">
                <div class="tabla-footer flex-main">
                </div>
            </div>
        </div>

        <!-- Pestaña Documentos -->
        <div id="tabDocumentos" class="flex flex-main scroll padding direction-vertical">
            <div style="display: block">
                <div class="row">
                    <div class="col s12 m3 l3">
                        <input type="file" id="inputDocumento" class="hide" accept="application/pdf" />
                        <div id="btnNuevoDocumento" class="card waves-effect btnNuevoArchivo">
                            <i class="material-icons">add</i>
                            <label>Agregar nuevo documento</label>
                        </div>
                    </div>
                </div>

                <div class="row" id="contenedorTituloDocumento" style="display: none">
                    <div class="col s12 no-padding margin-top">
                        <label class="subtitulo">Documentos agregados</label>
                    </div>
                </div>
            </div>

            <div id="contenedorArchivosDocumentos" class="flex-main scroll" style="display: none">
            </div>

            <div id="dragZoneDocumentos" class="dragzone" style="display: none">
                <i class="material-icons">arrow_downward</i>
                <label>Suelte aqui sus archivos para agregarlos</label>
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

        <!-- Pestaña Imagenes -->
        <div id="tabImagenes" class="flex-main scroll padding">
            <div style="display: block">
                <div class="row">
                    <div class="col s12 m3 l3">
                        <input type="file" id="inputImagen" class="hide" accept="image/*" />
                        <div id="btnNuevaImagen" class="card waves-effect btnNuevoArchivo">
                            <i class="material-icons">add</i>
                            <label>Agregar nueva imagen</label>
                        </div>
                    </div>
                </div>

                <div class="row" id="contenedorTituloImagenes" style="display: none">
                    <div class="col s12 no-padding margin-top">
                        <label class="subtitulo">Imagenes agregadas</label>
                    </div>
                </div>
            </div>

            <div id="contenedorArchivosImagenes" class="flex-main scroll" style="display: none">
            </div>

            <div id="dragZoneImagenes" class="dragzone" style="display: none">
                <i class="material-icons">arrow_downward</i>
                <label>Suelte aqui sus archivos para agregarlos</label>
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




    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>


