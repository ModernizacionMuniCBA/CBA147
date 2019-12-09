var tiposDocumento;
var barrios;
var cpc;
var usuario;

function init(data) {
    data = parse(data);

    //Usuario
    usuario = data.Usuario;
    initUsuario();

    //-------------------------
    // Inicializo
    //-------------------------

    initConsulta(data);
    initResultado();

    //----------------------
    // Anim inicial
    //----------------------
    $('#cardConsulta').css('opacity', 0);
    $('#cardConsulta').css('top', '50px');

    setTimeout(function () {
        $('#cardConsulta').animate({ opacity: 1, top: '0px' }, 300);
    }, 500);
}

function initUsuario() {

}

function initConsulta(data) {
    //-----------------------------------
    // Cargo los datos desde el servidor
    //-----------------------------------

    //Enter en el numero
    $('#input_NumeroDocumento, #input_Cuil, #input_Apellido, #input_Nombre').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnBuscar').click();
        }
    })


    //Tipo Doc
    tiposDocumento = data.TiposDocumento;
    $('#select_TipoDocumento').CargarSelect({
        Data: data.TiposDocumento,
        Value: 'Id',
        Text: 'Nombre',
        Default: 'Seleccione...',
        TitleCase: false,
        Sort: true
    });

    //Barrios
    barrios = data.Barrios;
    $('#select_Barrio').CargarSelect({
        Data: data.Barrios,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    //CPC
    cpc = data.CPC;
    $('#select_CPC').CargarSelect({
        Data: data.CPC,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre'
    });

    //evento click Consultar
    $('#btnBuscar').click(function () {
        if (!validarConsulta()) {
            return;
        }

        consultar();
    });
}

function initResultado() {

    initTablaResultadoConsulta();

    //evento click volver
    $('#btnVolverAConsulta').click(function () {
        mostrarCardConsulta();
    });

    //Imprimir
    $('#btnImprimir').click(function () {


    });
}

function validarConsulta() {

    $('#cardConsulta').find('.control-observacion').hide(300);

    var hayAlgunFiltro = false;

    //Tipo Documento
    var idTipoDocumento = "" + $('#select_TipoDocumento').val();
    if (idTipoDocumento != -1) {
        hayAlgunFiltro = true;
    }

    //Numero
    var numero = "" + $('#input_NumeroDocumento').val();
    if (numero != "") {
        hayAlgunFiltro = true;
    }

    //Cuil
    var cuil = "" + $('#input_Cuil').val();
    if (cuil != "") {
        hayAlgunFiltro = true;
    }

    //Nombre
    var nombre = "" + $('#input_Nombre').val();
    if (nombre != "") {
        hayAlgunFiltro = true;
    }

    //Apellido
    var apellido = "" + $('#input_Apellido').val();
    if (apellido != "") {
        hayAlgunFiltro = true;
    }

    //CPC
    if ($('#select_CPC').val() != undefined && $('#select_CPC').val() != -1) {
        hayAlgunFiltro = true;
    }

    //Barrio
    if ($('#select_Barrio').val() != undefined && $('#select_Barrio').val() != -1) {
        hayAlgunFiltro = true;
    }

    if (!hayAlgunFiltro) {
        Materialize.toast("Debe seleccionar al menos un filtro.", 5000, "colorAlerta");
        return false;
    }

    return true;
}

function getFiltrosConsulta() {
    var filtros = {};

    //Tipo Documento
    var idTipoDocumento = "" + $('#select_TipoDocumento').val();
    if (idTipoDocumento != -1) {
        filtros.IdTipoDocumento = idTipoDocumento;
    }

    //Numero
    var nro = "" + $('#input_NumeroDocumento').val();
    if (nro != "") {
        filtros.NumeroDocumento = nro;
    }

    //Cuil
    var cuil = "" + $('#input_Cuil').val();
    if (cuil != "") {
        filtros.Cuil = cuil;
    }

    //Nombre
    var nombre = "" + $('#input_Nombre').val();
    if (nombre != "") {
        filtros.Nombre = nombre;
    }

    //Apellido
    var apellido = "" + $('#input_Apellido').val();
    if (apellido != "") {
        filtros.Apellido = apellido;
    }

    //CPC
    if ($('#select_CPC').val() != -1) {
        filtros.IdCPC = "" + $('#select_CPC').val();
    }

    //Barrio
    if ($('#select_Barrio').val() != -1) {
        filtros.IdBarrio = "" + $('#select_Barrio').val();
    }

    //Relevamiento Oficio
    if (!$('#radio_SexoAmbos').is(':checked')) {
        if ($('#radio_SexoMasculino').is(':checked')) {
            filtros.SexoMasculino = true;
        } else {
            filtros.SexoMasculino = false;
        }
    }

    return filtros;
}

function getTextoFiltrosConsulta() {
    var filtros = "";
    $.each(getFiltrosConsulta(), function (key, val) {
        switch (key) {
            case 'IdTipoDocumento': {
                key = 'Tipo Documento';
                if (val == -1 || val == null) return true;
                val = $.grep(tiposDocumento, function (e) { return e.Id == val; })[0].Nombre;
            } break;

            case 'SexoMasculino': {
                key = 'Sexo';
                if (val == null) return true
                val = val ? 'Masculino' : 'Femenino';
            } break;

            case 'IdCPC': {
                key = 'CPC';
                val = toTitleCase($.grep(cpc, function (e) { return e.Id == val; })[0].Nombre);
            } break;

            case 'IdBarrio': {
                key = 'Barrio';
                val = toTitleCase($.grep(barrios, function (e) { return e.Id == val; })[0].Nombre);
            } break;
        }
        if (filtros != "") {
            filtros += " ";
        }
        filtros += '<b>' + key + "</b> " + val;
    });

    return 'Filtros<br/>' + filtros;
}

function consultar() {


    //Muestro cargando
    $('#cardConsulta').find('.cargando').stop(true, true).fadeIn(500);

    //Consulto
    var filtros = getFiltrosConsulta();
    var dataAjax = { filtros: JSON.stringify(filtros) };
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/Servicios/PersonaFisicaService.asmx/GetByFilters'),
        success: function (result) {
            result = parse(result.d);

            //Oculto el cargando
            $('#cardConsulta').find('.cargando').stop(true, true).fadeOut(500);

            //algo salio mal
            if ('Error' in result) {

                //Informo
                Materialize.toast('Error al realizar la consulta', 5000, 'colorError');
                console.log("Error al realizar la consulta");
                console.log("Ajax");
                console.log(parametros);
                console.log("result");
                console.log(result);
                return;
            }

            //No hay resultados
            if (result.Personas.length == 0) {
                //Informo
                Materialize.toast("No hay personas que coincidan con los filtros de búsqueda.", 5000);
                return;
            }

            cargarResultadoConsulta(result.Personas);
            mostrarCardResultado();
        },
        error: function (result) {
            //Oculto el cargando
            $('#cardConsulta').find('.cargando').stop(true, true).fadeOut(500);

            //Informo
            Materialize.toast('Error al realizar la consulta', 5000, 'colorError');
            console.log("Error al realizar la consulta");
            console.log("Ajax");
            console.log(parametros);
            console.log("result");
            console.log(result);
        }
    });
}

function mostrarCardConsulta() {
    $('#cardResultadoConsulta').fadeOut(300, function () {
        $('#cardConsulta').fadeIn(300);
    });
}

function mostrarCardResultado() {
    $('#filtros').html(getTextoFiltrosConsulta());
    $('#cardConsulta').fadeOut(300, function () {
        $('#cardResultadoConsulta').fadeIn(300, function () {
            calcularCantidadDeRows();
        });
    });
}

function initTablaResultadoConsulta() {
    $('#tablaResultadoConsulta').DataTablePersonaFisica({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        BotonEditar: true,
        BotonEditarOculto: false,
        CallbackEditar: function (persona) {
            actualizarRowEnGrilla(persona)
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

    var dt = $('#tablaResultadoConsulta').DataTable();
    dt.page.len(rows).draw();
}

function cargarResultadoConsulta(data) {
    var dt = $('#tablaResultadoConsulta').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function actualizarRowEnGrilla(persona) {
    //Busco el indice de la persona a actualizar
    var index = -1;
    var dt = $('#tablaResultadoConsulta').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == persona.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(persona);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });
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

/* Error */
