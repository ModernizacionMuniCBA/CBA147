let o;
let requerimientos;

let callback;

let estadosCerrarOT;
let estadoIncompleto;
let estadoCompleto;

let permiso = 0;

let dt;

let keyValueDefault = 7;

function init(data) {
    if ('Error' in data) {
        mostrarMensajeCritico({ Icono: 'error_outline', Titulo: data.Error });
        return;
    }

    o = data.OrdenTrabajo;
    if (esAmbitoMunicipalidad()) {
        keyValueDefault = 7;
    } else {
        keyValueDefault = 2;
    }

    permiso = data.Permiso;

    requerimientos = data.Requerimientos.Data;
    $.each(requerimientos, function (index, element) {
        element.EstadoNuevoKeyValue = keyValueDefault;
    });


    initTabla();

    setTimeout(function () {
        cargarDatos();
    }, 500);
}

function initTabla() {
    dt = $('#tablaReclamos').DataTableReclamo2({
        Columnas: [
             {
                 title: "Estado",
                 width: '140px',
                 data: null,
                 render: function (data, type, row) {
                     return "<select style='width: 100%'></select>"
                 }
             },
        ],
        Botones: [
             {
                 Titulo: 'Quitar',
                 Icono: 'swap_vertical_circle',
                 OnClick: function (data) {
                     if (dt.rows()[0].length == 1) {
                         mostrarMensajeAlerta('La Orden de Trabajo debe tener al menos un requerimiento.');
                         return;
                     }

                     crearDialogoOrdenTrabajoQuitarRequerimiento({
                         IdOt: o.Id,
                         IdRq: data.Id,
                         Callback: function () {
                             quitarFilaEnTabla(data);
                         }
                     });
                 }
             }
        ],
        Callback: function (entity) {
            //Le seteo al arreglo de datos el key value que tenia
            $.each(requerimientos, function (index, element) {
                if (element.Id == entity.Id) {
                    entity.EstadoNuevoKeyValue = element.EstadoNuevoKeyValue;
                    element = entity;
                    requerimientos[index] = element;
                }
            });


            //Init select de nuevo (se pierde al refrescarse la fila)
            let dataEstados = getEstadosPosibles();
            $.each(dataEstados, function (index, data) {
                data.html = '<div><div class="    display: flex !important; "><label class="indicador-estado" style="background-color: #' + data.Color + '"></label><span>' + toTitleCase(data.Nombre) + '</span></div></div>';
            });

            dt.rows().every(function () {
                let data = this.data();
                if (data.Id == entity.Id) {
                    let node = this.node();
                    $(node).find('select').CargarSelect({
                        Data: dataEstados,
                        Value: 'KeyValue',
                        Text: 'Nombre',
                        Sort: true
                    });

                    $(node).find('select').val(entity.EstadoNuevoKeyValue).trigger('change');
                }
            });
        },
        OnFilaCreada: function (row, data, index) {
            //Init select
            let dataEstados = getEstadosPosibles();
            $.each(dataEstados, function (index, data) {
                data.html = '<div><div class="    display: flex !important; "><label class="indicador-estado" style="background-color: #' + data.Color + '"></label><span>' + toTitleCase(data.Nombre) + '</span></div></div>';
            });

            $(row).find('select').CargarSelect({
                Data: dataEstados,
                Value: 'KeyValue',
                Text: 'Nombre',
                Sort: true
            });

            //Seteo el estado que tiene guardado
            $(row).find('select').val(data.EstadoNuevoKeyValue).trigger('change');

            //Al cambiar guardo el valor en el listado de rqs
            $(row).find('select').change(function () {
                let k = $(this).val();
                data.EstadoNuevoKeyValue = k;
            });
        }
    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
    dt.$('.selectMaterialize').material_select();

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
    $('.dataTables_info').hide();
}

function cargarDatos() {
    cargarTabla();
    calcularCantidadDeFilas();
}

function cargarTabla() {
    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (requerimientos != null) {
        dt.rows.add(requerimientos).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function getEstadosPosibles() {
    let keyValuesEstados = _.pluck(_.filter(getInitData().Requerimiento.Permisos, function (entity) { return entity.Permiso == permiso && entity.TienePermiso }), 'EstadoRequerimiento');
    let resultado = [];
    let estados = getInitData().Requerimiento.Estados;
    $.each(keyValuesEstados, function (index, keyValue) {
        resultado.push(_.find(estados, function (entity) { return entity.KeyValue == keyValue }));
    });
    return resultado;
}

function calcularCantidadDeFilas() {
    var hDisponible = $('#contenedor_Tabla').height() - 48;
    var rows = calcularCantidadRowsDataTable(hDisponible);
    dt.page.len(rows).draw();
    console.log(rows);
}

function quitarFilaEnTabla(rq) {
    //Busco el index del RQ a quitar
    var index;
    dt.rows(function (idx, data, node) {
        if (data.Id == rq.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        console.log("No se encontro el rq a quitar en el DataTable");
        return;
    }

    ////Oculto todas las tooltips
    dt.$('tr').find('.tooltipped').trigger('mouseleave');

    //Quito el reclamo
    dt.row(index).remove().draw();

    //Inicializo los tooltips
    dt.$('tr').find('.tooltipped').tooltip({ delay: 50 });
}

function completar() {
    let motivo = $('#input_Motivo').val().trim();
    if (motivo == "") {
        mostrarMensaje('Alerta', 'Inserte una descripcion del cierre de la Orden');
        $('#input_Motivo').focus();
        return;
    }

    crearDialogoConfirmacion({
        Texto: '¿Esta seguro de querer completar la Orden? Esta acción no se puede deshacer.',
        TextoBotonAceptar: 'Aceptar',
        ClassBotonAceptar: 'colorExito',
        CerrarDialogoBotonAceptar: false,
        CallbackPositivo: function (jAlert) {
            $(jAlert).MostrarDialogoCargando(true);

            let url = ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/Completar');

            crearAjax({
                Url: url,
                Data: { comando: getData() },
                OnSuccess: function (result) {

                    if (!result.Ok) {
                        $(jAlert).MostrarDialogoCargando(false);
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    if (!result.Return) {
                        $(jAlert).MostrarDialogoCargando(false);
                        mostrarMensaje('Error', 'Error procesando la solicitud');
                        return;
                    }

                    $(jAlert).CerrarDialogo();
                    informar();
                }
            });
        }
    });
}

function getData() {
    let data = {};
    data.Id = o.Id;
    data.Observaciones = $('#input_Motivo').val().trim();
    data.Requerimientos = [];
    dt.rows().every(function () {
        let d = this.data();
        data.Requerimientos.push({
            IdRequerimiento: d.Id,
            KeyValueEstado: d.EstadoNuevoKeyValue
        });
    });
    return data;
}

//----------------------------
// Listener Cerrar
//----------------------------

function informar() {
    if (callback == null) return;
    callback();
}

function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}