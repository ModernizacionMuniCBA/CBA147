const URL_BUSCAR = "~/Servicios/DomicilioService.asmx/Buscar";
const URL_SUGERIR = "~/Servicios/DomicilioService.asmx/Sugerir";

let controlDomicilioSelector_CallbackCargando;

let controlDomicilioSelector_Barrios;
let controlDomicilioSelector_Cpcs;
let controlDomicilioSelector_Ejido;

let controlDomicilioSelector_IncluirCpcsEnSugerencias;
let controlDomicilioSelector_IncluirBarriosEnSugerencias;

let controlDomicilioSelector_ModoManual = true;
let controlDomicilioSelector_MostrarEdificios = true;

function ControlDomicilioSelector_Init(valores) {
    if (valores == undefined) valores = {};

    $(".tooltipped").tooltip();
    if (valores === undefined) valores = {};

    if (!("CpcEnSugerencias" in valores)) valores.CpcEnSugerencias = true;
    controlDomicilioSelector_IncluirCpcsEnSugerencias = valores.CpcEnSugerencias;

    if (!("BarrioEnSugerencias" in valores)) valores.BarrioEnSugerencias = true;
    controlDomicilioSelector_IncluirBarriosEnSugerencias =
      valores.BarrioEnSugerencias;

    if (!("MostrarEdificiosMunicipales" in valores))
        valores.MostrarEdificiosMunicipales = true;
    MostrarEdificiosMunicipales = valores.MostrarEdificiosMunicipales;

    if (!("ModoManualDefecto" in valores)) valores.ModoManualDefecto = true;
    controlDomicilioSelector_ModoManual = valores.ModoManualDefecto;

    ControlDomicilioSelector_InitBuscar();

    if (valores.MostrarEdificiosMunicipales) {
        ControlDomicilioSelector_InitSwitch();
        ControlDomicilioSelector_InitEventosEdificiosMunicipales();
    }

    if (valores.Instrucciones) {
        $("#UbicacionSelector_ContenedorInstrucciones label").text(
          valores.Instrucciones
        );
        $("#UbicacionSelector_ContenedorInstrucciones").show();
    }

    $.getScript(
      "https://maps.googleapis.com/maps/api/js?key=" +
        KEY_GOOGLE_MAPS +
        "&libraries=visualization,places",
      function () {
          getGeoApiInfo()
            .then(function (data) {
                controlDomicilioSelector_Barrios = data.barrios;
                controlDomicilioSelector_Cpcs = data.cpcs;
                controlDomicilioSelector_Ejido = data.ejido;
            })
            .catch(function (error) {
                console.log(error);
            });
      }
    );

    //inicializo boton de mapa
    $("#UbicacionSelector_BtnMapa").click(function () {
        ControlDomicilioSelector_AbrirMapa();
    });
}

function ControlDomicilioSelector_InitBuscar() {
    $("#UbicacionSelector_InputBuscar").on("keydown", function (e) {
        let key = e.originalEvent.key;
        let busqueda = $(this).val();

        if (e.originalEvent.keyCode === 13) {
            $("#UbicacionSelector_BtnBuscar").trigger("click");
        }
    });

    //$('#UbicacionSelector_InputBuscar').on('input', function () {
    //    let busqueda = $(this).val();

    //});

    $("#UbicacionSelector_BtnBuscar").click(function () {
        let busqueda = $("#UbicacionSelector_InputBuscar").val();

        ControlDomicilioSelector_MostrarCargando();
        $("#UbicacionSelector_ContenedorSugerencias").removeClass("visible");

        ControlDomicilioSelector_Ajax_BuscarSugerencias(busqueda)
          .then(function (data) {
              //OCulto cargando
              ControlDomicilioSelector_OcultarCargando();

              //Cargo sugerencias
              ControlDomicilioSelector_CargarSugerencias(data);

              //Si uno solo Item click
              if (
                data.Cpcs.length === 0 &&
                data.Barrios.length === 0 &&
                data.Items.length === 1
              ) {
                  $(
                    $("#UbicacionSelector_ContenedorSugerencias .sugerencia")[0]
                  ).trigger("click");
              }
          })
          .catch(function (error) {
              ControlDomicilioSelector_OcultarCargando();
          });
    });
}

//-------------------------------
// Utiles
//-------------------------------

function ControlDomicilioSelector_MostrarCargando() {
    $("#UbicacionSelector_InputBuscar").prop("disabled", true);
    $("#ControlDomicilioSelector_ContenedorBuscar > div").addClass("progress");
}

function ControlDomicilioSelector_OcultarCargando() {
    $("#UbicacionSelector_InputBuscar").prop("disabled", false);
    $("#ControlDomicilioSelector_ContenedorBuscar > div").removeClass("progress");
}

function ControlDomicilioSelector_Ajax_Buscar(lat, lng) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: URL_BUSCAR,
            Data: { lat: lat, lng: lng },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}

function ControlDomicilioSelector_Ajax_BuscarSugerencias(consulta) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: URL_SUGERIR,
            Data: { consulta: consulta },
            OnSuccess: function (result) {
                consulta = consulta.toLowerCase();

                if (!result.Ok && result.Error != "Sin resultados") {
                    callbackError(result.Error);
                    return;
                }

                let data = {};

                //Barrios
                data.Barrios = [];
                if (controlDomicilioSelector_IncluirBarriosEnSugerencias) {
                    $.each(controlDomicilioSelector_Barrios, function (index, element) {
                        if (element.nombre.toLowerCase().indexOf(consulta) !== -1) {
                            data.Barrios.push(element);
                        }
                    });
                }

                //Cpcs
                data.Cpcs = [];
                if (controlDomicilioSelector_IncluirCpcsEnSugerencias) {
                    $.each(controlDomicilioSelector_Cpcs, function (index, element) {
                        let consultaCpc = consulta;
                        if (consultaCpc.indexOf("cpc") !== -1) {
                            consultaCpc = consultaCpc
                              .substring(consultaCpc.indexOf("cpc") + 3)
                              .trim();
                        }

                        if (
                          element.nombre.toLowerCase().indexOf(consultaCpc) !== -1 ||
                          consultaCpc == element.numero
                        ) {
                            data.Cpcs.push(element);
                        }
                    });
                }

                //Items
                data.Items = [];
                $.each(result.Return, function (index, element) {
                    if (
                      consulta.indexOf("cpc") === -1 &&
                      consulta.indexOf("c.p.c.") === -1
                    ) {
                        data.Items.push(element);
                    }
                });

                callback(data);
            },
            OnError: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}

var controlDomicilioSelector_UbicacionSeleccionada;

function ControlDomicilioSelector_Limpiar() {
    if (controlDomicilioSelector_ModoManual) {
        $("#UbicacionSelector_ContenedorBusqueda").show();
    } else {
        $("#UbicacionSelector_ContenedorBusqueda_EdificioMunicipal").show();
    }

    if (MostrarEdificiosMunicipales) {
        $("#UbicacionSelector_ContenedorSwitchUbicacion").show();
        $("#UbicacionSelector_SelectCategoriaEdificio")
          .val("-1")
          .trigger("change");
    }

    $("#UbicacionSelector_InputBuscar").val("");
    $("#UbicacionSelector_InputBuscar").trigger("focus");
    $("#UbicacionSelector_ContenedorSugerencias").empty();
    $("#UbicacionSelector_ContenedorSugerencias").removeClass("visible");
    $("#UbicacionSelector_ContenedorUbicacionSeleccionada").empty();
    $("#UbicacionSelector_ContenedorUbicacionSeleccionada").hide();
    $("#UbicacionSelector_ContenedorObservaciones").hide();
    controlDomicilioSelector_UbicacionSeleccionada = undefined;
}

function ControlDomicilioSelector_CargarSugerencias(data) {
    $("#UbicacionSelector_ContenedorSugerencias").empty();

    console.log(data);

    if (
      data.Cpcs.length === 0 &&
      data.Barrios.length === 0 &&
      data.Items.length === 0
    ) {
        let html = ControlDomicilioSelector_CrearHtmlSinSugerencia();

        //Agrego
        $("#UbicacionSelector_ContenedorSugerencias").append(html);
    } else {
        //Items
        $.each(data.Items, function (index, element) {
            let html = ControlDomicilioSelector_CrearHtmlSugerencia(element);

            //Click
            $(html).click(function () {
                ControlDomicilioSelector_CargarUbicacionSeleccionada(element);
            });

            //Agrego
            $("#UbicacionSelector_ContenedorSugerencias").append(html);
        });

        //Barrios
        $.each(data.Barrios, function (index, element) {
            let html = ControlDomicilioSelector_CrearHtmlSugerenciaBarrio(element);

            //Click
            $(html).click(function () {
                let barrios = [];
                barrios.push(element);
                let sugerencias = { Barrios: barrios, Cpcs: [], Items: [] };
                ControlDomicilioSelector_AbrirMapa(sugerencias);
            });

            //Agrego
            $("#UbicacionSelector_ContenedorSugerencias").append(html);
        });

        //Cpcs
        $.each(data.Cpcs, function (index, element) {
            let html = ControlDomicilioSelector_CrearHtmlSugerenciaCpc(element);

            //Click
            $(html).click(function () {
                let cpcs = [];
                cpcs.push(element);
                let sugerencias = { Cpcs: cpcs, Barrios: [], Items: [] };
                ControlDomicilioSelector_AbrirMapa(sugerencias);
            });

            //Agrego
            $("#UbicacionSelector_ContenedorSugerencias").append(html);
        });
    }

    $("#UbicacionSelector_ContenedorSugerencias").addClass("visible");
}

function ControlDomicilioSelector_CargarUbicacionSeleccionada(element) {
    ControlDomicilioSelector_Limpiar();
    controlDomicilioSelector_UbicacionSeleccionada = element;
    $("#UbicacionSelector_InputObservaciones").text("");

    let ubicacionHtml = ControlDomicilioSelector_CrearHtmlUbicacionSeleccionada(
      element
    );

    $("#UbicacionSelector_ContenedorUbicacionSeleccionada").append(ubicacionHtml);
    $("#UbicacionSelector_ContenedorSwitchUbicacion").hide();
    $("#UbicacionSelector_ContenedorUbicacionSeleccionada").show();
    $("#UbicacionSelector_ContenedorBusqueda").hide();
    $("#UbicacionSelector_ContenedorBusqueda_EdificioMunicipal").hide();
    $("#UbicacionSelector_ContenedorObservaciones").show();

    if (element.Observaciones != undefined) {
        $("#UbicacionSelector_InputObservaciones").text(element.Observaciones);
    }

    $("#UbicacionSelector_InputObservaciones").focus();
}

function ControlDomicilioSelector_AbrirMapa(sugerencia) {
    crearDialogoUbicacionManualSelector({
        Callback: function (domicilio) {
            ControlDomicilioSelector_CargarUbicacionSeleccionada(domicilio);
        },
        Sugerencia: sugerencia
    });
}

function ControlDomicilioSelector_CrearHtmlUbicacionSeleccionada(data) {
    let div = $($("#UbicacionSelector_TemplateUbicacionSeleccionada").html());
    $(div)
      .find(".cancelar")
      .click(function () {
          ControlDomicilioSelector_Limpiar();
      });
    $(div)
      .find(".domicilio")
      .text(data.Direccion);
    $(div)
      .find(".barrio")
      .html("<b>Barrio </b>" + data.Barrio.Nombre);
    $(div)
      .find(".cpc")
      .html("<b>CPC </b>" + data.Cpc.Nombre);


    if (data.Nombre){
        $(div).find(".observaciones").html("<b>" + data.Nombre + "</b>");
    }

    return div;
}

function ControlDomicilioSelector_CrearHtmlSinSugerencia() {
    let div = $($("#UbicacionSelector_TemplateSugerencia").html());
    $(div)
      .find("> .material-icons")
      .text("info_outline");
    $(div)
      .find("> .textos > .texto2")
      .text("No hay resultados para su búsqueda");
    $(div)
      .find(".contenedor-boton")
      .hide();
    $(div).removeClass("clickable");
    return div;
}

function ControlDomicilioSelector_CrearHtmlSugerencia(data) {
    let div = $($("#UbicacionSelector_TemplateSugerencia").html());
    if (data.Observaciones != undefined) {
        $(div)
          .find("> .material-icons")
          .text("place");
        $(div)
          .find("> .textos > .texto1")
          .text(data.Observaciones);
        $(div)
          .find("> .textos > .texto2")
          .text(data.Direccion);
    } else {
        $(div)
          .find("> .material-icons")
          .text("timeline");
        $(div)
          .find("> .textos > .texto1")
          .text("Calle");
        $(div)
          .find("> .textos > .texto2")
          .text(data.Direccion);
    }
    return div;
}

function ControlDomicilioSelector_CrearHtmlSugerenciaCpc(data) {
    let div = $($("#UbicacionSelector_TemplateSugerencia").html());
    $(div)
      .find("> .material-icons")
      .text("account_balance");
    $(div)
      .find("> .textos > .texto1")
      .text("CPC");
    $(div)
      .find("> .textos > .texto2")
      .text("N° " + data.numero + " - " + data.nombre);
    return div;
}

function ControlDomicilioSelector_CrearHtmlSugerenciaBarrio(data) {
    let div = $($("#UbicacionSelector_TemplateSugerencia").html());
    $(div)
      .find("> .material-icons")
      .text("business");
    $(div)
      .find("> .textos > .texto1")
      .text("Barrio");
    $(div)
      .find("> .textos > .texto2")
      .text(data.nombre);
    return div;
}

function ControlDomicilioSelector_GetDomicilio() {
    var obs = $("#UbicacionSelector_InputObservaciones").val();
    if (controlDomicilioSelector_UbicacionSeleccionada.Nombre) {
        obs = controlDomicilioSelector_UbicacionSeleccionada.Nombre + " - " + obs;
    }
    return {
        Latitud: parseFloat(
          controlDomicilioSelector_UbicacionSeleccionada.Latitud.replace(",", ".")
        ),
        Longitud: parseFloat(
          controlDomicilioSelector_UbicacionSeleccionada.Longitud.replace(",", ".")
        ),
        Observaciones:  obs,
        Direccion: controlDomicilioSelector_UbicacionSeleccionada.Sugerido
          ? undefined
          : controlDomicilioSelector_UbicacionSeleccionada.Direccion
    };
}

function ControlDomcilioSelector_HayDomicilioSeleccionado() {
    return controlDomicilioSelector_UbicacionSeleccionada != undefined;
}

function ControlDomcilioSelector_Validar() {
    if (controlDomicilioSelector_UbicacionSeleccionada == undefined) {
        top.mostrarMensaje(
          "Error",
          "Seleccione una ubicación validando con el botón buscar"
        );
        $("#ControlMapa_InputBuscar").focus();
        return false;
    }

    return true;
}

//------------------------------
// Switch
//------------------------------

function ControlDomicilioSelector_InitSwitch() {
    $("#UbicacionSelector_ContenedorSwitchUbicacion").show();
    $("#UbicacionSelector_SwitchUbicacion")
      .find("input")
      .prop("checked", controlDomicilioSelector_ModoManual);

    //Switch
    $("#UbicacionSelector_SwitchUbicacion")
      .find(".opcion1")
      .removeClass("active");
    $("#UbicacionSelector_SwitchUbicacion")
      .find(".opcion2")
      .addClass("active");

    //Click opcion1
    $("#UbicacionSelector_SwitchUbicacion")
      .find(".opcion1")
      .click(function () {
          $("#UbicacionSelector_SwitchUbicacion")
            .find("input")
            .prop("checked", false);
      });

    //Click opcion2
    $("#UbicacionSelector_SwitchUbicacion")
      .find(".opcion2")
      .click(function () {
          $("#UbicacionSelector_SwitchUbicacion")
            .find("input")
            .prop("checked", true);
      });

    //Click opcion2
    $("#UbicacionSelector_SwitchUbicacion")
      .find("label, span")
      .click(function () {
          $("#UbicacionSelector_SwitchUbicacion")
            .find("input")
            .prop(
              "checked",
              !$("#UbicacionSelector_SwitchUbicacion")
                .find("input")
                .is("checked")
            );
      });

    //Evento al cambiar
    $("#UbicacionSelector_SwitchUbicacion").click(function (e) {
        e.stopPropagation();

        var nuevoModo = $("#UbicacionSelector_SwitchUbicacion")
          .find("input")
          .is(":checked");
        controlDomicilioSelector_ModoManual = nuevoModo;

        $("#UbicacionSelector_SwitchUbicacion")
          .find("input")
          .prop("disabled", true);

        if (controlDomicilioSelector_ModoManual) {
            $("#UbicacionSelector_SwitchUbicacion")
              .find(".opcion1")
              .removeClass("active");
            $("#UbicacionSelector_SwitchUbicacion")
              .find(".opcion2")
              .addClass("active");

            $("#UbicacionSelector_ContenedorBusqueda_EdificioMunicipal").removeClass(
              "visible"
            );
            setTimeout(function () {
                $("#UbicacionSelector_ContenedorBusqueda").show();
                $("#UbicacionSelector_ContenedorBusqueda").addClass("visible");
            }, 300);
        } else {
            $("#UbicacionSelector_SwitchUbicacion")
              .find(".opcion1")
              .addClass("active");
            $("#UbicacionSelector_SwitchUbicacion")
              .find(".opcion2")
              .removeClass("active");

            $("#UbicacionSelector_ContenedorBusqueda").removeClass("visible");
            setTimeout(function () {
                $("#UbicacionSelector_ContenedorBusqueda_EdificioMunicipal").show();
                $("#UbicacionSelector_ContenedorBusqueda_EdificioMunicipal").addClass(
                  "visible"
                );
            }, 300);

            //Deselecciono el servicio
            setTimeout(function () {
                $("#UbicacionSelector_SelectCategoriaEdificio")
                  .val("-1")
                  .trigger("change");
                $("#UbicacionSelector_Edificio")
                  .val("-1")
                  .trigger("change");
            }, 300);
        }
    });

    $("#UbicacionSelector_SwitchUbicacion").trigger("click");
}

function ControlDomicilioSelector_InitEventosEdificiosMunicipales() {
    //los inicializo vacios
    ControlDomicilioSelector_CargarSelectEdificios([]);

    ControlDomicilioSelector_CargarSelectCategoriaEdificios([]);

    crearAjax({
        Url: ResolveUrl(
          "~/Servicios/EdificioMunicipalService.asmx/GetCategoriasConEdificios"
        ),
        OnSuccess: function (result) {
            result = parse(result);

            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                return;
            }
            ControlDomicilioSelector_CargarSelectCategoriaEdificios(result.Return);
        },
        OnError: function (result) {
            mostrarMensaje("Error", result.Error);
        }
    });

    $("#UbicacionSelector_SelectCategoriaEdificio").on("change", function () {
        var categoria = $(this).val();
        if (categoria == -1) {
            ControlDomicilioSelector_CargarSelectEdificios([]);
            return;
        }

        ControlDomicilioSelector_BuscarEdificios()
          .then(function (edificios) {
              ControlDomicilioSelector_CargarSelectEdificios(edificios);
          })
          .catch(function (error) {
              ControlDomicilioSelector_CargarSelectEdificios([]);
          });
    });

    $("#UbicacionSelector_SelectEdificio").on("change", function () {
        var edificio = $(this).val();
        if (edificio == -1) {
            return;
        }

        ControlDomicilioSelector_BuscarDomicilio()
          .then(function (domicilio) {
              ControlDomicilioSelector_CargarUbicacionSeleccionada(domicilio);
          })
          .catch(function (error) { });
    });
}

function ControlDomicilioSelector_BuscarEdificios() {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl(
              "~/Servicios/EdificioMunicipalService.asmx/GetResultadoTabla"
            ),
            Data: {
                idCategoria: $("#UbicacionSelector_SelectCategoriaEdificio").val()
            },
            OnSuccess: function (result) {
                result = parse(result);

                if (!result.Ok) {
                    mostrarMensaje("Error", result.Error);
                    callbackError();
                }

                callback(result.Return.Data);
            },
            OnError: function (result) {
                mostrarMensaje("Error", result.Error);
                callbackError();
            }
        });
    });
}

function ControlDomicilioSelector_BuscarDomicilio() {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl(
              "~/Servicios/EdificioMunicipalService.asmx/GetDomicilioById"
            ),
            Data: { id: $("#UbicacionSelector_SelectEdificio").val() },
            OnSuccess: function (result) {
                result = parse(result);

                if (!result.Ok) {
                    mostrarMensaje("Error", result.Error);
                    callbackError();
                }

                callback(result.Return);
            },
            OnError: function (result) {
                mostrarMensaje("Error", result.Error);
                callbackError();
            }
        });
    });
}

function ControlDomicilioSelector_CargarSelectCategoriaEdificios(data) {
    var deshabilitar = false;
    if (data.length == 0) {
        deshabilitar = true;
    }

    $("#UbicacionSelector_SelectCategoriaEdificio").prop(
      "disabled",
      deshabilitar
    );

    $("#UbicacionSelector_SelectCategoriaEdificio").CargarSelect({
        Data: data,
        Value: "Id",
        Text: "Nombre",
        Default: "Seleccione..."
    });
}

function ControlDomicilioSelector_CargarSelectEdificios(data) {
    var deshabilitar = false;
    if (data.length == 0) {
        deshabilitar = true;
    }

    $("#UbicacionSelector_SelectEdificio").prop("disabled", deshabilitar);

    $("#UbicacionSelector_SelectEdificio").CargarSelect({
        Data: data,
        Value: "Id",
        Text: "Nombre",
        Default: "Seleccione..."
    });
}
