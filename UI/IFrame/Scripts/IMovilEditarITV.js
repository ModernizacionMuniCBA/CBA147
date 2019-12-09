let callback;

let idMovil;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idMovil = data.IdMovil;

    //Evento datepicker fecha de UltimoITV
    $('#botonFechaUltimoITV').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaUltimoITV').click();
        e.stopPropagation()
    });

    $('#pickerFechaUltimoITV').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputFechaUltimoITV').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputFechaUltimoITV').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

    //Evento datepicker fecha de vencimiento ITV
    $('#botonFechaVencimientoITV').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#pickerFechaVencimientoITV').click();
        e.stopPropagation()
    });

    $('#pickerFechaVencimientoITV').pickadate({
        // Date limits
        min: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#inputFechaVencimientoITV').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#inputFechaVencimientoITV').val('');
            }

            console.log(value);

            Materialize.updateTextFields();
        }
    });

}

function editarITV() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Editando ITV...');
    
    var comando = {};

    comando.IdMovil = idMovil;

    var fechaUltimoITV = $("#inputFechaUltimoITV").val();
    var fechaMoment = moment(fechaUltimoITV, 'DD/MM/YYYY');
    if (fechaMoment.isValid()) {
        comando.fechaUltimoITV = fechaMoment;
    }

    var fechaVencimientoITV = $("#inputFechaVencimientoITV").val();
    var fechaMoment = moment(fechaVencimientoITV, 'DD/MM/YYYY');
    if (!fechaMoment.isValid()) {
        mostrarMensaje('Error', 'Error con la fecha de Vencimiento del ITV');
        return;
    }
    comando.FechaVencimientoITV = fechaMoment;
    comando.Observaciones = $('#inputFormulario_Observaciones').val();

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/EditarITV'),
        Data: { comando: comando},
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok || !result.Return) {
                mostrarMensaje(result.Error);
                return;
            }

            informarEdicionITV();
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

function informarEdicionITV() {
    if (callback == undefined) return;
    callback();
}