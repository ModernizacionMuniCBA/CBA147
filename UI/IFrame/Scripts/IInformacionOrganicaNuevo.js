let MODO_EDICION = 'editar';
let MODO_NUEVO = 'nuevo';

let modo;
let area;
let secretarias;

let callback;

function init(data) {
    area = data.Area;
    secretarias = data.Secretarias;

    initAcciones();

    $("#input_Secretaria").CargarSelect({
        Data: secretarias,
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    $("#input_Direccion").CargarSelect({
        Data: [],
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    $("#input_Secretaria").on('change', function () {
        let id = $("#input_Secretaria").val();
        if (id == -1) {
            $("#input_Direccion").CargarSelect({
                Data: [],
                Default: 'Seleccione...',
                Value: 'Id',
                Text: 'Nombre',
                Sort: true
            });
            return;
        }

        crearAjax({
            Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/GetByIdSecretaria'),
            Data: { idSecretaria: id },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    $("#input_Direccion").CargarSelect({
                        Data: [],
                        Default: 'Seleccione...',
                        Value: 'Id',
                        Text: 'Nombre',
                        Sort: true
                    });
                }

                $("#input_Direccion").CargarSelect({
                    Data: result.Return,
                    Default: 'Seleccione...',
                    Value: 'Id',
                    Text: 'Nombre',
                    Sort: true
                });
            },
            OnError: function (result) {
                $("#input_Direccion").CargarSelect({
                    Data: [],
                    Default: 'Seleccione...',
                    Value: 'Id',
                    Text: 'Nombre',
                    Sort: true
                });
            }
        });
    });

    $('#form').on('submit', function () {
        registrar();
    });
}

function initAcciones() {
    $('#btn_AgregarSecretaria').click(function () {
        crearDialogoInformacionOrganicaSecretariaNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje)
            },
            Callback: function (result) {
                actualizarSecretarias(result.Id);
            }
        });
    });

    $('#btn_AgregarDireccion').click(function () {
        if ($("#input_Secretaria").val() == -1) {
            mostrarMensaje('Alerta', 'Debe seleccionar la secretaria');
            return;
        }

        crearDialogoInformacionOrganicaDireccionNuevo({
            IdSecretaria: $("#input_Secretaria").val(),
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje)
            },
            Callback: function (result) {
                actualizarDirecciones(result.Id);
            }
        });
    });
}

function validar() {
    let formValido = $('#form').valid();
    let selects = $('#input_Direccion').val() != -1 && $('#input_Secretaria').val() != -1;
    return formValido && selects;
}

function registrar() {
    if (!validar()) return;

    mostrarCargando(true);

    var url = ResolveUrl('~/Servicios/InformacionOrganicaService.asmx/Insertar');

    crearAjax({
        Url: url,
        Data: { comando: getData() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "Información orgánica registrada correctamente");
            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    })
}

function getData() {
    let data = {};
    data.IdArea = area.Id;
    data.IdDireccion = $('#input_Direccion').val();
    return data;
}

function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(entity) {
    if (callback == undefined) return;
    callback(entity);
}

function actualizarSecretarias(idNuevo) {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/GetAll'),
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                return;
            }

            $("#input_Secretaria").CargarSelect({
                Data: result.Return,
                Default: 'Seleccione...',
                Value: 'Id',
                Text: 'Nombre',
                Sort: true
            });

            if (idNuevo != undefined) {
                $("#input_Secretaria").val(idNuevo).trigger('change');
            }

        },
        OnError: function (result) {
            mostrarCargando(false);
        }
    });
}

function actualizarDirecciones(idNuevo) {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/GetByIdSecretaria'),
        Data: { idSecretaria: $("#input_Secretaria").val() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                return;
            }

            $("#input_Direccion").CargarSelect({
                Data: result.Return,
                Default: 'Seleccione...',
                Value: 'Id',
                Text: 'Nombre',
                Sort: true
            });

            if (idNuevo != undefined) {
                $("#input_Direccion").val(idNuevo).trigger('change');
            }

        },
        OnError: function (result) {
            mostrarCargando(false);
        }
    });
}