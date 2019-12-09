
var secciones;
var usuario;
var permitirEditar;

var idArea = -1;

$(function () {
    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();

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

    $("#btnBuscar").click(function () {
        buscar();
    });

    $("input").on('keydown', function (e) {
        if (e.keyCode == 13) {
            $(this).focus();
            $(".ui-menu-item").hide();
            buscar();
        }
    });
});

function buscar() {
    var filtros = getFiltros();
    if (typeof (filtros) === "boolean" && !filtros) {
        mostrarMensaje('Error', 'Debe ingresar al menos algún filtro.');
        return;
    }

    mostrarCargando(true);

    crearAjax({
        Data: { filtros: filtros },
        Url: ResolveUrl('~/Servicios/UsuarioService.asmx/GetByFilters'),
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            if (result.Return == undefined || result.Return.length == 0) {
                mostrarMensaje('Info', 'No hay usuarios que cumplan con los filtros');
                cargarResultadoConsulta([]);
                return;
            }

            cargarResultadoConsulta(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', result.Error);
        }
    });
}

function getFiltros() {
    var filtros = {};
    var tieneValor = false;

    var nombre = $("#input_Nombre").val();
    if (nombre != "") {
        filtros.Nombre = nombre;
        tieneValor = true;
    }

    var apellido = $("#input_Apellido").val();
    if (apellido != "") {
        filtros.Apellido = apellido;
        tieneValor = true;
    }

    var dni = $("#input_Dni").val();
    if (dni != "") {
        filtros.Dni = dni;
        tieneValor = true;
    }

    var email = $("#input_Email").val();
    if (email != "") {
        filtros.Email = email;
        tieneValor = true;
    }

    var username = $("#input_Username").val();
    if (username != "") {
        filtros.Username= username;
        tieneValor = true;
    }

    if (!tieneValor) return tieneValor;

    return filtros;
}

function initTablaResultadoConsulta() {
    $('#tabla').DataTableUsuario({
        BotonEditar: validarPermisoAlta('UsuarioConsulta') && validarPermisoModificacion('UsuarioConsulta'),
        CallbackEditar: function (seccion) {
            actualizarLista(seccion);
        },
        BotonEditarOculto: false,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        BotonDarDeBaja: validarPermisoAlta('UsuarioConsulta') && validarPermisoModificacion('UsuarioConsulta'),
        CallbackDarDeBaja: function (seccion) {
            actualizarLista(seccion);
        },
        BotonDarDeAlta: validarPermisoAlta('UsuarioConsulta') && validarPermisoModificacion('UsuarioConsulta'),
        CallbackDarDeAlta: function (usuario) {
            actualizarLista(usuario);
        },
    });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function actualizarLista(usuario) {
    var index = -1;
    $.each(secciones, (function (idx, data) {
        if (data.Id == usuario.Id) {
            index = idx;
        }

        if (index == -1) {
            return;
        }

        secciones[index] = usuario;
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

function actualizarRowEnGrilla(zona) {
    //Busco el indice de la zona a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == zona.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(zona);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Actualizo la info nueva en el array de arriba
    $.each(secciones, function (index, element) {
        if (element.Id == zona.Id) {
            secciones[index] = zona;
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