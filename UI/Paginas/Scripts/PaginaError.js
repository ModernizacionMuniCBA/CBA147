function init(data) {
    //Mensaje
    $('label').text(data.Mensaje);
    $('label').css('color', data.MensajeColor);

    //Icono
    $('i').text(data.Icono);
    $('i').css('color', data.IconoColor);
}