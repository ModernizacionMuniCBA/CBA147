var tareas;
var usuario;
var permitirEditar;

var idArea = -1;


function init(data) {
    data = parse(data);

    if (data.Error != undefined) {
        mostrarMensaje('Error', data.Error);
        return;
    }

    if (data.Tareas != undefined) {
        tareas = data.Tareas;
    }

    initConsulta(data);

    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();
    cargarResultadoConsulta(tareas);

    //------------------------------------
    // Radio
    //------------------------------------

    filtrarBusqueda();

    $('#rdbTodos').change(function () {
        filtrarBusqueda();
    });

    $('#check_IncluirDeBaja').on('change', function (e) {
        filtrarBusqueda();
    });

    $('#selectFormulario_Area').on('change', function (e) {
        var a = $('#selectFormulario_Area').val();
        idArea = parseInt(a);
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

    $('#selectFormulario_Area').trigger('change');

    $('#btnNuevo').css('display', validarPermisoAlta('Secciones') ? 'auto' : 'none');

    $('#btnNuevo').click(function () {
        var idArea = $('#selectFormulario_Area').val();
        var nombreArea = $.grep(getUsuarioLogeado().Areas, function (x) { return x.Id == idArea })[0].Nombre;

        crearDialogoTareaNueva({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function (entity) {
                tareas.push(entity);
                filtrarBusqueda();
            },
            IdArea: idArea,
            NombreArea: nombreArea
        });
    })
};

function initConsulta(data) {
    var areas = getUsuarioLogeado().Areas;
    //Cargo las areas
    if (areas != undefined) {
        //Cargo los datos
        $('#selectFormulario_Area').CargarSelect({
            Data: areas,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true
        });
    }
}

$(function () {
    setTimeout(function () {
        calcularCantidadDeRows();
    });
});

function filtrarBusqueda() {
    var resultados = [];

    dadosDeBaja = $('#check_IncluirDeBaja').is(':checked');

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

    $.each(tareas, function (index, tarea) {

        if (tarea.Nombre.toUpperCase().indexOf(texto) != -1) {
            if (tarea.IdArea != idArea) {
                return;
            }

            if (!dadosDeBaja && tarea.FechaBaja != null) {
                return;
            }

            resultados.push(tarea);
        }
    });

    cargarResultadoConsulta(resultados);
}

function initTablaResultadoConsulta() {
    $('#tabla').DataTableTareas({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackActualizar: function (tarea) {
            actualizarLista(tarea);
        },
    });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function actualizarLista(tarea) {
    var index = -1;
    $.each(tareas, (function (idx, data) {
        if (data.Id == tarea.Id) {
            index = idx;
        }

        if (index == -1) {
            return;
        }

        tareas[index] = tarea;
    }));
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();

    console.log(rows);
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

function actualizarRowEnGrilla(tarea) {
    //Busco el indice de la tarea a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == tarea.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(tarea);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Actualizo la info nueva en el array de arriba
    $.each(tareas, function (index, element) {
        if (element.Id == tarea.Id) {
            tareas[index] = tarea;
            return;
        }
    });
}

/* Mensajes */
function mostrarMensaje(tipo, mensaje) {
    switch (tipo) {
        case 'Info':
            Materialize.toast(mensaje, 5000);
            break;

        case 'Alerta':
            Materialize.toast(mensaje, 5000, 'colorAlerta');
            break;

        case 'Error':
            Materialize.toast(mensaje, 5000, 'colorError');
            break;

        case 'Exito':
            Materialize.toast(mensaje, 5000, 'colorExito');
            break;
    }
}