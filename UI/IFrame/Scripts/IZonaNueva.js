let modo;
let MODO_ALTA = "alta";
let MODO_EDIT = "edit";

let tablaSeleccionados;
let tablaDisponibles;
let callback;

let centroCordoba;
let barrios;
let barriosSeleccionados = [];
let poligonosBarrios = [];
let poligonosBarriosSeleccionados = [];
let idsBarriosNoDisponibles = [];
let MAX_ZOOM = 19;
let map;

let color_barrio = '#4CAF50';
let color_barrio_default = '#000';
let color_barrio_no_disponible = '#FF0000';

let color_cpc = '#673AB7';
let color_ejido = '#000000';

let strokeOpacity_seleccionado = 0.8;
let fillOpacity_seleccionado = 0.4;
let strokeOpacity = 0.3;
let fillOpacity = 0.2;
let strokeWeight = 2;

let dadoDeBaja = false;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    //--------------------------
    // Init Data
    //--------------------------
    zona = data.Zona;
    area = data.Area;
    idsBarriosNoDisponibles = data.IdsBarrioNoDisponibles;

    console.log('Area');
    console.log(area);

    if (zona != undefined) {
        modo = MODO_EDIT;
    } else {
        modo = MODO_ALTA;
    }



    switch (modo) {
        case MODO_ALTA: {
            $('#contenedor_Acciones').hide();
        } break;

        case MODO_EDIT: {
            $('#contenedor_Acciones').show();
            if (zona.FechaBaja != undefined) {
                dadoDeBaja = true;
                $('#btn_DarDeAlta').show();
                $('#btn_DarDeBaja').hide();
            } else {
                $('#btn_DarDeAlta').hide();
                $('#btn_DarDeBaja').show();
            }

            console.log('Zona');
            console.log(zona);

            console.log('Barrios');
            console.log(data.IdsBarrio);

            $('#input_Nombre').val(zona.Nombre);
            barriosSeleccionados = data.IdsBarrio;

            Materialize.updateTextFields();


            $('#btn_DarDeBaja').click(function () {
                mostrarCargando(true);
                ajax_DarDeBaja(zona.Id)
                    .then(function (zona) {
                        informar(zona);
                    })
                    .catch(function (error) {
                        mostrarCargando(false);
                        top.mostrarMensaje('Error', error);
                    });
            });

            $('#btn_DarDeAlta').click(function () {
                mostrarCargando(true);
                ajax_DarDeAlta(zona.Id)
                    .then(function (zona) {
                        informar(zona);
                    })
                    .catch(function (error) {
                        mostrarCargando(false);
                        top.mostrarMensaje('Error', error);
                    });
            });
        } break;
    }

    initTabla(data);
    initMapa();
}


function initTabla(data) {
    tablaSeleccionados = $('#tablaSeleccionados').DataTableGeneral({
        KeyId: 'id',
        Buscar: true,
        InputBusqueda: '#input_BusquedaSeleccionados',
        Orden: [[0, 'asc']],
        Columnas: [
            {
                title: 'Nombre',
                data: 'nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            }
        ],
        Botones: [
            {
                Titulo: 'Deseleccionar',
                Icono: 'clear',
                Visible: !dadoDeBaja,
                OnClick: function (data) {
                    deseleccionarBarrio(data.id);
                    centrarEnSeleccionados();
                }
            }
        ]
    });

    tablaSeleccionados.$('.tooltipped').tooltip({ delay: 50 });
    $('#contenedor_TablaSeleccionados .tabla-footer').empty();
    $('#contenedor_TablaSeleccionados .dataTables_info').detach().appendTo($('#contenedor_TablaSeleccionados .tabla-footer'));
    $('#contenedor_TablaSeleccionados .dataTables_paginate').detach().appendTo($('#contenedor_TablaSeleccionados .tabla-footer'));

    tablaDisponibles = $('#tablaDisponibles').DataTableGeneral({
        KeyId: 'id',
        Buscar: true,
        InputBusqueda: '#input_BusquedaDisponibles',
        Orden: [[0, 'asc']],
        Columnas: [
            {
                title: 'Nombre',
                data: 'nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            }
        ],
        Botones: [
            {
                Titulo: 'Seleccionar',
                Icono: 'check',
                Visible: !dadoDeBaja,
                OnClick: function (data) {
                    seleccionarBarrio(data.id);
                    centrarEnSeleccionados();
                }
            }
        ]
    });

    tablaDisponibles.$('.tooltipped').tooltip({ delay: 50 });
    $('#contenedor_TablaDisponibles .tabla-footer').empty();
    $('#contenedor_TablaDisponibles .dataTables_info').detach().appendTo($('#contenedor_TablaDisponibles .tabla-footer'));
    $('#contenedor_TablaDisponibles .dataTables_paginate').detach().appendTo($('#contenedor_TablaDisponibles .tabla-footer'));

    calcularCantidadDeRows();
}

function calcularCantidadDeRows() {
    var hDisponible = $('#contenedor_TablaSeleccionados .tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaSeleccionados.page.len(rows).draw();

    hDisponible = $('#contenedor_TablaDisponibles .tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaDisponibles.page.len(rows).draw();
}

function initMapa() {
    $.getScript("https://maps.googleapis.com/maps/api/js?key=" + KEY_GOOGLE_MAPS + "&libraries=visualization", function () {

        var myStyles = [
            {
                featureType: "poi",
                elementType: "labels",
                stylers: [
                      { visibility: "off" }
                ]
            }
        ];

        centroCordoba = { lat: parseFloat(-31.416111), lng: parseFloat(-64.191174) };

        map = new google.maps.Map(document.getElementById('mapa'), {
            center: centroCordoba,
            zoom: 13,
            maxZoom: MAX_ZOOM,
            fullscreenControl: false,
            gestureHandling: 'greedy',
            mapTypeControlOptions: {
                style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
                position: google.maps.ControlPosition.TOP_RIGHT
            },
            styles: myStyles
        });

        google.maps.event.addListenerOnce(map, 'tilesloaded', function () {
            cargarBarrios();
        });
    });
}

function cargarBarrios() {
    mostrarCargando(true);

    getBarrios().then(function (b) {
        calcularCantidadDeRows();

        //Agrego barrios disponibles
        barrios = b;
        let barriosDisponibles = $.grep(barrios, function (element, index) {
            return !esBarrioSeleccionado(element.id) && !esBarrioNoDisponible(element.id);
        });
        tablaDisponibles.rows.add(barriosDisponibles).draw();

        //Agrego barrios seleccionados
        $.each(barriosSeleccionados, function (index, idBarrio) {
            let b = $.grep(barrios, function (b1, e1) { return b1.id == idBarrio })[0];
            if (b != undefined) {
                tablaSeleccionados.row.add(b);
                tablaSeleccionados.draw(false);
            }
        });

        let infowindow = new google.maps.InfoWindow({});

        //Creo los poligonos
        $.each(barrios, function (index, barrio) {

            let p = new google.maps.Polygon({
                paths: barrio.poligono,
                strokeColor: esBarrioNoDisponible(barrio.id) ? color_barrio_no_disponible : color_barrio_default,
                strokeOpacity: strokeOpacity,
                strokeWeight: strokeWeight,
                fillColor: esBarrioNoDisponible(barrio.id) ? color_barrio_no_disponible : color_barrio_default,
                fillOpacity: fillOpacity,
                map: map
            });
            p.set('id', barrio.id);
            poligonosBarrios.push(p);

            //Click en poligono
            google.maps.event.addListener(p, 'click', function (event) {
                if (dadoDeBaja) return;

                if (esBarrioNoDisponible(barrio.id)) {
                    mostrarMensaje('Alerta', 'El barrio indicado ya forma parte de otra zona en en el área indicada');
                    return;
                }

                toggleBarrioSeleccionado(barrio.id);
                tablaDisponibles.rows().invalidate('data').draw(false);

                if (esBarrioSeleccionado(barrio.id)) {
                    infowindow.setContent("Barrio " + barrio.nombre);
                    infowindow.open(map);
                    infowindow.setPosition(event.latLng);
                } else {
                    infowindow.close();
                }
            });
        });


        centrarEnSeleccionados();

        actualizarPoligonos();
        mostrarCargando(false);
    }).catch(function (error) {
        console.log(error);
    });
}

function toggleBarrioSeleccionado(idBarrio) {
    if (esBarrioSeleccionado(idBarrio)) {
        deseleccionarBarrio(idBarrio);
    } else {
        seleccionarBarrio(idBarrio);
    }
}

function seleccionarBarrio(idBarrio) {
    if (esBarrioSeleccionado()) return;

    //Lo agrego al listado
    barriosSeleccionados.push(idBarrio);

    //Quito de la tabla de disponibles
    let barrio;
    tablaDisponibles.rows().every(function () {
        let d = this.data();
        if (d != undefined) {
            if (d.id == idBarrio) {
                barrio = d;
                $(this.node()).addClass('borrado');
            }
        }
    });

    tablaDisponibles.rows('.borrado').remove().draw(false);

    //Agrego a la tabla de seleccionados
    tablaSeleccionados.row.add(barrio).draw(false);

    //Actualizo los poligonos
    actualizarPoligonos();
}

function actualizarPoligonos() {
    if (poligonosBarrios == undefined) return;
    $.each(poligonosBarrios, function (index, element) {
        let id = element.get('id');
        element.setOptions({
            fillColor: esBarrioNoDisponible(id) ? color_barrio_no_disponible : esBarrioSeleccionado(id) ? color_barrio : color_barrio_default,
            strokeColor: esBarrioNoDisponible(id) ? color_barrio_no_disponible : esBarrioSeleccionado(id) ? color_barrio : color_barrio_default,
            strokeOpacity: esBarrioNoDisponible(id) ? 1 : esBarrioSeleccionado(id) ? 1 : 0.3,
        });
    });
}


function centrarEnSeleccionados() {
    if (barriosSeleccionados.length == 0) {
        map.setCenter(centroCordoba);
        map.setZoom(11);
        return;
    }
    //Centro
    let bounds = new google.maps.LatLngBounds();
    $.each(barriosSeleccionados, function (index, idBarrio) {
        let barrio = $.grep(barrios, function (barrio, index) { return barrio.id == idBarrio })[0];
        if (barrio != undefined) {
            for (let i = 0; i < barrio.poligono.length; i++) {
                bounds.extend(barrio.poligono[i]);
            }
        }
    });
    map.fitBounds(bounds);
}


function deseleccionarBarrio(idBarrio) {
    if (!esBarrioSeleccionado(idBarrio)) return;

    //quito del listado de barrios
    barriosSeleccionados = $.grep(barriosSeleccionados, function (element, index) {
        return element != idBarrio;
    });

    //Quito de la tabla de seleccionados
    let barrio;
    tablaSeleccionados.rows().every(function () {
        let d = this.data();
        if (d != undefined) {
            if (d.id == idBarrio) {
                barrio = d;
                $(this.node()).addClass('borrado');
            }
        }
    });

    tablaSeleccionados.rows('.borrado').remove().draw(false);

    //Agrego a la tabla de disponibles
    tablaDisponibles.row.add(barrio).draw(false);

    //Actualizo poligonos
    actualizarPoligonos();
}

function esBarrioSeleccionado(idBarrio) {
    return barriosSeleccionados.indexOf(idBarrio) != -1;
}

function esBarrioNoDisponible(idBarrio) {
    return idsBarriosNoDisponibles.indexOf(idBarrio) != -1;
}


//--------------------------
// Operacion principal
//--------------------------

function registrar() {
    if (!validar()) return;

    mostrarCargando(true, 'Registrando Zona...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/ZonaService.asmx/Insertar'),
        Data: { comando: getZona() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function editar() {
    if (!validar()) return;

    mostrarCargando(true, 'Actualizando Zona...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/ZonaService.asmx/Actualizar'),
        Data: { comando: getZona() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function validar() {
    let formValido = $('#form').valid();
    let tieneBarrios = barriosSeleccionados.length != 0;

    if (!tieneBarrios) {
        mostrarMensaje('Alerta', 'Seleccione algun barrio');
    }

    return formValido && tieneBarrios;
}

function getZona() {

    var zon = {};
    if (modo == MODO_EDIT) {
        zon.Id = zona.Id;
    } else {
        zon.IdArea = area.Id;
    }

    zon.Nombre = "" + $('#input_Nombre').val();
    zon.IdsBarrios = barriosSeleccionados;
    return zon;
}



function ajax_DarDeBaja(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: '~/Servicios/ZonaService.asmx/DarDeBaja',
            Data: { id: id },
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

function ajax_DarDeAlta(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: '~/Servicios/ZonaService.asmx/DarDeAlta',
            Data: { id: id },
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
//-------------------------------
// Registrar
//-------------------------------

function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(zona) {
    if (callback == undefined || callback == null) return;
    callback(zona);
}