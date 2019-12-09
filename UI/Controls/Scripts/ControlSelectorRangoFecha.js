var ControlSelectorRangoFecha_CallbackMensaje;
var ControlSelectorRangoFecha_CallbackCargando;

var meses = [
    { Nombre: "Enero", Id: 1 },
    { Nombre: "Febrero", Id: 2 },
    { Nombre: "Marzo", Id: 3 },
    { Nombre: "Abril", Id: 4 },
    { Nombre: "Mayo", Id: 5 },
    { Nombre: "Junio", Id: 6 },
    { Nombre: "Julio", Id: 7 },
    { Nombre: "Agosto", Id: 8 },
    { Nombre: "Septiembre", Id: 9 },
    { Nombre: "Octubre", Id: 10 },
    { Nombre: "Noviembre", Id: 11 },
    { Nombre: "Diciembre", Id: 12 }
]

$(function () {
    //Servicios
    var fechas = [];
    fechas.push({
        Id: 1,
        Nombre: 'Entre fechas'
    });

    fechas.push({
        Id: 2,
        Nombre: 'Ultimos 7 dias'
    });

    fechas.push({
        Id: 3,
        Nombre: 'Ultimos 15 dias'
    });

    fechas.push({
        Id: 4,
        Nombre: 'Ultimos 30 dias'
    });

    fechas.push({
        Id: 5,
        Nombre: 'Año actual'
    });

    $('#ControlSelectorRangoFecha_select_Fecha').CargarSelect({
        Data: fechas,
        Value: 'Id',
        Text: 'Nombre',
        Sort: false
    });

    $('#ControlSelectorRangoFecha_select_Fecha').on('change', function () {
        var val = parseInt($('#ControlSelectorRangoFecha_select_Fecha').val());

        var fechaDesde;
        var fechaHasta = new moment();
        var ahora = new moment();
        switch (val) {
            case 1: {
                fechaDesde = undefined;
                //fechaHasta = ahora;
                //fechaDesde = ahora.clone().add(-30, 'days');
            } break;

                //Ultimos 7 dias
            case 2: {
                fechaDesde = ahora.clone().add(-7, 'days');
            } break;

                //Ultimos 15 dias
            case 3: {
                fechaDesde = ahora.clone().add(-15, 'days');
            } break;

                //Ultimos 30 dias
            case 4: {
                // fechaDesde = ahora.clone().date(1);
                fechaHasta = ahora;
                fechaDesde = ahora.clone().add(-30, 'days');
            } break;

                //Ultimo año
            case 5: {
                fechaDesde = ahora.clone().month(0).date(1);
            } break;
        }

        $('#ControlSelectorRangoFecha_ContenedorFechaDesde').stop(true, true).fadeOut(300);
        $('#ControlSelectorRangoFecha_ContenedorFechaHasta').stop(true, true).fadeOut(300, function () {
            if (fechaDesde == undefined) {
                $('#ControlSelectorRangoFecha_input_FechaDesde').val('');
                $('#ControlSelectorRangoFecha_input_FechaHasta').val('');
            } else {
                $('#ControlSelectorRangoFecha_input_FechaDesde').val(fechaDesde.format('DD/MM/YYYY'));
                $('#ControlSelectorRangoFecha_input_FechaHasta').val(fechaHasta.format('DD/MM/YYYY'));
            }


            $('#ControlSelectorRangoFecha_ContenedorFechaDesde').stop(true, true).fadeIn(300);
            $('#ControlSelectorRangoFecha_ContenedorFechaHasta').stop(true, true).fadeIn(300, function () {

            });
            $('#ControlSelectorRangoFecha_input_FechaDesde').trigger('focus');
            $('#ControlSelectorRangoFecha_input_FechaHasta').trigger('focus');
            $('#ControlSelectorRangoFecha_input_FechaHasta').trigger('blur');

        });

    });

    //Evento datepicker fecha desde
    $('#ControlSelectorRangoFecha_boton_FechaDesde').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#ControlSelectorRangoFecha_picker_FechaDesde').click();
        e.stopPropagation()
    });

    //Evento datepicker fecha hasta
    $('#ControlSelectorRangoFecha_boton_FechaHasta').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#ControlSelectorRangoFecha_picker_FechaHasta').click();
        e.stopPropagation()
    });

    //inicializar datepickeres
    $('#ControlSelectorRangoFecha_picker_FechaDesde').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: 'html',
        selectMonths: true,
        selectYears: 5,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#ControlSelectorRangoFecha_input_FechaDesde').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#ControlSelectorRangoFecha_input_FechaDesde').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

    $('#ControlSelectorRangoFecha_picker_FechaHasta').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: 'html',
        selectMonths: true,
        selectYears: 5,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#ControlSelectorRangoFecha_input_FechaHasta').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#ControlSelectorRangoFecha_input_FechaHasta').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

    /*Precarco datos para los meses*/

    var dataAños = getYears(2015);

    //Años
    $('#select_Años').CargarSelect({
        Data: dataAños,
        Value: 'Id',
        Text: 'Año',
        Default: 'Seleccione...',
        Sort: false
    });

    //Meses
    $('#select_Meses').CargarSelect({
        Data: [],
        Value: 'Id',
        Text: 'Nombre',
        Default: 'Seleccione...',
        Sort: false
    });

    //Cada vez q cambia el año   
    $('#select_Años').on('change', function (e) {
        var value = $(this).val();
        $('#select_Meses').CargarSelect({
            Data: getMonths(value),
            Value: 'Value',
            Text: 'Text',
            Default: 'Seleccione...',
            Sort: false
        });
    });

    //Evento solo mes 
    $('#check_SoloMes').on('change', function (e) {

        var value = $(this).val();

        if ($('#check_SoloMes').is(':checked')) {
            $('#ControlSelectorRangoFecha_select_Fecha').prop('disabled', true);
            $('#ControlSelectorRangoFecha_input_FechaDesde').prop('disabled', true);
            $('#ControlSelectorRangoFecha_picker_FechaDesde').prop('disabled', true);
            $('#ControlSelectorRangoFecha_boton_FechaDesde').prop('disabled', true);
            $('#ControlSelectorRangoFecha_input_FechaHasta').prop('disabled', true);
            $('#ControlSelectorRangoFecha_picker_FechaHasta').prop('disabled', true);
            $('#ControlSelectorRangoFecha_boton_FechaHasta').prop('disabled', true);
            $('#i_mes').show();
            $('#ControlSelectorRangoFecha_input_FechaHasta').siblings('.control-observacion').stop(true, true).slideUp(300);
            $('#ControlSelectorRangoFecha_input_FechaDesde').siblings('.control-observacion').stop(true, true).slideUp(300);

            $('#select_Meses').siblings('.control-observacion').stop(true, true).slideUp(300);
            $('#select_Años').siblings('.control-observacion').stop(true, true).slideUp(300);


            ControlSelectorMes_Limpiar();
            ControlSelectorRangoFecha_Limpiar();
        }
        else {
            $('#ControlSelectorRangoFecha_select_Fecha').prop('disabled', false);
            $('#ControlSelectorRangoFecha_input_FechaDesde').prop('disabled', false);
            $('#ControlSelectorRangoFecha_picker_FechaDesde').prop('disabled', false);
            $('#ControlSelectorRangoFecha_boton_FechaDesde').prop('disabled', false);
            $('#ControlSelectorRangoFecha_input_FechaHasta').prop('disabled', false);
            $('#ControlSelectorRangoFecha_picker_FechaHasta').prop('disabled', false);
            $('#ControlSelectorRangoFecha_boton_FechaHasta').prop('disabled', false);
            $('#i_mes').hide();
            $('#ControlSelectorRangoFecha_input_FechaHasta').siblings('.control-observacion').stop(true, true).slideUp(300);
            $('#ControlSelectorRangoFecha_input_FechaDesde').siblings('.control-observacion').stop(true, true).slideUp(300);
            $('#select_Meses').siblings('.control-observacion').stop(true, true).slideUp(300);
            $('#select_Años').siblings('.control-observacion').stop(true, true).slideUp(300);

            ControlSelectorMes_Limpiar();
            ControlSelectorRangoFecha_Limpiar();
        }

        if (value == -1) {
            return;
        }
    });
});


function ControlSelectorRangoFecha_Validar() {
    var valido = true;
    $('#ControlSelectorRangoFecha_input_FechaDesde').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#ControlSelectorRangoFecha_input_FechaHasta').siblings('.control-observacion').stop(true, true).slideUp(300);

    //Fecha desde
    var fechaDesdeString = $("#ControlSelectorRangoFecha_input_FechaDesde").val()
    if (fechaDesdeString == "") {
        $('#ControlSelectorRangoFecha_input_FechaDesde').siblings('.control-observacion').text('Dato requerido');
        $('#ControlSelectorRangoFecha_input_FechaDesde').siblings('.control-observacion').stop(true, true).slideDown(300);
        valido = false;
    }


    //Fecha hasta
    var fechaHastaString = $("#ControlSelectorRangoFecha_input_FechaHasta").val();
    if (fechaHastaString == "") {
        $('#ControlSelectorRangoFecha_input_FechaHasta').siblings('.control-observacion').text('Dato requerido');
        $('#ControlSelectorRangoFecha_input_FechaHasta').siblings('.control-observacion').stop(true, true).slideDown(300);
        valido = false;
    }


    //Si ya tengo las fechas valido que tengan datos validos
    if (fechaDesdeString != "" && fechaHastaString != "") {

        var fechaDesde = moment($("#ControlSelectorRangoFecha_input_FechaDesde").val(), 'DD/MM/YYYY');
        if (fechaDesdeString != undefined && fechaDesdeString != "" && !fechaDesde.isValid()) {
            $('#ControlSelectorRangoFecha_input_FechaDesde').siblings('.control-observacion').text('Dato invalido');
            $('#ControlSelectorRangoFecha_input_FechaDesde').siblings('.control-observacion').stop(true, true).slideDown(300);
            valido = false;
        }

        var fechaHasta = moment($("#ControlSelectorRangoFecha_input_FechaHasta").val(), 'DD/MM/YYYY');
        if (fechaHastaString != undefined && fechaHastaString != "" && !fechaHasta.isValid()) {
            $('#ControlSelectorRangoFecha_input_FechaHasta').siblings('.control-observacion').text('Dato invalido');
            $('#ControlSelectorRangoFecha_input_FechaHasta').siblings('.control-observacion').stop(true, true).slideDown(300);
            valido = false;
        }

        if (fechaDesde.isValid() && fechaHasta.isValid()) {
            if (fechaDesde > fechaHasta) {
                ControlSelectorRangoFecha_MostrarMensajeAlerta('La fecha de inicio del rango debe ser menor a la fecha de fin');
                valido = false;
            }
        }
    }

    return valido;
}

function getYears(from) {
    var d1 = new Date();
    d1.setFullYear(from);

    d2 = new Date();
    yr = [];

    for (var i = d2.getFullYear() ; i >= d1.getFullYear() ; i--) {
        yr.push({ Año: '' + i, Id: i });
    }

    return yr;
}

function getMonths(yearFrom) {
    //Parto desde enero siempre hasta diciembre
    var d1 = new Date(yearFrom, 0, 1);
    var d2 = new Date(yearFrom, 11, 31);
    //si el año pedido es igual al actual, se devuelven los meses desde enero hasta la fecha
    if (new Date().getFullYear() == d2.getFullYear()) {
        d2 = new Date();
    }

    var ms = [];
    for (var i = d2.getMonth() ; i >= d1.getMonth() ; i--) {
        ms.push({ "Value": "" + (i+1), "Text": getMes(i) });
    }

    return ms;
}

function getMes(numero) {
    switch (numero) {
        case 0:
            return "Enero";
        case 1:
            return "Febrero";
        case 2:
            return "Marzo";
        case 3:
            return "Abril";
        case 4:
            return "Mayo";
        case 5:
            return "Junio";
        case 6:
            return "Julio";
        case 7:
            return "Agosto";
        case 8:
            return "Septiembre";
        case 9:
            return "Octubre";
        case 10:
            return "Noviembre";
        case 11:
            return "Diciembre";
    }
}

function ControlSelectorRangoFecha_IsRangoSeleccionado() {
    return ControlSelectorRangoFecha_GetFechaDesde() != undefined && ControlSelectorRangoFecha_GetFechaHasta() != undefined;
}

function ControlSelectorRangoFecha_IsDatosIngresados() {
    return $("#ControlSelectorRangoFecha_input_FechaDesde").val() != "" || $("#ControlSelectorRangoFecha_input_FechaHasta").val() != "";
}

function ControlSelectorRangoFecha_GetFechaDesde() {
    var fecha = moment($("#ControlSelectorRangoFecha_input_FechaDesde").val(), 'DD/MM/YYYY');
    if (!fecha.isValid()) {
        return undefined;
    }

    return fecha;
}

/*
* Para el mes
*/
function ControlSelectorMes_Validar() {
    var valido = true;

    $('#select_Meses').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#select_Años').siblings('.control-observacion').stop(true, true).slideUp(300);


    //Mes
    var fechaMes = $('#select_Meses').val();
    var fechaAño = $('#select_Años').val();

    if (fechaMes == -1) {
        $('#select_Meses').siblings('.control-observacion').text('Dato requerido');
        $('#select_Meses').siblings('.control-observacion').stop(true, true).slideDown(300);
        valido = false;
    }

    if (fechaAño == -1) {
        $('#select_Años').siblings('.control-observacion').text('Dato requerido');
        $('#select_Años').siblings('.control-observacion').stop(true, true).slideDown(300);
        valido = false;
    }

    return valido;
}
function ControlSelectorMes_Limpiar() {
    $('#select_Meses').val() == -1;
    $('#select_Años').val() == -1;
}
function ControlSelectorCheckboxMes_GetState() {
    var check = true;
    if ($('#check_SoloMes').is(':checked')) { check = true; }
    else {
        check = false;
    }
    return check;
}

function ControlSelectorRangoFecha_GetMes() {

    var value = undefined;

    if ($('#select_Meses').val() != -1) {
        value = $('#select_Meses').val();

    }
    return value;
}
function ControlSelectorRangoFecha_GetAño() {
    var value = undefined;

    if ($('#select_Años').val() != -1) {
        value = $('#select_Años').val();
    }
    return value;
}
function ControlSelectorRangoFecha_GetTextoMes(numeromes) {

    var mescompleto = _.findWhere(meses, { Id: numeromes });
    var textoMes;
    if (numeromes != -1) {
        textoMes = mescompleto.Nombre;
    }
    return textoMes;
}

//-------------------------------------------
function ControlSelectorRangoFecha_GetFechaHasta() {
    var fecha = moment($("#ControlSelectorRangoFecha_input_FechaHasta").val(), 'DD/MM/YYYY');
    if (!fecha.isValid()) {
        return undefined;
    }

    return fecha;
}
function ControlSelectorRangoFecha_Limpiar() {
    $('#ControlSelectorRangoFecha_select_Fecha').val(1).trigger('change');
    $("#ControlSelectorRangoFecha_input_FechaHasta").val('');
    $("#ControlSelectorRangoFecha_input_FechaDesde").val('');
    Materialize.updateTextFields();
}

function ControlSelectorRangoFecha_OcultarPeriodos() {

    $("#filaCheckMes").hide()
    $('#filaEntreFechas').hide();

    $('#check_SoloMes').trigger('click');
}

function ControlSelectorRangoFecha_SetAñoMes(año,mes) {

    $('#select_Años').val(año).trigger('change');
    $('#select_Meses').val(mes).trigger('change');
}

function ControlSelectorRangoFecha_OcultarCheckMes() {
    $("#filaCheckMes").hide() 
}

var index;
function ControlSelectorRangoFecha_SetSelectIndex(index) {
    $('#ControlSelectorRangoFecha_select_Fecha').val(index).trigger('change');

}

//-------------------------------
// Cargando
//-------------------------------

function ControlSelectorRangoFecha_MostrarCargando(mostrar, mensaje) {
    if (ControlSelectorRangoFecha_CallbackCargando == undefined) return;
    ControlSelectorRangoFecha_CallbackCargando(mostrar, mensaje);
}

function ControlSelectorRangoFecha_SetOnCargandoListener(callback) {
    ControlSelectorRangoFecha_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function ControlSelectorRangoFecha_SetOnMensajeListener(callback) {
    ControlSelectorRangoFecha_CallbackMensaje = callback;
}

function ControlSelectorRangoFecha_MostrarMensajeError(tipo, mensaje) {
    if (ControlSelectorRangoFecha_CallbackMensaje == undefined) return;
    ControlSelectorRangoFecha_CallbackMensaje(tipo, mensaje);
}

function ControlSelectorRangoFecha_MostrarMensajeError(mensaje) {
    ControlSelectorRangoFecha_CallbackMensaje('Error', mensaje);
}

function ControlSelectorRangoFecha_MostrarMensajeAlerta(mensaje) {
    ControlSelectorRangoFecha_CallbackMensaje('Alerta', mensaje);
}

function ControlSelectorRangoFecha_MostrarMensajeInfo(mensaje) {
    ControlSelectorRangoFecha_CallbackMensaje('Info', mensaje);
}

function ControlSelectorRangoFecha_MostrarMensajeExito(mensaje) {
    ControlSelectorRangoFecha_CallbackMensaje('Exito', mensaje);
}
