try {
    $.fn.jAlert.defaults.size = 'lg';
    $.fn.jAlert.defaults.class = 'no-padding';
    $.fn.jAlert.defaults.closeBtn = 'false';
    $.fn.jAlert.defaults.closeOnEsc = 'false';
} catch (e) {

}

function onDialogOpen(jAlert) {
    $(jAlert).find('.ja_body').AgregarIndicadorCargando({ Opaco: true });
}

function onDialogoClose(jAlert) {

}

jQuery.fn.CerrarDialogo = function () {
    $(this).parents('.ja_wrap').removeClass('open');

    var jAlert = $(this);
    $(jAlert).closeAlert();
}

jQuery.fn.MostrarDialogoCargando = function (mostrar, animar) {
    var jAlert = $(this);

    if (animar) {
        if (mostrar) {
            $(getIndicadorCargando(jAlert)).stop(true, true).fadeIn(300);
        } else {
            $(getIndicadorCargando(jAlert)).stop(true, true).fadeOut(300);
        }
    } else {
        if (mostrar) {
            $(getIndicadorCargando(jAlert)).stop(true, true).fadeIn();
        } else {
            $(getIndicadorCargando(jAlert)).stop(true, true).fadeOut();
        }
    }
}

jQuery.fn.AnimarDialogoSalida = function (valores) {
    $(this).parents('.ja_wrap_black').removeClass('open');
}

function getIndicadorCargando(jAlert) {
    return $(jAlert).find('.ja_body').GetIndicadorCargando();
}

function crearDialogoIFrame(valores) {

    //ID
    var id;
    if ('Id' in valores) {
        id = valores.Id;
    } else {
        id = 'jAlert' + (new Date().getTime());
    }

    //Titulo
    if (!('Titulo' in valores)) {
        valores.Titulo = null;
    }


    //Height
    var porcentaje = 0.8;
    if ('Alto' in valores) {
        porcentaje = valores.Alto;
    }
    var h = ((top.$('body').height() * porcentaje) - 94) + 'px';
    if ('Height' in valores) {
        h = valores.Height;
    }

    //var jAlert;
    //var iFrame;

    var botones = [];

    $.each(valores.Botones, function (index, btn) {
        if (!('CerrarDialogo' in btn)) {
            btn.CerrarDialogo = true;
        }

        var btnClass = 'btn waves-effect';
        if ('Class' in btn) {
            btnClass = btnClass + ' ' + btn.Class;
        }

        botones.push({
            id: btn.Id,
            text: btn.Texto,
            closeAlert: false,
            class: btnClass,
            onClick: function () {
                if ('OnClick' in btn) {
                    var iFrame = $(jAlert).find('iframe')[0].contentWindow;

                    btn.OnClick(jAlert, iFrame);
                }

                if (btn.CerrarDialogo) {
                    $(jAlert).CerrarDialogo();
                }

            }
        })
    });

    top.$.jAlert({
        id: id,
        title: valores.Titulo,
        iframe: valores.Url,
        iframeheight: h,
        closeBtn: false,
        closeOnEsc: false,
        onOpen: function (jAlert) {
            onDialogOpen(jAlert);

            var ifr = $($(jAlert).find('iframe'));
            if (ifr == undefined) return;

            $(jAlert).find('iframe').off('load');
            $(jAlert).find('iframe').on('load', function () {

                $(overlay).fadeOut(300, function () {
                    $(overlay).remove();
                });

                var iFrame = $(jAlert).find('iframe')[0].contentWindow;

                var ocultar = true;
                if ('OnLoad' in valores) {
                    var result = valores.OnLoad(jAlert, iFrame);
                    if (result != undefined) {
                        ocultar = result;
                    }
                }

                if (ocultar) {
                    $(jAlert).MostrarDialogoCargando(false, true);
                }
            });

        },
        onClose: function (jAlert) {
            onDialogoClose(jAlert);

            $(overlay).fadeOut(300, function () {
                $(overlay).remove();
            });

            if ('OnClose' in valores) {
                valores.OnClose(jAlert);
            }
        },
        btns: botones
    });

    var jAlert = top.$('#' + id);

    if ('Ancho' in valores) {
        $(jAlert).css('width', (valores.Ancho * 100) + '%');
    }

    var overlay = $('<div>');
    $(overlay).css('width', '100%');
    $(overlay).css('height', '100%');
    $(overlay).css('background-color', 'transparent');
    $(overlay).css('position', 'absolute');
    $(overlay).css('left', '0');
    $(overlay).css('top', '0');
    $(overlay).css('z-index', '999999');
    $(overlay).css('opacity', '0');

    var btnCerrar = $('<a>', {
        'class': 'btn-flat btn-redondo no-select waves-effect',
    });
    $(btnCerrar).css('cssText', 'background-color: transparent !important');
    $(btnCerrar).css('position', 'absolute');
    $(btnCerrar).css('right', '16px');
    $(btnCerrar).css('top', '16px');

    $(btnCerrar).appendTo(overlay);

    var icono = $('<i>', {
        'class': 'material-icons',
        text: 'close'
    });
    $(icono).css('color', 'white');
    $(icono).css('line-height', '36px');
    $(icono).css('width', '36px');

    $(icono).appendTo(btnCerrar);

    $(btnCerrar).click(function () {
        $(jAlert).CerrarDialogo();
    });
    top.$('body').append(overlay);
    $(overlay).animate({ opacity: 1 }, 300);

    $(jAlert).parents('.ja_wrap').addClass('open');
}

function crearDialogoHTML(valores) {
    //ID
    var id;
    if ('Id' in valores) {
        id = valores.Id;
    } else {
        id = 'jAlert' + (new Date().getTime());
    }

    //Titulo
    if (!('Titulo' in valores)) {
        valores.Titulo = null;
    }

    //Fullscreen
    var full = false;
    if ('Fullscreen' in valores) {
        full = valores.Fullscreen;
    }

    var jAlert;

    var botones = [];

    $.each(valores.Botones, function (index, btn) {
        if (!('CerrarDialogo' in btn)) {
            btn.CerrarDialogo = true;
        }

        var btnClass = 'btn waves-effect';
        if ('Class' in btn) {
            btnClass = btnClass + ' ' + btn.Class;
        }

        botones.push({
            id: btn.Id,
            text: btn.Texto,
            closeAlert: false,
            class: btnClass,
            onClick: function () {
                var jAlert = top.$('#' + id);

                if ('OnClick' in btn) {
                    btn.OnClick(jAlert);
                }

                if (btn.CerrarDialogo) {
                    $(jAlert).CerrarDialogo();
                }
            }
        })
    });

    top.$.jAlert({
        id: id,
        title: valores.Titulo,
        content: valores.Content,
        closeBtn: false,
        closeOnEsc: false,
        fullscreen: full,
        onOpen: function (jAlert) {
            onDialogOpen(jAlert);
            $(jAlert).MostrarDialogoCargando(false, true);

            if ('OnLoad' in valores) {
                valores.OnLoad(jAlert);
            }
        },
        onClose: function (jAlert) {
            onDialogOpen(jAlert);

            if ('OnClose' in valores) {
                valores.OnClose(jAlert);
            }
        },
        btns: botones
    });

    top.$('#' + id).parents('.ja_wrap').addClass('open html');
}

function crearDialogoCargando(valores) {
    if (!('OnLoad' in valores)) {
        valores.OnLoad = function () { };
    }

    crearDialogoHTML({
        Id: 'dialogoCargando',
        Content: '<div style="height:100px"></div>',
        OnLoad: function (jAlert, iFrame) {
            $(jAlert).MostrarDialogoCargando(true, false);
            valores.OnLoad(jAlert);
        }
    });

}

function crearDialogoConfirmacion(valores) {

    if (!('CallbackPositivo' in valores)) {
        valores.CallbackPositivo = function () { };
    }

    if (!('CallbackNegativo' in valores)) {
        valores.CallbackNegativo = function () { };
    }

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Texto' in valores)) {
        valores.Texto = "Sin texto";
    }

    if (!('TextoBotonAceptar' in valores)) {
        valores.TextoBotonAceptar = 'Aceptar';
    }

    if (!('TextoBotonCancelar' in valores)) {
        valores.TextoBotonCancelar = 'Cancelar';
    }

    crearDialogoHTML({
        Content: '<div class="margin"><label>' + valores.Texto + '</label></div>',
        Botones: [
            {
                Texto: valores.TextoBotonCancelar,
                OnClick: function () {
                    valores.CallbackNegativo();
                }
            },
            {
                Texto: valores.TextoBotonAceptar,
                Class: 'colorExito',
                OnClick: function () {
                    valores.CallbackPositivo();
                }
            }
        ]
    })
}

jQuery.fn.MenuFlotante = function (valores) {

    if (valores == undefined) valores = {};
    if (!('e' in valores)) valores.e = undefined;
    if (!('TodoElAncho') in valores) valores.TodoElAncho = false;
    if (!('TodoElAlto') in valores) valores.TodoElAlto = false;

    if (valores.e != undefined) {
        valores.TodoElAncho = false;
        valores.TodoElAlto = false;
    }

    //Soluciono el offset
    var iframe = $(this)[0].ownerDocument.defaultView.frameElement;
    var xOffset = 0;
    var yOffset = 0;
    while (iframe != undefined) {
        xOffset += $(iframe).offset().left;
        yOffset += $(iframe).offset().top;
        iframe = $(iframe)[0].ownerDocument.defaultView.frameElement;
    }

    var pl = parseInt($(this).css('padding-left').replace("px", ""));
    var pr = parseInt($(this).css('padding-right').replace("px", ""));
    var pb = parseInt($(this).css('padding-bottom').replace("px", ""));
    var pt = parseInt($(this).css('padding-top').replace("px", ""));

    var x, y;
    if (valores.e == undefined) {
        if (!('x' in valores) || !('y' in valores)) {
            x = $(this).offset().left + xOffset;
            y = $(this).offset().top + yOffset;
        } else {
            x = valores.X;
            y = valores.Y;
        }
    } else {
        var e = valores.e;
        x = $(this).offset().left + xOffset + e.originalEvent.offsetX;
        y = $(this).offset().top + yOffset + e.originalEvent.offsetY;
    }

    //Defino los tamaños
    var hItem = 48;
    var hMax = 320
    var w = 220;
    if (valores.TodoElAlto) {
        hMax = $(this).height();
    }
    if (valores.TodoElAncho) {
        w = $(this).width();
    }


    //Creo el menu
    var menu = $('<div>');
    var id = new Date().getTime();
    $(menu).prop('id', id);
    $(menu).addClass('menu-flotante');

    $(menu).css('width', w + 'px');
    $(menu).css('max-height', hMax + 'px');

    if (valores.Menu) {
        //Creo los items
        var hCalculado = 0;
        var ul = $('<ul>');
        $(ul).appendTo(menu);

        $.each(valores.Menu, function (index, btn) {

            var visible = true;
            if ('Visible' in btn) {
                if (typeof (btn.Visible) === 'boolean') {
                    visible = btn.Visible;
                } else {
                    visible = btn.Visible();
                }
            }

            if (!visible) {
                return true;
            }

            var separador = false;
            if ('Separador' in btn && btn.Separador) {
                separador = true;
                hCalculado += 1;
            }
            else {
                if (!('Radio' in btn)) {
                    btn.Radio = false;
                }

                if (!('Check' in btn)) {
                    btn.Check = false;
                }

                if (!('Checked' in btn)) {
                    btn.Checked = false;
                }
                var classChecked = btn.Checked ? 'checked' : '';

                if (!('Texto' in btn)) {
                    btn.Texto = 'Sin texto';
                }

                if (!('OnClick' in btn)) {
                    btn.OnClick = function () { };
                }

                hCalculado += hItem;
            }

            if (!('Id' in btn)) {
                btn.Id = new Date().getTime();
            }

            var li = $('<li>');
            $(li).appendTo(ul);
            $(li).addClass('menu-item waves-effect');
            $(li).attr('id', btn.Id);
            if (separador) {
                $(li).addClass('menu-separador');
            }
            $(li).attr('index', index);

            if (!separador) {
                //Radio
                if (btn.Radio) {
                    var radio = $('<p><input name="group1" type="radio" id="' + id + '" class="with-gap" ' + classChecked + '/><label for="' + id + '" style="white-space: nowrap; text-overflow: ellipsis; overflow: hidden;">' + btn.Texto + '</label></p>');
                    $(radio).appendTo(li);
                    return true;
                }

                //Check
                if (btn.Check) {
                    var check = $('<p><input  type="checkbox" id="' + id + '" ' + classChecked + '/><label for="' + id + '" style="white-space: nowrap; text-overflow: ellipsis; overflow: hidden;">' + btn.Texto + '</label></p>');
                    $(check).appendTo(li);
                    return true;
                }

                //Texto
                if ('Icono' in btn) {
                    var icono = $('<i class="material-icons">' + btn.Icono + '</i>')
                    $(icono).css('margin-right', '0.5rem');
                    $(icono).appendTo(li);
                }
                var titulo = btn.Texto;
                if (typeof titulo != 'string') {
                    titulo = btn.Texto(data);
                }
                var texto = $('<label>');
                $(texto).addClass('texto');
                $(texto).text(titulo);
                $(texto).appendTo(li);
            }
        });
    }

    if (valores.Child) {
        $(menu).append(valores.Child);
    }

    //Limito el alto
    if (hCalculado < hMax) {
        $(menu).css('height', (hCalculado) + 'px');
    } else {
        hCalculado = hMax;
    }

    //Obtengo la posicion
    var posicion_x = "izquierda";
    var posicion_y = "abajo";
    if ('PosicionX' in valores) {
        posicion_x = valores.PosicionX;
    }
    if ('PosicionY' in valores) {
        posicion_y = valores.PosicionY;
    }

    //Calculo la posicion del MenuFlotante
    var objeto = calcularClaseMenuFoltante({
        Elemento: $(this),
        e: valores.e,
        PosicionX: posicion_x,
        PosicionY: posicion_y,
        X: x,
        Y: y,
        PaddingLeft: pl,
        PaddingRight: pr,
        PaddingTop: pt,
        PaddingBottom: pb,
        MenuFlotanteW: w,
        MenuFlotanteH: hCalculado
    });

    if ('MarginTop' in valores) {
        objeto.Y += valores.MarginTop;
    }



    $(menu).css('left', (objeto.X) + 'px');
    $(menu).css('top', (objeto.Y) + 'px');
    $(menu).addClass(objeto.Clase);

    if (valores.TodoElAncho) {
        $(menu).addClass('todo-el-ancho');
    }


    var fondo = $('<div>');
    $(fondo).addClass('menu-flotante-fondo waves-effect');
    $(fondo).append($('<div>'));
    $(fondo).click(function () {
        $(menu).removeClass('abierto');
        $(fondo).removeClass('abierto');

        setTimeout(function () {
            $(fondo).remove();
            $(menu).remove();
        }, 300);
    });

    //deshabilito click derecho en el fonmdo
    $(fondo).bind("contextmenu", function (event) {
        event.preventDefault();
    });

    top.$('body').append(fondo);
    top.$('body').append(menu);


    if (valores.OnReady) {
        valores.OnReady($(menu));
    }

    setTimeout(function () {
        $(menu).addClass('abierto');
        $(fondo).addClass('abierto');
    }, 100);
    top.$('#' + id).find('.menu-item').click(function () {
        var index = $(this).attr('index');

        var menuItem = valores.Menu[index];
        if ('Separador' in menuItem && menuItem.Separador) {
            //No hago nada
        } else {
            menuItem.OnClick();
            top.$(fondo).trigger('click');
        }
    });
}

function calcularClaseMenuFoltante(valores) {
    var element = valores.Elemento;
    var e = valores.e;
    var posicion_x = valores.PosicionX;
    var posicion_y = valores.PosicionY;
    var x = valores.X;
    var y = valores.Y;
    var pl = valores.PaddingLeft;
    var pr = valores.PaddingRight;
    var pt = valores.PaddingTop;
    var pb = valores.PaddingBottom;
    var w = valores.MenuFlotanteW;
    var h = valores.MenuFlotanteH;

    var clase;
    if (posicion_x == "izquierda") {
        if (posicion_y == "abajo") {
            clase = "abajo-izquierda";
        } else {
            clase = "arriba-izquierda";
        }
    } else {
        if (posicion_y == "abajo") {
            clase = "abajo-derecha";
        } else {
            clase = "arriba-derecha";
        }
    }

    var wElement = $(element).width();
    var hElement = $(element).height();

    switch (clase) {
        case 'abajo-izquierda': {
            //Como es para la izquierda, le resto a la posicion el ancho de la ventana
            x -= w;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                x += wElement + pl + pr;
            }
        } break;

        case 'abajo-derecha': {
            //No va nada aca porque es la posicion por defecto
        } break;

        case 'arriba-izquierda': {
            //Como es para la izquierda, le resto a la posicion el ancho de la ventana
            x -= w;

            //Como es para arriba, le resto a la posicion el alto de la ventana
            y -= h;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                x += wElement + pl + pr;
                y += hElement + pt + pb;
            }
        } break;

        case 'arriba-derecha': {
            //Como es para arriba, le resto a la posicion el alto de la ventana
            y -= h;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                y += hElement + pt + pb;
            }
        } break;
    }

    if (x < 0) {
        valores.PosicionX = "derecha";
        return calcularClaseMenuFoltante(valores);
    } else {
        var maxX = top.$('body').width();
        if (x + w > maxX) {
            valores.PosicionX = "izquierda";
            return calcularClaseMenuFoltante(valores);
        }
    }

    if (y < 0) {
        valores.PosicionY = "abajo";
        return calcularClaseMenuFoltante(valores);
    } else {
        var maxY = top.$('body').height();

        if (y + h > maxY) {
            valores.PosicionY = "arriba";
            return calcularClaseMenuFoltante(valores);
        }
    }

    var objeto = {};
    objeto.Clase = clase;
    objeto.X = x;
    objeto.Y = y;
    return objeto;
}

/* Usuario */

function crearDialogoUsuarioDetalle(valores) {
    if (!('Id' in valores)) return false;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    var id = valores.Id;
    var url = ResolveUrl('~/Paginas/IFrames/IUsuarioDetalle.aspx?Id=' + id);
    crearDialogoIFrame({
        Titulo: '<label>Detalle de Usuario</label>',
        Url: url,
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    });
}

function crearDialogoUsuarioEditar(valores) {
    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar usuario</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IUsuarioNuevo.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            //Editar
            iFrame.setOnEditarListener(function (app) {
                mostrarMensajeExito('Usuario editado correctamente');
                valores.Callback(app);
                $(jAlert).CerrarDialogo();
            });

            //Mensajes 
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    iFrame.editar();
                }
            }
        ]
    });
}

function crearDialogoUsuarioDarDeBaja(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de Baja Usuario</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer dar de baja el usuario?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorError',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/DarDebaja'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                if (DEBUG) {
                                    console.log('Error dando de baja el usuario');
                                    console.log(result);
                                }
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Usuario dado de baja correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                            if (DEBUG) {
                                console.log('Error dando de baja el usuario');
                                console.log(result);
                            }
                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoUsuarioActivar(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Activar Usuario</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer activar el usuario?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/Activar'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                if (DEBUG) {
                                    console.log('Error activando el usuario');
                                    console.log(result);
                                }
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Usuario activado correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error activando el usuario');
                                console.log(result);
                            }

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoUsuarioReiniciarContraseña(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Reiniciar contraseña</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer reiniciar la contraseña del usuario?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/ReiniciarPassword'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                if (DEBUG) {
                                    console.log('Error reiniciando la contraseña del usuario');
                                    console.log(result);
                                }
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Contraseña reiniciada correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error reiniciando la contraseña del usuario');
                                console.log(result);
                            }

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoUsuarioListado(valores) {
    //Seleccioanr
    if (!('Seleccionar' in valores)) {
        valores.Seleccionar = false;
    }

    //Busqueda
    if (!('Busqueda' in valores)) {
        valores.Busqueda = false;
    }

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Armo la url
    var url = '~/Paginas/IFrames/IUsuarioListado.aspx';
    var params = [];
    if (valores.Todo) {
        params.push('Todo=true');
    }
    if (valores.Seleccionar) {
        params.push('Seleccionar=true');
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }

    if (!('Ids' in valores)) {
        valores.Ids = [];
    }

    crearDialogoIFrame({
        Titulo: 'Usuarios',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame) {

            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback por si quiero seleccionar
            if (valores.Seleccionar) {
                iFrame.setOnSeleccionarListener(function (data) {
                    valores.CallbackSeleccionar(data);
                    $(jAlert).CerrarDialogo();
                });
            }

            //Busqueda
            iFrame.setBusquedaVisible(valores.Busqueda);

            //Pre cargo usuarios
            iFrame.setUsuarios(valores.Ids);
        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    })
}

/* Usuario Cerrojo */

function crearDialogoUsuarioCerrojoDetalle(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de Aplicación</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IUsuarioCerrojoDetalle.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).CerrarDialogo();
                }
            }
        ]
    });
}

function crearDialogoUsuarioCerrojoNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    if (!('Insertar' in valores)) {
        valores.Insertar = true;
    }

    if (!('IdUser' in valores)) {
        console.log('Debe ingresar el idUser');
        return false;
    }

    if (!('IdRolCerrojo' in valores)) {
        console.log('Debe ingresar el IdRolCerrojo');
        return false;
    }

    if (!('IdAplicacion' in valores)) {
        console.log('Debe ingresar el IdAplicacion');
        return false;
    }

    var usuarioCerrojo = {
        IdUser: valores.IdUser,
        IdRolCerrojo: valores.IdRolCerrojo,
        IdAplicacion: valores.IdAplicacion
    };

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/ServicioUsuarioCerrojo.asmx/Insertar'),
                Data: { usuarioCerrojo: usuarioCerrojo },
                OnSuccess: function (result) {
                    $(jAlert).CerrarDialogo();

                    if (!result.Ok) {

                        valores.CallbackMensajes('Error', result.Errores.Mensaje);
                        if (DEBUG) {
                            console.log('Error insertando el rol cerrojo');
                            console.log(result);
                            return;
                        }
                    }

                    valores.CallbackMensajes('Exito', 'Usuario de cerrojo registrado correctamente');
                    valores.Callback(result.Return);
                },
                OnError: function (result) {
                    $(jAlert).CerrarDialogo();
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}

function crearDialogoUsuarioCerrojoDarDeBaja(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de Baja Usuario de Cerrojo</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer dar de baja el usuario de cerrojo?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorError',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServiciousuarioCerrojo.asmx/DarDebaja'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error dando de baja el usuario cerrojo');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Usuario de Cerrojo dado de baja correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error dando de baja el usuario de cerrojo');
                                console.log(result);
                            }

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoUsuarioCerrojoActivar(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Activar Usuario de Cerrojo</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer activar el usuario de cerrojo?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioUsuarioCerrojo.asmx/Activar'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error activando el usuario cerrojo');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Usuario de Cerrojo activado correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error activando el usuario de cerrojo');
                                console.log(result);
                            }

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

/* Rol Cerrojo */

function crearDialogoRolCerrojoListado(valores) {
    //Todo
    if (!('Todo' in valores)) {
        valores.Todo = false;
    }

    //Seleccioanr
    if (!('Seleccionar' in valores)) {
        valores.Seleccionar = false;
    }

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Armo la url
    var url = '~/Paginas/IFrames/IRolCerrojoListado.aspx';
    var params = [];
    if (valores.Todo) {
        params.push('Todo=true');
    }
    if (valores.Seleccionar) {
        params.push('Seleccionar=true');
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }

    crearDialogoIFrame({
        Titulo: '<label>Roles de Cerrojo</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame) {

            iFrame.setOnCargandoListener(function (cargando, menasje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            if (valores.Seleccionar) {
                iFrame.setOnSeleccionarListener(function (data) {
                    valores.CallbackSeleccionar(data);
                    $(jAlert).CerrarDialogo();
                });
            }

            if (!valores.Todo) {
                iFrame.setRolesCerrojo(valores.Ids);
                return false;
            }
        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    })
}

function crearDialogoRolCerrojoListadoLogin(valores) {
    //Todo
    if (!('Todo' in valores)) {
        valores.Todo = false;
    }

    //Seleccioanr
    if (!('Seleccionar' in valores)) {
        valores.Seleccionar = false;
    }

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Armo la url
    var url = '~/Paginas/IFrames/IRolCerrojoListadoLogin.aspx';
    var params = [];
    if (valores.Todo) {
        params.push('Todo=true');
    }
    if (valores.Seleccionar) {
        params.push('Seleccionar=true');
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }

    crearDialogoIFrame({
        Titulo: '<label>Roles de Cerrojo</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame) {

            iFrame.setOnCargandoListener(function (cargando, menasje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            if (valores.Seleccionar) {
                iFrame.setOnSeleccionarListener(function (data) {
                    valores.CallbackSeleccionar(data);
                    $(jAlert).CerrarDialogo();
                });
            }

            iFrame.setOnReadyListener(function () {
                if (!valores.Todo) {
                    iFrame.setRolesCerrojo(valores.Ids);
                }
            });
            if (!valores.Todo) {
                return false;
            }
        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    })
}

/* Aplicacion */

function crearDialogoAplicacionDetalle(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de Aplicación</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IAplicacionDetalle.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).CerrarDialogo();
                }
            }
        ]
    });
}

function crearDialogoAplicacionNuevo() {

}

function crearDialogoAplicacionEditar(valores) {
    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar aplicacion</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IAplicacionNuevo.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            //Editar
            iFrame.setOnEditarListener(function (app) {
                mostrarMensajeExito('Aplicación editada correctamente');
                valores.Callback(app);
                $(jAlert).CerrarDialogo();
            });

            //Mensajes 
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Editar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    iFrame.editar();
                }
            }
        ]
    });
}

function crearDialogoAplicacionDarDeBaja(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de Baja Aplicacion</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer dar de baja la aplicación?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorError',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioAplicacion.asmx/DarDebaja'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error dando de baja la app');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Aplicación dada de baja correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            console.log('Error dando de baja la app');
                            console.log(result);

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoAplicacionActivar(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Activar Aplicacion</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer activar la aplicación?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioAplicacion.asmx/Activar'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error activando la app');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Aplicación activada correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            console.log('Error activando la app');
                            console.log(result);

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoAplicacionBloquear(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var html = "";
    html += "<div class='row'>";
    html += "   <div class='col s12'>";
    html += "       <div class='input-field'>";
    html += "           <input id='input_Motivo' type='text' maxlength='2000' autofocus />"
    html += "           <label for='input_Motivo' class='no-select'>Motivo</label>"
    html += "       </div>"
    html += "   </div>";
    html += "</div>";

    crearDialogoHTML({
        Titulo: '<label>Bloquear Aplicación</label>',
        Content: html,
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    var motivo = $(jAlert).find('#input_Motivo').val().trim();
                    if (motivo == '') {
                        valores.CallbackMensajes('Alerta', 'Debe ingresar el motivo');
                        $(jAlert).find('#input_Motivo').trigger('focus');
                        return;
                    }

                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioAplicacion.asmx/Bloquear'),
                        Data: { id: id, bloquear: true, motivo: motivo },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error bloqueando la app');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Aplicación bloqueada correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            console.log('Error bloqueando la app');
                            console.log(result);

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoAplicacionDesbloquear(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Desbloquear Aplicación</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer desbloquear la aplicación?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioAplicacion.asmx/Bloquear'),
                        Data: { id: id, bloquear: false, motivo: null },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error desbloqueando la app');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Aplicación desbloqueada correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            console.log('Error desbloqueando la app');
                            console.log(result);

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoAplicacionListadoLogin(valores) {

    //Todo
    if (!('Todo' in valores)) {
        valores.Todo = false;
    }

    //Seleccioanr
    if (!('Seleccionar' in valores)) {
        valores.Seleccionar = false;
    }

    //Busqueda
    if (!('Busqueda' in valores)) {
        valores.Busqueda = false;
    }

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Armo la url
    var url = '~/Paginas/IFrames/IAplicacionListadoLogin.aspx';
    var params = [];
    if (valores.Todo) {
        params.push('Todo=true');
    }
    if (valores.Seleccionar) {
        params.push('Seleccionar=true');
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }

    crearDialogoIFrame({
        Titulo: 'Aplicaciones',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {
                //Cargar solo algunos ids
                if (!valores.Todo) {
                    iFrame.setAplicaciones(valores.Ids);
                }
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Busqueda
            iFrame.setBusquedaVisible(valores.Busqueda);

            //Seleccionar
            if (valores.Seleccionar) {
                iFrame.setOnSeleccionarListener(function (data) {
                    valores.CallbackSeleccionar(data);
                    $(jAlert).CerrarDialogo();
                });
            }

        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    })
}

function crearDialogoAplicacionListado(valores) {

    //Todo
    if (!('Todo' in valores)) {
        valores.Todo = false;
    }

    //Seleccionar
    if (!('Seleccionar' in valores)) {
        valores.Seleccionar = false;
    }

    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Busqueda
    if (!('Busqueda' in valores)) {
        valores.Busqueda = false;
    }

    //Armo la url
    var url = '~/Paginas/IFrames/IAplicacionListado.aspx';
    var params = [];
    if (valores.Todo) {
        params.push('Todo=true');
    }
    if (valores.Seleccionar) {
        params.push('Seleccionar=true');
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }

    crearDialogoIFrame({
        Titulo: '<label>Aplicaciones</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {

                //mensaje
                iFrame.setOnMensajeListener(function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                });

                //Cargando
                iFrame.setOnCargandoListener(function (cargando, mensaje) {
                    $(jAlert).MostrarDialogoCargando(cargando, true);
                });

                //Busqueda
                iFrame.setBusquedaVisible(valores.Busqueda);

                //Seleccionar
                if (valores.Seleccionar) {
                    iFrame.setOnSeleccionarListener(function (data) {
                        valores.CallbackSeleccionar(data);
                        $(jAlert).CerrarDialogo();
                    });
                }

                //Cargar solo algunos ids
                if (!valores.Todo) {
                    iFrame.setAplicaciones(valores.Ids);
                }
            });
        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    })
}

/* Area */

function crearDialogoAreaListado(valores) {
    if (!('IdAplicacion' in valores)) {
        valores.IdAplicacion = null;
    }

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Todo
    if (!('Todo' in valores)) {
        valores.Todo = false;
    }

    //Seleccionar
    if (!('Seleccionar' in valores)) {
        valores.Seleccionar = false;
    }

    if (!('Ids' in valores)) {
        valores.Ids = [];
    }

    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Busqueda
    if (!('Busqueda' in valores)) {
        valores.Busqueda = false;
    }

    //Armo la url
    var url = '~/Paginas/IFrames/IAreaListado.aspx';
    var params = [];
    params.push('IdAplicacion=' + valores.IdAplicacion);
    if (valores.Todo) {
        params.push('Todo=true');
    }
    if (valores.Seleccionar) {
        params.push('Seleccionar=true');
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }

    crearDialogoIFrame({
        Titulo: '<label>Areas</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {
                //Busqueda
                iFrame.setBusquedaVisible(valores.Busqueda);

                //Cargar solo algunos ids
                if (!valores.Todo) {
                    iFrame.setAreas(valores.Ids);
                }
            });

            //mensaje
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Seleccionar
            if (valores.Seleccionar) {
                iFrame.setOnSeleccionarListener(function (data) {
                    valores.CallbackSeleccionar(data);
                    $(jAlert).CerrarDialogo();
                });
            }
        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    })
}

function crearDialogoAreaDetalle(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de Area</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IAreaDetalle.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).CerrarDialogo();
                }
            }
        ]
    });
}

function crearDialogoAreaNuevo(valores) {
    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    var url = '~/Paginas/IFrames/IAreaNuevo.aspx';
    var params = [];
    if ('IdPadre' in valores) {
        params.push('IdPadre=' + valores.IdPadre);
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }
    url = ResolveUrl(url);

    crearDialogoIFrame({
        Titulo: '<label>Nueva Area</label>',
        Url: url,
        OnLoad: function (jAlert, iFrame) {
            //Registrar
            iFrame.setOnRegistrarListener(function (app) {
                valores.CallbackMensajes('Exito', 'Area registrada correctamente');
                valores.Callback(app);
                $(jAlert).CerrarDialogo();
            });

            //Mensajes 
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    iFrame.registrar();
                }
            }
        ]
    });
}

function crearDialogoAreaEditar(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar Area</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IAreaNuevo.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            //Registrar
            iFrame.setOnEditarListener(function (app) {
                valores.CallbackMensajes('Exito', 'Area editada correctamente');
                valores.Callback(app);
                $(jAlert).CerrarDialogo();
            });

            //Mensajes 
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    iFrame.editar();
                }
            }
        ]
    });
}

function crearDialogoAreaDarDeBaja(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de Baja Area</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer dar de baja el area?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorError',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioArea.asmx/DarDebaja'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                if (DEBUG) {
                                    console.log('Error dando de baja el area');
                                    console.log(result);
                                }
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Area dada de baja correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error dando de baja el area');
                                console.log(result);
                            }
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoAreaActivar(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Activar Area</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer activar el area?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioArea.asmx/Activar'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                if (DEBUG) {
                                    console.log('Error activando el area');
                                    console.log(result);
                                }
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Area activada correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error activando el area');
                                console.log(result);
                            }
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}


/* Objeto */

function crearDialogoObjetoListado(valores) {
    if (!('IdAplicacion' in valores)) {
        console.log('Debe mandar el id de la app');
        return false;
    }

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Todo
    if (!('Todo' in valores)) {
        valores.Todo = false;
    }

    //Seleccionar
    if (!('Seleccionar' in valores)) {
        valores.Seleccionar = false;
    }

    if (!('Ids' in valores)) {
        valores.Ids = [];
    }

    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Busqueda
    if (!('Busqueda' in valores)) {
        valores.Busqueda = false;
    }

    //Armo la url
    var url = '~/Paginas/IFrames/IObjetoListado.aspx';
    var params = [];
    params.push('IdAplicacion=' + valores.IdAplicacion);
    if (valores.Todo) {
        params.push('Todo=true');
    }
    if (valores.Seleccionar) {
        params.push('Seleccionar=true');
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }

    crearDialogoIFrame({
        Titulo: '<label>Objetos</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {
                //Cargar solo algunos ids
                if (!valores.Todo) {
                    iFrame.setAplicaciones(valores.Ids);
                }
            });

            //Busqueda
            iFrame.setBusquedaVisible(valores.Busqueda);

            //mensaje
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Seleccionar
            if (valores.Seleccionar) {
                iFrame.setOnSeleccionarListener(function (data) {
                    valores.CallbackSeleccionar(data);
                    $(jAlert).CerrarDialogo();
                });
            }
        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    })
}

function crearDialogoObjetoDetalle(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de Objeto</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IObjetoDetalle.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).CerrarDialogo();
                }
            }
        ]
    });
}

function crearDialogoObjetoNuevo(valores) {
    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    var url = '~/Paginas/IFrames/IObjetoNuevo.aspx';
    var params = [];
    if (params.length != 0) {
        url += '?' + params.join('&');
    }
    url = ResolveUrl(url);

    crearDialogoIFrame({
        Titulo: '<label>Objeto Area</label>',
        Url: url,
        OnLoad: function (jAlert, iFrame) {
            //Registrar
            iFrame.setOnRegistrarListener(function (app) {
                valores.CallbackMensajes('Exito', 'Objeto registrado correctamente');
                valores.Callback(app);
                $(jAlert).CerrarDialogo();
            });

            //Mensajes 
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    iFrame.registrar();
                }
            }
        ]
    });
}

function crearDialogoObjetoEditar(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar Objeto</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IObjetoNuevo.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            //Registrar
            iFrame.setOnEditarListener(function (app) {
                valores.CallbackMensajes('Exito', 'Objeto editado correctamente');
                valores.Callback(app);
                $(jAlert).CerrarDialogo();
            });

            //Mensajes 
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Editar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    iFrame.editar();
                }
            }
        ]
    });
}

function crearDialogoObjetoDarDeBaja(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de Baja Objeto</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer dar de baja el objeto?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorError',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioObjeto.asmx/DarDebaja'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                if (DEBUG) {
                                    console.log('Error dando de baja el objeto');
                                    console.log(result);
                                }
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Objeto dado de baja correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error dando de baja el objeto');
                                console.log(result);
                            }
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoObjetoActivar(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Activar Objeto</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer activar el objeto?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioObjeto.asmx/Activar'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                if (DEBUG) {
                                    console.log('Error activando el objeto');
                                    console.log(result);
                                }
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Objeto activado correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error activando el area');
                                console.log(result);
                            }
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoObjetoAcceso(valores) {
    if (!('IdAplicacion' in valores)) return false;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Accesos
    if (!('Accesos' in valores)) {
        valores.Accesos = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/Paginas/IFrames/IObjetoAcceso.aspx?IdAplicacion=' + valores.IdAplicacion),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {
                iFrame.setOnCargandoListener(function (cargando, mensaje) {
                    $(jAlert).MostrarDialogoCargando(cargando, true);
                });

                iFrame.setOnMensajeListener(function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                });

                iFrame.setAccesos(valores.Accesos);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Seleccionar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    var accesos = iFrame.getAccesos();
                    if (accesos.length == 0) {
                        valores.CallbackMensajes('Alerta', 'Debe indicar el acceso de algun objeto');
                        return;
                    }

                    valores.Callback(accesos);
                    $(jAlert).CerrarDialogo();
                }
            }
        ]
    })
}

function crearDialogoObjetoKeyValue(valores) {
    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Objeto
    if (!('Objeto' in valores)) {
        console.log('Debe indicar el objeto');
        return false;
    }

    //Claves Valor
    if (!('KeyValues' in valores)) {
        valores.KeyValues = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>KeyValue para un Objeto</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IObjetoKeyValue.aspx'),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {
                iFrame.setOnCargandoListener(function (cargando, mensaje) {
                    $(jAlert).MostrarDialogoCargando(cargando, true);
                });

                iFrame.setOnMensajeListener(function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                });

                iFrame.setObjeto(valores.Objeto);
                iFrame.setKeyValues(valores.KeyValues);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function (jAlert, iFrame) {
                    var clavesValor = iFrame.getKeyValues();
                    valores.Callback(clavesValor);
                    $(jAlert).CerrarDialogo();
                }
            }
        ]
    })
}

function crearDialogoObjetoKeyValueNuevo(valores) {
    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Validar
    if (!('Validar' in valores)) {
        valores.Validar = function () { return true; };
    }

    //Objeto
    if (!('Objeto' in valores)) {
        console.log('Debe indicar el objeto');
        return false;
    }

    var html = '';
    html += '<div class="row margin">';
    html += '   <div class="col s6">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Clave" type="text" maxlength="200" autofocus />';
    html += '           <label for="input_Nombre" class="no-select">Clave</label>';
    html += '       </div>';
    html += '   </div>';
    html += '   <div class="col s6">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Valor" type="text" maxlength="200" />';
    html += '           <label for="input_Valor" class="no-select">Valor</label>';
    html += '       </div>';
    html += '   </div>';
    html += '</div>';

    crearDialogoHTML({
        Content: html,
        OnLoad: function (jAlert) {
            $(jAlert).find('#input_Clave').trigger('focus');

            $(jAlert).find('#input_Clave').enterKey(function () {
                btnAceptar(jAlert);
            });
            $(jAlert).find('#input_Valor').enterKey(function () {
                btnAceptar(jAlert);
            });

        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    btnAceptar(jAlert);
                }
            }
        ]
    });

    function btnAceptar(jAlert) {
        var clave = $(jAlert).find('#input_Clave').val().trim();
        var valor = $(jAlert).find('#input_Valor').val().trim();

        if (clave == "") {
            valores.CallbackMensajes('Alerta', 'La clave es requerida');
            return;
        }

        if (valor == "") {
            valores.CallbackMensajes('Alerta', 'El valor es requerido');
            return;
        }

        var keyValue = {
            Id: new Date().getTime(),
            Objeto: valores.Objeto,
            IdObjeto: valores.Objeto.Id,
            Key: clave,
            Value: valor
        };

        if (!valores.Validar(keyValue)) return;

        valores.Callback(keyValue);
        $(jAlert).CerrarDialogo();
    }
}

function crearDialogoObjetoKeyValueEditar(valores) {
    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Validar
    if (!('Validar' in valores)) {
        valores.Validar = function () { return true; };
    }

    //Objeto
    if (!('ObjetoKeyValue' in valores)) {
        console.log('Debe indicar el ObjetoKeyValue');
        return false;
    }

    var html = '';
    html += '<div class="row margin">';
    html += '   <div class="col s6">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Clave" type="text" maxlength="200" autofocus />';
    html += '           <label for="input_Nombre" class="no-select">Clave</label>';
    html += '       </div>';
    html += '   </div>';
    html += '   <div class="col s6">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Valor" type="text" maxlength="200" />';
    html += '           <label for="input_Valor" class="no-select">Valor</label>';
    html += '       </div>';
    html += '   </div>';
    html += '</div>';

    crearDialogoHTML({
        Content: html,
        OnLoad: function (jAlert) {
            $(jAlert).find('#input_Clave').trigger('focus');
            $(jAlert).find('#input_Clave').val(valores.ObjetoKeyValue.Key);

            $(jAlert).find('#input_Valor').val(valores.ObjetoKeyValue.Value);

            Materialize.updateTextFields();

            $(jAlert).find('#input_Clave').enterKey(function () {
                btnAceptar(jAlert);
            });
            $(jAlert).find('#input_Valor').enterKey(function () {
                btnAceptar(jAlert);
            });



        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    btnAceptar(jAlert);
                }
            }
        ]
    });

    function btnAceptar(jAlert) {
        var clave = $(jAlert).find('#input_Clave').val().trim();
        var valor = $(jAlert).find('#input_Valor').val().trim();

        if (clave == "") {
            valores.CallbackMensajes('Alerta', 'La clave es requerida');
            return;
        }

        if (valor == "") {
            valores.CallbackMensajes('Alerta', 'El valor es requerido');
            return;
        }


        valores.ObjetoKeyValue.Key = clave;
        valores.ObjetoKeyValue.Value = valor;

        if (!valores.Validar(valores.ObjetoKeyValue)) return;

        valores.Callback(valores.ObjetoKeyValue);
        $(jAlert).CerrarDialogo();
    }
}

function crearDialogoKeyValueNuevo(valores) {
    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Validar
    if (!('Validar' in valores)) {
        valores.Validar = function () { return true; };
    }

    var html = '';
    html += '<div class="row margin">';
    html += '   <div class="col s6">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Clave" type="text" maxlength="200" autofocus />';
    html += '           <label for="input_Nombre" class="no-select">Clave</label>';
    html += '       </div>';
    html += '   </div>';
    html += '   <div class="col s6">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Valor" type="text" maxlength="200" />';
    html += '           <label for="input_Valor" class="no-select">Valor</label>';
    html += '       </div>';
    html += '   </div>';
    html += '</div>';

    crearDialogoHTML({
        Content: html,
        OnLoad: function (jAlert) {
            $(jAlert).find('#input_Clave').trigger('focus');

            $(jAlert).find('#input_Clave').enterKey(function () {
                btnAceptar(jAlert);
            });
            $(jAlert).find('#input_Valor').enterKey(function () {
                btnAceptar(jAlert);
            });

        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    btnAceptar(jAlert);
                }
            }
        ]
    });

    function btnAceptar(jAlert) {
        var clave = $(jAlert).find('#input_Clave').val().trim();
        var valor = $(jAlert).find('#input_Valor').val().trim();

        if (clave == "") {
            valores.CallbackMensajes('Alerta', 'La clave es requerida');
            return;
        }

        if (valor == "") {
            valores.CallbackMensajes('Alerta', 'El valor es requerido');
            return;
        }

        var keyValue = {
            Id: -(new Date().getTime()),
            Key: clave,
            Value: valor
        };

        if (!valores.Validar(keyValue)) return;

        valores.Callback(keyValue);
        $(jAlert).CerrarDialogo();
    }
}

function crearDialogoKeyValueEditar(valores) {
    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Validar
    if (!('Validar' in valores)) {
        valores.Validar = function () { return true; };
    }

    //Objeto
    if (!('KeyValue' in valores)) {
        console.log('Debe indicar el KeyValue');
        return false;
    }

    var html = '';
    html += '<div class="row margin">';
    html += '   <div class="col s6">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Clave" type="text" maxlength="200" autofocus />';
    html += '           <label for="input_Nombre" class="no-select">Clave</label>';
    html += '       </div>';
    html += '   </div>';
    html += '   <div class="col s6">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Valor" type="text" maxlength="200" />';
    html += '           <label for="input_Valor" class="no-select">Valor</label>';
    html += '       </div>';
    html += '   </div>';
    html += '</div>';

    crearDialogoHTML({
        Content: html,
        OnLoad: function (jAlert) {
            $(jAlert).find('#input_Clave').trigger('focus');
            $(jAlert).find('#input_Clave').val(valores.KeyValue.Key);

            $(jAlert).find('#input_Valor').val(valores.KeyValue.Value);

            Materialize.updateTextFields();

            $(jAlert).find('#input_Clave').enterKey(function () {
                btnAceptar(jAlert);
            });
            $(jAlert).find('#input_Valor').enterKey(function () {
                btnAceptar(jAlert);
            });



        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    btnAceptar(jAlert);
                }
            }
        ]
    });

    function btnAceptar(jAlert) {
        var clave = $(jAlert).find('#input_Clave').val().trim();
        var valor = $(jAlert).find('#input_Valor').val().trim();

        if (clave == "") {
            valores.CallbackMensajes('Alerta', 'La clave es requerida');
            return;
        }

        if (valor == "") {
            valores.CallbackMensajes('Alerta', 'El valor es requerido');
            return;
        }


        valores.KeyValue.Key = clave;
        valores.KeyValue.Value = valor;

        if (!valores.Validar(valores.KeyValue)) return;

        valores.Callback(valores.KeyValue);
        $(jAlert).CerrarDialogo();
    }
}

function crearDialogoKeyValueQuitarNuevo(valores) {
    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Validar
    if (!('Validar' in valores)) {
        valores.Validar = function () { return true; };
    }

    var html = '';
    html += '<div class="row margin">';
    html += '   <div class="col s12">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Clave" type="text" maxlength="200" autofocus />';
    html += '           <label for="input_Nombre" class="no-select">Clave</label>';
    html += '       </div>';
    html += '   </div>';
    html += '</div>';

    crearDialogoHTML({
        Content: html,
        OnLoad: function (jAlert) {
            $(jAlert).find('#input_Clave').trigger('focus');

            $(jAlert).find('#input_Clave').enterKey(function () {
                btnAceptar(jAlert);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    btnAceptar(jAlert);
                }
            }
        ]
    });

    function btnAceptar(jAlert) {
        var clave = $(jAlert).find('#input_Clave').val().trim();

        if (clave == "") {
            valores.CallbackMensajes('Alerta', 'La clave es requerida');
            return;
        }

        var keyValue = {
            Id: -(new Date().getTime()),
            Key: clave
        };

        if (!valores.Validar(keyValue)) return;

        valores.Callback(keyValue);
        $(jAlert).CerrarDialogo();
    }
}

function crearDialogoKeyValueQuitarEditar(valores) {
    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Validar
    if (!('Validar' in valores)) {
        valores.Validar = function () { return true; };
    }

    //Objeto
    if (!('KeyValue' in valores)) {
        console.log('Debe indicar el KeyValue');
        return false;
    }

    var html = '';
    html += '<div class="row margin">';
    html += '   <div class="col s12">';
    html += '       <div class="input-field">';
    html += '           <input id="input_Clave" type="text" maxlength="200" autofocus />';
    html += '           <label for="input_Nombre" class="no-select">Clave</label>';
    html += '       </div>';
    html += '   </div>';
    html += '</div>';

    crearDialogoHTML({
        Content: html,
        OnLoad: function (jAlert) {
            $(jAlert).find('#input_Clave').trigger('focus');
            $(jAlert).find('#input_Clave').val(valores.KeyValue.Key);
            Materialize.updateTextFields();

            $(jAlert).find('#input_Clave').enterKey(function () {
                btnAceptar(jAlert);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    btnAceptar(jAlert);
                }
            }
        ]
    });

    function btnAceptar(jAlert) {
        var clave = $(jAlert).find('#input_Clave').val().trim();

        if (clave == "") {
            valores.CallbackMensajes('Alerta', 'La clave es requerida');
            return;
        }

        valores.KeyValue.Key = clave;

        if (!valores.Validar(valores.KeyValue)) return;

        valores.Callback(valores.KeyValue);
        $(jAlert).CerrarDialogo();
    }
}

/* Rol */

function crearDialogoRolListado(valores) {
    if (!('IdAplicacion' in valores)) {
        console.log('Debe mandar el id de la app');
        return false;
    }

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Todo
    if (!('Todo' in valores)) {
        valores.Todo = false;
    }

    //Seleccionar
    if (!('Seleccionar' in valores)) {
        valores.Seleccionar = false;
    }

    if (!('Ids' in valores)) {
        valores.Ids = [];
    }

    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Busqueda
    if (!('Busqueda' in valores)) {
        valores.Busqueda = false;
    }

    //Armo la url
    var url = '~/Paginas/IFrames/IRolListado.aspx';
    var params = [];
    params.push('IdAplicacion=' + valores.IdAplicacion);
    if (valores.Todo) {
        params.push('Todo=true');
    }
    if (valores.Seleccionar) {
        params.push('Seleccionar=true');
    }
    if (params.length != 0) {
        url += '?' + params.join('&');
    }

    crearDialogoIFrame({
        Titulo: '<label>Roles</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {
                //Cargar solo algunos ids
                if (!valores.Todo) {
                    iFrame.setRoles(valores.Ids);
                }
            });

            //Busqueda
            iFrame.setBusquedaVisible(valores.Busqueda);

            //mensaje
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Seleccionar
            if (valores.Seleccionar) {
                iFrame.setOnSeleccionarListener(function (data) {
                    valores.CallbackSeleccionar(data);
                    $(jAlert).CerrarDialogo();
                });
            }
        },
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    })
}

function crearDialogoRolDetalle(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de Rol</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IRolDetalle.aspx?Id=' + id),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).CerrarDialogo();
                }
            }
        ]
    });
}

function crearDialogoRolEditar(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var valoresIFrame = {
        Titulo: '<label>Editar Rol</label>',
        Url: ResolveUrl('~/Paginas/IFrames/IRolNuevo.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {

            });

            //Registrar
            iFrame.setOnEditarListener(function (data) {
                valores.CallbackMensajes('Exito', 'Rol editado correctamente');
                valores.Callback(data);
                $(jAlert).CerrarDialogo();
            });

            //Mensajes 
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame) {
                        iFrame.editar();
                    }
                }
            ]
    };

    if ('Alto' in valores) {
        valoresIFrame.Alto = valores.Alto;
    }

    if ('Ancho' in valores) {
        valoresIFrame.Ancho = valores.Ancho;
    }

    crearDialogoIFrame(valoresIFrame);
}

function crearDialogoRolDarDeBaja(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de Baja Rol</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer dar de baja el rol?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorError',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioRol.asmx/DarDebaja'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error dando de baja el rol');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Rol dado de baja correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error dando de baja el rol');
                                console.log(result);
                            }
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoRolActivar(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Activar Rol</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer activar el rol?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioRol.asmx/Activar'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error activando el rol');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Rol activado correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error activando la app');
                                console.log(result);
                            }
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    })
}

/* Usuario Aplicacion */

function crearDialogoUsuarioAplicacionEditar(valores) {

    if (!('Id' in valores)) {
        console.log('Debe mandar el id a editar');
        return false;
    }

    //if (!('IdAplicacion' in valores)) {
    //    console.log('debe mandar el id aplicacion');
    //    return false;
    //}

    //Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/Paginas/IFrames/IUsuarioAplicacionNuevo.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnReadyListener(function () {

            });

            //mensaje
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Cargando
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrame.setOnEditarListener(function (entity) {
                valores.Callback(entity);

                valores.CallbackMensajes('Exito', 'Usuario de la Aplicacion editado correctamente');
                $(jAlert).CerrarDialogo();
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Editar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    iFrame.editar();
                }
            }
        ]

    });
}

function crearDialogoUsuarioAplicacionDarDeBaja(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de Baja Usuario</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer dar de baja el usuario?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorError',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioRolPorAreaPorUsuarioPorAplicacion.asmx/DarDebaja'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error dando de baja el usuario');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'usuario dado de baja correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error dando de baja el usuario');
                                console.log(result);
                            }

                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

function crearDialogoUsuarioAplicacionActivar(valores) {
    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback 
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Activar Usuario</label>',
        Content: '<div class="margin"><label class="texto">¿Esta seguro de querer activar el usuario?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Si',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true, true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioRolPorAreaPorUsuarioPorAplicacion.asmx/Activar'),
                        Data: { id: id },
                        OnSuccess: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);

                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                console.log('Error activando el usuario');
                                console.log(result);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Usuario activado correctamente');
                            $(jAlert).CerrarDialogo();

                            valores.Callback(result.Return);
                        },
                        OnError: function (result) {
                            $(jAlert).MostrarDialogoCargando(false, true);
                            if (DEBUG) {
                                console.log('Error activando el usuario');
                                console.log(result);
                            }
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');

                        }
                    });
                }
            }
        ]
    })
}

/* Password */

function crearDialogoCambiarPassword(valores) {

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }
    crearDialogoIFrame({
        Titulo: '<label>Cambiar Contraseña</label>',
        Url: ResolveUrl('~/Paginas/IFrames/ICambiarClave.aspx'),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnClaveCambiadaListener(function () {
                $(jAlert).CerrarDialogo();
                valores.CallbackMensajes('Exito', 'Contraseña cambiada correctamente');
                valores.Callback();
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Cambiar Contraseña',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame) {
                    iFrame.cambiarClave();
                }
            }
        ]
    });
}

/* Log acceso */

function crearDialogoLogAccesoDetalle(valores) {

    if (!('Id' in valores)) {
        return false;
    }

    var id = valores.Id;

    //Callback Mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de Log de Acceso</label>',
        Url: ResolveUrl('~/Paginas/IFrames/ILogAccesoDetalle.aspx?id=' + id),
        OnLoad: function (jAlert, iFrame) {
            iFrame.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrame.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function (jAlert) {
                    $(jAlert).CerrarDialogo();
                }
            }
        ]
    });
}

function crearDialogoContacto() {
    crearDialogoHTML({
        Titulo: '<label>Contacto</label>',
        Content: getContent(),

        OnLoad: function (jAlert) {
            $(jAlert).find('#input_MailContacto').val(usuario.Email);
            $(jAlert).find('#input_TelefonoContacto').val(usuario.Telefono);

            Materialize.updateTextFields();
        },

        Botones: [
            {
                Texto: 'Cancelar',
            },
            {
                Texto: 'Enviar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    if (!isValidDescripcion()) {
                        mostrarMensaje('Error', "Ingrese una descripción más larga");
                    }

                    if (!$('#formContacto').valid()) return;

                    mostrarCargando();

                    var url = ResolveUrl('~/Servicios/ServicioContacto.asmx/EnviarMailContacto');

                    var data = {
                        mail: $('#input_MailContacto').val(),
                        telefono: $('#input_TelefonoContacto').val(),
                        mensaje: $('#input_DescripcionContacto').val()
                    }

                    crearAjax({
                        Url: url,
                        Data: data,

                        OnSuccess: function (result) {
                            ocultarCargando();

                            if (!result.Ok) {
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            mostrarMensaje('Exito', "Email enviado correctamente");
                            $(jAlert).CerrarDialogo();
                        },

                        OnError: function (result) {
                            ocultarCargando();
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    });

    function getContent() {
        var html = '';

        html += '<form id="formContacto" class="padding">';

        html += '    <div class="margin-bottom">';
        html += '        <label style="font-weight: 300;">Desde esta pantalla usted puede contactarse con el soporte de #CBA147 para informar un problema o realizar una solicitud. <strong>Por favor sea lo más descriptivo posible</strong></label>';
        html += '    </div>';

        html += '    <div class="row">';
        html += '        <div class="col s12">';
        html += '           <div class="input-field">';
        html += '                <i class="material-icons prefix">mail</i>';
        html += '                <input id="input_MailContacto" type="email" name="mail" required="true" aria-required="true" />';
        html += '                <label for="input_MailContacto" class="no-select">Mail de contacto</label>';
        html += '           </div>';
        html += '       </div>';
        html += '   </div>';

        html += '   <div class="row">';
        html += '       <div class="col s12">';
        html += '           <div class="input-field">';
        html += '               <i class="material-icons prefix">phone</i>';
        html += '               <input id="input_TelefonoContacto" type="number" name="telefono" />';
        html += '               <label for="input_TelefonoContacto" class="no-select">Teléfono de contacto</label>';
        html += '           </div>';
        html += '       </div>';
        html += '   </div>';

        html += '   <div class="row margin-top">';
        html += '       <div class="col s12">';
        html += '           <div class="input-field">';
        html += '               <textarea id="input_DescripcionContacto" name="mensaje" class="materialize-textarea" required="true" aria-required="true" lines="3" maxlength="2000" minlength="50"></textarea>';
        html += '               <label for="input_DescripcionContacto" class="no-select">Descripcion del problema, solicitud o requerimiento...</label>';
        html += '           </div>';
        html += '        </div>';
        html += '   </div>';

        html += '</form>';

        return html;
    }

    function isValidDescripcion() {
        var length = $('#input_DescripcionContacto').val().length;
        var minlength = parseInt($('#input_DescripcionContacto').attr('minlength'));

        return length > minlength;
    }

}
