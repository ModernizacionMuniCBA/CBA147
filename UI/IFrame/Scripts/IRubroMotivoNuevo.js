let modo;
let MODO_ALTA = "alta";
let MODO_EDIT = "edit";

let tablaSeleccionados;
let tablaParaSeleccionar;
let callback;

let rubro;

let motivos = [];
let motivosSeleccionados = [];

let dadoDeBaja = false;

function init(data) {
    if ('Error' in data && data.Error != undefined) {
        return;
    }

    setTimeout(function () {
        mostrarCargando(true);
        setTimeout(function () {
            mostrarCargando(false);
        }, 300);
    }, 50);



    //--------------------------
    // Init Data
    //--------------------------
    rubro = data.RubroMotivo;
    idGrupo = data.IdGrupo;
    motivos = data.Motivos;

    if (rubro != undefined) {
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
            if (rubro.FechaBaja != undefined) {
                dadoDeBaja = true;
                $('#btn_DarDeAlta').show();
                $('#btn_DarDeBaja').hide();
            } else {
                $('#btn_DarDeAlta').hide();
                $('#btn_DarDeBaja').show();
            }

            $('#input_Nombre').val(rubro.Nombre);
            $('#input_Observaciones').val(rubro.Observaciones);
            motivosSeleccionados = rubro.Motivos;

            Materialize.updateTextFields();


            $('#btn_DarDeBaja').click(function () {

                crearDialogoConfirmacion({
                    CallbackPositivo: function () {
                        mostrarCargando(true);
                        ajax_DarDeBaja(rubro.Id)
                          .then(function (rubro) {
                              informar(rubro);
                          })
                          .catch(function (error) {
                              mostrarCargando(false);
                              top.mostrarMensaje('Error', error);
                          });
                    },
                    CallbackNegativo: function () {
                        mostrarCargando(false);
                    },
                    CerrarDialogoBotonCancelar: true,
                    Texto: "¿Está seguro que desea dar de baja la categoría?"
                });
            });

            $('#btn_DarDeAlta').click(function () {
                crearDialogoConfirmacion({
                    CallbackPositivo: function () {
                        mostrarCargando(true);
                        ajax_DarDeAlta(rubro.Id)
                                   .then(function (rubro) {
                                       informar(rubro);

                                   })
                                   .catch(function (error) {
                                       mostrarCargando(false);
                                       top.mostrarMensaje('Error', error);
                                   });
                    },
                    CallbackNegativo: function () {
                        mostrarCargando(false);
                    },
                    CerrarDialogoBotonCancelar: true,
                    Texto: "¿Está seguro que desea dar de alta la categoría?"
                });

            });
        } break;
    }

    initTabla(data);
    cargarMotivos(data.Motivos);
    mostrarCargando(false);
}


function initTabla(data) {
    tablaSeleccionados = $('#tablaSeleccionados').DataTableGeneral({
        KeyId: 'Id',
        Buscar: true,
        InputBusqueda: '#input_BusquedaSeleccionados',
        Orden: [[0, 'asc']],
        Columnas: [
            {
                title: 'Motivo',
                data: 'Nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            }
        ],
        Botones: [
            {
                Titulo: 'Deseleccionar',
                Icono: 'clear',
                Visible: !dadoDeBaja,
                OnClick: function (data) {
                    deseleccionarMotivo(data.Id);
                }
            }
        ]
    });

    tablaSeleccionados.$('.tooltipped').tooltip({ delay: 50 });
    $('#contenedor_TablaSeleccionados .tabla-footer').empty();
    $('#contenedor_TablaSeleccionados .dataTables_info').detach().appendTo($('#contenedor_TablaSeleccionados .tabla-footer'));
    $('#contenedor_TablaSeleccionados .dataTables_paginate').detach().appendTo($('#contenedor_TablaSeleccionados .tabla-footer'));

    tablaParaSeleccionar = $('#tablaParaSeleccionar').DataTableGeneral({
        KeyId: 'Id',
        Buscar: true,
        InputBusqueda: '#input_BusquedaParaSeleccionar',
        Orden: [[0, 'asc']],
        Columnas: [
            {
                title: 'Nombre',
                data: 'Nombre',
                render: function (data, type, row) {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            }
        ],
        Botones: [
            {
                Titulo: 'Seleccionar',
                Icono: 'check',
                Visible: !dadoDeBaja,
                OnClick: function (data) {
                    seleccionarMotivo(data.Id);
                }
            }
        ]
    });

    tablaParaSeleccionar.$('.tooltipped').tooltip({ delay: 50 });
    $('#contenedor_TablaParaSeleccionar .tabla-footer').empty();
    $('#contenedor_TablaParaSeleccionar .dataTables_info').detach().appendTo($('#contenedor_TablaParaSeleccionar .tabla-footer'));
    $('#contenedor_TablaParaSeleccionar .dataTables_paginate').detach().appendTo($('#contenedor_TablaParaSeleccionar .tabla-footer'));

    calcularCantidadDeRows();
}

function calcularCantidadDeRows() {
    var hDisponible = $('#contenedor_TablaSeleccionados .tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaSeleccionados.page.len(rows).draw();

    hDisponible = $('#contenedor_TablaParaSeleccionar .tabla-contenedor').height();
    var rows = calcularCantidadRowsDataTable(hDisponible);
    tablaParaSeleccionar.page.len(rows).draw();
}

function cargarMotivos(motivos) {
    mostrarCargando(true);

    calcularCantidadDeRows();

    //Agrego motivos disponibles
    let motivosParaSeleccionar = $.grep(motivos, function (element, index) {
        return !esMotivoSeleccionado(element.Id);
    });

    $.each(motivosParaSeleccionar, function (index, emple) {

        tablaParaSeleccionar.draw(false);
    });
    tablaParaSeleccionar.rows.add(motivosParaSeleccionar).draw();

    //Agrego motivos seleccionados
    $.each(motivosSeleccionados, function (index, idMotivo) {

        tablaSeleccionados.draw(false);
    });
    tablaSeleccionados.rows.add(motivosSeleccionados).draw();

    mostrarCargando(false);

}

function toggleMotivoSeleccionado(idMotivo) {
    if (esMotivoSeleccionado(idMotivo)) {
        deseleccionarMotivo(idMotivo);
    } else {
        seleccionarMotivo(idMotivo);
    }
}

function seleccionarMotivo(idMotivo) {
    if (esMotivoSeleccionado()) return;

    //Quito de la tabla de disponibles
    let motivo;
    tablaParaSeleccionar.rows().every(function () {
        let d = this.data();
        if (d != undefined) {
            if (d.Id == idMotivo) {
                motivo = d;
                $(this.node()).addClass('borrado');
            }
        }
    });

    //Lo agrego al listado
    motivosSeleccionados.push(motivo);

    tablaParaSeleccionar.rows('.borrado').remove().draw(false);

    //Agrego a la tabla de seleccionados
    tablaSeleccionados.row.add(motivo).draw(false);
}

function deseleccionarMotivo(idMotivo) {
    if (!esMotivoSeleccionado(idMotivo)) return;

    //quito del listado de motivo
    motivosSeleccionados = $.grep(motivosSeleccionados, function (element, index) {
        return element.Id != idMotivo;
    });

    //Quito de la tabla de seleccionados
    let motivo;
    tablaSeleccionados.rows().every(function () {
        let d = this.data();
        if (d != undefined) {
            if (d.Id == idMotivo) {
                motivo = d;
                $(this.node()).addClass('borrado');
            }
        }
    });

    tablaSeleccionados.rows('.borrado').remove().draw(false);

    //Agrego a la tabla de disponibles
    tablaParaSeleccionar.row.add(motivo).draw(false);
}

function esMotivoSeleccionado(idMotivo) {
    var r = false;
    let b = $.grep(motivosSeleccionados, function (b1, e1) {
        if (b1.Id == idMotivo)
            r = true;
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
        Url: ResolveUrl('~/Servicios/RubroMotivoService.asmx/Insertar'),
        Data: { comando: getRubro() },
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

    mostrarCargando(true, 'Actualizando Rubro...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/RubroMotivoService.asmx/Editar'),
        Data: { comando: getRubro() },
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

function getRubro() {

    var s = {};
    if (modo == MODO_EDIT) {
        s.Id = rubro.Id;
    }

    s.IdGrupo = idGrupo;
    s.Nombre = "" + $('#input_Nombre').val();
    var obs = "" + $('#input_Observaciones').val();
    if (obs != undefined) {
        s.Observaciones = obs;
    }

    var ids = $.map(motivosSeleccionados, function (e) {
        return e.Id;
    });
    s.IdsMotivos = ids;
    return s;
}



function ajax_DarDeBaja(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: '~/Servicios/RubroMotivoService.asmx/DarDeBaja',
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

function ajax_DarDeAlta(id) {
    return new Promise(function (callback, callbackError) {
        crearAjax({
            Url: '~/Servicios/RubroMotivoService.asmx/DarDeAlta',
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

function informar(rubro) {
    if (callback == undefined || callback == null) return;
    callback(rubro);
}