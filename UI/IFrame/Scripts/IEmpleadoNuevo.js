var modoRegistrar = 'Registrar';
var modoEditar = 'Editar';

var modo;

var funciones = [];
var secciones = [];

var callback;

function init(data) {
    var areas = usuarioLogeado.Areas;

    $("#contenedorSelectFunciones").prop('disabled', true);

    $("#inputFormulario_SelectArea").CargarSelect({
        Data: usuarioLogeado.Areas,
        Value: 'Id',
        Text: 'Nombre',
        Sort: false
    });

    if (areas.length == 1) {
        $("#contenedorSelectArea").hide();
        buscarSecciones(areas[0].Id)
    } 

    if (data.Funciones != undefined && data.Funciones.length != 0) {
        funciones = data.Funciones;
    }

    cargarFunciones();
    SelectorUsuario_SetSoloEmpleado(true);
    initEventos();

    if (data.IdArea != undefined) {
        $("#inputFormulario_SelectArea").val(data.IdArea).trigger('change');
    }

    SelectorUsuario_SetModoEmpleado(true);
}

function initEventos() {
    $("#inputFormulario_SelectArea").on("change", function () {
        buscarSecciones(this.value);

        crearAjax({
            Url: ResolveUrl("~/Servicios/FuncionService.asmx/GetByIdArea"),
            Data: { idArea: this.value },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    mostrarMensaje('Error', result.Error);
                    return;
                }

                funciones = result.Return;
                cargarFunciones();
            },
            OnError: function (result) {
                mostrarMensaje('Error', result.Error);
            }
        });


    });

    $('#btnNuevaFuncion').click(function () {
        crearDialogoFuncionNueva({
            IdArea: $("#inputFormulario_SelectArea").val(),
            Callback: function (funciones) {
                //Cargo los tipos
                $('#inputFormulario_SelectFunciones').CargarSelect({
                    Data: funciones,
                    Value: 'Id',
                    Text: 'Nombre',
                    Multiple: true,
                    Sort: true
                });

            },
            CallbackMensajes: function () { }
        });
    });

}

function buscarSecciones(idArea) {
    crearAjax({
        Url: ResolveUrl("~/Servicios/SeccionService.asmx/GetByIdArea"),
        Data: { idArea: idArea },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            secciones = result.Return;
            cargarSecciones();
        },
        OnError: function (result) {
            mostrarMensaje('Error', result.Error);
        }
    });
}

function cargarFunciones() {
    if (funciones.length == 0) {
        $("#contenedorSelectFunciones").hide();
        return;
    }

    $("#contenedorSelectFunciones").show();
    $("#inputFormulario_SelectFunciones").CargarSelect({
        Data: funciones,
        Value: 'Id',
        Text: 'Nombre',
        Sort: false,
        Multiple: true
    });
}

function cargarSecciones() {
    if (secciones.length == 0) {
        $("#inputFormulario_SelectSeccion").CargarSelect({
            Data: [],
            Value: 'Id',
            Text: 'Nombre',
            Sort: false,
            Default: "No tiene secciones"
        });

        $("#inputFormulario_SelectSeccion").prop("disabled", true);
        return;
    }

    $("#inputFormulario_SelectSeccion").prop("disabled", false);
    $("#inputFormulario_SelectSeccion").CargarSelect({
        Data: secciones,
        Value: 'Id',
        Text: 'Nombre',
        Sort: false,
        Default: "Seleccione..."
    });
}

//-----------------------------
// Operaciones globales 
//-----------------------------
function registrar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Registrando empleado...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/Insert'),
        Data: { 'comando': getEntity() },
        OnSuccess: function (resultado) {
            mostrarCargando(false);

            if (!resultado.Ok) {
                mostrarMensaje('Error', resultado.Error);
                return;
            }

            informar(resultado.Return);
        },
        OnError: function (resultado) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function editar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true, 'Editando origen...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/OrigenService.asmx/Editar'),
        Data: { 'comando': getOrigen() },
        OnSuccess: function (resultado) {
            mostrarCargando(false);

            if (!resultado.Ok) {
                mostrarMensaje('Error', resultado.Error);
                return;
            }

            informar(resultado.Return);
        },
        OnError: function (resultado) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function validar() {
    $('.control-observacion').text('');

    var resultado = true;

    if (SelectorUsuario_GetUsuarioSeleccionado() == undefined) {
        mostrarMensaje('Alerta', 'Debe seleccionar un usuario');
        return false;
    }

    //Area
    var idArea = $('#inputFormulario_SelectArea').val();
    if (idArea == undefined || idArea == -1) {
        $('#inputFormulario_SelectOrigen').siblings('.control-observacion').text('Dato requerido');
        resultado = false;
    }

    return resultado;
}

function getEntity() {
    var entity = {};
    entity.IdUsuario = SelectorUsuario_GetUsuarioSeleccionado().Id;
    entity.IdArea = $('#inputFormulario_SelectArea').val();

    var seccion = $('#inputFormulario_SelectSeccion').val();
    if (seccion != undefined && seccion != "-1") {
        entity.IdSeccion = seccion;
    }

    var funciones = $('#inputFormulario_SelectFunciones').val();
    if (funciones != undefined && funciones.length != 0) {
        entity.IdFunciones = funciones;
    }
    return entity;
}

function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}