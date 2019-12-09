var camposPorMotivo;
var camposPorRequerimiento;

var idRequerimiento = -1;


function init(data) {
    data = parse(data);

    //------------------------------------
    // Init Datos
    //------------------------------------

    camposPorMotivo = data.CamposPorMotivo;
    camposPorRequerimiento = data.CamposPorRequerimiento;
    idRequerimiento = data.IdRequerimiento;


    cargarCamposDinamicos(camposPorMotivo).then(function (html) {
        console.log('termine de cargar los campos')
        $("#contenido").append(html);
        setCampoPorRequerimiento(camposPorRequerimiento);
        mostrarCargando(false);
        init_Campos();
        return 1;
    });
}

function editar() {
    if (!validar()) {
        return;
    }

    var data = { comando: { IdRequerimiento: idRequerimiento, CamposDinamicos: getCamposDinamicosPorRequerimiento(camposPorMotivo) } };

    crearAjax({
        Data: data,
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EditarCamposDinamicos'),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "La información adicional se ha actualizado correctamente");
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
    return validarCamposDinamicos(camposPorMotivo)
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

function informar() {
    if (callback != undefined) {
        callback();
    }
}