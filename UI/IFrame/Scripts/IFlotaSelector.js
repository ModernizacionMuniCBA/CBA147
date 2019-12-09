let callbackSeleccionar;
let callbackObjetosSeleccionar;

let flotas;
let idsFlotas = [];

let dt;

function init(data) {
    data = parse(data);
    flotas = data.Flotas;

    initTablaResultadoConsulta();
    informarInit();
}

function inicializarPantalla(ids) {
    idsFlotas = ids;
    cargarResultadoConsulta(flotas);
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
    var dt = $('#tablaSelector').DataTableFlota({
        Buscar: true,
        InputBusqueda: '#input_BusquedaFlota',
        Orden: [[0, 'asc']],
        BotonSeleccionar: true,
        Callback: function (data) {
            dt.rows().every(function () {
                let d = this.data();
                console.log(d);
                if (d.Id == data.Id) {
                    let node = this.node();
                    $(node).find('input').prop('checked', idsFlotas.indexOf(data.Id) != -1);
                }
            });
        },
        OnFilaCreada: function (row, data) {
            $(row).find('input').prop('checked', idsFlotas.indexOf(data.Id) != -1);
            $(row).find('input').change(function () {
                let check = $(this).is(':checked');
                if (check) {
                    if (idsFlotas.indexOf(data.Id) == -1) {
                        idsFlotas.push(data.Id);
                    }
                } else {
                    idsFlotas = $.grep(idsFlotas, function (element, index) { return element != data.Id });
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

//-------------------------------
// Listener
//-------------------------------

function informarSeleccion() {
    if (callbackSeleccionar != null && callbackSeleccionar != undefined) 
        callbackSeleccionar(idsFlotas);

    if (callbackObjetosSeleccionar != null && callbackObjetosSeleccionar != undefined) 
    callbackObjetosSeleccionar(getEmpleados());
}

function setOnSeleccionadoListener(callback) {
    callbackSeleccionar = callback;
}

function setOnObjetosSeleccionadoListener(callback) {
    callbackObjetosSeleccionar = callback;
}

function getEmpleados() {
    return $.grep(flotas, function (element) { return idsFlotas.some(function (id) { return element.Id == id }) });
}

let callbackInit;
function setCallbackInit(callback) {
    callbackInit = callback;
}

function informarInit() {
    if (callbackInit == undefined) return;
    callbackInit();
}