var modoRegistrar = 'Registrar';
var modoEditar = 'Editar';

var modo;

var callbackRegistrar;
var callbackEditar;

var tipos;
var estados;
var condiciones;
var tiposCombustible;

var entidad;

var idArea = -1;

function init(data) {

    tipos = data.Tipos;
    estados = data.Estados;
    condiciones = data.Condiciones;
    tiposCombustible = data.TiposCombustible;

    //Cargo los tipos
    $('#select_TipoMovil').CargarSelect({
        Data: tipos,
        Value: 'Id',
        Text: 'Nombre',
        Default: 'Seleccione...',
        Sort: true
    });

    //Cargo las condiciones
    $('#select_Condicion').CargarSelect({
        Data: condiciones,
        Value: 'KeyValue',
        Text: 'Nombre',
        Sort: false
    });

    //Cargo los estados
    $('#select_Estado').CargarSelect({
        Data: estados,
        Value: 'KeyValue',
        Text: 'Nombre',
        Sort: false
    });

    //Cargo las áreas
    $('#select_Area').CargarSelect({
        Data: usuarioLogeado.Areas,
        Value: 'Id',
        Text: 'Nombre',
        Sort: false
    });

    //Cargo los tipos de combustible
    $('#select_TipoCombustible').CargarSelect({
        Data: tiposCombustible,
        Value: 'KeyValue',
        Text: 'Nombre',
        Default: 'Seleccione...',
        Sort: false
    });

    if (idArea != -1) {
        $('#select_Area').val(idArea).trigger('change');
    }

    //----------------------------
    // MODO
    //----------------------------

    var editando = $.url().param('Id') != undefined && data.Movil != undefined;
    if (!editando) {
        modo = modoRegistrar;
        $("#contenedorDatosAdicionales").show();
    } else {
        modo = modoEditar;
        setMovil(data.Movil);
    }

    //Evento datepicker fecha de incorporacion
    $('#botonFechaIncorporacion').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaIncorporacion').click();
        e.stopPropagation()
    });

    //Evento datepicker fecha de valuacion
    $('#botonFechaValuacion').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaValuacion').click();
        e.stopPropagation()
    });

    //Evento datepicker fecha de km
    $('#botonFechaKilometraje').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaKilometraje').click();
        e.stopPropagation()
    });

    //Evento datepicker fecha de ITV
    $('#botonVencimientoITV').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerVencimientoITV').click();
        e.stopPropagation()
    });

    //Evento datepicker fecha de TUV
    $('#botonVencimientoTUV').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerVencimientoTUV').click();
        e.stopPropagation()
    });

    //inicializar datepickeres
    $('#pickerFechaIncorporacion').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        container: top.$(top.document).find('body'),
        max: new Date(),
        selectMonths: true,
        selectYears: 60,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputFechaIncorporacion').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputFechaIncorporacion').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

    $('#pickerFechaValuacion').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        selectYears: 5,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputFechaValuacion').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputFechaValuacion').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

    $('#pickerFechaKilometraje').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        selectYears: 5,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputFechaKilometraje').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputFechaKilometraje').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });


    $('#pickerVencimientoITV').pickadate({
        // Date limits
        min: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        selectYears: 5,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputVencimientoITV').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputVencimientoITV').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

    $('#pickerVencimientoTUV').pickadate({
        // Date limits
        min: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        selectYears: 5,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputVencimientoTUV').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputVencimientoTUV').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

     $('#btnNuevoTipo').click(function () {
        crearDialogoTipoMovilNuevo({
            CallbackTipoMovilNuevo: function (tipos) {
                //Cargo los tipos
                $('#select_TipoMovil').CargarSelect({
                    Data: tipos,
                    Value: 'Id',
                    Text: 'Nombre',
                    Default: 'Seleccione...',
                    Sort: true
                });

            },
            CallbackMensajes: function () { }
        });
    });

    $('#formNuevo')
    .validate({
        rules: {
            date: {
                fecha: true
            }
        }
    });

    $.validator.setDefaults({
        ignore: [],
        rules: {
            fechaMenorQueHoy: {
                fechaMenorQueHoy: true
            },
        },
    });

}

//--------------------------
// Operacion principal
//--------------------------

function registrar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Registrando Móvil...');
    var comando = getMovil();
    console.log(comando);

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/Insertar'),
        Data: { comando: comando },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensajeError('Error insertando el móvil');
                return;
            }

            informarRegistrar(result.Return);
            limpiar();
        },
        OnError: function (result) {
            console.log(result);
            mostrarCargando(false);
            mostrarMensajeError('Error insertando el móvil');
            mostrarMensajeError(result.Error);
        }
    });
}

function editar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Editando móvil...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/EditarInformacionBasica'),
        Data: { comando: getMovil() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok || !result.Return) {
                mostrarMensajeError('Error editando el móvil');
                return;
            }

            informarEditar();
            limpiar();
        },
        OnError: function (result) {
            console.log(result);
            mostrarCargando(false);
            mostrarMensajeError('Error editando la información básica del móvil');
            mostrarMensajeError(result.Error);
        }
    });
}

function validar() {
    //tipo movil
    //var tipo = $('#select_TipoMovil').val();
    //if (tipo == undefined || tipo <= 0) {
    //    $('#select_TipoMovil').siblings('.control-observacion').text('Dato requerido');
    //}

    //var hoy = new Date();
    //$('#inputFechaIncorporacion').siblings('.control-observacion').text('');
    //var fechaIncorporacion=$("#inputFechaIncorporacion").val();
    //var fechaIncorporacion = moment(fechaIncorporacion, 'DD/MM/YYYY');
    //if (fechaIncorporacion.isValid()) {
    //    if (fechaIncorporacion > hoy) {
    //        $('#inputFechaIncorporacion').siblings('.control-observacion').text('La fecha ingresada debe ser menor a hoy');
    //    }
    //} else {
    //    $('#inputFechaIncorporacion').siblings('.control-observacion').text('Dato inválido');
    //}

    //var fechaValuacion = moment($("#inputFechaValuacion").val(), 'DD/MM/YYYY');
    //if (fechaValuacion.isValid()) {
    //    if (fechaValuacion > hoy) {
    //        $('#inputFechaValuacion').siblings('.control-observacion').text('La fecha ingresada debe ser menor a hoy');
    //    }
    //} else {
    //    $('#inputFechaValuacion').siblings('.control-observacion').text('Dato inválido');
    //}

    //var FechaKilometraje = moment($("#inputFechaKilometraje").val(), 'DD/MM/YYYY');
    //if (FechaKilometraje.isValid()) {
    //    if (FechaKilometraje > hoy) {
    //        $('#inputFechaKilometraje').siblings('.control-observacion').text('La fecha ingresada debe ser menor a hoy');
    //    }
    //} else {
    //    $('#inputFechaKilometraje').siblings('.control-observacion').text('Dato inválido');
    //}

    //var fechaITV = moment($("#inputVencimientoITV").val(), 'DD/MM/YYYY');
    //if (fechaITV.isValid()) {
    //    if (fechaITV > hoy) {
    //        $('#inputVencimientoITV').siblings('.control-observacion').text('La fecha ingresada debe ser menor a hoy');
    //    }
    //} else {
    //    $('#inputVencimientoITV').siblings('.control-observacion').text('Dato inválido');
    //}

    //var fechaTaller = moment($("#inputFechaTaller").val(), 'DD/MM/YYYY');
    //if (fechaTaller.isValid()) {
    //    if (fechaTaller > hoy) {
    //        $('#inputFechaTaller').siblings('.control-observacion').text('La fecha ingresada debe ser menor a hoy');
    //    }
    //} else {
    //    $('#inputFechaTaller').siblings('.control-observacion').text('Dato inválido');
    //}

    return $('#formNuevo').valid();
}

function getMovil() {

    var entity = {};
    if (modo == modoEditar) {
        entity.Id = "" + entidad.Id;
    }
    entity.NumeroInterno = "" + $('#inputFormulario_NumeroInterno').val();

    var fechaIncorporacion = moment($("#inputFechaIncorporacion").val(), 'DD/MM/YYYY');
    if (fechaIncorporacion.isValid() && fechaIncorporacion != undefined) {
        //fechaIncorporacion = fechaIncorporacion.format('DD/MM/YYYY');
        entity.FechaIncorporacion = fechaIncorporacion;
    }

    entity.IdArea = $('#select_Area').val();
    entity.IdTipo = $('#select_TipoMovil').val();
    entity.Modelo = $('#inputFormulario_Modelo').val();
    entity.Marca = $('#inputFormulario_Marca').val();
    entity.Dominio = $('#inputFormulario_Dominio').val();
    entity.Año = $('#inputFormulario_Año').val();

    var carga = $('#inputFormulario_Carga').val();
    if (carga != "") {
        entity.Carga = carga;
    }

    var asientos = $('#inputFormulario_Asientos').val();
    if (asientos != "") {
        entity.Asientos = asientos;
    }

    var valuacion = $('#inputFormulario_Valuacion').val();
    if (valuacion != "") {
        entity.Valuacion = valuacion;
    }

    var fechaValuacion = moment($("#inputFechaValuacion").val(), 'DD/MM/YYYY');
    if (fechaValuacion.isValid() && fechaValuacion != undefined) {
        //fechaValuacion = fechaValuacion.format('DD/MM/YYYY');
        entity.FechaValuacion = fechaValuacion;
    }

    var km = $('#inputFormulario_Km').val();
    if (km != "") {
        entity.Kilometraje = km;
    }

    var FechaKilometraje = moment($("#inputFechaKilometraje").val(), 'DD/MM/YYYY');
    if (FechaKilometraje.isValid() && FechaKilometraje != undefined) {
        //FechaKilometraje = FechaKilometraje.format('DD/MM/YYYY');
        entity.FechaKilometraje = FechaKilometraje;
    }

    var fechaITV = moment($("#inputVencimientoITV").val(), 'DD/MM/YYYY');
    if (fechaITV.isValid() && fechaITV != undefined) {
        //fechaITV = fechaITV.format('DD/MM/YYYY');
        entity.VencimientoITV = fechaITV;
    }

    var fechaTUV = moment($("#inputVencimientoTUV").val(), 'DD/MM/YYYY');
    if (fechaTUV.isValid() && fechaTUV != undefined) {
        //fechaTUV = fechaTUV.format('DD/MM/YYYY');
        entity.VencimientoTUV = fechaTUV;
    }

    entity.Caracteristicas = $('#inputFormulario_Caracteristicas').val();
    entity.IdEstado = $('#select_Estado').val();

    var condicion = $('#select_Condicion').val();
    if (condicion != "-1") {
        entity.IdCondicion = condicion;
    }

    var tipoCombustible = $('#select_TipoCombustible').val();
    if (tipoCombustible != "-1") {
        entity.IdTipoCombustible = tipoCombustible;
    }
    return entity;
}

function setMovil(entity) {
    $('#inputFormulario_NumeroInterno').val(entity.NumeroInterno);
    $('#select_Area').val(entity.IdArea).trigger('change');
    if (entity.FechaIncorporacion != null) {
        $("#inputFechaIncorporacion").val(moment(entity.FechaIncorporacion).format('DD/MM/YYYY'));
    }
    $('#inputFechaIncorporacion').trigger('focus');
    $('#inputFechaIncorporacion').trigger('blur');
    $('#select_TipoMovil').val(entity.IdTipo).trigger('change');
    $('#inputFormulario_Modelo').val(entity.Modelo);
    $('#inputFormulario_Marca').val(entity.Marca);
    $('#inputFormulario_Dominio').val(entity.Dominio);
    $('#inputFormulario_Año').val(entity.Año);
    $('#inputFormulario_Carga').val(entity.Carga);
    $('#inputFormulario_Asientos').val(entity.Asientos);
    $('#inputFormulario_Valuacion').val(entity.Valuacion);
    $('#inputFormulario_Caracteristicas').val(entity.Caracteristicas);
    if (entity.FechaValuacion != null) {
        $("#inputFechaValuacion").val(moment(entity.FechaValuacion).format('DD/MM/YYYY'));
    }
    $('#inputFormulario_Km').val(entity.Kilometraje);
    if (entity.FechaKilometraje != null) {
        $("#inputFechaKilometraje").val(moment(entity.FechaKilometraje).format('DD/MM/YYYY'));
    }
    if (entity.VencimientoTUV != null) {
        $("#inputVencimientoTUV").val(moment(entity.VencimientoTUV).format('DD/MM/YYYY'));
    }
    if (entity.VencimientoITV != null) {
        $("#inputVencimientoITV").val(moment(entity.VencimientoITV).format('DD/MM/YYYY'));
    }
    $('#select_Estado').val(entity.IdEstado).trigger('change');
    $('#select_Condicion').val(entity.IdCondicion).trigger('change');
    if (entity.IdTipoCombustible != 0)
        $('#select_TipoCombustible').val(entity.IdTipoCombustible).trigger('change');
    Materialize.updateTextFields();
    entidad = entity;
}

function limpiar() {
    $('#inputFormulario_NumeroInterno').val("");
    $("#inputFechaIncorporacion").val("");
    $('#select_TipoMovil').val(-1).trigger('change');
    $('#inputFormulario_Modelo').val("");
    $('#inputFormulario_Marca').val("");
    $('#inputFormulario_Dominio').val("");
    $('#inputFormulario_Año').val("");
    $('#inputFormulario_Carga').val("");
    $('#inputFormulario_Asientos').val("");
    $('#inputFormulario_Valuacion').val("");
    $("#inputFechaValuacion").val("");
    $('#inputFormulario_Km').val("");
    $("#inputFechaKilometraje").val("");
    $("#inputVencimientoITV").val("");
    $("#inputVencimientoTUV").val("");
    $('#select_Estado').val(1).trigger('change');
    $('#select_Condicion').val(-1).trigger('change');
    $('#select_TipoCombustible').val(-1).trigger('change');
    entity = undefined;
}

//-------------------------------
// Registrar
//-------------------------------

function setOnRegistrarCompletoListener(callback) {
    callbackRegistrar = callback;
}

function informarRegistrar(movil) {
    if (callbackRegistrar == undefined || callbackRegistrar == null) return;
    callbackRegistrar(movil);
}

//-------------------------------
// Editar
//-------------------------------

function setOnEditarCompletoListener(callback) {
    callbackEditar = callback;
}

function informarEditar(zona) {
    if (callbackEditar == undefined || callbackEditar == null) return;
    callbackEditar(zona);
}


////-----------------------------
//// Alertas
////-----------------------------

function setOnMensajeListener(callback) {
    this.callbackMensaje = callback;
}

function mostrarMensajeError(mensaje) {
    if (callbackMensaje == undefined) return;
    callbackMensaje('Error', mensaje);
}

function mostrarMensajeAlerta(mensaje) {
    if (callbackMensaje == undefined) return;
    callbackMensaje('Alerta', mensaje);
}

function mostrarMensajeInfo(mensaje) {
    if (callbackMensaje == undefined) return;
    callbackMensaje('Info', mensaje);
}

function setArea(id) {
    idArea = id;
    if ($('#select_Area').length > 0) {
        $('#select_Area').val(id).trigger('change');
    }
}