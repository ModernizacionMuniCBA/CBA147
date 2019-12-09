
function init(data) {
    //Inicializo el form
    initTablaAreas();


    //Parse la data que me llega desde el servidor
    data = parse(data);

    //Obtengo el usuario
    var usuario = data.Usuario;

    //Cargo el Nombre y Apellido
    $('#textoNombre').text(toTitleCase(usuario.Nombre + " " + usuario.Apellido));

    //Cargo el Rol
    $('#textoRol').text(toTitleCase(usuario.Rol));

    //Areas
    var areas = usuario.Areas;
    cargarAreas(areas);
}

function initTablaAreas() {
    var dt = $('#tabla').DataTable({
        lengthChange: false,
        searching: false,
        pageLength: 10,
        pagingType: "simple",
        bDestroy: true,
        bAutoWidth: false,
        columns: [
            {
                sTitle: 'Nombre',
                mData: 'Nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                },
                width: '100%'
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

function cargarAreas(areas) {
    var dt = $('#tabla').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (areas != null) {
        dt.rows.add(areas).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Cantidad de rows
    calcularCantidadDeRows();
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').find('#tabla_wrapper').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();

    console.log(rows);
}