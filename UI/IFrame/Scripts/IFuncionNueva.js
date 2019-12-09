var funciones;
var usuario;
var permitirEditar;

var idArea = -1;

function init(data) {
    data = parse(data);

    //setTitulo('Tipo Móvil');

    //------------------------------------
    // Init Datos
    //------------------------------------

    funciones = data.Funciones;
    idArea = data.IdArea;

    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();
    cargarResultadoConsulta(funciones);

    //------------------------------------
    // Click
    //------------------------------------

    $("#btn_agregar").click(
        function () {
            insertar()
        });

    $("#inputFormulario_Nombre").on('keydown', function (e) {
        if (e.which == 13) {
            insertar();
            return false;
        }
    });

    $('#select_Area').on('change', function () {
        idArea = $(this).val();
        funciones = [];
        crearAjax({
            Data: {
                idArea: $(this).val()
            },
            Url: ResolveUrl('~/Servicios/FuncionService.asmx/GetByIdArea'),
            OnSuccess: function (result) {
                if (!result.Ok) {
                    mostrarMensaje('Error', result.Error);
                    return;
                }

                funciones = result.Return;
                cargarResultadoConsulta(funciones);
            },
            OnError: function (result) {
                mostrarMensaje('Error', 'Error insertando la función');
                mostrarMensaje('Error', result.Error);
            }
        });
    });

    if (getUsuarioLogeado().IdsAreas.length > 1) {
        $("#contenedor_Areas").show();
        $("#select_Area").CargarSelect({
            Data: getUsuarioLogeado().Areas,
            Value: 'Id',
            Text: 'Nombre',
            Sort: true
        });
    }

    $('#select_Area').trigger('change');

    $("#inputFormulario_Nombre").trigger('focus');

    setTimeout(function () {
        calcularCantidadDeRows();
    }, 300);
};

function insertar() {
    var obs = $("#inputFormulario_Nombre").val();

    if (obs == "" || obs == undefined) {
        mostrarMensaje('Error', 'Debe ingresar la función');
        return;
    }

    var data = {
        Nombre: obs,
        IdArea: idArea
    }

    crearAjax({
        Data: { comando: data },
        Url: ResolveUrl('~/Servicios/FuncionService.asmx/Insert'),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "La función '" + result.Return.Nombre + "' se ha creado correctamente");
            agregarRow(result.Return);
            $("#inputFormulario_Nombre").val('');
            $("#inputFormulario_Nombre").trigger('focus');
        },
        OnError: function (result) {
            mostrarMensaje('Error', 'Error insertando la función');
            mostrarMensaje('Error', result.Error);
        }
    });
}

function initTablaResultadoConsulta() {
    $('#tabla').DataTableGeneral({
        Orden: [[0, 'asc']],
        Columnas: [
            {
                title: 'Fecha Alta',
                data: 'FechaAlta',
                render: function (data, type, row) {
                    return "<div><span>" + toTitleCase(dateTimeToString(data)) + "</span></div>";
                }
            },
              {
                  title: 'Nombre',
                  data: 'Nombre',
                  render: function (data, type, row) {
                      return "<div><span>" + toTitleCase(data) + "</span></div>";
                  }
              },
            {
                title: 'Estado',
                data: 'FechaBaja',
                render: function (data, type, row) {
                    if (data != null) {
                        return '<div><span>Dado de baja</span></div>';
                    } else {
                        return '<div><span>Activo</span></div>';
                    }
                }
            },
        ],
        Botones: [
            {
                Titulo: 'Editar',
                Icono: 'edit',
                Visible: function (data, type, row) {
                    if (data.FechaBaja == null)
                        return true;
                    return false;
                },
                OnClick: function (data) {
                    var obs = data.Nombre;
                    crearDialogoHTML({
                        Titulo: '<label>Editar Función</label>',
                        Content: '<div class="row margin-top" >' +
                                            '<div class="col s12">' +
                                                '<div class="input-field">' +
                                                    '<input id="inputFormulario_NombreEditar" type="text"/>' +
                                                    '<label for="inputFormulario_NombreEditar" class=" no-select textarea">Función</label>' +
                                                '</div>' +
                                            '</div>' +
                                        '</div>',
                        OnLoad: function (jAlert) {
                            $(jAlert).find('#inputFormulario_NombreEditar').trigger('focus');
                            $(jAlert).find('#inputFormulario_NombreEditar').val(obs);
                        },
                        Botones:
                            [
                                {
                                    Texto: 'Cancelar'
                                },
                                {
                                    Texto: 'Aceptar',
                                    CerrarDialogo: false,
                                    Class: 'colorExito',
                                    OnClick: function (jAlert) {
                                        var obs = $(jAlert).find('#inputFormulario_NombreEditar').val();
                                        if (obs == "" || obs == undefined) {
                                            mostrarMensaje('Error', 'Debe ingresar la función');
                                            return;
                                        }

                                        //Muestro el cargando
                                        $(jAlert).MostrarDialogoCargando(true, true);

                                        var dataAjax = {
                                            Id: data.Id, Nombre: obs
                                        };

                                        crearAjax({
                                            Data: { comando: dataAjax },
                                            Url: ResolveUrl('~/Servicios/FuncionService.asmx/Update'),
                                            OnSuccess: function (result) {
                                                //Oculto el cargando
                                                $(jAlert).MostrarDialogoCargando(false, true);

                                                //Error
                                                if (!result.Ok) {
                                                    valores.CallbackMensaje('Error', result.Error);
                                                    console.log(result);
                                                    return;
                                                }

                                                //Muestro el mensaje de OK
                                                mostrarMensaje('Exito', 'Función editada correctamente.');

                                                //Cierro el dialogo
                                                $(jAlert).CerrarDialogo();

                                                //Cierro el dialogo
                                                actualizarRowEnGrilla(result.Return);
                                            },
                                            error: function (result) {
                                                //Oculto el cargando
                                                $(jAlert).MostrarDialogoCargando(false, true);

                                                //Muestro el Error
                                                mostrarMensaje('Error', 'Error editando la función');
                                                console.log(result);
                                            }
                                        });
                                    }
                                }],
                        CallbackMensaje: function (tipo, mensaje) {
                            mostrarMensaje(tipo, mensaje);
                        },
                        CallbackEditar: function (funcion) {
                            actualizarRowEnGrilla(funcion);
                        }
                    });
                }
            },

            {
                Titulo: 'Eliminar',
                Icono: 'delete',
                Visible: function (data) {
                    return data.FechaBaja == undefined;
                },
                OnClick: function (data) {

                    crearDialogoHTML({
                        Titulo: '<label>Eliminar Función</label>',
                        Content:
                            '<div class="padding">' +
                            '<label id="textoConfirmacion"class="titulo">¿Está seguro de eliminar la función?</label>' +
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

                                        var urlAjax = ResolveUrl('~/Servicios/FuncionService.asmx/DarDeBaja');

                                        var comando = { id: data.Id };
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
                                                mostrarMensaje('Exito', 'Función eliminada correctamente');

                                                //Cierro el dialogo
                                                $(jAlert).CerrarDialogo();
                                                actualizarRowEnGrilla(result.Return);
                                            },
                                            OnError: function (result) {
                                                //Oculto el cargando
                                                mostrarCargando(false);

                                                //Muestro el Error
                                                mostrarMensaje('Error', 'Error eliminando la función');
                                            }
                                        })

                                    }
                                }]
                    });
                }
            }
        ]
    });

    //Muevo el indicador y el paginado a mi propio div
    $('.tabla-footer').empty();
    $('.dataTables_info').detach().appendTo($('.tabla-footer'));
    $('.dataTables_paginate').detach().appendTo($('.tabla-footer'));
}

function calcularCantidadDeRows() {
    var hDisponible = $('.tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $('#tabla').DataTable();
    dt.page.len(rows).draw();

    console.log(rows);

}

function cargarResultadoConsulta(data) {
    var dt = $('#tabla').DataTable();

    //Borro los datos
    dt.clear().draw();

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$('.tooltipped').tooltip({ delay: 50 });

    calcularCantidadDeRows();

}

function agregarRow(funcion) {
    funciones.push(funcion);
    cargarResultadoConsulta(funciones);
}

function actualizarRowEnGrilla(funcion) {
    //Busco el indice de la funcion a actualizar
    var index = -1;
    var dt = $('#tabla').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == funcion.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(funcion);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Actualizo la info nueva en el array de arriba
    $.each(funciones, function (index, element) {
        if (element.Id == funcion.Id) {
            if (funcion.FechaBaja != null) {
                funciones.splice(index, 1);
                return;
            }

            funciones[index] = funcion;
            return;
        }
    });
}

function getFunciones() {
    return funciones;
}

/* Mensajes */
function mostrarMensaje(tipo, mensaje) {
    switch (tipo) {
        case 'Info':
            Materialize.toast(mensaje, 5000);
            break;

        case 'Alerta':
            Materialize.toast(mensaje, 5000, 'colorAlerta');
            break;

        case 'Error':
            Materialize.toast(mensaje, 5000, 'colorError');
            break;

        case 'Exito':
            Materialize.toast(mensaje, 5000, 'colorExito');
            break;
    }
}