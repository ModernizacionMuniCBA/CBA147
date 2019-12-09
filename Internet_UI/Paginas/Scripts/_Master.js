let geoApiInfo;
let dataDialogos = {};
let usuarioLogeado;
let token;

function initMaster(data) {
    LoadingPage.init();

    initMasterUsuario(data.Usuario);
    initMasterFotoPersonal(data.Usuario.IdentificadorFotoPersonal);
    token = data.Token;

    $('#header > .content > .logo').click(function () {
        redirigir('Inicio');
    });


    $('[type=password]').after("<a class='password-see waves-effect'><i class='mdi mdi-eye'></i></a>");
    $('.password-see').click(function () {
        let input = $(this).prev();

        if (input.attr('type') === 'password') {
            input.attr('type', 'text');
            $(this).find('i').addClass('mdi-eye-off');
            $(this).find('i').removeClass('mdi-eye');
        } else {
            input.attr('type', 'password');
            $(this).find('i').addClass('mdi-eye');
            $(this).find('i').removeClass('mdi-eye-off');
        }
    });

    initMasterApps();
    initMensajeExterno();
}

function cargarAppsMuniDigital() {
    //ordeno las apps
    _.sortBy(apps, 'nombre');

    //le saco el numero a cada una y creo el html
    _.each(apps, function (a) {
        var nombre = a.nombre.split('.');
        a.nombre = nombre[1].trim()
        a.id = nombre[0].trim();
        var html = crearHtmlApp(a);
        $("#template_AppsMuniOnline").append(html);
    })

    return $($("#contenedor_template_AppsMuniOnline").html());
}

function crearHtmlApp(app) {
    var div = $($("#template_App").html());

    $(div).find('.iconoApp').css('background-image', 'url(' + app.urlIcono + ')');

    $(div).attr('id', app.id);

    $(div).find('.nombreApp').text(app.nombre);

    return div;
}

function redirigir(url, mensaje, animar) {
    if (animar === undefined || animar === true) {
        redirigirAnimar();
    }


    let contenidoMensaje = mensaje !== undefined ? ('#mensaje=' + mensaje.texto + '&tipoMensaje=' + mensaje.tipo) : '';
    let pagina = ResolveUrl('~/' + url) + contenidoMensaje;

    window.location.href = pagina;
    if (location.href.indexOf(url) != -1) {
        window.location.reload();
    }
}

function redirigirAnimar() {
    $('#content').removeClass('visible');
    $('#texto_Titulo').removeClass('visible');
}

function initMasterUsuario(data) {
    usuarioLogeado = data;

    console.log(data);
    $('.contenedor_Usuario').click(function (e) {
        $('.contenedor_Usuario').MenuFlotante({
            PosicionX: 'izquierda',
            PosicionY: 'abajo',
            Width: 300,
            Menu: [
                {
                    Icono: 'perm_contact_calendar',
                    Texto: 'Mi perfil',
                    OnClick: function () {
                        window.location.href = 'https://servicios2.cordoba.gov.ar/MuniOnlinePerfil/#/?token=' + token;
                    }
                },
                {
                    Icono: 'settings',
                    Texto: 'Ajustes',
                    OnClick: function () {
                        redirigir('Ajustes');
                    }
                },
                {
                    Separador: true
                },
                {
                    Icono: 'exit_to_app',
                    Texto: 'Cerrar sesion',
                    OnClick: function () {
                        redirigir('CerrarSesion');
                    }
                }]
        });
    });

}

function initMasterFotoPersonal(identificador) {
    $('.contenedor_Usuario').css('background-image', 'Url(' + urlCordobaFiles + '/Archivo/' + identificador + ')');
}

var apps = [];
function initMasterApps() {
    crearAjax({
        Url: ResolveUrl('~/Servicios/ServicioMuniOnline.asmx/GetAppsMuniOnline'),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error cargando las aplicaciones de Muni Online');
                return;
            }

            apps = result.Return;
            var html = cargarAppsMuniDigital();
            $('.contenedor_BotonApps').click(function (e) {
                $('.contenedor_BotonApps').MenuFlotante({
                    Width: 300,
                    PosicionX: 'izquierda',
                    PosicionY: 'abajo',
                    //MarginTop: $('.contenedor_BotonApps').height() + 13,
                    Child: html,
                    OnReady: function (menu) {
                        _.each(apps, function (app) {

                            $(menu).find("[id='" + app.id + "']").click(function () {
                                var urlFinal = app.url;
                                if (app.urlToken) {
                                    urlFinal = app.urlToken.replace('{token}', token);
                                }
                                window.location.href = urlFinal;
                            })
                        })
                    }

                });
            });
        },
        OnError: function () { }
    })
}

function initMensajeExterno() {
    setTimeout(function () {

        let url = location.href;
        let partes = url.split('#');
        if (partes.length > 1 && url.indexOf('mensaje=') != -1) {
            let mensaje = '';
            let tipo = '';
            let parametros = partes[1].split('&');
            for (let i = 0; i < parametros.length; i++) {
                if (parametros[i].indexOf('mensaje=') != -1) {
                    mensaje = parametros[i].split('=')[1];
                }
                if (parametros[i].indexOf('tipoMensaje=') != -1) {
                    tipo = parametros[i].split('=')[1];
                }
            }

            mensaje = decodeURIComponent(mensaje);
            $('#mensaje').addClass('visible');
            $('#mensaje').addClass(tipo);
            $('#mensaje > label').text(mensaje);
        } else {
            $('#mensaje').removeClass('visible');
        }
        location.hash = "";
    }, 500);

    $('#mensaje > a').click(function () {
        $('#mensaje').removeClass('visible');
    });
}


// Dialogo
function crearDialogo(valores) {
    if (valores === undefined) valores = {};

    let html = $($('#template_Dialogo').html());
    $('#content').append(html);

    let id;
    if (valores.Id !== undefined) {
        id = valores.Id
    } else {
        id = 'dialogo_' + new Date().getTime();
    }
    $(html).prop('id', id);

    dataDialogos[id] = valores;

    //Fullscreen
    if (valores.Fullscreen === false) {
        $(html).addClass('not-fullscreen');
    }

    //Class
    if (valores.Class !== undefined) {
        $(html).addClass(valores.Class);
    }

    //Titulo
    if (valores.Titulo !== undefined) {
        $(html).find('.header .texto_Titulo').text(valores.Titulo);
    } else {
        $(html).find('.header').hide();
    }

    //Content
    if (valores.Html !== undefined) {
        $(html).find('.content > .content').html(valores.Html);
    }

    //Botones
    if (valores.Botones !== undefined) {
        $.each(valores.Botones, function (index, boton) {
            let clase = boton.Class || '';
            let autoCerrar = boton.AutoCerrar != false;

            let htmlBoton = $('<a class="btn btn-transparent waves-effect ' + clase + '">' + boton.Texto + '</a>');
            $(htmlBoton).click(function () {
                if (boton.OnClick != undefined) {
                    boton.OnClick(html, id);
                }

                if (autoCerrar == true) {
                    cerrarDialogo(id);
                }
            });

            $(html).find('.botones').append(htmlBoton);
        });
    } else {
        $(html).addClass('not-footer');
    }

    //Cerrar
    if (valores.Cerrable === undefined) {
        valores.Cerrable = true;
    }

    if (valores.Cerrable) {
        $(html).find('.btn-close, .fondo').click(function () {
            cerrarDialogo(id);
        });
    } else {
        $(html).find('.btn-close').hide();
    }

    //Espero para mostrar
    setTimeout(function () {
        $('.dialogo-flotante.visible > .fondo.visible').removeClass('visible');

        $(html).addClass('visible');

        // Autoheight
        if (valores.Autoheight === true) {
            let header = $('#' + id + ' > .content > .header')[0].scrollHeight;
            let content = $('#' + id + ' > .content > .content')[0].scrollHeight + 30;
            let botones = $('#' + id + ' > .content > .botones')[0].scrollHeight;

            $('#' + id + ' > .content').css('height', (header + content + botones) + 'px');
        }

        // Autocentrar
        Autocenter.init(document.querySelector('#' + id + ' > .content'));
        if (valores.OnShow != undefined) {
            valores.OnShow(html);
        }

        // Mover
        if (valores.Movible !== false) {
            $('#' + id + ' .header')[0].addEventListener('mousedown', function (event) {
                var content = document.querySelector('#' + id + ' > .content');
                var head = document.querySelector('#' + id + ' .header');

                HandleMove.init(event, content, head);
            });
        }
    }, 50);
}

function cerrarDialogo(id) {
    let valores = dataDialogos[id];

    $('#' + id).removeClass('visible');
    $('.dialogo-flotante.visible > .fondo:not(.visible)').addClass('visible');

    setTimeout(function () {
        $('#' + id).remove();
    }, 500);

    if (valores != undefined) {
        if (valores.OnHide != undefined) {
            valores.OnHide($('#' + id));
        }
    }
}


// CordobaGeoApi
function getGeoApiInfo() {
    return new Promise(function (callback, callbackError) {
        if (geoApiInfo !== undefined) {
            callback(geoApiInfo);
            return;
        }

        $.ajax({
            url: 'https://servicios2.cordoba.gov.ar/CordobaGeoApi/info/general?conPoligono=true',
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


// Utils
function isTest() {
    let url = document.URL;

    return url.indexOf('localhost') !== -1 || url.indexOf('127.0.0.1') !== -1 || url.indexOf('srv-dev') !== -1;
}

function getDate(date) {
    try {
        if (date == undefined) return '';
        dateSplit = date.split('-');
        return dateSplit[2].substring(0, 2) + '/' + dateSplit[1] + '/' + dateSplit[0];
    } catch (e) {
        return '';
    }
}

function mostrarCargando(mostrar) {
    if (mostrar) {
        $('#contenedor_Cargando').addClass('visible');
    } else {
        $('#contenedor_Cargando').removeClass('visible');
    }
}

function mostrarMensaje(mensaje) {
    crearDialogo({
        Class: 'dialogo-mensaje',
        Autoheight: true,
        Html: $('<label>' + mensaje + '</label>'),
        Botones: [
            {
                Texto: 'Aceptar'
            }
        ]
    });
}

function mostrarError(id, text) {
    $('#' + id + '').addClass('visible');
    $('#' + id + ' label').text(text);
}

function ocultarError(id, text) {
    $('#' + id + '').removeClass('visible');
    $('#' + id + ' label').text('');
}

function verificarError(data) {
    if (data != undefined && data.Error != undefined) {
        mostrarErrorCritico(data.Error, {
            Texto: 'Reintentar',
            OnClick: function () {
                location.reload();
            }
        });
        return true;
    }

    $('#content').addClass('visible');
    $('#texto_Titulo').addClass('visible');

    return false;
}

function mostrarErrorCritico(mensaje, boton) {
    $('#contenedor_Error').addClass('visible');
    $('#contenedor_Error label').text(mensaje || 'Error procesando la solicitud');

    if (boton != undefined) {
        $('#contenedor_Error .boton').show();
        $('#contenedor_Error .boton').text(boton.Texto);

        $('#contenedor_Error .boton').click(function () {
            boton.OnClick();
        });
    } else {
        $('#contenedor_Error .boton').hide();
    }
}

function achicarImagen(data, w) {
    return new Promise(function (callback, callbackError) {
        let canvas = $('<canvas>').get(0);
        let ctx = canvas.getContext("2d");

        let image = new Image();
        image.setAttribute('crossOrigin', 'anonymous');

        image.onload = function () {
            ctx.drawImage(image, 0, 0);

            let width = image.width;
            let height = image.height;

            if (width > height) {
                if (width > w) {
                    height *= w / width;
                    width = w;
                }
            } else {
                if (height > w) {
                    width *= w / height;
                    height = w;
                }
            }
            canvas.width = width;
            canvas.height = height;
            ctx.drawImage(image, 0, 0, width, height);

            callback(canvas.toDataURL("image/png", 0.7));
        };

        image.src = data;
    });
}

let Autocenter = {
    init: function (element) {
        Autocenter.centrar(element);
        window.addEventListener('resize', function () {
            Autocenter.centrar(element);
        });
    },

    centrar: function (element) {
        var top = (document.documentElement.offsetHeight - element.offsetHeight) / 2;
        if (top > 0) {
            element.style.top = top + 'px';
        } else {
            element.style.top = '';
        }

        var left = (document.documentElement.offsetWidth - element.offsetWidth) / 2;
        if (left > 0) {
            element.style.left = left + 'px';
        } else {
            element.style.left = '';
        }
    },
}

let HandleMove = {
    elementToMove: undefined,
    elementToClick: undefined,

    minLimitX: undefined,
    minLimitY: undefined,
    maxLimitX: undefined,
    maxLimitY: undefined,

    startX: undefined,
    startY: undefined,

    init: function (event, elementToMove, elementToClick) {
        HandleMove.elementToMove = elementToMove;
        HandleMove.elementToClick = elementToClick;

        HandleMove.minLimitX = 0;
        HandleMove.maxLimitX = document.documentElement.offsetWidth - HandleMove.elementToMove.offsetWidth;

        HandleMove.minLimitY = 0;
        HandleMove.maxLimitY = document.documentElement.offsetHeight - HandleMove.elementToMove.offsetHeight;

        HandleMove.startX = event.clientX - HandleMove.elementToMove.offsetLeft;
        HandleMove.startY = event.clientY - HandleMove.elementToMove.offsetTop;

        HandleMove.elementToClick.style.userSelect = 'none';
        document.body.style.userSelect = 'none';


        window.addEventListener('mousemove', HandleMove.move);
        window.addEventListener('mouseup', HandleMove.dest);
    },

    move: function (event) {
        var x = event.clientX - HandleMove.startX;
        var y = event.clientY - HandleMove.startY;

        if (x < HandleMove.minLimitX) {
            x = HandleMove.minLimitX;
        }
        if (x > HandleMove.maxLimitX) {
            x = HandleMove.maxLimitX;
        }

        if (y < HandleMove.minLimitY) {
            y = HandleMove.minLimitY;
        }
        if (y > HandleMove.maxLimitY) {
            y = HandleMove.maxLimitY;
        }

        HandleMove.elementToMove.style.left = x + 'px';
        HandleMove.elementToMove.style.top = y + 'px';
    },

    dest: function () {
        HandleMove.elementToClick.style.userSelect = '';
        document.body.style.userSelect = '';

        HandleMove.elementToMove = undefined;
        HandleMove.elementToClick = undefined;

        HandleMove.minLimitX = undefined;
        HandleMove.minLimitY = undefined;
        HandleMove.maxLimitX = undefined;
        HandleMove.maxLimitY = undefined;

        HandleMove.startX = undefined;
        HandleMove.startY = undefined;

        window.removeEventListener('mousemove', HandleMove.move);
        window.removeEventListener('mouseup', HandleMove.dest);
    }
};

let LoadingPage = {
    init: function () {
        window.addEventListener("load", function (event) {
            LoadingPage.off();
        });

        window.addEventListener("beforeunload", function (event) {
            LoadingPage.on();
        });
    },

    on: function () {
        $("#header > .progress").addClass('visible');
    },

    off: function () {
        $("#header > .progress").removeClass('visible');
    }
};