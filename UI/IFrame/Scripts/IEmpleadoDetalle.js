var empleado;
var estadosOcupado;

var PATH_IMAGEN_USUARIO_DEFAULT;

var panelCallback;

function init(data) {
    if ('Error' in data) {
        mostrarError(data.Error);
        return;
    }

    PATH_IMAGEN_USUARIO_DEFAULT = ResolveUrl('~/Resources/Imagenes/user-avatar.png');

    empleado = data.Empleado;
    estadosOcupado = data.EstadosOcupado;

    console.log(empleado);

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
    $('#btn_Acciones').click(function () {
        $('#contenedor_Acciones .contenido').toggleClass('visible');
        $(this).text($('#contenedor_Acciones .contenido').hasClass('visible') ? 'Ocultar acciones' : 'Ver acciones');
    });

    $('#texto_Nombre').click(function () {
        crearDialogoUsuarioDetalle({
            Id: empleado.IdUsuarioCerrojoEmpleado
        });
    });
}

function cargarDatosEncabezado() {
    //Foto
    let identificador;
    if (empleado.IdentificadorFotoPersonal != undefined) {
        identificador = empleado.IdentificadorFotoPersonal;
    } else {
        if (empleado.SexoMasculino == true) {
            identificador = top.identificadorFotoUserMale;
        } else {
            identificador = top.identificadorFotoUserFemale;
        }
    }
    $('#foto').css('background-image', 'url(' + top.urlCordobaFiles + '/Archivo/' + identificador + '/3)');

    //Nombre y apellido
    if (empleado.Nombre == undefined || empleado.Apellido == undefined) {
        $('#texto_Nombre').text('Persona desconocida');
    } else {
        $('#texto_Nombre').text(toTitleCase(empleado.Nombre + " " + empleado.Apellido));
    }

    //Dni
    if (empleado.Dni != undefined) {
        $("#texto_Dni").html("<b>DNI </b>" + empleado.Dni);
    }

    //Cargo
    if (empleado.Cargo != undefined) {
        $("#texto_Cargo").html("<b>Cargo </b>" + toTitleCase(empleado.Cargo));
    }

    //Seccion
    if (empleado.NombreSeccion != undefined) {
        $("#texto_Seccion").html("<b>Sección </b>" + toTitleCase(empleado.NombreSeccion));
    }

    //Funcion
    var funcion = getTextoFunciones();
    if (funcion != undefined && funcion != "") {
        $("#texto_Funcion").html("<b>Función </b>" + funcion);
    }

    //Estado
    $('#texto_IndicadorEstado').html('<b>Estado</b> ' + toTitleCase(empleado.NombreEstado));
    $('#icono_IndicadorEstado').css('color', '#' + empleado.ColorEstado);

    cargarAlertas();
}

//Alertas
function initAlertas() {

}

function cargarAlertas() {
    $('#contenedor_Alertas').empty();

    if (empleado.IdOrdenTrabajo != null) {
        mostrarAlertaEmpleadoEnOrdenDeTrabajo();
    }

    if (empleado.FechaBaja != null) {
        mostrarAlertaEmpleadoDadoDeBaja();
    } else {
        $("#contenedor_Indicadores .fila1").css({ opacity: 1 });
    }
}

function mostrarAlertaEmpleadoEnOrdenDeTrabajo() {

    var div = $('#template_Alerta').html();
    div = $(div);
    $(div).addClass('naranja');

    $(div).find('.contenido').text("Empleado actualmente en Orden de Trabajo N° " + empleado.NumeroOrdenTrabajo);
    $(div).find('.link').text("Ver detalle");
    $(div).appendTo('#contenedor_Alertas');

    $(div).find('.link').click(function () {
        crearDialogoOrdenTrabajoDetalle({
            Id: empleado.IdOrdenTrabajo,
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

    $(div).find('.contenido').text("Empleado dado de baja el " + dateTimeToString(empleado.FechaBaja));
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
    var textoArea = empleado.NombreArea;
    if (empleado.FechaBaja != null) {
        textoArea += " (dado de baja el " + dateToString(empleado.FechaBaja) + ")";
    }
}

function getTextoFunciones() {
    var funciones = empleado.Funciones;

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
    $('#contenedor_InfoAdicional .textoUsuarioCreador').click(function () {
        crearDialogoUsuarioDetalle({
            Id: empleado.UsuarioCreadorId
        });
    });

    $('#contenedor_InfoAdicional .textoUsuarioModificacion').click(function () {
        crearDialogoUsuarioDetalle({
            Id: empleado.UsuarioModificacionId
        });
    });
}

function cargarInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoFechaCreacion').html('<b>' + dateTimeToString(empleado.FechaAlta) + '</b>');

    $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').show();
    $('#contenedor_InfoAdicional .textoUsuarioCreador').show();
    $("#contenedor_InfoAdicional .textoArea").html("<b>" + empleado.NombreArea + "</b>");

    if (empleado.FechaModificacion != undefined) {
        $('#contenedor_InfoAdicional .linea3').show();
        $('#contenedor_InfoAdicional .textoFechaModificacion').html('<b>' + dateTimeToString(empleado.FechaModificacion) + '</b>');
        if (empleado.UsuarioModificacionNombre != undefined && empleado.UsuarioModificacionNombre.trim() != "") {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').html('<b>' + toTitleCase(empleado.UsuarioModificacionNombre + ' ' + empleado.UsuarioModificacionApellido) + '</b>');
        } else {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').hide();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').hide();
        }
    } else {
        $('#contenedor_InfoAdicional .linea3').hide();
    }
}

//Estados
function mostrarHistorialDeEstados() {

    abrirPanelDeslizable('Historial de Estados');

    var divHistorialEstados = $($('#template_HistorialEstados').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divHistorialEstados);

    $.each(empleado.Estados, function (index, element) {
        var div = $($('#template_HistorialEstadoItem').html());
        if (index == 0) {
            $(div).find('.linea1').css({ opacity: 0 });
        }
        if (index == empleado.Estados.length - 1) {
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

//Ordenes trabajo
function mostrarOrdenesTrabajo() {

    abrirPanelDeslizable('Historial de trabajos');

    var divContenido = $($('#template_Trabajos').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divContenido);

    var años = getYears(empleado.FechaAlta);
    $(divContenido).find(".select_TrabajoAño").CargarSelect({
        Data: años,
        Value: 'Año',
        Text: 'Año',
        Sort: false
    });

    $(divContenido).find(".select_TrabajoMes").CargarSelect({
        Data: [],
        Value: 'Value',
        Text: 'Text',
        Sort: false,
        Default: 'Seleccione...'
    });

    $(divContenido).find(".select_TrabajoAño").change(function () {
        $(divContenido).find(".select_TrabajoMes").CargarSelect({
            Data: getMonths($(this).val()),
            Value: 'Value',
            Text: 'Text',
            Sort: false
        });

    })

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

    $(divContenido).find(".select_TrabajoMes").change(function () {
        buscarTrabajos(divContenido, dt);
    });

    $(divContenido).find(".select_TrabajoAño").change(function () {
        buscarTrabajos(divContenido, dt);
    });

    $(divContenido).find(".select_TrabajoAño").trigger("change");

}

function buscarTrabajos(divContenido, dt) {
    dt.clear();

    $(divContenido).find('.contenedor_Cargando').addClass('visible');

    var mes = $(divContenido).find(".select_TrabajoMes").val();
    var año = $(divContenido).find(".select_TrabajoAño").val();

    mes = parseInt(mes);

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/GetDatosTablaByIdEmpleado'),
        Data: { consulta: { IdEmpleado: empleado.Id, Mes: mes, Año: año } },
        OnSuccess: function (result) {
            $(divContenido).find('.contenedor_Cargando').removeClass('visible');
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                cerrarPanelDeslizable();
                return;
            }

            dt.rows.add(result.Return.Data).draw(true);
        },
        OnError: function (result) {
            $(divContenido).find('.contenedor_Cargando').removeClass('visible');
            mostrarMensaje('Error', 'Error procesando la solicitud');
            cerrarPanelDeslizable();
        }
    });

    //Reintentar
    $(divContenido).find('.btn_Reintentar').click(function () {
        mostrarOrdenesTrabajo();
    });
}

//Acciones
function cargarAcciones() {
    $('#contenedor_Acciones .contenido').empty();

    var modificacion = validarPermisoModificacion('Personal') && empleado.FechaBaja == null;

    //Usuario
    agregarAccion({
        Texto: 'Ver información usuario',
        Icono: 'info_outline',
        OnClick: function () {
            verUsuarioDetalle();
        }
    });


    if (modificacion) {
        //Funciones
        agregarAccion({
            Texto: 'Editar funciones',
            Icono: 'edit',
            OnClick: function () {
                editarFunciones();
            }
        });

        //Estado
        agregarAccion({
            Texto: 'Cambiar Estado',
            Icono: 'swap_vert',
            Permiso: validarEstadoParaCambiarEstado(),
            OnClick: function () {
                cambiarEstado();
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

    if (modificacion) {
        //Dar de baja
        agregarAccion({
            Texto: 'Dar de baja',
            Icono: 'close',
            Permiso: validarEstadoParaCambiarEstado(),
            OnClick: function () {
                darDeBaja();
            }
        });
    }
}

function agregarAccion(valores) {
    var div = $($('#template_Accion').html());
    $(div).find('.texto').text(valores.Texto);
    $(div).find('.icono').text(valores.Icono);
    $('#contenedor_Acciones .contenido').append(div);
    if (('Permiso' in valores) && !valores.Permiso) {
        $(div).addClass('deshabilitado');
    }
    $(div).click(function () {
        valores.OnClick();
    });
}

function verUsuarioDetalle() {
    crearDialogoUsuarioDetalle({
        Id: empleado.IdUsuarioCerrojoEmpleado
    });
}

function editarFunciones() {
    crearDialogoEmpleadoEditarFunciones({
        IdEmpleado: empleado.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarInformacionAdicional()
                cargarDatosEncabezado();
            });
        }
    });
}

function cambiarEstado() {
    if (!validarEstadoParaCambiarEstado()) {
        mostrarMensaje('Error', 'El empleado no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    crearDialogoEmpleadoCambiarEstado({
        Id: empleado.Id,
        IdEstadoAnterior: empleado.IdEstado,
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

function darDeBaja() {
    if (!validarEstadoParaCambiarEstado()) {
        mostrarMensaje('Error', 'El empleado no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    crearDialogoConfirmacion({
        Texto: '¿Esta seguro de que quiere dar de baja al empleado de su área?',
        ClassBotonAceptar: 'colorExito',
        CallbackPositivo: function () {
            crearAjax({
                Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/DarDeBaja'),
                Data: { idEmpleado: empleado.Id },
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

function validarEstadoParaCambiarEstado() {
    return !_.some(estadosOcupado, function (est) { return empleado.IdEstado == est.Id });
}

//Utiles
function mostrarError(error) {
    mostrarMensajeCritico({ Icono: 'error_outline', Titulo: error })
}

function actualizarDetalle(callback) {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/GetDetalleById'),
        Data: { id: empleado.Id },
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarError(result.Error);
                return;
            }

            empleado = result.Return;
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

function getYears(from) {
    var d1 = new Date(from),
        d2 = new Date(),
        yr = [];

    for (var i = d2.getFullYear() ; i >= d1.getFullYear() ; i--) {
        yr.push({ Año: '' + i });
    }

    return yr;
}

function getMonths(yearFrom) {
    var d1 = new Date(empleado.FechaAlta);
    //si el año pedido es mayor que el de fecha de alta del empleado, se devuelven los meses desde enero
    if (yearFrom > d1.getFullYear()) {
        d1 = new Date(yearFrom, 0, 1);
    }

    var d2 = new Date(yearFrom, 11, 31);
    //si el año pedido es igual al actual, se devuelven los meses desde enero hasta la fecha
    if (new Date().getFullYear() == d2.getFullYear()) {
        d2 = new Date();
    }

    var ms = [];
    for (var i = d2.getMonth() ; i >= d1.getMonth() ; i--) {
        ms.push({ "Value": i+1, "Text": getMes(i) });
    }

    return ms;
}

function getMes(numero) {
    switch (numero) {
        case 0:
            return "Enero";
        case 1:
            return "Febrero";
        case 2:
            return "Marzo";
        case 3:
            return "Abril";
        case 4:
            return "Mayo";
        case 5:
            return "Junio";
        case 6:
            return "Julio";
        case 7:
            return "Agosto";
        case 8:
            return "Septiembre";
        case 9:
            return "Octubre";
        case 10:
            return "Noviembre";
        case 11:
            return "Diciembre";
    }
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