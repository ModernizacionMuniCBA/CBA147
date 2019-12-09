let callback;

let idRequerimiento;
let referenteProvisorio;

let generoMasculino = 1;
let generoFemenino = 2;

function init(data) {
    if ('Error' in data) {
        return;
    }

    idRequerimiento = data.IdRequerimiento;
    referenteProvisorio = data.ReferenteProvisorio;

    let generos = [
      { Id: generoMasculino, Nombre: "Masculino" },
      { Id: generoFemenino, Nombre: "Femenino" }
    ];

    $("#select_referenteGenero").CargarSelect({
        Data: generos,
        Default: "Seleccione...",
        Value: "Id",
        Text: "Nombre"
    });

    if (referenteProvisorio) {
        cargarReferenteProvisorio();
    }
}

function cargarReferenteProvisorio() {
    $("#input_referenteTelefono").val(referenteProvisorio.Telefono);
    $("#input_referenteDni").val(referenteProvisorio.DNI);
    $("#input_referenteNombre").val(referenteProvisorio.Nombre);
    $("#input_referenteApellido").val(referenteProvisorio.Apellido);

    if (referenteProvisorio.GeneroMasculino) {
        $("#select_referenteGenero").val(generoMasculino).trigger("change");
    } else {
        $("#select_referenteGenero").val(generoFemenino).trigger("change");
    }

    $("#input_referenteObservaciones").val(referenteProvisorio.Observaciones);

    Materialize.updateTextFields();
}

function getReferenteProvisorio() {
    //Referente
    let referente = {};
    referente.Nombre = $("#input_referenteNombre").val();
    referente.Apellido = $("#input_referenteApellido").val();
    referente.DNI = $("#input_referenteDni").val();
    let genero = $("#select_referenteGenero").val();
    referente.GeneroMasculino = genero == "" + generoMasculino ? true : false;
    referente.Telefono = $("#input_referenteTelefono").val();
    referente.Observaciones = $("#input_referenteObservaciones").val();
    referente.IdRequerimiento = idRequerimiento;
    return referente;
}

function editarReferente() {
    if (!validar()) {
        return;
    }

    mostrarCargando(true, 'Editando referente provisorio...');

    crearAjax({
        Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/EditarReferenteProvisorio'),
        Data: { comando: getReferenteProvisorio() },
        OnSuccess: function (result) {
            //Oculto el cargando
            mostrarCargando(false);

            //algo salio mal
            if (!result.Ok || !result.Return) {
                mostrarMensaje("Error",result.Error);
                return;
            }

            informar();
        },
        OnError: function (result) {
            mostrarCargando(false);
            mostrarMensaje('Error', 'Error procesando la solicitud');
        }
    });
}

function validar() {
    return $('#formNuevo').valid();
}

//-------------------------------
// Listener
//-------------------------------

function setOnListener(c) {
    callback = c;
}

function informar() {
    if (callback == undefined) return;
    callback();
}