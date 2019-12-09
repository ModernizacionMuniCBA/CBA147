let tabla;

function init(data) {
    initTabla();

    generar();

    //$('#btn_ExportarPdf').click(function () {
    //    $('#btn_ExportarPdf').html('<i class="btn-icono material-icons">cached</i>Procesando');
    //    setTimeout(function () {
    //        $(".buttons-pdf").trigger('click');
    //        $('#btn_ExportarPdf').html('<i class="btn-icono material-icons">file_download</i>PDF');
    //    }, 100);
    //});

    $('#btn_ExportarExcel').click(function () {
        $('#btn_ExportarExcel').html('<i class="btn-icono material-icons">cached</i>Procesando');
        setTimeout(function () {
            $(".buttons-excel").trigger('click');
            $('#btn_ExportarExcel').html('<i class="btn-icono material-icons">file_download</i>Excel');
        }, 100);
    });

}

function generar() {
    $('#contenedor_Cargando').addClass('visible');
    $('#contenedor_Resultado').removeClass('visible');
    $('#contenedor_Error').removeClass('visible');

    ajax_InfoGlobal()
        .then(function (result) {

            var hDisponible = $('.tabla-contenedor').height();
            var rows = calcularCantidadRowsDataTable(hDisponible);
            tabla.page.len(rows);

            tabla.clear();
            tabla.rows.add(result).draw();
            tabla.$('.tooltipped').tooltip({ delay: 50 });

            $('#contenedor_Cargando').removeClass('visible');
            $('#contenedor_Resultado').addClass('visible');
            $('#contenedor_Error').removeClass('visible');

        })
        .catch(function (error) {
            $('#contenedor_Cargando').removeClass('visible');
            $('#contenedor_Resultado').removeClass('visible');
            $('#contenedor_Error').addClass('visible');

            $('#contenedor_Error label').text(error);
        });
}

function initTabla() {
    tabla = $('#tabla').DataTableGeneral({
        Ordenar: false,
        Columnas: [
            {
                title: 'Numero',
                data: 'Numero',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Año',
                data: 'Año',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Servicio',
                data: 'Servicio',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Area',
                data: 'Area',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Motivo',
                data: 'Motivo',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
                                                                {
                                                                    title: 'Barrio',
                                                                    render: function (data, type, row) {
                                                                        return '<div><span>' + row.Barrio + '</span></div>';
                                                                    }
                                                                },
                                                                            {
                                                                                title: 'Cpc',
                                                                                render: function (data, type, row) {
                                                                                    var cpc = '' + row.CpcNumero;
                                                                                    if (cpc.length == 1) {
                                                                                        cpc = '0' + cpc;
                                                                                    }
                                                                                    return '<div><span>' + cpc + ' - ' + row.Cpc + '</span></div>';
                                                                                }
                                                                            },
            {
                title: 'Longitud',
                render: function (data, type, row) {
                    return '<div><span>' + row.Longitud + '</span></div>';
                }
            },                     
                                          {
                                              title: 'Latitud',
                                              render: function (data, type, row) {
                                                  return '<div><span>' + row.Latitud + '</span></div>';
                                              }
                                          },

            {
                title: 'Estado Nuevo',
                data: 'EstadoNuevo',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Estado Proceso',
                data: 'EstadoProceso',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Estado Completado',
                data: 'EstadoCompletado',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Estado Cancelado',
                data: 'EstadoCancelado',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Ultimo Estado',
                data: 'UltimoEstado',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
                        {
                            title: 'Id Origen',
                            data: 'OrigenId',
                            render: function (data, type, row) {
                                return '<div><span>' + (data || '') + '</span></div>';
                            }
                        },
                                    {
                                        title: 'Nombre Origen',
                                        data: 'OrigenNombre',
                                        render: function (data, type, row) {
                                            return '<div><span>' + (data || '') + '</span></div>';
                                        }
                                    },
                                                {
                                                    title: 'Género',
                                                    data: 'Genero',
                                                    render: function (data, type, row) {
                                                        return '<div><span>' + (data || '') + '</span></div>';
                                                    }
                                                },
                                                            {
                                                                title: 'Fecha de Nacimiento',
                                                                data: 'FechaNacimiento',
                                                                render: function (data, type, row) {
                                                                    return '<div><span>' + (data || '') + '</span></div>';
                                                                }
                                                            },
        ],
        //Export excell
        OpcionesExportarExcel: {
            extend: 'excelHtml5',
            title: 'Informacion Global de Requerimientos de #CBA147',
            exportOptions: {
                columns: [0,1, 2, 3, 4, 5,6, 7, 8, 9, 10, 11, 12, 13, 14,15,16,17]
            },
        },

        //Export pdf
        OpcionesExportarPdf: {
            extend: 'pdfHtml5',
            title: 'Informacion Global de Requerimientos de #CBA147',
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17]
            },
        }
    });


    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function ajax_InfoGlobal() {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetInfoGlobal'),
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        })
    });
}