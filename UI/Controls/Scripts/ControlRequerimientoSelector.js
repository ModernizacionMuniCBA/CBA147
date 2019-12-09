var RequerimientoSelector_Requerimiento;

var RequerimientoSelector_Callback;
var RequerimientoSelector_CallbackMensaje;
var RequerimientoSelector_CallbackCargando;

$(function () {

    //Enter en el numero de reclamo
    $('#RequerimientoSelector_Input_NumeroRequerimiento').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#RequerimientoSelector_BtnBuscar').click();
        }
    })

    //Boton buscar reclamo
    $('#RequerimientoSelector_BtnBuscar').click(function () {
        if ($(this).is(':disabled')) {
            return;
        }

        if (!RequerimientoSelector_Validar()) {
            $('#RequerimientoSelector_Input_NumeroRequerimiento').trigger('focus');
            return;
        }

        //Muestro cargando
        RequerimientoSelector_MostrarCargando(true, 'Buscando reclamo...');

        var nroRequerimiento = $('#RequerimientoSelector_Input_NumeroRequerimiento').val();
        var anio = $('#RequerimientoSelector_Input_Anio').val();
        if (anio == "") {
            anio = null;
        }
        var filtros = { Numero: nroRequerimiento, Año: anio };

        var url = ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTabla');

        crearAjax({
            Url: url,
            Data: {consulta: filtros},
            OnSuccess: function (result) {
                //Oculto el cargando
                RequerimientoSelector_MostrarCargando(false);

                if (!result.Ok) {
                    RequerimientoSelector_MostrarMensajeError(result.Error);
                    console.log('Error consultando el requerimiento');
                    console.log(data);
                    console.log(result);

                    $('#RequerimientoSelector_Input_NumeroRequerimiento').trigger('focus');
                    return;
                }

                if (result.Return == null || result.Return.Cantidad == 0) {
                    RequerimientoSelector_MostrarMensajeAlerta('Requerimiento no encontrado');
                    $('#RequerimientoSelector_Input_NumeroRequerimiento').trigger('focus');
                    return;
                }

                if (result.Return.Cantidad == 1) {
                    RequerimientoSelector_Requerimiento = result.Return.Data[0];
                    RequerimientoSelector_CargarRequerimientoSeleccionado(RequerimientoSelector_Requerimiento);
                } else {
                    RequerimientoSelector_MostrarMensajeAlerta('Se encontró mas de un requerimiento. Por favor indique tambien el año.');
                }
            },
            OnError: function (result) {
                //Informo el error
                RequerimientoSelector_MostrarMensajeError('Error al buscar el requerimiento');
                console.log('Error consultando el requerimiento');
                console.log(data);
                console.log(result);

                //Oculto el cargando
                RequerimientoSelector_MostrarCargando(false);
            }
        });
    });

    //Boton Cancelar Requerimiento
    $('#RequerimientoSelector_BtnCancelarRequerimiento').click(function () {
        RequerimientoSelector_CancelarRequerimientoSeleccionado();
    });

    //Boton Detalle Requerimiento
    $('#RequerimientoSelector_BtnDetalleRequerimiento').click(function () {
        if (RequerimientoSelector_Requerimiento == undefined) {
            RequerimientoSelector_MostrarMensajeError('No seleccionó ningun requerimiento');
            return;
        }

        crearDialogoIFrame({
            Titulo: 'Detalle de Requerimiento',
            Url: ResolveUrl('~/IFrame/IRequerimientoDetalle.aspx?Id=' + RequerimientoSelector_Requerimiento.Id),
            Botones:
                [
                    {
                        Texto: 'Aceptar',
                        Class: 'colorExito'
                    }
                ]
        });
    });

    $('#RequerimientoSelector_Tabla').DataTableReclamo({
        Botones: [
           {
               Titulo: 'Seleccionar',
               Icono: 'check',
               OnClick: function (data) {
                   RequerimientoSelector_Requerimiento = data;
                   RequerimientoSelector_CargarRequerimientoSeleccionado(data);
               }
           }
        ],
        VerDomicilio: false,
    });
});

function RequerimientoSelector_Validar() {
    RequerimientoSelector_BorrarValidaciones();

    var validar = true;

    //Nro reclamo
    var nroRequerimiento = $('#RequerimientoSelector_Input_NumeroRequerimiento').val();
    if (nroRequerimiento == undefined || nroRequerimiento == "") {
        $('#RequerimientoSelector_Input_NumeroRequerimiento').siblings('.control-observacion').text('Dato requerido');
        $('#RequerimientoSelector_Input_NumeroRequerimiento').siblings('.control-observacion').stop(true, true).slideDown(300);
        validar = false;
    }

    return validar;
}

function RequerimientoSelector_BorrarValidaciones() {
    $('#RequerimientoSelector_Input_NumeroRequerimiento').siblings('.control-observacion').text('');
    $('#RequerimientoSelector_Input_NumeroRequerimiento').siblings('.control-observacion').stop(true, true).slideUp(300);

    $('#RequerimientoSelector_Input_Anio').siblings('.control-observacion').text('');
    $('#RequerimientoSelector_Input_Anio').siblings('.control-observacion').stop(true, true).slideUp(300);
}

function RequerimientoSelector_CargarRequerimientoSeleccionado(requerimiento) {
    this.RequerimientoSelector_Requerimiento = requerimiento;

    $('#RequerimientoSelector_ContenedorBusqueda').fadeOut(300, function () {

        $('#RequerimientoSelector_ContenedorRequerimientoSeleccionado').find('.requerimiento-titulo').text('RECLAMO N° ' + RequerimientoSelector_Requerimiento.Numero);
        $('#RequerimientoSelector_ContenedorRequerimientoSeleccionado').find('.requerimiento-detalle').text(toTitleCase(RequerimientoSelector_Requerimiento.MotivoString));


        $('#RequerimientoSelector_ContenedorRequerimientoSeleccionado').fadeIn(300);
    });

    if (RequerimientoSelector_Callback != undefined && RequerimientoSelector_Callback != null) {
        RequerimientoSelector_Callback(RequerimientoSelector_Requerimiento);
    }
}

function RequerimientoSelector_CancelarRequerimientoSeleccionado() {
    this.RequerimientoSelector_Requerimiento = undefined;

    $('#RequerimientoSelector_ContenedorRequerimientoSeleccionado').stop(true, true).fadeOut(300, function () {

        $('#RequerimientoSelector_Input_NumeroRequerimiento').val('');

        Materialize.updateTextFields();

        $('#RequerimientoSelector_ContenedorBusqueda').stop(true, true).fadeIn(300);
        //Enfoco en el numero de requerimiento
        $('#RequerimientoSelector_Input_NumeroRequerimiento').trigger('focus');
    });

    if (RequerimientoSelector_Callback != undefined && RequerimientoSelector_Callback != null) {
        RequerimientoSelector_Callback(null);
    }
}

function RequerimientoSelector_GetRequerimientoSeleccionado() {
    return RequerimientoSelector_Requerimiento;
}

function RequerimientoSelector_IsRequerimientoSeleccionado() {
    return RequerimientoSelector_Requerimiento != undefined && RequerimientoSelector_Requerimiento != null;
}

function RequerimientoSelector_IsDatosIngresadosSinRequerimientoSeleccionado() {
    var a = !RequerimientoSelector_IsRequerimientoSeleccionado();
    var b = $('#RequerimientoSelector_Input_NumeroRequerimiento').val().trim() != "";
    return a && b
}

function RequerimientoSelector_ReiniciarUI() {
    RequerimientoSelector_Requerimiento = undefined;

    RequerimientoSelector_BorrarValidaciones();

    $('#RequerimientoSelector_Input_NumeroRequerimiento').val('');
    $('#RequerimientoSelector_ContenedorBusqueda').show();
    $('#RequerimientoSelector_ContenedorRequerimientoSeleccionado').hide();
    Materialize.updateTextFields();


    if (RequerimientoSelector_Callback != undefined && RequerimientoSelector_Callback != null) {
        RequerimientoSelector_Callback(null);
    }
}

function RequerimientoSelector_Focus() {
    $('#RequerimientoSelector_Input_NumeroRequerimiento').trigger('focus');
}

//-------------------------------
// Seleccion
//-------------------------------

function RequerimientoSelector_SetOnRequerimientoSeleccionadoListener(callback) {
    this.RequerimientoSelector_Callback = callback;
}

//-------------------------------
// Cargando
//-------------------------------

function RequerimientoSelector_MostrarCargando(mostrar, mensaje) {

    $('#RequerimientoSelector_Input_NumeroRequerimiento').prop('disabled', mostrar);
    $('#RequerimientoSelector_BtnBuscar').prop('disabled', mostrar);

    if (RequerimientoSelector_CallbackCargando != undefined) {
        RequerimientoSelector_CallbackCargando(mostrar, mensaje);
    }
}

function RequerimientoSelector_SetOnCargandoListener(callback) {
    this.RequerimientoSelector_CallbackCargando = callback;
}

//-----------------------------
// Mensajes
//-----------------------------

function RequerimientoSelector_SetOnMensajeListener(callback) {
    this.RequerimientoSelector_CallbackMensaje = callback;

}

function RequerimientoSelector_MostrarMensajeError(mensaje) {
    if (RequerimientoSelector_CallbackMensaje == undefined) return;
    RequerimientoSelector_CallbackMensaje('Error', mensaje);
}

function RequerimientoSelector_MostrarMensajeAlerta(mensaje) {
    if (RequerimientoSelector_CallbackMensaje == undefined) return;
    RequerimientoSelector_CallbackMensaje('Alerta', mensaje);
}

function RequerimientoSelector_MostrarMensajeInfo(mensaje) {
    if (RequerimientoSelector_CallbackMensaje == undefined) return;
    RequerimientoSelector_CallbackMensaje('Info', mensaje);
}
