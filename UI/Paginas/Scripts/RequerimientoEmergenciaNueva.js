let motivos;

let generoMasculino = 1;
let generoFemenino = 2;

$(document).ready(function () {
    $(".tooltipAyuda").each(function () {
        // Notice the .each() loop, discussed below
        $(this).qtip({
            content: {
                text: $(this).next("div") // Use the "div" element next to this for the content
            },
            position: {
                my: "top center",
                at: "bottom center"
            },
            style: {
                classes: "qtip-shadow qtip-rounded qtip-tipsy"
            }
        });
    });
});

function init(data) {
    data = parse(data);

    setDrawerExpandido(false, true);

    //--------------------------------
    // Ubicacion del reclamo
    //--------------------------------

    //--------------------------------
    // Motivos
    //--------------------------------

    //Cargo los motivos
    cargarMotivos(data.Motivos);
    motivos = data.Motivos;

    $("#btn_Registrar").click(function () {
        if (!validar()) return;
        insertar();
    });

    $("#btn_Limpiar").click(function () {
        limpiar();
    });

    $("#btnDatosAdicionales").click(function () {
        var abrir = $("#btnDatosAdicionales i").text() == "expand_more";
        if (abrir) {
            $("#btnDatosAdicionales i").text("expand_less");

            $("#contenedor_DatosAdicionales").addClass("visible");
            $("#contenedor_Observaciones").addClass("visible");
            return;
        }

        //si tengo que cerrar
        $("#btnDatosAdicionales i").text("expand_more");
        $("#contenedor_DatosAdicionales").removeClass("visible");
        $("#contenedor_Observaciones").removeClass("visible");
    })

    //Al apretar neuvo
    $("#btnNuevoRequerimiento").click(function () {
        limpiar();

        //Muestro el cargando mientras carga el iframe
        $("#cardFormulario")
          .find(".cargando")
          .stop(true, true)
          .fadeIn(300);

        $("#cardFormulario").fadeIn(300);
        $("#alertaOk").slideUp(500, function () {
            //Muestro el cargando mientras carga el iframe
            $("#cardFormulario")
              .find(".cargando")
              .stop(true, true)
              .fadeOut(300);
        });
    });

    $("#btnImprimirRequerimiento").click(function () {
        crearDialogoReporteRequerimientoDetalle({
            Id: idRequerimiento
        });
    });

    $("#btn_BuscarUsuario").click(function () {
        if ($("#input_referenteDni").val().length > 8) {
            mostrarMensaje("Error", "El DNI no puede tener más de 8 dígitos");
            return;
        }

        buscarUsuario();
    });

    $("#input_referenteDni").keydown(function (e) {
        //Enter
        if (e.keyCode == 13) {
            $("#btn_BuscarUsuario").trigger("click");
        }
    });

    let generos = [
      { Id: generoMasculino, Nombre: "Masculino" },
      { Id: generoFemenino, Nombre: "Femenino" }
    ];

    $("#select_referenteGenero").CargarSelect({
        Data: generos,
        Default: "Seleccione...",
        Value: "Id",
        Text: "Nombre"
    });

    ControlDomicilioSelector_Init({ MostrarEdificiosMunicipales: false });

    setTimeout(function () {
        $("#input_referenteTelefono").trigger("focus");
    }, 250);

    $("#input_FiltrarMotivos").keyup(function (e) {
        filtrarMotivos();
    })
}

//--------------------------------
//Usuario
//--------------------------------
let usuarioSeleccionado;
function buscarUsuario() {
    let dni = $("#input_referenteDni").val();
    if (dni == "" || dni == undefined) {
        mostrarMensaje("Error", "Ingrese un DNI para buscar");
        return;
    }

    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl("~/Servicios/UsuarioService.asmx/GetByFilters"),
        Data: { filtros: { Dni: dni } },
        OnSuccess: function (result) {
            result = parse(result);
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                return;
            }

            if (result.Return.length == 0) {
                mostrarMensaje(
                  "Info",
                  "El DNI ingresado no pertenece a ningún usuario de Muni Online. Por favor, ingrese los datos manualmente."
                );
                return;
            }

            cargarUsuario(result.Return[0]);
        },
        OnError: function () { }
    });
}

function cargarUsuario(usuario) {
    usuarioSeleccionado = usuario;

    let html = $($("#template_UsuarioSeleccionado").html());
    $(html).attr("id-usuario", usuario.Id);
    $(html)
      .find(".nombre")
      .html("<b>" + usuario.Nombre + " " + usuario.Apellido + "</b>");

    $(html)
      .find(".dni")
      .html("<b>DNI: </b>" + usuario.Dni + " " + usuario.Apellido);

    $(html)
      .find(".telefono")
      .html("<b>Teléfono: </b>" + usuario.TelefonoCelular);

    $(html)
      .find("a")
      .click(function () {
          $("#contenedor_ReferenteProvisorio").addClass("visible");
          $("#contenedor_UsuarioSeleccionado").empty();
          usuarioSeleccionado = undefined;
          $("#input_referenteDni").val("");

          setTimeout(function () {
              $("#input_referenteDni").trigger("focus");
          }, 250);
      });

    $("#contenedor_UsuarioSeleccionado").append(html);
    $("#contenedor_ReferenteProvisorio").removeClass("visible");
    $("#contenedor_UsuarioSeleccionado").addClass("visible");

    setTimeout(function () {
        $("#input_referenteObservaciones").trigger("focus");
    }, 250);
}

//--------------------------------
// Motivos
//--------------------------------
function cargarMotivos(motivos) {
    $.each(motivos, function (i, motivo) {
        let html = crearHtmlMotivo(motivo);
        //$(html).addClass("oculto");
        $("#contenedor-motivos").append(html);
    });
}

function crearHtmlMotivo(mot) {
    let html = $($("#template_Motivo").html());
    $(html).attr("id-motivo", mot.Id);
    $(html)
      .find(".texto")
      .text(mot.Nombre);

    $(html).click(function () {
        seleccionarMotivo(mot);
    });

    return html;
}

let motivoSeleccionado;

function seleccionarMotivo(motivo) {
    motivoSeleccionado = motivo;

    $("#contenedor-motivos .motivo").addClass("oculto");
    $("#contenedor-filtrarMotivos").hide();
    let html = $($("#template_MotivoSeleccionado").html());

    $(html)
      .find(".texto")
      .text(motivo.Nombre);

    $("#contenedor-motivoSeleccionado").append(html);

    $(html)
      .find("a")
      .click(function () {
          motivoSeleccionado = undefined;
          $("#contenedor-filtrarMotivos").show();
          $("#input_FiltrarMotivos").val("");
          $("#input_FiltrarMotivos").trigger("keyup");
          $("#contenedor-camposDinamicos").empty();
          $("#contenedor-motivoSeleccionado").empty();

      });

    setTimeout(function () {
        $(html)
          .find("#inputFormulario_Descripcion")
          .trigger("focus");
    }, 300);

    camposDinamicos = [];
    //vacio los campos dinamicos anteriores
    $("#contenedor-camposDinamicos").empty();
    mostrarCargando(true);
    getCamposDinamicosByMotivo(motivo.Id);
}

function filtrarMotivos() {
    var filtro = $("#input_FiltrarMotivos").val();
    var motivosFiltrados = [];
    if (filtro) {
        motivosFiltrados = _.filter(motivos,
            function (m) {
                var queonda = m.Nombre.toUpperCase().includes(filtro.toUpperCase());
                return queonda;
            });
    }

    $(".motivo").addClass("oculto");

    $.each(motivosFiltrados, function (i, m) {
        $(".motivo[id-motivo=" + m.Id + "]").removeClass("oculto");
    })
}

//------------------------------
// Campos dinámicos
//------------------------------
function getCamposDinamicosByMotivo(id) {
    crearAjax({
        Url: ResolveUrl("~/Servicios/MotivoService.asmx/GetCamposByIdMotivo"),
        Data: { idMotivo: id },
        OnSuccess: function (result) {
            result = parse(result);

            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje("error", result.Error);
                return;
            }

            camposDinamicos = result.Return;
            cargarCamposDinamicos(camposDinamicos).then(function (html) {
                $("#contenedor-camposDinamicos").append(html);
                $("#contenedor-camposDinamicos").show();
                mostrarCargando(false);
                init_Campos();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("error", result.Error);
            return;
        }
    });
}

/* Inserción */
function insertar() {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl("~/Servicios/RequerimientoService.asmx/Insertar"),
        Data: { comando: getRequerimiento() },
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (result == undefined || result.Return == null) {
                mostrarMensaje("Error", result.Mensaje);
                informarRegistroError();
                return;
            }

            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                informarRegistroError();
                return;
            }

            var rq = result.Return;
            informarRegistro(rq);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
            informarRegistroError();
        }
    });
}

function getRequerimiento() {
    var rq = {};

    //Motivo
    var motivo = motivoSeleccionado;
    rq.IdMotivo = "" + motivo.Id;

    //Descripcion
    var descripcion = $("#inputFormulario_Descripcion").val();
    rq.Descripcion = descripcion.trim();

    //Ubicacion
    rq.Domicilio = ControlDomicilioSelector_GetDomicilio();

    //Referente
    if (usuarioSeleccionado != undefined) {
        rq.IdUsuarioReferente = usuarioSeleccionado.Id;
        rq.ObservacionesUsuarioReferente = $("#input_referenteObservaciones").val();
    } else {
        let referente = {};
        referente.Nombre = $("#input_referenteNombre").val();
        referente.Apellido = $("#input_referenteApellido").val();
        referente.DNI = $("#input_referenteDni").val();
        let genero = $("#select_referenteGenero").val();
        referente.GeneroMasculino = genero == "" + generoMasculino ? true : false;
        referente.Telefono = $("#input_referenteTelefono").val();
        referente.Observaciones = $("#input_referenteObservaciones").val();
        rq.ReferenteProvisorio = referente;
    }

    rq.CamposDinamicos = getCamposDinamicosPorRequerimiento(camposDinamicos);

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
    $("#contenedor")
      .find(".control-observacion")
      .text("");
    $("#contenedor")
      .find(".control-observacion")
      .stop(true, true)
      .slideUp(300);

    var resultado = true;

    //Sin Motivo
    if (motivoSeleccionado == undefined) {
        $("#errorFormulario_Motivo").text("Debe seleccionar un motivo");
        $("#errorFormulario_Motivo")
          .stop(true, true)
          .slideDown(300);
        resultado = false;
    }

    //Sin datos referente
    //var dni = $("#input_referenteDni").val();
    //if ((dni == undefined || dni == "") && usuarioSeleccionado == undefined) {
    //    $("#errorFormulario_Dni").text("Debe introducir al menos un DNI");
    //    $("#errorFormulario_Dni")
    //      .stop(true, true)
    //      .slideDown(300);
    //    resultado = false;
    //}

    // var genero = $("#select_referenteGenero").val();
    // if (genero == undefined || genero == "-1") {
    //   $("#errorFormulario_Genero").text("Debe seleccionar el género");
    //   $("#errorFormulario_Genero")
    //     .stop(true, true)
    //     .slideDown(300);
    //   resultado = false;
    // }

    //Valido ubicacion
    if (!ControlDomcilioSelector_HayDomicilioSeleccionado()) {
        $("#errorFormulario_Domicilio").text("Debe seleccionar la ubicación");
        $("#errorFormulario_Domicilio").show();
        resultado = false;
    }

    if (!validarCamposDinamicos(camposDinamicos)) {
        resultado = false;
    }

    return resultado;
}

function limpiar() {
    $(".control-observacion").text("");
    $("#btnCancelarMotivoSeleccionado").trigger("click");
    $("#input_referenteNombre").val("");
    $("#input_referenteApellido").val("");
    $("#input_referenteDni").val("");
    $("#check_referenteGenero").prop("checked", false);
    $("#input_referenteTelefono").val("");
    $("#input_referenteObservaciones").val("");
    ControlDomicilioSelector_Limpiar();
    Materialize.updateTextFields();
}

function informarRegistro(requerimiento) {
    $("#cardFormulario").fadeOut(300);

    limpiar();

    $("#alertaOk")
      .find("#textoNumeroReclamo")
      .text(requerimiento.Numero + "/" + requerimiento.Año);
    //$('#alertaOk').find('#textoNumeroReclamo').text(requerimiento.Numero);

    $("#alertaOk").slideDown(500);
}
