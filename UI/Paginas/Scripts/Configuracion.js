function init(data) {

    console.log('init');

    //General

    $('#btnNotificacionesSistema').css('display', validarPermisoConsulta('Notificaciones') ? 'auto' : 'none');
    $('#btnNotificacionesSistema').click(function () {
        redirigir('Notificaciones');
    });



    //$('#btnCatalogo').click(function () {
    //    crearDialogoCatalogo();
    //});

    $('#btnVersionSistema').css('display', getUsuarioLogeado().Rol.Rol == 'Administrador' ? 'auto' : 'none');
    $('#btnVersionSistema').click(function () {
        crearDialogoVersionSistema({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function () {
                informarCabioVersionSistema();
            }
        });
    });

    $('#btnEdificiosMunicipales').css('display', getUsuarioLogeado().Rol.Rol == 'Administrador' ? 'auto' : 'none');
    $('#btnEdificiosMunicipales').click(function () {
        redirigir('EdificioMunicipalConsulta');
    });
    
    $('#btnBarrios').css('display', getUsuarioLogeado().Rol.Rol == 'Administrador' ? 'auto' : 'none');
    $('#btnBarrios').click(function () {
        crearDialogoConfirmacion({
            Texto: '¿Desea insertar todos los barrios del CordobaGeoApi al sistema?',
            CerrarDialogoBotonAceptar: false,
            CallbackPositivo: function (jAlert) {
                $(jAlert).MostrarDialogoCargando(true, true);

                crearAjax({
                    Url: ResolveUrl('~/Servicios/BarrioService.asmx/InsertarDesdeCordobaGeoApi'),
                    OnSuccess: function (result) {
                        $(jAlert).MostrarDialogoCargando(false, true);

                        if (!result.Ok) {
                            top.mostrarMensaje('Error', result.Error);
                            return;
                        }

                        $(jAlert).CerrarDialogo();
                        top.mostrarMensaje('Exito', 'Barrios insertados correctamente');
                    },
                    OnError: function () {
                        $(jAlert).CerrarDialogo();
                        top.mostrarMensaje('Error', 'Error procesando la solicitud');
                    }
                })
            }
        })
    });

    $('#btnInfoGlobal').css('display', getUsuarioLogeado().Rol.Rol == 'Administrador' ? 'auto' : 'none');
    $('#btnInfoGlobal').click(function () {
        redirigir('RequerimientosInfoGlobal');
    });

    $('#btnRubrosMotivo').css('display', validarPermisoConsulta('RubrosMotivo') ? 'auto' : 'none');
    $('#btnRubrosMotivo').click(function () {
        redirigir('RubrosMotivo');
    });

    //$('#btnServicioAreaMotivo').css('display', getUsuarioLogeado().Rol.Rol == 'Administrador' ? 'auto' : 'none');
    ////$('#btnServicioAreaMotivo').css('display', getUsuarioLogeado().Rol.Rol == 'Supervisor' ? 'auto' : 'none');
    //$('#btnServicioAreaMotivo').click(function () {
    //    redirigir('ServicioAreaMotivo');
    //});
 

    //Informacion organica
    let tieneAlgunItemEnOrganica = validarPermisoConsulta('InformacionOrganica') || validarPermisoConsulta('InformacionOrganicaSecretarias') || validarPermisoConsulta('InformacionOrganicaDirecciones');
    $('#contenedor_InformacionOrganica').css('display', tieneAlgunItemEnOrganica ? 'auto' : 'none');

    $('#btnInformacionOrganica').css('display', validarPermisoConsulta('InformacionOrganica') ? 'auto' : 'none');
    $('#btnInformacionOrganica').click(function () {
        redirigir('InformacionOrganica');
    });

    $('#btnSecretarias').css('display', validarPermisoConsulta('InformacionOrganicaSecretarias') ? 'auto' : 'none');
    $('#btnSecretarias').click(function () {
        redirigir('InformacionOrganicaSecretarias');
    });

    $('#btnDirecciones').css('display', validarPermisoConsulta('InformacionOrganicaDirecciones') ? 'auto' : 'none');
    $('#btnDirecciones').click(function () {
        redirigir('InformacionOrganicaDirecciones');
    });

    //Origenes
    let tieneAlgunItemEnOrigenes = validarPermisoConsulta('Origenes') || validarPermisoConsulta('OrigenesPorArea') || validarPermisoConsulta('OrigenesPorAmbito') || validarPermisoConsulta('OrigenesPorUsuario');
    $('#contenedor_Origenes').css('display', tieneAlgunItemEnOrigenes ? 'auto' : 'none');

    $('#btnOrigenes').css('display', validarPermisoConsulta('Origenes') ? 'auto' : 'none');
    $('#btnOrigenes').click(function () {
        redirigir('Origenes');
    });

    $('#btnOrigenesPorArea').css('display', validarPermisoConsulta('OrigenesPorArea') ? 'auto' : 'none');
    $('#btnOrigenesPorArea').click(function () {
        redirigir('OrigenesPorArea');
    });

    $('#btnOrigenesPorAmbito').css('display', validarPermisoConsulta('OrigenesPorAmbito') ? 'auto' : 'none');
    $('#btnOrigenesPorAmbito').click(function () {
        redirigir('OrigenesPorAmbito');
    });

    $('#btnOrigenesPorUsuario').css('display', validarPermisoConsulta('OrigenesPorUsuario') ? 'auto' : 'none');
    $('#btnOrigenesPorUsuario').click(function () {
        redirigir('OrigenesPorUsuario');
    });


    //Permisos
    let tieneAlgunItemEnPermisosRq = validarPermisoConsulta('PermisosRequerimiento') || validarPermisoConsulta('PermisosOrdenTrabajo');
    $('#contenedor_Permisos').css('display', tieneAlgunItemEnPermisosRq ? 'auto' : 'none');

    $('#btnPermisosRequerimiento').css('display', validarPermisoConsulta('PermisosRequerimiento') ? 'auto' : 'none');
    $('#btnPermisosRequerimiento').click(function () {
        redirigir('PermisosRequerimiento');
    });

    $('#btnPermisosOrdenTrabajo').css('display', validarPermisoConsulta('PermisosOrdenTrabajo') ? 'auto' : 'none');
    $('#btnPermisosOrdenTrabajo').click(function () {
        redirigir('PermisosOrdenTrabajo');
    });

    $('#btnPermisosOrdenInspeccion').css('display', validarPermisoConsulta('PermisosOrdenInspeccion') ? 'auto' : 'none');
    $('#btnPermisosOrdenInspeccion').click(function () {
        redirigir('PermisosOrdenInspeccion');
    });


    //$('#btnSecciones').css('display', validarPermisoConsulta('Secciones') ? 'auto' : 'none');
    //$('#btnSecciones').click(function () {
    //    redirigir('Secciones');
    //});

    //$('#btnEmpleados').css('display', validarPermisoConsulta('Empleados') ? 'auto' : 'none');
    //$('#btnEmpleados').click(function () {
    //    redirigir('Empleados');
    //});

}