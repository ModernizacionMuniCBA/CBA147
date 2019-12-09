var ranking_Motivos;
var totales;
var radial;
var promedioAtencion;
var criticidadServicios;
var coloresCriticidadServicio;
var periodoSeleccionado = 1; //se inicializa en 1=ultimos 30 dias

$(document).ready(function () {
    $('.tooltipAyuda').each(function () { // Notice the .each() loop, discussed below
        $(this).qtip({
            content: {
                text: $(this).next('div') // Use the "div" element next to this for the content
            },
            position: {
                my: 'top center',
                at: 'bottom center'
            },
            style: {
                classes: 'qtip-shadow qtip-rounded qtip-tipsy'
            }
        });
    });
});

function init(data) {

    $('#btnExpandirDrawer').trigger('click');
    data = parse(data);
    cargarDatos(data.DatosEstadisticaPanel);


    $('#contenedor-fecha label').click(function () {
        var periodoClickeado = $(this).attr('periodo');
        buscarEstadisticas(periodoClickeado);
    });


    //$('#lblTotalReclamos').text(totales.Total);
    //$('#lblIndice').text(totales.Porcentaje + "%");
    //$('#lblPromedioAtencion').text(promedioAtencion + " días")


    //initTablaRankingMotivos();
    //cargarRanking();

    //initTablaCriticidadServicio();
    //cargarCriticidadServicio();


    //generarGraficoIndicador(totales);
    //generarGraficoPaleta(totales);
    //generarGraficoRadial(radial);

    var d = new Date();

    var month = d.getMonth() + 1;
    var day = d.getDate();

    var output = (day < 10 ? '0' : '') + day + '/' +
        (month < 10 ? '0' : '') + month + '/' +
        d.getFullYear();
        

    $('#lblFechaActual').text('Fecha de hoy: ' + output);


    $('#iframeMapa').attr('src', ResolveUrl(data.UrlMapa));
    $('#iframeMapa').on('load', function () {
        $('.cargando').stop(true, true).fadeOut(300);
    });


}

function cargarDatos(data) {
    ranking_Motivos = data.Ranking_Motivos;
    totales = data.Totales;
    radial = data.Radial;
    promedioAtencion = data.PromedioAtencion
    criticidadServicios = data.CriticidadServicios;
    //coloresCriticidadServicio = data.ColoresCriticidadServicio;

    $('#lblTotalReclamos').text(totales.Total);
    $('#lblIndice').text(totales.Porcentaje + "%");
    $('#lblPromedioAtencion').text(promedioAtencion + " días")

    initTablaRankingMotivos();
    cargarRanking();
    initTablaCriticidadServicio();
    cargarCriticidadServicio();
    generarGraficoIndicador(totales);
    generarGraficoRadial(radial);
}

function buscarEstadisticas(periodo) {
    var data = { periodo: parseInt(periodo) };
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetDatosEstadisticaYMapa'),
        Data: data,
        OnSuccess: function (data) {

            mostrarCargando(false);
            if (data.Ok == false || data.Return == null) {
                //mostrarMensaje('error', 'Error generando las estadísticas')
                return;
            }

            switch (periodo) {
                case "1":
                    //setTitulo('Resumen Estadístico, últimos 30 días');
                    break;
                case "2":
                    //setTitulo('Resumen Estadístico, últimos 3 meses');
                    break;
                case "3":
                    //setTitulo('Resumen Estadístico, últimos 6 meses');
                    break;
            }

            $("#contenedor-fecha > [periodo=" + periodoSeleccionado + "]").removeClass('seleccionado');
            $("#contenedor-fecha > [periodo=" + periodo + "]").addClass('seleccionado');
            periodoSeleccionado = periodo;

            $('#iframeMapa').attr('src', ResolveUrl(data.Return.UrlMapa));
            $('.cargando').stop(true, true).fadeIn(300);
            $('#iframeMapa').on('load', function () {
                $('.cargando').stop(true, true).fadeOut(300);
            });

            cargarDatos(data.Return);
        },
        OnError: function (data) {
            //mostrarMensaje('error', 'Error generando las estadísticas')
            mostrarCargando(false);
        }
    });
}

function initTablaRankingMotivos() {

    $('#tablaRankingMotivos').DataTableGeneral({
        "Orden": [1, "desc"],
        Paginar: true,
        VerInfo: true,
        Columnas: [
            {
                title: "Nombre",
                data: "Motivo",
                orderable: false,
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            },
            {
                title: 'Requerimientos',
                data: "Cantidad",
                Orderable: false,
                render: function (data, type, row) {
                    return '<div><span>' + data + '</span></div>';
                }
            }

        ]

    });

    $('#tablaRankingMotivos').DataTable().page.len(5).draw();

    //Muevo el indicador y el paginado a mi propio div
    $('#cardRankingMotivos .tabla-footer').empty();
    $('#cardRankingMotivos .dataTables_info').detach().appendTo($('#cardRankingMotivos .tabla-footer'));
    $('#cardRankingMotivos .dataTables_paginate').detach().appendTo($('#cardRankingMotivos .tabla-footer'));
}

function cargarRanking() {
    var dt = $('#tablaRankingMotivos').DataTable();


    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (ranking_Motivos != null && ranking_Motivos != undefined) {
        dt.rows.add(ranking_Motivos).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function initTablaCriticidadServicio() {

    $('#tablaCriticidadServicios_Colores').DataTableGeneral({
        "Orden": [[2, "desc"]],
        Paginar: true,
        VerInfo: true,

        Columnas: [
            {
                title: "",
                            data: 'Color',
                         
                            render: function (data, type, row) {
                                return '<div><label class="indicador-estado" style="background-color: ' + data + '"></label><span></span></div>';


                                //var idServ = row.IdServicio;

                                //var objColor;
                                //$.each(coloresCriticidadServicio, function (index, element) {
                                //    if (element.IdServicio == idServ) {
                                //        objColor = element;
                                //        return;
                                //    }
                                //});


                                ////objColor = $.grep(coloresCriticidadServicio, function (element, index) {
                                ////    return element.IdServicio == idServ;
                                ////})[0];

                                //if (objColor == undefined) {
                                //    return '';
                                //}

                                //var color = objColor.Criticidad;
                                //return '<div><label class="indicador-estado" style="background-color: #' + color + '"></label><span></span></div>';
                            }
            },
            {
                title: "Área",
                data: 'Area',
                orderable: false,
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            },
            {
                title: 'Atendidos',
                data: 'Porcentaje',
                Orderable: false,
                render: function (data, type, row) {
                    return '<div><span>' + row.Atendidos + "/" + row.Total + "<br/>" + Math.floor(Math.round(row.Porcentaje * 100) / 100) + "%" + '</span></div>';
                }
            }
            //{
            //    title: '',
            //    data: 'Atendidos',
            //    Orderable: false,
            //    render: function (data, type, row) {
            //        return '<div><span>' + data + '</span></div>';
            //    }
            //},
            //{
            //    title: '',
            //    data: 'Porcentaje',
            //    Orderable: false,
            //    render: function (data, type, row) {
            //        return '<div><span>' + data + '</span></div>';
            //    }
            //}
        ]

    });

    $('#tablaCriticidadServicios_Colores').DataTable().page.len(5).draw();

    //Muevo el indicador y el paginado a mi propio div
    $('#cardCriticidadServicio .tabla-footer').empty();
    $('#cardCriticidadServicio .dataTables_info').detach().appendTo($('#cardCriticidadServicio .tabla-footer'));
    $('#cardCriticidadServicio .dataTables_paginate').detach().appendTo($('#cardCriticidadServicio .tabla-footer'));

    //$('#tablaCriticidadServicios_Colores').DataTableGeneral({
    //    "Orden": [[1, "desc"]],
    //    Paginar: false,
    //    VerInfo: false,
    //    Columnas: [
    //        //{
    //        //    "sTitle": "Estado",
    //        //    "mData": "Estado",
    //        //    "width": "120px",
    //        //    render: function (data, type, row) {
    //        //        return '<div><label class="indicador-estado" style="background-color: #' + data.Color + '"></label><span></span></div>';
    //        //    }
    //        //},
    //        {
    //            title: "Cantidad",
    //            data: 1,
    //            orderable: false,
    //            render: function (data, type, row) {
    //                return '<div><span>' + toTitleCase(data) + '</span></div>';
    //            }
    //        },

    //    ]

    //});
}

function cargarCriticidadServicio() {
    var dt = $('#tablaCriticidadServicios_Colores').DataTable();


    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (criticidadServicios != null && criticidadServicios != undefined) {
        dt.rows.add(criticidadServicios).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function generarGraficoIndicador(estadisticas) {

    var data = totales.Porcentaje;

    $('#chart-container').stop(true, true).animate({ opacity: 0 }, 600);

    FusionCharts.ready(function () {
        var grafico = new FusionCharts({
            type: 'angulargauge',
            renderAt: 'chart-container',
            width: '100%',
            height: '100%',
            dataFormat: 'json',
            dataSource: {
                "chart": {
                    //animation: 0,
                    "lowerLimit": "0",
                    "upperLimit": "100",
                    "showGaugeBorder": "0",
                    "showValue": "1",
                    "valueBelowPivot": "1",
                    "theme": "fint"
                },

                "colorRange": {
                    "color": [
                       {
                           "minValue": "0",
                           "maxValue": "12,5",
                           "code": "#FF3636"
                       },
                       {
                           "minValue": "12,6",
                           "maxValue": "37,5",
                           "code": "#FFA041"
                       },
                       {
                           "minValue": "37,6",
                           "maxValue": "62,5",
                           "code": "#FFF000"
                       },
                       {
                           "minValue": "62,6",
                           "maxValue": "87,5",
                           "code": "#DDFF53"
                       },
                       {
                           "minValue": "87,6",
                           "maxValue": "100",
                           "code": "#2ACB3B"
                       }
                    ]
                },
                "dials": {
                    "dial": [
                       {
                           "value": data
                       }
                    ]
                }
            }
        });

        grafico.render('chart-container', undefined, function () {
            $('#chart-container').stop(true, true).animate({ opacity: 1 }, 600);
        });
    });


}

function generarGraficoPaleta() {

    $('#chart-container2').stop(true, true).animate({ opacity: 0 }, 600);

    FusionCharts.ready(function () {
        var grafico = new FusionCharts({
            type: 'hlineargauge',
            renderAt: 'chart-container2',
            width: '100%',
            height: '7%',
            dataFormat: 'json',
            dataSource: {
                "chart": {
                    "theme": "fint",
                    //"showValue": "0",
                },
                "colorRange": {

                    "color": [{
                        "minValue": "0",
                        "maxValue": "12.5",
                        "code": "#2ACB3B",
                        "label": "Muy Baja"
                    },
                       {
                           "minValue": "12.6",
                           "maxValue": "37.5",
                           "code": "#DDFF53",
                           "label": "Baja",
                           "color": "#DDFF53"
                       },
                       {
                           "minValue": "37.6",
                           "maxValue": "62.5",
                           "code": "#FFF000",
                           "label": "Normal"
                       },
                       {
                           "minValue": "62.6",
                           "maxValue": "87.5",
                           "code": "#FFA041",
                           "label": "Alta"
                       },
                       {
                           "minValue": "87.6",
                           "maxValue": "100",
                           "code": "#FF3636",
                           "label": "Muy Alta"
                       }
                    ]
                },
            }
        })

        grafico.render('chart-container2', undefined, function () {
            $('#chart-container2').stop(true, true).animate({ opacity: 1 }, 600);
        });
    });
}

function generarGraficoRadial(datos) {

    var categorias = [];
    var data = [];
    $.each(datos, function (i, v) {
        categorias.push({
            'label': toTitleCase(v.Servicio)
        });
        data.push({
            'value': v.Dias
        });
    });


    $('#chart-container-radial').stop(true, true).animate({ opacity: 0 }, 600);

    FusionCharts.ready(function () {
        var grafico = new FusionCharts({
            type: 'radar',
            renderAt: 'chart-container-radial',
            width: '450',
            height: '400',
            dataFormat: 'json',
            dataSource: {
                "chart": {
                    "caption": "Tiempo de Resolución",
                    "theme": "fint",
                    "radarfillcolor": "#ffffff",
                },
                "categories": [
                    {
                        "category": categorias
                    }
                ],
                "dataset": [
                    {
                        "seriesname": "Resolución",
                        "data": data
                    }
                ]
            }
        })

        grafico.render('chart-container-radial', undefined, function () {
            $('#chart-container-radial').stop(true, true).animate({ opacity: 1 }, 600);

            // Centrar la scrollbar del grafico radial
            var outer = $('#contenedorGrafico')["0"].offsetWidth;
            var inner = $('#chart-container-radial')["0"].scrollWidth;
            console.log(inner);
            $('#contenedorGrafico').scrollLeft((inner - outer) / 2)
        });


    });


}






