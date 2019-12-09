var ultimos_RQ;
var ultimos_RQPeligrosos;
var ultimas_OT;
var ultimas_OI;

var estadoTituloRQPeligrosos;
var estadoTituloOrdenes;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    ultimos_RQ = data.Ultimos_RQ;
    ultimos_RQPeligrosos = data.Ultimos_RQPeligrosos;
        ultimas_OT = data.Ultimas_OT;

    estadoTituloRQPeligrosos = data.EstadoRQ;
    estadoTituloOrdenes = data.EstadoOrdenes;

    initTablaRequerimiento();
    cargarRequerimientos();

    initTablaOrdenes();
    cargarRQPeligrososAntiguos();

    cargarOrdenesAntiguas();
    $("#cardOrdenesAntiguas .titulo").text('Ordenes de Trabajo Pendientes');


    animarInicio();
}

function animarInicio() {
    var tDelay = 500;
    var tAnim = 500;

    $('#cardUltimosReclamos').css('top', '50px');
    $('#cardUltimosReclamos').css('opacity', '0');
    setTimeout(function () {
        $('#cardUltimosReclamos').animate({ opacity: 1, top: '0px' }, tAnim);
        $('#estadoTituloRQPeligrosos').animate({ opacity: 1, top: '0px' }, tAnim);
    }, tDelay);

    tDelay += 400;

    $('#cardRQPeligrososAntiguos').css('top', '50px');
    $('#cardRQPeligrososAntiguos').css('opacity', '0');
    setTimeout(function () {
        $('#cardRQPeligrososAntiguos').animate({ opacity: 1, top: '0px' }, tAnim);
        $("#estadoTituloRQPeligrosos").css('color', '#' + estadoTituloRQPeligrosos.Color);
    }, tDelay);

    tDelay += 400;

    $('#cardOrdenesAntiguas').css('top', '50px');
    $('#cardOrdenesAntiguas').css('opacity', '0');
    setTimeout(function () {
        $('#cardOrdenesAntiguas').animate({ opacity: 1, top: '0px' }, tAnim);
        $("#estadoTituloOrdenes").css('color', '#' + estadoTituloOrdenes.Color);
    }, tDelay);
}


function initTablaRequerimiento() {
    var dt = $('#tablaRequerimiento').DataTableReclamo2({
        Paginar: false,
        VerInfo: false
    });

    dt.$('.tooltipped').tooltip({ delay: 50 });

    dt = $('#tablaRQPeligrososAntiguos').DataTableReclamo2({
        Paginar: false,
        VerInfo: false,
        VerIndicadores: false,
        VerUbicacion: false,
        VerDias: true,
        ColumnaEstado: false,
        Orden: [[0, 'desc']],
        MotivoWidth: '200px'
    });

    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function cargarRequerimientos() {
    var dt = $('#tablaRequerimiento').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (ultimos_RQ != null && ultimos_RQ != undefined) {
        dt.rows.add(ultimos_RQ).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function initTablaOrdenes() {
        $('#tablaOrdenAntiguas').DataTableOrdenTrabajo({
            Paginar: false,
            VerInfo: false,
            ColumnaEstado: false,
            ColumnaDias: true,
            ColumnaArea: true,
            ColumnaDescripcion: false,
            Orden: 0
        });
}

function cargarRQPeligrososAntiguos() {
    var dt = $('#tablaRQPeligrososAntiguos').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (ultimos_RQPeligrosos != null && ultimos_RQPeligrosos != undefined) {
        dt.rows.add(ultimos_RQPeligrosos).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
}


function cargarOrdenesAntiguas() {
    var dt = $('#tablaOrdenAntiguas').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (ultimas_OT != null && ultimas_OT != undefined) {
        dt.rows.add(ultimas_OT).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
}


function cargarInspeccionesAntiguas() {
    var dt = $('#tablaOrdenAntiguas').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (ultimas_OI != null && ultimas_OI != undefined) {
        dt.rows.add(ultimas_OI).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

