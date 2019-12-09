
var requerimientos;

function init(data) {
    data = parse(data);

    requerimientos = data.Requerimientos;
    initTabla();
    cargarDatosTabla(requerimientos.Data);
};


function initTabla() {
    $('#tabla').DataTableReclamo2({
        //Callbacks generales
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackCargando: function (cargando, mensaje) {
            overlay({ Mostrar: cargando, Texto: mensaje });
        },
        //Boton Marcar
        BotonMarcar: true,    
        BotonDesmarcar: true,
        //Editar
        BotonEditar: true,
        //Mensaje
        BotonEnviarMensaje: true,
        //Agregar nota
        BotonAgregarNota: true,
        //Estado
        BotonCambiarEstado: true,
        //Cancelar
        BotonCancelar: true,
        //Mail
        BotonEnviarMail: true,
        //Imprimir
        BotonImprimir: true,
        BotonImprimirSinMapa: true,
    });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();
}

function cargarDatosTabla(data) {
    var dt = $('#tabla').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
    calcularCantidadDeRows();
}