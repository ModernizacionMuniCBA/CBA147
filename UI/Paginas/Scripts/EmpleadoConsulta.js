var empleados = [];
var funciones = [];

var dadosDeBaja=false;

function init(data) {
    data = parse(data);

    if(data.Funciones!=undefined){
        funciones = data.Funciones;
    }

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

    $('#selectFormulario_Area').on('change', function (e) {
       var idArea = parseInt($('#selectFormulario_Area').val());

        var funcionesDelArea = $.map(funciones, function (obj)
        {
            if (obj.IdArea == idArea) return obj;
        });

        cargarFunciones(funcionesDelArea);
        buscar();
    });

    $('.btnNuevaFuncion').click(function () {
        var idArea = $("#selectFormulario_Area").val();

        crearDialogoFuncionNueva({
            IdArea: idArea,
            Callback: function (list) {
                funciones = list;

                //Cargo las funciones
                $('#selectFormulario_Funcion').CargarSelect({
                    Data: list,
                    Value: 'Id',
                    Text: 'Nombre',
                    Default: 'Sin Funciones',
                    Multiple: true,
                    Sort: true
                });

                buscar();
            },
            CallbackMensajes: function () { }
        });
    });

    $('#check_IncluirDeBaja').on('change', function (e) {
        dadosDeBaja = $(this).is(':checked') ? null : false;
        buscar();
    });

    //------------------------------------
    // Anim inicio
    //------------------------------------

    $('#cardFormularioFiltros').css('opacity', 0);
    $('#cardFormularioFiltros').css('top', '50px');

    setTimeout(function () {
        $('#cardFormularioFiltros').animate({ 'opacity': 1, 'top': '0px' }, 500);
    }, 200);

    $('#cardResultado').css('opacity', 0);
    $('#cardResultado').css('top', '50px');

    setTimeout(function () {
        $('#cardResultado').animate({ 'opacity': 1, 'top': '0px' }, 500);
    }, 600);

    $('#selectFormulario_Area').trigger('change');

    $('#btnNuevo').css('display', validarPermisoAlta('PersonalConfiguracion') ? 'auto' : 'none');
    $('#btnNuevo').click(function () {
        var idArea = $("#selectFormulario_Area").val();
        crearDialogoEmpleadoNuevo({
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
    $("#cardFormularioFiltros .cargando").show();
    var data = { IdArea: $("#selectFormulario_Area").val(), DadosDeBaja: dadosDeBaja };

    crearAjax({
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/GetResultadoTablaByFilters'),
        Data: { consulta: data },
        OnSuccess: function (result) {
            $("#cardFormularioFiltros .cargando").hide();

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            empleados = result.Return.Data;
            cargarTabla(empleados);
        },
        OnError: function (result) {
            $("#cardFormularioFiltros .cargando").hide();
            mostrarMensaje('Error', result.Error);
        }
    });
}

function initTablaResultadoConsulta() {
    dt = $('#tabla').DataTableEmpleado({
        Funciones: function () { return funciones;},
        Orden: [[0, 'asc']],
        Buscar: true,
        InputBusqueda: '#inputBusqueda',
        InputBusquedaSelect: '#selectFormulario_Funcion',
        ColumnaEstado: true,
        Callback: function (empleado) {
            actualizarEnTabla(empleado);
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
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
    calcularCantidadDeRows();
    dt.clear().draw();
    if (data != null) {
        dt.rows.add(data).draw();
    }
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function cargarFunciones(funcionesDelArea) {
    if (funcionesDelArea.lenght == 0) {
        $("#selectFormulario_Funcion").prop("disabled", true);
        return;
    }

    $("#selectFormulario_Funcion").prop("disabled", false);
    $('#selectFormulario_Funcion').CargarSelect({
        Data: funcionesDelArea,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true,
        Multiple: true,
        Default: 'Sin Funciones'
    });
}

function actualizarEnTabla(entity) {
    crearAjax({
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/GetResultadoTablaById'),
        Data: { id: entity.Id },
        OnSuccess: function (result) {
            if (!result.Ok) return;

            actualizar(result.Return);

        },
        OnError: function (result) { }
    });

    function actualizar(empleado) {
        var index = -1;
        dt.rows(function (idx, data, node) {
            if (data.Id == empleado.Id) {
                index = idx;
            }
        });

        if (index == -1) {
            funciones.push(empleado);
            dt.row.add(empleado).draw(false);
            return;
        }

        //if (empleado.FechaBaja != null) {
        //    dt.row(index).remove();
        //    return;
        //}

        dt.row(index).data(empleado);

        dt.$('.tooltipped').tooltip({ delay: 50 });

        $.each(funciones, function (index, element) {
            if (element.Id == empleado.Id) {
                empleado[index] = empleado;
                return;
            }
        });
    }
}
