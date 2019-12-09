let callback;

let idMovil;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idMovil = data.IdMovil;

    //Evento datepicker fecha de UltimoTUV
    $('#botonFechaUltimoTUV').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaUltimoTUV').click();
        e.stopPropagation()
    });

    $('#pickerFechaUltimoTUV').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputFechaUltimoTUV').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputFechaUltimoTUV').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

    //Evento datepicker fecha de vencimiento TUV
    $('#botonFechaVencimientoTUV').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaVencimientoTUV').click();
        e.stopPropagation()
    });

    $('#pickerFechaVencimientoTUV').pickadate({
        // Date limits
        min: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputFechaVencimientoTUV').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputFechaVencimientoTUV').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

}

function editarTUV() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Editando TUV...');
    
    var comando = {};

    comando.IdMovil = idMovil;

    var fechaUltimoTUV = $("#inputFechaUltimoTUV").val();
    var fechaMoment = moment(fechaUltimoTUV, 'DD/MM/YYYY');
    if (fechaMoment.isValid()) {
        comando.fechaUltimoTUV = fechaMoment;
    }

    var fechaVencimientoTUV = $("#inputFechaVencimientoTUV").val();
    var fechaMoment = moment(fechaVencimientoTUV, 'DD/MM/YYYY');
    if (!fechaMoment.isValid()) {
        mostrarMensaje('Error', 'Error con la fecha de Vencimiento del TUV');
        return;
    }
    comando.FechaVencimientoTUV = fechaMoment;
    comando.Observaciones = $('#inputFormulario_Observaciones').val();

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/EditarTUV'),
        Data: { comando: comando},
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok || !result.Return) {
                mostrarMensaje(result.Error);
                return;
            }

            informarEdicionTUV();
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

function informarEdicionTUV() {
    if (callback == undefined) return;
    callback();
}