let flotas = [];

let idArea = 0;

let estadoOcupado;

let PATH_IMAGEN_USER_MALE;
let PATH_IMAGEN_USER_FEMALE;

// Init
function init(data) {
    data = parse(data);

    if (data == undefined) {
        mostrarMensaje('error', 'Error cargando la página');
        return;
    }

    estadoOcupado = data.EstadoOcupado;

    PATH_IMAGEN_USER_MALE =
    top.urlCordobaFiles + "/Archivo/" + top.identificadorFotoUserMale + "/3";
    PATH_IMAGEN_USER_FEMALE =
      top.urlCordobaFiles + "/Archivo/" + top.identificadorFotoUserFemale + "/3";

    initSelects();
    initEventos();

    consultaInicial();

    $("#btnNuevaFlota").click(function () {
        crearDialogoFlotaNueva({
            IdArea:  $('#select_Area').val(),
            Callback: function (flota) {
                cargarFlota(flota);
            }
        });
    })

    $("#btnTerminarTurno").click(function () {
        terminarTurno();
    })

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
        $('#contenedor-Area').hide();
    }
}

function initEventos() {
    $('#select_Area').on('change', function () {
        idArea = $("#select_Area").val();
        consultar();
    });

    $('#inputBusqueda').on('keyup', function () {
        filtrar();
    });

}

//Turno
function terminarTurno() {
    if (_.some(flotas, function (f) {
                 return f.EstadoKeyValue == estadoOcupado;
    })) {
        mostrarMensaje("Error", "No puede finalizar el turno si alguna de las flotas se encuentra realizando un trabajo");
        return;
    }

    var ids = _.pluck(flotas, "Id");

    crearDialogoConfirmacion({
        Texto: '¿Está seguro de que quiere finalizar el turno de las flotas actuales?',
        ClassBotonAceptar: 'colorExito',
        CallbackPositivo: function () {
            crearAjax({
                Data: {
                    idsFlotas: ids
                },
                Url: ResolveUrl('~/Servicios/FlotaService.asmx/TerminarTurnoTodasLasFlotas'),
                OnSuccess: function (result) {
                    $("#cardFormularioFiltros .cargando").hide();

                    if (!result.Ok) {
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    initFlotas([]);
                },
                OnError: function (result) {
                    $("#cardFormularioFiltros .cargando").hide();
                    mostrarMensaje('Error', result.Error);
                }
            });
        }
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
        Url: ResolveUrl('~/Servicios/FlotaService.asmx/GetPanel'),
        OnSuccess: function (result) {
            $("#cardFormularioFiltros .cargando").hide();

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            flotas = result.Return;
            initFlotas(flotas);
        },
        OnError: function (result) {
            $("#cardFormularioFiltros .cargando").hide();
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

//Flotas
function initFlotas(data) {
    $('#contenedor_Flotas').removeClass("visible");

    setTimeout(function () {
        $('#contenedor_Flotas').empty();
        // data = ordenarFlotas(data);

        if (data.length > 0) {
            $('#contenedor_Flotas').addClass("visible");

            mostrarCardAreaSinFlotas(false);

            $.each(data, function (index, element) {
                cargarFlota(element);
            });

            $("#inputBusqueda").prop("disabled", false);
        } else {
            mostrarCardAreaSinFlotas(true);
            $("#inputBusqueda").prop("disabled", true);
            $("#btnTerminarTurno").hide();
        }
    }, 300)
}

function cargarFlota(element) {
    var html = crearHtmlFlota(element);
    $('#contenedor_Flotas').addClass("visible");
    mostrarCardAreaSinFlotas(false);
    $("#btnTerminarTurno").show();
    $('#contenedor_Flotas').append(html).show('slow');
}

function crearHtmlFlota(entity) {
    var div = $($('#template_Flota').html());

    $(div).attr("id", entity.Id);

    cargarDatosFlota(entity, div);

    //Evento detalle del flota
    $(div).find(".contenedor_Nombre > .link").click(function () {
        crearDialogoFlotaDetalle({
            Id: entity.Id, Callback: function () {
                actualizarCardFlota(entity.Id);
            }
        })
    });

    // agregarAccion({
    //     Texto: "Editar",
    //     Icono: 'edit',
    //     OnClick: function () {
    //         crearDialogoFlotaEditar({
    //             IdArea: entity.IdArea,
    //             Id: entity.Id,
    //             CallbackMensajes: function (tipo, mensaje) {
    //                 mostrarMensaje(tipo, mensaje);
    //             },
    //             Callback: function (id) {
    //                 actualizarCardFlota(id);
    //             }
    //         });
    //     }
    // });

    //Evento detalle del móvil
    $(div).find('.contenedor_Movil > .btnVerDetalle').click(function () {
        crearDialogoMovilDetalle2({ Id: entity.Movil.Id });
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

    function agregarAccion(valores) {
        var divAccion = $($('#template_Accion').html());
        $(divAccion).find('.texto').text(valores.Texto);

        if (valores.IconoMdi != undefined) {
            $(divAccion).find('.icono').addClass('mdi mdi-' + valores.IconoMdi);
        } else {
            $(divAccion).find('.icono').text(valores.Icono);
        }

        $(divAccion).attr('permiso', valores.PermisoKeyValue);
        $(div).find('.contenedor_Acciones').append(divAccion);
        if (('Permiso' in valores) && !valores.Permiso) {
            $(divAccion).addClass('deshabilitado');
        }
        $(divAccion).click(function () {
            valores.OnClick();
        });
    }
}

function cargarDatosFlota(entity, div) {

    //Nombre
    $(div).find('.contenedor_Nombre .nombre').text(entity.Nombre);

    //Móvil
    $(div).find('.contenedor_Movil > .numero').text(entity.Movil.NumeroInterno);
    $(div).find('.contenedor_Movil > .tipo').text(entity.Movil.NombreTipo);
    $(div).find('.contenedor_Movil > .marca').text(entity.Movil.Marca + ' ' + entity.Movil.Modelo);

    //Estado
    let estadoColor = '#' + entity.EstadoColor;
    $(div).find('.contenedor_Estado > .indicador').css('color', estadoColor);
    $(div).find('.contenedor_Estado > .nombre').text(toTitleCase(entity.EstadoNombre).trim());

    //Divisor de color segun estado
    $(div).find('.separador').css('border-color', estadoColor);

    if (entity.Empleados.length > 0) {
        $(div).find(".contenedor_Personal .contenido").empty();

        //espacio disponible para empleados
        let espacioDisponible = $("#template_Flota").width();
        let tamañoMasEmpleados = $("#template_MasEmpleados").width();

        $.each(entity.Empleados, function (i, empleado) {
            let divEmpleado = crearHtmlEmpleado(empleado);

            //veo si hay espacio disponible, si es asi, lo agrego
            if (espacioDisponible - tamañoMasEmpleados > $(divEmpleado).width()) {
                $(div).find(".contenedor_Personal .contenido").append(divEmpleado);
                espacioDisponible = espacioDisponible - $(divEmpleado).width() - 10;
                return true;
            }

            //si llego aca, no hay más espacio disponible, entonces agrego el más empleados
            let divMasEmpleados = $("#template_MasEmpleados").html();
            divMasEmpleados = $(divMasEmpleados);

            $(divMasEmpleados)
              .find(".observaciones")
              .text("...y " + entity.Empleados.length - i);

            $(div).find(".contenedor_Personal .contenido").append(divMasEmpleados);
            return false;
        })
    }

    if (entity.NumeroOrdenTrabajo == null) {
        $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen .trabajandoEn").text('No está realizando ningún trabajo');
        $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen .OTNumero").text('');
        $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen  .btnVerMasTrabajo ").hide();
        return div;
    }

    var textoDias = '(Hoy)';
    if (entity.EstadoKeyValue != estadoOcupado) {
        $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen .trabajandoEn").text('Último trabajo: ');
        textoDias = "(" + toTitleCase(entity.NombreEstadoOrdenTrabajo) + ")";
    } else {
        if (entity.CantidadDiasOrdenTrabajo != 0) {
            textoDias = '(' + entity.CantidadDiasOrdenTrabajo + ' días)';
        }
    }

    $(div).find(".contenedor_Trabajo > .contenedor_Trabajo_Resumen .OTNumero").text(entity.NumeroOrdenTrabajo);


}

function crearHtmlEmpleado(data) {
    let divPersona = $("#template_Empleado").html();
    divPersona = $(divPersona);

    let foto;
    if (data.IdentificadorFotoPersonal != undefined) {
        foto =
          top.urlCordobaFiles + "/Archivo/" + data.IdentificadorFotoPersonal + "/3";
    } else {
        foto =
          data.SexoMasculino == true
            ? PATH_IMAGEN_USER_MALE
            : PATH_IMAGEN_USER_FEMALE;
    }

    $(divPersona)
      .find(".persona .foto")
      .css("background-image", "url(" + foto + ")");
    //$(divPersona).find('.persona img').attr('src', foto);

    $(divPersona)
      .find(".link")
      .text(data.Nombre + " " + data.Apellido);

    $(divPersona)
      .find(".persona > .foto, .link")
      .click(function () {
          crearDialogoEmpleadoDetalle({
              Id: data.Id
          });
      });

    return divPersona;
}

// function ordenarFlotas(list) {
//     if ($("#select_Estado").val() != -1) {
//         return ordenar(list);
//     }

//     var res = _.where(list, {
//         EstadoKeyValue: estadoOcupado
//     });

//     var flotasOrdenados = [];
//     flotasOrdenados = flotasOrdenados.concat(ordenar(res));
//     $.each(estados, function (i, estado) {
//         if (estado.KeyValue == estadoOcupado) return;
//         res = _.where(list, {
//             EstadoKeyValue: estado.KeyValue
//         });
//         flotasOrdenados = flotasOrdenados.concat(ordenar(res));
//     })

//     return flotasOrdenados;

//     function ordenar(emps) {
//         return emps.sort(function (a, b) {
//             return a.CantidadDiasOrdenTrabajo < b.CantidadDiasOrdenTrabajo;
//         });
//     }
// }

function actualizarCardFlota(id) {
    var div = $(".flota[id=" + id + "]");
    $(div).find(".contenedor_flota_cargando").show();
    $(div).find('.contenedor_Trabajo > .contenedor_Trabajo_MasInfo > .contenido').removeClass('visible');

    var data = { id: id };

    crearAjax({
        Url: ResolveUrl('~/Servicios/FlotaService.asmx/GetResultadoById'),
        Data: data,
        OnSuccess: function (result) {
            $(div).find(".contenedor_flota_cargando").hide();

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            if (result.Return.FechaBaja != null) {
                $.filter(flotas, function (f) {
                    return f.Id != result.Return;
                })

                $(div).remove();
                filtrar();
                return;
            }

            $.each(flotas, function (index, element) {
                if (element.Id == result.Return.Id) {
                    flotas[index] = result.Return;
                    return;
                }
            });

            cargarDatosFlota(result.Return, div);
        },
        OnError: function (result) {
            mostrarMensaje('Error', result.Error);
            $(div).find(".contenedor_flota_cargando").hide();
        }
    });


}

//Detalles
function mostrarMasInfoOT(entity, div) {
    $(div).find('.flota').addClass('cargandoMas');
    consultarMasInfo(entity.IdOrdenTrabajo, function (masInfo) {
        $(div).find('.flota').removeClass('cargandoMas');
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
                            actualizarCardFlota(entity.Id);
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
                            actualizarCardFlota(entity.Id);
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

function verDetalleOT(idOt, idFlota) {
    crearDialogoOrdenTrabajoDetalle({
        Id: idOt,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarCardFlota(idFlota);
        }
    });
}

//Filtros
function filtrar() {
    var texto = $("#inputBusqueda").val();

    var flotasFiltrados = [];
    $.each(flotas, function (i, flota) {
        var cumple = true;

        if (texto != "") {
            if (!filtrarTexto(flota, texto)) {
                setTimeout(function () {
                    eliminarTarjeta(flota.Id);
                }, 300);
                return;
            }
        }

        flotasFiltrados.push(flota);
    })

    if (flotasFiltrados.length == 0) {
        setTimeout(function () { mostrarCardNoCumpleFiltro(true) }, 300);
        return;
    }

    initFlotas(flotasFiltrados);

    function filtrarTexto(flota, textoIngresado) {
        var cumple = true;

        var palabras = textoIngresado.split(" ");

        $.each(palabras, function (i, texto) {
            var textosFlota = flota.Nombre.toUpperCase() + ' ';
            textosFlota += flota.AreaNombre.toUpperCase() + ' ';
            if (flota.Movil != null) {
                textosFlota += flota.Movil.NombreTipo.toUpperCase() + ' ';
                textosFlota += flota.Movil.Modelo.toUpperCase() + ' ';
                textosFlota += flota.Movil.Marca.toUpperCase() + ' ';
                textosFlota += flota.Movil.NumeroInterno.toUpperCase() + ' ';
            }
            if (flota.Empleados != null) {
                $.each(flota.Empleados, function (i, empleado) {
                    textosFlota += empleado.Nombre.toUpperCase() + ' ';
                    textosFlota += empleado.Apellido.toUpperCase() + ' ';
                    textosFlota += empleado.Dni + ' ';
                })
            };

            cumple = textosFlota.includes(texto.toUpperCase());
            if (!cumple) return false;
        })

        return cumple;
    }

    function eliminarTarjeta(id) {
        $("#contenedor_Flotas").find('div[id="' + id + '"]').remove();
    }

    function agregarTarjeta(flota) {
        if ($("#contenedor_Flotas").find('div[id="' + flota.Id + '"]').length == 0) {
            cargarFlota(flota);
        }
    }
}

//Sin elementos
function mostrarCardAreaSinFlotas(mostrar) {
    if (!mostrar) {
        $('#content_SinFlotas').hide();
        return;
    }
    $('#content_SinFlotas').show();
}

function mostrarCardNoCumpleFiltro(mostrar) {
    if (!mostrar) {
        $('#content_SinFlotas').hide();
        return;
    }

    $('#content_SinFlotas > label').text('No hay flotas que cumplan con los filtros ingresados');
    $('#content_SinFlotas > a').hide();
    $('#content_SinFlotas').show();
}

