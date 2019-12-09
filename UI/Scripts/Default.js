var usuarioLogeado;
var initData;
var UrlUsuarioNuevo;

var crearOrdenEspecial = false;
let alertandoEnToolbar = false;

let geoApiInfo;

function init(data) {

    usuarioLogeado = data.UsuarioLogeado;
    initData = data.InitData;
    UrlUsuarioNuevo = data.UrlUsuarioNuevo;

    alertandoEnToolbar = false;

    //Valido las alertas del entorno
    if (data.AlertarServerLocalDbProduccion == true || data.AlertarServerTestDbProduccion == true) {
        alertandoEnToolbar = true;
        $('#header').addClass('error');
        if (data.AlertarServerLocalDbProduccion == true) {
            $('#header_textoToolbar_Titulo').text('¡Producción en Localhost!');
        } else {
            $('#header_textoToolbar_Titulo').text('¡Producción en Test!');
        }
        $('#header_textoToolbar_Titulo').addClass('destacado');
    } else {
        if (data.AlertarServerProduccionDbTest == true) {
            mostrarMensajeCritico({
                Descripcion: 'Detalle: Entorno de test en producción.'
            });
            return;
        }
    }

    setInterval(function () {
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            url: ResolveUrl('~/Servicios/UsuarioService.asmx/IsLogin'),
            success: function (result) {
                result = parse(result.d);
                if (result) return;
                usuarioLogeado = undefined;
                cerrarSesion();
            },
            error: function (result) {

            }
        });
    }, 5000);

    if (usuarioLogeado.Ambito == null || usuarioLogeado.Ambito.KeyValue == 0) {
        $.each(usuarioLogeado.Areas, function (i, object) {
            if (object.CrearOrdenEspecial) {
                crearOrdenEspecial = true;

                return;
            }
        })
    }
}

function redirigir(pagina) {
    $('body').addClass('oculto');
    setTimeout(function () {
        var urlNueva = pagina;
        console.log(urlNueva);

        window.location.href = urlNueva;
    }, 300);
}

function getUsuarioLogeado() {
    return usuarioLogeado;
}

function setUsuarioLogeado(u) {
    usuarioLogeado = u;
    try {
        header_CargarDatosUsuario();
    } catch (ex) {

    }
}

function getInitData() {
    return initData;
}


function esAmbitoCPC() {
    return usuarioLogeado.Ambito != null && usuarioLogeado.Ambito.KeyValue > 0;
}

function esAmbitoTodosLosCPC() {
    return usuarioLogeado.Ambito != null && usuarioLogeado.Ambito.KeyValue == -1;
}

function esAmbitoMunicipalidad() {
    return usuarioLogeado.Ambito == null || usuarioLogeado.Ambito.KeyValue == 0;
}

function validarPermisoAlta(pagina) {
    return $.grep(getUsuarioLogeado().Rol.Objetos, function (element, index) {
        return element.Valor == pagina && element.Acceso.Alta == true;
    }).length != 0;
}

function validarPermisoConsulta(pagina) {
    return $.grep(getUsuarioLogeado().Rol.Objetos, function (element, index) {
        return element.Valor == pagina && element.Acceso.Consulta == true;
    }).length != 0;
}

function validarPermisoBaja(pagina) {
    return $.grep(getUsuarioLogeado().Rol.Objetos, function (element, index) {
        return element.Valor == pagina && element.Acceso.Baja == true;
    }).length != 0;
}

function validarPermisoModificacion(pagina) {
    return $.grep(getUsuarioLogeado().Rol.Objetos, function (element, index) {
        return element.Valor == pagina && element.Acceso.Modificacion == true;
    }).length != 0;
}


function validarPermiso(tipo, pagina) {
    switch (tipo) {
        case 'alta': {
            return validarPermisoAlta(pagina);
        } break;

        case 'baja': {
            return validarPermisoBaja(pagina);
        } break;

        case 'modificacion': {
            return validarPermisoModificacion(pagina);
        } break;

        case 'consulta': {
            return validarPermisoConsulta(pagina);
        } break;

        default: {
            return false;
        } break;
    }
}



function cerrarSesion() {
    //Cierro los dialogos
    if ($('.jAlert').length != 0) {
        $('.jAlert').CerrarDialogo();
    }

    //Cierro sesion
    $('#header').animate({ 'opacity': 0 }, 300);
    $('#main').animate({ 'opacity': 0 }, 300, function () {
        window.location.href = ResolveUrl('~/CerrarSesion.aspx');
    });
    $('#footer').animate({ 'opacity': 0 }, 500);
    $('nav').animate({ 'opacity': 0 }, 300);

    $('#sidenav-overlay').trigger('click');
}

function setTitulo(titulo) {
    if (alertandoEnToolbar) return;
    $('#header_textoToolbar_Titulo').text(titulo);
}

function mostrarMensaje(tipo, mensaje) {
    switch (tipo) {
        case 'Alerta':
            Materialize.toast(mensaje, 5000, 'colorAlerta');
            break;

        case 'Error':
            Materialize.toast(mensaje, 5000, 'colorError');
            break;

        case 'Exito':
            Materialize.toast(mensaje, 5000, 'colorExito');
            break;

        case 'Info':
            Materialize.toast(mensaje, 5000);
            break;
    }
}


function getBarrios() {
    return new Promise(function (callback, callbackError) {
        getGeoApiInfo()
            .then(function (data) {
                callback(data.barrios);
            })
            .catch(function (error) {
                callbackError(error);
            });
    });
}

function getCpcs() {
    return new Promise(function (callback, callbackError) {
        getGeoApiInfo()
            .then(function (data) {
                callback(data.cpcs);
            })
            .catch(function (error) {
                callbackError(error);
            });
    });
}

function getEjido() {
    return new Promise(function (callback, callbackError) {
        getGeoApiInfo()
            .then(function (data) {
                callback(data.ejido);
            })
            .catch(function (error) {
                callbackError(error);
            });
    });
}


function getGeoApiInfo() {
    return new Promise(function (callback, callbackError) {
        if (geoApiInfo !== undefined) {
            callback(geoApiInfo);
            return;
        }

        $.ajax({
            url: urlCordobaGeoApi + '/info/general?conPoligono=true',
            success: function (result) {
                if (result.estado !== "OK") {
                    callbackError(result.error);
                    return;
                }

                geoApiInfo = result.info;
                callback(geoApiInfo);
            },
            error: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}