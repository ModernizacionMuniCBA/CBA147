        var iframe;

function init(data) {
    //Muestro el cargando mientras carga el iframe
    $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(300);

    //En cuando el iframe esta cargado...
    $('#iframe').on('load', function () {

        iframe = $('#iframe')[0].contentWindow;
        $(iframe.document.body).find('#contenedor').scrollTop();

        //Oculto el cargando
        $('#cardFormulario').find('.cargando').stop(true,true).fadeOut(300);

        //Listener Guardar
        iframe.setOnRegistrarCompletoListener(function (rq) {
            Materialize.toast('Persona física registrada correctamente', 5000, 'colorExito');

            $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(300);
            $('#iframe').attr('src', ResolveUrl('~/IFrame/IPersonaFisicaNuevo.aspx'));
        });

        //Mensajes
        iframe.setOnMensajeListener(function (tipo, mensaje) {
            switch (tipo) {
                case 'Error':
                    Materialize.toast(mensaje, 5000, 'colorError');
                    break;

                case 'Info':
                    Materialize.toast(mensaje, 5000, 'colorInfo');
                    break;

                case 'Exito':
                    Materialize.toast(mensaje, 5000, 'colorExito');
                    break;

                case 'Alerta':
                    Materialize.toast(mensaje, 5000, 'colorAlerta');
                    break;
            }
        });

        //Cargando
        iframe.setOnCargandoListener(function (cargando, mensaje) {
            if (cargando) {
                $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(300);
            } else {
                $('#cardFormulario').find('.cargando').stop(true, true).fadeOut(300);
            }
        });
    });

    //Al apretar registrar
    $('#cardFormulario').find('.btnOk').click(function () {
        $('#iframe')[0].contentWindow.registrar();
    });

}