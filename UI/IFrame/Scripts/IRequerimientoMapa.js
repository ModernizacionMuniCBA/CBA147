
var map;
let MAX_ZOOM = 19;

//Para marcadores
let marcadores = [];
let marcadores_data = [];
let backup_data;

let markerCluster;
let callbackClickMarcador;
let clusterOptions = {};

let areaSeleccionada;
let estadoSeleccionado;

//Para HeatMap
let puntos = [];
let heatMap;
let viendoHeatMap = false;

let SVG_MARKER = 'M31.5,0C14.1,0,0,14,0,31.2C0,53.1,31.5,80,31.5,80S63,52.3,63,31.2C63,14,48.9,0,31.5,0z M31.5,52.3 c-11.8,0-21.4-9.5-21.4-21.2c0-11.7,9.6-21.2,21.4-21.2s21.4,9.5,21.4,21.2C52.9,42.8,43.3,52.3,31.5,52.3z';

function init(data) {
    ControlMapa_Init({
        ResaltarAlHacerClick: false,
        OnMapReady: function (mapaNuevo) {
            mapa = mapaNuevo;

            //Cluster
            clusterOptions = {
                ignoreHidden: true,
                imagePath: 'scripts/googlemaps/cluster/m',
                textColor: "white",
            }
            markerCluster = new MarkerClusterer(map, [], clusterOptions);


            let btn = $('<a class="btn-mapa">Mapa de calor</a>');
            $('#ControlMapa_Botones').prepend(btn);
            $(btn).click(function () {
                $(this).toggleClass('seleccionado');

                viendoHeatMap = !viendoHeatMap;
                refresh();
            });
            informarReady();
        }
    });

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetEstadoRequerimiento'),
        OnSuccess: function (result) {
            if (!result.Ok) {
                return;
            }

            $('#btn_Referencias').click(function () {
                $('.btn-filtro').fadeOut(300);
                $('#contenedor_Referencias').addClass('visible');
            });

            $('#contenedor_Referencias .btn_Cerrar').click(function () {
                $('.btn-filtro').fadeIn(300);
                $('#contenedor_Referencias').removeClass('visible');
            });

            $.each(result.Return, function (index, element) {
                let div = $($('#template_Referencia').html());
                $(div).find('svg').attr("fill", "#" + element.Color);
                $(div).find('svg path').attr("d", SVG_MARKER);
                $(div).find('.nombre').text(element.Nombre);

                $('#contenedor_Referencias .contenedor-main').append(div);
            });
        },
        OnError: function (result) {
        }
    })

    $('#btn_Areas').click(function () {
        $('.btn-filtro').fadeOut(300);
        $('#contenedor_Areas').addClass('visible');
    });
    $('#contenedor_Areas .btn_Cerrar').click(function () {
        $('.btn-filtro').fadeIn(300);
        $('#contenedor_Areas').removeClass('visible');
    });

    $('#btn_Estados').click(function () {
        $('.btn-filtro').fadeOut(300);
        $('#contenedor_Estados').addClass('visible');
    });
    $('#contenedor_Estados .btn_Cerrar').click(function () {
        $('.btn-filtro').fadeIn(300);
        $('#contenedor_Estados').removeClass('visible');
    });
}

/* Apu publica */

function verMarcadores(data, callbackClick) {
    if (map == undefined) return;

    mostrarCargando(true);
    ajax_GetData(data)
        .then(function (info) {
            mostrarCargando(false);

            marcadores_data = info;
            callbackClickMarcador = callbackClick;

            backup_data = $.extend(true, {}, marcadores_data);
            cargarData();

        })
        .catch(function (error) {
            mostrarMensaje('Error', error);
        });


}

function cargarData() {

    //Creo los marcadores
    $.each(marcadores_data, function (index, element) {

        var marcador = { lat: parseFloat(element.Latitud.replace(',', '.')), lng: parseFloat(element.Longitud.replace(',', '.')) };

        var marker = new google.maps.Marker({
            position: marcador
        });

        marker.setIcon(pinSymbol('#' + element.EstadoColor));
        marker.set("id", element.Id);
        marker.set("AreaId", element.AreaId);
        marker.set("EstadoKeyValue", element.EstadoKeyValue);

        var html = crearHtmlPopup(element, callbackClickMarcador);
        if (html != undefined) {

            var infowindow = new google.maps.InfoWindow({
                content: $(html).prop('outerHTML'),
                maxWidth: 400
            });

            marker.addListener('click', function () {
                infowindow.open(map, marker);
            });
        }

        //Lo agrego al listado
        marcadores.push(marker);
    });

    //Agrego al cluster
    markerCluster.addMarkers(marcadores);

    //Espero a que este listo
    google.maps.event.addListenerOnce(map, 'idle', function () {
        var bounds = new google.maps.LatLngBounds();
        $.each(marcadores_data, function (index, element) {
            var lat = parseFloat(element.Latitud.replace(',', '.'));
            var lng = parseFloat(element.Longitud.replace(',', '.'));

            var latlang = new google.maps.LatLng(lat, lng);
            bounds.extend(latlang);
        });
        map.fitBounds(bounds);
    });


    //Click en el cluster
    google.maps.event.addListener(markerCluster, 'clusterclick', function (cluster) {
        if (map.getZoom() == MAX_ZOOM) {
            var markers = cluster.getMarkers();

            //creo el texto del popup del cluster, con el detalle de todos los RQ que contiene
            var dataPopup = [];
            $.each(markers, function (index, element) {

                var idMarcador = element.get("id");
                var dataMarcador = $.grep(marcadores_data, function (element, index) {
                    return element.Id == idMarcador;
                })[0];

                dataPopup.push(dataMarcador);
            });

            var html = crearHtmlListaPopup(dataPopup, callbackClickMarcador);
            if (html != undefined) {
                var infowindow = new google.maps.InfoWindow({
                    content: $(html).prop('outerHTML'),
                    maxWidth: 400
                });

                infowindow.setPosition(cluster.getCenter());
                infowindow.open(map);
            }
        }
    });

    //Areas
    let areas = [];
    areaSeleccionada = {
        Id: -1,
        Nombre: 'Todas'
    };

    areas.push(areaSeleccionada);
    $.each(marcadores_data, function (index, element) {
        let encontrado = $.grep(areas, function (e, i) { return e.Id == element.AreaId })[0];
        if (encontrado == undefined) {
            areas.push({
                Id: element.AreaId,
                Nombre: element.AreaNombre,
                Cantidad: 1
            });
        } else {
            encontrado.Cantidad++;
        }
    });

    areas = ordenarJSON(areas, 'Cantidad', false);

    $('#contenedor_Areas .contenedor-main').empty();
    $.each(areas, function (index, element) {
        let div = $($('#template_Item').html());
        $(div).find('.nombre').text(element.Nombre);

        let esTodos = element.Id == -1;
        if (esTodos) {
            $(div).addClass('seleccionado');
            $(div).find('.cantidad').hide();
        } else {
            $(div).find('.cantidad').show();
            $(div).find('.cantidad').text(element.Cantidad);
        }
        $('#contenedor_Areas .contenedor-main').append(div);

        $(div).click(function () {
            $('#contenedor_Areas .item').removeClass('seleccionado');
            $(this).addClass('seleccionado');

            areaSeleccionada = element;
            if (areaSeleccionada.Id == -1) {
                $('#btn_Areas').removeClass('seleccionado');
            } else {
                $('#btn_Areas').addClass('seleccionado');
            }
            refresh();
        });
    });


    //Estados
    let estados = [];
    estadoSeleccionado = {
        KeyValue: -1,
        Nombre: 'Todos los estados'
    };

    estados.push(estadoSeleccionado);
    $.each(marcadores_data, function (index, element) {
        let encontrado = $.grep(estados, function (e, i) { return e.KeyValue == element.EstadoKeyValue })[0];
        if (encontrado == undefined) {
            estados.push({
                KeyValue: element.EstadoKeyValue,
                Nombre: element.EstadoNombre,
                Cantidad: 1
            });
        } else {
            encontrado.Cantidad++;
        }
    });

    estados = ordenarJSON(estados, 'Cantidad', false);

    $('#contenedor_Estados .contenedor-main').empty();
    $.each(estados, function (index, element) {
        let div = $($('#template_Item').html());
        $(div).find('.nombre').text(element.Nombre);

        let esTodos = element.KeyValue == -1;
        if (esTodos) {
            $(div).addClass('seleccionado');
            $(div).find('.cantidad').hide();
        } else {
            $(div).find('.cantidad').show();
            $(div).find('.cantidad').text(element.Cantidad);
        }
        $('#contenedor_Estados .contenedor-main').append(div);

        $(div).click(function () {
            $('#contenedor_Estados .item').removeClass('seleccionado');
            $(this).addClass('seleccionado');

            estadoSeleccionado = element;
            if (estadoSeleccionado.KeyValue == -1) {
                $('#btn_Estados').removeClass('seleccionado');
            } else {
                $('#btn_Estados').addClass('seleccionado');
            }
            refresh();
        });
    });
}

function crearHtmlPopup(data, callback) {

    var html = $($('#template_Requerimiento').html());
    $(html).find('.numero').text(data.Numero + '/' + data.Año);
    $(html).find('.estado').html('<b>Estado </b>' + data.EstadoNombre);
    $(html).find('.numero').attr('onClick', 'clickBotonDetalle(' + data.Id + ');');

    return html;
}

function crearHtmlListaPopup(data, callback) {

    var html = $('<div class="popupLista">');

    $.each(data, function (index, element) {
        var htmlPopup = crearHtmlPopup(element, callback);
        if (htmlPopup != undefined) {
            htmlPopup.appendTo(html);
        }
    });

    return html;
}

function clickBotonDetalle(id) {
    if (callbackClickMarcador == undefined) return;
    callbackClickMarcador(id);
}

function verHeatMap() {
    if (map == undefined) return;

    puntos = [];
    $.each(marcadores, function (index, element) {
        if (areaSeleccionada == undefined || areaSeleccionada.Id == -1) {
            puntos.push(element.getPosition());
        } else {
            let idMarcador = element.get('AreaId');
            let visible = idMarcador == areaSeleccionada.Id;
            if (visible) {
                puntos.push(element.getPosition());
            }
        }
    });

    if (heatMap != undefined) {
        heatMap.setMap(undefined);
    }
    heatMap = new google.maps.visualization.HeatmapLayer({
        data: puntos,
        map: map
    });
    heatMap.set('radius', 20);
}

function refresh() {
    actualizarMarcadores();

    if (viendoHeatMap) {
        verHeatMap();
    } else {
        if (heatMap != undefined) {
            heatMap.setMap(undefined);
            heatMap = undefined;
        }
    }
}

function actualizarMarcadores() {
    $.each(marcadores, function (i, e) {
        let esTodos = areaSeleccionada != undefined && areaSeleccionada.Id == -1;

        let visible;
        if (viendoHeatMap) {
            visible = false;
        } else {
            let area = false;
            if (areaSeleccionada == undefined || areaSeleccionada.Id == -1) {
                area = true;
            } else {
                area = e.get('AreaId') == areaSeleccionada.Id;
            }

            let estado = false;
            if (estadoSeleccionado == undefined || estadoSeleccionado.KeyValue == -1) {
                estado = true;
            } else {
                estado = e.get('EstadoKeyValue') == estadoSeleccionado.KeyValue;
            }

            visible = estado && area;
        }
        e.setVisible(visible);
    });
    markerCluster.setIgnoreHidden(true);
    markerCluster.repaint();
}


/* Utils */

function pinSymbol(color) {
    return {
        path: SVG_MARKER,
        fillColor: color,
        fillOpacity: 1,
        anchor: new google.maps.Point(35, 70),
        strokeColor: '#000',
        strokeWeight: 2,
        scale: 0.45,
    };
}

function ajax_GetData(ids) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetMarcadoresGoogleMapsPorIds'),
            Data: { ids: ids },
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

function imprimir() {
    window.print();
}


//Callback

let listenerReady;

function setListenerReady(callback) {
    listenerReady = callback;
}

function informarReady() {
    if (listenerReady == undefined) return;
    listenerReady();
}

