$(document).ready(function () {
    $('body').removeClass('oculto');
});

//Overlay

function overlay(valores) {
    if (valores.Mostrar) {
        mostrarOverlay(valores);
    } else {
        ocultarOverlay(valores);
    }
}

function mostrarCargando(mostrar) {
    if (mostrar) {
        mostrarOverlay({});
    } else {
        ocultarOverlay({});
    }
}

function mostrarOverlay(valores) {
    if (valores == undefined) {
        valores = {};
    }

    //Animacion
    var animacion = true;
    if ('Animacion' in valores) {
        animacion = valores.Animacion;
    }

    if (animacion) {
        $('#overlay').stop(true, true).fadeIn(500);
    } else {
        $('#overlay').stop(true, true).show();
    }

    //Indicador
    var indicador = true;
    if ('Indicador' in valores) {
        indicador = valores.Indicador;
    }
    if (indicador) {
        $('#overlay').find('.spinner-layer').show();
    } else {
        $('#overlay').find('.spinner-layer').hide();
    }

    //Fondo
    var fondo = true;
    if ('Fondo' in valores) {
        fondo = valores.Fondo;
    }
    if (fondo) {
        $('#overlay').addClass('conFondo');
    } else {
        $('#overlay').removeClass('conFondo');
    }

    var texto = "";
    if ('Texto' in valores) {
        texto = valores.Texto;
    }
    $('#overlay').find("label").text(texto);
}

function ocultarOverlay(valores) {
    if (valores == undefined) {
        valores = {};
    }

    //Animacion
    var animacion = true;
    if ('Animacion' in valores) {
        animacion = valores.Animacion;
    }

    if (animacion) {
        $('#overlay').stop(true, true).fadeOut(500);
    } else {
        $('#overlay').stop(true, true).hide();
    }
}

function mostrarMensajeCritico(valores) {
    $('#body1').empty();
    $('#contenedor-principal').empty();

    $('#errorCritico').addClass('visible');

    //Icono
    if (!('Icono' in valores)) {
        valores.Icono = 'error_outline';
    }
    $('#errorCritico .material-icons').text(valores.Icono);

    //Titulo
    if (!('Titulo' in valores)) {
        valores.Titulo = 'Se presentó un error irrecuperable. Por favor comuníquese con el area de soporte.';
    }
    $('#errorCritico .titulo').text(valores.Titulo);

    //Descripcion
    if (!('Descripcion' in valores)) {
        valores.Descripcion = '';
    }
    $('#errorCritico .detalle').text(valores.Descripcion);
}

function ir(pagina) {
    $('body').addClass('oculto');
    setTimeout(function () {
        var urlNueva = pagina;
        console.log(urlNueva);

        window.location.href = urlNueva;
    }, 300);
}
