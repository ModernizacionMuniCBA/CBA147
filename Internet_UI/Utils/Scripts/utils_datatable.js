$.fn.dataTableExt.oStdClasses.sPageButton = "btn-flat btn-redondo chico waves-effect";
$.fn.dataTableExt.oStdClasses.sPageButtonActive = "grey white-text waves-light";

$(function () {
    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "date-euro-pre": function (a) {
            return moment(a, 'DD/MM/YYYY hh:mm:ss');
        },

        "date-euro-asc": function (a, b) {
            return a.diff(b, 'seconds');
        },

        "date-euro-desc": function (a, b) {
            return b.diff(a, 'seconds');
        }
    });
});

var w_ColumnaFecha = '150px';

var botonesTablas = [];

jQuery.fn.DataTableGeneral = function (valores) {
    var idTabla = '' + $(this).prop('id');
    return procesarDatatable(idTabla, valores);
}


function calcularCantidadRowsDataTable(hDisponible) {
    var hEncabezado = 49;
    var hItem = 49;
    hDisponible = hDisponible - hEncabezado;
    return Math.floor(hDisponible / hItem);
}

function procesarDatatable(idTabla, valores) {
    if (valores == undefined) {
        valores = {};
    }

    var verInfo = true;
    if ('VerInfo' in valores) {
        verInfo = valores.VerInfo;
    }

    var paginar = true;
    if ('Paginar' in valores) {
        paginar = valores.Paginar;
    }

    //Cols
    if (valores.Columnas == undefined) {
        valores.Columnas = [];
    }

    var columnasPrimero = [];
    var columnasUltimo = [];
    $.each(valores.Columnas, function (index, col) {
        if ('Izquierda' in col) {
            columnasPrimero.push(col);
        } else {
            columnasUltimo.push(col);
        }
    });
    valores.Columnas = [];
    $.each(columnasPrimero, function (index, col) {
        valores.Columnas.push(col);
    });
    $.each(columnasUltimo, function (index, col) {
        valores.Columnas.push(col);
    });

    //Botones
    if (valores.Botones == undefined) {
        valores.Botones = [];
    }

    botonesTablas[idTabla] = {};

    if (valores.Botones.length != 0) {
        botonesTablas[idTabla].botones = valores.Botones;
        valores.Columnas.push({
            title: "",
            data: null,
            orderable: false,
            render: function (data, type, row) {
                var botones_html = "";

                var algunInvisible = false;

                $.each(valores.Botones, function (index, btn) {
                    if (btn.Oculto) {
                        var visible = true;

                        if ('Visible' in btn) {
                            if (typeof btn.Visible == 'function') {
                                visible = btn.Visible(row);
                            } else {
                                visible = btn.Visible;
                            }
                            if (!visible) {
                                return true;
                            }
                        }

                        if (visible) {
                            algunInvisible = true;
                        }
                        return true;
                    }

                    if ('Visible' in btn) {
                        var visible = true;
                        if (typeof btn.Visible == 'function') {
                            visible = btn.Visible(row);
                        } else {
                            visible = btn.Visible;
                        }
                        if (!visible) {
                            return true;
                        }
                    }

                    var titulo = btn.Texto;

                    var claseBoton;
                    if ('MostrarTooltip' in valores && valores.MostrarTooltip == false) {
                        claseBoton = 'btn btn-cuadrado chico no-select btnTabla waves-effect';
                    } else {
                        claseBoton = 'btn btn-cuadrado chico tooltipped no-select btnTabla waves-effect';
                    }

                    var html_boton = $('<a/>', {
                        class: claseBoton,
                        'data-position': 'bottom',
                        'data-delay': 50,
                        'data-tooltip': titulo
                        //'onclick': 'rowClick(' + idTabla + ', ' + index + ', ' + row.Id + ');'
                    });

                    $(html_boton).attr('id', idTabla + '_' + index);
                    $(html_boton).attr('idRow', row.Id);
                    $(html_boton).attr('index', index);

                    var html_icono = $('<i/>');
                    $(html_icono).addClass('material-icons');
                    $(html_icono).text(btn.Icono);
                    $(html_boton).append(html_icono);

                    botones_html += $(html_boton).prop('outerHTML');
                });

                if (algunInvisible) {
                    var html_boton = $('<a/>', {
                        class: 'btn btn-cuadrado chico tooltipped no-select btnMenu waves-effect',
                        'data-position': 'bottom',
                        'id': idTabla + '_overflow',
                        'data-delay': 50,
                        'data-tooltip': 'Mas...'
                        //'onclick': 'mostrarMenu(' + idTabla + ', ' + row.Id + ')'
                    });


                    var html_icono = $('<i/>');
                    $(html_icono).addClass('material-icons');
                    $(html_icono).text('more_vert');
                    $(html_boton).append(html_icono);

                    botones_html += $(html_boton).prop('outerHTML');
                }
                return "<div class='contenedor-botones'>" + botones_html + "</div>";
            }
        });
    }

    var colDefs = [];
    if ('Definiciones' in valores) {
        colDefs = valores.Definiciones;
    }
    colDefs.push({ "defaultContent": "", "targets": "_all" });

    var orden = [[0, 'desc']];
    if ('Orden' in valores) {
        orden = valores.Orden;
    }

    if (!('OnFilaCreada' in valores)) {
        valores.OnFilaCreada = function () { };
    }

    var dt = $('#' + idTabla).DataTable({
        lengthChange: false,
        searching: false,
        "info": verInfo,
        "paging": paginar,
        pageLength: 10,
        pagingType: "simple",
        "bDestroy": true,
        "bAutoWidth": false,
        "deferRender": true,
        "columns": valores.Columnas,
        "columnDefs": colDefs,
        "order": orden,
        "rowCallback": function (row, data, index) {
            valores.OnFilaCreada(row, data, index);
        },
        "oLanguage": {
            "sProcessing": "Procesando...",

            "sLengthMenu": "Tamaño de pagina _MENU_",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
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
        },
        "rowCallback": function (row, data, index) {
            $(row).find('.tooltipped').tooltip({ delay: 50 });

            $(row).off('contextmenu');
            $(row).bind("contextmenu", function (e) {
                // Avoid the real one
                e.preventDefault();

                
                $(this).MenuFlotanteTabla(e, idTabla, data, {
                    e: e,
                    PosicionX: "derecha",
                    PosicionY: "abajo"
                })
            });
        }
    });

    $('#' + idTabla + ' tbody').on('click', '.btnTabla', function (e) {
        var index = $(this).attr('index');
        var idRow = $(this).attr('idRow');

        var data = buscarRowPorId(idTabla, idRow);
        if (data == null) return;

        var btn = botonesTablas[idTabla].botones[index];
        if ('Validar' in btn) {
            var result = btn.Validar(data);
            if (result == undefined || result == false) {
                return;
            }
        }

        btn.OnClick(data, e);
    });

    $('#' + idTabla + ' tbody').on('click', '.btnMenu', function (e) {
        var data = dt.row($(this).parents('tr')).data();
        $(this).MenuFlotanteTabla(e, idTabla, data);
    });

    return dt;
}

function buscarRowPorId(idTabla, id) {
    if (id == undefined) return null;
    if (!typeof idTabla == 'string') {
        idTabla = $(idTabla).prop('id');
    }
    //Busco el indice de la persona a actualizar
    var i = -1;
    var info;
    var dt = $('#' + idTabla).DataTable();
    $.each(dt.data(), function (index, element) {
        if (element.Id == id) {
            i = index
            info = element;
        }
    });

    //Si no esta, corto
    if (i == -1) {
        return null;
    }

    return info;
}

function moverDatatableFooter(selectorTabla, selectorDestino) {
    $(selectorDestino).empty();
    $(selectorTabla + '_wrapper').find('.dataTables_info').detach().appendTo($(selectorDestino));
    $(selectorTabla + '_wrapper').find('.dataTables_paginate').detach().appendTo($(selectorDestino));
}

$.fn.GetData = function () {
    var data = [];
    $(this).DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
        data.push(this.data());
    });
    return data;
}

jQuery.fn.MenuFlotanteTabla = function (e, idTabla, data, valores) {

    if (valores == undefined) valores = {};
    if (!('e' in valores)) valores.e = undefined;

    //Soluciono el offset
    var iframe = $(this)[0].ownerDocument.defaultView.frameElement;
    var xOffset = 0;
    var yOffset = 0;
    while (iframe != undefined) {
        xOffset += $(iframe).offset().left;
        yOffset += $(iframe).offset().top;
        iframe = $(iframe)[0].ownerDocument.defaultView.frameElement;
    }

    console.log(xOffset);

    var pl = parseInt($(this).css('padding-left').replace("px", ""));
    var pr = parseInt($(this).css('padding-right').replace("px", ""));
    var pb = parseInt($(this).css('padding-bottom').replace("px", ""));
    var pt = parseInt($(this).css('padding-top').replace("px", ""));

    var x, y;
    if (valores.e == undefined) {
        x = $(this).offset().left + xOffset;
        y = $(this).offset().top + yOffset;
    } else {
        var e = valores.e;
        console.log(e);
        x = e.originalEvent.clientX + xOffset;
        y = e.originalEvent.clientY + yOffset;
    }

    var hItem = 48;
    var hMax = 192;
    var w = 200;

    var botones = botonesTablas[idTabla].botones;

    //Creo el menu
    var menu = $('<div>', {
        'class': 'card'
    });
    var id = new Date().getTime();
    $(menu).prop('id', id);
    $(menu).addClass('menu-flotante');

    $(menu).css('width', w + 'px');
    $(menu).css('max-height', hMax + 'px');

    //Calculo los items
    var hCalculado = 0;
    var ul = $('<ul>');
    $(ul).appendTo(menu);

    $.each(botones, function (index, btn) {
        if (valores.e ==undefined) {
            if (!btn.Oculto) return true;
        }

        var visible = true;
        if ('Visible' in btn) {
            if (typeof (btn.Visible) === 'boolean') {
                visible = btn.Visible;
            } else {
                visible = btn.Visible(data);
            }
        }

        if (!visible) {
            return true;
        }

        if (!('Texto' in btn)) {
            btn.Texto = 'Sin texto';
        }

        if (!('OnClick' in btn)) {
            btn.OnClick = function () { };
        }

        var hMenuItem = hItem;
        var separador = false;
        if ('Separador' in btn && btn.Separador == true) {
            hMenuItem = 32;
            separador = true;
        }

        hCalculado += hMenuItem;

        if (!('Id' in btn)) {
            btn.Id = new Date().getTime();
        }

        var li = $('<li>');
        $(li).appendTo(ul);
        $(li).addClass('menu-item waves-effect');
        $(li).attr('id', btn.Id);
        if (separador) {
            $(li).addClass('separador');
        }
        $(li).attr('index', index);

        if (separador) {
            //Texto
            var titulo = btn.Texto;
            if (typeof titulo != 'string') {
                titulo = btn.Texto(data);
            }
            var texto = $('<label>');
            $(texto).addClass('texto');
            $(texto).text(titulo);
            $(texto).appendTo(li);
            return true;
        }

        //Texto
        if ('Icono' in btn) {
            var icono = $('<i class="material-icons">' + btn.Icono + '</i>')
            $(icono).css('margin-right', '0.5rem');
            $(icono).appendTo(li);
        }
        var titulo = btn.Texto;
        if (typeof titulo != 'string') {
            titulo = btn.Texto(data);
        }
        var texto = $('<label>');
        $(texto).addClass('texto');
        $(texto).text(titulo);
        $(texto).appendTo(li);
    });

    //Limito el alto
    if (hCalculado < hMax) {
        $(menu).css('height', (hCalculado) + 'px');
    } else {
        hCalculado = hMax;
    }

    //Obtengo la posicion
    var posicion_x = "izquierda";
    var posicion_y = "abajo";
    if ('PosicionX' in valores) {
        posicion_x = valores.PosicionX;
    }
    if ('PosicionY' in valores) {
        posicion_y = valores.PosicionY;
    }

    //Calculo la posicion del MenuFlotante
    var objeto = calcularClaseMenuFoltante({
        Elemento: $(this),
        e: valores.e,
        PosicionX: posicion_x,
        PosicionY: posicion_y,
        X: x,
        Y: y,
        PaddingLeft: pl,
        PaddingRight: pr,
        PaddingTop: pt,
        PaddingBottom: pb,
        MenuFlotanteW: w,
        MenuFlotanteH: hCalculado
    });


    $(menu).css('left', (objeto.X) + 'px');
    $(menu).css('top', (objeto.Y) + 'px');
    $(menu).addClass(objeto.Clase);

    //Fondo
    var fondo = $('<div>');
    $(fondo).addClass('menu-flotante-fondo waves-effect');
    $(fondo).append($('<div>'));
    $(fondo).click(function () {
        $(menu).removeClass('abierto');
        $(fondo).removeClass('abierto');
        $(menu).fadeOut(300, function () {
            $(fondo).remove();
            $(menu).remove();
        });
    });

    //deshabilito click derecho en el fonmdo
    $(fondo).bind("contextmenu", function (event) {
        event.preventDefault();
    });

    top.$('body').append(fondo);
    top.$('body').append(menu);
    setTimeout(function () {
        $(menu).addClass('abierto');
        $(fondo).addClass('abierto');
    }, 100);
    top.$('#' + id).find('.menu-item').click(function () {
        var index = $(this).attr('index');

        var menuItem = botones[index];
        if ('Separador' in menuItem && menuItem.Separador) {
            //No hago nada
        } else {
            if ('Validar' in menuItem) {
                var result = menuItem.Validar(data);
                if (result == undefined || result == false) {
                    return;
                }
            }

            menuItem.OnClick(data);
            top.$(fondo).trigger('click');
        }
    });
}

function calcularClaseMenuFoltante(valores) {
    var element = valores.Elemento;
    var e = valores.e;
    var posicion_x = valores.PosicionX;
    var posicion_y = valores.PosicionY;
    var x = valores.X;
    var y = valores.Y;
    var pl = valores.PaddingLeft;
    var pr = valores.PaddingRight;
    var pt = valores.PaddingTop;
    var pb = valores.PaddingBottom;
    var w = valores.MenuFlotanteW;
    var h = valores.MenuFlotanteH;

    var clase;
    if (posicion_x == "izquierda") {
        if (posicion_y == "abajo") {
            clase = "abajo-izquierda";
        } else {
            clase = "arriba-izquierda";
        }
    } else {
        if (posicion_y == "abajo") {
            clase = "abajo-derecha";
        } else {
            clase = "arriba-derecha";
        }
    }

    var wElement = $(element).width();
    var hElement = $(element).height();

    switch (clase) {
        case 'abajo-izquierda': {
            //Como es para la izquierda, le resto a la posicion el ancho de la ventana
            x -= w;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                x += wElement + pl + pr;
            }
        } break;

        case 'abajo-derecha': {
            //No va nada aca porque es la posicion por defecto
        } break;

        case 'arriba-izquierda': {
            //Como es para la izquierda, le resto a la posicion el ancho de la ventana
            x -= w;

            //Como es para arriba, le resto a la posicion el alto de la ventana
            y -= h;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                x += wElement + pl + pr;
                y += hElement + pt + pb;
            }
        } break;

        case 'arriba-derecha': {
            //Como es para arriba, le resto a la posicion el alto de la ventana
            y -= h;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                y += hElement + pt + pb;
            }
        } break;
    }

    if (x < 0) {
        valores.PosicionX = "derecha";
        return calcularClaseMenuFoltante(valores);
    } else {
        var maxX = top.$('body').width();
        if (x + w > maxX) {
            valores.PosicionX = "izquierda";
            return calcularClaseMenuFoltante(valores);
        }
    }

    if (y < 0) {
        valores.PosicionY = "abajo";
        return calcularClaseMenuFoltante(valores);
    } else {
        var maxY = top.$('body').height();

        if (y + h > maxY) {
            valores.PosicionY = "arriba";
            return calcularClaseMenuFoltante(valores);
        }
    }

    var objeto = {};
    objeto.Clase = clase;
    objeto.X = x;
    objeto.Y = y;
    return objeto;
}

/* Datatable Rol Cerrojo */

jQuery.fn.DataTableRolCerrojo = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: "Nombre",
            data: "Nombre",
            render: function (data, type, row) {
                return '<div><span>' + data + '</span></div>';
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* DataTable Aplicacion */

jQuery.fn.DataTableAplicacionSimple = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: "Nombre",
            data: "Nombre",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableAplicacion = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Editar
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Callback Activar
    if (!('CallbackActivar' in valores)) {
        valores.CallbackActivar = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackBloquear' in valores)) {
        valores.CallbackBloquear = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDesbloquear' in valores)) {
        valores.CallbackDesbloquear = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
       {
           title: 'Identificador',
           data: 'Identificador',
           width: '100px',
           render: function (data, type, row) {
               return "<div><span>" + data.toUpperCase() + "</span></div>";
           }
       },
             {
                 title: 'Nombre',
                 data: 'Nombre',
                 width: '300px',
                 render: function (data, type, row) {
                     return "<div><span>" + toTitleCase(data) + "</span></div>";
                 }
             },
             {
                 title: 'Fecha Alta',
                 data: 'FechaAlta',
                 width: '170px',
                 render: function (data, type, row) {
                     if (data == '0001-01-01T00:00:00') {
                         return "";
                     }
                     return "<div><span>" + toDateTimeString(data) + "</span></div>";
                 }
             },
             {
                 title: 'Estado',
                 data: 'DadoDeBaja',
                 render: function (data, type, row) {
                     if (data) {
                         return '<div><label class="indicador-estado colorDeBaja"></label><span>De baja</span></div>';
                     } else {
                         return '<div><label class="indicador-estado colorActivo"></label><span>Activo</span></div>';
                     }
                 }
             }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAplicacionDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            })
        }
    });


    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Texto: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            var validar = true;
            if ('BotonEditarValidar' in valores) {
                validar = valores.BotonEditarValidar(data);
            }
            if (!validar) return false;

            if (data.DadoDeBaja) {
                valores.CallbackMensajes('Alerta', 'La aplicación se encuentra dada de baja. Para editarla primero debe activarla');
                return false;
            }

            return true;
        },
        OnClick: function (data) {
            crearDialogoAplicacionEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackEditar(app);
                }
            })
        }
    });

    //Boton Dar De Baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    botones.push({
        Texto: 'Dar de Baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                visible = valores.BotonDarDeBaja;
            } else {
                visible = valores.BotonDarDeBaja(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja == false;
        },
        Validar: function (data) {
            if ('BotonDarDeBajaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAplicacionDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackDarDeBaja(app);
                }
            })
        }
    });

    //Boton Activar
    if (!('BotonActivar' in valores)) {
        valores.BotonActivar = false;
    }

    if (!('BotonActivarOculto' in valores)) {
        valores.BotonActivarOculto = true;
    }

    botones.push({
        Texto: 'Activar',
        Icono: 'restore',
        Oculto: valores.BotonActivarOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonActivar) === 'boolean') {
                visible = valores.BotonActivar;
            } else {
                visible = valores.BotonActivar(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja;
        },
        Validar: function (data) {
            if ('BotonActivarValidar' in valores) {
                return valores.BotonDarDebajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAplicacionActivar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackActivar(app);
                }
            })
        }
    });

    //Boton Bloqeuar
    if (!('BotonBloquear' in valores)) {
        valores.BotonBloquear = false;
    }

    if (!('BotonBloquearOculto' in valores)) {
        valores.BotonBloquearOculto = true;
    }

    botones.push({
        Texto: 'Bloquear',
        Icono: 'lock',
        Oculto: valores.BotonBloquearOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonBloquear) === 'boolean') {
                visible = valores.BotonBloquear;
            } else {
                visible = valores.BotonBloquear(data);
            }

            if (!visible) return false;
            return !data.DadoDeBaja && !data.Bloqueado;
        },
        Validar: function (data) {
            if ('BotonBloquearValidar' in valores) {
                return valores.BotonBloquearValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAplicacionBloquear({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackBloquear(app);
                }
            })
        }
    });

    //Boton Desbloquear
    if (!('BotonDesbloquear' in valores)) {
        valores.BotonDesbloquear = false;
    }

    if (!('BotonDesbloquearOculto' in valores)) {
        valores.BotonDesbloquearOculto = true;
    }

    botones.push({
        Texto: 'Desbloquear',
        Icono: 'lock_open',
        Oculto: valores.BotonDesbloquearOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDesbloquear) === 'boolean') {
                visible = valores.BotonDesbloquear;
            } else {
                visible = valores.BotonDesbloquear(data);
            }

            if (!visible) return false;
            return !data.DadoDeBaja && data.Bloqueado;
        },
        Validar: function (data) {
            if ('BotonDesbloquearValidar' in valores) {
                return valores.BotonDesbloquearValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAplicacionDesbloquear({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackDesbloquear(app);
                }
            })
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* Datatable Usuario */

jQuery.fn.DataTableUsuario = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Editar
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Callback Activar
    if (!('CallbackActivar' in valores)) {
        valores.CallbackActivar = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    //Callback ReiniciarContraseña
    if (!('CallbackReiniciarContraseña' in valores)) {
        valores.CallbackReiniciarContraseña = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: "Nombre",
            data: "Nombre",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: "Apellido",
            data: "Apellido",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: "DNI",
            data: "Dni",
            render: function (data, type, row) {
                return '<div><span>' + data + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'DadoDeBaja',
            render: function (data, type, row) {
                if (data) {
                    return '<div><label class="indicador-estado colorDeBaja"></label><span>De baja</span></div>';
                } else {
                    return '<div><label class="indicador-estado colorActivo"></label><span>Activo</span></div>';
                }
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Texto: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            var validar = true;
            if ('BotonEditarValidar' in valores) {
                validar = valores.BotonEditarValidar(data);
            }
            if (!validar) return false;

            if (data.DadoDeBaja) {
                valores.CallbackMensajes('Alerta', 'El usuario se encuentra dado de baja. Para editarlo primero debe activarlo');
                return false;
            }

            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackEditar(app);
                }
            })
        }
    });

    //Boton Dar De Baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    botones.push({
        Texto: 'Dar de Baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                visible = valores.BotonDarDeBaja;
            } else {
                visible = valores.BotonDarDeBaja(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja == false;
        },
        Validar: function (data) {
            if ('BotonDarDeBajaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackDarDeBaja(app);
                }
            })
        }
    });

    //Boton Activar
    if (!('BotonActivar' in valores)) {
        valores.BotonActivar = false;
    }

    if (!('BotonActivarOculto' in valores)) {
        valores.BotonActivarOculto = true;
    }

    botones.push({
        Texto: 'Activar',
        Icono: 'restore',
        Oculto: valores.BotonActivarOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonActivar) === 'boolean') {
                visible = valores.BotonActivar;
            } else {
                visible = valores.BotonActivar(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja;
        },
        Validar: function (data) {
            if ('BotonActivarValidar' in valores) {
                return valores.BotonDarDebajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioActivar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackActivar(app);
                }
            })
        }
    });

    //Boton ReiniciarContraseña
    if (!('BotonReiniciarContraseña' in valores)) {
        valores.BotonReiniciarContraseña = false;
    }

    if (!('BotonReiniciarContraseñaOculto' in valores)) {
        valores.BotonReiniciarContraseñaOculto = true;
    }

    botones.push({
        Texto: 'Reiniciar Contraseña',
        Icono: 'vpn_key',
        Oculto: valores.BotonReiniciarContraseñaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonReiniciarContraseña) === 'boolean') {
                visible = valores.BotonReiniciarContraseña;
            } else {
                visible = valores.BotonReiniciarContraseña(data);
            }

            if (!visible) return false;
            return !data.DadoDeBaja;
        },
        Validar: function (data) {
            if ('BotonReiniciarContraseñaValidar' in valores) {
                return valores.BotonReiniciarContraseña(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioReiniciarContraseña({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackReiniciarContraseña(app);
                }
            })
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* Datatable Usuario Cerrojo */

jQuery.fn.DataTableUsuarioCerrojo = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Callback Activar
    if (!('CallbackActivar' in valores)) {
        valores.CallbackActivar = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: "Nombre",
            data: null,
            render: function (data, type, row) {
                if (!('User' in row) || row.User == undefined) return '';
                return '<div><span>' + toTitleCase(row.User.Nombre) + '</span></div>';
            }
        },
        {
            title: "Apellido",
            data: null,
            render: function (data, type, row) {
                if (!('User' in row) || row.User == undefined) return '';
                return '<div><span>' + toTitleCase(row.User.Apellido) + '</span></div>';
            }
        },
        {
            title: "Rol Cerrojo",
            data: null,
            render: function (data, type, row) {
                if (!('RolCerrojo' in row) || row.RolCerrojo == undefined) return '';
                return '<div><span>' + toTitleCase(row.RolCerrojo.Nombre) + '</span></div>';
            }
        },
        {
            title: "Aplicación",
            data: null,
            render: function (data, type, row) {
                if (!('Aplicacion' in row) || row.Aplicacion == undefined) return '';
                return '<div><span>' + toTitleCase(row.Aplicacion.Nombre) + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'DadoDeBaja',
            render: function (data, type, row) {
                if (data) {
                    return '<div><label class="indicador-estado colorDeBaja"></label><span>De baja</span></div>';
                } else {
                    return '<div><label class="indicador-estado colorActivo"></label><span>Activo</span></div>';
                }
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            var visible = true;
            if (typeof (valores.BotonDetalle) === 'boolean') {
                visible = valores.BotonDetalle;
            } else {
                visible = valores.BotonDetalle(data);
            }

            if (!visible) return false;

            return data.IdAplicacion != 0;
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioDetalle({
                Id: data.IdUser,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Texto: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            var validar = true;
            if ('BotonEditarValidar' in valores) {
                validar = valores.BotonEditarValidar(data);
            }
            if (!validar) return false;

            if (data.DadoDeBaja) {
                valores.CallbackMensajes('Alerta', 'El usuario se encuentra dado de baja. Para editarlo primero debe activarlo');
                return false;
            }

            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioCerrojoEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackEditar(app);
                }
            })
        }
    });

    //Boton Dar De Baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    botones.push({
        Texto: 'Dar de Baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                visible = valores.BotonDarDeBaja;
            } else {
                visible = valores.BotonDarDeBaja(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja == false;
        },
        Validar: function (data) {
            if ('BotonDarDeBajaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioCerrojoDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackDarDeBaja(app);
                }
            })
        }
    });

    //Boton Activar
    if (!('BotonActivar' in valores)) {
        valores.BotonActivar = false;
    }

    if (!('BotonActivarOculto' in valores)) {
        valores.BotonActivarOculto = true;
    }

    botones.push({
        Texto: 'Activar',
        Icono: 'restore',
        Oculto: valores.BotonActivarOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonActivar) === 'boolean') {
                visible = valores.BotonActivar;
            } else {
                visible = valores.BotonActivar(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja;
        },
        Validar: function (data) {
            if ('BotonActivarValidar' in valores) {
                return valores.BotonDarDebajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioCerrojoActivar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackActivar(app);
                }
            })
        }
    });

    //Agrego los botones
    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* Datatable Area */

jQuery.fn.DataTableArea = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Editar
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Callback Activar
    if (!('CallbackActivar' in valores)) {
        valores.CallbackActivar = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: "Nombre",
            data: "Nombre",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: "Codigo",
            data: "Codigo",
            render: function (data, type, row) {
                return '<div><span>' + data + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'DadoDeBaja',
            render: function (data, type, row) {
                if (data) {
                    return '<div><label class="indicador-estado colorDeBaja"></label><span>De baja</span></div>';
                } else {
                    return '<div><label class="indicador-estado colorActivo"></label><span>Activo</span></div>';
                }
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAreaDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Texto: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            var validar = true;
            if ('BotonEditarValidar' in valores) {
                validar = valores.BotonEditarValidar(data);
            }
            if (!validar) return false;

            if (data.DadoDeBaja) {
                valores.CallbackMensajes('Alerta', 'El usuario se encuentra dado de baja. Para editarlo primero debe activarlo');
                return false;
            }

            return true;
        },
        OnClick: function (data) {
            crearDialogoAreaEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackEditar(app);
                }
            })
        }
    });

    //Boton Dar De Baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    botones.push({
        Texto: 'Dar de Baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                visible = valores.BotonDarDeBaja;
            } else {
                visible = valores.BotonDarDeBaja(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja == false;
        },
        Validar: function (data) {
            if ('BotonDarDeBajaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAreaDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackDarDeBaja(app);
                }
            })
        }
    });

    //Boton Activar
    if (!('BotonActivar' in valores)) {
        valores.BotonActivar = false;
    }

    if (!('BotonActivarOculto' in valores)) {
        valores.BotonActivarOculto = true;
    }

    botones.push({
        Texto: 'Activar',
        Icono: 'restore',
        Oculto: valores.BotonActivarOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonActivar) === 'boolean') {
                visible = valores.BotonActivar;
            } else {
                visible = valores.BotonActivar(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja;
        },
        Validar: function (data) {
            if ('BotonActivarValidar' in valores) {
                return valores.BotonDarDebajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAreaActivar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackActivar(app);
                }
            })
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* DataTable Objeto */

jQuery.fn.DataTableObjeto = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Editar
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Callback Activar
    if (!('CallbackActivar' in valores)) {
        valores.CallbackActivar = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: "Titulo",
            data: "Titulo",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: "Nombre",
            data: "Nombre",
            render: function (data, type, row) {
                return '<div><span>' + data + '</span></div>';
            }
        },
        {
            title: "Valor",
            data: "Valor",
            render: function (data, type, row) {
                return '<div><span>' + data + '</span></div>';
            }
        },
        {
            title: "Código",
            data: "Codigo",
            render: function (data, type, row) {
                return '<div><span>' + data + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'DadoDeBaja',
            render: function (data, type, row) {
                if (data) {
                    return '<div><label class="indicador-estado colorDeBaja"></label><span>De baja</span></div>';
                } else {
                    return '<div><label class="indicador-estado colorActivo"></label><span>Activo</span></div>';
                }
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoObjetoDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Texto: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            var validar = true;
            if ('BotonEditarValidar' in valores) {
                validar = valores.BotonEditarValidar(data);
            }
            if (!validar) return false;

            if (data.DadoDeBaja) {
                valores.CallbackMensajes('Alerta', 'El objeto se encuentra dado de baja. Para editarlo primero debe activarlo');
                return false;
            }

            return true;
        },
        OnClick: function (data) {
            crearDialogoObjetoEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackEditar(app);
                }
            })
        }
    });

    //Boton Dar De Baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    botones.push({
        Texto: 'Dar de Baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                visible = valores.BotonDarDeBaja;
            } else {
                visible = valores.BotonDarDeBaja(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja == false;
        },
        Validar: function (data) {
            if ('BotonDarDeBajaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoObjetoDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackDarDeBaja(app);
                }
            })
        }
    });

    //Boton Activar
    if (!('BotonActivar' in valores)) {
        valores.BotonActivar = false;
    }

    if (!('BotonActivarOculto' in valores)) {
        valores.BotonActivarOculto = true;
    }

    botones.push({
        Texto: 'Activar',
        Icono: 'restore',
        Oculto: valores.BotonActivarOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonActivar) === 'boolean') {
                visible = valores.BotonActivar;
            } else {
                visible = valores.BotonActivar(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja;
        },
        Validar: function (data) {
            if ('BotonActivarValidar' in valores) {
                return valores.BotonDarDebajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoObjetoActivar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackActivar(app);
                }
            })
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* DataTable AccesoObjeto */

jQuery.fn.DataTableAccesoObjeto = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Editar
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Callback Activar
    if (!('CallbackActivar' in valores)) {
        valores.CallbackActivar = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: "Objeto",
            data: null,
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(row.Titulo) + '</span></div>';
            }
        },
        {
            title: "Acceso",
            data: null,
            render: function (data, type, row) {
                var alta = row.Alta;
                var baja = row.Baja;
                var modificacion = row.Modificacion;
                var consulta = row.Consulta;

                var abmc = '';
                if (alta) abmc += 'A';
                if (baja) abmc += 'B';
                if (modificacion) abmc += 'M';
                if (consulta) abmc += 'C';

                return '<div><span>' + abmc + '</span></div>';
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoObjetoDetalle({
                Id: data.IdObjeto,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Texto: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            var validar = true;
            if ('BotonEditarValidar' in valores) {
                validar = valores.BotonEditarValidar(data);
            }
            if (!validar) return false;

            if (data.DadoDeBaja) {
                valores.CallbackMensajes('Alerta', 'El objeto se encuentra dado de baja. Para editarlo primero debe activarlo');
                return false;
            }

            return true;
        },
        OnClick: function (data) {
            crearDialogoAccesoObjetoEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackEditar(app);
                }
            })
        }
    });

    //Boton Dar De Baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    botones.push({
        Texto: 'Dar de Baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                visible = valores.BotonDarDeBaja;
            } else {
                visible = valores.BotonDarDeBaja(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja == false;
        },
        Validar: function (data) {
            if ('BotonDarDeBajaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAccesoObjetoDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackDarDeBaja(app);
                }
            })
        }
    });

    //Boton Activar
    if (!('BotonActivar' in valores)) {
        valores.BotonActivar = false;
    }

    if (!('BotonActivarOculto' in valores)) {
        valores.BotonActivarOculto = true;
    }

    botones.push({
        Texto: 'Activar',
        Icono: 'restore',
        Oculto: valores.BotonActivarOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonActivar) === 'boolean') {
                visible = valores.BotonActivar;
            } else {
                visible = valores.BotonActivar(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja;
        },
        Validar: function (data) {
            if ('BotonActivarValidar' in valores) {
                return valores.BotonDarDebajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoAccesoObjetoActivar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackActivar(app);
                }
            })
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* DataTable Rol */

jQuery.fn.DataTableRol = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Editar
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Callback Activar
    if (!('CallbackActivar' in valores)) {
        valores.CallbackActivar = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: 'Nombre',
            data: 'Nombre',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'DadoDeBaja',
            render: function (data, type, row) {
                if (data) {
                    return '<div><label class="indicador-estado colorDeBaja"></label><span>De baja</span></div>';
                } else {
                    return '<div><label class="indicador-estado colorActivo"></label><span>Activo</span></div>';
                }
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoRolDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Texto: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            var validar = true;
            if ('BotonEditarValidar' in valores) {
                validar = valores.BotonEditarValidar(data);
            }
            if (!validar) return false;

            if (data.DadoDeBaja) {
                valores.CallbackMensajes('Alerta', 'El rol se encuentra dado de baja. Para editarlo primero debe activarlo');
                return false;
            }

            return true;
        },
        OnClick: function (data) {
            crearDialogoRolEditar({
                Id: data.Id,
                Ancho: '0.95',
                Alto: '0.95',
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackEditar(app);
                }
            })
        }
    });

    //Boton Dar De Baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    botones.push({
        Texto: 'Dar de Baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                visible = valores.BotonDarDeBaja;
            } else {
                visible = valores.BotonDarDeBaja(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja == false;
        },
        Validar: function (data) {
            if ('BotonDarDeBajaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoRolDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackDarDeBaja(app);
                }
            })
        }
    });

    //Boton Activar
    if (!('BotonActivar' in valores)) {
        valores.BotonActivar = false;
    }

    if (!('BotonActivarOculto' in valores)) {
        valores.BotonActivarOculto = true;
    }

    botones.push({
        Texto: 'Activar',
        Icono: 'restore',
        Oculto: valores.BotonActivarOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonActivar) === 'boolean') {
                visible = valores.BotonActivar;
            } else {
                visible = valores.BotonActivar(data);
            }

            if (!visible) return false;
            return data.DadoDeBaja;
        },
        Validar: function (data) {
            if ('BotonActivarValidar' in valores) {
                return valores.BotonDarDebajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoRolActivar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (app) {
                    valores.CallbackActivar(app);
                }
            })
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* DataTable Usuario Aplicacion */

jQuery.fn.DataTableUsuarioAplicacion = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Editar
    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //Callback Activar
    if (!('CallbackActivar' in valores)) {
        valores.CallbackActivar = function () { };
    }

    //Callback Dar De Baja
    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    //Callback Excepciones
    if (!('CallbackExcepciones' in valores)) {
        valores.CallbackExcepciones = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: 'Usuario',
            data: 'UserNombre',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Aplicacion',
            data: 'AplicacionNombre',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Area',
            data: 'AreaNombre',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Rol',
            data: 'RolNombre',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: null,
            render: function (data, type, row) {
                if ('DadoDeBaja' in row) {
                    if (row.DadoDeBaja) {
                        return '<div><label class="indicador-estado colorDeBaja"></label><span>De baja</span></div>';
                    } else {
                        return '<div><label class="indicador-estado colorActivo"></label><span>Activo</span></div>';
                    }
                } else {
                    if (row.FechaBaja != undefined) {
                        return '<div><label class="indicador-estado colorDeBaja"></label><span>De baja</span></div>';
                    } else {
                        return '<div><label class="indicador-estado colorActivo"></label><span>Activo</span></div>';
                    }
                }
            }
        },
        {
            title: 'Excepciones',
            data: 'TieneExcepciones',
            render: function (data, type, row) {
                var icono = data ? 'check' : '';
                return '<div><span><i class="material-icons">' + icono + '</i></span></div>';
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioAplicacionDetalle({
                Id: data.IdObjeto,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Texto: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            var validar = true;
            if ('BotonEditarValidar' in valores) {
                validar = valores.BotonEditarValidar(data);
            }
            if (!validar) return false;

            if (data.DadoDeBaja) {
                valores.CallbackMensajes('Alerta', 'El rol se encuentra dado de baja. Para editarlo primero debe activarlo');
                return false;
            }

            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioAplicacionEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (data) {
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioRolPorAreaPorUsuarioPorAplicacion.asmx/GetDatosTabla'),
                        Data: { id: data.Id },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                return;
                            }

                            valores.CallbackEditar(result.Return);
                        },
                        OnError: function (result) {
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            });
        }
    });

    //Boton Dar De Baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    botones.push({
        Texto: 'Dar de Baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                visible = valores.BotonDarDeBaja;
            } else {
                visible = valores.BotonDarDeBaja(data);
            }

            if (!visible) return false;
            if ('DadoDeBaja' in data) {
                return data.DadoDeBaja == false;
            } else {
                return data.FechaBaja == null;
            }
        },
        Validar: function (data) {
            if ('BotonDarDeBajaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioAplicacionDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (data) {
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioRolPorAreaPorUsuarioPorAplicacion.asmx/GetDatosTabla'),
                        Data: { id: data.Id },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                return;
                            }

                            valores.CallbackDarDeBaja(result.Return);
                        },
                        OnError: function (result) {
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            })
        }
    });

    //Boton Activar
    if (!('BotonActivar' in valores)) {
        valores.BotonActivar = false;
    }

    if (!('BotonActivarOculto' in valores)) {
        valores.BotonActivarOculto = true;
    }

    botones.push({
        Texto: 'Activar',
        Icono: 'restore',
        Oculto: valores.BotonActivarOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonActivar) === 'boolean') {
                visible = valores.BotonActivar;
            } else {
                visible = valores.BotonActivar(data);
            }

            if (!visible) return false;
            if ('DadoDeBaja' in data) {
                return data.DadoDeBaja;
            } else {
                return data.FechaBaja != null;
            }
        },
        Validar: function (data) {
            if ('BotonActivarValidar' in valores) {
                return valores.BotonDarDebajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoUsuarioAplicacionActivar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (data) {
                    crearAjax({
                        Url: ResolveUrl('~/Servicios/ServicioRolPorAreaPorUsuarioPorAplicacion.asmx/GetDatosTabla'),
                        Data: { id: data.Id },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                return;
                            }

                            valores.CallbackActivar(result.Return);
                        },
                        OnError: function (result) {
                            valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            })
        }
    });

    //Boton Excepciones
    if (!('BotonExcepciones' in valores)) {
        valores.BotonExcepciones = false;
    }

    if (!('BotonExcepcionesOculto' in valores)) {
        valores.BotonExcepcionesOculto = true;
    }

    botones.push({
        Texto: 'Excepciones',
        Oculto: valores.BotonExcepcionesOculto,
        Visible: function (data) {
            var visible;
            if (typeof (valores.BotonExcepciones) === 'boolean') {
                visible = valores.BotonExcepciones;
            } else {
                visible = valores.BotonExcepciones(data);
            }

            if (!visible) return false;
            if ('DadoDeBaja' in data) {
                return !data.DadoDeBaja;
            } else {
                return data.FechaBaja == null;
            }
        },
        Validar: function (data) {
            if ('BotonExcepcionesValidar' in valores) {
                return valores.BotonExcepcionesValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var url = '~/Paginas/IFrames/IExcepcionUsuarioAplicacion.aspx?';
            url += 'IdUser=' + data.IdUser + '&';
            url += 'IdArea=' + data.IdArea + '&';
            url += 'IdRol=' + data.IdRol;

            crearDialogoIFrame({
                Titulo: '<label>Excepciones para un Usuario</label>',
                Url: ResolveUrl(url),
                OnLoad: function (jAlert, iFrame) {
                    iFrame.setOnCargandoListener(function (cargando, mensaje) {
                        $(jAlert).MostrarDialogoCargando(cargando, true);
                    });

                    iFrame.setOnMensajeListener(function (tipo, mensaje) {
                        valores.CallbackMensajes(tipo, mensaje);
                    });

                    iFrame.setOnRegistrarListener(function () {

                        valores.CallbackMensajes('Exito', 'Excepciones registradas correctamente');

                        crearAjax({
                            Url: ResolveUrl('~/Servicios/ServicioRolPorAreaPorUsuarioPorAplicacion.asmx/GetDatosTabla'),
                            Data: { id: data.Id },
                            OnSuccess: function (result) {
                                if (!result.Ok) {
                                    valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                    return;
                                }


                                $(jAlert).CerrarDialogo();
                                valores.CallbackExcepciones(result.Return);
                            },
                            OnError: function (result) {
                                $(jAlert).CerrarDialogo();
                                valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                            }
                        });
                    });

                    iFrame.setOnCancelarExcepcionesListener(function () {
                        mostrarMensajeExito('Excepciones canceladas correctamente');

                        crearAjax({
                            Url: ResolveUrl('~/Servicios/ServicioRolPorAreaPorUsuarioPorAplicacion.asmx/GetDatosTabla'),
                            Data: { id: data.Id },
                            OnSuccess: function (result) {
                                if (!result.Ok) {
                                    valores.CallbackMensajes('Error', result.Errores.Mensaje);
                                    return;
                                }

                                $(jAlert).CerrarDialogo();
                                valores.CallbackExcepciones(result.Return);
                            },
                            OnError: function (result) {
                                $(jAlert).CerrarDialogo();
                                valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                            }
                        });
                    });
                },
                Botones: [
                    {
                        Texto: 'Cancelar'
                    },
                    {
                        Texto: 'Aceptar',
                        Class: 'colorExito',
                        CerrarDialogo: false,
                        OnClick: function (jAlert, iFrame) {
                            iFrame.registrar();
                        }
                    }
                ]
            });
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

/* Logs */

jQuery.fn.DataTableLogAcceso = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback Seleccion
    if (!('CallbackSeleccionar' in valores)) {
        valores.CallbackSeleccionar = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[6, 'desc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
        {
            title: 'Nombre',
            data: 'UsuarioNombre',
            render: function (data, type, row) {
                return data;
            }
        },
        {
            title: 'Apellido',
            data: 'UsuarioApellido',
            render: function (data, type, row) {
                return data;
            }
        },
        {
            title: 'Aplicacion',
            data: 'AplicacionNombre',
            render: function (data, type, row) {
                return data;
            }
        },
        {
            title: 'Area',
            data: 'AreaNombre',
            render: function (data, type, row) {
                return data;
            }
        },
        {
            title: 'Rol',
            data: 'RolNombre',
            render: function (data, type, row) {
                return data;
            }
        },
        {
            title: 'Accion',
            data: 'Accion',
            render: function (data, type, row) {
                return data;
            }
        },
        {
            title: 'Fecha',
            data: 'FechaAlta',
            render: function (data, type, row) {
                return toDateTimeString(data);
            }
        },
        {
            title: 'Exito',
            data: 'Exito',
            render: function (data, type, row) {
                if (data) {
                    return '<i class="material-icons">check</i>';
                }
                return '';
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Texto: 'Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoLogAccesoDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Seleccion
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    botones.push({
        Texto: 'Seleccionar',
        Icono: 'check',
        Oculto: false,
        Visible: function (data) {
            if (typeof (valores.BotonSeleccionar) === 'boolean') {
                return valores.BotonSeleccionar;
            } else {
                return valores.BotonSeleccionar(data);
            }
        },
        Validar: function (data) {
            if ('BotonSeleccionarValidar' in valores) {
                return valores.BotonSeleccionarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            valores.CallbackSeleccionar(data);
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}
