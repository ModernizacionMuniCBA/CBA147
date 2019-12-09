let callback;

let idMovil;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idMovil = data.IdMovil;

    //Evento datepicker fecha de kilometraje
    $('#botonFechaKilometraje').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaKilometraje').click();
        e.stopPropagation()
    });

    $('#pickerFechaKilometraje').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
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

}

function editarKilometraje() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Editando kilometraje...');
    
    var comando = {};

    comando.IdMovil = idMovil;

    comando.Kilometraje = $('#inputFormulario_Kilometraje').val();

    var fechaKilometraje = $("#inputFechaKilometraje").val();
    var fechaMoment = moment(fechaKilometraje, 'DD/MM/YYYY');
    if (!fechaMoment.isValid()) {
        mostrarMensaje('Error', 'Error con la fecha de kilometraje');
        return;
    }
    comando.FechaKilometraje = fechaMoment;
    comando.Observaciones = $('#inputFormulario_Observaciones').val();

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/EditarKilometraje'),
        Data: { comando: comando},
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok || !result.Return) {
                mostrarMensaje(result.Error);
                return;
            }

            informarEdicionKilometraje();
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

function informarEdicionKilometraje() {
    if (callback == undefined) return;
    callback();
}