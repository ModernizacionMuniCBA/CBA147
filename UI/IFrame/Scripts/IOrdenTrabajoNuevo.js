var callbackGuardado;

var zona;

var requerimientos;
var secciones;
var area;
var cantidadMoviles;
var cantidadPersonal;
var cantidadFlotas;

var moviles = [];
var personal = [];
var flotas = [];
var estadosOT = [];
var estadosRQ = [];
var estadoOTPorDefecto;

function init(_idsRequerimientos) {

    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/Init'),
        Data: { idsRequerimientos: _idsRequerimientos },
        OnSuccess: function (result) {

            mostrarCargando(false);

            if (result == undefined) {
                mostrarMensaje('Error', 'Error procesando la solicitud');
                return;
            }

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            requerimientos = result.Return.Requerimientos;
            area = result.Return.Area;
            secciones = result.Return.Secciones;
            cantidadMoviles = result.Return.CantidadMoviles;
            cantidadPersonal = result.Return.CantidadPersonal;
            cantidadFlotas = result.Return.CantidadFlotas;
            estadosRQ = result.Return.EstadosRequerimiento;
            estadosOT = result.Return.EstadosOrdenTrabajo;
            estadoOTPorDefecto = result.Return.EstadoOrdenTrabajoPorDefecto;

            if (requerimientos == undefined) {
                mostrarMensaje("Error", "Error inicializando");
                return;
            }

            if (area == undefined) {
                mostrarMensaje("Error", "Error inicializando");
                return;
            }

            var hayAlgunaTab = false;

            if (cantidadMoviles === 0) {
                $('#btnTabMoviles').hide();
            }
            else {
                initTablaMoviles();
                hayAlgunaTab = true;
            }

            if (cantidadPersonal === 0) {
                $('#btnTabPersonal').hide();
            }
            else {
                initTablaPersonal();
                hayAlgunaTab = true;
            }


            if (cantidadFlotas === 0) {
                $('#btnTabFlotas').hide();
            }
            else {
                initTablaFlotas();
                hayAlgunaTab = true;
            }

            if (!hayAlgunaTab) {
                $(".contenedorTabs").hide();
            }

            inicializar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });

    $('.tabs a').click(function () {
        setTimeout(function () {
            informarTabChange();
        }, 10);
    });
}

function inicializar() {
    $('#textoDiferenteZona').hide(300);

    if (esAmbitoMunicipalidad()) {
        //Estados de OT
        $("#contenedor-estadosOT").show();
        $.each(estadosOT, function (index, data) {
            $('#checkboxEstadosOT').AgregarCheckbox({
                Name: data.Nombre,
                Value: 'OT'+data.Id,
            });

            var chequear = false;
            if (estadoOTPorDefecto == undefined) {
                if (index == 0) {
                    chequear = true;
                }
            } else if (estadoOTPorDefecto.Id == data.Id) {
                chequear = true;
            }

            $('#checkboxEstadosOT').find('#cbOT' + data.Id).prop("checked", chequear);

            $('#checkboxEstadosOT').find('#cbOT'+ data.Id).click(function () {
                $('#checkboxEstadosOT').find('[type="checkbox"]').prop("checked", false);
                $('#checkboxEstadosOT').find('#cbOT' + data.Id).prop("checked", true);
            })

            $('#checkboxEstadosOT').find('#cblbOT' + data.Id).html('<div class="indicador-estado" style="background-color: #' + data.Color + '"/>' + toTitleCase(data.Nombre));
        });

        //Estados de RQ
        $("#contenedor-estadosRQ .titulo").text('Establecer estado de requerimientos a...');

        var primero = true;
        $.each(estadosRQ, function (index, data) {
            $('#checkboxEstadosRQ').AgregarCheckbox({
                Name: data.Nombre,
                Value: 'RQ'+data.Id,
            });

            if (primero) {
                $('#checkboxEstadosRQ').find('#cbRQ' + data.Id).prop("checked", true);
            }

            $('#checkboxEstadosRQ').find('#cbRQ' + data.Id).click(function () {
                $('#checkboxEstadosRQ').find('[type="checkbox"]').prop("checked", false);
                $('#checkboxEstadosRQ').find('#cbRQ' + data.Id).prop("checked", true);
            })

            primero = false;
            $('#checkboxEstadosRQ').find('#cblbRQ' + data.Id).html('<div class="indicador-estado" style="background-color: #' + data.Color + '"/>' + toTitleCase(data.Nombre));
        });
    } else {
        $("#contenedor-estadosRQ .titulo").text('Los requerimientos pasarán a estado...');
        $("#checkboxEstadosRQ").append('<label style="width: 180px;"><div class="indicador-estado" style="background-color: #' + estadosRQ[0].Color + '"></div>' + estadosRQ[0].Nombre + '</label>');
    }

    $.each(requerimientos, function (index, element) {
        var div = $('<div class="card card-requerimiento">');
        div.attr('id-rq', element.Id);

        var textos = $('<div class="textos">');
        $(textos).appendTo(div);

        var textoNumero = $('<label class="numero">').text(element.Numero);
        $(textoNumero).appendTo(textos);

        var textoMotivo = $('<label class="motivo">').text(element.MotivoString);
        $(textoMotivo).appendTo(textos);

        var btn = $('<a class="btn-flat waves-effect btn-redondo"><i class="material-icons">more_vert</i></a>');
        $(btn).appendTo(div);

        $(btn).click(function () {
            $(btn).MenuFlotante({
                Menu: [
                    {
                        Texto: 'Detalle',
                        Icono: 'description',
                        OnClick: function () {
                            crearDialogoDetalleRequerimiento({
                                Id: element.Id,
                                CallbackMensajes: function (tipo, mensaje) {
                                    mostrarMensaje(tipo, mensaje);
                                }
                            });
                        }
                    },
                    {
                        Texto: 'Quitar',
                        Icono: 'clear',
                        OnClick: function () {
                            if (requerimientos.length == 1) {
                                mostrarMensaje('Alerta', 'La orden de trabajo debe tener al menos un requerimiento');
                                return;
                            }
                            quitarRequerimiento(element.Id);
                        }
                    }
                ]
            });

        });

        $('#contenedor-requerimientos').append(div);
    });

    ////Zona
    //$("#inputFormulario_SelectZona").CargarSelect({
    //    Data: [],
    //    Default: 'Ninguna',
    //    Value: 'id',
    //    Text: 'nombre',
    //    Sort: true
    //});
    //$("#inputFormulario_SelectZona").prop('disabled', true);
    //buscarZonas();

    //Secciones
    if (secciones == undefined || secciones.length == 0) {
        $("#contenedor-seccion").hide();
        return;
    }

    $("#inputFormulario_SelectSeccion").CargarSelect({
        Data: secciones,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });
}

function quitarRequerimiento(id) {
    requerimientos = $.grep(requerimientos, function (element, index) {
        return element.Id != id;
    });

    $('.card-requerimiento[id-rq=' + id + ']').hide(300, function () {
        $('.card-requerimiento[id-rq=' + id + ']').remove();
    });

    validarZona();
}

function validarZona() {
    zona = getZonaDeRequerimientos();

    //Seteo la zona
    if (zona != undefined) {
        $('#textoDiferenteZona').hide(300);
    } else {
        if (requerimientos.length == 1) {
            $('#textoDiferenteZona').hide(300);
        } else {
            $('#textoDiferenteZona').show(300);
        }
    }
}

function getZonaDeRequerimientos() {
    zona = undefined;
    var soniguales = true;
    $.each(requerimientos, function (index, element) {
        if (element.Zona != undefined) {
            if (index == 0) {
                zona = element.Zona;
            }

            if (zona == null) {
                if (element.Zona != null) {
                    soniguales = false;
                } else {

                }
            } else {
                if (element.Zona == null) {
                    soniguales = false;
                } else {
                    if (element.Zona.Id != zona.Id) {
                        soniguales = false;
                    }
                }
            }
        }
    });

    return soniguales ? zona : undefined;
}

//function buscarZonas() {
//    mostrarCargando('true', '');

//    if (area.Id == -1) {
//        $('#inputFormulario_SelectZona').prop('disabled', true);
//        $('#inputFormulario_SelectZona').CargarSelect({
//            Data: [],
//            Value: 'Id',
//            Text: 'Nombre',
//            Sort: true,
//            Default: 'Seleccione...'
//        });
//    } else {
//        $('#inputFormulario_SelectZona').prop('disabled', true);
//        $('#inputFormulario_SelectZona').prop('disabled', true);

//        $.ajax({
//            type: "POST",
//            dataType: "json",
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify({ idArea: area.Id }),
//            url: ResolveUrl('~/Servicios/ZonaService.asmx/GetPorArea'),
//            success: function (result) {
//                result = parse(result.d);


//                //algo salio mal
//                if ('Error' in result) {
//                    //Informo
//                    mostrarMensaje('Error', result.Error.Publico);

//                    $('#inputFormulario_SelectZona').prop('disabled', false);
//                    $('#inputFormulario_SelectZona').prop('disabled', true);
//                    return;
//                }

//                $('#inputFormulario_SelectZona').prop('disabled', false);
//                $('#inputFormulario_SelectZona').prop('disabled', false);

//                $('#inputFormulario_SelectZona').CargarSelect({
//                    Data: result.Zonas,
//                    Value: 'Id',
//                    Text: 'Nombre',
//                    Sort: true,
//                    Default: 'Seleccione...'
//                });

//                buscarZonasDeRequerimientos();
//            },
//            error: function (result) {
//                //Informo
//                mostrarMensaje('Error', 'Error al realizar la consulta');

//                $('#inputFormulario_SelectZona').prop('disabled', false);
//                $('#inputFormulario_SelectZona').prop('disabled', true);
//            }
//        });
//    }
//}

function buscarZonasDeRequerimientos() {
    zona = "sindefinir";

    $.each(requerimientos, function (index, element) {
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ id: element.Id }),
            url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetZona'),
            success: function (result) {
                result = parse(result.d);

                //algo salio mal
                if ('Error' in result) {
                    //Informo
                    mostrarMensaje('Error', result.Error.Publico);

                    //$('#inputFormulario_SelectZona').prop('disabled', false);
                    //$('#inputFormulario_SelectZona').prop('disabled', true);
                    return;
                }

                element.Zona = result.Zona;

                if (element.Zona != undefined) {
                    var textos = $('.card-requerimiento[id-rq=' + element.Id + ']').find('.textos');
                    var textoZona = $('<div class="zona">').text('Zona: ' + toTitleCase(result.Zona.Nombre));
                    $(textoZona).appendTo(textos);
                }

                if (index == requerimientos.length - 1) {
                    setTimeout(function () {
                        mostrarCargando(false, '');
                        validarZona();
                    }, 200);
                }
            },
            error: function (result) {
                //Informo
                mostrarMensaje('Error', 'Error al realizar la consulta');
            }
        });
    });


}

function getOrdenTrabajo() {
    var ot = {};

    ot.Recursos = getRecursos();

    //Requerimientos
    ot.IdRequerimientos = [];
    $.each(requerimientos, function (index, element) {
        ot.IdRequerimientos.push(element.Id);
    });

    //Seccion
    var idSeccion = $('#inputFormulario_SelectSeccion').val();
    if (idSeccion != -1) {
        ot.IdSeccion = idSeccion;
    } else {
        ot.IdSeccion = null;
    }

    //Area
    ot.IdArea = area.Id;

    //Moviles
    ot.Moviles = getIdsMoviles();

    //Personal
    ot.Personal = getIdsPersonal();

    //Flotas
    ot.Flotas = getIdsFlotas();

    //Flotas
    ot.Flotas = getIdsFlotas();

    //Descripcion
    ot.Descripcion = $('#inputFormulario_Descripcion').val();

    //Estado OT
    var controles = $('#checkboxEstadosOT').find("input[type='checkbox']");
    $.each(controles, function (index, checkbox) {
        if ($(checkbox).is(":checked")) {
            var estado = $.grep(estadosOT, function (element, index) {
                return 'OT'+element.Id == $(checkbox).val();
            })[0];

            if (estado != undefined) {
                ot.KeyValueEstadoOrdenTrabajo = estado.KeyValue;
            }

            return false;
        }
    });

    //Estado RQ
    controles = $('#checkboxEstadosRQ').find("input[type='checkbox']");
    $.each(controles, function (index, checkbox) {
        if ($(checkbox).is(":checked")) {
            var estado = $.grep(estadosRQ, function (element, index) {
                return 'RQ' + element.Id == $(checkbox).val();
            })[0];

            if (estado != undefined) {
                ot.KeyValueEstadoRequerimiento = estado.KeyValue;
            }

            return false;
        }
    });

    //UserAgent
    ot.UserAgent = navigator.userAgent;

    //Tipo Cliente
    if (isMobileOrTablet()) {
        if (isMobile()) {
            ot.TipoDispositivo = 1;
        } else {
            ot.TipoDispositivo = 2;
        }
    } else {
        ot.TipoDispositivo = 3;
    }

    return ot;
}

function getRecursos() {
    var recurso = {
        Material: $('#inputFormulario_Material').val(),
        Flota: $('#inputFormulario_Flota').val(),
        Personal: $('#inputFormulario_Personal').val(),
        Observaciones: $('#inputFormulario_RecursoObservacion').val(),
    };

    //if ($('#inputFormulario_SelectZona').val() != -1) {
    //    recurso.Zona = $('#inputFormulario_SelectZona option:selected').text();
    //} else {
    //    recurso.Zona = null;
    //}

    return recurso;
}

//----------------------------
// Listener Guardar
//----------------------------

function guardar() {
    mostrarCargando(true);

    crearAjax({
        Data: { comando: getOrdenTrabajo() },
        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/Insertar'),
        OnSuccess: function (result) {
            //algo salio mal
            //if (!result.Ok) {
            //    mostrarCargando(false);
            //    //Informo
            //    mostrarMensaje('Error', result.Error.Publico);
            //    return;
            //}

            //callbackGuardado(result.Return);

            //algo salio mal
            if (!result.Ok) {
                mostrarCargando(false);
                //Informo
                mostrarMensaje('Error', result.Error);
                return;
            }

            callbackGuardado(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            //Informo
            mostrarMensaje('Error', 'Error registrando la orden de trabajo');
        }
    });
}

//Moviles 
function initTablaMoviles() {
    $('#tablaMoviles').DataTableMovil(
        {
            CallbackQuitar: function (data) {
                var i = -1;
                $.each(moviles, function (index, value) {
                    if (data.Movil.Id == value.Id) {
                        i = index;
                        return;
                    }
                });

                if (i > -1) {
                    moviles.splice(i, 1);
                }
                if (moviles != undefined && moviles != null && moviles.length != 0) {
                    if (moviles.length == 1) {
                        $('#textoTabMoviles').text('(' + moviles.length + ') MÓVIL');
                    } else {
                        $('#textoTabMoviles').text('(' + moviles.length + ') MÓVILES');
                    }
                } else {
                    $('#textoTabMoviles').text('MÓVILES');
                }
            }
        });

    //Muevo el indicador y el paginado a mi propio div
    $('#tabMoviles').find('.tabla-footer').empty();
    $('#tabMoviles').find('.dataTables_info').detach().appendTo($('#tabMoviles').find('.tabla-footer'));
    $('#tabMoviles').find('.dataTables_paginate').detach().appendTo($('#tabMoviles').find('.tabla-footer'));
}

function cargarMoviles(data) {
    var dt = $('#tablaMoviles').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    if (data != undefined && data != null && data.length != 0) {
        if (data.length == 1) {
            $('#textoTabMoviles').text('(' + data.length + ') MÓVIL');
        } else {
            $('#textoTabMoviles').text('(' + data.length + ') MÓVILES');
        }
    } else {
        $('#textoTabMoviles').text('MÓVILES');
    }
    calcularCantidadDeRowsMoviles();
}

function calcularCantidadDeRowsMoviles() {
    var hDisponible = $('#tabMoviles').find('.tabla-contenedor').height();

    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tablaMoviles').DataTable();
    dt.page.len(rows).draw();
}

var idMovilOt = 1;
function agregarMovil() {
    crearDialogoAgregarMovil({
        CallbackObjetosSeleccionar: function (data) {
            moviles = data;
            cargarMoviles(moviles);
        },
        IdArea: area.Id,
        IdsMoviles: getIdsMoviles()
    });
}

function getIdsMoviles() {
    var ids = [];
    $.each(moviles, function (i, elemento) {
        ids.push(elemento.Id);
    })

    return ids;
}

function addMoviles(moviles) {
    var mxot = [];
    $.each(moviles, function (i, m) {
        var movil = {};
        movil.Movil = m;
        movil.FechaAltaString = moment().format('DD/MM/YYYY');
        mxot.push(movil);
    });

    cargarMoviles(mxot);
}


//Personal 
function initTablaPersonal() {
    $('#tablaPersonal').DataTableEmpleado(
        {
            CallbackQuitar: function (data) {
                var i = -1;
                $.each(personal, function (index, value) {
                    if (data.Empleado.Id == value.Id) {
                        i = index;
                        return;
                    }
                });

                if (i > -1) {
                    personal.splice(i, 1);
                }
                if (personal != undefined && personal != null && personal.length != 0) {
                    $('#textoTabPersonal').text('(' + personal.length + ') PERSONAL');
                } else {
                    $('#textoTabPersonal').text('PERSONAL');
                }
            }
        });

    //Muevo el indicador y el paginado a mi propio div
    $('#tabPersonal').find('.tabla-footer').empty();
    $('#tabPersonal').find('.dataTables_info').detach().appendTo($('#tabPersonal').find('.tabla-footer'));
    $('#tabPersonal').find('.dataTables_paginate').detach().appendTo($('#tabPersonal').find('.tabla-footer'));
}

function cargarPersonal(data) {
    var dt = $('#tablaPersonal').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    if (data != undefined && data != null && data.length != 0) {
        $('#textoTabPersonal').text('(' + data.length + ') PERSONAL');
    } else {
        $('#textoTabPersonal').text('PERSONAL');
    }
    calcularCantidadDeRowsPersonal();
}

function calcularCantidadDeRowsPersonal() {
    var hDisponible = $('#tabPersonal').find('.tabla-contenedor').height();

    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tablaPersonal').DataTable();
    dt.page.len(rows).draw();
}

var idEmpleadoOt = 1;
function agregarEmpleado() {
    crearDialogoAgregarEmpleado({
        CallbackObjetosSeleccionar: function (data) {
            personal = data;
            cargarPersonal(personal);
        },
        IdArea: area.Id,
        IdsEmpleados: getIdsPersonal()
    });
}

function getIdsPersonal() {
    var ids = [];
    $.each(personal, function (i, elemento) {
        ids.push(elemento.Id);
    })

    return ids;
}

function addPersonal(personal) {
    var mxot = [];
    $.each(personal, function (i, m) {
        var empleado = {};
        empleado.Empleado = m;
        empleado.FechaAltaString = moment().format('DD/MM/YYYY');
        mxot.push(empleado);
    });

    cargarPersonal(mxot);
}


function setOnGuardadoListener(callback) {
    this.callbackGuardado = callback;
}


//Flotas 
function initTablaFlotas() {
    $('#tablaFlotas').DataTableFlota(
        {
            CallbackQuitar: function (data) {
                var i = -1;
                $.each(flotas, function (index, value) {
                    if (data.Flota.Id == value.Id) {
                        i = index;
                        return;
                    }
                });

                if (i > -1) {
                    flotas.splice(i, 1);
                }
                if (flotas != undefined && flotas != null && flotas.length != 0) {
                    $('#textoTabFlotas').text('(' + flotas.length + ') PERSONAL');
                } else {
                    $('#textoTabFlotas').text('PERSONAL');
                }
            }
        });

    //Muevo el indicador y el paginado a mi propio div
    $('#tabFlotas').find('.tabla-footer').empty();
    $('#tabFlotas').find('.dataTables_info').detach().appendTo($('#tabFlotas').find('.tabla-footer'));
    $('#tabFlotas').find('.dataTables_paginate').detach().appendTo($('#tabFlotas').find('.tabla-footer'));
}

function cargarFlotas(data) {
    var dt = $('#tablaFlotas').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    if (data != undefined && data != null && data.length != 0) {
        $('#textoTabFlotas').text('(' + data.length + ') FLOTAS');
    } else {
        $('#textoTabFlotas').text('FLOTAS');
    }
    calcularCantidadDeRowsFlotas();
}

function calcularCantidadDeRowsFlotas() {
    var hDisponible = $('#tabFlotas').find('.tabla-contenedor').height();

    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tablaFlotas').DataTable();
    dt.page.len(rows).draw();
}

var idFlotaOt = 1;
function agregarFlota() {
    crearDialogoAgregarFlota({
        CallbackObjetosSeleccionar: function (data) {
            flotas = data;
            cargarFlotas(flotas);
        },
        IdArea: area.Id,
        IdsFlotas: getIdsFlotas()
    });
}

function getIdsFlotas() {
    var ids = [];
    $.each(flotas, function (i, elemento) {
        ids.push(elemento.Id);
    })

    return ids;
}

function addFlotas(flotas) {
    var mxot = [];
    $.each(flotas, function (i, m) {
        var f = {};
        f.Flota = m;
        f.FechaAltaString = moment().format('DD/MM/YYYY');
        mxot.push(empleado);
    });

    cargarFlotas(mxot);
}


function setOnGuardadoListener(callback) {
    this.callbackGuardado = callback;
}

//-----------------------------
// Listener Tab Change
//-----------------------------

function informarTabChange() {
    if (callbackTab != null) {
        var tab = $('.tabs .active');
        var id = tab.attr('href');
        callbackTab(id);
    }
}
function setOnTabChangeListener(callback) {
    this.callbackTab = callback;
}