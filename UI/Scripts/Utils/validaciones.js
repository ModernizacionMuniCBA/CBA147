$(function () {
    $('.validarNumericoEntero').ForzarNumericoEntero();
    $('.validarNombre').ForzarNombre();
});

//jQuery.fn.ForzarTelefono= function () {
//    return this.each(function () {
//        $(this).keydown(function (e) {
//            var key = e.keyCode || 0;
//            var char = String.fromCharCode(key);

//            var texto = $(this).val() + ('' + char);
//            if (!validarTelefono(texto)) {
//                e.preventDefault();
//                return false;
//            }
//        });
//    });
//};


jQuery.fn.ForzarNumericoEntero = function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.keyCode || 0;
            if (key >= 96 && key <= 105) {
                key = key - 48;
            }
            var char = String.fromCharCode(key);

            if (esCopyPaste(e, key) == true) {
                return;
            }

            if (esCaracterEspecial(key) == true) {
                return;
            }

            if (validarEntero('' + char) == false) {
                e.preventDefault();
                return false;
            }
        });
    });
};

jQuery.fn.ForzarNombre = function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;
            var char = String.fromCharCode(key);

            if (esCopyPaste(e, key) == true) {
                return;
            }

            if (esCaracterEspecial(key) == true) {
                return;
            }

            if (validarNombre('' + char) == false) {
                e.preventDefault();
                return false;
            }
        });
    });
};

function validarEntero(texto) {
    var cadenaValida = "1234567890";
    for (var i = 0; i < texto.length; i++) {
        var c = texto.charAt(i);
        if (cadenaValida.indexOf(c) === -1) {
            return false;
        }
    }

    return true;
}

function validarNombre(texto) {
    var cadenaValida = "abcdefghijklmnñopqrstuvwxyzzABCDEFGHIJKLMNÑOPQRSTUVWXYZ' áéíóúÁÉÍÓÚ";
    for (var i = 0; i < texto.length; i++) {
        var c = texto.charAt(i);
        if (cadenaValida.indexOf(c) === -1) {
            return false;
        }
    }

    return true;
}

function validarFecha(fecha, formato) {
    var momentDate = moment(fecha, formato);
    if (momentDate.isValid()) {
        return true;
    } else {
        return false;
    }
}

function validarCuil(sCUIT) {
    sCUIT = sCUIT.replace(/-/g, '');

    var aMult = '5432765432';
    var aMult = aMult.split('');

    if (sCUIT && sCUIT.length == 11) {
        aCUIT = sCUIT.split('');
        var iResult = 0;
        for (i = 0; i <= 9; i++) {
            iResult += aCUIT[i] * aMult[i];
        }
        iResult = (iResult % 11);
        iResult = 11 - iResult;

        if (iResult == 11) iResult = 0;
        if (iResult == 10) iResult = 9;

        if (iResult == aCUIT[10]) {
            return true;
        }
    }
    return false;
}

function validarEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function validarTelefono(telefono) {
    var re = /[0-9\-\(\)\s]+/;
    return re.test(telefono);
}

function esCopyPaste(evt, key) {
    if (evt.ctrlKey) {
        if (key == 67 || key == 86 || key == 88) {
            return true;
        }
    }

    return false;
}

function esCaracterEspecial(key) {


    //Backspace
    if (key == 8) {
        return true;
    }

    //Tab
    if (key == 9) {
        return true;
    }

    //Enter
    if (key == 13) {
        return true;
    }

    //shift, ctrl, alt, pause/break, caps lock
    if (key >= 16 && key <= 20) {
        return true;
    }

    //ESCAPE
    if (key == 27) {
        return true;
    }

    //page up, page down, end, home, left arrow, up arrow, right arrow, down arrow
    if (key >= 36 && key <= 40) {
        return true;
    }

    //INSERT
    if (key == 45) {
        return true;
    }

    //DELETE
    if (key == 46) {
        return true;
    }
    return false;
}