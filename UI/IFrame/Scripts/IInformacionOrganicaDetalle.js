let informacionOrganica;
let area;
let permisoModificar;

function init(data) {
    if ('Error' in data) {
        mostrarError('Error cargando la página');
        return;
    }

    area = data.Area;
    informacionOrganica = data.InformacionOrganica;
    permisoModificar = validarPermisoAlta('InformacionOrganica') && validarPermisoModificacion('InformacionOrganica');

    initAcciones();
    cargarDatos();
}

function initAcciones() {
    $('#contenedor_Error .btn').click(function () {

        crearDialogoInformacionOrganicaNuevo({
            Id: area.Id,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function () {
                actualizar();
            }
        })
    });

    $('#texto_Secretaria').click(function () {
        crearDialogoInformacionOrganicaSecretariaDetalle({
            Id: informacionOrganica.Direccion.Secretaria.Id,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function () {
                actualizar();
            }
        });
    });

    $('#texto_Direccion').click(function () {
        crearDialogoInformacionOrganicaDireccionDetalle({
            Id: informacionOrganica.Direccion.Id,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function () {
                actualizar();
            }
        });
    });

    $('#btn_DarDeBaja').click(function () {
        darDeBaja();
    })
}

function cargarDatos() {

    $('#contenedor_Error .btn').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_DarDeBaja').css('display', permisoModificar ? 'auto' : 'none');

    $('#contenedor_Error').removeClass('visible');

    if (informacionOrganica != undefined && informacionOrganica.FechaBaja == undefined) {
        $('#texto_Area').text(area.Nombre);
        $('#texto_Secretaria').text(informacionOrganica.Direccion.Secretaria.Nombre);
        $('#texto_Direccion').text(informacionOrganica.Direccion.Nombre);
        $('#texto_Responsable').text(informacionOrganica.Direccion.Responsable);
        $('#texto_Domicilio').text(informacionOrganica.Direccion.Domicilio);
        $('#texto_Email').text(informacionOrganica.Direccion.Email);
        $('#texto_Telefono').text(informacionOrganica.Direccion.Telefono);
    } else {
        mostrarAvisoSinInformacionOrganica();
    }
}

let BASE_SERVICIO = '~/Servicios/InformacionOrganicaService.asmx';

function actualizar() {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl(BASE_SERVICIO + '/GetByIdArea'),
        Data: { idArea: area.Id },
        OnSuccess: function (result) {

            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                return;
            }

            informacionOrganica = result.Return;
            cargarDatos();
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function darDeBaja() {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl(BASE_SERVICIO + '/DarDeBaja'),
        Data: { id: informacionOrganica.Id },
        OnSuccess: function (result) {

            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                return;
            }

            informacionOrganica = result.Return;
            cargarDatos();
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function mostrarAvisoSinInformacionOrganica() {
    $('#contenedor_Error').addClass('visible');
    $('#contenedor_Error label').text('El area ' + area.Nombre + ' no tiene asociada información orgánica');

}