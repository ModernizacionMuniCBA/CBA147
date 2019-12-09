let modo;
let MODO_ALTA = "alta";
let MODO_EDIT = "edit";

let tablaMoviles;
let tablaEmpleados;
let callback;

let flota;

let empleados = [];
let empleadosSeleccionados = [];

let moviles = [];
let movilSeleccionado;

let dadoDeBaja = false;

let PATH_IMAGEN_USER_MALE;
let PATH_IMAGEN_USER_FEMALE;

function init(data) {
    if ("Error" in data && data.Error != undefined) {
        return;
    }

    PATH_IMAGEN_USER_MALE =
      top.urlCordobaFiles + "/Archivo/" + top.identificadorFotoUserMale + "/3";
    PATH_IMAGEN_USER_FEMALE =
      top.urlCordobaFiles + "/Archivo/" + top.identificadorFotoUserFemale + "/3";

    //--------------------------
    // Init Data
    //--------------------------
    idArea = data.IdArea;
    empleados = data.Empleados.Data;
    moviles = data.Moviles.Data;
    flota = data.Flota;

    if (flota != undefined) {
        modo = MODO_EDIT;
        empleados = empleados.concat(flota.Empleados);
        moviles.push(flota.Movil);

    } else {
        modo = MODO_ALTA;
    }

    initTabla(data);
    cargarEmpleados(empleados);
    cargarMoviles(moviles);

    switch (modo) {
        case MODO_ALTA:
            {
                $("#contenedor_Acciones").hide();
            }
            break;

        case MODO_EDIT:
            {

                $("#contenedor_Acciones").show();
                if (flota.FechaBaja != undefined) {
                    dadoDeBaja = true;
                    $("#btn_DarDeAlta").show();
                    $("#btn_DarDeBaja").hide();
                } else {
                    $("#btn_DarDeAlta").hide();
                    $("#btn_DarDeBaja").show();
                }

                $("#input_Observaciones").val(flota.Observaciones);
                Materialize.updateTextFields();

                $("#btn_DarDeBaja").click(function () {
                    mostrarCargando(true);
                    ajax_DarDeBaja(flota.Id)
                      .then(function (id) {
                          informar(id);
                      })
                      .catch(function (error) {
                          mostrarCargando(false);
                          top.mostrarMensaje("Error", error);
                      });
                });

                //$("#btn_DarDeAlta").click(function () {
                //    mostrarCargando(true);
                //    ajax_DarDeAlta(flota.Id)
                //      .then(function (flota) {
                //          informar(flota);
                //      })
                //      .catch(function (error) {
                //          mostrarCargando(false);
                //          top.mostrarMensaje("Error", error);
                //      });
                //});

                seleccionarMovil(flota.Movil.Id);

                $.each(flota.Empleados, function (i, emp) {
                    seleccionarEmpleado(emp.Id);
                })
            }
            break;
    }
}

function initTabla(data) {
    tablaMoviles = $("#tablaMoviles").DataTableMovil({
        Buscar: true,
        InputBusqueda: "#input_BusquedaMoviles",
        Orden: [[0, "asc"]],
        BotonSeleccionar: true,
        ColumnaDominio: false,
        CallbackSeleccionar: function (data) {
            seleccionarMovil(data);
        }
    });

    tablaMoviles.$(".tooltipped").tooltip({ delay: 50 });
    $("#contenedor_TablaMoviles .tabla-footer").empty();
    $("#contenedor_TablaMoviles .dataTables_info")
      .detach()
      .appendTo($("#contenedor_TablaMoviles .tabla-footer"));
    $("#contenedor_TablaMoviles .dataTables_paginate")
      .detach()
      .appendTo($("#contenedor_TablaMoviles .tabla-footer"));

    tablaEmpleados = $("#tablaEmpleados").DataTableGeneral({
        KeyId: "Id",
        Buscar: true,
        InputBusqueda: "#input_BusquedaEmpleados",
        Orden: [[0, "asc"]],
        Columnas: [
          {
              title: "Nombre",
              data: "Nombre",
              render: function (data, type, row) {
                  return (
                    "<div><span>" +
                    toTitleCase(data + " " + row.Apellido) +
                    "</span></div>"
                  );
              }
          }
        ],
        Botones: [
          {
              Titulo: "Seleccionar",
              Icono: "check",
              Visible: !dadoDeBaja,
              OnClick: function (data) {
                  seleccionarEmpleado(data.Id);
              }
          }
        ]
    });

    tablaEmpleados.$(".tooltipped").tooltip({ delay: 50 });
    $("#contenedor_TablaEmpleados .tabla-footer").empty();
    $("#contenedor_TablaEmpleados .dataTables_info")
      .detach()
      .appendTo($("#contenedor_TablaEmpleados .tabla-footer"));
    $("#contenedor_TablaEmpleados .dataTables_paginate")
      .detach()
      .appendTo($("#contenedor_TablaEmpleados .tabla-footer"));

    calcularCantidadDeRows();
}

function calcularCantidadDeRows() {
    var hDisponible = $("#contenedor_TablaMoviles .tabla-contenedor").height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaMoviles.page.len(rows).draw();

    hDisponible = $("#contenedor_TablaEmpleados .tabla-contenedor").height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaEmpleados.page.len(rows).draw();
}

//--------------------------
// ---     Empleados     ---
//--------------------------
function cargarEmpleados(empleados) {
    mostrarCargando(true);

    calcularCantidadDeRows();

    //Agrego empleados disponibles
    let empleadosDisponibles = $.grep(empleados, function (element, index) {
        return !esEmpleadoSeleccionado(element.Id);
    });

    tablaEmpleados.rows.add(empleadosDisponibles).draw();

    mostrarCargando(false);
}

function seleccionarEmpleado(idEmpleado) {
    if (esEmpleadoSeleccionado()) return;

    //Quito de la tabla de disponibles
    let empleado;
    tablaEmpleados.rows().every(function () {
        let d = this.data();
        if (d != undefined) {
            if (d.Id == idEmpleado) {
                empleado = d;
                $(this.node()).addClass("borrado");
            }
        }
    });

    //Lo agrego al listado
    empleadosSeleccionados.push(empleado);

    tablaEmpleados
      .rows(".borrado")
      .remove()
      .draw(false);

    //Agrego a la tabla de seleccionados
    let html = crearHtmlEmpleado(empleado);
    $("#contenedor_PersonalSeleccionado .contenido").append(html);

    if (empleadosSeleccionados.length > 0) {
        $("#contenedor_PersonalSeleccionadoTitulo .detalle").hide();
    }
}

function deseleccionarEmpleado(idEmpleado) {
    if (!esEmpleadoSeleccionado(idEmpleado)) return;

    let empleado;
    //quito del listado de empleado
    empleadosSeleccionados = $.grep(empleadosSeleccionados, function (
      element,
      index
    ) {
        if (element.Id == idEmpleado) {
            empleado = element;
            return false;
        }

        return true;
    });

    //Quito de la tabla de seleccionados
    $(
      "#contenedor_PersonalSeleccionado .contenido [id=" + idEmpleado + "]"
    ).empty();

    //Quito

    tablaMoviles
      .rows(".borrado")
      .remove()
      .draw(false);

    //Agrego a la tabla de disponibles
    tablaEmpleados.row.add(empleado).draw(false);

    if (empleadosSeleccionados.length == 0) {
        $("#contenedor_PersonalSeleccionadoTitulo .detalle").show();
    }
}

function esEmpleadoSeleccionado(idEmpleado) {
    var r = false;
    let b = $.grep(empleadosSeleccionados, function (b1, e1) {
        if (b1.Id == idEmpleado) r = true;
    });
    return r;
}

function crearHtmlEmpleado(e) {
    var html = $($("#template_Empleado").html());
    $(html).attr("id", e.Id);

    $(html)
      .find(" .nombre")
      .text(e.Nombre + " " + e.Apellido);

    let foto;
    if (e.IdentificadorFotoPersonal != undefined) {
        foto =
          top.urlCordobaFiles +
          "/Archivo/" +
          e.IdentificadorFotoPersonal +
          "/3";
    } else {
        foto =
          e.SexoMasculino == true
            ? PATH_IMAGEN_USER_MALE
            : PATH_IMAGEN_USER_FEMALE;
    }
    $(html)
      .find("img")
      .attr("src", foto);

    //Numero click
    $(html)
      .find(".nombre, .foto")
      .click(function () {
          crearDialogoEmpleadoDetalle({
              Id: e.Id,
              Callback: function () {
                  actualizarDetalle().then(function () {
                      cargarDatos();
                  });
              },
              CallbackMensajes: function (tipo, mensaje) {
                  mostrarMensaje(tipo, mensaje);
              }
          });
      });

    //Boton borrar
    $(html)
      .find(".borrar")
      .click(function () {
          if (e.Id == undefined) return;
          deseleccionarEmpleado(e.Id);
      });

    return html;
}

//--------------------------
// ---    Móviles     ---
//--------------------------

function cargarMoviles(moviles) {
    mostrarCargando(true);

    calcularCantidadDeRows();

    //Agrego moviles disponibles
    let movilesDisponibles = $.grep(moviles, function (element, index) {
        return !esMovilSeleccionado(element.Id);
    });

    tablaMoviles.rows.add(movilesDisponibles).draw();

    mostrarCargando(false);
}

function seleccionarMovil(idMovil) {
    if (esMovilSeleccionado()) return;

    //si hay un movil seleccionado, lo vuelvo a poner en la lista de disponibles
    if (movilSeleccionado != undefined) {
        //Agrego a la tabla de disponibles
        tablaMoviles.row.add(movilSeleccionado).draw(false);
    }

    //Quito de la tabla de disponibles
    let movil;
    tablaMoviles.rows().every(function () {
        let d = this.data();
        if (d != undefined) {
            if (d.Id == idMovil) {
                movil = d;
                $(this.node()).addClass("borrado");
            }
        }
    });

    //Lo agrego al listado
    movilSeleccionado = movil;

    tablaMoviles
      .rows(".borrado")
      .remove()
      .draw(false);

    //Agrego a la tabla de seleccionados
    $("#contenedor_MovilSeleccionado")
      .find(".numero")
      .text(movilSeleccionado.NumeroInterno);

    $("#contenedor_MovilSeleccionado")
      .find(".tipo")
      .text(movilSeleccionado.TipoMovilNombre);

    $("#contenedor_MovilSeleccionado")
      .find(".marca")
      .text(movilSeleccionado.Marca + " " + movilSeleccionado.Modelo);

    //Numero click
    $("#contenedor_MovilSeleccionado")
      .find(".nombre, .foto")
      .click(function () {
          crearDialogoMovilDetalle({
              Id: movilSeleccionado.Id,
              Callback: function () {
                  actualizarDetalle().then(function () {
                      cargarDatos();
                  });
              },
              CallbackMensajes: function (tipo, mensaje) {
                  mostrarMensaje(tipo, mensaje);
              }
          });
      });

    //Boton borrar
    $("#contenedor_MovilSeleccionado")
      .find(".borrar")
      .click(function () {
          if (movilSeleccionado.Id == undefined) return;
          deseleccionarMovil(movilSeleccionado.Id);
      });

    $("#contenedor_MovilSeleccionado")
      .find(".movil")
      .show();

    $("#contenedor_MovilSeleccionado .detalle").hide();
}

function deseleccionarMovil(idMovil) {
    if (!esMovilSeleccionado(idMovil)) return;

    let movil = movilSeleccionado;
    //quito del listado de empleado
    movilSeleccionado = undefined;

    //quito los datos seleccionados
    $("#contenedor_MovilSeleccionado")
      .find(".movil")
      .hide();

    //Quito
    tablaMoviles
      .rows(".borrado")
      .remove()
      .draw(false);

    //Agrego a la tabla de disponibles
    tablaMoviles.row.add(movil).draw(false);

    $("#contenedor_MovilSeleccionado .detalle").show();
}

function esMovilSeleccionado(idMovil) {
    if (movilSeleccionado == undefined) {
        return false;
    }
    return movilSeleccionado.Id == idMovil;
}

//--------------------------
// Operacion principal
//--------------------------

function registrar() {
    if (!validar()) return;

    mostrarCargando(true, "Registrando Flota...");

    crearAjax({
        Url: ResolveUrl("~/Servicios/FlotaService.asmx/Insertar"),
        Data: { comando: getFlota() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function editar() {
    if (!validar()) return;

    mostrarCargando(true, "Actualizando Flota...");

    crearAjax({
        Url: ResolveUrl("~/Servicios/FlotaService.asmx/Editar"),
        Data: { comando: getFlota() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje("Error", result.Error);
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje("Error", "Error procesando la solicitud");
        }
    });
}

function validar() {
    let ok = true;

    if (movilSeleccionado == undefined) {
        ok = false;
        mostrarMensaje("Error", "Debe seleccionar un móvil para la flota");
    }

    if (empleadosSeleccionados.length == 0) {
        ok = false;
        mostrarMensaje(
          "Error",
          "Debe seleccionar al menos un empleado para la flota"
        );
    }

    return ok;
}

function getFlota() {
    var f = {};
    if (modo == MODO_EDIT) {
        f.Id = flota.Id;
    }

    f.IdArea = idArea;
    var obs = "" + $("#input_Observaciones").val();
    if (obs != undefined) {
        f.Observaciones = obs;
    }

    var idsEmpleados = $.map(empleadosSeleccionados, function (e) {
        return e.Id;
    });
    f.IdsEmpleados = idsEmpleados;
    f.IdMovil = movilSeleccionado.Id;
    return f;
}

function ajax_DarDeBaja(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: "~/Servicios/FlotaService.asmx/DarDeBaja",
            Data: { id: id },
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

function ajax_DarDeAlta(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: "~/Servicios/SeccionService.asmx/DarDeAlta",
            Data: { id: id },
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
//-------------------------------
// Registrar
//-------------------------------

function setListener(callbackNuevo) {
    callback = callbackNuevo;
}

function informar(seccion) {
    if (callback == undefined || callback == null) return;
    callback(seccion);
}
