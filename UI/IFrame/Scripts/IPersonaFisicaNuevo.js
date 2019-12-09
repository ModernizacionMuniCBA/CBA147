var modoRegistrar = 'Registrar';
var modoEditar = 'Editar';

var modo;
var persona;

var callbackMensaje;
var callbackCargando;
var callbackRegistrar;
var callbackEditar;

function init(data) {
    //--------------------------
    // Init Data
    //--------------------------
    data = parse(data);
    persona = data.Persona;

    //Cargo los tipos de doc
    $('#select_TipoDocumento').CargarSelect({
        Data: data.TiposDocumento,
        Value: 'Id',
        Text: 'Nombre',
        TitleCase: false
    });

    //Fecha de nacimiento
    $('#btnFechaNacimiento').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#picker_FechaNacimiento').click();
        e.stopPropagation()
    });

    //inicializar datepickeres
    $('#picker_FechaNacimiento').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: window.parent.$(window.parent.document).find('body'),
        selectMonths: true,
        selectYears: 200,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#input_FechaNacimiento').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#input_FechaNacimiento').val('');
            }


            Materialize.updateTextFields();
        }
    });

    //-----------------------------
    // Domicilio
    //-----------------------------

    SelectorDomicilio_SetOnMensajeListener(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    });

    SelectorDomicilio_SetOnCargandoListener(function (mostrar, mensaje) {
        mostrarCargando(mostrar, mensaje);
    });

    //----------------------------
    // MODO
    //----------------------------

    var editando = $.url().param('Id') != undefined;
    if (!editando) {
        modo = modoRegistrar;

        var nombreRecomendado = $.url().param('nombre');
        if (nombreRecomendado != undefined) {
            $('#input_Nombre').val(toTitleCase(nombreRecomendado));
        }

        var apellidoRecomendado = $.url().param('apellido');
        if (apellidoRecomendado != undefined) {
            $('#input_Apellido').val(toTitleCase(apellidoRecomendado));
        }

        var nroDocRecomendado = $.url().param('nroDoc');
        if (nroDocRecomendado != undefined) {
            $('#input_NumeroDocumento').val(toTitleCase(nroDocRecomendado));
        }

    } else {
        modo = modoEditar;
        if (persona == null) {
            mostrarMensaje('Error', 'Error inicializando la pantalla');
            return;
        }

        $('#select_TipoDocumento').val(persona.TipoDocumento.Id).trigger('change');
        $('#input_NumeroDocumento').val(persona.NroDoc);
        $('#input_Cuil').val(persona.Cuil);
        $('#input_Nombre').val(toTitleCase(persona.Nombre));
        $('#input_Apellido').val(toTitleCase(persona.Apellido));
        $('#input_FechaNacimiento').val(persona.FechaNacimientoString);
        if (persona.Sexo == 'Masculino') {
            $('#rdbSexoMasculino').prop('checked', true);
        } else {
            $('#rdbSexoFemenino').prop('checked', true);
        }
        $('#input_Observaciones').val(persona.Observaciones);
        $('#input_Mail').val(persona.Mail);
        $('#input_Telefono').val(persona.Telefono);
        SelectorDomicilio_SetDomicilio(persona.Domicilio);
        Materialize.updateTextFields();
    }
}

function errorInit() {
    mostrarMensaje('Error', 'Error inicializando la pagina');
}

//--------------------------
// Operacion principal
//--------------------------

function registrar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Registrando persona física...');

    var dataAjax = { persona: JSON.stringify(getPersonaFisica()) };
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/Servicios/PersonaFisicaService.asmx/Insertar'),
        success: function (result) {
            result = parse(result.d);
            mostrarCargando(false);

            if ('Error' in result) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            if (result.PersonaFisica == null) {
                mostrarMensaje('Error', 'Error registrando la persona física');
                return;
            }

            informarRegistrar(result.PersonaFisica);
        },
        error: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', result.statusText);
        }
    });
}

function editar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Editando persona física...');

    var dataAjax = { persona: JSON.stringify(getPersonaFisica()) };
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/Servicios/PersonaFisicaService.asmx/Editar'),
        success: function (result) {
            result = parse(result.d);
            mostrarCargando(false);

            if ('Error' in result) {
                mostrarMensaje('Error', result.Error.Publico);
                console.log('Error editando la persona fisica');
                console.log(dataAjax);
                console.log(result);
                return;
            }

            if (result.PersonaFisica == null) {
                mostrarMensaje('Error', 'Error editando la persona física');
                console.log('Error editando la persona fisica');
                console.log(dataAjax);
                console.log(result);
                return;
            }

            informarEditar(result.PersonaFisica);
        },
        error: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error editando la persona física');
            console.log('Error editando la persona fisica');
            console.log(dataAjax);
            console.log(result);

        }
    });
}

function validar() {
    $('.control-observacion').text('');

    var todoOk = true;

    var tipoDoc = $('#select_TipoDocumento').val();
    if (tipoDoc == undefined || tipoDoc <= 0) {
        $('#select_TipoDocumento').siblings('.control-observacion').text('Dato requerido');
        todoOk = false;
    }

    var nroDoc = $('#input_NumeroDocumento').val();
    if (nroDoc == undefined || nroDoc == "") {
        $('#input_NumeroDocumento').siblings('.control-observacion').text('Dato requerido');
        todoOk = false;
    }

    var cuil = $('#input_Cuil').val();
    if (cuil != undefined && cuil != "") {
        if (!validarCuil(cuil)) {
            $('#input_Cuil').siblings('.control-observacion').text('Dato invalido');
            todoOk = false;
        }
    }

    var nombre = $('#input_Nombre').val();
    if (nombre == undefined || nombre == "") {
        $('#input_Nombre').siblings('.control-observacion').text('Dato requerido');
        todoOk = false;
    } else {
        if (validarNombre(nombre) == false) {
            $('#input_Nombre').siblings('.control-observacion').text('Dato invalido');
            todoOk = false;
        }
    }



    var apellido = $('#input_Apellido').val();
    if (apellido == undefined || apellido == "") {
        $('#input_Apellido').siblings('.control-observacion').text('Dato requerido');
        todoOk = false;
    } else {
        if (validarNombre(apellido) == false) {
            $('#input_Apellido').siblings('.control-observacion').text('Dato invalido');
            todoOk = false;
        }
    }

    var mail = $('#input_Mail').val();
    if (mail.trim() == "") {
        $('#input_Mail').siblings('.control-observacion').text('Dato requerido');
        todoOk = false;
    } else {
        if (!validarEmail(mail)) {
            $('#input_Mail').siblings('.control-observacion').text('Mail invalido');
            todoOk = false;
        }
    }



    var fechaNacimiento = $('#input_FechaNacimiento').val();
    if (fechaNacimiento != undefined && fechaNacimiento != "") {
        var date = moment(fechaNacimiento, 'DD/MM/YYYY');
        if (!date.isValid()) {
            $('#input_FechaNacimiento').siblings('.control-observacion').text('Dato invalido');
            todoOk = false;
        } else {
            var now = moment();
            if (date > now) {
                $('#input_FechaNacimiento').siblings('.control-observacion').text('Dato invalido');
                todoOk = false;
            }
        }
    }

    //Valido ubicacion
    if (SelectorDomicilio_IsDatosIngresadosSinDomicilioSeleccionado()) {
        $('#errorFormulario_Domicilio').text('Debe validar el domicilio');
        $('#errorFormulario_Domicilio').stop(true, true).slideDown(300);
        todoOk = false;
    } else {
        if (!SelectorDomicilio_IsDomicilioSeleccionado()) {
            $('#errorFormulario_Domicilio').text('Dato requerido');
            $('#errorFormulario_Domicilio').stop(true, true).slideDown(300);
            todoOk = false;
        }
    }

    return todoOk;
}

function getPersonaFisica() {
    var personaFisica = {};
    if (modo == modoEditar) {
        personaFisica.Id = "" + persona.Id;
    }
    personaFisica.IdTipoDocumento = "" + $('#select_TipoDocumento').val();
    personaFisica.NumeroDocumento = "" + $('#input_NumeroDocumento').val();
    personaFisica.Cuil = "" + $('#input_Cuil').val();
    personaFisica.Nombre = "" + $('#input_Nombre').val();
    personaFisica.Apellido = "" + $('#input_Apellido').val();
    personaFisica.FechaNacimiento = "" + $('#input_FechaNacimiento').val();
    personaFisica.SexoMasculino = "" + $('#rdbSexoMasculino').is(':checked');
    personaFisica.Observaciones = "" + $('#input_Observaciones').val();
    personaFisica.Telefono = "" + $('#input_Telefono').val();
    personaFisica.Mail = "" + $('#input_Mail').val();
    personaFisica.Domicilio = SelectorDomicilio_GetDomicilioSeleccionado();

    return personaFisica;
}

//-------------------------------
// Registrar
//-------------------------------

function setOnRegistrarCompletoListener(callback) {
    callbackRegistrar = callback;
}

function informarRegistrar(persona) {
    if (callbackRegistrar == undefined || callbackRegistrar == null) return;
    callbackRegistrar(persona);
}

//-------------------------------
// Editar
//-------------------------------

function setOnEditarCompletoListener(callback) {
    callbackEditar = callback;
}

function informarEditar(persona) {
    if (callbackEditar == undefined || callbackEditar == null) return;
    callbackEditar(persona);
}

////-------------------------------
//// Cargando
////-------------------------------

//function mostrarCargando(mostrar, mensaje) {
//    if (callbackCargando != undefined) {
//        callbackCargando(mostrar, mensaje);
//    }
//}

//function setOnCargandoListener(callback) {
//    this.callbackCargando = callback;
//}

////-----------------------------
//// Alertas
////-----------------------------

//function setOnMensajeListener(callback) {
//    this.callbackMensaje = callback;

//}

//function mostrarMensaje(tipo, mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje(tipo, mensaje);
//}