// Init
function init(data) {
    if (verificarError(data) == true) return;

    initActualizarPerfil();
    initActualizarPerfilEvent();
}


// Actualizar perfil
function initActualizarPerfil() {
    $('#input_Nombre').val(usuarioLogeado.Nombre);
    $('#input_Apellido').val(usuarioLogeado.Apellido);
    $('#input_Dni').val(usuarioLogeado.Dni);

    if (usuarioLogeado.SexoMasculino) {
        $('#radio_Masculino').prop('checked', true);
    } else {
        $('#radio_Femenino').prop('checked', true);
    }

    let input_FechaNacimiento = $('#input_FechaNacimiento').pickadate({
        selectMonths: true,
        selectYears: 50,
        today: 'Hoy',
        clear: 'Limpiar',
        close: 'Cerrar',
        format: 'dd/mm/yyyy'
    });
    let picker = input_FechaNacimiento.pickadate('picker');
    if (usuarioLogeado.FechaNacimiento != undefined) {
        picker.set('select', new Date(usuarioLogeado.FechaNacimiento));
    }
}

function initActualizarPerfilEvent() {
    $('.btn-guardar').click(actualizarPerfil);
}

function actualizarPerfil() {
    if (!validarActualizarPerfil()) return;

    mostrarCargando(true);
    ocultarError('error_ActualizarPerfil');

    ajaxActualizarPerfil(comandoActualizarPerfil())
        .then(function () {
            mostrarCargando(false);

            crearDialogo({
                Id: 'dialogoBienvenida',
                Titulo: 'Bienvenido',
                Html: $($('#template_DialogoBienvenida').html()),
                Cerrable: false,
                Botones: [
                    {
                        Texto: 'Ver Mi Perfil',
                        AutoCerrar: false,
                        OnClick: function (dialogo, idObservaciones) {
                            redirigir('MiPerfil');
                        }
                    }
                ]
            });
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarError('error_ActualizarPerfil', error);
        });
}

function ajaxActualizarPerfil(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/ActualizarDatosPersonales'),
            Data: { comando: comando },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }
                callback();
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        })
    });
}

function validarActualizarPerfil() {
    return $('#form').valid();
}

function comandoActualizarPerfil() {
    let comando = {};
    comando.Nombre = $('#input_Nombre').val();
    comando.Apellido = $('#input_Apellido').val();
    comando.Dni = $('#input_Dni').val();
    comando.SexoMasculino = $('#radio_Masculino').is(':checked');
    comando.FechaNacimiento = $('#input_FechaNacimiento').val();
    return comando;
}
