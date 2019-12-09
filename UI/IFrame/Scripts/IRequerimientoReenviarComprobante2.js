let callbackSeleccionar;
let callbackObjetosSeleccionar;
let usuarios;
let idsUsuarios = [];
let idRequerimiento;
let dt;

function init(data) {
    data = parse(data);

    if (data.Error != undefined) {
        mostrarMensaje("Error", data.Error);
        return;
    }

    usuarios= data.UsuariosReferentes;
    idRequerimiento = data.IdRequerimiento;

    initTablaResultadoConsulta();
    informarInit();
    cargarResultadoConsulta(usuarios);
}

function inicializarPantalla(ids) {
    idsUsuarios = ids;
    cargarResultadoConsulta(usuarios);
}

function cargarResultadoConsulta(data) {
    dt = $('#tablaSelector').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    calcularCantidadDeRowsSelector();
}

function calcularCantidadDeRowsSelector() {
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tablaSelector').DataTable();
    dt.page.len(rows).draw();
}

function initTablaResultadoConsulta() {
    var dt = $('#tablaSelector').DataTableUsuario({
        Orden: [[0, 'asc']],
        BotonSeleccionar: true,
        Callback: function (data) {
            dt.rows().every(function () {
                let d = this.data();
                console.log(d);
                if (d.Id == data.Id) {
                    let node = this.node();
                    $(node).find('input').prop('checked', idsUsuarios.indexOf(data.Id) != -1);
                }
            });
        },
        OnFilaCreada: function (row, data) {
            $(row).find('input').prop('checked', idsUsuarios.indexOf(data.Id) != -1);
            $(row).find('input').change(function () {
                let check = $(this).is(':checked');
                if (check) {
                    if (idsUsuarios.indexOf(data.Id) == -1) {
                        idsUsuarios.push(data.Id);
                    }
                } else {
                    idsUsuarios = $.grep(idsUsuarios, function (element, index) { return element != data.Id });
                }
            });
        }
    });

    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function reenviarComprobante() {
    if (!validar()) return;
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EnviarComprobanteAtencion'),
        Data: { id: idRequerimiento, idsUsuarios: idsUsuarios, email: $("#input_Email").val()},
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            informar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    })
}

function validar() {
    if (idsUsuarios.length == 0 && $("#input_Email").val()=="") {
        mostrarMensaje("Error", "Debe seleccionar al menos un usuario referente o ingresar un e-mail para reenviar el comprobante.");
        return false;
    }

    return true;
}

//-------------------------------
// Listener
//-------------------------------

function informarSeleccion() {
    if (callbackSeleccionar != null && callbackSeleccionar != undefined) 
        callbackSeleccionar(idsUsuarios);

    if (callbackObjetosSeleccionar != null && callbackObjetosSeleccionar != undefined) 
    callbackObjetosSeleccionar(getUsuarios());
}

function setOnSeleccionadoListener(callback) {
    callbackSeleccionar = callback;
}

function setOnObjetosSeleccionadoListener(callback) {
    callbackObjetosSeleccionar = callback;
}

function getUsuarios() {
    return $.grep(usuarios, function (element) { return idsUsuarios.some(function (id) { return element.Id == id }) });
}

let callbackInit;
function setCallbackInit(callback) {
    callbackInit = callback;
}

function informarInit() {
    if (callbackInit == undefined) return;
    callbackInit();
}

var callback;
function setOnComprobanteReenviadoListener(listener) {
    callback = listener;
}

function informar() {
    if (callback == undefined) return;
    callback();
}
