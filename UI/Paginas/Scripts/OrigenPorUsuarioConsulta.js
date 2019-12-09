var origenes;
var usuarios;
var dataTabla;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    origenes = data.Origenes;
    usuarios = data.Usuarios;
    dataTabla = [];

    $.each(usuarios, function (indexUsuario, usuario) {
        var busqueda = $.grep(origenes, function (origen, indexOrigen) {
            return origen.UsuarioOrigenId == usuario.Id;
        });

        dataTabla[indexUsuario] = usuario;


        if (busqueda != undefined && busqueda.length != 0) {
            dataTabla[indexUsuario].OrigenPorUsuarioId = busqueda[0].Id;
            dataTabla[indexUsuario].OrigenId = busqueda[0].OrigenId;
            dataTabla[indexUsuario].OrigenNombre = busqueda[0].OrigenNombre;
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
        crearDialogoOrigenPorUsuarioNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function (origen) {
                $.each(dataTabla, function (index, element) {
                    if (element.Id == origen.UsuarioOrigenId) {
                        element.OrigenPorUsuarioId = origen.Id;
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
    var palabras = texto.split(' ');


    var resultados = [];
    $.each(dataTabla, function (index, itemTabla) {
        var yaEsta = false;

        $.each(palabras, function (index, palabra) {
            if (yaEsta) return false;

            if (itemTabla.Nombre != undefined) {
                if (itemTabla.Nombre.toUpperCase().indexOf(palabra) != -1) {
                    resultados.push(itemTabla);
                    yaEsta = true;
                    return true;
                }
            }

            if (itemTabla.Apellido != undefined) {
                if (itemTabla.Apellido.toUpperCase().indexOf(palabra) != -1) {
                    resultados.push(itemTabla);
                    yaEsta = true;
                    return true;
                }
            }

            if (itemTabla.Email != undefined) {
                if (itemTabla.Email.toUpperCase().indexOf(palabra) != -1) {
                    resultados.push(itemTabla);
                    yaEsta = true;
                    return true;
                }
            }

            if (itemTabla.Dni != undefined) {
                if (itemTabla.Dni == palabra) {
                    resultados.push(itemTabla);
                    yaEsta = true;
                    return true;
                }
            }

            if (itemTabla.OrigenNombre != undefined) {
                if (itemTabla.OrigenNombre.toUpperCase().indexOf(texto) != -1) {
                    resultados.push(itemTabla);
                    yaEsta = true;
                    return true;
                }
            }
        });
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
               title: 'Nombre',
               data: 'Nombre',
               render: function (data, type, row) {
                   return "<div><span>" + toTitleCase(data) + "</span></div>";
               }
           },
           {
               title: 'Apellido',
               data: 'Apellido',
               render: function (data, type, row) {
                   return "<div><span>" + toTitleCase(data) + "</span></div>";
               }
           },
           {
               title: 'N° Doc',
               data: 'Dni',
               render: function (data, type, row) {
                   if (data == 0) return "";
                   return "<div><span>" + data + "</span></div>";
               }
           },
           {
               title: 'E-Mail',
               data: 'Email',
               render: function (data, type, row) {
                   if (data == 0 || data == undefined || data == null) return "";
                   return "<div><span>" + data + "</span></div>";
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
                    crearDialogoOrigenPorUsuarioNuevo({
                        IdUsuario: data.Id,
                        IdOrigen: data.OrigenId,
                        CallbackMensajes: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        Callback: function (origen) {
                            $.each(dataTabla, function (index, element) {
                                if (element.Id == origen.UsuarioOrigenId) {
                                    element.OrigenPorUsuarioId = origen.Id;
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
                    if ('OrigenPorUsuarioId' in data && data.OrigenPorUsuarioId != undefined) {
                        return true;
                    }

                    return false;
                },
                OnClick: function (data) {
                    crearDialogoOrigenPorUsuarioDarDeBaja({
                        Id: data.OrigenPorUsuarioId,
                        CallbackMensajes: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        Callback: function (origen) {
                            $.each(dataTabla, function (index, element) {
                                if (element.Id == origen.UsuarioOrigenId) {
                                    element.OrigenPorUsuarioId = undefined;
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
