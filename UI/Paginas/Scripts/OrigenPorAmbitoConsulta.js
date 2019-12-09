var origenes;
var ambitos;
var dataTabla;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    origenes = data.Origenes;
    ambitos = data.Ambitos;
    dataTabla = [];

    $.each(ambitos, function (indexAmbito, ambito) {
        var busqueda = $.grep(origenes, function (origen, indexOrigen) {
            return origen.AmbitoId == ambito.Id;
        });

        dataTabla[indexAmbito] = ambito;

        if (busqueda != undefined && busqueda.length != 0) {
            dataTabla[indexAmbito].OrigenPorAmbitoId = busqueda[0].Id;
            dataTabla[indexAmbito].OrigenId = busqueda[0].OrigenId;
            dataTabla[indexAmbito].OrigenNombre = busqueda[0].OrigenNombre;
        }
    });


    //------------------------------------
    // Tabla 
    //------------------------------------

    initTabla();
    cargarTabla(dataTabla);

    //------------------------------------
    // Filtros
    //------------------------------------

    filtrarBusqueda();

    $('#inputBusqueda').on('input', function () {
        filtrarBusqueda();
    });

    $('#btnNuevo').click(function () {
        crearDialogoOrigenPorAmbitoNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function (origen) {
                $.each(dataTabla, function (index, element) {
                    if (element.Id == origen.AmbitoId) {
                        element.OrigenPorAmbitoId = origen.Id;
                        element.OrigenId = origen.OrigenId;
                        element.OrigenNombre = origen.OrigenNombre;
                    }
                });

                filtrarBusqueda();
            }
        });
    })

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

    var texto = $('#inputBusqueda').val().trim().toUpperCase();


    var resultados = [];
    $.each(dataTabla, function (index, itemTabla) {
        if (itemTabla.Nombre.toUpperCase().indexOf(texto) != -1) {
            resultados.push(itemTabla);
            return true;
        }

        if (itemTabla.OrigenNombre != undefined) {
            if (itemTabla.OrigenNombre.toUpperCase().indexOf(texto) != -1) {
                resultados.push(itemTabla);
                return true;
            }
        }

    });

    cargarTabla(resultados);

}

function initTabla() {
    $('#tabla').DataTableGeneral({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Orden: [[0, 'asc']],
        Columnas: [
           {
               title: 'Ámbito',
               data: 'Nombre',
               render: function (data, type, row) {
                   return "<div><span>" + toTitleCase(data) + "</span></div>";
               }
           },
           {
               title: 'Origen',
               data: 'OrigenNombre',
               render: function (data, type, row) {
                   if (data == undefined) {
                       return "<div><span></span></div>";
                   } else {
                       return "<div><span>" + toTitleCase(data) + "</span></div>";
                   }
               }
           }
        ],
        Botones: [
            {
                Titulo: 'Editar',
                Icono: 'edit',
                OnClick: function (data) {
                    crearDialogoOrigenPorAmbitoNuevo({
                        IdAmbito: data.Id,
                        IdOrigen: data.OrigenId,
                        CallbackMensajes: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        Callback: function (origen) {
                            $.each(dataTabla, function (index, element) {
                                if (element.Id == origen.AmbitoId) {
                                    element.OrigenPorAmbitoId = origen.Id;
                                    element.OrigenId = origen.OrigenId;
                                    element.OrigenNombre = origen.OrigenNombre;
                                }
                            });

                            filtrarBusqueda();
                        }
                    });
                }
            },
            {
                Titulo: 'Dar de baja',
                Icono: 'delete',
                Oculto: true,
                Visible: function (data) {
                    if ('OrigenPorAmbitoId' in data && data.OrigenPorAmbitoId != undefined) {
                        return true;
                    }

                    return false;
                },
                OnClick: function (data) {
                    crearDialogoOrigenPorAmbitoDarDeBaja({
                        Id: data.OrigenPorAmbitoId,
                        CallbackMensajes: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        Callback: function (origen) {
                            $.each(dataTabla, function (index, element) {
                                if (element.Id == origen.AmbitoId) {
                                    element.OrigenPorAmbitoId = undefined;
                                    element.OrigenId = undefined;
                                    element.OrigenNombre = undefined;
                                }
                            });

                            filtrarBusqueda();
                        }
                    });
                }
            }
        ]
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
    $.each(dataTabla, function (index, element) {
        if (element.Id == entity.Id) {
            origenes[index] = entity;
            return;
        }
    });
}
