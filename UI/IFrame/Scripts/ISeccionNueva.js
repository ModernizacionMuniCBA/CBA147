let modo;
let MODO_ALTA = "alta";
let MODO_EDIT = "edit";

let tablaSeleccionados;
let tablaDisponibles;
let callback;

let empleados = [];
let empleadosSeleccionados = [];

let dadoDeBaja = false;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    //--------------------------
    // Init Data
    //--------------------------
    seccion = data.Seccion;
    idArea = data.IdArea;
    empleados = data.Empleados;

    if (seccion != undefined) {
        modo = MODO_EDIT;
    } else {
        modo = MODO_ALTA;
    }



    switch (modo) {
        case MODO_ALTA: {
            $('#contenedor_Acciones').hide();
        } break;

        case MODO_EDIT: {
            $('#contenedor_Acciones').show();
            if (seccion.FechaBaja != undefined) {
                dadoDeBaja = true;
                $('#btn_DarDeAlta').show();
                $('#btn_DarDeBaja').hide();
            } else {
                $('#btn_DarDeAlta').hide();
                $('#btn_DarDeBaja').show();
            }

            $('#input_Nombre').val(seccion.Nombre);
            $('#input_Observaciones').val(seccion.Observaciones);
            empleadosSeleccionados = seccion.Empleados;

            Materialize.updateTextFields();


            $('#btn_DarDeBaja').click(function () {
                mostrarCargando(true);
                ajax_DarDeBaja(seccion.Id)
                    .then(function (seccion) {
                        informar(seccion);
                    })
                    .catch(function (error) {
                        mostrarCargando(false);
                        top.mostrarMensaje('Error', error);
                    });
            });

            $('#btn_DarDeAlta').click(function () {
                mostrarCargando(true);
                ajax_DarDeAlta(seccion.Id)
                    .then(function (seccion) {
                        informar(seccion);
                    })
                    .catch(function (error) {
                        mostrarCargando(false);
                        top.mostrarMensaje('Error', error);
                    });
            });
        } break;
    }

    initTabla(data);
    cargarEmpleados(data.Empleados.Data);
}


function initTabla(data) {
    tablaSeleccionados = $('#tablaSeleccionados').DataTableGeneral({
        KeyId: 'Id',
        Buscar: true,
        InputBusqueda: '#input_BusquedaSeleccionados',
        Orden: [[0, 'asc']],
        Columnas: [
            {
                title: 'Nombre',
                data: 'Nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data + ' ' + row.Apellido) + '</span></div>';
                }
            }
        ],
        Botones: [
            {
                Titulo: 'Deseleccionar',
                Icono: 'clear',
                Visible: !dadoDeBaja,
                OnClick: function (data) {
                    deseleccionarEmpleado(data.Id);
                }
            }
        ]
    });

    tablaSeleccionados.$('.tooltipped').tooltip({ delay: 50 });
    $('#contenedor_TablaSeleccionados .tabla-footer').empty();
    $('#contenedor_TablaSeleccionados .dataTables_info').detach().appendTo($('#contenedor_TablaSeleccionados .tabla-footer'));
    $('#contenedor_TablaSeleccionados .dataTables_paginate').detach().appendTo($('#contenedor_TablaSeleccionados .tabla-footer'));

    tablaDisponibles = $('#tablaDisponibles').DataTableGeneral({
        KeyId: 'Id',
        Buscar: true,
        InputBusqueda: '#input_BusquedaDisponibles',
        Orden: [[0, 'asc']],
        Columnas: [
            {
                title: 'Nombre',
                data: 'Nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data + ' ' + row.Apellido) + '</span></div>';
                }
            }
        ],
        Botones: [
            {
                Titulo: 'Seleccionar',
                Icono: 'check',
                Visible: !dadoDeBaja,
                OnClick: function (data) {
                    seleccionarEmpleado(data.Id);
                }
            }
        ]
    });

    tablaDisponibles.$('.tooltipped').tooltip({ delay: 50 });
    $('#contenedor_TablaDisponibles .tabla-footer').empty();
    $('#contenedor_TablaDisponibles .dataTables_info').detach().appendTo($('#contenedor_TablaDisponibles .tabla-footer'));
    $('#contenedor_TablaDisponibles .dataTables_paginate').detach().appendTo($('#contenedor_TablaDisponibles .tabla-footer'));

    calcularCantidadDeRows();
}

function calcularCantidadDeRows() {
    var hDisponible = $('#contenedor_TablaSeleccionados .tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaSeleccionados.page.len(rows).draw();

    hDisponible = $('#contenedor_TablaDisponibles .tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaDisponibles.page.len(rows).draw();
}

function cargarEmpleados(empleados) {
    mostrarCargando(true);

    calcularCantidadDeRows();

    //Agrego empleados disponibles
    let empleadosDisponibles = $.grep(empleados, function (element, index) {
        return !esEmpleadoSeleccionado(element.Id);
    });

    //$.each(empleadosDisponibles, function (index, emple) {

    //    tablaDisponibles.draw(false);
    //});
    tablaDisponibles.rows.add(empleadosDisponibles).draw();

    //Agrego empleados seleccionados
    //$.each(empleadosSeleccionados, function (index, idEmpleado) {

    //    tablaSeleccionados.draw(false);
    //});
    tablaSeleccionados.rows.add(empleadosSeleccionados).draw();

    mostrarCargando(false);

}

function toggleEmpleadoSeleccionado(idEmpleado) {
    if (esEmpleadoSeleccionado(idEmpleado)) {
        deseleccionarEmpleado(idEmpleado);
    } else {
        seleccionarEmpleado(idEmpleado);
    }
}

function seleccionarEmpleado(idEmpleado) {
    if (esEmpleadoSeleccionado()) return;

    //Quito de la tabla de disponibles
    let empleado;
    tablaDisponibles.rows().every(function () {
        let d = this.data();
        if (d != undefined) {
            if (d.Id == idEmpleado) {
                empleado = d;
                $(this.node()).addClass('borrado');
            }
        }
    });

    //Lo agrego al listado
    empleadosSeleccionados.push(empleado);
  
    tablaDisponibles.rows('.borrado').remove().draw(false);

    //Agrego a la tabla de seleccionados
    tablaSeleccionados.row.add(empleado).draw(false);
}

function deseleccionarEmpleado(idEmpleado) {
    if (!esEmpleadoSeleccionado(idEmpleado)) return;

    //quito del listado de empleado
    empleadosSeleccionados = $.grep(empleadosSeleccionados, function (element, index) {
        return element.Id != idEmpleado;
    });

    //Quito de la tabla de seleccionados
    let empleado;
    tablaSeleccionados.rows().every(function () {
        let d = this.data();
        if (d != undefined) {
            if (d.Id == idEmpleado) {
                empleado = d;
                $(this.node()).addClass('borrado');
            }
        }
    });

    tablaSeleccionados.rows('.borrado').remove().draw(false);

    //Agrego a la tabla de disponibles
    tablaDisponibles.row.add(empleado).draw(false);
}

function esEmpleadoSeleccionado(idEmpleado) {
    var r = false;
    let b = $.grep(empleadosSeleccionados, function (b1, e1) {
        if (b1.Id == idEmpleado)
            r=true;
    });
    return r;
}

//--------------------------
// Operacion principal
//--------------------------

function registrar() {
    if (!validar()) return;

    mostrarCargando(true, 'Registrando Sección...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/SeccionService.asmx/Insertar'),
        Data: { comando: getSeccion() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function editar() {
    if (!validar()) return;

    mostrarCargando(true, 'Actualizando Seccion...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/SeccionService.asmx/Update'),
        Data: { comando: getSeccion() },
        OnSuccess: function (result) {
            mostrarCargando(false);

            if (!result.Ok) {
                mostrarMensaje('Error', result.Error);
                return;
            }

            informar(result.Return);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function validar() {
    return $('#form').valid();
}

function getSeccion() {

    var s = {};
    if (modo == MODO_EDIT) {
        s.Id = seccion.Id;
    }

    s.IdArea = idArea;
    s.Nombre = "" + $('#input_Nombre').val();
    var obs = "" + $('#input_Observaciones').val();
    if (obs != undefined){
        s.Observaciones = obs;
    }

    var ids = $.map(empleadosSeleccionados, function (e) {
        return e.Id;
    });
    s.IdsEmpleados = ids;
    return s;
}



function ajax_DarDeBaja(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: '~/Servicios/SeccionService.asmx/DarDeBaja',
            Data: {id: id },
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

function ajax_DarDeAlta(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: '~/Servicios/SeccionService.asmx/DarDeAlta',
            Data: { id: id },
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