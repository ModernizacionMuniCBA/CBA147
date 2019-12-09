let requerimiento;
let mapa;


$(function () {
    $('.btn-volver').click(function () {
        redirigir('Inicio');
    });
})


// Init
function init(data) {
    if (verificarError(data) == true) return;

    requerimiento = data.Requerimiento;

    initDescripcion();
    initEstado();
    initUbicacion();
    initImagenes();
    initAreaEncargada();

    initCancelarRequerimiento();
    initCancelarRequerimientoEvent();
}


// Descripcion
function initDescripcion() {
    $('._Numero').html('N° ' + requerimiento.Numero + '/' + requerimiento.Año);
    $('._Servicio').html(toTitleCase(requerimiento.ServicioNombre || 'Sin datos'));
    $('._Motivo').html(toTitleCase(requerimiento.MotivoNombre || 'Sin datos'));
    $('._Descripcion').html(requerimiento.Descripcion || 'Sin datos');
}


// Estado
function initEstado() {
    let estadoColor = '#' + (requerimiento.EstadoPublicoColor || requerimiento.EstadoColor || '000');
    let estadoNombre = toTitleCase(requerimiento.EstadoPublicoNombre || requerimiento.EstadoNombre || 'Sin datos').trim();

    $('._EstadoColor').css('color', estadoColor);
    $('._EstadoNombre').text(estadoNombre);
    $('._EstadoObservacion').text(requerimiento.EstadoObservaciones);

    if (requerimiento.EstadoFecha != undefined) {
        let date = requerimiento.EstadoFecha.split('-');
        $('._EstadoFecha').html(date[2].substring(0, 2) + '/' + date[1] + '/' + date[0]);
    } else {
        $('._EstadoFecha').html('Sin datos');
    }
}


// Ubicación
function initUbicacion() {
    if (requerimiento.DomicilioLatitud != undefined && requerimiento.DomicilioLongitud != undefined) {
        requerimiento.DomicilioLatitud = requerimiento.DomicilioLatitud.replace(',', '.');
        requerimiento.DomicilioLongitud = requerimiento.DomicilioLongitud.replace(',', '.');

        let mapa = 'https://maps.googleapis.com/maps/api/staticmap?center=&zoom=16&scale=1&size=600x600&maptype=roadmap&format=png&visual_refresh=true&markers=size:mid%7Ccolor:0xff0000%7Clabel:1%7C' + requerimiento.DomicilioLatitud + ',' + requerimiento.DomicilioLongitud + '&key=""';
        $('.mapa').css('background-image', 'url(' + mapa + ')');
        $('._Direccion').html(toTitleCase(requerimiento.DomicilioDireccion || 'Sin datos'));
        $('._Observaciones').html(requerimiento.DomicilioObservaciones || 'Sin datos');
        $('._Cpc').html(toTitleCase(requerimiento.DomicilioCpcNumero + '° ' + requerimiento.DomicilioCpcNombre));
        $('._Barrio').html(toTitleCase(requerimiento.DomicilioBarrioNombre || 'Sin datos'));

        $('.mapa, #btn_AbrirMapa').click(function () {
            url = "https://www.google.com.sa/maps/search/" + requerimiento.DomicilioLatitud + ',' + requerimiento.DomicilioLongitud;
            window.open(url, '_blank');
        });
    }
}


//Imagenes
let index = undefined;

function initImagenes() {
    if (requerimiento.Fotos != undefined && requerimiento.Fotos.length != 0) {
        $('#contenedor_Imagenes').show();
        $('#contenedor_Imagenes .content-body').empty();
        $.each(requerimiento.Fotos, function (index, element) {
            let html = $($('#template_Imagen').html());

            let urlMinitatura = urlCordobaFiles + '/Archivo/' + element + '/3';

            $(html).css('background-image', 'url(' + urlMinitatura + ')');
            $('#contenedor_Imagenes .content-body').append(html);

            $(html).click(function () {
                verFoto(index);
            });
        })
    } else {
        $('#contenedor_Imagenes').hide();
    }


    function verFoto(i) {
        let url = urlCordobaFiles + '/Archivo/' + requerimiento.Fotos[i];
        index = i;
        $('#visor_Foto').addClass('visible');
        $('#visor_Foto .foto').css('background-image', 'url(' + url + ')');
    }

    function cerrar() {
        index = undefined;
        $('#visor_Foto').removeClass('visible');
    }

    $('#visor_Foto .btn').click(function () {
        cerrar();
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            if ($('#visor_Foto').hasClass('visible')) {
                cerrar();
                e.preventDefault();
                return;
            }
            return;
        }

        //Arrow left
        if (e.keyCode == 37) {
            if ($('#visor_Foto').hasClass('visible')) {
                if (index > 0) {
                    verFoto(index - 1);
                }
                e.preventDefault();
                return;
            }
            return;
        }

        //Arrow right
        if (e.keyCode == 39) {
            if ($('#visor_Foto').hasClass('visible')) {
                if (index < requerimiento.Fotos.length - 1) {
                    verFoto(index + 1);
                }
                e.preventDefault();
                return;
            }
            return;
        }
    });
}


// Area encargada
function initAreaEncargada() {
    $('._OrganicaSecretaria').html(toTitleCase(requerimiento.InformacionOrganicaSecretariaNombre || 'Sin datos'));
    $('._OrganicaDireccion').html(toTitleCase(requerimiento.InformacionOrganicaDireccionNombre || 'Sin datos'));
    $('._OrganicaArea').html(toTitleCase(requerimiento.InformacionOrganicaAreaNombre || 'Sin datos'));
    $('._OrganicaDomicilio').html(toTitleCase(requerimiento.InformacionOrganicaDomicilio || 'Sin datos'));
}


// Acciones
let emailEnviado = false;

function initCancelarRequerimiento() {
    if (requerimiento.EstadoKeyValue !== 1) {
        $('.btn-cancelar').hide();
    }
}

function initCancelarRequerimientoEvent() {
    $('.btn-reenviar').click(reenviarRequerimiento);
    $('.btn-cancelar').click(cancelarRequerimiento);
}

function reenviarRequerimiento() {
    if (emailEnviado) {
        mostrarMensaje('Se ha enviado a su e-mail el comprobante del requerimiento');
    } else {
        mostrarCargando(true);

        ajaxReenviarRequerimiento()
            .then(function () {
                emailEnviado = true;
                mostrarMensaje('Se ha enviado a su e-mail el comprobante del requerimiento');
                mostrarCargando(false);
            })
            .catch(function (error) {
                mostrarCargando(false);
                mostrarMensaje(error);
            });
    }
}

function ajaxReenviarRequerimiento() {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioRequerimiento.asmx/EnviarEmailComprobante'),
            Data: { id: requerimiento.Id },
            OnSuccess: function (result) {
                if (result.Ok != true) {
                    callbackError(result.Error || 'Error procesando la solicitud');
                    return;
                }

                callback();
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        })
    });
}

function cancelarRequerimiento() {
    mostrarCargando(true);

    ajaxCancelarRequerimiento()
        .then(function () {
            redirigir('DetalleRequerimiento?id=' + requerimiento.Id, { texto: 'Requerimiento cancelado correctamente', tipo: 'exito' });
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarMensaje(error);
        });
}

function ajaxCancelarRequerimiento() {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioRequerimiento.asmx/Cancelar'),
            Data: { id: requerimiento.Id },
            OnSuccess: function (result) {
                if (result.Ok != true) {
                    callbackError(result.Error || 'Error procesando la solicitud');
                    return;
                }

                callback();
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        })
    });
}