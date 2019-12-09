let tablaExp;
var areas;
var barrios;
var cpc;
var estados;

var subzonas;
var zonas;

var origenes;

var rqsParaOt = [];

var modo_consulta = 'consulta';
var modo_ordentrabajo = 'ordendetrabajo';
var modo = modo_consulta;

var cantidadAreasConRequerimientosParaBandeja;

let REQUERIMIENTO_PERMISO_OT_POSIBLE_FILTRO = 10;
let REQUERIMIENTO_ESTADOS_OT_POSIBLE_FILTRO = [];

$(document).ready(function () {
    $('.tooltipAyuda').each(function () { // Notice the .each() loop, discussed below
        $(this).qtip({
            content: {
                text: $(this).next('div') // Use the "div" element next to this for the content
            },
            position: {
                my: 'bottom center',
                at: 'top center'
            },
            style: {
                classes: 'qtip-shadow qtip-rounded qtip-tipsy'
            }
        });
    });
});

//Inicialización
function init(data) {
    data = parse(data);
    initPermisos(data);

    var idPrimerPantalla = '#contenedor_ResultadoConsulta';

    var url = $.url().attr('path').split('/')[$.url().attr('path').split('/').length - 1]

    switch (url) {
        case 'OrdenesDeTrabajoBandeja':
            modo = modo_ordentrabajo;
            setDrawerExpandido(false, true);
            break;
        default:
            idPrimerPantalla = '#cardFormularioConsultaAvanzada';
            modo = modo_consulta;
            break;
    }


    //-------------------------
    // Inicializo
    //-------------------------

    initConsulta(data);
    initResultado();

    //$('#contenedor_SeccionArea').hide();

    //Enter en la fecha desde
    $('#inputFormulario_FechaDesde').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#cardFormularioConsultaAvanzada').find('.contenedor-footer').find('.btnOk').click();
        }
    })

    //Enter en la fecha hasta
    $('#inputFormulario_FechaHasta').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#cardFormularioConsultaAvanzada').find('.contenedor-footer').find('.btnOk').click();
        }
    })


    switch (modo) {
        case modo_consulta:
            let urgentes = null;

            //Veo si mande parametros
            let params = $.url().attr().query.split('?');
            if (params != "") {
                //Como mando parametros filtro por esos parametros
                if (params[0].split('=')[0] == 'urgentes') {
                    urgentes = true;
                }

                $("#btnUrgente").addClass("seleccionado");
                $('#checkboxEstados').find('#cblb' + data.EstadoNuevo.Id).click();
            }

            SelectorMotivo_Init({
                TipoMotivo: 1,
                ModoBusqueda: true,
                ModoUrgente: urgentes,
                //solo muestro todos los tipos de motivo si el ambito es muni, si el ambito es cpc solo muestro los generales
                MostrarTiposMotivo: esAmbitoMunicipalidad(),
                CallbackMensaje: function (tipo, mensaje) {
                    mostrarMensaje(tipo, mensaje);
                },
                SeleccionMultiple: true,
                CallbackTipoMotivo: function (tipo) { setTipoMotivo(tipo) }
            });

            if (params != "") {
                consultar();
            }
            break;
        case modo_ordentrabajo:
            $("#contenedor_SeccionSubarea").hide();
            if (esAmbitoMunicipalidad()) {
                $(".filtrosTiposMotivos").show();
            }
            consultaInicialBandeja();

            $('#btnActualizarAreas').click(function () {
                consultarCantidadRequerimientosParaBandeja();
            });

            $("#contenedor_SeccionMotivo > .form-separador").hide();
            break;
    }

    setTimeout(function () {
        ControlSelectorRangoFecha_OcultarCheckMes();
    }, 500);
}

function initPermisos(data) {
    estados = data.Estados;

    //Permiso para estar en una OT
    let keyValues_estar_en_ot = $.grep(getInitData().Requerimiento.Permisos, function (element, index) {
        return element.Permiso == REQUERIMIENTO_PERMISO_OT_POSIBLE_FILTRO && element.TienePermiso;
    }).map(function (a) { return a.EstadoRequerimiento });
    REQUERIMIENTO_ESTADOS_OT_POSIBLE_FILTRO = [];
    $.each(keyValues_estar_en_ot, function (index, element) {
        let estado = $.grep(estados, function (elementEstado, indexEstado) {
            return elementEstado.KeyValue == element;
        })[0];

        if (estado != undefined) {
            REQUERIMIENTO_ESTADOS_OT_POSIBLE_FILTRO.push(estado);
        }
    });
}

function initConsulta(data) {
    $('input').enterKey(function () {
        $('#cardFormularioConsultaAvanzada').find('.contenedor-footer').find('.btnOk').trigger('click');
    });

    //Estados 
    estados = data.Estados;

    data.Estados.forEach(function (estado) {
        var cumpleEstado = false;

        switch (modo) {
            case modo_consulta: {
                cumpleEstado = true;
            } break;

            case modo_ordentrabajo: {
                $.each(REQUERIMIENTO_ESTADOS_OT_POSIBLE_FILTRO, function (index, element) {
                    if (element.KeyValue == estado.KeyValue) {
                        cumpleEstado = true;
                    }
                })
            } break;
        }

        if (cumpleEstado) {
            $('#checkboxEstados').AgregarCheckbox({
                Name: estado.Nombre,
                Value: estado.Id,
            });
            $('#checkboxEstados').find('#cblb' + estado.Id).html('<div class="indicador-estado" style="background-color: #' + estado.Color + '"/>' + toTitleCase(estado.Nombre));
        }
    });

    //Areas
    var def = null;
    areas = usuarioLogeado.Areas;
    let areasFiltradas = areas;

    if (modo == modo_consulta) {
        def = "Todas";
        areasFiltradas = _.filter(usuarioLogeado.Areas, function (area) {
            return _.where(usuarioLogeado.Areas, { Id: area.IdAreaPadre }).length == 0;
        });
    }

    $('#selectFormulario_Area').CargarSelect({
        Data: areasFiltradas,
        Default: def,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    //Evento click en area
    $('#selectFormulario_Area').on('change', function (e) {
        seleccionarArea($(this).val());
    });

    setTimeout(function () {
        $('#selectFormulario_Area').trigger('change');
    }, 450);

    //Barrios
    barrios = data.Barrios;
    $('#selectFormulario_Barrio').CargarSelect({
        Multiple: true,
        Data: data.Barrios,
        Value: 'IdCatastro',
        Text: 'Nombre',
        Sort: true
    });

    //CPC
    cpc = data.CPC;
    $('#selectFormulario_CPC').CargarSelect({
        Data: data.CPC,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre'
    });

    //Zonas
    $('#select_Zona').prop('disabled', true);
    $('#select_Zona').CargarSelect({
        Data: [],
        Value: 'Id',
        Default: 'Seleccione...',
        Text: 'Nombre',
        TitleCase: false
    });

    //Cargo las Origenes
    origenes = data.Origenes;
    $('#select_Origen').CargarSelect({
        Multiple: true,
        Data: origenes,
        Value: 'Id',
        Text: 'Nombre',
        TitleCase: false
    });

    //Filtros de tipo de rq
    $("#tipoGeneral").click(function () {
        setTipoMotivo(1);
        consultar();
    });

    $("#tipoInterno").click(function () {
        setTipoMotivo(2);
        consultar();
    });

    $("#tipoPrivado").click(function () {
        setTipoMotivo(3);
        consultar();
    });

    // Usuario
    SelectorUsuario_SetOnCargandoListener(function (mostrar, mensaje) {
        if (mostrar) {
            mostrarOverlay({ Texto: mensaje });
        } else {
            ocultarOverlay();
        }
    });

    SelectorUsuario_SetOnMensajeListener(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    })

    // Fechas
    ControlSelectorRangoFecha_SetOnMensajeListener(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    });

    //Modo OT o no
    switch (modo) {
        case modo_ordentrabajo: {
            $('#contenedor_SeccionOT').show();
            $('#contenedor_SeccionArea').appendTo('#contenedor-area-ot');

            //CPC
            if (esAmbitoMunicipalidad()) {
                $('#selectFormulario_CPC').prop('disabled', false);
                $('#contenedor-ambito').hide();
            } else if (esAmbitoTodosLosCPC()) {
                $('#selectFormulario_CPC').prop('disabled', false);
                $('#contenedor-ambito').hide();
            }
            else {
                $('#selectFormulario_CPC').prop('disabled', true);

                var cpcSeleccionado = $.grep(cpc, function (element, index) {
                    return element.Numero == getUsuarioLogeado().Ambito.KeyValue;
                })[0];
                $('#selectFormulario_CPC').val(cpcSeleccionado.Id);
                $('#selectFormulario_CPC').trigger('change');

                $('#contenedor-ambito').show();
                $('#contenedor-ambito .titulo').text('Además como su usuario está perfilado en el CPC N° ' + cpcSeleccionado.Numero + ' - ' + cpcSeleccionado.Nombre + ', solo puede trabajar con requerimientos que esten asociados a el.');
            }
        } break;
        case modo_consulta: {
            $('#contenedor_SeccionOT').hide();
        } break;
    }

    //evento click Consultar
    $('#cardFormularioConsultaAvanzada').find('.contenedor-footer').find('.btnOk').click(function () {
        if (!validar()) {
            return;
        }
        consultar();
    });

    //evento click limpiar
    $('#btnLimpiar').click(function () {
        limpiar();
    });

    $("#btnUrgente").click(function () {
        $(this).toggleClass('seleccionado');
        var urgente = ($(this).hasClass('seleccionado'));
        SelectorMotivo_SetModoUrgentes(urgente);
        $('#selectFormulario_Area').trigger('change');
    });
}

function initResultado() {
    initTablaResultadoConsulta();
    initTablaExportar();

    /*Imprime los datos del datatable*/
    $('.btnExportarExcel').click(function () {

        $('.btnExportarExcel').MenuFlotante({
            PosicionX: 'izquierda',
            PosicionY: 'abajo',
            Menu: [
                {
                    Texto: 'Excel',
                    Icono: 'print',
                    OnClick: function () {
                        $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeIn(500);
                        var ids = [];

                        var dt = $('#tablaResultadoConsulta').DataTable();
                        $.each(dt.rows().data(), function (index, row) {
                            ids.push(parseInt(row.Id));
                        });

                        crearAjax({
                            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTablaByIdsExportar'),
                            Data: { ids: ids },
                            OnSuccess: function (result) {
                                if (!result.Ok) {
                                    mostrarMensaje("Error", result.Error);
                                    return;
                                }

                                if (result.Return.Cantidad == 0) {
                                    return;
                                }

                                generarExportar(result.Return.Data);

                                setTimeout(function () {
                                    $("#contenedor_Tabla .buttons-excel").trigger('click');
                                    $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeOut(500);
                                }, 100);
                            },
                            OnError: function (result) {
                                mostrarMensaje("Error", "Error procesando la solicitud");
                            }
                        });
                    }
                }, {
                    Texto: 'PDF',
                    Icono: 'print',
                    OnClick: function () {
                        var ids = [];

                        var dt = $('#tablaResultadoConsulta').DataTable();
                        $.each(dt.rows().data(), function (index, row) {
                            ids.push(parseInt(row.Id));
                        });

                        creado = crearDialogoReporteRequerimientoListadoV2({
                            Ids: ids,
                            Filtros: getTextoFiltrosConsulta()
                        });
                    }
                }
            ]
        })

    });

    $('.btnGenerarMapa').click(function () {

        var ids = [];

        var dt = $('#tablaResultadoConsulta').DataTable();
        $.each(dt.rows().data(), function (index, row) {
            ids.push(parseInt(row.Id));
        });

        crearDialogoMapaGoogleByIdsRequerimiento({
            Ids: ids,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            }
        });
    });

    //evento click volver
    $('#btnCambiarFiltro').click(function () {
        mostrarCardConsulta();
    });

    $('#btnCancelarOt').click(function () {
        deseleccionarTodo();
    });

    $('#btnOt').click(function () {
        var idsRq = [];
        $.each(rqsParaOt, function (index, element) {
            idsRq.push(element.Id);
        })

        crearDialogoOrdenTrabajoNuevo({
            Callback: function (ot) {
                crearDialogoOrdenTrabajoDetalle({
                    Id: ot.Id,
                    Callback: function () {
                        consultar();
                    }
                });

                deseleccionarTodo();
            },
            Requerimientos: idsRq
        });
    });
}

function initTablaResultadoConsulta() {
    if ($.fn.dataTable.isDataTable('#tablaResultadoConsulta')) {
        $('#tablaResultadoConsulta').DataTable().destroy();
        $('#tablaResultadoConsulta').empty();
    }

    /*Se hace debido a que los modos tienen distintas columnas para exportar*/
    let columnasExportar;
    if (modo == modo_consulta) {
        columnasExportar = [1, 2, 3, 4];
    } else {
        columnasExportar = [2, 3, 4, 5];
    }
    var parametros = {
        Orden: [[modo == modo_ordentrabajo ? 3 : 2, 'desc']],
        //Callbacks generales
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackCargando: function (cargando, mensaje) {
            overlay({ Mostrar: cargando, Texto: mensaje });
        },
        Callback: function (rq) {
            actualizarRequerimientoEnGrilla();
            if (modo == modo_ordentrabajo) {
                let idAreaFiltro = getFiltrosConsulta().IdArea;
                let idAreaRq = rq.AreaId;

                if (idAreaRq != idAreaFiltro) {
                    eliminarRequerimientoEnGrilla(rq);
                    deseleccionarRequerimientoParaOt(rq);
                    consultarCantidadRequerimientosParaBandeja();

                    let area = $.grep(areas, function (element, index) {
                        return element.Id == idAreaFiltro;
                    })[0];
                    mostrarMensaje('Alerta', 'El requerimiento se ha quitado del listado ya que no forma parte del area ' + area.Nombre);
                }
            }
        },
        OnFilaCreada: function (row, data, index) {
            var seleccionado = false;
            $.each(rqsParaOt, function (index, element) {
                if (element.Id == data.Id) {
                    seleccionado = true;
                    return false;
                }
            });

            if (seleccionado) {
                $(row).find('input[type="checkbox"]').prop('checked', true)
                $(row).addClass('seleccionada');
            } else {
                $(row).find('input[type="checkbox"]').prop('checked', false)
                $(row).removeClass('seleccionada');
            }
        }
    };

    if (modo == modo_ordentrabajo) {
        //Columnas extra
        parametros.Columnas = [
             {
                 Izquierda: true,
                 "sTitle": 'OT',
                 "mData": null,
                 "orderable": false,
                 width: "26px",
                 Visible: function () {
                     var id = $(this).attr("value");

                     var seleccionado = $.grep(rqsParaOt, function (element, index) {
                         return element.Id == id;
                     }).length != 0;

                     var puedeSeleccionar = validarSeleccionarRequerimientoOT(data);
                     return !seleccionado && (puedeSeleccionar == undefined);
                 },
                 render: function (data, type, full, meta) {
                     return '<div><p style="margin:0px; padding:0px; margin-top:6px;"><input type="checkbox" value="' + data.Id + '" id="chx' + data.Id + '" ><label style="  padding:0px!important" for="chx' + data.Id + '"></label> </p></div>';
                 }
             }
        ]
    }

    if (isTipoMotivoInternoSeleccionado()) {
        parametros.VerUbicacion = false;
        parametros.VerDescripcion = true;
    }

    var dt = $('#tablaResultadoConsulta').DataTableReclamo2(parametros);

    dt.off('click', 'input[type="checkbox"]');
    dt.on('click', 'input[type="checkbox"]', function () {
        var data = dt.row($(this).parents("tr")).data();

        if ($(this).is(':checked')) {
            seleccionarRequerimientoParaOt(data);
        } else {
            deseleccionarRequerimientoParaOt(data);
        }
    });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('<label class="texto_TablaCantidadTotal"></label>').detach().appendTo($('.tabla-footer'));
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));


}

function initTablaExportar() {
    tablaExp = $('#tablaGenerarExcel').DataTableGeneral({
        Ordenar: false,
        Columnas: [
            {
                title: 'Número',
                data: 'Numero',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Año',
                data: 'Año',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Fecha Alta',
                data: 'FechaAlta',
                render: function (data, type, row) {
                    var fecha = moment(data);
                    return '<div><span>' + fecha.format('DD/MM/YYYY HH:mm') + '</span></div>';
                }
            },
            {
                title: 'Motivo',
                data: 'MotivoNombre',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Descripción',
                data: 'Descripcion',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Ubicación',
                data: 'DomicilioDireccion',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
            {
                title: 'Barrio',
                data: 'BarrioNombre',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },

            {
                title: 'Descripción Ubicación',
                data: 'DomicilioObservaciones',
                render: function (data, type, row) {
                    return '<div><span>' + (data || '') + '</span></div>';
                }
            },
        ],
        //Export excell
        OpcionesExportarExcel: {
            extend: 'excelHtml5',
            title: 'Requerimientos de #CBA147',
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7]
            },
        },

        //Export pdf
        OpcionesExportarPdf: {
            extend: 'pdfHtml5',
            title: 'Requerimientos de #CBA147',
            exportOptions: {
                columns: [0, 1, 2, 3, 4, 5, 6, 7]
            },
        }
    });
}

//Filtros y Validaciones
function validar() {
    $('#cardFormularioConsultaAvanzada').find('.control-observacion').text('');
    $('#cardFormularioConsultaAvanzada').find('.control-observacion').hide(300);

    //controlo que los select tengan al menos un dato seleccionado
    var hayAlgunFiltro = false;

    if ($("#btnUrgente").hasClass('seleccionado')) {
        //Urgentes
        hayAlgunFiltro = true;
    }

    //Servicio
    if (SelectorMotivo_IsServicioSeleccionado() || SelectorMotivo_IsDatosIngresadosSinMotivoSeleccionado()) {
        hayAlgunFiltro = true;
    }

    //Motivo
    if (SelectorMotivo_IsMotivoSeleccionado() || SelectorMotivo_IsDatosIngresadosSinMotivoSeleccionado()) {
        hayAlgunFiltro = true;
    }

    //Categoria
    if (SelectorMotivo_IsCategoriaSeleccionada() || SelectorMotivo_IsDatosIngresadosSinMotivoSeleccionado()) {
        hayAlgunFiltro = true;
    }

    //Area
    if ($('#selectFormulario_Area').val() != undefined && $('#selectFormulario_Area').val() != -1) {
        hayAlgunFiltro = true;
    }

    //Origenes
    if ($('#select_Origen').val() != undefined && $('#select_Origen').val().length != 0) {
        hayAlgunFiltro = true;
    }

    //Usuario
    if (SelectorUsuario_IsUsuarioSeleccionado() || SelectorUsuario_IsDatosIngresadosSinUsuarioSeleccionado()) {
        hayAlgunFiltro = true;
    }

    //CPC
    if ($('#selectFormulario_CPC').val() != undefined && $('#selectFormulario_CPC').val() != -1) {
        hayAlgunFiltro = true;
    }

    //Barrio
    if ($('#selectFormulario_Barrio').val() != undefined && $('#selectFormulario_Barrio').val().length != 0) {
        hayAlgunFiltro = true;
    }

    //Domicilio
    if ($('#input_Domicilio').val().trim() != "") {
        hayAlgunFiltro = true;
    }

    //Fechas
    if (ControlSelectorRangoFecha_IsRangoSeleccionado()) {
        hayAlgunFiltro = true;
    }

    //Estados
    var tieneFiltro = false;
    var controles = $('#checkboxEstados').find("input[type='checkbox']");
    $.each(controles, function (index, checkbox) {
        if ($(checkbox).is(":checked")) {
            hayAlgunFiltro = true;
        }
    });


    //Prioridad
    if ($('#check_PrioridadNormal').is(':checked') || $('#check_PrioridadMedia').is(':checked') || $('#check_PrioridadAlta').is(':checked')) {
        hayAlgunFiltro = true;
    }

    if (!hayAlgunFiltro) {
        mostrarMensaje('Alerta', 'Debe seleccionar al menos un filtro');
        return false;
    }

    //Valido los datos
    var valido = true;

    //Motivo Valido
    if (SelectorMotivo_IsDatosIngresadosSinMotivoSeleccionado()) {
        $('#errorFormulario_Motivo').text('Debe validar el motivo');
        $('#errorFormulario_Motivo').stop(true, true).show(300);
        valido = false;
    }

    //Usuario valido
    if (SelectorUsuario_IsDatosIngresadosSinUsuarioSeleccionado()) {
        $('#errorFormulario_Usuario').text('Debe validar la persona');
        $('#errorFormulario_Usuario').stop(true, true).show(300);
        valido = false;
    }

    //Fecha desde
    if (ControlSelectorRangoFecha_IsDatosIngresados() && !ControlSelectorRangoFecha_Validar()) {
        valido = false;
    }

    return valido;
}

function getFiltrosConsulta() {
    var filtros = {
    };

    if ($("#btnUrgente").hasClass('seleccionado')) {
        //Urgentes
        filtros.Urgente = true;
    }

    //Tipo RQ
    var tiposRQ = [];
    tiposRQ.push(SelectorMotivo_GetTipoSeleccionado());
    filtros.Tipos = tiposRQ;

    //Servicio
    if (SelectorMotivo_IsServicioSeleccionado()) {
        filtros.IdsServicio = [];
        filtros.IdsServicio.push(SelectorMotivo_GetServicioSeleccionado().Id);
    } else {
        filtros.IdsServicio = null;
    }

    //Categoria
    if (SelectorMotivo_IsCategoriaSeleccionada()) {
        filtros.IdsCategoria = [];
        var cat = SelectorMotivo_GetCategoriaSeleccionada();
        if (cat) {
            filtros.IdsCategoria.push(cat.Id);
        }
    } else {
        filtros.IdsCategoria = null;
    }

    //Motivo
    if (SelectorMotivo_IsMotivoSeleccionado()) {
        filtros.IdsMotivo = [];
        var motivosSeleccionados = SelectorMotivo_GetMotivoSeleccionado();
        $.each(motivosSeleccionados, function (i, obj) {
            filtros.IdsMotivo.push(obj.Id);
        });
    } else {
        filtros.IdsMotivo = null;
    }

    //Area
    if (modo == modo_consulta) {
        if ($('#selectFormulario_Area').val() != -1) {
            var value = $('#selectFormulario_Area').val();
            filtros.IdsArea = [];
            filtros.IdsArea.push(value);

            //Subarea
            var areaSeleccionado = (_.where(areas, { Id: parseInt(value) }))[0];
            if (areaSeleccionado.Subareas != null && areaSeleccionado.Subareas.length != 0) {
                var subarea = $('#selectFormulario_Subarea').val();
                filtros.IdsArea = [];
                if (subarea != "-1") {
                    filtros.IdsArea.push(subarea);
                } else {
                    filtros.IdsArea = _.pluck(areaSeleccionado.Subareas, 'Id');
                }
            }
        }
    } else {
        //Si es modo OT, solo debe ser una, y es obligatorio
        filtros.IdArea = $('#selectFormulario_Area').val();
    }

    //Usuario
    var usuario = SelectorUsuario_GetUsuarioSeleccionado();
    if (usuario != undefined) {
        filtros.IdsUsuarioReferente = [];
        filtros.IdsUsuarioReferente.push(usuario.Id);
    } else {
        filtros.IdsUsuarioReferente = null;
    }

    //CPC
    if (modo == modo_consulta) {
        if ($('#selectFormulario_CPC').val() != -1) {
            var cpcSeleccionado = $.grep(cpc, function (element, index) {
                return element.Id == $('#selectFormulario_CPC').val();
            })[0];
            if (cpcSeleccionado != undefined) {
                filtros.KeyValuesCPC = [];
                filtros.KeyValuesCPC.push(cpcSeleccionado.Numero);
            } else {
                filtros.KeyValuesCPC = null;
            }
        } else {
            filtros.KeyValuesCPC = null;
        }

    } else {
        if ($('#selectFormulario_CPC').val() != -1) {
            var cpcSeleccionado = $.grep(cpc, function (element, index) {
                return element.Id == $('#selectFormulario_CPC').val();
            })[0];
            if (cpcSeleccionado != undefined) {
                filtros.KeyValueCPC = cpcSeleccionado.Numero;
            } else {
                filtros.KeyValueCPC = null;
            }
        } else {
            filtros.KeyValueCPC = null;
        }
    }

    //Barrio
    if ($('#selectFormulario_Barrio').val() != undefined && $('#selectFormulario_Barrio').val().Count != 0) {
        filtros.IdsBarrioCatastro = [];
        $.each($('#selectFormulario_Barrio').val(), function (i, val) {
            filtros.IdsBarrioCatastro.push(parseInt(val));
        })
    } else {
        filtros.IdsBarrioCatastro = null;
    }

    //Domicilio
    if ($('#input_Domicilio').val().trim() != "") {
        filtros.Domicilio = $('#input_Domicilio').val().trim();
    }

    //Estados
    var estadosKeyValue = [];
    var controles = $('#checkboxEstados').find("input[type='checkbox']");
    $.each(controles, function (index, checkbox) {
        if ($(checkbox).is(":checked")) {
            var estado = $.grep(estados, function (element, index) {
                return element.Id == $(checkbox).val();
            })[0];

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
        filtros.FechaDesde = fechaDesde.toDate();
        filtros.FechaHasta = fechaHasta.toDate();
    }

    //Prioridades
    var prioridades = [];
    if ($('#check_PrioridadNormal').is(':checked')) {
        prioridades.push(1);
    }
    if ($('#check_PrioridadMedia').is(':checked')) {
        prioridades.push(2);
    }
    if ($('#check_PrioridadAlta').is(':checked')) {
        prioridades.push(3);
    }
    filtros.Prioridades = prioridades;

    //Relevamiento Oficio
    if ($('#radio_RelevamientoInterno_Ambos').is(':checked')) {
        filtros.RelevamientoOficio = null;
    } else {
        if ($('#radio_RelevamientoInterno_Si').is(':checked')) {
            filtros.RelevamientoOficio = true;
        } else {
            filtros.RelevamientoOficio = false;
        }
    }

    //Inspeccionado
    if ($('#radio_Inspeccionado_Ambos').is(':checked')) {
        filtros.Inspeccionado = null;
    } else {
        if ($('#radio_Inspeccionado_Si').is(':checked')) {
            filtros.Inspeccionado = true;
        } else {
            filtros.Inspeccionado = false;
        }
    }

    //Zona
    filtros.IdsZona = [];
    if ($('#select_Zona').val() != undefined && $('#select_Zona').val() != -1) {
        filtros.IdsZona.push($('#select_Zona').val());
    }

    //Origenes
    filtros.IdsOrigen = [];
    if ($('#select_Origen').val() != undefined && $('#select_Origen').val().Count != 0) {
        $.each($('#select_Origen').val(), function (i, val) {
            filtros.IdsOrigen.push(parseInt(val));
        })
    }

    return filtros;
}

function getTextoFiltrosConsulta() {
    var filtros = "";
    $.each(getFiltrosConsulta(), function (key, val) {
        if (val == undefined) return true;

        switch (key) {
            case 'IdsServicio': {
                key = 'Servicio';
                if (val == -1 || val == null) return true;
                val = toTitleCase(SelectorMotivo_GetServicioSeleccionado().Nombre);
            } break;

            case 'IdsMotivo': {
                key = 'Motivos';
                if (val == null || val.length == 0) return true;

                var motivosSeleccionados = SelectorMotivo_GetMotivoSeleccionado();
                var texto = '';
                var primero = true;

                $.each(motivosSeleccionados, function (i, obj) {
                    if (!primero)
                        texto += ', ';

                    texto += obj.Nombre;
                    primero = false;
                });

                val = toTitleCase(texto);
            } break;
            case 'IdsArea':
                {
                    key = 'Areas';

                    if (val == null || val.length == 0) return true;

                    var areasSeleccionados = $.grep(areas, function (e) {
                        return ($.grep(val, function (a) {
                            return e.Id == a;
                        })).length > 0;
                    });
                    var texto = '';
                    var primero = true;

                    $.each(areasSeleccionados, function (i, obj) {
                        if (!primero)
                            texto += ', ';

                        texto += obj.Nombre;
                        primero = false;
                    });

                    val = toTitleCase(texto);
                } break;

            case "IdsCategoria": {
                key = 'Categoria';
                if (val == -1 || val == null) return true;
                val = toTitleCase(SelectorMotivo_GetCategoriaSeleccionada().Nombre);
            } break;

            case "IdArea": {
                key = 'Area';
                if (val == -1 || val == null) return true;
                val = toTitleCase($.grep(areas, function (e) { return e.Id == val; })[0].Nombre);
            } break;
            case "IdsOrigen": {
                key = 'Orígenes';

                if (val == null || val.length == 0) return true;

                var origenesSeleccionados = $.grep(origenes, function (e) {
                    return ($.grep(val, function (a) {
                        return e.Id == a;
                    })).length > 0;
                });
                var texto = '';
                var primero = true;

                $.each(origenesSeleccionados, function (i, obj) {
                    if (!primero)
                        texto += ', ';

                    texto += obj.Nombre;
                    primero = false;
                });

                val = toTitleCase(texto);
            } break;

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

            case 'Prioridades': {
                key = "Prioridad";
                if (val.length == 0) return true;
                var prioridades = "";
                $.each(val, function (index, prioridad) {
                    if (prioridades != "") {
                        prioridades += ', ';
                    }
                    switch (prioridad) {
                        case 1: prioridad = 'Normal'; break;
                        case 2: prioridad = 'Media'; break;
                        case 3: prioridad = 'Alta'; break;
                    }
                    prioridades += prioridad;
                });
                val = prioridades;
            } break;
            case 'Tipos': {
                key = "Tipo";
                if (val.length == 0) return true;
                var tipos = "";
                $.each(val, function (index, tipo) {
                    if (tipos != "") {
                        tipos += ', ';
                    }
                    switch (tipo) {
                        case 1: tipo = 'Generales'; break;
                        case 2: tipo = 'Internos'; break;
                        case 3: tipo = 'Privados'; break;
                    }
                    tipos += tipo;
                });
                val = tipos;
            } break;


            case 'RelevamientoOficio': {
                key = 'Relevamiento de Oficio';
                if (val == null) return true
                val = val ? 'Si' : 'No';
            } break;
            case 'Inspeccionado': {
                key = '';
                if (val == null) return true;
                val ? key = 'Inspeccionado' : key = 'Sin Inspección';
                val = ""
            } break;

            case 'Urgente': {
                key = 'Solo Peligrosos';
                val = "";
            } break;

            case 'IdsUsuarioReferente': {
                var usuario = SelectorUsuario_GetUsuarioSeleccionado();
                if (usuario == null) return true;
                key = 'Usuario';
                val = toTitleCase(usuario.Nombre + ' ' + usuario.Apellido + ' (' + usuario.Username + ')');
            } break;


            case 'IdsCPC': {
                if (val == -1) {
                    key = "Ámbito";
                    val = "Municipalidad de Córdoba";
                    break;
                };
                key = 'CPC';
                val = toTitleCase($.grep(cpc, function (e) { return e.Id == val; })[0].Nombre);
            } break;

            case 'KeyValuesCPC':
            case 'KeyValueCPC': {
                if (val == -1) {
                    key = "Ámbito";
                    val = "Municipalidad de Córdoba";
                    break;
                };
                key = 'CPC';
                val = toTitleCase($.grep(cpc, function (e) { return e.Numero == val; })[0].Nombre);
            } break;

            case 'Domicilio': {
                key = 'Dirección';
            } break;

            case 'IdsBarrioCatastro': {
                key = 'Barrio';

                if (val == null || val.length == 0) return true;

                var barriosSeleccionados = $.grep(barrios, function (e) {
                    return ($.grep(val, function (a) {
                        return e.IdCatastro == a;
                    })).length > 0;
                });
                var texto = '';
                var primero = true;

                $.each(barriosSeleccionados, function (i, obj) {
                    if (!primero)
                        texto += ', ';

                    texto += obj.Nombre;
                    primero = false;
                });

                val = toTitleCase(texto);
            } break;
            case 'IdsZona': {
                if (zonas) {
                    key = 'Zona';
                    var resultadoZonas = $.grep(zonas, function (e) { return e.Id == val; });
                    if (resultadoZonas != undefined && resultadoZonas.length != 0) {
                        val = toTitleCase(resultadoZonas[0].Nombre);
                    } else {
                        return true;
                    }
                }
                else {
                    return true;
                }

            } break;
            case 'IdsSubZona': {
                if (subzonas) {
                    key = 'Subzona';
                    var resultadoSubzonas = $.grep(subzonas, function (e) { return e.Id == val; });
                    if (resultadoSubzonas != undefined && resultadoSubzonas.length != 0) {
                        val = toTitleCase(resultadoSubzonas[0].Nombre);
                    } else {
                        return true;
                    }
                }
                else {
                    return true;
                }
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
        if (key == 'IdServicio') {
            key = 'Servicio';
        }
        if (filtros != "") {
            filtros += " - ";
        }
        filtros += '<u>' + key + "</u> " + val;
    });

    return filtros;
}

//Consultas
function consultar() {
    deseleccionarTodo();

    //Muestro cargando
    $('#cardFormularioConsultaAvanzada').find('.cargando').stop(true, true).fadeIn(500);
    $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeIn(500);

    var url = ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTabla');

    if (modo == modo_ordentrabajo) {
        url = ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTablaParaOrdenTrabajo');
    }

    cargarResultadoConsulta([]);
    $(".texto_TablaCantidadTotal").text("");

    crearAjax({
        Url: url,
        Data: {
            consulta: getFiltrosConsulta()
        },
        OnSuccess: function (result) {
            //Oculto el cargando
            $('#cardFormularioConsultaAvanzada').find('.cargando').stop(true, true).fadeOut(500);
            $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeOut(500);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            var resultado = result.Return;

            $('#textoFiltros').html('<b>Filtros</b> ' + getTextoFiltrosConsulta());

            //No hay resultados
            if (resultado.Data.length == 0) {
                mostrarMensaje('Alerta', 'No hay requerimientos que coincidan con los filtros de búsqueda.');
                return;
            }

            //Supero el limite
            if (resultado.SuperaElLimite) {
                mostrarMensaje('Alerta', 'La cantidad de requerimientos encontrados supera la cantidad permitida. Solo se mostrarán ' + resultado.CantidadMaxima + ' requerimientos.');
            }

            setDrawerExpandido('false', true);

            //Cargo
            mostrarCardResultado(function () {
                cargarResultadoConsulta(resultado.Data);
                $(".texto_TablaCantidadTotal").text('de un total de ' + resultado.Cantidad).trigger("change");
            });
        },
        OnError: function (result) {
            $('#cardFormularioConsultaAvanzada').find('.cargando').stop(true, true).fadeOut(500);
            $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeOut(500);
            mostrarMensaje('Error', 'Error al realizar la consulta');
        }
    });

    if (modo == modo_ordentrabajo) {
        consultarCantidadRequerimientosParaBandeja();
    }
}

//Bandeja
function consultaInicialBandeja() {
    //Muestro cargando
    $('#cardFormularioConsultaAvanzada').hide();
    $('#contenedor_ResultadoConsulta').show();

    $('#contenedor_Areas').removeClass('visible');
    $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeIn(500);

    consultarCantidadRequerimientosParaBandeja(
        function (idPrimerAreaConRequerimientos) {
            if (idPrimerAreaConRequerimientos == -1) {
                SelectorMotivo_Init({
                    //TipoMotivo: (a.TiposMotivoPorDefecto[0]),
                    ModoBusqueda: true,
                    MostrarTiposMotivo: esAmbitoMunicipalidad(),
                    CallbackMensaje: function (tipo, mensaje) {
                        mostrarMensaje(tipo, mensaje);
                    },
                    SeleccionMutilple: true,
                    CallbackTipoMotivo: function (tipo) { setTipoMotivo(tipo) }
                });
                mostrarCardBandejaVacia();
                return;
            }

            if (cantidadAreasConRequerimientosParaBandeja != undefined && cantidadAreasConRequerimientosParaBandeja.length > 1) {
                $('#contenedor_Areas').addClass('visible');
            } else {
                $('#contenedor_Areas').removeClass('visible');
            }

            //Veo si mande parametros
            let params = $.url().attr().query.split('?');
            if (params == "") {
                //Como no mando parametros muestro los requerimientos del primer area del usuario
                consultaPorAreaParaBandeja(idPrimerAreaConRequerimientos);
            } else {
                //Como mando parametros filtro por esos parametros
                params = params[0].split('&')

                let idArea = -1;
                let idBarrio = -1;
                let idCategoria = -1;
                let urgentes = null;

                //si tiene parametros, quiere decir que viene del top barrios o de los peligrosos y son rq generales
                setTipoMotivo(1);
                $.each(params, function (index, param) {
                    if (param.split('=')[0] == 'idArea') {
                        idArea = param.split('=')[1];
                    }
                    if (param.split('=')[0] == 'idBarrio') {
                        idBarrio = param.split('=')[1];
                    }
                    if (param.split('=')[0] == 'idCategoria') {
                        idCategoria = param.split('=')[1];
                    }
                    if (param.split('=')[0] == 'urgentes') {
                        urgentes = true;
                    }
                });

                if (urgentes) {
                    SelectorMotivo_Init({
                        ModoBusqueda: true,
                        MostrarTiposMotivo: esAmbitoMunicipalidad(),
                        CallbackMensaje: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        ModoUrgentes: true,
                        SeleccionMultiple: true,
                        CallbackTipoMotivo: function (tipo) { setTipoMotivo(tipo) }
                    });
                    $("#btnUrgente").trigger("click");
                    consultar();
                } else if (idArea == -1 || idBarrio == -1) {
                    top.mostrarMensaje('Error', 'Error en la consulta solicitada');
                    return;
                } else {
                    SelectorMotivo_Init({
                        TipoMotivo: 1,
                        ModoBusqueda: true,
                        MostrarTiposMotivo: false,
                        CallbackMensaje: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        IdArea: idArea,
                        IdCategoria: idCategoria,
                        SeleccionMultiple: true,
                        CallbackTipoMotivo: function (tipo) { setTipoMotivo(tipo) },
                        CallbackCategoriaSeteada: function () { consultar(); }
                    });

                    $('#selectFormulario_Area').val(idArea).trigger('change');;
                    $('#selectFormulario_Barrio').val(idBarrio).trigger('change');
                    $(".filtrosTiposMotivos").hide()

                    if (idCategoria == -1) {
                        consultar();
                    }
                    //if (idCategoria != undefined && idCategoria != -1) {
                    //    $('#selectFormulario_Categoria').val(idCategoria).trigger('change');
                    //}
                }

            }
        },
        function () {
            $('#contenedor_Areas').removeClass('visible');
            mostrarCardConsulta();
        });
}

function consultarCantidadRequerimientosParaBandeja(callback, callbackError) {
    $('#contenedor_Areas').find('.cargando').stop(true, true).fadeIn(500);
    $('#contenedor_Areas .contenedor-main').empty();

    var url = ResolveUrl('~/Servicios/RequerimientoService.asmx/GetCantidadRequerimientosParaOrdenDeTrabajoPorArea');

    crearAjax({
        Url: url,
        OnSuccess: function (result) {
            if (!result.Ok) {
                return;
            }

            $('#contenedor_Areas').find('.cargando').stop(true, true).fadeOut(500);

            var primerAreaConRequerimientos = -1;

            $('#contenedor_Areas .contenedor-main').empty();
            cantidadAreasConRequerimientosParaBandeja = [];
            $.each(result.Return, function (index, element) {
                if (element.Cantidad > 0) {

                    cantidadAreasConRequerimientosParaBandeja.push(element);

                    if (primerAreaConRequerimientos == -1) {
                        primerAreaConRequerimientos = element.IdArea;
                    }

                    var area = $.grep(areas, function (element1, index1) {
                        return element1.Id == element.IdArea;
                    })[0];
                    if (area == undefined) return true;

                    var div = $('<div>');
                    $(div).addClass('area');

                    var texto = $('<label>');
                    $(texto).addClass('nombre');
                    $(texto).text(area.Nombre);
                    $(texto).appendTo(div);

                    var cantidad = $('<label>');
                    $(cantidad).addClass('cantidadRequerimientos');
                    $(cantidad).text(element.Cantidad);
                    $(cantidad).appendTo(div);

                    $(div).appendTo($('#contenedor_Areas .contenedor-main'));

                    $(div).click(function () {
                        setTipoMotivoSeleccionadoSegunArea(area);
                        consultaPorAreaParaBandeja(area.Id);
                    });
                }
            });

            if (callback != undefined) {
                setTipoMotivoSeleccionadoSegunArea($.grep(areas, function (element1, index1) {
                    return element1.Id == primerAreaConRequerimientos;
                })[0]);
                callback(primerAreaConRequerimientos)
            }
        },
        OnError: function (result) {
            $('#contenedor_Areas').find('.cargando').stop(true, true).fadeOut(500);
            mostrarMensaje('Error', 'Error consultando las areas');

            if (callbackError != undefined) {
                callbackError();
            }
        }
    });
}

function consultaPorAreaParaBandeja(id) {
    $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeIn(500);

    cargarResultadoConsulta([]);
    $(".texto_TablaCantidadTotal").text("");

    var data = {
        consulta: {
            IdArea: id,
            DadosDeBaja: false
        }
    };

    //Tipo RQ
    var tipos = [];
    tipos.push(getTipoMotivoSeleccionado());
    if (tipos != []) {
        data.consulta.Tipos = tipos;
    }

    limpiar();
    SelectorMotivo_Init({
        ModoBusqueda: true,
        TipoMotivo: esAmbitoMunicipalidad() ? (tipos[0]) : 1,
        IdArea: id,
        MostrarTiposMotivo: esAmbitoMunicipalidad(),
        CallbackMensaje: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        SeleccionMultiple: true,
        CallbackTipoMotivo: function (tipo) { setTipoMotivo(tipo) }
    });

    $('#selectFormulario_Area').val(id).trigger("change");

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTablaParaOrdenTrabajo'),
        Data: data,
        OnSuccess: function (result) {
            //Oculto el cargando
            $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeOut(500);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            $('#textoFiltros').html('<b>Filtros</b> ' + getTextoFiltrosConsulta());

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

            //Cargo
            mostrarCardResultado(function () {
                cargarResultadoConsulta(resultado.Data);
                $(".texto_TablaCantidadTotal").text(' - Total: ' + resultado.Cantidad).trigger("change");
            });
        },
        OnError: function (result) {
            $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeOut(500);
            mostrarMensaje('Error', 'Error al realizar la consulta');
        }
    });

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetCantidadRequerimientosParaOrdenDeTrabajoPorAreaYTipo'),
        Data: { idArea: id },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarmensaje('error', result.error);
                return;
            }

            var resultado = result.Return;
            $(".filtrosTiposMotivos .cantidadRequerimientos").text("0");

            $.each(resultado, function (i, data) {
                switch (data.IdTipo) {
                    case 1:
                        $("#tipoGeneral .cantidadRequerimientos").show();
                        $("#tipoGeneral .cantidadRequerimientos").text(data.Cantidad);
                        break;
                    case 2:
                        $("#tipoInterno .cantidadRequerimientos").show();
                        $("#tipoInterno .cantidadRequerimientos").text(data.Cantidad);
                        break;
                    case 3:
                        $("#tipoPrivado .cantidadRequerimientos").show();
                        $("#tipoPrivado .cantidadRequerimientos").text(data.Cantidad);
                        break;
                }
            })
        },
        OnError: function (result) {
            $('#contenedor_TablaResultado').find('.cargando').stop(true, true).fadeOut(500);
            mostrarMensaje('Error', 'Error al realizar la consulta');
        }
    });
}

function seleccionarRequerimientoParaOt(rq) {
    //Verifico si debo agregarlo
    var busqueda = $.grep(rqsParaOt, function (element, index) {
        return element.Numero == rq.Numero;
    });

    if (busqueda == undefined) {
        return;
    }

    if (busqueda.length != 0) {
        mostrarMensaje('Alerta', 'El requerimiento seleccionado ya fue seleccionado');
        return;
    }

    var errorSeleccion = validarSeleccionarRequerimientoOT(rq);
    if (errorSeleccion != undefined) {
        mostrarMensaje("Alerta", errorSeleccion);
        return;
    }


    //Lo agrego al listado
    rqsParaOt.push(rq);

    actualizarInterfazPorCambioDeRequerimientosSeleccionados();

    //Creo el html
    var div = $('<div class="card requerimiento">');
    $(div).attr('id-rq', rq.Id);
    var texto = $('<label>').text(rq.Numero);
    $(texto).appendTo(div);

    var btnQuitar = $('<a class="btnQuitar btn-flat btn-redondo waves-effect"><i class="material-icons">clear</i><a>')
    $(btnQuitar).attr('id-rq', rq.Id);

    $(btnQuitar).appendTo(div);

    $('#contenedor-ot .contenedor-main').append(div);

    //Al quitar el requerimiento
    $(btnQuitar).click(function () {
        var id = $(this).attr('id-rq');

        //Animo la salida
        $(div).removeClass('visible');

        //Quito del listado de requerimientos
        rqsParaOt = $.grep(rqsParaOt, function (element, index) {
            return element.Id != id;
        });

        actualizarInterfazPorCambioDeRequerimientosSeleccionados();

        //Luego de la anim, desdibujo
        setTimeout(function () {
            $(div).remove();
        }, 600);
    });

    //Animo cuando aparece
    setTimeout(function () {
        $(div).addClass('visible');
    }, 1);
}

function deseleccionarRequerimientoParaOt(rq) {
    var existente = $.grep(rqsParaOt, function (element, index) {
        return element.Id == rq.Id;
    })[0];

    if (existente == undefined) return;

    rqsParaOt = $.grep(rqsParaOt, function (element, index) {
        return element.Id != rq.Id;
    });

    actualizarInterfazPorCambioDeRequerimientosSeleccionados();

    var div = $('#contenedor-ot .contenedor-main .requerimiento[id-rq=' + rq.Id + ']');
    $(div).removeClass('visible');
    setTimeout(function () {
        $(div).remove();
    }, 600);
}

function deseleccionarTodo() {
    rqsParaOt = [];
    actualizarInterfazPorCambioDeRequerimientosSeleccionados();
    setTimeout(function () {
        $('#contenedor-ot .contenedor-main').empty();
    }, 300);
}

var timeoutAnim;
function actualizarInterfazPorCambioDeRequerimientosSeleccionados() {
    actualizarCantidadRequerimientosSeleccionados();
    $('#tablaResultadoConsulta').DataTable().rows().invalidate('data').draw(false);

    if (timeoutAnim != undefined) {
        clearTimeout(timeoutAnim);
    }

    if (rqsParaOt.length == 0) {
        mostrarContenedorOT(false);
    } else {
        timeoutAnim = setTimeout(function () {
            mostrarContenedorOT(true);
        }, 500);
    }

    if (modo == modo_ordentrabajo) {
        actualizarAreaDeLaOrdenDeTrabajo();
    }
}

function actualizarCantidadRequerimientosSeleccionados() {
    var cantidad = rqsParaOt.length;
    var texto = "";
    if (cantidad == 0) {
        texto = "Ningun requerimiento seleccionado";
    } else {

        if (cantidad == 1) {
            texto = "1 requerimiento seleccionado";
        } else {
            texto = cantidad + " requerimientos seleccionados";
        }
    }
    $('#cantidadRequerimientosSeleccionados').text(texto);
}

function actualizarAreaDeLaOrdenDeTrabajo() {
    var texto = "";
    if (rqsParaOt.length == 0) {
        texto = "Ningun area";
    } else {
        texto = "Area: " + rqsParaOt[0].AreaNombre;
    }

    $('#areaOrdenTrabajo').text(texto);

    if (rqsParaOt.length == 0) {

        $('#selectFormulario_Area').prop('disabled', false);

        timeoutAnim = setTimeout(function () {
            if (cantidadAreasConRequerimientosParaBandeja != undefined && cantidadAreasConRequerimientosParaBandeja.length > 1) {
                $('#contenedor_Areas').addClass('visible');
            } else {
                $('#contenedor_Areas').removeClass('visible');
            }
        }, 500);

    } else {
        $('#contenedor_Areas').removeClass('visible');
        $('#selectFormulario_Area').prop('disabled', true);
    }
}

function mostrarContenedorOT(mostrar) {
    if (mostrar) {
        $('#contenedor-ot').addClass('visible');
        return;
    }

    $('#contenedor-ot').removeClass('visible');
}

function validarSeleccionarRequerimientoOT(rq) {

    //Valido el estado
    var cumpleEstado = false;
    $.each(REQUERIMIENTO_ESTADOS_OT_POSIBLE_FILTRO, function (index, element) {
        if (element.KeyValue == rq.EstadoKeyValue) {
            cumpleEstado = true;
        }
    });

    if (!cumpleEstado) {
        return 'El requerimiento no tiene un estado valido para formar parte de una Orden de Trabajo';
    }

    //Valido que sea del mismo area
    if (rqsParaOt.length != 0) {
        var primero = rqsParaOt[0];
        if (primero.AreaId != rq.AreaId) {
            return 'El requerimiento no es del mismo area que el resto de los requerimientos seleccionados';
        }
    }
    return undefined;
}

//Tipo Motivo
function setTipoMotivoSeleccionadoSegunArea(area) {
    if (area == undefined) {
        area = {};
        area.TiposMotivoPorDefecto = [];
    }

    if (area.TiposMotivoPorDefecto.length == 0) {
        area.TiposMotivoPorDefecto.push(1);
    }

    $.each(area.TiposMotivoPorDefecto, function (i, tipo) {
        setTipoMotivo(tipo);
    })
}

function setTipoMotivo(tipo) {
    $("#tipoGeneral").removeClass("activo");
    $("#tipoInterno").removeClass("activo");
    $("#tipoPrivado").removeClass("activo");

    switch (tipo) {
        case 1:
            $("#tipoGeneral").addClass("activo");
            break;
        case 2:
            $("#tipoInterno").addClass("activo");
            break;
        case 3:
            $("#tipoPrivado").addClass("activo");
            break;
    }

    actualizarTipoSelectorMotivo(tipo);
    //initTablaResultadoConsulta();
}

function getTipoMotivoSeleccionado(a) {
    if ($("#tipoGeneral").hasClass("activo"))
        return 1;
    if ($("#tipoInterno").hasClass("activo"))
        return 2;
    if ($("#tipoPrivado").hasClass("activo"))
        return 3;
}

function isTipoMotivoInternoSeleccionado() {
    return getTipoMotivoSeleccionado() == 2;
}

function actualizarTipoSelectorMotivo(tipo) {
    var tipos = [];
    if (SelectorMotivo_GetTipoSeleccionado() != tipo) {
        SelectorMotivo_SetTipoMotivo(tipo);
    };
}

//Area
function seleccionarArea(value) {
    $('#select_Zona').val('-1').trigger('change');
    $('#select_Zona').prop('disabled', true);

    SelectorMotivo_SetArea(value);

    if (value == -1) {
        return;
    }

    //Borro el error
    $('#select_Zona').siblings('.control-observacion').text('');

    //Bloqueo el select
    $('#select_Zona').prop('disabled', true);

    //Busco las zonas
    var filtros = {
    };
    var idsAreas = [];
    idsAreas.push(value);
    filtros.IdsArea = idsAreas;

    crearAjax({
        Url: ResolveUrl('~/Servicios/ZonaService.asmx/GetByArea'),
        Data: {
            consulta: filtros
        },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                return;
            }

            if (result.Return == 0) {
                return;
            }

            //Desbloqueo el select
            $('#select_Zona').prop('disabled', false);
            zonas = result.Return;

            //Cargo el select
            $('#select_Zona').CargarSelect({
                Data: result.Return,
                Default: "Seleccione...",
                Value: 'Id',
                Text: 'Nombre',
                Sort: true
            });

            $('#select_Zona').trigger('cargado');
        },
        OnError: function (result) {
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });

    var areaSeleccionada = (_.where(areas, { Id: parseInt(value) }))[0];
    if (modo == modo_ordentrabajo) {
        setTipoMotivoSeleccionadoSegunArea(areaSeleccionada);
        $("#tipoPrivado").hide();
        if (areaSeleccionada.CrearOrdenEspecial) {
            $("#tipoPrivado").show();
        }
        return;
    }

    if (areaSeleccionada.Subareas != null && areaSeleccionada.Subareas.length != 0) {
        $('#selectFormulario_Subarea').CargarSelect({
            Data: areaSeleccionada.Subareas,
            Value: 'Id',
            Text: 'Nombre',
            Default: "Todas",
            TitleCase: false
        });

        $('#selectFormulario_Subarea').prop('disabled', false);
    } else {
        $('#selectFormulario_Subarea').prop('disabled', true);
        $('#selectFormulario_Subarea').CargarSelect({
            Data: [],
            Value: 'Id',
            Text: 'Nombre',

            TitleCase: false
        });
    }
}

//Exportación en excel
function generarExportar(result) {
    $('#contenedor_Cargando').addClass('visible');
    $('#contenedor_Resultado').removeClass('visible');
    $('#contenedor_Error').removeClass('visible');


    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaExp.page.len(rows);

    tablaExp.clear();
    tablaExp.rows.add(result).draw();
    tablaExp.$('.tooltipped').tooltip({ delay: 50 });
}

//Utiles
function mostrarCardBandejaVacia() {
    $('#contenedor-busqueda').fadeOut(300).fadeOut(300, function () {
        $('#contenedor_BandejaVacia').fadeIn(300);
    });
}

function mostrarCardConsulta() {
    $('#contenedor_ResultadoConsulta').fadeOut(300, function () {
        $('#cardFormularioConsultaAvanzada').fadeIn(300);
    });
}

function mostrarCardResultado(callback) {
    $('#cardFormularioConsultaAvanzada').fadeOut(300, function () {
        $('#contenedor_ResultadoConsulta').fadeIn(300, function () {
            calcularCantidadDeRows();
            if (callback != undefined) {
                callback();
            }
        });
    });
}

function calcularCantidadDeRows() {
    var hDisponible = $('#contenedor_ResultadoConsulta .tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tablaResultadoConsulta').DataTable();
    dt.page.len(rows).draw();
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
}

function actualizarRequerimientoEnGrilla(rq) {
    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTablaById'),
        Data: {
            id: rq.Id
        },
        OnSuccess: function (result) {
            if (!result.Ok) {
                return;
            }

            //Busco el indice del rq a actualizar
            var index = -1;
            var dt = $('#tablaResultadoConsulta').DataTable();
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
            dt.row(index).data(result.Return);

            //Inicializo el tooltip
            dt.$('.tooltipped').tooltip({ delay: 50 });
        }
    });
}

function eliminarRequerimientoEnGrilla(rq) {
    var dt = $('#tablaResultadoConsulta').DataTable();

    //Quito de la tabla
    var indexFila = -1;
    $.each(dt.data(), function (index, element) {

        if (element.Id == rq.Id) {
            indexFila = index;
            return;
        }
    });

    if (indexFila == -1) {
        mostrarMensaje('Alerta', 'Error al actualizar la tabla');
        return;
    }

    dt.row(indexFila).remove().draw(false);
    $('.material-tooltip').fadeOut(300);
}

function limpiar() {
    //Servicio
    SelectorMotivo_ReiniciarUI();

    //Area
    if (modo == modo_consulta) {
        $('#selectFormulario_Area').val('-1').trigger('change');
    }

    //Usuario
    SelectorUsuario_ReiniciarUI();

    //Barrio
    $('#selectFormulario_Barrio').val('-1').trigger('change');
    $('#selectFormulario_Barrio').prop('disabled', false);

    //Estados
    $('#checkboxEstados').find("input[type='checkbox']").prop("checked", false);

    //Fechas
    ControlSelectorRangoFecha_Limpiar();

    //Prioridades
    $('#check_PrioridadNormal').prop("checked", false);

    $('#check_PrioridadMedia').prop("checked", false);
    $('#check_PrioridadAlta').prop("checked", false);

    //Relevamiento Oficio
    $('#radio_RelevamientoInterno_Si').prop("checked", false);
    $('#radio_RelevamientoInterno_No').prop("checked", false);
    $('#radio_RelevamientoInterno_Ambos').prop("checked", true);

    //Zona
    $('#select_Zona').val('-1').trigger('change');

    //Categoria
    //$('#selectFormulario_Categoria').val('-1').trigger('change');

    //Subzona
    $('#select_Subzona').val('-1').trigger('change');

    //Origen
    $('#select_Origen').val('-1').trigger('change');

    Materialize.updateTextFields();
}