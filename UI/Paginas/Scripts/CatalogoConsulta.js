let info;
let servicioSeleccionado;
let areaSeleccionada;
let servicios;
let areas;
let serviciosDestacados = false;
let idArea;

let catalogos = [
  {
      "Nombre": "Usuarios",
      "Id": 1,
  },
  {
      "Nombre": "Motivos",
      "Id": 2,
  },
  {
      "Nombre": "Tareas",
      "Id": 3,
  }, ];


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
}



function llamarDescargarCatalogo(tipoCatalogo, idArea) {

    var creado;
    switch (tipoCatalogo) {
        case 1:
            {
                creado = crearDialogoReporteCatalogoUsuarios({
                    TipoCatalogo: tipoCatalogo,
                    IdArea: idArea
                });

                if (creado == false) {
                    mostrarMensaje('Error', 'Error creando el reporte');
                }
            }
            break;
        case 2:
            {
                creado = crearDialogoReporteCatalogoMotivos({
                    TipoCatalogo: tipoCatalogo,
                    IdArea: idArea
                });

                if (creado == false) {
                    mostrarMensaje('Error', 'Error creando el reporte');
                }

            }
            break;
        case 3:
            {
                creado = crearDialogoReporteCatalogoTareas({
                    TipoCatalogo: tipoCatalogo,
                    IdArea: idArea
                });

                if (creado == false) {
                    mostrarMensaje('Error', 'Error creando el reporte');
                }

            }
            break;

    }














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

    //Borro las areas y catalogos y luego dibujo las areas
    $('#contenedor_Areas .entity').addClass('oculto');
    $('#contenedor_Catalogos .entity').addClass('oculto');
    setTimeout(function () {
        $('#contenedor_Areas .contenedor_Items').empty();
        $('#input_BuscarAreas').val('');

        $('#contenedor_Catalogos .contenedor_Items').empty();
        $('#input_BuscarCatalogos').val('');

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


    $('#contenedor_Catalogos .boton-filtro').removeClass('seleccionado');

    //Muestro seleccionado el servicio
    seleccionarServicio(idServicio, function () {
        //Muestro seleccionado el area
        $('#contenedor_Areas .entity').removeClass('seleccionado');
        $('#contenedor_Areas .entity[id-entity=' + id + ']').addClass('seleccionado');

        //Cargo el array




        //Borro los motivos y luego cargo los nuevos
        $('#contenedor_Catalogos .entity').addClass('oculto');
        setTimeout(function () {
            $('#contenedor_Catalogos .contenedor_Items').empty();
            $('#input_BuscarMotivos').val('');

            //Agrego a la UI
            $.each(catalogos, function (index, catalogo) {
                let html = crearHtmlCatalogo(catalogo);
                $(html).addClass('oculto');
                $('#contenedor_Catalogos .contenedor_Items').append(html);

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
    //areas.unshift({ Id: -1, Nombre: 'Todos' });

    return areas;
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
    //if (entity.Principal == true) {
    //    $(html).find('.principal').show();
    //    $(html).find('.principal').css('background-color', entity.Color);
    //    $(html).find('.principal i').addClass('mdi-' + entity.Icono);
    //} else {
    //    $(html).find('.principal').hide();
    //}



    //Editar
    //if (entity.Id != -1) {
    //    $(html).find('.btn-flat').show();
    //    $(html).find('.btn-flat').click(function (e) {
    //        llamarEditarServicio(entity.Id);
    //        e.stopPropagation();
    //        e.preventDefault();
    //    });
    //}

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
        idArea = entity.Id;
        onClickArea(entity);
    });
    return html;
}
function crearHtmlSubArea(entity) {
    let html = $($('#template_Entity').html());
    $(html).attr('id-entity', entity.Id);

    //Nombre
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));
    $(html).find('.principal').hide();

    //Click
    $(html).click(function () {
        seleccionarArea(entity.Id);
        idArea = entity.Id;
        onClickArea(entity);
    });
    return html;
}

function crearHtmlCatalogo(entity) {
    let html = $($('#template_Entity').html());
    $(html).attr('id-entity', entity.Id);

    //Nombre
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));
    $(html).find('.principal').hide();

    //Editar
    if (entity.Id != -1) {
        $(html).find('.btn-flat').show();
        $(html).find('.btn-flat').click(function (e) {

            consultar2(entity.Id, idArea);

            //llamarDescargarCatalogo(entity.Id, idArea);
            e.stopPropagation();
            e.preventDefault();
        });
    }

    //Click
    $(html).click(function () {
        onClickCatalogo(entity);
    });
    return html;
}

function consultar2(tipoCatalogo, idArea) { 

    var url = ResolveUrl('~/Servicios/EstadisticaService.asmx/GetCantidadByIdArea');

    datosService = { idArea: idArea, tipoCatalogo: tipoCatalogo };

    crearAjax({
        Url: url,
        Data: datosService,
        OnSuccess: function (result) {

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }
            var resultado = result.Return;

            //No hay resultados
            if (resultado == 0) {
                mostrarMensaje('Alerta', 'No hay elementos en el área seleccionada');
                return;
            }
            llamarDescargarCatalogo(tipoCatalogo, idArea);
        },
        OnError: function (result) {

            //Oculto el cargando
            $('#cardConsulta').find('.cargando').stop(true, true).fadeOut(500);

            //Informo
            mostrarMensaje('Error', 'Error al realizar la consulta');
        }
    });

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

function onClickCatalogo(motivo) {
}