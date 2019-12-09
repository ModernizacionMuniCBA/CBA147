
$(function () {

    $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(300);

    $('#iframeRegistrar').on('load', function () {

        //Muestro cargando mientras el iframe carga
        $('#cardFormulario').find('.cargando').stop(true,true).fadeOut(300);

        //Callback de servicio editado
        $('#iframeRegistrar')[0].contentWindow.setOnRegistrarCompletoListener(function (servicio) {
            mostrarMensaje('Exito','Móvil registrado correctamente');
        });

        //Callback de mensajes
        $('#iframeRegistrar')[0].contentWindow.setOnMensajeListener(function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        })

        //Callback cargando
        $('#iframeRegistrar')[0].contentWindow.setOnCargandoListener(function (cargando, mensaje) {
            if (cargando) {
                $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(300);
            }
            else {
                $('#cardFormulario').find('.cargando').stop(true, true).fadeOut(300);
            }
        })
    });

    //Al apretar OK
    $('#cardFormulario').find('.contenedor-footer').find('.btnOk').click(function () {
        $('#iframeRegistrar')[0].contentWindow.registrar();
    });

});

function mostrarMensaje(tipo, mensaje) {

    switch (tipo) {
        case 'Info':
            Materialize.toast(mensaje, 5000);
            break;

        case 'Alerta':
            Materialize.toast(mensaje, 5000, 'colorAlerta');
            break;

        case 'Error':
            Materialize.toast(mensaje, 5000, 'colorError');
            break;

        case 'Exito':
            Materialize.toast(mensaje, 5000, 'colorExito');
            break;
    }
}