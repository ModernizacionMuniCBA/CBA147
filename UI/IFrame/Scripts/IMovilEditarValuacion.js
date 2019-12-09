let callback;

let idMovil;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idMovil = data.IdMovil;

    //Evento datepicker fecha de valuacion
    $('#botonFechaValuacion').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaValuacion').click();
        e.stopPropagation()
    });

    $('#pickerFechaValuacion').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
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
}

function editarValuacion() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Editando valuación...');

    var comando = {};

    comando.IdMovil = idMovil;

    comando.Valuacion = parseInt($('#inputFormulario_Valuacion').val());

    var fechaValuacion = $("#inputFechaValuacion").val();
    var fechaMoment = moment(fechaValuacion, 'DD/MM/YYYY');
    if (!fechaMoment.isValid()) {
        mostrarMensaje('Error', 'Error con la fecha de valuación');
        return;
    }

    comando.FechaValuacion = fechaMoment;
    comando.Observaciones = $('#inputFormulario_Observaciones').val();

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/EditarValuacion'),
        Data: { comando: comando },
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok || !result.Return) {
                mostrarMensaje(result.Error);
                return;
            }

            informarEdicionValuacion();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function validar() {
    return $('#formNuevo').valid();
}

//-------------------------------
// Listener
//-------------------------------

function setOnListener(c) {
    callback = c;
}

function informarEdicionValuacion() {
    if (callback == undefined) return;
    callback();
}