var callbackCargando;

var servicios;

function init(data) {
    data = parse(data);

    servicios = data.Servicios;

    if (servicios == null) {
        mostrarMensaje('Error', 'Error inicializando la pantalla');
    } else {
        $('#select_Servicios').CargarSelect({
            Data: servicios,
            Value: 'Nombre',
            Text: 'Nombre',
            Default: 'Seleccione...',
            Sort: true
        });

        setTimeout(function () {
            mostrarCargando(false);
        }, 600);
    }

    $('#select_Servicios').on('change', function (e) {
        if ($(this).val() == -1) {
            $('#document').fadeOut(3000);
        } else {
            $('#document').fadeOut(100).fadeIn();

            $("#motivos label").text('Motivos de ' + toTitleCase($(this).val()));
            $("#usuarios label").text('Usuarios de ' + toTitleCase($(this).val()));
        }
    });

    $("#motivos").click(function () {
        crearDialogoReporteCatalogoMotivos($('#select_Servicios').val());
    });

    $("#usuarios").click(function () {
        crearDialogoReporteCatalogoUsuarios($('#select_Servicios').val());
    });
}