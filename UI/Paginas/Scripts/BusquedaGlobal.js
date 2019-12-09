let inputBusquedaActiva;
let ajax;

function init(data) {

    let input = top.$('#toolbar_InputBusqueda');

    parent.$(parent.document).on('busqueda', function (evento, valor) {
        buscar(valor.input);
    });

    parent.$(parent.document).on('busqueda_cancelada', function (evento, valor) {
        if (inputBusquedaActiva != undefined) {
            $(input).val(inputBusquedaActiva);
        }
    });

    function buscar(busqueda) {
        inputBusquedaActiva = busqueda;


        if (ajax != undefined) {
            ajax.abort();
            ajax = undefined;
        }

        $('#contenedor_Buscando').addClass('visible');
        $('#contenedor_IndicadorVacio').removeClass('visible');

        $('#contenedorResultadoRequerimientoBusquedaGlobal').removeClass('visible');
        ajax = crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTablaBusquedaGlobal'),
            Data: { input: busqueda.trim().toLowerCase() },
            OnSuccess: function (result) {
                $('#contenedor_Buscando').removeClass('visible');
                $('#contenedor_IndicadorVacio').removeClass('visible');

                if (!result.Ok) {
                    $('#contenedorResultadoRequerimientoBusquedaGlobal').removeClass('visible');
                    tablaRequerimientosBusquedaGlobal.clear().draw();
                    return;
                }

                tablaRequerimientosBusquedaGlobal.page.len(5).draw();
                tablaRequerimientosBusquedaGlobal.clear();
                tablaRequerimientosBusquedaGlobal.rows.add(result.Return.Data).draw();
                tablaRequerimientosBusquedaGlobal.$('.tooltipped').tooltip({ delay: 50 });

                if (result.Return.Cantidad == 0) {
                    $('#contenedor_IndicadorVacio').addClass('visible');
                    $('#contenedorResultadoRequerimientoBusquedaGlobal').removeClass('visible');
                } else {
                    $('#contenedor_IndicadorVacio').removeClass('visible');
                    $('#contenedorResultadoRequerimientoBusquedaGlobal').addClass('visible');
                }
            },
            OnError: function (result) {
                $('#contenedor_Buscando').removeClass('visible');
                $('#contenedor_IndicadorVacio').removeClass('visible');
                $('#contenedorResultadoRequerimientoBusquedaGlobal').removeClass('visible');
            }
        });
    }

    var tablaRequerimientosBusquedaGlobal = $('#tabla_RequerimientoBusqueda').DataTableReclamo2({
        //Callbacks generales
        CallbackMensajes: function (tipo, mensaje) {
            mostrarMensaje(tipo, mensaje);
        },
        CallbackCargando: function (cargando, mensaje) {
            mostrarCargando(cargando, mensaje);
        }
    });

    //Muevo el indicador y el paginado a mi propio div
    $('#contenedorResultadoRequerimientoBusquedaGlobal .tabla-footer').empty();
    $('#contenedorResultadoRequerimientoBusquedaGlobal .dataTables_info').detach().appendTo($('#contenedorResultadoRequerimientoBusquedaGlobal .tabla-footer'));
    $('#contenedorResultadoRequerimientoBusquedaGlobal .dataTables_paginate').detach().appendTo($('#contenedorResultadoRequerimientoBusquedaGlobal .tabla-footer'));


    var busquedaInicial = $.url().param('input');
    if (busquedaInicial != undefined) {
        $(input).val(busquedaInicial);
        buscar(busquedaInicial);
    }

    //    var htmlSTring = "\
    //<div class='padding'><label>\
    //Desde la busqueda global usted puede encontrar rapidamente cualquier requerimiento. </br>\
    //Puede ingresar texto libre y/o alguno de los siguientes comandos: </br>\
    //- Barrio, CPC, Area, Motivo, Servicio, Referente, Creador, Estado, Origen, Fecha</br>\
    //Ademas puede combinar comandos y texto libre con los operadores *O* y *Y*.</br> \
    //</br>\
    //Ejemplos: </br></br>\
    //focos quemados</br>Buscara todos los requerimientos que posean en alguno de sus atributos el texto 'focos quemados'. Puede ser en su descripcion, en su motivo, referente, email asociados... etc</br></br>\
    //Motivo: poda *Y* Area: espacios verdes </br>Buscara todos los requerimientos que posean de motivo 'Poda' y que esten en el Área 'Espacios verdes'</br></br>\
    //Fecha: 01/01/2018</br>Buscara todos los requerimientos que hallan sido creados el 1 de Enero de 2018</br></br>\
    //Referente: Jose Perez *O* Creador: Jose Perez</br>Buscara todos los requerimientos que posean como referente o hallan sido creados por Jose Perez</br></br>\
    //</label></div>";

    //    $('#btnInfo').click(function () {
    //        crearDialogoHTML({
    //            Titulo:'<label>Ayuda</label>',
    //            Content: htmlSTring,
    //            Height: 0.80,
    //            Botones: [
    //                {
    //                    Texto: 'Aceptar'
    //                }
    //            ]
    //        })
    //    });
}

