var categorias;
var usuario;
var permitirEditar;

var idArea = -1;

function init(data) {
    data = parse(data);

    //setTitulo('Tipo Móvil');

    //------------------------------------
    // Init Datos
    //------------------------------------

    categorias = data.CategoriasMotivosArea;
    idArea = data.IdArea;

    //------------------------------------
    // Tabla 
    //------------------------------------

    initTablaResultadoConsulta();
    cargarResultadoConsulta(categorias);

    //------------------------------------
    // Click
    //------------------------------------

    $("#btn_agregarCategoria").click(
        function () {
            insertar()
        });

    $("#inputFormulario_Nombre").on('keydown', function (e) {
        if (e.which == 13) {
            insertar();
            return false;
        }
    });

    $('#selectFormulario_Area').trigger('change');

    $("#inputFormulario_Nombre").trigger('focus');

    setTimeout(function () {
        calcularCantidadDeRows();
    }, 300);
};

function insertar() {
    var obs= $("#inputFormulario_Nombre").val();

    if (obs == "" || obs == undefined) {
        mostrarMensaje('Error', 'Debe ingresar la categoría');
        return;
    }

    var data = {
        Nombre: obs,
        IdArea:idArea 
    }

    crearAjax({
        Data: { comando: data },
        Url: ResolveUrl('~/Servicios/CategoriaMotivoAreaService.asmx/Insertar'),
        OnSuccess: function (result) {
            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            mostrarMensaje('Exito', "La categoría '"+result.Return.Nombre+"' se ha creado correctamente");
            agregarRow(result.Return);
            $("#inputFormulario_Nombre").val('');
            $("#inputFormulario_Nombre").trigger('focus');
        },
        OnError: function (result) {
            mostrarMensaje('Error', 'Error insertando la categoría');
            mostrarMensaje('Error', result.Error);
        }
    });
}

function initTablaResultadoConsulta() {
    $('#tablaCategorias').DataTableGeneral({
        Orden:[[0,'asc']],
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
                Visible: function (data,type,row) {
                    if (data.FechaBaja == null)
                        return true;
                    return false;
                },
                OnClick: function (data) {
                    var obs = data.Nombre;
                    crearDialogoCategoriaEditar({
                        Valor: obs,
                        Callback: function () { actualizarRowEnGrilla(row) },
                        CallbackMensajes: function () { mostrarMensaje(tipo, mensaje) }
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
                        Titulo: '<label>Eliminar Categoría</label>',
                        Content:
                            '<div class="padding">' +
                            '<label id="textoConfirmacion"class="titulo">¿Está seguro de eliminar la categoría?</label>' +
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

                                        var urlAjax = ResolveUrl('~/Servicios/CategoriaMotivoAreaService.asmx/DarDeBaja');

                                        crearAjax({
                                            Url: urlAjax,
                                            Data: { id: data.Id},
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
                                                mostrarMensaje('Exito', 'Categoría eliminada correctamente');

                                                //Cierro el dialogo
                                                $(jAlert).CerrarDialogo();
                                                actualizarRowEnGrilla(result.Return);
                                            },
                                            OnError: function (result) {
                                                //Oculto el cargando
                                                mostrarCargando(false);

                                                //Muestro el Error
                                                mostrarMensaje('Error', 'Error eliminando la categoría');
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

    var dt = $('#tablaCategorias').DataTable();
    dt.page.len(rows).draw();

    console.log(rows);
}

function cargarResultadoConsulta(data) {
    var dt = $('#tablaCategorias').DataTable();

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

function agregarRow(categoria) {
    categorias.push(categoria);
    cargarResultadoConsulta(categorias);
}

function actualizarRowEnGrilla(categoria) {
    //Busco el indice de la categoria a actualizar
    var index = -1;
    var dt = $('#tablaCategorias').DataTable();
    dt.rows(function (idx, data, node) {
        if (data.Id == categoria.Id) {
            index = idx;
        }
    });

    //Si no esta, corto
    if (index == -1) {
        return;
    }

    //Actualizo
    dt.row(index).data(categoria);

    //Inicializo el tooltip
    dt.$('.tooltipped').tooltip({ delay: 50 });

    //Actualizo la info nueva en el array de arriba
    $.each(categorias, function (index, element) {
        if (element.Id == categoria.Id) {
            categorias[index] = categoria;
            return;
        }
    });
}

function getCategorias(){
    return categorias;
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