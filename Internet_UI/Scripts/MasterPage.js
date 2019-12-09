let usuario;
let geoApiInfo;

$(function () {
    initUsuario();

    $('.menuItem').click(function () {
        var url = $(this).find('.url').attr('url');
        redirigir(ResolveUrl('~/' + url));
    });

    var url = window.location.href.split('/');
    url = url[url.length - 1];
    if (url == undefined) {
        url = 'Inicio';
        redirigir(ResolveUrl('~/Inicio'));
        return;
    }
    $('.url[url=' + url + ']').parents('.menuItem').addClass('menuItemSeleccionado');

    $('#contenedor-encabezado .usuario').click(function (e) {
        $('#contenedor-encabezado .usuario').MenuFlotante({
            MarginTop: $('#contenedor-encabezado .usuario').height() + 13,
            Menu: [
                {
                    Texto: 'Soporte',
                    Icono: 'help',
                    OnClick: function () {
                        crearDialogoContacto();
                    }
                },
                {
                    Texto: 'Editar Perfil',
                    Icono: 'account_circle',
                    OnClick: function () {
                        var url = urlEditarUsuario + token;
                        $('#dialogoUsuario .cargando').show();
                        $('#dialogoUsuario iframe').attr('src', url);

                        $('#dialogoUsuario').addClass('visible');
                    }
                },
                {
                    Separador: true,
                },
                {
                    Texto: 'Cerrar sesión',
                    Icono: 'exit_to_app',
                    OnClick: function () {
                        redirigir(ResolveUrl('~/CerrarSesion'));
                    }
                }
            ]
        });
    });
    $('.dialogo .fondo').click(function (e) {
        e.stopPropagation();
        console.log('alala');
    });

    $('.dialogo .btn, .dialogo .btn-flotante ').click(function () {
        $(this).parents('.dialogo').removeClass('visible')
    });

    $('#dialogoUsuario .btn, #dialogoUsuario .btn-flotante').click(function () {
        mostrarCargando();
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/UsuarioActualizado'),
            OnSuccess: function (result) {
                redirigir(window.location.href);
            },
            OnError: function (result) {
                redirigir(window.location.href);
            }
        });
    });

    $('.dialogo iframe').on('load', function () {
        $('.dialogo .cargando').stop(false, false).fadeOut(300);
    })

    $('.contenedor_BotonApps').click(function (e) {
        $('.contenedor_BotonApps').MenuFlotante({
            MarginTop:  $('.contenedor_BotonApps').height() + 13,
            Menu: [
                {
                    Texto: 'Soporte',
                    Icono: 'help',
                    OnClick: function () {
                        crearDialogoContacto();
                    }
                },
                {
                    Texto: 'Editar Perfil',
                    Icono: 'account_circle',
                    OnClick: function () {
                        var url = urlEditarUsuario + token;
                        $('#dialogoUsuario .cargando').show();
                        $('#dialogoUsuario iframe').attr('src', url);

                        $('#dialogoUsuario').addClass('visible');
                    }
                },
                {
                    Separador: true,
                },
                {
                    Texto: 'Cerrar sesión',
                    Icono: 'exit_to_app',
                    OnClick: function () {
                        redirigir(ResolveUrl('~/CerrarSesion'));
                    }
                }
            ]
        });
    });


    var menuVisible = false;
    $('#menuMobile').click(function () {
        if (!menuVisible) {
            $('#contenedorMenu').slideDown(300);
        } else {
            $('#contenedorMenu').slideUp(300);
        }

        menuVisible = !menuVisible;
    });

    $(window).on('resize', function () {
        menuVisible = false;
        if ($(window).width() > '768') {
            $('#contenedorMenu').css('display', 'flex');
        } else {
            $('#contenedorMenu').css('display', 'none');
        }
    });

});

function initUsuario() {
    crearAjax({
        Url: '~/Servicios/ServicioUsuario.asmx/GetDatosUsuario',
        OnSuccess: function (result) {
            if (!result.Ok) {
                return;
            }

            usuario = result.Return;
        }
    });
}

