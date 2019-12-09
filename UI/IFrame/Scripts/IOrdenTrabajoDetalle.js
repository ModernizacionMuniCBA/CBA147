let ot;
let permisos;
let secciones = [];
let cantidadMoviles = 0;
let cantidadFlotas = 0;
let cantidadEmpleados = 0;
let cantidadTareas = 0;

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

let PERMISO_EDITAR_DESCRIPCION = 1;
let PERMISO_AGREGAR_REQUERIMIENTO = 2;
let PERMISO_QUITAR_REQUERIMIENTO = 3;
let PERMISO_EDITAR_RECURSOS = 4;
let PERMISO_AGREGAR_NOTA = 5;
let PERMISO_EDITAR_MOVILES = 6;
let PERMISO_CERRAR = 7;
let PERMISO_CANCELAR = 8;
let PERMISO_CAMBIAR_SECCION = 9;
let PERMISO_EDITAR_EMPLEADOS = 10;
let PERMISO_EDITAR_FLOTAS = 11;

let PATH_IMAGEN_USER_MALE;
let PATH_IMAGEN_USER_FEMALE;


function init(data) {
    if ('Error' in data) {
        mostrarError(data.Error);
        return;
    }

    PATH_IMAGEN_USER_MALE = top.urlCordobaFiles + '/Archivo/' + top.identificadorFotoUserMale + '/3';
    PATH_IMAGEN_USER_FEMALE = top.urlCordobaFiles + '/Archivo/' + top.identificadorFotoUserFemale + '/3';

    ot = data.OrdenTrabajo;
    permisos = data.Permisos;

    if (esAmbitoMunicipalidad()) {
        secciones = data.Secciones;
        cantidadTareas = data.CantidadTareas;
        cantidadMoviles = data.cantidadMoviles;
        cantidadEmpleados = data.cantidadEmpleados;
        cantidadFlotas = data.cantidadFlotas;
        $("#contenedor-infoParaAO").show();
    }

    initEncabezado();
    initAlertas();
    initRecursos();
    initRequerimientos();
    initComentarios();
    initDescripcion();
    initUltimoEstado();
    initRecursosAdicionales();
    initInformacionAdicional();
    initEmpleados();
    initPanelDeslizable();
    initMapa();

    cargarDatos();
}

function cargarDatos() {
    cargarAcciones();
    cargarDatosEncabezado();
    cargarAlertas();
    cargarDescripcion();
    cargarRecursos();
    cargarRequerimientos();
    cargarEmpleados();
    cargarMoviles();
    cargarComentarios();
    cargarUltimoEstado();
    cargarRecursosAdicionales();
    cargarInformacionAdicional();
    cargarFlotas();
    if (map != undefined) {
        cargarMapa();
    }
}

//Encabezado
function initEncabezado() {

}

function cargarDatosEncabezado() {
    //Numero
    $('#texto_Numero').text(ot.Numero + '/' + ot.Año);

    //Area
    if (ot.AreaNombre == undefined || ot.AreaNombre == "") {
        $('#texto_Area').html('<b>Área </b>Sin datos');
    } else {
        $('#texto_Area').html('<b>Área </b>' + toTitleCase(ot.AreaNombre));
    }

    $('#texto_Ambito').html('<b>Ámbito </b> ' + ot.AmbitoNombre);

    //Estado
    if (('EstadoColor' in ot) && ot.EstadoColor != undefined) {
        $('#icono_IndicadorEstado').css('color', '#' + ot.EstadoColor);
    } else {
        $('#icono_IndicadorEstado').css('color', 'black');
    }
    if (('EstadoNombre' in ot) && ot.EstadoNombre != undefined) {
        $('#texto_IndicadorEstado').html('<b>Estado </b>' + toTitleCase(ot.EstadoNombre));
    } else {
        $('#texto_IndicadorEstado').text('Sin datos');
    }

    //Zonas
    $('#texto_Zonas').empty();
    if ('Barrios' in ot && ot.Barrios != undefined && ot.Barrios.length != 0) {
        let zonas = [];
        $.each(ot.Barrios, function (index, element) {
            if ($.grep(zonas, function (e1, i1) { return e1.Id == element.ZonaId }).length != 0) return;
            zonas.push({
                Id: element.ZonaId,
                Nombre: element.ZonaNombre,
                Color: element.ZonaColor
            });
        });

        $('#texto_Zonas').append('<label><b>Zonas </b> </label>');
        let primero = true;
        $.each(zonas, function (index, element) {
            if (!primero) {
                $('#texto_Zonas').append('<label>,</label>');
            }
            primero = false;

            let label = $('<label class="link" style="border-color: ' + element.Color + ';">' + element.Nombre + '</label>');
            $('#texto_Zonas').append(label);

            $(label).click(function () {

                $.each(mapa_popups, function (index, e) {
                    e.close();
                });

                var bounds = new google.maps.LatLngBounds();
                $.each(mapa_PoligonosBarrios, function (i1, poligonoBarrio) {

                    poligonoBarrio.setOptions({ fillOpacity: mapa_OpacityZona });

                    if (mapa_PopupZona != undefined) {
                        mapa_PopupZona.close();
                        mapa_PopupZona = undefined;
                    }

                    for (let i = 0; i < poligonoBarrio.getPath().length; i++) {
                        let punto = poligonoBarrio.getPath().getAt(i);
                        bounds.extend({ lat: punto.lat(), lng: punto.lng() });
                    }
                });
                map.fitBounds(bounds);
            });

        });
    } else {
        $('#texto_Zonas').html('<b>Zonas </b>Sin zonas registradas');
    }

    //Seccion
    $('#texto_Seccion').empty();
    if (secciones.length == 0) return;
    if (ot.SeccionNombre == undefined || ot.SeccionNombre == "") {
        $('#texto_Seccion').html('<b>Sin sección asignada </b>');
    } else {
        $('#texto_Seccion').html('<b>Sección </b>' + toTitleCase(ot.SeccionNombre));
    }
}

//Recursos
function initRecursos() {
    $('#btn_Moviles').click(function () {
        mostrarMoviles();
    });

    $('#btn_Flotas').click(function () {
        mostrarFlotas();
    });

    $('#btn_Empleados').click(function () {
        mostrarEmpleados();
    });
}

function mostrarMoviles() {
    abrirPanelDeslizable('Móviles');

    var divContenido = $($('#template_Moviles').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divContenido);

    $(divContenido).find('.contenedor_Cargando').addClass('visible');
    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/GetResultadoTablaByIdOrdenTrabajo'),
        Data: { idOrden: ot.Id },
        OnSuccess: function (result) {
            $(divContenido).find('.contenedor_Cargando').removeClass('visible');
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                cerrarPanelDeslizable();
                return;
            }

            $(divContenido).find('table').prop('id', 'tablaMoviles');
            $(divContenido).find('input.busqueda').prop('id', 'input_BusquedaMoviles');

            let dt = $('#tablaMoviles').DataTableMovil({
                Buscar: true,
                InputBusqueda: '#input_BusquedaMoviles',
                BotonBorrar: true,
                CallbackBorrar: function (data) {
                    quitarMovil(data.Id);
                }
            });

            //Inicializo los tooltips
            dt.$('.tooltipped').tooltip({ delay: 50 });
            dt.$('.selectMaterialize').material_select();

            //Muevo el indicador y el paginado a mi propio div
            $(divContenido).find('.tabla-footer').empty();
            $(divContenido).find('.dataTables_info').detach().appendTo($(divContenido).find('.tabla-footer'));
            $(divContenido).find('.dataTables_paginate').detach().appendTo($(divContenido).find('.tabla-footer'));
            $(divContenido).find('.dataTables_info').hide();

            //Agrego las filas
            let hDisponible = $(divContenido).find('.tabla-contenedor').height();
            let rows = calcularCantidadRowsDataTable(hDisponible);
            dt.page.len(rows);
            dt.rows.add(result.Return.Data).draw(true);
        },
        OnError: function (result) {
            $(divContenido).find('.contenedor_Cargando').removeClass('visible');
            mostrarMensaje('Error', 'Error procesando la solicitud');
            cerrarPanelDeslizable();
        }
    });

    //Agregar
    $(divContenido).find('.btn_Nuevo').css('display', validarPermisoOT(PERMISO_EDITAR_MOVILES) ? 'auto' : 'none');
    $(divContenido).find('.btn_Nuevo').click(function () {
        agregarMoviles();
    });

    //Reintentar
    $(divContenido).find('.btn_Reintentar').click(function () {
        mostrarMoviles();
    });
}

function cargarRecursos() {
    cargarRecursosMoviles();
    cargarRecursosEmpleados();
    cargarRecursosFlotas();
}

function cargarRecursosMoviles() {
    if (ot.Moviles.length == 0 && cantidadMoviles == 0) {
        $("#btn_Moviles").hide();
    } else {
        //Moviles
        $('#texto_CantidadMoviles').text(ot.Moviles.length);
        $('#contenedor_IndicadoresRecursos').show();
        $('#btn_Moviles').show();
    }
}

function actualizarRecursosMoviles() {
    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/GetCantidadParaAgregarAOT'),
        Data: { id: ot.AreaId },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarCargando(false);
            cantidadMoviles = result.Return;
            cargarRecursosMoviles();
        },
        OnError: function (result) {
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function cargarRecursosEmpleados() {
    if (ot.Empleados.length == 0 && cantidadEmpleados == 0) {
        $("#btn_Empleados").hide();
        $("#contenedor_Empleados").hide();
    } else {
        //Empleados
        $("#contenedor_Empleados").show();
        $('#texto_CantidadEmpleados').text(ot.Empleados.length);
        $('#contenedor_IndicadoresRecursos').show();
        $('#btn_Empleados').show();
    }
}

function actualizarRecursosEmpleados() {
    crearAjax({
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/GetCantidadParaAgregarAOT'),
        Data: { id: ot.AreaId },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarCargando(false);
            cantidadEmpleados = result.Return;
            cargarRecursosEmpleados();
        },
        OnError: function (result) {
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function cargarRecursosFlotas() {
    if (ot.Flotas.length == 0 && cantidadFlotas == 0) {
        $("#btn_Flotas").hide();
        $("#contenedor_Flotas").hide();
    } else {
        //Empleados
        $("#contenedor_Flotas").show();
        $('#texto_CantidadFlotas').text(ot.Flotas.length);
        $('#contenedor_IndicadoresRecursos').show();
        $('#btn_Flotas').show();
    }
}

function actualizarRecursosFlotas() {
    crearAjax({
        Url: ResolveUrl('~/Servicios/FlotaService.asmx/GetCantidadParaAgregarAOT'),
        Data: { id: ot.AreaId },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarCargando(false);
            cantidadFlotas = result.Return;
            cargarRecursosFlotas();
        },
        OnError: function (result) {
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

//Estados
function mostrarHistorialDeEstados() {
    abrirPanelDeslizable('Historial de Estados');

    var divHistorialEstados = $($('#template_HistorialEstados').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divHistorialEstados);

    $.each(ot.Estados, function (index, element) {
        var div = $($('#template_HistorialEstadoItem').html());
        if (index == 0) {
            $(div).find('.linea1').css({ opacity: 0 });
        }
        if (index == ot.Estados.length - 1) {
            $(div).find('.linea2').css({ opacity: 0 });
        }
        $(div).find('.circulo').css({ 'background-color': '#' + element.EstadoColor });
        $(div).find('.nombre').text(toTitleCase(element.EstadoNombre));
        $(div).find('.motivo').text(element.EstadoObservaciones);
        $(div).find('.nombrePersona').html('<b>' + toTitleCase(element.UsuarioNombre + ' ' + element.UsuarioApellido) + '</b>');
        $(div).find('.nombrePersona').click(function () {
            crearDialogoUsuarioDetalle({
                Id: element.UsuarioId,
                CallbackMensajes: function (tipo, mensaje) {
                    mostrarMensaje(tipo, mensaje);
                }
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

let callbackCerrarPanel;
function abrirPanelDeslizable(titulo, callback) {
    $('#contenedor_PanelDeslizable').addClass('visible');

    $('#contenedor_PanelDeslizable .encabezado .titulo').text(titulo);

    $('#contenedor_PanelDeslizable .contenedor_Contenido').scrollTop(0)
    $('#contenedor_PanelDeslizable .contenedor_Contenido').empty();

    callbackCerrarPanel = callback;
    panelAbierto(true)
}

function cerrarPanelDeslizable() {
    $('#contenedor_PanelDeslizable .encabezado .titulo').text('');
    $('#contenedor_PanelDeslizable').removeClass('visible');
    panelAbierto(false)

    if (callbackCerrarPanel != undefined) {
        callbackCerrarPanel();
        callbackCerrarPanel = undefined;
    }

    idsTareasSeleccionadas = [];
}


//Mapa
let mapa_centroCordoba = { lat: parseFloat(-31.416111), lng: parseFloat(-64.191174) };

function initMapa() {

    ControlMapa_Init({
        ResaltarAlHacerClick: false,
        OnMapReady: function (mapaNuevo) {
            map = mapaNuevo;

            let btnExpandir = $('<a class="btn-mapa">Expandir</a>');
            $('#ControlMapa_Botones').prepend(btnExpandir);

            $(btnExpandir).click(function () {

                if (mapaExpandido) {
                    $(btnExpandir).text('Expandir');
                    achicarMapa();
                } else {
                    $(btnExpandir).text('Achicar');
                    expandirMapa();
                }
            });

            let btnCentrar = $('<a class="btn-mapa">Centrar</a>');
            $('#ControlMapa_Botones').prepend(btnCentrar);

            $(btnCentrar).click(function () {
                centrarMapa();
            });

            cargarMapa();
        }
    });
}

function mover(x, y) {
    if (map == undefined) return;
    var center = new google.maps.LatLng(x, y);
    map.panTo(center);
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

function crearHtmlMarcadorPopup(requerimiento) {

    var html = $('<div class="popup">');
    var divTextos = $('<div class="textos">');

    //Titulo
    var titulo = requerimiento.Numero + "/" + requerimiento.Año;
    $(divTextos).append('<label class="titulo link" onclick="abrirRequerimientoDetalle(' + requerimiento.Id + ')">' + titulo + '</label>');

    //Estado
    let estado = requerimiento.EstadoNombre;
    $(divTextos).append('<label class="subtitulo"><b>Estado</b></label>')
    $(divTextos).append('<label class="descripcion">' + estado + '</label>')

    //Descripcion
    let descripcion = requerimiento.Descripcion;
    if (descripcion == undefined || descripcion == "") descripcion = "Sin datos";
    $(divTextos).append('<label class="subtitulo"><b>Descripción</b></label>')
    $(divTextos).append('<label class="descripcion">' + descripcion + '</label>')

    //Motivo
    let motivo = requerimiento.MotivoNombre;
    $(divTextos).append('<label class="subtitulo"><b>Motivo</b></label>')
    $(divTextos).append('<label class="descripcion">' + toTitleCase(motivo) + '</label>')


    //Direccion
    $(divTextos).append('<label class="subtitulo">Domicilio</label>')
    if ('DomicilioDireccion' in requerimiento && requerimiento.DomicilioDireccion != undefined && requerimiento.DomicilioDireccion != "") {
        $(divTextos).append('<label><b>Dirección: </b>' + toTitleCase(requerimiento.DomicilioDireccion) + '</b></label>');
    } else {
        $(divTextos).append('<label><b>Dirección: </b>Sin datos</b></label>');
    }

    //Observaciones
    if ('DomicilioObservaciones' in requerimiento && requerimiento.DomicilioObservaciones != undefined && requerimiento.DomicilioObservaciones != "") {
        $(divTextos).append('<label><b>Descripción: </b>' + toTitleCase(requerimiento.DomicilioObservaciones) + '</b></label>');
    } else {
        $(divTextos).append('<label><b>Descripción: </b>Sin datos</b></label>');
    }

    //Cpc
    let cpc = '';
    if ('CpcNombre' in requerimiento && 'CpcNumero' in requerimiento && requerimiento.CpcNombre != undefined && requerimiento.CpcNumero != undefined) {
        $(divTextos).append('<label><b>CPC: </b>' + ("N° " + requerimiento.CpcNumero + ' - ' + requerimiento.CpcNombre) + '</b></label>');
    } else {
        $(divTextos).append('<label><b>CPC: </b>Sin datos</b></label>');
    }

    //Barrio
    if ('BarrioNombre' in requerimiento && requerimiento.BarrioNombre != undefined && requerimiento.BarrioNombre != "") {
        $(divTextos).append('<label><b>Barrio: </b>' + "N° " + requerimiento.BarrioNombre + '</b></label>');
    } else {
        $(divTextos).append('<label><b>Barrio: </b>Sin datos</b></label>');
    }

    $(divTextos).appendTo(html);

    //Botones

    let divBotones = $('<div class="botones"></div>');

    //Nota
    let btnNota = $('<div class="accion" onclick="agregarComentario(' + requerimiento.Id + ')"><i class="material-icons icono">comment</i><label class="texto">Agregar nota</label></div>');
    if (!validarPermisoOT(PERMISO_AGREGAR_NOTA)) {
        $(btnNota).addClass('deshabilitado');
    }
    $(btnNota).appendTo(divBotones);

    let btnCambiarMotivo = $('<div class="accion" onclick="cambiarMotivoRequerimiento(' + requerimiento.Id + ')"><i class="material-icons icono">edit</i><label class="texto">Cambiar Motivo</label></div>');
    if (!validarPermisoOT(PERMISO_QUITAR_REQUERIMIENTO)) {
        $(btnCambiarMotivo).addClass('deshabilitado');
    }
    $(btnCambiarMotivo).on('click', function () {
        cambiarMotivoRequerimiento(requerimiento.Id);
    });
    $(btnCambiarMotivo).appendTo(divBotones);

    let btnBorrar = $('<div class="accion rojo" onclick="quitarRequerimiento(' + requerimiento.Id + ')"><i class="material-icons icono">delete</i><label class="texto">Quitar de OT</label></div>');
    if (!validarPermisoOT(PERMISO_QUITAR_REQUERIMIENTO)) {
        $(btnBorrar).addClass('deshabilitado');
    }
    $(btnBorrar).on('click', function () {
        quitarRequerimiento(requerimiento.Id);
    });
    $(btnBorrar).appendTo(divBotones);

    $(divBotones).appendTo(html);

    return html;
}

function BotonCpc(controlDiv, map) {
    $(controlDiv).addClass('contenedorBtnCpc');

    // Set CSS for the control border.
    var controlUI = document.createElement('div');
    $(controlUI).addClass('btnMapa');
    controlUI.setAttribute("id", "btnCPC");
    controlUI.title = 'Mapa de CPCs';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior.
    var controlText = document.createElement('div');
    controlText.innerHTML = 'Mapa de CPCs';
    controlUI.appendChild(controlText);

    // Setup the click event listeners: simply set the map to Chicago.
    controlUI.addEventListener('click', function () {
        $(controlUI).toggleClass('activo');
        viendoPoligonosCPC = !viendoPoligonosCPC;
        for (var i = 0; i < poligonosCPC.length; i++) {
            if (viendoPoligonosCPC) {
                poligonosCPC[i].setMap(map);
            } else {
                poligonosCPC[i].setMap(null);
            }
        }

        if (viendoPoligonosCPC) {
            if (map.getZoom() > 11) {
                map.setZoom(11);
            }
        }
    });
}

function BotonExpandir(controlDiv, map) {
    $(controlDiv).addClass('contenedorBtnExpandirMapa');

    // Set CSS for the control border.
    var controlUI = document.createElement('div');
    $(controlUI).addClass('btnMapa');
    controlUI.setAttribute("id", "btnExpandirMapa");
    controlUI.title = 'Expandir';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior.
    var controlText = document.createElement('div');
    controlText.innerHTML = 'Expandir';
    controlUI.appendChild(controlText);

    // Setup the click event listeners: simply set the map to Chicago.
    controlUI.addEventListener('click', function () {
        $(controlUI).toggleClass('activo');
        if (mapaExpandido) {
            $(controlUI).find('div').text("Expandir");
            achicarMapa();
        } else {
            $(controlUI).find('div').text("Comprimir");
            expandirMapa();
        }
    });
}

function cargarMapa() {
    //Quito los marcadores
    $.each(mapa_marcadores, function (index, element) {
        element.setMap(undefined);
        element = undefined;
    });
    $.each(mapa_popups, function (index, element) {
        element.setMap(undefined);
        element = undefined;
    });
    $.each(mapa_PoligonosBarrios, function (index, element) {
        element.setMap(undefined);
        element = undefined;
    });

    mapa_marcadores = [];
    mapa_popups = [];
    mapa_PoligonosBarrios = [];

    //Si no tiene marcadores la ot, cancelo aca
    if (!('Requerimientos' in ot) || ot.Requerimientos.length == 0) return;

    //Agrego marcadores y centro en ellos
    $.each(ot.Requerimientos, function (index, rq) {
        let keyLatitud = "DomicilioLatitud";
        let keyLongitud = "DomicilioLongitud";

        if (rq[keyLatitud] != undefined) {
            let lat = rq[keyLatitud].replace(',', '.');
            let lng = rq[keyLongitud].replace(',', '.');
            let pos = { lat: parseFloat(lat), lng: parseFloat(lng) };
            let marcador = new google.maps.Marker();
            marcador.setPosition(pos);
            marcador.setMap(map);
            marcador.set('id', rq.Id);
            marcador.setIcon(pinSymbol('#' + rq.EstadoColor));
            mapa_marcadores.push(marcador);

            //Popup
            let htmlPopup = crearHtmlMarcadorPopup(rq);
            if (htmlPopup != undefined) {


                let popupDomicilio = new google.maps.InfoWindow({
                    maxWidth: 300
                });
                popupDomicilio.set('id', rq.Id);

                popupDomicilio.setContent($(htmlPopup).prop('outerHTML'));

                marcador.addListener('click', function () {

                    //Cierro el popuop de una zona
                    if (mapa_PopupZona != undefined) {
                        mapa_PopupZona.close();
                        mapa_PopupZona = undefined;
                    }

                    //Desresalto las zonas
                    $.each(mapa_PoligonosBarrios, function (i1, e1) { e1.setOptions({ fillOpacity: mapa_OpacityZona }); });

                    //Cierro los popups de los requerimientos
                    $.each(mapa_popups, function (index1, p) {
                        p.close();
                    });

                    //Abro el popup actual
                    popupDomicilio.open(map, marcador);
                });

                mapa_popups.push(popupDomicilio);
            }
        }
    });

    //Centro
    centrarMapa();

    //Dibujo los poligonos de los barrios de las zonas
    top.getBarrios().then(function (barrios) {
        $.each(ot.Barrios, function (index, element) {
            let b = $.grep(barrios, function (e2, i2) { return e2.id == element.Id })[0];
            if (b != undefined) {
                let p = new google.maps.Polygon({
                    paths: b.poligono,
                    strokeColor: element.ZonaColor,
                    strokeOpacity: mapa_OpacityZona * 2,
                    strokeWeight: 2,
                    fillColor: element.ZonaColor,
                    fillOpacity: mapa_OpacityZona,
                    clickable: true,
                    zIndex: mapa_ZIndexZona,
                    map: map
                });
                p.set('IdZona', element.ZonaId);
                mapa_PoligonosBarrios.push(p);

                google.maps.event.addListener(p, 'click', function (event) {
                    $.each(mapa_PoligonosBarrios, function (i, poligonoBarrio) {
                        poligonoBarrio.setOptions({ fillOpacity: mapa_OpacityZona });
                    });

                    p.setOptions({ fillOpacity: mapa_OpacityZona * 2 });

                    if (mapa_PopupZona != undefined) {
                        mapa_PopupZona.close();
                        mapa_PopupZona = undefined;
                    }

                    mapa_PopupZona = new google.maps.InfoWindow({
                        maxWidth: 300
                    });
                    google.maps.event.addListener(mapa_PopupZona, 'closeclick', function () {
                        p.setOptions({ fillOpacity: mapa_OpacityZona });
                    });
                    mapa_PopupZona.setContent("Zona " + element.ZonaNombre + " | Barrio: " + b.nombre);

                    var bounds = new google.maps.LatLngBounds();
                    for (let i = 0; i < p.getPath().length; i++) {
                        let punto = p.getPath().getAt(i);
                        bounds.extend({ lat: punto.lat(), lng: punto.lng() });
                    }
                    map.fitBounds(bounds);
                    mapa_PopupZona.setPosition({ lat: bounds.getCenter().lat(), lng: bounds.getCenter().lng() });
                    mapa_PopupZona.open(map);
                });

            }

        });
    });
}


//Descripcion
function initDescripcion() {

}

function cargarDescripcion() {
    let descripcion = ot.Descripcion;
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

    if (!('Requerimientos' in ot) && ot.Requerimientos.length == 0) return;

    var rqPorMotivo = [];

    let rq = ot.Requerimientos;
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
            },
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
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
    let cantidad = _.where(ot.Notas, { RequerimientoId: rq.Id }).length;
    $(html).find('.botones > .nota > .badge').text(cantidad);
    $(html).find('.botones .nota').click(function () {
        if (rq.Id == undefined) return;
        mostrarTodosLosComentariosDeRequerimiento(rq.Id);
    });

    //Boton tareas
    if (cantidadTareas > 0) {
        $(html).find('.botones .tareas').click(function () {
            if (rq.Id == undefined) return;
            mostrarTareasDeRequerimiento(rq.Id);
        });
    } else {
        $(html).find('.botones .tareas').hide();
    }

    //Boton motivo
    $(html).find('.botones .motivo').click(function () {
        if (rq.Id == undefined) return;
        cambiarMotivoRequerimiento(rq.Id);
    });

    //Boton borrar
    $(html).find('.botones .borrar').click(function () {
        if (rq.Id == undefined) return;
        quitarRequerimiento(rq.Id);
    });

    return html;
}

//Flotas
function initFlotas() {
    $('#btn_AgregarFlotas').click(function () {
        agregarFlota();
    });
}

function cargarFlotas() {
    $('#contenedor_Flotas > .contenido > .items').empty();

    if (ot.Flotas.length != 0) {
        $('#contenedor_Flotas').show();

        $.each(ot.Flotas, function (index, emp) {
            let html_emp = crearHtmlFlota(emp);
            $('#contenedor_Flotas > .contenido > .items').append(html_emp);
        });
    } else {
        $('#contenedor_Flotas').hide();
    }
}

function crearHtmlFlota(entity) {
    var div = $($('#template_Flota').html());

    $(div).attr("id", entity.FlotaId);

    cargarDatosFlota(entity, div);

    //Evento detalle del flota
    $(div).find(".contenedor_Nombre > .link").click(function () {
        crearDialogoFlotaDetalle({
            Id: entity.FlotaId,
            Callback: function () {
                actualizarDetalle().then(function () {
                    cargarMoviles();
                });
            }
        })
    });

    // agregarAccion({
    //     Texto: "Editar",
    //     Icono: 'edit',
    //     OnClick: function () {
    //         crearDialogoFlotaEditar({
    //             IdArea: entity.IdArea,
    //             Id: entity.Id,
    //             CallbackMensajes: function (tipo, mensaje) {
    //                 mostrarMensaje(tipo, mensaje);
    //             },
    //             Callback: function (id) {
    //                 actualizarCardFlota(id);
    //             }
    //         });
    //     }
    // });

    //Evento detalle del móvil
    $(div).find('.contenedor_Movil > .btnVerDetalle').click(function () {
        crearDialogoMovilDetalle2({ Id: entity.MovilId });
    });

    return div;
}

function cargarDatosFlota(entity, div) {

    //Nombre
    $(div).find('.contenedor_Nombre .nombre').text(entity.FlotaNombre);

    //Móvil
    $(div).find('.contenedor_Movil > .numero').text(entity.MovilNumeroInterno);
    $(div).find('.contenedor_Movil > .tipo').text(entity.MovilNombreTipo);
    //$(div).find('.contenedor_Movil > .marca').text(entity.MovilMarca + ' ' + entity.MovilModelo);

    //Estado
    let estadoColor = '#' + entity.EstadoColor;
    $(div).find('.contenedor_Estado > .indicador').css('color', estadoColor);
    $(div).find('.contenedor_Estado > .nombre').text(toTitleCase(entity.EstadoNombre).trim());

    //Divisor de color segun estado
    $(div).find('.separador').css('border-color', estadoColor);

    if (entity.Empleados.length > 0) {
        $(div).find(".contenedor_Personal .contenido").empty();

        //espacio disponible para empleados
        let espacioDisponible = $("#template_Flota").width();
        let tamañoMasEmpleados = $("#template_MasEmpleados").width();

        $.each(entity.Empleados, function (i, empleado) {
            let divEmpleado = crearHtmlEmpleado(empleado);
            $(div).find(".contenedor_Personal .contenido").append(divEmpleado);
        })
    }
}

function mostrarFlotas() {
    abrirPanelDeslizable('Flotas');

    var divContenido = $($('#template_Flotas').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divContenido);

    $(divContenido).find('.contenedor_Cargando').addClass('visible');
    crearAjax({
        Url: ResolveUrl('~/Servicios/FlotaService.asmx/GetResultadoByIdOrdenTrabajo'),
        Data: { id: ot.Id },
        OnSuccess: function (result) {
            $(divContenido).find('.contenedor_Cargando').removeClass('visible');
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                cerrarPanelDeslizable();
                return;
            }

            $(divContenido).find('table').prop('id', 'tablaFlotas');
            $(divContenido).find('input.busqueda').prop('id', 'input_BusquedaFlotas');

            let dt = $('#tablaFlotas').DataTableFlota({
                Buscar: true,
                InputBusqueda: '#input_BusquedaFlotas',
                BotonBorrar: true,
                CallbackBorrar: function (flota) {
                    //var empEliminado = $.map(ot.Flotas, function (e) {
                    //    if (e.FlotaId == flota.Id)
                    //        return e.Id;
                    //});

                    //if (empEliminado.length == 0) {
                    //    mostrarMensaje("Error", "Error intentando eliminar el flota.")
                    //    return;
                    //}

                    //quitarFlota(empEliminado[0].Id);

                    quitarFlota(flota.Id);
                }
            });

            //Inicializo los tooltips
            dt.$('.tooltipped').tooltip({ delay: 50 });
            dt.$('.selectMaterialize').material_select();

            //Muevo el indicador y el paginado a mi propio div
            $(divContenido).find('.tabla-footer').empty();
            $(divContenido).find('.dataTables_info').detach().appendTo($(divContenido).find('.tabla-footer'));
            $(divContenido).find('.dataTables_paginate').detach().appendTo($(divContenido).find('.tabla-footer'));
            $(divContenido).find('.dataTables_info').hide();

            //Agrego las filas
            let hDisponible = $(divContenido).find('.tabla-contenedor').height();
            let rows = calcularCantidadRowsDataTable(hDisponible);
            dt.page.len(rows);
            dt.rows.add(result.Return).draw(true);
        },
        OnError: function (result) {
            $(divContenido).find('.contenedor_Cargando').removeClass('visible');
            mostrarMensaje('Error', 'Error procesando la solicitud');
            cerrarPanelDeslizable();
        }
    });

    //Agregar
    $(divContenido).find('.btn_Nuevo').css('display', validarPermisoOT(PERMISO_EDITAR_FLOTAS) ? 'auto' : 'none');
    $(divContenido).find('.btn_Nuevo').click(function () {
        agregarFlota();
    });

    //Reintentar
    $(divContenido).find('.btn_Reintentar').click(function () {
        mostrarFlotas();
    });
}


//Moviles
function cargarMoviles() {
    $('#contenedor_Moviles> .contenido > .items').empty();

    if (ot.Moviles.length != 0) {
        $('#contenedor_Moviles').show();

        $.each(ot.Moviles, function (index, m) {
            let html_movil = crearHtmlMovil(m);
            $('#contenedor_Moviles > .contenido > .items').append(html_movil);
        });
    } else {
        $('#contenedor_Moviles').hide();
    }
}

function crearHtmlMovil(entity) {
    var div = $($("#template_Movil").html());

    //Móvil
    $(div).find('.numero').text(entity.NumeroInterno);
    $(div).find('.tipo').text(entity.TipoNombre);
    $(div).find('.marca').text(entity.Marca + ' ' + entity.Modelo);

    //Numero click
    $(div).find('.numero').click(function () {
        crearDialogoMovilDetalle2({
            Id: entity.MovilId,
            Callback: function () {
                actualizarDetalle().then(function () {
                    cargarMoviles();
                });
            },
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
        });
    });

    return div;
}

//Empleados
function initEmpleados() {
    $('#btn_AgregarEmpleados').click(function () {
        agregarEmpleados();
    });
}

function cargarEmpleados() {
    $('#contenedor_Empleados > .contenido > .items').empty();

    if (ot.Empleados.length != 0) {
        $('#contenedor_Empleados').show();

        $.each(ot.Empleados, function (index, emp) {
            let html_emp = crearHtmlEmpleado(emp);
            $('#contenedor_Empleados > .contenido > .items').append(html_emp);
        });
    } else {
        $('#contenedor_Empleados').hide();
    }
}

function crearHtmlEmpleado(e) {
    var html = $($('#template_Empleado').html());
    $(html).attr('id-e', e.EmpleadoId);

    $(html).find(' .nombre').text(e.Nombre + ' ' + e.Apellido);

    let foto;
    if (e.UsuarioIdentificadorFotoPersonal != undefined) {
        foto = top.urlCordobaFiles + '/Archivo/' + e.UsuarioIdentificadorFotoPersonal + '/3';
    } else {
        foto = e.UsuarioSexoMasculino == true ? PATH_IMAGEN_USER_MALE : PATH_IMAGEN_USER_FEMALE;
    }

    $(html)
   .find(".persona .foto")
   .css("background-image", "url(" + foto + ")");

    //Numero click
    $(html).find('.nombre, .foto').click(function () {
        crearDialogoEmpleadoDetalle({
            Id: e.EmpleadoId,
            Callback: function () {
                actualizarDetalle().then(function () {
                    cargarDatos();
                });
            },
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
        });
    });

    //Boton borrar
    $(html).find('.borrar').click(function () {
        if (e.Id == undefined) return;
        quitarEmpleado(e.EmpleadoId);
    });

    return html;
}

function mostrarEmpleados() {
    abrirPanelDeslizable('Empleados');

    var divContenido = $($('#template_Empleados').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divContenido);

    $(divContenido).find('.contenedor_Cargando').addClass('visible');
    crearAjax({
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/GetResultadoTablaByIdOrdenTrabajo'),
        Data: { idOrden: ot.Id },
        OnSuccess: function (result) {
            $(divContenido).find('.contenedor_Cargando').removeClass('visible');
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                cerrarPanelDeslizable();
                return;
            }

            $(divContenido).find('table').prop('id', 'tablaEmpleados');
            $(divContenido).find('input.busqueda').prop('id', 'input_BusquedaEmpleados');

            let dt = $('#tablaEmpleados').DataTableEmpleado({
                Buscar: true,
                InputBusqueda: '#input_BusquedaEmpleados',
                BotonBorrar: true,
                CallbackBorrar: function (empleado) {
                    //var empEliminado = $.map(ot.Empleados, function (e) {
                    //    if (e.EmpleadoId == empleado.Id)
                    //        return e.Id;
                    //});

                    //if (empEliminado.length == 0) {
                    //    mostrarMensaje("Error", "Error intentando eliminar el empleado.")
                    //    return;
                    //}

                    //quitarEmpleado(empEliminado[0].Id);

                    quitarEmpleado(empleado.Id);
                }
            });

            //Inicializo los tooltips
            dt.$('.tooltipped').tooltip({ delay: 50 });
            dt.$('.selectMaterialize').material_select();

            //Muevo el indicador y el paginado a mi propio div
            $(divContenido).find('.tabla-footer').empty();
            $(divContenido).find('.dataTables_info').detach().appendTo($(divContenido).find('.tabla-footer'));
            $(divContenido).find('.dataTables_paginate').detach().appendTo($(divContenido).find('.tabla-footer'));
            $(divContenido).find('.dataTables_info').hide();

            //Agrego las filas
            let hDisponible = $(divContenido).find('.tabla-contenedor').height();
            let rows = calcularCantidadRowsDataTable(hDisponible);
            dt.page.len(rows);
            dt.rows.add(result.Return.Data).draw(true);
        },
        OnError: function (result) {
            $(divContenido).find('.contenedor_Cargando').removeClass('visible');
            mostrarMensaje('Error', 'Error procesando la solicitud');
            cerrarPanelDeslizable();
        }
    });

    //Agregar
    $(divContenido).find('.btn_Nuevo').css('display', validarPermisoOT(PERMISO_EDITAR_EMPLEADOS) ? 'auto' : 'none');
    $(divContenido).find('.btn_Nuevo').click(function () {
        agregarEmpleados();
    });

    //Reintentar
    $(divContenido).find('.btn_Reintentar').click(function () {
        mostrarEmpleados();
    });
}

//Comentarios
function initComentarios() {
    $('#contenedor_Comentarios .verMas').click(function () {
        mostrarTodosLosComentarios();
    });

    $('#btn_AgregarComentario').click(function () {
        agregarComentario();
    });
}

function mostrarTodosLosComentarios() {
    abrirPanelDeslizable('Notas internas');

    var div = $($('#template_ComentariosDetalle').html());
    $(div).attr('id', 'contenedor_ComentariosDetalle');
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(div);
    $('#contenedor_ComentariosDetalle .contenido').empty();

    //Cargo los comentarios
    let notas = _.filter(ot.Notas, function (e) { return e.RequerimientoId == undefined || e.RequerimientoId == null; });
    if (notas.length != 0) {
        $.each(notas, function (index, element) {
            var div = crearHtmlComentario(element);
            $('#contenedor_ComentariosDetalle').append(div);
        });
    }

    //BotonAgregar
    $('#contenedor_PanelDeslizable .btnNuevoComentario').click(function () {
        let comentario = $('#contenedor_PanelDeslizable input').val();
        if (comentario == undefined || comentario == "") {
            mostrarMensaje('Alerta', 'Ingrese el contenido de la nota interna');
            $('#contenedor_PanelDeslizable input').focus();
            return;
        }

        procesarAgregarComentario(undefined, comentario);
    });
}

let viendoNotasDeRequerimiento;

function mostrarTodosLosComentariosDeRequerimiento(id) {

    let rq = _.find(ot.Requerimientos, function (rq) { return rq.Id == id; });
    if (rq == undefined) return;

    viendoNotasDeRequerimiento = id;

    abrirPanelDeslizable('Notas internas de requerimiento ' + rq.Numero + '/' + rq.Año);

    var div = $($('#template_ComentariosDetalle').html());
    $(div).attr('id', 'contenedor_ComentariosDetalle');
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(div);
    $('#contenedor_ComentariosDetalle .contenido').empty();

    //Cargo los comentarios
    let notas = _.filter(ot.Notas, function (e) { return e.RequerimientoId == id; });
    if (notas.length != 0) {
        $.each(notas, function (index, element) {
            var div = crearHtmlComentario(element);
            $('#contenedor_ComentariosDetalle').append(div);
        });
    }

    //BotonAgregar
    $('#contenedor_PanelDeslizable .btnNuevoComentario').click(function () {
        let comentario = $('#contenedor_PanelDeslizable input').val();
        if (comentario == undefined || comentario == "") {
            mostrarMensaje('Alerta', 'Ingrese el contenido de la nota interna');
            $('#contenedor_PanelDeslizable input').focus();
            return;
        }

        procesarAgregarComentario(id, comentario);
    });
}

function crearHtmlComentario(data) {
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
                },
                CallbackMensajes: function (tipo, mensaje) {
                    mostrarMensaje(tipo, mensaje);
                }
            })
        });
    }

    let foto;
    if (data.UsuarioIdentificadorFotoPersonal != undefined) {
        foto = top.urlCordobaFiles + '/Archivo/' + data.UsuarioIdentificadorFotoPersonal + '/3';
    } else {
        foto = data.UsuarioSexoMasculino == true ? PATH_IMAGEN_USER_MALE : PATH_IMAGEN_USER_FEMALE;
    }
    $(div).find('.persona img').attr('src', foto);

    $(div).find('.persona label').text(data.UsuarioNombre + ' ' + data.UsuarioApellido);
    $(div).find('.card .contenido').text(data.Observaciones);
    $(div).find('.card .fecha').text(dateTimeToString(data.Fecha));

    $(div).find('.persona label').click(function () {
        crearDialogoUsuarioDetalle({
            Id: data.UsuarioId,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
        });
    });

    return div;
}

function cargarComentarios() {
    $('#contenedor_Comentarios .contenido .items').empty();

    let notas = _.filter(ot.Notas, function (e) { return e.RequerimientoId == undefined || e.RequerimientoId == null; });
    if (notas.length != 0) {
        //$('#contenedor_Comentarios .sinItems').hide();
        //$('#contenedor_Comentarios .contenido').show();
        $('#contenedor_Comentarios').show();

        $.each(notas, function (index, element) {
            if (index < 3) {
                var div = crearHtmlComentario(element);
                $('#contenedor_Comentarios .contenido .items').append(div);
            }
        });

        if ($('#contenedor_Comentarios .items').height() > $('#contenedor_Comentarios .contenido')) {
            $('#contenedor_Comentarios .verMas').hide();
        } else {
            $('#contenedor_Comentarios .verMas').show();
        }

    } else {
        //$('#contenedor_Comentarios .sinItems').show();
        $('#contenedor_Comentarios .verMas').hide();
        //$('#contenedor_Comentarios .contenido').hide();
        $('#contenedor_Comentarios').hide();
    }
}

//Tareas
let tareas = [];
let idsTareasSeleccionadas = [];
function mostrarTareasDeRequerimiento(idRq) {
    //if (!validarPermisoRequerimiento(PERMISO_AGREGAR_TAREAS)) {
    //    mostrarMensaje('Error', 'El requerimiento no se encuentra en un estado válido para realizar esta accion');
    //    return;
    //}

    abrirPanelDeslizable('Tareas', function () {
        editarTareas(idRq, getIdsTareasSeleccionadas());
    });

    var divContenido = $($('#template_Tareas').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divContenido);

    //Reintentar
    $(divContenido).find('.btn_Reintentar').click(function () {
        abrirPanelTareas();
    });

    $(divContenido).find('.contenedor_Cargando').addClass('visible');
    buscarTareas().then(function (data) {
        tareas = data;

        buscarTareasDelRequerimiento(idRq).then(function (tareasRq) {
            let tareasTotales = tareas.slice();;
            if (tareasRq != undefined && tareasRq.length != 0) {
                idsTareasSeleccionadas = _.pluck(tareasRq, 'Id');

                //elimino las tareas del rq que ya están en la lista general
                $.each(tareasRq, function (i, ta) {
                    let index;
                    let tareaYaEsta = _.find(tareasTotales, function (obj) {
                        index = tareasTotales.indexOf(obj);
                        return obj.Id == ta.Id;
                    });

                    //si la tarea ya está, la elimino porque total va a agregarse despues en la union
                    if (tareaYaEsta != undefined) {
                        tareasTotales.splice(index, 1);
                    }
                })

                tareasTotales = _.union(tareasTotales, tareasRq);
            }

            initTablaTareas(divContenido, tareasTotales);
        });

    }).catch(function () {
        cerrarPanelDeslizable();
    }).finally(function () {
        $(divContenido).find('.contenedor_Cargando').removeClass('visible');
    });
}

function initTablaTareas(divContenido, tareasTotales) {
    $(divContenido).find('table').prop('id', 'tablaTareas');
    $(divContenido).find('input.busqueda').prop('id', 'input_BusquedaTareas');

    let dt = $('#tablaTareas').DataTableTareas({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Orden: [0, "desc"],
        CallbackActualizar: function (tarea) {
            actualizarListaTareas(tarea);
        },
        Buscar: true,
        InputBusqueda: '#input_BusquedaTareas',
        BotonSeleccionar: true,
        Callback: function (data) {
            dt.rows().every(function () {
                let d = this.data();
                console.log(d);
                if (d.Id == data.Id) {
                    let node = this.node();
                    $(node).find('input').prop('checked', idsTareasSeleccionadas.indexOf(data.Id) != -1);
                }
                console.log(idsTareasSeleccionadas);
            });
        },
        OnFilaCreada: function (row, data) {
            $(row).find('input').prop('checked', idsTareasSeleccionadas.indexOf(data.Id) != -1);
            $(row).find('input').change(function () {
                let check = $(this).is(':checked');
                if (check) {
                    if (idsTareasSeleccionadas.indexOf(data.Id) == -1) {
                        idsTareasSeleccionadas.push(data.Id);
                    }
                } else {
                    idsTareasSeleccionadas = $.grep(idsTareasSeleccionadas, function (element, index) { return element != data.Id });
                }

                console.log(idsTareasSeleccionadas);
            });
        }
    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
    dt.$('.selectMaterialize').material_select();

    //Muevo el indicador y el paginado a mi propio div
    $(divContenido).find('.tabla-footer').empty();
    $(divContenido).find('.dataTables_info').detach().appendTo($(divContenido).find('.tabla-footer'));
    $(divContenido).find('.dataTables_paginate').detach().appendTo($(divContenido).find('.tabla-footer'));
    $(divContenido).find('.dataTables_info').hide();

    //Agrego las filas
    let hDisponible = $(divContenido).find('.tabla-contenedor').height();
    let rows = calcularCantidadRowsDataTable(hDisponible);
    dt.page.len(rows);

    var tareasOrdenadas = ordenarListaTareas(tareasTotales);
    dt.rows.add(tareasOrdenadas).draw(true);
}

function ordenarListaTareas(tareasTotales) {
    //recorro los ids de las tareas seleccionadas y busco el objeto en la lista de tareas
    var tareasOrdenadas = [];
    $.each(idsTareasSeleccionadas, function (i, id) {
        var index;
        var tareaSeleccionada = _.find(tareasTotales, function (t) {
            //guardo el index para poder borrar el elemento de la lista de tareas general
            index = tareasTotales.indexOf(t);

            return id == t.Id;
        });

        //cuando la encuentro la pongo en la lista ordenada
        tareasOrdenadas.push(tareaSeleccionada);
        //elimino de la lista de todas las tareas, la ya ordenada
        tareasTotales.splice(index, 1);
    })

    //concateno a las tareas ordenadas, las tareas restantes (que no estan seleccionadas)
    tareasOrdenadas = tareasOrdenadas.concat(tareasTotales);
    return tareasOrdenadas;
}

function buscarTareas() {
    return new Promise(function (callback, callbackError) {
        //si ya busque las tareas anteriormente, no las busco de nuevo
        if (tareas != undefined && tareas.length != 0) {
            callback(tareas);
            return;
        }

        crearAjax({
            Url: ResolveUrl('~/Servicios/TareaService.asmx/GetByIdArea'),
            Data: { idArea: ot.AreaId },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    mostrarMensaje('Error', result.Error);
                    callbackError();
                    return;
                }

                //tareas = result.Return;
                callback(result.Return);
            },
            OnError: function (result) {
                mostrarMensaje('Error', 'Error procesando la solicitud');
                callbackError();
            }
        })
    });
}

function buscarTareasDelRequerimiento(idRequerimiento) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/TareaService.asmx/GetByIdRequerimiento'),
            Data: { idRequerimiento: idRequerimiento },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    mostrarMensaje('Error', result.Error);
                    callbackError();
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                mostrarMensaje('Error', 'Error procesando la solicitud');
                callbackError();
            }
        })
    });
}

function calcularCantidadDeRowsTareas(div) {
    var hDisponible = $(div).find('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $(div).find('table').DataTable();
    dt.page.len(rows).draw();
    console.log(rows);
}

function editarTareas(idRequerimiento, idsTareas) {
    mostrarCargando(true);
    crearAjax({
        Data: {
            comando: {
                IdRequerimiento: idRequerimiento,
                IdsTareas: idsTareas,
            }
        },
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EditarTareas'),
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }


        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function getIdsTareasSeleccionadas() {
    return idsTareasSeleccionadas;
}

//Ultimo Estado
function initUltimoEstado() {
    $('#contenedor_UltimoEstado .estado .nombrePersona').click(function () {
        if (!('EstadoUsuarioId' in ot) || ot.EstadoUsuarioId == undefined) return;
        crearDialogoUsuarioDetalle({
            Id: ot.EstadoUsuarioId,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
        });
    });

    $('#btn_VerHistorialEstado').click(function () {
        mostrarHistorialDeEstados();
    });
}

function cargarUltimoEstado() {
    if ('EstadoNombre' in ot && ot.EstadoNombre != undefined) {
        $('#contenedor_UltimoEstado .estado .nombre').text(toTitleCase(ot.EstadoNombre));
    } else {
        $('#contenedor_UltimoEstado .estado .nombre').text('Sin datos');
    }

    if ('EstadoColor' in ot && ot.EstadoColor != undefined) {
        $('#contenedor_UltimoEstado .estado .circulo').css('background-color', '#' + ot.EstadoColor);
    } else {
        $('#contenedor_UltimoEstado .estado .circulo').css('background-color', 'black');
    }

    if ('EstadoObservaciones' in ot && ot.EstadoObservaciones != undefined) {
        $('#contenedor_UltimoEstado .estado .motivo').text(ot.EstadoObservaciones);
    } else {
        $('#contenedor_UltimoEstado .estado .motivo').text('Sin datos');
    }

    if ('EstadoUsuarioNombre' in ot && ot.EstadoUsuarioNombre != undefined && 'EstadoUsuarioApellido' in ot && ot.EstadoUsuarioApellido != undefined) {
        $('#contenedor_UltimoEstado .estado .nombrePersona').text(toTitleCase(ot.EstadoUsuarioNombre + ' ' + ot.EstadoUsuarioApellido));
    } else {
        $('#contenedor_UltimoEstado .estado .nombrePersona').text('Sin datos');
    }

    if ('EstadoFecha' in ot && ot.EstadoFecha != undefined) {
        $('#contenedor_UltimoEstado .estado .fecha').html(' el <b>' + dateTimeToString(ot.EstadoFecha) + '</b>');
    } else {
        $('#contenedor_UltimoEstado .estado .fecha').html(' el *sin datos*');
    }
}


//Recursos adicionales
function initRecursosAdicionales() {

}

function cargarRecursosAdicionales() {
    var hayRecursosAdicionales = false;
    if ('RecursoMaterial' in ot && ot.RecursoMaterial != undefined && ot.RecursoMaterial.trim() != "") {
        $('#contenedor_RecursosAdicionales .material').html('<b>Materiales </b>' + ot.RecursoMaterial);
        hayRecursosAdicionales = true;
    }

    if ('RecursoPersonal' in ot && ot.RecursoPersonal != undefined && ot.RecursoPersonal.trim() != "") {
        $('#contenedor_RecursosAdicionales .personal').html('<b>Personal </b>' + ot.RecursoPersonal);
        hayRecursosAdicionales = true;
    }

    if (!hayRecursosAdicionales) {
        $('#contenedor_RecursosAdicionales').hide();
    }
}


//Informacion adicional
function initInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoUsuarioCreador').click(function () {
        crearDialogoUsuarioDetalle({
            Id: requerimiento.UsuarioCreadorId,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
        });
    });

    $('#contenedor_InfoAdicional .textoUsuarioModificacion').click(function () {
        crearDialogoUsuarioDetalle({
            Id: requerimiento.UsuarioModificacionId,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
        });
    });
}

function cargarInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoFechaCreacion').html('<b>' + dateTimeToString(ot.FechaAlta) + '</b>');

    if ('UsuarioCreadorNombre' in ot && ot.UsuarioCreadorNombre != undefined && 'UsuarioCreadorApellido' in ot && ot.UsuarioCreadorApellido != undefined) {
        $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').show();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').show();
        var usuarioCreador = toTitleCase(ot.UsuarioCreadorNombre + ' ' + ot.UsuarioCreadorApellido).trim();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').html('<b>' + usuarioCreador + '</b>');
    } else {
        $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').hide();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').hide();
    }

    if ('FechaModificacion' in ot && ot.FechaModificacion != undefined) {
        $('#contenedor_InfoAdicional .linea2').show();
        $('#contenedor_InfoAdicional .textoFechaModificacion').html('<b>' + dateTimeToString(ot.FechaModificacion) + '</b>');
        if ('UsuarioModificacionNombre' in ot && ot.UsuarioModificacionNombre != undefined && 'UsuarioModificacionApellido' in ot && ot.UsuarioModificacionApellido != undefined) {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').html('<b>' + toTitleCase(ot.UsuarioModificacionNombre + ' ' + ot.UsuarioModificacionApellido) + '</b>');
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
        Permiso: validarPermisoOT(PERMISO_EDITAR_DESCRIPCION),
        Icono: 'comment',
        OnClick: function () {
            editarDescripcion();
        }
    });

    //Agregar RQ
    agregarAccion({
        Texto: 'Agregar requerimiento',
        Permiso: validarPermisoOT(PERMISO_AGREGAR_REQUERIMIENTO),
        Icono: 'add',
        OnClick: function () {
            agregarRequerimiento();
        }
    });

    //Editar recursos adicionales
    agregarAccion({
        Texto: 'Editar recursos',
        Permiso: validarPermisoOT(PERMISO_EDITAR_RECURSOS),
        Icono: 'edit',
        OnClick: function () {
            editarRecursos();
        }
    });

    //Agregar nota
    agregarAccion({
        Texto: 'Agregar nota',
        Permiso: validarPermisoOT(PERMISO_AGREGAR_NOTA),
        Icono: 'comment',
        OnClick: function () {
            agregarComentario();
        }
    });

    //Cerrar
    agregarAccion({
        Texto: 'Cerrar OT',
        Permiso: validarPermisoOT(PERMISO_CERRAR),
        Icono: 'build',
        OnClick: function () {
            cerrar();
        }
    });

    //Cancelar
    agregarAccion({
        Texto: 'Cancelar OT',
        Permiso: validarPermisoOT(PERMISO_CANCELAR),
        Icono: 'clear',
        OnClick: function () {
            cancelar();
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

    //Reenviar comprobante
    agregarAccion({
        Texto: 'Enviar por mail',
        Icono: 'mail',
        OnClick: function () {
            crearDialogoOrdenTrabajoEnviarMail({
                Id: ot.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    mostrarMensaje(tipo, mensaje);
                },
                CallbackCargando: function (cargando, mensaje) {
                    mostrarCargando(cargando, mensaje);
                }
            })
        }
    });

    if (ot.Moviles.length != 0 || cantidadMoviles != 0) {
        //Editar moviles
        agregarAccion({
            Texto: 'Agregar/quitar móviles',
            Permiso: validarPermisoOT(PERMISO_EDITAR_MOVILES),
            Icono: 'directions_car',
            OnClick: function () {
                agregarMoviles();
            }
        });
    }

    if (ot.Empleados.length != 0 || cantidadEmpleados != 0) {
        //Editar empleados
        agregarAccion({
            Texto: 'Agregar/quitar personal',
            Permiso: validarPermisoOT(PERMISO_EDITAR_EMPLEADOS),
            IconoMdi: 'worker',
            OnClick: function () {
                agregarEmpleados();
            }
        });
    }

    if (ot.Flotas.length != 0 || cantidadFlotas != 0) {
        //Editar empleados
        agregarAccion({
            Texto: 'Agregar/quitar flota',
            Permiso: validarPermisoOT(PERMISO_EDITAR_FLOTAS),
            IconoMdi: 'worker',
            OnClick: function () {
                agregarFlota();
            }
        });
    }
}

function agregarAccion(valores) {
    var div = $($('#template_Accion').html());
    $(div).find('.texto').text(valores.Texto);

    if (valores.IconoMdi != undefined) {
        $(div).find('.icono').addClass('mdi mdi-' + valores.IconoMdi);
    } else {
        $(div).find('.icono').text(valores.Icono);
    }

    $(div).attr('permiso', valores.PermisoKeyValue);
    $('#contenedor_Acciones .contenido').append(div);
    if (('Permiso' in valores) && !valores.Permiso) {
        $(div).addClass('deshabilitado');
    }
    $(div).click(function () {
        valores.OnClick();
    });
}

function expandirMapa() {
    mapaExpandido = true;
    $('#main').addClass('mapaExpandido');
}

function achicarMapa() {
    mapaExpandido = false;
    $('#main').removeClass('mapaExpandido');
}

function centrarMapa() {
    //Agrego marcadores y centro en ellos
    var bounds = new google.maps.LatLngBounds();
    $.each(ot.Requerimientos, function (index, rq) {
        let keyLatitud = "DomicilioLatitud";
        let keyLongitud = "DomicilioLongitud";

        if (rq[keyLatitud] != undefined) {
            let lat = rq[keyLatitud].replace(',', '.');
            let lng = rq[keyLongitud].replace(',', '.');
            let pos = { lat: parseFloat(lat), lng: parseFloat(lng) };
            bounds.extend(pos);
        }
    });
    map.fitBounds(bounds);
}

function editarDescripcion() {
    if (!validarPermisoOT(PERMISO_EDITAR_DESCRIPCION)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    let placeholder = ot.Descripcion;
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

                    var url = ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/EditarDescripcion');

                    crearAjax({
                        Url: url,
                        Data: { comando: { IdOrdenTrabajo: ot.Id, Descripcion: descripcion } },
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

function cambiarSeccion() {
    if (!validarPermisoOT(PERMISO_CAMBIAR_SECCION)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    crearDialogoOrdenTrabajoCambiarSeccion({
        Id: ot.Id,
        IdArea: ot.AreaId,
        IdSeccion: ot.SeccionId,
        Callback: function (id) {
            actualizarDetalle().then(function () {
                cargarDatosEncabezado();
                $(jAlert).CerrarDialogo();
            });
        },

    });
}

function agregarRequerimiento() {
    if (!validarPermisoOT(PERMISO_AGREGAR_REQUERIMIENTO)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    crearDialogoOrdenTrabajoAgregarRequerimientos({
        Id: ot.Id,
        Callback: function () {
            actualizarDetalle().then(function () {
                cargarDatos();
            });
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });
}

function agregarComentario(idRequerimiento) {
    if (!validarPermisoOT(PERMISO_AGREGAR_NOTA)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    let placeholder = idRequerimiento != undefined ? 'Nueva nota internat para requerimiento...' : 'Nueva nota interna...';
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
                    let comentario = input.val();
                    if (comentario == "") {
                        $(jAlert).find('input').focus();
                        mostrarMensaje('Alerta', 'Ingrese el contenido de la nota interna');
                        return;
                    }
                    $(jAlert).CerrarDialogo();
                    procesarAgregarComentario(idRequerimiento, comentario);
                }
            },

        ]
    });
}

function procesarAgregarComentario(idRequerimiento, comentario) {
    if (!validarPermisoOT(PERMISO_AGREGAR_NOTA)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    var url = ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/AgregarNota');
    var data = {
        comando: {
            IdOrdenTrabajo: ot.Id,
            IdRequerimiento: idRequerimiento,
            Observaciones: comentario
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
                cargarComentarios();

                if ($('#contenedor_PanelDeslizable').hasClass('visible') && $('#contenedor_PanelDeslizable').length != 0) {
                    if (idRequerimiento == undefined) {
                        mostrarTodosLosComentarios();
                    } else {
                        cargarRequerimientos();
                        mostrarTodosLosComentariosDeRequerimiento(idRequerimiento);
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

function cambiarMotivoRequerimiento(id) {
    crearDialogoRequerimientoCambiarMotivo({
        Id: id,
        VerDetalleRequerimiento: false,

        DesdeOT: true,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackCargando: function (mostrar, mensaje) {
            mostrarCargando(mostrar, mensaje);
        },
        Callback: function (result) {
            actualizarDetalle().then(function () {
                cargarDatos();
            });
        }
    });
}

function quitarRequerimiento(id) {
    console.log('borrar');
    if (!validarPermisoOT(PERMISO_QUITAR_REQUERIMIENTO)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta accion');
        return;
    }


    crearDialogoOrdenTrabajoQuitarRequerimiento({
        IdOt: ot.Id,
        IdRq: id,
        Callback: function () {
            actualizarDetalle().then(function () {
                cargarDatos();
            });
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });

}

function editarRecursos() {
    if (!validarPermisoOT(PERMISO_EDITAR_RECURSOS)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    crearDialogoOrdenTrabajoRecursos({
        Id: ot.Id,
        Callback: function () {
            actualizarDetalle().then(function () {
                cargarRecursosAdicionales();
            });
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });
}

function cerrar() {
    if (!validarPermisoOT(PERMISO_CERRAR)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    crearDialogoOrdenTrabajoCerrar({
        Id: ot.Id,
        Callback: function () {
            actualizarDetalle().then(function () {
                cargarDatos();
            });
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });
}

function cancelar() {
    if (!validarPermisoOT(PERMISO_CANCELAR)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para realizar esta acción');
        return;
    }


    crearDialogoInput({
        Titulo: 'Cancelar Orden de Trabajo',
        Placeholder: 'Ingrese un motivo...',
        Botones: [
            {
                Texto: 'Volver'
            },
            {
                Texto: 'Aceptar',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    let motivo = $(jAlert).find('input').val();
                    if (motivo == "") {
                        mostrarMensaje('Alerta', 'Ingrese el motivo de cierre de la orden');
                        $(jAlert).find('input').focus();
                        return;
                    }

                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/Cancelar'),
                        Data: { comando: { Id: ot.Id, Motivo: motivo } },
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

function agregarMoviles() {
    if (!validarPermisoOT(PERMISO_EDITAR_MOVILES)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para editar los moviles');
        return;
    }

    crearDialogoAgregarMovil({
        IdArea: ot.AreaId,
        IdOT: ot.Id,
        IdsMoviles: _.pluck(ot.Moviles, 'MovilId'),
        CallbackSeleccionar: function (data) {

            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/EditarMoviles'),
                Data: { comando: { IdOrdenTrabajo: ot.Id, IdMoviles: data } },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle().then(function () {

                        cargarDatosEncabezado();
                        actualizarRecursosMoviles();

                        if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                            mostrarMoviles();
                        }
                    });
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
            });
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });

}

function quitarMovil(id) {
    if (!validarPermisoOT(PERMISO_EDITAR_MOVILES)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para editar los moviles');
        return;
    }

    crearDialogoConfirmacion({
        Texto: "¿Está seguro de que desea quitar éste personal?",
        CallbackPositivo: function () {
            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/QuitarMovil'),
                Data: { comando: { IdOrdenTrabajo: ot.Id, IdMovil: id } },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle().then(function () {
                        cargarDatosEncabezado();
                        cargarAcciones();
                        actualizarRecursosMoviles();

                        mostrarMensaje('Exito', 'Móvil eliminado de la orden de trabajo');

                        if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                            mostrarMoviles();
                        }
                    });
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}

function agregarEmpleados() {
    if (!validarPermisoOT(PERMISO_EDITAR_EMPLEADOS)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para editar el personal');
        return;
    }

    crearDialogoAgregarEmpleado({
        IdArea: ot.AreaId,
        IdOT: ot.Id,
        IdsEmpleados: _.pluck(ot.Empleados, 'EmpleadoId'),
        CallbackSeleccionar: function (data) {

            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/EditarEmpleados'),
                Data: { comando: { IdOrdenTrabajo: ot.Id, IdEmpleados: data } },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle().then(function () {
                        actualizarRecursosEmpleados();
                        cargarDatosEncabezado();
                        cargarEmpleados();

                        if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                            mostrarEmpleados();
                        }
                    });
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
            });
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });

}

function quitarEmpleado(id) {
    if (!validarPermisoOT(PERMISO_EDITAR_EMPLEADOS)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para editar el personal');
        return;
    }

    crearDialogoConfirmacion({
        Texto: "¿Está seguro de que desea quitar al personal?",
        CallbackPositivo: function () {
            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/QuitarEmpleado'),
                Data: { comando: { IdOrdenTrabajo: ot.Id, IdEmpleado: id } },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle().then(function () {
                        cargarDatosEncabezado();
                        cargarEmpleados();
                        actualizarRecursosEmpleados();

                        mostrarMensaje('Exito', 'Personal eliminado de la orden de trabajo');

                        if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                            mostrarEmpleados();
                        }
                    });
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}

function agregarFlota() {
    if (!validarPermisoOT(PERMISO_EDITAR_FLOTAS)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para editar las flotas');
        return;
    }

    crearDialogoAgregarFlota({
        IdArea: ot.AreaId,
        IdOT: ot.Id,
        IdsFlotas: [],
        IdsFlotas: _.pluck(ot.Flotas, 'FlotaId'),
        CallbackSeleccionar: function (data) {

            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/EditarFlotas'),
                Data: { comando: { IdOrdenTrabajo: ot.Id, IdFlotas: data } },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle().then(function () {

                        cargarDatosEncabezado();
                        cargarFlotas();
                        actualizarRecursosFlotas();

                        if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                            mostrarFlotas();
                        }
                    });
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
            });
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });

}

function quitarFlota(id) {
    if (!validarPermisoOT(PERMISO_EDITAR_FLOTAS)) {
        mostrarMensaje('Error', 'La orden de trabajo no se encuentra en un estado válido para editar las flotas');
        return;
    }

    crearDialogoConfirmacion({
        Texto: "¿Está seguro de que desea quitar a la flota?",
        CallbackPositivo: function () {
            mostrarCargando(true);
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/QuitarFlota'),
                Data: { comando: { IdOrdenTrabajo: ot.Id, IdFlota: id } },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle().then(function () {
                        cargarDatosEncabezado();
                        cargarFlotas();
                        actualizarRecursosFlotas();

                        mostrarMensaje('Exito', 'Flota eliminada de la orden de trabajo');

                        if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                            mostrarFlotas();
                        }
                    });
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
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
        },
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });
}

function imprimirOrdenDatallada() {
    crearDialogoReporteOrdenTrabajoDatallada({
        Id: ot.Id
    });
}

function imprimirSinMapa() {
    crearDialogoReporteOrdenTrabajoDetalleSinMapa({
        Id: ot.Id
    });
}

function imprimirResumen() {
    crearDialogoReporteOrdenTrabajoCaratulaConMapa({
        Id: ot.Id
    });
}

function imprimirResumenSinMapa() {
    crearDialogoReporteOrdenTrabajoCaratulaSinMapa({
        Id: ot.Id
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
            Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/GetDetalleById'),
            Data: { id: ot.Id },
            OnSuccess: function (result) {
                mostrarCargando(false);
                if (!result.Ok) {
                    mostrarError(result.Error);
                    return;
                }

                ot = result.Return;
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

function validarPermisoOT(keyValuePermiso) {
    if (permisos == undefined) return false;
    if (permisos.length == 0) return false;

    var permiso = $.grep(permisos, function (element, index) {
        return element.EstadoOrdenTrabajo == ot.EstadoKeyValue && element.Permiso == keyValuePermiso && element.TienePermiso;
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