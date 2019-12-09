const URL_BUSCAR = '~/Servicios/ServicioDomicilio.asmx/Buscar';
const URL_SUGERIR = '~/Servicios/ServicioDomicilio.asmx/Sugerir';

let ControlMapa_CallbackCargando;

let controlMapa_CentroCordoba = { lat: -31.416111, lng: -64.191174 };

let controlMapa_Marcador;
let controlMapa_InfoWindows;

let controlMapa_ZoomMax;
let controlMapa_ZoomDefault;
let controlMapa_ZoomMarcador;

let controlMapa_Barrios;
let controlMapa_Cpcs;
let controlMapa_Ejido;

let controlMapa_PoligonoBarrioSeleccionado;
let controlMapa_PoligonoCpcSeleccionado;
let controlMapa_PoligonosCpcs;
let controlMapa_PoligonosBarrios;
let controlMapa_PoligonoEjido;

let controlMapa_ColorCpc;
let controlMapa_ColorBarrio;
let controlMapa_ColorEjido;

let controlMapa_FillOpacity;
let controlMapa_StrokeOpacity;
let controlMapa_StrokeWidth;

let controlMapa_ZIndexCpc = 1;
let controlMapa_ZIndexBarrio = 2;
let controlMapa_ZIndexEjido = 3;

let controlMapa_IncluirCpcsEnSugerencias;
let controlMapa_IncluirBarriosEnSugerencias;
let controlMapa_PermitirBuscar;
let controlMapa_ResaltarAlHacerClick;
let controlMapa_BotonCpcs;
let controlMapa_BotonBarrios;
let controlMapa_BotonEjido;
let controlMapa_BotonFullscreen
let controlMapa_BotonLocalizacion;


function ControlMapa_Init(valores) {
    $('.tooltipped').tooltip();
    if (valores === undefined) valores = {};

    //Buscar
    if (!('Buscar' in valores)) valores.Buscar = false;
    controlMapa_PermitirBuscar = valores.Buscar;
    $('#ControlMapa_ContenedorBusqueda').css('display', controlMapa_PermitirBuscar ? 'auto' : 'none');

    if (!('CpcEnSugerencias' in valores)) valores.CpcEnSugerencias = true;
    controlMapa_IncluirCpcsEnSugerencias = valores.CpcEnSugerencias;

    if (!('BarrioEnSugerencias' in valores)) valores.BarrioEnSugerencias = true;
    controlMapa_IncluirBarriosEnSugerencias = valores.BarrioEnSugerencias;

    //Resaltar al hacer click
    if (!('ResaltarAlHacerClick' in valores)) valores.ResaltarAlHacerClick = true;
    controlMapa_ResaltarAlHacerClick = valores.ResaltarAlHacerClick;

    //Color barrio
    if (!('ColorBarrio' in valores)) valores.ColorBarrio = '#00BCD4';
    controlMapa_ColorBarrio = valores.ColorBarrio;
    $('#ControlMapa_BtnBarrios').css('border-color', controlMapa_ColorBarrio);

    //Color CPC
    if (!('ColorCpc' in valores)) valores.ColorCpc = '#9C27B0';
    controlMapa_ColorCpc = valores.ColorCpc;
    $('#ControlMapa_BtnCpcs').css('border-color', controlMapa_ColorCpc);

    //Color Ejido
    if (!('ColorEjido' in valores)) valores.ColorEjido = '#000000';
    controlMapa_ColorEjido = valores.ColorEjido;
    $('#ControlMapa_BtnEjido').css('border-color', controlMapa_ColorEjido);

    //Fill Opacity
    if (!('FillOpacity' in valores)) valores.FillOpacity = 0.4;
    controlMapa_FillOpacity = valores.FillOpacity;

    //Stroke Opacity
    if (!('StrokeOpacity' in valores)) valores.StrokeOpacity = 0.3;
    controlMapa_StrokeOpacity = valores.StrokeOpacity;

    //Stroke Width
    if (!('StrokeWidth' in valores)) valores.StrokeWidth = 2;
    controlMapa_StrokeWidth = valores.StrokeWidth;

    //Max Zoom
    if (!('MaxZoom' in valores)) valores.MaxZoom = 19;
    controlMapa_ZoomMax = valores.MaxZoom;

    //Zoom Default
    if (!('DefaultZoom' in valores)) valores.DefaultZoom = 13;
    controlMapa_ZoomDefault = valores.DefaultZoom;

    //Zoom marcador
    if (!('MarcadorZoom' in valores)) valores.MarcadorZoom = 15;
    controlMapa_ZoomMarcador = valores.MarcadorZoom;

    //Botones
    if (!('BotonCpcs' in valores)) valores.BotonCpcs = true;
    controlMapa_BotonCpcs = valores.BotonCpcs;
    $('#ControlMapa_BtnCpcs').css('display', controlMapa_BotonCpcs ? 'auto' : 'none');

    if (!('BotonBarrios' in valores)) valores.BotonBarrios = true;
    controlMapa_BotonBarrios = valores.BotonBarrios;
    $('#ControlMapa_BtnBarrios').css('display', controlMapa_BotonBarrios ? 'auto' : 'none');

    if (!('BotonEjido' in valores)) valores.BotonEjido = true;
    controlMapa_BotonEjido = valores.BotonEjido;
    $('#ControlMapa_BtnEjido').css('display', controlMapa_BotonEjido ? 'auto' : 'none');

    if (!('BotonFullscreen' in valores)) valores.BotonFullscreen = false;
    controlMapa_BotonFullscreen = valores.BotonFullscreen;
    $('#ControlMapa_BtnFullscreen').css('display', controlMapa_BotonFullscreen ? 'block' : 'none');

    if ("geolocation" in navigator && /pixel|ipad|tablet|android|blackberry|ipod|lumia|mobile|opera mini|opera mobi|phone|playbook|webos/i.test(navigator.userAgent)) {
        if (!('BotonLocalizacion' in valores)) valores.BotonLocalizacion = true;
        controlMapa_BotonLocalizacion = valores.BotonLocalizacion;
        $('#ControlMapa_BtnLocalizacion').css('display', controlMapa_BotonLocalizacion ? 'block' : 'none');
    }

    if ('OnMapReady' in valores) controlMapa_Listener_MapReady = valores.OnMapReady;
    if ('OnLimpiar' in valores) controlMapa_Listener_Limpiar = valores.OnLimpiar;
    if ('OnCargando' in valores) controlMapa_Listener_Cargando = valores.OnCargando;
    if ('OnValidarLimpiar' in valores) controlMapa_Listener_ValidarLimpiar = valores.OnValidarLimpiar;
    if ('OnMapaClick' in valores) controlMapa_Listener_MapaClick = valores.OnMapaClick;
    if ('OnValidarMarcador' in valores) controlMapa_Listener_ValidarMarcador = valores.OnValidarMarcador;
    if ('OnMarcador' in valores) controlMapa_Listener_Marcador = valores.OnMarcador;
    if ('OnMarcadorIcono' in valores) controlMapa_Listener_MarcadorIcono = valores.OnMarcadorIcono;
    if ('OnMarcadorPopup' in valores) controlMapa_Listener_MarcadorPopup = valores.OnMarcadorPopup;
    if ('OnValidarResaltarCpc' in valores) controlMapa_Listener_ValidarResaltarCpc = valores.OnValidarResaltarCpc;
    if ('OnCpcResaltado' in valores) controlMapa_Listener_CpcResaltado = valores.OnCpcResaltado;
    if ('OnValidarResaltarBarrio' in valores) controlMapa_Listener_ValidarResaltarBarrio = valores.OnValidarResaltarBarrio;
    if ('OnBarrioResaltado' in valores) controlMapa_Listener_BarrioResaltado = valores.OnBarrioResaltado;
    if ('OnBotonCpcs' in valores) controlMapa_Listener_BotonCpcs = valores.OnBotonCpcs;
    if ('OnBotonBarrios' in valores) controlMapa_Listener_BotonBarrios = valores.OnBotonBarrios;
    if ('OnBotonEjido' in valores) controlMapa_Listener_BotonEjido = valores.OnBotonEjido;
    if ('OnBotonFullscreen' in valores) controlMapa_Listener_BotonFullscreen = valores.OnBotonFullscreen;
    if ('OnInputBusquedaKeyDown' in valores) controlMapa_Listener_InputBusquedaKeyDown = valores.OnInputBusquedaKeyDown;
    if ('OnInputBusquedaChange' in valores) controlMapa_Listener_InputBusquedaChange = valores.OnInputBusquedaChange;
    if ('OnValidarBusqueda' in valores) controlMapa_Listener_ValidarBusqueda = valores.OnValidarBusqueda;
    if ('OnCorregirBusqueda' in valores) controlMapa_Listener_CorregirBusqueda = valores.OnCorregirBusqueda;

    $.getScript("https://maps.googleapis.com/maps/api/js?key=" + KEY_GOOGLE_MAPS + "&libraries=visualization,places", function () {
        getGeoApiInfo()
            .then(function (data) {
                controlMapa_Barrios = data.barrios;
                controlMapa_Cpcs = data.cpcs;
                controlMapa_Ejido = data.ejido;

                if (controlMapa_PermitirBuscar) {
                    ControlMapa_InitBuscar();
                }
                ControlMapa_InitMapa();
                ControlMapa_InitBotones();

                $('#ControlMapa_Contenedor').addClass('visible');

                ControlMapa_Listener_MapReady(map);
            }).catch(function (error) {
                console.log(error);
            });
    });
}

function ControlMapa_InitBuscar() {

    $('#ControlMapa_InputBuscar').on('keydown', function (e) {
        let key = e.originalEvent.key;
        let busqueda = $(this).val();

        if (e.originalEvent.keyCode === 13) {
            $('#ControlMapa_BotonBuscar').trigger('click');
        }

        let consumir = ControlMapa_Listener_InputBusquedaKeyDown(key, busqueda);
        if (consumir === true) {
            return false;
        }
    });

    $('#ControlMapa_InputBuscar').on('input', function () {
        let busqueda = $(this).val();

        ControlMapa_Listener_InputBusquedaChange(busqueda);
    });

    $('#ControlMapa_BotonBuscar').click(function () {
        let busqueda = $('#ControlMapa_InputBuscar').val();

        //Valido la busqueda
        if (ControlMapa_Listener_ValidarBusqueda(busqueda) === false) return;

        //Corrijo la busqueda
        let busquedaCorregida = ControlMapa_Listener_CorregirBusqueda(busqueda);
        if (busquedaCorregida !== undefined) {
            busqueda = busquedaCorregida;
            $('#ControlMapa_InputBuscar').val(busqueda);
            Materialize.updateTextFields();
        }

        //Busco
        ControlMapa_MostrarCargando();
        $('#ControlMapa_ContenedorSugerencias').removeClass('visible');

        ControlMapa_Ajax_BuscarSugerencias(busqueda)
            .then(function (data) {
                //Filtro sugerencias
                let sugerenciasFiltradas = ControlMapa_Listener_FiltrarSugerencias(data);
                if (sugerenciasFiltradas !== undefined) {
                    data = sugerenciasFiltradas;
                }

                //Informo
                ControlMapa_Listener_Sugerencias(data);

                //OCulto cargando
                ControlMapa_OcultarCargando();

                //Cargo sugerencias
                ControlMapa_CargarSugerencias(data);

                //Si uno solo Item click
                if (data.Cpcs.length === 0 && data.Barrios.length === 0 && data.Items.length === 1) {
                    $($('#ControlMapa_ContenedorSugerencias .sugerencia')[0]).trigger('click');
                }
            })
            .catch(function (error) {
                ControlMapa_OcultarCargando();
            });
    });
}

function ControlMapa_InitMapa() {
    var myStyles = [
        {
            featureType: "poi",
            elementType: "labels",
            stylers: [{ visibility: "off" }]
        }
    ];

    map = new google.maps.Map(document.querySelector('#ControlMapa_Mapa'), {
        center: controlMapa_CentroCordoba,
        zoom: controlMapa_ZoomDefault,
        maxZoom: controlMapa_ZoomMax,
        fullscreenControl: false,
        clickableIcons: false,
        gestureHandling: 'greedy',
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            position: google.maps.ControlPosition.TOP_RIGHT
        },
        styles: myStyles
    });

    google.maps.event.addListener(map, "click", function (event) {
        //Mando evento al hacer click
        let eventoConsumido = ControlMapa_Listener_MapaClick(event);
        if (eventoConsumido === true) return;

        //Proceso el click
        ControlMapa_MostrarCargando();
        ControlMapa_Limpiar();

        ControlMapa_Ajax_Buscar(event.latLng.lat(), event.latLng.lng())
            .then(function (result) {
                ControlMapa_OcultarCargando();

                if (result == undefined) {
                    top.mostrarMensaje('Alerta', 'Ubicación fuera del ejido municipal');
                    return;
                }

                //Marcador
                ControlMapa_MostrarMarcador(result);

                //Si es que permito resaltar al hacer click...
                if (!controlMapa_ResaltarAlHacerClick) return;

                //Resalto barrio
                if (result.Barrio !== undefined) {
                    ControlMapa_ResaltarBarrio(result.Barrio.IdCatastro);
                }

                //Resalto cpc
                if (result.Cpc !== undefined) {
                    ControlMapa_ResaltarCpc(result.Cpc.IdCatastro);
                }

            })
            .catch(function (error) {
                ControlMapa_OcultarCargando();
                top.mostrarMensaje('Error', error);
            });
    });
}

function ControlMapa_InitBotones() {
    $('#ControlMapa_Botones').addClass('visible');

    $('#ControlMapa_BtnCpcs').click(function () {
        $(this).toggleClass('seleccionado');

        if (controlMapa_PoligonosCpcs != undefined) {
            $.each(controlMapa_PoligonosCpcs, function (index, element) {
                element.setMap(undefined);
            });
        }

        controlMapa_PoligonosCpcs = [];

        if ($(this).hasClass('seleccionado')) {
            $.each(controlMapa_Cpcs, function (index, element) {

                let p = new google.maps.Polygon({
                    paths: element.poligono,
                    strokeColor: controlMapa_ColorEjido,
                    strokeOpacity: controlMapa_StrokeOpacity,
                    strokeWeight: controlMapa_StrokeWidth,
                    fillColor: controlMapa_ColorCpc,
                    fillOpacity: controlMapa_FillOpacity,
                    clickable: false,
                    zIndex: controlMapa_ZIndexCpc,
                    map: map
                });

                controlMapa_PoligonosCpcs.push(p);
            });
        }
        ControlMapa_Listener_BotonCpcs($(this).hasClass('seleccionado'));
    });

    $('#ControlMapa_BtnBarrios').click(function () {
        $(this).toggleClass('seleccionado');

        if (controlMapa_PoligonosBarrios != undefined) {
            $.each(controlMapa_PoligonosBarrios, function (index, element) {
                element.setMap(undefined);
            });
        }

        controlMapa_PoligonosBarrios = [];

        if ($(this).hasClass('seleccionado')) {
            $.each(controlMapa_Barrios, function (index, element) {

                let p = new google.maps.Polygon({
                    paths: element.poligono,
                    strokeColor: controlMapa_ColorEjido,
                    strokeOpacity: controlMapa_StrokeOpacity,
                    strokeWeight: controlMapa_StrokeWidth,
                    fillColor: controlMapa_ColorBarrio,
                    fillOpacity: controlMapa_FillOpacity,
                    clickable: false,
                    zIndex: controlMapa_ZIndexBarrio,
                    map: map
                });

                controlMapa_PoligonosBarrios.push(p);
            });
        }
        ControlMapa_Listener_BotonBarrios($(this).hasClass('seleccionado'));

    });

    $('#ControlMapa_BtnEjido').click(function () {
        $(this).toggleClass('seleccionado');

        if (controlMapa_PoligonoEjido != undefined) {
            controlMapa_PoligonoEjido.setMap(undefined);
        }

        if ($(this).hasClass('seleccionado')) {
            controlMapa_PoligonoEjido = new google.maps.Polygon({
                paths: controlMapa_Ejido.poligono,
                strokeColor: controlMapa_ColorEjido,
                strokeOpacity: 1,
                strokeWeight: controlMapa_StrokeWidth,
                fillColor: controlMapa_ColorEjido,
                fillOpacity: 0,
                clickable: false,
                zIndex: controlMapa_ZIndexEjido + 10000,
                map: map
            });
        }
        ControlMapa_Listener_BotonEjido($(this).hasClass('seleccionado'));
    });

    $('#ControlMapa_BtnFullscreen').click(function () {
        let fullscreen = $("#ControlMapa_BtnFullscreen i").html() === "fullscreen";

        if (fullscreen) {
            $('#ControlMapa_Contenedor').addClass("fullscreen");
            $('#ControlMapa_BtnFullscreen').attr("data-tooltip", "Expandir");
            $('#ControlMapa_BtnFullscreen i').html("fullscreen_exit")
        } else {
            $('#ControlMapa_Contenedor').removeClass("fullscreen");
            $('#ControlMapa_BtnFullscreen').attr("data-tooltip", "Minimizar");
            $('#ControlMapa_BtnFullscreen i').html("fullscreen")
        }
        ControlMapa_Listener_BotonFullscreen(fullscreen);
    });

    $('#ControlMapa_BtnLocalizacion').click(function () {
        $('#ControlMapa_BtnLocalizacion a').addClass('buscando');

        navigator.geolocation.getCurrentPosition(function (position) {
            ControlMapa_Ajax_Buscar(position.coords.latitude, position.coords.longitude)
                .then(function (result) {
                    $('#ControlMapa_BtnLocalizacion a').removeClass('buscando');

                    if (result == undefined) {
                        top.mostrarMensaje('Alerta', 'Ubicación fuera del ejido municipal');
                        return;
                    }

                    //Marcador
                    ControlMapa_MostrarMarcador(result, true);

                    //Si es que permito resaltar al hacer click...
                    if (!controlMapa_ResaltarAlHacerClick) return;

                    //Resalto barrio
                    if (result.Barrio !== undefined) {
                        ControlMapa_ResaltarBarrio(result.Barrio.IdCatastro);
                    }

                    //Resalto cpc
                    if (result.Cpc !== undefined) {
                        ControlMapa_ResaltarCpc(result.Cpc.IdCatastro);
                    }
                })
                .catch(function (error) {
                    $('#ControlMapa_BtnLocalizacion a').removeClass('buscando');
                    top.mostrarMensaje('Error', error);
                });
        }, function (error) {
            informar('No podemos localizarte');
            $('#ControlMapa_BtnLocalizacion a').removeClass('buscando');
        });

        $(this).toggleClass('activo');
    });
}


//-------------------------------
// Utiles
//-------------------------------

function ControlMapa_MostrarCargando() {
    $('#ControlMapa_InputBuscar').prop('disabled', true);
    $('#ControlMapa_ContenedorBuscar > div').addClass('progress');
    ControlMapa_Listener_Cargando(true);
}

function ControlMapa_OcultarCargando() {
    $('#ControlMapa_InputBuscar').prop('disabled', false);
    $('#ControlMapa_ContenedorBuscar > div').removeClass('progress');
    ControlMapa_Listener_Cargando(false);
}

function ControlMapa_Ajax_Buscar(lat, lng) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: URL_BUSCAR,
            Data: { lat: lat, lng: lng },
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
        });
    });
}

function ControlMapa_Ajax_BuscarSugerencias(consulta) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: URL_SUGERIR,
            Data: { consulta: consulta },
            OnSuccess: function (result) {
                consulta = consulta.toLowerCase();

                if (!result.Ok && result.Error != "Sin resultados") {
                    callbackError(result.Error);
                    return;
                }

                let data = {};

                //Barrios
                data.Barrios = [];
                if (controlMapa_IncluirBarriosEnSugerencias) {
                    $.each(controlMapa_Barrios, function (index, element) {
                        if (element.nombre.toLowerCase().indexOf(consulta) !== -1) {
                            data.Barrios.push(element);
                        }
                    });
                }

                //Cpcs
                data.Cpcs = [];
                if (controlMapa_IncluirCpcsEnSugerencias) {
                    $.each(controlMapa_Cpcs, function (index, element) {
                        let consultaCpc = consulta;
                        if (consultaCpc.indexOf('cpc') !== -1) {
                            consultaCpc = consultaCpc.substring(consultaCpc.indexOf('cpc') + 3).trim();
                        }

                        if (element.nombre.toLowerCase().indexOf(consultaCpc) !== -1 || (consultaCpc == element.numero)) {
                            data.Cpcs.push(element);
                        }
                    });
                }

                //Items
                data.Items = [];
                $.each(result.Return, function (index, element) {
                    if (consulta.indexOf('cpc') === -1 && consulta.indexOf('c.p.c.') === -1) {
                        data.Items.push(element);
                    }
                });

                callback(data);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        });
    });
}

function ControlMapa_MostrarMarcador(data, centrar) {
    //Mando evento para ver si se consume
    if (ControlMapa_Listener_ValidarMarcador(data) === false) return;

    let x = parseFloat(data.Latitud.replace(',', '.'));
    let y = parseFloat(data.Longitud.replace(',', '.'));
    let pos = { lat: x, lng: y };

    //Mapa
    if (centrar !== undefined && centrar) {
        map.setCenter(pos);
        if (map.getZoom() < controlMapa_ZoomMarcador) {
            map.setZoom(controlMapa_ZoomMarcador);
        }
    }

    //Marcador
    if (controlMapa_Marcador === undefined) {
        controlMapa_Marcador = new google.maps.Marker();
    }

    //Icono por el cliente
    try {
        let icon = ControlMapa_Listener_MarcadorIcono(data);
        if (icon !== undefined) {
            controlMapa_Marcador.setIcon(icon);
        }
    } catch (ex) { }

    //Posicion y agrego
    controlMapa_Marcador.setPosition(pos);
    controlMapa_Marcador.setMap(map);

    //Ventana
    if (controlMapa_InfoWindows === undefined) {
        controlMapa_InfoWindows = new google.maps.InfoWindow({
            maxWidth: 400
        });
    }
    controlMapa_InfoWindows.setContent($(ControlMapa_CrearHtmlMarcador(data)).prop('outerHTML'));
    controlMapa_InfoWindows.open(map, controlMapa_Marcador);

    google.maps.event.addListener(controlMapa_InfoWindows, 'closeclick', function () {
        ControlMapa_Limpiar();
    });

    //Mando evento
    ControlMapa_Listener_Marcador(controlMapa_Marcador, data);
}

function ControlMapa_CrearHtmlMarcador(data) {
    let popup = ControlMapa_Listener_MarcadorPopup(data);
    if (popup !== undefined) return popup;

    let titulo = "";
    let descripcion = "";

    //Calle altura
    let textoPrincipal = "";
    if (data.Observaciones != null && data.Observaciones != "") {
        textoPrincipal = "<b>" + data.Observaciones + "</b></br>";
    }

    if (data.Direccion != undefined) {
        textoPrincipal += (data.Sugerido == 1 ? "Aproximado a " : "") + data.Direccion;
    }

    descripcion += "<label>" + textoPrincipal + "</label>";

    //Barrio
    let textoBarrio = "";
    if (data.Barrio !== undefined && data.Barrio.Nombre !== undefined) {
        textoBarrio = "<div class='barrio' style='border-color: " + controlMapa_ColorBarrio + "'><b>Barrio</b> " + toTitleCase(data.Barrio.Nombre) + '</div>';
    } else {
        textoBarrio = "<div class='barrio' style='border-color: " + controlMapa_ColorBarrio + "'><b>Barrio</b> sin datos</div>";
    }
    descripcion += "<label>" + textoBarrio + "</label>";

    //CPC
    let textoCPC = "";
    if (data.Cpc !== undefined && data.Cpc.Nombre !== undefined) {
        textoCPC = "<div class='cpc' style='border-color: " + controlMapa_ColorCpc + "'><b>CPC</b> N° " + data.Cpc.Numero + " - " + toTitleCase(data.Cpc.Nombre) + '</div>';
    } else {
        textoCPC = "<div class='cpc' style='border-color: " + controlMapa_ColorCpc + "'><b>CPC</b> Sin datos</div>";
    }
    descripcion += "<label>" + textoCPC + "</label>";

    var html = $('<div class="popup">');

    var divTextos = $('<div class="textos">');
    $(divTextos).appendTo(html);

    var labelTitulo = $('<label class="titulo">');
    $(labelTitulo).text(titulo);
    $(labelTitulo).appendTo(divTextos);

    if (descripcion !== undefined) {
        $(descripcion).appendTo(divTextos);
    }

    return html;
}


function ControlMapa_Limpiar(reset) {
    if (reset === true) {
        map.setCenter(controlMapa_CentroCordoba);
        map.setZoom(controlMapa_ZoomDefault);
    }

    //Valido
    if (ControlMapa_Listener_ValidarLimpiar() === false) return;

    if (controlMapa_Marcador !== undefined && controlMapa_Marcador.getMap() !== undefined) {
        controlMapa_Marcador.setMap(undefined);
        controlMapa_Marcador = undefined;
    }

    if (controlMapa_InfoWindows !== undefined) {
        controlMapa_InfoWindows.setMap(undefined);
        controlMapa_InfoWindows = undefined;
    }

    if (controlMapa_PoligonoBarrioSeleccionado !== undefined) {
        controlMapa_PoligonoBarrioSeleccionado.setMap(undefined);
        controlMapa_PoligonoBarrioSeleccionado = undefined;
    }

    if (controlMapa_PoligonoCpcSeleccionado !== undefined) {
        controlMapa_PoligonoCpcSeleccionado.setMap(undefined);
        controlMapa_PoligonoCpcSeleccionado = undefined;
    }

    $('#ControlMapa_InputBuscar').val('');
    $('#ControlMapa_ContenedorSugerencias').empty();
    $('#ControlMapa_ContenedorSugerencias').removeClass('visible');

    //Informo
    ControlMapa_Listener_Limpiar();
}

function ControlMapa_ResaltarCpc(id) {
    let cpc = $.grep(controlMapa_Cpcs, function (element, index) { return element.id === id })[0];
    let poligonoCpc = cpc.poligono;
    if (cpc == undefined) return;

    //Valido    
    if (ControlMapa_Listener_ValidarResaltarCpc(cpc) === false) return;


    if (controlMapa_PoligonoCpcSeleccionado !== undefined) {
        controlMapa_PoligonoCpcSeleccionado.setMap(null);
        controlMapa_PoligonoCpcSeleccionado = undefined;
    }

    controlMapa_PoligonoCpcSeleccionado = new google.maps.Polygon({
        paths: poligonoCpc,
        strokeColor: controlMapa_ColorEjido,
        strokeOpacity: controlMapa_StrokeOpacity,
        strokeWeight: controlMapa_StrokeWidth,
        fillColor: controlMapa_ColorCpc,
        fillOpacity: controlMapa_FillOpacity,
        clickable: false,
        zIndex: controlMapa_ZIndexCpc + 100,
        map: map
    });

    //Informo
    ControlMapa_Listener_CpcResaltado(controlMapa_PoligonoCpcSeleccionado, cpc);
}

function ControlMapa_ResaltarBarrio(id) {
    let barrio = $.grep(controlMapa_Barrios, function (element, index) { return element.id === id })[0];
    if (barrio == undefined) return;
    let poligonoBarrio = barrio.poligono;

    //Valido
    if (ControlMapa_Listener_ValidarResaltarBarrio(barrio) === false) return;

    if (controlMapa_PoligonoBarrioSeleccionado !== undefined) {
        controlMapa_PoligonoBarrioSeleccionado.setMap(null);
        controlMapa_PoligonoBarrioSeleccionado = undefined;
    }

    controlMapa_PoligonoBarrioSeleccionado = new google.maps.Polygon({
        paths: poligonoBarrio,
        strokeColor: controlMapa_ColorEjido,
        strokeOpacity: controlMapa_StrokeOpacity,
        strokeWeight: controlMapa_StrokeWidth,
        fillColor: controlMapa_ColorBarrio,
        fillOpacity: controlMapa_FillOpacity,
        clickable: false,
        zIndex: controlMapa_ZIndexBarrio + 100,
        map: map,

    });

    ControlMapa_Listener_BarrioResaltado(controlMapa_PoligonoBarrioSeleccionado, barrio);
}

function ControlMapa_CargarSugerencias(data) {
    $('#ControlMapa_ContenedorSugerencias').empty();

    console.log(data);

    if (data.Cpcs.length === 0 && data.Barrios.length === 0 && data.Items.length === 0) {
        let html = ControlMapa_CrearHtmlSinSugerencia();
        let htmlUsuario = ControlMapa_Listener_CrearHtmlSinSugerencia();
        if (htmlUsuario != undefined) {
            html = htmlUsuario;
        }

        //Agrego
        $('#ControlMapa_ContenedorSugerencias').append(html);
    } else {

        //Items
        $.each(data.Items, function (index, element) {
            let html = ControlMapa_CrearHtmlSugerencia(element);
            let htmlUsuario = ControlMapa_Listener_CrearHtmlSugerencia(element);
            if (htmlUsuario != undefined) {
                html = htmlUsuario;
            }

            //Click
            $(html).click(function () {
                ControlMapa_Limpiar();

                if (ControlMapa_Listener_SugerenciaClick(element) === false) return;

                if (!controlMapa_ResaltarAlHacerClick) {
                    ControlMapa_MostrarMarcador(element, true);
                } else {
                    ControlMapa_MostrarMarcador(element);

                    var bounds = new google.maps.LatLngBounds();

                    let x = parseFloat(element.Latitud.replace(',', '.'));
                    let y = parseFloat(element.Longitud.replace(',', '.'));
                    let pos = { lat: x, lng: y };
                    bounds.extend(pos);

                    if (element.Barrio != undefined) {
                        ControlMapa_ResaltarBarrio(element.Barrio.IdCatastro);

                        let p = $.grep(controlMapa_Barrios, function (e1, i1) { return e1.id == element.Barrio.IdCatastro })[0].poligono;
                        for (var i = 0; i < p.length; i++) {
                            var point = new google.maps.LatLng(p[i].lat, p[i].lng);
                            bounds.extend(point);
                        }
                    }

                    if (element.Cpc != undefined) {
                        ControlMapa_ResaltarCpc(element.Cpc.IdCatastro);

                        let p = $.grep(controlMapa_Cpcs, function (e1, i1) { return e1.id == element.Cpc.IdCatastro })[0].poligono;
                        for (var i = 0; i < p.length; i++) {
                            var point = new google.maps.LatLng(p[i].lat, p[i].lng);
                            bounds.extend(point);
                        }
                    }

                    map.fitBounds(bounds);
                }

            });

            if (index > 2) return;

            //Agrego
            $('#ControlMapa_ContenedorSugerencias').append(html);
        });

        //Barrios
        $.each(data.Barrios, function (index, element) {
            let html = ControlMapa_CrearHtmlSugerenciaBarrio(element);
            let htmlUsuario = ControlMapa_Listener_CrearHtmlSugerenciaBarrio(element);
            if (htmlUsuario != undefined) {
                html = htmlUsuario;
            }

            //Click
            $(html).click(function () {
                ControlMapa_Limpiar();

                if (ControlMapa_Listener_SugerenciaBarrioClick(element) === false) return;
                if (!controlMapa_ResaltarAlHacerClick) return;

                ControlMapa_ResaltarBarrio(element.id);

                if (controlMapa_Marcador !== undefined && controlMapa_Marcador.getMap() !== undefined) {
                    controlMapa_Marcador.setMap(undefined);
                    controlMapa_Marcador = undefined;
                }

                if (controlMapa_InfoWindows !== undefined) {
                    controlMapa_InfoWindows.setMap(undefined);
                    controlMapa_InfoWindows = undefined;
                }

                if (controlMapa_PoligonoCpcSeleccionado !== undefined) {
                    controlMapa_PoligonoCpcSeleccionado.setMap(undefined);
                    controlMapa_PoligonoCpcSeleccionado = undefined;
                }

                var bounds = new google.maps.LatLngBounds();
                for (var i = 0; i < element.poligono.length; i++) {
                    var point = new google.maps.LatLng(element.poligono[i].lat, element.poligono[i].lng);
                    bounds.extend(point);
                }
                map.fitBounds(bounds);
            });

            if (index++ > 2) return;

            //Agrego
            $('#ControlMapa_ContenedorSugerencias').append(html);
        });

        //Cpcs
        $.each(data.Cpcs, function (index, element) {
            let html = ControlMapa_CrearHtmlSugerenciaCpc(element);
            let htmlUsuario = ControlMapa_Listener_CrearHtmlSugerenciaCpc(element);
            if (htmlUsuario != undefined) {
                html = htmlUsuario;
            }

            //Click
            $(html).click(function () {
                ControlMapa_Limpiar();

                if (ControlMapa_Listener_SugerenciaCpcClick(element) === false) return;
                if (!controlMapa_ResaltarAlHacerClick) return;

                ControlMapa_ResaltarCpc(element.id);

                if (controlMapa_Marcador !== undefined && controlMapa_Marcador.getMap() !== undefined) {
                    controlMapa_Marcador.setMap(undefined);
                    controlMapa_Marcador = undefined;
                }

                if (controlMapa_InfoWindows !== undefined) {
                    controlMapa_InfoWindows.setMap(undefined);
                    controlMapa_InfoWindows = undefined;
                }

                if (controlMapa_PoligonoBarrioSeleccionado !== undefined) {
                    controlMapa_PoligonoBarrioSeleccionado.setMap(undefined);
                    controlMapa_PoligonoBarrioSeleccionado = undefined;
                }

                var bounds = new google.maps.LatLngBounds();
                for (var i = 0; i < element.poligono.length; i++) {
                    var point = new google.maps.LatLng(element.poligono[i].lat, element.poligono[i].lng);
                    bounds.extend(point);
                }
                map.fitBounds(bounds);
            });

            if (index > 2) return;

            //Agrego
            $('#ControlMapa_ContenedorSugerencias').append(html);
        });
    }

    $('#ControlMapa_ContenedorSugerencias').addClass('visible');
}

function ControlMapa_CrearHtmlSinSugerencia() {
    let div = $($('#ControlMapa_TemplateSugerencia').html());
    $(div).find('> .material-icons').text('info_outline');
    $(div).find('> .textos > .texto2').text('No hay resultados para su búsqueda');
    $(div).removeClass('clickable');
    return div;
}

function ControlMapa_CrearHtmlSugerencia(data) {
    let div = $($('#ControlMapa_TemplateSugerencia').html());
    if (data.Observaciones != undefined) {
        $(div).find('> .material-icons').text('place');
        $(div).find('> .textos > .texto1').text(data.Observaciones);
        $(div).find('> .textos > .texto2').text(data.Direccion);
    } else {
        $(div).find('> .material-icons').text('timeline');
        $(div).find('> .textos > .texto1').text('Calle');
        $(div).find('> .textos > .texto2').text(data.Direccion);

    }
    return div;
}

function ControlMapa_CrearHtmlSugerenciaCpc(data) {
    let div = $($('#ControlMapa_TemplateSugerencia').html());
    $(div).find('> .material-icons').text('account_balance');
    $(div).find('> .textos > .texto1').text('CPC');
    $(div).find('> .textos > .texto2').text('N° ' + data.numero + ' - ' + data.nombre);
    return div;
}

function ControlMapa_CrearHtmlSugerenciaBarrio(data) {
    let div = $($('#ControlMapa_TemplateSugerencia').html());
    $(div).find('> .material-icons').text('business');
    $(div).find('> .textos > .texto1').text('Barrio');
    $(div).find('> .textos > .texto2').text(data.nombre);
    return div;
}



//--------------------------------------------------
// LISTENERS
//--------------------------------------------------


//Basicos

let controlMapa_Listener_MapReady;
let controlMapa_Listener_Cargando;
let controlMapa_Listener_Limpiar;
let controlMapa_Listener_ValidarLimpiar;

function ControlMapa_Listener_MapReady(map) {
    if (controlMapa_Listener_MapReady == undefined) return;
    controlMapa_Listener_MapReady(map);
}

function ControlMapa_SetListenerMapReady(callback) {
    controlMapa_Listener_MapReady = callback;
}

function ControlMapa_Listener_Cargando(cargando) {
    if (controlMapa_Listener_Cargando == undefined) return;
    controlMapa_Listener_Cargando(cargando);
}

function ControlMapa_SetListenerCargando(callback) {
    controlMapa_Listener_Cargando = callback;
}

function ControlMapa_Listener_ValidarLimpiar() {
    if (controlMapa_Listener_ValidarLimpiar == undefined) return;
    controlMapa_Listener_ValidarLimpiar();
}

function ControlMapa_SetListenerValidarLimpiar(callback) {
    controlMapa_Listener_ValidarLimpiar = callback;
}

function ControlMapa_Listener_Limpiar(cargando) {
    if (controlMapa_Listener_Limpiar == undefined) return;
    controlMapa_Listener_Limpiar(cargando);
}

function ControlMapa_SetListenerLimpiar(callback) {
    controlMapa_Listener_Limpiar = callback;
}



//Mapa

let controlMapa_Listener_MapaClick;
let controlMapa_Listener_ValidarMarcador;
let controlMapa_Listener_Marcador;
let controlMapa_Listener_MarcadorIcono;
let controlMapa_Listener_MarcadorPopup;
let controlMapa_Listener_ValidarResaltarCpc;
let controlMapa_Listener_CpcResaltado;
let controlMapa_Listener_ValidarResaltarBarrio;
let controlMapa_Listener_BarrioResaltado;


function ControlMapa_Listener_MapaClick(ev) {
    if (controlMapa_Listener_MapaClick == undefined) return;
    return controlMapa_Listener_MapaClick(ev);
}

function ControlMapa_SetListenerMapaClick(callback) {
    controlMapa_Listener_MapaClick = callback;
}

function ControlMapa_Listener_ValidarMarcador(info) {
    if (controlMapa_Listener_ValidarMarcador == undefined) return;
    return controlMapa_Listener_ValidarMarcador(info);
}

function ControlMapa_SetListenerValidarMarcador(callback) {
    controlMapa_Listener_ValidarMarcador = callback;
}

function ControlMapa_Listener_Marcador(marcador, info) {
    if (controlMapa_Listener_Marcador == undefined) return;
    return controlMapa_Listener_Marcador(marcador, info);
}

function ControlMapa_SetListenerMarcador(callback) {
    controlMapa_Listener_Marcador = callback;
}

function ControlMapa_Listener_MarcadorIcono(marcador) {
    if (controlMapa_Listener_MarcadorIcono == undefined) return;
    return controlMapa_Listener_MarcadorIcono(marcador);
}

function ControlMapa_SetListenerMarcadorIcono(callback) {
    controlMapa_Listener_MarcadorIcono = callback;
}

function ControlMapa_Listener_MarcadorPopup(marcador) {
    if (controlMapa_Listener_MarcadorPopup == undefined) return;
    return controlMapa_Listener_MarcadorPopup(marcador);
}

function ControlMapa_SetListenerMarcadorPopup(callback) {
    controlMapa_Listener_MarcadorPopup = callback;
}

function ControlMapa_Listener_ValidarResaltarCpc(info) {
    if (controlMapa_Listener_ValidarResaltarCpc == undefined) return;
    return controlMapa_Listener_ValidarResaltarCpc(info);
}

function ControlMapa_SetListenerValidarResaltarCpc(callback) {
    controlMapa_Listener_ValidarResaltarCpc = callback;
}

function ControlMapa_Listener_CpcResaltado(poligono, info) {
    if (controlMapa_Listener_CpcResaltado == undefined) return;
    return controlMapa_Listener_CpcResaltado(poligono, info);
}

function ControlMapa_SetListenerCpcResaltado(callback) {
    controlMapa_Listener_CpcResaltado = callback;
}

function ControlMapa_Listener_ValidarResaltarBarrio(info) {
    if (controlMapa_Listener_ValidarResaltarBarrio == undefined) return;
    return controlMapa_Listener_ValidarResaltarBarrio(info);
}

function ControlMapa_SetListenerValidarResaltarBarrio(callback) {
    controlMapa_Listener_ValidarResaltarBarrio = callback;
}

function ControlMapa_Listener_BarrioResaltado(poligono, info) {
    if (controlMapa_Listener_BarrioResaltado == undefined) return;
    return controlMapa_Listener_BarrioResaltado(poligono, info);
}

function ControlMapa_SetListenerCpcResaltado(callback) {
    controlMapa_Listener_BarrioResaltado = callback;
}



//Botones
let controlMapa_Listener_BotonCpcs;
let controlMapa_Listener_BotonBarrios;
let controlMapa_Listener_BotonEjido;
let controlMapa_Listener_BotonFullscreen;

function ControlMapa_Listener_BotonCpcs(valor) {
    if (controlMapa_Listener_BotonCpcs == undefined) return;
    controlMapa_Listener_BotonCpcs(valor);
}

function ControlMapa_SetListenerBotonCpcs(callback) {
    controlMapa_Listener_BotonCpcs = callback;
}

function ControlMapa_Listener_BotonBarrios(valor) {
    if (controlMapa_Listener_BotonBarrios == undefined) return;
    controlMapa_Listener_BotonBarrios(valor);
}

function ControlMapa_SetListenerBotonBarrios(callback) {
    controlMapa_Listener_BotonBarrios = callback;
}

function ControlMapa_Listener_BotonEjido(valor) {
    if (controlMapa_Listener_BotonEjido == undefined) return;
    controlMapa_Listener_BotonEjido(valor);
}

function ControlMapa_SetListenerBotonEjido(callback) {
    controlMapa_Listener_BotonEjido = callback;
}

function ControlMapa_Listener_BotonFullscreen(valor) {
    if (controlMapa_Listener_BotonFullscreen == undefined) return;
    controlMapa_Listener_BotonFullscreen(valor);
}

function ControlMapa_SetListenerBotonFullscreen(callback) {
    controlMapa_Listener_BotonFullscreen = callback;
}


//Busqueda

let controlMapa_Listener_InputBusquedaKeyDown;
let controlMapa_Listener_InputBusquedaChange;
let controlMapa_Listener_ValidarBusqueda;
let controlMapa_Listener_CorregirBusqueda;

function ControlMapa_Listener_InputBusquedaKeyDown(key, busqueda) {
    if (controlMapa_Listener_InputBusquedaKeyDown == undefined) return;
    controlMapa_Listener_InputBusquedaKeyDown(key, busqueda);
}

function ControlMapa_SetListenerInputBusquedaKeyDown(callback) {
    controlMapa_Listener_InputBusquedaKeyDown = callback;
}

function ControlMapa_Listener_InputBusquedaChange(busqueda) {
    if (controlMapa_Listener_InputBusquedaChange == undefined) return;
    controlMapa_Listener_InputBusquedaChange(busqueda);
}

function ControlMapa_SetListenerInputBusquedaChange(callback) {
    controlMapa_Listener_InputBusquedaChange = callback;
}

function ControlMapa_Listener_ValidarBusqueda(busqueda) {
    if (controlMapa_Listener_ValidarBusqueda == undefined) return;
    return controlMapa_Listener_ValidarBusqueda(busqueda);
}

function ControlMapa_SetListenerValidarBusqueda(callback) {
    controlMapa_Listener_ValidarBusqueda = callback;
}

function ControlMapa_Listener_CorregirBusqueda(busqueda) {
    if (controlMapa_Listener_CorregirBusqueda == undefined) return;
    return controlMapa_Listener_CorregirBusqueda(busqueda);
}

function ControlMapa_SetListenerCorregirBusqueda(callback) {
    controlMapa_Listener_CorregirBusqueda = callback;
}


//Sugerencias

let controlMapa_Listener_CrearHtmlSinSugerencia;
let controlMapa_Listener_CrearHtmlSugerencia;
let controlMapa_Listener_CrearHtmlSugerenciaBarrio;
let controlMapa_Listener_CrearHtmlSugerenciaCpc;
let controlMapa_Listener_FiltrarSugerencias;
let controlMapa_Listener_Sugerencias;
let controlMapa_Listener_SugerenciaClick;
let controlMapa_Listener_SugerenciaBarrioClick;
let controlMapa_Listener_SugerenciaCpcClick;


function ControlMapa_Listener_CrearHtmlSinSugerencia() {
    if (controlMapa_Listener_CrearHtmlSinSugerencia == undefined) return undefined;
    return controlMapa_Listener_CrearHtmlSinSugerencia();
}

function ControlMapa_Listener_CrearHtmlSugerencia(data) {
    if (controlMapa_Listener_CrearHtmlSugerencia == undefined) return undefined;
    return controlMapa_Listener_CrearHtmlSugerencia(data);
}

function ControlMapa_SetListenerCrearHtmlSugerencia(callback) {
    controlMapa_Listener_CrearHtmlSugerencia = callback;
}

function ControlMapa_Listener_CrearHtmlSugerenciaBarrio(data) {
    if (controlMapa_Listener_CrearHtmlSugerenciaBarrio == undefined) return undefined;
    return controlMapa_Listener_CrearHtmlSugerenciaBarrio(data);

}

function ControlMapa_SetListenerCrearHtmlSugerenciaBarrio(callback) {
    controlMapa_Listener_CrearHtmlSugerenciaBarrio = callback;
}

function ControlMapa_Listener_CrearHtmlSugerenciaCpc(data) {
    if (controlMapa_Listener_CrearHtmlSugerenciaCpc == undefined) return undefined;
    return controlMapa_Listener_CrearHtmlSugerenciaCpc(data);
}

function ControlMapa_SetListenerCrearHtmlSugerenciaCpc(callback) {
    controlMapa_Listener_CrearHtmlSugerenciaCpc = callback;
}

function ControlMapa_Listener_FiltrarSugerencias(sugerencias) {
    if (controlMapa_Listener_FiltrarSugerencias == undefined) return;
    return controlMapa_Listener_FiltrarSugerencias(sugerencias);
}

function ControlMapa_SetListenerFiltrarSugerencias(callback) {
    controlMapa_Listener_FiltrarSugerencias = callback;
}

function ControlMapa_Listener_Sugerencias(sugerencias) {
    if (controlMapa_Listener_Sugerencias == undefined) return;
    return controlMapa_Listener_Sugerencias(sugerencias);
}

function ControlMapa_SetListenerSugerencias(callback) {
    controlMapa_Listener_Sugerencias = callback;
}

function ControlMapa_Listener_SugerenciaClick(info) {
    if (controlMapa_Listener_SugerenciaClick == undefined) return;
    return controlMapa_Listener_SugerenciaClick(info);
}

function ControlMapa_SetListenerSugerenciaClick(callback) {
    controlMapa_Listener_SugerenciaClick = callback;
}

function ControlMapa_Listener_SugerenciaBarrioClick(info) {
    if (controlMapa_Listener_SugerenciaBarrioClick == undefined) return;
    return controlMapa_Listener_SugerenciaBarrioClick(info);
}

function ControlMapa_SetListenerSugerenciaBarrioClick(callback) {
    controlMapa_Listener_SugerenciaBarrioClick = callback;
}


function ControlMapa_Listener_SugerenciaCpcClick(info) {
    if (controlMapa_Listener_SugerenciaCpcClick == undefined) return;
    return controlMapa_Listener_SugerenciaCpcClick(info);
}

function ControlMapa_SetListenerSugerenciaCpcClick(callback) {
    controlMapa_Listener_SugerenciaCpcClick = callback;
}