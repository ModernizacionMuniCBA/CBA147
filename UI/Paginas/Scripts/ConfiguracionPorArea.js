var tiposMotivo;
var estadosCreacionOT;

function init(data) {
    data = parse(data)
    data = data.DataInicial;

    $("#select_Area").CargarSelect({
        Data: getUsuarioLogeado().Areas,
        Value: 'Id',
        Text: 'Nombre',
        Sort: true
    });

    //evento de cambio de area
    $("#select_Area").on("change", function () {
        mostrarCargando(true);

        crearAjax({
            Url: ResolveUrl('~/Servicios/ConfiguracionBandejaService.asmx/GetConfiguraciones'),
            Data: { idArea: (this).value },
            OnSuccess: function (result) {
                result = parse(result);

                mostrarCargando(false);

                if (!result.Ok) {
                    mostrarMensaje("Error", result.Error);
                    return result;
                }

                setConfiguracion(result.Return);
            },
            OnError: function (result) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
            }
        })
    });

    $("#select_Area").trigger("change");

    $("#btnGuardar").click(function () {
        mostrarCargando(true);

        crearAjax({
            Url: ResolveUrl('~/Servicios/ConfiguracionBandejaService.asmx/SetConfiguraciones'),
            Data: { comando: getConfiguraciones() },
            OnSuccess: function (result) {
                mostrarCargando(false);
                result = parse(result);

                if (!result.Ok) {
                    mostrarMensaje("Error", result.Error);
                    return result;
                }

                setConfiguracion(result.Return);
            },
            OnError: function (result) {
                mostrarCargando(false);
                mostrarMensaje("Error", result.Error);
            }
        })
    })

    setDataInicial(data);
}

function setDataInicial(data) {
    tiposMotivo = data.TiposMotivo;
    estadosCreacionOT = data.EstadosCreacionOT;

    var primero = true;
    $.each(data.TiposMotivo, function (index, data) {
        var div = $("#contenedor_tipoMotivoBandeja .opciones");
        $(div).AgregarCheckbox({
            Name: data.Nombre,
            Value: 'tipoMotivoBandeja' + data.KeyValue,
        });

        if (primero) {
            $(div).find('#cb' + 'tipoMotivoBandeja' + +data.KeyValue).prop("checked", true);
        }

        $(div).find('#cb' + 'tipoMotivoBandeja' + +data.KeyValue).click(function () {
            $(div).find('[type="checkbox"]').prop("checked", false);
            $(div).find('#cb' + "tipoMotivoBandeja" + +data.KeyValue).prop("checked", true);
        })

        primero = false;
    });

    primero = true;
    $.each(data.EstadosCreacionOT, function (index, data) {
        var div = $("#contenedor_estadoCreacionOT .opciones");
        $(div).AgregarCheckbox({
            Name: data.Nombre,
            Value: "estadoCreacionOT" + data.KeyValue,
        });

        if (primero) {
            $(div).find('#cb' + "estadoCreacionOT" + +data.KeyValue).prop("checked", true);
        }

        $(div).find('#cb' + "estadoCreacionOT" + data.KeyValue).click(function () {
            $(div).find('[type="checkbox"]').prop("checked", false);
            $(div).find('#cb' + "estadoCreacionOT" + +data.KeyValue).prop("checked", true);
        })

        primero = false;
        $(div).find('#cblb' + "estadoCreacionOT" + data.KeyValue).html('<div class="indicador-estado" style="background-color: #' + data.Color + '"/>' + toTitleCase(data.Nombre));
    });
}

function setConfiguracion(data) {
    $("#contenedor_tipoMotivoBandeja .opciones").find('#cb' + 'tipoMotivoBandeja' + data.TipoMotivoDefectoBandeja.KeyValue).trigger("click");
    $("#contenedor_estadoCreacionOT .opciones").find('#cb' + 'estadoCreacionOT' + data.EstadoCreacionOT.KeyValue).trigger("click");
}

function getConfiguraciones() {
    var data = {};

    //Area
    data.IdArea = $("#select_Area").val();

    //Tipos Motivo
    $.each(tiposMotivo, function (i, tipo) {
        if ($("#contenedor_tipoMotivoBandeja .opciones").find('#cb' + 'tipoMotivoBandeja' + tipo.KeyValue).prop("checked")) {
            data.TipoMotivoDefectoBandeja = tipo.KeyValue;
            return false;
        }
    })

    //Estado Creacion OT
    $.each(estadosCreacionOT, function (i, estado) {
        if ($("#contenedor_estadoCreacionOT .opciones").find('#cb' + 'estadoCreacionOT' + estado.KeyValue).prop("checked")) {
            data.EstadoCreacionOT = estado.KeyValue;
            return false;
        }
    })

    return data;
}