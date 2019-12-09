function init(data) {

    console.log('init');

    //General

    $('#btnFlotas').css('display', validarPermisoConsulta('FlotaPanel') ? 'auto' : 'none');
    $('#btnFlotas').click(function () {
        redirigir('FlotaPanel');
    });

    $('#btnPersonal').css('display', validarPermisoConsulta('PersonalPanel') ? 'auto' : 'none');
    $('#btnPersonal').click(function () {
        redirigir('PersonalPanel');
    });

    $('#btnCatalogo').css('display', validarPermisoConsulta('Catalogos') ? 'auto' : 'none');
    $('#btnCatalogo').click(function () {
        redirigir('Catalogos');
    });

    $('#btnConfigurarPersonal').css('display', validarPermisoConsulta('PersonalConfiguracion') ? 'auto' : 'none');
    $('#btnConfigurarPersonal').click(function () {
        redirigir('PersonalConfiguracion');

    });

    $('#btnMoviles').css('display', validarPermisoConsulta('Moviles') ? 'auto' : 'none');
    $('#btnMoviles').click(function () {
        redirigir('Moviles');
    });

    $('#btnZonas').css('display', validarPermisoConsulta('Zonas') ? 'auto' : 'none');
    $('#btnZonas').click(function () {
        redirigir('Zonas');
    });

    $('#btnTareas').css('display', validarPermisoConsulta('Tareas') ? 'auto' : 'none');
    $('#btnTareas').click(function () {
        redirigir('Tareas');
    });

    $('#btnSecciones').css('display', validarPermisoConsulta('Secciones') ? 'auto' : 'none');
    $('#btnSecciones').click(function () {
        redirigir('Secciones');
    });

    $('#btnFunciones').css('display', validarPermisoConsulta('Funciones') ? 'auto' : 'none');
    $('#btnFunciones').click(function () {
        crearDialogoFuncionNueva({
            IdArea: getUsuarioLogeado().IdsAreas[0],
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
        });
    });

    var otrasConfig = validarPermisoConsulta('ConfiguracionPorArea');
    $('#btnOtrasConfiguraciones').css('display', otrasConfig ? 'auto' : 'none');
    $('#btnOtrasConfiguraciones').click(function () {
        redirigir('ConfiguracionPorArea');
        tieneConfiguracion = true;
    });

    var catalogo = validarPermisoConsulta('Catalogos');
    $('#btnCatalogo').css('display', catalogo ? 'auto' : 'none');
    $('#btnCatalogo').click(function () {
        redirigir('Catalogos');
    });

    var servicioAreaMotivo=validarPermisoConsulta('ServicioAreaMotivo');
    $('#btnServicioAreaMotivo').css('display',  servicioAreaMotivo? 'auto' : 'none');
    tieneConfiguracion = true;
    $('#btnServicioAreaMotivo').click(function () {
        redirigir('ServicioAreaMotivo');
 
    });

    if (servicioAreaMotivo || otrasConfig || catalogo) {
        $("#contenedor_Configuraciones").show();
    }

}