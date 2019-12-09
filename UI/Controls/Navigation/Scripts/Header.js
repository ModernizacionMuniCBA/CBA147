let info;
let menuItem;
let primeraPaginaCargada = false;

let PAGINA_ERROR;
let PAGINA_NO_ENCONTRADA;
let URL_MIPERFIL;

function initHeader(data) {

    info = data;
    URL_MIPERFIL = data.url_miperfil;

    PAGINA_ERROR = {
        Titulo: 'Error',
        Url: 'Error'
    };

    PAGINA_NO_ENCONTRADA = {
        Titulo: 'Página no encontrada',
        Url: 'NoEncontrado'
    };

    //Inicializo el menu
    header_InitDrawer();
    header_InitDrawerFooter();
    header_InitDatosUsuario();
    header_InitIndicadores();
    header_InitActionButtons();
    header_InitContenedorPagina();
    header_InitAbrirPrimerSubmenu();
}


//-------------------------------------------
// Drawer
//-------------------------------------------

function header_InitDrawer() {
    //Init drawer
    $(".button-collapse").sideNav();

    //Pongo el drawer en la forma que fue guardado
    setDrawerExpandido(isDrawerExpandido(), false);

    //Resalto la pagina en el menu
    header_ResaltarPaginaActualEnMenu();

    //Click en algun item del menu
    $('#header_contenedorMenu .url').click(function (ev) {
        ev.preventDefault();

        //Si ya es el seleccionado, no hago nada
        if ($(this).parent().hasClass('menuItemSeleccionado')) {
            return false;
        }

        //Si no tiene permiso esta pagina aviso
        if ($(this).hasClass('sinPermiso')) {
            Materialize.toast('No tiene permiso para acceder a esta pagina', 5000);
            return;
        }

        //Hack fix
        $('#sidenav-overlay').trigger('click');

        //Obtengo los datos del item presionado
        var url = $(this).attr('url');
        let menuItem = $(this).parents('.menuItem')[0];

        //Obtengo los datos de la pagina a la que voy a ir y redirijo
        let paginaNueva = header_BuscarInfoPaginaByValor(url);
        header_CambiarPagina(paginaNueva);
    });

    //Toggle drawer expandido
    $('#btnExpandirDrawer').click(function () {
        if (isDrawerExpandido() == true) {
            setDrawerExpandido(false, true);
        } else {
            setDrawerExpandido(true, true);
        }
    });

    //Al cambiar el tamaño de la pantalla arreglo el drawer
    $(window).resize(function () {
        var windowsWidth = $(window).width();

        if (windowsWidth >= 768) {
            return;
        }

        var expandido = isDrawerExpandido();
        if (expandido) {
            setDrawerExpandido(false, false);
        }
    });
}

function header_InitDrawerFooter() {
    $('#idContenedorContacto').click(function () {
        crearDialogoContacto({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje)
            }
        });
    })

    //Version del sistema
    $('#texto_Version').text(info.Version);
}

function header_InitDatosUsuario() {

    $('#header_Toolbar_Usuario').click(function (e) {

        var user = getUsuarioLogeado();

        let nombre = user.Usuario.Nombre + ' ' + user.Usuario.Apellido;
        let rol = '<b>Rol: </b>' + getUsuarioLogeado().Rol.Rol;

        let ambito;
        if (user.Ambito == undefined || user.Ambito.KeyValue == 0) {
            ambito = '<b>Ámbito: </b> Municipalidad de Córdoba';
        } else {
            ambito = '<b>Ámbito: </b>' + user.Ambito.Nombre;
        }

        let area = '<b>Área/s: </b>';
        area += user.Areas[0].Nombre;
        if (user.Areas.length > 1) {
            area += ' y ' + user.Areas.length + ' más'
        }

        let origen = '';
        if (user.OrigenesDisponibles != undefined && user.OrigenesDisponibles.length > 0) {
            var origenEntity = $.grep(user.OrigenesDisponibles, function (element, index) {
                return element.Id == user.IdOrigenElegido;
            });
            if (origenEntity != undefined && origenEntity.length > 0) {
                origen = '<b>Origen: </b>' + origenEntity[0].Nombre;
            }
        }

        //Foto
        let identificador;
        if (getUsuarioLogeado().Usuario.IdentificadorFotoPersonal != undefined) {
            identificador = getUsuarioLogeado().Usuario.IdentificadorFotoPersonal;
        } else {
            if (getUsuarioLogeado().Usuario.SexoMasculino == true) {
                identificador = top.identificadorFotoUserMale;
            } else {
                identificador = top.identificadorFotoUserFemale;
            }
        }
        let foto = 'url(' + top.urlCordobaFiles + '/Archivo/' + identificador + '/3)';

        //Menu ususario
        var menu = [
            {
                Height: 90,
                CustomView: '<div class="popup_usuario"><div class="imagen" style="background-image:' + foto + '"></div><div class="textos"><label class="nombre">' + nombre + '</label><label class="rol">' + rol + '</label><label class="ambito">' + ambito + '</label><label class="areas">' + area + '</label><label class="origen">' + origen + '</label></div></div>'
            },
            {
                Icono: 'info_outline',
                Texto: 'Información de usuario',
                OnClick: function () {
                    redirigir(URL_MIPERFIL + usuarioLogeado.Token);

                    //crearDialogoUsuarioDetalle({
                    //    Id: getUsuarioLogeado().Usuario.Id
                    //});
                }
            }
        ];

        //Áreas del usuario
        if (getUsuarioLogeado().Areas.length > 1) {
            menu.push({
                Icono: 'account_balance',
                Texto: 'Áreas',
                OnClick: function (data) {

                    let areas = '';
                    $.each(user.Areas, function (i, area) {
                        if (i != 0 && i < user.Areas.length)
                            areas += '</br>';

                        areas += area.Nombre;
                    });

                    crearDialogoHTML({
                        Titulo: '<label>Áreas</label>',
                        Content:
                            '<div class="padding"    style=" height: 25rem;overflow-y: scroll;">' +
                            '<label >' + areas + ' </label>' +
                            '</div>',

                        Botones:
                            [
                                {
                                    Id: 'btnNo',
                                    Texto: 'Volver'
                                }]
                    });
                }
            });
        }

        //Cambiar origen
        if (getUsuarioLogeado().OrigenesDisponibles.length > 1) {
            menu.push({
                Icono: 'settings',
                Texto: 'Seleccionar origen',
                OnClick: function () {

                    var menuOrigen = [];
                    $.each(user.OrigenesDisponibles, function (index, element) {
                        menuOrigen.push({
                            Texto: toTitleCase(element.Nombre),
                            OnClick: function () {
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

                                        getUsuarioLogeado().IdOrigenElegido = element.Id;
                                    },
                                    OnError: function (result) {
                                        mostrarCargando(false);
                                        mostrarMensaje('Error', 'Error cambiando el origen');
                                    }
                                })
                            }
                        });
                    });

                    $('#header_Toolbar_Usuario').MenuFlotante({
                        PosicionX: 'izquierda',
                        PosicionY: 'abajo',
                        Menu: menuOrigen
                    })
                }
            });
        }

        //Actualizar datos cerrojo
        if ($.url().attr('source').indexOf('localhost') != -1) {
            menu.push({
                Icono: 'settings',
                Texto: 'Actualizar datos Vecino Virtual',
                OnClick: function () {
                    mostrarCargando(true);
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/UsuarioService.asmx/ActualizarDatosCerrojo'),
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarCargando(false);
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            location.reload();
                        },
                        OnError: function (result) {
                            mostrarCargando(false);
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            });
        }

        //Cerrar sesion
        menu.push({
            Icono: 'exit_to_app',
            Texto: 'Cerrar Sesión',
            OnClick: function () {
                redirigir(ResolveUrl('~/CerrarSesion'));
            }
        });

        $('#header_Toolbar_Usuario').MenuFlotante({
            PosicionX: 'izquierda',
            PosicionY: 'abajo',
            Width: 300,
            Menu: menu
        })
    });

    header_CargarDatosUsuario();
}

function header_CargarDatosUsuario() {
    //Foto
    let identificador;
    if (getUsuarioLogeado().Usuario.IdentificadorFotoPersonal != undefined) {
        identificador = getUsuarioLogeado().Usuario.IdentificadorFotoPersonal;
    } else {
        if (getUsuarioLogeado().Usuario.SexoMasculino == true) {
            identificador = top.identificadorFotoUserMale;
        } else {
            identificador = top.identificadorFotoUserFemale;
        }
    }
    $('#header_Toolbar_Usuario').css('background-image', 'url(' + top.urlCordobaFiles + '/Archivo/' + identificador + '/3)');
}

function header_InitAbrirPrimerSubmenu() {
    if (menuItem !== undefined && menuItem.Url === 'Inicio' && $('#header_contenedorMenu .subMenu').length > 0) {
        var submenu = $('#header_contenedorMenu .subMenu').first();
        var submenuHeader = submenu.find('.collapsible-header').first();
        var submenuBody = submenu.find('.collapsible-body').first();

        submenu.addClass('active');
        submenuHeader.addClass('active');
        submenuBody.css('display', 'block');
    }
}


//-------------------------------------------
// Cambio pagina
//-------------------------------------------

function header_InitContenedorPagina() {
    $(window).bind("popstate", function (e) {
        header_CambiarPagina(e.originalEvent.state);
    });


    //Obtengo la pagina que inserto el usuario y lo llevo ahi
    var paginaInicial = $.url().param('pagina');
    if (paginaInicial == undefined) {
        paginaInicial = 'Inicio';
    }

    let nuevaPagina = header_BuscarInfoPaginaByValor(paginaInicial);
    if (nuevaPagina == undefined) {
        nuevaPagina = {
            Url: 'Inicio',
            Titulo: 'Inicio'
        }
    }

    let paramsQuery = $.url().data.attr.query.split('?');
    if (paramsQuery.length > 1) {
        nuevaPagina.Params = paramsQuery[1];
    }

    header_CambiarPagina(nuevaPagina);
}

function header_OnPaginaInit() {
    var iframe = $('#iframePagina')[0].contentWindow;

    if (!primeraPaginaCargada) {
        primeraPaginaCargada = true;
        $(window).trigger('first_page_load');
    }

    //Seteo el titulo
    setTitulo('Módulo de Gestión Operativa | ' + menuItem.Titulo);
    //document.title = '#CBA147 - ' + menuItem.Titulo;
    document.title = '#CBA147 | Módulo de Gestión Operativa';

    $('#iframePagina').stop(true, true).animate({ opacity: 1, top: '0' }, 500);
    $('#header_textoToolbar_Titulo').stop(true, true).animate({ opacity: 1 }, 300);


    //Callback Mensajes
    iframe.setCallbackMensajes(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    })

    //Callback cargando
    iframe.setCallbackCargando(function (cargando, mensaje) {
        overlay({ Mostrar: cargando, Texto: mensaje });
    });

    //Callback Drawer
    iframe.setCallbackExpandirDrawer(function () {
        setDrawerExpandido(true, true);
    });

    //Callback Redirigir
    iframe.setCallbackRedirigir(function (pagina) {
        let paginaNueva = header_BuscarInfoPaginaByValor(pagina);
        header_CambiarPagina(paginaNueva);
    });

    //Callback Permisos
    iframe.setCallbackPermisos(function (tipo, pagina) {
        return validarPermiso(tipo, pagina);
    });

    //Callback Version Sistema
    iframe.setCallbackCambioVersionSistema(function () {
        crearAjax({
            Url: ResolveUrl('~/Servicios/VersionSistemaService.asmx/GetVersion'),
            OnSuccess: function (result) {
                if (!result.Ok) {
                    $('#texto_Version').text('*Error');
                    return;
                }
                $('#texto_Version').text(result.Return);
            },
            OnError: function (result) {
                $('#texto_Version').text('*Error');
            }
        });
    });

    //Data
    iframe.setCrearOrdenEspecial(crearOrdenEspecial);
    iframe.setInitData(getInitData());
    iframe.setUsuarioLogeado(getUsuarioLogeado());
}

function header_CambiarPagina(nuevoItem) {
    if (nuevoItem == undefined) return;


    //Valido permiso de consulta en la pagina
    if (nuevoItem.Url.indexOf("Error") == -1) {
        let conPermiso = validarPermisoConsulta(nuevoItem.Url);
        if (!conPermiso) {
            nuevoItem = {
                Url: PAGINA_ERROR.Url,
                Titulo: PAGINA_ERROR.Titulo,
                Params: 'mensaje=No tiene permisos para acceder a la página solicitada&icono=security&icono_color=var(--colorError)'
            };
        }
    }

    //Guardo el item
    menuItem = nuevoItem;

    //Resalto el item
    header_ResaltarPagina(menuItem);

    let urlNueva = menuItem.Url;

    if (urlNueva != 'BusquedaGlobal') {
        $('#toolbar_InputBusqueda').val('');
    }


    if ('Params' in nuevoItem && nuevoItem.Params != undefined) {
        urlNueva += '?' + menuItem.Params;
    }

    //Oculto el titulo
    $('#header_textoToolbar_Titulo').stop(true, true).animate({ opacity: 0 }, 300, function () {
        setTitulo('');
    });

    //Quito titulo de la tab
    document.title = '#CBA147';

    //Oculto el contenido
    $('#iframePagina').stop(true, true).animate({ opacity: 0, top: '200px' }, 300, function () {
        //Cargo el contenido de la pagina en 'main'
        let url = ResolveUrl('~/' + urlNueva);
        $('#iframePagina')[0].contentWindow.location.replace(url);
    });

    //Evento
    $(window).trigger('page_change', { Pagina: nuevoItem });

    //Historial
    history.pushState(nuevoItem, nuevoItem.Titulo, "?pagina=" + urlNueva);
}

function header_ResaltarPagina(item) {
    if (item == undefined) return;
    var url = item.Url;

    //Cierro todos los menus
    $('#header_contenedorMenu li').find('.collapsible-header.active').trigger('click');
    $('#header_contenedorMenu .menuItem').removeClass('menuItemSeleccionado');

    //Itero los menu, para ver cual es el seleccionado
    $('#header_contenedorMenu .menuItem').each(function () {
        var item = $(this).children().get(0);
        var itemUrl = $(item).attr("url");

        //si el url del item es igfual a la actual, le agrego la clase que lo resalta
        if (url == itemUrl) {
            $(this).addClass("menuItemSeleccionado");

            //Expando los acordeones
            var subMenu = $(item).parents('.subMenu');
            $(subMenu).find('> .collapsible-header:not(".active")').trigger('click');
        }
    });
}

function header_BuscarInfoPaginaByValor(valor) {
    if (valor == undefined) return;

    //Corrijo la url
    if (valor.indexOf('?') != -1) {
        valor = valor.split('?')[0];
    }

    //Si estoy intentando redireccionar a 'Error', simplemente lo mando ahi.
    if (valor == 'Error') {
        return {
            Url: PAGINA_ERROR.Url,
            Titulo: PAGINA_ERROR.Titulo,
            Params: valor.Params
        };
    }

    //Busco la pagina dentro de los objectos de Cerrojo
    let pagina = $.grep(getUsuarioLogeado().Rol.Objetos, function (element, index) {
        return element.Valor == valor;
    })[0];
    if (pagina == undefined) {
        return {
            Url: PAGINA_ERROR.Url,
            Titulo: 'Página no encontrada',
            Params: 'mensaje=No se encontró la página solicitada&icono_color=var(--colorAlerta)'
        };
    }

    //Creo la pagina nueva
    pagina = {
        Titulo: pagina.Titulo,
        Url: pagina.Valor
    };

    return pagina;
}

function header_GetPaginaActiva() {
    return menuItem;
}


//-------------------------------------------
// Toolbar
//-------------------------------------------

var ajaxNotificaciones;
var ajaxRequerimientosFavoritos;

function header_InitActionButtons() {
    //Paneles flotantes
    var hToolbar = $('#header_Toolbar').height();
    $('.panelFlotante .contenido').css('margin-top', 'calc(' + hToolbar + 'px + 0.5rem)');
    $('.panelFlotante .contenido').css('margin-right', '0.5rem');

    //Init cada boton
    header_InitActionButton_Inicio();
    header_InitActionButton_Buscar();
    //header_InitActionButton_Notificaciones();
    header_InitActionButton_MisTrabajos();
    header_InitActionButton_RequerimientosFavoritos();

    //Tecla escape
    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            if ($('#header_ContenedorNotificacionesParaUsuario').hasClass('visible')) {
                $('#header_ContenedorNotificacionesParaUsuario .fondo').trigger('click');
                e.stopPropagation();
                return;
            }
        }
    });

    $(window).on('page_change', function (evt, data) {
        let page = data.Pagina;

        //Busqueda
        if (page.Url.indexOf('BusquedaGlobal') != -1) {
            $('#header_ContenedorBtnBusquedaGlobal').hide(300);
        } else {
            $('#header_ContenedorBtnBusquedaGlobal').show(300);
        }
        $('nav').removeClass('busqueda');
    });
}

function header_InitActionButton_Inicio() {
    $('#header_Toolbar_ContenedorBtnInicio').click(function () {

        //Busco la pagina a cambiar
        let pagina = header_BuscarInfoPaginaByValor('Inicio');

        //Si ya esta abierta no hago nada
        if (menuItem != undefined && pagina.Url == menuItem.Url) {
            mostrarMensaje('Info', 'Ya se encuentra en la página de inicio');
            return;
        }

        //Cambio de pagina
        header_CambiarPagina(pagina);
    });
}

function header_InitActionButton_Buscar() {

    $('#toolbar_InputBusqueda').on('click', function () {
        header_SetModoBusqueda(true);

        //Cargo las sugerencias
        let sugerencias = localStorage.getItem('sugerenciasBusqueda');
        if (sugerencias == undefined || sugerencias == 'undefined') sugerencias = "[]";
        sugerencias = JSON.parse(sugerencias);

        $('#toolbar_ContenedorSugerenciasBusqueda .sugerencias').empty();
        $('.sugerencias').width($('#header_Toolbar_Busqueda').width());
        $('.sugerencias').css('left', $('#toolbar_InputBusqueda')[0].getBoundingClientRect().left);


        $.each(sugerencias, function (index, element) {
            let sugerencia = $($('#template_SugerenciaBusqueda').html());
            $(sugerencia).find('.texto').text(element);
            $('#toolbar_ContenedorSugerenciasBusqueda .sugerencias').append(sugerencia);

            $(sugerencia).click(function () {
                buscar(element);
            });

            $(sugerencia).find('a').click(function (e) {
                e.stopPropagation();

                sugerencias = $.grep(sugerencias, function (s) {
                    return s != element;
                });
                localStorage.setItem('sugerenciasBusqueda', JSON.stringify(sugerencias));
                $(sugerencia).remove();
            });
        });
    });

    $('#toolbar_ContenedorSugerenciasBusqueda .fondo').on('click', function () {
        header_SetModoBusqueda(false);
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            header_SetModoBusqueda(false);
            return;
        }
    });

    $('#toolbar_InputBusqueda').keyup(function (e) {
        //Escape
        if (e.keyCode == 27) {
            header_SetModoBusqueda(false);
            return;
        }

        //Enter
        if (e.keyCode == 13) {
            let busqueda = $('#toolbar_InputBusqueda').val();
            buscar(busqueda);
        }
    });

    $('#toolbar_BtnCancelarBusqueda').click(function () {
        header_SetModoBusqueda(false);
    });

    function buscar(busqueda) {
        busqueda = busqueda.trim();

        //Si es vacio el input, cierro
        if (busqueda == "") {
            header_SetModoBusqueda(false);
            return;
        }





        //Guardo la sugerencia
        let sugerencias = localStorage.getItem('sugerenciasBusqueda');
        if (sugerencias == undefined || sugerencias == 'undefined') {
            sugerencias = [];
        } else {
            sugerencias = JSON.parse(sugerencias);
        }
        if (sugerencias.indexOf(busqueda) == -1) {
            sugerencias.unshift(busqueda);
        } else {
            sugerencias = $.grep(sugerencias, function (element, index) {
                return element != busqueda;
            });
            sugerencias.unshift(busqueda);
        }
        if (sugerencias.length > 10) {
            sugerencias = sugerencias.slice(0, 10)
        }
        localStorage.setItem('sugerenciasBusqueda', JSON.stringify(sugerencias));

        //Oculto las sugerencias
        header_SetModoBusqueda(false);
        $('#toolbar_InputBusqueda').val(busqueda);


        //Valido si es un numero de requerimiento

        let numeroRequerimiento = validarNumeroRequerimiento(busqueda);
        if (numeroRequerimiento != undefined) {
            buscarPorNumero(numeroRequerimiento)
                .then(function (ids) {
                    if (ids.length == 0 && numeroRequerimiento[1] != undefined) {
                        top.mostrarMensaje('Error', 'El requerimiento solicitado no existe');
                        return;
                    }

                    if (ids.length != 1) {
                        redirigirBuscar(busqueda);
                        return;
                    }

                    $('#toolbar_InputBusqueda').val('');
                    crearDialogoRequerimientoDetalle({
                        Id: ids[0]
                    });
                })
                .catch(function (error) {
                    top.mostrarMensaje('Error', error);
                });
            return;
        }

        redirigirBuscar(busqueda);
    }

    function validarNumeroRequerimiento(busqueda) {
        busqueda = busqueda.trim();

        if (busqueda.indexOf('/') != -1) {
            let numero = busqueda.split('/')[0].trim();
            let año = parseInt(busqueda.split('/')[1].trim());
            if (numero.length == 6 && año != NaN && año >= 1000) {
                return [numero, año];
            }
        } else {
            if (busqueda.length == 6) {
                return [busqueda, undefined];
            }
        }
        return undefined;
    }

    function redirigirBuscar(busqueda) {
        $(document).trigger('busqueda', [{ input: busqueda }]);

        //Busco la pagina a cambiar
        let pagina = $.grep(getUsuarioLogeado().Rol.Objetos, function (element, index) {
            return element.Valor == 'BusquedaGlobal';
        })[0];
        if (pagina == undefined) return;

        //Si ya esta abierta no hago nada
        if (menuItem != undefined && pagina.Valor == menuItem.Url) return;

        //Cambio de pagina
        header_CambiarPagina({
            Titulo: pagina.Titulo,
            Url: pagina.Valor,
            Params: 'input=' + busqueda
        });
    }

    function buscarPorNumero(busqueda) {

        let numero = busqueda[0];
        let año = null;
        if (busqueda[1] != undefined) {
            año = busqueda[1];
        }
        return new Promise(function (callback, callbackError) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetIdsByNumero'),
                Data: { numero: numero, año: año },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        callbackError(result.Error);
                        return;
                    }
                    callback(result.Return);
                },
                OnError: function (result) {
                    callbackError('Error procesando la solicitud');
                }
            });
        });
    }
}

function header_SetModoBusqueda(valor) {
    if (valor == true) {
        $('#toolbar_InputBusqueda').trigger('focus');
        $('#header_Toolbar').addClass('busqueda');
        $('#toolbar_ContenedorSugerenciasBusqueda').addClass('visible');
    } else {
        $('#toolbar_InputBusqueda').val('');
        $('#toolbar_InputBusqueda').trigger('blur');
        $('#header_Toolbar').removeClass('busqueda');
        $('#toolbar_ContenedorSugerenciasBusqueda').removeClass('visible');

        $(document).trigger('busqueda_cancelada');

    }
}

function header_InitActionButton_MisTrabajos() {
    if (top.esAmbitoCPC()) {
        $('#header_Toolbar_BtnMisTrabajos').hide();
        return;
    }

    $('#header_Toolbar_BtnMisTrabajos').click(function () {
        //Busco la pagina a cambiar
        let pagina = header_BuscarInfoPaginaByValor('MisTrabajos');

        //Si ya esta abierta no hago nada
        if (menuItem != undefined && pagina.Url == menuItem.Url) {
            mostrarMensaje('Info', 'Ya se encuentra viendo Mis Trabajos');
            return;
        }

        //Cambio de pagina
        header_CambiarPagina(pagina);
    });

    header_BuscarCantidadMisTrabajos();
}

function header_InitActionButton_Notificaciones() {
    //Buscar Notificaciones (despues de que se cargo la pagina)
    $(window).on('first_page_load', function () {
        header_BuscarCantidadNotificaciones();
        setInterval(function () {
            header_BuscarCantidadNotificaciones();
        }, 30000);
    });

    //Abrir panel notificaciones
    $('#header_Toolbar_BtnNotificacionesParaUsuario').click(function () {
        var hToolbar = $('#header_Toolbar').height();

        $('#header_ContenedorNotificacionesParaUsuario .contenido').css('margin-top', 'calc(' + hToolbar + 'px + 0.5rem)');
        $('#header_ContenedorNotificacionesParaUsuario .contenido').css('margin-right', '0.5rem');

        $('#header_ContenedorNotificacionesParaUsuario').addClass('visible');

        $('#header_ContenedorNotificacionesParaUsuario .items .content').empty();

        //Muestro el cargando y oculto el indicador vacio
        $('#header_ContenedorNotificacionesParaUsuario .indicadorCargando').addClass('visible');
        $('#header_ContenedorNotificacionesParaUsuario .items').removeClass('visible');
        $('#header_ContenedorNotificacionesParaUsuario .indicadorVacio').removeClass('visible');

        //Mando a buscar
        if (ajaxNotificaciones != undefined) {
            ajaxNotificaciones.abort()
            ajaxNotificaciones = undefined;
        }
        ajaxNotificaciones = crearAjax({
            Url: ResolveUrl('~/Servicios/NotificacionParaUsuarioService.asmx/Get'),
            OnSuccess: function (result) {

                //Oculto el cargando, el indicador vacio e items
                $('#header_ContenedorNotificacionesParaUsuario .indicadorCargando').removeClass('visible');
                $('#header_ContenedorNotificacionesParaUsuario .indicadorVacio').removeClass('visible');
                $('#header_ContenedorNotificacionesParaUsuario .items').removeClass('visible');

                if (!result.Ok) {
                    //Oculto el Contador
                    $('#header_Toolbar_TextoCantidadNotificaciones').removeClass('visible');

                    //Informo el error
                    mostrarMensaje('Error', result.Error);

                    //Invoco el cerrar el panel
                    $('#header_ContenedorNotificacionesParaUsuario .fondo').trigger('click');
                    return;
                }


                var notificacionesNoLeidas = [];
                var notificacionesLeidas = [];

                var leidas = localStorage.getItem("notificacionesLeidas");
                if (leidas == undefined || leidas == "" || leidas == "undefined") {
                    leidas = {};
                } else {
                    leidas = JSON.parse(leidas);
                }

                if (leidas[getUsuarioLogeado().Usuario.Id] == undefined) leidas[getUsuarioLogeado().Usuario.Id] = {};

                var algunaLeida = false;
                $.each(result.Return, function (index, element) {
                    var leida = leidas[getUsuarioLogeado().Usuario.Id][element.Id];

                    if (leida == undefined || leida == false) {
                        notificacionesNoLeidas.push(element);
                    } else {
                        notificacionesLeidas.push(element);
                        algunaLeida = true;
                    }
                });


                //Si tengo uno solo
                if (result.Return.length == 0) {
                    //Oculto el contador
                    $('#header_Toolbar_TextoCantidadNotificaciones').removeClass('visible');

                    //Muestro el indicador vacio
                    $('#header_ContenedorNotificacionesParaUsuario .indicadorVacio').addClass('visible');

                    //Oculto los items
                    $('#header_ContenedorNotificacionesParaUsuario .items').removeClass('visible');
                }
                else {
                    //Muestro el contador
                    $('#header_Toolbar_TextoCantidadNotificaciones').addClass('visible');
                    $('#header_Toolbar_TextoCantidadNotificaciones').text(result.Return.length);

                    //Oculto el vacio
                    $('#header_ContenedorNotificacionesParaUsuario .indicadorVacio').removeClass('visible');

                    //Muestro los items
                    $('#header_ContenedorNotificacionesParaUsuario .items').addClass('visible');
                }

                if (notificacionesLeidas.length != 0 && notificacionesNoLeidas.length != 0) {
                    var divTemplate = $('<label>');
                    $(divTemplate).text('Nuevas')
                    $(divTemplate).addClass('subtitulo');
                    $(divTemplate).appendTo('#header_ContenedorNotificacionesParaUsuario .items .content');
                }

                $.each(notificacionesNoLeidas, function (index, element) {
                    var divTemplate = $('#header_ContenedorNotificacionesParaUsuario .templateItem').html();
                    divTemplate = $(divTemplate);

                    $(divTemplate).find('.textoTitulo').text(element.Titulo);
                    $(divTemplate).find('.textoContenido').text($(element.Contenido).text());
                    if (!('Icono' in element) || element.Icono == undefined) {
                        $(divTemplate).find('.icono').text('notifications');
                    } else {
                        $(divTemplate).find('.icono').text(element.Icono);
                    }
                    $(divTemplate).appendTo('#header_ContenedorNotificacionesParaUsuario .items .content');

                    $(divTemplate).click(function () {
                        $('#header_ContenedorNotificacionesParaUsuario .fondo').trigger('click');
                        crearDialogoNotificacionparaUsuarioDetalle({
                            Id: element.Id,
                            Titulo: element.Titulo
                        });
                    });
                });

                if (notificacionesLeidas.length != 0) {
                    var divTemplate = $('<label>');
                    $(divTemplate).text('Ya leidas')
                    $(divTemplate).addClass('subtitulo');
                    $(divTemplate).appendTo('#header_ContenedorNotificacionesParaUsuario .items .content');
                }
                $.each(notificacionesLeidas, function (index, element) {
                    var divTemplate = $('#header_ContenedorNotificacionesParaUsuario .templateItem').html();
                    divTemplate = $(divTemplate);

                    $(divTemplate).addClass('leido');

                    $(divTemplate).find('.textoTitulo').text(element.Titulo);
                    $(divTemplate).find('.textoContenido').text($(element.Contenido).text());
                    if (!('Icono' in element) || element.Icono == undefined) {
                        $(divTemplate).find('.icono').text('notifications');
                    } else {
                        $(divTemplate).find('.icono').text(element.Icono);
                    }
                    $(divTemplate).appendTo('#header_ContenedorNotificacionesParaUsuario .items .content');

                    $(divTemplate).click(function () {
                        $('#header_ContenedorNotificacionesParaUsuario .fondo').trigger('click');
                        crearDialogoNotificacionparaUsuarioDetalle({
                            Id: element.Id,
                            Titulo: element.Titulo
                        });
                    });
                });
            },
            OnError: function (result) {
                //Oculto el Contador
                $('#header_Toolbar_TextoCantidadNotificaciones').removeClass('visible');

                //Invoco cerrar el panel
                $('#header_ContenedorNotificacionesParaUsuario .fondo').trigger('click');
            }
        });



    });

    //Notificaciones Close
    $('#header_ContenedorNotificacionesParaUsuario .fondo, #header_ContenedorNotificacionesParaUsuario .btnCerrar').click(function () {
        $('#header_ContenedorNotificacionesParaUsuario').removeClass('visible');

        $('#header_ContenedorNotificacionesParaUsuario .indicadorCargando').removeClass('visible');
        $('#header_ContenedorNotificacionesParaUsuario .indicadorVacio').removeClass('visible');
        $('#header_CcontenedorNotificacionesParaUsuario .items').removeClass('visible');

        if (ajaxNotificaciones != undefined) {
            ajaxNotificaciones.abort()
            ajaxNotificaciones = undefined;
        }
    });

}

function header_InitActionButton_RequerimientosFavoritos() {
    //Buscar RequerimientosFavoritos (despues de que se cargo la pagina)
    $(window).on('first_page_load', function () {
        header_BuscarCantidadRequerimientosFavoritos();
        setInterval(function () {
            header_BuscarCantidadRequerimientosFavoritos();
        }, 60 * 2 * 1000);
    });

    $('#header_BtnRequerimientosFavoritos').click(function () {
        //Busco la pagina a cambiar
        let pagina = header_BuscarInfoPaginaByValor('RequerimientosFavoritos');

        //Si ya esta abierta no hago nada
        if (menuItem != undefined && pagina.Url == menuItem.Url) {
            mostrarMensaje('Info', 'Ya se encuentra viendo sus requerimientos favoritos');
            return;
        }

        //Cambio de pagina
        header_CambiarPagina(pagina);
    });
}

function header_BuscarCantidadNotificaciones() {
    crearAjax({
        Url: ResolveUrl('~/Servicios/NotificacionParaUsuarioService.asmx/GetCantidad'),
        OnSuccess: function (result) {

            if (!result.Ok) {
                $('#header_Toolbar_TextoCantidadNotificaciones').removeClass('visible');
                return;
            }

            if (result.Return == 0) {
                $('#header_Toolbar_TextoCantidadNotificaciones').removeClass('visible');
            } else {
                $('#header_Toolbar_TextoCantidadNotificaciones').addClass('visible');
                $('#header_Toolbar_TextoCantidadNotificaciones').text(result.Return);
            }
        },
        OnError: function (result) {
            $('#header_Toolbar_TextoCantidadNotificaciones').removeClass('visible');
        }
    });
}

function header_BuscarCantidadMisTrabajos() {
    buscarCantidadMisTrabajos();
    setInterval(function () {
        buscarCantidadMisTrabajos();
    }, 30000)

    function buscarCantidadMisTrabajos() {
        crearAjax({
            Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/GetCantidadMisTrabajos'),
            OnSuccess: function (result) {
                if (!result.Ok) {
                    $('#header_Toolbar_TextoCantidadMisTrabajos').removeClass('visible');
                    return;
                }

                if (result.Return == 0) {
                    $('#header_Toolbar_TextoCantidadMisTrabajos').removeClass('visible');
                } else {
                    $('#header_Toolbar_TextoCantidadMisTrabajos').addClass('visible');
                    $('#header_Toolbar_TextoCantidadMisTrabajos').text(result.Return);
                }
            },
            OnError: function (resultado) {
                $('#header_Toolbar_TextoCantidadMisTrabajos').removeClass('visible');
            }
        });
    }
}

function header_BuscarCantidadRequerimientosFavoritos() {
    buscarCantidadFavoritos();
    setInterval(function () {
        buscarCantidadFavoritos();
    }, 30000);

    function buscarCantidadFavoritos() {
        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoFavoritoPorUsuarioService.asmx/GetCantidadByFilters'),
            Data: { consulta: { IdUser: getUsuarioLogeado().Usuario.Id, DadosDeBaja: false } },
            OnSuccess: function (result) {

                if (!result.Ok) {
                    $('#header_Toolbar_TextoCantidadRequerimientosFavoritos').removeClass('visible');
                    return;
                }

                if (result.Return == 0) {
                    $('#header_Toolbar_TextoCantidadRequerimientosFavoritos').removeClass('visible');
                } else {
                    $('#header_Toolbar_TextoCantidadRequerimientosFavoritos').addClass('visible');
                    $('#header_Toolbar_TextoCantidadRequerimientosFavoritos').text(result.Return);
                }
            },
            OnError: function (result) {
                $('#header_Toolbar_TextoCantidadRequerimientosFavoritos').removeClass('visible');
            }
        })
    }
}

function header_InitIndicadores() {
    $('#header_Toolbar_ContenedorIndicador_Urgentes').click(function () {
        let pagina = header_BuscarInfoPaginaByValor('OrdenesDeTrabajoBandeja');
        if (crearOrdenEspecial) {
            pagina = header_BuscarInfoPaginaByValor('Requerimientos');
        }
        pagina.Params = 'urgentes=true';
        header_CambiarPagina(pagina);
    });

    //Buscar
    $(window).on('first_page_load', function () {
        header_BuscarIndicador_Urgentes();
    });
}

function header_BuscarIndicador_Urgentes() {
    buscarCantidadUrgentes();
    setInterval(function () {
        buscarCantidadUrgentes();
    }, 30000);

    function buscarCantidadUrgentes() {
        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetCantidadUrgentesNuevos'),
            OnSuccess: function (resultado) {
                if (!resultado.Ok || resultado.Return==0) {
                    $('#header_Toolbar_ContenedorIndicador_Urgentes').removeClass('visible');
                    return;
                }
                $('#header_Toolbar_ContenedorIndicador_Urgentes').addClass('visible');
                $('#header_Toolbar_ContenedorIndicador_Urgentes label').text(resultado.Return);
            },
            OnError: function (resultado) {
                $('#header_Toolbar_ContenedorIndicador_Urgentes').removeClass('visible');
            }
        })
    }
}

function header_ResaltarPaginaActualEnMenu() {
    //Url actual (sin la barra /)
    var direccion = $.url();
    var url = direccion.attr('path').substring(1);
    var query = direccion.attr('query');
    if (query != undefined && query != "") {
        url = url + '?' + query;
    }

    if (url.indexOf('/') != -1) {
        url = url.split('/')[1];
    }

    //Por cada item
    $('.menuItem').each(function () {
        var item = $(this).children().get(0);

        //si el url del item es igfual a la actual, le agrego la clase que lo resalta
        if ($(item).attr("url") == url) {
            $(this).addClass("menuItemSeleccionado");

            jQuery.fx.off = true;
            //Le pido todos los padres al item resaltado que sean 'subMenu' (para exandirlos)
            if ($(this).parents('.subMenu').length != 0) {
                var padres = $(this).parents('.subMenu');
                $(padres).each(function () {
                    var header = $($(this).children().get(0)).children().get(0);
                    $(header).trigger('click');
                });
            }

            jQuery.fx.off = false;
            return;
        }
    });
}


//-------------------------------------------
// Utils
//-------------------------------------------

function isDrawerExpandido() {
    var drawerExpandido = localStorage.getItem('drawerExpandido') == 'true';

    if (drawerExpandido === null) {
        localStorage.setItem("drawerExpandido", true);
        return true;
    }

    return drawerExpandido;
}

function setDrawerExpandido(expandido, anim) {

    localStorage.setItem('drawerExpandido', expandido);

    $('#header').trigger('drawer');

    var windowsWidth = $(window).width();

    if (windowsWidth <= 768) {
        expandido = false;
    }

    var wDrawer = $('#nav-mobile').width();

    if (expandido != true) {
        $('#nav-mobile').addClass('expandido');
        $('#header_Toolbar').addClass('expandido');
        $('#main').addClass('expandido');
    } else {
        $('#nav-mobile').removeClass('expandido');
        $('#header_Toolbar').removeClass('expandido');
        $('#main').removeClass('expandido');
    }

}




