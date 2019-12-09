var callbackGuardado;
var requerimientos;

function init(_idsRequerimientos) {

    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/Init'),
        Data: { idsRequerimientos: _idsRequerimientos },
        OnSuccess: function (result) {

            mostrarCargando(false);

            if (result == undefined) {
                mostrarMensaje('Error', 'Error procesando la solicitud');
                return;
            }

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            requerimientos = result.Return.Requerimientos;

            if (requerimientos == undefined) {
                mostrarMensaje("Error", "Error inicializando");
                return;
            }

            inicializar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function inicializar() {
    $.each(requerimientos, function (index, element) {
        var div = $('<div class="card card-requerimiento">');
        div.attr('id-rq', element.Id);

        var textos = $('<div class="textos">');
        $(textos).appendTo(div);

        var textoNumero = $('<label class="numero">').text(element.Numero);
        $(textoNumero).appendTo(textos);

        var textoMotivo = $('<label class="motivo">').text(element.MotivoString);
        $(textoMotivo).appendTo(textos);

        var btn = $('<a class="btn-flat waves-effect btn-redondo"><i class="material-icons">more_vert</i></a>');
        $(btn).appendTo(div);

        $(btn).click(function () {
            $(btn).MenuFlotante({
                Menu: [
                    {
                        Texto: 'Detalle',
                        Icono: 'description',
                        OnClick: function () {
                            crearDialogoDetalleRequerimiento({
                                Id: element.Id,
                                CallbackMensajes: function (tipo, mensaje) {
                                    mostrarMensaje(tipo, mensaje);
                                }
                            });
                        }
                    },
                    {
                        Texto: 'Quitar',
                        Icono: 'clear',
                        OnClick: function () {
                            if (requerimientos.length == 1) {
                                mostrarMensaje('Alerta', 'La orden de inspección debe tener al menos un requerimiento');
                                return;
                            }
                            quitarRequerimiento(element.Id);
                        }
                    }
                ]
            });

        });

        $('#contenedor-requerimientos').append(div);
    });
}

function quitarRequerimiento(id) {
    requerimientos = $.grep(requerimientos, function (element, index) {
        return element.Id != id;
    });

    $('.card-requerimiento[id-rq=' + id + ']').hide(300, function () {
        $('.card-requerimiento[id-rq=' + id + ']').remove();
    });
}

function getOrdenInspeccion() {
    var ot = {};

    //Requerimientos
    ot.IdRequerimientos = [];
    $.each(requerimientos, function (index, element) {
        ot.IdRequerimientos.push(element.Id);
    });

    //Descripcion
    ot.Descripcion = $('#inputFormulario_Descripcion').val();

    //UserAgent
    ot.UserAgent = navigator.userAgent;
    ot.IsMobile = isMobile();
    ot.IsMobileOrTablet = isMobileOrTablet();
    return ot;
}


//----------------------------
// Listener Guardar
//----------------------------

function guardar() {
    mostrarCargando(true);

    crearAjax({
        Data: { comando: getOrdenInspeccion() },
        Url: ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/Insert'),
        OnSuccess: function (result) {
            //algo salio mal
            //if (!result.Ok) {
            //    mostrarCargando(false);
            //    //Informo
            //    mostrarMensaje('Error', result.Error.Publico);
            //    return;
            //}

            //callbackGuardado(result.Return);

            //algo salio mal
            if (!result.Ok) {
                mostrarCargando(false);
                //Informo
                mostrarMensaje('Error', result.Error);
                return;
            }

            callbackGuardado(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            //Informo
            mostrarMensaje('Error', 'Error registrando la orden de inspección');
        }
    });
}

function informarGuardado(entity) {
    if (callbackGuardado == undefined) return;
    callbackGuardado(entity);
}

function setOnGuardadoListener(callback) {
    this.callbackGuardado = callback;
}
