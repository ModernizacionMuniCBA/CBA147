var descripcionMotivo = "";

var modoRegistrar = 'Registrar';
var modoEditar = 'Editar';

var modo;

var callbackRegistrar;
var callbackRegistrarError;
var callbackEditar;
var callbackTab;
var callbackHeight;

var idEditar;
var documentos = [];
var imagenes = [];

var tablaNotas;

// ADJUNTOS
const IMAGE_SIZE = 5;
const DOCUMENT_SIZE = 5;
const IMAGE_EXTENSIONS = ['png', 'jpg', 'gif', 'tiff', 'jpeg'];
const DOCUMENT_EXTENSIONS = ['pdf', 'txt', 'doc', 'docx', 'xls', 'xlsx', 'pps', 'ppt', 'pptx', 'ogg', 'odt', 'ott', 'ods'];

var tipoMotivoGeneral = "1";
var tipoMotivoInterno = "2";
var tipoMotivoPrivado = "3";

$(document).ready(function () {
    $('.tooltipAyuda').each(function () { // Notice the .each() loop, discussed below
        $(this).qtip({
            content: {
                text: $(this).next('div') // Use the "div" element next to this for the content
            },
            position: {
                my: 'top center',
                at: 'bottom center'
            },
            style: {
                classes: 'qtip-shadow qtip-rounded qtip-tipsy'
            }
        });
    });
});

function init(data) {

    //----------------------------------
    // Datos enviados desde el server
    //----------------------------------
    data = parse(data);
    mostrarCargando(true);

    //--------------------------------
    // Motivo
    //--------------------------------

    SelectorMotivo_Init({
        TipoMotivo: tipoMotivo,
        MostrarTiposMotivo: false,
        CallbackMensaje: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackModo: function (modoManual) {
            //Oculto el error
            $('#errorFormulario_Motivo').stop(true, true).hide(300);
        },
        Callback: function (motivo) {
            camposDinamicos = [];
            //vacio los campos dinamicos anteriores
            $("#contenedor_CamposDinamicos").empty();

            if (motivo != undefined && motivo != null) {
                mostrarCargando(true);
                //Oculto el error
                $('#errorFormulario_Motivo').stop(true, true).hide(300);

                $('#contenedorDescripcionMotivo').show(300);
                setTimeout(function () { $('#inputFormulario_Descripcion').trigger('focus') }, 300);

                if (motivo.Tipo == tipoMotivoInterno) {
                    $("#contenedor_Iniciador").hide();
                } else {
                    $("#contenedor_Iniciador").show();
                }

                getCamposDinamicosByMotivo(motivo.Id);
            } else {
                $('#contenedorDescripcionMotivo').hide(300);
            }
        }
    });

    //--------------------------------
    // Ubicacion del reclamo
    //--------------------------------

    initEventosUbicacion();

    //--------------------------------
    // Usuario
    //--------------------------------

    SelectorUsuario_SetAbrirNuevoUsuarioSiNoEncuentro(true);

    SelectorUsuario_SetOnCargandoListener(function (mostrar, mensaje) {
        mostrarCargando(mostrar, mensaje);
    });

    SelectorUsuario_SetOnMensajeListener(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    })

    SelectorUsuario_SetOnUsuarioSeleccionadoListener(function (usuario) {
        //Quito el error
        $('#errorFormulario_Usuario').stop(true, true).hide(300);

        if (usuario == null) {
            $('#inputFormulario_Mail').val("");
        } else {
            $('#inputFormulario_Mail').val(usuario.Mail);
        }

        Materialize.updateTextFields();
    });

    initTablaNotas();
    initDocumentos();
    initImagenes();

    //Tab
    informarTabChange();

    $('.tabs a').click(function () {
        setTimeout(function () {
            informarTabChange();
        }, 10);
    });

    //----------------------------------
    // Inicializo segun el modo
    //----------------------------------

    idEditar = $.url().param('Id');
    modo = idEditar != undefined ? modoEditar : modoRegistrar;
    mostrarCargando(false);
}

//------------------------------
// Campos dinámicos
//------------------------------
function getCamposDinamicosByMotivo(id) {
    crearAjax({
        Url: ResolveUrl('~/Servicios/MotivoService.asmx/GetCamposByIdMotivo'),
        Data: { idMotivo: id },
        OnSuccess: function (result) {
            result = parse(result);

            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('error', result.Error);
                return;
            }

            camposDinamicos = result.Return;
            cargarCamposDinamicos(camposDinamicos).then(function (html) {
                html = '    <div class="form-separador"></div>' + html;
                $("#contenedor_CamposDinamicos").append(html);
                mostrarCargando(false);
                init_Campos();
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('error', result.Error);
            return;
        }
    });
}

//------------------------------
// Ubicación
//------------------------------

function initEventosUbicacion() {
    ControlDomicilioSelector_Init({ModoManualDefecto: tipoMotivo!=tipoMotivoInterno});
}
//------------------------------
// Notas
//------------------------------

function initTablaNotas() {
    tablaNotas = $('#tablaNotas').DataTableGeneral({
        Orden: [[0, 'desc']],
        Columnas: [
            {
                "sTitle": "Fecha",
                "mData": "FechaAltaString",
                "width": "140px",
                render: function (data, type, row) {
                    return '<div><span>' + data + '</span></div>';
                }
            },
            {
                "sTitle": "Orden de Trabajo",
                "mData": null,
                "width": "140px",
                render: function (data, type, row) {
                    if ('OrdenTrabajo' in row && row.OrdenTrabajo != null) {
                        return '<div><span>' + row.OrdenTrabajo.Numero + '/' + row.OrdenTrabajo.Año + '</span></div>';
                    } else {
                        return '';
                    }
                }
            },
            {
                "sTitle": "Observaciones",
                "mData": "Observaciones",
                render: function (data, type, row) {
                    return '<div><span>' + data + '</span></div>';
                }

            }
        ],
        Botones: [
            {
                Titulo: 'Quitar',
                Icono: 'clear',
                OnClick: function (data) {

                    tablaNotas.rows().every(function (rowIdx, tableLoop, rowLoop) {
                        var info = this.data();
                        if (info.Id == data.Id) {
                            this.node().remove();
                            return;
                        }
                    });

                    setTimeout(function () {
                        $('.material-tooltip').hide();
                    }, 100);
                }
            }
        ]
    });

    //Muevo el indicador y el paginado a mi propio div
    $('#tabNotas').find('.tabla-footer').empty();
    $('#tabNotas').find('.dataTables_info').detach().appendTo($('#tabNotas').find('.tabla-footer'));
    $('#tabNotas').find('.dataTables_paginate').detach().appendTo($('#tabNotas').find('.tabla-footer'));
}

function cargarFilasEnTablaNotas(data) {
    tablaNotas.clear();
    if (data != undefined && data.length != 0) {
        tablaNotas.rows.add(data);
    }
    tablaNotas.draw();
}

var idNota = 1;

function agregarNota() {
    crearDialogoHTML({
        Titulo: '<label>Agregar Nota</label>',
        Content: '<div class="row margin-top" >' +
                            '<div class="col s12">' +
                                '<div class="input-field">' +
                                    '<textarea id="inputFormulario_Observaciones" class="materialize-textarea"></textarea>' +
                                    '<label for="inputFormulario_Observaciones" class=" no-select textarea">Observaciones</label>' +
                                '</div>' +
                            '</div>' +
                        '</div>',
        OnLoad: function (jAlert) {
            setTimeout(function () {
                $(jAlert).find('#inputFormulario_Observaciones').trigger('focus');
            }, 300);
        },
        Botones:
            [
                {
                    Texto: 'Cancelar'
                },
                {
                    Texto: 'Aceptar',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert) {
                        var obs = $(jAlert).find('#inputFormulario_Observaciones').val();
                        if (obs == null || obs == "" || obs == undefined) {
                            mostrarMensajeAlerta('Debe ingresar el contenido de la nota');
                            return;
                        }

                        idNota += 1;

                        var nota = {}
                        nota.Id = -(idNota);
                        nota.Observaciones = '' + obs;
                        nota.FechaAlta = new Date();
                        nota.FechaAltaString = moment(nota.FechaAlta).format('DD/MM/YYYY HH:mm');
                        tablaNotas.row.add(nota).draw();
                        tablaNotas.$('.tooltipped').tooltip({ delay: 50 });

                        $(jAlert).CerrarDialogo();
                    }
                }
            ]
    });

}

//------------------------------
// Documentos
//------------------------------
var numDocumento = 1;

function initDocumentos() {

    // Btn Nuevo
    $('#inputDocumento').change(function (e) {
        var files = this.files;
        procesarDocumentos(files, function (documentos) {
            dibujarDocumentos();
        });
    });

    $('#btnNuevoDocumento').click(function () {
        $('#inputDocumento').val("");
        $('#inputDocumento').trigger('click');
    });

    //-------------------------------------
    // DragDrop
    //-------------------------------------

    //var dragTimer;
    //$('#tabDocumentos').on('dragover', function (e) {
    //    var dt = e.originalEvent.dataTransfer;
    //    if (dt.types && (dt.types.indexOf ? dt.types.indexOf('Files') != -1 : dt.types.contains('Files'))) {
    //        //$("#dragZoneDocumentos").show();
    //        window.clearTimeout(dragTimer);
    //    }
    //});

    //$('#tabDocumentos').on('dragleave', function (e) {
    //    dragTimer = window.setTimeout(function () {
    //        //$("#dragZoneDocumentos").hide();
    //    }, 25);
    //});

    $('#tabDocumentos').on('drop', function (e) {
        var files = e.originalEvent.dataTransfer.files;
        procesarDocumentos(files, function (documentos) {
            dibujarDocumentos();
        });

        //dragTimer = window.setTimeout(function () {
        //    //$("#dragZoneDocumentos").hide();
        //}, 25);
        e.preventDefault();
        e.stopPropagation();
    });

    window.addEventListener("dragover", function (e) {
        e = e || event;
        e.preventDefault();
    }, false);
    window.addEventListener("drop", function (e) {
        e = e || event;
        e.preventDefault();
    }, false);

    //Cargando
    var firstClick = true;
    $('#pestañaDocumentos').click(function () {
        if (modo != modoEditar) return;
        if (!firstClick) return;
        firstClick = false;

        //consultarDocumentos();
    });

}

function procesarDocumentos(documentosNuevos, callback) {
    var file = documentosNuevos[0];

    procesarDoc(file, function (doc) {
        if (doc != null) {
            documentos.push(doc);
        }

        if (documentosNuevos.length != 1) {
            var listaNueva = [];
            $.each(documentosNuevos, function (index, val) {
                if (index != 0) {
                    listaNueva.push(val);
                }
            });
            procesarDocumentos(listaNueva, callback);
        } else {
            callback();
        }
        return;
    });
}

function procesarDoc(file, callback) {
    if (!validarExtension(file, false)) {
        mostrarMensajeAlerta('El documento ' + file.name + ' no tiene un formato soportado. Formatos soportados: ' + DOCUMENT_EXTENSIONS.join(', '));
        callback(null);
        return;
    }

    if (!validarSize(file, false)) {
        mostrarMensajeAlerta('El documento ' + file.name + ' es muy grande. Tamaño maximo soportado: ' + DOCUMENT_SIZE + ' [Mb]');
        callback(null);
        return;
    }

    var extension = file.name.split('.').pop().toLowerCase();
    numDocumento += 1;

    //Creo la entidad
    var obj = {
        Id: -numDocumento,
        Nombre: file.name,
        Extension: extension,
        Tipo: 1,
        SinGuardar: true
    };
    console.log(obj);

    //Abro el archivo
    var fr = new FileReader();
    fr.onload = function (e) {
        obj.Data = e.target.result;
        callback(obj);
    }
    fr.readAsDataURL(file);
}

function dibujarDocumentos() {
    if (documentos == undefined || documentos.length == 0) {
        $('#contenedorTituloDocumentos').stop(true, true).fadeOut(300);
        $('#contenedorArchivosDocumentos').stop(true, true).fadeOut(300);
        return;
    }

    $('#contenedorTituloDocumento').stop(true, true).fadeIn(300);
    $('#contenedorArchivosDocumentos').stop(true, true).fadeIn(300);

    $.each(documentos, function (index, doc) {
        dibujarDocumento(doc);
    });
}

function dibujarDocumento(doc) {
    if (doc == undefined) return;
    var archivo = $('#tabDocumentos').find('.cardArchivo[idArchivo=' + doc.Id + ']');
    if (archivo.length != 0) return;

    $('#contenedorTituloDocumento').stop(true, true).fadeIn(300);
    $('#contenedorArchivosDocumentos').stop(true, true).fadeIn(300);

    //Card
    archivo = $('<div>');
    $(archivo).addClass('card');
    $(archivo).addClass('cardArchivo');
    $(archivo).addClass('waves-effect');
    $(archivo).attr('idArchivo', doc.Id)
    $(archivo).css({
        width: '200px',
        height: '200px'
    });
    $('#contenedorArchivosDocumentos').append(archivo);

    $(archivo).click(function () {
        if (doc.Nombre.endsWith('pdf')) {
            crearDialogoPDF({ Archivo: doc });
        } else {
            crearDialogoDocumento({ Archivo: doc });
        }
    });

    //Canvas
    var canvas = $('<img/>');
    $(canvas).css('width', '200px');
    $(canvas).css('height', '150px');
    $(archivo).append(canvas);
    if (doc.Nombre.endsWith('pdf')) {
        $(archivo).MostrarIndicadorCargando({ Opaco: true });
        renderImagePDF(doc, canvas, function () {
            $(archivo).OcultarIndicadorCargando();
        });
    }

    var divContenedorTitulo = $('<div>');
    $(divContenedorTitulo).addClass('contenedorTitulo');
    $(archivo).append(divContenedorTitulo);

    //Titulo
    var titulo = $('<label>');
    $(titulo).text(doc.Nombre);
    $(divContenedorTitulo).append(titulo);

    //Boton Menu
    var btnMenu = $('<a>');
    $(btnMenu).addClass('btn-flat btn-cuadrado');
    $(btnMenu).addClass('chico');
    var icono = $('<i>');
    $(icono).addClass('material-icons');
    $(icono).text('more_vert');
    $(btnMenu).append(icono);
    $(divContenedorTitulo).append(btnMenu);
    $(btnMenu).click(function (e) {
        $(btnMenu).MenuFlotante({
            Menu: [
                {
                    Texto: 'Ver',
                    Icono: 'search',
                    OnClick: function () {
                        $(archivo).trigger('click');
                    }
                },
                {
                    Texto: 'Renombrar',
                    Icono: 'edit',
                    OnClick: function () {
                        crearDialogoHTML({
                            Content: '<div class="row margin-top" >' +
                                                        '<div class="col s12">' +
                                                            '<div class="input-field">' +
                                                                '<input id="input_Nombre" type="text"></input>' +
                                                                '<label for="input_Nombre" class="no-select">Nombre</label>' +
                                                            '</div>' +
                                                        '</div>' +
                                                    '</div>',
                            OnLoad: function (jAlert) {
                                $(jAlert).find('#input_Nombre').keydown(function (e) {
                                    if (e.keyCode == 13) {
                                        $(jAlert).find('.btnAceptar').trigger('click');
                                    }
                                });

                                $(jAlert).find('#input_Nombre').val(doc.Nombre);
                                $(jAlert).find('#input_Nombre').trigger('focus');
                                Materialize.updateTextFields();
                            },
                            Botones: [
                                {
                                    Texto: 'Cancelar'
                                },
                                {
                                    Texto: 'Aceptar',
                                    Class: 'colorExito btnAceptar',
                                    CerrarDialogo: false,
                                    OnClick: function (jAlert) {



                                        var nombre = $(jAlert).find('#input_Nombre').val();
                                        if (nombre.trim() == "") {
                                            mostrarMensajeAlerta('Debe ingresar un nombre')
                                            return;
                                        }

                                        var extensionOriginal = doc.Extension;
                                        if (!nombre.match(extensionOriginal + '$')) {
                                            mostrarMensajeAlerta('El archivo debe ser del tipo ' + extensionOriginal);
                                            return;
                                        }

                                        doc.Nombre = nombre;
                                        $(titulo).text(nombre);
                                        $(jAlert).CerrarDialogo();
                                    }
                                }
                            ]
                        });
                    }
                },
                {
                    Texto: 'Descargar',
                    Icono: 'file_download',
                    OnClick: function () {
                        descargarArchivo(doc);
                    }
                },
                {
                    Texto: 'Borrar',
                    Icono: 'delete',
                    OnClick: function () {
                        //borro el item
                        var listaNueva = [];
                        $.each(documentos, function (index, d) {
                            if (d.Id != doc.Id) {
                                listaNueva.push(d);
                            }
                        });
                        documentos = listaNueva;

                        //Muestro el titulo si es que no hay mas documentos
                        if (documentos.length == 0) {
                            $('#contenedorTituloDocumento').stop(true, true).fadeOut(300);
                            $('#contenedorArchivosDocumentos').stop(true, true).fadeOut(300);
                        }

                        //Quito la card
                        $(archivo).remove();
                    }
                }
            ]
        });

        e.stopPropagation();
    });
}

function borrarDocumentos() {
    $(contenedorArchivosDocumentos).emty();
    $('#contenedorTituloDocumentos').stop(true, true).fadeOut(300);
    $('#contenedorArchivosDocumentos').stop(true, true).fadeOut(300);
    documentos = [];
}

function renderImagePDF(doc, imagen, callback) {
    if (doc == undefined) return;
    PDFJS.getDocument(doc.Data).then(function getPdf(pdf) {
        pdf.getPage(1).then(function getPage(page) {

            //Calculo el tamaño
            var desiredWidth = 200;
            var viewport = page.getViewport(1);
            var scale = desiredWidth / viewport.width;
            var scaledViewport = page.getViewport(scale);

            var canvas = $('<canvas>');
            $(canvas).get(0).width = 200;
            $(canvas).get(0).height = 150;

            //Renderizo
            var renderContext = {
                canvasContext: $(canvas).get(0).getContext('2d'),
                viewport: scaledViewport
            };
            page.render(renderContext).promise.then(function () {
                var url = $(canvas).get(0).toDataURL();
                $(imagen).attr('src', url);

                callback();
            });
        });
    });
}

//-----------------------------
// Imagenes
//-----------------------------

function initImagenes() {
    $('#inputImagen').change(function (e) {
        var files = this.files;
        procesarImagenes(files, function () {
            dibujarImagenes();
        });
    });

    $('#btnNuevaImagen').click(function () {
        $('#inputImagen').val("");
        $('#inputImagen').trigger('click');
    });

    //var dragTimer;
    //$('#tabImagenes').on('dragover', function (e) {
    //    var dt = e.originalEvent.dataTransfer;
    //    if (dt.types && (dt.types.indexOf ? dt.types.indexOf('Files') != -1 : dt.types.contains('Files'))) {
    //        //$("#dragZoneImagenes").show();
    //        //window.clearTimeout(dragTimer);
    //    }
    //});

    //$('#tabImagenes').on('dragleave', function (e) {
    //    //dragTimer = window.setTimeout(function () {
    //    //    $("#dragZoneImagenes").hide();
    //    //}, 25);
    //});

    $('#tabImagenes').on('drop', function (e) {
        var files = e.originalEvent.dataTransfer.files;
        procesarImagenes(files, function (imagenes) {
            dibujarImagenes();
        });

        //dragTimer = window.setTimeout(function () {
        //    $("#dragZoneImagenes").hide();
        //}, 25);
        e.preventDefault();
        e.stopPropagation();
    });

    window.addEventListener("dragover", function (e) {
        e = e || event;
        e.preventDefault();
    }, false);
    window.addEventListener("drop", function (e) {
        e = e || event;
        e.preventDefault();
    }, false);

    //Cargando
    var firstClick = true;
    $('#pestañaImagenes').click(function () {
        if (modo != modoEditar) return;
        if (!firstClick) return;
        firstClick = false;

        $('#tabImagenes').find('.cargando').show();

        //consultarImagenes();
    });
}

var imgPorProcesar;

function procesarImagenes(imagenesNuevas, callback) {
    imgPorProcesar = imagenesNuevas;

    var file = imgPorProcesar[0];

    procesarImagen(file, function (doc) {
        if (doc != null) {
            imagenes.push(doc);
        }

        if (imgPorProcesar.length != 1) {
            var listaNueva = [];
            $.each(imgPorProcesar, function (index, val) {
                if (index != 0) {
                    listaNueva.push(val);
                }
            });
            imgPorProcesar = listaNueva;
            procesarImagenes(imgPorProcesar, callback);
        } else {
            callback();
        }
        return;
    });
}

function procesarImagen(file, callback) {
    if (!validarExtension(file, true)) {
        mostrarMensajeAlerta('La imagen ' + file.name + ' no tiene un formato soportado. Formatos soportados: ' + IMAGE_EXTENSIONS.join(', '));
        callback(null);
        return;
    }

    if (!validarSize(file, true)) {
        mostrarMensajeAlerta('La imgane ' + file.name + ' es muy grande. Tamaño maximo soportado: ' + IMAGE_SIZE + ' [Mb]');
        callback(null);
        return;
    }

    var extension = file.name.split('.').pop().toLowerCase();
    numDocumento += 1;
    //Creo la entidad
    var obj = {
        Id: -numDocumento,
        Nombre: file.name,
        Extension: extension,
        Tipo: 2,
        SinGuardar: true
    };
    console.log(obj);

    //Abro el archivo
    var fr = new FileReader();
    fr.onload = function (e) {
        obj.Data = e.target.result;
        callback(obj);
    }
    fr.readAsDataURL(file);
}

function dibujarImagenes() {
    if (imagenes == undefined || imagenes.length == 0) {
        $('#contenedorTituloImagenes').stop(true, true).fadeOut(300);
        $('#contenedorArchivosImagenes').stop(true, true).fadeOut(300);
        return;
    }

    $('#contenedorTituloImagenes').stop(true, true).fadeIn(300);
    $('#contenedorArchivosImagenes').stop(true, true).fadeIn(300);

    $.each(imagenes, function (index, img) {
        dibujarImagen(img);
    });
}

function dibujarImagen(imagen) {
    if (imagen == undefined) return;
    var archivo = $('#tabImagenes').find('.cardArchivo[idArchivo=' + imagen.Id + ']');
    if (archivo.length != 0) return;

    //Card
    archivo = $('<div>');
    $(archivo).addClass('card');
    $(archivo).addClass('cardArchivo');
    $(archivo).addClass('waves-effect');
    $(archivo).addClass('no-select');
    $(archivo).attr('idArchivo', imagen.Id)
    $(archivo).css({
        width: '200px',
        height: '200px'
    });
    $('#contenedorArchivosImagenes').append(archivo);
    $(archivo).click(function () {
        var index = 0;
        $.each(imagenes, function (i, img) {
            if (imagen.Id == img.Id) {
                index = i;
            }
        });

        crearDialogoImagenes({
            Imagenes: imagenes,
            Index: index
        });
    });

    //Canvas
    var canvas = $('<img/>');
    $(canvas).addClass('no-select');
    $(canvas).attr('src', imagen.Data);
    $(canvas).css({ width: '200px', height: '150px' });
    $(archivo).append(canvas);

    //Titulo
    var divContenedorTitulo = $('<div>');
    $(divContenedorTitulo).addClass('contenedorTitulo');
    $(archivo).append(divContenedorTitulo);

    var titulo = $('<label>');
    $(titulo).text(imagen.Nombre);
    $(divContenedorTitulo).append(titulo);

    //Boton Menu
    var btnMenu = $('<a>');
    $(btnMenu).addClass('btn-flat btn-cuadrado');
    $(btnMenu).addClass('chico');
    var icono = $('<i>');
    $(icono).addClass('material-icons');
    $(icono).text('more_vert');
    $(btnMenu).append(icono);
    $(divContenedorTitulo).append(btnMenu);
    $(btnMenu).click(function (e) {
        $(btnMenu).MenuFlotante({
            Menu: [
                {
                    Texto: 'Ver',
                    Icono: 'search',
                    OnClick: function () {
                        $(archivo).trigger('click');
                    }
                },
                {
                    Texto: 'Renombrar',
                    Icono: 'edit',
                    OnClick: function () {
                        crearDialogoHTML({
                            Content: '<div class="row margin-top" >' +
                                                        '<div class="col s12">' +
                                                            '<div class="input-field">' +
                                                                '<input id="input_Nombre" type="text"></input>' +
                                                                '<label for="input_Nombre" class="no-select">Nombre</label>' +
                                                            '</div>' +
                                                        '</div>' +
                                                    '</div>',
                            OnLoad: function (jAlert) {
                                $(jAlert).find('#input_Nombre').keydown(function (e) {
                                    if (e.keyCode == 13) {
                                        $(jAlert).find('.btnAceptar').trigger('click');
                                    }
                                });

                                $(jAlert).find('#input_Nombre').val(imagen.Nombre);
                                $(jAlert).find('#input_Nombre').trigger('focus');
                                Materialize.updateTextFields();
                            },
                            Botones: [
                                {
                                    Texto: 'Cancelar'
                                },
                                {
                                    Texto: 'Aceptar',
                                    Class: 'colorExito btnAceptar',
                                    CerrarDialogo: false,
                                    OnClick: function (jAlert) {
                                        var nombre = $(jAlert).find('#input_Nombre').val();
                                        if (nombre.trim() == "") {
                                            mostrarMensajeAlerta('Debe ingresar un nombre')
                                            return;
                                        }

                                        var extensionOriginal = imagen.Extension;
                                        if (!nombre.match(extensionOriginal + '$')) {
                                            mostrarMensajeAlerta('El archivo debe ser del tipo ' + extensionOriginal);
                                            return;
                                        }

                                        imagen.Nombre = nombre;
                                        $(titulo).text(nombre);
                                        $(jAlert).CerrarDialogo();
                                    }
                                }
                            ]
                        });
                    }
                },
                {
                    Texto: 'Descargar',
                    Icono: 'file_download',
                    OnClick: function () {
                        descargarArchivo(imagen);
                    }
                },
                {
                    Titulo: 'Borrar',
                    Icono: 'delete',
                    OnClick: function () {

                        //Borro el item
                        var listaNueva = [];
                        $.each(imagenes, function (index, img) {
                            if (img.Id != imagen.Id) {
                                listaNueva.push(img);
                            }
                        });
                        imagenes = listaNueva;

                        //Oculto el titulo si es que no hay mas imagenes
                        if (imagenes.length == 0) {
                            $('#contenedorTituloImagenes').stop(true, true).fadeOut(300);
                            $('#contenedorArchivosImagenes').stop(true, true).fadeOut(300);
                        }

                        //Animo la salida de la card
                        $(archivo).remove();
                    }
                }
            ]
        });

        e.stopPropagation();
    });
}


//-----------------------------
// Operaciones globales 
//-----------------------------
function validarExtension(file, esImagen) {
    if (file == undefined) return false;
    var extension = file.name.split('.').pop().toLowerCase();
    return esImagen ? (IMAGE_EXTENSIONS.indexOf(extension) > -1) : (DOCUMENT_EXTENSIONS.indexOf(extension) > -1);
}

function validarSize(file, esImagen) {
    if (file == undefined) return false;
    return (file.size / 1048576) < (esImagen ? IMAGE_SIZE : DOCUMENT_SIZE);
}

function registrar() {
    if (!validar()) return;

    mostrarCargando(true);

    buscarCercanos(getRequerimiento())
        .then(function (resultado) {
            if (resultado.Cantidad == 0) {
                registrarSinImportarCercanos();
                return;
            }

            mostrarCargando(false);
            crearDialogoRequerimientosCercanos({
                IdMotivo: getRequerimiento().IdMotivo,
                Latitud: resultado.Latitud,
                Longitud: resultado.Longitud,
                CallbackCrearSinImportarCercanos: function () {
                    registrarSinImportarCercanos();
                },
                Callback: function (id) {
                    unirseARequerimiento(id);
                }
            });
        })
        .catch(function (error) {
            mostrarCargando(false);
            mostrarMensaje('Error', error);
        });
}

function registrarSinImportarCercanos() {
    mostrarCargando(true);

    subirDocumentos(function () {
        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/Insertar'),
            Data: { comando: getRequerimiento() },
            OnSuccess: function (result) {

                if (result == undefined || result.Return == null) {
                    mostrarCargando(false);

                    mostrarMensaje('Error', result.Mensaje);
                    informarRegistroError();
                    return;
                }

                if (!result.Ok) {
                    mostrarCargando(false);

                    mostrarMensaje('Error', result.Error);
                    informarRegistroError();
                    return;
                }

                var rq = result.Return;
                var usuario = SelectorUsuario_GetUsuarioSeleccionado();

                if (usuario == undefined) {
                    mostrarCargando(false);

                    informarRegistro(rq, usuario, false);
                    return;
                }

                //Enviar mail
                var idUsuario = [];
                idUsuario.push(usuario.Id);
                var dataMail = { id: rq.Id, idsUsuarios: idUsuario, email: null };


                crearAjax({
                    Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EnviarComprobanteAtencion'),
                    Data: dataMail,
                    OnSuccess: function (result) {
                        mostrarCargando(false);

                        if (result == undefined) {
                            informarRegistro(rq, usuario, false);
                            return;
                        }

                        informarRegistro(rq, usuario, true);
                    },
                    OnError: function (result) {
                        mostrarCargando(false);
                        informarRegistro(rq, usuario, false);
                    }
                });
            },
            OnError: function (result) {
                mostrarCargando(false);
                mostrarMensaje('Error', 'Error procesando la solicitud');
                informarRegistroError();
            }
        });
    }, function () {
        mostrarCargando(false);

        mostrarMensaje('Error', 'Error subiendo alguno de sus documentos, por favor intente nuevamente');
        return;
    });
}


function unirseARequerimiento(id) {
    mostrarCargando(true);

    subirDocumentos(function () {
        var comando = getRequerimiento();
        comando.IdRequerimientoUnir = id;

        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/UnirseARequerimiento'),
            Data: { comando: comando },
            OnSuccess: function (result) {

                if (result == undefined || result.Return == null) {
                    mostrarCargando(false);

                    mostrarMensaje('Error', result.Mensaje);
                    informarRegistroError();
                    return;
                }

                if (!result.Ok) {
                    mostrarCargando(false);

                    mostrarMensaje('Error', result.Error);
                    informarRegistroError();
                    return;
                }

                var rq = result.Return;
                var usuario = SelectorUsuario_GetUsuarioSeleccionado();

                if (usuario == undefined) {
                    mostrarCargando(false);

                    informarRegistro(rq, usuario, false);
                    return;
                }

                //Enviar mail
                var dataMail = { id: rq.Id, mail: null };
                dataMail = JSON.stringify(dataMail);

                crearAjax({
                    Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EnviarComprobanteAtencion'),
                    Data: { id: rq.Id, idUsuario: SelectorUsuario_GetUsuarioSeleccionado().Id, mail: null },
                    OnSuccess: function (result) {
                        mostrarCargando(false);

                        if (result == undefined) {
                            informarRegistro(rq, usuario, false);
                            return;
                        }

                        informarRegistro(rq, usuario, true);
                    },
                    OnError: function (result) {
                        mostrarCargando(false);
                        informarRegistro(rq, usuario, false);
                    }
                });
            },
            OnError: function (result) {
                mostrarCargando(false);
                mostrarMensaje('Error', 'Error procesando la solicitud');
                informarRegistroError();
            }
        });
    }, function () {
        mostrarCargando(false);

        mostrarMensaje('Error', 'Error subiendo alguno de sus documentos, por favor intente nuevamente');
        return;
    });
}


function editar() {
    if (validar() == false) {
        return;
    }

    mostrarCargando(true);

    subirDocumentos(function () {
        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/Actualizar'),
            Data: { comando: getRequerimiento() },
            OnSuccess: function (result) {

                if (result == undefined || result.Return == null) {
                    mostrarCargando(false);

                    mostrarMensaje('Error', 'Error procesando la solicitud');
                    return;
                }

                if (!result.Ok) {
                    mostrarCargando(false);

                    mostrarMensaje('Error', result.Error);
                    return;
                }

                informarEdicion(result.Return);

            },
            OnError: function (result) {
                mostrarCargando(false);
                mostrarMensaje('Error', "Error procesando la solicitud");
            }
        });
    }, function () {
        mostrarCargando(false);

        mostrarMensaje('Error', 'Error subiendo alguno de sus documentos, por favor intente nuevamente');
        return;
    });
}

function validar() {
    $('#contenedor').find('.control-observacion').text('');
    $('#contenedor').find('.control-observacion').stop(true, true).slideUp(300);

    var resultado = true;

    //Sin Motivo
    var motivo = SelectorMotivo_GetMotivoSeleccionado();
    if (motivo == undefined) {
        $('#errorFormulario_Motivo').text('Debe seleccionar un motivo');
        $('#errorFormulario_Motivo').stop(true, true).slideDown(300);
        resultado = false;
    }

    //Sin Usuario
    var usuario = SelectorUsuario_GetUsuarioSeleccionado();
    if (usuario == undefined && motivo.Tipo!=tipoMotivoInterno) {
        $('#errorFormulario_Usuario').text('Debe seleccionar un usuario');
        $('#errorFormulario_Usuario').stop(true, true).slideDown(300);
        resultado = false;
    }

    //Valido motivo
    if (SelectorMotivo_IsDatosIngresadosSinMotivoSeleccionado()) {
        $('#errorFormulario_Motivo').text('Debe validar el motivo');
        $('#errorFormulario_Motivo').stop(true, true).slideDown(300);
        resultado = false;
    }

    //Valido ubicacion
    if (ControlDomcilioSelector_HayDomicilioSeleccionado() == undefined) {
        $('#errorFormulario_Domicilio').text('Debe seleccionar la ubicación');
        $('#errorFormulario_Domicilio').show();
        resultado = false;
    }

    //Valido usuario
    if (SelectorUsuario_IsDatosIngresadosSinUsuarioSeleccionado()) {
        $('#errorFormulario_Usuario').text('Debe validar el usuario');
        $('#errorFormulario_Usuario').stop(true, true).slideDown(300);
        resultado = false;
    }

    if (!validarCamposDinamicos(camposDinamicos)) {
        resultado = false;
    }

    //else if (SelectorUsuario_GetUsuarioSeleccionado() == undefined) {
    //    $('#errorFormulario_Usuario').text('Debe indicar un usuario');
    //    $('#errorFormulario_Usuario').stop(true, true).slideDown(300);
    //    resultado = false;
    //}

    return resultado;
}

function getRequerimiento() {
    var rq = {};

    if (idEditar != undefined) {
        rq.Id = '' + idEditar;
    }

    //Motivo
    var motivo = SelectorMotivo_GetMotivoSeleccionado();
    rq.IdMotivo = '' + motivo.Id;

    //Descripcion
    var descripcion = $('#inputFormulario_Descripcion').val();
    rq.Descripcion = descripcion.trim();

  //Ubicacion
  rq.Domicilio = ControlDomicilioSelector_GetDomicilio();

    //usuario
    var usuarioSeleccionado = SelectorUsuario_GetUsuarioSeleccionado();
    if (usuarioSeleccionado != undefined) {
        rq.IdUsuarioReferente = '' + usuarioSeleccionado.Id;
    } else {
        rq.IdUsuarioReferente = undefined;
    }

    //Notas
    rq.Notas = [];
    $.each(tablaNotas.rows().data(), function (index, element) {
        var comandoNota = {
            Id: 0,
            Contenido: element.Observaciones
        };

        rq.Notas.push(comandoNota);
    });

    //Archivos
    rq.IdArchivos = [];
    if (documentos != undefined && documentos.length != 0) {
        $.each(documentos, function (index, doc) {
            rq.IdArchivos.push(doc.Id);
        });
    }
    if (imagenes != undefined && imagenes.length != 0) {
        $.each(imagenes, function (index, doc) {
            rq.IdArchivos.push(doc.Id);
        });
    }

    rq.CamposDinamicos = getCamposDinamicosPorRequerimiento(camposDinamicos);

    
    //UserAgent
    rq.UserAgent = navigator.userAgent;

    //Tipo Cliente
    if (isMobileOrTablet()) {
        if (isMobile()) {
            rq.TipoDispositivo = 1;
        } else {
            rq.TipoDispositivo = 2;
        }
    } else {
        rq.TipoDispositivo = 3;
    }
    return rq;
}

function buscarCercanos(rq) {

    return new Promise(function (callback, callbackError) {

        let lat = rq.Domicilio.Latitud;
        let lng = rq.Domicilio.Longitud;

        if (tipoMotivo != tipoMotivoGeneral) {
            callback({ Cantidad: 0, Latitud: lat, Longitud: lng });
            return;
        }

        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetCantidadCercanos'),
            Data: {
                consulta: {
                    Default: true,
                    IdMotivo: rq.IdMotivo,
                    Latitud: lat,
                    Longitud: lng
                }
            },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback({ Cantidad: result.Return, Latitud: lat, Longitud: lng });
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
        });
    });
}

var documentosPorSubir = []

function subirDocumentos(callback, callbackError) {
    $.each(imagenes, function (index, element) {
        if (element.Id <= 0) {
            documentosPorSubir.push(element);
        }
    });
    $.each(documentos, function (index, element) {
        if (element.Id <= 0) {
            documentosPorSubir.push(element);
        }
    });

    if (documentosPorSubir.length == 0) {
        callback();
        return;
    }

    subirDocumento(callback, callbackError);
}

function subirDocumento(callback, callbackError) {
    if (documentosPorSubir == undefined || documentosPorSubir.length == 0) {
        callback();
        return;
    }

    var a = documentosPorSubir[0];
    var comando = {
        Tipo: a.Tipo,
        Nombre: a.Nombre,
        Data: a.Data
    };

    crearAjax({
        Url: ResolveUrl('~/Servicios/ArchivoService.asmx/Subir'),
        Data: { comando: comando },
        OnSuccess: function (result) {
            if (result == undefined) {
                callbackError();
                return;
            }

            if (!result.Ok) {
                callbackError();
                return;
            }

            documentosPorSubir = $.grep(documentosPorSubir, function (element, index) {
                return element.Id != a.Id;
            });

            //Actualizo el id del documento, con el que me mandaron ahora
            if (a.Tipo == 1) {
                $.each(documentos, function (index, element) {
                    if (element.Id == a.Id) {
                        documentos[index].Id = result.Return;
                    }
                });
            } else {
                $.each(imagenes, function (index, element) {
                    if (element.Id == a.Id) {
                        imagenes[index].Id = result.Return;
                    }
                });
            }


            subirDocumento(callback, callbackError);
        },
        OnError: function (result) {
            callbackError();
        }
    });
}



function listarRequerimientos(idsR) {

    var ids = idsR;

    crearDialogoIFrame({
        Titulo: 'Ya existen requerimientos aproximados, ¿Desea continuar?',
        Url: ResolveUrl('~/IFrame/IRequerimientoListado.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setIds(ids);

            //Mensjaes
            iFrameContent.setOnMensajeListener(function (tipo, mensaje) {
                callbackMensaje(tipo, mensaje);
            });

            //Cargando
            iFrameContent.setOnCargandoListener(function (cargando, mensaje) {
                $(jAlert).MostrarDialogoCargando(cargando, true);
            });

            return false;
        },
        Botones:
       [
           {
               id: 'btnGenerarMapa',
               Texto: 'Mapa',
               CerrarDialogo: false,
               OnClick: function (jAlert, iFrame, iFrameContent) {
                   iFrameContent.generarMapa();
               }
           },
           {
               Texto: 'Aceptar',
               Class: 'colorExito',
               OnClick: function (jAlert, iFrame, iFrameContent) {
                   registrar();
               }
           },
           {
               Texto: 'Cancelar'
           }
       ]
    });
}

//----------------------------
// Listener Registro
//----------------------------

function informarRegistro(requerimiento, usuario, mailEnviado) {
    if (callbackRegistrar == null) return;
    callbackRegistrar(requerimiento, usuario, mailEnviado);
}

function informarRegistroError() {
    if (callbackRegistrarError == null || callbackRegistrarError == undefined) return;
    callbackRegistrarError();
}

function setOnRegistrarCompletoListener(callback) {
    this.callbackRegistrar = callback;
}

function setOnRegistrarErrorListener(callback) {
    callbackRegistrarError = callback;
}

//----------------------------
// Listener Editar
//----------------------------

function informarEdicion(requerimiento) {
    if (callbackEditar == null) return;
    callbackEditar(requerimiento);
}

function setOnEditarCompletoListener(callback) {
    this.callbackEditar = callback;
}


//-----------------------------
// Listener Tab Change
//-----------------------------

function informarTabChange() {
    if (callbackTab == undefined) return;

    var tab = $('.tabs .active');
    var id = tab.attr('href');
    callbackTab(id);
}

function setOnTabChangeListener(callback) {
    this.callbackTab = callback;
}

//------------------------------
// Tipo Motivo
//------------------------------
var tipoMotivo= null;
function setTipoMotivo(t) {
    tipoMotivo= t;
}