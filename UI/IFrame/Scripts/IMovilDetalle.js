let movil;

function init(data) {
    data = parse(data);

    cargarMovil(data.Movil);
}

function cargarMovil(Movil) {
    movil = Movil;
    //Area  
    $('#textoArea').text(toTitleCase(Movil.NombreArea));

    //Numero Interno
    $('#textoNombre').text(Movil.NombreTipo + ' : '+Movil.Marca + ' '+Movil.Modelo+' - '+ Movil.NumeroInterno);

    //Fecha de incorporacion
    $('#textoFechaIncorporacion').text(Movil.FechaIncorporacionString);

    //Dominio
    $('#textoDominio').text(Movil.Dominio);

    //Carga
    if (Movil.Carga == null || Movil.Carga == 0) {
        $('#contenedorCarga').hide();
    } else {
        $('#contenedorCarga').show();
        $('#textoCarga').text(Movil.Carga);
    }

    //Asientos
    if (Movil.Asientos == null || Movil.Asientos == 0) {
        $('#contenedorAsientos').hide();
    } else {
        $('#contenedorAsientos').show();
        $('#textoAsientos').text(Movil.Asientos);
    }

    //Valuacion
    if (Movil.Valuacion == null || Movil.Valuacion == 0) {
        $('#contenedorValuacion').hide();
    } else {
        $('#contenedorValuacion').show();
        var texto = Movil.Valuacion;
        if (Movil.FechaValuacionString != "") {
            texto = texto + " (" + Movil.FechaValuacionString + ")";
        }
        $('#textoValuacion').text(texto);
    }

    //Km
    if (Movil.Kilometraje == null || Movil.Kilometraje == 0) {
        $('#contenedorKm').hide();
    } else {
        $('#contenedorKm').show();
        var texto = Movil.Kilometraje;
        if (Movil.FechaKilometrajeString != "") {
            texto=texto+" (" + Movil.FechaKilometrajeString + ")";
        }
        $('#textoKm').text(texto);
    }

    //ITV
    if (Movil.VencimientoITVString == "" || Movil.VencimientoITVString == null) {
        $('#contenedorITV').hide();
    } else {
        $('#contenedorITV').show();
        var texto = Movil.VencimientoITVString;
        $('#textoITV').text(texto);
    }

    //TUV
    if (Movil.VencimientoTUVString == "" || Movil.VencimientoTUVString == null) {
        $('#contenedorTUV').hide();
    } else {
        $('#contenedorTUV').show();
        var texto = Movil.VencimientoTUVString;
        $('#textoTUV').text(texto);
    }

    //Taller
    if (Movil.FechaTallerString == "" || Movil.VencimientoITVString == null ) {
        $('#contenedorTaller').hide();
    } else {
        $('#contenedorTaller').show();
        $('#textoTaller').text(Movil.FechaTallerString);
    }

    //Condicion
    if (Movil.NombreCondicion == null || Movil.NombreCondicion.trim() == "") {
        $('#contenedorCondicion').hide();
    } else {
        $('#contenedorCondicion').show();
        $('#textoCondicion').text(Movil.NombreCondicion);
    }

    $('#textoEstado').text(Movil.NombreEstado);

    //Observaciones
    if (Movil.Observaciones == null || Movil.Observaciones.trim() == "") {
        $('#contenedorObservaciones').hide();
    } else {
        $('#contenedorObservaciones').show();
        $('#textoObservaciones').text(Movil.Observaciones);
    }
}

function getId() {
    return movil.Id;
}