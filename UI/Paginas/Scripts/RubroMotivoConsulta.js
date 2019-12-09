var rubros;
var grupos;
var usuario;
var permitirEditar;

var idGrupo = -1;

function init(data) {
    data = parse(data);

    if (data.Error != undefined) {
        mostrarMensaje('Error', data.Error);
        return;
    }

    if (data.RubrosMotivo != undefined) {
        rubros = data.RubrosMotivo;
    }

    $('#selectFormulario_Grupo').prop("disabled", true);

    //Cargo las areas
    if (data.Grupos != undefined && data.Grupos.length != 0) {
        grupos = data.Grupos;
        $('#selectFormulario_Grupo').prop("disabled", false);
        //Cargo los datos
        $('#selectFormulario_Grupo').CargarSelect({
            Data: grupos,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true
        });
    }

    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();

    $('#selectFormulario_Grupo').on('change', function (e) {
        var a = $('#selectFormulario_Grupo').val();
        idGrupo = parseInt(a);

        if (idGrupo == -1) {
            cargarResultadoConsulta([]);
            return;
        }

        var rubrosFiltradas = _.where(rubros, { GrupoRubroId: idGrupo })
        cargarResultadoConsulta(rubrosFiltradas);
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

    $('#selectFormulario_Grupo').trigger('change');

    $('#btnNuevo').css('display', validarPermisoAlta('RubrosMotivo') ? 'auto' : 'none');

    $('#btnNuevo').click(function () {
        var idGrupo = $('#selectFormulario_Grupo').val();

        if (idGrupo == "-1" || idGrupo == null) {
            mostrarMensaje("Info", "Debe seleccionar un grupo para crearle a éste un rubro");
            return;
        }

        var nombreGrupo = _.find(grupos, function (g) { return g.Id == idGrupo; }).Nombre;

        crearDialogoRubroMotivoNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function (entity) {
                rubros.push(entity);
                $('#selectFormulario_Grupo').trigger("change");
                filtrarBusqueda();
            },
            IdGrupo: idGrupo,
            NombreGrupo: nombreGrupo
        });
    })

    $('#btnNuevoGrupo').click(function () {
        crearDialogoGrupoRubroMotivoNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            CallbackGrupoNuevo: function (g) {
                grupos = g;

                var deshabilitar = grupos.length == 0;
                $('#selectFormulario_Grupo').prop("disabled", deshabilitar);

                //Cargo los tipos
                $('#selectFormulario_Grupo').CargarSelect({
                    Data: grupos,
                    Value: 'Id',
                    Text: 'Nombre',
                    Sort: true
                });
            }
        });
    })
};

$(function () {
    setTimeout(function () {
        calcularCantidadDeRows();
    });
});

function initTablaResultadoConsulta() {
    $('#tabla').DataTableRubroMotivo({
        Callback: function (cat) {
            actualizarLista(cat);
        },
        InputBusqueda: '#inputBusqueda',
        BotonBorrar: true,
        CallbackBorrar: function (elemento) {
            quitar(elemento.Id);
        }
    });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function actualizarLista(rubro) {
    var index = -1;
    $.each(rubros, (function (idx, data) {
        if (data.Id == rubro.Id) {
            index = idx;
        }

        if (index == -1) {
            return;
        }

        if (rubro.FechaBaja != null) {
            rubros.splice(index, 1);
            return;
        }

        rubros[index] = rubro;
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
    $.each(rubros, function (index, element) {
        if (element.Id == zona.Id) {
            rubros[index] = zona;
            return;
        }
    });
}

function quitar(id) {
    crearDialogoConfirmacion({
        Texto: "¿Está seguro de que desea eliminar éste rubro?",
        CallbackPositivo: function () {
            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/RubroMotivoService.asmx/DarDeBaja'),
                Data: { id: id },
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