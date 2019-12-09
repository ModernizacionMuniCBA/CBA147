var callback;

var isInit = false;
function initTabla() {
    if (isInit) return;

    var dt = $('#tabla').DataTable({
        lengthChange: false,
        searching: false,
        pageLength: 10,
        pagingType: "simple",
        bDestroy: true,
        bAutoWidth: false,
        columns: [
            {
                title: 'Nombre',
                data: 'Nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                },
                width: '100%'
            },
            {
                title: '',
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var btnMapa = "<a id='btnMapa' class='btn btn-cuadrado tooltipped no-select' data-position='bottom' data-delay='50' data-tooltip='Ver Mapa' onclick='onClick_Boton_Tabla(this)'><i class='material-icons'>room</i></a>"
                    var btnSeleccionar = "<a id='btnSeleccionar' class='btn btn-cuadrado tooltipped no-select' data-position='bottom' data-delay='50' data-tooltip='Seleccionar' onclick='onClick_Boton_Tabla(this)'><i class='material-icons'>check</i></a>";
                    return "<div class='contenedor-botones'> " + btnMapa + btnSeleccionar + "</div>";
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

    isInit = true;
}

function onClick_Boton_Tabla(btn) {
    var datatable = $('#tabla').DataTable();
    var idBoton = $(btn).attr('id');
    var data = datatable.row($(btn).parents('tr')).data();

    switch (idBoton) {
        case 'btnSeleccionar': {
            if (callback != undefined) {
                callback(data);
            }
        } break;

        case 'btnMapa': {
            var x = data.XCentro.replace(',', '.');
            var y = data.YCentro.replace(',', '.');

            

            crearDialogoIFrame({
                Titulo: 'Mapa del barrio: ' + toTitleCase(data.Nombre),
                Url: url,
                Botones:
                    [
                        {
                            Texto: 'Aceptar',
                            Class: 'colorExito',
                        }
                    ]
            });
        }
    }
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').find('#tabla_wrapper').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();

    console.log(rows);
}

function setBarrios(barrios) {
    initTabla();

    var dt = $('#tabla').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (barrios != null) {
        dt.rows.add(barrios).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Cantidad de rows
    calcularCantidadDeRows();
}

function setCallback(callback) {
    this.callback = callback;
}

////-----------------------------
//// Cargando
////-----------------------------
//function mostrarCargando(mostrar, valores) {
//    window.parent.$(window.parent.document).trigger('MostrarCargando', [mostrar, valores]);
//}

////-----------------------------
//// Alertas 
////-----------------------------
//function mostrarMensajeError(mensaje) {
//    window.parent.$(window.parent.document).trigger('MostrarMensajeError', mensaje);
//}

//function mostrarMensajeAlerta(mensaje) {
//    window.parent.$(window.parent.document).trigger('MostrarMensajeAlerta', mensaje);
//}

//function mostrarMensajeInfo(mensaje) {
//    window.parent.$(window.parent.document).trigger('MostrarMensajeInfo', mensaje);
//}
