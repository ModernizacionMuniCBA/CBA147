var ATIs = [];

var usuariosATI = [];

function init(data) {
    data = parse(data);

    if ('Error' in data) {
        mostrarMensajeCritico({});
        return;
    }

    if (data.UsuariosATI != undefined) {
        usuariosATI = data.UsuariosATI;
    }

    ATIs = data.ATIs;

    initTablaResultadoConsulta();

    
    if (usuariosATI.Count != 0) {
        //Cargo los datos
        $('#selectFormulario_ATI').CargarSelect({
            Data: data.ATIs,
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


    //$('#btnNuevo').css('display', validarPermisoAlta('UsuariosATI') ? 'auto' : 'none');
    $('#btnNuevo').click(function () {
        crearDialogoUsuarioPorATINuevo({
            Callback: function (usuario) {
                //usuariosATI= list;

                actualizarEnTabla(usuario);
            },
            CallbackMensajes: function () { },
            IdATI: $("#selectFormulario_ATI").val()
        });
    });

    $("#selectFormulario_ATI").on('change', function () {
        filtrar();
    });

    $("#selectFormulario_ATI").trigger('change');
};

function initTablaResultadoConsulta() {
    var dt = $('#tabla').DataTableUsuarioATI({
        Buscar: true,
        BotonBorrar: true,
        CallbackBorrar: function (usuarioATI){
            quitar(usuarioATI.Id);
        },
        CallbackEditar: function (usuarioATI){
            actualizarEnTabla(usuarioATI);
        },
        InputBusqueda: '#inputBusqueda',
        Callback: function (usuarioATI) {
            actualizarEnTabla(usuarioATI);
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
    if ($('#selectFormulario_ATI').val() != entity.IdATI) {
        borrarFila("#tabla", entity.Id);
        return;
    }

    crearAjax({
        Url: ResolveUrl('~/Servicios/UsuarioATIService.asmx/GetResultadoTablaById'),
        Data: { id: entity.Id },
        OnSuccess: function (result) {
            if (!result.Ok) return;

            actualizar(result.Return);

        },
        OnError: function (result) { }
    });

    function actualizar(usuarioATI) {
        var dt = $('#tabla').DataTable();
        var index = -1;
        dt.rows(function (idx, data, node) {
            if (data.Id == usuarioATI.Id) {
                index = idx;
            }
        });


        if (index ==-1) {
            dt.row.add(usuarioATI).draw();
            usuariosATI.push(usuarioATI);
            return;
        }

        dt.row(index).data(usuarioATI);

        dt.$('.tooltipped').tooltip({ delay: 50 });

        $.each(usuariosATI, function (index, element) {
            if (element.Id == usuarioATI.Id) {
                usuariosATI[index] = usuarioATI;
                return;
            }
        });
    }
}

function quitar(id) {
    crearDialogoConfirmacion({
        Texto: "¿Está seguro de que desea quitarde al usuario ésta Área territorial de incumbencia?",
        CallbackPositivo: function () {
            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/UsuarioATIService.asmx/DarDeBaja'),
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

