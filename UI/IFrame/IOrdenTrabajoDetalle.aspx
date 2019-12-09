<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IOrdenTrabajoDetalle.aspx.cs" Inherits="UI.IFrame.IOrdenTrabajoDetalle" %>

<%@ Register Src="~/Controls/ControlMapa.ascx" TagName="Mapa" TagPrefix="Controles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IOrdenTrabajoDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <input type="file" id="input_Imagen" accept="image/*" style="display: none" />
    <input type="file" id="input_Documento" accept=".xlsx,.xls,.doc,.docx,.ppt,.pptx,.txt,.pdf,.rtf" style="display: none" />

    <div id="contenedor_Detalle">
        <div id="contenedor_Encabezado">
            <div id="contenedor_Numero">
                <label id="texto_Numero"></label>
                <div id="contenedor_ServicioMotivoArea">
                    <label id="texto_Ambito"></label>
                    <label id="texto_Area"></label>
                    <div id="contenedor-infoParaAO" style="display: none">
                        <label id="texto_Zonas"></label>
                        <label id="texto_Seccion"></label>
                    </div>
                </div>
            </div>

            <div id="contenedor_Indicadores">
                <div class="fila fila1">
                    <div>
                        <i id="icono_IndicadorEstado" class="material-icons">swap_vertical_circle</i>
                        <div class="textos">
                            <label id="texto_IndicadorEstado"></label>
                        </div>
                    </div>
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

            <div class="flex direccion-horizontal" id="contenedor_SeccionDescripcion">

                <div id="contenedor_Descripcion" class="seccion flex-main">
                    <label class="titulo">Descripción</label>
                    <label id="texto_Descripcion"></label>
                </div>

                <div id="contenedor_IndicadoresRecursos" style="display: none">
                    <label class="titulo">Recursos</label>
                    <div>
                        <div id="btn_Empleados">
                            <label id="texto_CantidadEmpleados" class="badge"></label>
                            <i class="material-icons">person</i>
                        </div>
                        <div id="btn_Moviles">
                            <label id="texto_CantidadMoviles" class="badge"></label>
                            <i class="material-icons">directions_car</i>
                        </div>
                        <div id="btn_Flotas">
                            <label id="texto_CantidadFlotas" class="badge"></label>
                            <i class="material-icons">people</i>
                        </div>
                    </div>
                </div>
            </div>

            <div id="contenedor_Flotas" class="seccion">
                <label class="titulo">Flotas</label>
                <div class="contenido">
                    <div class="items"></div>
                </div>
            </div>

            <div id="contenedor_Empleados" class="seccion">
                <label class="titulo">Personal</label>
                <div class="contenido">
                    <div class="items"></div>
                </div>
            </div>

            <div id="contenedor_Moviles" class="seccion">
                <label class="titulo">Móviles</label>
                <div class="contenido">
                    <div class="items"></div>
                </div>
            </div>

            <div id="contenedor_Comentarios" class="seccion">
                <label class="titulo">Notas internas</label>

                <%--                <div class="sinItems">
                    <label>No hay notas internas</label>
                    <label class="link" id="btn_AgregarComentario">Agregar una</label>
                </div>--%>
                <div class="contenido">
                    <div class="items"></div>
                    <div class="verMas">
                        <label>Ver mas</label>
                    </div>
                </div>
            </div>

            <div id="contenedor_Requerimientos" class="seccion">
                <label class="titulo">Requerimientos</label>
                <div class="contenido">
                    <div class="items"></div>
                </div>
            </div>

            <div id="contenedor_UltimoEstado" class="seccion">
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

            </div>

            <div id="contenedor_RecursosAdicionales" class="seccion">
                <label class="titulo">Recursos adicionales</label>

                <div class="textos">
                    <label class="material"></label>
                    <label class="personal"></label>
                </div>

            </div>


            <div id="contenedor_InfoAdicional" class="seccion">
                <label class="titulo">Información adicional</label>

                <div class="linea1">
                    <label class="texto">Creado el</label>
                    <label class="textoFechaCreacion"></label>
                    <label class="texto textoUsuarioCreadorConector">por</label>
                    <label class="textoUsuarioCreador link"></label>
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

    <div id="contenedor_Mapa" style="display: none">
        <Controles:Mapa ID="mapa" runat="server" />
    </div>


    <%--Templates--%>
    <div id="template_Accion" style="display: none">
        <div class="accion">
            <i class="material-icons icono"></i>
            <label class="texto"></label>
        </div>
    </div>

    <div id="template_Comentario" style="display: none">
        <div class="comentario">
            -
            <div class="persona">
                <img src=""></img>
                <label class="link"></label>
            </div>

            <div class="card">
                <label class="para-requerimiento"></label>
                <label class="contenido"></label>
                <label class="fecha"></label>
            </div>
        </div>
    </div>

    <%--    <div id="template_Empleado" style="display: none">
        <div class="empleado">
            <div class="persona">
                <img class="foto" src=""></img>
                <label class=" nombre"></label>
                <a class="btn-flat btn-redondo">
                    <i class="material-icons borrar">delete</i></a>
            </div>
        </div>
    </div>--%>

    <div id="template_Empleado" style="display: none">
        <div class="empleado">
            <div class="persona">
                <div class="foto"></div>
                <label class="nombre link"></label>
                <label class="observaciones"></label>
            </div>
        </div>
    </div>

    <div id="template_GrupoRequerimientos" style="display: none">
        <div class="grupoRequerimientos">
            <div class="titulo">
                <label class="motivo"></label>
            </div>

            <div class="items"></div>
        </div>
    </div>

    <div id="template_Requerimiento" style="display: none">
        <div class="requerimiento">
            <div class="textos">
                <label class="numero link"></label>
                <label class="ubicacion"></label>
                <label class="descripcion"></label>
            </div>
            <div class="botones">
                <a class="btn-flat btn-redondo tareas"><i class="mdi mdi-hammer"></i></a>
                <a class="btn-flat btn-redondo ubicacion"><i class="material-icons">location_on</i></a>
                <a class="btn-flat btn-redondo nota "><i class="material-icons">comment</i><label class="badge"></label></a>
                <a class="btn-flat btn-redondo motivo"><i class="material-icons">edit</i></a>
                <a class="btn-flat btn-redondo borrar"><i class="material-icons">swap_vertical_circle</i></a>
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

    <div id="template_HistorialEstados" style="display: none">
        <div id="contenedor_HistorialEstados">
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


    <div id="template_Moviles" style="display: none">
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

    <div id="template_Empleados" style="display: none">
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


    <%-- Templantes --%>
    <div id="template_Flota" style="display: none">
        <div class="flota">
            <div class="contenedor_Encabezado">
                <div class="contenedor_Nombre">
                    <label class="nombre link"></label>
                </div>
            </div>

            <div class="contenedor_Informacion">
                <!-- <div class="contenedor_Acciones">
                </div> -->

                <div class="contenedor_Movil" style="display: none">
                    <label class="titulo">Móvil</label>
                    <label class="numero"></label>
                    <label class="tipo"></label>
                    <label class="marca"></label>
                    <label class="btnVerDetalle link">Ver detalle</label>
                </div>

                <div class="contenedor_Personal">
                    <div class="contenido">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="template_Movil" style="display: none">
        <div class="movil">
            <label class="numero link"></label>
            <label class="tipo"></label>
            <label class="marca"></label>
        </div>
    </div>

    <div id="template_Flotas" style="display: none">
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


    <%--Panel Deslizable--%>
    <div id="contenedor_PanelDeslizable">
        <div class="fondo"></div>
        <div class="contenido">
            <div class="encabezado">
                <label class="titulo">Titulo seccion deslizable</label>
                <a id="btn_CerrarPanelDeslizable" class="btn-flat btn-redondo waves-effect"><i class="material-icons">close</i></a>
            </div>
            <div class="contenedor_Contenido"></div>
        </div>
    </div>

    <!-- Mi Js -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IOrdenTrabajoDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
