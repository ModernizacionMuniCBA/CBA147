var grupos;
var usuario;
var permitirEditar;

var idArea = -1;

function init(data) {
    data = parse(data);

    //setTitulo('Tipo Móvil');

    //------------------------------------
    // Init Datos
    //------------------------------------

    grupos = data.GruposRubro;


    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();
    cargarResultadoConsulta(grupos);

    //------------------------------------
    // Click
    //------------------------------------

    $("#btn_agregarRubro").click(
        function () {
            insertar()
        });

    $("#inputFormulario_Nombre").on('keydown', function (e) {
        if (e.which == 13) {
            insertar();
            return false;
        }
    });

    $("#inputFormulario_Nombre").trigger('focus');

    setTimeout(function () {
        calcularCantidadDeRows();
    }, 300);
};

function insertar() {
    var obs = $("#inputFormulario_Nombre").val();

    if (obs == "" || obs == undefined) {
        mostrarMensaje('Error', 'Debe ingresar la grupo de rubros');
        return;
    }

    var data = {
        Nombre: obs
    }

    crearAjax({
        Data: { comando: data },
        Url: ResolveUrl('~/Servicios/GrupoRubroMotivoService.asmx/Insertar'),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "La rubro '" + result.Return.Nombre + "' se ha creado correctamente");
            agregarRow(result.Return);
            $("#inputFormulario_Nombre").val('');
            $("#inputFormulario_Nombre").trigger('focus');
        },
        OnError: function (result) {
            mostrarMensaje('Error', 'Error insertando la rubro');
            mostrarMensaje('Error', result.Error);
        }
    });
}

function initTablaResultadoConsulta() {
    $('#tablaRubros').DataTableGeneral({
        Orden: [[0, 'asc']],
        Columnas: [
            {
                title: 'Fecha Alta',
                data: 'FechaAltaString',
                render: function (data, type, row) {
                    return "<div><span>" + toTitleCase(data) + "</span></div>";
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
                        Titulo: '<label>Editar Categoría</label>',
                        Content: '<div class="row margin-top" >' +
                                            '<div class="col s12">' +
                                                '<div class="input-field">' +
                                                    '<input id="inputFormulario_NombreEditar" type="text"/>' +
                                                    '<label for="inputFormulario_NombreEditar" class=" no-select textarea">Grupo de rubros</label>' +
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
                                            mostrarMensaje('Error', 'Debe ingresar la grupo de rubros');
                                            return;
                                        }

                                        //Muestro el cargando
                                        $(jAlert).MostrarDialogoCargando(true, true);

                                        var dataAjax = {
                                            Id: data.Id, Nombre: obs
                                        };

                                        crearAjax({
                                            Data: { comando: dataAjax },
                                            Url: ResolveUrl('~/Servicios/GrupoRubroMotivoService.asmx/Editar'),
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
                                                mostrarMensaje('Exito', 'Grupo de rubros editada correctamente.');

                                                //Cierro el dialogo
                                                $(jAlert).CerrarDialogo();

                                                //Cierro el dialogo
                                                actualizarRowEnGrilla(result.Return);
                                            },
                                            error: function (result) {
                                                //Oculto el cargando
                                                $(jAlert).MostrarDialogoCargando(false, true);

                                                //Muestro el Error
                                                mostrarMensaje('Error', 'Error editando la grupo de rubros');
                                                console.log(result);
                                            }
                                        });
                                    }
                                }],
                        CallbackMensaje: function (grupo, mensaje) {
                            mostrarMensaje(grupo, mensaje);
                        },
                        CallbackEditar: function (grupo) {
                            actualizarRowEnGrilla(grupo);
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
                        Titulo: '<label>Eliminar Grupo de rubros</label>',
                        Content:
                            '<div class="padding">' +
                            '<label id="textoConfirmacion"class="titulo">¿Está seguro de eliminar la grupo de rubros?</label>' +
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

                                        var urlAjax = ResolveUrl('~/Servicios/GrupoRubroMotivoService.asmx/DarDeBaja');

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
                                                mostrarMensaje('Exito', 'Grupo de rubros eliminada correctamente');

                                                //Cierro el dialogo
                                                $(jAlert).CerrarDialogo();
                                                actualizarRowEnGrilla(result.Return);
                                            },
                                            OnError: function (result) {
                                                //Oculto el cargando
                                                mostrarCargando(false);

                                                //Muestro el Error
                                                mostrarMensaje('Error', 'Error eliminando la grupo de rubros');
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

    var dt = $('#tablaRubros').DataTable();
    dt.page.len(rows).draw();

    console.log(rows);
}

function cargarResultadoConsulta(data) {
    var dt = $('#tablaRubros').DataTable();

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

function agregarRow(grupo) {
    grupos.push(grupo);
    cargarResultadoConsulta(grupos);
}

function actualizarRowEnGrilla(zona) {
    //Busco el indice de la zona a actualizar
    var index = -1;
    var dt = $('#tablaRubros').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == zona.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(zona);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Actualizo la info nueva en el array de arriba
    $.each(grupos, function (index, element) {
        if (element.Id == zona.Id) {
            grupos[index] = zona;
            return;
        }
    });
}

function getGrupos() {
    return _.where(grupos,
        {
            FechaBaja: null
        });
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