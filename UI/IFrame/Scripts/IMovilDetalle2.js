var movil;

var PATH_IMAGEN_USUARIO_DEFAULT;

var panelCallback;

var estadosOcupado;

function init(data) {
    if ('Error' in data) {
        mostrarError(data.Error);
        return;
    }

    PATH_IMAGEN_USUARIO_DEFAULT = ResolveUrl('~/Resources/Imagenes/user-avatar.png')

    movil = data.Movil;
    estadosOcupado = data.EstadosOcupado;

    console.log(movil);

    initEncabezado();
    initAlertas();
    initPanelDeslizable();

    initNotas();
    initCaracteristicas();
    initReparaciones();
    initInformacionAdicional();
    cargarDatos();
}

function cargarDatos() {
    cargarAcciones();
    cargarDatosEncabezado();
    cargarValuacionYKilometraje();
    cargarCaracteristicas();
    cargarNotas();
    cargarInformacionAdicional();

    if (validarPermisoModificacion('Moviles')) {
        cargarITVTUV();
        cargarReparaciones();
    }
}

//Encabezado
function initEncabezado() {
    $('#btn_Acciones').click(function () {
        $('#contenedor_Acciones .contenido').toggleClass('visible');
        $(this).text($('#contenedor_Acciones .contenido').hasClass('visible') ? 'Ocultar acciones' : 'Ver acciones');
    });
}

function cargarDatosEncabezado() {
    //Marca Modelo Año
    var titulo = movil.Marca + ' ' + movil.Modelo + ' ';
    if (movil.Año != undefined && movil.Año != "") {
        titulo += "(" + movil.Año + ")";
    }
    $('#texto_Titulo').text(titulo);

    //Numero Interno
    if (movil.NumeroInterno == undefined || movil.NumeroInterno == "") {
        $('#texto_NumeroInterno').hide();
    } else {
        $('#texto_NumeroInterno').html('<b>Número interno </b>' + toTitleCase(movil.NumeroInterno));
    }

    //Dominio
    if (movil.Dominio == undefined || movil.Dominio == "") {
        $('#texto_Dominio').html('<b>Motivo </b>Sin datos');
    } else {
        $('#texto_Dominio').html('<b>Dominio </b>' + toTitleCase(movil.Dominio));
    }

    //Area
    if (movil.NombreArea == undefined || movil.NombreArea == "") {
        $('#texto_Area').html('<b>Área </b>Sin datos');
    } else {
        $('#texto_Area').html('<b>Área </b>' + toTitleCase(movil.NombreArea));
    }

    //Estado
    $('#texto_IndicadorEstado').html('<b>Estado</b> ' + toTitleCase(movil.NombreEstado));
    $('#icono_IndicadorEstado').css('color', '#' + movil.ColorEstado);

    //Condicion
    if (movil.NombreCondicion == undefined || movil.NombreCondicion == "") {
        $('#texto_IndicadorCondicion').html('<b>Condición </b> Sin datos');
        $('#icono_IndicadorCondicion').css('color', 'var(--colorIndicadorApagado)');
    } else {
        $('#texto_IndicadorCondicion').html('<b>Condición </b>' + movil.NombreCondicion);
        $('#icono_IndicadorCondicion').css('color', 'var(--colorTexto_ContenedorTitulo)');
    }

    //Tipo de movil
    if (movil.NombreTipo == undefined || movil.NombreTipo == "") {
        $('#texto_IndicadorTipo').html('<b>Tipo </b> Sin datos');
        $('#icono_IndicadorTipo').css('color', 'var(--colorIndicadorApagado)');
    } else {
        $('#texto_IndicadorTipo').html('<b>Tipo </b>' + movil.NombreTipo);
        $('#icono_IndicadorTipo').css('color', 'var(--colorTexto_ContenedorTitulo)');
    }

    //Carga
    if (movil.Carga == undefined || movil.Carga == "") {
        $('#texto_IndicadorCarga').html('<b>Carga </b> Sin datos');
        $('#icono_IndicadorCarga').css('color', 'var(--colorIndicadorApagado)');
    } else {
        $('#texto_IndicadorCarga').html('<b>Carga </b>' + movil.Carga);
        $('#icono_IndicadorCarga').css('color', 'var(--colorTexto_ContenedorTitulo)');
    }

    //Carga
    if (movil.Asientos == undefined || movil.Asientos == "") {
        $('#texto_IndicadorAsientos').html('<b>Asientos </b> Sin datos');
        $('#icono_IndicadorAsientos').css('color', 'var(--colorIndicadorApagado)');
    } else {
        $('#texto_IndicadorAsientos').html('<b>Asientos </b>' + movil.Asientos);
        $('#icono_IndicadorAsientos').css('color', 'var(--colorTexto_ContenedorTitulo)');
    }

    //Tipo de combustible
    if (movil.NombreTipoCombustible == undefined || movil.NombreTipoCombustible == "") {
        $('#texto_IndicadorTipoCombustible').html('<b>Combustible </b> Sin datos');
        $('#icono_IndicadorTipoCombustible').css('color', 'var(--colorIndicadorApagado)');
    } else {
        $('#texto_IndicadorTipoCombustible').html('<b></b>' + movil.NombreTipoCombustible);
        $('#icono_IndicadorTipoCombustible').css('color', 'var(--colorTexto_ContenedorTitulo)');
    }
}

//Estados
function mostrarHistorialDeEstados() {

    abrirPanelDeslizable('Historial de Estados');

    var divHistorialEstados = $($('#template_HistorialEstados').html());
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(divHistorialEstados);

    $.each(movil.Estados, function (index, element) {
        var div = $($('#template_HistorialEstadoItem').html());
        if (index == 0) {
            $(div).find('.linea1').css({ opacity: 0 });
        }
        if (index == movil.Estados.length - 1) {
            $(div).find('.linea2').css({ opacity: 0 });
        }
        $(div).find('.circulo').css({ 'background-color': '#' + element.EstadoColor });
        $(div).find('.nombre').text(toTitleCase(element.EstadoNombre));
        $(div).find('.motivo').text(element.EstadoObservaciones);
        $(div).find('.nombrePersona').html('<b>' + toTitleCase(element.UsuarioNombre + ' ' + element.UsuarioApellido) + '</b>');
        $(div).find('.nombrePersona').click(function () {
            crearDialogoUsuarioDetalle({
                Id: element.UsuarioId
            });
        });


        $(div).find('.fecha').html(' el <b>' + dateTimeToString(element.EstadoFecha) + '</b>');

        $(divHistorialEstados).append(div);
    });
}

//Alertas
function initAlertas() {

}

function mostrarAlertaVencimientoITV(diferenciaDias) {
    var texto = "El ITV vence ";
    if (diferenciaDias < 0) {
        texto = "El ITV venció ";
    }

    var div = $('#template_Alerta').html();
    div = $(div);

    $(div).attr('id', 'alertaVencimientoITV');

    $(div).addClass("naranja");
    $(div).empty();

    var divEncabezado = $('<div class="encabezado"><label class="primerTexto"></label><label class="fechaVencimientoITV"></label></div>');
    $(divEncabezado).appendTo($(div));

    $(divEncabezado).find('.primerTexto').text(texto);

    var textoFecha = " el " + dateToString(movil.FechaVencimientoITV);
    if (diferenciaDias == 0) {
        textoFecha = "HOY";
    } else if (diferenciaDias ==1) {
        textoFecha = "MAÑANA";
    }
    $(divEncabezado).find('.fechaVencimientoITV').html('<b>' + textoFecha + '</b>');

    $(div).appendTo('#contenedor_Alertas');
}

function mostrarAlertaVencimientoTUV(diferenciaDias) {
    var clase = "naranja";
    var texto = "El TUV vence ";
    if (diferenciaDias < 0) {
        //clase = "rojo";
        texto = "El TUV venció ";
    }

    var div = $('#template_Alerta').html();
    div = $(div);

    $(div).attr('id', 'alertaVencimientoTUV');

    $(div).addClass(clase);
    $(div).empty();

    var divEncabezado = $('<div class="encabezado"><label class="primerTexto"></label><label class="fechaVencimientoTUV"></label></div>');
    $(divEncabezado).appendTo($(div));

    $(divEncabezado).find('.primerTexto').text(texto);

    var textoFecha = " el " + dateToString(movil.FechaVencimientoTUV);
    if (diferenciaDias == 0) {
        textoFecha = "HOY";
    }
    $(divEncabezado).find('.fechaVencimientoTUV').html('<b>' + textoFecha + '</b>');

    $(div).appendTo('#contenedor_Alertas');
}

//Panel Deslizable
function initPanelDeslizable() {
    $('#btn_CerrarPanelDeslizable').click(function () {
        cerrarPanelDeslizable();
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 27) {
            if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                $('#btn_CerrarPanelDeslizable').trigger('click');
            }
        }
    });
}

function abrirPanelDeslizable(titulo) {
    $('#contenedor_PanelDeslizable').addClass('visible');

    $('#contenedor_PanelDeslizable .encabezado .titulo').text(titulo);

    $('#contenedor_PanelDeslizable .contenedor_Contenido').scrollTop(0)
    $('#contenedor_PanelDeslizable .contenedor_Contenido').empty();

    panelAbierto(true)
}

function cerrarPanelDeslizable() {
    $('#contenedor_PanelDeslizable .encabezado .titulo').text('');
    $('#contenedor_PanelDeslizable').removeClass('visible');
    panelAbierto(false)
}

//Caracteristicas
function initCaracteristicas() {

}

function cargarCaracteristicas() {
    var caracteristicas = movil.Caracteristicas;
    if (caracteristicas == undefined || caracteristicas.trim() == "") {
        $('#contenedor_SeccionCaracteristicas').hide();
        return;
    }

    $('#contenedor_SeccionCaracteristicas').show();
    $('#texto_Caracteristicas').text(caracteristicas);

}

function cargarValuacionYKilometraje() {
    $("#contenedor_ValuacionKilometraje").hide();
    cargarValuacion();
    cargarKilometraje();
}

//Valuacion
function cargarValuacion() {
    var valuacion = movil.Valuacion;
    if (valuacion == undefined || valuacion == 0) {
        $('#contenedor_Valuacion').hide();
    } else {
        $('#contenedor_Valuacion').show();
        $("#contenedor_ValuacionKilometraje").show();
        $("#contenedor_Valuacion .textoValuacion").html('<b>$' + valuacion + '</b>');
        $("#contenedor_Valuacion .textoFechaValuacion").html('<b>' + dateToString(movil.FechaValuacion) + '</b>')

        if (movil.ObservacionesValuacion != undefined || movil.ObservacionesValuacion != "" || movil.ObservacionesValuacion != null) {
            $("#contenedor_Valuacion .textoObservacionesValuacion").html("<br>Observaciones: " + movil.ObservacionesValuacion + "</br>");
        }
    }
}

//Kilometraje
function cargarKilometraje() {
    var kilometraje = movil.Kilometraje;
    if (kilometraje == undefined || kilometraje == 0) {
        $('#contenedor_Kilometraje').hide();
    } else {
        $('#contenedor_Kilometraje').show();
        $("#contenedor_ValuacionKilometraje").show();
        $("#contenedor_Kilometraje .textoKilometraje").html('<b>' + kilometraje + '</b>');
        $("#contenedor_Kilometraje .textoFechaKilometraje").html('<b>' + dateToString(movil.FechaKilometraje) + '</b>');

        if (movil.ObservacionesKilometraje != undefined || movil.ObservacionesKilometraje != "") {
            $("#contenedor_Kilometraje .textoObservacionesKilometraje").html("<br>Observaciones: " + movil.ObservacionesKilometraje + "</br>");
        }
    }
}

//ITV Y TUV
function cargarITVTUV() {
    $("#contenedor_ITVTUV").hide();
    cargarITV();
    cargarTUV();
}

//ITV
function cargarITV() {
    //borro alerta de itv si la hubiere
    if ($("#alertaVencimientoITV").length > 0) {
        $('#alertaVencimientoITV').remove();
    }

    var fechaVencimientoITV = movil.FechaVencimientoITV;
    if (fechaVencimientoITV == undefined) {
        $('#contenedor_ITV').hide();
    } else {
        $('#contenedor_ITV').show();
        $("#contenedor_ITVTUV").show();
        var fechaVencimientoString = dateToString(movil.FechaVencimientoITV);
        $("#contenedor_ITV .textoFechaVencimientoITV").html('<b>' + fechaVencimientoString + '</b>');

        if (validarFechaMenorQueHoy(fechaVencimientoString)) {
            $(".textoConectorITV").text(" fue el ");
        }

        var diferenciaDias = calcularDiferenciaDias(fechaVencimientoString);
        if (diferenciaDias < 15) {
            mostrarAlertaVencimientoITV(diferenciaDias);
        }

        if (movil.ObservacionesITV != undefined && movil.ObservacionesITV != "") {
            $("#contenedor_ITV .textoObservacionesITV").html("Observaciones: " + movil.ObservacionesITV);
        }

        if (movil.FechaUltimoITV == undefined) {
            $("#contenedor_ITV .textoUltimoITV").hide();
            $("#contenedor_ITV .textoFechaUltimoITV").hide();
        }

        $("#contenedor_ITV .textoFechaUltimoITV").html(dateToString(movil.FechaUltimoITV));
    }
}

//TUV
function cargarTUV() {
    //borro alerta de itv si la hubiere
    if ($("#alertaVencimientoTUV").length > 0) {
        $('#alertaVencimientoTUV').remove();
    }

    var fechaVencimientoTUV = movil.FechaVencimientoTUV;
    if (fechaVencimientoTUV == undefined) {
        $('#contenedor_TUV').hide();
    } else {
        $("#contenedor_TUV .textoFechaVencimientoTUV").html('<b>' + dateToString(movil.FechaVencimientoTUV) + '</b>');
        $('#contenedor_TUV').show();
        $("#contenedor_ITVTUV").show();
        var fechaVencimientoString = dateToString(movil.FechaVencimientoTUV);

        if (validarFechaMenorQueHoy(fechaVencimientoString)) {
            $(".textoConectorTUV").text(" fue el ");
        }

        var diferenciaDias = calcularDiferenciaDias(fechaVencimientoString);
        if (diferenciaDias < 15) {
            mostrarAlertaVencimientoTUV(diferenciaDias);
        }

        if (movil.ObservacionesTUV != undefined && movil.ObservacionesTUV != "") {
            $("#contenedor_TUV .textoObservacionesTUV").html("Observaciones: " + movil.ObservacionesTUV);
        }

        if (movil.FechaUltimoTUV == undefined) {
            $("#contenedor_TUV .textoUltimoTUV").hide();
            $("#contenedor_TUV .textoFechaUltimoTUV").hide();
        }

        $("#contenedor_TUV .textoFechaUltimoTUV").html(dateToString(movil.FechaUltimoTUV));
    }
}

//Notas
function initNotas() {
    $('#contenedor_Notas .verMas').click(function () {
        mostrarTodasLasNotas();
    });

    $('#btn_AgregarNota').click(function () {
        agregarNota();
    });
}

function cargarNotas() {
    $('#contenedor_Notas .contenido .items').empty();

    if (movil.Notas == undefined || movil.Notas.length == 0) {
        $('#contenedor_Notas .sinItems').show();
        $('#contenedor_Notas .verMas').hide();
        $('#contenedor_Notas .contenido').hide();

    } else {
        var itemsMostrados = 0;
        $.each(movil.Notas, function (index, element) {
            if (!element.Visto) {
                itemsMostrados = itemsMostrados + 1;
                var div = crearHtmlNota(element);
                $('#contenedor_Notas .contenido .items').append(div);
            }
        });

        $("#contenedor_Notas .titulo").text('Notas sin visar');
        if (itemsMostrados == 0) {
            itemsMostrados = 1;
            $("#contenedor_Notas .titulo").text('Notas');
            var div = crearHtmlNota(movil.Notas[0]);
            $('#contenedor_Notas .contenido .items').append(div);
        }

        $('#contenedor_Notas .sinItems').hide();
        $('#contenedor_Notas .contenido').show();

        if (movil.Notas.length > itemsMostrados) {
            $('#contenedor_Notas .verMas').show();
        } else {
            $('#contenedor_Notas .verMas').hide();
        }
    }
}

function crearHtmlNota(data) {
    var div = $('#template_Nota').html();
    div = $(div);
    $(div).find('.persona img').attr('src', PATH_IMAGEN_USUARIO_DEFAULT);
    $(div).find('.persona label').text(data.UsuarioCreadorNombre + ' ' + data.UsuarioCreadorApellido);
    $(div).find('.card .contenido').text(data.Contenido);
    $(div).find('.card .fecha').text('Creada el ' + dateToString(data.FechaAlta));

    $(div).find('.persona label').click(function () {
        crearDialogoUsuarioDetalle({
            Id: data.UsuarioCreadorId
        });
    });

    if (data.Visto) {
        $(div).find('.vistoPor').show();
        $(div).find('.vistoPor .personaVisto').text(data.UsuarioVistoNombre + ' ' + data.UsuarioVistoApellido)

        $(div).find('.vistoPor label').click(function () {
            crearDialogoUsuarioDetalle({
                Id: data.UsuarioVistoId
            });
        });
    }

    if (validarPermisoModificacion('Moviles')) {
        var texto = 'Visar';
        var icono = 'check';

        if (data.Visto) {
            texto = 'Quitar visado';
            icono = 'undo'
        }
        agregarBotonVisar(div, {
            Texto: texto,
            Icono: icono,
            OnClick: function () {
                visarNota(data);
            }
        });
    }

    return div;
}

function mostrarTodasLasNotas() {
    abrirPanelDeslizable('Todas las Notas');

    var div = $($('#template_NotasDetalle').html());
    $(div).attr('id', 'contenedor_NotasDetalle');
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(div);
    $('#contenedor_NotasDetalle .contenido').empty();

    //Cargo las notas
    $.each(movil.Notas, function (index, element) {
        var div = crearHtmlNota(element);
        $('#contenedor_NotasDetalle').append(div);
    });

    //BotonAgregar
    $('#contenedor_PanelDeslizable .btnNuevoNota').click(function () {
        let nota = $('#contenedor_PanelDeslizable input').val();
        if (nota == undefined || nota == "") {
            mostrarMensaje('Alerta', 'Ingrese el contenido de la nota interna');
            $('#contenedor_PanelDeslizable input').focus();
            return;
        }

        procesarAgregarNota(nota);
    });
}

//Reparaciones
function initReparaciones() {
    $('#contenedor_Reparaciones > .contenido > .verMas').click(function () {
        mostrarTodasLasReparaciones();
    });
}

function cargarReparaciones() {
    $('#contenedor_Reparaciones > .contenido > .item').empty();

    if (!('Reparaciones' in movil) || movil.Reparaciones.length == 0) {
        $('#contenedor_Reparaciones').hide();
        $('#contenedor_Reparaciones .verMas').hide();
        return;
    }

    $('#contenedor_Reparaciones').show();
    $('#contenedor_Reparaciones .verMas').show();

    if (movil.Reparaciones.length == 1) {
        $('#contenedor_Reparaciones .verMas').hide();
    }

    let rep = movil.Reparaciones[0];

    let html_rep = crearHtmlReparacion(rep);
    $('#contenedor_Reparaciones > .contenido > .item').append(html_rep);
}

function crearHtmlReparacion(rep) {
    var html = $($('#template_Reparacion').html());

    let motivoFecha = rep.Motivo;

    if (rep.FechaReparacion != undefined) {
        motivoFecha += ' (' + dateToString(rep.FechaReparacion) + ')';
    }

    $(html).find('.textos > .linea1 > .motivo ').html('<b>' + motivoFecha + '</b>');

    if (rep.Taller != "" & rep.Taller != null) {
        $(html).find(' .textos > .linea2 > .taller').html('Taller: ' + '<b>' + rep.Taller + '</b>');
    }

    if (rep.MontoReparacion != 0 && rep.MontoReparacion != null) {
        $(html).find(' .textos > .linea2 > .monto').html('Monto: ' + '<b>$' + rep.MontoReparacion + '</b>');
    }

    if (rep.Observaciones != "" && rep.Observaciones != null) {
        $(html).find('.textos > .linea3  > .observaciones').text('Observaciones: ' + rep.Observaciones);
    }

    //Boton borrar
    $(html).find('.botones .borrar').click(function () {
        if (rep.Id == undefined) return;
        borrarReparacion(rep.Id);
    });

    return html;
}

function borrarReparacion(id) {

    crearDialogoHTML({
        Titulo: '<label>Borrar Reparación</label>',
        Width: 600,
        Content:
            '<div class="padding">' +
            '<label id="textoConfirmacion"class="titulo">¿Está seguro de borrar esta reparación?</label>' +
            '</div>',
        Botones:
            [
                {
                    Id: 'btnNo',
                    Texto: 'No'
                },
                {
                    Id: 'btnSi',
                    Texto: 'Si',
                    Class: 'colorExito',
                    CerrarDialogo: false,
                    OnClick: function (jAlert) {

                        //Muestro el cargando
                        // mostrarCargando(true);
                        $(jAlert).MostrarDialogoCargando(true);

                        var urlAjax = ResolveUrl('~/Servicios/MovilService.asmx/BorrarReparacion');

                        var comando = { Id: id };
                        crearAjax({
                            Url: urlAjax,
                            Data: { comando: comando },
                            OnSuccess: function (result) {
                                //Oculto el cargando
                                $(jAlert).MostrarDialogoCargando(false);
                                mostrarCargando(false);

                                if (!result.Ok) {
                                    mostrarMensaje('Error', result.Error);
                                    $(jAlert).CerrarDialogo();
                                    return;
                                }

                                //Muestro el mensaje de OK
                                mostrarMensaje('Exito', 'Reparación borrada correctamente');

                                actualizarDetalle(function () {
                                    cerrarPanelDeslizable();
                                    cargarReparaciones();
                                    cargarInformacionAdicional();
                                    $(jAlert).CerrarDialogo();
                                });
                            },
                            OnError: function (result) {
                                //Oculto el cargando
                                mostrarCargando(false);

                                //Muestro el Error
                                mostrarMensaje('Error', 'Error borrando la reparación');
                                $(jAlert).CerrarDialogo();
                            }
                        })

                    }
                }]
    });
}

function mostrarTodasLasReparaciones() {
    abrirPanelDeslizable('Todas las Reparaciones');

    var div = $($('#template_ReparacionesDetalle').html());
    $(div).attr('id', 'contenedor_ReparacionesDetalle');
    $('#contenedor_PanelDeslizable .contenedor_Contenido').append(div);
    $('#contenedor_ReparacionesDetalle .contenido').empty();

    //Cargo las Reparaciones
    $.each(movil.Reparaciones, function (index, element) {
        var div = crearHtmlReparacion(element);
        $('#contenedor_ReparacionesDetalle').append(div);
    });
}

//Informacion adicional
function initInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoUsuarioCreador').click(function () {
        crearDialogoUsuarioDetalle({
            Id: movil.UsuarioCreadorId
        });
    });

    $('#contenedor_InfoAdicional .textoUsuarioModificacion').click(function () {
        crearDialogoUsuarioDetalle({
            Id: movil.UsuarioModificacionId
        });
    });
}

function cargarInformacionAdicional() {
    $('#contenedor_InfoAdicional .textoFechaCreacion').html('<b>' + dateTimeToString(movil.FechaAlta) + '</b>');

    if (movil.FechaIncorporacion != undefined) {
        $('#contenedor_InfoAdicional .linea1').show();
        var fechaIncorporacion = dateToString(movil.FechaIncorporacion)
        $('#contenedor_InfoAdicional .textoFechaIncorporacion').html('<b>' + fechaIncorporacion + '</b>');
    } else {
        $('#contenedor_InfoAdicional .linea1').hide();
    }

    if (movil.UsuarioCreadorNombre != undefined) {
        $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').show();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').show();
        var usuarioCreador = toTitleCase(movil.UsuarioCreadorNombre + ' ' + movil.UsuarioCreadorApellido).trim();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').html('<b>' + usuarioCreador + '</b>');
    } else {
        $('#contenedor_InfoAdicional .textoUsuarioCreadorConector').hide();
        $('#contenedor_InfoAdicional .textoUsuarioCreador').hide();
    }

    if (movil.FechaModificacion != undefined) {
        $('#contenedor_InfoAdicional .linea3').show();
        $('#contenedor_InfoAdicional .textoFechaModificacion').html('<b>' + dateTimeToString(movil.FechaModificacion) + '</b>');
        if (movil.UsuarioModificacionNombre != undefined && movil.UsuarioModificacionNombre.trim() != "") {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').show();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').html('<b>' + toTitleCase(movil.UsuarioModificacionNombre + ' ' + movil.UsuarioModificacionApellido) + '</b>');
        } else {
            $('#contenedor_InfoAdicional .textoUsuarioModificacionConector').hide();
            $('#contenedor_InfoAdicional .textoUsuarioModificacion').hide();
        }

    } else {
        $('#contenedor_InfoAdicional .linea3').hide();
    }
}

//Acciones
function cargarAcciones() {
    $('#contenedor_Acciones .contenido').empty();

    var modificacion = validarPermisoModificacion('Moviles') && movil.FechaBaja == null;

    if (modificacion ) {
        //Información básica
        agregarAccion({
            Texto: 'Editar información básica',
            Icono: 'edit',
            OnClick: function () {
                editarInformacionBasica();
            }
        });

        //Editar caracteristicas
        agregarAccion({
            Texto: 'Editar características',
            Icono: 'comment',
            OnClick: function () {
                editarCaracteristicas();
            }
        });

        //Estado
        agregarAccion({
            Texto: 'Cambiar Estado',
            Permiso: validarEstadoParaCambiarEstado(),
            Icono: 'swap_vert',
            OnClick: function () {
                cambiarEstado();
            }
        });
    }


    //Historico
    agregarAccion({
        Texto: 'Historial de estados',
        Icono: 'history',
        OnClick: function () {
            mostrarHistorialDeEstados();
        }
    });

    if (modificacion) {
        //Condición
        agregarAccion({
            Texto: 'Editar condición',
            Icono: 'opacity',
            OnClick: function () {
                editarCondicion();
            }
        });

        //Valuación
        agregarAccion({
            Texto: 'Editar valuación',
            Icono: 'attach_money',
            OnClick: function () {
                editarValuacion();
            }
        });

        //Kilometraje
        agregarAccion({
            Texto: 'Editar kilometraje',
            Icono: 'map',
            OnClick: function () {
                editarKilometraje();
            }
        });

        //ITV
        agregarAccion({
            Texto: 'Editar ITV',
            Icono: 'outlined_flag',
            OnClick: function () {
                editarITV();
            }
        });

        //TUV
        agregarAccion({
            Texto: 'Editar TUV',
            Icono: 'outlined_flag',
            OnClick: function () {
                editarTUV();
            }
        });

        //Reparacion
        agregarAccion({
            Texto: 'Agregar Reparación',
            Icono: 'healing',
            OnClick: function () {
                agregarReparacion();
            }
        });

        if (movil.Reparaciones.length > 1) {
            //Historial de Reparaciones
            agregarAccion({
                Texto: 'Histórico de reparaciones',
                Icono: 'history',
                OnClick: function () {
                    mostrarTodasLasReparaciones();
                }
            });
        }
    }

    //Nota
    agregarAccion({
        Texto: 'Agregar nota',
        Icono: 'comment',
        OnClick: function () {
            agregarNota();
        }
    });

    if (modificacion) {

        //Dar de baja
        agregarAccion({
            Texto: 'Dar de baja',
            Icono: 'close',
            OnClick: function () {
                darDeBaja();
            }
        });
    }

}

function agregarAccion(valores) {
    var div = $($('#template_Accion').html());
    $(div).find('.texto').text(valores.Texto);
    $(div).find('.icono').text(valores.Icono);
    $('#contenedor_Acciones .contenido').append(div);
    if (('Permiso' in valores) && !valores.Permiso) {
        $(div).addClass('deshabilitado');
    }
    $(div).click(function () {
        valores.OnClick();
    });
}

function agregarBotonVisar(divPadre, valores) {
    var div = $($('#template_Accion').html());
    $(div).find('.texto').text(valores.Texto);
    $(div).find('.icono').text(valores.Icono);
    $(divPadre).append(div);
    $(div).click(function () {
        valores.OnClick();
    });
}

function editarInformacionBasica() {
    crearDialogoMovilEditarInformacionBasica({
        Id: movil.Id,
        Callback: function () {
            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
                $(jAlert).CerrarDialogo();
            });
        }
    });
}

function editarCaracteristicas() {
    let placeholder = movil.Caracteristicas;
    if (placeholder == null) placeholder = "";
    crearDialogoInput({
        Titulo: 'Editar caracteristicas',
        Placeholder: 'Caracteristicas',
        Valor: placeholder,
        OnLoad: function (jAlert) {
            let input = $(jAlert).find('input');
            $(input).focus();
        },
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let input = $(jAlert).find('input');
                    let caracteristicas = input.val();
                    caracteristicas = caracteristicas.trim();

                    var url = ResolveUrl('~/Servicios/MovilService.asmx/EditarCaracteristicas');

                    crearAjax({
                        Url: url,
                        Data: { comando: { IdMovil: movil.Id, Caracteristicas: caracteristicas } },
                        OnSuccess: function (result) {
                            if (!result.Ok) {
                                mostrarMensaje('Error', result.Error);
                                return;
                            }

                            mostrarMensaje('Exito', 'Características editadas correctamente');

                            actualizarDetalle(function () {
                                cargarCaracteristicas();
                                cargarInformacionAdicional();
                                $(jAlert).CerrarDialogo();
                            });
                        },
                        OnError: function (result) {
                            mostrarMensaje('Error', 'Error procesando la solicitud');
                        }
                    });
                }
            },

        ]
    });
}

function editarCondicion() {
    crearDialogoMovilEditarCondicion({
        Id: movil.Id,
        IdCondicion: movil.IdCondicion,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        }
    });
}

function editarValuacion() {
    crearDialogoMovilEditarValuacion({
        Id: movil.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarValuacionYKilometraje();
                cargarInformacionAdicional();
            });
        }
    });
}

function editarKilometraje() {
    crearDialogoMovilEditarKilometraje({
        Id: movil.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarValuacionYKilometraje();
                cargarInformacionAdicional();
            });
        }
    });
}

function editarITV() {
    crearDialogoMovilEditarITV({
        Id: movil.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarITVTUV();
                cargarInformacionAdicional();
            });
        }
    });
}

function editarTUV() {
    crearDialogoMovilEditarTUV({
        Id: movil.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarTUV();
                cargarInformacionAdicional();
            });
        }
    });
}

function cambiarEstado() {
    if (!validarEstadoParaCambiarEstado()) {
        mostrarMensaje('Error', 'El empleado no se encuentra en un estado válido para realizar esta accion');
        return;
    }

    crearDialogoMovilCambiarEstado({
        Id: movil.Id,
        IdEstadoAnterior: movil.IdEstado,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarDatosEncabezado();
                cargarInformacionAdicional();
            });
        }
    });
}

function agregarNota() {
    crearDialogoInput({
        Titulo: 'Nueva nota ',
        Placeholder: 'Nota...',
        Botones: [
            {
                Texto: 'Cancelar'
            },
            {
                Texto: 'Guardar',
                Class: 'colorExito',
                CerrarDialogo: false,
                OnClick: function (jAlert) {
                    let input = $(jAlert).find('input');
                    let nota = input.val();
                    if (nota == "") {
                        $(jAlert).find('input').focus();
                        mostrarMensaje('Alerta', 'Ingrese el contenido de la nota');
                        return;
                    }
                    $(jAlert).CerrarDialogo();
                    procesarAgregarNota(nota);
                }
            },

        ]
    });
}

function visarNota(nota) {
    var visar = !nota.Visto;
    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/VisarNota'),
        Data: { comando: { IdNota: nota.Id, IdMovil: movil.Id, Visar: visar } },
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', 'Nota editada correctamente');

            actualizarDetalle(function () {
                cargarNotas();
                cargarInformacionAdicional();

                if (!visar) {
                    cerrarPanelDeslizable();
                }

                if ($('#contenedor_PanelDeslizable').hasClass('visible')) {
                    mostrarTodasLasNotas();
                }

            });
        },
        OnError: function (result) {
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function procesarAgregarNota(nota) {
    var url = ResolveUrl('~/Servicios/MovilService.asmx/AgregarNota');
    var data = {
        comando: {
            IdMovil: movil.Id,
            Contenido: nota
        }
    };

    mostrarCargando(true);
    crearAjax({
        Url: url,
        Data: data,
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', 'Nota interna agregada');

            actualizarDetalle(function () {
                cargarNotas();

                if ($('#contenedor_PanelDeslizable').hasClass('visible') && $('#contenedor_PanelDeslizable').length != 0) {
                    mostrarTodasLasNotas();
                }
            });
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function agregarReparacion() {
    crearDialogoMovilAgregarReparacion({
        Id: movil.Id,
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        Callback: function () {
            actualizarDetalle(function () {
                cargarReparaciones();
                cargarInformacionAdicional();
            });
        }
    });
}

function darDeBaja() {
    crearDialogoConfirmacion({
        Texto: '¿Esta seguro de que quiere dar de baja el móvil de su área?',
        ClassBotonAceptar: 'colorExito',
        CallbackPositivo: function () {
            crearAjax({
                Url: ResolveUrl('~/Servicios/MovilService.asmx/DarDeBaja'),
                Data: { comando:{Id: movil.Id }},
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        mostrarMensaje('Error', result.Error);
                        return;
                    }

                    actualizarDetalle(function () {
                        cargarInformacionAdicional()
                        cargarAcciones();
                    });
                },
                OnError: function (result) {
                    mostrarMensaje('Error', result.Error);
                    return;
                }
            });
        }
    });
}

function validarEstadoParaCambiarEstado() {
    return !_.some(estadosOcupado, function (est) { return movil.IdEstado == est.Id });
}

//Utiles
function mostrarError(error) {
    mostrarMensajeCritico({ Icono: 'error_outline', Titulo: error })
}

function actualizarDetalle(callback) {
    mostrarCargando(true);
    crearAjax({
        Url: ResolveUrl('~/Servicios/MovilService.asmx/GetDetalleById'),
        Data: { id: movil.Id },
        OnSuccess: function (result) {
            mostrarCargando(false);
            if (!result.Ok) {
                mostrarError(result.Error);
                return;
            }

            movil = result.Return;
            if (callback != undefined)
                callback();
            cargarInformacionAdicional();
            cargarAcciones();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error procesando la solicitud');
        }
    });
}

function calcularDiferenciaDias(fecha) {
    var fechaVencimiento = moment(fecha, "DD/MM/YYYY", true);
    var hoy = moment(new Date(), "DD/MM/YYYY", true);

    return fechaVencimiento.startOf('day').diff(hoy.startOf('day'), 'days')
}

//Listener panel
function setOnPanelAbiertoListener(callback) {
    panelCallback = callback;
}

function panelAbierto(abierto) {
    if (panelCallback != undefined) {
        panelCallback(abierto);
    }
}