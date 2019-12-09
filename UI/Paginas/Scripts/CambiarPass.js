var user;

function init(data) {
    data = parse(data);
    user = data.User;

    $('.btnCambiarPass').click(function () {
        if (!validar()) return;

        cambiarPass();
    });

    $('#input_PassNueva').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnCambiarPass').click();
        }
    })

    $('#input_PassNueva2').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnCambiarPass').click();
        }
    })

    //$('#input_Pass').keydown(function (e) {
    //    if (e.keyCode == 13) {
    //        $('#btnCambiarPass').click();
    //    }
    //})

    setTimeout(function () {
        $('#input_Pass').trigger('focus');
    }, 300);
}

function validar() {
    $('.control-observacion').slideUp(300);

    var validar = true;

    //var pass = $('#input_Pass').val();
    //if (pass == "") {
    //    $('#input_Pass').siblings('.control-observacion').text('Dato requerido');
    //    $('#input_Pass').siblings('.control-observacion').slideDown(300);

    //    validar = false;
    //}

    var passNueva = $('#input_PassNueva').val();
    if (passNueva == "") {
        $('#input_PassNueva').siblings('.control-observacion').text('Dato requerido');
        $('#input_PassNueva').siblings('.control-observacion').slideDown(300);

        validar = false;
    }

    var passNueva2 = $('#input_PassNueva2').val();
    if (passNueva2 == "") {
        $('#input_PassNueva2').siblings('.control-observacion').text('Dato requerido');
        $('#input_PassNueva2').siblings('.control-observacion').slideDown(300);

        validar = false;
    }

    if (validar) {
        if (passNueva != passNueva2) {
            Materialize.toast('Las contraseñas no son son iguales', 5000, 'colorAlerta');
            validar = false;

        }
    }

    return validar;
}

function cambiarPass() {
    //var pass = $('#input_Pass').val();
    var passNueva = $('#input_PassNueva').val();

    var dataAjax = {
        passNueva: passNueva
    };
    dataAjax = JSON.stringify(dataAjax);

    $('.cargando').stop(true, true).fadeIn(300);

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: dataAjax,
        url: ResolveUrl('~/Servicios/UsuarioService.asmx/CambiarPass'),
        success: function (result) {
            result = parse(result.d);

            //Oculto el cargando
            $('.cargando').stop(true, true).fadeOut(300);

            //algo salio mal
            if ('Error' in result) {
                //Informo
                Materialize.toast('Error al cambiar la contraseña', 5000, 'colorError');
                console.log("Error al cambiar la contraseña");
                console.log(dataAjax);
                console.log(result);
                return;
            }

            Materialize.toast('Contraseña cambiada correctamente', 5000, 'colorExito');
            $('#input_Pass').val('');
            $('#input_PassNueva').val('');
            $('#input_PassNueva2').val('');
            Materialize.updateTextFields();
        },
        error: function (result) {
            //Oculto el cargando
            $('.cargando').stop(true, true).fadeOut(300);

            //Informo
            Materialize.toast('Error al cambiar la contraseña', 5000, 'colorError');
            console.log("Error al cambiar la contraseña");
            console.log(dataAjax);
            console.log(result);
        }
    });
}