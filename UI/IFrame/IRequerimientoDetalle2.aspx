<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IRequerimientoDetalle2.aspx.cs" Inherits="UI.IFrame.IRequerimientoDetalle2" ClientIDMode="Static" %>

<%@ Register Src="~/Controls/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Styles/Libs/photoswipe.css") %>" media="screen,projection" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Styles/Libs/default-skin/default-skin.css")%>" media="screen,projection" />
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IRequerimientoDetalle2.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <input type="file" id="input_Imagen" accept="image/*" style="display: none" />
    <input type="file" id="input_Documento" accept=".xlsx,.xls,.doc,.docx,.ppt,.pptx,.txt,.pdf,.rtf" style="display: none" />

    <div id="contenedor_Detalle">
        <div id="contenedor_Encabezado">
            <div id="contenedor_Numero">
                <label id="texto_Numero"></label>
                <div id="contenedor_ServicioMotivoArea">
                    <label id="texto_Servicio"></label>
                    <label id="texto_Motivo"></label>
                    <label id="texto_Area"></label>
                </div>
            </div>


            <div id="contenedor_Indicadores">
                <div class="fila fila1">
                    <div>
                        <i id="icono_IndicadorEstado" class="material-icons">swap_vertical_circle</i>
                        <div class="textos">
                            <label id="texto_IndicadorEstado"></label>
                            <%--<label id="btn_VerHistorialEstados" class="detalle link">Ver historial</label>--%>
                        </div>
                    </div>

                    <div>
                        <i id="icono_IndicadorPeligroso" class="material-icons">warning</i>
                        <div class="textos">
                            <label id="texto_IndicadorPeligroso">Peligroso</label>
                        </div>
                    </div>
                </div>
                <div class="fila fila2">
                    <div>
                        <i id="icono_IndicadorPrioridad" class="material-icons">flag</i>
                        <div class="textos">
                            <label id="texto_IndicadorPrioridad"><b>Prioridad: </b>Alta</label>
                        </div>
                    </div>
                    <div>
                        <i id="icono_IndicadorFavorito" class="material-icons">star</i>
                        <div class="textos">
                            <label id="texto_IndicadorFavorito">Es favorito</label>
                        </div>
                    </div>

                </div>
                <div class="fila fila3">
                    <div>
                        <label id="icono_IndicadorCpc" class="indicadorCPC">CPC</label>
                        <div class="textos">
                            <label id="texto_IndicadorCpc">En CPC</label>
                        </div>
                    </div>
                    <%--  <div>
                        <i id="icono_IndicadorOrdenAtencionCritica" class="material-icons">security</i>
                        <div class="textos">
                            <label id="texto_IndicadorOrdenAtencionCritica">Con Orden de Atencion Critica</label>
                           <label id="btn_IndicadorVerOrdenAtencionCritica" class="detalle link">Ver detalle</label>
                        </div>
                    </div>--%>
                </div>
            </div>
            <div id="contenedor_Acciones">
                <div class="contenido">
                </div>
            </div>
        </div>

        <div id="contenedor_Contenido">
            <div id="contenedor_Alertas">
            </div>

            <div>
                <div class="direction-horizontal flex">
                    <div class="flex direccion-horizontal" id="contenedor_SeccionDescripcion">

                        <div id="contenedor_Descripcion" class="flex-main">
                            <label class="titulo">Descripcion</label>
                            <label id="texto_Descripcion">"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</label>
                        </div>
                    </div>

                    <div id="contenedor_IndicadoresArchivos">
                        <label class="titulo">Adjuntos</label>
                        <div>
                            <div id="btn_Fotos">
                                <label id="texto_CantidadFotos" class="badge">3</label>
                                <i class="material-icons">photo_library</i>
                            </div>

                            <div id="btn_Documentos">
                                <label id="texto_CantidadDocumentos" class="badge">3</label>
                                <i class="material-icons">attach_file</i>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-separador"></div>
            </div>


            <div id="contenedor_CamposDinamicos" style="display: none">
                <div class="contenido"></div>

                <div class="form-separador"></div>
            </div>


            <div id="contenedor_SeccionUbicacion" class="direction-vertical flex">
                <label class="titulo">Ubicación del requerimiento</label>
                <label class="domicilio"></label>
                <label class="descripcion"></label>
                <label class="cpc"></label>
                <label class="barrio"></label>
                <label class="link" id="btn_VerMapa">Ver en mapa</label>
                <div class="form-separador"></div>
            </div>



            <div id="contenedor_Comentarios">
                <label class="titulo">Notas internas</label>

                <div class="sinItems">
                    <label>No hay notas internas</label>
                    <label class="link" id="btn_AgregarComentario">Agregar una</label>
                </div>
                <div class="contenido">
                    <div class="items"></div>
                    <div class="verMas">
                        <label>Ver mas</label>
                    </div>
                </div>

                <div class="form-separador"></div>
            </div>

            <div id="contenedor_Tareas">
                <label class="titulo">Tareas</label>

                <div class="sinItems">
                    <label>No hay tareas</label>
                    <label class="link" id="btn_AgregarTarea">Agregar una</label>
                </div>

                <div class="contenido">
                    <div class="items"></div>
                </div>

                <div class="form-separador"></div>
            </div>

            <div id="contenedor_ReferenteProvisorio" style="display: none">
                <label class="titulo">Referente Provisorio</label>
                <label class="nombre"></label>
                <label class="dni"></label>
                <label class="genero"></label>
                <label class="telefono"></label>
                <label class="observaciones"></label>

                <div class="form-separador"></div>
            </div>
            <div id="contenedor_UltimoEstado">
                <label class="titulo">Último estado</label>
                <div class="estado">
                    <div class="indicador">
                        <div class="circulo">
                            <i class="material-icons">swap_vertical</i>
                        </div>
                    </div>

                    <div class="textos">
                        <label class="nombre">Nuevo</label>
                        <label class="motivo">Test test test test test test 1</label>
                        <div class="contenedor_Fecha">
                            <label>Por </label>
                            <label class="nombrePersona link"></label>
                            <label class="fecha"></label>
                        </div>
                    </div>
                </div>
                <label id="btn_VerHistorialEstado" class="link" style="display: none">Ver historial de estados</label>

                <div class="form-separador"></div>
            </div>

            <div id="contenedor_UsuarioReferente">
                <label class="titulo">Usuarios Referentes</label>
                <div class="contenido">
                    <div class="items" style="flex-direction: row; display: flex; width: fit-content;">
                    </div>
                    <div class="verMas">
                        <label class="link">Ver/editar referentes</label>
                        <img src="" />
                    </div>
                </div>

                <div class="form-separador"></div>
            </div>

            <div id="contenedor_InformacionOrganica">
                <label class="titulo">Información orgánica</label>
                <label class="direccion">Direccion: test</label>
                <label class="secretaria">Secretaria: test</label>
                <label id="btn_VerInformacionOrgánica" class="link">Ver detalle</label>

                <div class="form-separador"></div>
            </div>

            <div id="contenedor_InfoAltaModificacion">
                <label class="titulo">Información de carga</label>
                <div class="linea1">
                    <label class="texto">Creado el</label>
                    <label class="textoFechaCreacion"></label>
                    <label class="texto textoUsuarioCreadorConector">por</label>
                    <label class="textoUsuarioCreador link"></label>
                    <label class="texto textoOrigenConector">desde</label>
                    <label class="textoOrigen"></label>
                </div>
                <div class="linea2">
                    <label class="texto">Fue modificado el</label>
                    <label class="textoFechaModificacion"></label>
                    <label class="texto textoUsuarioModificacionConector">por</label>
                    <label class="textoUsuarioModificacion link"></label>
                </div>
            </div>
        </div>
    </div>

    <%--Mapa--%>
    <%--<div id="contenedor_Mapa">
        <Controles:Mapa ID="mapa" runat="server" />
    </div>--%>


    <%--Templates--%>
    <div id="template_Accion" style="display: none">
        <div class="accion">
            <i class="material-icons icono"></i>
            <label class="texto"></label>
        </div>
    </div>

    <div id="template_Comentario" style="display: none">
        <div class="comentario">
            <div class="persona">
                <div class="foto"></div>
                <label class="link"></label>
            </div>

            <div class="card">
                <label class="contenido"></label>
                <label class="fecha"></label>
            </div>
        </div>
    </div>

    <%--Usuario referente--%>
    <div id="template_UsuarioReferente" style="display: none">
        <div class="usuarioReferente">
            <div class="persona">
                <div class="foto tooltipped" data-position="bottom" data-delay="50" data-tooltip=""></div>
                <label class="link"></label>
                <label class="observaciones"></label>
            </div>
        </div>
    </div>

    <div id="template_Alerta" style="display: none">
        <div class="alerta">
            <div class="textos">
                <label class="contenido"></label>
                <label class="detalle"></label>
            </div>
            <label class="link"></label>
        </div>
    </div>

    <div id="template_HistorialEstados" style="display: none">
        <div id="contenedor_HistorialEstados">
        </div>
    </div>

    <%--<div id="template_Tareas" style="display: none">
        <div class="flex direction-vertical no-scroll full-height">
            <div class="tabla-contenedor flex-main no-scroll flex direction-vertical">
                <table></table>
            </div>
            <div class="tabla-footer padding-left">
            </div>
        </div>
    </div>--%>


    <div id="template_Tareas" style="display: none">
        <div class="contenedor_Listado">
            <div class="contenido">
                <div class="busqueda">
                    <i class="material-icons">search</i>
                    <div>
                        <input type="text" placeholder="Buscar..." class="busqueda" />
                    </div>

                    <%--<a class="btn waves-effect btn_Nuevo">Agregar/Quitar</a>--%>
                </div>
                <div class="contenedor_Tabla">
                    <div class="tabla-contenedor">
                        <table></table>
                    </div>
                    <div class="tabla-footer">
                    </div>
                </div>
            </div>
            <div class="contenedor_Cargando panel-mensaje">
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
            <div class="contenedor_Error panel-mensaje">
                <i class="material-icons">error_outline</i>
                <label>Error procesando la solicitud</label>
                <a class="btn waves-effect btn_Reintentar">Reintentar</a>
            </div>
        </div>
    </div>

    <div id="template_Tarea" style="display: none">
        <div class="tarea">
            <div class="textos">
                <label class="nombre link"></label>
                <label class="descripcion"></label>
            </div>
            <div class="botones">
                <a class="btn-flat btn-redondo borrar"><i class="material-icons">delete</i></a>
            </div>
        </div>
    </div>
    <div id="template_HistorialEstadoItem" style="display: none">
        <div class="estado">
            <div class="indicador">
                <div class="circulo">
                    <i class="material-icons">swap_vertical</i>
                </div>
                <div class="linea1">
                </div>
                <div class="linea2">
                </div>
            </div>

            <div class="textos">
                <label class="nombre">Nuevo</label>

                <label class="motivo">Test test test test test test 1</label>
                <div class="contenedor_Fecha">
                    <label>Por </label>
                    <label class="nombrePersona link"></label>
                    <label class="fecha"></label>
                </div>
            </div>
        </div>
    </div>

    <div id="template_ComentariosDetalle" style="display: none">
        <div>
            <div class="contenedor_Input">
                <input type="text" placeholder="Nuevo comentario..." />
                <a class="btn btnNuevoComentario waves-effect colorExito">Agregar</a>
            </div>
            <div class="contenido"></div>
        </div>
    </div>

    <div id="template_HistoricoOrdenesTrabajo" style="display: none">
        <div class="contenido">
            <table></table>
            <div class="tabla-footer"></div>
        </div>
    </div>

    <div id="template_UsuariosReferentes" style="display: none">
        <div class="contenedor_Listado">
            <div class="contenido">
                <div class="busqueda">
                    <i class="material-icons">search</i>
                    <div>
                        <input type="text" placeholder="Buscar..." class="busqueda" />
                    </div>

                    <a class="btn waves-effect btn_Nuevo">Agregar/Quitar</a>
                </div>
                <div class="contenedor_Tabla">
                    <div class="tabla-contenedor">
                        <table></table>
                    </div>
                    <div class="tabla-footer">
                    </div>
                </div>
            </div>
            <div class="contenedor_Cargando panel-mensaje">
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
            <div class="contenedor_Error panel-mensaje">
                <i class="material-icons">error_outline</i>
                <label>Error procesando la solicitud</label>
                <a class="btn waves-effect btn_Reintentar">Reintentar</a>
            </div>
        </div>
    </div>


    <div id="template_HistoricoOrdenTrabajo" style="display: none">
        <div class="contenedor_HistoricoOrdenTrabajo card">
            <label class="numero"></label>
            <label class="fecha"></label>
            <div class="contenedor_Estado">
                <div class="indicador"></div>
                <div class="estado"></div>
            </div>
        </div>
    </div>

    <div id="template_Documentos" style="display: none">
        <div class="contenedor_Documentos">
            <div class="contenido">
            </div>
            <div class="error panel-mensaje">
                <i class="material-icons">error_outline</i>
                <label>Error consultando las fotos adjuntas.</label>
                <a class="btn_Reintentar btn waves-effect">Reintentar</a>
            </div>
            <div class="indicador_Vacio panel-mensaje">
                <i class="material-icons">photo_library</i>
                <label class="mensaje">El requerimiento no tiene ninguna foto adjunta</label>
                <a class="btn waves-effect colorExito">Agregar una</a>
            </div>

            <div class="cargando panel-mensaje">
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

    <div id="template_FotoPreview" style="display: none">
        <div class="fotoPreview card waves-effect">
            <div class="foto"></div>
            <div class="datos">
                <div class="datosTexto">
                    <label class="nombre"></label>
                    <label class="subidaPor"></label>
                </div>
                <a class="btn-flat waves-effect btn-redondo btn-circular"><i class="material-icons">more_vert</i></a>
            </div>
            <div class="cargando">
                <div class="preloader-wrapper small active">
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

    <%--Panel Deslizable--%>
    <div id="contenedor_PanelDeslizable">
        <div class="fondo"></div>
        <div class="contenido">
            <div class="encabezado">
                <label class="titulo">Titulo sección deslizable</label>
                <a id="btn_CerrarPanelDeslizable" class="btn-flat btn-redondo waves-effect"><i class="material-icons">close</i></a>
            </div>
            <div class="contenedor_Contenido"></div>
        </div>

    </div>


    <!-- Visor foto -->
    <!-- Root element of PhotoSwipe. Must have class pswp. -->
    <div class="pswp" tabindex="-1" role="dialog" aria-hidden="true">

        <!-- Background of PhotoSwipe. 
         It's a separate element as animating opacity is faster than rgba(). -->
        <div class="pswp__bg"></div>

        <!-- Slides wrapper with overflow:hidden. -->
        <div class="pswp__scroll-wrap">

            <!-- Container that holds slides. 
            PhotoSwipe keeps only 3 of them in the DOM to save memory.
            Don't modify these 3 pswp__item elements, data is added later on. -->
            <div class="pswp__container">
                <div class="pswp__item"></div>
                <div class="pswp__item"></div>
                <div class="pswp__item"></div>
            </div>

            <!-- Default (PhotoSwipeUI_Default) interface on top of sliding area. Can be changed. -->
            <div class="pswp__ui pswp__ui--hidden">

                <div class="pswp__top-bar">

                    <!--  Controls are self-explanatory. Order can be changed. -->

                    <div class="pswp__counter"></div>

                    <button class="pswp__button pswp__button--zoom" title="Zoom in/out"></button>
                    <button class="pswp__button pswp__button--close" title="Close (Esc)"></button>

                    <!-- Preloader demo http://codepen.io/dimsemenov/pen/yyBWoR -->
                    <!-- element will get class pswp__preloader--active when preloader is running -->
                    <div class="pswp__preloader">
                        <div class="pswp__preloader__icn">
                            <div class="pswp__preloader__cut">
                                <div class="pswp__preloader__donut"></div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="pswp__share-modal pswp__share-modal--hidden pswp__single-tap">
                    <div class="pswp__share-tooltip"></div>
                </div>

                <button class="pswp__button pswp__button--arrow--left" title="Previous (arrow left)">
                </button>

                <button class="pswp__button pswp__button--arrow--right" title="Next (arrow right)">
                </button>

                <div class="pswp__caption">
                    <div class="pswp__caption__center"></div>
                </div>

            </div>

        </div>

    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/photoswipe.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/Scripts/Libs/photoswipe-ui-default.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/_PoligonosCPC.js?v=1")%>"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IRequerimientoDetalle2.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>

</asp:Content>
