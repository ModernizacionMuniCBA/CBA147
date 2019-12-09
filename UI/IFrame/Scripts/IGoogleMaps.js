
var map;
var callbackReady;

var ZOOM_UN_MARCADOR = 17;
var MAX_ZOOM = 19;

var informado = false;

//Capas
var layerCPC;

//Para marcadores
var marcadores = [];
var dataMarcadores = [];
var markerCluster;
var callbackClickMarcador;
var clusterOptions = {};

//Para HeatMap
var puntos = [];
var heatMap;

function initMap() {

    var myStyles = [
    {
        featureType: "poi",
        elementType: "labels",
        stylers: [
              { visibility: "off" }
        ]
    }
    ];

    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 8,
        maxZoom: MAX_ZOOM,
        fullscreenControl: false,
        gestureHandling: 'greedy',
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            position: google.maps.ControlPosition.TOP_RIGHT
        },
        styles: myStyles
    });

    initLayers();
    initBotones();

    //Informar listo
    var w = $(window).width();
    var h = $(window).height();
    if (w != 0 && h != 0) {
        centrarEnCordoba(function () {
            if (!informado) {
                informado = true;
                informarReady();
            }
        });
    }

    $(window).resize(function () {

        var w = $(window).width();
        var h = $(window).height();
        if (w != 0 && h != 0) {
            centrarEnCordoba(function () {
                if (!informado) {
                    informado = true;
                    informarReady();
                }
            });
        }
    });

    google.maps.event.addDomListener(window, "resize", function () {
        var center = map.getCenter();
        google.maps.event.trigger(map, "resize");
        map.setCenter(center);
    });
}

function centrarEnCordoba(callback) {
    var geocoder = new google.maps.Geocoder();

    geocoder.geocode({ 'address': 'Cordoba, Argentina' }, function (results, status) {
        var ne = results[0].geometry.viewport.getNorthEast();
        var sw = results[0].geometry.viewport.getSouthWest();

        map.fitBounds(results[0].geometry.viewport);

        callback();
    });
}

function mover(x, y) {
    if (map == undefined) return;

    var center = new google.maps.LatLng(x, y);
    map.panTo(center);
}


/* Apu publica */

function verMarcadores(data, callbackClick) {
    if (map == undefined) return;
    mostrarCargando(true);
    reiniciar();

    dataMarcadores = data;

    //guardo el callback para cuando hago click en un marcador
    callbackClickMarcador = callbackClick;

    //Creo el marcador
    $.each(data, function (index, element) {

        var marcador = { lat: element.Latitud, lng: element.Longitud };

        var marker = new google.maps.Marker({
            position: marcador
        });

        marker.setIcon(pinSymbol(element.Color));
        marker.set("id", element.Id);

        var html = crearHtmlPopup(element.Id, element.Titulo, element.Descripcion, callbackClick);
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

    //Cluster
    clusterOptions = {
        ignoreHidden: true,
        imagePath: 'scripts/googlemaps/cluster/cluster'
    }

    markerCluster = new MarkerClusterer(map, marcadores, clusterOptions);

    //Espero a que este listo
    google.maps.event.addListenerOnce(map, 'idle', function () {
        var bounds = new google.maps.LatLngBounds();
        $.each(data, function (index, element) {
            var lat = element.Latitud;
            var lng = element.Longitud;

            var latlang = new google.maps.LatLng(lat, lng);
            bounds.extend(latlang);
        });
        map.fitBounds(bounds);

        mostrarCargando(false);
    });

    //Click en el cluster
    google.maps.event.addListener(markerCluster, 'clusterclick', function (cluster) {
        if (map.getZoom() == MAX_ZOOM) {
            var markers = cluster.getMarkers();

            //creo el texto del popup del cluster, con el detalle de todos los RQ que contiene
            var dataPopup = [];
            $.each(markers, function (index, element) {


                var idMarcador = element.get("id");
                var dataMarcador = $.grep(data, function (element, index) {
                    return element.Id == idMarcador;
                })[0];

                console.log(dataMarcador);

                dataPopup.push({
                    Id: idMarcador,
                    Titulo: dataMarcador.Titulo,
                    Descripcion: dataMarcador.Descripcion
                });
            });

            var html = crearHtmlListaPopup(dataPopup, callbackClick);
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
}

function crearHtmlPopup(id, titulo, descripcion, callback) {
    if (titulo == undefined || titulo == "") return undefined;

    var html = $('<div class="popup">');

    var divTextos = $('<div class="textos">');
    $(divTextos).appendTo(html);

    var labelTitulo = $('<label class="titulo">');
    $(labelTitulo).text(titulo);
    $(labelTitulo).appendTo(divTextos);

    if (descripcion != undefined) {
        var labelDescripcion = $('<label class="descripcion">');
        $(labelDescripcion).text(descripcion);
        $(labelDescripcion).appendTo(divTextos);
    }

    if (callback != undefined) {
        var btn = $('<a class="btn waves-effect btn-cuadrado" onclick="clickBotonDetalle(' + id + ')"><i class="material-icons btn-icono">description</i></a>');
        $(btn).appendTo(html);
    }

    return html;
}

function crearHtmlListaPopup(data, callback) {

    var html = $('<div class="popupLista">');

    $.each(data, function (index, element) {
        var htmlPopup = crearHtmlPopup(element.Id, element.Titulo, element.Descripcion, callback);
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

function verHeatMap(data) {
    if (map == undefined) return;
    mostrarCargando(true);
    reiniciar();

    puntos = [];
    $.each(data, function (index, element) {
        puntos.push(new google.maps.LatLng(element.Latitud, element.Longitud));
    });

    heatmap = new google.maps.visualization.HeatmapLayer({
        data: puntos,
        map: map
    });

    //Espero a que este listo
    google.maps.event.addListenerOnce(map, 'idle', function () {
        setTimeout(function () {
            mostrarCargando(false);
        }, 100);
    });
}

function reiniciar() {
    if (map == undefined) return;

    //Marcadores
    dataMarcadores = undefined;
    if (marcadores != undefined) {
        $.each(marcadores, function (index, element) {
            element.map = null;
        });
    }
    marcadores = [];
    callbackClickMarcador = undefined;
    if (markerCluster != undefined) {
        markerCluster.clearMarkers();
        markerCluster.setMap(undefined);
    }
    markerCluster = undefined;
    clusterOptions = undefined;

    //HeatMap
    puntos = [];
    if (heatMap != undefined) {
        heatMap.setMap(undefined);
    }
    heatMap = undefined;
}


/* Utils */

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


/* Capas */

function initLayers() {
    initLayerCPC();
}

function initLayerCPC() {
    //CPC
    layerCPC = new google.maps.KmlLayer({
        url: 'http://res.cloudinary.com/dtwwgntjc/raw/upload/v1515777575/cpc.kmz',
        map: null
    });
}


/* Botones */

function initBotones() {

    var contenedorBtnCentrar = document.createElement('div');
    new BotonCentrar(contenedorBtnCentrar, map);
    contenedorBtnCentrar.index = 1;
    map.controls[google.maps.ControlPosition.TOP_RIGHT].push(contenedorBtnCentrar);

    var contenedorBtnLayerCPC = document.createElement('div');
    new BotonCpc(contenedorBtnLayerCPC, map);
    contenedorBtnLayerCPC.index = 1;
    map.controls[google.maps.ControlPosition.LEFT_TOP].push(contenedorBtnLayerCPC);

    var contenedorBtnMapaCalor = document.createElement('div');
    new BotonMapaCalor(contenedorBtnMapaCalor, map);
    contenedorBtnMapaCalor.index = 1;
    map.controls[google.maps.ControlPosition.LEFT_TOP].push(contenedorBtnMapaCalor);
}

function BotonCentrar(controlDiv, map) {
    // Set CSS for the control border.
    var controlUI = document.createElement('div');
    $(controlUI).addClass('btnMapa');
    controlUI.setAttribute("id", "btnCentrar");
    controlUI.title = 'Click para centrar el mapa';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior.
    var controlText = document.createElement('div');
    controlText.innerHTML = 'Centrar';
    controlUI.appendChild(controlText);

    // Setup the click event listeners: simply set the map to Chicago.
    controlUI.addEventListener('click', function () {
        var bounds = new google.maps.LatLngBounds();
        $.each(dataMarcadores, function (index, element) {
            var lat = element.Latitud;
            var lng = element.Longitud;

            var latlang = new google.maps.LatLng(lat, lng);
            bounds.extend(latlang);
        });
        map.fitBounds(bounds);
    });
}

function BotonCpc(controlDiv, map) {

    // Set CSS for the control border.
    var controlUI = document.createElement('div');
    $(controlUI).addClass('btnMapa');
    controlUI.setAttribute("id", "btnLayerCPC");
    controlUI.title = 'Click to recenter the map';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior.
    var controlText = document.createElement('div');
    controlText.innerHTML = 'Ver CPC';
    controlUI.appendChild(controlText);

    // Setup the click event listeners: simply set the map to Chicago.
    controlUI.addEventListener('click', function () {
        $(controlUI).toggleClass('activo');

        if (layerCPC.getMap() == null) {
            layerCPC.setMap(map);
            controlText.innerHTML = "Ocultar CPC";
        } else {
            layerCPC.setMap(null);
            controlText.innerHTML = "Ver CPC";
        }
    });
}

var viendoMapaDeCalor = false;
var heatMapMarcadores;

function BotonMapaCalor(controlDiv, map) {

    // Set CSS for the control border.
    var controlUI = document.createElement('div');
    $(controlUI).addClass('btnMapa');
    controlUI.setAttribute("id", "btnLayerCPC");
    controlUI.title = 'Click to recenter the map';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior.
    var controlText = document.createElement('div');
    controlText.innerHTML = 'Ver Mapa de Calor';
    controlUI.appendChild(controlText);

    // Setup the click event listeners: simply set the map to Chicago.
    controlUI.addEventListener('click', function () {
        $(controlUI).toggleClass('activo');


        if (viendoMapaDeCalor) {
            heatMapMarcadores.setMap(null);
            $.each(marcadores, function (index, element) {
                element.setMap(null);
            });
            markerCluster.addMarkers(marcadores);
        } else {
            markerCluster.clearMarkers();

            var puntos = [];
            $.each(dataMarcadores, function (index, element) {
                puntos.push(new google.maps.LatLng(element.Latitud, element.Longitud));
            });

            heatMapMarcadores = new google.maps.visualization.HeatmapLayer({
                data: puntos,
                map: map
            });
            heatMapMarcadores.set('radius', 20);
        }

        viendoMapaDeCalor = !viendoMapaDeCalor;
    });
}

/* Listener */

function setOnReadyListener(callback) {
    callbackReady = callback;
}

function informarReady() {
    if (callbackReady == undefined) return;
    callbackReady();
}

