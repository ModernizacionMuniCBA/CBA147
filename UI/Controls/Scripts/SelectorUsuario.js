var SelectorUsuario_Usuario;

var SelectorUsuario_Callback;
var SelectorUsuario_CallbackMensaje;
var SelectorUsuario_CallbackCargando;
var SelectorUsuario_AbrirNuevoUsuarioSiNoEncuentro;

var SelectorUsuario_ModoEmpleado=false;

function initSelectorUsuario(data) {
    data = parse(data);

    //Tipo de busqueda
    var tipos = [
        //{
        //    Id: 1,
        //    Nombre: "Por N° Doc"
        //},
        {
            Id: 2,
            Nombre: "Por Nombre"
        },
        {
            Id: 3,
            Nombre: "Por Usuario"
        },
        {
            Id: 4,
            Nombre: "Por E-Mail"
        }
    ];

    $('#SelectorUsuario_Select_Tipo').CargarSelect({
        Data: tipos,
        Value: 'Id',
        Text: 'Nombre',
        TitleCase: false,
        Sort: false
    });

    //Al cambiar el tipo
    $('#SelectorUsuario_Select_Tipo').on('change', function () {
        var tipo = $('#SelectorUsuario_Select_Tipo').val();

        //$('#SelectorUsuario_ContenedorNumeroDocumento').stop(false, false).fadeOut(300);
        $('#SelectorUsuario_ContenedorUsername').stop(false, false).fadeOut(300);
        $('#SelectorUsuario_ContenedorEmail').stop(false, false).fadeOut(300);
        $('#SelectorUsuario_ContenedorNombre').stop(false, false).fadeOut(300);

        setTimeout(function () {
            switch (tipo) {
                //case "1": //Nro doc
                //    {
                //        $('#SelectorUsuario_ContenedorNumeroDocumento').stop(false, false).fadeIn(300, function () {

                //        });
                //    }
                //    break;

                case "2": //Nombre
                    {
                        $('#SelectorUsuario_ContenedorNombre').stop(false, false).fadeIn(300);
                    }
                    break;

                case "3": //Usuario
                    {
                        $('#SelectorUsuario_ContenedorUsername').stop(false, false).fadeIn(300);
                    }
                    break;

                case "4": //Email
                    {
                        $('#SelectorUsuario_ContenedorEmail').stop(false, false).fadeIn(300);
                    }
                    break;
            }
        }, 300);

    });

    //Enter en el numero de documento de la persona (dispara buscar)
    var inputs = "#SelectorUsuario_Input_Nombre, #SelectorUsuario_Input_Apellido, #SelectorUsuario_Input_Email, #SelectorUsuario_Input_Username";
    $(inputs).keydown(function (e) {
        if (e.keyCode == 13) {
            $('#SelectorUsuario_BtnBuscar').click();
        }
    });

    //Boton buscar persona
    $('#SelectorUsuario_BtnBuscar').click(function () {
        if ($(this).is(':disabled')) {
            return;
        }

        if (!SelectorUsuario_Validar()) {
            return;
        }

        var url;
        var dataAjax;
        var filtros;

        SelectorUsuario_MostrarCargando(true, 'Buscando usuario...');

        url = ResolveUrl('~/Servicios/UsuarioService.asmx/GetByFilters');

        var tipo = $('#SelectorUsuario_Select_Tipo').val();
        switch (tipo) {
            //case "1": //Numero
            //    {
            //        filtros = {
            //            NumeroDocumento: $('#SelectorUsuario_Input_NumeroDocumento').val().trim()
            //        };
            //    }
            //    break;
            case "2": //Nombre
                {
                    filtros = {
                        Nombre: $('#SelectorUsuario_Input_Nombre').val().trim(),
                        Apellido: $('#SelectorUsuario_Input_Apellido').val().trim()
                    };
                }
                break;
            case "3": //Usuario
                {
                    filtros = {
                        Username: $('#SelectorUsuario_Input_Username').val().trim()
                    };
                }
                break;
            case "4": //Mail
                {
                    filtros = {
                        Email: $('#SelectorUsuario_Input_Email').val().trim()
                    };
                }
                break;
        }

        if (soloEmpleados) {
            filtros.SoloEmpleados = true;
        }

        dataAjax = { filtros: filtros };

        crearAjax({
            Url: url,
            Data: dataAjax,
            OnSuccess: onSuccess,
            OnError: onError
        });

        function onSuccess(result) {
            //Oculto el cargando
            SelectorUsuario_MostrarCargando(false);

            //Error
            if (!result.Ok) {
                SelectorUsuario_MostrarMensajeError(result.Error);
                console.log('Error consultando el usuario');
                return;
            }

            //Si hay un solo usuario, lo selecciono
            if (result.Return.length == 1) {
                //Selecciono el usuario
                SelectorUsuario_Usuario = result.Return[0];
                SelectorUsuario_CargarUsuarioSeleccionado(SelectorUsuario_Usuario);
                return;
            }

            var hayUsuario = result.Return == null || result.Return.length == 0 ? false : true;
            var botones = [{ Texto: 'Cancelar' }];

            if (nuevoUsuarioVisible) {
                botones.push({
                    Texto: 'Crear usuario',
                    Class: 'colorExito',
                    OnClick: function () {
                        if (jAlertDialogoUsuario != undefined) {
                            $(jAlertDialogoUsuario).CerrarDialogo()
                        }

                        $('#SelectorUsuario_BtnNuevoUsuario').trigger('click');
                    }
                });
            }

            var jAlertDialogoUsuario;

            //Sino permito elegir
            crearDialogoIFrame({
                Titulo: 'Seleccionar Usuario',
                Url: ResolveUrl('~/IFrame/IUsuarioSelector.aspx'),
                OnLoad: function (jAlert, iFrame, iFrameContent) {
                    jAlertDialogoUsuario = jAlert;

                    iFrameContent.setUsuarios(result.Return);

                    //Callback
                    iFrameContent.setOnUsuarioSeleccionadoListener(function (usuario) {
                        if (usuario == null || usuario == undefined) return;

                        //Cierro el dialogo
                        $(jAlert).CerrarDialogo();

                        //Selecciono la persona
                        SelectorUsuario_Usuario = usuario;

                        SelectorUsuario_CargarUsuarioSeleccionado(SelectorUsuario_Usuario);
                    });
                },
                Botones: botones
            });
        }

        function onError(result) {

            //Oculto el cargando
            SelectorUsuario_MostrarCargando(false);

            //Muestro el error
            SelectorUsuario_MostrarMensajeError('Error consultando el usuario');
            console.log('Error consultando el usuario');
            console.log(dataAjax);
            console.log(result);
        }

    });

    //Boton Cancelar Usuario
    $('#SelectorUsuario_BtnCancelarUsuario').click(function () {
        SelectorUsuario_CancelarUsuarioSeleccionado();
    });

    //Boton Detalle Persona
    $('#SelectorUsuario_BtnDetalleUsuario').click(function () {
        if (SelectorUsuario_Usuario == undefined) {
            SelectorUsuario_MostrarMensajeError('No seleccionó ningun usuario');
            return;
        }

        crearDialogoIFrame({
            Titulo: 'Detalle de Usuario',
            Url: ResolveUrl('~/IFrame/IUsuarioDetalle.aspx?Id=' + SelectorUsuario_Usuario.Id),
            Botones:
                [
                    {
                        Texto: 'Aceptar',
                        Class: 'colorExito'
                    }
                ]
        });
    });

    //Nuevo Usuario
    $('#SelectorUsuario_BtnNuevoUsuario').click(function () {

        crearDialogoUsuarioNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                if (SelectorUsuario_CallbackMensaje != undefined) {
                    SelectorUsuario_CallbackMensaje(tipo, mensaje);
                }
            },
            Callback: function (usuario) {
                SelectorUsuario_CargarUsuarioSeleccionado(usuario);
            },
            Empleado: SelectorUsuario_ModoEmpleado
        });
    });

}

function SelectorUsuario_MostrarNuevoUsuario(filtros) {
    if (!SelectorUsuario_AbrirNuevoUsuarioSiNoEncuentro) return;

    if (filtros != undefined) {
        if ('Nombre' in filtros) {
            nombreRecomendado = filtros.Nombre;
        }
        if ('Apellido' in filtros) {
            apellidoRecomendado = filtros.Apellido;
        }
        //if ('NumeroDocumento' in filtros) {
        //    nroDocRecomendado = filtros.NumeroDocumento;
        //}
        if ('Email' in filtros) {
            emailRecomendado = filtros.Email;
        }
        if ('Username' in filtros) {
            usernameRecomendado = filtros.Username;
        }
    }

    $('#SelectorUsuario_BtnNuevoUsuario').trigger('click');
    nombreRecomendado = undefined;
    apellidoRecomendado = undefined;
    //nroDocRecomendado = undefined;
    emailRecomendado = undefined;
    usernameRecomendado = undefined;
}

function SelectorUsuario_Validar() {
    //Borro las validaciones
    SelectorUsuario_BorrarValidaciones();

    var validar = true;

    //Tipo Documento
    var tipoBusqueda = $('#SelectorUsuario_Select_Tipo').val();
    switch (tipoBusqueda) {
        //case "1": //Numero
        //    {
        //        var nroDocumento = $('#SelectorUsuario_Input_NumeroDocumento').val();
        //        if (nroDocumento == undefined || nroDocumento == "") {
        //            $('#SelectorUsuario_Input_NumeroDocumento').siblings('.control-observacion').text('Dato requerido');
        //            $('#SelectorUsuario_Input_NumeroDocumento').siblings('.control-observacion').stop(true, true).slideDown(300);
        //            validar = false;
        //        }
        //    }
        //    break;
        case "2": //Nombre
            {
                var nombre = $('#SelectorUsuario_Input_Nombre').val().trim();
                var apellido = $('#SelectorUsuario_Input_Apellido').val().trim();

                if ((nombre == undefined || nombre == "") && (apellido == undefined || apellido == "")) {
                    $('#SelectorUsuario_Input_Nombre').siblings('.control-observacion').text('Dato requerido');
                    $('#SelectorUsuario_Input_Nombre').siblings('.control-observacion').stop(true, true).slideDown(300);
                    validar = false;
                }
            }
            break;
        case "3": //Usuario
            {
                var username = $('#SelectorUsuario_Input_Username').val();
                if (username == undefined || username == "") {
                    $('#SelectorUsuario_Input_Username').siblings('.control-observacion').text('Dato requerido');
                    $('#SelectorUsuario_Input_Username').siblings('.control-observacion').stop(true, true).slideDown(300);
                    validar = false;
                }
            }
            break;
        case "4": //Mail
            {
                var email = $('#SelectorUsuario_Input_Email').val();
                if (email == undefined || email == "") {
                    $('#SelectorUsuario_Input_Email').siblings('.control-observacion').text('Dato requerido');
                    $('#SelectorUsuario_Input_Email').siblings('.control-observacion').stop(true, true).slideDown(300);
                    validar = false;
                }
            }
            break;
    }

    return validar;
}

function SelectorUsuario_BorrarValidaciones() {
    $('#SelectorUsuario_Select_TipoPersona').siblings('.control-observacion').text('');
    $('#SelectorUsuario_Select_TipoPersona').siblings('.control-observacion').stop(true, true).slideUp(300);
    //$('#SelectorUsuario_Input_NumeroDocumento').siblings('.control-observacion').text('');
    //$('#SelectorUsuario_Input_NumeroDocumento').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#SelectorUsuario_Input_Nombre').siblings('.control-observacion').text('');
    $('#SelectorUsuario_Input_Nombre').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#SelectorUsuario_Input_Apellido').siblings('.control-observacion').text('');
    $('#SelectorUsuario_Input_Apellido').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#SelectorUsuario_Input_Username').siblings('.control-observacion').text('');
    $('#SelectorUsuario_Input_Username').siblings('.control-observacion').stop(true, true).slideUp(300);
    $('#SelectorUsuario_Input_Email').siblings('.control-observacion').text('');
    $('#SelectorUsuario_Input_Email').siblings('.control-observacion').stop(true, true).slideUp(300);
}

function SelectorUsuario_CargarUsuarioSeleccionado(usuario) {
    SelectorUsuario_Usuario = usuario;

    $('#SelectorUsuario_ContenedorBusqueda').fadeOut(300, function () {
        $('#SelectorUsuario_ContenedorUsuarioSeleccionado .entity-detalle .titulo').text(toTitleCase(usuario.Nombre + ' ' + usuario.Apellido));
        $('#SelectorUsuario_ContenedorUsuarioSeleccionado .entity-detalle .detalle').text('Usuario: ' + usuario.Username);
        $('#SelectorUsuario_ContenedorUsuarioSeleccionado').fadeIn(300);
    });

    if (SelectorUsuario_Callback != undefined && SelectorUsuario_Callback != null) {
        SelectorUsuario_Callback(SelectorUsuario_Usuario);
    }
}

function SelectorUsuario_CancelarUsuarioSeleccionado() {
    this.SelectorUsuario_Usuario = undefined;

    $('#SelectorUsuario_ContenedorUsuarioSeleccionado').stop(true, true).fadeOut(300, function () {

        //$('#SelectorUsuario_Input_NumeroDocumento').val('');
        $('#SelectorUsuario_Input_Nombre').val('');
        $('#SelectorUsuario_Input_Apellido').val('');
        $('#SelectorUsuario_Input_Email').val('');
        $('#SelectorUsuario_Input_Username').val('');

        Materialize.updateTextFields();

        $('#SelectorUsuario_ContenedorBusqueda').stop(true, true).fadeIn(300);

        //Enfoco segun tipo
    });

    if (SelectorUsuario_Callback != undefined && SelectorUsuario_Callback != null) {
        SelectorUsuario_Callback(null);
    }
}

function SelectorUsuario_SetOnUsuarioSeleccionadoListener(callback) {
    this.SelectorUsuario_Callback = callback;
}

function SelectorUsuario_GetUsuarioSeleccionado() {
    return SelectorUsuario_Usuario;
}

function SelectorUsuario_IsUsuarioSeleccionado() {
    return SelectorUsuario_Usuario != undefined && SelectorUsuario_Usuario != null;
}

function SelectorUsuario_IsDatosIngresadosSinUsuarioSeleccionado() {
    var a = !SelectorUsuario_IsUsuarioSeleccionado();
    var b = false;
    //var b = $('#SelectorUsuario_Input_NumeroDocumento').val().trim() != "";
    return a && b
}

function SelectorUsuario_SetUsuario(usuario) {
    SelectorUsuario_Usuario = usuario;
    SelectorUsuario_CargarUsuarioSeleccionado(usuario);
}

function SelectorUsuario_SetModoEmpleado(modoEmpleado) {
    SelectorUsuario_ModoEmpleado = modoEmpleado;
}

function SelectorUsuario_ReiniciarUI() {
    SelectorUsuario_Usuario = undefined;

    SelectorUsuario_BorrarValidaciones();

    $('#SelectorUsuario_Select_Tipo').val('2').trigger('change');
    //$('#SelectorUsuario_Input_NumeroDocumento').val('');
    $('#SelectorUsuario_ContenedorBusqueda').show();
    $('#SelectorUsuario_ContenedorUsuarioSeleccionado').hide();
    Materialize.updateTextFields();
}

function SelectorUsuario_SetAbrirNuevoUsuarioSiNoEncuentro(value) {
    SelectorUsuario_AbrirNuevoUsuarioSiNoEncuentro = value;
}

var nuevoUsuarioVisible = true;
function SelectorUsuario_SetVisibleNuevoUsuario(visible) {
    nuevoUsuarioVisible = visible;
    if (visible) {
        $('#SelectorUsuario_BtnNuevoUsuario').show();
    } else {
        $('#SelectorUsuario_BtnNuevoUsuario').hide();
    }
}

var soloEmpleados = false;
function SelectorUsuario_SetSoloEmpleado(soloE) {
    soloEmpleados = soloE;
}

//-------------------------------
// Cargando
//-------------------------------

function SelectorUsuario_MostrarCargando(mostrar, mensaje) {
    $('#SelectorUsuario_Select_Tipo').prop('disabled', mostrar);
    //$('#SelectorUsuario_Input_NumeroDocumento').prop('disabled', mostrar);
    $('#SelectorUsuario_Input_Apellido').prop('disabled', mostrar);
    $('#SelectorUsuario_Input_Nombre').prop('disabled', mostrar);
    $('#SelectorUsuario_Input_Email').prop('disabled', mostrar);
    $('#SelectorUsuario_Input_Username').prop('disabled', mostrar);
    $('#SelectorUsuario_BtnBuscar').prop('disabled', mostrar);

    if (SelectorUsuario_CallbackCargando != undefined) {
        SelectorUsuario_CallbackCargando(mostrar, mensaje);
    }
}

function SelectorUsuario_SetOnCargandoListener(callback) {
    this.SelectorUsuario_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function SelectorUsuario_SetOnMensajeListener(callback) {
    this.SelectorUsuario_CallbackMensaje = callback;
}

function SelectorUsuario_MostrarMensajeError(mensaje) {
    if (SelectorUsuario_CallbackMensaje == undefined) return;
    SelectorUsuario_CallbackMensaje('Error', mensaje);
}

function SelectorUsuario_MostrarMensajeAlerta(mensaje) {
    if (SelectorUsuario_CallbackMensaje == undefined) return;
    SelectorUsuario_CallbackMensaje('Alerta', mensaje);
}

function SelectorUsuario_MostrarMensajeInfo(mensaje) {
    if (SelectorUsuario_CallbackMensaje == undefined) return;
    SelectorUsuario_CallbackMensaje('Info', mensaje);
}

function SelectorUsuario_MostrarMensajeExito(mensaje) {
    if (SelectorUsuario_CallbackMensaje == undefined) return;
    SelectorUsuario_CallbackMensaje('Exito', mensaje);
}
