let callback;

let o;

let modo_OT = 'OrdenTrabajo';
let modo_OI = "OrdenInspeccion";
let modo = modo_OT;

let dt;
let rqsParaOt = [];
let primeraVez = true;
let cantidad;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        mostrarError(data.Error);
        return;
    }

    if (data.OrdenTrabajo != undefined) {
        o = data.OrdenTrabajo;
        modo=modo_OT;
    }

    if (data.OrdenInspeccion != undefined) {
        o = data.OrdenInspeccion;
        modo=modo_OI;
    }

    console.log(data);

    $('#btn_OcultarEncabezado').click(function () {
        $('#contenedor_Encabezado').toggleClass('oculto');
        setTimeout(function () {
            calcularCantidadDeRows();
        }, 400);
    });

    $('#btn_Buscar').click(function () {
        $('#contenedor_Resultado').removeClass('visible');
        buscar();
    });

    initTabla();

    //Cargo por defecto
    cantidad = data.Limite;
    $('#contenedor_Encabezado').addClass('oculto');
    $('#contenedor_Resultado').addClass('visible');
    $('.indicador label').html(getFiltrosTexto());
    setTimeout(function () {
        calcularCantidadDeRows();
        cargarTabla(data.Requerimientos.Data);
    }, 500);
}

function initTabla() {
    dt = $('#tabla').DataTableReclamo2({
        Columnas: [
            {
                Izquierda: true,
                title: '',
                data: null,
                orderable: false,
                width: "26px",
                Visible: function () {
                    var id = $(this).attr("value");

                    var seleccionado = $.grep(rqsParaOt, function (element, index) {
                        return element.Id == id;
                    }).length != 0;

                    return !seleccionado;
                },
                render: function (data, type, full, meta) {
                    return '<div><p style="margin:0px; padding:0px; margin-top:6px;"><input type="checkbox" value="' + data.Id + '" id="chx' + data.Id + '" ><label style="  padding:0px!important" for="chx' + data.Id + '"></label> </p></div>';
                }
            }
        ],
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function (rq) {
            let idAreaFiltro = getFiltros().IdArea;
            let idAreaRq = rq.AreaId;

            if (idAreaRq != idAreaFiltro) {
                eliminarRequerimientoEnTabla(rq);
                deseleccionar(rq);

                mostrarMensajeAlerta('El requerimiento se ha quitado del listado ya que no forma parte del area ' + ot.NombreArea);
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
    });

    dt.on('click', 'input[type="checkbox"]', function () {
        var data = dt.row($(this).parents("tr")).data();

        if ($(this).is(':checked')) {
            seleccionar(data);
        } else {
            deseleccionar(data);
        }
    });

    dt.$('.tooltipped').tooltip({ delay: 50 });
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    dt.page.len(rows).draw(false);
}

function cargarTabla(data) {
    dt.clear().draw();
    if (data != null) {
        dt.rows.add(data).draw();
    }
    dt.$('.tooltipped').tooltip({ delay: 50 });
}

function eliminarRequerimientoEnTabla(rq) {
    var indexFila = -1;
    $.each(dt.data(), function (index, element) {

        if (element.Id == rq.Id) {
            indexFila = index;
            return;
        }
    });

    if (indexFila == -1) {
        mostrarMensajeAlerta('Error al actualizar la tabla');
        return;
    }

    dt.row(indexFila).remove().draw(false);
    $('.material-tooltip').fadeOut(300);
}


function buscar() {
    if (!validar()) return;

    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTablaParaOrdenTrabajo'),
        Data: { consulta: getFiltros() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensajeError(result.Error);
                return;
            }

            $('#contenedor_Encabezado').addClass('oculto');
            $('#contenedor_Resultado').addClass('visible');

            $('.indicador label').html(getFiltrosTexto());
            setTimeout(function () {
                calcularCantidadDeRows();
                cargarTabla(result.Return.Data);
            }, 500);
        },
        OnError: function (result) {
            mostrarMensajeError('Error procesando la solicitud');
            mostrarCargando(false);
        }
    });
}

function validar() {
    let hayAlgunFiltro = false;

    //Numero
    var numero = "" + $('#input_Numero').val();
    if (numero != "") {
        hayAlgunFiltro = true;
    }

    var anio = "" + $('#input_Año').val();
    if (numero == "" && anio != "") {
        mostrarMensajeError('Debe introducir el número del requerimiento');
        $('#input_Numero').trigger('focus');
        return false;
    }

    if (ControlSelectorRangoFecha_IsDatosIngresados()) {
        hayAlgunFiltro = true;
    }

    if (!hayAlgunFiltro) {
        mostrarMensajeAlerta('Debe seleccionar al menos un filtro');
        return false;
    }


    return hayAlgunFiltro;
}

function getFiltros() {
    let filtros = {};

    if(modo=modo_OT){
        //Area
        filtros.IdArea = o.AreaId;
    }

    //Numero
    var nro = "" + $('#input_Numero').val();
    if (nro != "") {
        filtros.Numero = nro;
    }

    //Año
    var anio = "" + $('#input_Año').val();
    if (nro != "" && anio != "") {
        filtros.Año = anio;
    }

    //Fechas
    var fechaDesde = ControlSelectorRangoFecha_GetFechaDesde();
    var fechaHasta = ControlSelectorRangoFecha_GetFechaHasta();
    if (fechaDesde != undefined && fechaHasta != undefined) {
        filtros.FechaDesde = fechaDesde.toDate();
        filtros.FechaHasta = fechaHasta.toDate();
    }

    return filtros;
}

function getFiltrosTexto() {
    var filtros = "";
    if (primeraVez) {
        primeraVez = false;
        return 'Para OT ' + o.Numero + '/' + o.Año + ' </br> Últimos ' + cantidad + ' requerimientos';
    }
    $.each(getFiltros(), function (key, val) {
        if (val == undefined) return true;

        switch (key) {
            case 'IdArea': {
                key = 'Área';
                if (val == -1 || val == null) return true;
                val = toTitleCase(o.NombreArea);
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

    return 'Para OT ' + o.Numero + '/' + o.Año + ' </br><b>Filtros</b> ' + filtros;
}


function seleccionar(rq) {
    //Verifico si debo agregarlo
    var busqueda = $.grep(rqsParaOt, function (element, index) {
        return element.Numero == rq.Numero;
    });

    if (busqueda == undefined) {
        return;
    }

    if (busqueda.length != 0) {
        mostrarMensajeAlerta('El requerimiento seleccionado ya fue seleccionado');
        return;
    }

    //Lo agrego al listado
    rqsParaOt.push(rq);
}

function deseleccionar(rq) {
    var existente = $.grep(rqsParaOt, function (element, index) {
        return element.Id == rq.Id;
    })[0];

    if (existente == undefined) return;

    rqsParaOt = $.grep(rqsParaOt, function (element, index) {
        return element.Id != rq.Id;
    });
}

function guardar() {
    if (!validarGuardar()) return;

    let data = {
            IdsRequerimientos: _.pluck(rqsParaOt, 'Id')
    };

    var url;
    if (modo == modo_OT) {
        data.IdOrdenTrabajo = o.Id;
        url = ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/AgregarRequerimientos');
    } else {
        data.IdOrdenInspeccion = o.Id;
        url = ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/AgregarRequerimientos');
    }

    mostrarCargando();
    crearAjax({
        Url: url,
        Data: { comando: data },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensajeError(result.Error);
                return;
            }

            if (!result.Return) {
                mostrarCargando(false);
                mostrarMensajeError('Error procesando la solicitud');
                return;
            }

            informar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensajeError('Error procesando la solicitud');
        }
    });
}
function validarGuardar() {
    if (_.pluck(rqsParaOt, 'Id').length == 0) {
        mostrarMensajeError('Debe seleccionar algun requerimiento');
        return false;
    }

    return true;
}

function mostrarError(error) {
    $('#contenedor_Error').addClass('visible');
    $('#contenedor_Error').css('background-color', 'white');
    $('#contenedor_Error label').text(error);
    $('#contenedor_Error label').css('color', 'black');
    $('#contenedor_Error i').css('color', 'var(--colorError');
}



function informar() {
    if (callback == undefined) return;
    callback();
}

function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}