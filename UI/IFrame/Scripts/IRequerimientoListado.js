var estadosEditarRQ;
var estadosCancelarRQ;
var estadosCambiarEstadoRQ;

function init(data) {
    //Parse la data que me llega desde el servidor
    data = parse(data);
    estadosEditarRQ = data.EstadosParaEditarRQ;
    estadosCambiarEstadoRQ = data.EstadosParaCambiarEstadoRQ;
    estadosCancelarRQ = data.EstadosParaCancelarRQ;

    //Inicializo el form
    initTabla();
}

function setIds(ids) {
    mostrarCargando(true);

    var dataAjax = { ids: ids };
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTablaByIds'),
        success: function (result) {
            result = parse(result.d);

            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }
            var resultado = result.Return;

            //No hay resultados
            if (resultado.Data.length == 0) {
                mostrarMensaje('Alerta', 'No hay requerimientos que coincidan con los filtros de búsqueda.');
                return;
            }

            //Supero el limite
            if (resultado.SuperaElLimite) {
                mostrarMensaje('Alerta', 'La cantidad de requerimientos encontrados supera la cantidad permitida. Solo se mostrarán ' + resultado.CantidadMaxima + ' requerimientos.');
            }

            //setDrawerExpandido('false', true);

            //Cargo

            cargarDatosTabla(resultado.Data);
           
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
/*
function initTabla() {
    var dt = $('#tabla').DataTableReclamo2({
        //Callbacks generales
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackCargando: function (cargando, mensaje) {
            mostrarCargando(cargando, mensaje);
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
            $.each(estadosEditarRQ, function (index, estado) {
                if (estado.KeyValue == data.Estado.KeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede editar el requerimiento por estar en estado: ' + toTitleCase(data.Estado.Nombre));
                return false;
            }

            return true;
        },
        CallbackEditar: function (rq) {
            actualizarRequerimientoEnGrilla(rq);
        },
        //Estado
        BotonCambiarEstado: true,
        BotonCambiarEstadoValidar: function (data) {
            //En algun momento, valido los permisos
            var cumplePermisos = true;

            if (!cumplePermisos) {
                mostrarMensaje('Alerta', 'No los permisos necesarios para realizar esta accion');
                return false;
            }

            //Valido el estado
            var cumpleEstado = false;
            $.each(estadosCambiarEstadoRQ, function (index, estado) {
                if (estado.KeyValue == data.Estado.KeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede cambiar el estado del requerimiento por estar en estado: ' + toTitleCase(data.Estado.Nombre));
                return false;
            }

            return true;
        },
        CallbackCambiarEstado: function (rq) {
            actualizarRequerimientoEnGrilla(rq);
        },
        //Mensaje
        BotonEnviarMensaje: true,
        BotonCambiarEstadoValidar: function (data) {
            //En algun momento, valido los permisos
            var cumplePermisos = true;

            if (!cumplePermisos) {
                mostrarMensaje('Alerta', 'No los permisos necesarios para realizar esta accion');
                return false;
            }

            return true;
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
            $.each(estadosCancelarRQ, function (index, estado) {
                if (estado.KeyValue == data.Estado.KeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede cancelar el requerimiento por estar en estado: ' + toTitleCase(data.Estado.Nombre));
                return false;
            }

            return true;
        },
        CallbackCancelar: function (rq) {
            actualizarRequerimientoEnGrilla(rq);
        },
        //Mail
        BotonEnviarMail: true,
        //Imprimir
        BotonImprimir: true,
        //Imprimir Sin Mapa
        BotonImprimirSinMapa: true,
        VerDomicilio:false,
    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function cargarReclamos(data) {
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

    console.log(rows);
}
*/
function actualizarRequerimientoEnGrilla(rq) {
    //Busco el indice de la persona a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == rq.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(rq);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

//function imprimir() {

//    var ids = [];

//    var dt = $('#tabla').DataTable();
//    $.each(dt.rows().data(), function (index, row) {
//        ids.push(parseInt(row.Id));
//    });
//    var filtrosReporte = "";
//    var creado = crearDialogoReporteRequerimientoListado({
//        Ids: ids,
//        Filtros: filtrosReporte       
//    })

//    if (creado == false) {
//        mostrarMensajeError('Error imprimiendo');
//    }
//}

function imprimir() {
    crearDialogoExcel({
        Id: 1000
    });
}

function generarMapa() {
    var ids = [];

    var dt = $('#tabla').DataTable();
    $.each(dt.rows().data(), function (index, row) {
        ids.push(parseInt(row.Id));
    });

     crearDialogoMapaGoogleByIdsRequerimiento({
            Ids: ids,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
     });
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