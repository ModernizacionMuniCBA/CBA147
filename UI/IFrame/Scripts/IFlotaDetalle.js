var flota;
var estadoOcupado;
var estadoTurnoTerminado;
var ordenesTrabajo;

var PATH_IMAGEN_USUARIO_DEFAULT;

var panelCallback;

var PATH_IMAGEN_USER_MALE;
var PATH_IMAGEN_USER_FEMALE;

function init(data) {
    if ('Error' in data) {
        mostrarError(data.Error);
        return;
    }

    PATH_IMAGEN_USER_MALE =
    top.urlCordobaFiles + "/Archivo/" + top.identificadorFotoUserMale + "/3";
    PATH_IMAGEN_USER_FEMALE =
      top.urlCordobaFiles + "/Archivo/" + top.identificadorFotoUserFemale + "/3";

    flota = data.Flota;
    estadoOcupado = data.EstadoOcupado;
    estadoTurnoTerminado = data.EstadoTurnoTerminado;
    ordenesTrabajo = data.OrdenesTrabajo;

    console.log(flota);

    initEncabezado();
    initAlertas();
    initPanelDeslizable();
    initInformacionAdicional();

    cargarDatos();
}

function cargarDatos() {
    cargarAcciones();
    cargarDatosEncabezado();
    cargarFunciones();
    cargarInformacionAdicional();
}

//Encabezado
function initEncabezado() {

}

function cargarDatosEncabezado() {
    //Nombre
    $('.contenedor_Encabezado .nombre').text(toTitleCase(flota.Nombre));

    //Fecha Alta
    $('.contenedor_Encabezado .fechaAlta').html('<b>Fecha de Alta</b> ' + flota.FechaAltaString);

    //Movil
    $('.contenedor_Movil > .numero').text(flota.Movil.NumeroInterno);
    $('.contenedor_Movil > .tipo').text(flota.Movil.NombreTipo);
    $('.contenedor_Movil > .marca').text(flota.Movil.Marca + ' ' + flota.Movil.Modelo);

    //Estado
    $('.contenedor_Estado .texto').html('<b>Estado</b> ' + toTitleCase(flota.EstadoNombre));
    $('.contenedor_Estado .icono').css('color', '#' + flota.EstadoColor);

    $(".contenedor_Personal .contenido").empty();
    $.each(flota.Empleados, function (i, empleado) {
        let divEmpleado = crearHtmlEmpleado(empleado);
        $(".contenedor_Personal .contenido").append(divEmpleado);
    })

    cargarAlertas();
}

function crearHtmlEmpleado(data) {
    let divPersona = $("#template_Empleado").html();
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
      .find(".link")
      .text(data.Nombre + " " + data.Apellido);

    $(divPersona)
      .find(".persona > .foto, .link")
      .click(function () {
          crearDialogoEmpleadoDetalle({
              Id: data.Id
          });
      });

    return divPersona;
}

//Alertas
function initAlertas() {

}

function cargarAlertas() {
    $('.contenedor_Alertas').empty();

    if (flota.IdOrdenTrabajo != null) {
        mostrarAlertaEmpleadoEnOrdenDeTrabajo();
    }

    if (flota.FechaBaja != null) {
        mostrarAlertaEmfpleadoDadoDeBaja();
    } else {
        $(".contenedor_Indicadores .fila1").css({ opacity: 1 });
    }
}

function mostrarAlertaEmpleadoEnOrdenDeTrabajo() {

    var div = $('#template_Alerta').html();
    div = $(div);
    $(div).addClass('naranja');

    $(div).find('.contenido').text("Empleado actualmente en Orden de Trabajo N° " + flota.NumeroOrdenTrabajo);
    $(div).find('.link').text("Ver detalle");
    $(div).appendTo('#contenedor_Alertas');

    $(div).find('.link').click(function () {
        crearDialogoOrdenTrabajoDetalle({
            Id: flota.IdOrdenTrabajo,
            Callback: function () {
                actualizarDetalle(function () {
                    cargarDatos();
                });
            }
        });
    });
}

function mostrarAlertaEmpleadoDadoDeBaja() {
    var div = $('#template_Alerta').html();
    div = $(div);
    $(div).addClass('rojo');

    $(div).find('.contenido').text("Empleado dado de baja el " + dateTimeToString(flota.FechaBaja));
    $(div).appendTo('#contenedor_Alertas');

    $("#contenedor_Indicadores .fila1").css({ opacity: 0 });
}

//Panel Deslizable
function initPanelDeslizable() {
    $('#btn_CerrarPanelDeslizable').click(function () {
        cerrarPanelDeslizable();
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                $('#btn_CerrarPanelDeslizable').trigger('click');
            }
        }
    });
}

function abrirPanelDeslizable(titulo) {
    $('#contenedor_PanelDeslizable').addClass('visible');

    $('#contenedor_PanelDeslizable .encabezado .titulo').text(titulo);

    $('#contenedor_PanelDeslizable .contenedor_Contenido').scrollTop(0)
    $('#contenedor_PanelDeslizable .contenedor_Contenido').empty();

    panelAbierto(true)
}

function cerrarPanelDeslizable() {
    $('#contenedor_PanelDeslizable .encabezado .titulo').text('');
    $('#contenedor_PanelDeslizable').removeClass('visible');
    panelAbierto(false)
}

function cargarFunciones() {
    var textoArea = flota.NombreArea;
    if (flota.FechaBaja != null) {
        textoArea += " (dado de baja el " + dateToString(flota.FechaBaja) + ")";
    }
}

function getTextoFunciones() {
    var funciones = flota.Funciones;

    var primera = true;
    var texto = "";
    $.each(funciones, function (index, id) {
        if (!primera) {
            texto += ', ';
        }
        primera = false;
        texto += funciones[index].FuncionNombre;
    })
    return toTitleCase(texto);
}

//Informacion adicional
function initInformacionAdicional() {
    $('.contenedor_InfoAdicional .textoUsuarioCreador').click(function () {
        crearDialogoUsuarioDetalle({
            Id: flota.UsuarioCreadorId
        });
    });

    $('.contenedor_InfoAdicional .textoUsuarioModificacion').click(function () {
        crearDialogoUsuarioDetalle({
            Id: flota.UsuarioModificacionId
        });
    });
}

function cargarInformacionAdicional() {
    $('.contenedor_InfoAdicional .textoFechaCreacion').html('<b>' + dateTimeToString(flota.FechaAlta) + '</b>');

    $('.contenedor_InfoAdicional .textoUsuarioCreadorConector').show();
    $('.contenedor_InfoAdicional .textoUsuarioCreador').show();

    if (flota.FechaModificacion != undefined) {
        $('.contenedor_InfoAdicional .linea3').show();
        $('.contenedor_InfoAdicional .textoFechaModificacion').html('<b>' + dateTimeToString(flota.FechaModificacion) + '</b>');
        if (flota.UsuarioModificacionNombre != undefined && flota.UsuarioModificacionNombre.trim() != "") {
            $('.contenedor_InfoAdicional .textoUsuarioModificacionConector').show();
            $('.contenedor_InfoAdicional .textoUsuarioModificacion').show();
            $('.contenedor_InfoAdicional .textoUsuarioModificacion').html('<b>' + toTitleCase(flota.UsuarioModificacionNombre + ' ' + flota.UsuarioModificacionApellido) + '</b>');
        } else {
            $('.contenedor_InfoAdicional .textoUsuarioModificacionConector').hide();
            $('.contenedor_InfoAdicional .textoUsuarioModificacion').hide();
        }
    } else {
        $('.contenedor_InfoAdicional .linea3').hide();
    }
}

//Estados
function mostrarHistorialDeEstados() {

    abrirPanelDeslizable('Historial de Estados');

    var divHistorialEstados = $($('#template_HistorialEstados').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divHistorialEstados);

    $.each(flota.Estados, function (index, element) {
        var div = $($('#template_HistorialEstadoItem').html());
        if (index == 0) {
            $(div).find('.linea1').css({ opacity: 0 });
        }
        if (index == flota.Estados.length - 1) {
            $(div).find('.linea2').css({ opacity: 0 });
        }
        $(div).find('.circulo').css({ 'background-color': '#' + element.EstadoColor });
        $(div).find('.nombre').text(toTitleCase(element.EstadoNombre));
        $(div).find('.motivo').text(element.EstadoObservaciones);
        $(div).find('.nombrePersona').html('<b>' + toTitleCase(element.UsuarioNombre + ' ' + element.UsuarioApellido) + '</b>');
        $(div).find('.nombrePersona').click(function () {
            crearDialogoUsuarioDetalle({
                Id: element.UsuarioId
            });
        });


        $(div).find('.fecha').html(' el <b>' + dateTimeToString(element.EstadoFecha) + '</b>');

        $(divHistorialEstados).append(div);
    });
}

function cambiarEstado() {
    if (!validarEstadoParaCambiarEstado()) {
        mostrarMensaje('Error', 'La flota no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    crearDialogoFlotaCambiarEstado({
        Id: flota.Id,
        IdEstadoAnterior: flota.EstadoId,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        }
    });
}

//Ordenes trabajo
function mostrarOrdenesTrabajo() {

    abrirPanelDeslizable('Historial de trabajos');

    var divContenido = $($('#template_Trabajos').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divContenido);

    //por error de interfaz se duplica el select, por lo cual lo oculto
    $(".contenedor_Select span:nth-child(4)").hide();

    //Inicializo la tabla
    $(divContenido).find('table').prop('id', 'tablaOT');

    let dt = $('#tablaOT').DataTableOrdenTrabajo({
        CallbackMensajes: function () { mostrarMensaje(tipo, mensaje) }
    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
    dt.$('.selectMaterialize').material_select();

    //Muevo el indicador y el paginado a mi propio div
    $(divContenido).find('.tabla-footer').empty();
    $(divContenido).find('.dataTables_info').detach().appendTo($(divContenido).find('.tabla-footer'));
    $(divContenido).find('.dataTables_paginate').detach().appendTo($(divContenido).find('.tabla-footer'));
    $(divContenido).find('.dataTables_info').hide();

    //Agrego las filas
    let hDisponible = $(divContenido).find('.tabla-contenedor').height();
    let rows = calcularCantidadRowsDataTable(hDisponible);
    dt.page.len(rows);

    dt.rows.add(ordenesTrabajo).draw(true);
}

//Editar
function editar() {
    if (!validarEstadoParaEditar()) {
        mostrarMensaje('Error', 'No se puede realizar ésta acción ya que la flota ya ha formado parte de al menos una orden de trabajo.');
        return;
    }

    if (!validarEstadoParaCambiarEstado()) {
        mostrarMensaje('Error', 'La flota no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    crearDialogoFlotaEditar({
        Id: flota.Id,
        IdArea: flota.IdArea,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            mostrarMensaje("Exito", "La flota ha sido editada con éxito");
            actualizarDetalle(function () {
                cargarInformacionAdicional()
                cargarDatosEncabezado();
            });
        }
    });
}

function terminarTurno() {
    if (!validarEstadoParaCambiarEstado()) {
        mostrarMensaje('Error', 'La flota no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    crearDialogoConfirmacion({
        Texto: '¿Está seguro de que quiere finalizar el turno de la flota ' + flota.Nombre + '?',
        ClassBotonAceptar: 'colorExito',
        CallbackPositivo: function () {
            crearAjax({
                Url: ResolveUrl('~/Servicios/FlotaService.asmx/TerminarTurno'),
                Data: { id: flota.Id },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    mostrarMensaje("Exito", "El turno de la flota se ha terminado con éxito.");

                    actualizarDetalle(function () {
                        cargarDatosEncabezado();
                        cargarAlertas();
                        cargarAcciones();
                    });
                },
                OnError: function (result) {
                    mostrarMensaje('Error', result.Error);
                    return;
                }
            });
        }
    });

}

//Acciones
function cargarAcciones() {
    $('.contenedor_Acciones .contenido').empty();

    var modificacion = validarPermisoModificacion('FlotaPanel') && flota.FechaBaja == null;

    if (modificacion) {
        //Funciones
        agregarAccion({
            Texto: 'Finalizar turno',
            Icono: 'swap_vert',
            Permiso: validarEstadoParaCambiarEstado(),
            OnClick: function () {
                terminarTurno();
            }
        });

        ////Estado
        //agregarAccion({
        //    Texto: 'Cambiar Estado',
        //    Icono: '',
        //    Permiso: validarEstadoParaCambiarEstado(),
        //    OnClick: function () {
        //        cambiarEstado();
        //    }
        //});

        //Dar de baja
        agregarAccion({
            Texto: 'Editar',
            Icono: 'edit',
            Permiso: function () { return validarEstadoParaEditar() && validarEstadoParaCambiarEstado() },
            OnClick: function () {
                editar();
            }
        });
    }

    //Historico
    agregarAccion({
        Texto: 'Historial de estados',
        Icono: 'history',
        OnClick: function () {
            mostrarHistorialDeEstados();
        }
    });

    //Trabajos
    agregarAccion({
        Texto: 'Historial de Trabajos',
        Icono: 'build',
        OnClick: function () {
            mostrarOrdenesTrabajo();
        }
    });

}

function agregarAccion(valores) {
    var div = $($('#template_Accion').html());
    $(div).find('.texto').text(valores.Texto);
    $(div).find('.icono').text(valores.Icono);
    $('.contenedor_Acciones .contenido').append(div);

    if (('Permiso' in valores) && !valores.Permiso) {
        $(div).addClass('deshabilitado');
    }

    if (typeof (valores.Permiso) === "function") {
        if (!valores.Permiso())
            $(div).addClass('deshabilitado');
    }

    $(div).click(function () {
        valores.OnClick();
    });
}

function darDeBaja() {
    crearDialogoConfirmacion({
        Texto: '¿Esta seguro de que quiere dar de baja al flota de su área?',
        ClassBotonAceptar: 'colorError',
        CallbackPositivo: function () {
            crearAjax({
                Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/DarDeBaja'),
                Data: { idEmpleado: flota.Id },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle(function () {
                        cargarInformacionAdicional();
                        cargarAlertas();
                        cargarAcciones();
                    });
                },
                OnError: function (result) {
                    mostrarMensaje('Error', result.Error);
                    return;
                }
            });
        }
    });
}

//validaciones
function validarEstadoParaCambiarEstado() {
    return flota.EstadoId != estadoOcupado.Id && flota.EstadoId != estadoTurnoTerminado.Id;
}

function validarEstadoParaEditar() {
    return ordenesTrabajo.length == 0;
}

//Utiles
function mostrarError(error) {
    mostrarMensajeCritico({ Icono: 'error_outline', Titulo: error })
}

function actualizarDetalle(callback) {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/FlotaService.asmx/GetResultadoById'),
        Data: { id: flota.Id },
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarError(result.Error);
                return;
            }

            flota = result.Return;
            if (callback != undefined)
                callback();
            cargarInformacionAdicional();
            cargarAcciones();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error procesando la solicitud');
        }
    });
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