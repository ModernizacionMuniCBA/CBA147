var edificiosMunicipales = [];
var categorias=[];

function init(data) {
    data = parse(data);

    if(data.Categorias !=undefined){
        funciones = data.Categorias;
    }

    initTablaResultadoConsulta();

    
    if (categorias.Count != 0) {
        //Cargo los datos
        $('#selectFormulario_Categoria').CargarSelect({
            Data: data.Categorias,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true
        });
    }


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


    //$('#btnNuevo').css('display', validarPermisoAlta('EdificiosMunicipales') ? 'auto' : 'none');
    $('#btnNuevo').click(function () {
        crearDialogoEdificioMunicipalNuevo({
            Callback: function (edificio) {
                //edificiosMunicipales= list;

                actualizarEnTabla(edificio);
            },
            CallbackMensajes: function () { },
            IdCategoria: $("#selectFormulario_Categoria").val()
        });
    });

    $("#selectFormulario_Categoria").on('change', function () {
        buscar();
    });

    $("#selectFormulario_Categoria").trigger('change');
};

function buscar() {
    $("#cardFormularioFiltros .cargando").show();

    var data = { idCategoria: $("#selectFormulario_Categoria").val() }
    crearAjax({
        Url: ResolveUrl('~/Servicios/EdificioMunicipalService.asmx/GetResultadoTabla'),
        Data: data,
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
    var dt = $('#tabla').DataTableEdificioMunicipal({
        Buscar: true,
        BotonBorrar: true,
        CallbackBorrar: function (edificio){
            quitar(edificio.Id);
        },
        CallbackEditar: function (edificio){
            actualizarEnTabla(edificio);
        },
        InputBusqueda: '#inputBusqueda',
        Callback: function (edificio) {
            actualizarEnTabla(edificio);
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
    var dt = $('#tabla').DataTable();
    calcularCantidadDeRows();
    dt.clear().draw();
    if (data != null) {
        dt.rows.add(data).draw();
    }
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function actualizarEnTabla(entity) {
    //si no es de la misma categoria que está filtrando, paso
    if ($('#selectFormulario_Categoria').val() != entity.IdCategoria) {
        borrarFila("#tabla", entity.Id);
        return;
    }

    crearAjax({
        Url: ResolveUrl('~/Servicios/EdificioMunicipalService.asmx/GetResultadoTablaById'),
        Data: { id: entity.Id },
        OnSuccess: function (result) {
            if (!result.Ok) return;

            actualizar(result.Return);

        },
        OnError: function (result) { }
    });

    function actualizar(edificio) {
        var dt = $('#tabla').DataTable();
        var index = -1;
        dt.rows(function (idx, data, node) {
            if (data.Id == edificio.Id) {
                index = idx;
            }
        });


        if (index ==-1) {
            dt.row.add(edificio).draw();
            edificiosMunicipales.push(edificio);
            return;
        }

        dt.row(index).data(edificio);

        dt.$('.tooltipped').tooltip({ delay: 50 });

        $.each(edificiosMunicipales, function (index, element) {
            if (element.Id == edificio.Id) {
                edificiosMunicipales[index] = edificio;
                return;
            }
        });
    }
}

function quitar(id) {
    crearDialogoConfirmacion({
        Texto: "¿Está seguro de que desea eliminar éste edificio?",
        CallbackPositivo: function () {
            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/EdificioMunicipalService.asmx/DarDeBaja'),
                Data: { id: id},
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    borrarFila("#tabla", id);
                    mostrarCargando(false);
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}

