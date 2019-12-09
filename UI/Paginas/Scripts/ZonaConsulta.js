var zonas;
var usuario;
var permitirEditar;

function init(data) {
    data = parse(data);

    initConsulta(data);


    //------------------------------------
    // Init Datos
    //------------------------------------

    zonas = data.Zonas.Data;
    usuario = data.Usuario;


    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();
    cargarResultadoConsulta(zonas);

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

    $('#selectFormulario_Area').on('change', function (e) {
        filtrarBusqueda();
    });

    //------------------------------------
    // Anim inicio
    //------------------------------------

    $('#cardFormularioFiltros').css('opacity', 0);
    $('#cardFormularioFiltros').css('top', '50px');

    setTimeout(function () {
        $('#cardFormularioFiltros').animate({ 'opacity': 1, 'top': '0px' }, 500);
    }, 200);

    $('#cardResultadoReclamos').css('opacity', 0);
    $('#cardResultadoReclamos').css('top', '50px');

    setTimeout(function () {
        $('#cardResultadoReclamos').animate({ 'opacity': 1, 'top': '0px' }, 500);
    }, 600);


    $('#btnNuevo').css('display', validarPermisoAlta('Zonas') ? 'auto' : 'none');
    $('#btnNuevo').click(function () {
        crearDialogoZona({
            IdArea: $('#selectFormulario_Area').val(),
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function (entity) {
                zonas.push(entity);
                filtrarBusqueda();
            }
        });
    })

    $('#selectFormulario_Area').trigger('change');
};

function initConsulta(data) {
    //Cargo las areas
    if ('Areas' in data) {
        //Cargo los datos
        $('#selectFormulario_Area').CargarSelect({
            Data: data.Areas,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true
        });
    }
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

    let idArea = parseInt($('#selectFormulario_Area').val());
    console.log(idArea);

    $.each(zonas, function (index, zona) {

        if (zona.Nombre.toUpperCase().indexOf(texto) != -1) {

            switch (estado) {
                case 1:
                    if (zona.AreaId == idArea) {
                        resultados.push(zona);
                    }

                    break;

                case 2:
                    if (zona.FechaBaja == null && zona.AreaId == idArea) {
                        resultados.push(zona);
                    }
                    break;

                case 3:
                    if (zona.FechaBaja != null && zona.AreaId == idArea) {
                        resultados.push(zona);
                    }
                    break;
            }
        }
    });
    cargarResultadoConsulta(resultados);
}

function initTablaResultadoConsulta() {
    $('#tabla').DataTableZona({
        Callback: function (zona) {
            $.each(zonas, function (index, element) {
                if (element.Id == zona.Id) {
                    zonas[index] = zona;
                }
            });
            filtrarBusqueda();
        }
    });
    cargarResultadoConsulta([]);

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

function cargarResultadoConsulta(data) {
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
