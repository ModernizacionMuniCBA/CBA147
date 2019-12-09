var areas;
var zonas;
var secciones;

var estados;

var estadosMisTrabajos;

var modo_consulta = 'consulta';
var modo_mistrabajos = 'mistrabajos';
var modo = modo_consulta;

//Inicialización
function init2(data) {
    data = parse(data);

    initModo();

    areas = getUsuarioLogeado().Areas;
    estados = data.Estados;
    estadosMisTrabajos = data.EstadosPorDefectoMisTrabajos;

    initBotones();
    initSelectores();
    initInputs();
    initControles();

    //Tabla resultado
    initTablaResultadoConsulta();

    if (modo == modo_mistrabajos) {
        initFiltrosMisTrabajos();
        consultar();
    }
}

function initModo() {
    var url = $.url().attr('path').split('/')[$.url().attr('path').split('/').length - 1]

    switch (url) {
        case 'OrdenesDeTrabajoConsulta':
            modo = modo_consulta;
            break;
        default:
            setDrawerExpandido(false, true);
            modo = modo_mistrabajos;
            break;
    }
}

function initFiltrosMisTrabajos() {
    $.each(estadosMisTrabajos, function (i, estado) {
        $('#checkboxEstados').find('#cb' + estado.Id).prop("checked", true);

    });
}

function initControles() {
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

    setTimeout(function () {
        ControlSelectorRangoFecha_OcultarCheckMes();
    }, 500);
}

function initInputs() {
    //Enter en el numero
    $('#input_NroOrdenTrabajo').keydown(function (e) {
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

}

function initSelectores() {
    var def = "Todas mis áreas";
    if (areas.length == 1) {
        def = null;
    } else {
        $("#contenedor_Area").show("slow");
    }

    $('#select_Area').CargarSelect({
        Data: areas,
        Default: def,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true,
    });


    if (esAmbitoMunicipalidad()) {
        $("#contenedor-selectoresZonaSeccion").show();

        $('#select_Zona').CargarSelect({
            Data: [],
            Value: 'Id',
            Text: 'Nombre',
            Sort: true,
            Default: 'Seleccione...'
        });
        $('#select_Zona').prop('disabled', true);

        $('#select_Seccion').CargarSelect({
            Data: [],
            Value: 'Id',
            Text: 'Nombre',
            Sort: true,
            Default: 'Seleccione...'
        });
        $('#select_Seccion').prop('disabled', true);

        //Areas
        $('#select_Area').on('change', function () {
            buscarZonasYSecciones().then(function (hayZonasOSecciones) {
                if (hayZonasOSecciones) {
                    $("#contenedor-selectoresZonaSeccion").show();
                } else {
                    $("#contenedor-selectoresZonaSeccion").hide();
                }

                var mostrarContenedor = hayZonasOSecciones;
                if (areas.length > 1) {
                    mostrarContenedor = true;
                }

                if (mostrarContenedor) {
                    $("#contenedor_Area").show("swing");
                } 
            });
        });

        $('#select_Area').trigger('change');
    }
}

function initBotones() {
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

        var creado = crearDialogoReporteOrdenTrabajoListado({
            Ids: ids,
            Filtros: getTextoFiltrosConsulta()
        });

        if (creado == false) {
            mostrarMensaje('Error', 'Error creando el reporte');
        }
    });
}

function initTablaResultadoConsulta() {
    var dt = $('#tablaResultadoConsulta').DataTableOrdenTrabajo();

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

//Selectores
function cargarSelector(id, data, def) {
    var valorDefecto = null;
    if (typeof def === 'string') {
        valorDefecto = def;
    } else if (def) {
        valorDefecto = "Seleccione...";
    }

    $(id).CargarSelect({
        Data: data,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true,
        Default: valorDefecto
    });
}

function cargarSelectorAreas(data, def) {
    cargarSelector('#select_Area', data, def);
}

function cargarSelectorZonas(data, def) {
    cargarSelector('#select_Zona', data, def);
}

function cargarSelectorSecciones(data, def) {
    cargarSelector('#select_Seccion', data, def);
}

//Zonas y secciones
function buscarZonasYSecciones() {
    return new Promise(function (callback, callbackError) {
        var llamadas = 0;
        var hayZonasOSecciones=false;

        buscarZonas().then(function (hayZonas) {
            llamadas++;
            if(hayZonas) hayZonasOSecciones=true;

            if (llamadas == 2) callback(hayZonasOSecciones);
        });
        buscarSecciones().then(function (haySecciones) {
            llamadas++;
            if(haySecciones) hayZonasOSecciones=true;

            if (llamadas == 2) callback(hayZonasOSecciones);
        })
    });
}

function buscarZonas() {
    return new Promise(function (callback, callbackError) {
        $('#select_Zona').prop('disabled', true);

        var idArea = $('#select_Area').val();
        if (idArea == -1) {
            cargarSelectorZonas([], true);
            callback(false);
            return;
        }

        $('#select_Area').prop('disabled', true);
        crearAjax(
            {
                Url: ResolveUrl('~/Servicios/ZonaService.asmx/GetByArea'),
                Data: { consulta: { IdsArea: [parseInt(idArea)] } },
                OnSuccess: function (result) {
                    $('#select_Area').prop('disabled', false);

                    //algo salio mal
                    if (!result.Ok) {
                        mostrarMensaje('Error', result.Error);
                    }

                    zonas = result.Return;
                    if (zonas == undefined || zonas == 0) {
                        cargarSelectorZonas([], true);
                        callback(false);
                        return;
                    }

                    $('#select_Zona').prop('disabled', false);
                    cargarSelectorZonas(zonas, true);
                    callback(true);
                },
                OnError: function (result) {
                    mostrarMensaje('Error', 'Error al realizar la consulta');
                    $('#select_Area').prop('disabled', false);
                }
            }
        );

    })
}

function buscarSecciones() {
    return new Promise(function (callback, callbackError) {
        $('#select_Seccion').prop('disabled', true);

        var idArea = $('#select_Area').val();
        if (idArea == -1) {
            cargarSelectorSecciones([], true);
            callback(false);
            return;
        }

        $('#select_Area').prop('disabled', true);
        var listAreas = [];
        listAreas.push(idArea);

        crearAjax({
            Data: { consulta: { IdsArea: listAreas } },
            Url: ResolveUrl('~/Servicios/SeccionService.asmx/GetByFilters'),
            OnSuccess: function (result) {
                $('#select_Area').prop('disabled', false);
                if (!result.Ok) {
                    mostrarMensaje('Error', result.Error);
                }

                secciones = result.Return;
                //algo salio mal
                if (secciones == undefined || secciones.length == 0) {
                    cargarSelectorSecciones([], true);
                    callback(false);
                    return;
                }

                $('#select_Seccion').prop('disabled', false);
                cargarSelectorSecciones(secciones, true);
                callback(true);
            },
            error: function (result) {
                //Informo
                mostrarMensaje('Error', 'Error al realizar la consulta');
                $('#select_Area').prop('disabled', false);
            }
        });
    })
}

//Consulta
function consultar() {
    if (!validar()) {
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
        Url: modo == modo_consulta ? ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/GetDatosTabla') : ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/GetDatosTablaMisTrabajos'),
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
                mostrarMensaje('Alerta', 'No hay órdenes de trabajo que coincidan con los filtros de búsqueda.');
                return;
            }

            //Supero el limite
            if (resultado.SuperaElLimite) {
                mostrarMensaje('Alerta', 'La cantidad de órdenes de trabajo encontradas supera la cantidad permitida. Solo se mostrarán ' + resultado.CantidadMaxima + ' órdenes de trabajo.');
            }

            setDrawerExpandido('false', true);

            //Muestro la otra card
            $('#cardConsulta').fadeOut(300, function () {
                $('#cardResultado').fadeIn(300);
                $("#textoFiltros").html('<b>Filtros</b> ' + getTextoFiltrosConsulta());
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

//Validaciones
function validar() {
    $('.control-observacion').slideUp(300);
    var valido = true;

    //Fecha desde
    if (ControlSelectorRangoFecha_IsDatosIngresados() && !ControlSelectorRangoFecha_Validar()) {
        valido = false;
    }

    return valido;
}

//Filtros
function getFiltrosConsulta() {
    var tieneFiltros = false;
    var filtros = {};

    //Numero
    var numero = $('#input_NroOrdenTrabajo').val();
    if (numero != "") {
        tieneFiltros = true;
        filtros.Numero = numero;
    }

    //Año
    var anio = $('#input_Anio').val();
    if (anio != "") {
        filtros.Año = parseInt(anio);
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

    //Area
    var idArea = $('#select_Area').val();
    tieneFiltros = true;
    if (idArea == -1) {
        filtros.IdsArea = [];
        $.each(areas, function (index, data) {
            filtros.IdsArea.push(data.Id);
        });
    } else {
        filtros.IdsArea = [];
        filtros.IdsArea.push(idArea);
    }

    //Zona
    var idZona = $('#select_Zona').val();
    if (idZona != -1) {
        tieneFiltros = true;
        filtros.IdZona = idZona;
    }

    //Seccion
    var idSeccion = $('#select_Seccion').val();
    if (idSeccion != -1) {
        tieneFiltros = true;
        filtros.IdSeccion = idSeccion;
    }

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
                key = 'Area/s';
                if (val.length > 1) {
                    val = "Todas mis áreas";
                    break;
                }
                if (val == -1 || val == null) return true;
                val = toTitleCase($.grep(areas, function (e) { return e.Id == val; })[0].Nombre);

            } break;

            case 'IdZona': {
                key = 'Zona';
                if (val == -1 || val == null) return true;
                val = toTitleCase($.grep(zonas, function (e) { return e.Id == val; })[0].Nombre);
            } break;

            case 'IdSeccion': {
                key = 'Seccion';
                if (val == -1 || val == null) return true;
                val = toTitleCase($.grep(secciones, function (e) { return e.Id == val; })[0].Nombre);
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

    return filtros;
}

function limpiarFiltros() {
    $('#input_NroOrdenTrabajo').val('');
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

//Utils
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
        $('#cardResultadoOrdenesTrabajo').stop(true, true).fadeOut(300);
        $('#cardConsulta').find('.soloFiltros').hide();
    } else {
        $('#cardResultadoOrdenesTrabajo').stop(true, true).fadeIn(300);
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

function actualizarOrdenTrabajoEnGrilla(ot) {
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