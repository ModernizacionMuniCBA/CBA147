let MODO_EDITAR = "editar";
let MODO_REGISTRAR = "regitrar";
let modo;

let motivo;
let callback;



function init(data) {
    //Cargo los tipos
    $("#select_Area").CargarSelect({
        Data: data.Areas,
        Value: "Id",
        Text: "Nombre",
        Default: "Seleccione...",
        Sort: true
    });

    $("#select_Servicio").CargarSelect({
        Data: data.Servicios,
        Value: "Id",
        Text: "Nombre",
        Default: "Seleccione...",
        Sort: true
    });

    if (data.Categorias != undefined && data.Categorias.length != 0) {
        $("#select_Categoria").CargarSelect({
            Data: data.Categorias,
            Value: "Id",
            Text: "Nombre",
            Default: "Seleccione...",
            Sort: true
        });
    } else {
        $("#select_Categoria").CargarSelect({
            Data: [],
            Value: "Id",
            Text: "Nombre",
            Default: "Seleccione...",
            Sort: true
        });

        $("#select_Categoria").prop('disabled', true);
    }

    if (data.Esfuerzos.length > 0) {
        $("#select_Esfuerzo").CargarSelect({
            Data: data.Esfuerzos,
            Value: "KeyValue",
            Text: "Nombre",
            Default: "Seleccione...",
            Sort: false
        });
    }

    $("#btnAgregarCampo").click(function () {
        if (modo == MODO_EDITAR) {
            crearDialogoCampos();
            return;
        }

        crearDialogoConfirmacion({
            Texto:
              "Para agregar campos dinámicos al motivo, éste ya debe estar creado. ¿Desea confirmar la creación del mismo?",
            CallbackPositivo: function () {
                registrarMotivo().then(function (m) {
                    motivo = m;
                    modo = MODO_EDITAR;

                    crearDialogoCampos();
                });
            }
        });
    });

    $("#select_Area").on("change", function () {
        let idArea = $(this).val();
        ajax_GetServicioByArea(idArea)
          .then(function (data) {
              if (data == undefined) {
                  $("#select_Servicio").prop("disabled", false);
                  $("#select_Servicio")
                    .val(-1)
                    .trigger("change");
              } else {
                  $("#select_Servicio").prop("disabled", true);
                  $("#select_Servicio")
                    .val(data.Id)
                    .trigger("change");
              }
          })
          .catch(function (data) {
              console.log(data);
          });

        cargarCategorias($(this).val());
    });

    $("#btnNuevaCategoria").click(function () {
        crearDialogoCategoriaMotivoAreaNuevo({
            CallbackMensajes: function (tipo, mensaje) {
                mostrarMensaje(tipo, mensaje);
            },
            IdArea: $("#select_Area").val(),
            Callback: function () {
                cargarCategorias($("#select_Area").val());
            }
        });
    });

    //Tabla
    initTablaResultadoConsulta();

    setTimeout(function () {
        if (data.Motivo != undefined) {
            modo = MODO_EDITAR;
            motivo = data.Motivo;

            cargarDatos();
        } else {
            modo = MODO_REGISTRAR;

            if (
              data.IdArea != undefined &&
              data.IdArea != "-1" &&
              data.IdArea != "undefined"
            ) {
                $("#select_Area")
                  .val(data.IdArea)
                  .trigger("change");
            } else {
                if (
                  data.IdServicio != undefined &&
                  data.IdServicio != "-1" &&
                  data.IdServicio != "undefined"
                ) {
                    $("#select_Servicio")
                      .val(data.IdServicio)
                      .trigger("change");
                }
            }

            setTimeout(function () {
                if (
                  data.IdCategoria != undefined &&
                  data.IdCategoria != "-1" &&
                  data.IdCategoria != "undefined"
                ) {
                    $("#select_Categoria")
                      .val(data.IdCategoria)
                      .trigger("change");
                }
            }, 500);
        }

        calcularCantidadDeRows();
    }, 400);
}

function cargarDatos() {
    $("#select_Area")
      .val(motivo.IdArea)
      .trigger("change");
    $("#input_Nombre").val(motivo.Nombre);
    $("#input_Descripcion").val(motivo.Observaciones);
    $("#input_Keywords").val(motivo.Keywords);
    $("#check_Urgente").prop("checked", motivo.Urgente);
    $("#check_Principal").prop("checked", motivo.Principal);
    $("#select_Categoria")
      .val(motivo.IdCategoria)
      .trigger("change");

    $("#select_Esfuerzo")
  .val(motivo.Esfuerzo)
  .trigger("change");

    if (motivo.Prioridad == 1) {
        $("#radio_PrioridadNormal").prop("checked", true);
    } else {
        if (motivo.Prioridad == 2) {
            $("#radio_PrioridadMedia").prop("checked", true);
        } else {
            $("#radio_PrioridadAlta").prop("checked", true);
        }
    }

    if (motivo.Tipo == 1) {
        $("#radio_TipoGeneral").prop("checked", true);
    } else {
        if (motivo.Tipo == 2) {
            $("#radio_TipoInterno").prop("checked", true);
        } else {
            $("#radio_TipoPrivado").prop("checked", true);
        }
    }
    Materialize.updateTextFields();

    if (motivo.Campos != undefined && motivo.Campos.length != 0) {
        calcularCantidadDeRows();
        cargarTablaCampos(motivo.Campos);
    }
}

function validar() {
    let motivo = getMotivo();

    if (motivo.IdArea == -1) {
        mostrarMensajeError("Seleccione el Area");
        return false;
    }

    if (motivo.IdServicio == -1) {
        mostrarMensajeError("Seleccione el servicio");
        return false;
    }

    if (motivo.Nombre == "") {
        mostrarMensajeError("Ingrese el nombre");
        return false;
    }

    if (
      $("#select_Categoria > option").length > 1 &&
      $("#select_Categoria").val() == -1
    ) {
        mostrarMensajeError("Debe seleccionar una categoría");
        return false;
    }

    //if (motivo.Descripcion == "") {
    //    mostrarMensajeError('Ingrese la descripcion');
    //    return false;
    //}

    return true;
}

function registrar() {
    //Si ya se creo el motivo, mando a editar
    if (modo == MODO_EDITAR) {
        editar();
        return;
    }

    registrarMotivo().then(function () {
        informar(true);
    });
}

function registrarMotivo() {
    return new Promise(function (callback) {
        if (!validar()) return;

        mostrarCargando(true);
        ajax_Registrar(getMotivo())
          .then(function (data) {
              mostrarCargando(false);
              callback(data);
              mostrarMensaje("Exito", "Motivo registrado con éxito");
          })
          .catch(function (error) {
              mostrarCargando(false);
              mostrarMensajeError(error);
          });
    });
}

function editar() {
    if (!validar()) return;

    mostrarCargando(true);
    ajax_Editar(getMotivo())
      .then(function (data) {
          informar(true);
      })
      .catch(function (error) {
          mostrarCargando(false);
          mostrarMensajeError(error);
      });
}

function getMotivo() {
    let data = {};
    if (modo == MODO_EDITAR) {
        data.Id = motivo.Id;
    }
    data.IdArea = $("#select_Area").val();
    data.IdServicio = $("#select_Servicio").val();

    if ($("#select_Categoria > option").length > 1) {
        data.IdCategoria = $("#select_Categoria").val();
    }

    data.Nombre = $("#input_Nombre").val();
    data.Descripcion = $("#input_Descripcion").val();
    data.Keywords = $("#input_Keywords").val();
    data.Urgente = $("#check_Urgente").is(":checked");
    data.Principal = $("#check_Principal").is(":checked");

    if ($("#radio_TipoGeneral").is(":checked")) {
        data.Tipo = 1;
    } else {
        if ($("#radio_TipoInterno").is(":checked")) {
            data.Tipo = 2;
        } else {
            data.Tipo = 3;
        }
    }

    if ($("#radio_PrioridadNormal").is(":checked")) {
        data.Prioridad = 1;
    } else {
        if ($("#radio_PrioridadMedia").is(":checked")) {
            data.Prioridad = 2;
        } else {
            data.Prioridad = 3;
        }
    }

    let esfuerzo = $("#select_Esfuerzo").val();
    if (esfuerzo != undefined && esfuerzo != "-1") {
        data.Esfuerzo = parseInt(esfuerzo);
    }
    data.Campos = [];
    return data;
}

function cargarCategorias(idArea) {
    ajax_GetCategoriasByArea(idArea)
      .then(function (data) {
          if (data == undefined || data == [] || data.length == 0) {
              $("#select_Categoria").prop("disabled", true);
              $("#select_Categoria").CargarSelect({
                  Data: [],
                  Value: "Id",
                  Text: "Nombre",
                  Default: "Seleccione...",
                  Sort: true
              });
          } else {
              $("#select_Categoria").prop("disabled", false);
              $("#select_Categoria").CargarSelect({
                  Data: data,
                  Value: "Id",
                  Text: "Nombre",
                  Default: "Seleccione...",
                  Sort: true
              });
          }

          if (motivo.IdCategoria != null) {
              $("#select_Categoria")
                .val(motivo.IdCategoria)
                .trigger("change");
          }
      })
      .catch(function (data) {
          console.log(data);
      });
}

function ajax_GetServicioByArea(idArea) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl("~/Servicios/MotivoService.asmx/GetServicioByIdArea"),
            Data: { idArea: idArea },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}

function ajax_GetCategoriasByArea(idArea) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl("~/Servicios/MotivoService.asmx/GetCategoriasByIdArea"),
            Data: { idArea: idArea },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}

function ajax_Registrar(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl("~/Servicios/MotivoService.asmx/Insertar"),
            Data: { comando: comando },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}

function ajax_Editar(comando) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: ResolveUrl("~/Servicios/MotivoService.asmx/Editar"),
            Data: { comando: comando },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                callbackError("Error procesando la solicitud");
            }
        });
    });
}

//------------------------------
//Campos dinámicos
//------------------------------

function initTablaResultadoConsulta() {
    $("#tablaCamposDinamicos").DataTableCampoPorMotivo({
        InputBusqueda: "#input_BusquedaCampos"
    });

    //Muevo el indicador y el paginado a mi propio div
    $(".tabla-footer").empty();
    $(".dataTables_info")
      .detach()
      .appendTo($(".tabla-footer"));
    $(".dataTables_paginate")
      .detach()
      .appendTo($(".tabla-footer"));
}

function crearDialogoCampos() {
    crearDialogoCampoPorMotivoNuevo({
        IdMotivo: motivo.Id,
        Callback: function (c) {
            agregarRow(c);
        }
    });
}

function calcularCantidadDeRows() {
    var hDisponible = $(".tabla-contenedor").height();
    var rows = calcularCantidadRowsDataTable(hDisponible);

    var dt = $("#tablaCamposDinamicos").DataTable();
    dt.page.len(rows).draw();

    console.log(rows);
}

function cargarTablaCampos(data) {
    var dt = $("#tablaCamposDinamicos").DataTable();

    //Borro los datos
    dt.clear().draw();

    //ordeno la lista
    data = _.sortBy(data, "Orden");

    //Agrego la info nueva
    if (data != null) {
        dt.rows.add(data).draw();
    }

    //Inicializo los tooltips
    dt.$(".tooltipped").tooltip({ delay: 50 });

    calcularCantidadDeRows();
}

function agregarRow(tipo) {
    if (motivo.Campos == null) {
        motivo.Campos = [];
    }
    motivo.Campos.push(tipo);
    cargarTablaCampos(motivo.Campos);
}

function actualizarRowEnGrilla(zona) {
    //Busco el indice de la zona a actualizar
    var index = -1;
    var dt = $("#tablaCamposDinamicos").DataTable();
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
    dt.$(".tooltipped").tooltip({ delay: 50 });

    //Actualizo la info nueva en el array de arriba
    $.each(motivo.Campos, function (index, element) {
        if (element.Id == zona.Id) {
            motivo.Campos[index] = zona;
            return;
        }
    });
}

//-------------------------------
// Registrar
//-------------------------------

function setCallback(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(cerrar) {
    if (callback == undefined) return;
    callback(cerrar);
}
