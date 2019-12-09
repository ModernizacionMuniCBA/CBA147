var SelectorMotivo_Manual = true;

var SelectorMotivo_Motivo;
var SelectorMotivo_MotivosSeleccionados = [];
var SelectorMotivo_Servicio;
var SelectorMotivo_Categoria;
var SelectorMotivo_Area;

var SelectorMotivo_IdArea;
var SelectorMotivo_IdCategoria;

var SelectorMotivo_Servicios = [];
var SelectorMotivo_Areas = [];
var SelectorMotivo_Categorias = [];
var SelectorMotivo_Motivos = [];

var SelectorMotivo_Callback;
var SelectorMotivo_CallbackModo;
var SelectorMotivo_CallbackMensaje;
var SelectorMotivo_CallbackCargando;
var SelectorMotivo_CallbackTipoMotivo;

var SelectorMotivo_ModoUrgentes = 'urgentes';
var SelectorMotivo_ModoNormal = 'normal';
var SelectorMotivo_Modo = SelectorMotivo_ModoNormal;

var SelectorMotivo_TipoGeneral = 1;
var SelectorMotivo_TipoInterno = 2;
var SelectorMotivo_TipoPrivado = 3;
var SelectorMotivo_Tipo = null;

var SelectorMotivo_Multiple = false;
var SelectorMotivo_MostrarTiposMotivo = false;
var SelectorMotivo_Busqueda = false;

//Inicialización
function SelectorMotivo_Init(valores) {
    if (!valores) {
        valores = {};
    }

    //------------------------------
    //Modo busqueda o no
    //------------------------------
    SelectorMotivo_Busqueda = valores.ModoBusqueda;
    if (!SelectorMotivo_Busqueda) {
        SelectorMotivo_Busqueda = false;
    }

    //------------------------------
    //Area o categoria predefinida
    //------------------------------
    SelectorMotivo_IdArea = valores.IdArea;
    if (SelectorMotivo_IdArea == "-1") {
        SelectorMotivo_IdArea = undefined;
    }

    SelectorMotivo_IdCategoria = valores.IdCategoria;
    if (SelectorMotivo_IdCategoria == "-1") {
        SelectorMotivo_IdCategoria = undefined;
    }

    //------------------------------
    //Tipo de motivo: privado, interno o general
    //------------------------------
    SelectorMotivo_Tipo = parseInt(valores.TipoMotivo);
    if (!SelectorMotivo_Tipo) {
        SelectorMotivo_Tipo = SelectorMotivo_TipoGeneral;
    }
    SelectorMotivo_SetTipoMotivo(SelectorMotivo_Tipo);

    SelectorMotivo_MostrarTiposMotivo = valores.MostrarTiposMotivo;
    if (!SelectorMotivo_MostrarTiposMotivo) {
        SelectorMotivo_MostrarTiposMotivo = false;
    }

    SelectorMotivo_Multiple = valores.SeleccionMultiple;
    if (!SelectorMotivo_Multiple) {
        SelectorMotivo_Multiple = false;
    }

    //Veo si el usuario tiene permiso para crear req con motivos privados
    if (crearOrdenEspecial) {
        $("#contenedor_RadioTipoMotivo_Privado").show();
    }

    if (SelectorMotivo_MostrarTiposMotivo) {
        SelectorMotivo_InitRadiosTiposMotivos();
    }

    //Urgentes
    if (valores.ModoUrgente == true) {
        SelectorMotivo_SetModoUrgentes(true);
    }

    //------------------------------
    //Callbacks
    //------------------------------
    SelectorMotivo_Callback = valores.Callback;
    if (!SelectorMotivo_Callback) {
        SelectorMotivo_Callback = function () { }
    }

    SelectorMotivo_CallbackCargando = valores.CallbackCargando;
    if (!SelectorMotivo_CallbackCargando) {
        SelectorMotivo_CallbackCargando = function () { }
    }

    SelectorMotivo_CallbackModo = valores.CallbackModo;
    if (!SelectorMotivo_CallbackModo) {
        SelectorMotivo_CallbackModo = function () { }
    }

    SelectorMotivo_CallbackMensaje = valores.CallbackMensaje;
    if (!SelectorMotivo_CallbackMensaje) {
        SelectorMotivo_CallbackMensaje = function () { }
    }

    SelectorMotivo_CallbackTipoMotivo = valores.CallbackTipoMotivo;
    if (!SelectorMotivo_CallbackTipoMotivo) {
        SelectorMotivo_CallbackTipoMotivo = function () { }
    }

    SelectorMotivo_CallbackCategoriaSeteada = valores.CallbackCategoriaSeteada;
     if (!SelectorMotivo_CallbackCategoriaSeteada) {
         SelectorMotivo_CallbackCategoriaSeteada = function () { }
    }

    //SelectorMotivo_InitSwitch();
    
    SelectorMotivo_InitSelects();

    //$('#SelectorMotivo_Switch').find('input').prop('checked', true);
    //$('#SelectorMotivo_Switch').trigger('change');

    //SelectorMotivo_InitCardBusqueda();
    $('#SelectorMotivo_BtnCancelarMotivo').click(function () {
        SelectorMotivo_OcultarMotivoSeleccionado();
    })
}

function SelectorMotivo_ConsultaInicial() {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/MotivoService.asmx/GetDataInicialControlMotivos'),
            Data: { tipo: SelectorMotivo_Tipo, modoBusqueda: SelectorMotivo_Busqueda },
            OnSuccess: function (result) {
                result = parse(result);

                if (!result.Ok) {
                    SelectorMotivo_MostrarMensajeError(result.Error);
                    return;
                }

                SelectorMotivo_Servicios = result.Return.Servicios;
                SelectorMotivo_Areas = result.Return.Areas;
                callback();
            },
            OnError: function (result) {
                SelectorMotivo_MostrarMensajeError(result.Error);
                callbackError();
            }
        });
    });
}

//function SelectorMotivo_InitSwitch() {
//    //Switch
//    $('#SelectorMotivo_Switch').find('.opcion1').addClass('active');
//    $('#SelectorMotivo_Switch').find('.opcion2').removeClass('active');

//    //Click opcion1
//    $('#SelectorMotivo_Switch').find('.opcion1').click(function () {
//        $('#SelectorMotivo_Switch').find('input').prop('checked', false);
//        $('#SelectorMotivo_Switch').trigger('change');
//    });

//    //Click opcion2
//    $('#SelectorMotivo_Switch').find('.opcion2').click(function () {
//        $('#SelectorMotivo_Switch').find('input').prop('checked', true);
//        $('#SelectorMotivo_Switch').trigger('change');
//    });

//    //Evento al cambiar
//    $('#SelectorMotivo_Switch').change(function () {
//        var nuevoModo = $('#SelectorMotivo_Switch').find('input').is(':checked');
//        SelectorMotivo_Manual = nuevoModo;

//        if (SelectorMotivo_CallbackModo) {
//            SelectorMotivo_CallbackModo(nuevoModo);
//        }

//        $('#SelectorMotivo_Switch').find('input').prop('disabled', true);

//        if (!SelectorMotivo_Manual) {

//            //Deselecciono el servicio
//            if (SelectorMotivo_Servicio != undefined) {
//                $('#SelectorMotivo_Select_Servicio').val('-1').trigger('change');
//            }

//            //Deselecciono el motivo
//            if (SelectorMotivo_Motivo != undefined) {
//                $('#SelectorMotivo_Select_Motivo').val('-1').trigger('change');
//            }

//            if (idArea != undefined) {
//                $.each(SelectorMotivo_Motivos, function (i, motivo) {
//                    if (motivo.IdArea == idArea) {
//                        $('#SelectorMotivo_Select_Servicio').val(motivo.Servicio.Id).trigger('change', false);
//                        $('#SelectorMotivo_Select_Servicio').prop('disabled', true);
//                        return;
//                    }
//                });
//            }

//            $('#SelectorMotivo_Switch').find('.opcion1').addClass('active');
//            $('#SelectorMotivo_Switch').find('.opcion2').removeClass('active');

//            $('#SelectorMotivo_ContenedorManual').stop(true, true).fadeOut(300, function () {
//                $('#SelectorMotivo_ContenedorBusqueda').stop(true, true).fadeIn(300, function () {
//                    $('#SelectorMotivo_Switch').find('input').prop('disabled', false);
//                });

//                //Foco en busqueda
//                $('#SelectorMotivo_Input_Buscar').trigger('focus');
//            });
//        } else {
//            //Borro el texto de la busqueda
//            $('#SelectorMotivo_Input_Buscar').val('');
//            Materialize.updateTextFields();

//            $('#SelectorMotivo_Switch').find('.opcion1').removeClass('active');
//            $('#SelectorMotivo_Switch').find('.opcion2').addClass('active');

//            $('#SelectorMotivo_ContenedorBusqueda').stop(true, true).fadeOut(300, function () {
//                $('#SelectorMotivo_ContenedorManual').stop(true, true).fadeIn(300, function () {
//                    $('#SelectorMotivo_Switch').find('input').prop('disabled', false);
//                });
//            });

//            //Foco en busqueda
//            $('#SelectorMotivo_Input_Buscar').trigger('blur');
//        }
//    });
//}

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

    //$('#SelectorMotivo_Input_Buscar').on('input', function () {
    //    //Posiciono correctamente
    //    var w = $('#SelectorMotivo_Input_Buscar').width();
    //    $('#SelectorMotivo_Card').css('width', w);

    //    var pos = $('#SelectorMotivo_Input_Buscar').offset();
    //    pos.top = pos.top + $('#SelectorMotivo_Input_Buscar').height();
    //    $('#SelectorMotivo_Card').css(pos);



    //    //Obtengo el valor tipeado
    //    var contenido = $('#SelectorMotivo_Input_Buscar').val().trim();

    //    //Si no hay nada, cierro
    //    if (contenido == undefined || contenido == "") {
    //        $('#SelectorMotivo_Card').stop(true, true).slideUp(300).fadeOut(300);
    //        return;
    //    }

    //    //Obtengo los motivos
    //    var resultado = [];
    //    $.each(SelectorMotivo_Motivos, function (index, val) {
    //        if (val.Nombre.toLowerCase().indexOf(contenido.toLowerCase()) >= 0) {
    //            if (SelectorMotivo_Modo == SelectorMotivo_ModoNormal || (SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes && val.Urgente)) {
    //                if (idArea != undefined) {
    //                    if (val.IdArea == idArea) {
    //                        resultado.push(val);
    //                    }
    //                } else {
    //                    resultado.push(val);
    //                }
    //            }
    //        }
    //    });

    //    //Si no tengo motivos cierro
    //    if (resultado.length == 0) {
    //        $('#SelectorMotivo_Card').stop(true, true).slideUp(300).fadeOut(300);
    //        return;
    //    }

    //    var h = 49 * resultado.length + 49;
    //    if (h > hmax) {
    //        h = hmax;
    //    }

    //    $('#SelectorMotivo_Card').find('.contenido').height(h);

    //    //Agrego la info nueva
    //    var dt = $('#SelectorMotivo_Card').find('table').DataTable();
    //    dt.clear();
    //    dt.rows.add(resultado);
    //    dt.draw();

    //    //Muestro la card
    //    $('#SelectorMotivo_Card').stop(true, true).slideDown(300);

    //    //Click en cada fila
    //    dt.$('tr').mousedown(function () {

    //        var data = dt.row($(this)).data();

    //        if (!SelectorMotivo_Multiple) {
    //            //Guardo la seleccion
    //            SelectorMotivo_Servicio = $.grep(SelectorMotivo_Servicios, function (e) { return e.Id == data.Servicio.Id; })[0];
    //            SelectorMotivo_Motivo = data;

    //            //Informo la seleccion
    //            setTimeout(function () {
    //                if (SelectorMotivo_Callback != undefined) {
    //                    SelectorMotivo_Callback(SelectorMotivo_Motivo);
    //                }
    //            }, 500);

    //            //Muestro la seleccion
    //            SelectorMotivo_MostrarMotivoSeleccionado();

    //            //Oculto el card
    //            $('#SelectorMotivo_Card').stop(true, true).slideUp(300);
    //        }
    //    });
    //});
}

function SelectorMotivo_InitSelects() {
    //Evento Servicio
    $('#SelectorMotivo_Select_Servicio').on('change', function (e) {
        var value = $(this).val();

        SelectorMotivo_Servicio = undefined;

        $('#SelectorMotivo_Select_Motivo').val('-1').trigger('change');
        $('#SelectorMotivo_Select_Motivo').prop('disabled', true);
        $('#SelectorMotivo_Select_Categoria').val('-1').trigger('change');
        $('#SelectorMotivo_Select_Categoria').prop('disabled', true);

        //Borro el error
        $('#SelectorMotivo_Select_Servicio').siblings('.control-observacion').text('');

        if (value == -1 && (SelectorMotivo_Tipo != SelectorMotivo_TipoGeneral || (SelectorMotivo_Tipo == SelectorMotivo_TipoGeneral && SelectorMotivo_Busqueda))) {
            SelectorMotivo_CargarSelectAreas(SelectorMotivo_Areas);
        }

        if (value == -1 && !SelectorMotivo_Multiple) {
            return;
        } else if (value == -1 && SelectorMotivo_Multiple) {

            //Borro el servicio seleccionado
            SelectorMotivo_GetMotivosTodos().then(function (data) {
                SelectorMotivo_SetMotivosGeneral(data);
            }).catch();
            return;
        }

        SelectorMotivo_Servicio = _.filter(SelectorMotivo_Servicios, function (s) { return s.Id == value })[0];
        //si es general...
        if (SelectorMotivo_Tipo == SelectorMotivo_TipoGeneral && !SelectorMotivo_Busqueda) {

            //Obtengo los motivos
            SelectorMotivo_GetMotivosPorServicio(value).then(function (data) {
                SelectorMotivo_SetMotivosGeneral(data);
            });

            //Guardo el servicio seleccionado
            SelectorMotivo_Servicio = $.grep(SelectorMotivo_Servicios, function (e) { return e.Id == value; })[0];;
            return;
        }

        //si es interno
        $('#SelectorMotivo_Select_Area').prop('disabled', true);
        SelectorMotivo_GetAreasPorServicio(value);
    });

    //Evento Motivo
    $('#SelectorMotivo_Select_Motivo').on('change', function (e) {
        var value = $(this).val();
        if (!value) {
            return;
        }

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

    //Evento Areas
    $('#SelectorMotivo_Select_Area').on('change', function (e) {
        var value = $(this).val();

        $('#SelectorMotivo_Select_Motivo').val('-1').trigger('change');
        $('#SelectorMotivo_Select_Motivo').prop('disabled', true);
        $('#SelectorMotivo_Select_Categoria').val('-1').trigger('change');
        $('#SelectorMotivo_Select_Categoria').prop('disabled', true);

        SelectorMotivo_Area = undefined;
        SelectorMotivo_IdArea = undefined;

        if (value == -1) {
            return;
        }

        SelectorMotivo_Area = _.filter(SelectorMotivo_Areas, function (a) {
            return a.Id == value;
        })[0];

        //si es interno
        SelectorMotivo_GetMotivosPorArea(value).then(function (data) {
            //Seteo el servicio 
            if (data.length > 0) {
                SelectorMotivo_Servicio = _.filter(SelectorMotivo_Servicios, function (s) { return s.Id == data[0].Servicio.Id; })[0];
            }
            SelectorMotivo_SetMotivosInternos(data);
        });
    });

    //Evento Categorias
    $('#SelectorMotivo_Select_Categoria').on('change', function (e) {
        var value = $(this).val();

        $('#SelectorMotivo_Select_Motivo').val('-1').trigger('change');
        $('#SelectorMotivo_Select_Motivo').prop('disabled', true);

        SelectorMotivo_Categoria = undefined;
        //SelectorMotivo_IdCategoria = undefined;
        SelectorMotivo_MotivosSeleccionados = [];
        SelectorMotivo_Motivo = undefined;

        var motivosFiltrados = _.filter(SelectorMotivo_Motivos, function (m) {
            return m.IdCategoria == value;
        });

        if (value == -1 && !SelectorMotivo_Busqueda) {
            return;
        }
        else if (value == -1 && SelectorMotivo_Busqueda) {
            motivosFiltrados = SelectorMotivo_Motivos;

        }

        SelectorMotivo_Categoria = _.filter(SelectorMotivo_Categorias, function (c) { return c.Id == value })[0];
        SelectorMotivo_CargarSelectMotivos(motivosFiltrados);

        //if (value != "-1") {
        //    SelectorMotivo_IdCategoria = value;
        //}
    });
}

function SelectorMotivo_CargarSelects() {
    //Cargo los servicios
    if (SelectorMotivo_Servicios && SelectorMotivo_Servicios.length > 0) {
        SelectorMotivo_CargarSelectServicios(SelectorMotivo_Servicios);
        $('#SelectorMotivo_Select_Servicio').prop('disabled', false);
    } else {
        $('#SelectorMotivo_Select_Servicio').hide();
    }

    SelectorMotivo_CargarSelectMotivos([]);

    if (SelectorMotivo_Tipo == SelectorMotivo_TipoGeneral && !SelectorMotivo_Busqueda) {
        $("#SelectorMotivo_Contenedor_Select_Area").hide();
        $("#SelectorMotivo_Contenedor_Select_Categoria").hide();
        return;
    }

    if (!SelectorMotivo_IdArea) {
        $("#SelectorMotivo_Contenedor_Select_Area").show();
        SelectorMotivo_CargarSelectAreas(SelectorMotivo_Areas);
        $('#SelectorMotivo_Select_Area').prop('disabled', false);
    }

    $("#SelectorMotivo_Contenedor_Select_Categoria").show();
    $('#SelectorMotivo_Select_Categoria').prop('disabled', true);
    SelectorMotivo_CargarSelectCategorias([]);
}

function SelectorMotivo_InitRadiosTiposMotivos() {
    $("#SelectorMotivo_ContenedorTiposMotivo").show();
    $("input[name='tiposMotivos']").change(function (e) {
        SelectorMotivo_Motivo=undefined;
        SelectorMotivo_MotivosSeleccionados = [];
        SelectorMotivo_Servicio = undefined;
        SelectorMotivo_Categoria = undefined;
        SelectorMotivo_Area = undefined;

        SelectorMotivo_IdArea = undefined;
        SelectorMotivo_IdCategoria = undefined;

        SelectorMotivo_Servicios = [];
        SelectorMotivo_Areas = [];
        SelectorMotivo_Categorias = [];
        SelectorMotivo_Motivos = [];

        if ($("#radio_TipoMotivo_Privado").is(':checked')) {
            SelectorMotivo_SetTipoMotivo(SelectorMotivo_TipoPrivado);
        } else if ($("#radio_TipoMotivo_Interno").is(':checked')) {
            SelectorMotivo_SetTipoMotivo(SelectorMotivo_TipoInterno);
        } else {
            SelectorMotivo_SetTipoMotivo(SelectorMotivo_TipoGeneral);
        }
    });
}

function SelectorMotivo_SetTipoMotivo(tipo) {
    SelectorMotivo_Tipo = parseInt(tipo);

    SelectorMotivo_ReiniciarSelects();

    $("#radio_TipoMotivo_General").prop("checked", false);
    $("#radio_TipoMotivo_Interno").prop("checked", false);
    $("#radio_TipoMotivo_Privado").prop("checked", false);
    switch (SelectorMotivo_Tipo) {
        case SelectorMotivo_TipoGeneral:
            $("#radio_TipoMotivo_General").prop("checked", true);
            break;
        case SelectorMotivo_TipoInterno:
            $("#radio_TipoMotivo_Interno").prop("checked", true);
            break;
        case SelectorMotivo_TipoPrivado:
            $("#radio_TipoMotivo_Privado").prop("checked", true);
            break;
    }

    //si ya tengo un area especifica, no muestro los selects de servicio y area
    if (SelectorMotivo_IdArea) {
        //$('#SelectorMotivo_Select_Servicio').hide();
        //$('#SelectorMotivo_Select_Area').hide();
        SelectorMotivo_SetArea(SelectorMotivo_IdArea);
        return;
    }

    SelectorMotivo_ConsultaInicial().then(function () {
        SelectorMotivo_CargarSelects();
    }).catch(function () {
        return;
    })

    if (SelectorMotivo_CallbackTipoMotivo)
        SelectorMotivo_CallbackTipoMotivo(SelectorMotivo_Tipo);
}

//Carga de selects
function SelectorMotivo_ReiniciarSelects() {
    SelectorMotivo_Servicios = [];
    SelectorMotivo_Areas = [];
    SelectorMotivo_Categorias = [];
    SelectorMotivo_Motivos = [];

    SelectorMotivo_CargarSelectServicios([]);
    SelectorMotivo_CargarSelectMotivos([]);
    SelectorMotivo_CargarSelectAreas([]);
    SelectorMotivo_CargarSelectCategorias([]);
}

function SelectorMotivo_CargarSelectServicios(data) {
    var def = "Seleccione...";
    if (data.length == 1) {
        def = null;
    }

    if (data.length == 0) {
        $('#SelectorMotivo_Select_Servicio').prop("disabled", true);
    }

    $('#SelectorMotivo_Select_Servicio').CargarSelect({
        Data: data,
        Default: def ? "Seleccione..." : null,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    //if (data.length == 1) {
    //    $('#SelectorMotivo_Select_Servicio').trigger('change');
    //}
}

function SelectorMotivo_CargarSelectAreas(data) {
    var def = "Seleccione...";
    if (data.length == 1) {
        def = null;
    }

    if (data.length == 0) {
        $('#SelectorMotivo_Select_Area').prop("disabled", true);
    }

    $('#SelectorMotivo_Select_Area').CargarSelect({
        Data: data,
        Default: def ? "Seleccione..." : null,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    if (data.length == 1) {
        $('#SelectorMotivo_Select_Area').trigger('change');
    }
}

function SelectorMotivo_CargarSelectCategorias(data) {
    var def = "Seleccione...";
    if (data.length == 1) {
        def = null;
    } else if (SelectorMotivo_Busqueda) {
        def = "Todas";
    }

    if (data.length == 0) {
        $('#SelectorMotivo_Select_Categoria').prop("disabled", true);
    }

    $('#SelectorMotivo_Select_Categoria').CargarSelect({
        Data: data,
        Default: def,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    if (data.length == 1 || SelectorMotivo_Busqueda) {
        $('#SelectorMotivo_Select_Categoria').trigger('change');
    }
}

function SelectorMotivo_CargarSelectMotivos(data) {
    var def = "Seleccione...";
    if (SelectorMotivo_Multiple) {
        def = null;
    }

    $('#SelectorMotivo_Select_Motivo').prop("disabled", data.length == 0);

    $('#SelectorMotivo_Select_Motivo').CargarSelect({
        Data: data,
        Default: def,
        Multiple: SelectorMotivo_Multiple,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });
}

//Tipo de requerimiento
function SelectorMotivo_GetTipos() {
    if (!SelectorMotivo_Tipo) return null;

    var tipos = [];
    if (SelectorMotivo_Tipo == SelectorMotivo_TipoPrivado && !crearOrdenEspecial) {
        SelectorMotivo_MostrarMensajeError("Usted no tiene permiso para estos tipos de motivos");
        return;
    }
    tipos.push(SelectorMotivo_Tipo);
    return tipos;
}

function SelectorMotivo_SetMotivosInternos(data) {
    SelectorMotivo_Motivos = data;
    SelectorMotivo_CargarSelectMotivos([]);

    if (data == [] || data.length == 0) {
        SelectorMotivo_CargarSelectCategorias([]);
        return;
    }

    //si es interno, cargo las categorias
    let categorias = _.uniq(_.map(data, function (m) {
        if (m.IdCategoria == 0) {
            m.IdCategoria = -1000000;
            m.NombreCategoria = "Sin Categoría"
        };
        return { Id: m.IdCategoria, Nombre: m.NombreCategoria }
    }), _.property('Id'))

    SelectorMotivo_Categorias = categorias;
    SelectorMotivo_CargarSelectCategorias(SelectorMotivo_Categorias);
    $('#SelectorMotivo_Select_Categoria').prop("disabled", false);

    if (SelectorMotivo_IdCategoria) {
        if (!_.some(SelectorMotivo_Categorias, function (c) {
            return c.Id == SelectorMotivo_IdCategoria;
        })) {
            SelectorMotivo_IdCategoria = undefined;
            return;
        }

        $('#SelectorMotivo_Select_Categoria').val(SelectorMotivo_IdCategoria).trigger("change");
        SelectorMotivo_Categoria = _.filter(SelectorMotivo_Categorias, function (cat) {
            return cat.Id == SelectorMotivo_IdCategoria;
        })[0];
        SelectorMotivo_CallbackCategoriaSeteada();
        SelectorMotivo_IdCategoria = undefined;
    }

    return;
}

function SelectorMotivo_SetMotivosGeneral(data) {
    SelectorMotivo_Motivos = data;

    //si tengo un area seleccionada, filtro primero
    if (SelectorMotivo_IdArea) {
        SelectorMotivo_Motivos = _.filter(SelectorMotivo_Motivos, function (m) {
            return m.IdArea == SelectorMotivo_IdArea;
        })
    }

    //Cargo el select
    SelectorMotivo_CargarSelectMotivos(SelectorMotivo_Motivos);
}

//Getters
function SelectorMotivo_GetMotivosTodos(callback, callbackError) {
    return new Promise(function (callback, callbackError) {
        if (!SelectorMotivo_GetTipos()) return;

        SelectorMotivo_GetMotivos({
            Urgente: SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes ? true : null,
            Tipos: SelectorMotivo_GetTipos()
        }).then(function (data) {
            callback(data)
        }).catch(function () {
            callbackError(data)
        });
    })
}

function SelectorMotivo_GetMotivosPorServicio(idServicio, callback, callbackError) {
    return new Promise(function (callback, callbackError) {
        if (!SelectorMotivo_GetTipos()) return;

        SelectorMotivo_GetMotivos({
            IdServicio: idServicio,
            Urgente: SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes ? true : null,
            Tipos: SelectorMotivo_GetTipos()
        }).then(function (data) {
            callback(data)
        }).catch(function () {
            callbackError(data)
        });
    })
}

function SelectorMotivo_GetMotivosPorArea(idArea, callback, callbackError) {
    return new Promise(function (callback, callbackError) {
        if (!SelectorMotivo_GetTipos()) return;

        SelectorMotivo_GetMotivos({
            IdArea: idArea,
            Urgente: SelectorMotivo_Modo == SelectorMotivo_ModoUrgentes ? true : null,
            Tipos: SelectorMotivo_GetTipos()
        }).then(function (data) {
            callback(data)
        }).catch(function () {
            callbackError(data)
        });
    });
}

function SelectorMotivo_GetMotivos(data, callback, callbackError) {

    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/MotivoService.asmx/GetByFilters'),
            Data: { consulta: data },
            OnSuccess: function (result) {
                result = parse(result);

                if (result.Return.length == 0) {
                    SelectorMotivo_MostrarMensajeAlerta('No hay motivos del tipo seleccionado para éste servicio o área.');
                    callback([]);
                    return;
                }

               callback(result.Return);
            },
            OnError: function (result) {
                SelectorMotivo_MostrarMensajeError(result.Error);
                callbackError();
            }
        });
    });
}

function SelectorMotivo_GetAreasPorServicio(idServicio) {
    crearAjax({
        Url: ResolveUrl('~/Servicios/ServicioService.asmx/GetAreasById'),
        Data: { consulta: { IdServicio: idServicio, Tipos: SelectorMotivo_GetTipos() } },
        OnSuccess: function (result) {
            result = parse(result);

            if (result.Return.length == 0) {
                SelectorMotivo_MostrarMensajeAlerta('No hay áreas para éste servicio.');
                return;
            }

            //Cargo el select con las areas
            SelectorMotivo_CargarSelectAreas(_.filter(SelectorMotivo_Areas, function (elem) {
                return _.contains(result.Return, elem.Id);
            }));

            //$('#SelectorMotivo_Select_Area').trigger("change");
            $('#SelectorMotivo_Select_Area').prop('disabled', false);
        },
        OnError: function (result) {
            SelectorMotivo_MostrarMensajeError(result.Error);
        }
    });
}

//Motivo Seleccionado
function SelectorMotivo_MostrarMotivoSeleccionado() {
    $('#SelectorMotivo_ContenedorMotivoSeleccionado').find("label").text("");

    $('#SelectorMotivo_ContenedorSeleccion').stop(true, true).fadeOut(500, function () {

        $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-nombre').html('<u>Motivo:</u> <b>' + toTitleCase(SelectorMotivo_Motivo.Nombre)) + '</b>';
        $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-descripcion').text(SelectorMotivo_Motivo.Observaciones);

        if (SelectorMotivo_Tipo != SelectorMotivo_TipoGeneral) {
            $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-area').html('<u>Área:</u> <b>' + toTitleCase(SelectorMotivo_Area.Nombre)) + '</b>';
            $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-categoria').html('<u>Categoría:</u> <b>' + toTitleCase(SelectorMotivo_Categoria.Nombre)) + '</b>';
            $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-descripcion').html(SelectorMotivo_Motivo.Observaciones);
        } else {
            $('#SelectorMotivo_ContenedorMotivoSeleccionado').find('.motivo-servicio').html('<u>Servicio:</u> <b>' + toTitleCase(SelectorMotivo_Servicio.Nombre)) + '</b>';
        }

        $('#SelectorMotivo_ContenedorMotivoSeleccionado').stop(true, true).fadeIn(500);
    });
}

function SelectorMotivo_OcultarMotivoSeleccionado() {
    $('#SelectorMotivo_Select_Servicio').prop('disabled', false);

    //Deselecciono el servicio
    if (SelectorMotivo_IdArea != undefined && SelectorMotivo_IdArea != -1) {
        $.each(SelectorMotivo_Motivos, function (i, motivo) {
            if (motivo.IdArea == SelectorMotivo_IdArea) {
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

//Servicio Seleccionado
function SelectorMotivo_IsServicioSeleccionado() {
    return SelectorMotivo_Servicio != undefined;
}

function SelectorMotivo_GetServicioSeleccionado() {
    return SelectorMotivo_Servicio;
}

//Categoría Seleccionada
//function SelectorMotivo_IsCategoriaSeleccionada() {
//    return (SelectorMotivo_Categoria && SelectorMotivo_Categoria.Id > 0) || SelectorMotivo_IdCategoria;
//}

function SelectorMotivo_IsCategoriaSeleccionada() {
    return (SelectorMotivo_Categoria && SelectorMotivo_Categoria.Id > 0) ;
}

function SelectorMotivo_GetCategoriaSeleccionada() {
    if (SelectorMotivo_IsCategoriaSeleccionada()) {
        return SelectorMotivo_Categoria;
    }
}

function SelectorMotivo_GetIdCategoriaSeleccionada() {
    return SelectorMotivo_IdCategoria;
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
    $('#SelectorMotivo_Select_Area').val('-1').trigger('change');
    $('#SelectorMotivo_Select_Categoria').val('-1').trigger('change');

    //Reinicio el motivo
    $('#SelectorMotivo_Select_Motivo').val('-1').trigger('change');
    $('#SelectorMotivo_Select_Motivo').prop('disabled', true);
    SelectorMotivo_CargarSelectMotivos([]);
    $('#SelectorMotivo_Input_Buscar').val('').trigger('change');
    Materialize.updateTextFields();

    $('#SelectorMotivo_Card').hide();

    $('#SelectorMotivo_ContenedorSeleccion').show();
    $('#SelectorMotivo_ContenedorMotivoSeleccionado').hide();
    $('#SelectorMotivo_ContenedorManual').show();
    $('#SelectorMotivo_ContenedorBusqueda').hide();
}


//------------------------------
//Modos
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

function SelectorMotivo_SetArea(id) {
    //si ya tengo idea de area seteado, y los motivos que tengo  son de ella
    if (id == "-1" || (SelectorMotivo_IdArea == id && _.some(SelectorMotivo_Motivos, function (m) {
        return m.IdArea == id;
    }))) {
        SelectorMotivo_IdArea = undefined;
        return;
    }

    if (SelectorMotivo_IsMotivoSeleccionado()) {
        $('#SelectorMotivo_BtnCancelarMotivo').trigger('click');
    }

    SelectorMotivo_IdArea = id;

    if (SelectorMotivo_IdArea == -1) {
        SelectorMotivo_OcultarMotivoSeleccionado();
        return;
    }

    SelectorMotivo_GetMotivosPorArea(SelectorMotivo_IdArea).then(function (data) {
        $('#SelectorMotivo_Contenedor_Select_Servicio').hide();
        $('#SelectorMotivo_Contenedor_Select_Area').hide();

        if (SelectorMotivo_Tipo == SelectorMotivo_TipoGeneral && !SelectorMotivo_Busqueda) {
            $('#SelectorMotivo_Contenedor_Select_Categoria').hide();
            SelectorMotivo_SetMotivosGeneral(data);
            return;
        }

        $('#SelectorMotivo_Contenedor_Select_Categoria').show();
        SelectorMotivo_SetMotivosInternos(data);
    });
}

function SelectorMotivo_GetTipoSeleccionado() {
    return SelectorMotivo_Tipo;
}

// Cargando
function SelectorMotivo_MostrarCargando(mostrar, mensaje) {
    //$('#SelectorDomicilio_Input_Calle').prop('disabled', mostrar);
    //$('#SelectorDomicilio_Input_Altura').prop('disabled', mostrar);
    //$('#SelectorDomicilio_Input_Barrio').prop('disabled', mostrar);
    //$('#SelectorDomicilio_BtnBuscar').prop('disabled', mostrar);

    if (SelectorMotivo_CallbackCargando != undefined) {
        SelectorMotivo_CallbackCargando(mostrar, mensaje);
    }
}

// Alertas
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
