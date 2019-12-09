var tipos;

var idMotivo;

var idArea = -1;

var modo_editar = 'editar';
var modo_registrar = 'registrar';
var modo = modo_registrar;

var tipo_selector = "7";
var tipo_selector = "7";

var campo;

function init(data) {
    data = parse(data);

    //------------------------------------
    // Init Datos
    //------------------------------------

    tipos = data.TiposCampos;
    idMotivo = data.IdMotivo;

    $("#inputFormulario_Nombre").trigger('focus');

    $("#select_Tipos").CargarSelect({
        Value: 'Id',
        Text: 'Nombre',
        Data: tipos,
        Default: 'Seleccione...'
    });

    $("#select_Tipos").on("change", function () {
        if ((this).value == tipo_selector) {
            $("#contenedor_opcionesSelector").show();
            return;
        }

        $("#contenedor_opcionesSelector").hide();
    });

    $("#btnAgregarOpcionSelector").click(function () {
        var opcion = $("#contenedor_opcionesSelector input").val();
        if (opcion == "") {
            mostrarMensaje("Error", "Debe ingresar una opción para agregarla");
            return;
        }

        if (opcion.includes("^")) {
            mostrarMensaje("Error", "No se puede utilizar el símbolo ^ para las opciones");
            return;
        }

        if (!validarOpciones(opcion)) {
            mostrarMensaje("Error", "La opción '" + opcion + "' ya ha sido ingresada");
            return;
        }

        crearHtmlOpcionesSelector(opcion);
    })

    if (data.Campo != undefined) {
        modo = modo_editar;
        campo = data.Campo;
        $("#select_Tipos").prop("disabled", true);
        setCampoPorMotivo(campo);
    }
}

function insertar() {
    if (!validar()) {
        return;
    }

    crearAjax({
        Data: { comando: getCampoPorMotivo() },
        Url: ResolveUrl('~/Servicios/MotivoService.asmx/InsertCampo'),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "El campo '" + result.Return.Nombre + "' se ha creado correctamente");
            informar(result.Return);

        },
        OnError: function (result) {
            $("#inputFormulario_Nombre").trigger('focus');
            console.log(result);
            mostrarMensaje('Error', 'Error insertando el campo');
            mostrarMensaje('Error', result.Error);
        }
    });
}

function editar() {
    if (!validar()) {
        return;
    }

    crearAjax({
        Data: { comando: getCampoPorMotivo() },
        Url: ResolveUrl('~/Servicios/MotivoService.asmx/EditarCampo'),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "El campo '" + result.Return.Nombre + "' se ha editado correctamente");
            informar(result.Return);

        },
        OnError: function (result) {
            $("#inputFormulario_Nombre").trigger('focus');
            console.log(result);
            mostrarMensaje('Error', 'Error editando el campo');
            mostrarMensaje('Error', result.Error);
        }
    });
}

function validar() {
    var tipo = $("#select_Tipos").val();
    if (tipo == -1 || tipo == undefined) {
        mostrarMensaje('Error', 'Debe ingresar el tipo del campo');
        return false;
    }

    //var error = true;
    //if (tipo == 7) {
    //    error = validarOpciones();
    //}

    if (tipo == 7) {
    if (getOpcionesSelector().length < 2) {
        mostrarMensaje('Error', 'Debe haber al menos dos opciones');
        return false;
    }
    }

    return $('#cardFormulario').valid();
}

function validarOpciones(opcion) {
    var ok = true;
    var opciones = getOpcionesSelector();
    $.each(opciones, function (i, element) {
        if (element == opcion) {
            ok = false;
            return false;
        }
    });

    return ok;
}

function getCampoPorMotivo() {
    var nombre = $("#inputFormulario_Nombre").val();
    var tipo = $("#select_Tipos").val();
    var obligatorio = $('#check_Obligatorio').is(':checked');
    var orden = $("#inputFormulario_Orden").val();
    if (orden == undefined || orden == "") {
        orden = null;
    }
    var grupo = $("#inputFormulario_Grupo").val();
    var observaciones = $("#inputFormulario_Observaciones").val();

    var data = {
        Nombre: nombre,
        IdTipoCampoPorMotivo: tipo,
        Obligatorio: obligatorio,
        Orden: orden,
        Grupo: grupo,
        Observaciones: observaciones,
        IdMotivo: idMotivo
    }

    if (tipo == 7) {
        data.Opciones = getOpcionesSelector();
    }


    if (modo == modo_editar) {
        data.Id = campo.Id;
    }

    return data;
}

function getOpcionesSelector() {
    var opciones = [];

    $("#contenedor_opcionesSelector .opcion").each(function (i, element) {
        var opcion = $(element).find(".nombre").text();

        opciones.push(opcion);
    });

    return opciones;
}

function crearHtmlOpcionesSelector(opcion) {
    var div = $($('#template_OpcionSelector').html());
    $(div).attr('id', Math.random());

    $(div).find('label').text(opcion);

    $(div).find('a').click(function () {
        $(this).parents(".opcion").remove();
    });

    $("#contenedor_opcionesSelector .contenido .items").append(div);
}

function setCampoPorMotivo(campo) {
    $("#inputFormulario_Nombre").val(campo.Nombre);
    $("#select_Tipos").val(campo.IdTipoCampoPorMotivo).trigger("change");
    $("#inputFormulario_Orden").val(campo.Orden);
    $("#inputFormulario_Grupo").val(campo.Grupo);
    $("#inputFormulario_Observaciones").val(campo.Observaciones);
    if (campo.Obligatorio) {
        $('#check_Obligatorio').prop('checked', true);
    } else {
        $('#check_Obligatorio').prop('checked', false);
    }

    if (campo.IdTipoCampoPorMotivo == tipo_selector) {
        $.each(campo.Opciones, function (i, opc) {
            crearHtmlOpcionesSelector(opc);
        })
    }

    Materialize.updateTextFields();
}
/* Mensajes */
function mostrarMensaje(tipo, mensaje) {
    switch (tipo) {
        case 'Info':
            Materialize.toast(mensaje, 5000);
            break;

        case 'Alerta':
            Materialize.toast(mensaje, 5000, 'colorAlerta');
            break;

        case 'Error':
            Materialize.toast(mensaje, 5000, 'colorError');
            break;

        case 'Exito':
            Materialize.toast(mensaje, 5000, 'colorExito');
            break;
    }
}

let callback;
/* Callback */
function setCallback(c) {
    callback = c;
}

function informar(campo) {
    if (callback != undefined) {
        callback(campo);
    }
}