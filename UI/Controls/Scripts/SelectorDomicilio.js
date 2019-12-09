var SelectorDomicilio_ModoCalle = "calle";
var SelectorDomicilio_ModoBarrio = "barrio";
var SelectorDomicilio_ModoOtro = "otro";

var SelectorDomicilio_Modo = SelectorDomicilio_ModoCalle;
var SelectorDomicilio_Domicilio;

var SelectorDomicilio_Callback;
var SelectorDomicilio_CallbackModo;

var SelectorDomicilio_CallbackMensaje;
var SelectorDomicilio_CallbackCargando;

$(function () {

    //Switch por barrio

    $('#SelectorDomicilio_Icono_PorCalle').css('color', 'var(--colorVerde)');
    $('#SelectorDomicilio_Icono_PorBarrio').css('color', 'rgba(0,0,0,0.4)');

    $('#SelectorDomicilio_Switch_PorBarrio').find('.opcion1').click(function () {
        $('#SelectorDomicilio_Switch_PorBarrio').find('input').prop('checked', false);
        $('#SelectorDomicilio_Switch_PorBarrio').trigger('change');
    });


    $('input:radio[name=SelectorDomicilio_group1]').click(function () {
        if ($('#SelectorDomicilio_radio_Calle').is(':checked')) {
            SelectorDomicilio_Modo = SelectorDomicilio_ModoCalle;
        } else {
            if ($('#SelectorDomicilio_radio_Barrio').is(':checked')) {
                SelectorDomicilio_Modo = SelectorDomicilio_ModoBarrio;

            } else {
                SelectorDomicilio_Modo = SelectorDomicilio_ModoOtro;
            }
        }

        SelectorDomicilio_CambiarModo();
        SelectorDomicilio_InformarModo();
    });

    //Boton buscar domicilio
    $('#SelectorDomicilio_BtnBuscar').click(function () {

        if ($(this).is(':disabled')) {
            return;
        }

        if (!SelectorDomicilio_Validar()) {
            return;
        }


        var porCalle = SelectorDomicilio_Modo == SelectorDomicilio_ModoCalle;

        if (porCalle) {

            //Bloqueo cargando
            SelectorDomicilio_MostrarCargando(true, 'Buscando calles...');

            //Genero el Ajax
            var data = { nombre: $('#SelectorDomicilio_Input_Calle').val().trim() };
            data = JSON.stringify(data);

            crearAjax({
                Url: ResolveUrl('~/Servicios/CalleService.asmx/BuscarCalleEnCatastro'),
                Data: { nombre: $('#SelectorDomicilio_Input_Calle').val().trim() },
                OnSuccess: function (result) {

                    //Desbloqueo cargando
                    SelectorDomicilio_MostrarCargando(false);

                    if (!result.Ok) {
                        SelectorDomicilio_MostrarMensajeError(result.Error);
                        return;
                    }

                    //Valido que halla calles
                    if (result.Return.length == 0) {
                        SelectorDomicilio_MostrarMensajeAlerta('No se encontró ninguna calle');
                        $('#SelectorDomicilio_Input_Calle').trigger('focus');
                        return;
                    }

                    //Si hay una sola calle, cargo el domicilio
                    if (result.Return.length == 1) {
                        var altura = $('#SelectorDomicilio_Input_Altura').val();
                        SelectorDomicilio_BuscarPorCalleAltura(result.Return[0].IdCatastro, altura);
                        return;
                    }

                    //Si hay mas de una, muestro el dialogo para que el usuario seleccione una
                    crearDialogoIFrame({
                        Titulo: 'Seleccione una calle',
                        Url: ResolveUrl('~/IFrame/CalleCatastroSelector.aspx'),
                        OnLoad: function (jAlert, iFrame, iFrameContent) {
                            iFrameContent.setCalles(result.Return);
                            iFrameContent.setCallback(function (calle) {
                                var altura = $('#SelectorDomicilio_Input_Altura').val();
                                SelectorDomicilio_BuscarPorCalleAltura(calle.IdCatastro, altura);

                                $(jAlert).CerrarDialogo();
                            });
                        },
                        Botones:
                            [
                                {
                                    Texto: 'Volver',
                                }
                            ]
                    });
                },
                OnError: function (result) {
                    //Desbloqueo cargando
                    SelectorDomicilio_MostrarCargando(false);

                    //Informo error
                    SelectorDomicilio_MostrarMensajeError("Error procesando la solicitud");
                }
            });
        } else {
            //Muestro el cargando
            SelectorDomicilio_MostrarCargando(true, 'Buscando barrios...');

            //Genero el Ajax
            var data = { nombre: $('#SelectorDomicilio_Input_Barrio').val().trim() };
            data = JSON.stringify(data);

            crearAjax({
                Url: ResolveUrl('~/Servicios/BarrioService.asmx/BuscarBarrioEnCatastro'),
                Data: { nombre: $('#SelectorDomicilio_Input_Barrio').val().trim() },
                OnSuccess: function (result) {
                    //Desbloqueo cargando
                    SelectorDomicilio_MostrarCargando(false);

                    if (!result.Ok) {
                        SelectorDomicilio_MostrarMensajeError(result.Error);
                        return;
                    }

                    //Valido que halla barrios
                    if (result.Return.length == 0) {


                        SelectorDomicilio_MostrarMensajeAlerta('No se encontró ningun barrio');
                        $('#SelectorDomicilio_Input_Barrio').trigger('focus');
                        return;
                    }

                    //Si hay un solo barrio, lo cargo
                    if (result.Return.length == 1) {
                        SelectorDomicilio_BuscarPorBarrio(result.Return[0].IdCatastro);
                        return;
                    }

                    crearDialogoIFrame({
                        Titulo: 'Seleccione un barrio',
                        Url: ResolveUrl('~/IFrame/BarrioCatastroSelector.aspx'),
                        OnLoad: function (jAlert, iFrame, iFrameContent) {
                            iFrameContent.setBarrios(result.Return);
                            iFrameContent.setCallback(function (barrio) {
                                $(jAlert).CerrarDialogo();
                                SelectorDomicilio_BuscarPorBarrio(barrio.IdCatastro);
                            });

                        },
                        Botones:
                            [
                                {
                                    Texto: 'Volver',
                                }
                            ]
                    });

                },
                OnError: function (result) {
                    //Desbloqueo cargando
                    SelectorDomicilio_MostrarCargando(false);

                    //Informo error
                    SelectorDomicilio_MostrarMensajeError("Error procesando la solicitud");
                }
            });
        }

    });

    //Enter en calle
    $('#SelectorDomicilio_Input_Calle').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#SelectorDomicilio_BtnBuscar').click();
        }
    })

    //Enter en numero
    $('#SelectorDomicilio_Input_Altura').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#SelectorDomicilio_BtnBuscar').click();
        }
    })

    //Enter en barrio
    $('#SelectorDomicilio_Input_Barrio').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#SelectorDomicilio_BtnBuscar').click();
        }
    })

    //----------------------------------
    // Botones
    //----------------------------------

    //Btn Ver Mapa
    $('#SelectorDomicilio_BtnVerMapa').click(function () {
        if (SelectorDomicilio_Domicilio == undefined) return;

        var x = SelectorDomicilio_Domicilio.Xcatastro.replace(',', '.');
        var y = SelectorDomicilio_Domicilio.Ycatastro.replace(',', '.');        

        crearDialogoIFrame({
            Url: url,
            Ancho: 0.95,
            Alto: 0.95,
            Botones:
                [
                    
                        Texto: 'Aceptar',
                        Class: 'colorExito',
                    }
                ]
        });
    });

    //Btn Cancelar domicilio
    $('#SelectorDomicilio_BtnCancelarDomicilio').click(function () {

        //Cancelo el domicilio
        SelectorDomicilio_Domicilio = undefined;

        //Infodmo 
        if (SelectorDomicilio_Callback != undefined) {
            SelectorDomicilio_Callback(SelectorDomicilio_Domicilio);
        }

        //Oculto el domicilio seleccionado
        SelectorDomicilio_OcultarDomicilioSeleccionado();
    });
});

function SelectorDomicilio_SetModo(modo) {
    switch (modo) {
        case SelectorDomicilio_ModoCalle:
            $('#SelectorDomicilio_radio_Calle').trigger('click');
            break;
        case SelectorDomicilio_ModoBarrio:
            $('#SelectorDomicilio_radio_Barrio').trigger('click');
            break;
        case SelectorDomicilio_ModoOtro:
            $('#SelectorDomicilio_radio_Otro').trigger('click');
            break;
    }

    SelectorDomicilio_Modo = modo;
    SelectorDomicilio_CambiarModo();
}

function SelectorDomicilio_CambiarModo() {
    switch (SelectorDomicilio_Modo) {
        case SelectorDomicilio_ModoCalle:
            $('#SelectorDomicilio_ContenedorPorCalle').show();
            $('#SelectorDomicilio_ContenedorBarrio').hide();
            $('#SelectorDomicilio_ContenedorOtro').hide();
            $('#SelectorDomicilio_ContenedorBotones').show(300);

            $('#SelectorDomicilio_Input_Calle').trigger('focus');

            break;

        case SelectorDomicilio_ModoBarrio:
            $('#SelectorDomicilio_ContenedorPorCalle').hide();
            $('#SelectorDomicilio_ContenedorBarrio').show();
            $('#SelectorDomicilio_ContenedorOtro').hide();
            $('#SelectorDomicilio_ContenedorBotones').show(300);

            $('#SelectorDomicilio_Input_Barrio').trigger('focus');
            break;

        case SelectorDomicilio_ModoOtro:
            $('#SelectorDomicilio_ContenedorPorCalle').hide();
            $('#SelectorDomicilio_ContenedorBarrio').hide();
            $('#SelectorDomicilio_ContenedorOtro').show();
            $('#SelectorDomicilio_ContenedorBotones').hide(300);

            $('#SelectorDomicilio_Input_Otro').trigger('focus');
            break;
    }
}

function SelectorDomicilio_BorrarValidaciones() {
    $('#SelectorDomicilio_Input_Calle').siblings('.control-observacion').text('');
    $('#SelectorDomicilio_Input_Calle').siblings('.control-observacion').stop(true, true).slideUp(300);

    $('#SelectorDomicilio_Input_Altura').siblings('.control-observacion').text('');
    $('#SelectorDomicilio_Input_Altura').siblings('.control-observacion').stop(true, true).slideUp(300);

    $('#SelectorDomicilio_Input_Barrio').siblings('.control-observacion').text('');
    $('#SelectorDomicilio_Input_Barrio').siblings('.control-observacion').stop(true, true).slideUp(300);

    $('#SelectorDomicilio_Input_Otro').siblings('.control-observacion').text('');
    $('#SelectorDomicilio_Input_Otro').siblings('.control-observacion').stop(true, true).slideUp(300);
}

function SelectorDomicilio_Validar() {
    //Borro las validaciones
    SelectorDomicilio_BorrarValidaciones();

    var validar = true;

    switch (SelectorDomicilio_Modo) {
        case SelectorDomicilio_ModoCalle:
            //Calle
            var calle = $('#SelectorDomicilio_Input_Calle').val();
            if (calle == undefined || calle == "") {
                $('#SelectorDomicilio_Input_Calle').siblings('.control-observacion').text('Dato requerido');
                $('#SelectorDomicilio_Input_Calle').siblings('.control-observacion').stop(true, true).slideDown(300);
                validar = false;
            }

            //Altura
            var altura = $('#SelectorDomicilio_Input_Altura').val();
            if (altura == undefined || altura == "") {
                $('#SelectorDomicilio_Input_Altura').siblings('.control-observacion').text('Dato requerido');
                $('#SelectorDomicilio_Input_Altura').siblings('.control-observacion').stop(true, true).slideDown(300);
                validar = false;
            }

            break;
        case SelectorDomicilio_ModoBarrio:

            //Barrio
            var barrio = $('#SelectorDomicilio_Input_Barrio').val();
            if (barrio == undefined || barrio == "") {
                $('#SelectorDomicilio_Input_Barrio').siblings('.control-observacion').text('Dato requerido');
                $('#SelectorDomicilio_Input_Barrio').siblings('.control-observacion').stop(true, true).slideDown(300);
                validar = false;
            }
            break;
        case SelectorDomicilio_ModoOtro:
            var descripcion = $('#SelectorDomicilio_Input_Otro').val().trim;
            if (descripcion == undefined || descripcion == "") {
                $('#SelectorDomicilio_Input_Otro').siblings('.control-observacion').text('Dato requerido');
                $('#SelectorDomicilio_Input_Otro').siblings('.control-observacion').stop(true, true).slideDown(300);
                validar = false;
            }
            break;
    }

    return validar;
}


function SelectorDomicilio_BuscarPorCalleAltura(idCalle, altura) {

    //Muestro cargando
    SelectorDomicilio_MostrarCargando(true, 'Obteniendo domicilio...');

    var comando = {
        IdCalleCatastro: idCalle,
        Altura: altura
    };

    crearAjax({
        Url: ResolveUrl('~/Servicios/DomicilioService.asmx/BuscarDomicilioEnCatastro'),
        Data: { comando: comando },
        OnSuccess: function (result) {
            //Oculto el cargando
            SelectorDomicilio_MostrarCargando(false);

            if (!result.Ok) {
                SelectorDomicilio_MostrarMensajeError("Error procesando la solicitud");
                return;
            }

            if (result.Return == null) {
                SelectorDomicilio_MostrarMensajeError("La altura no es correcta para la calle ingresada");
                return;
            }

            //Guardo el domicilio
            SelectorDomicilio_Domicilio = result.Return;
            SelectorDomicilio_Domicilio.PorBarrio = false;

            //Muestro el domicilio
            SelectorDomicilio_MostrarDomicilioSeleccionado();

            //Informo el domicilio
            if (SelectorDomicilio_Callback != undefined) {
                setTimeout(function () {
                    SelectorDomicilio_Callback(SelectorDomicilio_Domicilio);
                }, 500);
            }
        },
        OnError: function (result) {
            SelectorDomicilio_MostrarCargando(false);
            SelectorDomicilio_MostrarMensajeError("Error procesando la solicitud");
        }
    });
}

function SelectorDomicilio_BuscarPorBarrio(idBarrio) {

    //Muestro cargando
    SelectorDomicilio_MostrarCargando(true, 'Obteniendo domicilio...');

    var comando = {
        IdBarrioCatastro: idBarrio,
        PorBarrio: true
    };

    crearAjax({
        Url: ResolveUrl('~/Servicios/DomicilioService.asmx/BuscarDomicilioEnCatastro'),
        Data: { comando: comando },
        OnSuccess: function (result) {
            //Oculto el cargando
            SelectorDomicilio_MostrarCargando(false);

            if (!result.Ok) {
                SelectorDomicilio_MostrarMensajeError("Error procesando la solicitud");
                return;
            }

            if (result.Return == null) {
                SelectorDomicilio_MostrarMensajeError("La altura no es correcta para la calle ingresada");
                return;
            }

            //Guardo el domicilio
            SelectorDomicilio_Domicilio = result.Return;
            SelectorDomicilio_Domicilio.PorBarrio = true;

            //Muestro el domicilio
            SelectorDomicilio_MostrarDomicilioSeleccionado();

            //Informo el domicilio
            if (SelectorDomicilio_Callback != undefined) {
                setTimeout(function () {
                    SelectorDomicilio_Callback(SelectorDomicilio_Domicilio);
                }, 500);
            }
        },
        OnError: function (result) {
            SelectorDomicilio_MostrarCargando(false);
            SelectorDomicilio_MostrarMensajeError("Error procesando la solicitud");
        }
    });
}

function SelectorDomicilio_MostrarDomicilioSeleccionado() {
    if (SelectorDomicilio_Modo == SelectorDomicilio_ModoOtro) {
        $('#SelectorDomicilio_Input_Otro').val(SelectorDomicilio_Domicilio);
        $('#SelectorDomicilio_Input_Otro').trigger('focus');
        Materialize.updateTextFields();
    } else {
        $('#SelectorDomicilio_ContenedorBusqueda').fadeOut(300, function () {

            if (!SelectorDomicilio_Domicilio.PorBarrio) {
                //Por calle
                $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').find('.titulo').text(toTitleCase(SelectorDomicilio_Domicilio.Calle.Nombre) + ' ' + SelectorDomicilio_Domicilio.Altura);
                $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').find('.barrio').text('Barrio: ' + toTitleCase(SelectorDomicilio_Domicilio.Barrio.Nombre));
                $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').find('.barrio').show(300);
                $('#SelectorDomicilio_ContenedorDescripcion').hide(300);
            } else {
                //Por Barrio
                $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').find('.titulo').text('Barrio: ' + toTitleCase(SelectorDomicilio_Domicilio.Barrio.Nombre));
                $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').find('.barrio').hide(300);
                $('#SelectorDomicilio_ContenedorDescripcion').show(300);
            }

            $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').find('.cpc').text('CPC: ' + SelectorDomicilio_Domicilio.Cpc.Numero + ' ' + toTitleCase(SelectorDomicilio_Domicilio.Cpc.Nombre));
            $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').find('.cpc').show(300);

            if (SelectorDomicilio_Domicilio.Observaciones != undefined && SelectorDomicilio_Domicilio.Observaciones != null) {
                $('#SelectorDomicilio_Input_Observaciones').val(SelectorDomicilio_Domicilio.Observaciones);
                Materialize.updateTextFields();
            }

            $('#SelectorDomicilio_ContenedorObservaciones').show();
            $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').fadeIn(300);

            //Enfoco en el la descripcion del domicilio
            $('#SelectorDomicilio_Input_Observaciones').trigger('focus');
        });
    }
}

function SelectorDomicilio_OcultarDomicilioSeleccionado() {
    $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').fadeOut(300, function () {
        $('#SelectorDomicilio_Input_Calle').val('');
        $('#SelectorDomicilio_Input_Altura').val('');
        $('#SelectorDomicilio_Input_Barrio').val('');
        $('#SelectorDomicilio_Input_Otro').val('');
        $('#SelectorDomicilio_Input_Observaciones').val('');
        $('#SelectorDomicilio_Input_Observaciones').trigger('autoresize');
        Materialize.updateTextFields();

        $('#SelectorDomicilio_ContenedorObservaciones').hide();
        $('#SelectorDomicilio_ContenedorBusqueda').fadeIn(300);

        switch (SelectorDomicilio_Modo) {
            case SelectorDomicilio_ModoCalle:
                $('#SelectorDomicilio_Input_Calle').trigger('focus');
                break;
            case SelectorDomicilio_ModoBarrio:
                $('#SelectorDomicilio_Input_Barrio').trigger('focus');
                break;
            case SelectorDomicilio_ModoOtro:
                $('#SelectorDomicilio_Input_Otro').trigger('focus');
                break;

        }
    });
}

function SelectorDomicilio_GetDomicilioSeleccionado() {
    var domicilio = undefined;

    switch (SelectorDomicilio_Modo) {
        case SelectorDomicilio_ModoCalle:
            domicilio = SelectorDomicilio_Domicilio;
            if (domicilio != undefined && domicilio != null) {
                domicilio.PorBarrio = false;
                domicilio.Observaciones = $('#SelectorDomicilio_Input_Observaciones').val().trim();
            }
            break;
        case SelectorDomicilio_ModoBarrio:
            {
                domicilio = SelectorDomicilio_Domicilio;
                if (domicilio != undefined && domicilio != null) {
                    domicilio.PorBarrio = true;
                    domicilio.Observaciones = $('#SelectorDomicilio_Input_Observaciones').val().trim();
                }
            }
            break;

        case SelectorDomicilio_ModoOtro:
            {
                domicilio = $('#SelectorDomicilio_Input_Otro').val().trim();
            }
            break;
    }

    return domicilio;
}

function SelectorDomicilio_IsDomicilioSeleccionado() {
    return SelectorDomicilio_GetDomicilioSeleccionado() != undefined && SelectorDomicilio_GetDomicilioSeleccionado() != null && SelectorDomicilio_GetDomicilioSeleccionado() != "";
}

function SelectorDomicilio_IsDatosIngresadosSinDomicilioSeleccionado() {
    if (SelectorDomicilio_Modo == SelectorDomicilio_ModoOtro) {
        return false;
    }

    var porBarrio = SelectorDomicilio_Modo == SelectorDomicilio_ModoBarrio;
    var a = !SelectorDomicilio_IsDomicilioSeleccionado();
    var b = porBarrio ? $('#SelectorDomicilio_Input_Barrio').val().trim() != "" : ($('#SelectorDomicilio_Input_Calle').val().trim() || $('#SelectorDomicilio_Input_Altura').val().trim());
    return a && b;
}

function SelectorDomicilio_SetDomicilio(domicilio) {

    SelectorDomicilio_Domicilio = domicilio;

    if (typeof domicilio == "string") {
        SelectorDomicilio_Modo = SelectorDomicilio_ModoOtro;
        $('#SelectorDomicilio_radio_Otro').attr('checked', 'checked');
    } else {

        if (domicilio.PorBarrio) {
            SelectorDomicilio_Modo = SelectorDomicilio_ModoBarrio;
            $('#SelectorDomicilio_radio_Barrio').attr('checked', 'checked');
        } else {
            SelectorDomicilio_Modo = SelectorDomicilio_ModoCalle;
            $('#SelectorDomicilio_radio_Calle').attr('checked', 'checked');
        }

        if (domicilio.Observaciones != null && domicilio.Observaciones != "") {
            $('#SelectorDomicilio_Input_Observaciones').val(domicilio.Observaciones);
        } else {
            $('#SelectorDomicilio_Input_Observaciones').val("");
        }

        Materialize.updateTextFields();
    }

    SelectorDomicilio_CambiarModo();
    SelectorDomicilio_MostrarDomicilioSeleccionado();

    SelectorDomicilio_InformarModo();
    SelectorDomicilio_InformarDomicilio();
}

function SelectorDomicilio_ReiniciarUI() {
    SelectorDomicilio_PorBarrio = false;
    SelectorDomicilio_Domicilio = undefined;

    SelectorDomicilio_BorrarValidaciones();

    $('#SelectorDomicilio_Input_Calle').val('');
    $('#SelectorDomicilio_Input_Altura').val('');
    $('#SelectorDomicilio_Input_Barrio').val('');
    $('#SelectorDomicilio_Input_Otro').val('');
    $('#SelectorDomicilio_Input_Observaciones').val('');
    $('#SelectorDomicilio_Input_Observaciones').trigger('autoresize');

    $('#SelectorDomicilio_ContenedorCalle').show();
    $('#SelectorDomicilio_ContenedorAltura').show();
    $('#SelectorDomicilio_ContenedorBarrio').hide();
    $('#SelectorDomicilio_BtnDomicilioCatastro').hide();
    $('#SelectorDomicilio_BtnDomicilioManual').show();
    $('#SelectorDomicilio_ContenedorBusqueda').show();
    $('#SelectorDomicilio_ContenedorDomicilioSeleccionada').hide();

    $('#SelectorDomicilio_ContenedorObservaciones').hide();

    Materialize.updateTextFields();

}

//--------------------------------
// Domicilio Seleccionado
//--------------------------------

function SelectorDomicilio_InformarDomicilio() {
    if (SelectorDomicilio_Callback == undefined || SelectorDomicilio_Callback == null) return;
    SelectorDomicilio_Callback(SelectorDomicilio_Modo, SelectorDomicilio_GetDomicilioSeleccionado());
}

function SelectorDomicilio_SetOnDomicilioSeleccionadoListener(callback) {
    SelectorDomicilio_Callback = callback;
}

//-------------------------------
// Modo Cambiado 
//--------------------------------

function SelectorDomicilio_InformarModo() {
    if (SelectorDomicilio_CallbackModo == undefined || SelectorDomicilio_CallbackModo == null) return;
    SelectorDomicilio_CallbackModo(SelectorDomicilio_Modo);
}

function SelectorDomicilio_SetOnModoCambiadoListener(callback) {
    SelectorDomicilio_CallbackModo = callback;
}

//-------------------------------
// Cargando
//-------------------------------

function SelectorDomicilio_MostrarCargando(mostrar, mensaje) {
    $('#SelectorDomicilio_Input_Calle').prop('disabled', mostrar);
    $('#SelectorDomicilio_Input_Altura').prop('disabled', mostrar);
    $('#SelectorDomicilio_Input_Barrio').prop('disabled', mostrar);
    $('#SelectorDomicilio_BtnBuscar').prop('disabled', mostrar);

    if (SelectorDomicilio_CallbackCargando != undefined) {
        SelectorDomicilio_CallbackCargando(mostrar, mensaje);
    }
}

function SelectorDomicilio_SetOnCargandoListener(callback) {
    this.SelectorDomicilio_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function SelectorDomicilio_SetOnMensajeListener(callback) {
    this.SelectorDomicilio_CallbackMensaje = callback;

}

function SelectorDomicilio_MostrarMensajeError(mensaje) {
    if (SelectorDomicilio_CallbackMensaje == undefined) return;
    SelectorDomicilio_CallbackMensaje('Error', mensaje);
}

function SelectorDomicilio_MostrarMensajeAlerta(mensaje) {
    if (SelectorDomicilio_CallbackMensaje == undefined) return;
    SelectorDomicilio_CallbackMensaje('Alerta', mensaje);
}

function SelectorDomicilio_MostrarMensajeInfo(mensaje) {
    if (SelectorDomicilio_CallbackMensaje == undefined) return;
    SelectorDomicilio_CallbackMensaje('Info', mensaje);
}

function SelectorDomicilio_MostrarMensajeExito(mensaje) {
    if (SelectorDomicilio_CallbackMensaje == undefined) return;
    SelectorDomicilio_CallbackMensaje('Exito', mensaje);
}