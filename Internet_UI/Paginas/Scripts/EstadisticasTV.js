var primeraVez = true;

var tablaRankingMotivos;
var tablaAreasOperativas;

var habilitarAutoPaginacion = false;
var verBotonesPaginar = true;

var mapaCritico;
var estadisticas;

var mapa;
var poligonos;
var marcador;
var infoWindow;

var intervaloActualizacion;
var tiempoEntreActualizaciones = 1000 * 120;

function init(data) {
    $(window).on('resize', function () {
        sizeChanged();
    });

    initLogin(parse(data));

    initMenu();
    initBotones();
    initPaginacion();

    initDomicilio();

    initTablaRankingMotivos();
    initTablaAreasOperativas();
}


// Init
function initLogin(data) {
    if (('Login' in data && data.Login) || (!('Usuario' in data) || data.Usuario === undefined)) {
        $('#login').addClass('visible');
        $('#login .contenido').addClass('visible');
    } else {
        $('#login').removeClass('visible');
        $('#login .contenido').removeClass('visible');
        cargarUsuario(data.Usuario);

        actualizarDatos();
    }

    $("#formLogin").submit(function () {
        iniciarSesion();
    });
    $('#btnLogin').click(function () {
        iniciarSesion();
    });
}

function initMenu() {
    $('#encabezado .usuario').click(function (e) {
        $('#encabezado .usuario').MenuFlotante({
            MarginTop: $('#encabezado .usuario').height() + 13,
            Menu: [
                {
                    Texto: 'Cerrar sesión',
                    Icono: 'exit_to_app',
                    OnClick: function () {
                        cerrarSesion();
                    }
                }
            ]
        });
    });
}

function initBotones() {
    $('.periodos label').click(function () {
        $('.periodos label').removeClass('seleccionado');
        $(this).addClass('seleccionado');
        primeraVez = true;
        actualizarDatos();
    });

    $('#btnActualizar').click(function () {
        $('.paneles').animate({ opacity: 0 }, 300, function () {
            cargarEstadisticas();
            $('.paneles').animate({ opacity: 1 }, 300);
        });
    });
}

function initPaginacion() {
    $('.panel .tabla .contenedor-footer .anterior').click(function () {
        $(this).parents('.contenedor').find('table').DataTable().page('previous').draw('page');
    });

    $('.panel .tabla .contenedor-footer .siguiente').click(function () {
        $(this).parents('.contenedor').find('table').DataTable().page('next').draw('page');
    });

    $('.vista .anterior').click(function () {
        $(this).parents('.vista').find('table').DataTable().page('previous').draw('page');
    });

    $('.vista .siguiente').click(function () {
        $(this).parents('.vista').find('table').DataTable().page('next').draw('page');
    });

    if (!verBotonesPaginar) {
        $('.panel .tabla .contenedor-footer').hide();
    }
}

function initDomicilio() {
    ControlMapa_Init({
        Buscar: false,
        BotonCpcs: true,
        BotonBarrios: true,
        BotonEjido: true,
        BotonFullscreen: true,
        ResaltarAlHacerClick: false,
        ColorCpc: "transparent",
        OnMapReady: function (map) {
            mapa = map;
        }
    });

    ControlMapa_SetListenerMapaClick(function () {
        return true;
    });
}


// Login
function iniciarSesion() {
    if (!$('#formLogin').valid()) return false;

    mostrarCargando("Iniciando Sesión...");

    var usuario = $('#input_Username').val();
    var pass = $('#input_Password').val();

    var data = {};
    data.user = usuario;
    data.pass = pass;

    crearAjax({
        Url: ResolveUrl('~/Paginas/EstadisticasTV.aspx/IniciarSesion'),
        Data: data,
        OnSuccess: function (result) {
            ocultarCargando();

            if (!result.Ok) {
                Materialize.toast(result.Error, 5000, 'colorError');
                return;
            }

            $('#login').removeClass('visible');
            $('#login .contenido').removeClass('visible');
            cargarUsuario(result.Return);

            actualizarDatos();
        },
        OnError: function (data) {
            ocultarCargando();
            Materialize.toast('Error procesando la solicitud', 5000, 'colorError');
        }
    });

    return false;
}

function cerrarSesion() {
    mostrarCargando("Cerrando sesion...");
    crearAjax({
        Url: ResolveUrl('~/Paginas/EstadisticasTV.aspx/CerrarSesion'),
        OnSuccess: function (result) {
            ocultarCargando();

            if (!result.Ok) {
                Materialize.toast(result.Error, 5000, 'colorError');
                return;
            }

            primeraVez = true;
            Tablas.cancelar();
            $('#login').addClass('visible');
            $('#login .contenido').addClass('visible');
        },
        OnError: function (data) {
            ocultarCargando();
            Materialize.toast('Error procesando la solicitud', 5000, 'colorError');
        }
    });
}

function cargarUsuario(usuario) {
    $('#encabezado .usuario .nombre').text(usuario.Nombre + " " + usuario.Apellido);

    crearAjax({
        Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/GetFotousuario'),
        OnSuccess: function (result) {
            if (result.Ok && result.Return != undefined) {
                $('#encabezado .usuario .imagen').css('background-image', 'url(' + result.Return + ')');
            } else {
                $('#encabezado .usuario .imagen').css('background-image', 'url(./../../Resources/Imagenes/user-male-icon-blanco.png)');
            }
        },
        OnError: function (data) {
            $('#encabezado .usuario .imagen').css('background-image', 'url(./../../Resources/Imagenes/user-male-icon-blanco.png)');
        }
    });
}


// Datos
function actualizarDatos() {
    $(".mapa").css("display", "none");
    $("#cargando_mapa").css("display", "flex");

    Tablas.cancelar();

    buscarEstadisticas(function () {
        cargarEstadisticas(function () {
            buscarMapa(function () {
                cargarMapa();

                setTimeout(function () {
                    Tablas.mover();
                }, 200);
            });
        });
    });

    if (intervaloActualizacion !== undefined) {
        clearInterval(intervaloActualizacion);
    }

    setInterval(procesarCargarDatos, tiempoEntreActualizaciones);
}

function procesarCargarDatos() {
    Tablas.cancelar();

    buscarEstadisticas(function () {
        cargarEstadisticas(function () {
            setTimeout(function () {
                Tablas.mover();
            }, 200);
        });
    });
}

function buscarEstadisticas(callback) {
    if (primeraVez) {
        mostrarCargando();
        primeraVez = false;
    }

    var periodo = $('.periodos label.seleccionado').attr('data-periodo');

    crearAjax({
        Url: ResolveUrl('~/Servicios/ServicioRequerimiento.asmx/GetDatosEstadisticaPanel'),
        Data: { periodo: periodo },
        OnSuccess: function (result) {
            ocultarCargando();

            if (!result.Ok) {
                return;
            }

            estadisticas = result.Return;

            if (callback !== undefined) {
                callback();
            }
        },
        OnError: function (data) {
            estadisticas = undefined;

            if (callback !== undefined) {
                callback();
            }
        }
    });
}

function cargarEstadisticas(callback) {
    cargarAtencion(estadisticas);
    cargarTiempoResolucion(estadisticas);
    cargarRankingMotivos(estadisticas);
    cargarAreasOperativas(estadisticas);

    setTimeout(function () {
        setTimeout(function () {
            $('.panel').eq(0).addClass('visible');
        }, 300);
        setTimeout(function () {
            $('.panel').eq(1).addClass('visible');
        }, 600);
        setTimeout(function () {
            $('.panel').eq(2).addClass('visible');
        }, 900);
        setTimeout(function () {
            $('.panel').eq(3).addClass('visible');

            if (callback !== undefined) {
                callback();
            }
        }, 1200);
    }, 500);
}

function buscarMapa(callback) {
    var periodo = $('.periodos label.seleccionado').attr('data-periodo');

    crearAjax({
        Url: ResolveUrl('~/Servicios/ServicioRequerimiento.asmx/GetMapaCritico'),
        Data: { periodo: periodo },
        OnSuccess: function (result) {
            if (!result.Ok) {
                return;
            }

            mapaCritico = result.Return;

            if (callback !== undefined) {
                callback();
            }
        },
        OnError: function (data) {

            mapaCritico = undefined;

            if (callback !== undefined) {
                callback();
            }
        }
    });
}

function cargarMapa() {
    removePoligonos();
    removeInfoWindow();

    $.each(poligonos, function (index, poligono) {
        poligono.setMap(null);
    });

    poligonos = [];

    $.each(mapaCritico, function (index, data) {
        var cpc = $.grep(geoApiInfo.cpcs, function (element, index) { return element.numero === data.Cpc.Numero })[0];

        var poligono = new google.maps.Polygon({
            paths: cpc.poligono,
            strokeColor: data.Color,
            strokeOpacity: 1,
            strokeWeight: 2,
            fillColor: data.Color,
            fillOpacity: 0.6,
            map: mapa,
            data: data
        });

        google.maps.event.addListener(poligono, "click", function (event) {
            removeInfoWindow();

            marcador = new google.maps.Marker({
                position: event.latLng,
                map: mapa
            });

            infoWindow = new google.maps.InfoWindow({
                maxWidth: 400,
                content: getInfoWindowContent(poligono.data)
            });

            infoWindow.open(mapa, marcador);

            google.maps.event.addListener(infoWindow, 'closeclick', function () {
                removeInfoWindow();
            });
        });

        poligonos.push(poligono);
    });

    $(".mapa").css("display", "block");
    $("#cargando_mapa").css("display", "none");
}

function getInfoWindowContent(data) {
    var html = "";

    html += "<div class='popup'>";
    html += "   <label>" + "<strong>CPC</strong> N° " + data.Cpc.Numero + " - " + data.Cpc.Nombre + "</label>";
    html += "   <label>" + "<strong>Criticidad</strong> " + data.Criticidad + " (" + data.Porcentaje + " %)</label>";
    html += "   <label>" + "<strong>Cantidad de requerimientos</strong> " + data.CantidadRequerimientos + "</label>";
    html += "</div>";

    return html;
}

function removeInfoWindow() {
    if (marcador !== undefined) {
        marcador.setMap(null);
    }
    if (infoWindow !== undefined) {
        infoWindow.setMap(null);
    }
}

function removePoligonos() {
    $.each(poligonos, function (index, poligono) {
        poligono.setMap(null);
    });
}


// Atencion
function cargarAtencion(data) {
    $('#textoAtencionTotal').text(data.Totales.Total);
    $('#textoAtencionPorcentaje').text(data.Totales.Porcentaje + "%");
    generarGraficoAtencion(data);
}

function generarGraficoAtencion(data) {
    FusionCharts.ready(function () {
        var grafico = new FusionCharts({
            type: 'angulargauge',
            width: '100%',
            height: '100%',
            dataFormat: 'json',
            dataSource: {
                "chart": {
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
                           "value": data.Totales.Porcentaje
                       }
                    ]
                }
            }
        });

        grafico.render('atencion-grafico', undefined);
    });
}


// Tiempo resolucion
function cargarTiempoResolucion(data) {
    $('#textoPromedioAtencion').text(data.PromedioAtencion + " dias");
    generarGraficoTiempoResolucion(data.Radial);
}

function generarGraficoTiempoResolucion(data) {
    var categorias = [];
    var datacategorias = [];

    $.each(data, function (i, v) {
        categorias.push({
            'label': toTitleCase(v.Servicio)
        });

        datacategorias.push({
            'value': v.Dias
        });
    });

    FusionCharts.ready(function () {
        var grafico = new FusionCharts({
            type: 'radar',
            width: '100%',
            height: '100%',
            dataFormat: 'json',
            dataSource: {
                "chart": {
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
                        "data": datacategorias
                    }
                ]
            }
        });

        grafico.render('tiempoResolucion-grafico', undefined);
    });
}


// Ranking motivos
function initTablaRankingMotivos() {
    tablaRankingMotivos = $('#tablaRankingMotivos').DataTableGeneral({
        "Orden": [2, "desc"],
        Paginar: true,
        VerInfo: true,
        Columnas: [
            {
                title: '',
                data: 'Posicion',
                orderable: false,
                render: function (data, type, row) {
                    return '<div><span>' + (data + '°') + '</span></div>';
                }
            },
            {
                title: "Nombre",
                data: "Motivo",
                orderable: false,
                render: function (data, type, row) {
                    return '<div><span class="texto-chico tooltipped" data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(data) + '">' + toTitleCase(data) + '</span></div>';
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

    //Muevo el indicador y el paginado a mi propio div
    /*
    $('#rankingMotivo .tabla-footer, #rankingMotivo2 .tabla-footer').empty();
    $('#rankingMotivo .dataTables_info, #rankingMotivo2 .dataTables_info').detach();
    $('#rankingMotivo .dataTables_paginate, #rankingMotivo2 .dataTables_paginate').detach();
    */
}

function cargarRankingMotivos(data) {
    $.each(data.Ranking_Motivos, function (index, element) {
        if (element !== undefined) {
            element.Posicion = (index + 1);
        }
    });

    // Rows tabla
    var height = calcularRows($('#rankingMotivo .contenedor .contenedor-main').height());
    $('#tablaRankingMotivos').DataTable().page.len(height).draw();

    // Borro los datos
    tablaRankingMotivos.clear().draw();

    // Agrego la info nueva
    if (data.Ranking_Motivos !== null && data.Ranking_Motivos !== undefined) {
        tablaRankingMotivos.rows.add(data.Ranking_Motivos).draw();
    }
}


//Areas operativas
function initTablaAreasOperativas() {
    tablaAreasOperativas = $('#tablaAreasOperativas').DataTableGeneral({
        Paginar: true,
        VerInfo: true,
        Columnas: [
            {
                title: "",
                data: 'Color',
                render: function (data, type, row) {
                    return '<div><label class="indicador-estado" style="background-color: ' + data + '"></label><span></span></div>';
                }
            },
            {
                title: "Area",
                data: 'Area',
                orderable: false,
                render: function (data, type, row) {
                    return '<div><span class="texto-chico tooltipped" data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(data) + '">' + toTitleCase(data) + '</span></div>';
                }
            },
            {
                title: 'Atendidos',
                data: null,
                Orderable: false,
                render: function (data, type, row) {
                    return '<div><span>' + row.Atendidos + "/" + row.Total + "<br/><label style='color: " + row.Color + "'>" + Math.floor(Math.round(row.Porcentaje * 100) / 100) + "%" + '</label></span></div>';
                }
            }
        ]
    });

    //Muevo el indicador y el paginado a mi propio div
    /*
    $('#areasOperativas .tabla-footer, #areasOperativas2 .tabla-footer').empty();
    $('#areasOperativas .dataTables_info, #areasOperativas2 .dataTables_info').detach();
    $('#areasOperativas .dataTables_paginate, #areasOperativas2 .dataTables_paginate').detach();
    */
}

function cargarAreasOperativas(data) {
    // Rows tabla
    var height = calcularRows($('#areasOperativas .contenedor .contenedor-main').height());
    $('#tablaAreasOperativas').DataTable().page.len(height).draw();

    // Borro los datos
    tablaAreasOperativas.clear().draw();

    // Agrego la info nueva
    if (data.CriticidadServicios != null && data.CriticidadServicios != undefined) {
        tablaAreasOperativas.rows.add(data.CriticidadServicios).draw();
    }
}


/* Generales */
function calcularRows(hDisponible) {
    var hEncabezado = 50;
    var hItem = 40;
    hDisponible = hDisponible - hEncabezado;
    return Math.floor(hDisponible / hItem);
}

function sizeChanged() {
    var heightAreasOperativas = calcularRows($('#areasOperativas .contenedor .contenedor-main').height());
    $('#tablaAreasOperativas').DataTable().page.len(heightAreasOperativas).draw();

    var heightRankingMotivo = calcularRows($('#rankingMotivo .contenedor .contenedor-main').height());
    $('#tablaRankingMotivos').DataTable().page.len(heightRankingMotivo).draw();
}

var Tablas = {
    intervaloPaginacion: undefined,
    tiempoEntrePaginacion: 15000,
    tiempoFadePaginacion: 500,

    mover: function () {
        if (!habilitarAutoPaginacion) return;

        Tablas.cancelar();
        Tablas.intervaloPaginacion = setInterval(Tablas.procesar, Tablas.tiempoEntrePaginacion);
    },

    cancelar: function () {
        if (!habilitarAutoPaginacion) return;

        if (Tablas.intervaloPaginacion != undefined) {
            clearInterval(Tablas.intervaloPaginacion);
        }
    },

    procesar: function () {
        if (!habilitarAutoPaginacion) return;

        Tablas.animar(tablaRankingMotivos, '#rankingMotivo .contenedor');
        Tablas.animar(tablaAreasOperativas, '#areasOperativas .contenedor');
    },

    animar: function (tabla, contenedor) {
        if (!habilitarAutoPaginacion) return;

        $(contenedor).animate({ opacity: 0 }, Tablas.tiempoFadePaginacion, function () {
            var actual = tabla.page.info().page;
            var fin = tabla.page.info().pages;

            if (actual < fin - 1) {
                tabla.page('next').draw('page');
            } else {
                tabla.page('first').draw('page');
            }

            $(contenedor).animate({ opacity: 1 }, Tablas.tiempoFadePaginacion);
        });
    }
};
