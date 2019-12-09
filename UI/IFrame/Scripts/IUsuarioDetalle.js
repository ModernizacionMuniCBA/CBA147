let usuario;

function init(data) {
    if ('Error' in data) {
        return;
    }

    usuario = data.Usuario;
    cargarUsuario();

    if (getUsuarioLogeado().Usuario.Id == usuario.Id) {
        $('#contenedor_Acciones').show();
    } else {
        $('#contenedor_Acciones').hide();
    }

    $('#btn_Editar').click(function () {
        crearDialogoUsuarioEditar({
            Id: usuario.Id,
            Callback: function (u) {

                //Actualizo esta pagina
                usuario = u;
                cargarUsuario();

                crearDialogoCargando({
                    OnLoad: function (jAlert) {
                        ajaxActualizarDatosUsuarioLogeado()
                            .then(function (data) {
                                $(jAlert).CerrarDialogo();
                                top.setUsuarioLogeado(data);
                            })
                            .catch(function (error) {
                                $(jAlert).CerrarDialogo();
                                mostrarMensaje('Error', error);
                            });
                    }
                });
            }
        });
    });

    $('#btn_Password').click(function () {
        crearDialogoUsuarioCambiarPassword();
    });

    $("#btn_Username").click(function () {
        let placeholder = usuario.Username;
        crearDialogoInput({
            Titulo: 'Cambiar username',
            Placeholder: placeholder,
            OnLoad: function (jAlert) {
                let input = $(jAlert).find('input');
                $(input).focus();
            },
            Botones: [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Cambiar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert) {
                        let input = $(jAlert).find('input');
                        let username = input.val();
                        if (username == "") {
                            $(jAlert).find('input').focus();
                            mostrarMensaje('Alerta', "Ingrese el nuevo nombre de usuario");
                            return;
                        }
                        $(jAlert).CerrarDialogo();
                        procesarCambiarUsername(username);
                    }
                },

            ]
        });
    });
}

function cargarUsuario() {
    //Foto
    let identificador;
    if (usuario.IdentificadorFotoPersonal != undefined) {
        identificador = usuario.IdentificadorFotoPersonal;
    } else {
        if (usuario.SexoMasculino == true) {
            identificador = top.identificadorFotoUserMale;
        } else {
            identificador = top.identificadorFotoUserFemale;
        }
    }
    $('#foto').css('background-image', 'url(' + top.urlCordobaFiles + '/Archivo/' + identificador + '/3)');

    //Nombre y apellido
    if (usuario.Nombre == undefined || usuario.Apellido == undefined) {
        $('#texto_Nombre').text('Persona desconocida');
    } else {
        $('#texto_Nombre').text(toTitleCase(usuario.Nombre + " " + usuario.Apellido));
    }

    //Username
    if (usuario.Username == undefined || usuario.Username == "") {
        $('#texto_Usuario').html('Sin datos');
    } else {
        $('#texto_Usuario').html(usuario.Username);
    }

    //Sexo
    if (usuario.SexoMasculino == undefined) {
        $('#texto_Sexo').html('Sin datos');
    } else {
        $('#texto_Sexo').html(usuario.SexoMasculino ? 'Masculino' : 'Femenino');
    }

    //Dni
    if (usuario.Dni == undefined) {
        $('#texto_Dni').html('Sin datos');
    } else {
        $('#texto_Dni').html(usuario.Dni);
    }

    //Mail
    if (usuario.Email == null || usuario.Email.trim() == "") {
        $('#texto_Email').text('Sin datos');
    } else {
        $('#texto_Email').text(usuario.Email);
    }

    //Cuil
    if (usuario.Cuil == null || usuario.Cuil.trim() == "") {
        $('#texto_Cuil').text('Sin datos');
    } else {
        $('#texto_Cuil').text(usuario.Cuil);
    }

    //Telefono Fijo
    if (usuario.TelefonoFijo == null || usuario.TelefonoFijo.trim() == "") {
        $('#texto_TelefonoFijo').text('Sin datos');
    } else {
        $('#texto_TelefonoFijo').text(usuario.TelefonoFijo);
    }

    //Telefono Celular
    if (usuario.TelefonoCelular == null || usuario.TelefonoCelular.trim() == "") {
        $('#texto_TelefonoCelular').text('Sin datos');
    } else {
        $('#texto_TelefonoCelular').text(usuario.TelefonoCelular);
    }

    //Ambito
    if (getUsuarioLogeado().Ambito != undefined) {
        $('#texto_Ambito').text(toTitleCase(getUsuarioLogeado().Ambito.Nombre));
    } else {
        $('#texto_Ambito').text('Municipalidad de Córdoba');
    }

    if (getUsuarioLogeado().Usuario.Id == usuario.Id) {
        $('#contenedor_Rol').show();
        $('#texto_Rol').text(toTitleCase(getUsuarioLogeado().Rol.Rol));
    } else {
        $('#contenedor_Rol').hide();
    }

}

function ajaxActualizarDatosUsuarioLogeado() {
    const url = ResolveUrl('~/Servicios/UsuarioService.asmx/ActualizarDatosCerrojo')
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: url,
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

function procesarCambiarUsername(username) {
    crearAjax({
        Url: '~/Servicios/UsuarioService.asmx/CambiarUsername',
        Data: { username: username },
        OnSuccess: function (result) {
            if (!result.Ok) {
                callbackError(result.Error);
                return;
            }

            //cargo el nuevo username en el detalle
            if (username == undefined || username == "") {
                $('#texto_Usuario').html('Sin datos');
            } else {
                $('#texto_Usuario').html(username);
            }

            ajaxActualizarDatosUsuarioLogeado()
                              .then(function (data) {
                                  top.setUsuarioLogeado(data);
                                               })
                              .catch(function (error) {
                                  mostrarMensaje('Error', error);
                              });
        },
        OnError: function (result) {
            callbackError('Error procesando la solicitud');
        }
    })
}
