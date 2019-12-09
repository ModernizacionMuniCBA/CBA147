var areas;
var funciones;
var funcionesSeleccionadas = [];

function init(data) {

    areas = data.Areas;

    //Cargo las areas
    $('#select_Area').CargarSelect({
        Data: areas,
        Value: 'Id',
        Text: 'Nombre'
    });

    //Funciones
    $('#select_Funcion').CargarSelect({
        Data: [],
        Value: 'Id',
        Text: 'Nombre'
    });

    //---------------------
    // Persona
    //---------------------
    SelectorPersona_SetAbrirPersonaSiNoEncuentro(true);

    SelectorPersona_SetOnMensajeListener(function (tipo, mensaje) {
        mostrarMensaje(tipo, mensaje);
    })

    //Asignar
    $('#btnAsignar').click(function () {
        var idArea = $('#select_Area').val();
        if (idArea == undefined || idArea <= 0) {
            mostrarMensaje('Alerta', 'Debe seleccionar el área');
            return;
        }

        if (SelectorPersona_GetPersonaSeleccionada() == undefined) {
            mostrarMensaje('Alerta', 'Debe seleccionar una persona');
            return;
        }

        cargarFunciones();

    });

    $('#btnAgregar').click(function () {
        var funcion = $.grep(funciones, function (element, index) {
            return element.Id == $('#select_Funcion').val();
        })[0];
        if (funcion != undefined) {
            agregarFuncion(funcion);
        }
    });
    $('#btnCancelar').click(function () {
        mostrarFormulario();
    });

    $('#btnGuardar').click(function () {
        guardar();
    });

    $('#contenedorResultado').hide();
}

function cargarFunciones() {

    $('#btnAsignar').addClass('boton-deshabilitado');


    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ idArea: $('#select_Area').val() }),
        url: ResolveUrl('~/Servicios/FuncionService.asmx/GetPorArea'),
        success: function (result) {
            result = parse(result.d);

            //algo salio mal
            if ('Error' in result) {
                $('#btnAsignar').removeClass('boton-deshabilitado');
                mostrarMensaje('Alerta', result.Error.Publico);
                return;
            }

            funciones = result.Funciones;
            //Funciones
            $('#select_Funcion').CargarSelect({
                Data: funciones,
                Value: 'Id',
                Text: 'Nombre'
            });

            var filtros = {};
            filtros.idArea = $('#select_Area').val();
            filtros.idPersonaFisica = SelectorPersona_GetPersonaSeleccionada().Id;
            filtros.dadosDeBaja = false;
            filtros = JSON.stringify({ filtros: filtros });
            $.ajax({
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: filtros,
                url: ResolveUrl('~/Servicios/PersonalService.asmx/GetByFilters'),
                success: function (result) {
                    result = parse(result.d);

                    $('#btnAsignar').removeClass('boton-deshabilitado');

                    //algo salio mal
                    if ('Error' in result) {
                        mostrarMensaje('Alerta', result.Error.Publico);
                        return;
                    }

                    mostrarResultado();

                    //Quito las funciones
                    funcionesSeleccionadas = [];
                    $('#contenedorFuncionesSeleccionadas').empty();

                    //Agrego las que tenia en la base de datos
                    $.each(result.Return, function (index, element) {
                        var f = $.grep(funciones, function (element1, index1) {
                            return element1.Id == element.IdFuncion;
                        })[0];
                        if (f != undefined) {
                            agregarFuncion(f);
                        }
                    });
                },
                error: function (result) {
                    $('#btnAsignar').removeClass('boton-deshabilitado');
                    mostrarMensaje('Error', 'Error procesando la solicitud');
                }
            });
        },
        error: function (result) {
            $('#btnAsignar').removeClass('boton-deshabilitado');
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function agregarFuncion(data) {
    var yaExiste = $.grep(funcionesSeleccionadas, function (element, index) {
        return element.Id == data.Id;
    }).length != 0;

    if (yaExiste) {
        mostrarMensaje('Alerta', 'La funcion ya se encuentra seleccionada')
        return;
    }

    funcionesSeleccionadas.push(data);

    //Card
    var div = $('<div>', {
        'id-funcion': data.Id,
        class: 'card funcionSeleccionada'
    });

    //Texto
    var texto = $('<label>', {
        class: 'nombre'
    }).text(toTitleCase(data.Nombre));
    $(texto).appendTo(div);

    //Boton quitar
    var btnQuitar = $('<a>', {
        id: 'btnQuitar',
        class: 'btn-flat waves-effect chico btn-redondo'
    });
    var icono = $('<i>', { class: 'material-icons' }).text('clear');
    $(icono).appendTo(btnQuitar);
    $(btnQuitar).appendTo(div);
    $(btnQuitar).click(function () {
        quitarFuncion(data);
    });

    //Lo agrego
    $('#contenedorFuncionesSeleccionadas').append(div);

    setTimeout(function () {
        $(div).addClass('visible');
    }, 1);
}

function quitarFuncion(data) {
    var yaExiste = $.grep(funcionesSeleccionadas, function (element, index) {
        return element.Id == data.Id;
    }).length != 0;

    if (!yaExiste) {
        mostrarMensaje('Alerta', 'La funcion no existe')
        return;
    }

    funcionesSeleccionadas = $.grep(funcionesSeleccionadas, function (element, index) {
        return element.Id != data.Id;
    });

    $('#contenedorFuncionesSeleccionadas').find('[id-funcion=' + data.Id + ']').removeClass('visible');
}

function mostrarFormulario() {
    $('#cardFormulario').removeClass('invisible');
    $('#contenedorResultado').fadeOut(300);
}

function mostrarResultado() {
    var area = $.grep(areas, function (element, index) {
        return element.Id == $('#select_Area').val();
    })[0];
    $('#textoArea').html('<b>Area: </b>' + toTitleCase(area.Nombre));
    $('#textoPersona').html('<b>Persona: </b>' + toTitleCase(SelectorPersona_GetPersonaSeleccionada().Nombre + ' ' + SelectorPersona_GetPersonaSeleccionada().Apellido));

    $('#cardFormulario').addClass('invisible');
    $('#contenedorResultado').fadeIn(300);
}

function getData() {
    var data = {};
    data.IdArea = $('#select_Area').val();
    data.IdPersonaFisica = SelectorPersona_GetPersonaSeleccionada().Id;
    data.IdFunciones = [];
    $.each(funcionesSeleccionadas, function (index, element) {
        data.IdFunciones.push(element.Id);
    });
    return data;
}

function guardar() {
    var idArea = $('#select_Area').val();
    if (idArea == undefined || idArea <= 0) {
        mostrarMensaje('Alerta', 'Debe seleccionar al area');
        return;
    }

    if (SelectorPersona_GetPersonaSeleccionada() == undefined || SelectorPersona_GetPersonaSeleccionada().Id <= 0) {
        mostrarMensaje('Alerta', 'Debe seleccionar la persona');
        return;
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ data: getData() }),
        url: ResolveUrl('~/Servicios/PersonalService.asmx/Insertar'),
        success: function (result) {
            result = parse(result.d);

            if ('Error' in result) {
                mostrarMensaje('Alerta', result.Error.Publico);
                return;
            }

            mostrarMensaje('Exito', 'Personal asignado correctamente');
            mostrarFormulario();
        },
        error: function (result) {
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

// Soporte
function mostrarMensaje(tipo, mensaje) {
    switch (tipo) {
        case 'Error':
            Materialize.toast(mensaje, 5000, 'colorError');
            break;

        case 'Info':
            Materialize.toast(mensaje, 5000, 'colorInfo');
            break;

        case 'Exito':
            Materialize.toast(mensaje, 5000, 'colorExito');
            break;

        case 'Alerta':
            Materialize.toast(mensaje, 5000, 'colorAlerta');
            break;
    }
}