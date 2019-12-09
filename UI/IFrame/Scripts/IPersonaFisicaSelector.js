var callbackSeleccionar;
var callbackMensaje;
var callbackCargando;

var isInit = false;

function initTabla() {
    if (isInit) return;

    $('#tabla').DataTableGeneral({
        Columnas: [
            {
                title: 'Nombre',
                data: 'Nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span><div>';
                }
            },
            {
                title: 'Apellido',
                data: 'Apellido',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span><div>';
                }
            }
        ],
        Botones: [
            {
                Titulo: 'Seleccionar',
                Icono: 'check',
                OnClick: function (data) {
                    informarSeleccion(data);
                }
            }
        ]
    });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));

    isInit = true;
}

function setPersonas(data) {
    initTabla();

    var dt = $('#tabla').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Cantidad de rows
    calcularCantidadDeRows();
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').find('#tabla_wrapper').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();
}

//-------------------------------
// Listener
//-------------------------------

function informarSeleccion(persona) {
    if (callbackSeleccionar == null || callbackSeleccionar == undefined) return;
    callbackSeleccionar(persona);
}

function setOnPersonaSeleccionadaListener(callback) {
    callbackSeleccionar = callback;
}

////-------------------------------
//// Listener Cargando
////-------------------------------

//function mostrarCargando(mostrar, mensaje) {
//    if (callbackCargando != undefined) {
//        callbackCargando(mostrar, mensaje);
//    }
//}

//function setOnCargandoListener(callback) {
//    this.callbackCargando = callback;
//}

////-----------------------------
//// Listener Alertas
////-----------------------------

//function setOnMensajeListener(callback) {
//    this.callbackMensaje = callback;

//}

//function mostrarMensaje(tipo, mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje(tipo, mensaje);
//}

//function mostrarMensajeExito(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Exito', mensaje);
//}

//function mostrarMensajeError(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Error', mensaje);
//}

//function mostrarMensajeAlerta(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Alerta', mensaje);
//}

//function mostrarMensajeInfo(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Info', mensaje);
//}