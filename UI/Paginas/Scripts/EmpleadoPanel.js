let PATH_IMAGEN_USER_MALE;
let PATH_IMAGEN_USER_FEMALE;

let funciones = [];
let estados = [];
let estadoOcupado;

let idArea = 0;

let empleados = [];

// Init
function init(data) {
    data = parse(data);

    if (data == undefined) {
        mostrarMensaje('error', 'Error cargando la página');
        return;
    }

    if (data.Estados == undefined) {
        mostrarMensaje('error', 'Error cargando la página');
        return;
    }

    estados = data.Estados;
    estadoOcupado = data.EstadoOcupado;
    estadoEnFlota = data.EstadoEnFlota;

    PATH_IMAGEN_USER_MALE = top.urlCordobaFiles + '/Archivo/' + top.identificadorFotoUserMale + '/3';
    PATH_IMAGEN_USER_FEMALE = top.urlCordobaFiles + '/Archivo/' + top.identificadorFotoUserFemale + '/3';

    initSelects();
    initEventos();

    consultaInicial();
}

function initSelects() {
    if (usuarioLogeado.Areas.length > 0) {
        //Cargo los datos
        $('#select_Area').CargarSelect({
            Data: usuarioLogeado.Areas,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true
        });
    }

    if (usuarioLogeado.Areas.length == 1) {
        $('#select_Area').hide();
    }

    if (estados.length > 0) {
        //Cargo los datos
        $('#select_Estado').CargarSelect({
            Data: estados,
            Value: 'Id',
            Text: 'Nombre',
            Default: 'Todos',
            Sort: true
        });
    }

    //Inicializo el select de las funciones
    limpiarFunciones();
}

function initEventos() {
    $('#select_Area').on('change', function () {
        idArea = $("#select_Area").val();
        consultar();
    });

    $('#select_Funcion').on('change', function () {
        filtrar();
    });

    $('#select_Estado').on('change', function () {
        filtrar();
    });

    $('#inputBusqueda').on('keyup', function () {
        filtrar();
    });

    $("#content_SinEmpleados > a").click(function () {
        redirigir("PersonalConfiguracion")
    });
}

//Consultas
function consultaInicial() {
    $('#select_Area').trigger('change');
}

function consultar() {
    $("#cardFormularioFiltros .cargando").show();
    crearAjax({
        Data: { consulta: { IdArea: idArea } },
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/GetResultadoTablaPanelByFilters'),
        OnSuccess: function (result) {
            $("#cardFormularioFiltros .cargando").hide();

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            if (result.Return.Data.length != 0) {
                consultarFunciones();
            }

            empleados = result.Return.Data;
            initEmpleados(empleados);
        },
        OnError: function (result) {
            $("#cardFormularioFiltros .cargando").hide();
            mostrarMensaje('Error', result.Error);
        }
    });
}

function consultarFunciones() {
    crearAjax({
        Data: { idArea: idArea },
        Url: ResolveUrl('~/Servicios/FuncionService.asmx/GetByIdArea'),
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            funciones = result.Return;
            cargarFunciones(funciones);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', result.Error);
        }
    });
}

function consultarMasInfo(idOt, callbackPositivo, callbackNegativo) {
    crearAjax({
        Data: { id: idOt },
        Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/GetResultadoPanelMasInfoById'),
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            callbackPositivo(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', result.Error);
            callbackNegativo();
        }
    });
}

//Carga de datos
function cargarFunciones(funciones) {
    if (funciones.length == 0) {
        $('#select_Funcion').prop('disabled', true);
        limpiarFunciones();

        if (usuarioLogeado.Areas.length == 1) {
            $('#select_Funcion').hide();
        }
        return;
    }

    $('#select_Funcion').prop('disabled', false);
    $('#select_Funcion').show();

    //Cargo los datos
    $('#select_Funcion').CargarSelect({
        Data: funciones,
        Value: 'Id',
        Text: 'Nombre',
        Default: 'Todas',
        Sort: true
    });
}

function limpiarFunciones() {
    $('#select_Funcion').CargarSelect({
        Data: [],
        Value: 'Id',
        Text: 'Nombre',
        Default: 'Todas',
        Sort: true
    });
}

//Empleados
function initEmpleados(data) {
    $('#contenedor_Empleados').removeClass("visible");

    setTimeout(function () {
        $('#contenedor_Empleados').empty();
        data = ordenarEmpleados(data);

        if (data.length > 0) {
            $('#contenedor_Empleados').addClass("visible");

            mostrarCardAreaSinEmpleados(false);

            $.each(data, function (index, element) {
                cargarEmpleado(element);
            });
            $("#select_Estado").prop("disabled", false);
            $("#inputBusqueda").prop("disabled", false);
            $("#select_Funcion").prop("disabled", false);
        } else {
            mostrarCardAreaSinEmpleados(true);

            $("#select_Estado").prop("disabled", true);
            $("#inputBusqueda").prop("disabled", true);
            $("#select_Funcion").prop("disabled", true);
        }
    }, 300)
}

function cargarEmpleado(element) {
    var html = crearHtmlEmpleado(element);
    $('#contenedor_Empleados').append(html).show('slow');
}

function crearHtmlEmpleado(entity) {
    var div = $($('#template_Empleado').html());

    $(div).attr("id", entity.Id);

    cargarDatosEmpleado(entity, div);

    //Evento detalle del empleado
    $(div).find(".contenedor_Nombre > .link").click(function () {
        crearDialogoEmpleadoDetalle({
            Id: entity.Id,
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            Callback: function () {
                actualizarCardEmpleado(entity.Id);
            }
        });
    });


    $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen  .OTNumero").click(function () {
        verDetalleOT(entity.IdOrdenTrabajo, entity.Id);
    });

    $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen  .btnVerMasTrabajo ").click(function () {

        if ($(div).find('.contenedor_Trabajo > .contenedor_Trabajo_MasInfo > .contenido').hasClass('visible')) {
            $(div).find('.btnVerMasTrabajo i').text('keyboard_arrow_up');
            $(div).find('.contenedor_Trabajo > .contenedor_Trabajo_MasInfo > .contenido').removeClass('visible');
        } else {
            mostrarMasInfoOT(entity, div);
            $(div).find('.btnVerMasTrabajo i').text('keyboard_arrow_down');
        }
    });

    $(div).find("#btn_VerDetalle").click(function () {
        verDetalleOT(entity.IdOrdenTrabajo);
    });

    return div;
}

function cargarDatosEmpleado(entity, div) {

    //Nombre
    $(div).find('.nombre').text(entity.Nombre + ' ' + entity.Apellido);

    //Dni
    $(div).find('.contenedor_Numero > .dni').text(entity.Dni);

    let foto;
    if (entity.IdentificadorFotoPersonal != undefined) {
        foto = top.urlCordobaFiles + '/Archivo/' + entity.IdentificadorFotoPersonal + '/3';
    } else {
        foto = entity.SexoMasculino == true ? PATH_IMAGEN_USER_MALE : PATH_IMAGEN_USER_FEMALE;
    }
    $(div).find('img').attr('src', foto);

    //Estado
    let estadoColor = '#' + entity.EstadoColor;
    $(div).find('.contenedor_Estado > .indicador').css('color', estadoColor);
    $(div).find('.contenedor_Estado > .nombre').text(toTitleCase(entity.EstadoNombre).trim());


    //Divisor de color segun estado
    $(div).find('.separador').css('border-color', estadoColor);

    //Servicio
    $(div).find('.contenedor_Dni > .nombre').text(entity.Dni);

    //Motivo
    if (entity.Cargo != undefined && entity.Cargo != "") {
        $(div).find('.contenedor_Cargo > .nombre').text(toTitleCase(entity.Cargo).trim());
        $(div).find('.contenedor_Cargo').show();
    } else {
        $(div).find('.contenedor_Cargo').hide();
    }

    if (entity.Funciones != undefined && entity.Funciones != "") {
        $(div).find('.contenedor_Funciones').show();
        $(div).find('.contenedor_Funciones > .nombre').text(toTitleCase(entity.Funciones));
    } else {
        $(div).find('.contenedor_Funciones').hide();
    }

    if (entity.NombreSeccion != undefined & entity.NombreSeccion != "") {
        $(div).find('.contenedor_Seccion > .nombre').text(toTitleCase((entity.NombreSeccion).trim()));
        $(div).find('.contenedor_Seccion').show();
    } else {
        $(div).find('.contenedor_Seccion').hide();
    }

    if (entity.NumeroOrdenTrabajo == null) {
        $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen .trabajandoEn").text('No realizó ningún trabajo');
        $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen  .btnVerMasTrabajo ").hide();
        return div;
    }

    var textoDias = '(Hoy)';
    if (entity.EstadoKeyValue == estadoEnFlota) {

    }
    else if (entity.EstadoKeyValue == estadoOcupado) {
        if (entity.CantidadDiasOrdenTrabajo != 0) {
            textoDias = '(' + entity.CantidadDiasOrdenTrabajo + ' días)';
        }
    } else {
        $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen .trabajandoEn").text('Último trabajo: ');
        textoDias = "(" + toTitleCase(entity.NombreEstadoOrdenTrabajo) + ")";
    }

    $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen .OTNumero").text(entity.NumeroOrdenTrabajo);

    $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen .dias").text(textoDias);

}

function ordenarEmpleados(list) {
    if ($("#select_Estado").val() != -1) {
        return ordenar(list);
    }

    var res = _.where(list, {
        EstadoKeyValue: estadoOcupado
    });

    var empleadosOrdenados = [];
    empleadosOrdenados = empleadosOrdenados.concat(ordenar(res));
    $.each(estados, function (i, estado) {
        if (estado.KeyValue == estadoOcupado) return;
        res = _.where(list, {
            EstadoKeyValue: estado.KeyValue
        });
        empleadosOrdenados = empleadosOrdenados.concat(ordenar(res));
    })

    return empleadosOrdenados;

    function ordenar(emps) {
        return emps.sort(function (a, b) {
            return a.CantidadDiasOrdenTrabajo < b.CantidadDiasOrdenTrabajo;
        });
    }
}

function actualizarCardEmpleado(id) {
    var div = $(".contenedor_empleado[id=" + id + "]");
    $(div).find(".contenedor_empleado_cargando").show();
    $(div).find('.contenedor_Trabajo > .contenedor_Trabajo_MasInfo > .contenido').removeClass('visible');

    crearAjax({
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/GetResultadoTablaPanelById'),
        Data: { id: id },
        OnSuccess: function (result) {
            $(div).find(".contenedor_empleado_cargando").hide();

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            if (result.Return != null) {
                cargarDatosEmpleado(result.Return, div);
            }
        },
        OnError: function (result) {
            mostrarMensaje('Error', result.Error);
            $(div).find(".contenedor_empleado_cargando").hide();
        }
    });


}

//Detalles
function mostrarMasInfoOT(entity, div) {
    $(div).find('.empleado').addClass('cargandoMas');
    consultarMasInfo(entity.IdOrdenTrabajo, function (masInfo) {
        $(div).find('.empleado').removeClass('cargandoMas');
        $(div).find('.contenedor_Trabajo > .contenedor_Trabajo_MasInfo > .contenido').addClass('visible');

        $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_MasInfo .contenido").prop("opacity", 1);

        var textoFecha = $(div).find("#texto_Fecha");
        textoFecha.empty();
        textoFecha.append('<label><b>Fecha de comienzo de OT </b> </label>');
        textoFecha.append('<label>' + dateToString(entity.FechaComienzoOrdenTrabajo) + '</label>');

        var textoCantidad = $(div).find("#texto_CantidadRQ");
        textoCantidad.empty();
        textoCantidad.append('<label><b>Cantidad de requerimientos </b> </label>');
        textoCantidad.append('<label>' + masInfo.CantidadRequerimientos + '</label>');

        var textoZonas = $(div).find('#texto_Zonas');
        textoZonas.empty();
        textoZonas.append('<label><b>Zonas </b> </label>');

        if (masInfo.Zonas != null && masInfo.Zonas.length != 0) {
            var primero = true;

            $.each(masInfo.Zonas, function (i, zona) {
                if (!primero) {
                    textoZonas.append('<label >, </label>');
                }

                primero = false;

                var label = $('<label class="link">' + zona.Nombre + '</label>');

                $(label).click(function () {
                    crearDialogoZona({
                        IdArea: idArea,
                        Id: zona.Id,
                        CallbackMensajes: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje)
                        },
                        Callback: function () {
                            actualizarCardEmpleado(entity.Id);
                        }
                    });
                });

                textoZonas.append(label);
            })
        } else {
            textoZonas.append('Sin zonas registradas');
        }

        var textoMoviles = $(div).find('#texto_Moviles');
        textoMoviles.empty();
        textoMoviles.append('<label><b>Flota </b> </label>');

        if (masInfo.Moviles != null && masInfo.Moviles.length != 0) {
            var primero = true;
            $.each(masInfo.Moviles, function (i, movil) {
                if (!primero) {
                    textoMoviles.append('<label >, </label>');
                }

                primero = false;

                var label = $('<label class="link">' + movil.NumeroInterno + '</label>');

                textoMoviles.append(label);

                $(label).click(function () {
                    crearDialogoMovilDetalle2({
                        Id: movil.MovilId,
                        CallbackMensajes: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje)
                        },
                        Callback: function () {
                            actualizarCardEmpleado(entity.Id);
                        }
                    });
                });
            })
        } else {
            textoMoviles.append('Sin móviles registrados');
        }

    }, function () {

    })
}

function verDetalleOT(idOt, idEmpleado) {
    crearDialogoOrdenTrabajoDetalle({
        Id: idOt,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarCardEmpleado(idEmpleado);
        }
    });
}

//Filtros
function filtrar() {
    var estado = $("#select_Estado").val();
    var funcion = $("#select_Funcion option:selected").text();
    var funcionValue = $("#select_Funcion").val();
    var texto = $("#inputBusqueda").val();

    var empleadosFiltrados = [];
    $.each(empleados, function (i, empleado) {
        var cumple = true;
        if (estado != -1 && estado != empleado.EstadoKeyValue) {
            setTimeout(function () {
                eliminarTarjeta(empleado.Id);
            }, 300);
            return;
        }

        if (funcionValue != -1) {
            if (empleado.Funciones == null || !empleado.Funciones.toUpperCase().includes(funcion.toUpperCase())) {
                setTimeout(function () {
                    eliminarTarjeta(empleado.Id);
                }, 300);
                return;
            }
        }

        if (texto != "") {
            if (!filtrarTexto(empleado, texto)) {
                setTimeout(function () {
                    eliminarTarjeta(empleado.Id);
                }, 300);
                return;
            }
        }

        empleadosFiltrados.push(empleado);
    })

    if (empleadosFiltrados.length == 0) {
        mostrarCardNoCumpleFiltro(true);
        return;
    }

    initEmpleados(empleadosFiltrados);

    function filtrarTexto(empleado, textoIngresado) {
        var cumple = true;

        var palabras = textoIngresado.split(" ");

        $.each(palabras, function (i, texto) {
            var textosEmpleado = empleado.Apellido.toUpperCase() + ' ';
            textosEmpleado += empleado.Nombre.toUpperCase() + ' ';
            if (empleado.Cargo != null) { textosEmpleado += empleado.Cargo.toUpperCase() + ' '; }
            textosEmpleado += empleado.Dni + ' ';
            if (empleado.NumeroOrdenTrabajo != null) { textosEmpleado += empleado.NumeroOrdenTrabajo.toUpperCase() + ' ' };
            if (empleado.NombreSeccion != null) { textosEmpleado += empleado.NombreSeccion.toUpperCase() + ' ' };

            cumple = textosEmpleado.includes(texto.toUpperCase());
            if (!cumple) return false;
        })

        return cumple;
    }

    function eliminarTarjeta(id) {
        $("#contenedor_Empleados").find('div[id="' + id + '"]').remove();
    }

    function agregarTarjeta(empleado) {
        if ($("#contenedor_Empleados").find('div[id="' + empleado.Id + '"]').length == 0) {
            cargarEmpleado(empleado);
        }
    }
}

//Sin elementos
function mostrarCardAreaSinEmpleados(mostrar) {
    if (!mostrar) {
        $('#content_SinEmpleados').hide();
        return;
    }

    $('#content_SinEmpleados > label').text('No posee ningún empleado en el área');
    $('#content_SinEmpleados > a').show();
    $('#content_SinEmpleados').show();
}

function mostrarCardNoCumpleFiltro(mostrar) {
    if (!mostrar) {
        $('#content_SinEmpleados').hide();
        return;
    }

    $('#content_SinEmpleados > label').text('No hay empleados que cumplan con los filtros ingresados');
    $('#content_SinEmpleados > a').hide();
    $('#content_SinEmpleados').show();
}

