var tabla;

var indicadorCargando;
var tiposMotivo;

var infoConfiguracion = [];

function init(data) {
    tiposMotivo = data.TiposMotivo;

    initLista();
    initTabla();
    initBotones();

    $('#main > .card').AgregarIndicadorCargando();
    indicadorCargando = $('#main > .card').GetIndicadorCargando();
    $(indicadorCargando).fadeOut();
}

function initLista() {
    var areas = getUsuarioLogeado().Areas;

    $.each(areas, function (indexArea, area) {
        $.each(tiposMotivo, function (index, tipo) {
            let porDefecto = false;
            $.each(area.TiposMotivoPorDefecto, function (index, element) {
                if (element == tipo.KeyValue) {
                    porDefecto = true;
                    return false;
                }
            });

            infoConfiguracion.push({ IdArea: area.Id, IdTipoMotivo: tipo.KeyValue, PorDefecto: porDefecto });
        });
    })
}

function initTabla() {
    //$.each( getUsuarioLogeado().Areas, function (index, element) {
    //    element.EstadoNombre = $.grep(estados, function (element1, index1) {
    //        return element1.KeyValue == element.EstadoOrdenTrabajo;
    //    })[0].Nombre;

    //    element.PermisoNombre = $.grep(permisos, function (element1, index1) {
    //        return element1.KeyValue == element.Permiso;
    //    })[0].Nombre;
    //});


    var cols = [];
    cols.push({
        title: 'Area',
        data: 'Nombre',
        render: function (data, type, row) {
            return '<div><span>' + data + '</span></div>';
        }
    });

    $.each(tiposMotivo, function (index, tipo) {
        cols.push({
            title: tipo.Nombre,
            width: '20px',
            render: function (data, type, row, meta) {
                let indexRow = meta.row;
                let indexCol = meta.col;
                let area = tabla.data()[indexRow];

                let id = 'check_' + tipo.KeyValue + '_' + area.Id;

                var c = _.find(infoConfiguracion, function (config) {
                    return config.IdArea == area.Id && config.IdTipoMotivo == tipo.KeyValue
                });
                let inputCheck = c.PorDefecto ? 'checked' : '';
                return ' <div><input ' + inputCheck + ' onChange="changeCheck(' + indexRow + ', ' + indexCol + ')" class="with-gap" type="checkbox" id="' + id + '"/><label for="' + id + '"></label></div>'
            }
        });
    });

    tabla = $('#tabla').DataTableGeneral({
        Orden: [[0, 'asc']],
        Columnas: cols
    });

    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tabla.page.len(rows).draw();
    tabla.rows.add(getUsuarioLogeado().Areas).draw();
}

function initBotones() {
    $('#btn_Guardar').click(function () {
        $(indicadorCargando).stop(true, true).fadeIn(300);

        crearAjax({
            Url: ResolveUrl('~/Servicios/ConfiguracionBandejaService.asmx/Insertar'),
            Data: { comando: { ConfiguracionesBandejas: infoConfiguracion } },
            OnSuccess: function (result) {
                $(indicadorCargando).stop(true, true).fadeOut(300);
                if (!result.Ok) {
                    mostrarMensaje('Error', result.Error);
                    return;
                }

                mostrarMensaje('Exito', "La configuracion de la bandeja se cambio con exito");

                var areas = getUsuarioLogeado().Areas;
                //actualizo las areas del usuario logueado
                $.each(areas, function (indexArea, area) {
                    area.TiposMotivoPorDefecto = _.pluck(_.filter(infoConfiguracion, function (config) {
                        return config.IdArea == area.Id && config.PorDefecto
                    }), 'IdTipoMotivo');
                })

                setAreas(areas);

                //crearAjax({
                //    Url: ResolveUrl('~/Servicios/ConfiguracionBandejaPorAreaService.asmx/Insertar'),
                //    OnSuccess: function (result2) {
                //        $(indicadorCargando).stop(true, true).fadeOut(300);

                //        if (!result2.Ok) {
                //            mostrarMensaje('Error', result2.Error);
                //            return;
                //        }

                //        infoConfiguracion = result2.Return;
                //        tabla.clear();
                //        tabla.rows.add(permisos);
                //        tabla.draw();

                //        mostrarMensaje('Exito', 'Permisos actualizados');
                //    },
                //    OnError: function (result) {
                //        $(indicadorCargando).stop(true, true).fadeOut(300);

                //        mostrarMensaje('Error', 'Error procesando la solicitud');
                //    }
                //});
            },
            OnError: function (result) {
                $(indicadorCargando).stop(true, true).fadeOut(300);

                mostrarMensaje('Error', 'Error procesando la solicitud');
            }
        });
    });
}

function changeCheck(row, col) {
    //pongo en false el valor de los otros tipos en el area
    _.each(tiposMotivo, function (t) {
        if (t.KeyValue == col ) {
            return false;
        }

        var id = "#check_" + t.KeyValue + '_' + tabla.data()[row].Id;
        $(id).prop("checked", false);
    })

    let tipo = tiposMotivo[col - 1];
    console.log('tipo');
    console.log(tipo);

    let area = tabla.data()[row];
    console.log('area');
    console.log(area);

    $.each(infoConfiguracion, function (index, element) {
        if (element.IdArea == area.Id) {
            if (element.IdTipoMotivo == tipo.KeyValue) {
                element.PorDefecto = !element.PorDefecto
                return;
            }

            element.PorDefecto = false;
        }
    });
}