var estados;
var permisos;
var infoPermisos;
var tabla;

var indicadorCargando;

function init(data) {

    estados = data.Estados;
    permisos = data.Permisos;
    infoPermisos = data.InfoPermisos;

    initTabla();
    initBotones();

    $('#main > .card').AgregarIndicadorCargando();
    indicadorCargando = $('#main > .card').GetIndicadorCargando();
    $(indicadorCargando).fadeOut();
}

function initTabla() {
    $.each(infoPermisos, function (index, element) {
        element.EstadoNombre = $.grep(estados, function (element1, index1) {
            return element1.KeyValue == element.EstadoOrdenTrabajo;
        })[0].Nombre;

        element.PermisoNombre = $.grep(permisos, function (element1, index1) {
            return element1.KeyValue == element.Permiso;
        })[0].Nombre;
    });


    var cols = [];
    cols.push({
        title: 'Posicion',
        data: 'Posicion',
        render: function (data, type, row) {
            return '<div><span>' + data + '</span></div>';
        }
    });

    cols.push({
        title: 'Estado',
        data: 'Nombre',
        render: function (data, type, row) {
            return '<div><span>' + data + '</span></div>';
        }
    });

    $.each(estados, function (index, estado) {
        cols.push({
            title: estado.Nombre,
            width: '20px',
            render: function (data, type, row, meta) {
                let indexRow = meta.row;
                let indexCol = meta.col;
                let permiso = tabla.data()[indexRow];

                let id = 'check_' + estado.KeyValue + '_' + permiso.KeyValue;

                let tienePermiso = false;
                $.each(infoPermisos, function (index, element) {
                    if (element.Permiso == permiso.KeyValue && element.EstadoOrdenTrabajo == estado.KeyValue) {
                        tienePermiso = element.TienePermiso;
                    }
                });

                let inputCheck = tienePermiso ? 'checked' : '';
                return ' <div><input ' + inputCheck + ' onChange="changeCheck(' + indexRow + ', ' + indexCol + ')" class="with-gap" type="checkbox" id="' + id + '"/><label for="' + id + '"></label></div>'
            }
        });
    });

    tabla = $('#tabla').DataTableGeneral({
        Orden: [[0, 'asc']],
        Columnas: cols
    });

    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tabla.page.len(rows).draw();
    tabla.column(0).visible(false);
    tabla.rows.add(permisos).draw();
}

function initBotones() {
    $('#btn_Guardar').click(function () {
        $(indicadorCargando).stop(true, true).fadeIn(300);

        crearAjax({
            Url: ResolveUrl('~/Servicios/PermisoEstadoOrdenTrabajoService.asmx/SetPermisos'),
            Data: { items: infoPermisos },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    $(indicadorCargando).stop(true, true).fadeOut(300);
                    mostrarMensaje('Error', result.Error);
                    return;
                }

                crearAjax({
                    Url: ResolveUrl('~/Servicios/PermisoEstadoOrdenTrabajoService.asmx/GetPermisos'),
                    OnSuccess: function (result2) {
                        $(indicadorCargando).stop(true, true).fadeOut(300);

                        if (!result2.Ok) {
                            mostrarMensaje('Error', result2.Error);
                            return;
                        }

                        infoPermisos = result2.Return;
                        tabla.clear();
                        tabla.rows.add(permisos);
                        tabla.draw();

                        mostrarMensaje('Exito', 'Permisos actualizados');
                    },
                    OnError: function (result) {
                        $(indicadorCargando).stop(true, true).fadeOut(300);

                        mostrarMensaje('Error', 'Error procesando la solicitud');
                    }
                });
            },
            OnError: function (result) {
                $(indicadorCargando).stop(true, true).fadeOut(300);

                mostrarMensaje('Error', 'Error procesando la solicitud');
            }
        });
    });
}

function changeCheck(row, col) {
    let estado = estados[col - 2];
    console.log('estado');
    console.log(estado);

    let permiso = tabla.data()[row];
    console.log('permiso');
    console.log(permiso);

    $.each(infoPermisos, function (index, element) {
        if (element.Permiso == permiso.KeyValue && element.EstadoOrdenTrabajo == estado.KeyValue) {
            element.TienePermiso = !element.TienePermiso
        }
    });
}