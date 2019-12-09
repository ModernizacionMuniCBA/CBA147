$(function () {
    $('.btn-volver').click(function () {
        redirigir('Inicio');
    });
})

var estados = [];
var rqs = [];
// Init
function init(data) {
    if (verificarError(data) == true) return;

    rqs = data.Requerimientos;

    if (rqs.length != 0) {
        $('#content_SinRequerimientos').hide();
        $("#contenedor_Filtros").show();
        initRequerimientos(rqs);
    }

    initRequerimientosEvent();

    let idsEstados = _.uniq(_.pluck(rqs, 'EstadoId'));
    _.each(idsEstados, function (idEstado, i) {
        var objeto = _.findWhere(rqs, { 'EstadoId': idEstado });
        var e = {};
        e.Id = objeto.EstadoId;
        e.Nombre = objeto.EstadoPublicoNombre || objeto.EstadoNombre || 'Sin datos'
        e.Color = objeto.EstadoPublicoColor || objeto.EstadoColor || '000'
        estados.push(e);
    })

    initFiltrosEstados()

    $('.btn-volver').click(function () {
        redirigir('Inicio');
    });

    $('#inputBusqueda').keyup(function () {
        filtrar();
    })
}

function initFiltrosEstados() {
    _.each(estados, function (estado, i) {
        var div = crearHtmlEstado(estado);
        $("#contenedor_Filtros .filtros_Estados").append(div);
    })
}

function crearHtmlEstado(estado) {
    var div = $($('#template_Estado').html());

    $(div).attr('id', estado.Id);

    $(div).find('.texto').text(estado.Nombre);

    $(div).find('i').css('color', '#' + estado.Color);

    $(div).click(function () {
        if ($(div).find('.estado').hasClass('activo')) {
            $(div).find('.estado').removeClass('activo');
        } else {
            $(div).find('.estado').addClass('activo');
        }

        filtrar();
    });

    return div;
}

function filtrar() {
    let textoFiltro = $("#inputBusqueda").val();
    var requerimientosFiltrados = rqs;
    if (textoFiltro != "") {
        requerimientosFiltrados = _.filter(rqs, function (r) {
            if (r.Numero.toLowerCase().indexOf(textoFiltro.toLowerCase()) >= 0) {
                return true;
            }

            return false;
        })
    }
    let estadosFiltrados = [];
    _.each($("#contenedor_Filtros .filtros_Estados").find('.contenedor_estado'), function (div) {
        if ($(div).find('.estado').hasClass('activo')) {
            estadosFiltrados.push($(div).attr('id'));
        }
    })

    if (estadosFiltrados.length == 0) {
        estadosFiltrados = _.pluck(estados, 'Id');
    }

    requerimientosFiltrados = _.filter(requerimientosFiltrados, function (rq) {
        var cumple = false;
        _.each(estadosFiltrados, function (e) {
            if (e == rq.EstadoId) {
                cumple = true;
            }
        })

        return cumple;
    });

    initRequerimientos(requerimientosFiltrados);
}

// Requerimientos
function initRequerimientos(data) {
    $('#contenedor_Requerimientos').empty();

    if (data.length > 0) {
                $("#content_SinRequerimientosFiltrados").hide();
        $.each(data, function (index, element) {
            var html = crearHtmlRequerimiento(element);
            $('#contenedor_Requerimientos').append(html);
        });
    } else {
        $("#content_SinRequerimientosFiltrados").show();
                //$("#content_SinRequerimientosFiltrados").find('> label').text("No hay requerimientos que cumplan con los filtros...");
        $('#fab').hide();
    }
}

function initRequerimientosEvent() {
    $('#fab, #content_SinRequerimientos .btn').click(function () {
        redirigir('NuevoRequerimiento');
    });
}

function crearHtmlRequerimiento(entity) {
    var div = $($('#template_Requerimiento').html());

    //Numero
    $(div).find('.numero').text(entity.Numero + '/' + entity.Año);

    //Fecha
    if (entity.EstadoFecha != undefined) {
        $(div).find('.contenedor_Numero > .fecha').text(getDate(entity.EstadoFecha));
    } else {
        $(div).find('.contenedor_Numero > .fecha').text('');
    }

    //Estado
    let estadoColor = '#' + (entity.EstadoPublicoColor || entity.EstadoColor || '000');
    $(div).find('.contenedor_Estado > .indicador').css('color', estadoColor);
    $(div).find('.contenedor_Estado > .nombre').text(toTitleCase(entity.EstadoPublicoNombre || entity.EstadoNombre || 'Sin datos').trim());


    //Divisor de color segun estado
    $(div).find('.separador').css('border-color', estadoColor);

    //Servicio
    $(div).find('.contenedor_Servicio > .nombre').text(toTitleCase(entity.ServicioNombre || 'Sin datos').trim());

    //Motivo
    $(div).find('.contenedor_Motivo > .nombre').text(toTitleCase(entity.MotivoNombre || 'Sin datos').trim());

    //CPC
    $(div).find('.contenedor_Cpc > .nombre').text(toTitleCase('N° ' + (entity.CpcNumero || '0') + ' - ' + (entity.CpcNombre || 'Sin nombre').trim()));

    //Barrio
    $(div).find('.contenedor_Barrio > .nombre').text(toTitleCase(entity.BarrioNombre || 'Sin nombre').trim());


    $(div).click(function () {
        redirigir('DetalleRequerimiento?id=' + entity.Id);
    });
    return div;
}