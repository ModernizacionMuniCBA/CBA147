let callback;

let idMovil;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idMovil = data.IdMovil;

    //Evento datepicker fecha de reparacion
    $('#botonFechaReparacion').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaReparacion').click();
        e.stopPropagation()
    });

    $('#pickerFechaReparacion').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputFechaReparacion').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputFechaReparacion').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });
}

function agregarReparacion() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Editando reparación...');

    var comando = {};

    comando.IdMovil = idMovil;

    comando.Motivo = $('#inputFormulario_Motivo').val();

    comando.Taller = $('#inputFormulario_Taller').val();

    var monto = $('#inputFormulario_MontoReparacion').val();
    if (monto != "") {
        comando.MontoReparacion = parseInt(monto);
    }

    var fechaReparacion = $("#inputFechaReparacion").val();
    var fechaMoment = moment(fechaReparacion, 'DD/MM/YYYY');
    if (!fechaMoment.isValid()) {
        mostrarMensaje('Error', 'Error con la fecha de valuación');
        return;
    }

    comando.FechaReparacion = fechaMoment;

    comando.Observaciones = $('#inputFormulario_Observaciones').val();

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/AgregarReparacion'),
        Data: { comando: comando },
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok || !result.Return) {
                mostrarMensaje('Error',result.Error);
                return;
            }

            informar();
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

function informar() {
    if (callback == undefined) return;
    callback();
}