var formulario = "file://Documents/PlanillaUsuarios.docx";
var strings;

function init(data) {
    strings = data.Strings;

    if (data.AlertarAplicacionBloqueada == true) {
        $('#contenedor_Mantenimiento').addClass('visible');
        $('#contenedor-encabezado').hide();
        return;
    }

    initTextos();

    //Errores
    if (data.AlertarServerLocalDbProduccion == true || data.AlertarServerTestDbProduccion == true) {
        $('#body1').addClass('error');
        $('#textoTituloInfoCBA147').text('¡Atencion!')
        $('#textoInfoCBA147').text('¡Esta usando la base de datos de producción en un entorno de test!')
    } else {
        if (data.AlertarServerProduccionDbTest == true) {
            mostrarMensajeCritico({
                Descripcion: 'Detalle: Entorno de test en producción.'
            });
            return;
        }
    }

    //Clicks en botones
    $('#btnRecuperarCuenta').click(function () {
        var url = window.location.href;
        ir(data.UrlRecuperarCuenta + '?redirigir=' + url);
    });

    //Clicks en botones
    $('#btnCrearUsuario').click(function () {
        var win = window.open(data.UrlNuevoUsuario, '_blank');
        if (win) {
            win.focus();
        }
    });

    $('#btnLogin').click(function () {
        iniciarSesion();
    });

    $("#form").submit(function () {
        iniciarSesion();
        return false;
    });


    $('input').on('focus', function () {
        var contenedor = $(this).parents('.input');
        $(contenedor).addClass('focus');
    });

    $('input').on('blur', function () {
        var contenedor = $(this).parents('.input');
        $(contenedor).removeClass('focus');
    });

    //$('#descargarArchivo').click(function(){
    //    descargarFormulario(formulario);
    //});

    //$('#textoFormularioUsuarios').click(function (e) {
    //    e.preventDefault();
    //    var win = window.open('https://goo.gl/forms/7xoffQSLbBgNEdmB2', '_blank');
    //    if (win) {
    //        win.focus();
    //    } else {
    //        Materialize.toast('Por favor habilite los popups para este website', 5000);
    //    }
    //});

    setTimeout(function () {
        $('#contenedor-encabezado').addClass('visible');
        $('#contenedor-body').addClass('visible');
    }, 300);
}

function initTextos() {
    $('#texto_IniciarSesion').html(strings.Texto_IniciarSesion);
    $('#btnRecuperarCuenta').html(strings.Boton_RecuperarCuenta);
    $('#btnLogin').html(strings.Boton_IniciarSesion);
    $('#texto_NoTieneCuenta').html(strings.Texto_NoTieneCuenta);
    $('#btnCrearUsuario').html(strings.Boton_CrearUsuario);
    $('#textoTituloInfoCBA147').html(strings.Texto_InfoCBA147_Titulo);
    $('#textoInfoCBA147').html(strings.Texto_InfoCBA147_Texto);
    $('#btnContacto').html(strings.Boton_Contacto);
    $('#btnPoliticaSeguridad').html(strings.Boton_PoliticaSeguridad);
    $('#btnPoliticaPrivacidad').html(strings.Boton_PoliticaPrivacidad);
    $('#btnTerminosCondiciones').html(strings.Boton_TerminosCondiciones);
}

function iniciarSesion() {
    if (!validar()) {
        return;
    }

    mostrarCargando("Iniciando Sesión...");

    var usuario = $('#input_Username').val();
    var pass = $('#input_Password').val();

    let url = ResolveUrl('~/Servicios/UsuarioService.asmx/IniciarSesion');
    console.log(url);

    crearAjax({
        Url: url,
        Data: { user: usuario, pass: pass },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                Materialize.toast(result.Errores.Mensaje, 5000, 'colorError');
                return;
            }


            if (result.Return.OrigenesDisponibles.length == 1) {
                mostrarCargando(true);

                crearAjax({
                    Url: ResolveUrl('~/Servicios/UsuarioService.asmx/SetOrigen'),
                    Data: { id: result.Return.OrigenesDisponibles[0].Id },
                    OnSuccess: function (result) {
                        mostrarCargando(false);

                        if (!result.Ok) {
                            mostrarMensaje('Error', result.Error);
                            return;
                        }

                        ir(ResolveUrl('~/Sistema?pagina=Inicio'));
                    },
                    OnError: function (result) {
                        mostrarCargando(false);
                        mostrarMensaje('Error', 'Error procesando la solicitud');
                    }
                })

                return;
            }

            $('#login').fadeOut(300, function () {
                $('#contenedor_SelectorOrigen').fadeIn(300);
                $('#contenedor_SelectorOrigen').addClass('visible');


                $.each(result.Return.OrigenesDisponibles, function (index, element) {
                    var item = $('<div>');
                    $(item).addClass('item');
                    var label = $('<label>');
                    $(label).text(element.Nombre);
                    $(label).appendTo(item);

                    $('#contenedor_SelectorOrigen .items').append(item);


                    $(item).click(function () {

                        mostrarCargando(true);

                        crearAjax({
                            Url: ResolveUrl('~/Servicios/UsuarioService.asmx/SetOrigen'),
                            Data: { id: element.Id },
                            OnSuccess: function (result) {
                                mostrarCargando(false);

                                if (!result.Ok) {
                                    mostrarMensaje('Error', result.Error);
                                    return;
                                }

                                $('#contenedor_SelectorOrigen').removeClass('visible');

                                ir(ResolveUrl('~/Sistema?pagina=Inicio'));
                            },
                            OnError: function (result) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', 'Error procesando la solicitud');
                            }
                        })
                    });
                });
            });

        },
        OnError: function (result) {
            mostrarCargando(false);
            Materialize.toast('Error procesando la solicitud', 5000, 'colorError');
        }
    });
}

function validar() {
    var usuario = $('#input_Username').val();
    var pass = $('#input_Password').val();

    var ok = true;
    if (usuario == undefined || usuario == null || usuario == "") {
        ok = false;
    }

    if (pass == undefined || pass == null || pass == "") {
        ok = false;
    }

    return ok;
}

function borrarErroresValidaciones() {

}

function descargarFormulario(archivo) {
    /*
    Archivo: {
        Nombre: Nombre del archivo a descargar,
        Data: contenido del archivo a descargar
    }
    */

    var link = document.createElement('a');
    document.body.appendChild(link);
    link.download = archivo;
    link.href = archivo;
    link.click();
    $(link).remove();
}

