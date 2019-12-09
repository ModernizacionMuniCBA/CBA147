var persona;

function init(data) {

    data = parse(data);
    persona = data.Persona;

    //Domicilio
    ControlDomicilioDetalle_SetDomicilio(data.Domicilio);
    if(data.Domicilio==undefined){
        $('#contenedorDomicilio').hide();
    }else{
        $('#contenedorDomicilio').show();
    }
}
