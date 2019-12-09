var servicios;
var usuario;
var permitirEditar;



function init(data) {
    data = parse(data);
   

    //------------------------------------
    // Init Datos
    //------------------------------------

    servicios = data.Servicios;
    usuario = data.Usuario;
   

    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();
    cargarResultadoConsulta(servicios);

    //------------------------------------
    // Radio
    //------------------------------------

    filtrarBusqueda();

    $('#rdbTodos').change(function () {
        filtrarBusqueda();
    });

    $('#rdbActivoSi').change(function () {
        filtrarBusqueda();
    });

    $('#rdbActivoNo').change(function () {
        filtrarBusqueda();
    });

    $('#inputBusqueda').on('input', function () {
        filtrarBusqueda();
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
};

$(function () {
    setTimeout(function () {
        calcularCantidadDeRows();
    });
});

function filtrarBusqueda() {
    var resultados = [];

    var estado = 2;
    if ($('#contenedorEstados').lenght != 0) {
        if ($('#rdbTodos').is(':checked')) {
            estado = 1;
        } else {
            if ($('#rdbActivoSi').is(':checked')) {
                estado = 2;
            } else {
                estado = 3;
            }
        }
    }

    var texto = $('#inputBusqueda').val().trim().toUpperCase();

    $.each(servicios, function (index, servicio) {

        if (servicio.Nombre.toUpperCase().indexOf(texto) != -1) {

            switch (estado) {
                case 1:

                    resultados.push(servicio);
                    break;

                case 2:
                    if (servicio.FechaBaja == null) {
                        resultados.push(servicio);
                    }
                    break;

                case 3:
                    if (servicio.FechaBaja != null) {
                        resultados.push(servicio);
                    }
                    break;
            }
        }

    });

    cargarResultadoConsulta(resultados);

}

function initTablaResultadoConsulta() {
    $('#tabla').DataTableGeneral({
        Columnas: [
            {
                title: 'Servicio',
                data: 'Nombre',
                render: function (data, type, row) {
                    return "<div><span>" + toTitleCase(data) + "</span></div>";
                }
            },


            {
                title: 'Estado',
                data: 'FechaBaja',
                render: function (data, type, row) {
                    if (data != null) {
                        return '<div><span>Dado de baja</span></div>';
                    } else {
                        return '<div><span>Activo</span></div>';
                    }


                }
            },
            {
                title: 'Observación',
                data: 'Observaciones',
                render: function (data, type, row) {
                    if (data != null) {
                        return '<div><span>' + data + '</span></div>';
                    }
                }
            },



        ],
        Botones: [
            {
                Titulo: 'Editar',
                Icono: 'edit',
                OnClick: function (data) {

                    crearDialogoIFrame({
                        Titulo: 'Editar Servicio',
                        Url: ResolveUrl('~/IFrame/IServicioNuevo.aspx?Id=' + data.Id),
                        OnLoad: function (jAlert, iFrame, iFrameContent) {

                            //Callback de mensajes
                            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                                mostrarMensaje(tipo, mensaje);
                            });


                            //Callback cargando
                            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                                $(jAlert).MostrarDialogoCargando(cargando, true);
                            });

                            //Callback editar
                            iFrameContent.setOnEditarCompletoListener(function (servicio) {
                                mostrarMensaje('Exito', 'Servicio editada correctamente');
                                actualizarRowEnGrilla(servicio);

                                $(jAlert).CerrarDialogo();
                            });
                        },
                        Botones:
                            [
                                {
                                    Texto: 'Cancelar'
                                },
                                {
                                    Texto: 'Guardar',
                                    CerrarDialogo: false,
                                    Class: 'colorExito',
                                    OnClick: function (jAlert, iFrame, iFrameContent) {
                                        iFrameContent.editar();
                                    }
                                }
                            ]
                    });
                }
            },

            {
                Titulo: 'Eliminar',
                Icono: 'delete',
                OnClick: function (data) {

                    crearDialogoHTML({
                        Titulo: '<label>Eliminar Servicio</label>',
                        Content:
                            '<div class="padding">' +
                            '<label id="textoConfirmacion"class="titulo">¿Esta seguro de eliminar el servicio?</label>' +
                            '</div>',
                        Botones:
                            [
                                {
                                    Id: 'btnNo',
                                    Texto: 'No'
                                },
                                {
                                    Id: 'btnSi',
                                    Texto: 'Si',
                                    Class: 'colorExito',
                                    CerrarDialogo: false,
                                    OnClick: function (jAlert) {

                                        //Muestro el cargando
                                        // mostrarCargando(true);
                                        $(jAlert).MostrarDialogoCargando(true);

                                        var dataAjax = { id: data.Id };
                                        dataAjax = JSON.stringify(dataAjax);

                                        var urlAjax = ResolveUrl('~/Servicios/ServicioService.asmx/DarDeBaja');



                                        $.ajax({
                                            type: "POST",
                                            dataType: "json",
                                            contentType: "application/json; charset=utf-8",
                                            data: dataAjax,
                                            url: urlAjax,
                                            success: function (result) {
                                                result = parse(result.d);

                                                //Oculto el cargando
                                                $(jAlert).MostrarDialogoCargando(false);

                                                //Error
                                                if ('Error' in result) {
                                                    mostrarMensaje('Error', result.Error);
                                                    return;
                                                }

                                                //Muestro el mensaje de OK
                                                mostrarMensaje('Exito', 'Servicio eliminado correctamente');


                                                //Cierro el dialogo
                                                $(jAlert).CerrarDialogo();
                                                actualizarRowEnGrilla(result.Servicio);                                                
                                            },
                                            error: function (result) {
                                                //Oculto el cargando
                                                mostrarCargando(false);

                                                //Muestro el Error
                                                mostrarMensajeError('Error eliminando el servicio');
                                            }
                                        });
                                    }
                                }]
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

function actualizarRowEnGrilla(servicio) {
    //Busco el indice de la zona a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == servicio.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(servicio);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Actualizo la info nueva en el array de arriba
    $.each(servicios, function (index, element) {
        if (element.Id == servicio.Id) {
            servicios[index] = servicio;
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



//function onClick_Boton_TablaResultadoConsulta(btn) {
//    var datatable = $('#tabla').DataTable();
//    var idBoton = $(btn).attr('id');
//    var data = datatable.row($(btn).parents('tr')).data();
//    //window.location.href = "./Servicio.aspx?Id=" + data.Id;

//    crearDialogoIFrame({
//        Titulo: 'Editar servicio',
//        Url: ResolveUrl('~/IFrame/IServicioNuevo.aspx?Id=' + data.Id),
//        OnLoad: function (jAlert, iFrame, iFrameContent) {
//            //Callback de servicio editado
//            iFrameContent.setOnServicioEditadoListener(function (servicio) {
//                //Actualizar fila en grilla
//                actualizarServicioResultadoConsulta(servicio);

//                //Cierro el modal
//                $(jAlert).CerrarDialogo();
//                Materialize.toast('Servicio editado correctamente', 5000, 'colorExito');
//            });


//            //Callback de mensajes
//            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {

//                switch (tipo) {
//                    case 'Info':
//                        Materialize.toast(mensaje, 5000);
//                        break;

//                    case 'Alerta':
//                        Materialize.toast(mensaje, 5000, 'colorAlerta');
//                        break;

//                    case 'Error':
//                        Materialize.toast(mensaje, 5000, 'colorError');
//                        break;
//                }

//            })

//            //Callback cargando
//            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
//                mostrarDialogoCargando(jAlert, cargando, true);
//            })
//        },
//        Botones:
//            [
//                {
//                    Texto: 'Cancelar',
//                },
//                {
//                    Texto: 'Aceptar',
//                    Class: 'colorExito',
//                    CerrarDialogo: false,
//                    OnClick: function (jAlert, iFrame, iFrameContent) {
//                        iFrameContent.editar();
//                    }
//                },

//            ]
//    });
//}

//function actualizarServicioResultadoConsulta(servicio) {
//    //Busco el indice de la persona a actualizar
//    var index = -1;
//    var dt = $('#tabla').DataTable();
//    dt.rows(function (idx, data, node) {
//        if (data.Id == servicio.Id) {
//            index = idx;
//        }
//    });

//    //Si no esta, corto
//    if (index == -1) {
//        return;
//    }

//    //Actualizo
//    dt.row(index).data(servicio);

//    //Inicializo el tooltip
//    dt.$('.tooltipped').tooltip({ delay: 50 });

//    //Busco la posicion del servicio actualizado en mi array de servicios
//    var indexEnArray = -1;
//    $.each(servicios, function (index, s) {
//        if (s.Id == servicio.Id) {
//            indexEnArray = index;
//        }
//    });

//    //Actualizo el servicio
//    servicios[indexEnArray] = servicio;
//}

//function initTablaResultadoConsulta() {
//    var dt = $('#tabla').DataTable({
//        lengthChange: false,
//        searching: false,
//        pageLength: 10,
//        pagingType: "simple",
//        "bDestroy": true,
//        bAutoWidth: false,
//        "columns": [
//            {
//                "sTitle": "Servicio",
//                "mData": "Nombre",
//                width: '30%',
//                render: function (data, type, row) {
//                    return '<div><span>' + toTitleCase(data) + '</span></div>';
//                }
//            },
//              {
//                  "sTitle": "Estado",
//                  "mData": "FechaBaja",
//                  width: '20%',
//                  render: function (data, type, row) {
//                      if (data != null) {
//                          return '<div><span>Dado de baja</span></div>';
//                      } else {
//                          return '<div><span>Activo</span></div>';
//                      }


//                  }
//              },
//            {
//                "sTitle": "Observación",
//                "mData": "Observaciones",
//                width: '50%',
//                render: function (data, type, row) {
//                    if (data != null) {
//                        return '<div><span>' + data + '</span></div>';
//                    }
//                }
//            },
//            {
//                "sTitle": "",
//                "mData": null,
//                "orderable": false,
//                width: '15px',
//                render: function (data, type, row) {
//                    if (!permitirEditar) return;
//                    var btnEditar = "<a id='btnEditar' class='btn btn-cuadrado tooltipped no-select' data-position='bottom' data-delay='50' data-tooltip='Editar' onclick='onClick_Boton_TablaResultadoConsulta(this)'><i class='material-icons'>edit</i></a>";
//                    return "<div class='contenedor-botones'>" + btnEditar + "</div>";
//                }
//            }
//        ],

//        "columnDefs": [{ "defaultContent": "", "targets": "_all" }],
//        "oLanguage": {
//            "sProcessing": "Procesando...",
//            "sLengthMenu": "Tamaño de pagina _MENU_",
//            "sZeroRecords": "No se encontraron resultados",
//            "sEmptyTable": "Ningún dato disponible en esta tabla",
//            "sInfo": "_START_-_END_ de _TOTAL_",
//            "sInfoEmpty": "",
//            "sInfoFiltered": "(filtrado de un total de _MAX_)",
//            "sInfoPostFix": "",
//            "sSearch": "Buscar:",
//            "sUrl": "",
//            "sInfoThousands": ",",
//            "sLoadingRecords": "Cargando...",
//            "oPaginate": {
//                "sFirst": "Primero",
//                "sLast": "Último",
//                "sNext": "<i class='material-icons'>chevron_right</i>",
//                "sPrevious": "<i class='material-icons'>chevron_left</i>"
//            },
//            "oAria": {
//                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
//                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
//            }
//        }
//    });

//    //Inicializo los tooltips
//    dt.$('.tooltipped').tooltip({ delay: 50 });

//    //Muevo el indicador y el paginado a mi propio div
//    $('.tabla-footer').empty();
//    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
//    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
//}

//function initUsuario() {
//    if (usuario == null || usuario.Permiso == null || !usuario.Permiso.NivelAcceso == null) {
//        permitirEditar = false;
//    } else {
//        permitirEditar = usuario.Permiso.NivelAcceso.indexOf(WRITE) != -1;
//    }
//}