var modo;

var funciones = [];
var empleado = {};

var callback;

function init(data) {
    empleado = data.Empleado;

    if (data.Funciones != undefined && data.Funciones.length != 0) {
        funciones = data.Funciones;
    }
  
    cargarInformacion();
    cargarFunciones();
}

function cargarInformacion() {
    $("#texto_Empleado").text(empleado.Nombre + ' ' + empleado.Apellido);
    $("#texto_Area").text( empleado.NombreArea);
}

function cargarFunciones() {
    $("#inputFormulario_SelectFunciones").CargarSelect({
        Data: funciones,
        Value: 'Id',
        Text: 'Nombre',
        Sort: false,
        Multiple: true
    });

    var ids = [];
    $.each(empleado.Funciones, function (i, obj) {
        ids.push(obj.FuncionId);
    })

    $("#inputFormulario_SelectFunciones").val(ids).change();
}

//-----------------------------
// Operaciones globales 
//-----------------------------
function registrar() {
    mostrarCargando(true, 'Editando funciones...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/EmpleadoService.asmx/EditarFunciones'),
        Data: { 'comando': getFunciones() },
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

function getFunciones() {
    var entity = {};
    entity.IdEmpleado= empleado.Id;
    entity.IdFunciones = $("#inputFormulario_SelectFunciones").val();
    return entity;
}

function setListener(c) {
    callback = c;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}