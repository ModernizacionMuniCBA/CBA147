$.fn.jAlert.defaults.size = 'lg';
$.fn.jAlert.defaults.class = 'no-padding';
$.fn.jAlert.defaults.closeBtn = 'false';
$.fn.jAlert.defaults.closeOnEsc = 'false';

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

jQuery.fn.HabilitarBotonesFooter = function (valores) {
    $(this).find('.ja_body > div.ja_btn_wrap.optBack').removeClass('deshabilitado');
}

jQuery.fn.DeshabilitarBotonesFooter = function (valores) {
    $(this).find('.ja_body > div.ja_btn_wrap.optBack').addClass('deshabilitado');
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
    var titulo;
    if ('Titulo' in valores) {
        titulo = valores.Titulo;
    }

    if (titulo != undefined && titulo.indexOf('<label>') == -1) {
        titulo = '<label>' + titulo + '</label>';
    }

    //Height
    var dif_height = titulo == undefined ? 60 : 94;
    var porcentaje = 0.8;
    if ('Alto' in valores) {
        porcentaje = valores.Alto;
    }
    var h = ((top.$('body').height() * porcentaje) - dif_height) + 'px';
    if ('Height' in valores) {
        h = valores.Height;
    }

    //Si el alto es mayor al maximo de la pantalla... lo limito
    if (h > ((top.$('body').height() * 0.95) - dif_height)) {
        h = ((top.$('body').height() * 0.95) - dif_height)
    }


    var botones = [];
    $.each(valores.Botones, function (index, btn) {
        if (!('CerrarDialogo' in btn)) {
            btn.CerrarDialogo = true;
        }

        var btnClass = 'btn-flat waves-effect';
        if ('Class' in btn) {
            btnClass = btnClass + ' ' + btn.Class;
        }

        var idBoton = btn.Id || btn.id;

        var textoBoton;
        if ('Icono' in btn) {
            textoBoton = '<i class="btn-icono material-icons">' + btn.Icono + '</i>' + btn.Texto;
        } else {
            textoBoton = btn.Texto;
        }
        botones.push({
            id: idBoton,
            text: textoBoton,
            closeAlert: false,
            class: btnClass,
            onClick: function () {
                if ('OnClick' in btn) {
                    var iFrame = $(jAlert).find('iframe')[0].contentWindow;

                    btn.OnClick(jAlert, iFrame, iFrame);
                }

                if (btn.CerrarDialogo) {
                    $(jAlert).CerrarDialogo();
                }

            }
        })
    });

    var btnCerrarVisible = false;
    if ('BotonCerrarVisible' in valores) {
        btnCerrarVisible = valores.BotonCerrarVisible;
    }

    top.$.jAlert({
        id: id,
        title: titulo,
        iframe: valores.Url,
        iframeheight: h,
        closeBtn: btnCerrarVisible,
        closeOnEsc: false,
        onOpen: function (jAlert) {
            onDialogOpen(jAlert);

            var ifr = $($(jAlert).find('iframe'));
            if (ifr == undefined) return;

            $(jAlert).find('iframe').off('load');
            $(jAlert).find('iframe').on('load', function () {
                console.log('load');

                var iFrame = $(jAlert).find('iframe')[0].contentWindow;

                $(overlay).fadeOut(300, function () {
                    $(overlay).remove();
                });


                var ocultar = true;
                if ('OnLoad' in valores) {
                    var result = valores.OnLoad(jAlert, iFrame, iFrame);
                    if (result != undefined) {
                        ocultar = result;
                    }
                }

                if (ocultar) {
                    $(jAlert).MostrarDialogoCargando(false, true);
                }

                if (!('Botones' in valores) || valores.Botones == undefined || valores.Botones.length == 0) {
                    $(jAlert).addClass('sinBotones');
                }

                if ('ScrollY' in valores && valores.ScrollY == true) {

                    //$(iFrame).find('#contenedor-principal').css('overflow-y', 'auto !important');
                    //$($(iFrame).find('#contenedor-principal')).css('background-color', 'red !important');
                    $($(iFrame)[0].document).find('#contenedor-principal').css('overflow', 'auto !important');

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
    if ('Width' in valores) {
        $(jAlert).css('width', valores.Width + 'px');
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

        var btnClass = 'btn-flat waves-effect';
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

    top.$('#' + id).parents('.ja_wrap').addClass('open');

    if ('Width' in valores) {
        top.$('#' + id).css('width', valores.Width + 'px');
    }

    if ('Height' in valores) {
        top.$('#' + id).css('height', valores.Height + '%');
    }
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

    if (!('ClassBotonAceptar' in valores)) {
        valores.ClassBotonAceptar = 'colorExito';
    }

    if (!('CerrarDialogoBotonCancelar' in valores)) {
        valores.CerrarDialogoBotonCancelar = true;
    }

    if (!('CerrarDialogoBotonAceptar' in valores)) {
        valores.CerrarDialogoBotonAceptar = true;
    }

    crearDialogoHTML({
        Content: '<div class="margin"><label>' + valores.Texto + '</label></div>',
        Botones: [
            {
                Texto: valores.TextoBotonCancelar,
                CerrarDialogo: valores.CerrarDialogoBotonCancelar,
                OnClick: function (jAlert) {
                    valores.CallbackNegativo(jAlert);
                }
            },
            {
                Texto: valores.TextoBotonAceptar,
                Class: valores.ClassBotonAceptar,
                CerrarDialogo: valores.CerrarDialogoBotonAceptar,
                OnClick: function (jAlert) {
                    valores.CallbackPositivo(jAlert);
                }
            }
        ]
    })
}

function crearDialogoInput(valores) {
    if (valores == undefined) {
        valores = {};
    }

    if (!('Valor' in valores)) {
        valores.Valor = '';
    }

    if (!('Botones' in valores)) {
        valores.Botones = []
    }

    if (!('Placeholder' in valores)) {
        valores.Placeholder = "Ingrese texto...";
    }

    if (!('Titulo' in valores)) {
        valores.Titulo = '';
    }

    if (!('Width' in valores)) {
        valores.Width = 600;
    }


    if (!('OnLoad' in valores)) {
        valores.OnLoad = function () { };
    }

    if (!('OnClose' in valores)) {
        valores.OnClose = function () { };
    }


    crearDialogoHTML({
        Titulo: '<label>' + valores.Titulo + '</label>',
        Width: valores.Width,
        Content: '<div class="margin"><input type="text" placeholder="' + valores.Placeholder + '" value="' + valores.Valor + '"/></div>',
        OnLoad: valores.OnLoad,
        OnClose: valores.OnClose,
        Botones: valores.Botones
    })
}

jQuery.fn.MenuFlotante = function (valores) {


    if (valores == undefined) valores = {};
    if (!('e' in valores)) valores.e = undefined;
    if (!('TodoElAncho') in valores) valores.TodoElAncho = false;
    if (!('TodoElAlto') in valores) valores.TodoElAlto = false;


    if (!('Menu' in valores) || valores.Menu == undefined) return;
    if (valores.Menu.length == 0) return;

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
    var hMax = 500;
    var w = 200;
    if (valores.TodoElAlto) {
        hMax = $(this).height();
    }
    if (valores.TodoElAncho) {
        w = $(this).width();
    } else {
        if ('Width' in valores && valores.Width != undefined) {
            w = valores.Width;
        }
    }


    //Creo el menu
    var menu = $('<div>');
    var id = new Date().getTime();
    $(menu).prop('id', id);
    $(menu).addClass('menu-flotante');

    $(menu).css('width', w + 'px');
    $(menu).css('max-height', hMax + 'px');

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

        var hMenuItem = hItem;
        var separador = false;
        if ('Separador' in btn && btn.Separador == true) {
            hMenuItem = 32;
            separador = true;
        }

        hCalculado += hMenuItem;

        if (!('Id' in btn)) {
            btn.Id = new Date().getTime();
        }

        var li;
        if ('Class' in btn) {
            li = $('<li class="' + btn.Class + '">');
        } else {
            li = $('<li>');
        }


        $(li).appendTo(ul);
        $(li).addClass('menu-item waves-effect');
        $(li).attr('id', btn.Id);
        if (separador) {
            $(li).addClass('separador');
        }
        $(li).attr('index', index);

        if (separador) {
            //Texto
            var titulo = btn.Texto;
            if (typeof titulo != 'string') {
                titulo = btn.Texto(data);
            }
            var texto = $('<label>');
            $(texto).addClass('texto');
            $(texto).text(titulo);
            $(texto).appendTo(li);
            return true;
        }

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

        if ('CustomView' in btn) {
            hCalculado -= hItem;
            hCalculado += btn.Height;

            $(li).addClass('custom-view');
            let div = $(btn.CustomView);
            $(div).css('min-height', btn.Height + 'px');
            $(div).css('max-height', btn.Height + 'px');
            $(div).appendTo(li);
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
    });

    console.log(hCalculado);
    console.log(hMax);

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


/* Dialogos de persona fisica */
function crearDialogoPersonaFisicaDetalle(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de persona física</label>',
        Url: ResolveUrl('~/IFrame/IPersonaFisicaDetalle.aspx?Id=' + id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                 {
                     Texto: 'Aceptar',
                     Class: 'colorExito'
                 }
            ]
    });
}

function crearDialogoPersonaFisicaEditar(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback editado
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }


    crearDialogoIFrame({
        Titulo: '<label>Editar Persona Fisica</label>',
        Url: ResolveUrl('~/IFrame/IPersonaFisicaNuevo.aspx?Id=' + id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback editar
            iFrameContent.setOnEditarCompletoListener(function (persona) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Persona física editada correctamente');

                //Informo que edite
                valores.CallbackEditar(persona);
            });
        },
        Ancho: 0.95,
        Alto: 0.95,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}


/* Dialogos de Requerimiento */

function crearDialogoRequerimientoDetalle(valores) {
    return crearDialogoDetalleRequerimiento(valores);
}

function crearDialogoDetalleRequerimiento(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    var botones = [];

    botones.push({
        Id: 'btnImprimirRequerimiento',
        Texto: 'Imprimir',
        CerrarDialogo: false,
        OnClick: function (jAlert, iFrame, iFrameContent) {
            $(jAlert).find('#btnImprimirRequerimiento').MenuFlotante({
                PosicionX: 'izquierda',
                PosicionY: 'abajo',
                Menu: [
                    {
                        Texto: 'Imprimir con mapa',
                        Icono: 'print',
                        OnClick: function () {
                            iFrameContent.imprimirConMapa();
                        }
                    },
                    {
                        Texto: 'Imprimir sin mapa',
                        Icono: 'print',
                        OnClick: function () {
                            iFrameContent.imprimirSinMapa();
                        }
                    }
                ]
            })
        }
    });


    botones.push({
        Texto: 'Aceptar',
        Class: 'colorExito',
        OnClick: function () {
            valores.Callback();
        }
    });


    var url = ResolveUrl('~/IFrame/IRequerimientoDetalle.aspx?Id=' + id);
    url = ResolveUrl('~/IFrame/IRequerimientoDetalle2.aspx?Id=' + id);

    crearDialogoIFrame({
        Url: url,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setOnPanelAbiertoListener(function (abierto) {
                if (abierto) {
                    $(jAlert).DeshabilitarBotonesFooter();
                    return;
                }

                $(jAlert).HabilitarBotonesFooter();
            });
        },
        Ancho: 0.95,
        Alto: 0.95,
        Botones: botones
    });
}

function crearDialogoEditarRequerimiento(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback editado
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar Requerimiento</label>',
        Url: ResolveUrl('~/IFrame/IRequerimientoNuevo.aspx?Id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            $(jAlert).find('#btnAgregarNota').hide();

            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Editar 
            iFrameContent.setOnEditarCompletoListener(function (rq) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Requerimiento editado correctamente');

                //Informo que edite
                valores.CallbackEditar(rq);
            });

            //Callback Tab
            iFrameContent.setOnTabChangeListener(function (tab) {
                switch (tab) {
                    case '#tabDetalle':
                        $(jAlert).find('#btnAgregarNota').hide();
                        break;

                    case '#tabNotas':
                        $(jAlert).find('#btnAgregarNota').show();
                        break;

                    case '#tabDocumentos':
                        $(jAlert).find('#btnAgregarNota').hide();
                        break;

                    case '#tabImagenes':
                        $(jAlert).find('#btnAgregarNota').hide();
                        break;
                }
            });
            return false;
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Id: 'btnAgregarNota',
                     Texto: 'Agregar nota',
                     CerrarDialogo: false,
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.agregarNota();
                     }
                 },
                 {
                     Texto: 'Guardar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.editar();
                     }
                 }
            ]
    });
}

function crearDialogoRequerimientoCambiarEstado(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    crearDialogoIFrame({
        Titulo: 'Cambiar estado de Requerimiento',
        Url: ResolveUrl('~/IFrame/IRequerimientoCambiarEstado.aspx?Id=' + id),
        Height: 240,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Cambair Estado 
            iFrameContent.setOnEstadoCambiadoListener(function (requerimiento, estado) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Estado cambiado correctamente');

                //Informo que cambie
                valores.Callback(requerimiento, estado);
            });
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Guardar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.cambiarEstado();
                     }
                 }
            ]
    });
}

function crearDialogoRequerimientoEditarReferenteProvisorio(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    crearDialogoIFrame({
        Titulo: 'Editar Referente Provisorio',
        Url: ResolveUrl('~/IFrame/IRequerimientoEditarReferenteProvisorio.aspx?Id=' + id),
        Height: 240,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Cambair Estado 
            iFrameContent.setOnListener(function () {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                    //Informo el mensaje
                    valores.CallbackMensajes('Exito', 'Referente editado correctamente');

                //Informo que cambie
                valores.Callback();
            });
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Guardar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.editarReferente();
                     }
                 }
            ]
    });
}

function crearDialogoRequerimientoCancelar(valores) {
    if (valores == undefined) {
        return false;
    }

    //Id
    var id;
    if (!('Id' in valores)) {
        return false;
    }
    id = valores.Id;
    if (id == undefined || id <= 0) {
        return false;
    }

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) {
        callbackMensajes = valores.CallbackMensajes;
    }

    //Callback Cargando
    var callbackCargando = function () { };
    if ('CallbackCargando' in valores) {
        callbackCargando = valores.CallbackCargando;
    }

    //Callback
    var callback = function () { };
    if ('Callback' in valores) {
        callback = valores.Callback;
    }

    var autoCerrar = true;
    if ('AutoCerrar' in valores) {
        autoCerrar = valores.AutoCerrar;
    }

    crearDialogoHTML({
        Titulo: '<label>Cancelar Requerimiento</label>',
        Content: '<div class="row margin-top" >' +
                            '<div class="col s12">' +
                                '<div class="input-field">' +
                                    '<textarea id="inputFormulario_Observaciones" class="materialize-textarea"></textarea>' +
                                    '<label for="inputFormulario_Observaciones" class=" no-select textarea">Motivo de cancelación</label>' +
                                '</div>' +
                            '</div>' +
                        '</div>',
        OnLoad: function (jAlert) {
            $(jAlert).find('#inputFormulario_Observaciones').trigger('focus');
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert) {

                        var obs = $(jAlert).find('#inputFormulario_Observaciones').val();
                        if (obs == "" || obs == undefined) {
                            callbackMensajes('Error', 'Debe ingresar el motivo de cancelación');
                            return;
                        }

                        //Muestro el cargando
                        $(jAlert).MostrarDialogoCargando(true, true);

                        var dataAjax = {
                            id: id, observaciones: obs
                        };
                        dataAjax = JSON.stringify(dataAjax);

                        crearAjax({
                            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/Cancelar'),
                            Data: { id: id, observaciones: obs },
                            OnSuccess: function (result) {
                                $(jAlert).MostrarDialogoCargando(false, true);

                                if (!result.Ok) {
                                    callbackMensajes('Error', result.Error);
                                    return;
                                }

                                //Muestro el mensaje de OK
                                callbackMensajes('Exito', 'Requerimiento cancelado correctamente.');

                                //Cierro el dialogo
                                $(jAlert).CerrarDialogo();

                                //Cierro el dialogo
                                callback(result.Requerimiento);
                            },
                            OnError: function (result) {
                                $(jAlert).MostrarDialogoCargando(false, true);
                                callbackMensajes('Error', 'Error cancelando el requerimiento');
                            }
                        })
                    }
                }]
    });
}

function crearDialogoRequerimientoAgregarReferente(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    crearDialogoIFrame({
        Titulo: 'Agregar referente de Requerimiento',
        Url: ResolveUrl('~/IFrame/IRequerimientoCambiarReferente.aspx?Id=' + id),
        Height: 200,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Cambair Estado 
            iFrameContent.setListener(function (usuario) {
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Referente agregado correctamente');

                //Informo que cambie
                valores.Callback(usuario);
            });
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Guardar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.cambiarReferente();
                     }
                 }
            ]
    });
}

function crearDialogoRequerimientoCambiarMotivo(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //Desde OT
    if (!('DesdeOT' in valores)) {
        valores.DesdeOT = false;
    }

    //Tipo Motivo
    if (!('DesdeOT' in valores)) {
        valores.TipoMotivo = 1;
    }


    crearDialogoIFrame({
        Titulo: 'Cambiar motivo de Requerimiento',
        Height: 400,
        Url: ResolveUrl('~/IFrame/IRequerimientoCambiarMotivo.aspx?Id=' + id + '&DesdeOT=' + valores.DesdeOT + '&TipoMotivo' + valores.TipoMotivo),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Cambair Estado 
            iFrameContent.setListener(function (result) {
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Motivo modificado correctamente');

                //Informo que cambie
                valores.Callback(result);
            });
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Guardar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.guardar();
                     }
                 }
            ]
    });
}

function crearDialogoEnviarMailRequerimiento(valores) {
    //Id
    var id;
    if (!('Id' in valores)) { return false; }
    id = valores.Id;
    if (id == undefined || id <= 0) { return false; }

    //Numero
    if (!('Numero') in valores) return false;
    if (!('Anio') in valores) return false;
    var numero = valores.Numero;
    var anio = valores.Anio;

    //Mail
    var mail = "";
    if ('Mail' in valores) { mail = valores.Mail };

    //Anio
    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) { callbackMensajes = valores.CallbackMensajes; }

    //Callback Cargando
    var callbackCargando = function () { };
    if ('CallbackCargando' in valores) { callbackCargando = valores.CallbackCargando; }

    //Callback
    var callbackEnviarMail = function () { };
    if ('CallbackEnviarMail' in valores) { callbackEnviarMail = valores.CallbackEnviarMail; }

    crearDialogoHTML({
        Titulo: '<label>Enviar Mail</label>',
        Content: '<div class="row margin-top" >' +
                            '<div class="col s12">' +
                                '<div class="input-field">' +
                                    '<textarea id="inputFormulario_Mail" class="materialize-textarea"></textarea>' +
                                    '<label for="inputFormulario_Mail" class=" no-select textarea">Mail</label>' +
                                '</div>' +
                            '</div>' +
                        '</div>',
        OnLoad: function (jAlert) {
            $(jAlert).find('#inputFormulario_Mail').trigger('focus');
            $(jAlert).find('#inputFormulario_Mail').val(mail);
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Enviar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert) {

                        var mail = $(jAlert).find('#inputFormulario_Mail').val();
                        if (mail == undefined || mail == "") {
                            callbackMensajes('Alerta', 'Debe ingresar el e-mail');
                            $(jAlert).find('#inputFormulario_Mail').trigger('focus');
                            return;
                        }

                        if (!validarEmail(mail)) {
                            callbackMensajes('Alerta', 'El e-mail no es válido');
                            $(jAlert).find('#inputFormulario_Mail').trigger('focus');
                            return;
                        }

                        //Muestro el cargando
                        $(jAlert).MostrarDialogoCargando(true, true);


                    }
                }]
    });
}

function crearDialogoEnviarMailOrdenTrabajo(valores) {
    //Id
    var id;
    if (!('Id' in valores)) { return false; }
    id = valores.Id;
    if (id == undefined || id <= 0) { return false; }

    //Numero
    if (!('Numero') in valores) return false;
    if (!('Anio') in valores) return false;
    var numero = valores.Numero;
    var anio = valores.Anio;

    //Mail
    var mail = "";
    if ('Mail' in valores) { mail = valores.Mail };

    //Anio
    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) { callbackMensajes = valores.CallbackMensajes; }

    //Callback Cargando
    var callbackCargando = function () { };
    if ('CallbackCargando' in valores) { callbackCargando = valores.CallbackCargando; }

    //Callback
    var callbackEnviarMail = function () { };
    if ('CallbackEnviarMail' in valores) { callbackEnviarMail = valores.CallbackEnviarMail; }

    crearDialogoHTML({
        Titulo: '<label>Enviar Mail</label>',
        Content: '<div class="row margin-top" >' +
                            '<div class="col s12">' +
                                '<div class="input-field">' +
                                    '<textarea id="inputFormulario_Mail" class="materialize-textarea"></textarea>' +
                                    '<label for="inputFormulario_Mail" class=" no-select textarea">Mail</label>' +
                                '</div>' +
                            '</div>' +
                        '</div>',
        OnLoad: function (jAlert) {
            $(jAlert).find('#inputFormulario_Mail').trigger('focus');
            $(jAlert).find('#inputFormulario_Mail').val(mail);
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Enviar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert) {

                        var mail = $(jAlert).find('#inputFormulario_Mail').val();
                        if (mail == undefined || mail == "") {
                            callbackMensajes('Alerta', 'Debe ingresar el e-mail');
                            $(jAlert).find('#inputFormulario_Mail').trigger('focus');
                            return;
                        }

                        if (!validarEmail(mail)) {
                            callbackMensajes('Alerta', 'El e-mail no es válido');
                            $(jAlert).find('#inputFormulario_Mail').trigger('focus');
                            return;
                        }

                        //Muestro el cargando
                        $(jAlert).MostrarDialogoCargando(true, true);

                        //Enviar mail
                        var dataAjax = { Numero: numero, Anio: anio, Email: mail };

                        crearAjax({
                            Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/EnviarMail'),
                            Data: dataAjax,
                            OnSuccess: function (result) {

                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false, true);

                                if (!result.Ok) {
                                    callbackMensajes('Error', result.Error);
                                    console.log("Error mandando mail");
                                    console.log(dataAjax);
                                    console.log(result);
                                    return;
                                }

                                callbackMensajes('Exito', 'E-mail enviado correctamente');
                                $(jAlert).CerrarDialogo();

                                //Informo el exito
                                callbackEnviarMail(result.Return);
                            },
                            error: function (result) {
                                $(jAlert).MostrarDialogoCargando(false, true);

                                //Error
                                Materialize.toast("No se ha podido enviar el e-mail", 5000, 'colorError');
                                console.log("Error mandando mail");
                                console.log(dataAjax);
                                console.log(result);
                            }
                        });
                    }
                }]
    });
}

function crearDialogoMapaRequerimiento(valores) {

    if (valores == undefined) valores = {};

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) { callbackMensajes = valores.CallbackMensajes; }

    //Callback Cargando
    var callbackCargando = function () { };
    if ('CallbackCargando' in valores) { callbackCargando = valores.CallbackCargando; }

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GenerarMapaPorId'),
                Data: { id: id },
                OnSuccess: function (result) {
                    $(jAlert).CerrarDialogo();

                    if (!result.Ok) {
                        valores.CallbackMensajes('Error', result.Error);
                        return;
                    }

                    crearDialogoIFrame({
                        Url: result.Return,
                        Ancho: 0.95,
                        Alto: 0.95,
                        Botones:
                            [
                                {
                                    Texto: 'Aceptar',
                                    Class: 'colorExito'
                                }
                            ]
                    });
                },
                OnError: function (result) {
                    $(jAlert).CerrarDialogo();

                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}

function crearDialogoMapaRequerimientos(valores) {

    if (valores == undefined) valores = {};

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) { callbackMensajes = valores.CallbackMensajes; }

    //Callback Cargando
    var callbackCargando = function () { };
    if ('CallbackCargando' in valores) { callbackCargando = valores.CallbackCargando; }

    if (!('Ids' in valores)) return false;
    var ids = valores.Ids;

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GenerarMapaPorIds'),
                Data: { ids: ids },
                OnSuccess: function (result) {
                    $(jAlert).CerrarDialogo();

                    if (!result.Ok) {
                        valores.CallbackMensajes('Error', result.Error);
                        return;
                    }

                    crearDialogoIFrame({
                        Url: result.Return,
                        Ancho: 0.95,
                        Alto: 0.95,
                        Botones:
                            [
                                {
                                    Texto: 'Aceptar',
                                    Class: 'colorExito'
                                }
                            ]
                    });

                },
                OnError: function (result) {
                    $(jAlert).CerrarDialogo();
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                }
            })
        }
    });
}

function crearDialogoRequerimientoListado(valores) {
    if (valores == undefined) return false;

    if (!('Ids' in valores)) return false;
    var ids = valores.Ids;

    var callbackMensaje = function () { };
    if ('CallbackMensajes' in valores) {
        callbackMensaje = valores.CallbackMensajes;
    }

    var titulo = null;
    if ('Titulo' in valores) {
        titulo = valores.Titulo;
    }

    crearDialogoIFrame({
        Titulo: titulo,
        Url: ResolveUrl('~/IFrame/IRequerimientoListado.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setIds(ids);

            //Mensjaes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            return false;
        },
        Botones:
       [
           {
               id: 'btnGenerarMapa',
               Texto: 'Mapa',
               CerrarDialogo: false,
               OnClick: function (jAlert, iFrame, iFrameContent) {
                   iFrameContent.generarMapa();
               }
           },
           {
               Texto: 'Imprimir',
               CerrarDialogo: false,
               OnClick: function (jAlert, iFrame, iFrameContent) {
                   iFrameContent.imprimir();
               }
           },
           {
               Texto: 'Aceptar',
               Class: 'colorExito'
           }
       ]
    });
}

function crearDialogoRequerimientoNotaNuevo(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) {
        console.log('Debe indicar el id del requerimiento al cual le quiere agregar una nota');
        return false;
    }

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback agregada
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Agregar Nota</label>',
        Content: '<div class="row margin-top" >' +
                            '<div class="col s12">' +
                                '<div class="input-field">' +
                                    '<textarea id="inputFormulario_Observaciones" class="materialize-textarea"></textarea>' +
                                    '<label for="inputFormulario_Observaciones" class=" no-select textarea">Observaciones</label>' +
                                '</div>' +
                            '</div>' +
                        '</div>',
        OnLoad: function (jAlert) {
            $(jAlert).find('#inputFormulario_Observaciones').trigger('focus');
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert) {

                        var obs = $(jAlert).find('#inputFormulario_Observaciones').val();
                        if (obs == "" || obs == undefined) {
                            callbackMensajes('Error', 'Debe ingresar el contenido de la nota');
                            return;
                        }

                        //Muestro el cargando
                        $(jAlert).MostrarDialogoCargando(true, true);

                        var dataAjax = {
                            id: valores.Id,
                            idOrden: valores.IdOrden,
                            observaciones: obs
                        };
                        dataAjax = JSON.stringify(dataAjax);

                        $.ajax({
                            type: "POST",
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            data: dataAjax,
                            url: ResolveUrl('~/Servicios/RequerimientoService.asmx/AgregarNota'),
                            success: function (result) {
                                result = parse(result.d);

                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false, true);

                                //Error
                                if ('Error' in result) {
                                    valores.CallbackMensajes('Error', result.Error.Publico);

                                    console.log('Error agregando nota');
                                    console.log(dataAjax);
                                    console.log(result);
                                    return;
                                }

                                //Muestro el mensaje de OK
                                valores.CallbackMensajes('Exito', 'Nota agregada correctamente');

                                //Cierro el dialogo
                                $(jAlert).CerrarDialogo();

                                valores.Callback();
                            },
                            error: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false, true);

                                //Muestro el Error
                                valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                                console.log('Error agregando nota');
                                console.log(dataAjax);
                                console.log(result);
                            }
                        });
                    }
                }]
    });
}

function crearDialogoRequerimientoReenviarComprobante(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) {
        console.log('Debe indicar el id del requerimiento al cual le quiere mandar un email.');
        return false;
    }

    //Callback agregada
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Reenviar comprobante de requerimiento',
        Url: ResolveUrl('~/IFrame/IRequerimientoReenviarComprobante2.aspx?id=' + valores.Id),
        Height: 400,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnCargandoListener(function (cargando) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setOnComprobanteReenviadoListener(function () {
                $(jAlert).CerrarDialogo();

                top.mostrarMensaje('Exito', 'Comprobante de atencion reenviado correctamente');
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Reenviar',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.reenviarComprobante();
                }
            }
        ]
    });
}

function crearDialogoRequerimientoMailContacto(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) {
        console.log('Debe indicar el id del requerimiento al cual le quiere mandar un email.');
        return false;
    }

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback agregada
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Enviar mensaje al vecino',
        Url: ResolveUrl('~/IFrame/IRequerimientoMailContacto.aspx?id=' + valores.Id),
        Height: 300,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnCargandoListener(function (cargando) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrameContent.setOnEnviadoListener(function () {
                $(jAlert).CerrarDialogo();
                valores.CallbackMensajes('Exito', 'E-mail enviado correctamente');
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Enviar',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.enviar();
                }
            }
        ]
    });
}

function crearDialogoRequerimientosCercanos(valores) {
    if (valores == undefined) return false;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback agregada
    if (!('CallbackCrearSinImportarCercanos' in valores)) {
        valores.CallbackCrearSinImportarCercanos = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Requerimientos Cercanos',
        Alto: 0.95,
        Ancho: 0.95,
        Url: ResolveUrl('~/IFrame/IRequerimientosCercanos.aspx?Latitud=' + (valores.Latitud + '').replace(",", ".") + '&Longitud=' + (valores.Longitud + "").replace(",", ".") + '&IdMotivo=' + valores.IdMotivo + '&Default=true'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnCargandoListener(function (cargando) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setOnReadyListener(function (id) {
                $(jAlert).CerrarDialogo();
                valores.Callback(id);
            });

            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Registrar de todos modos',
                Class: 'colorExito',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    valores.CallbackCrearSinImportarCercanos();
                }
            }
        ]
    });
}

function crearDialogoRequerimientoEditarCamposDinamicos(valores) {
    if (valores == undefined) return false;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Editar información adicional',
        Alto: 0.95,
        Ancho: 0.95,
        Url: ResolveUrl('~/IFrame/ICamposDinamicosEditar.aspx?Id=' + valores.Id + '&IdMotivo=' + valores.IdMotivo + '&IdRequerimiento=' + valores.IdRequerimiento),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnCargandoListener(function (cargando) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            iFrameContent.setCallback(function () {
                $(jAlert).CerrarDialogo();
                valores.Callback();
            })
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.editar();
                }
            }
        ]
    });
}


/* Dialogos de Orden Trabajo */

function crearDialogoOrdenTrabajoNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Nueva Orden de Trabajo</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoNuevo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            $(jAlert).find('#btnAgregarMovil').hide();
            $(jAlert).find('#btnAgregarPersonal').hide();
            $(jAlert).find('#btnAgregarFlota').hide();

            //Callback de mensajes
            iFrameContent.setOnGuardadoListener(function (ot) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Orden de trabajo registrada correctamente');

                //Informo
                valores.Callback(ot);
            });


            //Calback Tab
            iFrameContent.setOnTabChangeListener(function (tab) {
                $(jAlert).find('#btnAgregarMovil').hide();
                $(jAlert).find('#btnAgregarPersonal').hide();
                $(jAlert).find('#btnAgregarFlota').hide();

                if (tab == "#tabMoviles") {
                    $(jAlert).find('#btnAgregarMovil').show();
                }

                if (tab == "#tabPersonal") {
                    $(jAlert).find('#btnAgregarPersonal').show();
                }

                if (tab == "#tabFlotas") {
                    $(jAlert).find('#btnAgregarFlota').show();
                }
            });


            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            $(jAlert).MostrarDialogoCargando(true, true);
            iFrameContent.init(valores.Requerimientos);

            return false;
        },
        Botones:
    [
        {
            Texto: 'Cancelar'
        }, {
            id: 'btnAgregarMovil',
            Texto: 'Agregar Móvil',
            CerrarDialogo: false,
            Visible: false,
            OnClick: function (jAlert, iFrame, iFrameContent) {
                iFrameContent.agregarMovil();
            }
        }, {
            id: 'btnAgregarFlota',
            Texto: 'Agregar Flota',
            CerrarDialogo: false,
            Visible: false,
            OnClick: function (jAlert, iFrame, iFrameContent) {
                iFrameContent.agregarFlota();
            }
        },
         {
             id: 'btnAgregarPersonal',
             Texto: 'Agregar Personal',
             CerrarDialogo: false,
             Visible: false,
             OnClick: function (jAlert, iFrame, iFrameContent) {
                 iFrameContent.agregarEmpleado();
             }
         },

        {
            Texto: 'Aceptar',
            Class: 'colorExito',
            CerrarDialogo: false,
            OnClick: function (jAlert, iFrame, iFrameContent) {
                iFrameContent.guardar();
            }
        }
    ]
    });
}

function crearDialogoOrdenTrabajoDetalle(valores) {
    if (valores == undefined) {
        return false;
    }

    //Id
    var id;
    if (!('Id' in valores)) return false;
    id = valores.Id;

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoDetalle.aspx?Id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setOnPanelAbiertoListener(function (abierto) {
                if (abierto) {
                    $(jAlert).DeshabilitarBotonesFooter();
                    return;
                }

                $(jAlert).HabilitarBotonesFooter();
            });
        },
        Botones:
            [
                {
                    Texto: 'Imprimir',
                    Id: 'btnImprimir',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        $(jAlert).find('#btnImprimir').MenuFlotante({
                            PosicionX: 'izquierda',
                            PosicionY: 'abajo',
                            Menu: [
                                {
                                    Texto: 'Imprimir resumen',
                                    Icono: 'print',
                                    OnClick: function () {
                                        iFrameContent.imprimirResumenSinMapa();
                                    }
                                },
                                {
                                    Texto: 'Imprimir orden detallada',
                                    Icono: 'print',
                                    OnClick: function () {
                                        iFrameContent.imprimirOrdenDatallada();
                                    }
                                }
                            ]
                        })

                    }
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    OnClick: function () {
                        valores.Callback();
                    }
                }
            ]
    });
}

function crearDialogoOrdenTrabajoCerrar(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Completar Orden de Trabajo</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoCerrar.aspx?IdOT=' + id),
        Width: 1000,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            })

            //Cerrar OT
            iFrameContent.setCallback(function () {
                top.mostrarMensaje('Exito', 'La operacion se completó con éxito. Orden de trabajo cerrada correctamente');
                valores.Callback();
                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar',
                    OnClick: function () {
                        valores.Callback();
                    }
                },
                 {
                     id: 'btnCerrar',
                     Texto: 'Aceptar',
                     Class: 'colorExito',
                     CerrarDialogo: false,
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.completar();
                     }
                 }
            ]
    });
}

function crearDialogoOrdenTrabajoListado(valores) {
    if (valores == undefined) return false;

    if (!('Ids' in valores)) return false;
    var ids = valores.Ids;

    var callbackMensaje = function () { };
    if ('CallbackMensajes' in valores) {
        callbackMensaje = valores.CallbackMensajes;
    }

    var titulo = null;
    if ('Titulo' in valores) {
        titulo = valores.Titulo;
    }

    crearDialogoIFrame({
        Titulo: '<label>' + titulo + '</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoListado.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setIds(ids);

            //Mensjaes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            return false;
        },
        Botones:
       [
           {
               Texto: 'Imprimir',
               CerrarDialogo: false,
               OnClick: function (jAlert, iFrame, iFrameContent) {
                   iFrameContent.imprimir();
               }
           },
           {
               Texto: 'Aceptar',
               Class: 'colorExito'
           }
       ]
    });
}

function crearDialogoOrdenTrabajoAgregarRequerimientos(valores) {
    if (valores == undefined) valores = {};
    if (!('Id' in valores)) return false;


    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Agregar requerimientos a OT</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoAgregarRequerimiento.aspx?IdOT=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setCallback(function () {
                valores.Callback();
                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Agregar requerimientos',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.guardar();
                    }
                }
            ]
    });
}

function crearDialogoOrdenTrabajoRecursos(valores) {
    if (valores == undefined) valores = {};
    if (!('Id' in valores)) return false;

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Recursos de Orden de Trabajo</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoRecursos.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setCallback(function () {
                valores.Callback();
                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.guardar();
                    }
                }
            ]
    });
}

function crearDialogoOrdenTrabajoCambiarSeccion(valores) {
    if (valores == undefined) valores = {};
    if (!('Id' in valores)) return false;

    if (!('IdArea' in valores)) return false;

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Cambiar Sección</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoCambiarSeccion.aspx?Id=' + valores.Id + '&IdArea=' + valores.IdArea + '&IdSeccion=' + valores.IdSeccion),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setCallback(function () {
                valores.Callback();
                $(jAlert).CerrarDialogo();
            });
        },
        Width: 600,
        Height: 120,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.cambiarSeccion();
                    }
                }
            ]
    });
}

function crearDialogoOrdenTrabajoQuitarRequerimiento(valores) {
    if (valores == undefined) valores = {};

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Cambiar estado de requerimiento</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoQuitarRequerimiento.aspx?IdOt=' + valores.IdOt + '&IdRq=' + valores.IdRq),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setCallback(function () {
                valores.Callback();
                $(jAlert).CerrarDialogo();
            });
        },
        Width: 600,
        Height: 500,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Cambiar estado',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.quitar();
                    }
                }
            ]
    });
}

function crearDialogoOrdenTrabajoEnviarMail(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) {
        console.log('Debe indicar el id de la Orden a la cual le quiere mandar un email.');
        return false;
    }

    //Callback agregada
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Enviar mail de Orden de Trabajo',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoEnviarMail.aspx?id=' + valores.Id),
        Height: 120,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnCargandoListener(function (cargando) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setOnComprobanteReenviadoListener(function () {
                $(jAlert).CerrarDialogo();

                top.mostrarMensaje('Exito', 'Mail de orden enviado correctamente');
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                CerrarDialogo: false,
                Class: 'colorExito',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.reenviarComprobante();
                }
            }
        ]
    });
}

/* Dialogos de Orden Inspeccion */

function crearDialogoOrdenInspeccionNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Nueva Orden de Inspección</label>',
        Url: ResolveUrl('~/IFrame/IOrdenInspeccionNuevo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnGuardadoListener(function (oi) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Orden de inspección registrada correctamente');

                //Informo
                valores.Callback(oi);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            $(jAlert).MostrarDialogoCargando(true, true);
            iFrameContent.init(valores.Requerimientos);

            return false;
        },
        Botones:
    [
        {
            Texto: 'Cancelar'
        },
        {
            Texto: 'Aceptar',
            Class: 'colorExito',
            CerrarDialogo: false,
            OnClick: function (jAlert, iFrame, iFrameContent) {
                iFrameContent.guardar();
            }
        }
    ]
    });
}

function crearDialogoOrdenInspeccionDetalle(valores) {
    if (valores == undefined) {
        return false;
    }

    //Id
    var id;
    if (!('Id' in valores)) return false;
    id = valores.Id;

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IOrdenInspeccionDetalle.aspx?Id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setOnPanelAbiertoListener(function (abierto) {
                if (abierto) {
                    $(jAlert).DeshabilitarBotonesFooter();
                    return;
                }

                $(jAlert).HabilitarBotonesFooter();
            });
        },
        Botones:
            [

                {
                    Texto: 'Imprimir',
                    Id: 'btnImprimir',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        $(jAlert).find('#btnImprimir').MenuFlotante({
                            PosicionX: 'izquierda',
                            PosicionY: 'abajo',
                            Menu: [

                                {
                                    Texto: 'Imprimir resumen',
                                    Icono: 'print',
                                    OnClick: function () {
                                        iFrameContent.imprimirResumenSinMapa();
                                    }
                                },
                                {
                                    Texto: 'Imprimir orden detallada',
                                    Icono: 'print',
                                    OnClick: function () {
                                        iFrameContent.imprimirOrdenDatallada();
                                    }
                                }
                            ]
                        })

                    }
                },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function () {
                    valores.Callback();
                }
            }
            ]
    });
}

function crearDialogoOrdenInspeccionAgregarRequerimientos(valores) {
    if (valores == undefined) valores = {};
    if (!('Id' in valores)) return false;


    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Agregar requerimientos a OI</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoAgregarRequerimiento.aspx?IdOI=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setCallback(function () {
                valores.Callback();
                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.guardar();
                    }
                }
            ]
    });
}

function crearDialogoOrdenInspeccionQuitarRequerimiento(valores) {
    if (valores == undefined) valores = {};

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Cambiar estado de requerimiento</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoQuitarRequerimiento.aspx?IdOi=' + valores.IdOi + '&IdRq=' + valores.IdRq),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.setCallback(function () {
                valores.Callback();
                $(jAlert).CerrarDialogo();
            });
        },
        Width: 600,
        Height: 500,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Cambiar estado',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.quitar();
                    }
                }
            ]
    });
}

function crearDialogoOrdenInspeccionCompletar(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback editado
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Completar Orden de Inspección</label>',
        Url: ResolveUrl('~/IFrame/IOrdenTrabajoCerrar.aspx?IdOI=' + id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            })

            //Cerrar OT
            iFrameContent.setCallback(function () {
                top.mostrarMensaje('Exito', 'La operacion se completó con éxito. Orden de inspección completada correctamente');
                valores.Callback();
                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar',
                    OnClick: function () {
                        valores.Callback();
                    }
                },
                 {
                     id: 'btnCerrar',
                     Texto: 'Aceptar',
                     Class: 'colorExito',
                     CerrarDialogo: false,
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.completar();
                     }
                 }
            ]
    });
}

/* Dialogos de Orden de Atencion Critica */
function crearDialogoOrdenAtencionCritica(valores) {
    var descripcion = "";
    var editar = false;
    var ordenId = valores.OrdenAtencionCritica.Id;

    if (ordenId != undefined) {
        descripcion = valores.OrdenAtencionCritica.Descripcion;
        editar = true;
    }

    crearDialogoHTML({
        Titulo: '<label>Orden de Atención Crítica</label>',
        Content: '<div class="row margin-top" >' +
                            '<div class="col s12">' +
                                '<div class="input-field">' +
                                    '<textarea id="inputFormulario_Descripcion" class="materialize-textarea">' + descripcion + '</textarea>' +
                                    '<label for="inputFormulario_Descripcion" class=" no-select textarea">Descripción</label>' +
                                '</div>' +
                            '</div>' +
                        '</div>',
        OnLoad: function (jAlert) {
            setTimeout(function () {
                $(jAlert).find('#inputFormulario_Descripcion').trigger('focus');
            }, 300);
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert) {
                        var descripcion = $(jAlert).find('#inputFormulario_Descripcion').val();

                        var ordenEspecial = {}
                        ordenEspecial.descripcion = '' + descripcion;
                        ordenEspecial.idRequerimiento = valores.OrdenAtencionCritica.IdRequerimiento;
                        if (ordenId != undefined) {
                            ordenEspecial.id = ordenId;
                        }

                        var url = ResolveUrl('~/Servicios/OrdenAtencionCriticaService.asmx/Insertar');
                        if (editar) {
                            url = ResolveUrl('~/Servicios/OrdenAtencionCriticaService.asmx/Editar');
                        }

                        crearAjax({
                            Url: url,
                            Data: { comando: ordenEspecial },
                            OnSuccess: function (result) {
                                if (!result.Ok) {
                                    valores.CallbackMensajes('Error', result.Error);
                                    return;
                                }

                                var mensaje = 'Se ha creado con éxito la orden de atención crítica para el requerimiento.';
                                if (editar) {
                                    mensaje = 'Se ha editado con éxito la orden de atención crítica.';
                                }

                                valores.CallbackMensajes('Exito', mensaje);

                                valores.CallbackEditar(result.Return);
                            },
                            OnError: function (result) {
                                valores.CallbackMensajes('Error', result.Error);
                            }
                        });

                        $(jAlert).CerrarDialogo();
                    }
                }
            ]
    });
}

function crearDialogoDetalleOrdenAtencionCritica(valores) {
    if (valores == undefined) {
        return false;
    }

    //Id
    var id;
    if (!('Id' in valores)) return false;
    id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de Orden de Atención Crítica</label>',
        Url: ResolveUrl('~/IFrame/IOrdenAtencionCriticaDetalle.aspx?Id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                 {
                     id: 'btnGenerarMapa',
                     Texto: 'Mapa',
                     CerrarDialogo: false,
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.generarMapa();
                     }
                 },
                 {
                     Texto: 'Aceptar',
                     Class: 'colorExito'
                 }
            ]
    });
}

function crearDialogoEditarOrdenAtencionCritica(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) {
        callbackMensajes = valores.CallbackMensajes;
    }

    //Callback editado
    var callbackEditar = function () { };
    if ('CallbackEditar' in valores) {
        callbackEditar = valores.CallbackEditar;
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar Orden de Atención Crítica</label>',
        Url: ResolveUrl('~/IFrame/IOrdenAtencionCriticaEditar.aspx?id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Oculto los botones
            $(jAlert).find('#btnAgregarNota').hide();
            $(jAlert).find('#btnAgregarMovil').hide();
            $(jAlert).find('#btnAgregarRequerimiento').hide();

            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            })

            //Calback Tab
            iFrameContent.setOnTabChangeListener(function (tab) {
                $(jAlert).find('#btnAgregarNota').hide();
                $(jAlert).find('#btnAgregarMovil').hide();
                $(jAlert).find('#btnAgregarRequerimiento').hide();
                $(jAlert).find('#btnGenerarMapa').hide();

                switch (tab) {
                    case '#tabDetalle':
                        $(jAlert).find('#btnAgregarNota').hide();
                        $(jAlert).find('#btnAgregarMovil').hide();
                        $(jAlert).find('#btnAgregarRequerimiento').hide();
                        $(jAlert).find('#btnGenerarMapa').hide();
                        break;
                    case '#tabReclamos':
                        $(jAlert).find('#btnAgregarMovil').hide();
                        $(jAlert).find('#btnAgregarNota').hide();
                        $(jAlert).find('#btnAgregarRequerimiento').show();
                        $(jAlert).find('#btnGenerarMapa').show();
                        break;
                    case '#tabRecurso':
                        $(jAlert).find('#btnAgregarMovil').hide();
                        $(jAlert).find('#btnAgregarNota').hide();
                        $(jAlert).find('#btnAgregarRequerimiento').hide();
                        $(jAlert).find('#btnGenerarMapa').hide();
                        break;
                    case '#tabNotas':
                        $(jAlert).find('#btnAgregarMovil').hide();
                        $(jAlert).find('#btnAgregarNota').show();
                        $(jAlert).find('#btnAgregarRequerimiento').hide();
                        $(jAlert).find('#btnGenerarMapa').hide();
                        break;
                    case '#tabMoviles':
                        $(jAlert).find('#btnAgregarMovil').show();
                        $(jAlert).find('#btnAgregarNota').hide();
                        $(jAlert).find('#btnAgregarRequerimiento').hide();
                        $(jAlert).find('#btnGenerarMapa').hide();
                        break;
                }
            });

            //Editado
            iFrameContent.setOnEditarCompletoListener(function (ot) {
                callbackMensajes('Exito', 'Orden de trabajo editada correctamente');
                callbackEditar(ot);

                $(jAlert).CerrarDialogo();
            });

            $(jAlert).MostrarDialogoCargando(false, true);
            return false;
        },
        Botones:
            [
                {
                    Texto: 'Cancelar',
                    class: 'btn waves-effect'
                },
                //{
                //    id: 'btnGenerarMapa',
                //    Texto: 'MAPA',
                //    class: 'btn waves-effect no-select colorExito',
                //    CerrarDialogo: false,
                //    OnClick: function (jAlert, iFrame, iFrameContent) {
                //        iFrameContent.generarMapa();
                //    }
                //},
                {
                    id: 'btnGuardarCambios',
                    Texto: 'Guardar',
                    Class: 'btn waves-effect  waves-light colorExito ',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

function crearDialogoCompletarOrdenAtencionCritica(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) {
        callbackMensajes = valores.CallbackMensajes;
    }

    //Callback editado
    var callbackCompletar = function () { };
    if ('CallbackCompletar' in valores) {
        callbackCompletar = valores.CallbackCompletar;
    }

    crearDialogoHTML({
        Titulo: '<label>Completar Orden de Atención Crítica</label>',
        Content: '<div class="row margin-top" >' +
                            '<div class="col s12">' +
                                '<div class="input-field">' +
                                    '<textarea id="inputFormulario_Observaciones" class="materialize-textarea"></textarea>' +
                                    '<label for="inputFormulario_Observaciones" class=" no-select textarea">Observaciones</label>' +
                                '</div>' +
                            '</div>' +
                        '</div>',
        OnLoad: function (jAlert) {
            $(jAlert).find('#inputFormulario_Observaciones').trigger('focus');
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert) {

                        //Muestro el cargando
                        $(jAlert).MostrarDialogoCargando(true, true);

                        var obs = $(jAlert).find('#inputFormulario_Observaciones').val();
                        var parametros = { Id: id, Descripcion: obs };


                        crearAjax({
                            Data: { comando: parametros },
                            Url: ResolveUrl('~/Servicios/OrdenAtencionCriticaService.asmx/Completar'),
                            OnSuccess: function (result) {

                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false, true);

                                //Error
                                if (!result.Ok) {
                                    callbackMensajes('Error', result.Error.Publico);
                                    console.log('Error completando la Orden de Atención Crítica');
                                    console.log(result);
                                    return;
                                }

                                //Informo
                                callbackMensajes('Exito', 'Orden de Atención Crítica completada correctamente.');
                                callbackCompletar(result.Return);

                                //Cierro el dialogo
                                $(jAlert).CerrarDialogo();
                            },
                            OnError: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false, true);

                                callbackMensajes('Error', 'Error al completar la Orden de Atención Crítica');
                                console.log(result);
                            }
                        });
                    }
                }]
    });
}

/*Funcion */
function crearDialogoFuncionNueva(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Funciones',
        Url: ResolveUrl('~/IFrame/IFuncionNueva.aspx?IdArea=' + valores.IdArea),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                              {
                                  Texto: 'Aceptar',
                                  CerrarDialogo: true,
                                  Class: 'colorExito',
                                  OnClick: function (jAlert, iFrame, iFrameContent) {
                                      valores.Callback(iFrameContent.getFunciones());
                                  }
                              }
            ]
    });
}

/*Dialogos Empleado */
function crearDialogoEmpleadoNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Nuevo Empleado</label>',
        Url: ResolveUrl('~/IFrame/IEmpleadoNuevo.aspx?IdArea=' + valores.IdArea),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Height: '300px',
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });

}

function crearDialogoEmpleadoDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('Callback' in valores)) valores.Callback = function () { };

    if (!('CallbackCargando' in valores)) valores.CallbackCargando = function () { };

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IEmpleadoDetalle.aspx?id=' + id),
        Botones: [
               {
                   Texto: 'Aceptar',
                   Class: 'colorExito',
                   OnClick: function () {
                       valores.Callback();
                   }
               }
        ],
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrame.setOnCargandoListener(function (mostrar, mensaje) {
                $(jAlert).MostrarDialogoCargando(mostrar, mensaje);
            });

            iFrameContent.setOnPanelAbiertoListener(function (abierto) {
                if (abierto) {
                    $(jAlert).DeshabilitarBotonesFooter(abierto);
                    return;
                }

                $(jAlert).HabilitarBotonesFooter(abierto);
            });
        }
    })
}

function crearDialogoEmpleadoEditarFunciones(valores) {
    if (valores == undefined) return false;

    if (!('IdEmpleado' in valores)) return false;
    var id = valores.IdEmpleado;

    if (!('Callback' in valores)) valores.Callback = function () { };

    if (!('CallbackCargando' in valores)) valores.CallbackCargando = function () { };

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IEmpleadoEditarFunciones.aspx?idEmpleado=' + id),
        Botones: [
               {
                   Texto: 'Cancelar'
               },
               {
                   Texto: 'Guardar',
                   Class: 'colorExito',
                   CerrarDialogo: false,
                   OnClick: function (jAlert, iFrame, iFrameContent) {

                       iFrameContent.registrar();
                   }
               }
        ],
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrame.setOnCargandoListener(function (mostrar, mensaje) {
                $(jAlert).MostrarDialogoCargando(mostrar, mensaje);
            });

            iFrame.setListener(function (empleado) {
                valores.CallbackMensajes('Exito', 'Las funciones del empleado fueron editadas correctamente');
                valores.Callback(empleado);

                $(jAlert).CerrarDialogo();
            });
        },
        Height: '190px'
    })
}

function crearDialogoEmpleadoCambiarEstado(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Id Estado Anterior
    if (!('IdEstadoAnterior' in valores)) return false;
    var idEstadoAnterior = valores.IdEstadoAnterior;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    crearDialogoIFrame({
        Titulo: 'Cambiar estado de Empleado',
        Url: ResolveUrl('~/IFrame/IEmpleadoCambiarEstado.aspx?Id=' + id + '&IdEstadoAnterior=' + idEstadoAnterior),
        Height: 240,
        Width: 600,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Cambair Estado 
            iFrameContent.setOnEstadoCambiadoListener(function () {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Estado cambiado correctamente');

                //Informo que cambie
                valores.Callback();
            });
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Cambiar estado',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.cambiarEstado();
                     }
                 }
            ]
    });
}

function crearDialogoAgregarEmpleado(valores) {
    if (valores == undefined) return false;

    if (!('IdArea' in valores)) return false;
    if (!('IdsEmpleados' in valores)) return false;
    if (!('IdOT' in valores)) {
        valores.IdOT = 0;
    };

    crearDialogoIFrame({
        Titulo: 'Selección de Empleados',
        Url: ResolveUrl('~/IFrame/IEmpleadoSelector.aspx?idArea=' + valores.IdArea + '&idOT=' + valores.IdOT),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            iFrameContent.setOnCargandoListener(function (mostrar, mensaje) {
                $(jAlert).MostrarDialogoCargando(mostrar, mensaje);
            });

            iFrameContent.setOnSeleccionadoListener(valores.CallbackSeleccionar);

            iFrameContent.setOnObjetosSeleccionadoListener(valores.CallbackObjetosSeleccionar);

            iFrameContent.setCallbackInit(function () {
                iFrameContent.inicializarPantalla(valores.IdsEmpleados);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                CerrarDialogo: true,
                Class: 'colorExito',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.informarSeleccion();
                }
            }
        ],
    });
}

/*Dialogos Flota */
function crearDialogoAgregarFlota(valores) {
    if (valores == undefined) return false;

    if (!('IdArea' in valores)) return false;
    if (!('IdsFlotas' in valores)) return false;
    var params = '?idArea=' + valores.IdArea;
    if ('IdOT' in valores) {
        params += '&idOt=' + valores.IdOT;
    };

    crearDialogoIFrame({
        Titulo: 'Selección de Flota',
        Url: ResolveUrl('~/IFrame/IFlotaSelector.aspx' + params),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            iFrameContent.setOnCargandoListener(function (mostrar, mensaje) {
                $(jAlert).MostrarDialogoCargando(mostrar, mensaje);
            });

            iFrameContent.setOnSeleccionadoListener(valores.CallbackSeleccionar);

            iFrameContent.setOnObjetosSeleccionadoListener(valores.CallbackObjetosSeleccionar);

            iFrameContent.setCallbackInit(function () {
                iFrameContent.inicializarPantalla(valores.IdsFlotas);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                CerrarDialogo: true,
                Class: 'colorExito',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.informarSeleccion();
                }
            }
        ],
    });
}

function crearDialogoFlotaDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('Callback' in valores)) valores.Callback = function () { };

    if (!('CallbackCargando' in valores)) valores.CallbackCargando = function () { };

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IFlotaDetalle.aspx?id=' + id),
        Botones: [
               {
                   Texto: 'Aceptar',
                   Class: 'colorExito',
                   OnClick: function () {
                       valores.Callback();
                   }
               }
        ],
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrame.setOnCargandoListener(function (mostrar, mensaje) {
                $(jAlert).MostrarDialogoCargando(mostrar, mensaje);
            });

            iFrameContent.setOnPanelAbiertoListener(function (abierto) {
                if (abierto) {
                    $(jAlert).DeshabilitarBotonesFooter(abierto);
                    return;
                }

                $(jAlert).HabilitarBotonesFooter(abierto);
            });
        }
    })
}

/* Dialogos de Tarea */
function crearDialogoTareaNueva(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/ITareaNueva.aspx?IdArea=' + valores.IdArea;

    crearDialogoIFrame({
        Titulo: '<label>Nueva Tarea</label>' + valores.NombreArea,
        Height: '200px',
        Width: '400px',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Ancho: 0.95,
        Alto: 0.95,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoTareaDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('Callback' in valores)) valores.Callback = function () { };

    if (!('CallbackCargando' in valores)) valores.CallbackCargando = function () { };

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/ITareaPorAreaDetalle.aspx?Id=' + id),
        Botones: [
               {
                   Texto: 'Aceptar',
                   Class: 'colorExito',
                   OnClick: function () {
                       valores.Callback();
                   }
               }
        ],
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrame.setOnCargandoListener(function (mostrar, mensaje) {
                $(jAlert).MostrarDialogoCargando(mostrar, mensaje);
            });
        }
    })
}

/* Dialogos de Edificio Municipal */
function crearDialogoEdificioMunicipalNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var titulo = '<label>Nuevo Edificio Municipal</label>';
    var url = '~/IFrame/IEdificioMunicipalNuevo.aspx';

    if ('IdCategoria' in valores && valores.IdCategoria != undefined) {
        url = '~/IFrame/IEdificioMunicipalNuevo.aspx?IdCategoria=' + valores.IdCategoria;
    }

    crearDialogoIFrame({
        Titulo: titulo,
        Height: '300px',
        Width: '300px',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            ////Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
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
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoEdificioMunicipalEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var titulo = '<label>Editar Edificio Municipal</label>';
    var url = '~/IFrame/IEdificioMunicipalNuevo.aspx';

    if ('Id' in valores && valores.Id != undefined) {
        url = '~/IFrame/IEdificioMunicipalNuevo.aspx?Id=' + valores.Id;
    }

    crearDialogoIFrame({
        Titulo: titulo,
        Height: '300px',
        Width: '300px',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            ////Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

/* Dialogos de Seccion */
function crearDialogoSeccionNueva(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/ISeccionNueva.aspx?IdArea=' + valores.IdArea;

    crearDialogoIFrame({
        Titulo: '<label>Nueva Sección</label>' + valores.NombreArea,
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Ancho: 0.95,
        Alto: 0.95,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoEditarSeccion(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback editado
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar Sección</label>',
        Url: ResolveUrl('~/IFrame/ISeccionNueva.aspx?Id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            $(jAlert).find('#btnAgregarNota').hide();

            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Editar 
            iFrameContent.setListener(function (rq) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Sección editada correctamente');

                //Informo que edite
                valores.CallbackEditar(rq);
            });

            return false;
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Guardar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.editar();
                     }
                 }
            ]
    });
}

function crearDialogoSeccionDarDeBaja(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) {
        callbackMensajes = valores.CallbackMensajes;
    }

    //Callback editado
    var callbackDarDeBaja = function () { };
    if ('CallbackDarDeBaja' in valores) {
        callbackDarDeBaja = valores.CallbackDarDeBaja;
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de baja sección</label>',
        Content:
            '<div class="padding">' +
            '<label id="textoConfirmacion"class="titulo">¿Está seguro de dar de baja la sección?</label>' +
            '</div>',
        Botones:
            [
                {
                    Id: 'btnNo',
                    Texto: 'No'
                },
                {
                    Id: 'btnSi',
                    Texto: 'Si',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert) {

                        //Muestro el cargando
                        // mostrarCargando(true);
                        $(jAlert).MostrarDialogoCargando(true);

                        var dataAjax = { Id: id };

                        var urlAjax = ResolveUrl('~/Servicios/SeccionService.asmx/DarDeBaja');


                        crearAjax({
                            Data: { comando: dataAjax },
                            Url: urlAjax,
                            OnSuccess: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false);

                                //Error
                                if (!result.Ok) {
                                    callbackMensajes('Error', result.Error);
                                    return;
                                }

                                //Muestro el mensaje de OK
                                callbackMensajes('Exito', 'Sección dada de baja correctamente');
                                callbackDarDeBaja(result.Return);

                                //Cierro el dialogo
                                $(jAlert).CerrarDialogo();
                            },
                            OnError: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false);

                                //Muestro el Error
                                callbackMensajes('Error', 'Error eliminando la sección');
                            }
                        });
                    }
                }]
    });

}

function crearDialogoSeccionDarDeAlta(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) {
        callbackMensajes = valores.CallbackMensajes;
    }

    //Callback editado
    var callbackDarDeAlta = function () { };
    if ('CallbackDarDeAlta' in valores) {
        callbackDarDeAlta = valores.CallbackDarDeAlta;
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de alta sección</label>',
        Content:
            '<div class="padding">' +
            '<label id="textoConfirmacion"class="titulo">¿Está seguro de dar de alta la sección?</label>' +
            '</div>',
        Botones:
            [
                {
                    Id: 'btnNo',
                    Texto: 'No'
                },
                {
                    Id: 'btnSi',
                    Texto: 'Si',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert) {

                        //Muestro el cargando
                        // mostrarCargando(true);
                        $(jAlert).MostrarDialogoCargando(true);

                        var dataAjax = { Id: id };

                        var urlAjax = ResolveUrl('~/Servicios/SeccionService.asmx/DarDeAlta');


                        crearAjax({
                            Data: { comando: dataAjax },
                            Url: urlAjax,
                            OnSuccess: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false);

                                //Error
                                if (!result.Ok) {
                                    callbackMensajes('Error', result.Error);
                                    return;
                                }

                                //Muestro el mensaje de OK
                                callbackMensajes('Exito', 'Sección dada de alta correctamente');
                                callbackDarDeAlta(result.Return);

                                //Cierro el dialogo
                                $(jAlert).CerrarDialogo();
                            },
                            OnError: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false);

                                //Muestro el Error
                                callbackMensajes('Error', 'Error dando de alta la sección');
                            }
                        });
                    }
                }]
    });

}


/* Reportes  de Estadisticas */
function crearDialogoReporteEstadisticaCPC(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaCPC(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function crearDialogoReporteEstadisticaOrigen(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaOrigen(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteEstadisticaEficacia(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaEficacia(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteEstadisticaResueltos(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaResueltos(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteEstadisticaArea(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaArea(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteEstadisticaSubArea(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaSubArea(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteEstadisticaServicios(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaServicios(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function crearDialogoReporteEstadisticaMotivos(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaMotivos(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteEstadisticaRubros(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaRubros(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function crearDialogoReporteEstadisticaZona(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaZona(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function crearDialogoReporteEstadisticaUsuario(base64, consulta, htmlFiltros) {


    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteEstadisticaUsuario(base64, consulta, htmlFiltros);

            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}


/* Reportes  de Requerimiento */
function crearDialogoReporteRequerimientoNuevo(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx?tipo=rq_nuevo&id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}

function crearDialogoReporteRequerimientoDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx?tipo=rq_detalle&id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}

function crearDialogoReporteRequerimientoDetalleMapa(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx?tipo=rq_detalle_mapa&id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}

function crearDialogoReporteRequerimientoDetalleSinMapa(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx?tipo=rq_detalle_sinMapa&id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}

//function crearDialogoReporteRequerimientoListado(valores) {
//    if (valores == undefined) return false;

//    if (!('Ids' in valores)) return false;
//    var ids = valores.Ids;

//    //if (!('Filtros' in valores)) return false;
//    var filtros = valores.Filtros;

//    let url = ResolveUrl('~/IFrame/IReporte.aspx');
//    console.log(url);

//    crearDialogoIFrame({
//        Url: url,
//        Ancho: 0.95,
//        Alto: 0.95,
//        OnLoad: function (jAlert, iFrame, iFrameContent) {
//            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
//                callbackMensaje(tipo, mensaje);
//            });

//            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
//                $(jAlert).MostrarDialogoCargando(cargando, true);
//            });

//            iFrameContent.GenerarReporteListadoRequerimiento(ids, filtros);
//            return false;
//        },
//        Botones: [
//            {
//                Texto: 'Aceptar',
//                Class: 'colorExito'
//            }
//        ]
//    });
//}
function crearDialogoReporteRequerimientoListado(valores) {
    if (valores == undefined) return false;

    if (!('Ids' in valores)) return false;
    var ids = valores.Ids;

    if (!('Filtros' in valores)) return false;
    var filtros = valores.Filtros;

    let url = ResolveUrl('~/IFrame/IGenerarExcel.aspx');
    console.log(url);

    crearDialogoIFrame({
        Url: url,
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteRequerimientoListadoV2(ids, filtros);
            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function crearDialogoReporteRequerimientoListadoV2(valores) {
    if (valores == undefined) return false;

    if (!('Ids' in valores)) return false;
    var ids = valores.Ids;

    //if (!('Filtros' in valores)) return false;
    var filtros = valores.Filtros;

    let url = ResolveUrl('~/IFrame/IReporte.aspx');
    console.log(url);

    crearDialogoIFrame({
        Url: url,
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteRequerimientoListadoV2(ids, filtros);
            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}


/*Reportes de OT*/
function crearDialogoReporteOrdenTrabajoCaratulaSinMapa(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx?tipo=ot_detalle_caratula_sinMapa&id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}
function crearDialogoReporteOrdenTrabajoListado(valores) {
    if (valores == undefined) return false;

    if (!('Ids' in valores)) return false;
    var ids = valores.Ids;

    if (!('Filtros' in valores)) return false;
    var filtros = valores.Filtros;

    let url = ResolveUrl('~/IFrame/IReporte.aspx');
    console.log(url);

    crearDialogoIFrame({
        Url: url,
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteListadoOrdenTrabajo(ids, filtros);
            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteOrdenTrabajoDatallada(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx?tipo=ot_detallada&id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}

/*Reportes de OT*/
function crearDialogoReporteOrdenInspeccionCaratulaSinMapa(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx?tipo=oi_detalle_caratula_sinMapa&id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}
function crearDialogoReporteOrdenInspeccionListado(valores) {
    if (valores == undefined) return false;

    if (!('Ids' in valores)) return false;
    var ids = valores.Ids;

    if (!('Filtros' in valores)) return false;
    var filtros = valores.Filtros;

    let url = ResolveUrl('~/IFrame/IReporte.aspx');
    console.log(url);

    crearDialogoIFrame({
        Url: url,
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteListadoOrdenInspeccion(ids, filtros);
            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteOrdenInspeccionDatallada(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IReporte.aspx?tipo=oi_detallada&id=' + id),
        Ancho: 0.95,
        Alto: 0.95,
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}


/* Domicilio */
function crearDialogoBarrioBusqueda(valores) {

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IBarrioCatastroBusqueda.aspx'),
        Height: '100px',
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrame.setCallback(function (barrio) {
                $(jAlert).CerrarDialogo();

                valores.Callback(barrio);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            }
        ]
    });
}

function crearDialogoUbicacionManualSelector(valores) {

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IUbicacionSelector.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Sugerencia
            if (valores.Sugerencia != undefined) {
                iFrameContent.setSugerencia(valores.Sugerencia);
            }

            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.init();
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
             {
                 Texto: 'Aceptar',
                 Class: 'colorExito',
                 CerrarDialogo: false,
                 OnClick: function (jAlert, iframe, iframeContent) {
                     let u = iframeContent.seleccionar();
                     if (u == undefined) return;

                     $(jAlert).CerrarDialogo();
                     valores.Callback(u);
                 }
             },
        ]
    });
}


function crearDialogoUbicacionSelector(valores) {

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IUbicacionAutomaticoSelector.aspx'),
        Ancho: 0.4,
        Alto: 0.6,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            //iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
            //    valores.CallbackMensajes(tipo, mensaje);
            //})

            ////Callback cargando
            //iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
            //    $(jAlert).MostrarDialogoCargando(cargando, true);
            //});
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
             {
                 Texto: 'Guardar',
                 Class: 'colorExito',
                 CerrarDialogo: false,
                 OnClick: function (jAlert, iframe, iframeContent) {
                     let u = iframeContent.UbicacionAutomaticaSelector_Seleccionar();
                     if (u == undefined) return;

                     $(jAlert).CerrarDialogo();
                     valores.Callback(u);
                 }
             },
        ]
    });
}


/* Usuario */
function crearDialogoUsuarioNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!("Empleado" in valores)) {
        valores.Empleado = false;
    }

    var url = '~/IFrame/IUsuarioCerrojoNuevo.aspx';

    if (valores.Empleado) {
        url = url + '?Empleado=1';
    }

    crearDialogoIFrame({
        Titulo: '<label>Nuevo Usuario</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (data) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                top.mostrarMensaje('Exito', 'Usuario registrado correctamente');

                //Informo
                valores.Callback(data);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });


        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}
function crearDialogoUsuarioEditar(valores) {

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar Perfil</label>',
        Url: ResolveUrl('~/IFrame/IUsuarioCerrojoNuevo.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (data) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                top.mostrarMensaje('Exito', 'Usuario editado correctamente');

                //Informo
                valores.Callback(data);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}
function crearDialogoUsuarioDetalle(valores) {
    if (!('Id' in valores)) {
        return;
    }

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Detalle de usuario</label>',
        Width: 450,
        Height: 750,
        Url: ResolveUrl('~/IFrame/IUsuarioDetalle.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito'
                }
            ]
    });

}
function crearDialogoUsuarioCambiarPassword(valores) {

    if (valores == undefined) {
        valores = {};
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Cambiar contraseña</label>',
        Width: 450,
        Height: 270,
        Url: ResolveUrl('~/IFrame/IUsuarioCerrojoCambiarPassword.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback proceso
            iFrameContent.setCallback(function () {
                $(jAlert).CerrarDialogo();
                top.mostrarMensaje('Exito', 'Contraseña actualizada correctamente');
                valores.Callback();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.cambiarPassword();
                    }
                }
            ]
    });

}


/* Domicilio */

function crearDialogoMapaDomicilio(valores) {

    if (valores == undefined) valores = {};

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) { callbackMensajes = valores.CallbackMensajes; }

    //Callback Cargando
    var callbackCargando = function () { };
    if ('CallbackCargando' in valores) { callbackCargando = valores.CallbackCargando; }

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/DomicilioService.asmx/GenerarMapaPorId'),
                Data: { id: id },
                OnSuccess: function (result) {
                    $(jAlert).CerrarDialogo();

                    if (!result.Ok) {
                        valores.CallbackMensajes('Error', result.Error);
                        return;
                    }

                    crearDialogoIFrame({
                        Url: result.Return,
                        Ancho: 0.95,
                        Alto: 0.95,
                        Botones:
                            [
                                {
                                    Texto: 'Aceptar',
                                    Class: 'colorExito'
                                }
                            ]
                    });
                },
                OnError: function (result) {
                    $(jAlert).CerrarDialogo();

                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}
function crearDialogoMapaGoogleByIdRequerimiento(valores) {
    if (!('Id' in valores)) return false;

    var ids = [];
    ids.push(valores.Id);
    valores.Ids = ids;

    crearDialogoMapaGoogleByIdsRequerimiento(valores);
}
function crearDialogoMapaGoogleByIdsRequerimiento(valores) {

    if (valores == undefined) valores = {};

    if (!('Ids' in valores)) return false;
    var ids = valores.Ids;

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IRequerimientoMapa.aspx'),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando);
            });

            iFrameContent.setListenerReady(function () {

                iFrameContent.verMarcadores(ids, function (id) {
                    crearDialogoRequerimientoDetalle({
                        Id: id
                    });
                });
            });

            return false;
        },
        Botones: [
            {
                Texto: 'Imprimir',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.imprimir();
                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}


/* Origen */
function crearDialogoOrigenNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Nuevo Origen</label>',
        Url: ResolveUrl('~/IFrame/IOrigenNuevo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Origen registrado correctamente');

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoOrigenEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    if (!('Id' in valores)) {
        valores.Id = 0;
    }

    crearDialogoIFrame({
        Titulo: '<label>Editar Origen</label>',
        Url: ResolveUrl('~/IFrame/IOrigenNuevo.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Origen editado correctamente');

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

function crearDialogoOrigenDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    crearDialogoIFrame({
        Titulo: 'Detalle de Origen',
        Url: ResolveUrl('~/IFrame/IOrigenDetalle.aspx?id=' + id),
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    })
}

function crearDialogoOrigenDarDeBaja(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Confirmar dar de baja</label>',
        Content: '<div class="margin"><label>¿Esta seguro que desea dar de baja el origen seleccionado?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar',
                OnClick: function () {

                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorError',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/OrigenService.asmx/DarDeBaja'),
                        Data: { id: id },
                        OnSuccess: function (resultado) {

                            $(jAlert).CerrarDialogo();

                            if (!resultado.Ok) {
                                valores.CallbackMensajes('Error', resultado.Error);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Origen dado de baja');
                            valores.Callback(resultado.Return);
                        },
                        OnError: function (resultado) {
                            $(jAlert).CerrarDialogo();
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    });
}

function crearDialogoOrigenDarDeAlta(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrigenService.asmx/DarDeAlta'),
                Data: { id: id },
                OnSuccess: function (resultado) {

                    $(jAlert).CerrarDialogo();

                    if (!resultado.Ok) {
                        valores.CallbackMensajes('Error', resultado.Error);
                        return;
                    }

                    valores.CallbackMensajes('Exito', 'Origen dado de alta');
                    valores.Callback(resultado.Return);
                },
                OnError: function (resultado) {
                    $(jAlert).CerrarDialogo();
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}


/* Origen por Area */
function crearDialogoOrigenPorAreaNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IOrigenPorAreaNuevo.aspx';
    var params = [];
    if ('IdArea' in valores && valores.IdArea != undefined) {
        params.push('IdArea=' + valores.IdArea);
    }
    if ('IdOrigen' in valores && valores.IdOrigen != undefined) {
        params.push('IdOrigen=' + valores.IdOrigen);
    }

    if (params.length != 0) {
        url += "?" + params.join("&");;
    }

    crearDialogoIFrame({
        Titulo: '<label>Asignar Origen</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Origen asignado correctamente');

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoOrigenPorAreaDarDeBaja(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Confirmar dar de baja</label>',
        Content: '<div class="margin"><label>¿Esta seguro que desea dar de baja el origen seleccionado?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar',
                OnClick: function () {

                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorError',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/OrigenPorAreaService.asmx/DarDeBaja'),
                        Data: { id: id },
                        OnSuccess: function (resultado) {

                            $(jAlert).CerrarDialogo();

                            if (!resultado.Ok) {
                                valores.CallbackMensajes('Error', resultado.Error);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Origen dado de baja');
                            valores.Callback(resultado.Return);
                        },
                        OnError: function (resultado) {
                            $(jAlert).CerrarDialogo();
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    });
}


/* Origen por Ambito */
function crearDialogoOrigenPorAmbitoNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IOrigenPorAmbitoNuevo.aspx';
    var params = [];
    if ('IdAmbito' in valores && valores.IdAmbito != undefined) {
        params.push('IdAmbito=' + valores.IdAmbito);
    }
    if ('IdOrigen' in valores && valores.IdOrigen != undefined) {
        params.push('IdOrigen=' + valores.IdOrigen);
    }

    if (params.length != 0) {
        url += "?" + params.join("&");;
    }

    crearDialogoIFrame({
        Titulo: '<label>Asignar Origen</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Origen asignado correctamente');

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoOrigenPorAmbitoDarDeBaja(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Confirmar dar de baja</label>',
        Content: '<div class="margin"><label>¿Esta seguro que desea dar de baja el origen seleccionado?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar',
                OnClick: function () {

                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorError',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/OrigenPorAmbitoService.asmx/DarDeBaja'),
                        Data: { id: id },
                        OnSuccess: function (resultado) {

                            $(jAlert).CerrarDialogo();

                            if (!resultado.Ok) {
                                valores.CallbackMensajes('Error', resultado.Error);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Origen dado de baja');
                            valores.Callback(resultado.Return);
                        },
                        OnError: function (resultado) {
                            $(jAlert).CerrarDialogo();
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    });
}


/* Origen por Usuario */
function crearDialogoOrigenPorUsuarioNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IOrigenPorUsuarioNuevo.aspx';
    var params = [];
    if ('IdUsuario' in valores && valores.IdUsuario != undefined) {
        params.push('IdUsuario=' + valores.IdUsuario);
    }
    if ('IdOrigen' in valores && valores.IdOrigen != undefined) {
        params.push('IdOrigen=' + valores.IdOrigen);
    }

    if (params.length != 0) {
        url += "?" + params.join("&");;
    }

    crearDialogoIFrame({
        Titulo: '<label>Asignar Origen</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Origen asignado correctamente');

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoOrigenPorUsuarioDarDeBaja(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Confirmar dar de baja</label>',
        Content: '<div class="margin"><label>¿Esta seguro que desea dar de baja el origen seleccionado?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar',
                OnClick: function () {

                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorError',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/OrigenPorUsuarioService.asmx/DarDeBaja'),
                        Data: { id: id },
                        OnSuccess: function (resultado) {

                            $(jAlert).CerrarDialogo();

                            if (!resultado.Ok) {
                                valores.CallbackMensajes('Error', resultado.Error);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Origen dado de baja');
                            valores.Callback(resultado.Return);
                        },
                        OnError: function (resultado) {
                            $(jAlert).CerrarDialogo();
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    });
}


/* Notificacion Para Usuario */
function crearDialogoNotificacionParaUsuarioNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/INotificacionParaUsuarioNuevo.aspx';

    crearDialogoIFrame({
        Titulo: '<label>Nueva Notificación para Usuario</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Notificación registrada correctamente');

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
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
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoNotificacionParaUsuarioEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/INotificacionParaUsuarioNuevo.aspx?Id=' + valores.Id;

    crearDialogoIFrame({
        Titulo: '<label>Editar Notificación para Usuario</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Notificación editada correctamente');

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

function crearDialogoNotificacionParaUsuarioDarDeBaja(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = ResolveUrl('~/Servicios/NotificacionParaUsuarioService.asmx/DarDeBaja');
    var data = { id: valores.Id };

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: url,
                Data: data,
                OnSuccess: function (result) {
                    $(jAlert).CerrarDialogo();

                    if (!result.Ok) {
                        valores.CallbackMensajes('Error', result.Error);
                        return;
                    }

                    valores.Callback(result.Return);
                },
                OnError: function (result) {
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                    $(jAlert).CerrarDialogo();
                }
            });
        }
    });

}

function crearDialogoNotificacionParaUsuarioDarDeAlta(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = ResolveUrl('~/Servicios/NotificacionParaUsuarioService.asmx/DarDeAlta');
    var data = { id: valores.Id };

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: url,
                Data: data,
                OnSuccess: function (result) {
                    $(jAlert).CerrarDialogo();

                    if (!result.Ok) {
                        valores.CallbackMensajes('Error', result.Error);
                        return;
                    }

                    valores.Callback(result.Return);
                },
                OnError: function (result) {
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                    $(jAlert).CerrarDialogo();
                }
            });
        }
    });

}

function crearDialogoNotificacionParaUsuarioSetNotificar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = ResolveUrl('~/Servicios/NotificacionParaUsuarioService.asmx/SetNotificar');
    var data = { id: valores.Id, notificar: valores.Notificar };

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: url,
                Data: data,
                OnSuccess: function (result) {
                    $(jAlert).CerrarDialogo();

                    if (!result.Ok) {
                        valores.CallbackMensajes('Error', result.Error);
                        return;
                    }

                    valores.Callback(result.Return);
                },
                OnError: function (result) {
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                    $(jAlert).CerrarDialogo();
                }
            });
        }
    });

}

function crearDialogoNotificacionparaUsuarioDetalle(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    var botones = [];
    var leidas = localStorage.getItem("notificacionesLeidas");
    if (leidas == undefined || leidas == "" || leidas == "undefined") {
        leidas = {};
    } else {
        leidas = JSON.parse(leidas);
    }

    if (leidas[getUsuarioLogeado().Usuario.Id] == undefined) {
        leidas[getUsuarioLogeado().Usuario.Id] = {};
    }
    var leida = leidas[getUsuarioLogeado().Usuario.Id][valores.Id];
    if (leida != undefined && leida == true) {
        botones.push({
            Texto: 'Marcar como no leida',
            OnClick: function () {
                leidas[getUsuarioLogeado().Usuario.Id][valores.Id] = false;
                localStorage.setItem("notificacionesLeidas", JSON.stringify(leidas));
            }
        });
    } else {
        botones.push({
            Texto: 'Marcar como leida',
            OnClick: function () {
                leidas[getUsuarioLogeado().Usuario.Id][valores.Id] = true;
                localStorage.setItem("notificacionesLeidas", JSON.stringify(leidas));
            }
        });
    }
    botones.push({
        Texto: 'Aceptar',
        Class: 'colorExito'
    });

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/INotificacionParaUsuarioDetalle.aspx?Id=' + valores.Id),
        Titulo: '<label>' + valores.Titulo + '</label>',
        CallbackMensajes: function (tipo, mensaje) {
            valores.CallbackMensajes(tipo, mensaje);
        },
        Botones: botones
    });
}


/*¨Favorito */
function crearDialogoRequerimientoFavoritoPorUsuarioMarcar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = ResolveUrl('~/Servicios/RequerimientoFavoritoPorUsuarioService.asmx/MarcarFavorito');
    var comando = {
        IdUser: valores.IdUsuario,
        IdRequerimiento: valores.IdRequerimiento,
        Favorito: valores.Favorito
    };

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: url,
                Data: { comando: comando },
                OnSuccess: function (result) {
                    $(jAlert).CerrarDialogo();

                    if (!result.Ok) {
                        valores.CallbackMensajes('Error', result.Error);
                        return;
                    }



                    valores.Callback(result.Return);
                },
                OnError: function (result) {
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                    $(jAlert).CerrarDialogo();
                }
            });
        }
    });
}

/*Categoria Motivo Area*/
function crearDialogoCategoriaMotivoAreaNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    if (!('IdArea' in valores)) {
        valores.IdArea = 0;
    }

    crearDialogoIFrame({
        Titulo: 'Categoría',
        Url: ResolveUrl('~/IFrame/ICategoriaMotivoAreaNuevo.aspx?IdArea=' + valores.IdArea),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                              {
                                  Texto: 'Aceptar',
                                  CerrarDialogo: true,
                                  Class: 'colorExito',
                                  OnClick: function (jAlert, iFrame, iFrameContent) {
                                      valores.Callback();
                                  }
                              }
            ]
    });
}

function crearDialogoCategoriaAreaEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Editar Categoría</label>',
        Content: '<div class="row margin-top" >' +
                            '<div class="col s12">' +
                                '<div class="input-field">' +
                                    '<input id="inputFormulario_NombreEditar" type="text"/>' +
                                    '<label for="inputFormulario_NombreEditar" class=" no-select textarea">Categoría</label>' +
                                '</div>' +
                            '</div>' +
                        '</div>',
        OnLoad: function (jAlert) {
            $(jAlert).find('#inputFormulario_NombreEditar').trigger('focus');
            $(jAlert).find('#inputFormulario_NombreEditar').val(valores.Valor);
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert) {
                        var obs = $(jAlert).find('#inputFormulario_NombreEditar').val();
                        if (obs == "" || obs == undefined) {
                            mostrarMensaje('Error', 'Debe ingresar la categoría');
                            return;
                        }

                        //Muestro el cargando
                        $(jAlert).MostrarDialogoCargando(true, true);

                        var dataAjax = {
                            Id: valores.Id, Nombre: obs
                        };

                        crearAjax({
                            Data: { comando: dataAjax },
                            Url: ResolveUrl('~/Servicios/CategoriaMotivoAreaService.asmx/Editar'),
                            OnSuccess: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false, true);

                                //Error
                                if (!result.Ok) {
                                    valores.CallbackMensaje('Error', result.Error);
                                    console.log(result);
                                    return;
                                }

                                //Muestro el mensaje de OK
                                mostrarMensaje('Exito', 'Categoría editada correctamente.');

                                //Cierro el dialogo
                                $(jAlert).CerrarDialogo();


                                //Cierro el dialogo
                                //actualizarRowEnGrilla(result.Return);
                                valores.Callback(result.Return);
                            },
                            error: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false, true);

                                //Muestro el Error
                                mostrarMensaje('Error', 'Error editando la categoría');
                                console.log(result);
                            }
                        });
                    }
                }],
        CallbackMensaje: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        }
    });
}

/* Movil*/

function crearDialogoTipoMovilNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    if (!('CallbackTipoMovilNuevo' in valores)) {
        valores.CallbackTipoMovilNuevo = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Tipo Móvil',
        Url: ResolveUrl('~/IFrame/ITipoMovilNuevo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                              {
                                  Texto: 'Aceptar',
                                  CerrarDialogo: true,
                                  Class: 'colorExito',
                                  OnClick: function (jAlert, iFrame, iFrameContent) {
                                      valores.CallbackTipoMovilNuevo(iFrameContent.getTiposMoviles());
                                  }
                              }
            ]
    });
}

function crearDialogoMovilNuevo(valores) {

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    if (!'IdArea' in valores) {
        console.log('ingrese el area');
        return false;
    }

    crearDialogoIFrame({
        Titulo: 'Nuevo Móvil',
        Url: ResolveUrl('~/IFrame/IMovilNuevo2.aspx'),
        Height: '450px',
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback editar
            iFrameContent.setOnRegistrarCompletoListener(function (movil) {
                valores.CallbackMensajes('Exito', 'Móvil registrado correctamente');
                valores.Callback(movil);

                $(jAlert).CerrarDialogo();
            });

            iFrameContent.setArea(valores.IdArea);

        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoMovilEditarInformacionBasica(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Editar información básica',
        Url: ResolveUrl('~/IFrame/IMovilNuevo2.aspx?Id=' + valores.Id),
        Height: 320,
        Width: 800,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback editar
            iFrameContent.setOnEditarCompletoListener(function (movil) {
                valores.CallbackMensajes('Exito', 'Móvil editado correctamente');
                valores.Callback(movil);

                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

function crearDialogoMovilEditarCondicion(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Editar condición',
        Url: ResolveUrl('~/IFrame/IMovilEditarCondicion.aspx?Id=' + valores.Id + "&IdCondicion=" + valores.IdCondicion),
        Height: '100px',
        Width: '350',
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            ////Callback cargando
            //iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
            //    $(jAlert).MostrarDialogoCargando(cargando, true);
            //});

            //Callback editar
            iFrameContent.setOnListener(function () {
                valores.CallbackMensajes('Exito', 'La condición del móvil se ha editada correctamente');
                valores.Callback();

                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.cambiarCondicion();
                    }
                }
            ]
    });
}

function crearDialogoMovilEditarValuacion(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Editar valuación',
        Url: ResolveUrl('~/IFrame/IMovilEditarValuacion.aspx?Id=' + valores.Id),
        Height: '180px',
        Width: '800',
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            ////Callback cargando
            //iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
            //    $(jAlert).MostrarDialogoCargando(cargando, true);
            //});

            //Callback editar
            iFrameContent.setOnListener(function () {
                valores.CallbackMensajes('Exito', 'La valuación del móvil se ha editada correctamente');
                valores.Callback();

                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editarValuacion();
                    }
                }
            ]
    });
}

function crearDialogoMovilEditarKilometraje(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Editar kilometraje',
        Url: ResolveUrl('~/IFrame/IMovilEditarKilometraje.aspx?Id=' + valores.Id),
        Height: '180px',
        Width: '800',
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            ////Callback cargando
            //iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
            //    $(jAlert).MostrarDialogoCargando(cargando, true);
            //});

            //Callback editar
            iFrameContent.setOnListener(function () {
                valores.CallbackMensajes('Exito', 'El kilometraje del móvil se ha editada correctamente');
                valores.Callback();

                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editarKilometraje();
                    }
                }
            ]
    });
}

function crearDialogoMovilEditarITV(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Editar ITV',
        Url: ResolveUrl('~/IFrame/IMovilEditarITV.aspx?Id=' + valores.Id),
        Height: 180,
        Width: 700,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            ////Callback cargando
            //iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
            //    $(jAlert).MostrarDialogoCargando(cargando, true);
            //});

            //Callback editar
            iFrameContent.setOnListener(function () {
                valores.CallbackMensajes('Exito', 'El ITV del móvil se ha editada correctamente');
                valores.Callback();

                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editarITV();
                    }
                }
            ]
    });
}

function crearDialogoMovilEditarTUV(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Editar TUV',
        Url: ResolveUrl('~/IFrame/IMovilEditarTUV.aspx?Id=' + valores.Id),
        Height: '180px',
        Width: '700',
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            ////Callback cargando
            //iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
            //    $(jAlert).MostrarDialogoCargando(cargando, true);
            //});

            //Callback editar
            iFrameContent.setOnListener(function () {
                valores.CallbackMensajes('Exito', 'El TUV del móvil se ha editada correctamente');
                valores.Callback();

                $(jAlert).CerrarDialogo();
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    CerrarDialogo: false,
                    Class: 'colorExito',
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editarTUV();
                    }
                }
            ]
    });
}

function crearDialogoMovilCambiarEstado(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Id Estado Anterior
    if (!('IdEstadoAnterior' in valores)) return false;
    var idEstadoAnterior = valores.IdEstadoAnterior;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    crearDialogoIFrame({
        Titulo: 'Cambiar estado de Móvil',
        Url: ResolveUrl('~/IFrame/IMovilCambiarEstado.aspx?Modo=Movil&Id=' + id + '&IdEstadoAnterior=' + idEstadoAnterior),
        Height: 240,
        Width: 600,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Cambair Estado 
            iFrameContent.setOnEstadoCambiadoListener(function () {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Estado cambiado correctamente');

                //Informo que cambie
                valores.Callback();
            });
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Guardar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.cambiarEstado();
                     }
                 }
            ]
    });
}

function crearDialogoMovilAgregarReparacion(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Agregar reparación',
        Url: ResolveUrl('~/IFrame/IMovilAgregarReparacion.aspx?Id=' + id),
        Height: 240,
        Width: 800,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Cambair Estado 
            iFrameContent.setOnListener(function () {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Reparación agregada correctamente');

                //Informo que cambie
                valores.Callback();
            });
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Aceptar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.agregarReparacion();
                     }
                 }
            ]
    });
}

function crearDialogoMovilDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('Callback' in valores)) valores.Callback = function () { };

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IMovilDetalle.aspx?id=' + id),
        Botones: [
            {
                Texto: 'Aceptar',
                CerrarDialogo: false,
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    crearDialogoEditarMovil({
                        Id: valores.Id,
                        Callback: function (movil) {
                            iFrameContent.cargarMovil(movil);
                        }
                    });
                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function () {
                    console.log('btnaceptar');
                    valores.Callback();
                }
            }
        ]
    })
}

function crearDialogoMovilDetalle2(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('Callback' in valores)) valores.Callback = function () { };

    if (!('CallbackCargando' in valores)) valores.CallbackCargando = function () { };

    crearDialogoIFrame({
        Url: ResolveUrl('~/IFrame/IMovilDetalle2.aspx?id=' + id),
        Botones: [
               {
                   Texto: 'Aceptar',
                   Class: 'colorExito',
                   OnClick: function () {
                       console.log('btnaceptar');
                       valores.Callback();
                   }
               }
        ],
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrame.setOnCargandoListener(function (mostrar, mensaje) {
                $(jAlert).MostrarDialogoCargando(mostrar, mensaje);
            });

            iFrameContent.setOnPanelAbiertoListener(function (abierto) {
                if (abierto) {
                    $(jAlert).DeshabilitarBotonesFooter(abierto);
                    return;
                }

                $(jAlert).HabilitarBotonesFooter(abierto);
            });
        }
    })
}

function crearDialogoAgregarMovil(valores) {
    if (valores == undefined) return false;

    if (!('IdArea' in valores)) return false;
    if (!('IdsMoviles' in valores)) return false;
    if (!('IdOt' in valores)) {
        valores.IdOT = 0;
    };

    crearDialogoIFrame({
        Titulo: 'Selección de Móviles',
        Url: ResolveUrl('~/IFrame/IMovilSelector.aspx?idArea=' + valores.IdArea + '&idOT=' + valores.IdOT),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            iFrameContent.setOnCargandoListener(function (mostrar, mensaje) {
                $(jAlert).MostrarDialogoCargando(mostrar, mensaje);
            });

            iFrameContent.setOnMovilSeleccionadoListener(valores.CallbackSeleccionar);

            iFrameContent.setOnMovilObjetosSeleccionadoListener(valores.CallbackObjetosSeleccionar);

            iFrameContent.setCallbackInit(function () {
                iFrameContent.inicializarPantalla(valores.IdsMoviles);
            });
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Aceptar',
                CerrarDialogo: true,
                Class: 'colorExito',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.informarSeleccion();
                }
            }
        ],
    });
}

function crearDialogoMovilDarDeBaja(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Callback mensajes
    var callbackMensajes = function () { };
    if ('CallbackMensajes' in valores) {
        callbackMensajes = valores.CallbackMensajes;
    }

    //Callback editado
    var callback = function () { };
    if ('Callback' in valores) {
        callback = valores.Callback;
    }

    crearDialogoHTML({
        Titulo: '<label>Dar de baja móvil</label>',
        Content:
            '<div class="padding">' +
            '<label id="textoConfirmacion"class="titulo">¿Está seguro de dar de baja el móvil?</label>' +
            '</div>',
        Botones:
            [
                {
                    Id: 'btnNo',
                    Texto: 'No'
                },
                {
                    Id: 'btnSi',
                    Texto: 'Si',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert) {

                        //Muestro el cargando
                        // mostrarCargando(true);
                        $(jAlert).MostrarDialogoCargando(true);

                        var dataAjax = { Id: id };

                        var urlAjax = ResolveUrl('~/Servicios/MovilService.asmx/DarDeBaja');


                        crearAjax({
                            Data: { comando: dataAjax },
                            Url: urlAjax,
                            OnSuccess: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false);

                                //Cierro el dialogo
                                $(jAlert).CerrarDialogo();

                                //Error
                                if (!result.Ok) {
                                    callbackMensajes('Error', result.Error);
                                    return;
                                }

                                //Muestro el mensaje de OK
                                callbackMensajes('Exito', 'Móvil dado de baja correctamente');
                                callback(result.Return);

                            },
                            OnError: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false);
                                //Cierro el dialogo
                                $(jAlert).CerrarDialogo();

                                //Muestro el Error
                                callbackMensajes('Error', 'Error eliminando la sección');
                            }
                        });
                    }
                }]
    });

}

function crearDialogoMovilDarDeAlta(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var comando = { Id: id };
    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/MovilService.asmx/DarDeAlta'),
                Data: { comando: comando },
                OnSuccess: function (resultado) {

                    $(jAlert).CerrarDialogo();

                    if (!resultado.Ok) {
                        valores.CallbackMensajes('Error', resultado.Error);
                        return;
                    }

                    valores.CallbackMensajes('Exito', 'Móvil dada de alta');
                    valores.Callback(resultado.Return);
                },
                OnError: function (resultado) {
                    $(jAlert).CerrarDialogo();
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}

/* Categorias Motivo */
function crearDialogoGrupoRubroMotivoNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    if (!('CallbackGrupoNuevo' in valores)) {
        valores.CallbackGrupoNuevo = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Grupo Nuevo',
        Url: ResolveUrl('~/IFrame/IGrupoRubroMotivoNuevo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                              {
                                  Texto: 'Aceptar',
                                  CerrarDialogo: true,
                                  Class: 'colorExito',
                                  OnClick: function (jAlert, iFrame, iFrameContent) {
                                      valores.CallbackGrupoNuevo(iFrameContent.getGrupos());
                                  }
                              }
            ]
    });
}

function crearDialogoRubroMotivoNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IRubroMotivoNuevo.aspx?IdGrupo=' + valores.IdGrupo;

    crearDialogoIFrame({
        Titulo: '<label>Nueva Categoría</label>' + valores.NombreGrupo,
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.mostrarCargando(true);

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Ancho: 0.95,
        Alto: 0.95,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Guardar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoRubroMotivoEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IRubroMotivoNuevo.aspx?Id=' + valores.Id;

    crearDialogoIFrame({
        Titulo: '<label>Editar Categoría</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Ancho: 0.95,
        Alto: 0.95,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

/* Dialogos de Flota */
function crearDialogoFlotaNueva(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IFlotaNueva.aspx?IdArea=' + valores.IdArea;

    crearDialogoIFrame({
        Titulo: '<label>Nueva Flota</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Ancho: 0.95,
        Alto: 0.95,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Registrar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoFlotaEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IFlotaNueva.aspx?IdArea=' + valores.IdArea + '&Id=' + valores.Id;

    crearDialogoIFrame({
        Titulo: '<label>Editar Flota</label>' + valores.NombreArea,
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Ancho: 0.95,
        Alto: 0.95,
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

function crearDialogoFlotaCambiarEstado(valores) {
    if (valores == undefined) return false;

    //Id
    if (!('Id' in valores)) return false;
    var id = valores.Id;

    //Id Estado Anterior
    if (!('IdEstadoAnterior' in valores)) return false;
    var idEstadoAnterior = valores.IdEstadoAnterior;

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    crearDialogoIFrame({
        Titulo: 'Cambiar estado de Móvil',
        Url: ResolveUrl('~/IFrame/IMovilCambiarEstado.aspx?Modo=Flota&Id=' + id + '&IdEstadoAnterior=' + idEstadoAnterior),
        Height: 240,
        Width: 600,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            })

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callbar Cambair Estado 
            iFrameContent.setOnEstadoCambiadoListener(function () {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo el mensaje
                valores.CallbackMensajes('Exito', 'Estado cambiado correctamente');

                //Informo que cambie
                valores.Callback();
            });
        },
        Botones:
            [
                 {
                     Texto: 'Cancelar'
                 },
                 {
                     Texto: 'Aceptar',
                     CerrarDialogo: false,
                     Class: 'colorExito',
                     OnClick: function (jAlert, iFrame, iFrameContent) {
                         iFrameContent.cambiarEstado();
                     }
                 }
            ]
    });
}

/* Archivos */
function crearDialogoUrlImagenes(valores) {
    if (valores == undefined) valores = {};

    /*
    Imagenes: [
        {
            Url: url de la imagen,
            Nombre: nombre del archivo de imagen
        }
    ]
    */

    if (!'Imagenes' in valores || valores.Imagenes == undefined) {
        console.log('Debe mandar las imagenes');
        return;
    }
    if (!'Index' in valores || valores.Index == undefined) {
        valores.Index = 0;
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/Iframe/IVisorArchivo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setUrlImagenes(valores.Imagenes, valores.Index);
        },
        Botones: [
            {
                Texto: 'Descargar',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.descargar();
                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}


/* Contacto */
function crearDialogoContacto(valores) {
    if (valores == undefined) valores = {};

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Url: ResolveUrl('~/Iframe/IContacto.aspx'),
        Titulo: '<label>Contacto</label>',
        Height: 350,
        Width: 500,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function () {
                $(jAlert).CerrarDialogo();

                valores.Callback();
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
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
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.enviarMail();
                }
            }
        ]
    });
}


/* Informacion Organica Secretarias */
function crearDialogoInformacionOrganicaSecretariaNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IInformacionOrganicaSecretariaNuevo.aspx';

    crearDialogoIFrame({
        Titulo: '<label>Nueva Secretaría</label>',
        Url: ResolveUrl(url),
        Height: 100,
        Width: 600,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
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
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoInformacionOrganicaSecretariaEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IInformacionOrganicaSecretariaNuevo.aspx?Id=' + valores.Id;

    crearDialogoIFrame({
        Titulo: '<label>Nueva Secretaría</label>',
        Url: ResolveUrl(url),
        Height: 100,
        Width: 600,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

function crearDialogoInformacionOrganicaSecretariaDarDeBaja(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Confirmar dar de baja</label>',
        Content: '<div class="margin"><label>¿Esta seguro que desea dar de baja la secretaria seleccionada?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar',
                OnClick: function () {

                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorError',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/DarDeBaja'),
                        Data: { id: id },
                        OnSuccess: function (resultado) {

                            $(jAlert).CerrarDialogo();

                            if (!resultado.Ok) {
                                valores.CallbackMensajes('Error', resultado.Error);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Secretaria dada de baja');
                            valores.Callback(resultado.Return);
                        },
                        OnError: function (resultado) {
                            $(jAlert).CerrarDialogo();
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    });
}

function crearDialogoInformacionOrganicaSecretariaDarDeAlta(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/DarDeAlta'),
                Data: { id: id },
                OnSuccess: function (resultado) {

                    $(jAlert).CerrarDialogo();

                    if (!resultado.Ok) {
                        valores.CallbackMensajes('Error', resultado.Error);
                        return;
                    }

                    valores.CallbackMensajes('Exito', 'Secretaria dada de alta');
                    valores.Callback(resultado.Return);
                },
                OnError: function (resultado) {
                    $(jAlert).CerrarDialogo();
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}

function crearDialogoInformacionOrganicaSecretariaDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Detalle de Secretaria',
        Url: ResolveUrl('~/IFrame/IInformacionOrganicaSecretariaDetalle.aspx?id=' + id),
        Width: 400,
        Height: 500,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function () {
                    valores.Callback();
                }
            }
        ]
    })
}


/* Zonas y subzonas */

function crearDialogoZona(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IZonaNueva.aspx?IdArea=' + valores.IdArea;

    let editar = false;
    if ('Id' in valores) {
        editar = true;
        url += "&Id=" + valores.Id;
    }

    crearDialogoIFrame({
        Titulo: '<label>Zona nueva</label>',
        Url: ResolveUrl(url),
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, mensaje);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            return false;
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: editar ? 'Aceptar' : 'Registrar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        if (editar) {
                            iFrameContent.editar();
                        } else {
                            iFrameContent.registrar();
                        }
                    }
                }
            ]
    });
}


/* Informacion Organica Direcciones */
function crearDialogoInformacionOrganicaDireccionNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    var url = '~/IFrame/IInformacionOrganicaDireccionNuevo.aspx';

    if ('IdSecretaria' in valores && valores.IdSecretaria != undefined) {
        url += '?IdSecretaria=' + valores.IdSecretaria;
    }

    crearDialogoIFrame({
        Titulo: '<label>Nueva Dirección</label>',
        Url: ResolveUrl(url),
        Height: 400,
        Width: 600,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
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
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoInformacionOrganicaDireccionEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IInformacionOrganicaDireccionNuevo.aspx?Id=' + valores.Id;

    crearDialogoIFrame({
        Titulo: '<label>Editar Dirección</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}

function crearDialogoInformacionOrganicaDireccionDarDeBaja(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Confirmar dar de baja</label>',
        Content: '<div class="margin"><label>¿Esta seguro que desea dar de baja la dirección seleccionada?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar',
                OnClick: function () {

                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorError',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/DarDeBaja'),
                        Data: { id: id },
                        OnSuccess: function (resultado) {

                            $(jAlert).CerrarDialogo();

                            if (!resultado.Ok) {
                                valores.CallbackMensajes('Error', resultado.Error);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Dirección dada de baja');
                            valores.Callback(resultado.Return);
                        },
                        OnError: function (resultado) {
                            $(jAlert).CerrarDialogo();
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    });
}

function crearDialogoInformacionOrganicaDireccionDarDeAlta(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoCargando({
        OnLoad: function (jAlert) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/DarDeAlta'),
                Data: { id: id },
                OnSuccess: function (resultado) {

                    $(jAlert).CerrarDialogo();

                    if (!resultado.Ok) {
                        valores.CallbackMensajes('Error', resultado.Error);
                        return;
                    }

                    valores.CallbackMensajes('Exito', 'Dirección dada de alta');
                    valores.Callback(resultado.Return);
                },
                OnError: function (resultado) {
                    $(jAlert).CerrarDialogo();
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                }
            });
        }
    });
}

function crearDialogoInformacionOrganicaDireccionDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Detalle de Dirección',
        Url: ResolveUrl('~/IFrame/IInformacionOrganicaDireccionDetalle.aspx?id=' + id),
        Width: 400,
        Height: 500,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function () {
                    valores.Callback();
                }
            }
        ]
    })
}


/* Informacion Organica */

function crearDialogoInformacionOrganicaNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    var url = '~/IFrame/IInformacionOrganicaNuevo.aspx?IdArea=' + valores.Id;

    crearDialogoIFrame({
        Titulo: '<label>Asignar informacion orgánica</label>',
        Url: ResolveUrl(url),
        Height: 100,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (origen) {
                //Cierro si es que el usuario asi lo quiere
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(origen);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
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
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoInformacionOrganicaDarDeBaja(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoHTML({
        Titulo: '<label>Confirmar dar de baja</label>',
        Content: '<div class="margin"><label>¿Esta seguro que desea dar de baja la información orgánica del área?</label></div>',
        Botones: [
            {
                Texto: 'Cancelar',
                OnClick: function () {

                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorError',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    $(jAlert).MostrarDialogoCargando(true);

                    crearAjax({
                        Url: ResolveUrl('~/Servicios/InformacionOrganicaService.asmx/DarDeBaja'),
                        Data: { id: id },
                        OnSuccess: function (resultado) {

                            $(jAlert).CerrarDialogo();

                            if (!resultado.Ok) {
                                valores.CallbackMensajes('Error', resultado.Error);
                                return;
                            }

                            valores.CallbackMensajes('Exito', 'Información orgánica dada de baja');
                            valores.Callback(resultado.Return);
                        },
                        OnError: function (resultado) {
                            $(jAlert).CerrarDialogo();
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            }
        ]
    });
}

function crearDialogoInformacionOrganicaDetalle(valores) {
    if (valores == undefined) return false;

    if (!('Id' in valores)) return false;
    var id = valores.Id;

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: 'Información orgánica de Área',
        Url: ResolveUrl('~/IFrame/IInformacionOrganicaDetalle.aspx?id=' + id),
        Width: 400,
        Height: 500,
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito',
                OnClick: function () {
                    valores.Callback();
                }
            }
        ]
    })
}


/* Catalogo */

//function crearDialogoReporteCatalogoMotivos(servicio) {
//    if (servicio == undefined) return false;

//    crearDialogoIFrame({
//        Titulo: '<label>Motivos de ' + toTitleCase(servicio) + '</label>',
//        Url: ResolveUrl('~/Resources/Catalogo/' + servicio + ' MOTIVOS.pdf'),
//        Ancho: 0.95,
//        Alto: 0.95,
//        Botones: [
//            {
//                Texto: 'Aceptar',
//                Class: 'colorExito'
//            }
//        ]
//    })
//}

//function crearDialogoReporteCatalogoUsuarios(servicio) {
//    if (servicio == undefined) return false;

//    crearDialogoIFrame({
//        Titulo: '<label>Usuarios de ' + toTitleCase(servicio) + '</label>',
//        Url: ResolveUrl('~/Resources/Catalogo/' + servicio + ' USUARIOS.pdf'),
//        Ancho: 0.95,
//        Alto: 0.95,
//        Botones: [
//            {
//                Texto: 'Aceptar',
//                Class: 'colorExito'
//            }
//        ]
//    })
//}

function crearDialogoCatalogo() {
    crearDialogoIFrame({
        Titulo: '<label>Catálogo</label>',
        Height: 300,
        Width: 500,
        Url: ResolveUrl('~/IFrame/ICatalogo.aspx'),
        Botones:
            [
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito'
                }
            ]
    });
}

/*V2 Catalogos*/
function crearDialogoReporteCatalogoUsuarios(valores) {
    if (valores == undefined) return false;

    if (!('TipoCatalogo' in valores)) return false;
    var tipoCatalogo = valores.TipoCatalogo;

    if (!('IdArea' in valores)) return false;
    var idArea = valores.IdArea;

    let url = ResolveUrl('~/IFrame/IReporte.aspx');
    console.log(url);

    crearDialogoIFrame({
        Url: url,
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteCatalogoUsuarios(tipoCatalogo, idArea);
            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function crearDialogoReporteCatalogoMotivos(valores) {
    if (valores == undefined) return false;

    if (!('TipoCatalogo' in valores)) return false;
    var tipoCatalogo = valores.TipoCatalogo;

    if (!('IdArea' in valores)) return false;
    var idArea = valores.IdArea;

    let url = ResolveUrl('~/IFrame/IReporte.aspx');
    console.log(url);

    crearDialogoIFrame({
        Url: url,
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteCatalogoMotivos(tipoCatalogo, idArea);
            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}
function crearDialogoReporteCatalogoTareas(valores) {
    if (valores == undefined) return false;

    if (!('TipoCatalogo' in valores)) return false;
    var tipoCatalogo = valores.TipoCatalogo;

    if (!('IdArea' in valores)) return false;
    var idArea = valores.IdArea;

    let url = ResolveUrl('~/IFrame/IReporte.aspx');
    console.log(url);

    crearDialogoIFrame({
        Url: url,
        Ancho: 0.95,
        Alto: 0.95,
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            iFrameContent.GenerarReporteCatalogoTareas(tipoCatalogo, idArea);
            return false;
        },
        Botones: [
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

/* Version */

function crearDialogoVersionSistema(valores) {
    if (valores == undefined) valores = {};

    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Titulo: '<label>Versión del sistema</label>',
        Height: 300,
        Width: 500,
        Url: ResolveUrl('~/IFrame/IVersionSistema.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setListener(function (entity) {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback(entity);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            //Callback cargando
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
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
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}


/* Motivo */
function crearDialogoMotivoNuevo(valores) {
    if (valores == undefined) valores = {};

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    let url = '~/IFrame/IMotivoNuevo.aspx';
    let parametros = '';
    let primero = true;

    if (valores.IdArea != undefined) {
        if (!primero) {
            parametros += '&';
        }

        parametros += 'IdArea=' + valores.IdArea;
        primero = false;
    }

    if (valores.IdServicio != undefined) {
        if (!primero) {
            parametros += '&';
        }

        parametros += 'IdServicio=' + valores.IdServicio;
        primero = false;
    }

    if (valores.IdCategoria != undefined) {
        if (!primero) {
            parametros += '&';
        }

        parametros += 'IdCategoria=' + valores.IdCategoria;
        primero = false;
    }

    if (!primero) {
        url += '?' + parametros;
    }

    crearDialogoIFrame({
        Alto: 0.95,
        Ancho: 0.95,
        Titulo: '<label>Motivo nuevo</label>',
        Url: ResolveUrl(url),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function (cerrar) {
                if (cerrar) {
                    $(jAlert).CerrarDialogo();
                }

                //Informo
                valores.Callback();
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
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
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoMotivoEditar(valores) {
    if (valores == undefined) valores = {};

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Alto: 0.95,
        Ancho: 0.95,
        Titulo: '<label>Editar motivo</label>',
        Url: ResolveUrl('~/IFrame/IMotivoNuevo.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function () {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback();
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}


/*Campo Por Motivo*/

function crearDialogoCampoPorMotivoNuevo(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    if (!('IdMotivo' in valores)) {
        valores.IdMotivo = '-1';
    }

    if (!('NombreMotivo' in valores)) {
        valores.NombreMotivo = 'Motivo';
    }

    crearDialogoIFrame({
        Titulo: 'Campo para ' + valores.NombreMotivo,
        Url: ResolveUrl('~/IFrame/ICampoPorMotivoNuevo.aspx?IdMotivo=' + valores.IdMotivo),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Callback
            iFrameContent.setCallback(function (tipo, mensaje) {
                $(jAlert).CerrarDialogo();
                valores.Callback(tipo, mensaje);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        ScrollY: true,
        Height: '22rem',
        Botones:
            [{
                Texto: 'Cancelar',
                CerrarDialogo: true
            },
                              {
                                  Texto: 'Aceptar',
                                  CerrarDialogo: false,
                                  Class: 'colorExito',
                                  OnClick: function (jAlert, iFrame, iFrameContent) {
                                      iFrameContent.insertar();
                                  }
                              }
            ]
    });
}

function crearDialogoCampoPorMotivoEditar(valores) {
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }


    if (!('Id' in valores)) {
        valores.Id = '-1';
    }

    crearDialogoIFrame({
        Titulo: 'Campo para motivo',
        Url: ResolveUrl('~/IFrame/ICampoPorMotivoNuevo.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            //Callback de mensajes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            });

            //Callback
            iFrameContent.setCallback(function (data) {
                $(jAlert).CerrarDialogo();
                valores.Callback(data);
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [{
                Texto: 'Cancelar',
                CerrarDialogo: true
            },
                              {
                                  Texto: 'Aceptar',
                                  CerrarDialogo: false,
                                  Class: 'colorExito',
                                  OnClick: function (jAlert, iFrame, iFrameContent) {
                                      iFrameContent.editar();
                                  }
                              }
            ]
    });
}


/* Servicio */
function crearDialogoServicioNuevo(valores) {
    if (valores == undefined) valores = {};

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Width: 400,
        Height: 500,
        Titulo: '<label>Servicio nuevo</label>',
        Url: ResolveUrl('~/IFrame/IServicioNuevo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function () {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback();
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
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
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.registrar();
                    }
                }
            ]
    });
}

function crearDialogoServicioEditar(valores) {
    if (valores == undefined) valores = {};

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    crearDialogoIFrame({
        Width: 400,
        Height: 500,
        Titulo: '<label>Editar servicio</label>',
        Url: ResolveUrl('~/IFrame/IServicioNuevo.aspx?Id=' + valores.Id),
        OnLoad: function (jAlert, iFrame, iFrameContent) {

            //Callback de mensajes
            iFrameContent.setCallback(function () {
                $(jAlert).CerrarDialogo();

                //Informo
                valores.Callback();
            });

            //Callback cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert, iFrame, iFrameContent) {
                        iFrameContent.editar();
                    }
                }
            ]
    });
}
