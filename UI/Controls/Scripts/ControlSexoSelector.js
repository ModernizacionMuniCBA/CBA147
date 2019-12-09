var ControlSexoSelector_modoOtro = false;
var ControlSexoSelector_ListenerChange;
var ControlSexoSelector_Requerido = false;
function ControlSexoSelector_init() {

    $('#ControlSexoSelector_select_Sexo').CargarSelect({
        Data: [
            {
                Valor: 1,
                Texto: 'Masculino'
            },
            {
                Valor: 2,
                Texto: 'Femenino'
            },
            //{
            //    Valor: 3,
            //    Texto: 'Otro'
            //}
        ],
        Default: 'Seleccione...',
        Value: 'Valor',
        Text: 'Texto'
    });

    $('#ControlSexoSelector_select_Sexo').on('change', function () {
        var valSeleccionado = $('#ControlSexoSelector_select_Sexo').val();

        ControlSexoSelector_modoOtro = valSeleccionado == 3;
        if (valSeleccionado == 3) {
            $('#ControlSexoSelector_ContenedorSelect').fadeOut(300, function () {
                $('#ControlSexoSelector_ContenedorInput').fadeIn(300);
                $('#ControlSexoSelector_ContenedorInput').trigger('focus');

                $('#ControlSexoSelector_input_SexoOtro').val('');
                Materialize.updateTextFields();
            });
        } else {
            $('#ControlSexoSelector_ContenedorInput').fadeOut(300, function () {
                $('#ControlSexoSelector_ContenedorSelect').fadeIn(300);

                $('#ControlSexoSelector_input_SexoOtro').val('');
                Materialize.updateTextFields();
            });
        }

        ControlSexoSelector_InformarCambio();
        ControlSexoSelector_Validar();
    });

    $('#ControlSexoSelector_input_SexoOtro').on('input', function () {
        ControlSexoSelector_InformarCambio();
        ControlSexoSelector_Validar();
    });

    $('#ControlSexoSelector_BotonCancelar').click(function () {
        $('#ControlSexoSelector_ContenedorInput').fadeOut(300, function () {
            $('#ControlSexoSelector_ContenedorSelect').fadeIn(300);
            $('#ControlSexoSelector_select_Sexo').val('-1').trigger('change');
        });

    });
}

function ControlSexoSelector_GetSexoSeleccionado() {
    if (ControlSexoSelector_modoOtro) {
        var sexoString = $('#ControlSexoSelector_input_SexoOtro').val().trim();
        if (sexoString == "") {
            return null;
        } else {
            return sexoString;
        }
    } else {
        var valSeleccionado = $('#ControlSexoSelector_select_Sexo').val();
        if (valSeleccionado == -1 || valSeleccionado == 3) {
            return null;
        } else {
            return valSeleccionado == '1' ? 'Masculino' : 'Femenino';
        }
    }
}

function ControlSexoSelector_Clear() {
    $('#ControlSexoSelector_select_Sexo').val(-1).trigger('change');
}

function ControlSexoSelector_SetSexo(sexo) {
    var sexoTest = sexo.toLowerCase();
    if (sexoTest == 'masculino' || sexoTest == 'femenino') {
        $('#ControlSexoSelector_select_Sexo').val(sexoTest == 'masculino' ? 1 : 2).trigger('change');
    } else {

        $('#ControlSexoSelector_select_Sexo').val('3').trigger('change');
        setTimeout(function () {
            $('#ControlSexoSelector_input_SexoOtro').val(sexo);
            ControlSexoSelector_InformarCambio();
            ControlSexoSelector_Validar();
            Materialize.updateTextFields();
        }, 350);
    }
}

function ControlSexoSelector_SetRequerido(validar) {
    ControlSexoSelector_Requerido = validar;
}

function ControlSexoSelector_Validar() {
    if (!ControlSexoSelector_Requerido) return true;

    $('#ControlSexoSelector_select_Sexo').parent().find('.input-error').find('a').text('Dato requerido');
    $('#ControlSexoSelector_input_SexoOtro').parent().find('.input-error').find('a').text('Dato requerido');

    var tieneSexo = ControlSexoSelector_GetSexoSeleccionado() != null;

    if (!tieneSexo) {
        $('#ControlSexoSelector_select_Sexo').addClass('error');
        $('#ControlSexoSelector_input_SexoOtro').addClass('error');
    } else {
        $('#ControlSexoSelector_select_Sexo').removeClass('error');
        $('#ControlSexoSelector_input_SexoOtro').removeClass('error');
    }
    return tieneSexo;
}

function ControlSexoSelector_InformarCambio() {
    if (ControlSexoSelector_ListenerChange == undefined) return;
    ControlSexoSelector_ListenerChange(ControlSexoSelector_GetSexoSeleccionado());
}

function ControlSexoSelector_SetOnChangeListener(listener) {
    ControlSexoSelector_ListenerChange = listener;
}