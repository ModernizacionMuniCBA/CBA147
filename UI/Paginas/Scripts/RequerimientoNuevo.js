var iframe;
var idRequerimiento;
var id;
var numero;
var anio;
var mail;
var tipo = null;

$(document).ready(function () {
    $('.tooltipped').each(function (index, element) {
        var span = $('#' + $(element).attr('data-tooltip-id') + '>span:first-child');
        span.before($(element).attr('data-tooltip'));
        span.remove();
    });

    //Muestro el cargando mientras carga el iframe
    $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(300);

    //En cuando el iframe esta cargado...
    $('#iframe').on('load', function () {

        iframe = $('#iframe')[0].contentWindow;
        $(iframe.document.body).find('#contenedor').scrollTop();

        //Oculto el cargando
        $('#cardFormulario').find('.cargando').stop(true, true).fadeOut(300);

        iframe.setTipoMotivo(tipo);

        //Listener tab
        iframe.setOnTabChangeListener(function (tab) {
            switch (tab) {
                case '#tabDetalle':
                    $('#btnAgregarNota').hide();
                    break;

                case '#tabNotas':
                    $('#btnAgregarNota').show();
                    break;

                case '#tabDocumentos':
                    $('#btnAgregarNota').hide();
                    break;

                case '#tabImagenes':
                    $('#btnAgregarNota').hide();
                    break;
            }
        });

        //Listener Guardar
        iframe.setOnRegistrarCompletoListener(function (rq, usuario, mailEnviado) {
            idRequerimiento = rq.Id;

            //laslalslas asasas
            if (usuario == undefined) {
                $('#btnEnviarEmail').hide();
            } else {
                id = rq.Id;
                numero = rq.Numero;
                anio = rq.Año;
                mail = usuario.Email;
                $('#btnEnviarEmail').show();
            }

            $('#contenedorFormulario').fadeOut(300);
            mostrarOk(rq, usuario, mailEnviado);
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

    $('#cardFormulario').find('.btnOk').click(function () {
        $('#iframe')[0].contentWindow.registrar();
    });

    $('#btnLimpiar').click(function () {
        idRequerimiento = undefined;
        numero = undefined;
        anio = undefined;
        mail = undefined;
        id = undefined;

        //Muestro el cargando mientras carga el iframe
        $('#iframe').attr('src', 'about:blank');
        $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(300);
        $('#iframe').attr('src', ResolveUrl('~/IFrame/IRequerimientoNuevo.aspx'));
    });

    $('#btnCancelar').click(function () {
        redirigir('Inicio.aspx');
    });

    //Al apretar nueva nota
    $('#btnAgregarNota').click(function () {
        iframe.agregarNota();
    });

    //Al apretar neuvo
    $('#btnNuevoRequerimiento').click(function () {
        idRequerimiento = undefined;
        numero = undefined;
        anio = undefined;
        mail = undefined;
        id = undefined;

        //Muestro el cargando mientras carga el iframe
        $('#cardFormulario').find('.cargando').stop(true, true).fadeIn(300);

        $('#contenedorFormulario').fadeIn(300);
        $('#alertaOk').slideUp(500, function () {

            //Muestro el cargando mientras carga el iframe
            $('#cardFormulario').find('.cargando').stop(true, true).fadeOut(300);
        });
        $('#iframe').attr('src', ResolveUrl('~/IFrame/IRequerimientoNuevo.aspx'));
    });

    $('#btnImprimirRequerimiento').click(function () {

        crearDialogoReporteRequerimientoDetalle({
            Id: idRequerimiento
        });

    });

    $('#btnEnviarMail').click(function () {
        if (numero == undefined || anio == undefined || mail == undefined || id == undefined) return;     

        crearDialogoRequerimientoReenviarComprobante({
            Id: id,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            CallbackCargando: function (cargando, mensaje) {
                mostrarCargando(cargando, mensaje);
            }
        })

        ////Enviar mail
        //var dataAjax = { id: id, mail: mail };
        //dataAjax = JSON.stringify(dataAjax);

        //$.ajax({
        //    type: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    dataType: 'json',
        //    url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EnviarComprobanteAtencion'),
        //    data: dataAjax,
        //    traditional: true,
        //    success: function (result) {
        //        mostrarCargando(false);

        //        result = parse(result.d);

        //        if ('Error' in result) {
        //            //callbackMensajes('Error', result.Error.Publico);
        //            console.log("Error mandando mail");
        //            console.log(dataAjax);
        //            console.log(result);
        //            return;
        //        }

        //        //Error
        //        Materialize.toast("Se ha enviado un e-mail a la casilla " + mail + " con el comprobante del requerimiento", 5000, 'colorExito');
        //    },
        //    error: function (result) {
        //        mostrarCargando(false);

        //        //Error
        //        Materialize.toast("No ha podido enviar el comprobante del requerimiento a la casilla " + mail, 5000, 'colorError');
        //    }
        //});
    });
});

function init(data) {
    data = parse(data);

    tipo = data.Tipo;

}

function mostrarOk(requerimiento, usuario, mailEnviado) {
    $('#alertaOk').find('#textoNumeroReclamo').text(requerimiento.Numero + '/' + requerimiento.Año);
    //$('#alertaOk').find('#textoNumeroReclamo').text(requerimiento.Numero);
    if (usuario == undefined) {
        $('#alertaOk').find('.mail').hide();
    } else {
        $('#alertaOk').find('.mail').show();
        if (mailEnviado) {
            $('#alertaOk .mail i').css('color', 'var(--colorExito)');
            $('#alertaOk .mail label').html('Se ha enviado un e-mail a la casilla ' + usuario.Email + ' con el comprobante del requerimiento');
        } else {
            $('#alertaOk .mail i').css('color', 'var(--colorError)');
            $('#alertaOk .mail label').html('No ha podido enviar el comprobante del requerimiento a la casilla ' + usuario.Email);
        }
    }

    $('#alertaOk').slideDown(500);
}