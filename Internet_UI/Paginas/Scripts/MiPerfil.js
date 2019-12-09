$(function () {
    $('.btn-volver').click(function () {

        window.location.href = ResolveUrl('~/Inicio');
    });
})

// Init
function init(data) {
    if (verificarError(data) == true) return;

    initFotoPersonal(usuarioLogeado.IdentificadorFotoPersonal);
    initFotoPersonalEvent()

    initDatosPersonales(usuarioLogeado);

    initDatosAcceso(usuarioLogeado);
    initDatosAccesoEvent(usuarioLogeado);

    initDatosContacto(usuarioLogeado);
    initDatosContactoEvent();
}


// Foto personal
function initFotoPersonal(identificador) {
    $('#fotoPersonal').css('background-image', 'Url(' + urlCordobaFiles + '/Archivo/' + identificador + ')');
}

function initFotoPersonalEvent() {
    $('#contenedor_FotoPersonal input').on('change', function () {
        actualizarFotoPersonal(this.files);
    });

    $('#fotoPersonal').click(function () {
        $('#contenedor_FotoPersonal input').val('');
        $('#contenedor_FotoPersonal input').trigger('click');
    });
}

function actualizarFotoPersonal(files) {
    if (files === undefined || files.length === 0) return;

    let file = files[0];

    let fileReader = new FileReader();
    fileReader.onload = function (e) {
        mostrarCargando(true);
        let content = e.target.result;
        achicarImagen(content, 500)
            .then(function (imagenAchicada) {
                ajaxActualizarFotoPersonal(comandoFotoPersonal(imagenAchicada))
                    .then(function (identificador) {
                        initFotoPersonal(identificador);
                        initMasterFotoPersonal(identificador);
                        mostrarCargando(false);
                    })
                    .catch(function (error) {
                        mostrarCargando(false);
                        mostrarMensaje(error);
                    });
            });
    }
    fileReader.readAsDataURL(file);
}

function ajaxActualizarFotoPersonal(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/ActualizarFotoPersonal'),
            Data: { comando: comando },
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

function comandoFotoPersonal(imagen) {
    let comando = {};
    comando.Content = imagen;
    return comando;
}


// Datos personales
function initDatosPersonales(usuario) {
    $('.nombre .detalle').html(usuario.Nombre + ' ' + usuario.Apellido);
    $('.dni .detalle').html(usuario.Dni);
    $('.cuil .detalle').html(usuario.Cuil);
    $('.domicilioLegal .detalle').html(usuario.DomicilioLegal);

    if (usuario.FechaNacimiento != undefined) {
        $('.fechaNacimiento .detalle').html(getDate(usuario.FechaNacimiento));
    }

    $('.sexo i').removeClass('mdi-gender-male-female');
    if (usuario.SexoMasculino) {
        $('.sexo .detalle').html('Masculino');
        $('.sexo i').addClass('mdi-gender-male');
    } else {
        $('.sexo .detalle').html('Femenino');
        $('.sexo i').addClass('mdi-gender-female');
    }
}


// Datos acceso
function initDatosAcceso(usuario) {
    let username = usuario.Username;

    if (username === usuario.Cuil) {
        $('.username .detalle').html('No tiene ningún nombre de usuario asociado');
    } else {
        $('.username .detalle').html(username);
    }
}

function initDatosAccesoEvent() {
    $('.username .link').click(function () {
        redirigir('CambiarUsername');
    });

    $('.password .link').click(function () {
        redirigir('CambiarPassword');
    });
}


// Datos contacto
function initDatosContacto(usuario) {
    $('#input_Email').val(usuario.Email);

    if (usuario.TelefonoFijo != undefined && usuario.TelefonoFijo !== '') {
        let array = usuario.TelefonoFijo.split('-');
        if (array.length > 1) {
            $('#telA').val(array[0]);
            $('#telN').val(array[1]);
        }
    }

    if (usuario.TelefonoCelular != undefined && usuario.TelefonoCelular !== '') {
        let array = usuario.TelefonoCelular.split('-');
        if (array.length > 1) {
            $('#celA').val(array[0]);
            $('#celN').val(array[1]);
        }
    }
}

function initDatosContactoEvent() {
    $('.btn-guardar').click(actualizarDatosContacto);
}

function actualizarDatosContacto() {
    if (!validarDatosContacto()) return;

    mostrarCargando(true);
    ocultarError('error_DatosContacto');

    ajaxActualizarDatosContacto(comandoDatosContacto())
        .then(function () {
            redirigir('MiPerfil', { texto: 'Datos de contacto actualizados correctamente', tipo: 'exito' });
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarError('error_DatosContacto', error);
        });
}

function ajaxActualizarDatosContacto(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/ActualizarDatosContacto'),
            Data: { comando: comando },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
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

function validarDatosContacto() {
    if (!$('#formContacto').valid()) {
        mostrarError('error_DatosContacto', 'Verifique su E-mail');
        return false;
    }

    let telA = $('#telA').val();
    let telN = $('#telN').val();
    let celA = $('#celA').val();
    let celN = $('#celN').val();

    if ((celA === '' && celN !== '')) {
        $('#celA').addClass('error');
        mostrarError('error_DatosContacto', 'Complete el área del Celular');
        return false;
    }

    if (celA !== '' && celN === '') {
        $('#celN').addClass('error');
        mostrarError('error_DatosContacto', 'Completar el número de Celular');
        return false;
    }

    if (telA === '' && telN !== '') {
        $('#telA').addClass('error');
        mostrarError('error_DatosContacto', 'Complete el área del Teléfono');
        return false;
    }

    if (telA !== '' && telN === '') {
        $('#telN').addClass('error');
        mostrarError('error_DatosContacto', 'Completar el número de Teléfono');
        return false;
    }

    if (celA === '' && telA === '') {
        mostrarError('error_DatosContacto', 'Complete algún Teléfono');
        return false;
    }

    return true;
}

function comandoDatosContacto() {
    let comando = {};
    comando.Email = $('#input_Email').val();
    comando.TelefonoFijo = $('#telA') !== '' && $('#telN') !== '' ? $('#telA').val() + '-' + $('#telN').val() : '';
    comando.TelefonoCelular = $('#celA') !== '' && $('#celN') !== '' ? $('#celA').val() + '-' + $('#celN').val() : '';
    return comando;
}