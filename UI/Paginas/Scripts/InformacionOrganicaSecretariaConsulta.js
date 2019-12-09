var secretarias;

function init(data) {
    secretarias = data.Secretarias;


    //------------------------------------
    // Tabla 
    //------------------------------------

    initTabla();
    cargarTabla(secretarias);

    //------------------------------------
    // Radio
    //------------------------------------

    filtrarBusqueda();

    $('#rdbTodos').change(function () {
        filtrarBusqueda();
    });

    $('#rdbActivoSi').change(function () {
        filtrarBusqueda();
    });

    $('#rdbActivoNo').change(function () {
        filtrarBusqueda();
    });

    $('#inputBusqueda').on('input', function () {
        filtrarBusqueda();
    });

    $('#btnNuevo').click(function () {
        crearDialogoInformacionOrganicaSecretariaNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function (entity) {
                secretarias.push(entity);
                filtrarBusqueda();
            }
        });
    });

    //------------------------------------
    // Anim inicio
    //------------------------------------

    $('#contenedorFiltros').css('opacity', 0);
    $('#contenedorFiltros').css('top', '50px');

    setTimeout(function () {
        $('#contenedorFiltros').animate({ 'opacity': 1, 'top': '0px' }, 500);
    }, 200);

    $('#cardResultado').css('opacity', 0);
    $('#cardResultado').css('top', '50px');

    setTimeout(function () {
        $('#cardResultado').animate({ 'opacity': 1, 'top': '0px' }, 500);
    }, 600);
}

function filtrarBusqueda() {
    var resultados = [];

    var estado = 2;
    if ($('#contenedorEstados').lenght != 0) {
        if ($('#rdbTodos').is(':checked')) {
            estado = 1;
        } else {
            if ($('#rdbActivoSi').is(':checked')) {
                estado = 2;
            } else {
                estado = 3;
            }
        }
    }

    var texto = $('#inputBusqueda').val().trim().toUpperCase();

    $.each(secretarias, function (index, entity) {

        if (entity.Nombre.toUpperCase().indexOf(texto) != -1) {

            switch (estado) {
                case 1:

                    resultados.push(entity);
                    break;

                case 2:
                    if (entity.FechaBaja == null) {
                        resultados.push(entity);
                    }
                    break;

                case 3:
                    if (entity.FechaBaja != null) {
                        resultados.push(entity);
                    }
                    break;
            }
        }

    });

    cargarTabla(resultados);

}

function initTabla() {
    $('#tabla').DataTableInformacionOrganicaSecretaria({
        Orden: [[0, 'asc']],
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        BotonEditar: true,
        BotonEditarOculto: false,
        BotonDarDeBaja: true,
        BotonDarDeAlta: true,
        CallbackEditar: function (origenNuevo) {
            actualizarRowEnGrilla(origenNuevo);
            filtrarBusqueda();
        },
        CallbackDarDeBaja: function (origenNuevo) {
            actualizarRowEnGrilla(origenNuevo);
            filtrarBusqueda();
        },
        CallbackDarDeAlta: function (origenNuevo) {
            actualizarRowEnGrilla(origenNuevo);
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

    console.log(rows);
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
    $.each(secretarias, function (index, element) {
        if (element.Id == entity.Id) {
            secretarias[index] = entity;
            return;
        }
    });
}
