let moviles;
let movilesFiltrados;

let estados;
let idArea = -1;
let idEstado = -1;
let dadosDeBaja = false;

function init(data) {
    data = parse(data);

    if (data == undefined) {
        mostrarMensaje('Error', 'Error cargando la página');
        return;
    }

    estados = data.Estados;

    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();

    if (usuarioLogeado.Areas.Count != 0) {
        //Cargo los datos
        $('#selectFormulario_Area').CargarSelect({
            Data: usuarioLogeado.Areas,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true
        });
    }

    $('#selectFormulario_Estado').CargarSelect({
        Data: estados,
        Value: 'Id',
        Default: 'Todos',
        Text: 'Nombre',
        Sort: true
    });

    $('#selectFormulario_Area').on('change', function (e) {
        idArea = parseInt($('#selectFormulario_Area').val());
        buscar();
    });

    $('#selectFormulario_Estado').on('change', function (e) {
        idEstado = parseInt($('#selectFormulario_Estado').val());
        filtrar();
    });

    $('#check_IncluirDeBaja').on('change', function (e) {
        dadosDeBaja = $(this).is(':checked') ? null : false;
        buscar();
    });

    $('#btnNuevoTipo').css('display', validarPermisoAlta('Moviles') ? 'auto' : 'none');
    $("#btnNuevoTipo").click(function () {
        crearDialogoTipoMovilNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje)
            }
        });
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

    $('#btnNuevo').css('display', validarPermisoAlta('Moviles') ? 'auto' : 'none');
    $('#btnNuevo').click(function () {
        var idArea = $("#selectFormulario_Area").val();
        crearDialogoMovilNuevo({
            IdArea: idArea,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function (entity) {
                actualizarEnTabla(entity);
            }
        });
    });

};

function buscar() {

    mostrarCargando(true);

    var data = { IdArea: $("#selectFormulario_Area").val(), DadosDeBaja: dadosDeBaja };

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/GetResultadoTablaByFilters'),
        Data: { consulta: data },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            moviles = result.Return.Data;
            filtrar(moviles);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', result.Error);
        }
    });
}

function initTablaResultadoConsulta() {
    dt = $('#tabla').DataTableMovil({
        Orden: [[0, 'asc']],
        Buscar: true,
        InputBusqueda: '#inputBusqueda',
        ColumnaEstado: true,
        Callback: function (movil) {
            actualizarEnTabla(movil);
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        ColumnasITVTUV: function () {
            return validarPermisoModificacion('Moviles');
        }
    });

    dt.$('.tooltipped').tooltip({ delay: 50 });
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
    movilesFiltrados = data;
    calcularCantidadDeRows();
    dt.clear().draw();
    if (data != null) {
        dt.rows.add(data).draw();
    }
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function filtrar() {
    if (idEstado == -1) {
        cargarTabla(moviles);
        return;
    }

    cargarTabla($.grep(moviles, function (a) {
        return a.IdEstado == idEstado && (dadosDeBaja || a.FechaBaja == null);
    }));
}

function actualizarEnTabla(entity) {
    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/GetResultadoTablaById'),
        Data: { id: entity.Id },
        OnSuccess: function (result) {
            if (!result.Ok) return;

            actualizar(result.Return);

        },
        OnError: function (result) { }
    });

    function actualizar(movil) {
        var index = -1;
        dt.rows(function (idx, data, node) {
            if (data.Id == movil.Id) {
                index = idx;
            }
        });

        if (index == -1) {
            moviles.push(movil);
            dt.row.add(movil).draw(false);
            return;
        }

        //if (movil.FechaBaja != null) {
        //    dt.row(index).remove();
        //    return;
        //}

        dt.row(index).data(movil);

        dt.$('.tooltipped').tooltip({ delay: 50 });

        $.each(moviles, function (index, element) {
            if (element.Id == movil.Id) {
                moviles[index] = movil;
                return;
            }
        });
    }
}
