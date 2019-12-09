var crearOrdenEspecial;

function initBase(data) {
    try {
        parent.header_OnPaginaInit();
    } catch (e) {
        console.log(e);
    }
}


let callbackLoad;
function setOnLoad(callback) {
    callbackLoad = callback;
}


//Utils

function setUsuarioLogeado(usuario) {
    usuarioLogeado = usuario;
}

function getUsuarioLogeado() {
    return usuarioLogeado;
}

function esAmbitoCPC() {
    return usuarioLogeado.Ambito != null && usuarioLogeado.Ambito.KeyValue>0;
}

function esAmbitoTodosLosCPC() {
    return usuarioLogeado.Ambito != null && usuarioLogeado.Ambito.KeyValue == -1;
}

function esAmbitoMunicipalidad() {
    return usuarioLogeado.Ambito == null || usuarioLogeado.Ambito.KeyValue == 0;
}

function setAreas(areas) {
    usuarioLogeado.Areas = areas;
}

function setOrigenElegido(id) {
    return idOrigenElegido = id;
}

function getOrigenElegido() {
    return idOrigenElegido;
}

function setInitData(data) {
    initData = data;
}

function getInitData() {
    return initData;
}

function setCrearOrdenEspecial(crear) {
    crearOrdenEspecial = crear;
}


//Permisos

let callbackPermisos;

function validarPermisoAlta(pagina) {
    if (callbackPermisos == undefined) return false;
    return callbackPermisos('alta', pagina);
}

function validarPermisoConsulta(pagina) {
    if (callbackPermisos == undefined) return false;
    return callbackPermisos('consulta', pagina);
}

function validarPermisoBaja(pagina) {
    if (callbackPermisos == undefined) return false;
    return callbackPermisos('baja', pagina);
}

function validarPermisoModificacion(pagina) {
    if (callbackPermisos == undefined) return false;
    return callbackPermisos('modificacion', pagina);
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

function setCallbackPermisos(callbackNuevo) {
    callbackPermisos = callbackNuevo;
}


//Cerrar sesion

let callbackCerrarSesion;

function cerrarSesion() {
    if (callbackCerrarSesion == undefined) return;
    callbackCerrarSesion();
}

function setCallbackCerrarSesion(callbackNuevo) {
    callbackCerrarSesion = callbackNuevo;
}


// Mensajes

function setCallbackMensajes(callback) {

}

function mostrarMensaje(tipo, mensaje) {
    if (self == top) {
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
        return;
    }
    top.mostrarMensaje(tipo, mensaje);
}

function mostrarMensajeError(mensaje) {
    mostrarMensaje('Error', mensaje);
}

function mostrarMensajeExito(mensaje) {
    mostrarMensaje('Exito', mensaje);
}

function mostrarMensajeAlerta(mensaje) {
    mostrarMensaje('Alerta', mensaje);

}

function mostrarMensajeInfo(mensaje) {
    mostrarMensaje('Info', mensaje);
}


//Cargando 

let callbackCargando;

function mostrarCargando(mostrar, mensaje) {
    if (callbackCargando == undefined) return;
    callbackCargando(mostrar, mensaje);
}

function setCallbackCargando(callbackNuevo) {
    callbackCargando = callbackNuevo;
}


//Drawer

let callbackDrawer;

function setDrawerExpandido(expandido) {
    if (callbackDrawer == undefined) return;
    callbackDrawer(expandido);
}

function setCallbackExpandirDrawer(callbackNuevo) {
    callbackDrawer = callbackNuevo;
}


//redirigir

let callbackRedirigir;

function redirigir(pagina) {
    if (callbackRedirigir == undefined) return;
    callbackRedirigir(pagina);
}

function setCallbackRedirigir(callbackNuevo) {
    callbackRedirigir = callbackNuevo;
}


//Version sistema

let callbackVersionSistema;

function informarCabioVersionSistema() {
    if (callbackVersionSistema == undefined) return;
    callbackVersionSistema();
}

function setCallbackCambioVersionSistema(callbackNuevo) {
    callbackVersionSistema = callbackNuevo;
}


function getCpcs() {
    return top.getCpcs();
}

function getBarrios() {
    return top.getBarrios();
}

function getEjido() {
    return top.getEjido();
}

function getGeoApiInfo() {
    if (top === self) {
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

    return top.getGeoApiInfo();
}