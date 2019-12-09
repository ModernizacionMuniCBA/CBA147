var areas;
var estados;

function init(data) {
    data = parse(data);

    //-----------------------------
    // Cargo los datos iniciales 
    //-----------------------------

    estados = data.Estados;

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
    $('#btnLimpiar').click(function () {
        limpiarFiltros();
    });

    $('.btnBuscar').click(function () {
        consultar();
    });

    $('#btnCambiarFiltro').click(function () {
        $('#cardResultado').fadeOut(300, function () {
            $('#cardConsulta').fadeIn(300);
        });
    });
    
    $('.btnImprimir').click(function () {

        var ids = [];

        var dt = $('#tablaResultadoConsulta').DataTable();
        $.each(dt.rows().data(), function (index, row) {
            ids.push(parseInt(row.Id));
        });

        var creado = crearDialogoReporteOrdenInspeccionListado({
            Ids: ids,
            Filtros: filtrosReporte
        });

         if (creado == false) {
            mostrarMensaje('Error', 'Error creando el reporte');
         }
    });

    //Enter en el numero
    $('#input_NroOrden').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnBuscar').click();
        }
    })

    //Enter en el año
    $('#input_Anio').keydown(function (e) {
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
    })

    $('#select_Zona').CargarSelect({
        Data: [],
        Value: 'Id',
        Text: 'Nombre',
        Sort: true,
        Default: 'Seleccione...'
    });
    $('#select_Zona').prop('disabled', true);

    //si es cpc, no filtra por areas
    if (usuarioLogeado.Ambito.KeyValue != 0) {
        $("#contenedor_Area").hide();
        return;
    }

    areas = usuarioLogeado.Areas;
    if (areas.length == 1) {
        $('#select_Area').CargarSelect({
            Data: areas,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true,
        });
    }
    else {
        $('#select_Area').CargarSelect({
            Data: areas,
            Value: 'Id',
            Text: 'Nombre',
            Default: 'Todas',
            Sort: true,
        });

    }

    //Areas
    $('#select_Area').on('change', function () {
        buscarZonas();
    });

    $('#select_Area').trigger('change');
};

function buscarZonas() {
    var idArea = $('#select_Area').val();
    if (idArea == -1) {
        $('#select_Zona').prop('disabled', true);
        $('#select_Zona').CargarSelect({
            Data: [],
            Value: 'Id',
            Text: 'Nombre',
            Sort: true,
            Default: 'Seleccione...'
        });
    } else {
        $('#select_Area').prop('disabled', true);
        $('#select_Zona').prop('disabled', true);

        var data = { consulta: { IdsArea: [parseInt(idArea)] } };
        crearAjax(
            {
                Url: ResolveUrl('~/Servicios/ZonaService.asmx/GetByArea'),
                Data: data,
                OnSuccess: function (result) {
                    //algo salio mal
                    if (!result.Ok) {
                        //Informo
                        mostrarMensaje('Error', result.Error);

                        $('#select_Area').prop('disabled', false);
                        $('#select_Zona').prop('disabled', true);
                        return;
                    }

                    zonas = result.Return;
                    if (zonas == 0) {
                        $('#select_Zona').CargarSelect({
                            Data: [],
                            Value: 'Id',
                            Text: 'Nombre',
                            Sort: true,
                            Default: 'Seleccione...'
                        });
                        $('#select_Area').prop('disabled', false);
                        $('#select_Zona').prop('disabled', true);
                        return;
                    }

                    $('#select_Area').prop('disabled', false);
                    $('#select_Zona').prop('disabled', false);

                    $('#select_Zona').CargarSelect({
                        Data: zonas,
                        Value: 'Id',
                        Text: 'Nombre',
                        Sort: true,
                        Default: 'Seleccione...'
                    });
                },
                OnError: function (result) {
                    //Informo
                    mostrarMensaje('Error', 'Error al realizar la consulta');

                    $('#select_Area').prop('disabled', false);
                    $('#select_Zona').prop('disabled', true);
                }
            }
        );
    }
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

    //Muestro cargando
    $('#cardConsulta').find('.cargando').stop(true, true).fadeIn(500);

    crearAjax({
        Data: { consulta: getFiltrosConsulta() },
        Url: ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/GetDatosTabla'),
        OnSuccess: function (result) {
            //Oculto el cargando
            $('#cardConsulta').find('.cargando').stop(true, true).fadeOut(500);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            var resultado = result.Return;

            //No hay resultados
            if (resultado.Data.length == 0) {
                mostrarMensaje('Alerta', 'No hay órdenes de inspección que coincidan con los filtros de búsqueda.');
                return;
            }

            //Supero el limite
            if (resultado.SuperaElLimite) {
                mostrarMensaje('Alerta', 'La cantidad de órdenes de inspección encontradas supera la cantidad permitida. Solo se mostrarán ' + resultado.CantidadMaxima + ' órdenes de inspección.');
            }

            setDrawerExpandido('false', true);

            //Muestro la otra card
            $('#cardConsulta').fadeOut(300, function () {
                $('#cardResultado').fadeIn(300);
                $("#textoFiltros").html(getTextoFiltrosConsulta());
                cargarResultadoConsulta(resultado.Data);
            });

        },
        OnError: function (result) {
            //Oculto el cargando
            $('#cardConsulta').find('.cargando').stop(true, true).fadeOut(500);

            //Informo
            mostrarMensaje('Error', 'Error al realizar la consulta');
            console.log("Error al realizar la consulta");
            console.log(dataAjax);
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

    //Numero
    var numero = $('#input_NroOrden').val();
    if (numero != "") {
        tieneFiltros = true;
        filtros.Numero = numero;
    }

    //Año
    var anio = $('#input_Anio').val();
    if (anio != "") {
        filtros.Año = parseInt(anio);
    }

    //Area
    var idArea = $('#select_Area').val();
    if (idArea != -1) {
        tieneFiltros = true;
        filtros.IdArea = idArea;
    }

    //Zona
    var idZona = $('#select_Zona').val();
    if (idZona != -1) {
        tieneFiltros = true;
        filtros.IdZona = idZona;
    }

    //Estados
    var estadosKeyValue = [];
    var controles = $('#checkboxEstados').find("input[type='checkbox']");
    $.each(controles, function (index, checkbox) {
        if ($(checkbox).is(":checked")) {
            var estado = $.grep(estados, function (element, index) {
                return element.Id == $(checkbox).val();
            })[0];
            tieneFiltros = true;
            if (estado != undefined) {
                estadosKeyValue.push(estado.KeyValue);
            }
        }
    });
    filtros.EstadosKeyValue = estadosKeyValue;

    //Fechas
    var fechaDesde = ControlSelectorRangoFecha_GetFechaDesde();
    var fechaHasta = ControlSelectorRangoFecha_GetFechaHasta();
    if (fechaDesde != undefined && fechaHasta != undefined) {
        tieneFiltros = true;
        filtros.FechaDesde = fechaDesde.toDate();
        filtros.FechaHasta = fechaHasta.toDate();
    }

    if (!tieneFiltros) {
        return null;
    }
    return filtros;

}

function getTextoFiltrosConsulta() {
    var filtros = "";
    $.each(getFiltrosConsulta(), function (key, val) {
        if (val == undefined) return true;

        switch (key) {
            case 'EstadosKeyValue': {
                key = 'Estados';
                if (val.length == 0) return true;
                var estadosString = "";
                $.each(val, function (index, estado) {
                    if (estadosString != "") {
                        estadosString += ', ';
                    }
                    estadosString += toTitleCase($.grep(estados, function (e) { return e.KeyValue == estado; })[0].Nombre);
                });
                val = estadosString;
            } break;

            case 'IdsArea':
            case "IdArea": {
                key = 'Area';
                if (val == -1 || val == null) return true;
                val = toTitleCase($.grep(areas, function (e) { return e.Id == val; })[0].Nombre);
            } break;

            case 'IdZona': {
                key = 'Zona';
                if (val == -1 || val == null) return true;
                val = toTitleCase($.grep(zonas, function (e) { return e.Id == val; })[0].Nombre);
            } break;


            case 'FechaDesde': {
                key = 'Fecha desde:';
                val = dateToString(val);
            } break;

            case 'FechaHasta': {
                key = 'Fecha hasta:';
                val = dateToString(val);
            } break;
        }
        if (val == undefined) {
            return true;
        }

        if (filtros != "") {
            filtros += " - ";
        }
        filtros += '<u>' + key + "</u> " + val;
    });
    filtrosReporte = filtros;
    return '<b>Filtros</b> ' + filtros;
}

function limpiarFiltros() {
    $('#input_NroOrden').val('');
    $('#input_FechaDesde').val('');
    $('#input_FechaHasta').val('');
    $('#input_Anio').val('');
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
        $('#cardResultadoOrdenes').stop(true, true).fadeOut(300);
        $('#cardConsulta').find('.soloFiltros').hide();
    } else {
        $('#cardResultadoOrdenes').stop(true, true).fadeIn(300);
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
    var dt = $('#tablaResultadoConsulta').DataTableOrdenInspeccion({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackEditar: function (data) {
            actualizarOrdenEnGrilla(data);
        }
    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function actualizarOrdenEnGrilla(ot) {
    //Busco el indice de la persona a actualizar
    var index = -1;
    var dt = $('#tablaResultadoConsulta').DataTable();
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

function mostrarMensaje(tipo, mensaje) {
    switch (tipo) {
        case 'Alerta':
            Materialize.toast(mensaje, 5000, 'colorAlerta');
            break;

        case 'Error':
            Materialize.toast(mensaje, 5000, 'colorError');
            break;

        case 'Exito':
            Materialize.toast(mensaje, 5000, 'colorExito');
            break;

        case 'Info':
            Materialize.toast(mensaje, 5000);
            break;
    }
}