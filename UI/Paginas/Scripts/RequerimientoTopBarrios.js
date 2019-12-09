let MAX_ZOOM = 19;
let limiteTop = 20;

let map;
let topData;
let marcadoresData;

let info;
let idCategoria;

let categoriasFiltradas;

//Barrios
let barrios;
let barrioResaltado;
let poligonosBarrios = [];

let infoWindow;

//HeatMap
let viendoHeatMap = false;
let puntosHeatMap = [];
let heatMap;

let centroCordoba = { lat: parseFloat(-31.416111), lng: parseFloat(-64.191174) };
let infoAgrupadaPorBarrio;

function init(data) {
    data = parse(data);

    info = data.Info;


    if (getUsuarioLogeado().Ambito == undefined || getUsuarioLogeado().Ambito.KeyValue == 0) {
        $('#contenedor_Info').hide();
    } else {
        $('#contenedor_Info').show();
        $('#contenedor_Info > label').html('Como usted esta perfilado en ' + getUsuarioLogeado().Ambito.Nombre + ' solo ve  s asociados a ese CPC');
    }

    topData = data.Top;
    marcadoresData = data.Marcadores;

    initMapa();
    initFiltros();
};

function initFiltros() {
    $('#btn_Filtros').click(function () {
        $('#contenedor_FiltrosContenido').toggleClass('visible');
        if (!$('#contenedor_FiltrosContenido').hasClass('visible')) {
            $('#btn_Filtros').find('label').text('Filtrar');

            mostrarCargando(true);
            ajax_TopData(undefined)
                .then(function (response) {
                    mostrarCargando(false);
                    topData = response.TopData;
                    marcadoresData = response.Marcadores;
                    cargarDatos();
                })
                .catch(function (error) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', error);
                });
        } else {
            $('#btn_Filtros').find('label').text('Quitar filtros');
        }
    });

    $('#btn_Filtrar').click(function () {
        let idArea = $('#selectFiltro_Area').val();
        if (idArea == undefined) {
            mostrarMensaje('Error', 'Debe seleccionar el área');
            return;
        }

        if (idArea == "-1") {
            idArea = undefined;
        }

        let idZona = $('#selectFiltro_Zona').val();
        if (idZona == undefined || idZona == -1) {
            idZona = undefined;
            //mostrarMensaje('Error', 'Debe seleccionar la zona');
            //return;
        }

        idCategoria = $('#selectFiltro_Categoria').val();
        if (idCategoria == undefined || idCategoria == -1 || idCategoria == 0) {
            idCategoria = undefined;

        }


        if (idArea == "-1") {
            idArea = undefined;
        }

        mostrarCargando(true);
        ajax_TopData(idZona, idArea, idCategoria)
            .then(function (response) {
                mostrarCargando(false);
                topData = response.TopData;
                marcadoresData = response.Marcadores;
                cargarDatos();
            })
            .catch(function (error) {
                mostrarCargando(false);
                mostrarMensaje('Error', error);
            });
    });

    var def = "Todas mis áreas";
    if (getUsuarioLogeado().Areas.length == 1) {
        def = undefined;
        setTimeout(function () {
            $('#selectFiltro_Area').trigger('change');
        }, 100);

    }
    $('#selectFiltro_Area').CargarSelect({
        Data: getUsuarioLogeado().Areas,
        Default: def,
        Value: 'Id',
        Text: 'Nombre'
    });

    var cant = [{ Value: 50, Text: '50' }, { Value: 100, Text: '100' }, { Value: 200, Text: '200' }, { Value: 10000, Text: 'Todos' }];
    $('#selectFiltro_Cantidad').CargarSelect({
        Data: cant,
        Default: '20',
        Sort: false,
        Value: 'Value',
        Text: 'Text'
    });

    $('#selectFiltro_Cantidad').on('change', function () {
        let cantidad = $(this).val();
        if (cantidad == -1) {
            limiteTop = 20;
            return;
        }

        limiteTop = cantidad;
    });

    $('#selectFiltro_Zona').prop('disabled', true);
    $('#selectFiltro_Categoria').prop('disabled', true);

    $('#selectFiltro_Area').on('change', function () {
        let idArea = $(this).val();
        if (idArea == -1) {
            $('#selectFiltro_Zona').CargarSelect({
                Default: 'Seleccione...',
                Data: [],
                Value: 'Id',
                Text: 'Nombre'
            });
            $('#selectFiltro_Categoria').CargarSelect({
                Default: 'Seleccione...',
                Data: [],
                Value: 'Id',
                Text: 'Nombre'
            });

            return;
        }

        $('#selectFiltro_Zona').prop('disabled', true);
        $('#selectFiltro_Categoria').prop('disabled', true);

        crearAjax({
            Url: ResolveUrl('~/Servicios/ZonaService.asmx/GetByArea'),
            Data: { consulta: { IdsArea: [idArea], DadosDeBaja: false } },
            OnSuccess: function (result) {
                if (!result.Ok || result.Return.length == 0) {
                    $('#selectFiltro_Zona').CargarSelect({
                        Default: 'Seleccione...',
                        Data: [],
                        Value: 'Id',
                        Text: 'Nombre'
                    });
                    $('#selectFiltro_Zona').prop("disabled", true);
                    return;
                }
                $('#selectFiltro_Zona').prop('disabled', false);
                $('#selectFiltro_Zona').CargarSelect({
                    Default: 'Seleccione...',
                    Data: result.Return,
                    Value: 'Id',
                    Text: 'Nombre'
                });
            }
        })


        categoriasFiltradas = getCategoriasDeAreaBarrios(idArea);
        if (categoriasFiltradas.length == 0) {
            $('#selectFiltro_Categoria').CargarSelect({
                Default: 'Seleccione...',
                Data: [],
                Value: 'Id',
                Text: 'Nombre'
            });
            $('#selectFiltro_Categoria').prop("disabled", true);
            return;
        } else {
            $('#selectFiltro_Categoria').CargarSelect({
                Default: 'Seleccione...',
                Data: categoriasFiltradas,
                Value: 'Id',
                Text: 'Nombre'
            });
            $('#selectFiltro_Categoria').prop('disabled', false);
        }


    });
    //$('#selectFiltro_Area').trigger('change');
}

function initMapa() {
    getBarrios().then(function (b) {
        barrios = b;
        ControlMapa_Init({
            ResaltarAlHacerClick: false,
            OnValidarMarcador: function (data) {
                return false;
            },
            OnMapReady: function (mapaNuevo) {
                map = mapaNuevo;
                cargarDatos();
            }
        });
    });
}

function cargarDatos() {

    //Poligonos de los barrios
    infoAgrupadaPorBarrio = _.groupBy(topData, function (element) { return element.BarrioId; });

    //Creo la info de cada barrio
    let dataBarrios = [];
    $.each(infoAgrupadaPorBarrio, function (index, element) {
        let cantidad = 0;
        $.each(element, function (index1, element1) {
            cantidad += element1.Cantidad;
        });

        dataBarrios.push({
            Id: element[0].BarrioId,
            Nombre: element[0].BarrioNombre,
            Cantidad: cantidad
        });
    });
    //Ordeno y limito los barrios
    let idsBarrios = [];
    dataBarrios = _.sortBy(dataBarrios, 'Cantidad').reverse();
    if (dataBarrios.length > limiteTop) {
        dataBarrios = dataBarrios.slice(0, limiteTop);
    }
    idsBarrios = _.pluck(dataBarrios, 'Id');

    //Filtro los marcadores segun los barrios
    marcadoresData = $.grep(marcadoresData, function (element, index) {
        return idsBarrios.indexOf(element.BarrioId) != -1;
    });

    //Itero los barrios
    if (poligonosBarrios != undefined && poligonosBarrios.length != 0) {
        $.each(poligonosBarrios, function (index, element) {
            element.setMap(undefined);
            element = undefined;
        });
        poligonosBarrios = [];
    }

    $.each(dataBarrios, function (indexBarrio, elementBarrio) {

        if (elementBarrio.Id > 0) {
            let barrio = $.grep(barrios, function (element, index) { return element.id == elementBarrio.Id })[0];
            if (barrio == undefined) {
                return true;
            }

            let poligono = barrio.poligono;
            let p = new google.maps.Polygon({
                paths: poligono,
                strokeColor: 'black',
                strokeOpacity: 1,
                strokeWeight: 1,
                fillColor: 'black',
                fillOpacity: 0,
                clickable: true,
                zIndex: 1000,
                map: map
            });

            p.set('BarrioId', elementBarrio.Id);
            poligonosBarrios.push(p);

            google.maps.event.addListener(p, 'click', function (event) {
                if (barrioResaltado == elementBarrio.Id) {
                    desresaltarBarrio();
                } else {
                    resaltarBarrio(elementBarrio.Id);
                }
            });
        }

    });

    //Contenido del costado
    cargarContenido();

    //Por defecto sin resaltar barrio
    desresaltarBarrio();
    verHeatMap();
}

function cargarContenido() {
    let agrupada = _.groupBy(topData, function (element) { return element.BarrioId; });

    let dataBarrios = [];
    $.each(agrupada, function (index, element) {
        let cantidad = 0;
        $.each(element, function (index1, element1) {
            cantidad += element1.Cantidad;
        });

        dataBarrios.push({
            Id: element[0].BarrioId,
            Nombre: element[0].BarrioNombre,
            Cantidad: cantidad
        });
    });

    dataBarrios = _.sortBy(dataBarrios, 'Cantidad').reverse();
    if (dataBarrios.length > limiteTop) {
        dataBarrios = dataBarrios.slice(0, limiteTop);
    }
    $('#contenedor_Data').empty();

    $.each(dataBarrios, function (index, element) {
        let div = $($('#template_Barrio').html());
        $(div).attr('id-barrio', element.Id);

        $(div).find('.encabezado .numero').text((index + 1) + '°');
        $(div).find('.encabezado .barrio').text(toTitleCase(element.Nombre));
        $(div).find('.encabezado .cantidad').text(element.Cantidad);

        $(div).click(function () {
            if (barrioResaltado == element.Id) {
                desresaltarBarrio();
            } else {
                resaltarBarrio(element.Id);
            }
        });

        $('#contenedor_Data').append(div);
    });
}

function crearHtmlMarcadorPopup(data) {
    var html = $($('#template_BarrioArea').html());
    $(html).attr('area-id', data.AreaId);
    $(html).attr('area-nombre', data.AreaNombre);
    $(html).attr('categoria-id', data.CategoriaId);
    $(html).attr('categoria-nombre', data.CategoriaNombre);
    $(html).find('.nombre').text(data.AreaNombre);
    $(html).find('.cantidad').text(data.Cantidad);
    return html;
}



function crearHtmlMarcadorPopupLista(barrio, data, categoria) {

    var html = $('<div class="popupLista">');
    $(html).attr('barrio-id', barrio.id);
    $(html).attr('barrio-nombre', barrio.nombre);

    $(html).append('<div class="encabezado"><label class="titulo">' + toTitleCase(barrio.nombre) + '</label></div>');

    $.each(data, function (index, element) {
        var htmlPopup = crearHtmlMarcadorPopup(element);
        if (htmlPopup != undefined) {
            htmlPopup.appendTo(html);
        }
    });

    return html;
}

function onClickPopupArea(e) {
    let areaId = $(e).attr('area-id');
    let areaNombre = $(e).attr('area-nombre');

    let barrioId = $(e).parents('.popupLista').attr('barrio-id');
    let barrioNombre = $(e).parents('.popupLista').attr('barrio-nombre');

    let categoriaId = idCategoria;

    //var categoriaNombre = _.where(categoriasFiltradas, { Id: parseInt(idCategoria) })[0].Nombre;

    //var resultadoZonas = $.grep(zonas, function (e) { return e.Id == val; });
    //if (resultadoZonas != undefined && resultadoZonas.length != 0) {
    //    val = toTitleCase(resultadoZonas[0].Nombre);
    //} else {
    //    return true;
    //}
    //let categoriaNombre = categoriasFiltradas;

    let texto = '¿Desea ver los requerimientos asociados en la <b>bandeja de entrada</b> con los siguientes filtros? <br/><b>Área: </b>' + toTitleCase(areaNombre) + '<br/><b>Barrio: </b>' + toTitleCase(barrioNombre);
    let parametros = 'idBarrio=' + barrioId + '&idArea=' + areaId;

    if (idCategoria) {
        let categoriaNombre = _.where(categoriasFiltradas, { Id: parseInt(idCategoria) })[0].Nombre;
        texto += '<br/><b>Categoria: </b>' + toTitleCase(categoriaNombre);
        parametros += '&idCategoria=' + categoriaId
    }

    crearDialogoConfirmacion({

        Texto: texto,
        CallbackPositivo: function () {
            top.header_CambiarPagina({
                Url: 'OrdenesDeTrabajoBandeja',
                Titulo: 'Bandeja de entrada',
                Params: parametros
            });
        },
        TextoBotonAceptar: 'Si',
        TextoBotonCancelar: 'No'
    });
}

function pinSymbol(color) {
    return {
        path: 'M31.5,0C14.1,0,0,14,0,31.2C0,53.1,31.5,80,31.5,80S63,52.3,63,31.2C63,14,48.9,0,31.5,0z M31.5,52.3 c-11.8,0-21.4-9.5-21.4-21.2c0-11.7,9.6-21.2,21.4-21.2s21.4,9.5,21.4,21.2C52.9,42.8,43.3,52.3,31.5,52.3z',
        fillColor: color,
        fillOpacity: 1,
        anchor: new google.maps.Point(35, 70),
        strokeColor: '#000',
        strokeWeight: 2,
        scale: 0.45,
    };
}

function verHeatMap() {
    if (map == undefined) return;

    puntosHeatMap = [];
    $.each(marcadoresData, function (index, element) {
        let pos = new google.maps.LatLng(parseFloat(element.Latitud.replace(',', '.')), parseFloat(element.Longitud.replace(',', '.')));
        puntosHeatMap.push(pos);
    });

    if (heatMap != undefined) {
        heatMap.setMap(undefined);
    }
    heatMap = new google.maps.visualization.HeatmapLayer({
        data: puntosHeatMap,
        map: map,
        zIndex: 20000
    });
    heatMap.set('radius', 20);
}



function resaltarBarrio(id) {
    barrioResaltado = id;

    //resalto el poligono
    $.each(poligonosBarrios, function (index, element) {
        let idBarrio = element.get('BarrioId');
        element.setOptions({
            fillOpacity: idBarrio == id ? 0.2 : 0
        });
    });

    //Centro en el barrio
    let barrio = $.grep(barrios, function (element, index) { return element.id == id })[0];
    let barrioPoligono = barrio.poligono;
    let bounds = new google.maps.LatLngBounds();
    for (let i = 0; i < barrioPoligono.length; i++) {
        bounds.extend(barrioPoligono[i]);
    }
    map.fitBounds(bounds);

    //Resaltado en el listado
    $('.data-barrio').removeClass('seleccionada');
    $('.data-barrio[id-barrio=' + id + ']').addClass('seleccionada');


    //Popup
    if (infoWindow != undefined) {
        infoWindow.setMap(undefined);
        infoWindow = undefined;
    }

    let html = crearHtmlMarcadorPopupLista(barrio, infoAgrupadaPorBarrio[id]);

    infoWindow = new google.maps.InfoWindow({
        content: $(html).prop('outerHTML'),
        maxWidth: 400,
        maxHeight: 400
    });

    google.maps.event.addListener(infoWindow, 'closeclick', function () {
        desresaltarBarrio();
    });

    infoWindow.setPosition(bounds.getCenter());
    infoWindow.open(map);

}

function desresaltarBarrio() {
    barrioResaltado = undefined;

    //desresalto el poligono
    $.each(poligonosBarrios, function (index, element) {
        element.setOptions({
            fillOpacity: 0
        });
    });

    //centro
    map.setCenter(centroCordoba);
    map.setZoom(11);

    //Desresaltado en el listado
    $('.data-barrio').removeClass('seleccionada');

    //Cierro el popup
    if (infoWindow != undefined) {
        infoWindow.setMap(undefined);
        infoWindow = undefined;
    }
}


function ajax_TopData(idZona, idArea, idCategoria) {

    let consulta = idZona != undefined || idArea != undefined || idCategoria != undefined ? { IdZona: idZona, IdArea: idArea, IdCategoria: idCategoria } : null;

    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetTopPorBarrio'),
            Data: { consulta: consulta },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                crearAjax({
                    Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetTopMarcadoresPorBarrio'),
                    Data: { consulta: { IdZona: idZona } },
                    OnSuccess: function (result2) {
                        if (!result2.Ok) {
                            callbackError(result2.Error);
                            return;
                        }

                        callback({ TopData: result.Return, Marcadores: result2.Return });
                    },
                    OnError: function (result) {
                        callbackError('Error procesando la solicitud');
                    }
                });
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        });
    });

}

function getCategoriasDeAreaBarrios(id) {
    let data = [];
    if (id != -1) {
        data = _.groupBy(_.where(info, { AreaId: parseInt(id) }), function (item) { return item.CategoriaId });
    }

    //Cargo el array
    let categorias = [];
    var sinAsignar = false;
    $.each(data, function (index, element) {
        //Genero la categoría
        if (element[0].CategoriaId != 0) {
            let categoria = {
                Id: element[0].CategoriaId,
                Nombre: element[0].CategoriaNombre,
                IdArea: element[0].AreaId
            };
            categorias.push(categoria);
        } else {
            sinAsignar = true;
        }
    });
    categorias = _.sortBy(categorias, 'Nombre');

    //Elemento todos
    // categorias.unshift({ Id: -1, Nombre: 'Todas' });

    //Elemento sin asignar
    if (sinAsignar && categorias.length>0) {
        categorias.push({ Id: 0, Nombre: 'Sin Asignar' });
    }

    return categorias;
}