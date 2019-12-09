let contenedorObs;
let ubicacion;
let sugerencia;

function init() {
    ControlMapa_Init({
        ResaltarAlHacerClick: true,
        Buscar: true,
        OnMapReady: function (mapaNuevo) {
            map = mapaNuevo;

            contenedorObs = $($('#template_Input').html());
            $('#ControlMapa_ContenedorBusqueda').append(contenedorObs);
        },
        OnMarcador: function (marcador, data) {
            ubicacion = data;

            if (data.Observaciones == undefined || data.Observaciones == "") {
                $(contenedorObs).addClass('visible');
                $(contenedorObs).removeClass('error');
                $(contenedorObs).find('input').val('');
                Materialize.updateTextFields();

                top.mostrarMensaje('Info', 'Si lo desea ingrese una observación para el domicilio');
                $(contenedorObs).find('input').focus();
            } else {
                $(contenedorObs).removeClass('visible');
            }
            
        },
        Sugerencia: sugerencia
    });
}

function setSugerencia(sug) {
    sugerencia = sug;
}

function seleccionar() {
    if (ubicacion == undefined) {
        top.mostrarMensaje('Error', 'Seleccione una ubicación');
        $('#ControlMapa_InputBuscar').focus();
        return undefined;
    }

    if (ubicacion.Observaciones == undefined || ubicacion.Observaciones == "") {
        ubicacion.Observaciones = $(contenedorObs).find('input').val();
    }

    return ubicacion;
}