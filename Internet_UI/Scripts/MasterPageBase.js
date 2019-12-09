$(function () {
    $('body').removeClass('oculto');

    var selectLogo = "#contenedor-encabezado .logo, #contenedor-encabezado .nombre-muni";
    $(selectLogo).click(function () {
        redirigir(ResolveUrl('~/Login?sinSaludo=true'));
    });
});

function redirigir(pagina) {
    $('body').addClass('oculto');
    setTimeout(function () {
        var urlNueva = pagina
        window.location.href = urlNueva;
    }, 300);
}

function mostrarMensaje(tipo, mensaje) {

    var _event = {
        TIMEOUT: 5000,

        mostrado: function () {
            window.dispatchEvent(new CustomEvent("mensajeMostrado"));
        },

        ocultado: function () {
            window.dispatchEvent(new CustomEvent("mensajeOcultado"));
        }
    }

    switch (tipo.toLowerCase()) {
        case 'error': {
            Materialize.toast(mensaje, _event.TIMEOUT, 'colorError');
        } break;

        case 'info': {
            Materialize.toast(mensaje, _event.TIMEOUT);
        } break;

        case 'alert': {
            Materialize.toast(mensaje, _event.TIMEOUT, 'colorAlerta');
        } break;

        case 'alerta': {
            Materialize.toast(mensaje, _event.TIMEOUT, 'colorAlerta');
        } break;
        case 'exito': {
            Materialize.toast(mensaje, _event.TIMEOUT, 'colorExito');
        } break;
    }

    _event.mostrado();
    setTimeout(function () {
        _event.ocultado();
    }, _event.TIMEOUT);
}


function mostrarPanelMensaje(valores) {

    $('#mensaje').addClass('visible');

    $('#mensaje .texto').css('opacity', 0);
    $('#mensaje .texto').html(valores.Mensaje);

    $('#btnMensaje').css('opacity', 0);
    $('#btnMensaje').css('pointer-events', 'none');
    $('#btnMensaje').text(valores.BotonMensaje);
    $('#btnMensaje').off('click');

    $('#mensaje .material-icons').css('opacity', 0);
    $('#mensaje .material-icons').html(valores.Icono);
    $('#mensaje .material-icons').css('color', valores.IconoColor);

    setTimeout(function () {
        $('#mensaje .texto').animate({ 'opacity': 1 }, 300);
        $('#mensaje .material-icons').animate({ 'opacity': 1 }, 300);

        if ('BotonVisible' in valores && valores.BotonVisible) {
            $('#btnMensaje').animate({ 'opacity': 1 }, 300);
            $('#btnMensaje').css('pointer-events', 'all');

            if (valores.OnClick != undefined) {
                $('#btnMensaje').click(function () {
                    valores.OnClick();
                });
            }
        }
    }, 100);




}

function ocultarPanelMensaje() {
    $('#mensaje').removeClass('visible');
}

function mostrarCargando(texto) {
    $('#indicador-cargando').stop(false, false).fadeIn(300);
    if (texto == undefined) {
        texto = "Cargando...";
    }
    $('#indicador-cargando label').text(texto);
}

function ocultarCargando() {
    $('#indicador-cargando').stop(false, false).fadeOut(300);
}