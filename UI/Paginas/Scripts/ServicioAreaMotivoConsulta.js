
let info;

let servicioSeleccionado;
let areaSeleccionada;

let servicios;
let areas;
let motivos;

let serviciosDestacados = false;

let motivosGenerales = false;
let motivosUrgentes = false;
let motivosInternos = false;
let motivosPrivados= false;

function init(data) {
    info = data.Info;
    cargarServicios();

    $('#input_BuscarServicios').on('input', function () {
        filtrarServicios($(this).val());
    });

    $('#input_BuscarAreas').on('input', function () {
        filtrarAreas($(this).val());
    });

    $('#input_BuscarCategorias').on('input', function () {
        filtrarCategorias($(this).val());
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

    $('#btn_ConfigurarCategorias').click(function () {
        llamarConfigurarCategorias();
    })

    $('#btn_ServiciosDestacados').click(function () {
        serviciosDestacados = !serviciosDestacados;
        $(this).toggleClass('seleccionado');
        $('#input_BuscarServicios').trigger('input');
    });

    $('#btn_MotivosGenerales').click(function () {
        motivosGenerales = !motivosGenerales;
        $(this).toggleClass('seleccionado');
        $('#btn_MotivosInternos').removeClass('seleccionado');
        $('#btn_MotivosPrivados').removeClass('seleccionado');
        motivosPrivados= false;
        motivosInternos = false;
        $('#input_BuscarMotivos').trigger('input');
    });

    $('#btn_MotivosInternos').click(function () {
        motivosInternos = !motivosInternos;
        $(this).toggleClass('seleccionado');
        $('#btn_MotivosGenerales').removeClass('seleccionado');
        $('#btn_MotivosPrivados').removeClass('seleccionado');
        motivosGenerales = false;
        motivosPrivados = false;
        $('#input_BuscarMotivos').trigger('input');
    });

    $('#btn_MotivosPrivados').click(function () {
        motivosPrivados = !motivosPrivados;
        $(this).toggleClass('seleccionado');
        $('#btn_MotivosGenerales').removeClass('seleccionado');
        $('#btn_MotivosInternos').removeClass('seleccionado');
        motivosGenerales = false;
        motivosInternos= false;
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
    let idCategoria = $('#contenedor_Categorias .entity.seleccionado').attr('id-entity');

    if (idCategoria == '0') {
        idCategoria = '-1';
    }

    crearDialogoMotivoNuevo({
        IdArea: idArea,
        IdCategoria: idCategoria,
        IdServicio: idArea != undefined ? undefined : idServicio,
        Callback: function (data) {
            let idArea = undefined;
            if ($('#contenedor_Areas .entity.seleccionado').attr('id-entity') != undefined) {
                idArea = parseInt($('#contenedor_Areas .entity.seleccionado').attr('id-entity'));
            } else {
                idArea = -1;
            }

            let idCategoria = undefined;
            if (idArea != -1) {
                if ($('#contenedor_Categorias .entity.seleccionado').attr('id-entity') != undefined) {
                    idCategoria = parseInt($('#contenedor_Categorias .entity.seleccionado').attr('id-entity'));
                } else {
                    idCategoria = -1;
                }
            }

            actualizar(idArea, idCategoria);
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

            let idCategoria = undefined;
            if (idArea != -1) {
                if ($('#contenedor_Categorias .entity.seleccionado').attr('id-entity') != undefined) {
                    idCategoria = parseInt($('#contenedor_Categorias .entity.seleccionado').attr('id-entity'));
                } else {
                    idCategoria = -1;
                }
            }

            actualizar(idArea, idCategoria);
        }
    });
}

//function llamarBorrarMotivo(id) {
//    crearDialogoMotivoEditar({
//        Id: id,
//        Callback: function (data) {
//            let idArea = undefined;
//            if ($('#contenedor_Areas .entity.seleccionado').attr('id-entity') != undefined) {
//                idArea = parseInt($('#contenedor_Areas .entity.seleccionado').attr('id-entity'));
//            }

//            let idCategoria = undefined;
//            if (idArea != -1) {
//                if ($('#contenedor_Categorias .entity.seleccionado').attr('id-entity') != undefined) {
//                    idCategoria = parseInt($('#contenedor_Categorias .entity.seleccionado').attr('id-entity'));
//                } else {
//                    idCategoria = -1;
//                }
//            }

//            actualizar(idArea, idCategoria);
//        }
//    });
//}

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

function llamarConfigurarCategorias() {
    let idArea = parseInt($('#contenedor_Areas .entity.seleccionado').attr('id-entity'));
    if (idArea == undefined || idArea == "-1" || isNaN(idArea)) {
        mostrarMensaje("Error", "Las categorias son por area. Seleccione una para poder configurar.");
        return;
    }

    crearDialogoCategoriaMotivoAreaNuevo({
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje)
        },
        IdArea: idArea,
        Callback: function () {
            actualizar(idArea);
        }
    });
}

function llamarEditarCategoria(data) {
    let idArea = parseInt($('#contenedor_Areas .entity.seleccionado').attr('id-entity'));
    crearDialogoCategoriaAreaEditar({
        Valor: data.Nombre,
        Id: data.Id,
        Callback: function () { actualizar(idArea) },
        CallbackMensajes: function () { mostrarMensaje(tipo, mensaje) }
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

function filtrarCategorias(busqueda) {
    $.each($('#contenedor_Categorias .entity'), function (index, element) {
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
        if (motivosGenerales == true) {
            if ($(element).find('.interno').css('display') != "none" || $(element).find('.privado').css('display') != "none") {
                return false;
            }
        }

        if (motivosInternos == true) {
            if ($(element).find('.interno').css('display') == "none") {
                return false;
            }
        }

        if (motivosPrivados == true) {
            if ($(element).find('.privado').css('display') == "none") {
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
        $('#contenedor_Areas .contenedor_Items').empty();
        $('#input_BuscarAreas').val('');

        $('#contenedor_Categorias .contenedor_Items').empty();
        $('#input_BuscarCategorias').val('');

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
    motivosGenerales = false;

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
            $.each(motivos, function (index, m) {
                let html = crearHtmlMotivo(m);
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
    motivosGenerales = false;

    //Muestro seleccionado el servicio
    seleccionarServicio(idServicio, function () {
        //Muestro seleccionado el area
        $('#contenedor_Areas .entity').removeClass('seleccionado');
        $('#contenedor_Areas .entity[id-entity=' + id + ']').addClass('seleccionado');

        //Cargo el array
        let categorias = getCategoriasDeArea(id);

        //Borro los motivos y luego cargo los nuevos
        $('#contenedor_Categorias .entity').addClass('oculto');
        setTimeout(function () {
            $('#contenedor_Categorias .contenedor_Items').empty();
            $('#contenedor_Motivos .contenedor_Items').empty();
            $('#input_BuscarCategorias').val('');

            //Agrego a la UI
            $.each(categorias, function (index, c) {
                let html = crearHtmlCategoria(c);
                $(html).addClass('oculto');
                $('#contenedor_Categorias .contenedor_Items').append(html);

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

function seleccionarCategoria(id, callback) {
    console.log('Categoría seleccionada ' + id);

    $('#contenedor_Motivos .boton-filtro').removeClass('seleccionado');
    motivosUrgentes = false;
    motivosInternos = false;
    motivosGenerales = false;

    ////Muestro seleccionado el area
    //$('#contenedor_Areas .entity').removeClass('seleccionado');
    //$('#contenedor_Areas .entity[id-entity=' + id + ']').addClass('seleccionado');

    //Cargo el array
    motivos = getMotivosDeCategoria(id);

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

    //Muestro seleccionada la categoria
    $('#contenedor_Categorias .entity').removeClass('seleccionado');
    $('#contenedor_Categorias .entity[id-entity=' + id + ']').addClass('seleccionado');
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

function getCategoriasDeArea(id) {
    let data = [];
    if (id != -1) {
        data = _.groupBy(_.where(info, { AreaId: id }), function (item) { return item.CategoriaId });
    }

    //Cargo el array
    let categorias = [];
    var sinAsignar = false;
    $.each(data, function (index, element) {
        //Genero la categoría
        if (element[0].CategoriaId != 0) {
            let categoria = {
                Id: element[0].CategoriaId,
                Nombre: element[0].CategoriaNombre,
                IdArea: element[0].AreaId
            };
            categorias.push(categoria);
        } else {
            sinAsignar = true;
        }
    });
    categorias = _.sortBy(categorias, 'Nombre');

    //Elemento todos
    categorias.unshift({ Id: -1, Nombre: 'Todas' });

    //Elemento sin asignar
    if (sinAsignar) {
        categorias.push({ Id: 0, Nombre: 'Sin Asignar' });
    }

    return categorias;
}

function getMotivosDeCategoria(id) {
    let data;
    let idArea = $('#contenedor_Areas .entity.seleccionado').attr('id-entity');
    if (id == -1) {
        if (idArea != undefined && idArea!= "-1") {
            data = _.where(info, { AreaId: parseInt(idArea) });
        } else {
            data = info;
        }
    } else {
        data = _.where(info, { CategoriaId: id, AreaId: parseInt(idArea) });
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
            Tipo: element.MotivoTipo,
            Prioridad: element.MotivoPrioridad
        };
        motivos.push(motivo);
    });
    motivos = _.sortBy(motivos, 'Nombre');
    return motivos;
}

function actualizar(idArea, idCategoria) {
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

            if (idCategoria != undefined) {
                seleccionarCategoria(idCategoria);
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
    if (entity.Tipo == 2) {
        $(html).find('.interno').show();
    } else if (entity.Tipo ==3) {
        $(html).find('.privado').show();
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
    
    ////Borrar
    //if (entity.Id != -1) {
    //    $(html).find('.borrar').show();
    //    $(html).find('.borrar').click(function (e) {
    //        llamarBorrarMotivo(entity.Id);
    //        e.stopPropagation();
    //        e.preventDefault();
    //    });
    //}

    //Click
    $(html).click(function () {
        onClickMotivo(entity);
    });
    return html;
}

function crearHtmlCategoria(entity) {
    let html = $($('#template_Entity').html());
    $(html).attr('id-entity', entity.Id);

    //Nombre
    $(html).find('.nombre').text(toTitleCase(entity.Nombre));

    //Editar
    if (entity.Id != -1 && entity.Id != 0) {
        $(html).find('.btn-flat').show();
        $(html).find('.btn-flat').click(function (e) {
            llamarEditarCategoria(entity);
            e.stopPropagation();
            e.preventDefault();
        });
    }

    //Click
    $(html).click(function () {
        seleccionarCategoria(entity.Id);
        onClickCategoria(entity);
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

function onClickCategoria(categoria) {
}