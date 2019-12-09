$(function () {
    $('.btn-volver').click(function () {
        redirigir('Inicio');
    });
})

// Init
function init(data) {
    if (verificarError(data) === true) return;

    initAppData(data.AppData);
}

// AppData
function initAppData(appData) {
    $('._logo').attr('src', appData.urlLogoMuni);

    $('._titulo').html('Municipalidad de Córdoba');
    $('._secretaria').html(appData.acercaDe_secretaria);
    $('._direccion').html(appData.acercaDe_direccion);

    $('._domicilio a').html(appData.acercaDe_domicilio);
    $('._domicilio').click(function () {
        let url = 'https://www.google.com.sa/maps/search/' + appData.acercaDe_domicilioLatitud + ',' + appData.acercaDe_domicilioLongitud;
        window.open(url, '_blank');
    });

    $('._telefono a').html(appData.acercaDe_telefono);
    $('._telefono').click(function () {
        let url = 'tel://' + appData.acercaDe_telefonoDato;
        window.open(url, '_blank');
    });

    $('._email a').html(appData.acercaDe_email);
    $('._email').click(function () {
        let url = 'mailto:' + appData.acercaDe_email;
        window.open(url, '_blank');
    });

    $('._version').html('Version ' + appData.versionWeb);
    $('._versionFecha').html(appData.versionWebFecha);
}