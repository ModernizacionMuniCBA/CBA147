let callback;

let condiciones;
let idMovil;

function init(data) {
    if ('Error' in data) {
        return;
    }

    condiciones = data.Condiciones;
    idMovil = data.IdMovil;
    idCondicion = data.IdCondicion;

    $('#select_Condicion').CargarSelect({
        Data: condiciones,
        Value: 'KeyValue',
        Text: 'Nombre',
        Default: 'Seleccione...',
        Sort: false
    });

    if(idCondicion!=-1){
        $('#select_Condicion').val(idCondicion).trigger('change');
    }
}

function cambiarCondicion() {
    mostrarCargando(true, 'Editando condición...');
    
    let keyValue = $.grep(condiciones, function (element, index) {
        return element.KeyValue == $('#select_Condicion').val();
    })[0].KeyValue;

    if (keyValue == undefined) {
        mostrarMensaje('Error', 'Error con la condición elegida');
        return;
    }

    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/EditarCondicion'),
        Data: { comando: { IdMovil: idMovil, Condicion: keyValue }},
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok || !result.Return) {
                mostrarMensaje(result.Error);
                return;
            }

            informarEdicionCondicion();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

//-------------------------------
// Listener
//-------------------------------

function setOnListener(c) {
    callback = c;
}

function informarEdicionCondicion() {
    if (callback == undefined) return;
    callback();
}