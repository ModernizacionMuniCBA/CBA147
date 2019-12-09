function init(data) {

    console.log('init');

    //General

    $('#btnCpcGlobal').css('display', validarPermisoConsulta('FlotaPanel') ? 'auto' : 'none');
    $('#btnCpcGlobal').click(function () {
        redirigir('EstadisticaReclamoCPC');
    });
    $('#btnOrigen').css('display', validarPermisoConsulta('PersonalPanel') ? 'auto' : 'none');
    $('#btnOrigen').click(function () {
        redirigir('EstadisticaReclamoOrigen');
    });
    $('#btnEficacia').css('display', validarPermisoConsulta('Catalogos') ? 'auto' : 'none');
    $('#btnEficacia').click(function () {
        redirigir('EstadisticaReclamoEficacia');
    });
    $('#btnResueltos').css('display', validarPermisoConsulta('Funciones') ? 'auto' : 'none');
    $('#btnResueltos').click(function () {
        redirigir('EstadisticaReclamoResueltos');
    });
    $('#btnServicios').css('display', validarPermisoConsulta('FlotaPanel') ? 'auto' : 'none');
    $('#btnServicios').click(function () {
        redirigir('EstadisticaReclamoServicio');
    });
    $('#btnZona').css('display', validarPermisoConsulta('PersonalPanel') ? 'auto' : 'none');
    $('#btnZona').click(function () {
        redirigir('EstadisticaReclamoZona');
    });
    $('#btnUsuarios').css('display', validarPermisoConsulta('Catalogos') ? 'auto' : 'none');
    $('#btnUsuarios').click(function () {
        redirigir('EstadisticaReclamoUsuario');
    });
    $('#btnMotivos').css('display', validarPermisoConsulta('Funciones') ? 'auto' : 'none');
    $('#btnMotivos').click(function () {
        redirigir('EstadisticaReclamoMotivos');
    });
    $('#btnRubros').css('display', validarPermisoConsulta('FlotaPanel') ? 'auto' : 'none');
    $('#btnRubros').click(function () {
        redirigir('EstadisticaReclamoRubros');
    });
    $('#btnAreas').css('display', validarPermisoConsulta('PersonalPanel') ? 'auto' : 'none');
    $('#btnAreas').click(function () {
        redirigir('EstadisticaReclamoArea');
    });
    $('#btnSubareas').css('display', validarPermisoConsulta('PersonalPanel') ? 'auto' : 'none');
    $('#btnSubareas').click(function () {
        redirigir('EstadisticaReclamoSubArea');
    });




}