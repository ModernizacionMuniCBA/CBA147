let callback;

let usuario;

const modoEditar = "Editar";
const modoRegistrar = "Registrar";
let modo = modoRegistrar;

let empleado = false;

const MAX_W = 500;

let imagenDefault_Male;
let imagenDefault_Female;

function init(data) {
    if (data.Error != undefined) {
        mostrarMensaje('Error', data.Error);
        return;
    }

    imagenDefault_Male = 'url(' + top.urlCordobaFiles + '/Archivo/' + top.identificadorFotoUserMale + '/3)'
    imagenDefault_Female = 'url(' + top.urlCordobaFiles + '/Archivo/' + top.identificadorFotoUserFemale + '/3)'

    ControlSexoSelector_init();
    ControlSexoSelector_SetRequerido(true);

    //Fecha de nacimiento
    $('#btnFechaNacimiento').click(function (e) {
        if ($(this).prop('disabled') == true) {
            return;
        }
        $('#picker_FechaNacimiento').click();
        e.stopPropagation()
    });

    //inicializar datepickeres
    $('#picker_FechaNacimiento').pickadate({
        // Date limits
        min: new Date(1900, 1, 1),
        max: new Date(),
        container: top.$(top.document).find('body'),
        selectMonths: true,
        selectYears: 200,
        onSet: function (value) {
            if ('select' in value) {
                var s = this.get("select", "dd/mm/yyyy");
                $('#input_FechaNacimiento').val(s);
                this.close();
            }

            if ('clear' in value) {
                $('#input_FechaNacimiento').val('');
            }
            Materialize.updateTextFields();
        }
    });

    empleado = data.Empleado == undefined ? false : true;

    if (data.Usuario != undefined) {
        modo = modoEditar;
        usuario = data.Usuario;
        empleado = usuario.Empleado;
        cargarDatosUsuario();
    }

    $("#formDatosPersonales, #formDatosContacto").submit(function () {
        if (modo == modoEditar) {
            editar();
        } else {
            registrar();
        }
        return false;
    });
    initFotoPersonal();

    if (empleado) {
        $("#contenedor_Empleado").show();
    }

    setTimeout(function () {
        mostrarCargando(false);
    }, 200);

}

function initFotoPersonal() {
    $('#pickerFoto').on('change', function () {
        var files = this.files;
        if (files == undefined || files.length == 0) {
            if (usuario.SexoMasculino == true) {
                $('#contenedor_DatosUsuario .imagen').css('background-image', 'url(' + imagenDefault_Male + ')');
            } else {
                $('#contenedor_DatosUsuario .imagen').css('background-image', 'url(' + imagenDefault_Female + ')');
            }
            return;
        }

        var file = files[0];

        var fr = new FileReader();
        fr.onload = function (e) {
            mostrarCargando(true);
            let content = e.target.result;
            achicarImagen(content, 500)
                .then(function (imagenChica) {
                    ajaxCambiarFoto(usuario.Id, imagenChica)
                        .then(function (identificador) {
                            mostrarCargando(false);

                            let url = 'url(' + top.urlCordobaFiles + '/Archivo/' + identificador + '/3)';
                            $('#contenedor_DatosUsuario .imagen').css('background-image', url);
                            if (getUsuarioLogeado().Usuario.Id == usuario.Id) {
                                top.$('#header_Toolbar_Usuario').css('background-image', url);
                            }
                        })
                        .catch(function (error) {
                            mostrarCargando(false);
                            mostrarMensajeError(error);
                        });
                });


        }
        fr.readAsDataURL(file);
    });

    $('#contenedor_DatosUsuario .imagen').click(function () {
        $('#pickerFoto').val('');
        $('#pickerFoto').trigger('click');
    });
}

function cargarDatosUsuario() {
    if (usuario.ValidadoRenaper == true) {
        let identificador;
        if (usuario.IdentificadorFotoPersonal != undefined) {
            identificador = usuario.IdentificadorFotoPersonal;
        } else {
            identificador = usuario.SexoMasculino ? top.identificadorFotoUserMale : top.identificadorFotoUserFemale;
        }
        $('#contenedor_DatosUsuario .imagen').css('background-image', 'url(' + top.urlCordobaFiles + '/Archivo/' + identificador + '/3)');

        $('#contenedor_DatosPersonales').hide();
        $('#contenedor_InfoDatosPersonalesNoEditable').show();


        $('#texto_Nombre').text(toTitleCase(usuario.Nombre + ' ' + usuario.Apellido));
        $('#texto_Dni').text(usuario.Dni);
    } else {
        $('#contenedor_DatosPersonales').show();
        $('#contenedor_InfoDatosPersonalesNoEditable').hide();

        $('#input_Nombre').val(toTitleCase(usuario.Nombre));
        $('#input_Apellido').val(toTitleCase(usuario.Apellido));
        $('#input_Dni').val(usuario.Dni);
        $('#input_Cuil').val(usuario.Cuil);
        if (usuario.FechaNacimiento != null) {
            $('#input_FechaNacimiento').val(moment(usuario.FechaNacimiento).format('DD/MM/YYYY'));
        }
        if (usuario.SexoMasculino != undefined) {
            ControlSexoSelector_SetSexo(usuario.SexoMasculino == true ? 'Masculino' : 'Femenino');
        }
    }

    $("#input_Cargo").val(empleado.Cargo);

    //Contacto
    $('#input_Email').val(usuario.Email);
    if (usuario.TelefonoFijo != undefined && usuario.TelefonoFijo != "") {
        $('#input_TelefonoFijoCaracteristica').val(usuario.TelefonoFijo.split('-')[0]);
        $('#input_TelefonoFijoNumero').val(usuario.TelefonoFijo.split('-')[1]);
    }
    if (usuario.TelefonoCelular != undefined && usuario.TelefonoCelular != "") {
        $('#input_TelefonoCelularCaracteristica').val(usuario.TelefonoCelular.split('-')[0]);
        $('#input_TelefonoCelularNumero').val(usuario.TelefonoCelular.split('-')[1]);
    }

    Materialize.updateTextFields();
}

function getUsuario() {
    var data = {};
    var modificarDatos = true;

    if (modo == modoEditar) {
        data.Id = usuario.Id;
        modificarDatos = !usuario.ValidadoRenaper;
    }

    //Datos personales
    if (modificarDatos == true) {
        data.Nombre = $('#input_Nombre').val().trim();
        data.Apellido = $('#input_Apellido').val().trim();
        data.Dni = $('#input_Dni').val().trim();
        data.FechaNacimiento = $('#input_FechaNacimiento').val().trim();
        data.SexoMasculino = ControlSexoSelector_GetSexoSeleccionado() == "Masculino";
    }

    //Empleado
    if (empleado) {
        data.Cargo = $('#input_Cargo').val().trim();
    }

    //Contacto
    data.Email = $('#input_Email').val().trim();
    let telefonoCelularCaracteristica = $('#input_TelefonoCelularCaracteristica').val().trim();
    let telefonoCelularNumero = $('#input_TelefonoCelularNumero').val().trim();
    let telefonoFijoCaracteristica = $('#input_TelefonoFijoCaracteristica').val().trim();
    let telefonoFijoNumero = $('#input_TelefonoFijoNumero').val().trim();
    if (telefonoCelularCaracteristica != "") {
        data.TelefonoCelular = telefonoCelularCaracteristica + '-' + telefonoCelularNumero;
    }
    if (telefonoFijoCaracteristica != "") {
        data.TelefonoFijo = telefonoFijoCaracteristica + '-' + telefonoFijoNumero;
    }

    return data;
}


function registrar() {
    if (!validar()) return false;

    mostrarCargando(true);
    ajaxRegistrar(getUsuario())
    .then(function (data) {
        mostrarCargando(false);
        informar(data);
    })
    .catch(function (error) {
        mostrarCargando(false);
        mostrarMensajeError(error);
    });
}

function editar() {
    if (!validar()) return false;

    mostrarCargando(true);
    ajaxEditar(getUsuario())
        .then(function (data) {
            mostrarCargando(false);
            informar(data);
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarMensajeError(error);
        });
}

function validar() {
    var formDatosPersonales = $('#formDatosPersonales').valid();
    var formDatosContacto = $('#formDatosContacto').valid();
    var sexoValido = ControlSexoSelector_Validar();

    if (modo == modoEditar && usuario.ValidadoRenaper == true) {
        formDatosPersonales = true;
        sexoValido = true;
    }

    if (formDatosPersonales != true || formDatosContacto != true || sexoValido != true) {
        return false;
    }

    //Ahora valido el telefono
    let telefonoValido = true;
    let telefonoCelularCaracteristica = $('#input_TelefonoCelularCaracteristica').val().trim();
    let telefonoCelularNumero = $('#input_TelefonoCelularNumero').val().trim();
    let telefonoFijoCaracteristica = $('#input_TelefonoFijoCaracteristica').val().trim();
    let telefonoFijoNumero = $('#input_TelefonoFijoNumero').val().trim();

    if (telefonoCelularCaracteristica == "" && telefonoCelularNumero == "" && telefonoFijoCaracteristica == "" && telefonoFijoNumero == "") {
        mostrarMensajeAlerta('Ingrese algun telefono de contacto');
        return false;
    }

    if ((telefonoCelularCaracteristica == "") != (telefonoCelularNumero == "")) {
        mostrarMensajeAlerta('Complete el telefono celular');
        return false;
    }

    if ((telefonoFijoCaracteristica == "") != (telefonoFijoNumero == "")) {
        mostrarMensajeAlerta('Complete el telefono fijo');
        return false;
    }

    return true;
}


function achicarImagen(data, w) {
    return new Promise(function (callback, callbackError) {
        var canvas = $('<canvas>').get(0);
        var ctx = canvas.getContext("2d");

        var image = new Image();
        image.setAttribute('crossOrigin', 'anonymous');

        image.onload = function () {
            ctx.drawImage(image, 0, 0);

            var width = image.width;
            var height = image.height;

            if (width > height) {
                if (width > w) {
                    height *= w / width;
                    width = w;
                }
            } else {
                if (height > w) {
                    width *= w / height;
                    height = w;
                }
            }
            canvas.width = width;
            canvas.height = height;
            ctx.drawImage(image, 0, 0, width, height);

            callback(canvas.toDataURL("image/png", 0.7))
        };

        image.src = data;

    });
}


//----------------------------
// AJAXs 
//----------------------------

function ajaxEditar(data) {
    var url = ResolveUrl('~/Servicios/UsuarioService.asmx/ActualizarUsuario');

    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: url,
            Data: { usuario: data },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        });
    });
}

function ajaxRegistrar(data) {
    var url = ResolveUrl('~/Servicios/UsuarioService.asmx/CrearUsuario');
    if (empleado) {
        url = ResolveUrl('~/Servicios/UsuarioService.asmx/CrearUsuarioEmpleado');
    }

    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: url,
            Data: { usuario: data },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        });
    });
}

function ajaxCambiarFoto(id, data) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: '~/Servicios/UsuarioService.asmx/CambiarFoto',
            Data: { id: id, content: data },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        });
    });

}

//----------------------------
// Listener 
//----------------------------

function informar(data) {
    if (callback == null) return;
    callback(data);
}

function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}
