var SelectorMotivo_Manual = true;
var SelectorMotivo_Motivo;
var SelectorMotivo_MotivosSeleccionados = [];
var SelectorMotivo_Servicio;

var SelectorMotivo_Servicios;
var SelectorMotivo_Motivos;

var SelectorMotivo_Callback;
var SelectorMotivo_CallbackServicio;
var SelectorMotivo_CallbackModo;

var SelectorMotivo_CallbackMensaje;
var SelectorMotivo_CallbackCargando;
var SelectorMotivo_ModoUrgentes = 'urgentes';
var SelectorMotivo_ModoNormal = 'normal';
var SelectorMotivo_Modo = SelectorMotivo_ModoNormal;

var SelectorMotivo_TipoGeneral = "1";
var SelectorMotivo_TipoInterno = "2";
var SelectorMotivo_TipoPrivado = "3";
var SelectorMotivo_Tipo = null;

function initSelectorMotivo(data) {
    data = parse(data);

    $('#SelectorMotivo_ContenedorManual').removeClass('hide');
    $('#SelectorMotivo_ContenedorManual').hide();

    SelectorMotivo_InitSwitch();
    SelectorMotivo_InitCardBusqueda();
    SelectorMotivo_InitSelects();

    $('#SelectorMotivo_BtnCancelarMotivo').click(function () {
        SelectorMotivo_OcultarMotivoSeleccionado();
    })


    //-----------------------------------
    // Cargo los datos desde el server
    //-----------------------------------

    SelectorMotivo_Servicios = data.Servicios;
    SelectorMotivo_Motivos = data.Motivos;

    SelectorMotivo_CargarServicios();

    var manualPorDefecto = true;
    if (manualPorDefecto) {
        $('#SelectorMotivo_Switch').find('input').prop('checked', true);
    } else {
        $('#SelectorMotivo_Switch').find('input').prop('checked', false);
    }
    $('#SelectorMotivo_Switch').trigger('change');
}

function SelectorMotivo_InitSwitch() {
    //Switch
    $('#SelectorMotivo_Switch').find('.opcion1').addClass('active');
    $('#SelectorMotivo_Switch').find('.opcion2').removeClass('active');

    //Click opcion1
    $('#SelectorMotivo_Switch').find('.opcion1').click(function () {
        $('#SelectorMotivo_Switch').find('input').prop('checked', false);
        $('#SelectorMotivo_Switch').trigger('change');
    });

    //Click opcion2
    $('#SelectorMotivo_Switch').find('.opcion2').click(function () {
        $('#SelectorMotivo_Switch').find('input').prop('checked', true);
        $('#SelectorMotivo_Switch').trigger('change');
    });

    //Evento al cambiar
    $('#SelectorMotivo_Switch').change(function () {
        var nuevoModo = $('#SelectorMotivo_Switch').find('input').is(':checked');
        SelectorMotivo_Manual = nuevoModo;

        if (SelectorMotivo_CallbackModo) {
            SelectorMotivo_CallbackModo(nuevoModo);
        }

        $('#SelectorMotivo_Switch').find('input').prop('disabled', true);

        if (!SelectorMotivo_Manual) {

            //Deselecciono el servicio
            if (SelectorMotivo_Servicio != undefined) {
                $('#SelectorMotivo_Select_Servicio').val('-1').trigger('change');
            }

            //Deselecciono el motivo
            if (SelectorMotivo_Motivo != undefined) {
                $('#SelectorMotivo_Select_Motivo').val('-1').trigger('change');
            }

            if (idArea != undefined) {
                $.each(SelectorMotivo_Motivos, function (i, motivo) {
                    if (motivo.IdArea == idArea) {
                        $('#SelectorMotivo_Select_Servicio').val(motivo.Servicio.Id).trigger('change', false);
                        $('#SelectorMotivo_Select_Servicio').prop('disabled', true);
                        return;
                    }
                });
            }

            $('#SelectorMotivo_Switch').find('.opcion1').addClass('active');
            $('#SelectorMotivo_Switch').find('.opcion2').removeClass('active');

            $('#SelectorMotivo_ContenedorManual').stop(true, true).fadeOut(300, function () {
                $('#SelectorMotivo_ContenedorBusqueda').stop(true, true).fadeIn(300, function () {
                    $('#SelectorMotivo_Switch').find('input').prop('disabled', false);
                });

                //Foco en busqueda
                $('#SelectorMotivo_Input_Buscar').trigger('focus');
            });
        } else {
            //Borro el texto de la busqueda
            $('#SelectorMotivo_Input_Buscar').val('');
            Materialize.updateTextFields();

            $('#SelectorMotivo_Switch').find('.opcion1').removeClass('active');
            $('#SelectorMotivo_Switch').find('.opcion2').addClass('active');

            $('#SelectorMotivo_ContenedorBusqueda').stop(true, true).fadeOut(300, function () {
                $('#SelectorMotivo_ContenedorManual').stop(true, true).fadeIn(300, function () {
                    $('#SelectorMotivo_Switch').find('input').prop('disabled', false);
                });
            });

            //Foco en busqueda
            $('#SelectorMotivo_Input_Buscar').trigger('blur');
        }
    });
}

function SelectorMotivo_InitCardBusqueda() {
    var hmax = 200;
    $('#SelectorMotivo_Card').css('position', 'fixed');
    $('#SelectorMotivo_Card').css('z-index', '10');


    var dt = $('#SelectorMotivo_Card').find('table').DataTable({
        lengthChange: false,
        searching: false,
        paginate: false,
        bDestroy: true,
        bAutoWidth: false,
        columns: [
            {
                sTitle: 'Servicio',
                mData: 'Servicio',
                width: '40%',
                orderable: false,
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data.Nombre) + '</span></div>';
                }
            },
            {
                sTitle: 'Motivo',
                mData: 'Nombre',
                width: '60%',
                orderable: false,
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            }
        ],
        "columnDefs": [{ "defaultContent": "", "targets": "_all" }],
        "oLanguage": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Tamaño de pagina _MENU_",
            "sZeroRecords": "",
            "sEmptyTable": "",
            "sInfo": "_START_-_END_ de _TOTAL_",
            "sInfoEmpty": "",
            "sInfoFiltered": "(filtrado de un total de _MAX_)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "<i class='material-icons'>chevron_right</i>",
                "sPrevious": "<i class='material-icons'>chevron_left</i>"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        }
    });

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });
    //Muevo el indicador y el paginado a mi propio div
    $('#SelectorMotivo_Card').find('.tabla-footer').empty();
    $('#SelectorMotivo_Card').find('.dataTables_info').detach();
    $('#SelectorMotivo_Card').find('.dataTables_paginate').detach();


    $('#SelectorMotivo_Input_Buscar').focusout(function () {
        $('#SelectorMotivo_Card').stop(true, true).slideUp(300).fadeOut(300);
    });

    $('#SelectorMotivo_Input_Buscar').on('input', function () {
        //Posiciono correctamente
        var w = $('#SelectorMotivo_Input_Buscar').width();
        $('#SelectorMotivo_Card').css('width', w);

        var pos = $('#SelectorMotivo_Input_Buscar').offset();
        pos.top = pos.top + $('#SelectorMotivo_Input_Buscar').height();
        $('#SelectorMotivo_Card').css(pos);



        //Obtengo el valor tipeado
        var contenido = $('#SelectorMotivo_Input_Buscar').val().trim();

        //Si no hay nada, cierro
        if (contenido == undefined || contenido == "") {
            $('#SelectorMotivo_Card').stop(true, true).slideUp(300).fadeOut(300);
            return;
        }

        //Obtengo los motivos
        var resultado = [];
        $.each(SelectorMotivo_Motivos, function (index, val) {
            if (val.Nombre.toLowerCase().indexOf(contenido.toLowerCase()) >= 0) {
                if (SelectorMotivo_Modo == SelectorMotivo_ModoNormal || (SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes && val.Urgente)) {
                    if (idArea != undefined) {
                        if (val.IdArea == idArea) {
                            resultado.push(val);
                        }
                    } else {
                        resultado.push(val);
                    }
                }
            }
        });

        //Si no tengo motivos cierro
        if (resultado.length == 0) {
            $('#SelectorMotivo_Card').stop(true, true).slideUp(300).fadeOut(300);
            return;
        }

        var h = 49 * resultado.length + 49;
        if (h > hmax) {
            h = hmax;
        }

        $('#SelectorMotivo_Card').find('.contenido').height(h);

        //Agrego la info nueva
        var dt = $('#SelectorMotivo_Card').find('table').DataTable();
        dt.clear();
        dt.rows.add(resultado);
        dt.draw();

        //Muestro la card
        $('#SelectorMotivo_Card').stop(true, true).slideDown(300);

        //Click en cada fila
        dt.$('tr').mousedown(function () {

            var data = dt.row($(this)).data();

            if (!SelectorMotivo_Multiple) {
                //Guardo la seleccion
                SelectorMotivo_Servicio = $.grep(SelectorMotivo_Servicios, function (e) { return e.Id == data.Servicio.Id; })[0];
                SelectorMotivo_Motivo = data;

                //Informo la seleccion
                setTimeout(function () {
                    if (SelectorMotivo_Callback != undefined) {
                        SelectorMotivo_Callback(SelectorMotivo_Motivo);
                    }
                    if (SelectorMotivo_CallbackServicio != undefined) {
                        SelectorMotivo_CallbackServicio(SelectorMotivo_Servicio)
                    }
                }, 500);

                //Muestro la seleccion
                SelectorMotivo_MostrarMotivoSeleccionado();

                //Oculto el card
                $('#SelectorMotivo_Card').stop(true, true).slideUp(300);
            }
        });
    });
}

function SelectorMotivo_InitSelects() {
    //Cargo los servicios
    $('#SelectorMotivo_Select_Servicio').CargarSelect({
        Data: [],
        Default: 'Seleccione...',
        Value: 'Id',
        Text: 'Nombre'
    });

    var def = SelectorMotivo_Multiple ? null : 'Seleccione...'
    //Cargo los motivos
    $('#SelectorMotivo_Select_Motivo').prop('disabled', true);
    $('#SelectorMotivo_Select_Motivo').CargarSelect({
        Multiple: SelectorMotivo_Multiple,
        Data: [],
        Default: def,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    //Evento Servicio
    $('#SelectorMotivo_Select_Servicio').on('change', function (e) {
        var value = $(this).val();

        $('#SelectorMotivo_Select_Motivo').val('-1').trigger('change');
        $('#SelectorMotivo_Select_Motivo').prop('disabled', true);

        if (value == -1 && !SelectorMotivo_Multiple) {
            return;
        } else if (value == -1 && SelectorMotivo_Multiple) {

            //Borro el servicio seleccionado
            SelectorMotivo_Servicio = undefined;

            var data = SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes ? GetMotivosUrgentes() : SelectorMotivo_Motivos;

            //me fijo que tipo son 
            if (SelectorMotivo_Tipo != null) {
                data = _.where(data, { Tipo: SelectorMotivo_Tipo});
            }

            $('#SelectorMotivo_Select_Motivo').prop('disabled', false);

            var def = SelectorMotivo_Multiple ? null : 'Seleccione...'

            //Cargo el select
            $('#SelectorMotivo_Select_Motivo').CargarSelect({
                Data: data,
                Multiple: SelectorMotivo_Multiple,
                Default: def,
                Value: 'Id',
                Text: 'Nombre',
                Sort: true
            });

            $('#SelectorMotivo_Select_Motivo').prop('disabled', false);

            //Informo
            if (SelectorMotivo_CallbackServicio != undefined) {
                SelectorMotivo_CallbackServicio(SelectorMotivo_Servicio);
            }
            return;
        }

        //Borro el error
        $('#SelectorMotivo_Select_Servicio').siblings('.control-observacion').text('');

        //Bloqueo el select
        $('#SelectorMotivo_Select_Motivo').prop('disabled', true);

        //Obtengo los motivos
        var resultado = SelectorMotivo_GetMotivosPorServicio(value);

        var mensaje = 'No hay motivos ';
        if (SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes) {
            mensaje += 'peligrosos ';
        }
        mensaje += 'para éste servicio.';

        if (resultado.length == 0) {
            SelectorMotivo_MostrarMensajeAlerta(mensaje);
            return;
        }
        //Desbloqueo el select
        $('#SelectorMotivo_Select_Motivo').prop('disabled', false);

        var def = SelectorMotivo_Multiple ? null : 'Seleccione...'

        //Cargo el select
        $('#SelectorMotivo_Select_Motivo').CargarSelect({
            Data: resultado,
            Multiple: SelectorMotivo_Multiple,
            Default: def,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true
        });


        //Guardo el servicio seleccionado
        SelectorMotivo_Servicio = $.grep(SelectorMotivo_Servicios, function (e) { return e.Id == value; })[0];;

        //Informo el servicio
        if (SelectorMotivo_CallbackServicio != undefined) {
            SelectorMotivo_CallbackServicio(SelectorMotivo_Servicio);
        }

    });

    //Evento Motivo
    $('#SelectorMotivo_Select_Motivo').on('change', function (e) {
        var value = $(this).val();
        SelectorMotivo_MotivosSeleccionados = [];

        if (SelectorMotivo_Multiple) {

            if (value.length == 0) {
                SelectorMotivo_Motivo = [];
                if (SelectorMotivo_Callback != undefined) {
                    SelectorMotivo_Callback(SelectorMotivo_MotivosSeleccionados);
                }
                return;
            }

            //Borro el error
            $('#SelectorMotivo_Select_Motivo').siblings('.control-observacion').text('');

            //agarro los motivos seleccionados
            var mot = $.grep(SelectorMotivo_Motivos, function (e) {
                return $.grep(value, function (a) {
                    return e.Id == a;
                }).length > 0;
            });

            //los guardo
            SelectorMotivo_MotivosSeleccionados = mot;

            //Informo los motivo
            setTimeout(function () {
                if (SelectorMotivo_Callback != undefined) {
                    SelectorMotivo_Callback(SelectorMotivo_MotivosSeleccionados);
                }
            }, 500);

            return;
        }

        if (value == -1) {
            //Informo el motivo
            SelectorMotivo_Motivo = undefined;
            if (SelectorMotivo_Callback != undefined) {
                SelectorMotivo_Callback(SelectorMotivo_Motivo);
            }
            return;
        }

        //Borro el error
        $('#SelectorMotivo_Select_Motivo').siblings('.control-observacion').text('');

        //averiguo si estaba seleccionado
        var mot = $.grep(SelectorMotivo_Motivos, function (e) { return e.Id == value; })[0];

        //Guardo el motivo
        SelectorMotivo_Motivo = $.grep(SelectorMotivo_Motivos, function (e) { return e.Id == value; })[0];

        //Informo el motivo
        setTimeout(function () {
            if (SelectorMotivo_Callback != undefined) {
                SelectorMotivo_Callback(SelectorMotivo_Motivo);
            }
        }, 500);

        //Muestro el motivo
        SelectorMotivo_MostrarMotivoSeleccionado(SelectorMotivo_Motivo);
    });

    function GetMotivosUrgentes(servicio) {
        var resultado = [];
        $.each(SelectorMotivo_Motivos, function (index, val) {
            //verifico que cumpla con urgente
            if (SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes && !val.Urgente) return;

            resultado.push(val);
        });
        return resultado;
    }
}

function SelectorMotivo_GetMotivosPorServicio(servicio) {
    var resultado = [];
    $.each(SelectorMotivo_Motivos, function (index, val) {
        //verifico que cumpla con el filtro de tipo de motivo
        if (SelectorMotivo_Tipo != null) {
            if (''+val.Tipo != SelectorMotivo_Tipo) return;
        }

        //verifico que sea el mismo servicio
        if (val.Servicio.Id != servicio) return;

        //verifico que cumpla con urgente
        if (SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes && !val.Urgente) return;

        resultado.push(val);
    });
    return resultado;
}

function SelectorMotivo_CargarServicios() {
    let data = _.filter(SelectorMotivo_Servicios, function (servicio) {
        return _.where(SelectorMotivo_GetMotivosPorServicio(servicio.Id)).length > 0;
    });
    //Cargo los datos
    $('#SelectorMotivo_Select_Servicio').CargarSelect({
        Data: data,
        Value: 'Id',
        Text: 'Nombre',
        Default: 'Seleccione...',
        Sort: true
    });
}

function SelectorMotivo_MostrarMotivoSeleccionado() {
    $('#SelectorMotivo_ContenedorSeleccion').stop(true, true).fadeOut(500, function () {

        $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-titulo').text('Motivo: ' + toTitleCase(SelectorMotivo_Motivo.Nombre));
        $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-detalle').text('Servicio: ' + toTitleCase(SelectorMotivo_Servicio.Nombre));
        $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-descripcion').text(SelectorMotivo_Motivo.Observaciones);

        $('#SelectorMotivo_ContenedorMotivoSeleccionado').stop(true, true).fadeIn(500);
    });
}

function SelectorMotivo_OcultarMotivoSeleccionado() {
    $('#SelectorMotivo_Select_Servicio').prop('disabled', false);

    //Deselecciono el servicio
    if (idArea != undefined && idArea != -1) {
        $.each(SelectorMotivo_Motivos, function (i, motivo) {
            if (motivo.IdArea == idArea) {
                $('#SelectorMotivo_Select_Servicio').val(motivo.Servicio.Id).trigger('change');
                $('#SelectorMotivo_Select_Servicio').prop('disabled', true);
            }
        });
    } else {
        $('#SelectorMotivo_Select_Servicio').val('-1').trigger('change');
    }

    //Borro lo que busco
    $('#SelectorMotivo_Input_Buscar').val('').trigger('change');
    Materialize.updateTextFields();

    $('#SelectorMotivo_ContenedorMotivoSeleccionado').stop(true, true).fadeOut(500, function () {
        //Muestro el contenedor para seleccionar
        $('#SelectorMotivo_ContenedorSeleccion').stop(true, true).fadeIn(500);

        //Enfoco en busqueda
        if (!SelectorMotivo_Manual) {
            $('#SelectorMotivo_Input_Buscar').trigger('focus');
        }
    });
}

function SelectorMotivo_IsServicioSeleccionado() {
    return SelectorMotivo_Servicio != undefined;
}

function SelectorMotivo_GetServicioSeleccionado() {
    return SelectorMotivo_Servicio;
}

function SelectorMotivo_IsMotivoSeleccionado() {
    if (SelectorMotivo_Multiple) {
        return SelectorMotivo_MotivosSeleccionados.length != 0;
    }
    return SelectorMotivo_Motivo != undefined;
}

function SelectorMotivo_GetMotivoSeleccionado() {
    if (SelectorMotivo_Multiple) {
        return SelectorMotivo_MotivosSeleccionados;
    }
    return SelectorMotivo_Motivo;
}

function SelectorMotivo_IsDatosIngresadosSinMotivoSeleccionado() {
    var a = !SelectorMotivo_IsMotivoSeleccionado();
    var b = SelectorMotivo_Manual ? false : $('#SelectorMotivo_Input_Buscar').val().trim() != "";
    return a && b;
}

function SelectorMotivo_SetOnModoCambiadoListener(callback) {
    SelectorMotivo_CallbackModo = callback;
}

let idArea;

function SelectorMotivo_SetArea(id) {
    if (SelectorMotivo_IsMotivoSeleccionado()) {
        $('#SelectorMotivo_BtnCancelarMotivo').trigger('click');
    }

    let motivos = [];
    idArea = parseInt(id);

    if (idArea == -1) {
        SelectorMotivo_OcultarMotivoSeleccionado();
        return;
    }

    $.each(SelectorMotivo_Motivos, function (i, motivo) {
        if (motivo.IdArea == idArea) {
            $('#SelectorMotivo_Select_Servicio').val(motivo.Servicio.Id).trigger('change', false);
            $('#SelectorMotivo_Select_Servicio').prop('disabled', true);
            return false;
        }
    });
}

let SelectorMotivo_Multiple = false;

function SelectorMotivo_SetMultiple(multiple) {
    SelectorMotivo_Multiple = multiple;
    if (multiple) {
        $("#contenedorSwitch").hide();
        $("#contenedorSwitch").prop("disabled", true);
    }
}

//Eventos creados para la edicion del requerimiento
function SelectorMotivo_SetMotivo(motivo, servicio, animar) {
    if (animar == undefined) {
        animar = false;
    }
    if (!animar) {
        $.fx.off = true;
    }

    SelectorMotivo_Servicio = servicio;
    SelectorMotivo_Motivo = motivo;
    SelectorMotivo_MostrarMotivoSeleccionado();

    if (SelectorMotivo_Callback != undefined) {
        SelectorMotivo_Callback(SelectorMotivo_Motivo);
    }

    if (!animar) {
        $.fx.off = false;
    }
}

function SelectorMotivo_ReiniciarUI() {
    SelectorMotivo_Manual = true;

    //Desselecciono el servicio
    $('#SelectorMotivo_Select_Servicio').val('-1').trigger('change');

    var def = SelectorMotivo_Multiple ? null : 'Seleccione...'
    //Reinicio el motivo
    $('#SelectorMotivo_Select_Motivo').val('-1').trigger('change');
    $('#SelectorMotivo_Select_Motivo').prop('disabled', true);
    $('#SelectorMotivo_Select_Motivo').CargarSelect({
        Data: [],
        Multiple: SelectorMotivo_Multiple,
        Default: def,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    $('#SelectorMotivo_Input_Buscar').val('').trigger('change');
    Materialize.updateTextFields();

    $('#SelectorMotivo_Card').hide();

    $('#SelectorMotivo_ContenedorSeleccion').show();
    $('#SelectorMotivo_ContenedorMotivoSeleccionado').hide();
    $('#SelectorMotivo_ContenedorManual').show();
    $('#SelectorMotivo_ContenedorBusqueda').hide();
}

//------------------------------
//Modo Urgentes
//------------------------------
function SelectorMotivo_SetModoUrgentes(value) {
    if (value) {
        SelectorMotivo_Modo = SelectorMotivo_ModoUrgentes;
        $("#SelectorMotivo_ContenedorMensajeUrgentes").show();
        return;
    }

    $("#SelectorMotivo_ContenedorMensajeUrgentes").hide();
    SelectorMotivo_Modo = SelectorMotivo_ModoNormal;
}

//------------------------------
//Tipo: privado, interno o general
//------------------------------
function SelectorMotivo_SetTipo(value) {
    SelectorMotivo_Tipo = parseInt(value);
    SelectorMotivo_CargarServicios();
}

//-------------------------------
// Callback
//-------------------------------
function SelectorMotivo_SetOnMotivoSeleccionadoListener(callback) {
    this.SelectorMotivo_Callback = callback;
}

function SelectorMotivo_SetOnServicioSeleccionadoListener(callback) {
    this.selectorMotivo_CallbackServicio = callback;
}

//-------------------------------
// Cargando
//-------------------------------

function SelectorMotivo_MostrarCargando(mostrar, mensaje) {
    //$('#SelectorDomicilio_Input_Calle').prop('disabled', mostrar);
    //$('#SelectorDomicilio_Input_Altura').prop('disabled', mostrar);
    //$('#SelectorDomicilio_Input_Barrio').prop('disabled', mostrar);
    //$('#SelectorDomicilio_BtnBuscar').prop('disabled', mostrar);

    if (SelectorMotivo_CallbackCargando != undefined) {
        SelectorMotivo_CallbackCargando(mostrar, mensaje);
    }
}

function SelectorMotivo_SetOnCargandoListener(callback) {
    this.SelectorMotivo_CallbackCargando = callback;
}

//-----------------------------
// Alertas
//-----------------------------

function SelectorMotivo_SetOnMensajeListener(callback) {
    this.SelectorMotivo_CallbackMensaje = callback;
}

function SelectorMotivo_MostrarMensajeError(mensaje) {
    if (SelectorMotivo_CallbackMensaje == undefined) return;
    SelectorMotivo_CallbackMensaje('Error', mensaje);
}

function SelectorMotivo_MostrarMensajeAlerta(mensaje) {
    if (SelectorMotivo_CallbackMensaje == undefined) return;
    SelectorMotivo_CallbackMensaje('Alerta', mensaje);
}

function SelectorMotivo_MostrarMensajeInfo(mensaje) {
    if (SelectorMotivo_CallbackMensaje == undefined) return;
    SelectorMotivo_CallbackMensaje('Info', mensaje);
}
