let oi;
let permisos;

let panelCallback;

//Mapa
let MAPA_MAX_ZOOM = 19;
let MAPA_ZOOM_MARCADOR = 17;
let MAPA_ZOOM_INICIAL = 13;
let map;

let mapaExpandido
let mapa_marcadores = [];
let mapa_popups = [];
let mapa_PoligonosBarrios = [];
let mapa_PopupZona;
let mapa_ColorZona = '#E91E63';
let mapa_OpacityZona = 0.3;
let mapa_ZIndexZona = 500;

let PATH_IMAGEN_USUARIO_DEFAULT;

let PERMISO_EDITAR_DESCRIPCION = 1;
let PERMISO_AGREGAR_REQUERIMIENTO = 2;
let PERMISO_QUITAR_REQUERIMIENTO = 3;
let PERMISO_AGREGAR_NOTA = 4;
let PERMISO_CERRAR = 5;
let PERMISO_CANCELAR = 6;

function init(data) {
    if ('Error' in data) {
        mostrarError(data.Error);
        return;
    }

    PATH_IMAGEN_USUARIO_DEFAULT = ResolveUrl('~/Resources/Imagenes/user-avatar.png')

    oi = data.OrdenInspeccion;
    permisos = data.Permisos;

    initEncabezado();
    initAlertas();
    initRequerimientos();
    initNotas();
    initDescripcion();
    initUltimoEstado();
    initInformacionAdicional();

    initPanelDeslizable();
    //initMapa();

    cargarDatos();
}

function cargarDatos() {
    cargarAcciones();
    cargarDatosEncabezado();
    cargarAlertas();
    cargarDescripcion();
    cargarRequerimientos();
    cargarNotas();
    cargarUltimoEstado();
    cargarInformacionAdicional();
    //if (map != undefined) {
    //    cargarMapa();
    //}
}


//Encabezado
function initEncabezado() {
    //$('#btn_Acciones').click(function () {
    //    $('#contenedor_Acciones .contenido').toggleClass('visible');
    //    $(this).text($('#contenedor_Acciones .contenido').hasClass('visible') ? 'Ocultar acciones' : 'Ver acciones');
    //});
}

function cargarDatosEncabezado() {
    //Numero
    $('#texto_Numero').text(oi.Numero + '/' + oi.Año);

    //Estado
    if (('EstadoColor' in oi) && oi.EstadoColor != undefined) {
        $('#icono_IndicadorEstado').css('color', '#' + oi.EstadoColor);
    } else {
        $('#icono_IndicadorEstado').css('color', 'black');
    }
    if (('EstadoNombre' in oi) && oi.EstadoNombre != undefined) {
        $('#texto_IndicadorEstado').html('<b>Estado </b>' + toTitleCase(oi.EstadoNombre));
    } else {
        $('#texto_IndicadorEstado').text('Sin datos');
    }
}

//Estados
function mostrarHistorialDeEstados() {
    abrirPanelDeslizable('Historial de Estados');

    var divHistorialEstados = $($('#template_HistorialEstados').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divHistorialEstados);

    $.each(oi.Estados, function (index, element) {
        var div = $($('#template_HistorialEstadoItem').html());
        if (index == 0) {
            $(div).find('.linea1').css({ opacity: 0 });
        }
        if (index == oi.Estados.length - 1) {
            $(div).find('.linea2').css({ opacity: 0 });
        }
        $(div).find('.circulo').css({ 'background-color': '#' + element.EstadoColor });
        $(div).find('.nombre').text(toTitleCase(element.EstadoNombre));
        $(div).find('.motivo').text(element.EstadoObservaciones);
        $(div).find('.nombrePersona').html('<b>' + toTitleCase(element.UsuarioNombre + ' ' + element.UsuarioApellido) + '</b>');
        $(div).find('.nombrePersona').click(function () {
            crearDialogoUsuarioDetalle({
                Id: element.UsuarioId
            });
        });


        $(div).find('.fecha').html(' el <b>' + dateTimeToString(element.EstadoFecha) + '</b>');

        $(divHistorialEstados).append(div);
    });
}

//Alertas
function initAlertas() {

}

function ocultarAlertas() {
    $('#contenedor_Alertas').empty();
}

function cargarAlertas() {
    ocultarAlertas();
}


//Panel Deslizable
function initPanelDeslizable() {
    $('#btn_CerrarPanelDeslizable').click(function () {
        cerrarPanelDeslizable();
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                cerrarPanelDeslizable();
            }
        }
    });
}

function abrirPanelDeslizable(titulo) {
    $('#contenedor_PanelDeslizable').addClass('visible');

    $('#contenedor_PanelDeslizable .encabezado .titulo').text(titulo);

    $('#contenedor_PanelDeslizable .contenedor_Contenido').scrollTop(0)
    $('#contenedor_PanelDeslizable .contenedor_Contenido').empty();

    panelAbierto(true)
}

function cerrarPanelDeslizable() {
    $('#contenedor_PanelDeslizable .encabezado .titulo').text('');
    $('#contenedor_PanelDeslizable').removeClass('visible');
    panelAbierto(false)
}


//Mapa
//let mapa_centroCordoba = { lat: parseFloat(-31.416111), lng: parseFloat(-64.191174) };

//function initMapa() {

//    ControlMapa_Init({
//        ResaltarAlHacerClick: false,
//        OnMapReady: function (mapaNuevo) {
//            map = mapaNuevo;

//            let btnExpandir = $('<a class="btn-mapa">Expandir</a>');
//            $('#ControlMapa_Botones').prepend(btnExpandir);

//            $(btnExpandir).click(function () {

//                if (mapaExpandido) {
//                    $(btnExpandir).text('Expandir');
//                    achicarMapa();
//                } else {
//                    $(btnExpandir).text('Achicar');
//                    expandirMapa();
//                }
//            });

//            let btnCentrar = $('<a class="btn-mapa">Centrar</a>');
//            $('#ControlMapa_Botones').prepend(btnCentrar);

//            $(btnCentrar).click(function () {
//                centrarMapa();
//            });

//            cargarMapa();
//        }
//    });
//}

//function mover(x, y) {
//    if (map == undefined) return;
//    var center = new google.maps.LatLng(x, y);
//    map.panTo(center);
//}

//function pinSymbol(color) {
//    return {
//        path: 'M31.5,0C14.1,0,0,14,0,31.2C0,53.1,31.5,80,31.5,80S63,52.3,63,31.2C63,14,48.9,0,31.5,0z M31.5,52.3 c-11.8,0-21.4-9.5-21.4-21.2c0-11.7,9.6-21.2,21.4-21.2s21.4,9.5,21.4,21.2C52.9,42.8,43.3,52.3,31.5,52.3z',
//        fillColor: color,
//        fillOpacity: 1,
//        anchor: new google.maps.Point(35, 70),
//        strokeColor: '#000',
//        strokeWeight: 2,
//        scale: 0.45,
//    };
//}

//function crearHtmlMarcadorPopup(requerimiento) {

//    var html = $('<div class="popup">');
//    var divTextos = $('<div class="textos">');

//    //Titulo
//    var titulo = requerimiento.Numero + "/" + requerimiento.Año;
//    $(divTextos).append('<label class="titulo link" onclick="abrirRequerimientoDetalle(' + requerimiento.Id + ')">' + titulo + '</label>');

//    //Estado
//    let estado = requerimiento.EstadoNombre;
//    $(divTextos).append('<label class="subtitulo"><b>Estado</b></label>')
//    $(divTextos).append('<label class="descripcion">' + estado + '</label>')

//    //Descripcion
//    let descripcion = requerimiento.Descripcion;
//    if (descripcion == undefined || descripcion == "") descripcion = "Sin datos";
//    $(divTextos).append('<label class="subtitulo"><b>Descripción</b></label>')
//    $(divTextos).append('<label class="descripcion">' + descripcion + '</label>')

//    //Motivo
//    let motivo = requerimiento.MotivoNombre;
//    $(divTextos).append('<label class="subtitulo"><b>Motivo</b></label>')
//    $(divTextos).append('<label class="descripcion">' + toTitleCase(motivo) + '</label>')


//    //Direccion
//    $(divTextos).append('<label class="subtitulo">Domicilio</label>')
//    if ('DomicilioDireccion' in requerimiento && requerimiento.DomicilioDireccion != undefined && requerimiento.DomicilioDireccion != "") {
//        $(divTextos).append('<label><b>Dirección: </b>' + toTitleCase(requerimiento.DomicilioDireccion) + '</b></label>');
//    } else {
//        $(divTextos).append('<label><b>Dirección: </b>Sin datos</b></label>');
//    }

//    //Observaciones
//    if ('DomicilioObservaciones' in requerimiento && requerimiento.DomicilioObservaciones != undefined && requerimiento.DomicilioObservaciones != "") {
//        $(divTextos).append('<label><b>Descripción: </b>' + toTitleCase(requerimiento.DomicilioObservaciones) + '</b></label>');
//    } else {
//        $(divTextos).append('<label><b>Descripción: </b>Sin datos</b></label>');
//    }

//    //Cpc
//    let cpc = '';
//    if ('CpcNombre' in requerimiento && 'CpcNumero' in requerimiento && requerimiento.CpcNombre != undefined && requerimiento.CpcNumero != undefined) {
//        $(divTextos).append('<label><b>CPC: </b>' + ("N° " + requerimiento.CpcNumero + ' - ' + requerimiento.CpcNombre) + '</b></label>');
//    } else {
//        $(divTextos).append('<label><b>CPC: </b>Sin datos</b></label>');
//    }

//    //Barrio
//    if ('BarrioNombre' in requerimiento && requerimiento.BarrioNombre != undefined && requerimiento.BarrioNombre != "") {
//        $(divTextos).append('<label><b>Barrio: </b>' + "N° " + requerimiento.BarrioNombre + '</b></label>');
//    } else {
//        $(divTextos).append('<label><b>Barrio: </b>Sin datos</b></label>');
//    }

//    $(divTextos).appendTo(html);

//    //Botones

//    let divBotones = $('<div class="botones"></div>');

//    //Nota
//    let btnNota = $('<div class="accion" onclick="agregarNota(' + requerimiento.Id + ')"><i class="material-icons icono">comment</i><label class="texto">Agregar nota</label></div>');
//    if (!validarPermisoOI(PERMISO_AGREGAR_NOTA)) {
//        $(btnNota).addClass('deshabilitado');
//    }
//    $(btnNota).appendTo(divBotones);

//    let btnBorrar = $('<div class="accion rojo" onclick="quitarRequerimiento(' + requerimiento.Id + ')"><i class="material-icons icono">delete</i><label class="texto">Quitar de OI</label></div>');
//    if (!validarPermisoOI(PERMISO_QUITAR_REQUERIMIENTO)) {
//        $(btnBorrar).addClass('deshabilitado');
//    }
//    $(btnBorrar).on('click', function () {
//        quitarRequerimiento(requerimiento.Id);
//    });
//    $(btnBorrar).appendTo(divBotones);

//    $(divBotones).appendTo(html);

//    return html;
//}

//function BotonCpc(controlDiv, map) {
//    $(controlDiv).addClass('contenedorBtnCpc');

//    // Set CSS for the control border.
//    var controlUI = document.createElement('div');
//    $(controlUI).addClass('btnMapa');
//    controlUI.setAttribute("id", "btnCPC");
//    controlUI.title = 'Mapa de CPCs';
//    controlDiv.appendChild(controlUI);

//    // Set CSS for the control interior.
//    var controlText = document.createElement('div');
//    controlText.innerHTML = 'Mapa de CPCs';
//    controlUI.appendChild(controlText);

//    // Setup the click event listeners: simply set the map to Chicago.
//    controlUI.addEventListener('click', function () {
//        $(controlUI).toggleClass('activo');
//        viendoPoligonosCPC = !viendoPoligonosCPC;
//        for (var i = 0; i < poligonosCPC.length; i++) {
//            if (viendoPoligonosCPC) {
//                poligonosCPC[i].setMap(map);
//            } else {
//                poligonosCPC[i].setMap(null);
//            }
//        }

//        if (viendoPoligonosCPC) {
//            if (map.getZoom() > 11) {
//                map.setZoom(11);
//            }
//        }
//    });
//}

//function BotonExpandir(controlDiv, map) {
//    $(controlDiv).addClass('contenedorBtnExpandirMapa');

//    // Set CSS for the control border.
//    var controlUI = document.createElement('div');
//    $(controlUI).addClass('btnMapa');
//    controlUI.setAttribute("id", "btnExpandirMapa");
//    controlUI.title = 'Expandir';
//    controlDiv.appendChild(controlUI);

//    // Set CSS for the control interior.
//    var controlText = document.createElement('div');
//    controlText.innerHTML = 'Expandir';
//    controlUI.appendChild(controlText);

//    // Setup the click event listeners: simply set the map to Chicago.
//    controlUI.addEventListener('click', function () {
//        $(controlUI).toggleClass('activo');
//        if (mapaExpandido) {
//            $(controlUI).find('div').text("Expandir");
//            achicarMapa();
//        } else {
//            $(controlUI).find('div').text("Comprimir");
//            expandirMapa();
//        }
//    });
//}

//function cargarMapa() {
//    //Quito los marcadores
//    $.each(mapa_marcadores, function (index, element) {
//        element.setMap(undefined);
//        element = undefined;
//    });
//    $.each(mapa_popups, function (index, element) {
//        element.setMap(undefined);
//        element = undefined;
//    });
//    $.each(mapa_PoligonosBarrios, function (index, element) {
//        element.setMap(undefined);
//        element = undefined;
//    });

//    mapa_marcadores = [];
//    mapa_popups = [];
//    mapa_PoligonosBarrios = [];

//    //Si no tiene marcadores la oi, cancelo aca
//    if (!('Requerimientos' in oi) || oi.Requerimientos.length == 0) return;

//    //Agrego marcadores y centro en ellos
//    $.each(oi.Requerimientos, function (index, rq) {
//        let keyLatitud = "DomicilioLatitud";
//        let keyLongitud = "DomicilioLongitud";

//        if (rq[keyLatitud] != undefined) {
//            let lat = rq[keyLatitud].replace(',', '.');
//            let lng = rq[keyLongitud].replace(',', '.');
//            let pos = { lat: parseFloat(lat), lng: parseFloat(lng) };
//            let marcador = new google.maps.Marker();
//            marcador.setPosition(pos);
//            marcador.setMap(map);
//            marcador.set('id', rq.Id);
//            marcador.setIcon(pinSymbol('#' + rq.EstadoColor));
//            mapa_marcadores.push(marcador);

//            //Popup
//            let htmlPopup = crearHtmlMarcadorPopup(rq);
//            if (htmlPopup != undefined) {


//                let popupDomicilio = new google.maps.InfoWindow({
//                    maxWidth: 300
//                });
//                popupDomicilio.set('id', rq.Id);

//                popupDomicilio.setContent($(htmlPopup).prop('outerHTML'));

//                marcador.addListener('click', function () {

//                    //Cierro el popuop de una zona
//                    if (mapa_PopupZona != undefined) {
//                        mapa_PopupZona.close();
//                        mapa_PopupZona = undefined;
//                    }

//                    //Desresalto las zonas
//                    $.each(mapa_PoligonosBarrios, function (i1, e1) { e1.setOptions({ fillOpacity: mapa_OpacityZona }); });

//                    //Cierro los popups de los requerimientos
//                    $.each(mapa_popups, function (index1, p) {
//                        p.close();
//                    });

//                    //Abro el popup actual
//                    popupDomicilio.open(map, marcador);
//                });

//                mapa_popups.push(popupDomicilio);
//            }
//        }
//    });

//    //Centro
//    centrarMapa();

//    //Dibujo los poligonos de los barrios de las zonas
//    top.getBarrios().then(function (barrios) {
//        $.each(oi.Barrios, function (index, element) {
//            let b = $.grep(barrios, function (e2, i2) { return e2.id == element.Id })[0];
//            if (b != undefined) {
//                let p = new google.maps.Polygon({
//                    paths: b.poligono,
//                    strokeColor: element.ZonaColor,
//                    strokeOpacity: mapa_OpacityZona * 2,
//                    strokeWeight: 2,
//                    fillColor: element.ZonaColor,
//                    fillOpacity: mapa_OpacityZona,
//                    clickable: true,
//                    zIndex: mapa_ZIndexZona,
//                    map: map
//                });
//                p.set('IdZona', element.ZonaId);
//                mapa_PoligonosBarrios.push(p);

//                google.maps.event.addListener(p, 'click', function (event) {
//                    $.each(mapa_PoligonosBarrios, function (i, poligonoBarrio) {
//                        poligonoBarrio.setOptions({ fillOpacity: mapa_OpacityZona });
//                    });

//                    p.setOptions({ fillOpacity: mapa_OpacityZona * 2 });

//                    if (mapa_PopupZona != undefined) {
//                        mapa_PopupZona.close();
//                        mapa_PopupZona = undefined;
//                    }

//                    mapa_PopupZona = new google.maps.InfoWindow({
//                        maxWidth: 300
//                    });
//                    google.maps.event.addListener(mapa_PopupZona, 'closeclick', function () {
//                        p.setOptions({ fillOpacity: mapa_OpacityZona });
//                    });
//                    mapa_PopupZona.setContent("Zona " + element.ZonaNombre + " | Barrio: " + b.nombre);

//                    var bounds = new google.maps.LatLngBounds();
//                    for (let i = 0; i < p.getPath().length; i++) {
//                        let punto = p.getPath().getAt(i);
//                        bounds.extend({ lat: punto.lat(), lng: punto.lng() });
//                    }
//                    map.fitBounds(bounds);
//                    mapa_PopupZona.setPosition({ lat: bounds.getCenter().lat(), lng: bounds.getCenter().lng() });
//                    mapa_PopupZona.open(map);
//                });

//            }

//        });
//    });
//}


//Descripcion
function initDescripcion() {

}

function cargarDescripcion() {
    let descripcion = oi.Descripcion;
    if (descripcion == undefined || descripcion.trim() == "") {
        $('#texto_Descripcion').text("Sin datos");
    } else {
        $('#texto_Descripcion').text(descripcion);
    }
}

//Requerimientos
function initRequerimientos() {

}

function cargarRequerimientos() {
    $('#contenedor_Requerimientos > .contenido > .items').empty();

    if (!('Requerimientos' in oi) && oi.Requerimientos.length == 0) return;

    var rqPorMotivo = [];

    let rq = oi.Requerimientos;
    let rq_AgrupadosPorMotivo = _.groupBy(rq, function (element) { return element.MotivoId; });

    $.each(rq_AgrupadosPorMotivo, function (index, rqs) {
        let html_grupo = $($('#template_GrupoRequerimientos').html());
        $(html_grupo).find('.motivo').html('<b>Motivo: </b>' + toTitleCase(rqs[0].MotivoNombre));

        $.each(rqs, function (index1, rq) {
            let html_rq = crearHtmlRequerimiento(rq);
            $(html_grupo).find('.items').append(html_rq);
        });

        $('#contenedor_Requerimientos > .contenido > .items').append(html_grupo);
    });
}

function crearHtmlRequerimiento(rq) {
    //Ubicacion
    let ubicacion = '';
    if (rq.DomicilioDireccion) {
        ubicacion += rq.DomicilioDireccion;
    }
    if (rq.DomicilioObservaciones != undefined) {
        if (ubicacion != "") ubicacion += " | ";
        ubicacion += rq.DomicilioObservaciones;
    }
    if (ubicacion == '') ubicacion = "Sin datos";

    //Descripcion
    let descripcion = rq.Descripcion != undefined ? rq.Descripcion : 'Sin datos';

    var html = $($('#template_Requerimiento').html());
    $(html).attr('id-rq', rq.Id);
    $(html).find('> .textos > .numero').text(rq.Numero + '/' + rq.Año);

    $(html).find('> .textos > .ubicacion').html('<b>Ubicación </b>' + ubicacion);
    $(html).find('> .textos > .descripcion').html('<b>Descripción </b>' + descripcion);

    //Numero click
    $(html).find('> .textos > .numero').click(function () {
        crearDialogoRequerimientoDetalle({
            Id: rq.Id,
            Callback: function () {
                actualizarDetalle().then(function () {
                    cargarDatos();
                });
            }
        });
    });

    //Boton ubicacion
    $(html).find('.botones .ubicacion').click(function () {
        if (rq.Id == undefined) return;

        let marcador = undefined;
        $.each(mapa_marcadores, function (index, element) {
            if (element.get('id') == rq.Id) {
                marcador = element;
            }
        });
        if (marcador == undefined) return;

        let popup = undefined;
        $.each(mapa_popups, function (index, element) {
            if (element.get('id') == rq.Id) {
                popup = element;
            }
        });
        if (popup == undefined) return;

        $.each(mapa_popups, function (index, element) {
            element.close();
        });
        if (popup != undefined) {
            popup.open(map, marcador);
            map.setZoom(MAPA_ZOOM_MARCADOR);
            map.setCenter(marcador.position);
        }

    });

    //Boton nota
    let cantidad = _.where(oi.Notas, { RequerimientoId: rq.Id }).length;
    $(html).find('.botones > .nota > .badge').text(cantidad);
    $(html).find('.botones .nota').click(function () {
        if (rq.Id == undefined) return;
        mostrarTodosLosNotasDeRequerimiento(rq.Id);
    });

    //Boton borrar
    $(html).find('.botones .borrar').click(function () {
        if (rq.Id == undefined) return;
        quitarRequerimiento(rq.Id);
    });

    return html;
}


//Notas
function initNotas() {
    $('#contenedor_Comentarios .verMas').click(function () {
        mostrarTodosLosNotas();
    });

    $('#btn_AgregarComentario').click(function () {
        agregarNota();
    });
}

function mostrarTodosLosNotas() {
    abrirPanelDeslizable('Notas internas');

    var div = $($('#template_ComentariosDetalle').html());
    $(div).attr('id', 'contenedor_ComentariosDetalle');
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(div);
    $('#contenedor_ComentariosDetalle .contenido').empty();

    //Cargo los notas
    let notas = _.filter(oi.Notas, function (e) { return e.RequerimientoId == undefined || e.RequerimientoId == null; });
    if (notas.length != 0) {
        $.each(notas, function (index, element) {
            var div = crearHtmlNota(element);
            $('#contenedor_ComentariosDetalle').append(div);
        });
    }

    //BotonAgregar
    $('#contenedor_PanelDeslizable .btnNuevoComentario').click(function () {
        let nota = $('#contenedor_PanelDeslizable input').val();
        if (nota == undefined || nota == "") {
            mostrarMensaje('Alerta', 'Ingrese el contenido de la nota interna');
            $('#contenedor_PanelDeslizable input').focus();
            return;
        }

        procesarAgregarNota(undefined, nota);
    });
}

let viendoNotasDeRequerimiento;

function mostrarTodosLosNotasDeRequerimiento(id) {

    let rq = _.find(oi.Requerimientos, function (rq) { return rq.Id == id; });
    if (rq == undefined) return;

    viendoNotasDeRequerimiento = id;

    abrirPanelDeslizable('Notas internas de requerimiento ' + rq.Numero + '/' + rq.Año);

    var div = $($('#template_ComentariosDetalle').html());
    $(div).attr('id', 'contenedor_ComentariosDetalle');
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(div);
    $('#contenedor_ComentariosDetalle .contenido').empty();

    //Cargo los notas
    let notas = _.filter(oi.Notas, function (e) { return e.RequerimientoId == id; });
    if (notas.length != 0) {
        $.each(notas, function (index, element) {
            var div = crearHtmlNota(element);
            $('#contenedor_ComentariosDetalle').append(div);
        });
    }

    //BotonAgregar
    $('#contenedor_PanelDeslizable .btnNuevoComentario').click(function () {
        let nota = $('#contenedor_PanelDeslizable input').val();
        if (nota == undefined || nota == "") {
            mostrarMensaje('Alerta', 'Ingrese el contenido de la nota interna');
            $('#contenedor_PanelDeslizable input').focus();
            return;
        }

        procesarAgregarNota(id, nota);
    });
}

function crearHtmlNota(data) {
    var div = $('#template_Comentario').html();
    div = $(div);
    if ('RequerimientoId' in data && data.RequerimientoId != undefined) {
        $(div).find('.card .para-requerimiento').html('Para requerimiento <b><label class="link">' + data.RequerimientoNumero + '/' + data.RequerimientoAño + '</label></b>');
        $(div).find('.card .para-requerimiento label').click(function () {
            crearDialogoRequerimientoDetalle({
                Id: data.RequerimientoId,
                Callback: function () {
                    actualizarDetalle().then(function () {
                        cargarDatos();
                    });
                }
            })
        });
    }

    $(div).find('.persona img').attr('src', PATH_IMAGEN_USUARIO_DEFAULT);
    $(div).find('.persona label').text(data.UsuarioNombre + ' ' + data.UsuarioApellido);
    $(div).find('.card .contenido').text(data.Observaciones);
    $(div).find('.card .fecha').text(dateTimeToString(data.Fecha));

    $(div).find('.persona label').click(function () {
        crearDialogoUsuarioDetalle({
            Id: data.UsuarioId
        });
    });

    return div;
}

function cargarNotas() {
    $('#contenedor_Comentarios .contenido .items').empty();

    let notas = _.filter(oi.Notas, function (e) { return e.RequerimientoId == undefined || e.RequerimientoId == null; });
    if (notas.length != 0) {
        $('#contenedor_Comentarios .sinItems').hide();
        $('#contenedor_Comentarios .contenido').show();

        $.each(notas, function (index, element) {
            if (index < 3) {
                var div = crearHtmlNota(element);
                $('#contenedor_Comentarios .contenido .items').append(div);
            }
        });

        if ($('#contenedor_Comentarios .items').height() > $('#contenedor_Comentarios .contenido')) {
            $('#contenedor_Comentarios .verMas').hide();
        } else {
            $('#contenedor_Comentarios .verMas').show();
        }

    } else {
        $('#contenedor_Comentarios .sinItems').show();
        $('#contenedor_Comentarios .verMas').hide();
        $('#contenedor_Comentarios .contenido').hide();
    }
}


//Ultimo Estado
function initUltimoEstado() {
    $('#contenedor_UltimoEstado .estado .nombrePersona').click(function () {
        if (!('EstadoUsuarioId' in oi) || oi.EstadoUsuarioId == undefined) return;
        crearDialogoUsuarioDetalle({
            Id: oi.EstadoUsuarioId
        });
    });

    $('#btn_VerHistorialEstado').click(function () {
        mostrarHistorialDeEstados();
    });
}

function cargarUltimoEstado() {
    if ('EstadoNombre' in oi && oi.EstadoNombre != undefined) {
        $('#contenedor_UltimoEstado .estado .nombre').text(toTitleCase(oi.EstadoNombre));
    } else {
        $('#contenedor_UltimoEstado .estado .nombre').text('Sin datos');
    }

    if ('EstadoColor' in oi && oi.EstadoColor != undefined) {
        $('#contenedor_UltimoEstado .estado .circulo').css('background-color', '#' + oi.EstadoColor);
    } else {
        $('#contenedor_UltimoEstado .estado .circulo').css('background-color', 'black');
    }

    if ('EstadoObservaciones' in oi && oi.EstadoObservaciones != undefined) {
        $('#contenedor_UltimoEstado .estado .motivo').text(oi.EstadoObservaciones);
    } else {
        $('#contenedor_UltimoEstado .estado .motivo').text('Sin datos');
    }

    if ('EstadoUsuarioNombre' in oi && oi.EstadoUsuarioNombre != undefined && 'EstadoUsuarioApellido' in oi && oi.EstadoUsuarioApellido != undefined) {
        $('#contenedor_UltimoEstado .estado .nombrePersona').text(toTitleCase(oi.EstadoUsuarioNombre + ' ' + oi.EstadoUsuarioApellido));
    } else {
        $('#contenedor_UltimoEstado .estado .nombrePersona').text('Sin datos');
    }

    if ('EstadoFecha' in oi && oi.EstadoFecha != undefined) {
        $('#contenedor_UltimoEstado .estado .fecha').html(' el <b>' + dateTimeToString(oi.EstadoFecha) + '</b>');
    } else {
        $('#contenedor_UltimoEstado .estado .fecha').html(' el *sin datos*');
    }
}


//Informacion adicional
function initInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoUsuarioCreador').click(function () {
        crearDialogoUsuarioDetalle({
            Id: oi.UsuarioCreadorId
        });
    });

    $('#contenedor_InfoAdicional .textoUsuarioModificacion').click(function () {
        crearDialogoUsuarioDetalle({
            Id: oi.UsuarioModificacionId
        });
    });
}

function cargarInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoFechaCreacion').html('<b>' + dateTimeToString(oi.FechaAlta) + '</b>');

    if ('UsuarioCreadorNombre' in oi && oi.UsuarioCreadorNombre != undefined && 'UsuarioCreadorApellido' in oi && oi.UsuarioCreadorApellido != undefined) {
        $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').show();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').show();
        var usuarioCreador = toTitleCase(oi.UsuarioCreadorNombre + ' ' + oi.UsuarioCreadorApellido).trim();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').html('<b>' + usuarioCreador + '</b>');
    } else {
        $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').hide();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').hide();
    }

    if ('FechaModificacion' in oi && oi.FechaModificacion != undefined) {
        $('#contenedor_InfoAdicional .linea2').show();
        $('#contenedor_InfoAdicional .textoFechaModificacion').html('<b>' + dateTimeToString(oi.FechaModificacion) + '</b>');
        if ('UsuarioModificacionNombre' in oi && oi.UsuarioModificacionNombre != undefined && 'UsuarioModificacionApellido' in oi && oi.UsuarioModificacionApellido != undefined) {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').html('<b>' + toTitleCase(oi.UsuarioModificacionNombre + ' ' + oi.UsuarioModificacionApellido) + '</b>');
        } else {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').hide();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').hide();
        }

    } else {
        $('#contenedor_InfoAdicional .linea2').hide();
    }
}


//Acciones
function cargarAcciones() {
    $('#contenedor_Acciones .contenido').empty();

    //Editar descripcion
    agregarAccion({
        Texto: 'Editar descripción',
        Permiso: validarPermisoOI(PERMISO_EDITAR_DESCRIPCION),
        Icono: 'comment',
        OnClick: function () {
            editarDescripcion();
        }
    });

    //Agregar RQ
    agregarAccion({
        Texto: 'Agregar requerimiento',
        Permiso: validarPermisoOI(PERMISO_AGREGAR_REQUERIMIENTO),
        Icono: 'add',
        OnClick: function () {
            agregarRequerimiento();
        }
    });

    //Agregar nota
    agregarAccion({
        Texto: 'Agregar nota',
        Permiso: validarPermisoOI(PERMISO_AGREGAR_NOTA),
        Icono: 'comment',
        OnClick: function () {
            agregarNota();
        }
    });

    //Cancelar
    agregarAccion({
        Texto: 'Cancelar OI',
        Permiso: validarPermisoOI(PERMISO_CANCELAR),
        Icono: 'clear',
        OnClick: function () {
            cancelar();
        }
    });

    //Completar
    agregarAccion({
        Texto: 'Completar OI',
        Permiso: validarPermisoOI(PERMISO_CERRAR),
        Icono: 'build',
        OnClick: function () {
            completar();
        }
    });

    //Historial de estados
    agregarAccion({
        Texto: 'Historial de estados',
        Icono: 'history',
        OnClick: function () {
            mostrarHistorialDeEstados();
        }
    });
}

function agregarAccion(valores) {
    var div = $($('#template_Accion').html());
    $(div).find('.texto').text(valores.Texto);
    $(div).find('.icono').text(valores.Icono);
    $(div).attr('permiso', valores.PermisoKeyValue);
    $('#contenedor_Acciones .contenido').append(div);
    if (('Permiso' in valores) && !valores.Permiso) {
        $(div).addClass('deshabilitado');
    }
    $(div).click(function () {
        valores.OnClick();
    });
}

//function expandirMapa() {
//    mapaExpandido = true;
//    $('#main').addClass('mapaExpandido');
//}

//function achicarMapa() {
//    mapaExpandido = false;
//    $('#main').removeClass('mapaExpandido');
//}

//function centrarMapa() {
//    //Agrego marcadores y centro en ellos
//    var bounds = new google.maps.LatLngBounds();
//    $.each(oi.Requerimientos, function (index, rq) {
//        let keyLatitud = "DomicilioLatitud";
//        let keyLongitud = "DomicilioLongitud";

//        if (rq[keyLatitud] != undefined) {
//            let lat = rq[keyLatitud].replace(',', '.');
//            let lng = rq[keyLongitud].replace(',', '.');
//            let pos = { lat: parseFloat(lat), lng: parseFloat(lng) };
//            bounds.extend(pos);
//        }
//    });
//    map.fitBounds(bounds);
//}

function editarDescripcion() {
    if (!validarPermisoOI(PERMISO_EDITAR_DESCRIPCION)) {
        mostrarMensaje('Error', 'La orden de inspección no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    let placeholder = oi.Descripcion;
    crearDialogoInput({
        Titulo: 'Editar descripcion',
        Placeholder: 'Descripción',
        Valor: placeholder,
        OnLoad: function (jAlert) {
            let input = $(jAlert).find('input');
            $(input).focus();
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let input = $(jAlert).find('input');
                    let descripcion = input.val();
                    if (descripcion == "") {
                        $(jAlert).find('input').focus();
                        mostrarMensaje('Alerta', 'Ingrese la nueva descripción');
                        return;
                    }
                    descripcion = descripcion.trim();

                    var url = ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/EditarDescripcion');

                    crearAjax({
                        Url: url,
                        Data: { comando: { IdOrdenInspeccion: oi.Id, Descripcion: descripcion } },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            mostrarMensaje('Exito', 'Descripcion editada correctamente');

                            actualizarDetalle().then(function () {
                                cargarDescripcion();
                                $(jAlert).CerrarDialogo();
                            });
                        },
                        OnError: function (result) {
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            },

        ]
    });
}

function agregarRequerimiento() {
    if (!validarPermisoOI(PERMISO_AGREGAR_REQUERIMIENTO)) {
        mostrarMensaje('Error', 'La orden de inspección no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    crearDialogoOrdenInspeccionAgregarRequerimientos({
        Id: oi.Id,
        Callback: function () {
            actualizarDetalle().then(function () {
                cargarDatos();
            });
        }
    });
}

function agregarNota(idRequerimiento) {
    if (!validarPermisoOI(PERMISO_AGREGAR_NOTA)) {
        mostrarMensaje('Error', 'La orden de inspección no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    let placeholder = idRequerimiento != undefined ? 'Nueva nota interna para requerimiento...' : 'Nueva nota interna...';
    crearDialogoInput({
        Titulo: 'Nueva nota interna',
        Placeholder: placeholder,
        OnLoad: function (jAlert) {
            let input = $(jAlert).find('input');
            $(input).focus();
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let input = $(jAlert).find('input');
                    let nota = input.val();
                    if (nota == "") {
                        $(jAlert).find('input').focus();
                        mostrarMensaje('Alerta', 'Ingrese el contenido de la nota interna');
                        return;
                    }
                    $(jAlert).CerrarDialogo();
                    procesarAgregarNota(idRequerimiento, nota);
                }
            },

        ]
    });
}

function procesarAgregarNota(idRequerimiento, nota) {
    if (!validarPermisoOI(PERMISO_AGREGAR_NOTA)) {
        mostrarMensaje('Error', 'La orden de inspección no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    var url = ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/AgregarNota');
    var data = {
        comando: {
            IdOrdenInspeccion: oi.Id,
            IdRequerimiento: idRequerimiento,
            Observaciones: nota
        }
    };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', 'Nota interna agregada');

            actualizarDetalle().then(function () {
                cargarNotas();

                if ($('#contenedor_PanelDeslizable').hasClass('visible') && $('#contenedor_PanelDeslizable').length != 0) {
                    if (idRequerimiento == undefined) {
                        mostrarTodosLosNotas();
                    } else {
                        cargarRequerimientos();
                        mostrarTodosLosNotasDeRequerimiento(idRequerimiento);
                    }
                }
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function quitarRequerimiento(id) {
    console.log('borrar');
    if (!validarPermisoOI(PERMISO_QUITAR_REQUERIMIENTO)) {
        mostrarMensaje('Error', 'La orden de inspeccipon no se encuentra en un estado válido para realizar esta acción');
        return;
    }


    crearDialogoOrdenInspeccionQuitarRequerimiento({
        IdOi: oi.Id,
        IdRq: id,
        Callback: function () {
            actualizarDetalle().then(function () {
                cargarDatos();
            });
        }
    });

}

function cancelar() {
    if (!validarPermisoOI(PERMISO_CANCELAR)) {
        mostrarMensaje('Error', 'La orden de inspección no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    crearDialogoInput({
        Titulo: 'Cancelar Orden de Inspección',
        Placeholder: 'Ingrese un motivo...',
        Botones: [
            {
                Texto: 'Volver'
            },
            {
                Texto: 'Cancelar',
                CerrarDialogo: false,
                Class: 'colorError',
                OnClick: function (jAlert) {
                    let motivo = $(jAlert).find('input').val();
                    if (motivo == "") {
                        mostrarMensaje('Alerta', 'Ingrese el motivo de la cancelación de la orden');
                        $(jAlert).find('input').focus();
                        return;
                    }

                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/Cancelar'),
                        Data: { comando: { Id: oi.Id, Motivo: motivo } },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                $(jAlert).MostrarDialogoCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            $(jAlert).CerrarDialogo();

                            actualizarDetalle().then(function () {
                                cargarDatos();
                                $(jAlert).CerrarDialogo();
                            });
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false);
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    })
                }
            }
        ]
    });
}

function completar() {
    if (!validarPermisoOI(PERMISO_CERRAR)) {
        mostrarMensaje('Error', 'La orden de inspección no se encuentra en un estado válido para realizar esta acción');
        return;
    }

    crearDialogoOrdenInspeccionCompletar({
        Id: oi.Id,
        Callback: function () {
            actualizarDetalle().then(function () {
                cargarDatos();
            });
        }
    });
}

function abrirRequerimientoDetalle(id) {
    crearDialogoRequerimientoDetalle({
        Id: id,
        Callback: function () {
            actualizarDetalle().then(function () {
                cargarDatos();
            });
        }
    });
}

function imprimirResumenSinMapa() {
    crearDialogoReporteOrdenInspeccionCaratulaSinMapa({
        Id: oi.Id
    });
}

function imprimirOrdenDatallada() {
    crearDialogoReporteOrdenInspeccionDatallada({
        Id: oi.Id
    });
}

//Utiles
function mostrarError(error) {
    mostrarMensajeCritico({ Icono: 'error_outline', Titulo: error })
}

function actualizarDetalle() {
    return new Promise(function (callback, callbackError) {
        mostrarCargando(true);
        crearAjax({
            Url: ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/GetDetalleById'),
            Data: { id: oi.Id },
            OnSuccess: function (result) {
                mostrarCargando(false);
                if (!result.Ok) {
                    mostrarError(result.Error);
                    return;
                }

                oi = result.Return;
                cargarInformacionAdicional();
                cargarAcciones();

                callback();
            },
            OnError: function (result) {
                mostrarCargando(false);
                mostrarError('Error procesando la solicitud');
            }
        });
    });
}

function validarPermisoOI(keyValuePermiso) {
    if (permisos == undefined) return false;
    if (permisos.length == 0) return false;

    var permiso = $.grep(permisos, function (element, index) {
        return element.EstadoOrdenInspeccion == oi.EstadoKeyValue && element.Permiso == keyValuePermiso && element.TienePermiso;
    })[0];

    if (permiso == undefined) {
        return false;
    }

    return true;
}

//Listener panel
function setOnPanelAbiertoListener(callback) {
    panelCallback = callback;
}

function panelAbierto(abierto) {
    if (panelCallback != undefined) {
        panelCallback(abierto);
    }
}