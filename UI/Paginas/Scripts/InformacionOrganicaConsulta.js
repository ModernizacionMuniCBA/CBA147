var entities;
var areas;
var filas = [];

function init(data) {
    if ('Error' in data) return;

    entities = data.InformacionOrganica;
    areas = data.Areas;


    $.each(areas, function (index, element) {
        let entity = $.grep(entities, function (element1, index1) {
            return element1.Area.Id == element.Id;
        })[0];
        filas.push({
            Id: element.Id,
            Area: element,
            InformacionOrganica: entity
        });
    });


    //------------------------------------
    // Tabla 
    //------------------------------------

    initTabla();
    cargarTabla(filas);

    //------------------------------------
    // Radio
    //------------------------------------

    filtrarBusqueda();

    $('#inputBusqueda').on('input', function () {
        filtrarBusqueda();
    });

    //------------------------------------
    // Anim inicio
    //------------------------------------


    $('#cardResultado').css('opacity', 0);
    $('#cardResultado').css('top', '50px');

    setTimeout(function () {
        $('#cardResultado').animate({ 'opacity': 1, 'top': '0px' }, 500);
    }, 600);
}

function filtrarBusqueda() {
    var resultados = [];

    var texto = $('#inputBusqueda').val().trim().toUpperCase();

    $.each(filas, function (index, entity) {

        if (entity.Area.Nombre.toUpperCase().indexOf(texto) != -1) {
            resultados.push(entity);
        }

    });

    cargarTabla(resultados);
}

function initTabla() {
    $('#tabla').DataTableInformacionOrganica({
        Orden: [[0, 'asc']],
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        BotonAsignar: true,
        BotonAsignarOculto: false,
        CallbackAsignar: function (entity) {
            actualizarRowEnGrilla(entity);
            filtrarBusqueda();
        },
        BotonDarDeBaja: true,
        CallbackDarDeBaja: function (entity) {
            actualizarRowEnGrilla(entity);
            filtrarBusqueda();
        }
    });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();
}

function cargarTabla(data) {
    var dt = $('#tabla').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    calcularCantidadDeRows();
}

function actualizarRowEnGrilla(entity) {
    //Busco el indice de la zona a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == entity.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(entity);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Actualizo la info nueva en el array de arriba
    $.each(entities, function (index, element) {
        if (element.Id == entity.Id) {
            entities[index] = entity;
            return;
        }
    });
}
