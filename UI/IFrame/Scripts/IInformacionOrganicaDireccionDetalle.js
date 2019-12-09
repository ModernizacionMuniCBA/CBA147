let direccion;
let permisoModificar;

function init(data) {
    direccion = data.Direccion;
    permisoModificar = validarPermisoAlta('InformacionOrganica') && validarPermisoModificacion('InformacionOrganica');

    initAcciones();
    cargarEntidad();
}


function cargarEntidad() {
    $('#texto_Secretaria').text(direccion.Secretaria.Nombre);

    $('#texto_Nombre').text(direccion.Nombre);
    $('#texto_Responsable').text(direccion.Responsable);
    $('#texto_Domicilio').text(direccion.Domicilio);
    $('#texto_Telefono').text(direccion.Telefono);
    $('#texto_Email').text(direccion.Email);

    if (direccion.FechaBaja == undefined) {
        $('#texto_Estado').hide();
    } else {
        $('#texto_Estado').show();
        $('#texto_Estado').text('Entidad dada de baja');
    }

    cargarAcciones();
}

function initAcciones() {
    $('#texto_Secretaria').click(function () {
        crearDialogoInformacionOrganicaSecretariaDetalle({
            Id: direccion.Secretaria.Id,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function () {
                actualizar();
            }
        });
    });

    $('#btn_CambiarNombre').click(function () {
        cambiarNombre();
    });

    $('#btn_CambiarResponsable').click(function () {
        cambiarResponsable();
    });

    $('#btn_CambiarDomicilio').click(function () {
        cambiarDomicilio();
    });

    $('#btn_CambiarEmail').click(function () {
        cambiarEmail();
    });

    $('#btn_CambiarTelefono').click(function () {
        cambiarTelefono();
    });

    $('#btn_DarDeAlta').click(function () {
        darDeAlta();
    });

    $('#btn_DarDeBaja').click(function () {
        darDeBaja();
    });
}

function cargarAcciones() {
    $('#btn_DarDeAlta').hide();
    $('#btn_DarDeBaja').hide();
    $('.btnEdit').hide();

    $('#btn_CambiarNombre').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_CambiarResponsable').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_CambiarDomicilio').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_CambiarEmail').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_CambiarTelefono').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_DarDeAlta').css('display', permisoModificar ? 'auto' : 'none');
    $('#btn_DarDeBaja').css('display', permisoModificar ? 'auto' : 'none');

    if (!permisoModificar) return;

    if (direccion.FechaBaja == undefined) {
        $('#btn_DarDeAlta').hide();
        $('#btn_DarDeBaja').show();
        $('.btnEdit').show();
    } else {
        $('#btn_DarDeAlta').show();
        $('#btn_DarDeBaja').hide();
        $('.btnEdit').hide();

    }


}


function cambiarNombre() {
    crearDialogoInput({
        Placeholder: direccion.Nombre,
        Titulo: 'Cambiar nombre',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let nombre = $(jAlert).find('input').val();
                    if (nombre == undefined || nombre.length == 0) {
                        mostrarMensaje('Alerta', 'Ingrese el nombre');
                        $(jAlert).find('input').focus();
                        return;
                    }


                    mostrarCargando(true);
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/CambiarNombre'),
                        Data: { id: direccion.Id, nombre: nombre },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            actualizar();
                            mostrarCargando(false);
                            $(jAlert).CerrarDialogo();
                        },
                        OnError: function (result) {
                            mostrarCargando(false);
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function cambiarResponsable() {
    crearDialogoInput({
        Placeholder: direccion.Responsable,
        Titulo: 'Cambiar responsable',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let valor = $(jAlert).find('input').val();
                    if (valor == undefined || valor.length == 0) {
                        mostrarMensaje('Alerta', 'Ingrese el nombre del responsable');
                        $(jAlert).find('input').focus();
                        return;
                    }


                    mostrarCargando(true);
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/CambiarResponsable'),
                        Data: { id: direccion.Id, responsable: valor },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            actualizar();
                            mostrarCargando(false);
                            $(jAlert).CerrarDialogo();
                        },
                        OnError: function (result) {
                            mostrarCargando(false);
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function cambiarDomicilio() {
    crearDialogoInput({
        Placeholder: direccion.Domicilio,
        Titulo: 'Cambiar domicilio',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let valor = $(jAlert).find('input').val();
                    if (valor == undefined || valor.length == 0) {
                        mostrarMensaje('Alerta', 'Ingrese el domicilio');
                        $(jAlert).find('input').focus();
                        return;
                    }


                    mostrarCargando(true);
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/CambiarDomicilio'),
                        Data: { id: direccion.Id, domicilio: valor },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            actualizar();
                            mostrarCargando(false);
                            $(jAlert).CerrarDialogo();
                        },
                        OnError: function (result) {
                            mostrarCargando(false);
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function cambiarEmail() {
    crearDialogoInput({
        Placeholder: direccion.Email,
        Titulo: 'Cambiar e-mail',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let valor = $(jAlert).find('input').val();
                    if (valor == undefined || valor.length == 0) {
                        mostrarMensaje('Alerta', 'Ingrese el e-mail');
                        $(jAlert).find('input').focus();
                        return;
                    }


                    mostrarCargando(true);
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/CambiarEmail'),
                        Data: { id: direccion.Id, email: valor },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            actualizar();
                            mostrarCargando(false);
                            $(jAlert).CerrarDialogo();
                        },
                        OnError: function (result) {
                            mostrarCargando(false);
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function cambiarTelefono() {
    crearDialogoInput({
        Placeholder: direccion.Telefono,
        Titulo: 'Cambiar teléfono',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let valor = $(jAlert).find('input').val();
                    if (valor == undefined || valor.length == 0) {
                        mostrarMensaje('Alerta', 'Ingrese el teléfono');
                        $(jAlert).find('input').focus();
                        return;
                    }


                    mostrarCargando(true);
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/CambiarTelefono'),
                        Data: { id: direccion.Id, telefono: valor },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            actualizar();
                            mostrarCargando(false);
                            $(jAlert).CerrarDialogo();
                        },
                        OnError: function (result) {
                            mostrarCargando(false);
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function darDeAlta() {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/DarDeAlta'),
        Data: { id: direccion.Id},
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
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/DarDebaja'),
        Data: { id: direccion.Id},
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

function actualizar() {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/GetById'),
        Data: { id: direccion.Id },
        OnSuccess: function (result) {

            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                return;
            }

            direccion = result.Return;
            cargarEntidad();
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}