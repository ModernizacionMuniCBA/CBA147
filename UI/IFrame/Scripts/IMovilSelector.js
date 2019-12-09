let callbackSeleccionar;
let callbackObjetosSeleccionar;
let moviles;
let idsMoviles;
let dt;


function init(data) {
    data = parse(data);
    moviles = data.Moviles;

    initTablaResultadoConsulta();
    informarInit();
}

function inicializarPantalla(ids) {
    idsMoviles = ids;
    cargarResultadoConsulta(moviles);
}

function cargarResultadoConsulta(data) {
    dt = $('#tablaMovilesSelector').DataTable();

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

    var dt = $('#tablaMovilesSelector').DataTable();
    dt.page.len(rows).draw();
}

function initTablaResultadoConsulta() {
    var dt = $('#tablaMovilesSelector').DataTableMovil({
        ColumnaEstado: false,
        Orden: [[0, 'asc']],
        BotonCheckbox: true,
        Callback: function (data) {
            dt.rows().every(function () {
                let d = this.data();
                console.log(d);
                if (d.Id == data.Id) {
                    let node = this.node();
                    $(node).find('input').prop('checked', idsMoviles.indexOf(data.Id) != -1);
                }
            });
        },
        OnFilaCreada: function (row, data) {
            $(row).find('input').prop('checked', idsMoviles.indexOf(data.Id) != -1);
            $(row).find('input').change(function () {
                let check = $(this).is(':checked');
                if (check) {
                    if (idsMoviles.indexOf(data.Id) == -1) {
                        idsMoviles.push(data.Id);
                    }
                } else {
                    idsMoviles = $.grep(idsMoviles, function (element, index) { return element != data.Id });
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
        callbackSeleccionar(idsMoviles);

    if (callbackObjetosSeleccionar != null && callbackObjetosSeleccionar != undefined) 
    callbackObjetosSeleccionar(getMoviles());
}

function setOnMovilSeleccionadoListener(callback) {
    callbackSeleccionar = callback;
}

function setOnMovilObjetosSeleccionadoListener(callback) {
    callbackObjetosSeleccionar = callback;
}

function getMoviles() {
    return $.grep(moviles, function (element) { return idsMoviles.some(function (id) { return element.Id == id }) });
}

let callbackInit;
function setCallbackInit(callback) {
    callbackInit = callback;
}

function informarInit() {
    if (callbackInit == undefined) return;
    callbackInit();
}