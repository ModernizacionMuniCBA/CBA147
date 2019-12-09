var areas;
var usuario;
var permitirEditar;

var READ = 1;
var WRITE = 2;
var DELETE = 3;
var EJECUCION = 4;

function init(data) {
    data = parse(data);

    //------------------------------------
    // Init Datos
    //------------------------------------

    areas = data.Areas;
    usuario = data.Usuario;
    initUsuario();

    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();
    cargarResultadoConsulta(areas);

    //-------------------------------------
    // RadioButtons
    //-------------------------------------
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

    //------------------------------------
    // Input
    //------------------------------------

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

    $('#cardResultadoConsulta').css('opacity', 0);
    $('#cardResultadoConsulta').css('top', '50px');

    setTimeout(function () {
        $('#cardResultadoConsulta').animate({ 'opacity': 1, 'top': '0px' }, 500);
    }, 600);
};

$(function () {
    setTimeout(function () {
        calcularCantidadDeRows();
    }, 300);
});

function initUsuario() {
    if (usuario == null || usuario.Permiso == null || !usuario.Permiso.NivelAcceso == null) {
        permitirEditar = false;
    } else {
        permitirEditar = usuario.Permiso.NivelAcceso.indexOf(WRITE) != -1;
    }
}

function filtrarBusqueda() {
    var resultados = [];

    var estado = 2;
    if ($('#contenedorEstado').length!=0) {

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

    $.each(areas, function (index, area) {

        if (area.Nombre.toUpperCase().indexOf(texto) != -1) {

            switch (estado) {
                case 1:

                    resultados.push(area);
                    break;

                case 2:
                    if (area.FechaBaja == null) {
                        resultados.push(area);
                    }
                    break;

                case 3:
                    if (area.FechaBaja != null) {
                        resultados.push(area);
                    }
                    break;
            }
        }

    });

    cargarResultadoConsulta(resultados);

}

function actualizarAreaResultadoConsulta(area) {
    //Busco el indice de la persona a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == area.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(area);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Busco la posicion del area actualizado en mi array de areas
    var indexEnArray = -1;
    $.each(areas, function (index, s) {
        if (s.Id == area.Id) {
            indexEnArray = index;
        }
    });

    //Actualizo el area
    areas[indexEnArray] = area;
}

function initTablaResultadoConsulta() {
    var dt = $('#tabla').DataTable({
        lengthChange: false,
        searching: false,
        pageLength: 10,
        pagingType: "simple",
        "bDestroy": true,
        bAutoWidth: false,
        "columns": [
            {
                title: "Area",
                data: "Nombre",
                width: '30%',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            },
              {
                  title: "Estado",
                  data: "FechaBaja",
                  width: '20%',
                  render: function (data, type, row) {
                      if (data != null) {
                          return '<div><span>Dado de baja</span></div>';
                      } else {
                          return '<div><span>Activo</span></div>';
                      }


                  }
              },
            {
                title: "Observación",
                data: "Observaciones",
                width: '50%',
                render: function (data, type, row) {
                    if (data != null) {
                        return '<div><span>' + data + '</span></div>';
                    }
                }
            },
            {
                title: "",
                data: null,
                orderable: false,
                width: '15px',
                render: function (data, type, row) {
                    if (permitirEditar) {
                        var btnEditar = "<a id='btnEditar' class='btn btn-cuadrado tooltipped no-select' data-position='bottom' data-delay='50' data-tooltip='Editar' onclick='onClick_Boton_TablaResultadoConsulta(this)'><i class='material-icons'>edit</i></a>";
                        return "<div class='contenedor-botones'>" + btnEditar + "</div>";
                    }
                }
            }
        ],

        "columnDefs": [{ "defaultContent": "", "targets": "_all" }],
        "oLanguage": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Tamaño de pagina _MENU_",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "_START_-_END_ de _TOTAL_",
            "sInfoEmpty": "",
            "sInfoFiltered": "(filtrado de un total de _MAX_)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "<i class='material-icons'>chevron_right</i>",
                "sPrevious": "<i class='material-icons'>chevron_left</i>"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        }
    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

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
}

function onClick_Boton_TablaResultadoConsulta(btn) {
    var datatable = $('#tabla').DataTable();
    var idBoton = $(btn).attr('id');
    var data = datatable.row($(btn).parents('tr')).data();

    crearDialogoIFrame({
        Titulo: 'Detalle de Area',
        Url: ResolveUrl('~/IFrame/IAreaNuevo.aspx?Id=' + data.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de area editado
            iFrameContent.setOnAreaEditadoListener(function (area) {
                //Actualizar fila en grilla
                actualizarAreaResultadoConsulta(area);

                //Cierro el modal
                $(alert).closeAlert();
            });

            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {

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
                }

            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                mostrarDialogoCargando(jAlert, cargando, true);
            })

            return false;
        },
        Botones:
           [
               {
                   Texto: 'Cancelar'
               },
               {
                   Texto: 'Aceptar',
                   Class: 'colorExito',
                   CerrarDialogo: false,
                   OnClick: function (jAlert, iFrame, iFrameContent) {
                       iFrameContent.editar();
                   }
               }
           ]
    });

}