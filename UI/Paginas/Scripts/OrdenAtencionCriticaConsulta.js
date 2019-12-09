var areas;
var estados;
var estadosEditarOT;
var estadosCerrarOT;
var estadosCancelarOT;

function init(data) {
    data = parse(data);

    //-----------------------------
    // Cargo los datos iniciales 
    //-----------------------------

    areas = data.Areas;
    estados = data.Estados;
    estadosEditarOAC = data.EstadosParaEditar;
    estadosCompletarOAC = data.EstadosParaCompletar;

    if (areas.length == 1) {
        $('#select_Area').CargarSelect({
            Data: data.Areas,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true,
        });
    }
    else {
        $('#select_Area').CargarSelect({
            Data: data.Areas,
            Value: 'Id',
            Text: 'Nombre',
            Default: 'Todas',
            Sort: true,
        });

    }

    //Eestados
    estados.forEach(function (estado) {
        $('#checkboxEstados').AgregarCheckbox({
            Name: estado.Nombre,
            Value: estado.Id,
        });
        $('#checkboxEstados').find('#cblb' + estado.Id).html('<div class="indicador-estado" style="background-color: #' + estado.Color + '"/>' + toTitleCase(estado.Nombre));
    });

    //Fechas
    ControlSelectorRangoFecha_SetOnMensajeListener(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    });

    //Tabla resultado
    initTablaResultadoConsulta();

    //-----------------------
    // Botones Footer 
    //-----------------------
    $('.btnLimpiarFiltros').click(function () {
        limpiarFiltros();
    });

    $('.btnBuscar').click(function () {
        consultar();
    });

    $('.btnVolverAConsulta').click(function () {
        $('#cardResultado').fadeOut(300, function () {
            $('#cardConsulta').fadeIn(300);
        });
    });


    //Enter en el area
    $('#select_Area').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnBuscar').click();
        }
    })

    //Enter en la fecha desde
    $('#input_FechaDesde').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnBuscar').click();
        }
    })

    //Enter en la fecha hasta
    $('#input_FechaHasta').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnBuscar').click();
        }
    });

    $('.btnBuscar').trigger('click');
}

function consultar() {

    if (!validarConsulta()) {
        return;
    }

    var filtros = getFiltrosConsulta();
    if (filtros == null) {
        Materialize.toast('Debe seleccionar algun filtro de busqueda', 5000);
        return;
    }

    var data = { consulta: filtros };

    //Muestro cargando
    $('#cardConsulta').find('.cargando').stop(true, true).fadeIn(500);

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrdenAtencionCriticaService.asmx/GetResultadoTabla'),
        Data: data,
        OnSuccess: function (result) {
            //Oculto el cargando
            $('#cardConsulta').find('.cargando').stop(true, true).fadeOut(500);

            //algo salio mal
            if (!result.Ok || result.Return==null) {
                //Informo
                mostrarMensaje('Error', result.Error.Publico);
                console.log("Error al realizar la consulta");
                console.log(result);
                return;
            }

            //No hay resultados
            if (result.Return.Data.length==0) {
                Materialize.toast("No hay ordenes de atención crítica que coincidan con los filtros de búsqueda.", 5000);
                return;
            }

            if (result.Return.SuperaElLimite) {
                mostrarMensaje('Alerta', 'La cantidad de Ordenes de atención crítica encontradas supera la cantidad permitida. Solo se mostrarán ' + result.CantidadMaxima + ' registros.');
            }

            //Muestro la otra card
            $('#cardConsulta').fadeOut(300, function () {
                $('#cardResultado').fadeIn(300);
                cargarResultadoConsulta(result.Return.Data);
            });
        },
        OnError: function (result) {
            //Oculto el cargando
            $('#cardConsulta').find('.cargando').stop(true, true).fadeOut(500);

            //Informo
            mostrarMensaje('Error', 'Error al realizar la consulta');
            console.log("Error al realizar la consulta");
            console.log(result);
        }
    });
}

function validarConsulta() {
    $('.control-observacion').slideUp(300);
    var valido = true;

    //Fecha desde
    if (ControlSelectorRangoFecha_IsDatosIngresados() && !ControlSelectorRangoFecha_Validar()) {
        valido = false;
    }

    return valido;
}


function getFiltrosConsulta() {
    var tieneFiltros = false;
    var filtros = {};

    //Servicio
    //if (SelectorMotivo_IsServicioSeleccionado()) {
    //    filtros.IdsServicio = [];
    //    filtros.IdsServicio.push(SelectorMotivo_GetServicioSeleccionado().Id);
    //    tieneFiltros = true;
    //} else {
    //    filtros.IdsServicio = null;
    //}

    ////Motivo
    //if (SelectorMotivo_IsMotivoSeleccionado()) {
    //    filtros.IdsMotivo = [];
    //    filtros.IdsMotivo.push(SelectorMotivo_GetMotivoSeleccionado().Id);
    //    tieneFiltros = true;
    //} else {
    //    filtros.IdsMotivo = null;
    //}

    //Area
    var idArea = $('#select_Area').val();
    if (idArea != -1) {
        tieneFiltros = true;
        filtros.IdArea = idArea;
    }
    else {
        tieneFiltros = true;
    }

    //Estados
    var estados = [];
    var controles = $('#checkboxEstados').find("input[type='checkbox']");
    $.each(controles, function (index, checkbox) {
        if ($(checkbox).is(":checked")) {
            tieneFiltros = true;
            estados.push($(checkbox).val());
        }
    });
    filtros.EstadosKeyValue = estados;

    //Fechas
    var fechaDesde = ControlSelectorRangoFecha_GetFechaDesde();
    var fechaHasta = ControlSelectorRangoFecha_GetFechaHasta();;
    if (fechaDesde != undefined && fechaHasta != undefined) {
        filtros.FechaDesde = fechaDesde.toDate();
        filtros.FechaHasta = fechaHasta.toDate();
    }

    if (!tieneFiltros) {
        return null;
    }
    return filtros;

}

function limpiarFiltros() {
    //SelectorMotivo_ReiniciarUI();
    $('#input_FechaDesde').val('');
    $('#input_FechaHasta').val('');
    $('#select_Area').val('-1').trigger('change');
    var controles = $('#checkboxEstados').find("input[type='checkbox']");
    $.each(controles, function (index, checkbox) {
        $(checkbox).prop('checked', false);
    });

    ControlSelectorRangoFecha_Limpiar();
    Materialize.updateTextFields();
}


function cargarResultadoConsulta(data) {
    var dt = $('#tablaResultadoConsulta').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    calcularCantidadDeRows();

    if (data == null || data.length == 0) {
        $('#cardResultado').stop(true, true).fadeOut(300);
        $('#cardConsulta').find('.soloFiltros').hide();
    } else {
        $('#cardResultado').stop(true, true).fadeIn(300);
        $('#cardConsulta').find('.soloFiltros').show();
        $('#cardConsulta').find('.soloFiltros').find('a').click();
    }

}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tablaResultadoConsulta').DataTable();
    dt.page.len(rows).draw();
}

function initTablaResultadoConsulta() {
    var dt = $('#tablaResultadoConsulta').DataTableOrdenAtencionCritica({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        //Editar
        BotonEditar: function (data) {
            var cumple = false;
            $.each(estadosEditarOAC, function (index, estado) {
                if (estado.KeyValue == data.EstadoKeyValue) {
                    cumple= true;
                }
            });
            return cumple;
        },
        BotonEditarValidar: function (data) {
            //En algun momento, valido los permisos
            var cumplePermisos = true;

            if (!cumplePermisos) {
                mostrarMensaje('Alerta', 'No los permisos necesarios para realizar esta accion');
                return false;
            }

            //Valido el estado
            var cumpleEstado = false;
            $.each(estadosEditarOAC, function (index, estado) {
                if (estado.KeyValue == data.EstadoKeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede editar la Orden de Atención Crítica por estar en estado: ' + toTitleCase(data.EstadoNombre));
                return false;
            }

            return true;
        },
        CallbackEditar: function (data) {
            actualizarOrdenAtencionCriticaEnGrilla(data);
        },
        //Cerrar
        BotonCompletar: function (data) {
            var cumple = false;
            //Valido el estado
            $.each(estadosCompletarOAC, function (index, estado) {
                if (estado.KeyValue == data.EstadoKeyValue) {
                    cumple= true;
                }
            });
            return cumple;
        },
        BotonCompletarValidar: function (data) {
            //En algun momento, valido los permisos
            var cumplePermisos = true;

            if (!cumplePermisos) {
                mostrarMensaje('Alerta', 'No los permisos necesarios para realizar esta accion');
                return false;
            }

            //Valido el estado
            var cumpleEstado = false;
            $.each(estadosCompletarOAC, function (index, estado) {
                if (estado.KeyValue == data.EstadoKeyValue) {
                    cumpleEstado = true;
                }
            });

            if (!cumpleEstado) {
                mostrarMensaje('Alerta', 'No se puede Completar la Orden de Atención Crítica por estar en estado: ' + toTitleCase(data.EstadoNombre));
                return false;
            }

            return true;
        },
        CallbackCompletar: function (data) {
            actualizarOrdenAtencionCriticaEnGrilla(data);
        },
   
    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

//function actualizarOrdenAtencionCriticaEnGrilla(ot) {
//    //Busco el indice de la persona a actualizar
//    var index = -1;
//    var dt = $('#tablaResultadoConsulta').DataTable();
//    dt.rows(function (idx, data, node) {
//        if (data.Id == ot.Id) {
//            index = idx;
//        }
//    });

//    //Si no esta, corto
//    if (index == -1) {
//        return;
//    }

//    //Actualizo
//    dt.row(index).data(ot);

//    //Inicializo el tooltip
//    dt.$('.tooltipped').tooltip({ delay: 50 });
//}
