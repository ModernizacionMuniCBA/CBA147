
let info;

let servicioSeleccionado;
let areaSeleccionada;

let servicios;
let areas;
let motivos;

let serviciosDestacados = false;

let motivosDestacados = false;
let motivosUrgentes = false;
let motivosInternos = false;

function init(data) {
    info = data.Info;
    cargarServicios();

    $('#input_BuscarServicios').on('input', function () {
        filtrarServicios($(this).val());
    });

    $('#input_BuscarAreas').on('input', function () {
        filtrarAreas($(this).val());
    });

    $('#input_BuscarMotivos').on('input', function () {
        filtrarMotivos($(this).val());
    });

    $('#btn_NuevoMotivo').click(function () {
        llamarNuevoMotivo();
    });

    $('#btn_NuevoServicio').click(function () {
        llamarNuevoServicio();
    });

    $('#btn_ServiciosDestacados').click(function () {
        serviciosDestacados = !serviciosDestacados;
        $(this).toggleClass('seleccionado');
        $('#input_BuscarServicios').trigger('input');
    });

    $('#btn_MotivosDestacados').click(function () {
        motivosDestacados = !motivosDestacados;
        $(this).toggleClass('seleccionado');
        $('#input_BuscarMotivos').trigger('input');
    });

    $('#btn_MotivosInternos').click(function () {
        motivosInternos = !motivosInternos;
        $(this).toggleClass('seleccionado');
        $('#input_BuscarMotivos').trigger('input');
    });

    $('#btn_MotivosUrgentes').click(function () {
        motivosUrgentes = !motivosUrgentes;
        $(this).toggleClass('seleccionado');
        $('#input_BuscarMotivos').trigger('input');
    });
}

function llamarNuevoMotivo() {
    let idArea = $('#contenedor_Areas .entity.seleccionado').attr('id-entity');
    let idServicio = $('#contenedor_Servicios .entity.seleccionado').attr('id-entity');


    crearDialogoMotivoNuevo({
        IdArea: idArea,
        IdServicio: idArea != undefined ? undefined : idServicio,
        Callback: function (data) {
            let idArea = undefined;
            if ($('#contenedor_Areas .entity.seleccionado').attr('id-entity') != undefined) {
                idArea = parseInt($('#contenedor_Areas .entity.seleccionado').attr('id-entity'));
            } else {
                idArea = -1;
            }

            actualizar(idArea);
        }
    });
}

function llamarEditarMotivo(id) {
    crearDialogoMotivoEditar({
        Id: id,
        Callback: function (data) {
            let idArea = undefined;
            if ($('#contenedor_Areas .entity.seleccionado').attr('id-entity') != undefined) {
                idArea = parseInt($('#contenedor_Areas .entity.seleccionado').attr('id-entity'));
            }

            actualizar(idArea);
        }
    });
}

function llamarNuevoServicio() {

    crearDialogoServicioNuevo({
        Callback: function (data) {
            actualizar();
        }
    });
}

function llamarEditarServicio(id) {
    crearDialogoServicioEditar({
        Id: id,
        Callback: function (data) {
            actualizar();
        }
    });
}



//-----------------------------------
// Filtros
//-----------------------------------

function filtrarServicios(busqueda) {
    let items = $.grep($('#contenedor_Servicios .entity'), function (element) {
        if (serviciosDestacados == true) {
            if ($(element).find('.principal').css('display') == "none") {
                return false;
            }
        }

        return true;
    });

    $('#contenedor_Servicios .entity').addClass('oculto');
    $.each(items, function (index, element) {
        let nombre = $(element).find('.nombre').text();
        if (nombre.toLowerCase().indexOf(busqueda.toLowerCase()) != -1) {
            $(element).removeClass('oculto');
        } else {
            $(element).addClass('oculto');
        }
    });
}

function filtrarAreas(busqueda) {
    $.each($('#contenedor_Areas .entity'), function (index, element) {
        let nombre = $(element).find('.nombre').text();
        if (nombre.toLowerCase().indexOf(busqueda.toLowerCase()) != -1) {
            $(element).removeClass('oculto');
        } else {
            $(element).addClass('oculto');
        }
    });
}

function filtrarMotivos(busqueda) {
    let items = $.grep($('#contenedor_Motivos .entity'), function (element) {
        if (motivosDestacados == true) {
            if ($(element).find('.principal2').css('display') == "none") {
                return false;
            }
        }

        if (motivosInternos == true) {
            if ($(element).find('.interno').css('display') == "none") {
                return false;
            }
        }

        if (motivosUrgentes == true) {
            if ($(element).find('.urgente').css('display') == "none") {
                return false;
            }
        }
        return true;
    });


    $('#contenedor_Motivos .entity').addClass('oculto');
    $.each(items, function (index, element) {
        let nombre = $(element).find('.nombre').text();
        if (nombre.toLowerCase().indexOf(busqueda.toLowerCase()) != -1) {
            $(element).removeClass('oculto');
        } else {
            $(element).addClass('oculto');
        }
    });
}

function cargarServicios() {
    //Cargo el array
    servicios = getServicios();

    $('#contenedor_Servicios .boton-filtro').removeClass('seleccionado');
    serviciosDestacados = false;


    //Cargo la UI
    $('#contenedor_Servicios .contenedor_Items').empty();
    $.each(servicios, function (index, servicio) {
        let html = crearHtmlServicio(servicio);
        $('#contenedor_Servicios .contenedor_Items').append(html);
    });
}

function seleccionarServicio(id, callback) {
    console.log('Servicio seleccionado ' + id);
    if ($('#contenedor_Servicios .entity.seleccionado').attr('id-entity') == id) {
        if (callback != undefined) {
            callback();
        }
        return;
    }

    //Muestro seleccionado el servicio
    $('#contenedor_Servicios .entity').removeClass('seleccionado');
    $('#contenedor_Servicios .entity[id-entity=' + id + ']').addClass('seleccionado');

    //Cargo el array
    areas = getAreasDeServicio(id);

    //Borro las areas y motivos y leugo dibujo las areas
    $('#contenedor_Areas .entity').addClass('oculto');
    $('#contenedor_Motivos .entity').addClass('oculto');
    setTimeout(function () {
        $('#contenedor_Areas .contenedor_Items').empty();
        $('#input_BuscarAreas').val('');

        $('#contenedor_Motivos .contenedor_Items').empty();
        $('#input_BuscarMotivos').val('');

        //Agrego a la UI
        $.each(areas, function (index, area) {
            let html = crearHtmlArea(area);
            $(html).addClass('oculto');
            $('#contenedor_Areas .contenedor_Items').append(html);

            setTimeout(function () {
                $(html).removeClass('oculto');
            }, 50);
        });

        if (callback != undefined) {
            callback();
        }
    }, 300);
}

function seleccionarArea(id, callback) {
    console.log('Area seleccionada ' + id);

    //Busco el servicio
    let idServicio = -1;
    if (id != -1) {
        let servicio = _.findWhere(info, { AreaId: id });
        if (servicio != undefined) {
            idServicio = servicio.ServicioId;
        }
    } else {
        let idActual = $('#contenedor_Servicios .entity.seleccionado').attr('id-entity');
        if (idActual == undefined) {
            idServicio = -1;
        } else {
            idServicio = parseInt(idActual);
        }
    }


    $('#contenedor_Motivos .boton-filtro').removeClass('seleccionado');
    motivosUrgentes = false;
    motivosInternos = false;
    motivosDestacados = false;

    //Muestro seleccionado el servicio
    seleccionarServicio(idServicio, function () {
        //Muestro seleccionado el area
        $('#contenedor_Areas .entity').removeClass('seleccionado');
        $('#contenedor_Areas .entity[id-entity=' + id + ']').addClass('seleccionado');

        //Cargo el array
        motivos = getMotivosDeArea(id);

        //Borro los motivos y luego cargo los nuevos
        $('#contenedor_Motivos .entity').addClass('oculto');
        setTimeout(function () {
            $('#contenedor_Motivos .contenedor_Items').empty();
            $('#input_BuscarMotivos').val('');

            //Agrego a la UI
            $.each(motivos, function (index, area) {
                let html = crearHtmlMotivo(area);
                $(html).addClass('oculto');
                $('#contenedor_Motivos .contenedor_Items').append(html);

                setTimeout(function () {
                    $(html).removeClass('oculto');
                }, 50);
            });

            //Callback
            if (callback != undefined) {
                callback();
            }
        }, 300);
    });
}


function getServicios() {
    let data = _.groupBy(info, function (item) { return item.ServicioId; });

    let servicios = [];
    $.each(data, function (index, element) {
        //Genero el servicio
        let servicio = {
            Id: element[0].ServicioId,
            Nombre: element[0].ServicioNombre,
            Principal: element[0].ServicioPrincipal,
            Icono: element[0].ServicioIcono,
            Color: element[0].ServicioColor
        }
        servicios.push(servicio);
    });
    servicios = _.sortBy(servicios, 'Nombre');

    //Elemento todos
    servicios.unshift({ Id: -1, Nombre: 'Todos' });
    return servicios;
}

function getAreasDeServicio(id) {
    let data;
    if (id == -1) {
        data = _.groupBy(_.where(info, {}), function (item) { return item.AreaId });
    } else {
        data = _.groupBy(_.where(info, { ServicioId: id }), function (item) { return item.AreaId });
    }

    //Cargo el array
    let areas = [];
    $.each(data, function (index, element) {

        //Genero el area
        if (element[0].AreaId != 0) {
            let area = {
                Id: element[0].AreaId,
                Nombre: element[0].AreaNombre,
                IdServicio: element[0].ServicioId
            };
            areas.push(area);
        }
    });
    areas = _.sortBy(areas, 'Nombre');


    //Elemento todos
    areas.unshift({ Id: -1, Nombre: 'Todos' });

    return areas;
}

function getMotivosDeArea(id) {
    let data;
    if (id == -1) {
        if ($('#contenedor_Servicios .entity.seleccionado').attr('id-entity') == undefined) {
            data = _.where(info, {});
        } else {
            let idServicio = parseInt($('#contenedor_Servicios .entity.seleccionado').attr('id-entity'));
            let idsAreas = _.pluck(getAreasDeServicio(idServicio), 'Id');

            data = _.filter(info, function (item) {
                return idsAreas.indexOf(item.AreaId) != -1;
            });
        }
    } else {
        data = _.where(info, { AreaId: id });
    }

    let motivos = [];
    $.each(data, function (index, element) {
        let motivo = {
            Id: element.MotivoId,
            Nombre: toTitleCase(element.MotivoNombre),
            IdServicio: element.ServicioId,
            AreaId: element.AreaId,
            Principal: element.MotivoPrincipal,
            Urgente: element.MotivoUrgente,
            Interno: element.MotivoInterno,
            Prioridad: element.MotivoPrioridad
        };
        motivos.push(motivo);
    });
    motivos = _.sortBy(motivos, 'Nombre');
    return motivos;
}


function actualizar(idArea) {
    ajax_Actualizar()
        .then(function (data) {
            info = data;
            let idActual = $('#contenedor_Servicios .entity.seleccionado').attr('id-entity');
            cargarServicios();
            if (idActual != undefined) {
                $('#contenedor_Servicios .entity').removeClass('seleccionado');
                $('#contenedor_Servicios .entity[id-entity=' + idActual + ']').addClass('seleccionado');
            }


            if (idArea != undefined) {
                seleccionarArea(idArea);
            }
        })
        .catch(function (error) {
            mostrarMensajeError(error);
        });
}


//-----------------------------------
// HTML
//-----------------------------------

function crearHtmlServicio(entity) {
    let html = $($('#template_Entity').html());
    $(html).attr('id-entity', entity.Id);

    //Nombre
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));

    //Principal
    if (entity.Principal == true) {
        $(html).find('.principal').show();
        $(html).find('.principal').css('background-color', entity.Color);
        $(html).find('.principal i').addClass('mdi-' + entity.Icono);
    } else {
        $(html).find('.principal').hide();
    }



    //Editar
    if (entity.Id != -1) {
        $(html).find('.btn-flat').show();
        $(html).find('.btn-flat').click(function (e) {
            llamarEditarServicio(entity.Id);
            e.stopPropagation();
            e.preventDefault();
        });
    }

    //Click
    $(html).click(function () {
        seleccionarServicio(entity.Id);
        onClickServicio(entity);
    });
    return html;
}

function crearHtmlArea(entity) {
    let html = $($('#template_Entity').html());
    $(html).attr('id-entity', entity.Id);

    //Nombre
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));
    $(html).find('.principal').hide();

    //Click
    $(html).click(function () {
        seleccionarArea(entity.Id);
        onClickArea(entity);
    });
    return html;
}

function crearHtmlMotivo(entity) {
    let html = $($('#template_Entity').html());
    $(html).attr('id-entity', entity.Id);

    //Nombre
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));
    $(html).find('.principal').hide();

    //Principal
    if (entity.Principal == true) {
        $(html).find('.principal2').show();
    } else {
        $(html).find('.principal2').hide();
    }

    //Interno
    if (entity.Interno == true) {
        $(html).find('.interno').show();
    } else {
        $(html).find('.interno').hide();
    }

    //Urgente
    if (entity.Urgente == true) {
        $(html).find('.urgente').show();
    } else {
        $(html).find('.urgente').hide();

    }

    //Prioridad
    $(html).find('.prioridad').show();
    if (entity.Prioridad == 1) {
        $(html).find('.prioridad').css('color', 'var(--colorPrioridadNormal)');
    } else {
        if (entity.Prioridad == 2) {
            $(html).find('.prioridad').css('color', 'var(--colorPrioridadMedia)');
        } else {
            $(html).find('.prioridad').css('color', 'var(--colorPrioridadAlta)');
        }
    }

    //Editar
    if (entity.Id != -1) {
        $(html).find('.btn-flat').show();
        $(html).find('.btn-flat').click(function (e) {
            llamarEditarMotivo(entity.Id);
            e.stopPropagation();
            e.preventDefault();
        });
    }

    //Click
    $(html).click(function () {
        onClickMotivo(entity);
    });
    return html;
}


function ajax_Actualizar() {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/MotivoService.asmx/GetInfo'),
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
        })
    });
}


//-----------------------------------
// OnClick
//-----------------------------------
function onClickServicio(servicio) {

}

function onClickArea(area) {

}

function onClickMotivo(motivo) {
}