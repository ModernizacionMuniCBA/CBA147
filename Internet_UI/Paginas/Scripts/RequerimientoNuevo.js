const DESCRIPCION_MIN_L = isTest() ? 5 : 20;
const FOTO_MAX_W = 1000;

const INFO_SERVICIO = "Seleccione el servicio que mejor se ajusta a su requerimiento";
const INFO_MOTIVO = "Ahora seleccione el motivo de su requerimiento";
const INFO_DESCRIPCION = "Indique de la forma más detallada posible toda la información asociada al requerimiento";
const INFO_UBICACION = "Su requerimiento debe tener una ubicación asociada, por favor seleccionela haciendo click en 'Seleccionar ubicación'";
const INFO_FOTO = "Si lo desea puede adjuntar una foto al requerimiento haciendo click en 'Agregar foto'. Este paso es opcional.";

let servicios;
let motivos;
let motivosActuales;

let idServicio;
let idMotivo;
let descripcion;
let ubicacion;
let imagen;

let animacion;
let idRequerimiento;

$(function () {
    $('.btn-volver').click(function () {
        redirigir('Inicio', undefined, false);
    });
})


// Init
function init(data) {
    if (verificarError(data) == true) return;

    servicios = data.Servicios;
    motivos = data.Motivos;

    initPaso1();
    initPaso2();
    initPaso3();
    initConfirmacion();

    window.addEventListener("beforeunload", verificarDatosSinGuardar);
}

function verificarDatosSinGuardar(event) {
    if (idServicio !== undefined || idMotivo !== undefined || descripcion !== undefined || ubicacion !== undefined || imagen !== undefined) {
        LoadingPage.off();
        event.returnValue = '';
    } else {
        redirigirAnimar();
    }
}

// Motivo
function initPaso1() {
    initServiciosPrincipales();

    // Info
    $('#contenedor_SeleccionarServicio .card-info > label').html(INFO_SERVICIO);
    $('#contenedor_SeleccionarMotivo .card-info > label').html(INFO_MOTIVO);
    $('#contenedor_InsertarDescripcion .card-info > label').html(INFO_DESCRIPCION);

    // Event
    $('#paso1 > .content-header').click(abrirPaso1);
    $('.btn-cancelarServicio, .btn-cancelarMotivo').click(reiniciarPaso1);
    $('#paso1 .btn-siguiente').click(siguientePaso1);

    $('.btn-servicios').click(dialogoServicio);
    $('.btn-motivos').click(dialogoMotivo);
    $('.btn-serviciosMotivos').click(dialogoServicioMotivo);
    $('#input_Descripcion').keyup(insertarDescripcion);
}

function initServiciosPrincipales() {
    $.each(servicios, function (index, element) {
        if (element.Principal == true) {
            var html = getHtmlServicioPrincipal(element);
            $(html).appendTo($('.contenedor_ServiciosPrincipales .row'));
        }
    });
}

function abrirPaso1() {
    $('.card-detalle').removeClass('seleccionada');
    $('#paso1').addClass('seleccionada');
}

function reiniciarPaso1() {
    idServicio = undefined;
    idMotivo = undefined;
    motivosActuales = undefined;

    verificarBtnFinalizar();

    $('#paso1 .indicador').removeClass('completo');
    $('.btn-serviciosMotivos').fadeIn(300);
    $('#contenedor_SeleccionarMotivo').fadeOut(300, function () {
        $('#contenedor_InsertarDescripcion').fadeOut(300, function () {
            $('#contenedor_SeleccionarServicio').fadeIn(300);
        });
    });
}

function siguientePaso1() {
    if (!validarPaso1(true)) return;
    abrirPaso2();
}

function validarPaso1(conMensajes) {
    if (idServicio === undefined) {
        if (conMensajes) mostrarMensaje('Seleccione un servicio');
        return false;
    }

    if (idMotivo === undefined) {
        if (conMensajes) mostrarMensaje('Seleccione un motivo');
        return false;
    }

    if (descripcion === undefined || descripcion.length < DESCRIPCION_MIN_L) {
        if (conMensajes) mostrarMensaje('Ingrese una descripción de al menos ' + DESCRIPCION_MIN_L + ' caracteres');
        return false;
    }

    return true;
}

function dialogoServicio() {
    crearDialogo({
        Id: 'dialogoServicios',
        Class: 'dialogo-busqueda',
        Html: $($('#template_DialogoServicios').html()),
        OnShow: function (dialogo) {
            $.each(servicios, function (index, element) {
                let html = getHtmlServicioDialogo(element);
                $(html).appendTo($(dialogo).find('.contenedor-resultados'));
            });

            $(dialogo).find('input').on('input', function () {
                let busqueda = $(this).val().toLowerCase().trim().split(' ');

                $.each($('#dialogoServicios .item'), function (index, element) {
                    let visible = false;

                    $.each(busqueda, function (index, palabra) {
                        let nombre = $(element).find('label').html().toLowerCase();

                        if (nombre != undefined && nombre.indexOf(palabra) != -1) {
                            visible = true;
                        }
                    });

                    if (visible) {
                        $(element).removeClass('oculto');
                    } else {
                        $(element).addClass('oculto');
                    }
                });
            });

            setTimeout(function () {
                $(dialogo).find('input').focus();
            }, 100);
        }
    });
}

function dialogoMotivo() {
    crearDialogo({
        Id: 'dialogoMotivos',
        Class: 'dialogo-busqueda',
        Html: $($('#template_DialogoMotivos').html()),
        OnShow: function (dialogo) {
            $.each(motivosActuales, function (index, element) {
                let html = getHtmlMotivoDialogo(element);
                $(html).appendTo($(dialogo).find('.contenedor-resultados'));
            });

            $(dialogo).find('input').on('input', function () {
                let busqueda = $(this).val().toLowerCase().trim().split(' ');

                $.each($('#dialogoMotivos .item'), function (index, element) {
                    let visible = false;

                    $.each(busqueda, function (index, palabra) {
                        let nombre = $(element).find('label').html().toLowerCase();
                        let keywords = ($(element).attr('data-keywords') || '').toLowerCase();

                        if (nombre != undefined && nombre.indexOf(palabra) != -1) {
                            visible = true;
                        }

                        if (keywords != undefined && keywords.indexOf(palabra) != -1) {
                            visible = true;
                        }
                    });

                    if (visible) {
                        $(element).removeClass('oculto');
                    } else {
                        $(element).addClass('oculto');
                    }
                });
            });

            setTimeout(function () {
                $(dialogo).find('input').focus();
            }, 100);
        }
    });
}

function dialogoServicioMotivo() {
    crearDialogo({
        Id: 'dialogoServiciosMotivos',
        Class: 'dialogo-busqueda',
        Html: $($('#template_DialogoServiciosMotivos').html()),
        OnShow: function (dialogo) {
            $.each(motivos, function (index, element) {
                let html = getHtmlServicioMotivoDialogo(element);
                $(html).appendTo($(dialogo).find('.contenedor-resultados'));
            });

            $(dialogo).find('input').on('input', function () {
                let busqueda = $(this).val().toLowerCase().trim().split(' ');

                $.each($('#dialogoServiciosMotivos .item'), function (index, element) {
                    let visible = false;

                    $.each(busqueda, function (index, palabra) {
                        let nombre = ($(element).attr('data-motivo-nombre') || '').toLowerCase();
                        let keywords = ($(element).attr('data-keywords') || '').toLowerCase();

                        if (nombre != undefined && nombre.indexOf(palabra) != -1) {
                            visible = true;
                        }

                        if (keywords != undefined && keywords.indexOf(palabra) != -1) {
                            visible = true;
                        }
                    });

                    if (visible) {
                        $(element).removeClass('oculto');
                    } else {
                        $(element).addClass('oculto');
                    }
                });
            });

            setTimeout(function () {
                $(dialogo).find('input').focus();
            }, 100);
        }
    });
}

function seleccionarServicio(id) {
    let servicio = $.grep(servicios, function (element) { return element.Id === id })[0];
    if (servicio === undefined) return;

    idServicio = servicio.Id;

    $('#paso1').addClass('cargando');
    ajaxBuscarMotivos(id)
        .then(function (data) {
            motivosActuales = data;

            $('#paso1').removeClass('cargando');

            // Cargo servicio
            $('.contenedor_ServicioSeleccionado .nombre').text(toTitleCase(servicio.Nombre));

            // Cargo motivos principales
            let contador = 0;

            $('.contenedor_MotivosPrincipales').empty();
            $.each(motivosActuales, function (index, element) {
                if (element.Principal == true && contador < 3) {
                    let html = getHtmlMotivoPrincipal(element);
                    $(html).appendTo($('.contenedor_MotivosPrincipales'));
                    contador++;
                }
            });

            $('.texto_SeleccioneMotivo').text('Motivos principales de ' + toTitleCase(servicio.Nombre))

            if (true || contador === 0) {
                $('.texto_SeleccioneMotivo').hide();
            } else {
                $('.texto_SeleccioneMotivo').show();
            }

            // Cambio de pagina
            $('.btn-serviciosMotivos').hide();
            $('#contenedor_SeleccionarServicio').fadeOut(300, function () {
                $('.btn-serviciosMotivos').fadeOut(300);
                $('#contenedor_SeleccionarMotivo').fadeIn(300);
            });
        })
        .catch(function (error) {
            $('#paso1').removeClass('cargando');
            mostrarMensaje('Error', error);
        })
}

function seleccionarMotivo(id) {
    let entity = $.grep(motivosActuales, function (element) { return element.Id === id })[0];
    if (entity === undefined) return;

    idMotivo = entity.Id;

    verificarBtnFinalizar();

    // Cargo motivo
    $('.contenedor_MotivoSeleccionado .nombre').text(toTitleCase(entity.Nombre));

    // Cambio de pagina
    $('#contenedor_SeleccionarMotivo').fadeOut(300, function () {
        $('#contenedor_InsertarDescripcion').fadeIn(300);
    });
}

function seleccionarServicioMotivo(entity) {
    idServicio = entity.ServicioId;
    idMotivo = entity.MotivoId;

    verificarBtnFinalizar();

    // Cargo servicio y motivo
    $('.contenedor_ServicioSeleccionado .nombre').text(toTitleCase(entity.ServicioNombre));
    $('.contenedor_MotivoSeleccionado .nombre').text(toTitleCase(entity.MotivoNombre));

    // Cambio de pagina
    $('.btn-serviciosMotivos').hide();
    $('#contenedor_SeleccionarServicio').fadeOut(300, function () {
        $('#contenedor_SeleccionarMotivo').fadeOut(300, function () {
            $('#contenedor_InsertarDescripcion').fadeIn(300);
        });
    });
}

function insertarDescripcion() {
    descripcion = $('#input_Descripcion').val().trim().replace(/\n\s*\n\s*\n/g, '\n\n');

    verificarBtnFinalizar();

    if (descripcion.length < DESCRIPCION_MIN_L) {
        $('#paso1 .indicador').removeClass('completo');
        $('#paso1 .btn-siguiente').addClass('btn-gris');
        return;
    }

    $('#paso1 .indicador').addClass('completo');
    $('#paso1 .btn-siguiente').removeClass('btn-gris');
}

function getHtmlServicioPrincipal(entity) {
    var html = $($('#template_ServicioPrincipal').html());

    $(html).find('i').css('background-color', entity.Color);
    $(html).find('i').css('background-image', 'url(' + entity.UrlIcono + ')');
    $(html).find('label').text(toTitleCase(entity.Nombre));

    $(html).click(function () {
        seleccionarServicio(entity.Id);
    });

    return html;
}

function getHtmlMotivoPrincipal(entity) {
    var html = $($('#template_MotivoPrincipal').html());
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));

    $(html).click(function () {
        seleccionarMotivo(entity.Id);
    });

    return html;
}

function getHtmlServicioDialogo(entity) {
    var html = $($('#template_ServicioDialogo').html());
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));

    $(html).click(function () {
        seleccionarServicio(entity.Id);
        cerrarDialogo('dialogoServicios');
    });

    return html;
}

function getHtmlMotivoDialogo(entity) {
    var html = $($('#template_MotivoDialogo').html());
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));
    $(html).attr('data-keywords', entity.Keywords);
    if (entity.Principal == true) {
        $(html).addClass('principal');
    }

    $(html).click(function () {
        seleccionarMotivo(entity.Id);
        cerrarDialogo('dialogoMotivos');
    });

    return html;
}

function getHtmlServicioMotivoDialogo(entity) {
    var html = $($('#template_ServicioMotivoDialogo').html());
    $(html).find('.motivoNombre').text(toTitleCase(entity.MotivoNombre));
    $(html).find('.servicioNombre').text('Servicio: ' + toTitleCase(entity.ServicioNombre));
    $(html).attr('data-motivo-nombre', entity.MotivoNombre);
    $(html).attr('data-keywords', entity.MotivoKeywords);

    $(html).click(function () {
        seleccionarServicioMotivo(entity);
        cerrarDialogo('dialogoServiciosMotivos');
    });

    return html;
}

function ajaxBuscarMotivos(idServicio) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioMotivo.asmx/GetByIdServicio'),
            Data: { id: idServicio },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error)
                    return;
                }
                callback(result.Return);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        })
    });
}


// Paso 2
function initPaso2() {
    initMapa();

    // Info
    $('#paso2 .card-info > label').html(INFO_UBICACION);

    // Event
    $('#paso2 > .content-header').click(abrirPaso2);
    $('.btn-cancelarUbicacion').click(reiniciarPaso2);
    $('#paso2 .btn-siguiente').click(siguientePaso2);

    $('.btn-ubicacion, .link-ubicacion, .mapa').click(dialogoUbicacion);
    $('.btn-ubicacionObservaciones, .link-ubicacionObservaciones').click(dialogoUbicacionObservaciones);
}

function initMapa() {
    ControlMapa_Init({
        Buscar: true,
        BotonCpcs: false,
        BotonBarrios: false,
        BotonEjido: false,
        ResaltarAlHacerClick: false,
        OnMapReady: function (map) {
            mapa = map;
        },
        OnMarcador: function (marcador, data) {
            let observaciones;
            let editable;

            if (ubicacion != undefined) {
                observaciones = ubicacion.ObservacionesVieja;
            }

            ubicacion = data;

            if (observaciones != undefined) {
                ubicacion.ObservacionesVieja = observaciones;
            }
        },
        OnValidarResaltarBarrio: function (info) {
            return false;
        },
        OnValidarResaltarCpc: function (info) {
            return false;
        }
    });
}

function abrirPaso2() {
    if (!validarPaso1(true)) return;

    $('.card-detalle').removeClass('seleccionada');
    $('#paso2').addClass('seleccionada');
}

function reiniciarPaso2() {
    ubicacion = undefined;
    ControlMapa_Limpiar(true);

    verificarBtnFinalizar();

    $('#paso2 .indicador').removeClass('completo');
    $('#contenedor_UbicacionSeleccionada').fadeOut(300, function () {
        $('#contenedor_UbicacionNoSeleccionada').fadeIn(300);
    });
}

function siguientePaso2() {
    if (!validarPaso2(true)) return;
    abrirPaso3();
}

function validarPaso2(conMensajes) {
    if (!validarPaso1(conMensajes)) return false;

    if (ubicacion === undefined) {
        if (conMensajes) mostrarMensaje('Ingrese una ubicación');
        return false;
    }

    return true;
}

function dialogoUbicacion() {
    crearDialogo({
        Id: 'dialogoUbicacion',
        Class: 'dialogo-ubicacion',
        Titulo: 'Seleccionar ubicación',
        Html: $('#template_DialogoUbicacion').html(),
        OnShow: function (dialogo) {
            $(dialogo).find('.contenedor-mapa').append($('#contenedor-mapa > div'));

            if (ubicacion !== undefined && ubicacion.Observaciones != undefined) {
                ubicacion.Observaciones = undefined;
            }
        },
        OnHide: function (dialogo) {
            $(dialogo).find('.contenedor-mapa > div').appendTo($('#contenedor-mapa'));
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Confirmar',
                AutoCerrar: false,
                OnClick: function (dialogo, idDialogo) {
                    if (ubicacion == undefined) {
                        top.mostrarMensaje('Alerta', 'Indique la ubicación de su requerimiento');
                        return;
                    }

                    if (ubicacion.Observaciones == undefined) {
                        dialogoUbicacionObservaciones();
                        return;
                    } else {
                        ubicacion.ObservacionesVieja = undefined;
                    }

                    cerrarDialogo(idDialogo);
                    informarUbicacion(ubicacion);
                }
            }
        ]
    })
}

function dialogoUbicacionObservaciones() {
    crearDialogo({
        Id: 'dialogoUbicacionObservaciones',
        Class: 'dialogo-ubicacionObservaciones',
        Titulo: 'Observaciones del domicilio',
        Fullscreen: false,
        Html: $('<textarea placeholder="Indique información complementaria sobre el domicilio seleccionado. Ejemplo: &quot;Manzana J Casa 3&quot; o bien &quot;Frente a un local comercial&quot;"></textarea>'),
        OnShow: function (dialogo) {
            if (ubicacion.ObservacionesVieja != undefined) {
                $('#dialogoUbicacionObservaciones textarea').val(ubicacion.ObservacionesVieja);
            }

            $(dialogo).find('textarea').trigger('focus');
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                AutoCerrar: false,
                OnClick: function (dialogo, idDialogo) {
                    let observaciones = $(dialogo).find('textarea').val().trim();
                    if (observaciones == "") {
                        top.mostrarMensaje('Alerta', 'Ingrese la descripción');
                        return;
                    }

                    ubicacion.Observaciones = observaciones.replace(/\n\s*\n\s*\n/g, '\n\n');
                    ubicacion.ObservacionesVieja = ubicacion.Observaciones;

                    informarUbicacion(ubicacion);

                    cerrarDialogo('dialogoUbicacionObservaciones');
                    cerrarDialogo('dialogoUbicacion');
                }
            }
        ]
    })
}

function informarUbicacion(data) {
    let latitud = data.Latitud.replace(',', '.');
    let longitud = data.Longitud.replace(',', '.');

    verificarBtnFinalizar();

    $('#contenedor_UbicacionNoSeleccionada').fadeOut(300, function () {
        $('#paso2 .indicador').addClass('completo');
        $('#contenedor_UbicacionSeleccionada').fadeIn(300);
    });

    let urlMaps = 'https://maps.googleapis.com/maps/api/staticmap?center=&zoom=16&scale=1&size=600x300&maptype=roadmap&format=png&visual_refresh=true&markers=size:mid%7Ccolor:0xff0000%7Clabel:1%7C' + latitud + ',+' + longitud + '&key=""';

    $('#contenedor_UbicacionSeleccionada .mapa').css('background-image', 'url(' + urlMaps + ')');
    $('#contenedor_UbicacionSeleccionada ._direccion').html((data.Sugerido == true ? 'Aproximado a ' : '') + data.Direccion);
    $('#contenedor_UbicacionSeleccionada ._observaciones').html(toTitleCase(data.Observaciones));
    $('#contenedor_UbicacionSeleccionada ._barrio').html(toTitleCase(data.Barrio.Nombre));
    $('#contenedor_UbicacionSeleccionada ._CPC').html('N° ' + data.Cpc.Numero + ' - ' + toTitleCase(data.Cpc.Nombre));

    if (ubicacion.ObservacionesVieja !== undefined) {
        $('.editar-observaciones').show();
    } else {
        $('.editar-observaciones').hide();
    }
}


// Paso 3
function initPaso3() {
    //Info
    $('#paso3 .card-info > label').html(INFO_FOTO);

    $('#paso3 > .content-header').click(abrirPaso3);
    $('.btn-cancelarImagen').click(reiniciarPaso3);

    $('.btn-imagen').click(agregarImagen);
    $('#input_Imagen').on('change', seleccionarImagen);
}

function abrirPaso3() {
    if (!validarPaso2(true)) return;

    $('.card-detalle').removeClass('seleccionada');
    $('#paso3').addClass('seleccionada');
    $('#paso3 .indicador').addClass('completo');
}

function reiniciarPaso3() {
    imagen = undefined;
    $('#input_Imagen').val('');

    $('#paso3 .indicador').removeClass('completo');
    $('#contenedor_ImagenAgregada').fadeOut(300, function () {
        $('#contenedor_ImagenNoAgregada').fadeIn(300);
    });
}

function validarPaso3(conMensajes) {
    return true;
}

function agregarImagen() {
    $('#input_Imagen').val('');
    $('#input_Imagen').trigger('click');
};

function seleccionarImagen() {
    var files = this.files;
    if (files == undefined || files.length == 0) {
        return;
    }

    var file = files[0];
    var fr = new FileReader();
    $('#paso3').addClass('cargando');
    fr.onload = function (e) {
        let content = e.target.result;
        achicarImagen(content, 500)
            .then(function (imagenChica) {
                imagen = imagenChica;

                $('#paso3').removeClass('cargando');

                $('#contenedor_ImagenAgregada .content-imagen').css('background-image', 'url(' + imagenChica + ')');

                $('#contenedor_ImagenNoAgregada').fadeOut(300, function () {
                    $('#contenedor_ImagenAgregada').fadeIn(300);
                });

            });

    }
    fr.readAsDataURL(file);
}


// Confirmacion
function initConfirmacion() {
    initAnimacionConfirmacion();

    // Event
    $('.btn-finalizar').click(abrirConfirmacion);
    $('.btn-verDetalle').click(verDetalle);
}

function initAnimacionConfirmacion() {
    animacion = lottie.loadAnimation({
        container: document.getElementById("lottie"),
        renderer: "svg",
        loop: false,
        autoplay: false,
        rendererSettings: {
            preserveAspectRatio: "xMidYMid meet"
        },
        path: ResolveUrl('~/Resources/Anims/anim_success.json'),
    });
}

function abrirConfirmacion() {
    if (!validarConfirmacion(true)) return;

    let _servicio = $('#contenedor_InsertarDescripcion .contenedor_ServicioSeleccionado .nombre').text();
    let _motivo = $('#contenedor_InsertarDescripcion .contenedor_MotivoSeleccionado .nombre').text();
    let _ubicacion = (ubicacion.Sugerido === true ? 'Aproximado a ' : '') + ubicacion.Direccion;
    let _imagen = imagen !== undefined ? 'Una imagen adjuntada' : 'No adjuntó imagen';

    $('#template_DialogoConfirmacion ._servicio').html(toTitleCase(_servicio));
    $('#template_DialogoConfirmacion ._motivo').html(toTitleCase(_motivo));
    $('#template_DialogoConfirmacion ._descripcion').html(descripcion);
    $('#template_DialogoConfirmacion ._ubicacion').html(_ubicacion);
    $('#template_DialogoConfirmacion ._imagen').html(_imagen);

    dialogoConfirmacion();
}

function validarConfirmacion(conMensajes) {
    if (!validarPaso1(conMensajes)) return false;
    if (!validarPaso2(conMensajes)) return false;
    if (!validarPaso3(conMensajes)) return false;

    return true;
}

function verificarBtnFinalizar() {
    if (validarConfirmacion(false)) {
        $('.btn-finalizar').removeClass('btn-gris');
    } else {
        $('.btn-finalizar').addClass('btn-gris');
    }
}

function dialogoConfirmacion() {
    crearDialogo({
        Id: 'dialogoConfirmacion',
        Class: 'dialogo-confirmacion',
        Titulo: 'Confirmar nuevo requerimiento',
        Fullscreen: false,
        Html: $('#template_DialogoConfirmacion').html(),
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                AutoCerrar: false,
                OnClick: function (dialogo, idDialogo) {
                    mostrarCargando(true);

                    ajaxConfirmacion(comandoConfirmacion())
                        .then(function (data) {
                            cerrarDialogo(idDialogo);

                            mostrarCargando(false);

                            setTimeout(function () {
                                animacion.play();
                            }, 300);

                            idRequerimiento = data.Id;

                            $('#panelExito').addClass('visible');
                            $('#panelExito .numero').html(data.Numero + '/' + data.Año);
                            $('#panelExito .email').html('Se ha enviado un e-mail a ' + usuarioLogeado.Email + ' con el comprobante de la operación');

                            ajaxEnviarComprobante(idRequerimiento);
                        })
                        .catch(function (error) {
                            mostrarCargando(false);
                            mostrarMensaje('Error', error);
                        });
                }
            }
        ]
    })
}

function verDetalle() {
    if (idRequerimiento !== undefined) {
        redirigir('DetalleRequerimiento?id=' + idRequerimiento);
    }
}

function comandoConfirmacion() {
    let comando = {};
    comando.IdMotivo = idMotivo;
    comando.Descripcion = $('#input_Descripcion').val();
    comando.Domicilio = {
        Direccion: ubicacion.Direccion,
        Observaciones: ubicacion.Observaciones,
        Latitud: ubicacion.Latitud.replace(',', '.'),
        Longitud: ubicacion.Longitud.replace(',', '.')
    };
    comando.Imagen = imagen;
    return comando;
}

function ajaxConfirmacion(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioRequerimiento.asmx/Insertar'),
            Data: { comando: comando },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                idServicio = undefined;
                idMotivo = undefined;
                descripcion = undefined;
                ubicacion = undefined;
                imagen = undefined;

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        });
    });
}

function ajaxEnviarComprobante(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioRequerimiento.asmx/EnviarEmailComprobante'),
            Data: { id: id },
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