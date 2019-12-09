var SelectorPersona_Persona;

var SelectorPersona_Callback;
var SelectorPersona_CallbackMensaje;
var SelectorPersona_CallbackCargando;
var SelectorPersona_AbrirNuevaPersonaSiNoEncuentro;

var nroDocRecomendado;
var nombreRecomendado;
var apellidoRecomendado;

function initSelectorPersona(data) {
    data = parse(data);

    SelectorPersona_InitSelectTipoPersona();

    //Tipo de busqueda

    var tipos = [
        {
            Id: 1,
            Nombre: "Por N°"
        },
        {
            Id: 2,
            Nombre: "Por Nombre"
        }
    ];

    $('#SelectorPersona_Select_Tipo').CargarSelect({
        Data: tipos,
        Value: 'Id',
        Text: 'Nombre',
        TitleCase: false,
        Sort:false
    });

    //Al cambiar el tipo
    $('#SelectorPersona_Select_Tipo').on('change', function () {
        var tipo = $('#SelectorPersona_Select_Tipo').val();

        if (tipo == 1) {
            //Por Numero
            $('#SelectorPersona_ContenedorNombre').stop(true, true).fadeOut(300, function () {
                $('#SelectorPersona_ContenedorNumero').stop(true, true).fadeIn(300);

                $('#SelectorPersona_Input_NumeroDocumento').trigger('focus');
            });
        }
        else {
            //Por Nombre
            $('#SelectorPersona_ContenedorNumero').stop(true, true).fadeOut(300, function () {
                $('#SelectorPersona_ContenedorNombre').stop(true, true).fadeIn(300);

                $('#SelectorPersona_Input_Nombre').trigger('focus');
            });
        }
    });

    //Enter en el numero de documento de la persona (dispara buscar)
    $('#SelectorPersona_Input_NumeroDocumento').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#SelectorPersona_BtnBuscar').click();
        }
    })

    $('#SelectorPersona_Input_Nombre').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#SelectorPersona_BtnBuscar').click();
        }
    })

    $('#SelectorPersona_Input_Apellido').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#SelectorPersona_BtnBuscar').click();
        }
    })


    //Boton buscar persona
    $('#SelectorPersona_BtnBuscar').click(function () {
        if ($(this).is(':disabled')) {
            return;
        }

        if (!SelectorPersona_Validar()) {
            return;
        }

        //Busco la persona
        var esPersonaFisica = true;

        var url;
        var dataAjax;
        var filtros;

        if (esPersonaFisica) {
            SelectorPersona_MostrarCargando(true, 'Buscando persona física...');

            url = ResolveUrl('~/Servicios/PersonaFisicaService.asmx/GetByFilters');

            var tipo = $('#SelectorPersona_Select_Tipo').val();
            if (tipo == 1) {
                filtros = {
                    NumeroDocumento: $('#SelectorPersona_Input_NumeroDocumento').val()
                }
            } else {
                filtros = {
                    Nombre: $('#SelectorPersona_Input_Nombre').val(),
                    Apellido: $('#SelectorPersona_Input_Apellido').val()
                }
            }
        } else {
            SelectorPersona_MostrarCargando(true, 'Buscando persona jurídica...');
            url = ResolveUrl('~/Servicios/PersonaJuridicaService.asmx/GetPersona');
            filtros = {
                NumeroDocumento: nroDocumento
            }
        }
        dataAjax = { filtros: JSON.stringify(filtros) };
        dataAjax = JSON.stringify(dataAjax);

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            url: url,
            data: dataAjax,
            success: onSuccess,
            error: onError
        });


        function onSuccess(result) {
            result = parse(result.d);

            //Oculto el cargando
            SelectorPersona_MostrarCargando(false);

            //Error
            if ('Error' in result) {
                SelectorPersona_MostrarMensajeError(result.Error.Publico);
                console.log('Error consultando la persona');
                console.log(dataAjax);
                console.log(result);
                return;
            }

            if (result.Personas == null || result.Personas.length == 0) {
                SelectorPersona_MostrarMensajeAlerta('Persona no encontrada');
                SelectorPersona_MostrarNuevaPersona(filtros);
                return;
            }

            //Si hay una sola persona, la selecciono
            if (result.Personas.length == 1) {
                //Selecciono la persona
                SelectorPersona_Persona = result.Personas[0];
                SelectorPersona_Persona.EsPersonaFisica = esPersonaFisica;

                SelectorPersona_CargarPersonaSeleccionada(SelectorPersona_Persona);
                return;
            }

            //Sino permito elegir
            crearDialogoIFrame({
                Titulo: 'Seleccionar Persona Física',
                Url: ResolveUrl('~/IFrame/IPersonaFisicaSelector.aspx'),
                OnLoad: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.setPersonas(result.Personas);

                    //Callback
                    iFrameContent.setOnPersonaSeleccionadaListener(function (persona) {
                        if (persona == null || persona == undefined) return;

                        //Cierro el dialogo
                        $(jAlert).CerrarDialogo();

                        //Selecciono la persona
                        SelectorPersona_Persona = persona;
                        SelectorPersona_Persona.EsPersonaFisica = esPersonaFisica;

                        SelectorPersona_CargarPersonaSeleccionada(SelectorPersona_Persona);
                    });
                },
                Botones: [
                    {
                        Texto: 'Cancelar'
                    }
                ]
            });
        }

        function onError(result) {

            //Oculto el cargando
            SelectorPersona_MostrarCargando(false);

            //Muestro el error
            SelectorPersona_MostrarMensajeError('Error consultando la persona');
            console.log('Error consultando la persona');
            console.log(dataAjax);
            console.log(result);
        }

    });

    //Boton Cancelar Persona
    $('#SelectorPersona_BtnCancelarPersona').click(function () {
        SelectorPersona_CancelarPersonaSeleccionada();
    });

    //Boton Detalle Persona
    $('#SelectorPersona_BtnDetallePersona').click(function () {
        if (SelectorPersona_Persona == undefined) {
            SelectorPersona_MostrarMensajeError('No seleccionó ninguna persona');
            return;
        }

        if (SelectorPersona_Persona.EsPersonaFisica) {
            crearDialogoIFrame({
                Titulo: 'Detalle de Persona Fisica',
                Url: ResolveUrl('~/IFrame/IPersonaFisicaDetalle.aspx?Id=' + SelectorPersona_Persona.Id),
                Botones:
                    [
                        {
                            Texto: 'Aceptar',
                            Class: 'colorExito'
                        }
                    ]
            });
        } else {
            crearDialogoIFrame({
                Titulo: 'Detalle de Persona Jurídica',
                Url: ResolveUrl('~/IFrame/IPersonaJuridicaDetalle.aspx?Id=' + SelectorPersona_Persona.Id),
                Botones:
                    [
                        {
                            Texto: 'Aceptar',
                            Class: 'colorExito'
                        }
                    ]
            });
        }
    });

    //Nueva Persona Fisica
    $('#SelectorPersona_BtnNuevaPersonaFisica').click(function () {
        var url = '~/IFrame/IPersonaFisicaNuevo.aspx';
        var queryString = "";
        if (nroDocRecomendado != undefined) {
            if (queryString != "") {
                queryString += '&';
            }
            queryString += 'nroDoc=' + nroDocRecomendado;
        }

        if (nombreRecomendado != undefined) {
            if (queryString != "") {
                queryString += '&';
            }
            queryString += 'nombre=' + nombreRecomendado;
        }

        if (apellidoRecomendado != undefined) {
            if (queryString != "") {
                queryString += '&';
            }
            queryString += 'apellido=' + apellidoRecomendado;
        }

        if (queryString != "") {
            url += '?' + queryString;
        }
        crearDialogoIFrame({
            Titulo: 'Nueva Persona Física',
            Url: ResolveUrl(url),
            OnLoad: function (jAlert, iFrame, iFrameContent) {
                //Callback de persona guardada
                iFrameContent.setOnRegistrarCompletoListener(function (persona) {
                    //Cargo la persona
                    persona.EsPersonaFisica = true;
                    SelectorPersona_CargarPersonaSeleccionada(persona);

                    //Cierro el modal
                    $(jAlert).CerrarDialogo();
                });

                //Callback de mensajes
                iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                    if (SelectorPersona_CallbackMensaje != undefined) {
                        SelectorPersona_CallbackMensaje(tipo, mensaje);
                    }
                })

                //Callback cargando
                iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                    (jAlert).MostrarDialogoCargando(cargando, true);
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
    });
}

function SelectorPersona_MostrarNuevaPersona(filtros) {
    if (!SelectorPersona_AbrirNuevaPersonaSiNoEncuentro) return;

    if (filtros != undefined) {
        if ('Nombre' in filtros) {
            nombreRecomendado = filtros.Nombre;
        }
        if ('Apellido' in filtros) {
            apellidoRecomendado = filtros.Apellido;
        }
        if ('NumeroDocumento' in filtros) {
            nroDocRecomendado = filtros.NumeroDocumento;
        }
    }

    $('#SelectorPersona_BtnNuevaPersonaFisica').trigger('click');
    nombreRecomendado = undefined;
    apellidoRecomendado = undefined;
    nroDocRecomendado = undefined;
}

function SelectorPersona_Validar() {
    //Borro las validaciones
    SelectorPersona_BorrarValidaciones();

    var validar = true;

    //Tipo Documento
    var tipoBusqueda = $('#SelectorPersona_Select_Tipo').val();

    if (tipoBusqueda == 1) {

        //Por Numero 

        //NroDocumento
        var nroDocumento = $('#SelectorPersona_Input_NumeroDocumento').val();
        if (nroDocumento == undefined || nroDocumento == "") {
            $('#SelectorPersona_Input_NumeroDocumento').siblings('.control-observacion').text('Dato requerido');
            $('#SelectorPersona_Input_NumeroDocumento').siblings('.control-observacion').stop(true, true).slideDown(300);
            validar = false;
        }
    } else {

        //Por Nombre

        var nombre = $('#SelectorPersona_Input_Nombre').val().trim();
        var apellido = $('#SelectorPersona_Input_Apellido').val().trim();

        if ((nombre == undefined || nombre == "") && (apellido == undefined || apellido == "")) {
            $('#SelectorPersona_Input_Nombre').siblings('.control-observacion').text('Dato requerido');
            $('#SelectorPersona_Input_Nombre').siblings('.control-observacion').stop(true, true).slideDown(300);
            validar = false;
        }
    }

    return validar;
}

function SelectorPersona_BorrarValidaciones() {
    $('#SelectorPersona_Select_TipoPersona').siblings('.control-observacion').text('');
    $('#SelectorPersona_Select_TipoPersona').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#SelectorPersona_Input_NumeroDocumento').siblings('.control-observacion').text('');
    $('#SelectorPersona_Input_NumeroDocumento').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#SelectorPersona_Input_Nombre').siblings('.control-observacion').text('');
    $('#SelectorPersona_Input_Nombre').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#SelectorPersona_Input_Apellido').siblings('.control-observacion').text('');
    $('#SelectorPersona_Input_Apellido').siblings('.control-observacion').stop(true, true).slideUp(300);
}

function SelectorPersona_InitSelectTipoPersona() {
    $('#SelectorPersona_Select_TipoPersona').CargarSelect({
        Data: [{ value: 1, text: 'Física' }, { value: 2, text: 'Jurídica' }],
        Value: 'value',
        Text: 'text'
    });

    //Evento tipo Persona
    $('#SelectorPersona_Select_TipoPersona').on('change', function (e) {
        var value = $(this).val();

        if (value == 1) {
            $('#SelectorPersona_Input_NumeroDocumento').siblings('label').text("Nro Documento");
            $('#SelectorPersona_ContenedorTipoDocumento').show(500);
        } else {
            $('#SelectorPersona_Input_NumeroDocumento').siblings('label').text("CUIL");
            $('#SelectorPersona_ContenedorTipoDocumento').hide(500);
        }
        Materialize.updateTextFields();
    });
}

function SelectorPersona_CargarPersonaSeleccionada(persona) {
    SelectorPersona_Persona = persona;

    $('#SelectorPersona_ContenedorBusqueda').fadeOut(300, function () {
        if (persona.EsPersonaFisica) {
            $('#SelectorPersona_ContenedorPersonaSeleccionada').find('.persona-nombre').text(toTitleCase(persona.Nombre + ' ' + persona.Apellido));
            $('#SelectorPersona_ContenedorPersonaSeleccionada').find('.persona-detalle').text(persona.TipoDocumentoString + ' ' + persona.NroDoc);
        } else {
            $('#SelectorPersona_ContenedorPersonaSeleccionada').find('.persona-nombre').text(toTitleCase(persona.RazonSocial + ' - ' + persona.NombreFantasia));
            $('#SelectorPersona_ContenedorPersonaSeleccionada').find('.persona-detalle').text(persona.Cuit);
        }

        $('#SelectorPersona_ContenedorPersonaSeleccionada').fadeIn(300);
    });

    if (SelectorPersona_Callback != undefined && SelectorPersona_Callback != null) {
        SelectorPersona_Callback(SelectorPersona_Persona);
    }
}

function SelectorPersona_CancelarPersonaSeleccionada() {
    this.SelectorPersona_Persona = undefined;

    $('#SelectorPersona_ContenedorPersonaSeleccionada').stop(true, true).fadeOut(300, function () {

        $('#SelectorPersona_Select_TipoPersona').val(1).trigger('change');
        $('#SelectorPersona_Input_NumeroDocumento').val('');
        $('#SelectorPersona_Input_Nombre').val('');
        $('#SelectorPersona_Input_Apellido').val('');

        Materialize.updateTextFields();

        $('#SelectorPersona_ContenedorBusqueda').stop(true, true).fadeIn(300);

        //Enfoco
        if ($('#SelectorPersona_Select_Tipo').val() == 1) {
            $('#SelectorPersona_Input_NumeroDocumento').trigger('focus');
        } else {
            $('#SelectorPersona_Input_Nombre').trigger('focus');
        }
    });

    if (SelectorPersona_Callback != undefined && SelectorPersona_Callback != null) {
        SelectorPersona_Callback(null);
    }
}

function SelectorPersona_MostrarNuevaPersonaFisica(mostrar) {
    if (mostrar) {
        $('#SelectorPersona_BtnNuevaPersonaFisica').show();
    } else {
        $('#SelectorPersona_BtnNuevaPersonaFisica').hide();
    }
}

function SelectorPersona_MostrarNuevaPersonaJuridica(mostrar) {
    if (mostrar) {
        $('#SelectorPersona_BtnNuevaPersonaJuridica').show();
    } else {
        $('#SelectorPersona_BtnNuevaPersonaJuridica').hide();
    }
}

function SelectorPersona_SetOnPersonaSeleccionadaListener(callback) {
    this.SelectorPersona_Callback = callback;
}

function SelectorPersona_GetPersonaSeleccionada() {
    return SelectorPersona_Persona;
}

function SelectorPersona_IsPersonaSeleccionada() {
    return SelectorPersona_Persona != undefined && SelectorPersona_Persona != null;
}

function SelectorPersona_IsDatosIngresadosSinPersonaSeleccionada() {
    var a = !SelectorPersona_IsPersonaSeleccionada();
    var b = $('#SelectorPersona_Input_NumeroDocumento').val().trim() != "";
    return a && b
}

function SelectorPersona_MostrarAgregarPersona(mostrar) {
    if (mostrar) {
        $('#SelectorPersona_BtnNuevaPersonaFisica').show(300);
    } else {
        $('#SelectorPersona_BtnNuevaPersonaFisica').hide(300);
    }
}

function SelectorPersona_SetPersona(persona) {
    SelectorPersona_Persona = persona;
    SelectorPersona_CargarPersonaSeleccionada(persona);


}

function SelectorPersona_ReiniciarUI() {
    SelectorPersona_Persona = undefined;

    SelectorPersona_BorrarValidaciones();

    $('#SelectorPersona_Select_TipoPersona').val(1).trigger('change');
    $('#SelectorPersona_Select_TipoDocumento').val(1).trigger('change');
    $('#SelectorPersona_Input_NumeroDocumento').val('');
    $('#SelectorPersona_ContenedorBusqueda').show();
    $('#SelectorPersona_ContenedorPersonaSeleccionada').hide();
    Materialize.updateTextFields();
}

function SelectorPersona_SetAbrirPersonaSiNoEncuentro(value) {
    SelectorPersona_AbrirNuevaPersonaSiNoEncuentro = value;
}

//-------------------------------
// Cargando
//-------------------------------

function SelectorPersona_MostrarCargando(mostrar, mensaje) {
    $('#SelectorPersona_Select_TipoDocumento').prop('disabled', mostrar);
    $('#SelectorPersona_Select_TipoPersona').prop('disabled', mostrar);
    $('#SelectorPersona_Input_NumeroDocumento').prop('disabled', mostrar);
    $('#SelectorPersona_BtnBuscar').prop('disabled', mostrar);

    if (SelectorPersona_CallbackCargando != undefined) {
        SelectorPersona_CallbackCargando(mostrar, mensaje);
    }
}

function SelectorPersona_SetOnCargandoListener(callback) {
    this.SelectorPersona_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function SelectorPersona_SetOnMensajeListener(callback) {
    this.SelectorPersona_CallbackMensaje = callback;
}

function SelectorPersona_MostrarMensajeError(mensaje) {
    if (SelectorPersona_CallbackMensaje == undefined) return;
    SelectorPersona_CallbackMensaje('Error', mensaje);
}

function SelectorPersona_MostrarMensajeAlerta(mensaje) {
    if (SelectorPersona_CallbackMensaje == undefined) return;
    SelectorPersona_CallbackMensaje('Alerta', mensaje);
}

function SelectorPersona_MostrarMensajeInfo(mensaje) {
    if (SelectorPersona_CallbackMensaje == undefined) return;
    SelectorPersona_CallbackMensaje('Info', mensaje);
}
