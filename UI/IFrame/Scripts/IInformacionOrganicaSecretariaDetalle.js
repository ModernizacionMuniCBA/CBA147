let secretaria;
let direcciones;
let permisoModificar;

function init(data) {
    if ('Error' in data) {
        return;
    }

    secretaria = data.Entity;
    direcciones = data.Direcciones;
    permisoModificar = validarPermisoAlta('InformacionOrganica') && validarPermisoModificacion('InformacionOrganica');

    initAcciones();

    cargarDatos();
}

function initAcciones() {

    $('#btn_DarDeAlta').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_DarDeAlta').click(function () {
        darDeAlta();
    });

    $('#btn_DarDeBaja').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_DarDeBaja').click(function () {
        darDeBaja();
    });


    $('#btn_AgregarDireccion').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_AgregarDireccion').click(function () {
        if (!permisoModificar) return;

        crearDialogoInformacionOrganicaDireccionNuevo({
            IdSecretaria: secretaria.Id,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje)
            },
            Callback: function (direccion) {
                actualizar();
            }
        });
    });

    $('#btn_EditarNombre').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_EditarNombre').click(function () {
        if (!validarPermisoAlta('InformacionOrganica') || !validarPermisoModificacion('InformacionOrganica')) return;

        crearDialogoInput({
            Placeholder: secretaria.Nombre,
            Titulo: 'Cambiar nombre de secretaría',
            Botones: [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Cambiar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert) {

                        let nombre = $(jAlert).find('input').val();
                        if (nombre == undefined || nombre == "") {
                            $(jAlert).find('input').focus();
                            mostrarMensaje('Alerta', 'Ingrese el nombre');
                            return;
                        }

                        $(jAlert).MostrarDialogoCargando(true);

                        cambiarNombre(nombre)
                            .then(function () {
                                $(jAlert).CerrarDialogo();
                                actualizar();
                            })
                            .catch(function (error) {
                                $(jAlert).MostrarDialogoCargando(false);
                                mostrarMensaje('Error', error);
                            });
                    }
                }
            ]
        });
    });
}

function cargarDatos() {
    $('#texto_Nombre').text(secretaria.Nombre);

    $('#contenedor_Direcciones').empty();
    if (direcciones == undefined || direcciones.length == 0) {
        agregarMensajeSinDirecciones();
    } else {
        $.each(direcciones, function (index, element) {
            agregarDireccion(element);
        });
    }


    if (secretaria.FechaBaja == undefined) {
        $('#texto_Estado').hide();
    } else {
        $('#texto_Estado').show();
        $('#texto_Estado').text('Entidad dada de baja');
    }

    cargarAcciones();
}

function cargarAcciones() {
    $('#btn_DarDeAlta').hide();
    $('#btn_DarDeBaja').hide();
    $('.btnEdit').hide();
    $('#btn_AgregarDireccion').hide();
    if (!permisoModificar) return;

    let dadoDeBaja = secretaria.FechaBaja != undefined;
    if (!dadoDeBaja) {
        $('#btn_DarDeAlta').hide();
        $('#btn_DarDeBaja').show();
        $('.btnEdit').show();
        $('#btn_AgregarDireccion').show();
    } else {
        $('#btn_DarDeAlta').show();
        $('#btn_DarDeBaja').hide();
        $('.btnEdit').hide();
        $('#btn_AgregarDireccion').hide();
    }
}


function actualizar() {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/GetById'),
        Data: { id: secretaria.Id },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                return;
            }

            crearAjax({
                Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/GetByIdSecretaria'),
                Data: { idSecretaria: secretaria.Id },
                OnSuccess: function (result2) {
                    if (!result2.Ok) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', result2.Error);
                        return;
                    }

                    secretaria = result.Return;
                    direcciones = result2.Return;
                    cargarDatos();
                    mostrarCargando(false);
                },
                OnError: function (result) {
                    mostrarCargando(false);
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
            })
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function agregarMensajeSinDirecciones() {
    let div = $('<label>Esta secretaria no tiene ninguna dirección</label>');
    $('#contenedor_Direcciones').append(div);
}

function agregarDireccion(direccion) {
    let div = $('<div class="direccion card"><label class="nombre">' + direccion.Nombre + '</label><a class="btn-flat chico btn-redondo waves-effect"><i class="material-icons">more_vert</i></a></div>');
    $('#contenedor_Direcciones').append(div);

    $(div).find('a.btn-flat').click(function (e) {
        e.stopPropagation();

        $(div).find('a.btn-flat').MenuFlotante({
            Menu: [
                {
                    Texto: 'Detalle',
                    Icono: 'description',
                    OnClick: function () {
                        $(div).trigger('click');
                    }
                },
                {
                    Texto: 'Quitar',
                    Visible: permisoModificar && secretaria.FechaBaja == undefined,
                    Icono: 'delete',
                    OnClick: function () {
                        quitarDireccion(direccion.Id);
                    }
                }
            ]
        });
    });

    $(div).click(function () {
        crearDialogoInformacionOrganicaDireccionDetalle({
            Id: direccion.Id,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function () {
                actualizar();
            }
        });
    });
}


//Acciones

function cambiarNombre(nombre) {

    return new Promise(function (callback, callbackError) {
        if (!validarPermisoAlta('InformacionOrganica') || !validarPermisoModificacion('InformacionOrganica')) {
            callbackError('sin permisos');
        };

        crearAjax({
            Url: ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/CambiarNombre'),
            Data: { id: secretaria.Id, nombre: nombre },
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
        });
    });


}

function quitarDireccion(idDireccion) {
    if (!validarPermisoAlta('InformacionOrganica') || !validarPermisoModificacion('InformacionOrganica')) return;

    mostrarCargando(true);
    ajax_QuitarDireccion(idDireccion,
        function () {
            actualizar();
        },
        function (error) {
            mostrarError(error);
            mostrarCargando(false);
        });
}

function ajax_QuitarDireccion(idDireccion, callback, callbackError) {
    crearAjax({
        Url: ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/QuitarDireccion'),
        Data: { id: secretaria.Id, idDireccion: idDireccion },
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
    });
}

let BASE_SERVICE = '~/Servicios/InformacionOrganicaSecretariaService.asmx';

function darDeAlta() {
    if (!validarPermisoAlta('InformacionOrganica') || !validarPermisoModificacion('InformacionOrganica')) return;

    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl(BASE_SERVICE + '/DarDeAlta'),
        Data: { id: secretaria.Id },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                return;
            }

            actualizar();
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function darDeBaja() {
    if (!validarPermisoAlta('InformacionOrganica') || !validarPermisoModificacion('InformacionOrganica')) return;

    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl(BASE_SERVICE + '/DarDeBaja'),
        Data: { id: secretaria.Id },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                return;
            }

            actualizar();
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}