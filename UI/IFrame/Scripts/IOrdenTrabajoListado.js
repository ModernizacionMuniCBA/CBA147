var callbackMensaje;
var callbackCargando;

var estadosEditarOT;
var estadosCerrarOT;
var estadosCancelarOT;

function init(data) {
    //Parse la data que me llega desde el servidor
    data = parse(data);

    estadosEditarOT = data.EstadosParaEditar;
    estadosCerrarOT = data.EstadosParaCerrar;
    estadosCancelarOT = data.EstadosParaCancelar;

    //Inicializo el form
    initTabla();
}

function setIds(ids) {
    mostrarCargando(true);

    var dataAjax = { ids: ids };

    crearAjax({
        Data: dataAjax,
        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/GetDatosTablaByIds'),
        OnSuccess: function (result) {
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensajeError(result.Error);
                console.log("Error al realizar la consulta");
                console.log("Ajax");
                console.log("result");
                console.log(result);
                return;
            }

            //No hay resultados
            if (result.Data.length == 0) {
                //Informo
                mostrarMensajeAlerta("No hay ordenes de trabajo que coincidan con los filtros de búsqueda.");
                return;
            }

            if (result.SuperaElLimite) {
                mostrarMensaje('Error', "La cantidad de Ordenes de Trabajo encontradas supera la cantidad permitida. Solo se mostrarán " + result.CantidadMaxima + ' registros.');
            }

            cargarOrdenesDeTrabajo(result.Data);
        },
        error: function (result) {
            mostrarCargando(false);

            //Informo
            mostrarMensajeError('Error al realizar la consulta');
            console.log("Error al realizar la consulta");
            console.log("Ajax");
            console.log("result");
            console.log(result);
        }
    });
}


function imprimir() {

    var ids = [];

    var dt = $('#tabla').DataTable();
    $.each(dt.rows().data(), function (index, row) {
        ids.push(parseInt(row.Id));
    });

    var creado = crearDialogoReporteOrdenTrabajoListado({
        Ids: ids,
        CallbackCargando: function (cargando, mensaje) {
            callbackCargando(cargando, mensaje)
        }
    })

    if (creado == false) {
        mostrarMensajeError('Error imprimiendo');
    }
}

function initTabla() {
    var dt = $('#tabla').DataTableOrdenTrabajo({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        //Nota
        BotonNota: true,
        BotonNotaValidar: function (data) {
            //En algun momento, valido los permisos
            var cumplePermisos = true;

            if (!cumplePermisos) {
                mostrarMensaje('Alerta', 'No los permisos necesarios para realizar esta accion');
                return false;
            }

            //Valido el estado
            var cumpleEstado = false;
            $.each(estadosEditarOT, function (index, estado) {
                if (estado.KeyValue == data.Estado.KeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede agregar una nota a la Orden de Trabajo por estar en estado: ' + toTitleCase(data.EstadoString));
                return false;
            }

            return true;
        },
        //Editar
        BotonEditar: true,
        BotonEditarValidar: function (data) {
            //En algun momento, valido los permisos
            var cumplePermisos = true;

            if (!cumplePermisos) {
                mostrarMensaje('Alerta', 'No los permisos necesarios para realizar esta accion');
                return false;
            }

            //Valido el estado
            var cumpleEstado = false;
            $.each(estadosEditarOT, function (index, estado) {
                if (estado.KeyValue == data.Estado.KeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede editar la Orden de Trabajo por estar en estado: ' + toTitleCase(data.EstadoString));
                return false;
            }

            return true;
        },
        CallbackEditar: function (data) {
            actualizarOrdenTrabajoEnGrilla(data);
        },
        //Cerrar
        BotonCerrar: true,
        BotonCerrarValidar: function (data) {
            //En algun momento, valido los permisos
            var cumplePermisos = true;

            if (!cumplePermisos) {
                mostrarMensaje('Alerta', 'No los permisos necesarios para realizar esta accion');
                return false;
            }

            //Valido el estado
            var cumpleEstado = false;
            $.each(estadosCerrarOT, function (index, estado) {
                if (estado.KeyValue == data.Estado.KeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede cerrar la Orden de Trabajo por estar en estado: ' + toTitleCase(data.EstadoString));
                return false;
            }

            return true;
        },
        CallbackCerrar: function (data) {
            actualizarOrdenTrabajoEnGrilla(data);
        },
        //Cancelar
        BotonCancelar: true,
        BotonCancelarValidar: function (data) {
            //En algun momento, valido los permisos
            var cumplePermisos = true;

            if (!cumplePermisos) {
                mostrarMensaje('Alerta', 'No los permisos necesarios para realizar esta accion');
                return false;
            }

            //Valido el estado
            var cumpleEstado = false;
            $.each(estadosCancelarOT, function (index, estado) {
                if (estado.KeyValue == data.Estado.KeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede cancelar la Orden de Trabajo por estar en estado: ' + toTitleCase(data.EstadoString));
                return false;
            }
            return true;
        },
        CallbackCancelar: function (data) {
            actualizarOrdenTrabajoEnGrilla(data);
        },
        //Mail
        BotonEnviarMail: true,
        //Imprimir
        BotonImprimir: true,

        BotonImprimirSinMapa: true,

        BotonImprimirCaratulaConMapa: true,

        BotonImprimirCaratulaSinMapa: true


    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function actualizarOrdenTrabajoEnGrilla(ot) {
    //Busco el indice de la persona a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == ot.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(ot);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function cargarOrdenesDeTrabajo(data) {
    var dt = $('#tabla').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Cantidad de rows
    calcularCantidadDeRows();
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').find('#tabla_wrapper').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();
}

////-------------------------------
//// Listener Cargando
////-------------------------------

//function mostrarCargando(mostrar, mensaje) {
//    if (callbackCargando != undefined) {
//        callbackCargando(mostrar, mensaje);
//    }
//}

//function setOnCargandoListener(callback) {
//    this.callbackCargando = callback;
//}

////-----------------------------
//// Listener Alertas
////-----------------------------

//function setOnMensajeListener(callback) {
//    this.callbackMensaje = callback;

//}

//function mostrarMensaje(tipo, mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje(tipo, mensaje);
//}

//function mostrarMensajeExito(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Exito', mensaje);
//}
//function mostrarMensajeError(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Error', mensaje);
//}

//function mostrarMensajeAlerta(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Alerta', mensaje);
//}

//function mostrarMensajeInfo(mensaje) {
//    if (callbackMensaje == undefined) return;
//    callbackMensaje('Info', mensaje);
//}