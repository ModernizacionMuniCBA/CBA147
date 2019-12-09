let callbackCargando;

let usuarioLogeado;
let initData;
let crearOrdenEspecial;

function initBase(data) {
    usuarioLogeado = data.UsuarioLogeado;
    initData = data.InitData;

    if (usuarioLogeado.Ambito == null || usuarioLogeado.Ambito.KeyValue == 0) {
        $.each(usuarioLogeado.Areas, function (i, object) {
            if (object.CrearOrdenEspecial) {
                crearOrdenEspecial = true;
                return;
            }
        });
    }

    console.log('init');
}

function getUsuarioLogeado() {
    return top.getUsuarioLogeado();
}

function esAmbitoCPC() {
    return top.esAmbitoCPC();
}

function esAmbitoTodosLosCPC() {
    return top.esAmbitoTodosLosCPC();
}

function esAmbitoMunicipalidad() {
    return top.esAmbitoMunicipalidad();
}

function getOrigenElegido() {
    return top.getOrigenElegido();
}

function setUsuarioLogeado(usuario) {
    return top.setUsuarioLogeado(usuario);
}

function getInitData() {
    return top.getInitData();
}

//-------------------------------
// Listener Cargando
//-------------------------------

function mostrarCargando(mostrar, mensaje) {
    if (callbackCargando == undefined) return;
    callbackCargando(mostrar, mensaje);
}

function setOnCargandoListener(callbackNuevo) {
    callbackCargando = callbackNuevo;
}

//-----------------------------
// Listener Alertas
//-----------------------------

function setOnMensajeListener(callback) {

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


function mostrarMensajeCritico(valores) {
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


//-----------------------------
// Permisos
//-----------------------------

function validarPermisoAlta(pagina) {
    return top.validarPermisoAlta(pagina);
}

function validarPermisoConsulta(pagina) {
    return top.validarPermisoConsulta(pagina);
}

function validarPermisoBaja(pagina) {
    return top.validarPermisoBaja(pagina);
}

function validarPermisoModificacion(pagina) {
    return top.validarPermisoModificacion(pagina);
}

function validarPermiso(tipo, pagina) {
    return top.validarPermiso(tipo, pagina);
}



let geoApiInfo;

function getCpcs() {
    if (top === self) {
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
    return top.getCpcs();
}

function getBarrios() {
    if (top === self) {
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
    return top.getBarrios();
}

function getEjido() {
    if (top === self) {
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