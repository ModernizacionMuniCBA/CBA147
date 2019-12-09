var requerimiento;
var permisos;
var tareas;
var tieneTareas = false;
var tieneCamposDinamicos = false;

//Mapa
var MAX_ZOOM = 19;
var map;
var viendoPoligonosCPC = false;
var poligonosCPC = [];
var mapaExpandido;
var marcador;
var popupInfoDomicilio;

var panelCallback;

var PERMISO_EDITAR_FAVORITO = 1;
var PERMISO_EDITAR_UBICACION = 2;
var PERMISO_EDITAR_ESTADO = 3;
var PERMISO_CANCELAR = 4;
var PERMISO_EDITAR_PRIORIDAD = 5;
var PERMISO_EDITAR_MARCADO = 6;
var PERMISO_EDITAR_REFERENTE = 7;
var PERMISO_EDITAR_COMENTARIOS = 8;
var PERMISO_EDITAR_MOTIVO = 9;
var PERMISO_EDITAR_DOCUMENTOS = 12;
var PERMISO_EDITAR_RELEVAMIENTO_OFICIO = 13;
var PERMISO_EDITAR_OBSERVACIONES = 14;
var PERMISO_AGREGAR_TAREAS = 24;

var PATH_IMAGEN_ERROR;
var PATH_IMAGEN_ICON_ADD;

let PATH_IMAGEN_USER_MALE;
let PATH_IMAGEN_USER_FEMALE;

// ADJUNTOS
const IMAGE_SIZE = 5;
const DOCUMENT_SIZE = 5;
const IMAGE_EXTENSIONS = ["png", "jpg", "gif", "tiff", "jpeg"];
const DOCUMENT_EXTENSIONS = [
  "pdf",
  "txt",
  "doc",
  "docx",
  "xls",
  "xlsx",
  "pps",
  "ppt",
  "pptx",
  "ogg",
  "odt",
  "ott",
  "ods"
];

function init(data) {
    if ("Error" in data) {
        mostrarError(data.Error);
        return;
    }

    PATH_IMAGEN_USER_MALE =
      top.urlCordobaFiles + "/Archivo/" + top.identificadorFotoUserMale + "/3";
    PATH_IMAGEN_USER_FEMALE =
      top.urlCordobaFiles + "/Archivo/" + top.identificadorFotoUserFemale + "/3";

    PATH_IMAGEN_ERROR = ResolveUrl("~/Resources/Imagenes/imagen_placeholder.jpg");
    PATH_IMAGEN_ICON_ADD = ResolveUrl("~/Resources/Imagenes/icon_add.png");

    requerimiento = data.Requerimiento;
    permisos = data.Permisos;
    tieneTareas = data.TieneTareas;
    tieneCamposDinamicos = data.TieneCamposDinamicos;

    console.log(permisos);

    console.log(requerimiento);

    initEncabezado();
    initAlertas();
    initPanelDeslizable();
    //initMapa();

    initComentarios();
    initDescripcion();
    initUltimoEstado();
    initUbicacion();
    initUsuariosReferentes();
    initInformacionOrganica();
    initInformacionAdicional();
    initAdjuntos();
    cargarDatos();
}

function cargarDatos() {
    cargarAcciones();
    cargarDatosEncabezado();
    cargarAlertas();
    cargarDescripcion();
    cargarUbicacion();
    cargarComentarios();
    cargarUltimoEstado();
    cargarUsuariosReferentes();
    cargarInformacionOrganica();
    cargarInformacionAdicional();
    cargarAdjuntos();
    cargarTareas();
    cargarReferenteProvisorio();
    cargarCamposDinamicosPorRequerimiento();
}

//Encabezado
function initEncabezado() {
    $("#btn_Acciones").click(function () {
        $("#contenedor_Acciones .contenido").toggleClass("visible");
        $(this).text(
          $("#contenedor_Acciones .contenido").hasClass("visible")
            ? "Ocultar acciones"
            : "Ver acciones"
        );
    });
}

function cargarDatosEncabezado() {
    //Numero
    $("#texto_Numero").text(requerimiento.Numero + "/" + requerimiento.Año);

    //Servicio
    if (
      requerimiento.ServicioNombre == undefined ||
      requerimiento.ServicioNombre == ""
    ) {
        $("#texto_Servicio").html("<b>Servicio </b> Sin datos");
    } else {
        $("#texto_Servicio").html(
          "<b>Servicio </b>" + toTitleCase(requerimiento.ServicioNombre)
        );
    }

    //Motivo
    if (
      requerimiento.MotivoNombre == undefined ||
      requerimiento.MotivoNombre == ""
    ) {
        $("#texto_Motivo").html("<b>Motivo </b>Sin datos");
    } else {
        $("#texto_Motivo").html(
          "<b>Motivo </b>" + toTitleCase(requerimiento.MotivoNombre)
        );
    }

    //Area
    if (requerimiento.AreaNombre == undefined || requerimiento.AreaNombre == "") {
        $("#texto_Area").html("<b>Área </b>Sin datos");
    } else {
        $("#texto_Area").html(
          "<b>Área </b>" + toTitleCase(requerimiento.AreaNombre)
        );
    }

    //Estado
    $("#texto_IndicadorEstado").html(
      "<b>Estado</b> " + toTitleCase(requerimiento.EstadoNombre)
    );
    $("#icono_IndicadorEstado").css("color", "#" + requerimiento.EstadoColor);

    //Prioridad
    if (requerimiento.Prioridad == undefined) {
        $("#texto_IndicadorPrioridad").html("<b>Prioridad </b> Sin datos");
        $("#icono_IndicadorPrioridad").css("color", "var(--colorIndicadorApagado)");
    } else {
        switch (requerimiento.Prioridad) {
            case 1:
                {
                    $("#texto_IndicadorPrioridad").html("<b>Prioridad </b> Baja");
                    $("#icono_IndicadorPrioridad").css(
                      "color",
                      "var(--colorPrioridadNormal)"
                    );
                }
                break;

            case 2:
                {
                    $("#texto_IndicadorPrioridad").html("<b>Prioridad </b> Media");
                    $("#icono_IndicadorPrioridad").css(
                      "color",
                      "var(--colorPrioridadMedia)"
                    );
                }
                break;

            case 3:
                {
                    $("#texto_IndicadorPrioridad").html("<b>Prioridad </b> Alta");
                    $("#icono_IndicadorPrioridad").css(
                      "color",
                      "var(--colorPrioridadAlta)"
                    );
                }
                break;
            default:
                {
                    $("#texto_IndicadorPrioridad").html("<b>Prioridad </b> Sin datos");
                    $("#icono_IndicadorPrioridad").css(
                      "color",
                      "var(--colorIndicadorApagado)"
                    );
                }
                break;
        }
    }

    //Peligroso
    if (requerimiento.Peligroso) {
        $("#icono_IndicadorPeligroso").css(
          "color",
          "var(--colorIndicadorPeligroso)"
        );
        $("#texto_IndicadorPeligroso").text("Peligroso");
    } else {
        $("#icono_IndicadorPeligroso").css("color", "var(--colorIndicadorApagado)");
        $("#texto_IndicadorPeligroso").text("No peligroso");
    }

    //Cpc
    if (requerimiento.Marcado) {
        $("#icono_IndicadorCpc").css("background", "var(--colorIndicadorCpc");
        $("#texto_IndicadorCpc").text("En control de CPC");
    } else {
        $("#icono_IndicadorCpc").css("background", "var(--colorIndicadorApagado)");
        $("#texto_IndicadorCpc").text("En control de Área Operativa");
    }

    //Favorito
    if (requerimiento.Favorito) {
        $("#icono_IndicadorFavorito").css("color", "var(--colorIndicadorFavorito");
        $("#texto_IndicadorFavorito").text("Es favorito");
    } else {
        $("#icono_IndicadorFavorito").css("color", "var(--colorIndicadorApagado)");
        $("#texto_IndicadorFavorito").text("No es favorito");
    }
}

//Estados
function mostrarHistorialDeEstados() {
    abrirPanelDeslizable("Historial de Estados");

    var divHistorialEstados = $($("#template_HistorialEstados").html());
    $("#contenedor_PanelDeslizable .contenedor_Contenido").append(
      divHistorialEstados
    );

    $.each(requerimiento.Estados, function (index, element) {
        var div = $($("#template_HistorialEstadoItem").html());
        if (index == 0) {
            $(div)
              .find(".linea1")
              .css({ opacity: 0 });
        }
        if (index == requerimiento.Estados.length - 1) {
            $(div)
              .find(".linea2")
              .css({ opacity: 0 });
        }
        $(div)
          .find(".circulo")
          .css({ "background-color": "#" + element.EstadoColor });
        $(div)
          .find(".nombre")
          .text(toTitleCase(element.EstadoNombre));
        $(div)
          .find(".motivo")
          .text(element.EstadoObservaciones);
        $(div)
          .find(".nombrePersona")
          .html(
            "<b>" +
              toTitleCase(element.UsuarioNombre + " " + element.UsuarioApellido) +
              "</b>"
          );
        $(div)
          .find(".nombrePersona")
          .click(function () {
              crearDialogoUsuarioDetalle({
                  Id: element.UsuarioId
              });
          });

        $(div)
          .find(".fecha")
          .html(" el <b>" + dateTimeToString(element.EstadoFecha) + "</b>");

        $(divHistorialEstados).append(div);
    });
}

//Tareas
function abrirPanelTareas() {
    if (!validarPermisoRequerimiento(PERMISO_AGREGAR_TAREAS)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    abrirPanelDeslizable("Tareas", function () {
        editarTareas();
    });

    var divContenido = $($("#template_Tareas").html());
    $("#contenedor_PanelDeslizable .contenedor_Contenido").append(divContenido);

    //Reintentar
    $(divContenido)
      .find(".btn_Reintentar")
      .click(function () {
          abrirPanelTareas();
      });

    if (tareas == undefined) {
        $(divContenido)
          .find(".contenedor_Cargando")
          .addClass("visible");
        buscarTareas()
          .then(function (data) {
              tareas = data;
              idsTareasSeleccionadas = _.pluck(requerimiento.Tareas, "Id");
              initTablaTareas(divContenido);
          })
          .catch(function () {
              cerrarPanelDeslizable();
          })
          .finally(function () {
              $(divContenido)
                .find(".contenedor_Cargando")
                .removeClass("visible");
          });
        return;
    }

    initTablaTareas(divContenido);
}

let idsTareasSeleccionadas = [];
function initTablaTareas(divContenido) {
    $(divContenido)
      .find("table")
      .prop("id", "tablaTareas");
    $(divContenido)
      .find("input.busqueda")
      .prop("id", "input_BusquedaTareas");

    let dt = $("#tablaTareas").DataTableTareas({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackActualizar: function (tarea) {
            actualizarListaTareas(tarea);
        },
        Orden: [[0, "desc"]],
        Buscar: true,
        InputBusqueda: "#input_BusquedaTareas",
        BotonSeleccionar: true,
        Callback: function (data) {
            dt.rows().every(function () {
                let d = this.data();
                console.log(d);
                if (d.Id == data.Id) {
                    let node = this.node();
                    $(node)
                      .find("input")
                      .prop("checked", idsTareasSeleccionadas.indexOf(data.Id) != -1);
                }
                console.log(idsTareasSeleccionadas);
            });
        },
        OnFilaCreada: function (row, data) {
            //$(row).find('.ordenar span').prop('value', idsTareasSeleccionadas.indexOf(data.Id) != -1? '1':'0')
            $(row)
              .find("input")
              .prop("checked", idsTareasSeleccionadas.indexOf(data.Id) != -1);
            $(row)
              .find("input")
              .change(function () {
                  let check = $(this).is(":checked");
                  if (check) {
                      if (idsTareasSeleccionadas.indexOf(data.Id) == -1) {
                          idsTareasSeleccionadas.push(data.Id);
                      }
                  } else {
                      idsTareasSeleccionadas = $.grep(idsTareasSeleccionadas, function (
                        element,
                        index
                      ) {
                          return element != data.Id;
                      });
                  }

                  console.log(idsTareasSeleccionadas);
              });
        }
    });

    //Inicializo los tooltips
    dt.$(".tooltipped").tooltip({ delay: 50 });
    dt.$(".selectMaterialize").material_select();

    //Muevo el indicador y el paginado a mi propio div
    $(divContenido)
      .find(".tabla-footer")
      .empty();
    $(divContenido)
      .find(".dataTables_info")
      .detach()
      .appendTo($(divContenido).find(".tabla-footer"));
    $(divContenido)
      .find(".dataTables_paginate")
      .detach()
      .appendTo($(divContenido).find(".tabla-footer"));
    $(divContenido)
      .find(".dataTables_info")
      .hide();

    //Agrego las filas
    let hDisponible = $(divContenido)
      .find(".tabla-contenedor")
      .height();
    let rows = calcularCantidadRowsDataTable(hDisponible);
    dt.page.len(rows);

    tareas = ordenarListaTareas(tareas);
    dt.rows.add(tareas).draw(true);
    dt.order([0, "desc"]).draw();
}

function ordenarListaTareas() {
    //recorro los ids de las tareas seleccionadas y busco el objeto en la lista de tareas
    var tareasOrdenadas = [];
    $.each(idsTareasSeleccionadas, function (i, id) {
        var index;
        var tareaSeleccionada = _.find(tareas, function (t) {
            //guardo el index para poder borrar el elemento de la lista de tareas general
            index = tareas.indexOf(t);

            return id == t.Id;
        });

        //cuando la encuentro la pongo en la lista ordenada
        tareasOrdenadas.push(tareaSeleccionada);
        //elimino de la lista de todas las tareas, la ya ordenada
        tareas.splice(index, 1);
    });

    //concateno a las tareas ordenadas, las tareas restantes (que no estan seleccionadas)
    tareasOrdenadas = tareasOrdenadas.concat(tareas);
    return tareasOrdenadas;
}

function buscarTareas() {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl(
              "~/Servicios/TareaService.asmx/GetByIdRequerimientoYArea"
            ),
            Data: { idRequerimiento: requerimiento.Id },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    mostrarMensaje("Error", result.Error);
                    callbackError();
                    return;
                }

                //tareas = result.Return;
                callback(result.Return);
            },
            OnError: function (result) {
                mostrarMensaje("Error", "Error procesando la solicitud");
                callbackError();
            }
        });
    });
}

function calcularCantidadDeRowsTareas(div) {
    var hDisponible = $(div)
      .find(".tabla-contenedor")
      .height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $(div)
      .find("table")
      .DataTable();
    dt.page.len(rows).draw();
    console.log(rows);
}

function editarTareas() {
    mostrarCargando(true);
    crearAjax({
        Data: {
            comando: {
                IdRequerimiento: requerimiento.Id,
                IdsTareas: getIdsTareasSeleccionadas()
            }
        },
        Url: ResolveUrl("~/Servicios/RequerimientoService.asmx/EditarTareas"),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
                return;
            }

            actualizarDetalle(function () {
                cargarTareas();
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function quitarTarea(idTarea) {
    mostrarCargando(true);
    crearAjax({
        Data: {
            comando: {
                IdRequerimiento: requerimiento.Id,
                IdTarea: idTarea
            }
        },
        Url: ResolveUrl("~/Servicios/RequerimientoService.asmx/QuitarTarea"),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
                return;
            }

            actualizarDetalle(function () {
                cargarTareas();
                deseleccionarTarea(idTarea);
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function getIdsTareasSeleccionadas() {
    return idsTareasSeleccionadas;
}

function cargarTareas() {
    if (!tieneTareas) {
        $("#contenedor_Tareas").hide();
        return;
    }

    $("#contenedor_Tareas > .contenido > .items").empty();

    if (requerimiento.Tareas == undefined || requerimiento.Tareas.length == 0) {
        $("#contenedor_Tareas .sinItems").show();
        $("#contenedor_Tareas .verMas").hide();
        $("#contenedor_Tareas .contenido").hide();

        if (!validarPermisoRequerimiento(PERMISO_AGREGAR_TAREAS)) {
            $("#contenedor_Tareas .sinItems .link").hide();
            return;
        }

        $("#contenedor_Tareas .sinItems .link").click(function () {
            abrirPanelTareas();
        });
    } else {
        $("#contenedor_Tareas .sinItems").hide();
        $("#contenedor_Tareas .contenido").show();

        $.each(requerimiento.Tareas, function (index1, tarea) {
            let html_tarea = crearHtmlTarea(tarea);
            $("#contenedor_Tareas")
              .find(".items")
              .append(html_tarea);
        });
    }
}

function crearHtmlTarea(tarea) {
    //Descripcion
    let nombre = tarea.Nombre;

    //Descripcion
    let descripcion = tarea.Descripcion != undefined ? tarea.Descripcion : "";

    var html = $($("#template_Tarea").html());
    $(html).attr("id-tarea", tarea.Id);
    $(html)
      .find("> .textos > .nombre")
      .text(nombre);

    $(html)
      .find("> .textos > .descripcion")
      .html(descripcion);

    //Nombre click
    $(html)
      .find("> .textos > .nombre")
      .click(function () {
          crearDialogoTareaDetalle({
              Id: tarea.Id,
              Callback: function () {
                  actualizarDetalle(function () {
                      cargarTareas();
                  });
              },
              CallbackMensajes: function (tipo, mensaje) {
                  mostrarMensaje(tipo, mensaje);
              }
          });
      });

    //Boton borrar
    $(html)
      .find(".botones .borrar")
      .click(function () {
          if (tarea.Id == undefined) return;
          quitarTarea(tarea.Id);
      });

    return html;
}

function deseleccionarTarea(id) {
    //si las tareas del panel deslizable todavia no se buscaron, salgo del metodo
    if (tareas == undefined) return;

    $("#tablaTareas tr:has(td)")
      .find('input[type="checkbox"]')
      .prop("checked", true);
}

//Alertas
function initAlertas() { }

function mostrarAlertaRequerimientoCancelado() {
    var div = $("#template_Alerta").html();
    div = $(div);
    $(div).attr("id", "alertaRequerimientoCancelado");
    $(div).addClass("negro");
    $(div).empty();

    var divEncabezado = $(
      '<div class="encabezado"><label class="primerTexto"></label><div class="contenedor_Persona"><label class="nombre link"></label></div><label class="segundoTexto"></label></div>'
    );
    $(divEncabezado).appendTo($(div));
    $(divEncabezado)
      .find(".primerTexto")
      .text("El requerimiento fue cancelado por ");

    $(divEncabezado)
      .find(".nombre")
      .html(
        "<b>" +
          toTitleCase(
            (
              requerimiento.EstadoUsuarioNombre +
              " " +
              requerimiento.EstadoUsuarioApellido
            ).trim()
          ) +
          "</b>"
      );
    $(divEncabezado)
      .find(".nombre")
      .click(function () {
          crearDialogoUsuarioDetalle({
              Id: requerimiento.EstadoUsuarioId
          });
      });

    $(divEncabezado)
      .find(".segundoTexto")
      .html(" el <b>" + dateTimeToString(requerimiento.EstadoFecha) + "</b>");

    var labelMotivo = $('<label class="detalle">');
    console.log(labelMotivo);

    $(labelMotivo).appendTo($(div));
    if (
      requerimiento.EstadoObservaciones == undefined ||
      requerimiento.EstadoObservaciones == ""
    ) {
        $(labelMotivo).html("<b>Motivo </b> Sin datos");
    } else {
        $(labelMotivo).html("<b>Motivo </b>" + requerimiento.EstadoObservaciones);
    }

    $(div).appendTo("#contenedor_Alertas");
}

function mostrarAlertaRequerimientoEnOrdenDeTrabajo() {
    var div = $("#template_Alerta").html();
    div = $(div);
    $(div).addClass("naranja");

    $(div)
      .find(".contenido")
      .text(
        "Requerimiento en Orden de Trabajo N° " +
          requerimiento.OrdenTrabajoNumero +
          "/" +
          requerimiento.OrdenTrabajoAño
      );
    $(div)
      .find(".link")
      .text("Ver detalle");
    $(div).appendTo("#contenedor_Alertas");

    $(div)
      .find(".link")
      .click(function () {
          crearDialogoOrdenTrabajoDetalle({
              Id: requerimiento.OrdenTrabajoId,
              Callback: function () {
                  actualizarDetalle(function () {
                      cargarDatos();
                  });
              }
          });
      });
}

function mostrarAlertaHistoricoOrdenesDeTrabajo() {
    var div = $("#template_Alerta").html();
    div = $(div);
    $(div).addClass("amarillo");

    var unaSola = false;
    var texto = "";
    if (requerimiento.OrdenesDeTrabajo.length == 1) {
        unaSola = true;
        texto =
          "El requerimiento formó parte de la Orden de Trabajo <b>N° " +
          requerimiento.OrdenesDeTrabajo[0].Numero +
          "/" +
          requerimiento.OrdenesDeTrabajo[0].Año +
          "</b>";
    } else {
        texto =
          "El requerimiento formó parte de <b>" +
          requerimiento.OrdenesDeTrabajo.length +
          "</b> ordenes de trabajo";
    }
    $(div)
      .find(".contenido")
      .html(texto);
    $(div)
      .find(".link")
      .text(unaSola ? "Ver detalle" : "Ver histórico");
    $(div).appendTo("#contenedor_Alertas");

    $(div)
      .find(".link")
      .click(function () {
          if (unaSola) {
              crearDialogoOrdenTrabajoDetalle({
                  Id: requerimiento.OrdenesDeTrabajo[0].Id,
                  Callback: function () {
                      actualizarDetalle(function () {
                          cargarDatos();
                      });
                  }
              });
          } else {
              mostrarHistoricoOrdenesDeTrabajo();
          }
      });
}

function ocultarAlertas() {
    $("#contenedor_Alertas").empty();
}

function cargarAlertas() {
    ocultarAlertas();

    var estaCancelado = requerimiento.EstadoKeyValue == 5;
    var tieneOT =
      requerimiento.OrdenTrabajoId != undefined &&
      requerimiento.OrdenTrabajoId != 0;
    var tieneHistoricoOT =
   requerimiento.OrdenesDeTrabajo != undefined &&
   requerimiento.OrdenesDeTrabajo.length != 0;

    if (estaCancelado) {
        mostrarAlertaRequerimientoCancelado();
    }

    if (tieneOT) {
        mostrarAlertaRequerimientoEnOrdenDeTrabajo();
    }

    if (tieneHistoricoOT) {
        mostrarAlertaHistoricoOrdenesDeTrabajo();
    }

}

//Panel Deslizable
function initPanelDeslizable() {
    $("#btn_CerrarPanelDeslizable").click(function () {
        cerrarPanelDeslizable();
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            if ($("#contenedor_PanelDeslizable").hasClass("visible")) {
                cerrarPanelDeslizable();
            }
        }
    });
}

let callbackPanel;
function abrirPanelDeslizable(titulo, callback) {
    $("#contenedor_PanelDeslizable").addClass("visible");

    $("#contenedor_PanelDeslizable .encabezado .titulo").text(titulo);

    $("#contenedor_PanelDeslizable .contenedor_Contenido").scrollTop(0);
    $("#contenedor_PanelDeslizable .contenedor_Contenido").empty();

    panelAbierto(true);

    callbackPanel = callback;
}

function cerrarPanelDeslizable() {
    $("#contenedor_PanelDeslizable .encabezado .titulo").text("");
    $("#contenedor_PanelDeslizable").removeClass("visible");
    panelAbierto(false);

    //llamo al callback que se definio (si es que se hizo) cuando se abrio el panel
    if (callbackPanel != undefined) {
        callbackPanel();
    }

    //reseteo el callback
    callbackPanel = undefined;
}

//Mapa
//function initMapa() {
//    ControlMapa_Init({
//        ResaltarAlHacerClick: false,
//        OnMapReady: function (mapaNuevo) {
//            map = mapaNuevo;
//            cargarMapa();

//            let btnExpandir = $('<a class="btn-mapa">Expandir</a>');
//            $('#ControlMapa_Botones').prepend(btnExpandir);

//            $(btnExpandir).click(function () {

//                if (mapaExpandido) {
//                    $(btnExpandir).text('Expandir');
//                    achicarMapa();
//                } else {
//                    $(btnExpandir).text('Achicar');
//                    expandirMapa();
//                }
//            });

//            let btnCentrar = $('<a class="btn-mapa">Centrar</a>');
//            $('#ControlMapa_Botones').prepend(btnCentrar);

//            $(btnCentrar).click(function () {
//                centrarMapa();
//            });

//        }
//    });
//}

//function mover(x, y) {
//    if (map == undefined) return;

//    var center = new google.maps.LatLng(x, y);
//    console.log('x: ' + x);
//    console.log('y: ' + y);

//    map.panTo(center);
//}

//function pinSymbol(color) {
//    return {
//        path: 'M31.5,0C14.1,0,0,14,0,31.2C0,53.1,31.5,80,31.5,80S63,52.3,63,31.2C63,14,48.9,0,31.5,0z M31.5,52.3 c-11.8,0-21.4-9.5-21.4-21.2c0-11.7,9.6-21.2,21.4-21.2s21.4,9.5,21.4,21.2C52.9,42.8,43.3,52.3,31.5,52.3z',
//        fillColor: color,
//        fillOpacity: 1,
//        anchor: new google.maps.Point(35, 70),
//        strokeColor: '#000',
//        strokeWeight: 2,
//        scale: 0.45,
//    };
//}

//function crearHtmlPopup() {

//    var divTextos = $('<div class="textos">');

//    //Titulo
//    $(divTextos).append('<label class="titulo">Ubicación del requerimiento</label>');

//    //Direccion
//    if ('DomicilioDireccion' in requerimiento && requerimiento.DomicilioDireccion != undefined && requerimiento.DomicilioDireccion != "") {
//        $(divTextos).append('<label><b>Dirección' + (requerimiento.DomicilioSugerido !== undefined && requerimiento.DomicilioSugerido ? ' aprox' : '') + ': </b>' + toTitleCase(requerimiento.DomicilioDireccion) + (requerimiento.DomicilioDistancia !== undefined && requerimiento.DomicilioDistancia > 0 ? ' (' + requerimiento.DomicilioDistancia + ' m)' : '') + '</b></label>');
//    } else {
//        $(divTextos).append('<label><b>Dirección: </b>Sin datos</b></label>');
//    }

//    //Observaciones
//    if ('DomicilioObservaciones' in requerimiento && requerimiento.DomicilioObservaciones != undefined && requerimiento.DomicilioObservaciones != "") {
//        $(divTextos).append('<label><b>Descripción: </b>' + toTitleCase(requerimiento.DomicilioObservaciones) + '</b></label>');
//    } else {
//        $(divTextos).append('<label><b>Descripción: </b>Sin datos</b></label>');
//    }

//    //Cpc
//    let cpc = '';
//    if ('DomicilioCpcNombre' in requerimiento && 'DomicilioCpcNumero' in requerimiento && requerimiento.DomicilioCpcNombre != undefined && requerimiento.DomicilioCpcNumero != undefined) {
//        $(divTextos).append('<label><b>CPC: </b>' + ("N° " + requerimiento.DomicilioCpcNumero + ' - ' + requerimiento.DomicilioCpcNombre) + '</b></label>');
//    } else {
//        $(divTextos).append('<label><b>CPC: </b>Sin datos</b></label>');
//    }

//    //Barrio
//    if ('DomicilioBarrioNombre' in requerimiento && requerimiento.DomicilioBarrioNombre != undefined && requerimiento.DomicilioBarrioNombre != "") {
//        $(divTextos).append('<label><b>Barrio: </b>' + requerimiento.DomicilioBarrioNombre + '</b></label>');
//    } else {
//        $(divTextos).append('<label><b>Barrio: </b>Sin datos</b></label>');
//    }

//    var html = $('<div class="popup">');
//    $(divTextos).appendTo(html);
//    return html;
//}

//function cargarMapa() {
//    var tieneDomicilio = requerimiento.DomicilioLatitud != undefined && requerimiento.DomicilioLatitud != 0 && requerimiento.DomicilioLongitud != undefined && requerimiento.DomicilioLongitud != 0;

//    if (tieneDomicilio) {

//        let pos = { lat: parseFloat(requerimiento.DomicilioLatitud.replace(',', '.')), lng: parseFloat(requerimiento.DomicilioLongitud.replace(',', '.')) };
//        centrarMapa();

//        if (marcador == undefined) {
//            marcador = new google.maps.Marker();
//        }

//        marcador.setPosition(pos);
//        marcador.setMap(map);
//        marcador.setIcon(pinSymbol('#' + requerimiento.EstadoColor));

//        var html = crearHtmlPopup();

//        if (html != undefined) {

//            if (popupInfoDomicilio == undefined) {
//                popupInfoDomicilio = new google.maps.InfoWindow({
//                    maxWidth: 200
//                });
//            }

//            popupInfoDomicilio.setContent($(html).prop('outerHTML'));

//            marcador.addListener('click', function () {
//                popupInfoDomicilio.open(map, marcador);
//            });

//            popupInfoDomicilio.open(map, marcador);
//        }

//    } else {

//        var centroCordoba = { lat: parseFloat(-31.416111), lng: parseFloat(-64.191174) };
//        map.setCenter(centroCordoba);
//        map.setZoom(13);

//        if (marcador != undefined) {
//            marcador.setMap(undefined);
//        }

//        if (popupInfoDomicilio != undefined) {
//            popupInfoDomicilio.close();
//        }
//    }
//}

//Descripcion
function initDescripcion() { }

function cargarDescripcion() {
    var descripcion = requerimiento.Descripcion;
    if (descripcion == undefined || descripcion.trim() == "") {
        $("#texto_Descripcion").text("Sin datos");
    } else {
        $("#texto_Descripcion").text(descripcion);
    }
}

//Ubicacion
function initUbicacion() {
    $("#btn_VerMapa").click(function () {
        window.open(
          "https://www.google.com/maps/search/?api=1&query=" +
            requerimiento.DomicilioLatitud.replace(",", ".") +
            "," +
            requerimiento.DomicilioLongitud.replace(",", "."),
          "_blank"
        );
    });
}

function cargarUbicacion() {
    //direccion
    $("#contenedor_SeccionUbicacion .domicilio").html(
      "<b>Dirección" +
        (requerimiento.DomicilioSugerido !== undefined &&
        requerimiento.DomicilioSugerido
          ? " aprox"
          : "") +
        ": </b>" +
        toTitleCase(requerimiento.DomicilioDireccion) +
        (requerimiento.DomicilioDistancia !== undefined &&
        requerimiento.DomicilioDistancia > 0
          ? " (" + requerimiento.DomicilioDistancia + " m)"
          : "") +
        "</b>"
    );

    //Observaciones
    if (
      "DomicilioObservaciones" in requerimiento &&
      requerimiento.DomicilioObservaciones != undefined &&
      requerimiento.DomicilioObservaciones != ""
    ) {
        $("#contenedor_SeccionUbicacion .descripcion").html(
          "<b>Descripción: </b>" +
            toTitleCase(requerimiento.DomicilioObservaciones) +
            "</b>"
        );
    }
    //else {
    //    $(divTextos).append('<label><b>Descripción: </b>Sin datos</b></label>');
    //}

    //Cpc
    let cpc = "";
    if (
      "DomicilioCpcNombre" in requerimiento &&
      "DomicilioCpcNumero" in requerimiento &&
      requerimiento.DomicilioCpcNombre != undefined &&
      requerimiento.DomicilioCpcNumero != undefined
    ) {
        $("#contenedor_SeccionUbicacion .cpc").html(
          "<b>CPC: </b>" +
            ("N° " +
              requerimiento.DomicilioCpcNumero +
              " - " +
              requerimiento.DomicilioCpcNombre) +
            "</b>"
        );
    }

    //else {
    //    $(divTextos).append('<label><b>CPC: </b>Sin datos</b></label>');
    //}

    //Barrio
    if (
      "DomicilioBarrioNombre" in requerimiento &&
      requerimiento.DomicilioBarrioNombre != undefined &&
      requerimiento.DomicilioBarrioNombre != ""
    ) {
        $("#contenedor_SeccionUbicacion .barrio").html(
          "<b>Barrio: </b>" + requerimiento.DomicilioBarrioNombre + "</b>"
        );
    }
    //else {
    //    $(divTextos).append('<label><b>Barrio: </b>Sin datos</b></label>');
    //}
}

//Campos Dinamicos
function cargarCamposDinamicosPorRequerimiento() {
    $("#contenedor_CamposDinamicos").hide();

    if (
      requerimiento.CamposDinamicos == undefined ||
      requerimiento.CamposDinamicos.length == 0
    ) {
        return;
    }

    //saco los campos dinamicos sin valor
    let campos = _.filter(requerimiento.CamposDinamicos, function (campo) {
        if (campo.IdTipoCampoPorMotivo == tipoCampo_Selector) {
            return false;
        }

        if (campo.Valor == "" || campo.Valor == undefined || campo.Valor == null) {
            return false;
        }

        return true;
    });

    if (campos.length == 0) {
        return;
    }

    $.each(campos, function (i, c) {
        if (c.Grupo == null) {
            c.Grupo = "Otra información";
        }
    });

    var grupos = _.groupBy(campos, "Grupo");
    var ordenGrupos = [];

    for (var keyGrupo in grupos) {
        if (grupos.hasOwnProperty(keyGrupo)) {
            var grupo = grupos[keyGrupo];
            console.log("grupo no ordenado");
            console.log(grupo);

            console.log("orden grupos");
            var primerOrden = grupo[0].Orden;
            ordenGrupos.push({ Grupo: keyGrupo, Orden: primerOrden });
            console.log(ordenGrupos);
        }
    }

    ordenGrupos = _.sortBy(ordenGrupos, "Orden");

    var html = '<label class="titulo">Información adicional</label>';
    $.each(ordenGrupos, function (index) {
        var g = grupos[ordenGrupos[index].Grupo];
        g = _.sortBy(g, "Orden");
        crearGrupo(g, ordenGrupos[index].Grupo);
    });

    function crearGrupo(grupo, nombreGrupo) {
        html += '<br/><label class="titulo">' + nombreGrupo + "</label><br/>";
        $.each(grupo, function (i, c) {
            if (c.IdTipoCampoPorMotivo == tipoCampo_Selector) {
                return true;
            }

            var valor = c.Valor;
            if (c.IdTipoCampoPorMotivo == tipoCampo_SiNo) {
                valor = valor == "true" ? "Si" : "No";
            }

            html += "<label><b>" + c.Nombre + ": </b></label>" + valor + "<br/>";

            //if (grupo[i].Observaciones != "" && grupo[i].Observaciones != undefined) {
            //    descripciones = descripciones += "<u>" + grupo[i].Nombre + ": " + "</u>" + "<br/>" + grupo[i].Observaciones + "<br/>";
            //}

            //$(contenido).append(divCampo);
        });
    }

    $("#contenedor_CamposDinamicos .contenido").html(html);
    $("#contenedor_CamposDinamicos").show();
}

//Comentarios
function initComentarios() {
    $("#contenedor_Comentarios .verMas").click(function () {
        mostrarTodosLosComentarios();
    });

    $("#btn_AgregarComentario").click(function () {
        agregarComentario();
    });
}

function mostrarTodosLosComentarios() {
    abrirPanelDeslizable("Notas internas");

    var div = $($("#template_ComentariosDetalle").html());
    $(div).attr("id", "contenedor_ComentariosDetalle");
    $("#contenedor_PanelDeslizable .contenedor_Contenido").append(div);
    $("#contenedor_ComentariosDetalle .contenido").empty();

    //Cargo los comentarios
    $.each(requerimiento.Comentarios, function (index, element) {
        var div = crearHtmlComentario(element);
        $("#contenedor_ComentariosDetalle").append(div);
    });

    //BotonAgregar
    $("#contenedor_PanelDeslizable .btnNuevoComentario").click(function () {
        let comentario = $("#contenedor_PanelDeslizable input").val();
        if (comentario == undefined || comentario == "") {
            mostrarMensaje("Alerta", "Ingrese el contenido de la nota interna");
            $("#contenedor_PanelDeslizable input").focus();
            return;
        }

        procesarAgregarComentario(comentario);
    });
}

function crearHtmlComentario(data) {
    var div = $("#template_Comentario").html();
    div = $(div);

    let foto;
    if (data.UsuarioIdentificadorFotoPersonal != undefined) {
        foto =
          top.urlCordobaFiles +
          "/Archivo/" +
          data.UsuarioIdentificadorFotoPersonal +
          "/3";
    } else {
        foto =
          data.UsuarioSexoMasculino == true
            ? PATH_IMAGEN_USER_MALE
            : PATH_IMAGEN_USER_FEMALE;
    }
    //$(div).find('.persona > .foto').attr('src', foto);
    $(div)
      .find(".persona > .foto")
      .css("background-image", "url(" + foto + ")");

    $(div)
      .find(".persona label")
      .text(data.UsuarioNombre + " " + data.UsuarioApellido);
    $(div)
      .find(".card .contenido")
      .text(data.Observaciones);
    $(div)
      .find(".card .fecha")
      .text(dateTimeToString(data.Fecha));

    $(div)
      .find(".persona label, .persona > .foto")
      .click(function () {
          crearDialogoUsuarioDetalle({
              Id: data.UsuarioId
          });
      });

    return div;
}

function cargarComentarios() {
    $("#contenedor_Comentarios .contenido .items").empty();

    if (
      requerimiento.Comentarios == undefined ||
      requerimiento.Comentarios.length == 0
    ) {
        $("#contenedor_Comentarios .sinItems").show();
        $("#contenedor_Comentarios .verMas").hide();
        $("#contenedor_Comentarios .contenido").hide();
    } else {
        $("#contenedor_Comentarios .sinItems").hide();
        $("#contenedor_Comentarios .contenido").show();

        $.each(requerimiento.Comentarios, function (index, element) {
            if (index < 3) {
                var div = crearHtmlComentario(element);
                $("#contenedor_Comentarios .contenido .items").append(div);
            }
        });

        if (
          $("#contenedor_Comentarios .items").height() >
          $("#contenedor_Comentarios .contenido")
        ) {
            $("#contenedor_Comentarios .verMas").hide();
        } else {
            $("#contenedor_Comentarios .verMas").show();
        }
    }
}

//Usuario Referente
function initUsuariosReferentes() {
    $("#contenedor_UsuarioReferente .verMas").click(function () {
        mostrarTodosUsuariosReferentes();
    });
}

function cargarUsuariosReferentes() {
    if (requerimiento.UsuariosReferentes.length == 0) {
        $("#contenedor_UsuarioReferente").hide();
        return;
    }

    $("#contenedor_UsuarioReferente").show();

    $("#contenedor_UsuarioReferente .contenido .items").empty();

    $("#contenedor_UsuarioReferente .contenido").show();

    let cantidadUsers = requerimiento.UsuariosReferentes.length;
    $.each(requerimiento.UsuariosReferentes, function (index, element) {
        let divsPersonas = crearHtmlUsuarioReferente(element);
        cantidadUsers--;

        $("#contenedor_UsuarioReferente .contenido .items").append(divsPersonas);
        let total = $("#contenedor_UsuarioReferente .contenido").width();
        let usado = $("#contenedor_UsuarioReferente .items").width();

        let lugarNecesitado = $(divsPersonas).width() * 2;

        if (total - usado < lugarNecesitado) {
            return false;
        }
    });

    if (requerimiento.UsuariosReferentes.length == 1) {
        $("#contenedor_UsuarioReferente")
          .find(".titulo")
          .text("Usuario Referente");
        $("#contenedor_UsuarioReferente")
          .find(".persona")
          .find("label.link")
          .text(
            requerimiento.UsuariosReferentes[0].Nombre +
              " " +
              requerimiento.UsuariosReferentes[0].Apellido
          );

        $("#contenedor_UsuarioReferente")
          .find(".persona")
          .find("label.link")
          .click(function () {
              crearDialogoUsuarioDetalle({
                  Id: requerimiento.UsuariosReferentes[0].Id
              });
          });

        if (requerimiento.UsuariosReferentes[0].Observaciones) {
            $("#contenedor_UsuarioReferente")
              .find(".persona")
              .find(".observaciones")
              .html(
                "<b>Observaciones:<b/> " +
                  requerimiento.UsuariosReferentes[0].Observaciones
              );
        }

        $("#contenedor_UsuarioReferente")
          .find(".persona")
          .css("max-width", "fit-content");
    }
}

function cargarReferenteProvisorio() {
    var hayAlgo = false;

    if (
      requerimiento.ReferenteProvisorioNombre ||
      requerimiento.ReferenteProvisorioApellido
    ) {
        hayAlgo = true;
        $("#contenedor_ReferenteProvisorio")
          .find(".nombre")
          .html(
            "<b>Nombre: </b>" +
              requerimiento.ReferenteProvisorioNombre +
              " " +
              requerimiento.ReferenteProvisorioApellido
          );
    }

    if (requerimiento.ReferenteProvisorioDni) {
        hayAlgo = true;
        $("#contenedor_ReferenteProvisorio")
          .find(".dni")
          .html("<b>DNI: </b>" + requerimiento.ReferenteProvisorioDni);
    }

    let genero = requerimiento.ReferenteProvisorioGeneroMasculino
      ? "Masculino"
      : "Femenino";
    $("#contenedor_ReferenteProvisorio")
      .find(".genero")
      .html("<b>Género: </b>" + genero);

    if (requerimiento.ReferenteProvisorioTelefono) {
        hayAlgo = true;
        $("#contenedor_ReferenteProvisorio")
          .find(".telefono")
          .html("<b>Teléfono: </b>" + requerimiento.ReferenteProvisorioTelefono);
    }

    if (requerimiento.ReferenteProvisorioObservaciones) {
        hayAlgo = true;
        $("#contenedor_ReferenteProvisorio")
          .find(".observaciones")
          .html(
            "<b>Observaciones: </b>" +
              requerimiento.ReferenteProvisorioObservaciones
          );
    }

    if (hayAlgo) {
        $("#contenedor_ReferenteProvisorio ").show();
    }
}

function crearHtmlUsuarioReferente(data) {
    let divPersona = $("#template_UsuarioReferente").html();
    divPersona = $(divPersona);

    let foto;
    if (data.IdentificadorFotoPersonal != undefined) {
        foto =
          top.urlCordobaFiles + "/Archivo/" + data.IdentificadorFotoPersonal + "/3";
    } else {
        foto =
          data.SexoMasculino == true
            ? PATH_IMAGEN_USER_MALE
            : PATH_IMAGEN_USER_FEMALE;
    }

    $(divPersona)
      .find(".persona .foto")
      .css("background-image", "url(" + foto + ")");
    //$(divPersona).find('.persona img').attr('src', foto);

    $(divPersona)
      .find(".tooltipped")
      .attr("data-tooltip", data.Nombre + " " + data.Apellido);

    $(divPersona)
      .find(".persona > .foto")
      .click(function () {
          crearDialogoUsuarioDetalle({
              Id: data.Id
          });
      });

    //Toltip
    $(divPersona)
      .find(".tooltipped")
      .tooltip({ delay: 50 });

    return divPersona;
}

//Todos los usuarios referentes
function mostrarTodosUsuariosReferentes() {
    abrirPanelDeslizable("Usuarios Referentes", function () {
        actualizarDetalle(function () {
            cargarUsuariosReferentes();
            cargarInformacionAdicional();
        });
    });

    var div = $($("#template_UsuariosReferentes").html());
    $(div).attr("id", "contenedor_TablaUsuariosReferentes");
    $("#contenedor_PanelDeslizable .contenedor_Contenido").append(div);
    $(div)
      .find("input.busqueda")
      .prop("id", "input_BusquedaUsuarios");

    $("#contenedor_TablaUsuariosReferentes table").attr(
      "id",
      "tablaUsuariosReferentes"
    );

    var dt = $("#tablaUsuariosReferentes").DataTableUsuario({
        Buscar: true,
        InputBusqueda: "#input_BusquedaUsuarios",
        ColumnaObservaciones: true,
        Orden: [[0, "asc"]],
        BotonBorrar: true,
        CallbackBorrar: function (data, callback) {
            quitarReferente(data, callback);
        },
        BotonBorrarValidar: function (data) {
            if (!validarPermisoRequerimiento(PERMISO_EDITAR_REFERENTE)) {
                mostrarMensaje(
                  "Error",
                  "El requerimiento no se encuentra en un estado válido para realizar ésta accion"
                );
                return false;
            }

            if (requerimiento.UsuariosReferentes.length == 1) {
                mostrarMensaje(
                  "Info",
                  "No se puede eliminar al único referente del requerimiento."
                );
                return false;
            }

            return true;
        }
    });

    //Inicializo los tooltips
    dt.$(".tooltipped").tooltip({ delay: 50 });
    dt.$(".selectMaterialize").material_select();

    //Muevo el indicador y el paginado a mi propio div
    $(div)
      .find(".tabla-footer")
      .empty();
    $(div)
      .find(".dataTables_info")
      .detach()
      .appendTo($(div).find(".tabla-footer"));
    $(div)
      .find(".dataTables_paginate")
      .detach()
      .appendTo($(div).find(".tabla-footer"));
    $(div)
      .find(".dataTables_info")
      .hide();

    //Agregar
    $(div)
      .find(".btn_Nuevo")
      .css(
        "display",
        validarPermisoRequerimiento(PERMISO_EDITAR_REFERENTE) ? "auto" : "none"
      );
    $(div)
      .find(".btn_Nuevo")
      .click(function () {
          agregarReferente();
      });

    //Agrego las filas
    let hDisponible = $(div)
      .find(".tabla-contenedor")
      .height();
    let rows = calcularCantidadRowsDataTable(hDisponible);
    dt.page.len(rows);
    dt.rows.add(requerimiento.UsuariosReferentes).draw();
}

//Ultimo Estado
function initUltimoEstado() {
    $("#contenedor_UltimoEstado .estado .nombrePersona").click(function () {
        crearDialogoUsuarioDetalle({
            Id: requerimiento.EstadoUsuarioId
        });
    });

    $("#btn_VerHistorialEstado").click(function () {
        mostrarHistorialDeEstados();
    });
}

function cargarUltimoEstado() {
    $("#contenedor_UltimoEstado .estado .nombre").text(
      toTitleCase(requerimiento.EstadoNombre)
    );
    $("#contenedor_UltimoEstado .estado .circulo").css(
      "background-color",
      "#" + requerimiento.EstadoColor
    );
    $("#contenedor_UltimoEstado .estado .motivo").text(
      requerimiento.EstadoObservaciones
    );
    $("#contenedor_UltimoEstado .estado .nombrePersona").text(
      toTitleCase(
        requerimiento.EstadoUsuarioNombre +
          " " +
          requerimiento.EstadoUsuarioApellido
      )
    );
    $("#contenedor_UltimoEstado .estado .fecha").html("el <b>" + +"</b>");
    $("#contenedor_UltimoEstado .estado .fecha").html(
      " el <b>" + dateTimeToString(requerimiento.EstadoFecha) + "</b>"
    );
}

//Informacion organica
function initInformacionOrganica() {
    $("#btn_VerInformacionOrgánica").click(function () {
        crearDialogoInformacionOrganicaDetalle({
            Id: requerimiento.AreaId,
            Callback: function () {
                actualizarDetalle(function () {
                    cargarInformacionOrganica();
                });
            }
        });
    });
}

function cargarInformacionOrganica() {
    if (requerimiento.InformacionOrganicaDireccionId != undefined) {
        $("#contenedor_InformacionOrganica .direccion").html(
          "<b>Dirección: </b>" + requerimiento.InformacionOrganicaDireccionNombre
        );
        $("#contenedor_InformacionOrganica .secretaria").html(
          "<b>Secretaría: </b>" + requerimiento.InformacionOrganicaSecretariaNombre
        );
        $("#contenedor_InformacionOrganica .secretaria").show();
        $("#btn_VerInformacionOrgánica").show();
    } else {
        $("#contenedor_InformacionOrganica .direccion").html(
          "Sin datos disponibles"
        );
        $("#contenedor_InformacionOrganica .secretaria").hide();
        $("#btn_VerInformacionOrgánica").hide();
    }
}

//Informacion adicional
function initInformacionAdicional() {
    $("#contenedor_InfoAltaModificacion .textoUsuarioCreador").click(function () {
        crearDialogoUsuarioDetalle({
            Id: requerimiento.UsuarioCreadorId
        });
    });

    $("#contenedor_InfoAltaModificacion .textoUsuarioModificacion").click(
      function () {
          crearDialogoUsuarioDetalle({
              Id: requerimiento.UsuarioModificacionId
          });
      }
    );
}

function cargarInformacionAdicional() {
    $("#contenedor_InfoAltaModificacion .textoFechaCreacion").html(
      "<b>" + dateTimeToString(requerimiento.FechaAlta) + "</b>"
    );

    if (requerimiento.UsuarioCreadorNombre != undefined) {
        $("#contenedor_InfoAltaModificacion .textoUsuarioCreadorConector").show();
        $("#contenedor_InfoAltaModificacion .textoUsuarioCreador").show();
        var usuarioCreador = toTitleCase(
          requerimiento.UsuarioCreadorNombre +
            " " +
            requerimiento.UsuarioCreadorApellido
        ).trim();
        $("#contenedor_InfoAltaModificacion .textoUsuarioCreador").html(
          "<b>" + usuarioCreador + "</b>"
        );
    } else {
        $("#contenedor_InfoAltaModificacion .textoUsuarioCreadorConector").hide();
        $("#contenedor_InfoAltaModificacion .textoUsuarioCreador").hide();
    }

    if (requerimiento.OrigenNombre != undefined) {
        $("#contenedor_InfoAltaModificacion .textoOrigenConector").show();
        $("#contenedor_InfoAltaModificacion .textoOrigen").show();
        $("#contenedor_InfoAltaModificacion .textoOrigen").html(
          "<b>" + toTitleCase(requerimiento.OrigenNombre) + "</b>"
        );
    } else {
        $("#contenedor_InfoAltaModificacion .textoOrigenConector").hide();
        $("#contenedor_InfoAltaModificacion .textoOrigen").hide();
    }

    if (requerimiento.FechaModificacion != undefined) {
        $("#contenedor_InfoAltaModificacion .linea2").show();
        $("#contenedor_InfoAltaModificacion .textoFechaModificacion").html(
          "<b>" + dateTimeToString(requerimiento.FechaModificacion) + "</b>"
        );
        if (
          requerimiento.UsuarioModificacionNombre != undefined &&
          requerimiento.UsuarioModificacionNombre.trim() != ""
        ) {
            $(
              "#contenedor_InfoAltaModificacion .textoUsuarioModificacionConector"
            ).show();
            $("#contenedor_InfoAltaModificacion .textoUsuarioModificacion").show();
            $("#contenedor_InfoAltaModificacion .textoUsuarioModificacion").html(
              "<b>" +
                toTitleCase(
                  requerimiento.UsuarioModificacionNombre +
                    " " +
                    requerimiento.UsuarioModificacionApellido
                ) +
                "</b>"
            );
        } else {
            $(
              "#contenedor_InfoAltaModificacion .textoUsuarioModificacionConector"
            ).hide();
            $("#contenedor_InfoAltaModificacion .textoUsuarioModificacion").hide();
        }
    } else {
        $("#contenedor_InfoAltaModificacion .linea2").hide();
    }
}

//Adjuntos
function initAdjuntos() {
    $("#btn_Fotos").click(function () {
        mostrarFotos();
    });

    $("#input_Imagen").change(function (e) {
        if (this.files == undefined) return;
        if (this.files.length == 0) return;

        var file = this.files[0];

        mostrarCargando(true);
        procesarAjunto(
          file,
          true,
          function (doc) {
              crearAjax({
                  Url: ResolveUrl(
                    "~/Servicios/RequerimientoService.asmx/AgregarArchivo"
                  ),
                  Data: { id: requerimiento.Id, comando: doc },
                  OnSuccess: function (result) {
                      if (!result.Ok) {
                          mostrarCargando(false);
                          mostrarMensaje("Error", result.Error);
                          return;
                      }

                      mostrarMensaje("Exito", "Imagen agregada correctamente");

                      actualizarDetalle(function () {
                          cargarAdjuntos();

                          if ($("#contenedor_PanelDeslizable").hasClass("visible")) {
                              buscarFotos();
                          }
                      });
                  },
                  OnError: function (result) {
                      mostrarCargando(false);
                      mostrarMensaje("Error", "Error procesando la solicitud");
                  }
              });
          },
          function (error) {
              mostrarCargando(false);
              mostrarMensaje("Error", error);
          }
        );
    });

    $("#btn_Documentos").click(function () {
        mostrarDocumentos();
    });

    $("#input_Documento").change(function (e) {
        if (this.files == undefined) return;
        if (this.files.length == 0) return;

        var file = this.files[0];

        mostrarCargando(true);
        procesarAjunto(
          file,
          false,
          function (doc) {
              crearAjax({
                  Url: ResolveUrl(
                    "~/Servicios/RequerimientoService.asmx/AgregarArchivo"
                  ),
                  Data: { id: requerimiento.Id, comando: doc },
                  OnSuccess: function (result) {
                      if (!result.Ok) {
                          mostrarCargando(false);
                          mostrarMensaje("Error", result.Error);
                          return;
                      }

                      actualizarDetalle(function () {
                          cargarAdjuntos();

                          if ($("#contenedor_PanelDeslizable").hasClass("visible")) {
                              buscarDocumentos();
                          }
                      });
                  },
                  OnError: function (result) {
                      mostrarCargando(false);
                      mostrarMensaje("Error", "Error procesando la solicitud");
                  }
              });
          },
          function (error) {
              mostrarCargando(false);
              mostrarMensaje("Error", error);
          }
        );
    });
}

function procesarAjunto(file, esImagen, callback, callbackError) {
    if (!validarExtension(file, esImagen)) {
        if (esImagen) {
            callbackError(
              "La imagen " +
                file.name +
                " no tiene un formato soportado. Formatos soportados: " +
                IMAGE_EXTENSIONS.join(", ")
            );
        } else {
            callbackError(
              "El documento " +
                file.name +
                " no tiene un formato soportado. Formatos soportados: " +
                DOCUMENT_EXTENSIONS.join(", ")
            );
        }
        return;
    }

    if (!validarSize(file, esImagen)) {
        if (esImagen) {
            callbackError(
              "La imgane " +
                file.name +
                " es muy grande. Tamaño maximo soportado: " +
                IMAGE_SIZE +
                " [Mb]"
            );
        } else {
            callbackError(
              "El documento " +
                file.name +
                " es muy grande. Tamaño maximo soportado: " +
                DOCUMENT_SIZE +
                " [Mb]"
            );
        }
        return;
    }

    try {
        var extension = file.name
          .split(".")
          .pop()
          .toLowerCase();
        //Creo la entidad
        var obj = {
            Nombre: file.name,
            Extension: extension,
            Tipo: esImagen ? 2 : 1
        };
        console.log(obj);

        //Abro el archivo
        var fr = new FileReader();
        fr.onload = function (e) {
            try {
                obj.Data = e.target.result;
                callback(obj);
            } catch (e) {
                callbackError("Error procesando la solicitud");
            }
        };
        fr.readAsDataURL(file);
    } catch (e) {
        callbackError("Error procesando la solicitud");
    }

    function validarExtension(file, esImagen) {
        if (file == undefined) return false;
        var extension = file.name
          .split(".")
          .pop()
          .toLowerCase();
        return esImagen
          ? IMAGE_EXTENSIONS.indexOf(extension) > -1
          : DOCUMENT_EXTENSIONS.indexOf(extension) > -1;
    }

    function validarSize(file, esImagen) {
        if (file == undefined) return false;
        return file.size / 1048576 < (esImagen ? IMAGE_SIZE : DOCUMENT_SIZE);
    }
}

function verFoto(fotos, index) {
    var pswpElement = document.querySelectorAll(".pswp")[0];

    // build items array
    var items = [];
    $.each(fotos, function (index, element) {
        items.push({
            src: element.Url,
            w: element.Width,
            h: element.Height,
            msrc: element.UrlPreview
        });
    });

    // define options (if needed)
    var options = {
        index: index,
        clickToCloseNonZoomable: false,
        history: false,
        focus: false,
        showHideOpacity: true,
        getThumbBoundsFn: function (index) {
            let element = $(
              "#contenedor_Fotos > div.contenido > div:nth-child(" +
                (index + 2) +
                ") > div.foto"
            );
            return {
                x: $(element).offset().left,
                y: $(element).offset().top,
                w: $(element).width()
            };
        }
    };

    // Initializes and opens PhotoSwipe
    var gallery = new PhotoSwipe(
      pswpElement,
      PhotoSwipeUI_Default,
      items,
      options
    );
    gallery.init();
}

function mostrarFotos() {
    abrirPanelDeslizable("Fotos adjuntas");

    var div = $($("#template_Documentos").html());
    $(div).attr("id", "contenedor_Fotos");
    $("#contenedor_PanelDeslizable .contenedor_Contenido").append(div);
    buscarFotos();

    $("#contenedor_Fotos .btn_Reintentar").click(function () {
        buscarFotos();
    });

    $("#contenedor_Fotos .indicador_Vacio .btn").click(function () {
        agregarImagen();
    });
}

function buscarFotos() {
    $("#contenedor_Fotos .cargando").addClass("visible");
    $("#contenedor_Fotos .error").removeClass("visible");
    $("#contenedor_Fotos .indicador_Vacio").removeClass("visible");
    $("#contenedor_Fotos .contenido").empty();
    crearAjax({
        Url: ResolveUrl("~/Servicios/RequerimientoService.asmx/GetImagenes"),
        Data: { id: requerimiento.Id },
        OnSuccess: function (result) {
            $("#contenedor_Fotos .cargando").removeClass("visible");
            if (!result.Ok) {
                $("#contenedor_Fotos .error").addClass("visible");
                return;
            }

            if (result.Return.length == 0) {
                $("#contenedor_Fotos .indicador_Vacio").addClass("visible");
                $("#contenedor_Fotos .indicador_Vacio i").text("photo_library");
                $("#contenedor_Fotos .indicador_Vacio .mensaje").text(
                  "El requerimiento no tiene fotos adjuntas"
                );
                $("#contenedor_Fotos .indicador_Vacio .btn").text("Adjuntar foto");

                return;
            }

            var fotos = [];
            fotos.push({
                Boton: true
            });

            $.each(result.Return, function (index, element) {
                if (element == undefined) {
                    let foto = {
                        Boton: false,
                        Error: true
                    };
                    fotos.push(foto);
                } else {
                    let foto = {
                        Boton: false,
                        Id: element.Id,
                        Url: element.Url,
                        UrlPreview: element.UrlPreview,
                        Nombre: element.Nombre,
                        Identificador: element.Identificador,
                        Width: element.Width,
                        Height: element.Height,
                        UsuarioReferenteNombre: element.UsuarioReferenteNombre,
                        UsuarioReferenteApellido: element.UsuarioReferenteApellido,
                        UsuarioReferenteId: element.UsuarioReferenteId
                    };
                    fotos.push(foto);
                }
            });

            let fotosSolas = $.grep(fotos, function (element) {
                return element.Boton == false;
            });

            $.each(fotos, function (index, element) {
                let div;
                if (element.Boton) {
                    div = getHtmlBotonAgregarFoto();
                } else {
                    div = getHtmlPreviewFoto(element, fotosSolas, index - 1);
                }
                $("#contenedor_Fotos .contenido").append(div);
            });
        },
        OnError: function (result) {
            $("#contenedor_Fotos .cargando").removeClass("visible");
            $("#contenedor_Fotos .error").addClass("visible");
        }
    });
}

function getHtmlPreviewFoto(element, fotos, index) {
    let div = $($("#template_FotoPreview").html());
    let error = false;
    if ("Error" in element && element.Error == true) {
        $(div)
          .find(".nombre")
          .text("Error en la imagen");
        $(div)
          .find(".foto")
          .css("background-image", 'url("' + PATH_IMAGEN_ERROR + '")');
        error = true;
    } else {
        $(div)
          .find(".nombre")
          .text(element.Nombre);
        $(div)
          .find(".foto")
          .css("background-image", 'url("' + element.UrlPreview + '")');
        if (element.UsuarioReferenteId != null) {
            $(div)
              .find(".subidaPor")
              .html(
                "Subida por <u><b>" +
                  element.UsuarioReferenteNombre +
                  " " +
                  element.UsuarioReferenteApellido +
                  "</b></u>"
              );
            $(div)
              .find(".subidaPor b")
              .click(function () {
                  crearDialogoUsuarioDetalle({
                      Id: element.UsuarioReferenteId
                  });
              });
        }

        $(div)
          .find(".foto")
          .click(function () {
              verFoto(fotos, index);
          });
    }

    $(div)
      .find(".btn-flat")
      .click(function (e) {
          e.preventDefault();
          e.stopPropagation();

          //Menu general
          $($(div).find(".btn-flat")).MenuFlotante({
              PosicionX: "izquierda",
              PosicionY: "arriba",
              Menu: [
                {
                    Texto: "Abrir",
                    Icono: "open_with",
                    Visible: !error,
                    OnClick: function () {
                        $(div).trigger("click");
                    }
                },
                {
                    Texto: "Descargar",
                    Icono: "file_download",
                    Visible: !error,
                    OnClick: function () {
                        var win = top.window.open(element.Url, "_blank");
                        if (win) {
                            //Browser has allowed it to be opened
                            win.focus();
                        } else {
                            //Browser has blocked it
                            alert("Please allow popups for this website");
                        }
                    }
                },
                {
                    Texto: "Borrar",
                    Icono: "delete",
                    OnClick: function () {
                        crearDialogoConfirmacion({
                            Texto: "¿Esta seguro de querer borrar la imagen?",
                            ClassBotonAceptar: "colorError",
                            CallbackPositivo: function () {
                                quitarArchivo(element.Id);
                            }
                        });
                    }
                }
              ]
          });
      });

    return div;
}

function getHtmlBotonAgregarFoto() {
    var div = $($("#template_FotoPreview").html());
    $(div).addClass("boton");

    $(div)
      .find(".foto")
      .css("background-image", 'url("' + PATH_IMAGEN_ICON_ADD + '")');
    $(div)
      .find(".nombre")
      .text("Agregar imagen");

    $(div).click(function () {
        agregarImagen();
    });
    return div;
}

function mostrarDocumentos() {
    abrirPanelDeslizable("Documentos adjuntos");

    var div = $($("#template_Documentos").html());
    $(div).attr("id", "contenedor_Documentos");
    $("#contenedor_PanelDeslizable .contenedor_Contenido").append(div);
    buscarDocumentos();

    $("#contenedor_Documentos .btn_Reintentar").click(function () {
        buscarDocumentos();
    });

    $("#contenedor_Documentos .indicador_Vacio .btn").click(function () {
        agregarDocumento();
    });
}

function buscarDocumentos() {
    $("#contenedor_Documentos .cargando").addClass("visible");
    $("#contenedor_Documentos .error").removeClass("visible");
    $("#contenedor_Documentos .indicador_Vacio").removeClass("visible");
    $("#contenedor_Documentos .contenido").empty();

    crearAjax({
        Url: ResolveUrl("~/Servicios/RequerimientoService.asmx/GetDocumentos"),
        Data: { id: requerimiento.Id },
        OnSuccess: function (result) {
            $("#contenedor_Documentos .cargando").removeClass("visible");
            if (!result.Ok) {
                $("#contenedor_Documentos .error").addClass("visible");
                return;
            }

            if (result.Return.length == 0) {
                $("#contenedor_Documentos .indicador_Vacio").addClass("visible");
                $("#contenedor_Documentos .indicador_Vacio i").text("attach_file");
                $("#contenedor_Documentos .indicador_Vacio .mensaje").text(
                  "El requerimiento no tiene documentos adjuntos"
                );
                $("#contenedor_Documentos .indicador_Vacio .btn").text(
                  "Adjuntar documento"
                );
                return;
            }

            let documentos = [];
            documentos.push({
                Boton: true
            });
            $.each(result.Return, function (index, element) {
                documentos.push(element);
            });

            $.each(documentos, function (index, element) {
                let div;
                if (element.Boton) {
                    div = getHtmlBotonAgregarDocumento();
                } else {
                    div = getHtmlPreviewDocumento(element);
                }
                $("#contenedor_Documentos .contenido").append(div);
            });
        },
        OnError: function (result) {
            $("#contenedor_Documentos .cargando").removeClass("visible");
            $("#contenedor_Documentos .error").addClass("visible");
        }
    });
}

function getHtmlPreviewDocumento(data) {
    var div = $($("#template_FotoPreview").html());
    $(div)
      .find(".nombre")
      .text(data.Nombre);

    let icon = "file_otros.png";
    let sePuedeAbrir = false;

    switch (data.ContentType) {
        case "application/pdf":
            {
                sePuedeAbrir = true;
                icon = "file_pdf.png";
            }
            break;

        case "application/msword":
        case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
        case "text/plain":
            {
                icon = "file_doc.png";
            }
            break;

        case "application/vnd.ms-excel":
        case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
            {
                icon = "file_xls.png";
            }
            break;

        case "application/vnd.ms-powerpoint":
        case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
            {
                icon = "file_ppt.png";
            }
            break;
    }

    $(div)
      .find(".foto")
      .css(
        "background-image",
        "url(" + ResolveUrl("~/Resources/Imagenes/" + icon + ")")
      );
    $(div)
      .find(".foto")
      .css("background-size", "80% 80%");
    $(div)
      .find(".foto")
      .css("background-position", "center");
    $(div)
      .find(".foto")
      .css("background-repeat", "no-repeat");
    if (data.UsuarioReferenteId != null) {
        $(div)
          .find(".subidaPor")
          .html(
            "Subida por <u><b>" +
              data.UsuarioReferenteNombre +
              " " +
              data.UsuarioReferenteApellido +
              "</b></u>"
          );
        $(div)
          .find(".subidaPor b")
          .click(function () {
              crearDialogoUsuarioDetalle({
                  Id: data.UsuarioReferenteId
              });
          });
    }

    //Click en el elemento
    $(div)
      .find(".foto")
      .click(function (e) {
          abrir(e);
      });

    function abrir(e) {
        if (e != undefined) {
            e.preventDefault(); //stop the browser from following
        }

        var win = top.window.open(data.Url, "_blank");
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            alert("Please allow popups for this website");
        }
    }

    //Menu
    $(div)
      .find(".btn-flat")
      .click(function (e) {
          e.preventDefault();
          e.stopPropagation();

          //Menu general
          $($(div).find(".btn-flat")).MenuFlotante({
              PosicionX: "izquierda",
              PosicionY: "arriba",
              Menu: [
                {
                    Texto: "Descargar",
                    Icono: "file_download",
                    OnClick: function () {
                        abrir();
                    }
                },
                {
                    Texto: "Borrar",
                    Icono: "delete",
                    OnClick: function () {
                        crearDialogoConfirmacion({
                            Texto: "¿Esta seguro de querer borrar la imagen?",
                            ClassBotonAceptar: "colorError",
                            CallbackPositivo: function () {
                                quitarArchivo(data.Id);
                            }
                        });
                    }
                }
              ]
          });
      });

    return div;
}

function getHtmlBotonAgregarDocumento() {
    var div = $($("#template_FotoPreview").html());
    $(div).addClass("boton");
    $(div)
      .find(".foto")
      .css("background-image", 'url("' + PATH_IMAGEN_ICON_ADD + '")');
    $(div)
      .find(".nombre")
      .text("Agregar documento");

    $(div).click(function () {
        agregarDocumento();
    });
    return div;
}

function cargarAdjuntos() {
    $("#texto_CantidadFotos").text(requerimiento.CantidadFotos);
    $("#texto_CantidadDocumentos").text(requerimiento.CantidadDocumentos);
}

//Historico ordenes de trabajo
function mostrarHistoricoOrdenesDeTrabajo() {
    abrirPanelDeslizable("Histórico de Ordenes de Trabajo");

    var div = $($("#template_HistoricoOrdenesTrabajo").html());
    $(div).attr("id", "contenedor_HistoricoOrdenesTrabajo");
    $("#contenedor_PanelDeslizable .contenedor_Contenido").append(div);

    $("#contenedor_HistoricoOrdenesTrabajo table").attr(
      "id",
      "tablaHistoricoOrdenesTrabajo"
    );

    var dt = $("#tablaHistoricoOrdenesTrabajo").DataTableGeneral({
        Orden: [[3, "desc"]],
        Paginar: false,
        VerInfo: false,
        Columnas: [
          {
              sTitle: "Estado",
              render: function (data, type, row) {
                  return (
                    '<div><i class="material-icons tooltipped "  style="color: #' +
                    row.EstadoColor +
                    '" data-position="bottom" data-delay="50" data-tooltip="' +
                    toTitleCase(row.EstadoNombre) +
                    '">swap_vertical_circle</i> </div>'
                  );
              }
          },
          {
              sTitle: "Número",
              render: function (data, type, row) {
                  return "<div><span>" + row.Numero + "/" + row.Año + "</span></div>";
              }
          },
          {
              sTitle: "Fecha Alta",
              mData: "FechaAlta",
              render: function (data, type, row) {
                  return "<div><span>" + dateTimeToString(data) + "</span></div>";
              }
          },
          {
              sTitle: "Fecha de Cierre",
              mData: "FechaCierre",
              render: function (data, type, row) {
                  return "<div><span>" + dateTimeToString(data) + "</span></div>";
              }
          },
          {
              sTitle: "Area",
              mData: "AreaNombre",
              render: function (data, type, row) {
                  return "<div><span>" + toTitleCase(data) + "</span></div>";
              }
          }
        ]
    });

    //Inicializo los tooltips
    dt.$(".tooltipped").tooltip({ delay: 50 });
    dt.rows.add(requerimiento.OrdenesDeTrabajo).draw();
}

//Acciones
function cargarAcciones() {
    $("#contenedor_Acciones .contenido").empty();

    //Marcar
    if (requerimiento.Marcado == true) {
        agregarAccion({
            Texto: "Enviar al Area Op.",
            Icono: "check_circle",
            Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_MARCADO),
            OnClick: function () {
                desmarcar();
            }
        });
    } else {
        agregarAccion({
            Texto: "Enviar a CPC",
            Icono: "check_circle",
            Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_MARCADO),
            OnClick: function () {
                marcar();
            }
        });
    }

    //Motivo
    agregarAccion({
        Texto: "Cambiar motivo",
        Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_MOTIVO),
        Icono: "edit",
        OnClick: function () {
            cambiarMotivo();
        }
    });

    //Estado
    agregarAccion({
        Texto: "Cambiar estado",
        Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_ESTADO),
        Icono: "swap_vert",
        OnClick: function () {
            cambiarEstado();
        }
    });

    //Historico
    agregarAccion({
        Texto: "Historial de estados",
        Icono: "history",
        OnClick: function () {
            mostrarHistorialDeEstados();
        }
    });

    //Ubicacion
    agregarAccion({
        Texto: "Cambiar ubicación",
        Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_UBICACION),
        Icono: "location_on",
        OnClick: function () {
            cambiarUbicacion();
        }
    });

    //Favorito
    if (requerimiento.Favorito) {
        agregarAccion({
            Texto: "Quitar favorito",
            Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_FAVORITO),
            Icono: "star",
            OnClick: function () {
                cambiarFavorito(false);
            }
        });
    } else {
        agregarAccion({
            Texto: "Agregar favorito",
            Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_FAVORITO),
            Icono: "star",
            OnClick: function () {
                cambiarFavorito(true);
            }
        });
    }

    //Referente
    agregarAccion({
        Texto: "Agregar referente",
        Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_REFERENTE),
        Icono: "person",
        OnClick: function () {
            agregarReferente();
        }
    });

    if (requerimiento.MotivoTipo == 3) {
        //Referente
        agregarAccion({
            Texto: "Editar referente provisorio",
            Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_REFERENTE),
            Icono: "person",
            OnClick: function () {
                editarReferenteProvisorio();
            }
        });
    }

    //Nota interna
    agregarAccion({
        Texto: "Agregar nota interna",
        Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_COMENTARIOS),
        Icono: "comment",
        OnClick: function () {
            agregarComentario();
        }
    });

    if (tieneTareas) {
        agregarAccion({
            Texto: "Editar tareas",
            Permiso: validarPermisoRequerimiento(PERMISO_AGREGAR_TAREAS),
            IconoMdi: "hammer",
            OnClick: function () {
                abrirPanelTareas();
            }
        });
    }

    //Archivos
    agregarAccion({
        Texto: "Insertar adjunto",
        PermisoKeyValue: PERMISO_EDITAR_DOCUMENTOS,
        Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_DOCUMENTOS),
        Icono: "attach_file",
        OnClick: function () {
            let menuPrioridad = [];
            menuPrioridad.push({
                Texto: "Imagen",
                Icono: "photo_library",
                OnClick: function () {
                    agregarImagen();
                }
            });
            menuPrioridad.push({
                Texto: "Documento",
                Icono: "attach_file",
                OnClick: function () {
                    agregarDocumento();
                }
            });

            $(".accion[permiso=" + PERMISO_EDITAR_DOCUMENTOS + "]").MenuFlotante({
                PosicionX: "izquierda",
                PosicionY: "abajo",
                Menu: menuPrioridad
            });
        }
    });

    //agregarAccion({
    //    Texto: 'Agregar documento',
    //    Icono: 'attach_file',
    //    OnClick: function () {
    //        agregarDocumento();
    //    }
    //});

    //Prioridad
    agregarAccion({
        Texto: "Cambiar prioridad",
        PermisoKeyValue: PERMISO_EDITAR_PRIORIDAD,
        Permiso: validarPermisoRequerimiento(PERMISO_EDITAR_PRIORIDAD),
        Icono: "flag",
        OnClick: function () {
            let menuPrioridad = [];
            menuPrioridad.push({
                Class: "colorIconoPrioridadNormal",
                Texto: "Prioridad baja",
                Icono: "flag",
                OnClick: function () {
                    cambiarPrioridad(1);
                }
            });
            menuPrioridad.push({
                Class: "colorIconoPrioridadMedia",
                Texto: "Prioridad media",
                Icono: "flag",
                OnClick: function () {
                    cambiarPrioridad(2);
                }
            });
            menuPrioridad.push({
                Class: "colorIconoPrioridadAlta",
                Texto: "Prioridad alta",
                Icono: "flag",
                OnClick: function () {
                    cambiarPrioridad(3);
                }
            });
            $(".accion[permiso=" + PERMISO_EDITAR_PRIORIDAD + "]").MenuFlotante({
                PosicionX: "izquierda",
                PosicionY: "abajo",
                Menu: menuPrioridad
            });
        }
    });

    //Reenviar comprobante
    agregarAccion({
        Texto: "Reenviar comprobante",
        Icono: "mail",
        OnClick: function () {
            crearDialogoRequerimientoReenviarComprobante({
                Id: requerimiento.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    mostrarMensaje(tipo, mensaje);
                },
                CallbackCargando: function (cargando, mensaje) {
                    mostrarCargando(cargando, mensaje);
                }
            });
        }
    });

    //Enviar mensaje
    agregarAccion({
        Texto: "Enviar mensaje a vecino/s",
        Icono: "mail",
        OnClick: function () {
            crearDialogoRequerimientoMailContacto({
                Id: requerimiento.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    mostrarMensaje(tipo, mensaje);
                },
                CallbackCargando: function (cargando, mensaje) {
                    mostrarCargando(cargando, mensaje);
                }
            });
        }
    });

    if (tieneCamposDinamicos) {
        //Editar campos dinámicos
        agregarAccion({
            Texto: "Editar información adicional",
            Icono: "subject",
            OnClick: function () {
                crearDialogoRequerimientoEditarCamposDinamicos({
                    Id: requerimiento.Id,
                    IdMotivo: requerimiento.MotivoId,
                    CallbackMensajes: function (tipo, mensaje) {
                        mostrarMensaje(tipo, mensaje);
                    },
                    CallbackCargando: function (cargando, mensaje) {
                        mostrarCargando(cargando, mensaje);
                    },
                    Callback: function () {
                        actualizarDetalle(function () {
                            cargarCamposDinamicosPorRequerimiento();
                            cargarInformacionAdicional();
                        });
                    }
                });
            }
        });
    }

    //Imprimir
    agregarAccion({
        Texto: "Imprimir",
        PermisoKeyValue: -10,
        Icono: "print",
        OnClick: function () {
            let menuPrioridad = [];
            menuPrioridad.push({
                Texto: "Imprimir con mapa",
                Icono: "print",
                OnClick: function () {
                    imprimirConMapa();
                }
            });
            menuPrioridad.push({
                Texto: "Imprimir sin mapa",
                Icono: "print",
                OnClick: function () {
                    imprimirSinMapa();
                }
            });
            $(".accion[permiso=-10]").MenuFlotante({
                PosicionX: "izquierda",
                PosicionY: "abajo",
                Menu: menuPrioridad
            });
        }
    });
}

function agregarAccion(valores) {
    var div = $($("#template_Accion").html());
    $(div)
      .find(".texto")
      .text(valores.Texto);

    if (valores.IconoMdi != undefined) {
        $(div)
          .find(".icono")
          .addClass("mdi mdi-" + valores.IconoMdi);
    } else {
        $(div)
          .find(".icono")
          .text(valores.Icono);
    }

    $(div).attr("permiso", valores.PermisoKeyValue);
    $("#contenedor_Acciones .contenido").append(div);
    if ("Permiso" in valores && !valores.Permiso) {
        $(div).addClass("deshabilitado");
    }
    $(div).click(function () {
        valores.OnClick();
    });
}

function expandirMapa() {
    mapaExpandido = true;
    $("#main").addClass("mapaExpandido");
}

function achicarMapa() {
    mapaExpandido = false;
    $("#main").removeClass("mapaExpandido");
}

function centrarMapa() {
    let pos = {
        lat: parseFloat(requerimiento.DomicilioLatitud.replace(",", ".")),
        lng: parseFloat(requerimiento.DomicilioLongitud.replace(",", "."))
    };
    map.setZoom(15);
    map.setCenter(pos);
}

function toggleFavorito() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_FAVORITO)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    let url = ResolveUrl("~/Servicios/RequerimientoService.asmx/ToggleFavorito");
    let data = { id: requerimiento.Id };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
                return;
            }

            actualizarDetalle(function () {
                if (requerimiento.Favorito) {
                    mostrarMensaje("Exito", "Requerimiento marcado como favorito");
                } else {
                    mostrarMensaje("Exito", "Requerimiento quitado de favoritos");
                }

                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function cambiarFavorito(favorito) {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_FAVORITO)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    let url;
    if (favorito) {
        url = ResolveUrl("~/Servicios/RequerimientoService.asmx/SetFavorito");
    } else {
        url = ResolveUrl("~/Servicios/RequerimientoService.asmx/SetNoFavorito");
    }

    var data = { id: requerimiento.Id };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
                return;
            }

            actualizarDetalle(function () {
                if (requerimiento.Favorito) {
                    mostrarMensaje("Exito", "Requerimiento marcado como favorito");
                } else {
                    mostrarMensaje("Exito", "Requerimiento quitado de favoritos");
                }

                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function togglePrioridad() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_PRIORIDAD)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    var url = ResolveUrl("~/Servicios/RequerimientoService.asmx/TogglePrioridad");
    var data = { id: requerimiento.Id };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
                return;
            }

            mostrarMensaje("Exito", "Prioridad editada");

            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function cambiarPrioridad(prioridad) {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_PRIORIDAD)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    var url = ResolveUrl("~/Servicios/RequerimientoService.asmx/SetPrioridad");
    var data = { id: requerimiento.Id, prioridad: prioridad };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
                return;
            }

            mostrarMensaje("Exito", "Prioridad editada");
            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function toggleMarcado() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_MARCADO)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    var url = ResolveUrl("~/Servicios/RequerimientoService.asmx/ToggleMarcado");
    var data = { id: requerimiento.Id };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                return;
            }

            actualizarDetalle(function () {
                if (requerimiento.Marcado) {
                    mostrarMensaje("Exito", "Requerimiento marcado");
                } else {
                    mostrarMensaje("Exito", "Requerimiento desmarcado");
                }

                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function marcar() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_MARCADO)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    var url = ResolveUrl("~/Servicios/RequerimientoService.asmx/Marcar");
    var data = { id: requerimiento.Id };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                return;
            }

            mostrarMensaje("Exito", "Requerimiento marcado");

            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function desmarcar() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_MARCADO)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    var url = ResolveUrl("~/Servicios/RequerimientoService.asmx/Desmarcar");
    var data = { id: requerimiento.Id };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                return;
            }

            mostrarMensaje("Exito", "Requerimiento desmarcado");

            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function cambiarEstado() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_ESTADO)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    crearDialogoRequerimientoCambiarEstado({
        Id: requerimiento.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            //Ya se notifica desde el Dialogo
            // mostrarMensaje('Exito', 'Estado del requerimiento modificado');
            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarAlertas();
                cargarUltimoEstado();
                cargarInformacionAdicional();
            });
        }
    });
}

function cancelar() {
    if (!validarPermisoRequerimiento(PERMISO_CANCELAR)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    crearDialogoRequerimientoCancelar({
        Id: requerimiento.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            mostrarMensaje("Exito", "Requerimiento cancelado");

            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarAlertas();
                cargarUltimoEstado();
                cargarInformacionAdicional();
            });
        }
    });
}

function editarReferenteProvisorio() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_REFERENTE)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar ésta accion"
        );
        return;
    }

    crearDialogoRequerimientoEditarReferenteProvisorio({
        Id: requerimiento.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarUsuariosReferentes();
                cargarReferenteProvisorio();
                cargarInformacionAdicional();
            });
        }
    });
}

function agregarReferente() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_REFERENTE)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar ésta accion"
        );
        return;
    }

    crearDialogoRequerimientoAgregarReferente({
        Id: requerimiento.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarUsuariosReferentes();
                cargarInformacionAdicional();

                if ($("#contenedor_PanelDeslizable").hasClass("visible")) {
                    mostrarTodosUsuariosReferentes();
                }
            });
        }
    });
}

function quitarReferente(data, callback) {
    crearDialogoConfirmacion({
        Texto: "¿Está seguro de querer eliminar al referente?",
        ClassBotonAceptar: "colorError",
        CallbackPositivo: function () {
            crearAjax({
                Url: ResolveUrl(
                  "~/Servicios/RequerimientoService.asmx/QuitarReferente"
                ),
                Data: { id: requerimiento.Id, idUsuario: data.Id },
                OnSuccess: function (result) {
                    //Oculto el cargando
                    mostrarCargando(false);

                    //algo salio mal
                    if (!result.Ok) {
                        mostrarMensaje(result.Error);
                        return;
                    }

                    callback(data.Id);
                    informar(result.Return);
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje("Error", "Error procesando la solicitud");
                }
            });
        }
    });
}

function cambiarUbicacion() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_UBICACION)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    crearDialogoUbicacionSelector({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackCargando: function (mostrar, mensaje) {
            mostrarCargando(mostrar, mensaje);
        },
        Callback: function (u) {
            let comando = {
                Latitud: parseFloat(u.Latitud.replace(",", ".")),
                Longitud: parseFloat(u.Longitud.replace(",", ".")),
                Direccion: u.Direccion,
                Observaciones: u.Observaciones
            };

            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl(
                  "~/Servicios/RequerimientoService.asmx/CambiarDomicilio"
                ),
                Data: { id: requerimiento.Id, domicilio: comando },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje("Error", result.Error);
                        return;
                    }

                    actualizarDetalle(function () {
                        cargarDatosEncabezado();
                        cargarUbicacion();
                    });
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje("Error", "Error procesando la solicitud");
                }
            });
        }
    });
}

function cambiarMotivo() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_MOTIVO)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    crearDialogoRequerimientoCambiarMotivo({
        Id: requerimiento.Id,
        VerDetalleRequerimiento: false,
        TipoMotivo: requerimiento.MotivoTipo,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackCargando: function (mostrar, mensaje) {
            mostrarCargando(mostrar, mensaje);
        },
        Callback: function (result) {
            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        }
    });
}

function agregarImagen() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_DOCUMENTOS)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    $("#input_Imagen").val("");
    $("#input_Imagen").trigger("click");
}

function agregarDocumento() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_DOCUMENTOS)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    $("#input_Documento").val("");
    $("#input_Documento").trigger("click");
}

function quitarArchivo(idArchivo) {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl("~/Servicios/RequerimientoService.asmx/QuitarArchivo"),
        Data: { id: requerimiento.Id, idArchivo: idArchivo },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                mostrarCargando(false);
                return;
            }

            actualizarDetalle(function () {
                cargarAdjuntos();

                if ($("#contenedor_PanelDeslizable").hasClass("visible")) {
                    if ($("#contenedor_Fotos").length != 0) {
                        buscarFotos();
                    } else {
                        buscarDocumentos();
                    }
                }
            });
        },
        OnError: function (result) {
            mostrarMensaje("Error", "Error procesando la solicitud");
            mostrarCargando(false);
        }
    });
}

function agregarComentario() {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_COMENTARIOS)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    crearDialogoInput({
        Titulo: "Nueva nota interna",
        Placeholder: "Nota interna...",
        Botones: [
          {
              Texto: "Cancelar"
          },
          {
              Texto: "Guardar",
              Class: "colorExito",
              CerrarDialogo: false,
              OnClick: function (jAlert) {
                  let input = $(jAlert).find("input");
                  let comentario = input.val();
                  if (comentario == "") {
                      $(jAlert)
                        .find("input")
                        .focus();
                      mostrarMensaje("Alerta", "Ingrese el contenido de la nota interna");
                      return;
                  }
                  $(jAlert).CerrarDialogo();
                  procesarAgregarComentario(comentario);
              }
          }
        ]
    });
}

function procesarAgregarComentario(comentario) {
    if (!validarPermisoRequerimiento(PERMISO_EDITAR_COMENTARIOS)) {
        mostrarMensaje(
          "Error",
          "El requerimiento no se encuentra en un estado válido para realizar esta accion"
        );
        return;
    }

    var url = ResolveUrl(
      "~/Servicios/RequerimientoService.asmx/AgregarComentario"
    );
    var data = {
        comando: {
            IdRequerimiento: requerimiento.Id,
            Comentario: comentario
        }
    };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
                return;
            }

            mostrarMensaje("Exito", "Nota interna agregada");

            actualizarDetalle(function () {
                cargarComentarios();

                if (
                  $("#contenedor_PanelDeslizable").hasClass("visible") &&
                  $("#contenedor_PanelDeslizable").length != 0
                ) {
                    mostrarTodosLosComentarios();
                }
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function imprimirConMapa() {
    crearDialogoReporteRequerimientoDetalleMapa({
        Id: requerimiento.Id
    });
}

function imprimirSinMapa() {
    crearDialogoReporteRequerimientoDetalle({
        Id: requerimiento.Id
    });
}

//Utiles
function mostrarError(error) {
    mostrarMensajeCritico({ Icono: "error_outline", Titulo: error });
}

function actualizarDetalle(callback) {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl("~/Servicios/RequerimientoService.asmx/GetDetalleById"),
        Data: { id: requerimiento.Id },
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarError(result.Error);
                return;
            }

            requerimiento = result.Return;
            idsTareasSeleccionadas = _.pluck(requerimiento.Tareas, "Id");
            callback();
            cargarInformacionAdicional();
            cargarAcciones();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError("Error procesando la solicitud");
        }
    });
}

function validarPermisoRequerimiento(keyValuePermiso) {
    var permiso = $.grep(permisos, function (element, index) {
        return (
          element.EstadoRequerimiento == requerimiento.EstadoKeyValue &&
          element.Permiso == keyValuePermiso &&
          element.TienePermiso
        );
    })[0];

    if (permiso == undefined) {
        return false;
    }

    return true;
}

//Listener panel
function setOnPanelAbiertoListener(callback) {
    panelCallback = callback;
}

function panelAbierto(abierto) {
    if (panelCallback != undefined) {
        panelCallback(abierto);
    }
}
