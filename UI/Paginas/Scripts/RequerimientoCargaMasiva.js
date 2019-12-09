let modo;
let MODO_REQUERIMIENTO_INTERNO = "rq_interno";
let MODO_NORMAL = "rq_normal";
let cardFormularioCargando;

let domicilioSeleccionado;
let estados;
let estadoNuevo;

let idRq;

function init(data) {
    $("#cardFormulario").AgregarIndicadorCargando({ Opaco: true });
    cardFormularioCargando = $("#cardFormulario").GetIndicadorCargando();

    modo = MODO_REQUERIMIENTO_INTERNO;
    initModo();

    initFormulario();
    initAlerta();
    SelectorMotivo_Init({
        CallbackModo: function (modoManual) {
            $("#errorFormulario_Motivo")
              .stop(true, true)
              .hide(300);
        },
        CallbackMensaje: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje)
        },
        Callback: function (motivo) {
            $("#errorFormulario_Motivo")
              .stop(true, true)
              .hide(300);
            $("#errorFormulario_Descripcion")
              .stop(true, true)
              .hide(300);

            if (motivo != undefined && motivo != null) {
                $("#contenedorDescripcionMotivo").addClass("visible");
                $("#inputFormulario_Descripcion").trigger("focus");
            } else {
                $("#contenedorDescripcionMotivo").removeClass("visible");
            }
        }
    });

    $("#cardFormulario").addClass("visible");
}

function initModo() {
    switch (modo) {
        case MODO_REQUERIMIENTO_INTERNO:
            {
                $("#cardFormulario .contenedor-header .titulo").text(
                  "Nueva carga masiva"
                );
                $(".contenedor-mantener").show();
            }
            break;

        case MODO_NORMAL:
            {
                $("#cardFormulario .contenedor-header .titulo").text(
                  "Nuevo requerimiento"
                );
                $(".contenedor-mantener").hide();
            }
            break;
    }
}

function initAlerta() {
    $("#btn_CerrarAlerta").click(function () {
        $("#cardAlerta").removeClass("visible");
        idRq = undefined;
    });

    $("#cardAlerta .link").click(function () {
        if (idRq == undefined) return;

        crearDialogoRequerimientoDetalle({
            Id: idRq
        });
    });

    $("#btn_ReenviarComprobante").click(function () {
        crearDialogoRequerimientoReenviarComprobante({
            Id: idRq
        });
    });

    $("#btn_Imprimir").click(function () {
        crearDialogoReporteRequerimientoNuevo({ Id: idRq });
    });
}

function initFormulario() {
    estadoNuevo = _.find(getInitData().Requerimiento.Estados, { KeyValue: 1 });

    jQuery.validator.addMethod(
      "inputDescripcion",
      function (value, element) {
          if (SelectorMotivo_GetMotivoSeleccionado() != undefined) {
              return value != "";
          }
          return true;
      },
      "Dato requerido."
    );

    jQuery.validator.addMethod(
      "selectEstado",
      function (value, element) {
          return parseInt(value) != -1;
      },
      "Dato requerido."
    );

    jQuery.validator.addMethod(
      "inputDescripcionEstado",
      function (value, element) {
          if (
            $("#selectFormulario_Estado").val() != -1 &&
            $("#selectFormulario_Estado").val() != estadoNuevo.Id
          ) {
              return value != "";
          } else {
              return true;
          }
      },
      "Dato requerido."
    );

    $("#formRequerimiento").validate({
        rules: {
            input_Descripcion: {
                inputDescripcion: true
            },
            select_Estado: {
                selectEstado: true
            },
            input_DescripcionEstado: {
                inputDescripcionEstado: true
            }
        }
    });

    initMotivo();
    initDomicilio();
    initEstado();

    $("#btn_Registrar").click(function () {
        registrar();
    });

    $("#btn_Limpiar").click(function () {
        limpiar();
    });
}

let idAreaInicial;

function initMotivo() {
    $("#selectFormulario_Area").CargarSelect({
        Data: getUsuarioLogeado().Areas,
        Value: "Id",
        Text: "Nombre"
    });

    $("#selectFormulario_Area").on("change", function () {
        //SelectorMotivo_SetArea($(this).val());
    });

    setTimeout(function () {
        idAreaInicial = $("#selectFormulario_Area").val();
        $("#selectFormulario_Area").trigger("change");
    }, 300);
}

function initDomicilio() {
    //--------------------------------
    // Ubicacion del reclamo
    //--------------------------------

    $("#btn_SeleccionarDomicilio").click(function () {
        $("#errorFormulario_Domicilio")
          .stop(true, true)
          .slideUp(300);

        crearDialogoUbicacionSelector({
            Callback: function (domicilio) {
                domicilioSeleccionado = domicilio;

                $("#contenedor_DomicilioNoSeleccionado")
                  .stop(true, true)
                  .fadeOut(300, function () {
                      $("#contenedor_DomicilioSeleccionado")
                        .stop(true, true)
                        .fadeIn(300);
                      if (domicilio.Direccion != undefined) {
                          $("#texto_DomicilioTitulo").html(
                            "<b>Dirección: </b>" + domicilio.Direccion
                          );
                      } else {
                          $("#texto_DomicilioTitulo").text("");
                      }

                      if (domicilio.Observaciones != undefined) {
                          $("#texto_DomicilioDescripcion").html(
                            "<b>Descripción: </b>" + domicilio.Observaciones
                          );
                      } else {
                          $("#texto_DomicilioDescripcion").html(
                            "<b>Descripción: </b> Sin datos"
                          );
                      }

                      if (domicilio.Barrio != undefined) {
                          $("#texto_DomicilioBarrio").html(
                            "<b>Barrio: </b>" + toTitleCase(domicilio.Barrio.Nombre)
                          );
                      } else {
                          $("#texto_DomicilioBarrio").html("<b>Barrio: </b>Sin datos");
                      }

                      if (domicilio.Cpc != undefined) {
                          $("#texto_DomicilioCpc").html(
                            "<b>Cpc: </b>N° " +
                              domicilio.Cpc.Numero +
                              " - " +
                              toTitleCase(domicilio.Cpc.Nombre)
                          );
                      } else {
                          $("#texto_DomicilioCpc").html("<b>Cpc: </b>Sin datos");
                      }

                      let url =
                       
                        domicilio.Latitud.replace(",", ".") +
                        "," +
                        domicilio.Longitud.replace(",", ".");
                      $("#contenedor_DomicilioSeleccionado .mapa").css(
                        "background-image",
                        "url(" + url + ")"
                      );
                  });
            }
        });
    });

    $("#btn_CancelarDomicilio").click(function () {
        $("#errorFormulario_Domicilio")
          .stop(true, true)
          .slideUp(300);

        domicilioSeleccionado = undefined;

        $("#contenedor_DomicilioSeleccionado")
          .stop(true, true)
          .fadeOut(300, function () {
              $("#contenedor_DomicilioNoSeleccionado")
                .stop(true, true)
                .fadeIn(300);
          });
    });
}

function initEstado() {
    let keysEstados = _.pluck(
      _.where(getInitData().Requerimiento.Permisos, {
          Permiso: 21,
          TienePermiso: true
      }),
      "EstadoRequerimiento"
    );
    estados = [];

    $.each(getInitData().Requerimiento.Estados, function (index, e) {
        if (keysEstados.indexOf(e.KeyValue) != -1) {
            estados.push(e);
        }
    });

    $.each(estados, function (index, data) {
        data.html =
          '<div><div class="    display: flex !important; "><label class="indicador-estado" style="background-color: #' +
          data.Color +
          '"></label><span>' +
          toTitleCase(data.Nombre) +
          "</span></div></div>";
    });

    $("#selectFormulario_Estado").CargarSelect({
        Data: estados,
        Default: "Seleccione...",
        Value: "Id",
        Text: "Nombre"
    });

    $("#selectFormulario_Estado")
      .val(estadoNuevo.Id)
      .trigger("change");

    $("#selectFormulario_Estado").on("change", function () {
        let id = $(this).val();
        if (id == -1 || id == estadoNuevo.Id) {
            $("#contenedorFormulario_EstadoDescripcion").removeClass("visible");
            $("#inputFormulario_EstadoDescripcion").val("");
            Materialize.updateTextFields();
        } else {
            $("#contenedorFormulario_EstadoDescripcion").addClass("visible");
            setTimeout(function () {
                $("#inputFormulario_EstadoDescripcion").trigger("focus");
            }, 300);
        }
    });
}

function registrar() {
    if (!validar()) return;

    cardFormularioCargando.addClass("visible");
    $("#cardAlerta").removeClass("visible");

    switch (modo) {
        case MODO_NORMAL:
            {
                var rq = getRequerimiento();
                let lat = parseFloat(domicilioSeleccionado.Latitud.replace(",", "."));
                let lng = parseFloat(domicilioSeleccionado.Longitud.replace(",", "."));

                ajax_BuscarCercanos(lat, lng, rq.IdMotivo)
                  .then(function (cercanos) {
                      cardFormularioCargando.removeClass("visible");

                      if (cercanos.Cantidad == 0) {
                          registrarSinImportarCercanos();
                          return;
                      }

                      crearDialogoRequerimientosCercanos({
                          IdMotivo: rq.IdMotivo,
                          Latitud: lat,
                          Longitud: lng,
                          CallbackCrearSinImportarCercanos: function () {
                              registrarSinImportarCercanos();
                          }
                      });
                  })
                  .catch(function (error) {
                      cardFormularioCargando.removeClass("visible");
                      top.mostrarMensaje("Error", error);
                  });
            }
            break;

        case MODO_REQUERIMIENTO_INTERNO:
            {
                registrarSinImportarCercanos();
            }
            break;
    }
}

function registrarSinImportarCercanos() {
    var rq = getRequerimiento();
    let email = getUsuarioLogeado().Usuario.Email;

    cardFormularioCargando.addClass("visible");
    $("#cardAlerta").removeClass("visible");

    ajax_Registrar(rq, email)
      .then(function (resultado) {
          cardFormularioCargando.removeClass("visible");
          limpiar();
          mostrarRequerimientoCreado(
            resultado.Requerimiento,
            resultado.Email,
            resultado.Enviado
          );
      })
      .catch(function (error) {
          cardFormularioCargando.removeClass("visible");
          top.mostrarMensaje("Error", error);
      });
}

function mostrarRequerimientoCreado(rq, mail, enviado) {
    idRq = rq.Id;

    $("#cardAlerta").addClass("visible");
    $("#cardAlerta .numero").text(rq.Numero + "/" + rq.Año);
    $("#cardAlerta .mail").html(
      mail == undefined
        ? "Se produjo un error al enviar el comprobante de atención a <b>" +
            mail +
            "</b>"
        : "Se ha enviado un e-mail a <b>" +
            mail +
            "</b> con el comprobante de atención"
    );
    if (enviado == true) {
        $("#cardAlerta > div.contenedor-main > div > i").css(
          "color",
          "var(--colorExito)"
        );
        $("#cardAlerta .contenedor_Mail").show();
    } else {
        if (enviado == undefined) {
            $("#cardAlerta .contenedor_Mail").hide();
        } else {
            $("#cardAlerta .contenedor_Mail").show();
            $("#cardAlerta > div.contenedor-main > div > i").css(
              "color",
              "var(--colorError)"
            );
        }
    }
}

function getRequerimiento() {
    var rq = {};

    //Relevamiento interno
    rq.RelevamientoInterno = true;

    //Motivo
    var motivo = SelectorMotivo_GetMotivoSeleccionado();
    rq.IdMotivo = "" + motivo.Id;

    //Descripcion
    var descripcion = $("#inputFormulario_Descripcion").val();
    rq.Descripcion = descripcion.trim();

    //Ubicacion
    if (domicilioSeleccionado != undefined) {
        var comandoUbicacionNuevo = {
            Latitud: parseFloat(domicilioSeleccionado.Latitud.replace(",", ".")),
            Longitud: parseFloat(domicilioSeleccionado.Longitud.replace(",", ".")),
            Observaciones: domicilioSeleccionado.Observaciones,
            Direccion: domicilioSeleccionado.Direccion
        };
        rq.Domicilio = comandoUbicacionNuevo;
    } else {
        rq.Domicilio = null;
    }

    //Usuario
    rq.IdUsuarioReferente = "" + getUsuarioLogeado().Usuario.Id;

    //Estado
    if ($("#selectFormulario_Estado").val() != estadoNuevo.Id) {
        rq.EstadoKeyValue = _.find(getInitData().Requerimiento.Estados, {
            Id: parseInt($("#selectFormulario_Estado").val())
        }).KeyValue;
        rq.EstadoMotivo = $("#inputFormulario_EstadoDescripcion")
          .val()
          .trim();
    }

    //UserAgent
    rq.UserAgent = navigator.userAgent;

    //Tipo Cliente
    if (isMobileOrTablet()) {
        if (isMobile()) {
            rq.TipoDispositivo = 1;
        } else {
            rq.TipoDispositivo = 2;
        }
    } else {
        rq.TipoDispositivo = 3;
    }
    return rq;
}

function validar() {
    $(".control-observacion").text("");
    $(".control-observacion")
      .stop(true, true)
      .slideUp(300);
    var resultado = true;

    //Valido motivo
    if (SelectorMotivo_GetMotivoSeleccionado() == undefined) {
        $("#errorFormulario_Motivo").text("Debe seleccionar el servicio y motivo");
        $("#errorFormulario_Motivo")
          .stop(true, true)
          .slideDown(300);
        resultado = false;
    }

    //Valido ubicacion
    if (domicilioSeleccionado == undefined) {
        $("#errorFormulario_Domicilio").text("Debe seleccionar la ubicación");
        $("#errorFormulario_Domicilio")
          .stop(true, true)
          .slideDown(300);
        resultado = false;
    }

    resultado = $("#formRequerimiento").valid() && resultado;
    if (!resultado) {
        top.mostrarMensaje("Error", "Revise los datos del formulario");
    }
    return resultado;
}

function limpiar() {
    if (!$("#check_MantenerArea").is(":checked")) {
        $("#selectFormulario_Area").val(idAreaInicial);
        $("#selectFormulario_Area").trigger("change");
    }

    if (!$("#check_MantenerMotivo").is(":checked")) {
        $("#SelectorMotivo_BtnCancelarMotivo").trigger("click");

        $("#inputFormulario_Descripcion").val("");
        Materialize.updateTextFields();
    }

    if (!$("#check_MantenerUbicacion").is(":checked")) {
        $("#btn_CancelarDomicilio").trigger("click");
    }

    if (!$("#check_MantenerEstado").is(":checked")) {
        $("#selectFormulario_Estado").val(estadoNuevo.Id);
        $("#selectFormulario_Estado").trigger("change");
    }
}

function ajax_BuscarCercanos(lat, lng, idMotivo) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl(
              "~/Servicios/RequerimientoService.asmx/GetCantidadCercanos"
            ),
            Data: {
                consulta: {
                    Default: true,
                    IdMotivo: idMotivo,
                    Latitud: lat,
                    Longitud: lng
                }
            },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback({ Cantidad: result.Return, Latitud: lat, Longitud: lng });
            },
            OnError: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}

function ajax_Registrar(rq, email) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl("~/Servicios/RequerimientoService.asmx/Insertar"),
            Data: { comando: rq },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                if (modo != MODO_REQUERIMIENTO_INTERNO) {
                    crearAjax({
                        Url: ResolveUrl(
                          "~/Servicios/RequerimientoService.asmx/EnviarComprobanteAtencion"
                        ),
                        Data: {
                            id: result.Return.Id,
                            usuario: SelectorUsuario_GetUsuarioSeleccionado().Id,
                            mail: null
                        },
                        OnSuccess: function (resultMail) {
                            callback({
                                Requerimiento: result.Return,
                                Email: email,
                                Enviado: resultMail.Ok
                            });
                        },
                        OnError: function (resultMail) {
                            callback({
                                Requerimiento: result.Return,
                                Email: email,
                                Enviado: false
                            });
                        }
                    });
                } else {
                    callback({
                        Requerimiento: result.Return,
                        Email: undefined,
                        Enviado: undefined
                    });
                }
            },
            OnError: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}
