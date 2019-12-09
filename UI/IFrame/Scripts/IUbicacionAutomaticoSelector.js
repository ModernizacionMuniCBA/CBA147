const URL_BUSCAR = '~/Servicios/DomicilioService.asmx/Buscar';
const URL_SUGERIR = '~/Servicios/DomicilioService.asmx/Sugerir';

let ubicacionAutomaticaSelector_CallbackCargando;

let ubicacionAutomaticaSelector_Barrios;
let ubicacionAutomaticaSelector_Cpcs;
let ubicacionAutomaticaSelector_Ejido;

let ubicacionAutomaticaSelector_IncluirCpcsEnSugerencias;
let ubicacionAutomaticaSelector_IncluirBarriosEnSugerencias;

function init(valores) {
    $('.tooltipped').tooltip();
    if (valores === undefined) valores = {};

    if (!('CpcEnSugerencias' in valores)) valores.CpcEnSugerencias = true;
    ubicacionAutomaticaSelector_IncluirCpcsEnSugerencias = valores.CpcEnSugerencias;

    if (!('BarrioEnSugerencias' in valores)) valores.BarrioEnSugerencias = true;
    ubicacionAutomaticaSelector_IncluirBarriosEnSugerencias = valores.BarrioEnSugerencias;

    if ('OnCargando' in valores) ubicacionAutomaticaSelector_Listener_Cargando = valores.OnCargando;
    if ('OnValidarBusqueda' in valores) ubicacionAutomaticaSelector_Listener_ValidarBusqueda = valores.OnValidarBusqueda;
    if ('OnCorregirBusqueda' in valores) ubicacionAutomaticaSelector_Listener_CorregirBusqueda = valores.OnCorregirBusqueda;

    UbicacionAutomaticaSelector_InitBuscar();

    $.getScript("https://maps.googleapis.com/maps/api/js?key=" + KEY_GOOGLE_MAPS + "&libraries=visualization,places", function () {
        getGeoApiInfo()
            .then(function (data) {
                ubicacionAutomaticaSelector_Barrios = data.barrios;
                ubicacionAutomaticaSelector_Cpcs = data.cpcs;
                ubicacionAutomaticaSelector_Ejido = data.ejido;
            }).catch(function (error) {
                console.log(error);
            });
    });

    //inicializo boton de mapa
    $("#UbicacionSelector_BtnMapa").click(function () {
        UbicacionAutomaticaSelector_AbrirMapa();
    });
}

function UbicacionAutomaticaSelector_InitBuscar() {

    $('#UbicacionSelector_InputBuscar').on('keydown', function (e) {
        let key = e.originalEvent.key;
        let busqueda = $(this).val();

        if (e.originalEvent.keyCode === 13) {
            $('#UbicacionSelector_BtnBuscar').trigger("click");
        }
    });

    //$('#UbicacionSelector_InputBuscar').on('input', function () {
    //    let busqueda = $(this).val();

    //});

    $('#UbicacionSelector_BtnBuscar').click(function () {
        let busqueda = $('#UbicacionSelector_InputBuscar').val();

        //Valido la busqueda
        if (UbicacionAutomaticaSelector_Listener_ValidarBusqueda(busqueda) === false) return;

        //Corrijo la busqueda
        let busquedaCorregida = UbicacionAutomaticaSelector_Listener_CorregirBusqueda(busqueda);
        if (busquedaCorregida !== undefined) {
            busqueda = busquedaCorregida;
            $('#UbicacionSelector_InputBuscar').val(busqueda);
            Materialize.updateTextFields();
        }

        //Busco
        UbicacionAutomaticaSelector_MostrarCargando();
        $('#UbicacionSelector_ContenedorSugerencias').removeClass('visible');

        UbicacionAutomaticaSelector_Ajax_BuscarSugerencias(busqueda)
            .then(function (data) {
                //OCulto cargando
                UbicacionAutomaticaSelector_OcultarCargando();

                //Cargo sugerencias
                UbicacionAutomaticaSelector_CargarSugerencias(data);

                //Si uno solo Item click
                if (data.Cpcs.length === 0 && data.Barrios.length === 0 && data.Items.length === 1) {
                    $($('#UbicacionSelector_ContenedorSugerencias .sugerencia')[0]).trigger('click');
                }
            })
            .catch(function (error) {
                UbicacionAutomaticaSelector_OcultarCargando();
            });
    });
}

//-------------------------------
// Utiles
//-------------------------------

function UbicacionAutomaticaSelector_MostrarCargando() {
    $('#UbicacionSelector_InputBuscar').prop('disabled', true);
    $('#UbicacionAutomaticaSelector_ContenedorBuscar > div').addClass('progress');
    UbicacionAutomaticaSelector_Listener_Cargando(true);
}

function UbicacionAutomaticaSelector_OcultarCargando() {
    $('#UbicacionSelector_InputBuscar').prop('disabled', false);
    $('#UbicacionAutomaticaSelector_ContenedorBuscar > div').removeClass('progress');
    UbicacionAutomaticaSelector_Listener_Cargando(false);
}

function UbicacionAutomaticaSelector_Ajax_Buscar(lat, lng) {
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

function UbicacionAutomaticaSelector_Ajax_BuscarSugerencias(consulta) {
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
                if (ubicacionAutomaticaSelector_IncluirBarriosEnSugerencias) {
                    $.each(ubicacionAutomaticaSelector_Barrios, function (index, element) {
                        if (element.nombre.toLowerCase().indexOf(consulta) !== -1) {
                            data.Barrios.push(element);
                        }
                    });
                }

                //Cpcs
                data.Cpcs = [];
                if (ubicacionAutomaticaSelector_IncluirCpcsEnSugerencias) {
                    $.each(ubicacionAutomaticaSelector_Cpcs, function (index, element) {
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

var ubicacionAutomaticaSelector_UbicacionSeleccionada;

function UbicacionAutomaticaSelector_Limpiar() {
    $('#UbicacionSelector_ContenedorBusqueda').show();
    $('#UbicacionSelector_InputBuscar').val('');
    $('#UbicacionSelector_InputBuscar').trigger('focus');
    $('#UbicacionSelector_ContenedorSugerencias').empty();
    $('#UbicacionSelector_ContenedorSugerencias').removeClass('visible');
    $('#UbicacionSelector_ContenedorUbicacionSeleccionada').empty();
    $("#UbicacionSelector_ContenedorObservaciones").hide();
    ubicacionAutomaticaSelector_UbicacionSeleccionada = undefined;
}

function UbicacionAutomaticaSelector_CargarSugerencias(data) {
    $('#UbicacionSelector_ContenedorSugerencias').empty();

    console.log(data);

    if (data.Cpcs.length === 0 && data.Barrios.length === 0 && data.Items.length === 0) {
        let html = UbicacionAutomaticaSelector_CrearHtmlSinSugerencia();

        //Agrego
        $('#UbicacionSelector_ContenedorSugerencias').append(html);
    } else {

        //Items
        $.each(data.Items, function (index, element) {
            let html = UbicacionAutomaticaSelector_CrearHtmlSugerencia(element);
            let htmlUsuario = UbicacionAutomaticaSelector_Listener_CrearHtmlSugerencia(element);
            if (htmlUsuario != undefined) {
                html = htmlUsuario;
            }

            //Click
            $(html).click(function () {
                UbicacionAutomaticaSelector_CargarUbicacionSeleccionada(element);
            });

            //Agrego
            $('#UbicacionSelector_ContenedorSugerencias').append(html);
        });

        //Barrios
        $.each(data.Barrios, function (index, element) {
            let html = UbicacionAutomaticaSelector_CrearHtmlSugerenciaBarrio(element);
            let htmlUsuario = UbicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaBarrio(element);
            if (htmlUsuario != undefined) {
                html = htmlUsuario;
            }

            //Click
            $(html).click(function () {
                let barrios = [];
                barrios.push(element);
                let sugerencias = { Barrios: barrios, Cpcs: [], Items: [] }
                UbicacionAutomaticaSelector_AbrirMapa(sugerencias);
            });

            //Agrego
            $('#UbicacionSelector_ContenedorSugerencias').append(html);
        });

        //Cpcs
        $.each(data.Cpcs, function (index, element) {
            let html = UbicacionAutomaticaSelector_CrearHtmlSugerenciaCpc(element);
            let htmlUsuario = UbicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaCpc(element);
            if (htmlUsuario != undefined) {
                html = htmlUsuario;
            }

            //Click
            $(html).click(function () {
                let cpcs = [];
                cpcs.push(element);
                let sugerencias = { Cpcs: cpcs, Barrios: [], Items: [] }
                UbicacionAutomaticaSelector_AbrirMapa(sugerencias);
            });

            //Agrego
            $('#UbicacionSelector_ContenedorSugerencias').append(html);
        });
    }

    $('#UbicacionSelector_ContenedorSugerencias').addClass('visible');
}

function UbicacionAutomaticaSelector_CargarUbicacionSeleccionada(element) {
    UbicacionAutomaticaSelector_Limpiar();

    ubicacionAutomaticaSelector_UbicacionSeleccionada = element;

    let ubicacionHtml = UbicacionAutomaticaSelector_Listener_CrearHtmlUbicacionSeleccionada(element);

    $('#UbicacionSelector_ContenedorUbicacionSeleccionada').append(ubicacionHtml);
    $('#UbicacionSelector_ContenedorBusqueda').hide();
    $("#UbicacionSelector_ContenedorObservaciones").show();

    if (element.Observaciones != undefined) {
        $("#UbicacionSelector_InputObservaciones").text(element.Observaciones);
    }

    $("#UbicacionSelector_InputObservaciones").focus();
}


function UbicacionAutomaticaSelector_AbrirMapa(sugerencia) {
    crearDialogoUbicacionManualSelector({
        Callback: function (domicilio) {
            UbicacionAutomaticaSelector_CargarUbicacionSeleccionada(domicilio);
        },
        Sugerencia: sugerencia
    });
}

function UbicacionAutomaticaSelector_Listener_CrearHtmlUbicacionSeleccionada(data) {
    let div = $($('#UbicacionSelector_TemplateUbicacionSeleccionada').html());
    $(div).find('.cancelar').click(function () {
        UbicacionAutomaticaSelector_Limpiar();
    });
    $(div).find('.domicilio').text(data.Direccion);
    $(div).find('.barrio').html('<b>Barrio </b>' + data.Barrio.Nombre);
    $(div).find('.cpc').html('<b>CPC </b>' + data.Cpc.Nombre);
    return div;
}

function UbicacionAutomaticaSelector_CrearHtmlSinSugerencia() {
    let div = $($('#UbicacionSelector_TemplateSugerencia').html());
    $(div).find('> .material-icons').text('info_outline');
    $(div).find('> .textos > .texto2').text('No hay resultados para su búsqueda');
    $(div).find('.contenedor-boton').hide();
    $(div).removeClass('clickable');
    return div;
}

function UbicacionAutomaticaSelector_CrearHtmlSugerencia(data) {
    let div = $($('#UbicacionSelector_TemplateSugerencia').html());
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

function UbicacionAutomaticaSelector_CrearHtmlSugerenciaCpc(data) {
    let div = $($('#UbicacionSelector_TemplateSugerencia').html());
    $(div).find('> .material-icons').text('account_balance');
    $(div).find('> .textos > .texto1').text('CPC');
    $(div).find('> .textos > .texto2').text('N° ' + data.numero + ' - ' + data.nombre);
    return div;
}

function UbicacionAutomaticaSelector_CrearHtmlSugerenciaBarrio(data) {
    let div = $($('#UbicacionSelector_TemplateSugerencia').html());
    $(div).find('> .material-icons').text('business');
    $(div).find('> .textos > .texto1').text('Barrio');
    $(div).find('> .textos > .texto2').text(data.nombre);
    return div;
}

function UbicacionAutomaticaSelector_Seleccionar() {
    if (ubicacionAutomaticaSelector_UbicacionSeleccionada == undefined) {
        top.mostrarMensaje('Error', 'Seleccione una ubicación validando con el botón buscar');
        $('#ControlMapa_InputBuscar').focus();
        return undefined;
    }

    if (ubicacionAutomaticaSelector_UbicacionSeleccionada.Observaciones == undefined || ubicacionAutomaticaSelector_UbicacionSeleccionada.Observaciones == "") {
        ubicacionAutomaticaSelector_UbicacionSeleccionada.Observaciones = $('#UbicacionSelector_InputObservaciones').val();
    }

    return ubicacionAutomaticaSelector_UbicacionSeleccionada;
}


//--------------------------------------------------
// LISTENERS
//--------------------------------------------------


//Basicos
let ubicacionAutomaticaSelector_Listener_Cargando;

function UbicacionAutomaticaSelector_Listener_Cargando(cargando) {
    if (ubicacionAutomaticaSelector_Listener_Cargando == undefined) return;
    ubicacionAutomaticaSelector_Listener_Cargando(cargando);
}

function UbicacionAutomaticaSelector_SetListenerCargando(callback) {
    ubicacionAutomaticaSelector_Listener_Cargando = callback;
}

//Busqueda
let ubicacionAutomaticaSelector_Listener_ValidarBusqueda;
let ubicacionAutomaticaSelector_Listener_CorregirBusqueda;

function UbicacionAutomaticaSelector_Listener_ValidarBusqueda(busqueda) {
    if (ubicacionAutomaticaSelector_Listener_ValidarBusqueda == undefined) return;
    return ubicacionAutomaticaSelector_Listener_ValidarBusqueda(busqueda);
}

function UbicacionAutomaticaSelector_SetListenerValidarBusqueda(callback) {
    ubicacionAutomaticaSelector_Listener_ValidarBusqueda = callback;
}

function UbicacionAutomaticaSelector_Listener_CorregirBusqueda(busqueda) {
    if (ubicacionAutomaticaSelector_Listener_CorregirBusqueda == undefined) return;
    return ubicacionAutomaticaSelector_Listener_CorregirBusqueda(busqueda);
}

function UbicacionAutomaticaSelector_SetListenerCorregirBusqueda(callback) {
    ubicacionAutomaticaSelector_Listener_CorregirBusqueda = callback;
}


//Sugerencias
let ubicacionAutomaticaSelector_Listener_CrearHtmlSugerencia;
let ubicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaBarrio;
let ubicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaCpc;

function UbicacionAutomaticaSelector_Listener_CrearHtmlSugerencia(data) {
    if (ubicacionAutomaticaSelector_Listener_CrearHtmlSugerencia == undefined) return undefined;
    return ubicacionAutomaticaSelector_Listener_CrearHtmlSugerencia(data);
}

function UbicacionAutomaticaSelector_SetListenerCrearHtmlSugerencia(callback) {
    ubicacionAutomaticaSelector_Listener_CrearHtmlSugerencia = callback;
}

function UbicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaBarrio(data) {
    if (ubicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaBarrio == undefined) return undefined;
    return ubicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaBarrio(data);

}

function UbicacionAutomaticaSelector_SetListenerCrearHtmlSugerenciaBarrio(callback) {
    ubicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaBarrio = callback;
}

function UbicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaCpc(data) {
    if (ubicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaCpc == undefined) return undefined;
    return ubicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaCpc(data);
}

function UbicacionAutomaticaSelector_SetListenerCrearHtmlSugerenciaCpc(callback) {
    ubicacionAutomaticaSelector_Listener_CrearHtmlSugerenciaCpc = callback;
}
