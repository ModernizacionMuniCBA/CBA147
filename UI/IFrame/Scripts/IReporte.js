
$(document).ready(function () {
    $('#btn').click(function () {
        imprimir();
    });
});

function init(data) {
    if (data.Error != undefined) {
        mostrarError(data.Error);
        return;
    }
}
function imprimir() {
    reporte.PrintReport();
}
function setPDF(data) {
    $('#contenedorDocumento').empty();
    var obj = $('<object data="' + data + '" type="application/pdf" width="100%" height="100%" id="contenedorPDF">');
    $('#contenedorDocumento').append(obj);
    $('#contenedor_Error').removeClass('visible');
}
/*Estadoisticas*/
function GenerarReporteEstadisticaCPC(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros };
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaCPC'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaOrigen(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros};
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaOrigen'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaEficacia(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros};
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaEficacia'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaResueltos(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros};
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaResueltos'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaArea(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros};
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaArea'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaSubArea(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta, htmlFiltros: htmlFiltros };
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaSubArea'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaServicios(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros};
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaServicios'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaMotivos(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros};
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaMotivos'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaRubros(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta, htmlFiltros: htmlFiltros };
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaRubros'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaZona(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros};
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaZona'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}
function GenerarReporteEstadisticaUsuario(base64, consulta, htmlFiltros) {
    mostrarCargando(true);

    var dataAjax = { ids: base64, consulta: consulta , htmlFiltros: htmlFiltros};
    dataAjax = JSON.stringify(dataAjax);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteEstadisticaUsuario'),
        success: function (result) {
            setPDF(result.d);
            mostrarCargando(false);
        },
        error: function (result) {
            console.log(result.responseJSON);
        }
    });
}

/*Requerimientos*/
function GenerarReporteListadoRequerimientoPdf(ids, filtros) {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteListadoRequerimiento'),
        Data: { ids: ids, filtros: filtros },
        OnSuccess: function (result) {
            
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarError('Error generando el reporte');
                return;
            }

            setPDF(result.Return);
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error generando el reporte');
        }
    });
}

function GenerarReporteRequerimientoListadoV2(ids, filtros) {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteRequerimientoListadoV2'),
        Data: { ids: ids, filtros: filtros },
        OnSuccess: function (result) {

            if (!result.Ok) {
                mostrarCargando(false);
                mostrarError('Error generando el reporte');
                return;
            }

            setPDF(result.Return);
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error generando el reporte');
        }
    });
}

/*OTs*/
function GenerarReporteListadoOrdenTrabajo(ids, filtros) {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteListadoOrdenTrabajo'),
        Data: { ids: ids, filtros: filtros },
        OnSuccess: function (result) {
            
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarError('Error generando el reporte');
                return;
            }

            setPDF(result.Return);
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error generando el reporte');
        }
    });
}
function GenerarReporteListadoOrdenInspeccion(ids, filtros) {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteListadoOrdenInspeccion'),
        Data: { ids: ids, filtros: filtros },
        OnSuccess: function (result) {
            
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarError('Error generando el reporte');
                return;
            }

            setPDF(result.Return);
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error generando el reporte');
        }
    });
}

/*CATALOGOS*/
function GenerarReporteCatalogoUsuarios(tipoCatalogo, idArea) {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteCatalogoUsuarios'),
        Data: { tipoCatalogo: tipoCatalogo , idArea: idArea },
        OnSuccess: function (result) {
            
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                //mostrarError('Error generando el reporte');
                return;
            }

            setPDF(result.Return);
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error generando el reporte');
        }
    });
}
function GenerarReporteCatalogoMotivos(tipoCatalogo, idArea) {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteCatalogoMotivos'),
        Data: { tipoCatalogo: tipoCatalogo , idArea: idArea },
        OnSuccess: function (result) {
            
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                //mostrarError('Error generando el reporte');
                return;
            }

            setPDF(result.Return);
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error generando el reporte');
        }
    });
}
function GenerarReporteCatalogoTareas(tipoCatalogo, idArea) {
    mostrarCargando(true);

    crearAjax({
        Url: ResolveUrl('~/IFrame/IReporte.aspx/GenerarReporteCatalogoTareas'),
        Data: { tipoCatalogo: tipoCatalogo , idArea: idArea },
        OnSuccess: function (result) {
            
            if (!result.Ok) {
                mostrarCargando(false);
                mostrarMensaje('Error', result.Error);
                //mostrarError('Error generando el reporte');
                return;
            }

            setPDF(result.Return);
            mostrarCargando(false);
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarError('Error generando el reporte');
        }
    });
}

function mostrarError(mensaje) {
    $('#contenedor_Error').addClass('visible');
    $('#contenedor_Error label').text(mensaje);
}

