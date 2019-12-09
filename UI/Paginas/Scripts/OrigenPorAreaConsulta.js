var origenes;
var areas;
var dataTabla;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    origenes = data.Origenes;
    areas = data.Areas;
    dataTabla = [];

    $.each(areas, function (indexArea, area) {
        var busqueda = $.grep(origenes, function (origen, indexOrigen) {
            return origen.AreaId == area.Id;
        });

        dataTabla[indexArea] = area;

        if (busqueda != undefined && busqueda.length != 0) {
            dataTabla[indexArea].OrigenPorAreaId = busqueda[0].Id;
            dataTabla[indexArea].OrigenId = busqueda[0].OrigenId;
            dataTabla[indexArea].OrigenNombre = busqueda[0].OrigenNombre;
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
        crearDialogoOrigenPorAreaNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function (origen) {
                $.each(dataTabla, function (index, element) {
                    if (element.Id == origen.AreaId) {
                        element.OrigenPorAreaId = origen.Id;
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
               title: 'Area',
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
                    crearDialogoOrigenPorAreaNuevo({
                        IdArea: data.Id,
                        IdOrigen: data.OrigenId,
                        CallbackMensajes: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        Callback: function (origen) {
                            $.each(dataTabla, function (index, element) {
                                if (element.Id == origen.AreaId) {
                                    element.OrigenPorAreaId = origen.Id;
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
                Oculto:true,
                Visible: function(data){
                    if('OrigenPorAreaId' in data && data.OrigenPorAreaId!=undefined){
                        return true;
                    }

                    return false;
                },
                OnClick: function (data) {
                    crearDialogoOrigenPorAreaDarDeBaja({
                        Id: data.OrigenPorAreaId,
                        CallbackMensajes: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        Callback: function (origen) {
                            $.each(dataTabla, function (index, element) {
                                if (element.Id == origen.AreaId) {
                                    element.OrigenPorAreaId = undefined;
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
