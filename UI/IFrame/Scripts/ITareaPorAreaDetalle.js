var tarea;

var PATH_IMAGEN_USUARIO_DEFAULT;

function init(data) {
    if ('Error' in data) {
        mostrarError(data.Error);
        return;
    }

    PATH_IMAGEN_USUARIO_DEFAULT = ResolveUrl('~/Resources/Imagenes/user-avatar.png');

    tarea = data.Tarea;

    initEncabezado();
    initAlertas();

    initInformacionAdicional();
    cargarDatos();
}

function cargarDatos() {
    cargarAcciones();
    cargarDatosEncabezado();
    cargarDescripcion();
    cargarInformacionAdicional();
}

function cargarDescripcion() {
    if (tarea.Observaciones == null || tarea.Observaciones == "") {
        $("#contenedor_SeccionDescripcion").hide();
        return;
    }

    $("#contenedor_SeccionDescripcion").show();
    $("#texto_Descripcion").text(tarea.Observaciones);
}

//Encabezado
function initEncabezado() {
    $('#btn_Acciones').click(function () {
        $('#contenedor_Acciones .contenido').toggleClass('visible');
        $(this).text($('#contenedor_Acciones .contenido').hasClass('visible') ? 'Ocultar acciones' : 'Ver acciones');
    });
}

function cargarDatosEncabezado() {
    $('#texto_Nombre').text(toTitleCase(tarea.Nombre) );
    $("#texto_Area").text(tarea.AreaNombre);

    if (tarea.FechaBaja == null) {
        $("#texto_IndicadorEstado").hide();
    } else {
    $("#texto_IndicadorEstado").show();
    }

    cargarAlertas();
}

//Alertas
function initAlertas() {

}

function cargarAlertas() {
    $('#contenedor_Alertas').empty();
}

//Informacion adicional
function initInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoUsuarioCreador').click(function () {
        crearDialogoUsuarioDetalle({
            Id: tarea.UsuarioCreadorId
        });
    });

    $('#contenedor_InfoAdicional .textoUsuarioModificacion').click(function () {
        crearDialogoUsuarioDetalle({
            Id: tarea.UsuarioModificacionId
        });
    });
}

function cargarInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoFechaCreacion').html('<b>' + dateTimeToString(tarea.FechaAlta) + '</b>');

    if (tarea.UsuarioCreadorNombre != undefined) {
        $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').show();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').show();
        var usuarioCreador = toTitleCase(tarea.UsuarioCreadorNombre + ' ' + tarea.UsuarioCreadorApellido).trim();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').html('<b>' + usuarioCreador + '</b>');
    } else {
        $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').hide();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').hide();
    }

    if (tarea.FechaModificacion != undefined) {
        $('#contenedor_InfoAdicional .linea3').show();
        $('#contenedor_InfoAdicional .textoFechaModificacion').html('<b>' + dateTimeToString(tarea.FechaModificacion) + '</b>');
        if (tarea.UsuarioModificacionNombre != undefined && tarea.UsuarioModificacionNombre.trim() != "") {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').html('<b>' + toTitleCase(tarea.UsuarioModificacionNombre + ' ' + tarea.UsuarioModificacionApellido) + '</b>');
        } else {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').hide();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').hide();
        }

    } else {
        $('#contenedor_InfoAdicional .linea3').hide();
    }
}

//Acciones
function cargarAcciones() {
    $('#contenedor_Acciones .contenido').empty();

    var modificacion = true;
    var modificacion = validarPermisoModificacion('Tareas');

    if (modificacion && tarea.FechaBaja==null) {
        //Funciones
        agregarAccion({
            Texto: 'Editar nombre',
            Icono: 'edit',
            OnClick: function () {
                editarNombre();
            }
        });

        agregarAccion({
            Texto: 'Editar descripción',
            Icono: 'edit',
            OnClick: function () {
                editarDescripcion();
            }
        });

        //Dar de baja
        agregarAccion({
            Texto: 'Dar de baja',
            Icono: 'close',
            OnClick: function () {
                darDeBaja();
            }
        });
    }

    if (modificacion && tarea.FechaBaja != null) {
        //Dar de alta
        agregarAccion({
            Texto: 'Dar de alta',
            Icono: 'close',
            OnClick: function () {
                darDeAlta();
            }
        });
    }

}

function agregarAccion(valores) {
    var div = $($('#template_Accion').html());
    $(div).find('.texto').text(valores.Texto);
    $(div).find('.icono').text(valores.Icono);
    $('#contenedor_Acciones .contenido').append(div);
    if (('Permiso' in valores) && !valores.Permiso) {
        $(div).addClass('deshabilitado');
    }
    $(div).click(function () {
        valores.OnClick();
    });
}

function editarNombre() {
    crearDialogoInput({
        Valor: tarea.Nombre,
        Titulo: 'Editar nombre',
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

                    mostrarCargando(true);
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/TareaService.asmx/EditarNombre'),
                        Data: { comando: { IdTarea: tarea.Id, Valor: valor } },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            mostrarCargando(false);
                            $(jAlert).CerrarDialogo();

                            actualizarDetalle(function () {
                                cargarDatosEncabezado();
                            });
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

function editarDescripcion() {
    crearDialogoInput({
        Valor: tarea.Observaciones,
        Titulo: 'Editar descripción',
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

                    mostrarCargando(true);
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/TareaService.asmx/EditarDescripcion'),
                        Data: { comando: { IdTarea: tarea.Id, Valor: valor } },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            mostrarCargando(false);
                            $(jAlert).CerrarDialogo();

                            actualizarDetalle(function () {
                                cargarDescripcion();
                            });
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

function darDeBaja() {
    crearDialogoConfirmacion({
        Texto: '¿Esta seguro de que quiere dar de baja a la tarea en su área?',
        ClassBotonAceptar: 'colorExito',
        CallbackPositivo: function () {
            crearAjax({
                Url: ResolveUrl('~/Servicios/TareaService.asmx/DarDeBaja'),
                Data: { id: tarea.Id },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle(function () {
                        cargarDatosEncabezado();
                    });
                },
                OnError: function (result) {
                    mostrarMensaje('Error', result.Error);
                    return;
                }
            });
        }
    });
}

function darDeAlta() {
    crearDialogoConfirmacion({
        Texto: '¿Esta seguro de que quiere dar de alta nuevamente a la tarea en su área?',
        ClassBotonAceptar: 'colorExito',
        CallbackPositivo: function () {
            crearAjax({
                Url: ResolveUrl('~/Servicios/TareaService.asmx/DarDeAlta'),
                Data: { id: tarea.Id },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle(function () {
                        cargarDatosEncabezado();
                    });
                },
                OnError: function (result) {
                    mostrarMensaje('Error', result.Error);
                    return;
                }
            });
        }
    });
}

//Utiles
function mostrarError(error) {
    mostrarMensajeCritico({ Icono: 'error_outline', Titulo: error })
}

function actualizarDetalle(callback) {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/TareaService.asmx/GetDetalleById'),
        Data: { id: tarea.Id },
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarError(result.Error);
                return;
            }

            tarea = result.Return;
            if (callback != undefined)
                callback();
            cargarInformacionAdicional();
            cargarAcciones();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error procesando la solicitud');
        }
    });
}
