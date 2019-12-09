let selectAbierto;

function initJs() {

    //evento que tomar el enter en toda la pagina y dispara el buscar...
    $("form").submit(function (e) {
        return false;
    });

    //Contadores
    $('.contador').characterCounter();

    //Fecha Nacimiento
    try {
        $('.date').mask('00/00/0000');


        //CUIT
        $('.cuit').mask('00-00000000-0');
        $('.cuil').mask('00-00000000-0');

    } catch (e) {

    }


    //Toltip
    $('.tooltipped').tooltip({ delay: 50 });


    //Datatables
    try {
        //$.fn.dataTableExt.oStdClasses.sPageButton = "btn-flat btn-redondo chico btn-paginacion waves-effect";
        $.fn.dataTableExt.oStdClasses.sPageButton = "btn-flat btn-paginacion waves-effect";
        $.fn.dataTableExt.oStdClasses.sPageButtonActive = "grey white-text waves-light";

        jQuery.extend(jQuery.fn.dataTableExt.oSort, {
            "date-euro-pre": function (a) {
                return moment(a, 'DD/MM/YYYY hh:mm:ss');
            },

            "date-euro-asc": function (a, b) {
                return a.diff(b, 'seconds');
            },

            "date-euro-desc": function (a, b) {
                return b.diff(a, 'seconds');
            }
        });
    } catch (e) {

    }

    //Traduccion date picker
    $.extend($.fn.pickadate.defaults, {
        monthsFull: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthsShort: ['ene', 'feb', 'mar', 'abr', 'may', 'jun', 'jul', 'ago', 'sep', 'oct', 'nov', 'dic'],
        weekdaysFull: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        weekdaysShort: ['dom', 'lun', 'mar', 'mié', 'jue', 'vie', 'sáb'],
        weekdaysLetter: ['D', 'L', 'M', 'M', 'J', 'V', 'S'],
        labelMonthNext: 'Mes siguiente',
        labelMonthPrev: 'Mes anterior',
        labelMonthSelect: 'Selecionar mes',
        labelYearSelect: 'Seleccionar año',
        today: 'hoy',
        clear: 'borrar',
        close: 'cerrar',
        firstDay: 1,
        format: 'dddd d !de mmmm !de yyyy',
        formatSubmit: 'dd/mm/yyyy'
    });

    //JAlert default
    try {
        $.fn.jAlert.defaults.size = 'lg';
        $.fn.jAlert.defaults.class = 'no-padding';
        $.fn.jAlert.defaults.closeBtn = 'false';
        $.fn.jAlert.defaults.closeOnEsc = 'false';

    } catch (ex) {
        console.log(ex);
    }

    //Ajax cancel
    setupCancelarAjax();

    initForms();

    // (*) Magia    
    //Cerrar los Select2 cuando hago click afuera del iframe que es padre de el mismo.
    $('html').click(function (e) {
        if (top.selectAbierto != undefined) {

            let iframes = $('iframe');
            cerrarRecursivo(iframes);

            function cerrarRecursivo(iframes) {
                $.each(iframes, function (index, iframe) {
                    iframe.contentWindow.$('select').select2('close');

                    let i = $(iframe).contents().find('iframe');
                    if (i != undefined && i.length != 0) {
                        cerrarRecursivo(i);
                    }
                });
            }
        }
    })
}



function initForms() {
    $('form input').keypress(function (e) {
        if (e.which == 13) {
            $(this).parents('form').submit();
            return false;
        }
    });

    try {
        jQuery.validator.addClassRules("select-requerido", {
            select_requerido: true
        });

        var inputs = $(".input-field > input, .input-field > textarea, .input-field > select, .mi-input-field > select");

        $.each(inputs, function (index, input) {
            var contenedorError = $(input).siblings(".input-error");
            if ($(input).siblings(".input-error").length == 0) {
                contenedorError = $('<div class="input-error">');
                $(contenedorError).append('<a>');
                $(input).parent().append(contenedorError);
            }
        });

        try {
            $('select').select2().on("change", function (e) {
                try {
                    $(this).valid(); //jquery validation script validate on change                
                }
                catch (ex) {

                }
            });
        } catch (e) {
            console.log(e);
        }

        //Forms
        $.validator.setDefaults({
            ignore: [],
            rules: {
                dateField: {
                    fecha: true
                },
                date: {
                    fecha: true
                },
                nombre: {
                    lettersonly: true
                },
                apellido: {
                    lettersonly: true
                },
                numeroEntero: {
                    number: true
                },
                dni: {
                    number: true,
                    minlength: 7
                },
                cuil: {
                    cuil: true
                },
                email: {
                    email: true
                },
                telefono: {
                    number: true
                },
                repeatPassword: {
                    equalTo: "#input_Password"
                },
                selectRequerido: {
                    select_requerido: true
                }
            },
            messages: {
                repeatPassword: "Las contraseñas no coinciden",
            },
            errorClass: 'error',
            validClass: "",
            success: function (label) {

            },
            errorPlacement: function (error, element) {
                var contenedorError = $(element).siblings(".input-error");
                $(contenedorError).find('a').text(error.text());
            },
            submitHandler: function (form) {
                console.log('form ok');
            },
        });

        //Traduccion del validador
        jQuery.extend(jQuery.validator.messages, {
            required: "Dato requerido.",
            remote: "Verificar dato.",
            email: "E-mail inválido.",
            url: "URL inválida.",
            date: "Fecha inválida.",
            dateISO: "Fecha inválida.",
            number: "Dato inválido.",
            digits: "Solo se permiten digitos.",
            creditcard: "Dato inválido.",
            equalTo: "Los datos debes ser iguales.",
            accept: "Extensión invalida.",
            confirmarPassword: "Las contraseñas no coinciden",
            maxlength: jQuery.validator.format("Máximo {0} caracteres."),
            minlength: jQuery.validator.format("Al menos {0} caracteres."),
            rangelength: jQuery.validator.format("Ingrese entre {0} y {1} caracteres."),
            range: jQuery.validator.format("Ingrese un valor entre {0} y {1}."),
            max: jQuery.validator.format("Ingrese un valor igual o menor a {0}."),
            min: jQuery.validator.format("Ingrese un valor igual o mayor a {0}.")
        });

        //Validador de los selects
        jQuery.validator.addMethod("select_requerido", function (value, element) {
            return this.optional(element) || (parseInt(value) != -1);
        }, "Dato requerido.");
        jQuery.validator.addMethod("lettersonly", function (value, element) {
            return this.optional(element) || /^[a-z áãâäàéêëèíîïìóõôöòúûüùçñ]+$/i.test(value);
        }, "Solo letras permitidas");
        jQuery.validator.addMethod("fecha", function (value, element) {
            try{
                var a = this.optional(element);
                var b = moment(value, "DD/MM/YYYY", true).isValid();
                var resultado = a || b;
                return resultado;
            } catch (ex) {
                return true;
            }
        }, "Fecha inválida");
        jQuery.validator.addMethod("fechaMenorQueHoy", function (value, element) {
            return validarFechaMenorQueHoy(value)
        }, "La fecha debe ser menor a la actual");
        jQuery.validator.addMethod("fechaMayorQueHoy", function (value, element) {
            return !validarFechaMenorQueHoy(value);
        }, "La fecha debe ser mayor a la actual");
        jQuery.validator.addMethod("date", function (value, element) {
            try{
                let a = this.optional(element);
                let b;
                if (value.indexOf('-') != -1) {
                    b = moment(value, "YYYY-MM-DD", true).isValid();
                } else {
                    b = moment(value, "DD/MM/YYYY", true).isValid();
                }
                var resultado = a || b;
                return resultado;
            } catch (ex) {
                return true;
            }
        }, "Fecha inválida");
        jQuery.validator.addMethod("cuil", function (value, element) {
            var cuil = validaCuit(value);
            return this.optional(element) || cuil;
        }, "Cuil invalido");
        jQuery.validator.addMethod("año", function (value, element) {
            var año = validarAño(value);
            return this.optional(element) || año;
        }, "Año invalido");

        jQuery.validator.addMethod("fede", function (value, element) {
            var cuil = value == 'fede';
            return this.optional(element) || cuil;
        }, "Solo fede permitido");
    } catch (e) {

    }

}

function validaCuit(sCUIT) {
    sCUIT = sCUIT.replace(/\-/g, '');
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

function validarAño(year) {

    var text = /^[0-9]+$/;
    if (year != 0) {
        if ((year != "") && (!text.test(year))) {
            return false;
        }

        if (year.length != 4) {
            return false;
        }
        var current_year = new Date().getFullYear();
        if ((year < 1920) || (year > current_year)) {
            return false;
        }
        return true;
    }
}

function ordenarJSON(data, prop, asc) {
    var datasort = data.sort(function (a, b) {


        let valorA = a[prop];
        let valorB = b[prop];

        try {
            valorA = a[prop].toLowerCase();
            valorB = b[prop].toLowerCase();
        } catch (ex) {

        }

        if (asc) {
            return (valorA > valorB) ? 1 : ((valorA < valorB) ? -1 : 0);
        } else {
            return (valorB > valorA) ? 1 : ((valorB < valorA) ? -1 : 0);
        }
    });
    return datasort;
}


//----------------------------------------
// AJAX
//----------------------------------------

var xhrPool = [];
function setupCancelarAjax() {
    $(document).ajaxSend(function (e, jqXHR, options) {
        xhrPool.push(jqXHR);
    });
    $(document).ajaxComplete(function (e, jqXHR, options) {
        xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR });
    });

    var oldbeforeunload = window.onbeforeunload;
    window.onbeforeunload = function () {
        var r = oldbeforeunload ? oldbeforeunload() : undefined;
        if (r == undefined) {
            // only cancel requests if there is no prompt to stay on the page
            // if there is a prompt, it will likely give the requests enough time to finish
            cancelarAjax();
        }
        return r;
    }
}

function cancelarAjax() {
    $.each(xhrPool, function (idx, jqXHR) {
        jqXHR.abort();
    });
};

function crearAjax(valores) {

    return $.ajax({
        url: ResolveUrl(valores.Url),
        data: JSON.stringify(valores.Data),
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        type: 'POST',
        success: function (result) {
            result = result.d;
            result = parse(result);
            valores.OnSuccess(result);
        },
        error: function (result) {
            valores.OnError(result);
            console.log(valores.Url);
            console.log(result);
            console.log(valores.Data);

            if (result.readyState == 0) {
                try {
                    //top.SinConexion();
                }
                catch (e) {

                }
            }
        }
    });
}




jQuery.fn.CargarSelect = function (valores) {

    if (!('Data' in valores) || valores.Data == null) {
        alert('Debe enviar Data');
        return;
    }

    if (!('Value' in valores) || valores.Value == null) {
        alert('Debe enviar Key');
        return;
    }

    if (!('Text' in valores) || valores.Text == null) {
        alert('Debe enviar Text');
        return;
    }

    var titleCase = true;
    if ('TitleCase' in valores) {
        titleCase = valores.TitleCase;
    }

    //Creo mis array de datos
    var opciones = [];
    $.each(valores.Data, function (index, val) {
        var texto = val[valores.Text];
        if (titleCase) {
            texto = toTitleCase(texto);
        }
        var mihtml = null;
        if ('html' in val) {
            mihtml = val.html;
        }
        opciones[index] = { id: val[valores.Value], text: texto, html: mihtml };
    });

    //ordeno
    if (!('Sort' in valores)) {
        valores.Sort = true;
    }

    if (!('Asc' in valores)) {
        valores.Asc = true;
    }

    if (!('Multiple' in valores)) {
        valores.Multiple = false;
    }

    if (valores.Sort) {
        var ascendente = valores.Asc;
        valores.Data = ordenarJSON(opciones, 'text', ascendente);
    }

    //Agrego por defecto
    if ('Default' in valores && valores.Default != null) {
        var opciones2 = [];
        opciones2[0] = { id: -1, text: valores.Default };
        $.each(opciones, function (index, val) {
            opciones2[index + 1] = val;
        });
        opciones = opciones2;
    }

    ////Cargo en el select
    //var output = "";
    //$.each(opciones, function (index, val) {
    //    var text = val.text;
    //    if (val.html != null) {
    //        text = val.html;
    //    }
    //    output += '<option value="' + val.value + '" data-foo="">' + text + '</option>';
    //});
    //$(this).html(output);

    $(this).empty();
    var parent = window.top.$('body');

    $(this).select2({
        multiple: valores.Multiple,
        data: opciones, dropdownParent: parent,
        templateResult: function (d) {
            if (d.html) {
                return $(d.html);
            }
            return d.text;
        },
        templateSelection: function (d) {
            if (d.html) {
                return $(d.html);
            }
            return d.text;
        },
        "language": {
            "noResults": function () {
                return "Sin resultados";
            }
        }
    })
        .on('select2:open', function () {

            let select = $(this);
            setTimeout(function () {
                top.selectAbierto = select;
            }, 100);

            var iframe = $(this)[0].ownerDocument.defaultView.frameElement;
            var xOffset = 0;
            var yOffset = 0;
            while (iframe != undefined) {
                xOffset += $(iframe).offset().left;
                yOffset += $(iframe).offset().top;
                iframe = $(iframe)[0].ownerDocument.defaultView.frameElement;
            }

            var xAnterior = parseInt(window.top.$('.select2-container--open').css('left'), 10);
            var xNuevo = xAnterior + xOffset;

            var yAnterior = parseInt(window.top.$('.select2-container--open').css('top'), 10);
            var yNuevo = yAnterior + yOffset;

            window.top.$('.select2-container--open').css('z-index', 999999);
            window.top.$('.select2-container--open').css('margin-left', xOffset);
            window.top.$('.select2-container--open').css('margin-top', yOffset);
        })
        .on('select2:close', function () {
            top.selectAbierto = undefined;

            $(this).siblings('span.select2').css('z-index', '1');
        });
}

jQuery.fn.AgregarCheckbox = function (valores) {
    $('<input />', { type: 'checkbox', value: valores.Value, id: 'cb' + valores.Value }).appendTo($(this));
    $('<label />', { 'for': 'cb' + valores.Value, color: 'black', text: valores.Name, width: '180px', id: 'cblb' + valores.Value }).appendTo($(this));

}

jQuery.fn.AgregarIndicadorCargando = function (valores) {
    if (valores == undefined) valores = {};
    var opaco = false;
    if ('Opaco' in valores) {
        opaco = valores.Opaco;
    }

    var clases = opaco ? 'opaco' : '';

    var spinner = ' <div class="preloader-wrapper big active"><div class="spinner-layer"><div class="circle-clipper left"><div class="circle"></div></div><div class="gap-patch"><div class="circle"></div></div><div class="circle-clipper right"><div class="circle"></div></div></div></div>';
    var divCargando = "<div id='indicadorCargando' class='contenedor' style='position:absolute; top:0; left:0; margin:0; padding:0; width:100%; height:100%'><div class='cargando " + clases + "'>" + spinner + "</div></div>";
    $(divCargando).appendTo($(this));
}

jQuery.fn.GetIndicadorCargando = function (valores) {
    return $(this).find('#indicadorCargando');
}

jQuery.fn.MostrarIndicadorCargando = function (valores) {
    $(this).AgregarIndicadorCargando(valores);
    $(this).find('#indicadorCargando').fadeIn(300);
}

jQuery.fn.OcultarIndicadorCargando = function (valores) {
    $(this).find('#indicadorCargando').fadeOut(300);
}

function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}

function parse(json) {
    if (json == null || json == undefined || json == "" || typeof json != 'string') {
        return json;
    }
    ////json = json.replace(/\\n/g, "\\n")
    ////           .replace(/\\'/g, "\\'")
    ////           .replace(/\\"/g, '\\"')
    ////           .replace(/\\&/g, "\\&")
    ////           .replace(/\\r/g, "\\r")
    ////           .replace(/\\t/g, "\\t")
    ////           .replace(/\\b/g, "\\b")
    ////           .replace(/\\f/g, "\\f");
    ////json = json.replace(/[\u0000-\u0019]+/g, "");

    try {
        json = JSON.parse(json);
    } catch (e) {
        json = json.replace(/\\/g, "");
        json = JSON.parse(json);
    }
    return json;
}

String.prototype.endsWith = function (pattern) {
    var d = this.length - pattern.length;
    return d >= 0 && this.lastIndexOf(pattern) === d;
};

function isMobile() {
    var check = false;
    (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
    return check;
};

function isMobileOrTablet() {
    var check = false;
    (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
    return check;
};

function toTitleCase(str) {
    if (str == null) return "";
    str = str.toLowerCase();
    return str.replace(/(?:^|\s)\w/g, function (match) {
        return match.toUpperCase();
    });
}

jQuery.fn.enterKey = function (fnc) {

    $(this).on('enterKey', function () {
        fnc.call(this);
    });

    return this.each(function () {
        $(this).keypress(function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == '13') {
                fnc.call(this, ev);
            }
        })
    });
}

function dateTimeToString(date) {
    return moment(date).locale('es').format("DD/MM/YYYY HH:mm:ss");
}

function dateToString(date) {
    return moment(date).locale('es').format("DD/MM/YYYY");
}

function validarFechaMenorQueHoy(value) {
    var fecha = moment(value, "DD/MM/YYYY", true);
    var currentDate = moment(new Date(), "DD/MM/YYYY", true);

    if (fecha > currentDate) {
        return false;
    }
    return true;
}

/* Archivos */

function crearDialogoPDF(valores) {
    if (valores == undefined) valores = {};

    var archivo = valores.Archivo;

    /*
    Archivo: [
        {
            Data: info del archivo (url, data lo que sea),
            Nombre: nombre del archivo
        }
    ]
    */


    crearDialogoIFrame({
        Url: ResolveUrl('~/Iframe/IVisorArchivo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setPDF(archivo);
        },
        Botones: [
            {
                Texto: 'Descargar',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.descargar();
                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function crearDialogoDocumento(valores) {
    if (valores == undefined) valores = {};

    var archivo = valores.Archivo;

    /*
    Archivo: [
        {
            Data: info del archivo (url, data lo que sea),
            Nombre: nombre del archivo
        }
    ]
    */


    crearDialogoIFrame({
        Url: ResolveUrl('~/Iframe/IVisorArchivo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setDocumento(archivo);
        },
        Botones: [
            {
                Texto: 'Descargar',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.descargar();
                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function crearDialogoImagenes(valores) {
    if (valores == undefined) valores = {};

    /*
    Imagenes: [
        {
            Data: info de la imagen,
            Nombre: nombre del archivo de imagen
        }
    ]
    */

    var imagenes = valores.Imagenes;
    var index = valores.Index;

    crearDialogoIFrame({
        Url: ResolveUrl('~/Iframe/IVisorArchivo.aspx'),
        OnLoad: function (jAlert, iFrame, iFrameContent) {
            iFrameContent.setImagenes(imagenes, index);
        },
        Botones: [
            {
                Texto: 'Descargar',
                OnClick: function (jAlert, iFrame, iFrameContent) {
                    iFrameContent.descargar();
                }
            },
            {
                Texto: 'Aceptar',
                Class: 'colorExito'
            }
        ]
    });
}

function descargarArchivo(archivo) {
    /*
    Archivo: {
        Nombre: Nombre del archivo a descargar,
        Data: contenido del archivo a descargar
    }
    */

    var link = document.createElement('a');
    document.body.appendChild(link);
    link.download = archivo.Nombre;
    link.href = archivo.Data;
    link.click();
    $(link).remove();
}


/* DataTables */

var w_ColumnaFecha = '15m0px';
var w_ColumnaBoton = 45;

var botonesTablas = [];

jQuery.fn.DataTableGeneral = function (valores) {
    var idTabla = '' + $(this).prop('id');
    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableReclamo2 = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[2, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------
    var cols = [];
    var claseActivo = " activo ";

    //Col Dias
    var verDias = false;
    if ('VerDias' in valores) {
        verDias = valores.VerDias;
    }

    if (verDias) {
        cols.push({
            "sTitle": "Días",
            "mData": "Dias",
            "width": "80px",
            render: function (data, type, row) {
                return '<div><span>' + data + ' días' + '</span></div>';
            }
        });
    }

    //Col Indicadores
    var verIndicadores = true;
    if ('VerIndicadores' in valores) {
        verIndicadores = valores.VerIndicadores;
    }

    if (verIndicadores) {
        cols.push({
            title: '',
            //title: '<div><label style="display:none;">a</label><div><i class="material-icons tooltipped" data-position="bottom" data-delay="50" data-position="bottom" data-tooltip="Prioridad baja">flag</i></div>',
            data: "Prioridad",
            orderable: false,
            "width": "10px",
            render: function (data, type, row) {
                //Prioridad
                var prioridad = "";
                switch (data) {
                    case 1: {
                        prioridad = '<i class="material-icons indicador activo colorTextoPrioridadNormal tooltipped" data-position="bottom" data-delay="50" data-tooltip="Prioridad baja">flag</i>';
                    } break;

                    case 2: {
                        prioridad = '<i class="material-icons indicador activo colorTextoPrioridadMedia tooltipped" data-position="bottom" data-delay="50" data-tooltip="Prioridad media">flag</i>';

                    } break;

                    case 3: {
                        prioridad = '<i class="material-icons indicador activo colorTextoPrioridadAlta tooltipped" data-position="bottom" data-delay="50" data-tooltip="Prioridad alta">flag</i>';

                    } break;
                }

                //Estado
                var estado = '<i class="material-icons tooltipped"  style="color: #' + row.EstadoColor + '" data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(row.EstadoNombre) + '">swap_vertical_circle</i>';

                //Urgente A.K.A Peligroso
                var apagado = row.Urgente == false ? "" : claseActivo;
                var tooltip = row.Urgente == false ? "No peligroso" : "Peligroso";
                var urgente = '<div><i class="material-icons tooltipped colorTextoPrioridadAlta indicador' + apagado + '" data-position="bottom" data-delay="50" data-tooltip="' + tooltip + '">warning</i>';

                //Orden especial
                apagado = row.OrdenEspecialId == null ? "" : claseActivo;
                tooltip = row.OrdenEspecialId == null ? "Sin Orden de Atención Crítica" : "Orden de Atención Crítica " + toTitleCase(row.EstadoOrdenEspecialNombre);
                var ordenEspecial = '<div><i class="material-icons tooltipped indicador' + apagado + '" data-position="bottom" style="color: #' + row.EstadoOrdenEspecialColor + '" data-delay="50" data-tooltip="' + tooltip + '">security</i>';

                //Marcado
                apagado = row.Marcado == false ? "" : claseActivo;
                tooltip = row.Marcado == false ? "En control de Área Operativa" : "En control de CPC";
                var marcado = '<div class="flex"><label class="tooltipped indicador' + apagado + '"  data-position="bottom" data-delay="50" data-tooltip="' + tooltip + '" style="border-radius: 25px; background: #9F25F6; color: white;padding: 0.2rem;font-weight: bolder; font-size: 0.5rem; line-height: 1rem;">CPC</label></div>';

                //Inspeccionado
                apagado = row.Inspeccionado == false ? "" : claseActivo;
                tooltip = row.Inspeccionado == false ? "Sin inspección" : "Inspeccionado";
                var inspeccionado = '<div><i style="padding-left: 3px" class="material-icons tooltipped indicador indicador-inspeccionado' + apagado + '" data-position="bottom"  data-delay="50" data-tooltip="' + tooltip + '">done_all</i>';

                //Favorito
                var favorito = "";
                if (row.Favorito != undefined) {
                    apagado = row.Favorito == false ? "" : claseActivo;
                    tooltip = row.Favorito == false ? "No es favorito" : "Favorito";
                    favorito = '<div class="btnToggleFavorito no-select indicador indicador-favorito ' + apagado + '" id-rq="' + row.Id + '"><i class="material-icons tooltipped" data-position="bottom" data-delay="50" data-tooltip="' + tooltip + '">star</i>';
                }


                return '<div><label style="display:none;">a</label>' + estado + prioridad + urgente + ordenEspecial + marcado + inspeccionado + favorito + '</div>';
            }
        });
    }

    var motivoWidth = "100px";
    if (valores.MotivoWidth != undefined) {
        motivoWidth = valores.MotivoWidth;
    }

    //Boton Favorito
    $('#' + idTabla).on('click', '.btnToggleFavorito', function () {

        $('.material-tooltip').hide();

        var idRq = $(this).attr('id-rq');
        var row = buscarRowPorId(idTabla, idRq);

        var btn = $(this);

        valores.CallbackCargando(true);

        var url = ResolveUrl('~/Servicios/RequerimientoService.asmx/ToggleFavorito');

        crearAjax({
            Url: url,
            Data: { id: idRq },
            OnSuccess: function (result) {

                if (!result.Ok) {
                    valores.CallbackCargando(false);
                    valores.CallbackMensajes('Error', result.Error);
                    return;
                }

                convertirResultadoTabla(idRq, function (rqTabla) {
                    if (rqTabla == undefined) return;
                    actualizarRequerimientoEnGrilla(rqTabla);

                    try {
                        header_BuscarCantidadRequerimientosFavoritos();
                    } catch (e) {

                    }

                    valores.CallbackCargando(false);

                }, function () {
                    valores.CallbackCargando(false);
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                });

            },
            OnError: function (result) {
                valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                valores.CallbackCargando(false);
            }
        });
    });

    cols.push(
        {
            "sTitle": "Número",
            "mData": "Numero",
            "width": "100px",
            render: function (data, type, row) {
                return '<div><span class="link-numero tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-rq="' + row.Id + '">' + row.Numero + '/' + row.Año + '</span></div>';
            }
        },
        {
            "sTitle": "Fecha Alta",
            "mData": "FechaAlta",
            "width": "75px",
            "type": 'date-euro',
            render: function (data, type, row) {
                var fecha = moment(data);
                return '<div><span>' + fecha.format('DD/MM/YYYY HH:mm') + '</span></div>';
            }
        },
        {
            "sTitle": "Motivo",
            "mData": "MotivoNombre",
            "width": motivoWidth,
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }

        }
    );
    $('#' + idTabla).on('click', '.link-numero', function () {
        var idRq = $(this).attr('id-rq');
        verDetalle(idRq);
    });

    $('#' + idTabla).on('dblclick', 'tr', function (e) {
        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
        verDetalle(id);
    });

    function verDetalle(idRq) {
        $('.material-tooltip').hide();

        crearDialogoRequerimientoDetalle({
            Id: idRq,
            CallbackMensajes: function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            },
            CallbackCargando: function (mostrar, mensaje) {
                valores.CallbackCargando(mostrar, mensaje);
            },
            Callback: function () {
                convertirResultadoTabla(idRq, function (rqTabla) {
                    if (rqTabla == undefined) return;
                    actualizarRequerimientoEnGrilla(rqTabla);

                    try {
                        header_BuscarCantidadRequerimientosFavoritos();
                    } catch (e) {

                    }

                    valores.Callback(rqTabla);
                }, function () {
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                });
            }
        });
    }

    //Col barrio
    var verUbicacion = true;
    if ('VerUbicacion' in valores) {
        verUbicacion = valores.VerUbicacion;
    }


    if (verUbicacion) {
        cols.push({
            title: "Ubicación",
            data: "BarrioNombre",
            render: function (data, type, row) {
                let direccion = "";

                if (data == undefined) {
                    return '<div><span>Sin datos</span></div>';
                }

                direccion = "Barrio: " + row.BarrioNombre;

                let detalle = "";
                if (row.DomicilioDireccion != undefined) {
                    detalle = "Direccion: " + row.DomicilioDireccion;
                }

                if (row.DomicilioObservaciones != undefined) {
                    if (detalle != "") detalle += " - ";
                    detalle += row.DomicilioObservaciones;
                }

                if (detalle != "") {
                    direccion = direccion + " </br> " + detalle;
                }

                return '<div><span>' + toTitleCase(direccion) + '</span></div>';
            }
        })
    }


    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    var botones = [];

    if ('BotonesAntes' in valores) {
        $.each(valores.BotonesAntes, function (index, val) {
            botones.push(val);
        });
    }


    //botones.push({
    //    Titulo: 'Crear Orden de Atención Crítica',
    //    Icono: 'local_hospital',
    //    Visible: function (data) {
    //        if (!crearOrdenEspecial) {
    //            return false;
    //        }
    //        if (data.OrdenEspecialId != null) {
    //            return false;
    //        }
    //    },
    //    Validar: function (data) {
    //        if (data.OrdenEspecialId != null) {
    //            mostrarMensaje('Info', 'Éste requerimiento ya tiene una orden especial.');
    //            return false;
    //        }
    //        return true;
    //    }
    //});


    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }


    valores.Botones = botones;


    function convertirResultadoTabla(id, callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/RequerimientoService.asmx/GetResultadoTablaById'),
            Data: { id: id },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    if (callbackError != undefined) {
                        callbackError();
                    }
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                if (callbackError != undefined) {
                    callbackError();
                }
            }
        });
    }

    function actualizarRequerimientoEnGrilla(rq) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == rq.Id) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(rq);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableOrdenTrabajo = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Columna de días
    if (!('ColumnaDias' in valores)) {
        valores.ColumnaDias = false;
    }

    //Columna de estado
    if (!('ColumnaEstado' in valores)) {
        valores.ColumnaEstado = true;
    }

    //Columna de area
    if (!('ColumnaArea' in valores)) {
        valores.ColumnaArea = false;
    }

    //Columna de seccion
    if (!('ColumnaSeccion' in valores)) {
        valores.ColumnaSeccion = true;
    }

    //Columna de descripcion
    if (!('ColumnaDescripcion' in valores)) {
        valores.ColumnaDescripcion = true;
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[2, 'desc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------
    var cols = [];

    if (valores.ColumnaEstado) {
        cols.push({
            "sTitle": "Estado",
            "mData": "EstadoNombre",
            "width": "80px",
            render: function (data, type, row) {
                //return '<div><i class="material-icons tooltipped "  style="color: #' + data.Color + '" data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(data.Nombre) + '">swap_vertical_circle</i> <span style="margin-left: 8px">' + '    ' + toTitleCase(data.Nombre) + '</span></div>';
                return '<div><i class="material-icons tooltipped "  style="color: #' + row.EstadoColor + '" data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(data) + '">swap_vertical_circle</i> </div>';
            }
        });
    }

    if (valores.ColumnaDias) {
        cols.push({
            "sTitle": "Días",
            "mData": "Dias",
            "width": "80px",
            render: function (data, type, row) {
                return '<div><span>' + data + ' días' + '</span></div>';
            }
        });
    }

    cols.push(
                {
                    title: "Número",
                    data: "Numero",
                    width: "100px",
                    render: function (data, type, row) {

                        return '<div><span class="link-numero tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-ot="' + row.Id + '">' + row.Numero + '</span></div>';
                    }
                },
                {
                    title: "Fecha Alta",
                    data: "FechaAlta",
                    width: "75px",
                    type: 'date-euro',
                    render: function (data, type, row) {
                        var fecha = moment(data);
                        return '<div><span>' + fecha.format('DD/MM/YYYY HH:mm') + '</span></div>';
                    }
                });

    //solo se agrega la columna area, en caso de que el usuario tenga mas de un area asociado
    if (valores.ColumnaArea && usuarioLogeado.Areas.length > 1) {
        cols.push({
            "sTitle": "Area",
            "mData": "AreaNombre",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        });
    } else if (valores.ColumnaSeccion) {
        cols.push({
            "sTitle": "Seccion",
            "mData": "SeccionNombre",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        });
    }

    if (valores.ColumnaDescripcion) {
        cols.push(
                  {
                      title: "Descripción",
                      data: "Descripcion",
                      render: function (data, type, row) {
                          return '<div><span>' + toTitleCase(data) + '</span></div>';
                      }
                  });
    }

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    $('#' + idTabla).on('click', '.link-numero', function () {
        var idOt = $(this).attr('id-ot');
        verDetalle(idOt);
    });

    function verDetalle(id) {
        crearDialogoOrdenTrabajoDetalle({
            Id: id,
            Callback: function () {
                console.log('asasas');
                convertirResultadoTabla(id).then(function (entity) {
                    actualizarEntityEnGrilla(entity.Data[0]);
                    valores.Callback()
                });
            }
        });
    }

    function convertirResultadoTabla(id) {
        return new Promise(function (callback, callbackError) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrdenTrabajoService.asmx/GetDatosTablaByIds'),
                Data: { ids: [id] },
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
            });
        });
    }

    function actualizarEntityEnGrilla(entity) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == entity.Id) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(entity);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }


    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableOrdenInspeccion = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Columna de días
    if (!('ColumnaDias' in valores)) {
        valores.ColumnaDias = false;
    }

    //Columna de estado
    if (!('ColumnaEstado' in valores)) {
        valores.ColumnaEstado = true;
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[2, 'desc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------
    var cols = [];

    if (valores.ColumnaEstado) {
        cols.push({
            "sTitle": "Estado",
            "mData": "EstadoNombre",
            "width": "80px",
            render: function (data, type, row) {
                //return '<div><i class="material-icons tooltipped "  style="color: #' + data.Color + '" data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(data.Nombre) + '">swap_vertical_circle</i> <span style="margin-left: 8px">' + '    ' + toTitleCase(data.Nombre) + '</span></div>';
                return '<div><i class="material-icons tooltipped "  style="color: #' + row.EstadoColor + '" data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(data) + '">swap_vertical_circle</i> </div>';
            }
        });
    }

    if (valores.ColumnaDias) {
        cols.push({
            "sTitle": "Días",
            "mData": "Dias",
            "width": "80px",
            render: function (data, type, row) {
                return '<div><span>' + data + ' días' + '</span></div>';
            }
        });
    }

    cols.push(
                {
                    title: "Número",
                    data: "Numero",
                    width: "100px",
                    render: function (data, type, row) {

                        return '<div><span class="link-numero tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-ot="' + row.Id + '">' + row.Numero + '</span></div>';
                    }
                },
                {
                    title: "Fecha Alta",
                    data: "FechaAlta",
                    width: "75px",
                    type: 'date-euro',
                    render: function (data, type, row) {
                        var fecha = moment(data);
                        return '<div><span>' + fecha.format('DD/MM/YYYY HH:mm') + '</span></div>';
                    }
                },
                {
                    title: "Descripción",
                    data: "Descripcion",
                    render: function (data, type, row) {
                        return '<div><span>' + toTitleCase(data) + '</span></div>';
                    }
                }
                );

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    $('#' + idTabla).on('click', '.link-numero', function () {
        var idOt = $(this).attr('id-ot');
        verDetalle(idOt);
    });

    function verDetalle(id) {
        crearDialogoOrdenInspeccionDetalle({
            Id: id,
            Callback: function () {
                console.log('asasas');
                convertirResultadoTabla(id).then(function (entity) {
                    actualizarEntityEnGrilla(entity.Data[0]);
                    valores.Callback()
                });
            }
        });
    }

    function convertirResultadoTabla(id) {
        return new Promise(function (callback, callbackError) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/OrdenInspeccionService.asmx/GetDatosTablaByIds'),
                Data: { ids: [id] },
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
            });
        });
    }

    function actualizarEntityEnGrilla(entity) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == entity.Id) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(entity);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }


    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableOrdenAtencionCritica = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    var cols = [
                {
                    "sTitle": "Estado",
                    "mData": "EstadoNombre",
                    "width": "30px",
                    render: function (data, type, row) {
                        return '<div><i class="material-icons tooltipped "  style="color: #' + row.EstadoColor + '" data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(data) + '">security</i> <span style="margin-left: 8px">' + toTitleCase(data.EstadoNombre) + '</span></div>';
                    }
                },

                {
                    "sTitle": "Fecha Alta",
                    "mData": "FechaAlta",
                    "width": "135px",
                    "type": 'date-euro',
                    render: function (data, type, row) {
                        var fecha = moment(data);
                        return '<div><span>' + fecha.format('DD/MM/YYYY HH:mm') + '</span></div>';
                    }
                },
        {
            "sTitle": "Requerimiento",
            "mData": "RequerimientoNumero",
            "width": "120px",
            render: function (data, type, row) {
                return '<div><span>' + data + '</span></div>';
            }
        },
                {
                    "sTitle": "Motivo",
                    "mData": "MotivoNombre",
                    "width": "120px",
                    render: function (data, type, row) {
                        return '<div><span class="tooltipped"  data-position="bottom" data-delay="50" data-tooltip="' + toTitleCase(data) + '">' + toTitleCase(data) + '</span></div>';
                    }
                },
                        {
                            "sTitle": "Descripción",
                            "mData": "Descripcion",

                            render: function (data, type, row) {
                                return '<div><span class="tooltipped"  data-position="bottom" data-delay="50" data-tooltip="' + data + '">' + data + '</span></div>';
                            }
                        },
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //--------------------------------------
    // Botones
    //--------------------------------------

    var botones = [];

    ////Boton Detalle
    //if (!('BotonDetalle' in valores)) {
    //    valores.BotonDetalle = true;
    //}

    //if (!('BotonDetalleOculto' in valores)) {
    //    valores.BotonDetalleOculto = false;
    //}

    //botones.push({
    //    Titulo: 'Ver Detalle',
    //    Icono: 'description',
    //    Oculto: valores.BotonDetalleOculto,
    //    Visible: function (data) {
    //        if (typeof (valores.BotonDetalle) === 'boolean') {
    //            return valores.BotonDetalle;
    //        } else {
    //            return valores.BotonDetalle(data);
    //        }
    //    },
    //    Validar: function (data) {
    //        if ('BotonDetalleValidar' in valores) {
    //            return valores.BotonDetalleValidar(data);
    //        }
    //        return true;
    //    },
    //    OnClick: function (data) {
    //        crearDialogoDetalleOrdenAtencionCritica({
    //            Id: data.Id,
    //            CallbackMensajes: function (tipo, mensaje) {
    //                valores.CallbackMensajes(tipo, mensaje);
    //            }
    //        });
    //    }
    //});

    //Boton Detalle Requerimiento
    if (!('BotonDetalleRequerimiento' in valores)) {
        valores.BotonDetalleRequerimiento = true;
    }

    if (!('BotonDetalleRequerimientoOculto' in valores)) {
        valores.BotonDetalleRequerimientoOculto = false;
    }

    botones.push({
        Titulo: 'Ver Detalle Requerimiento',
        Icono: 'description',
        Oculto: valores.BotonDetalleRequerimientoOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalleRequerimiento) === 'boolean') {
                return valores.BotonDetalleRequerimiento;
            } else {
                return valores.BotonDetalleRequerimiento(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleRequerimientoValidar' in valores) {
                return valores.BotonDetalleRequerimientoValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoDetalleRequerimiento({
                Id: data.IdRequerimiento,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });
        }
    });


    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };;
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = false;
    }

    botones.push({
        Titulo: 'Editar Descripción',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (!crearOrdenEspecial) {
                return false;
            }

            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            if ('BotonEditarValidar' in valores) {
                return valores.BotonEditarValidar(data);
            }
            return true;
        }
    });

    //Boton cerrar
    if (!('BotonCompletar' in valores)) {
        valores.BotonCompletar = true;
    }

    if (!('CallbackCompletar' in valores)) {
        valores.CallbackCompletar = function () { };
    }

    if (!('BotonCompletarOculto' in valores)) {
        valores.BotonCompletarOculto = false;
    }

    botones.push({
        Titulo: 'Marcar como completada',
        Icono: 'done_all',
        Oculto: valores.BotonCompletarOculto,
        Visible: function (data) {
            if (!crearOrdenEspecial) {
                return false;
            }

            if (typeof (valores.BotonCompletar) === 'boolean') {
                return valores.BotonCompletar;
            } else {
                return valores.BotonCompletar(data);
            }
        },
        Validar: function (data) {
            if ('BotonCompletarValidar' in valores) {
                return valores.BotonCompletarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            crearDialogoCompletarOrdenAtencionCritica({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                CallbackCompletar: function (data) {
                    convertirResultadoTabla(data, function (rqTabla) {
                        actualizarOrdenAtencionCriticaEnGrilla(rqTabla);
                        valores.CallbackEditar(rqTabla);
                    });
                }
            });
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    function convertirResultadoTabla(rq, callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/OrdenAtencionCriticaService.asmx/GetResultadoTablaById'),
            Data: { id: rq.Id },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    if (callbackError != undefined) {
                        callbackError();
                    }
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                if (callbackError != undefined) {
                    callbackError();
                }
            }
        });
    }


    function actualizarOrdenAtencionCriticaEnGrilla(rq) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == rq.Id) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(rq);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTablePersonaFisica = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------

    var cols = [
        {
            "sTitle": "Nombre",
            "mData": "Nombre",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            "sTitle": "Apellido",
            "mData": "Apellido",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            "sTitle": "N° de Doc",
            "mData": null,
            "width": "120px",
            render: function (data, type, row) {
                return '<div><span>' + row.TipoDocumentoString + ' ' + row.NroDoc + '</span></div>';
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Titulo: 'Ver Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoPersonaFisicaDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo del detalle de la persona física');
            }
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Titulo: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            if ('BotonEditarValidar' in valores) {
                return valores.BotonEditarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoPersonaFisicaEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                CallbackEditar: function (rq) {
                    valores.CallbackEditar(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo de la edición de la persona física');
            }
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableMovil = function (valores) {

    var idTabla = '' + $(this).prop('id');
    if (valores == undefined) valores = {};


    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //--------------------------------------
    // Columnas
    //--------------------------------------

    //Boton Seleccionar
    if (!('BotonSeleccionar' in valores)) {
        valores.BotonSeleccionar = false;
    }

    var cols = [];
    if (valores.BotonSeleccionar) {
        cols.push({
            title: '',
            data: 'Id',
            "width": "24px",
            render: function (data, type, full, meta) {
                return '<div><p style="margin:0px; padding:0px; margin-top:6px;"><input type="checkbox" value="' + data + '" id="chx' + data + '" ><label style="  padding:0px!important" for="chx' + data + '"></label> </p></div>';
            }
        })
    }

    //Cols generales
    cols.push(
        {
            title: 'Número Interno',
            data: 'NumeroInterno',
            render: function (data, type, row) {
                return "<div><span class='link-detalle' id-entity='" + row.Id + "'>" + toTitleCase(data) + "</span></div>";
            }
        },
                        {
                            title: 'Marca - Modelo',
                            render: function (data, type, row) {
                                return "<div><span>" + toTitleCase(row.Marca + ' ' + row.Modelo) + "</span></div>";
                            }
                        },
                                {
                                    title: 'Dominio',
                                    data: 'Dominio',
                                    render: function (data, type, row) {
                                        return "<div><span>" + toTitleCase(data) + "</span></div>";
                                    }
                                },
        {
            title: 'Tipo',
            data: 'TipoMovilNombre',
            render: function (data, type, row) {
                return "<div><span>" + toTitleCase(data) + "</span></div>";
            }
        });



    //Estado
    if (!('ColumnaEstado' in valores)) valores.ColumnaEstado = false;
    if (valores.ColumnaEstado) {
        cols.push({
            title: 'Estado',
            data: 'NombreEstado',
            render: function (data, type, row) {
                if (row.FechaBaja != undefined) {
                    return '<div><span>Dado de baja</span></div>';
                } else {
                    return '<div><span>' + toTitleCase(data) + '</span></div>';
                }
            }
        });
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('ColumnasITVTUV' in valores)) {
        valores.ColumnasITVTUV = function () {
            return false;
        };
    }

    if (valores.ColumnasITVTUV()) {
        cols.push(
            {
                title: 'Vencimiento ITV',
                data: 'DiasITV',
                render: function (data, type, row) {
                    if (row.VencimientoITV != null) {
                        var fecha = moment(row.VencimientoITV).format('DD/MM/YYYY');

                        var style = ' style="color: red"';
                        var mensaje = data + ' días (' + fecha + ')';

                        if (data < 0) {
                            mensaje = 'Venció el ' + fecha;
                        } else if (data == 0) {
                            mensaje = 'HOY';
                        } else if (data == 1) {
                            mensaje = 'MAÑANA';
                        } else if (data > 10) {
                            style = '';
                        }

                        return '<div><span ' + style + '>' + mensaje + '</span></div>';
                    }
                }
            },
                 {
                     title: 'Vencimiento TUV',
                     data: 'DiasTUV',
                     render: function (data, type, row) {
                         if (row.VencimientoTUV != null) {
                             var fecha = moment(row.VencimientoTUV).format('DD/MM/YYYY');

                             var style = ' style="color: red"';
                             var mensaje = data + ' días (' + fecha + ')';

                             if (data < 0) {
                                 mensaje = 'Venció el ' + fecha;
                             } else if (data == 0) {
                                 mensaje = 'HOY';
                             } else if (data == 1) {
                                 mensaje = 'MAÑANA';
                             } else if (data > 10) {
                                 style = '';
                             }

                             return '<div><span ' + style + '>' + mensaje + '</span></div>';
                         }
                     }
                 }
        );
    }

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;


    ////Boton Editar
    //if (!('BotonEditar' in valores)) {
    //    valores.BotonEditar = true;
    //}

    //if (!('CallbackEditar' in valores)) {
    //    valores.CallbackEditar = function () { };
    //}

    //if (!('BotonEditarOculto' in valores)) {
    //    valores.BotonEditarOculto = false;
    //}

    //var botones = [];
    //botones.push({
    //    Titulo: 'Editar',
    //    Icono: 'edit',
    //    Oculto: valores.BotonEditarOculto,
    //    Visible: function (data) {
    //        if (data.FechaBaja != undefined) {
    //            return false;
    //        }

    //        if (typeof (valores.BotonEditar) === 'boolean') {
    //            return valores.BotonEditar;
    //        } else {
    //            return valores.BotonEditar(data);
    //        }
    //    },
    //    Validar: function (data) {
    //        if ('BotonEditarValidar' in valores) {
    //            return valores.BotonEditarValidar(data);
    //        }
    //        return true;
    //    },
    //    OnClick: function (data) {
    //        var creado = crearDialogoEditarMovil({
    //            Id: data.Id,
    //            CallbackMensajes: function (tipo, mensaje) {
    //                valores.CallbackMensajes(tipo, mensaje);
    //            },
    //            Callback: function (movil) {
    //                valores.Callback(movil);
    //            }
    //        });

    //        if (creado == false) {
    //            valores.CallbackMensajes(tipo, 'Error creando el dialogo de la edición del móvil');
    //        }
    //    }
    //});

    ////Boton Dar de baja
    //if (!('BotonDarDeBaja' in valores)) {
    //    valores.BotonDarDeBaja = true;
    //}

    //if (!('BotonDarDeBajaOculto' in valores)) {
    //    valores.BotonDarDeBajaOculto = false;
    //}

    //if (!('CallbackDarDeBaja' in valores)) {
    //    valores.CallbackDarDeBaja = function () { };
    //}

    //botones.push({
    //    Titulo: 'Dar de baja',
    //    Icono: 'delete',
    //    Oculto: valores.BotonDarDeBajaOculto,
    //    Visible: function (data) {
    //        if (data.FechaBaja != undefined) {
    //            return false;
    //        }
    //        if (typeof (valores.BotonDarDeBaja) === 'boolean') {
    //            return valores.BotonDarDeBaja;
    //        } else {
    //            return valores.BotonDarDeBaja(data);
    //        }
    //    },
    //    Validar: function (data) {
    //        if ('BotonDarDebjaValidar' in valores) {
    //            return valores.BotonDarDeBajaValidar(data);
    //        }
    //        return true;
    //    },
    //    OnClick: function (data) {
    //        var creado = crearDialogoMovilDarDeBaja({
    //            Id: data.Id,
    //            CallbackMensajes: function (tipo, mensaje) {
    //                valores.CallbackMensajes(tipo, mensaje);
    //            },
    //            Callback: function (movil) {
    //                valores.Callback(movil);
    //            }
    //        });

    //        if (creado == false) {
    //            valores.CallbackMensajes(tipo, 'Error creando el dialogo');
    //        }
    //    }
    //});


    ////Boton Restaurar
    //if (!('BotonDarDeAlta' in valores)) {
    //    valores.BotonDarDeAlta = true;
    //}

    //if (!('BotonDarDeAltaOculto' in valores)) {
    //    valores.BotonDarDeAltaOculto = false;
    //}

    //if (!('CallbackDarDeAlta' in valores)) {
    //    valores.CallbackDarDeAlta = function () { };
    //}

    //botones.push({
    //    Titulo: 'Dar de alta',
    //    Icono: 'restore',
    //    Oculto: valores.BotonDarDeAltaOculto,
    //    Visible: function (data) {
    //        if (data.FechaBaja == undefined) {
    //            return false;
    //        }
    //        if (typeof (valores.BotonDarDeAlta) === 'boolean') {
    //            return valores.BotonDarDeAlta;
    //        } else {
    //            return valores.BotonDarDeAlta(data);
    //        }
    //    },
    //    Validar: function (data) {
    //        if ('BotonDarDeAltaValidar' in valores) {
    //            return valores.BotonDarDeAltaValidar(data);
    //        }
    //        return true;
    //    },
    //    OnClick: function (data) {
    //        var creado = crearDialogoMovilDarDeAlta({
    //            Id: data.Id,
    //            CallbackMensajes: function (tipo, mensaje) {
    //                valores.CallbackMensajes(tipo, mensaje);
    //            },
    //            Callback: function (movil) {
    //                valores.Callback(movil);
    //            }
    //        });

    //        if (creado == false) {
    //            valores.CallbackMensajes(tipo, 'Error creando el dialogo');
    //        }
    //    }
    //});

    $('#' + idTabla).on('click', '.link-detalle', function () {
        var id = $(this).attr('id-entity');
        verDetalle(id);
    });

    $('#' + idTabla).on('dblclick', 'tr', function (e) {
        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
        verDetalle(id);
    });

    function verDetalle(id) {
        $('.material-tooltip').hide();

        crearDialogoMovilDetalle2({
            Id: id,
            Callback: function () {
                console.log('callback');

                convertirResultadoTabla(id).then(function (dataNueva) {
                    if (dataNueva == undefined) return;
                    actualizarEnTabla(dataNueva);
                    valores.Callback(dataNueva);
                });
            },
            CallbackMensajes: function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            }
        });
    }

    function convertirResultadoTabla(id) {
        return new Promise(function (callback, callbackError) {
            crearAjax({
                Url: ResolveUrl('~/Servicios/MovilService.asmx/GetResultadoTablaById'),
                Data: { id: id },
                OnSuccess: function (result) {
                    if (!result.Ok) {
                        callbackError();
                        return;
                    }

                    callback(result.Return);
                },
                OnError: function (result) {
                    callbackError();
                }
            });
        });
    }

    function actualizarEnTabla(entity) {
        console.log('actualizando');
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == entity.Id) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(entity);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    //valores.Botones = botones;
    return procesarDatatable(idTabla, valores);
}

//jQuery.fn.DataTableMovilXOrdenTrabajo = function (valores) {
//    var idTabla = '' + $(this).prop('id');
//    if (valores == undefined) {
//        valores = {};
//    }

//    //--------------------------------------
//    // Callbacks
//    //--------------------------------------

//    //Callback mensajes
//    if (!('CallbackMensajes' in valores)) {
//        valores.CallbackMensajes = function () { };
//    }

//    //Callback Cargando
//    if (!('CallbackCargando' in valores)) {
//        valores.CallbackCargando = function () { };
//    }

//    //Botón Quitar
//    if (!('BotonQuitar' in valores)) {
//        valores.BotonQuitar = true;
//    }

//    if (!('CallbackQuitar' in valores)) {
//        valores.CallbackQuitar = function () { };
//    }

//    //--------------------------------------
//    //Orden
//    //--------------------------------------
//    if (!('Orden' in valores)) {
//        valores.Orden = [[0, 'desc']];
//    }


//    var cols = [
//        {
//            "sTitle": "Se unió",
//            "mData": "FechaAltaString",
//            "width": "140px",
//            render: function (data, type, row) {

//                return '<div><span>' + data + '</span></div>';
//            }
//        },
//        {
//            "sTitle": "Nombre del vehículo",
//            "mData": "Movil.NumeroInterno",
//            render: function (data, type, row) {
//                return '<div><span>' + data + '</span></div>';
//            }
//        },
//                        {
//                            "sTitle": "Marca - Modelo",
//                            "mData": "Movil.Marca",
//                            render: function (data, type, row) {
//                                return '<div><span>' + data + ' - ' + row.Movil.Modelo + '</span></div>';
//                            }
//                        },
//                    {
//                        "sTitle": "Observaciones",
//                        "mData": "Movil.Observaciones",
//                        render: function (data, type, row) {
//                            return '<div><span>' + data + '</span></div>';
//                        }
//                    },
//    ]

//    valores.Columnas = cols;

//    var botones = [
//         {
//             Titulo: 'Quitar',
//             Icono: 'clear',
//             Visible: function (data) {
//                 if (typeof (valores.BotonQuitar) === 'boolean') {
//                     return valores.BotonQuitar;
//                 } else {
//                     return valores.BotonQuitar(data);
//                 }
//             },
//             OnClick: function (data) {
//                 borrarFila("#" + idTabla, data.Id);

//                 if (valores.CallbackQuitar != undefined) {
//                     valores.CallbackQuitar(data);
//                 }
//             }
//         }
//    ]

//    //Boton Detalle
//    if (!('BotonDetalle' in valores)) {
//        valores.BotonDetalle = true;
//    }

//    if (!('BotonDetalleOculto' in valores)) {
//        valores.BotonDetalleOculto = false;
//    }

//    botones.push({
//        Titulo: 'Ver Detalle',
//        Icono: 'description',
//        Oculto: valores.BotonDetalleOculto,
//        Visible: function (data) {
//            if (typeof (valores.BotonDetalle) === 'boolean') {
//                return valores.BotonDetalle;
//            } else {
//                return valores.BotonDetalle(data);
//            }
//        },
//        Validar: function (data) {
//            if ('BotonDetalleValidar' in valores) {
//                return valores.BotonDetalleValidar(data);
//            }
//            return true;
//        },
//        OnClick: function (data) {
//            crearDialogoMovilDetalle({
//                Id: data.Movil.Id,
//                CallbackMensajes: function (tipo, mensaje) {
//                    valores.CallbackMensajes(tipo, mensaje);
//                }
//            });
//        }
//    });

//    valores.Botones = botones;

//    return procesarDatatable(idTabla, valores);
//}

jQuery.fn.DataTableOrigen = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------

    var cols = [
        {
            "sTitle": "Nombre",
            "mData": "Nombre",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'FechaBaja',
            render: function (data, type, row) {
                if (data != null) {
                    return '<div><span>Dado de baja</span></div>';
                } else {
                    return '<div><span>Activo</span></div>';
                }


            }
        },
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Titulo: 'Ver Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoOrigenDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo del detalle del origen');
            }
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Titulo: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            if ('BotonEditarValidar' in valores) {
                return valores.BotonEditarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoOrigenEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (rq) {
                    valores.CallbackEditar(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo de la edición del origen');
            }
        }
    });

    //Boton Dar de baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    botones.push({
        Titulo: 'Dar de baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            if (data.FechaBaja != undefined) {
                return false;
            }
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                return valores.BotonDarDeBaja;
            } else {
                return valores.BotonDarDeBaja(data);
            }
        },
        Validar: function (data) {
            if ('BotonDarDebjaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoOrigenDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (rq) {
                    valores.CallbackDarDeBaja(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    //Boton Restaurar
    if (!('BotonDarDeAlta' in valores)) {
        valores.BotonDarDeAlta = false;
    }

    if (!('BotonDarDeAltaOculto' in valores)) {
        valores.BotonDarDeAltaOculto = true;
    }

    if (!('CallbackDarDeAlta' in valores)) {
        valores.CallbackDarDeAlta = function () { };
    }

    botones.push({
        Titulo: 'Dar de alta',
        Icono: 'restore',
        Oculto: valores.BotonDarDeAltaOculto,
        Visible: function (data) {
            if (data.FechaBaja == undefined) {
                return false;
            }
            if (typeof (valores.BotonDarDeAlta) === 'boolean') {
                return valores.BotonDarDeAlta;
            } else {
                return valores.BotonDarDeAlta(data);
            }
        },
        Validar: function (data) {
            if ('BotonDarDeAltaValidar' in valores) {
                return valores.BotonDarDeAltaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoOrigenDarDeAlta({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (rq) {
                    valores.CallbackDarDeAlta(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableSeccion = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------

    var cols = [
        {
            "sTitle": "Nombre",
            "mData": "Nombre",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Área',
            data: 'AreaNombre',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'FechaBaja',
            render: function (data, type, row) {
                if (data != null) {
                    return '<div><span>Dado de baja</span></div>';
                } else {
                    return '<div><span>Activo</span></div>';
                }
            }
        },
                {
                    title: 'Observaciones',
                    data: 'Observaciones',
                    render: function (data, type, row) {
                        return '<div><span>' + toTitleCase(data) + '</span></div>';
                    }
                },
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = false;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Titulo: 'Ver Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoSeccionDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo del detalle de la sección');
            }
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Titulo: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            if ('BotonEditarValidar' in valores) {
                return valores.BotonEditarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoEditarSeccion({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                CallbackEditar: function (sec) {
                    actualizarSeccionEnGrilla(sec);
                    valores.CallbackEditar(sec);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo de la edición del sección');
            }
        }
    });

    //Boton Dar de baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = false;
    }

    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    botones.push({
        Titulo: 'Dar de baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            if (data.FechaBaja != undefined) {
                return false;
            }
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                return valores.BotonDarDeBaja;
            } else {
                return valores.BotonDarDeBaja(data);
            }
        },
        Validar: function (data) {
            if ('BotonDarDebjaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoSeccionDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                CallbackDarDeBaja: function (sec) {
                    actualizarSeccionEnGrilla(sec);
                    valores.CallbackDarDeBaja(sec);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    //Boton Restaurar
    if (!('BotonDarDeAlta' in valores)) {
        valores.BotonDarDeAlta = false;
    }

    if (!('BotonDarDeAltaOculto' in valores)) {
        valores.BotonDarDeAltaOculto = false;
    }

    if (!('CallbackDarDeAlta' in valores)) {
        valores.CallbackDarDeAlta = function () { };
    }

    botones.push({
        Titulo: 'Dar de alta',
        Icono: 'restore',
        Oculto: valores.BotonDarDeAltaOculto,
        Visible: function (data) {
            if (data.FechaBaja == undefined) {
                return false;
            }
            if (typeof (valores.BotonDarDeAlta) === 'boolean') {
                return valores.BotonDarDeAlta;
            } else {
                return valores.BotonDarDeAlta(data);
            }
        },
        Validar: function (data) {
            if ('BotonDarDeAltaValidar' in valores) {
                return valores.BotonDarDeAltaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoSeccionDarDeAlta({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                CallbackDarDeAlta: function (sec) {
                    actualizarSeccionEnGrilla(sec);
                    valores.CallbackDarDeAlta(sec);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    function actualizarSeccionEnGrilla(sec) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        var dataVieja;
        dt.rows(function (idx, data, node) {
            if (data.Id == sec.Id) {
                dataVieja = data;
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        if (dataVieja.IdArea != sec.IdArea) {
            var dt2 = $("#" + idTabla).dataTable();
            dt2.fnDeleteRow(index);
            return;
        }

        //Actualizo
        dt.row(index).data(sec);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableUsuario = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------

    var cols = [
        {
            title: '',
            data: undefined,
            render: function (data, type, row) {
                let foto;
                if (row.IdentificadorFotoPersonal != undefined) {
                    foto = top.urlCordobaFiles + '/Archivo/' + row.IdentificadorFotoPersonal + '/3';
                } else {
                    if (row.SexoMasculino == true) {
                        foto = top.urlCordobaFiles + '/Archivo/' + top.identificadorFotoUserMale + '/3';
                    } else {
                        foto = top.urlCordobaFiles + '/Archivo/' + top.identificadorFotoUserFemale + '/3';
                    }
                }
                return '<div><div class="foto" style="min-width: 2rem; min-height:2rem; background-image: url(' + foto + ');border-radius: 2rem; max-width: 2rem;background-size: cover;background-repeat: no-repeat;max-height: 2rem;"></div></div>';
            }
        },
        {
            "sTitle": "Apellido",
            "mData": "Apellido",
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Nombre',
            data: 'Nombre',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Usuario',
            data: 'Username',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'DNI',
            data: 'Dni',
            render: function (data, type, row) {
                return '<div><span>' + data + '</span></div>';
            }
        },
        {
            title: 'Email',
            data: 'Email',
            render: function (data, type, row) {
                return '<div><span>' + data.toLowerCase() + '</span></div>';
            }
        },

    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Titulo: 'Ver Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoUsuarioDetalle({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo del detalle del usuario');
            }
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Titulo: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            if ('BotonEditarValidar' in valores) {
                return valores.BotonEditarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoUsuarioEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (usuario) {
                    actualizarUsuarioEnGrilla(usuario);
                    valores.Callback(usuario);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo de la edición del sección');
            }
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    function actualizarUsuarioEnGrilla(sec) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        var dataVieja;
        dt.rows(function (idx, data, node) {
            if (data.Id == sec.Id) {
                dataVieja = data;
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(sec);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableNotificacionParaUsuario = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------

    var cols = [
        {
            data: 'Notificar',
            "orderable": false,
            render: function (data, type, row) {
                if (data != undefined && data == true) {
                    return '<div><i class="material-icons">notifications</i></div>';
                } else {
                    return '<div></div>';
                }


            }
        },
        {
            "sTitle": "Titulo",
            "mData": "Titulo",
            "orderable": false,
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'FechaBaja',
            "orderable": false,
            render: function (data, type, row) {
                if (data != null) {
                    return '<div><span>Dado de baja</span></div>';
                } else {
                    return '<div><span>Activo</span></div>';
                }


            }
        },
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    var botones = [];

    //Boton Detalle
    if (!('BotonDetalle' in valores)) {
        valores.BotonDetalle = true;
    }

    if (!('BotonDetalleOculto' in valores)) {
        valores.BotonDetalleOculto = false;
    }

    botones.push({
        Titulo: 'Ver Detalle',
        Icono: 'description',
        Oculto: valores.BotonDetalleOculto,
        Visible: function (data) {
            if (typeof (valores.BotonDetalle) === 'boolean') {
                return valores.BotonDetalle;
            } else {
                return valores.BotonDetalle(data);
            }
        },
        Validar: function (data) {
            if ('BotonDetalleValidar' in valores) {
                return valores.BotonDetalleValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoNotificacionParaUsuarioDetalle({
                Id: data.Id,
                Titulo: data.Titulo,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    //Boton Editar
    if (!('BotonEditar' in valores)) {
        valores.BotonEditar = false;
    }

    if (!('CallbackEditar' in valores)) {
        valores.CallbackEditar = function () { };
    }

    if (!('BotonEditarOculto' in valores)) {
        valores.BotonEditarOculto = true;
    }

    botones.push({
        Titulo: 'Editar',
        Icono: 'edit',
        Oculto: valores.BotonEditarOculto,
        Visible: function (data) {
            if (typeof (valores.BotonEditar) === 'boolean') {
                return valores.BotonEditar;
            } else {
                return valores.BotonEditar(data);
            }
        },
        Validar: function (data) {
            if ('BotonEditarValidar' in valores) {
                return valores.BotonEditarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoNotificacionParaUsuarioEditar({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (rq) {
                    valores.CallbackEditar(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    //Boton Notificar
    if (!('BotonNotificar' in valores)) {
        valores.BotonNotificar = false;
    }

    if (!('CallbackNotificar' in valores)) {
        valores.CallbackNotificar = function () { };
    }

    if (!('BotonNotificarOculto' in valores)) {
        valores.BotonNotificarOculto = false;
    }

    botones.push({
        Titulo: 'Notificar',
        Icono: 'notifications',
        Oculto: valores.BotonNotificarOculto,
        Visible: function (data) {
            if (data.Notificar) return false;

            if (typeof (valores.BotonNotificar) === 'boolean') {
                return valores.BotonNotificar;
            } else {
                return valores.BotonNotificar(data);
            }
        },
        Validar: function (data) {
            if ('BotonNotificarValidar' in valores) {
                return valores.BotonNotificarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoNotificacionParaUsuarioSetNotificar({
                Id: data.Id,
                Notificar: true,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (rq) {
                    valores.CallbackNotificar(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    //Boton No Notificar
    if (!('BotonNoNotificar' in valores)) {
        valores.BotonNoNotificar = false;
    }

    if (!('CallbackNoNotificar' in valores)) {
        valores.CallbackNoNotificar = function () { };
    }

    if (!('BotonNoNotificarOculto' in valores)) {
        valores.BotonNoNotificarOculto = false;
    }

    botones.push({
        Titulo: 'No notificar',
        Icono: 'notifications_none',
        Oculto: valores.BotonNoNotificarOculto,
        Visible: function (data) {
            if (!data.Notificar) return false;

            if (typeof (valores.BotonNoNotificar) === 'boolean') {
                return valores.BotonNoNotificar;
            } else {
                return valores.BotonNoNotificar(data);
            }
        },
        Validar: function (data) {
            if ('BotonNoNotificarValidar' in valores) {
                return valores.BotonNoNotificarValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoNotificacionParaUsuarioSetNotificar({
                Id: data.Id,
                Notificar: false,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (rq) {
                    valores.CallbackNoNotificar(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    //Boton Dar de baja
    if (!('BotonDarDeBaja' in valores)) {
        valores.BotonDarDeBaja = false;
    }

    if (!('BotonDarDeBajaOculto' in valores)) {
        valores.BotonDarDeBajaOculto = true;
    }

    if (!('CallbackDarDeBaja' in valores)) {
        valores.CallbackDarDeBaja = function () { };
    }

    botones.push({
        Titulo: 'Dar de baja',
        Icono: 'delete',
        Oculto: valores.BotonDarDeBajaOculto,
        Visible: function (data) {
            if (data.FechaBaja != undefined) {
                return false;
            }
            if (typeof (valores.BotonDarDeBaja) === 'boolean') {
                return valores.BotonDarDeBaja;
            } else {
                return valores.BotonDarDeBaja(data);
            }
        },
        Validar: function (data) {
            if ('BotonDarDebjaValidar' in valores) {
                return valores.BotonDarDeBajaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoNotificacionParaUsuarioDarDeBaja({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (rq) {
                    valores.CallbackDarDeBaja(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    //Boton Restaurar
    if (!('BotonDarDeAlta' in valores)) {
        valores.BotonDarDeAlta = false;
    }

    if (!('BotonDarDeAltaOculto' in valores)) {
        valores.BotonDarDeAltaOculto = true;
    }

    if (!('CallbackDarDeAlta' in valores)) {
        valores.CallbackDarDeAlta = function () { };
    }

    botones.push({
        Titulo: 'Dar de alta',
        Icono: 'restore',
        Oculto: valores.BotonDarDeAltaOculto,
        Visible: function (data) {
            if (data.FechaBaja == undefined) {
                return false;
            }
            if (typeof (valores.BotonDarDeAlta) === 'boolean') {
                return valores.BotonDarDeAlta;
            } else {
                return valores.BotonDarDeAlta(data);
            }
        },
        Validar: function (data) {
            if ('BotonDarDeAltaValidar' in valores) {
                return valores.BotonDarDeAltaValidar(data);
            }
            return true;
        },
        OnClick: function (data) {
            var creado = crearDialogoNotificacionParaUsuarioDarDeAlta({
                Id: data.Id,
                CallbackMensajes: function (tipo, mensaje) {
                    valores.CallbackMensajes(tipo, mensaje);
                },
                Callback: function (rq) {
                    valores.CallbackDarDeAlta(rq);
                }
            });

            if (creado == false) {
                valores.CallbackMensajes(tipo, 'Error creando el dialogo');
            }
        }
    });

    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }

    valores.Botones = botones;

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableInformacionOrganicaSecretaria = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------

    var cols = [
        {
            "sTitle": "Nombre",
            "mData": "Nombre",
            render: function (data, type, row) {
                return '<div><span class="link-detalle tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-entity="' + row.Id + '">' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'FechaBaja',
            render: function (data, type, row) {
                if (data != null) {
                    return '<div><span>Dado de baja</span></div>';
                } else {
                    return '<div><span>Activo</span></div>';
                }


            }
        },
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    valores.Botones = [];


    //-------------------------------------
    // Eventos
    //-------------------------------------

    $('#' + idTabla).on('click', '.link-detalle', function () {
        var idEntity = $(this).attr('id-entity');
        verDetalle(idEntity);
    });

    $('#' + idTabla).on('dblclick', 'tr', function (e) {
        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
        verDetalle(id);
    });


    //-------------------------------------
    // Acciones
    //-------------------------------------

    function verDetalle(id) {
        crearDialogoInformacionOrganicaSecretariaDetalle({
            Id: id,
            CallbackMensajes: function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            },
            Callback: function () {
                buscarDatos(id).then(function (result) {
                    actualizarFila(result);
                });
            }
        });
    }

    function buscarDatos(id) {
        return new Promise(function (callback, callbackError) {

            crearAjax({
                Url: ResolveUrl('~/Servicios/InformacionOrganicaSecretariaService.asmx/GetById'),
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
            });
        });
    }

    function actualizarFila(entity) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == entity.Id) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(entity);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableInformacionOrganicaDireccion = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------

    var cols = [
        {
            title: "Nombre",
            data: "Nombre",
            render: function (data, type, row) {
                return '<div><span class="link-detalle tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-entity="' + row.Id + '">' + toTitleCase(data) + '</span></div>';
            }
        },
        {
            title: 'Secretaria',
            render: function (data, type, row) {
                return '<div><span>' + toTitleCase(row.Secretaria.Nombre) + '</span></div>';
            }
        },
        {
            title: 'Estado',
            data: 'FechaBaja',
            render: function (data, type, row) {
                if (data != null) {
                    return '<div><span>Dado de baja</span></div>';
                } else {
                    return '<div><span>Activo</span></div>';
                }


            }
        },
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    valores.Botones = [];


    //-------------------------------------
    // Eventos
    //-------------------------------------

    $('#' + idTabla).on('click', '.link-detalle', function () {
        var idEntity = $(this).attr('id-entity');
        verDetalle(idEntity);
    });

    $('#' + idTabla).on('dblclick', 'tr', function (e) {
        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
        verDetalle(id);
    });


    //-------------------------------------
    // Acciones
    //-------------------------------------

    function verDetalle(idEntity) {
        $('.material-tooltip').hide();
        crearDialogoInformacionOrganicaDireccionDetalle({
            Id: idEntity,
            CallbackMensajes: function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            },
            Callback: function () {
                buscarDatos(idEntity)
                .then(function (result) {
                    actualizarFila(result);
                });
            }
        });
    }

    function buscarDatos(id) {
        return new Promise(function (callback, callbackError) {

            crearAjax({
                Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/GetById'),
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
            });
        });
    }

    function actualizarFila(entity) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == entity.Id) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(entity);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableInformacionOrganica = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[1, 'desc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------

    var cols = [
        {
            title: "Área",
            render: function (data, type, row) {
                return '<div><span class="link-detalle tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-entity="' + row.Id + '">' + toTitleCase(row.Area.Nombre) + '</span></div>';
            }
        },
        {
            title: 'Secretaria',
            render: function (data, type, row) {
                if (row.InformacionOrganica != undefined) {
                    return '<div><span>' + toTitleCase(row.InformacionOrganica.Direccion.Secretaria.Nombre) + '</span></div>';
                } else {
                    return '<div><span></span></div>';
                }
            }
        },
        {
            title: 'Direccion',
            render: function (data, type, row) {
                if (row.InformacionOrganica != undefined) {
                    return '<div><span>' + toTitleCase(row.InformacionOrganica.Direccion.Nombre) + '</span></div>';
                } else {
                    return '<div><span></span></div>';
                }
            }
        }
    ];

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    valores.Botones = [];


    //-------------------------------------
    // Eventos
    //-------------------------------------

    $('#' + idTabla).on('click', '.link-detalle', function () {
        var idEntity = $(this).attr('id-entity');
        verDetalle(idEntity);
    });

    $('#' + idTabla).on('dblclick', 'tr', function (e) {
        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
        verDetalle(id);
    });


    //-------------------------------------
    // Acciones
    //-------------------------------------

    function verDetalle(idArea) {
        $('.material-tooltip').hide();
        crearDialogoInformacionOrganicaDetalle({
            Id: idArea,
            CallbackMensajes: function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            },
            Callback: function () {
                buscarDatos(idArea)
                .then(function (result) {
                    actualizarFila(idArea, result);
                });
            }
        });
    }

    function buscarDatos(id) {
        return new Promise(function (callback, callbackError) {

            crearAjax({
                Url: ResolveUrl('~/Servicios/InformacionOrganicaService.asmx/GetByIdArea'),
                Data: { idArea: id },
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
            });
        });
    }

    function actualizarFila(idEntity, entity) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == idEntity) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        let data = dt.row(index).data();
        data.InformacionOrganica = entity;
        dt.row(index).data(data);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    return procesarDatatable(idTabla, valores);
}

jQuery.fn.DataTableZona = function (valores) {

    var idTabla = '' + $(this).prop('id');

    if (valores == undefined) {
        valores = {};
    }

    //--------------------------------------
    // Callbacks
    //--------------------------------------

    //Callback mensajes
    if (!('CallbackMensajes' in valores)) {
        valores.CallbackMensajes = function () { };
    }

    //Callback Cargando
    if (!('CallbackCargando' in valores)) {
        valores.CallbackCargando = function () { };
    }

    //Callback
    if (!('Callback' in valores)) {
        valores.Callback = function () { };
    }

    //--------------------------------------
    //Orden
    //--------------------------------------
    if (!('Orden' in valores)) {
        valores.Orden = [[0, 'asc']];
    }

    //-------------------------------------
    // Columnas
    //-------------------------------------
    var cols = [];

    cols.push(
        {
            title: "Nombre",
            data: 'Nombre',
            render: function (data, type, row) {
                return '<div><span class="link-detalle tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-entity="' + row.Id + '" id-area="' + row.AreaId + '">' + row.Nombre + '</span></div>';
            }
        },
        {
            title: "Area",
            data: 'AreaNombre',
            render: function (data, type, row) {
                return '<div><span>' + row.AreaNombre + '</span></div>';
            }
        },
        {
            title: 'Estado',
            render: function (data, type, row) {
                if (row.FechaBaja == undefined) {
                    return '<div><span>Activo</span></div>';
                } else {
                    return '<div><span>Dado de baja</span></div>';
                }
            }
        }
    );

    $('#' + idTabla).on('click', '.link-detalle', function () {
        var id = $(this).attr('id-entity');
        var idArea = $(this).attr('id-area');
        verDetalle(id, idArea);
    });

    $('#' + idTabla).on('dblclick', 'tr', function (e) {
        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
        let idArea = $('#' + idTabla).DataTable().row($(this)).data().AreaId;
        verDetalle(id, idArea);
    });

    function verDetalle(id, idArea) {
        $('.material-tooltip').hide();

        crearDialogoZona({
            Id: id,
            IdArea: idArea,
            CallbackMensajes: function (tipo, mensaje) {
                valores.CallbackMensajes(tipo, mensaje);
            },
            CallbackCargando: function (mostrar, mensaje) {
                valores.CallbackCargando(mostrar, mensaje);
            },
            Callback: function () {
                buscarResultadoTabla(id, function (entity) {
                    if (entity == undefined) return;
                    actualizarFilaEnTabla(entity);
                    valores.Callback(entity);
                }, function () {
                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
                });
            }
        });
    }

    //Agrego las cols
    if ('Columnas' in valores) {
        $.each(valores.Columnas, function (index, val) {
            cols.push(val);
        });
    }

    valores.Columnas = cols;

    //-------------------------------------
    // Botones
    //-------------------------------------

    var botones = [];



    if ('Botones' in valores) {
        $.each(valores.Botones, function (index, val) {
            botones.push(val);
        });
    }


    valores.Botones = botones;


    function buscarResultadoTabla(id, callback, callbackError) {
        crearAjax({
            Url: ResolveUrl('~/Servicios/ZonaService.asmx/GetResultadoTablaById'),
            Data: { id: id },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    if (callbackError != undefined) {
                        callbackError();
                    }
                    return;
                }

                callback(result.Return);
            },
            OnError: function (result) {
                if (callbackError != undefined) {
                    callbackError();
                }
            }
        });
    }

    function actualizarFilaEnTabla(entity) {
        //Busco el indice de la persona a actualizar
        var index = -1;
        var dt = $("#" + idTabla).DataTable();
        dt.rows(function (idx, data, node) {
            if (data.Id == entity.Id) {
                index = idx;
            }
        });

        //Si no esta, corto
        if (index == -1) {
            return;
        }

        //Actualizo
        dt.row(index).data(entity);

        //Inicializo el tooltip
        dt.$('.tooltipped').tooltip({ delay: 50 });
    }

    return procesarDatatable(idTabla, valores);
}



function borrarFila(tabla, id) {
    var borrado = false;
    $(tabla).DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
        var info_row = this.data();
        if (info_row != undefined && info_row.Id == id) {
            this.remove();
            borrado = true;
            return;
        }
    });

    if (borrado) {
        $(tabla).DataTable().draw(true);
        $('.material-tooltip').hide();
    }
}

function calcularCantidadRowsDataTable(hDisponible, hMio) {
    var hEncabezado = 49;
    hEncabezado = 24;
    var hItem = 49;
    if (hMio != undefined) {
        hItem = hMio;
    }

    hDisponible = hDisponible - hEncabezado;
    return Math.floor(hDisponible / hItem);
}

function procesarDatatable(idTabla, valores) {
    if (valores == undefined) {
        valores = {};
    }

    var verInfo = true;
    if ('VerInfo' in valores) {
        verInfo = valores.VerInfo;
    }

    var paginar = true;
    if ('Paginar' in valores) {
        paginar = valores.Paginar;
    }

    //Cols
    if (valores.Columnas == undefined) {
        valores.Columnas = [];
    }

    if (!('KeyId' in valores)) {
        valores.KeyId = 'Id';
    }

    var columnasPrimero = [];
    var columnasUltimo = [];
    $.each(valores.Columnas, function (index, col) {
        if ('Izquierda' in col) {
            columnasPrimero.push(col);
        } else {
            columnasUltimo.push(col);
        }
    });
    valores.Columnas = [];
    $.each(columnasPrimero, function (index, col) {
        valores.Columnas.push(col);
    });
    $.each(columnasUltimo, function (index, col) {
        valores.Columnas.push(col);
    });

    //Botones
    if (valores.Botones == undefined) {
        valores.Botones = [];
    }

    botonesTablas[idTabla] = {};

    if (valores.Botones.length != 0) {

        var w_ColumnaBotones = w_ColumnaBoton * valores.Botones.length + "px";

        botonesTablas[idTabla].botones = valores.Botones;
        valores.Columnas.push({
            title: "",
            data: null,
            orderable: false,
            witdh: w_ColumnaBotones,
            render: function (data, type, row) {
                var botones_html = "";

                var algunInvisible = false;

                $.each(valores.Botones, function (index, btn) {
                    if (btn.Oculto) {
                        var visible = true;

                        if ('Visible' in btn) {
                            if (typeof btn.Visible == 'function') {
                                visible = btn.Visible(row);
                            } else {
                                visible = btn.Visible;
                            }
                            if (!visible) {
                                return true;
                            }
                        }

                        if (visible) {
                            algunInvisible = true;
                        }
                        return true;
                    }

                    if ('Visible' in btn) {
                        var visible = true;
                        if (typeof btn.Visible == 'function') {
                            visible = btn.Visible(row);
                        } else {
                            visible = btn.Visible;
                        }
                        if (!visible) {
                            return true;
                        }
                    }


                    var titulo = '';
                    if ('Texto' in btn) {
                        titulo = btn.Texto;
                    } else {
                        if ('Titulo' in btn) {
                            titulo = btn.Titulo;
                        } else {
                            titulo = 'Sin texto';
                        }
                    }

                    var claseBoton;
                    if ('MostrarTooltip' in valores && valores.MostrarTooltip == false) {
                        claseBoton = 'btn btn-cuadrado chico no-select btnTabla waves-effect';
                    } else {
                        claseBoton = 'btn btn-cuadrado chico tooltipped no-select btnTabla waves-effect';
                    }

                    var html_boton = $('<a/>', {
                        class: claseBoton,
                        'data-position': 'bottom',
                        'data-delay': 50,
                        'data-tooltip': titulo
                        //'onclick': 'rowClick(' + idTabla + ', ' + index + ', ' + row.Id + ');'
                    });

                    $(html_boton).attr('id', idTabla + '_' + index);
                    $(html_boton).attr('idRow', row[valores.KeyId]);
                    $(html_boton).attr('index', index);

                    var html_icono = $('<i/>');
                    $(html_icono).addClass('material-icons');
                    $(html_icono).text(btn.Icono);
                    $(html_boton).append(html_icono);

                    botones_html += $(html_boton).prop('outerHTML');
                });

                if (algunInvisible) {
                    var html_boton = $('<a/>', {
                        class: 'btn btn-cuadrado chico tooltipped no-select btnMenu waves-effect',
                        'data-position': 'bottom',
                        'id': idTabla + '_overflow',
                        'data-delay': 50,
                        'data-tooltip': 'Mas...'
                        //'onclick': 'mostrarMenu(' + idTabla + ', ' + row.Id + ')'
                    });


                    var html_icono = $('<i/>');
                    $(html_icono).addClass('material-icons');
                    $(html_icono).text('more_vert');
                    $(html_boton).append(html_icono);

                    botones_html += $(html_boton).prop('outerHTML');
                }
                return "<div class='contenedor-botones'>" + botones_html + "</div>";
            }
        });
    }

    var colDefs = [];
    if ('Definiciones' in valores) {
        colDefs = valores.Definiciones;
    }
    colDefs.push({ "defaultContent": "", "targets": "_all" });

    var orden = [[0, 'desc']];
    if ('Orden' in valores) {
        orden = valores.Orden;
    }

    var ordenar = true;
    if ('Ordenar' in valores) {
        ordenar = valores.Ordenar;

    }
    if (!('OnFilaCreada' in valores)) {
        valores.OnFilaCreada = function () { };
    }

    if (!('Buscar' in valores)) {
        valores.Buscar = false;
    }

    if (!('OpcionesExportarExcel' in valores)) {
        valores.OpcionesExportarExcel = { extend: 'excelHtml5' };
    }

    if (!('OpcionesExportarPdf' in valores)) {
        valores.OpcionesExportarPdf = { extend: 'pdfHtml5' };
    }

    let dt = $('#' + idTabla).DataTable({


        dom: 'Bfrtip',
        buttons: [
              	'copy',
                'csv',
                valores.OpcionesExportarExcel,
                valores.OpcionesExportarPdf,
                'print'
        ],



        lengthChange: false,
        searching: valores.Buscar,
        "info": verInfo,
        "paging": paginar,
        pageLength: 10,
        pagingType: "simple",
        "bDestroy": true,
        "bAutoWidth": false,
        "deferRender": true,
        "columns": valores.Columnas,
        "columnDefs": colDefs,
        "order": orden,
        "bSort": ordenar,
        "oLanguage": {
            "sProcessing": "Procesando...",

            "sLengthMenu": "Tamaño de pagina _MENU_",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "(_START_-_END_ de _TOTAL_)",
            "sInfoEmpty": "",
            "sInfoFiltered": "(filtrado de un total de _MAX_)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente <i class='material-icons'>chevron_right</i>",
                "sPrevious": "<i class='material-icons'>chevron_left</i> Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        "rowCallback": function (row, data, index) {
            valores.OnFilaCreada(row, data, index);

            $('.material-tooltip').hide();
            $(row).find('.tooltipped').tooltip({ delay: 50 });

            $(row).off('contextmenu');
            $(row).bind("contextmenu", function (e) {
                // Avoid the real one
                e.preventDefault();

                $(this).MenuFlotanteTabla(e, idTabla, data, {
                    e: e,
                    PosicionX: "derecha",
                    PosicionY: "abajo"
                })
            });
        }
    });


    if ('InputBusqueda' in valores) {
        $(valores.InputBusqueda).on('input', function () {
            dt.search($(this).val()).draw(false);
        });
        $('#' + idTabla).parents('.dataTables_wrapper').find('.dataTables_filter').hide();
    }

    $('#' + idTabla + ' tbody').on('click', '.btnTabla', function (e) {
        var index = $(this).attr('index');
        var idRow = $(this).attr('idRow');

        var data = buscarRowPorId(idTabla, idRow, valores.KeyId);
        if (data == null) return;

        var btn = botonesTablas[idTabla].botones[index];
        if ('Validar' in btn) {
            var result = btn.Validar(data);
            if (result == undefined || result == false) {
                return;
            }
        }

        btn.OnClick(data, e);
    });

    $('#' + idTabla + ' tbody').on('click', '.btnMenu', function (e) {
        var data = dt.row($(this).parents('tr')).data();
        $(this).MenuFlotanteTabla(e, idTabla, data);
    });

    return dt;
}

function buscarRowPorId(idTabla, id, keyId) {
    if (keyId == undefined) keyId = "Id";

    if (id == undefined) return null;
    if (!typeof idTabla == 'string') {
        idTabla = $(idTabla).prop('id');
    }
    //Busco el indice de la persona a actualizar
    var i = -1;
    var info;
    var dt = $('#' + idTabla).DataTable();
    $.each(dt.data(), function (index, element) {
        if (element[keyId] == id) {
            i = index
            info = element;
        }
    });

    //Si no esta, corto
    if (i == -1) {
        return null;
    }

    return info;
}

function moverDatatableFooter(selectorTabla, selectorDestino) {
    $(selectorDestino).empty();
    $(selectorTabla + '_wrapper').find('.dataTables_info').detach().appendTo($(selectorDestino));
    $(selectorTabla + '_wrapper').find('.dataTables_paginate').detach().appendTo($(selectorDestino));
}

$.fn.GetData = function () {
    var data = [];
    $(this).DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
        data.push(this.data());
    });
    return data;
}

jQuery.fn.MenuFlotanteTabla = function (e, idTabla, data, valores) {

    if (valores == undefined) valores = {};
    if (!('e' in valores)) valores.e = undefined;

    //Soluciono el offset
    var iframe = $(this)[0].ownerDocument.defaultView.frameElement;
    var xOffset = 0;
    var yOffset = 0;
    while (iframe != undefined) {
        xOffset += $(iframe).offset().left;
        yOffset += $(iframe).offset().top;
        iframe = $(iframe)[0].ownerDocument.defaultView.frameElement;
    }


    var pl = parseInt($(this).css('padding-left').replace("px", ""));
    var pr = parseInt($(this).css('padding-right').replace("px", ""));
    var pb = parseInt($(this).css('padding-bottom').replace("px", ""));
    var pt = parseInt($(this).css('padding-top').replace("px", ""));

    var x, y;
    if (valores.e == undefined) {
        x = $(this).offset().left + xOffset;
        y = $(this).offset().top + yOffset;
    } else {
        var e = valores.e;
        x = e.originalEvent.clientX + xOffset;
        y = e.originalEvent.clientY + yOffset;
    }

    var hItem = 48;
    var hMax = 192;
    var w = 200;

    var botones = botonesTablas[idTabla].botones;
    if (botones == undefined || botones.length == 0) return;

    //Creo el menu
    var menu = $('<div>', {
        'class': 'card'
    });
    var id = new Date().getTime();
    $(menu).prop('id', id);
    $(menu).addClass('menu-flotante');

    $(menu).css('width', w + 'px');
    $(menu).css('max-height', hMax + 'px');

    //Calculo los items
    var hCalculado = 0;
    var ul = $('<ul>');
    $(ul).appendTo(menu);

    $.each(botones, function (index, btn) {
        if (valores.e == undefined) {
            if (!btn.Oculto) return true;
        }

        var visible = true;
        if ('Visible' in btn) {
            if (typeof (btn.Visible) === 'boolean') {
                visible = btn.Visible;
            } else {
                visible = btn.Visible(data);
            }
        }

        if (!visible) {
            return true;
        }

        var texto = '';
        if ('Texto' in btn) {
            texto = btn.Texto;
        } else {
            if ('Titulo' in btn) {
                texto = btn.Titulo;
            } else {
                texto = 'Sin texto';
            }
        }
        btn.Texto = texto;

        if (!('OnClick' in btn)) {
            btn.OnClick = function () { };
        }

        var hMenuItem = hItem;
        var separador = false;
        if ('Separador' in btn && btn.Separador == true) {
            hMenuItem = 32;
            separador = true;
        }

        hCalculado += hMenuItem;

        if (!('Id' in btn)) {
            btn.Id = new Date().getTime();
        }

        var li = $('<li>');
        $(li).appendTo(ul);
        $(li).addClass('menu-item waves-effect');
        $(li).attr('id', btn.Id);
        if (separador) {
            $(li).addClass('separador');
        }
        $(li).attr('index', index);

        if (separador) {
            //Texto
            var titulo = btn.Texto;
            if (typeof titulo != 'string') {
                titulo = btn.Texto(data);
            }
            var texto = $('<label>');
            $(texto).addClass('texto');
            $(texto).text(titulo);
            $(texto).appendTo(li);
            return true;
        }

        //Texto
        if ('Icono' in btn) {
            var icono = $('<i class="material-icons">' + btn.Icono + '</i>')
            $(icono).css('margin-right', '0.5rem');
            $(icono).appendTo(li);
        }
        var titulo = btn.Texto;
        if (typeof titulo != 'string') {
            titulo = btn.Texto(data);
        }
        var texto = $('<label>');
        $(texto).addClass('texto');
        $(texto).text(titulo);
        $(texto).appendTo(li);
    });

    //Limito el alto
    if (hCalculado < hMax) {
        $(menu).css('height', (hCalculado) + 'px');
    } else {
        hCalculado = hMax;
    }

    //Obtengo la posicion
    var posicion_x = "izquierda";
    var posicion_y = "abajo";
    if ('PosicionX' in valores) {
        posicion_x = valores.PosicionX;
    }
    if ('PosicionY' in valores) {
        posicion_y = valores.PosicionY;
    }

    //Calculo la posicion del MenuFlotante
    var objeto = calcularClaseMenuFoltante({
        Elemento: $(this),
        e: valores.e,
        PosicionX: posicion_x,
        PosicionY: posicion_y,
        X: x,
        Y: y,
        PaddingLeft: pl,
        PaddingRight: pr,
        PaddingTop: pt,
        PaddingBottom: pb,
        MenuFlotanteW: w,
        MenuFlotanteH: hCalculado
    });


    $(menu).css('left', (objeto.X) + 'px');
    $(menu).css('top', (objeto.Y) + 'px');
    $(menu).addClass(objeto.Clase);

    //Fondo
    var fondo = $('<div>');
    $(fondo).addClass('menu-flotante-fondo waves-effect');
    $(fondo).append($('<div>'));
    $(fondo).click(function () {
        $(menu).removeClass('abierto');
        $(fondo).removeClass('abierto');
        $(menu).fadeOut(300, function () {
            $(fondo).remove();
            $(menu).remove();
        });
    });

    //deshabilito click derecho en el fonmdo
    $(fondo).bind("contextmenu", function (event) {
        event.preventDefault();
    });

    top.$('body').append(fondo);
    top.$('body').append(menu);
    setTimeout(function () {
        $(menu).addClass('abierto');
        $(fondo).addClass('abierto');
    }, 100);
    top.$('#' + id).find('.menu-item').click(function () {
        var index = $(this).attr('index');

        var menuItem = botones[index];
        if ('Separador' in menuItem && menuItem.Separador) {
            //No hago nada
        } else {
            if ('Validar' in menuItem) {
                var result = menuItem.Validar(data);
                if (result == undefined || result == false) {
                    return;
                }
            }

            menuItem.OnClick(data);
            top.$(fondo).trigger('click');
        }
    });
}

function calcularClaseMenuFoltante(valores) {
    var element = valores.Elemento;
    var e = valores.e;
    var posicion_x = valores.PosicionX;
    var posicion_y = valores.PosicionY;
    var x = valores.X;
    var y = valores.Y;
    var pl = valores.PaddingLeft;
    var pr = valores.PaddingRight;
    var pt = valores.PaddingTop;
    var pb = valores.PaddingBottom;
    var w = valores.MenuFlotanteW;
    var h = valores.MenuFlotanteH;

    var clase;
    if (posicion_x == "izquierda") {
        if (posicion_y == "abajo") {
            clase = "abajo-izquierda";
        } else {
            clase = "arriba-izquierda";
        }
    } else {
        if (posicion_y == "abajo") {
            clase = "abajo-derecha";
        } else {
            clase = "arriba-derecha";
        }
    }

    var wElement = $(element).width();
    var hElement = $(element).height();

    switch (clase) {
        case 'abajo-izquierda': {
            //Como es para la izquierda, le resto a la posicion el ancho de la ventana
            x -= w;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                x += wElement + pl + pr;
            }
        } break;

        case 'abajo-derecha': {
            //No va nada aca porque es la posicion por defecto
        } break;

        case 'arriba-izquierda': {
            //Como es para la izquierda, le resto a la posicion el ancho de la ventana
            x -= w;

            //Como es para arriba, le resto a la posicion el alto de la ventana
            y -= h;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                x += wElement + pl + pr;
                y += hElement + pt + pb;
            }
        } break;

        case 'arriba-derecha': {
            //Como es para arriba, le resto a la posicion el alto de la ventana
            y -= h;

            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
            if (e == undefined) {
                y += hElement + pt + pb;
            }
        } break;
    }

    if (x < 0) {
        valores.PosicionX = "derecha";
        return calcularClaseMenuFoltante(valores);
    } else {
        var maxX = top.$('body').width();
        if (x + w > maxX) {
            valores.PosicionX = "izquierda";
            return calcularClaseMenuFoltante(valores);
        }
    }

    if (y < 0) {
        valores.PosicionY = "abajo";
        return calcularClaseMenuFoltante(valores);
    } else {
        var maxY = top.$('body').height();

        if (y + h > maxY) {
            valores.PosicionY = "arriba";
            return calcularClaseMenuFoltante(valores);
        }
    }

    var objeto = {};
    objeto.Clase = clase;
    objeto.X = x;
    objeto.Y = y;
    return objeto;
}



//    //-------------------------------------
//    // Columnas
//    //-------------------------------------

//    var cols = [
//        {
//            title: "Nombre",
//            data: "Nombre",
//            render: function (data, type, row) {
//                return '<div><span class="link-detalle tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-entity="' + row.Id + '">' + toTitleCase(data) + '</span></div>';
//            }
//        },
//        {
//            title: 'Secretaria',
//            render: function (data, type, row) {
//                return '<div><span>' + toTitleCase(row.Secretaria.Nombre) + '</span></div>';
//            }
//        },
//        {
//            title: 'Estado',
//            data: 'FechaBaja',
//            render: function (data, type, row) {
//                if (data != null) {
//                    return '<div><span>Dado de baja</span></div>';
//                } else {
//                    return '<div><span>Activo</span></div>';
//                }


//            }
//        },
//    ];

//    //Agrego las cols
//    if ('Columnas' in valores) {
//        $.each(valores.Columnas, function (index, val) {
//            cols.push(val);
//        });
//    }

//    valores.Columnas = cols;

//    //-------------------------------------
//    // Botones
//    //-------------------------------------

//    valores.Botones = [];


//    //-------------------------------------
//    // Eventos
//    //-------------------------------------

//    $('#' + idTabla).on('click', '.link-detalle', function () {
//        var idEntity = $(this).attr('id-entity');
//        verDetalle(idEntity);
//    });

//    $('#' + idTabla).on('dblclick', 'tr', function (e) {
//        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
//        verDetalle(id);
//    });


//    //-------------------------------------
//    // Acciones
//    //-------------------------------------

//    function verDetalle(idEntity) {
//        $('.material-tooltip').hide();
//        crearDialogoInformacionOrganicaDireccionDetalle({
//            Id: idEntity,
//            CallbackMensajes: function (tipo, mensaje) {
//                valores.CallbackMensajes(tipo, mensaje);
//            },
//            Callback: function () {
//                buscarDatos(idEntity)
//                .then(function (result) {
//                    actualizarFila(result);
//                });
//            }
//        });
//    }

//    function buscarDatos(id) {
//        return new Promise(function (callback, callbackError) {

//            crearAjax({
//                Url: ResolveUrl('~/Servicios/InformacionOrganicaDireccionService.asmx/GetById'),
//                Data: { id: id },
//                OnSuccess: function (result) {
//                    if (!result.Ok) {
//                        callbackError(result.Error);
//                        return;
//                    }

//                    callback(result.Return);
//                },
//                OnError: function (result) {
//                    callbackError('Error procesando la solicitud');
//                }
//            });
//        });
//    }

//    function actualizarFila(entity) {
//        //Busco el indice de la persona a actualizar
//        var index = -1;
//        var dt = $("#" + idTabla).DataTable();
//        dt.rows(function (idx, data, node) {
//            if (data.Id == entity.Id) {
//                index = idx;
//            }
//        });

//        //Si no esta, corto
//        if (index == -1) {
//            return;
//        }

//        //Actualizo
//        dt.row(index).data(entity);

//        //Inicializo el tooltip
//        dt.$('.tooltipped').tooltip({ delay: 50 });
//    }

//    return procesarDatatable(idTabla, valores);
//}

//jQuery.fn.DataTableInformacionOrganica = function (valores) {

//    var idTabla = '' + $(this).prop('id');

//    if (valores == undefined) {
//        valores = {};
//    }

//    //--------------------------------------
//    // Callbacks
//    //--------------------------------------

//    //Callback mensajes
//    if (!('CallbackMensajes' in valores)) {
//        valores.CallbackMensajes = function () { };
//    }

//    //Callback Cargando
//    if (!('CallbackCargando' in valores)) {
//        valores.CallbackCargando = function () { };
//    }

//    //--------------------------------------
//    //Orden
//    //--------------------------------------
//    if (!('Orden' in valores)) {
//        valores.Orden = [[1, 'desc']];
//    }

//    //-------------------------------------
//    // Columnas
//    //-------------------------------------

//    var cols = [
//        {
//            title: "Área",
//            render: function (data, type, row) {
//                return '<div><span class="link-detalle tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-entity="' + row.Id + '">' + toTitleCase(row.Area.Nombre) + '</span></div>';
//            }
//        },
//        {
//            title: 'Secretaria',
//            render: function (data, type, row) {
//                if (row.InformacionOrganica != undefined) {
//                    return '<div><span>' + toTitleCase(row.InformacionOrganica.Direccion.Secretaria.Nombre) + '</span></div>';
//                } else {
//                    return '<div><span></span></div>';
//                }
//            }
//        },
//        {
//            title: 'Direccion',
//            render: function (data, type, row) {
//                if (row.InformacionOrganica != undefined) {
//                    return '<div><span>' + toTitleCase(row.InformacionOrganica.Direccion.Nombre) + '</span></div>';
//                } else {
//                    return '<div><span></span></div>';
//                }
//            }
//        }
//    ];

//    //Agrego las cols
//    if ('Columnas' in valores) {
//        $.each(valores.Columnas, function (index, val) {
//            cols.push(val);
//        });
//    }

//    valores.Columnas = cols;

//    //-------------------------------------
//    // Botones
//    //-------------------------------------

//    valores.Botones = [];


//    //-------------------------------------
//    // Eventos
//    //-------------------------------------

//    $('#' + idTabla).on('click', '.link-detalle', function () {
//        var idEntity = $(this).attr('id-entity');
//        verDetalle(idEntity);
//    });

//    $('#' + idTabla).on('dblclick', 'tr', function (e) {
//        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
//        verDetalle(id);
//    });


//    //-------------------------------------
//    // Acciones
//    //-------------------------------------

//    function verDetalle(idArea) {
//        $('.material-tooltip').hide();
//        crearDialogoInformacionOrganicaDetalle({
//            Id: idArea,
//            CallbackMensajes: function (tipo, mensaje) {
//                valores.CallbackMensajes(tipo, mensaje);
//            },
//            Callback: function () {
//                buscarDatos(idArea)
//                .then(function (result) {
//                    actualizarFila(idArea, result);
//                });
//            }
//        });
//    }

//    function buscarDatos(id) {
//        return new Promise(function (callback, callbackError) {

//            crearAjax({
//                Url: ResolveUrl('~/Servicios/InformacionOrganicaService.asmx/GetByIdArea'),
//                Data: { idArea: id },
//                OnSuccess: function (result) {
//                    if (!result.Ok) {
//                        callbackError(result.Error);
//                        return;
//                    }

//                    callback(result.Return);
//                },
//                OnError: function (result) {
//                    callbackError('Error procesando la solicitud');
//                }
//            });
//        });
//    }

//    function actualizarFila(idEntity, entity) {
//        //Busco el indice de la persona a actualizar
//        var index = -1;
//        var dt = $("#" + idTabla).DataTable();
//        dt.rows(function (idx, data, node) {
//            if (data.Id == idEntity) {
//                index = idx;
//            }
//        });

//        //Si no esta, corto
//        if (index == -1) {
//            return;
//        }

//        //Actualizo
//        let data = dt.row(index).data();
//        data.InformacionOrganica = entity;
//        dt.row(index).data(data);

//        //Inicializo el tooltip
//        dt.$('.tooltipped').tooltip({ delay: 50 });
//    }

//    return procesarDatatable(idTabla, valores);
//}

//jQuery.fn.DataTableZona = function (valores) {

//    var idTabla = '' + $(this).prop('id');

//    if (valores == undefined) {
//        valores = {};
//    }

//    //--------------------------------------
//    // Callbacks
//    //--------------------------------------

//    //Callback mensajes
//    if (!('CallbackMensajes' in valores)) {
//        valores.CallbackMensajes = function () { };
//    }

//    //Callback Cargando
//    if (!('CallbackCargando' in valores)) {
//        valores.CallbackCargando = function () { };
//    }

//    //Callback
//    if (!('Callback' in valores)) {
//        valores.Callback = function () { };
//    }

//    //--------------------------------------
//    //Orden
//    //--------------------------------------
//    if (!('Orden' in valores)) {
//        valores.Orden = [[0, 'asc']];
//    }

//    //-------------------------------------
//    // Columnas
//    //-------------------------------------
//    var cols = [];

//    cols.push(
//        {
//            title: "Nombre",
//            data: 'Nombre',
//            render: function (data, type, row) {
//                return '<div><span class="link-detalle tooltipped" data-position="bottom" data-delay="50" data-tooltip="Ver detalle" id-entity="' + row.Id + '" id-area="' + row.AreaId + '">' + row.Nombre + '</span></div>';
//            }
//        },
//        {
//            title: "Area",
//            data: 'AreaNombre',
//            render: function (data, type, row) {
//                return '<div><span>' + row.AreaNombre + '</span></div>';
//            }
//        },
//        {
//            title: 'Estado',
//            render: function (data, type, row) {
//                if (row.FechaBaja == undefined) {
//                    return '<div><span>Activo</span></div>';
//                } else {
//                    return '<div><span>Dado de baja</span></div>';
//                }
//            }
//        }
//    );

//    $('#' + idTabla).on('click', '.link-detalle', function () {
//        var id = $(this).attr('id-entity');
//        var idArea = $(this).attr('id-area');
//        verDetalle(id, idArea);
//    });

//    $('#' + idTabla).on('dblclick', 'tr', function (e) {
//        let id = $('#' + idTabla).DataTable().row($(this)).data().Id;
//        let idArea = $('#' + idTabla).DataTable().row($(this)).data().AreaId;
//        verDetalle(id, idArea);
//    });

//    function verDetalle(id, idArea) {
//        $('.material-tooltip').hide();

//        crearDialogoZona({
//            Id: id,
//            IdArea: idArea,
//            CallbackMensajes: function (tipo, mensaje) {
//                valores.CallbackMensajes(tipo, mensaje);
//            },
//            CallbackCargando: function (mostrar, mensaje) {
//                valores.CallbackCargando(mostrar, mensaje);
//            },
//            Callback: function () {
//                buscarResultadoTabla(id, function (entity) {
//                    if (entity == undefined) return;
//                    actualizarFilaEnTabla(entity);
//                    valores.Callback(entity);
//                }, function () {
//                    valores.CallbackMensajes('Error', 'Error procesando la solicitud');
//                });
//            }
//        });
//    }

//    //Agrego las cols
//    if ('Columnas' in valores) {
//        $.each(valores.Columnas, function (index, val) {
//            cols.push(val);
//        });
//    }

//    valores.Columnas = cols;

//    //-------------------------------------
//    // Botones
//    //-------------------------------------

//    var botones = [];



//    if ('Botones' in valores) {
//        $.each(valores.Botones, function (index, val) {
//            botones.push(val);
//        });
//    }


//    valores.Botones = botones;


//    function buscarResultadoTabla(id, callback, callbackError) {
//        crearAjax({
//            Url: ResolveUrl('~/Servicios/ZonaService.asmx/GetResultadoTablaById'),
//            Data: { id: id },
//            OnSuccess: function (result) {
//                if (!result.Ok) {
//                    if (callbackError != undefined) {
//                        callbackError();
//                    }
//                    return;
//                }

//                callback(result.Return);
//            },
//            OnError: function (result) {
//                if (callbackError != undefined) {
//                    callbackError();
//                }
//            }
//        });
//    }

//    function actualizarFilaEnTabla(entity) {
//        //Busco el indice de la persona a actualizar
//        var index = -1;
//        var dt = $("#" + idTabla).DataTable();
//        dt.rows(function (idx, data, node) {
//            if (data.Id == entity.Id) {
//                index = idx;
//            }
//        });

//        //Si no esta, corto
//        if (index == -1) {
//            return;
//        }

//        //Actualizo
//        dt.row(index).data(entity);

//        //Inicializo el tooltip
//        dt.$('.tooltipped').tooltip({ delay: 50 });
//    }

//    return procesarDatatable(idTabla, valores);
//}



//function borrarFila(tabla, id) {
//    var borrado = false;
//    $(tabla).DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
//        var info_row = this.data();
//        if (info_row != undefined && info_row.Id == id) {
//            this.remove();
//            borrado = true;
//            return;
//        }
//    });

//    if (borrado) {
//        $(tabla).DataTable().draw(true);
//        $('.material-tooltip').hide();
//    }
//}

//function calcularCantidadRowsDataTable(hDisponible, hMio) {
//    var hEncabezado = 49;
//    hEncabezado = 24;
//    var hItem = 49;
//    if (hMio != undefined) {
//        hItem = hMio;
//    }

//    hDisponible = hDisponible - hEncabezado;
//    return Math.floor(hDisponible / hItem);
//}

//function procesarDatatable(idTabla, valores) {
//    if (valores == undefined) {
//        valores = {};
//    }

//    var verInfo = true;
//    if ('VerInfo' in valores) {
//        verInfo = valores.VerInfo;
//    }

//    var paginar = true;
//    if ('Paginar' in valores) {
//        paginar = valores.Paginar;
//    }

//    //Cols
//    if (valores.Columnas == undefined) {
//        valores.Columnas = [];
//    }

//    if (!('KeyId' in valores)) {
//        valores.KeyId = 'Id';
//    }

//    var columnasPrimero = [];
//    var columnasUltimo = [];
//    $.each(valores.Columnas, function (index, col) {
//        if ('Izquierda' in col) {
//            columnasPrimero.push(col);
//        } else {
//            columnasUltimo.push(col);
//        }
//    });
//    valores.Columnas = [];
//    $.each(columnasPrimero, function (index, col) {
//        valores.Columnas.push(col);
//    });
//    $.each(columnasUltimo, function (index, col) {
//        valores.Columnas.push(col);
//    });

//    //Botones
//    if (valores.Botones == undefined) {
//        valores.Botones = [];
//    }

//    botonesTablas[idTabla] = {};

//    if (valores.Botones.length != 0) {

//        var w_ColumnaBotones = w_ColumnaBoton * valores.Botones.length + "px";

//        botonesTablas[idTabla].botones = valores.Botones;
//        valores.Columnas.push({
//            title: "",
//            data: null,
//            orderable: false,
//            witdh: w_ColumnaBotones,
//            render: function (data, type, row) {
//                var botones_html = "";

//                var algunInvisible = false;

//                $.each(valores.Botones, function (index, btn) {
//                    if (btn.Oculto) {
//                        var visible = true;

//                        if ('Visible' in btn) {
//                            if (typeof btn.Visible == 'function') {
//                                visible = btn.Visible(row);
//                            } else {
//                                visible = btn.Visible;
//                            }
//                            if (!visible) {
//                                return true;
//                            }
//                        }

//                        if (visible) {
//                            algunInvisible = true;
//                        }
//                        return true;
//                    }

//                    if ('Visible' in btn) {
//                        var visible = true;
//                        if (typeof btn.Visible == 'function') {
//                            visible = btn.Visible(row);
//                        } else {
//                            visible = btn.Visible;
//                        }
//                        if (!visible) {
//                            return true;
//                        }
//                    }


//                    var titulo = '';
//                    if ('Texto' in btn) {
//                        titulo = btn.Texto;
//                    } else {
//                        if ('Titulo' in btn) {
//                            titulo = btn.Titulo;
//                        } else {
//                            titulo = 'Sin texto';
//                        }
//                    }

//                    var claseBoton;
//                    if ('MostrarTooltip' in valores && valores.MostrarTooltip == false) {
//                        claseBoton = 'btn btn-cuadrado chico no-select btnTabla waves-effect';
//                    } else {
//                        claseBoton = 'btn btn-cuadrado chico tooltipped no-select btnTabla waves-effect';
//                    }

//                    var html_boton = $('<a/>', {
//                        class: claseBoton,
//                        'data-position': 'bottom',
//                        'data-delay': 50,
//                        'data-tooltip': titulo
//                        //'onclick': 'rowClick(' + idTabla + ', ' + index + ', ' + row.Id + ');'
//                    });

//                    $(html_boton).attr('id', idTabla + '_' + index);
//                    $(html_boton).attr('idRow', row[valores.KeyId]);
//                    $(html_boton).attr('index', index);

//                    var html_icono = $('<i/>');
//                    $(html_icono).addClass('material-icons');
//                    $(html_icono).text(btn.Icono);
//                    $(html_boton).append(html_icono);

//                    botones_html += $(html_boton).prop('outerHTML');
//                });

//                if (algunInvisible) {
//                    var html_boton = $('<a/>', {
//                        class: 'btn btn-cuadrado chico tooltipped no-select btnMenu waves-effect',
//                        'data-position': 'bottom',
//                        'id': idTabla + '_overflow',
//                        'data-delay': 50,
//                        'data-tooltip': 'Mas...'
//                        //'onclick': 'mostrarMenu(' + idTabla + ', ' + row.Id + ')'
//                    });


//                    var html_icono = $('<i/>');
//                    $(html_icono).addClass('material-icons');
//                    $(html_icono).text('more_vert');
//                    $(html_boton).append(html_icono);

//                    botones_html += $(html_boton).prop('outerHTML');
//                }
//                return "<div class='contenedor-botones'>" + botones_html + "</div>";
//            }
//        });
//    }

//    var colDefs = [];
//    if ('Definiciones' in valores) {
//        colDefs = valores.Definiciones;
//    }
//    colDefs.push({ "defaultContent": "", "targets": "_all" });

//    var orden = [[0, 'desc']];
//    if ('Orden' in valores) {
//        orden = valores.Orden;
//    }

//    var ordenar = true;
//    if ('Ordenar' in valores) {
//        ordenar = valores.Ordenar;

//    }
//    if (!('OnFilaCreada' in valores)) {
//        valores.OnFilaCreada = function () { };
//    }

//    if (!('Buscar' in valores)) {
//        valores.Buscar = false;
//    }

//    if (!('OpcionesExportarExcel' in valores)) {
//        valores.OpcionesExportarExcel = { extend: 'excelHtml5' };
//    }

//    if (!('OpcionesExportarPdf' in valores)) {
//        valores.OpcionesExportarPdf = { extend: 'pdfHtml5' };
//    }

//    let dt = $('#' + idTabla).DataTable({


//        dom: 'Bfrtip',
//        buttons: [
//              	'copy',
//                'csv',
//                valores.OpcionesExportarExcel,
//                valores.OpcionesExportarPdf,
//                'print'
//        ],



//        lengthChange: false,
//        searching: valores.Buscar,
//        "info": verInfo,
//        "paging": paginar,
//        pageLength: 10,
//        pagingType: "simple",
//        "bDestroy": true,
//        "bAutoWidth": false,
//        "deferRender": true,
//        "columns": valores.Columnas,
//        "columnDefs": colDefs,
//        "order": orden,
//        "bSort": ordenar,
//        "oLanguage": {
//            "sProcessing": "Procesando...",

//            "sLengthMenu": "Tamaño de pagina _MENU_",
//            "sZeroRecords": "No se encontraron resultados",
//            "sEmptyTable": "Ningún dato disponible en esta tabla",
//            "sInfo": "(_START_-_END_ de _TOTAL_)",
//            "sInfoEmpty": "",
//            "sInfoFiltered": "(filtrado de un total de _MAX_)",
//            "sInfoPostFix": "",
//            "sSearch": "Buscar:",
//            "sUrl": "",
//            "sInfoThousands": ",",
//            "sLoadingRecords": "Cargando...",
//            "oPaginate": {
//                "sFirst": "Primero",
//                "sLast": "Último",
//                "sNext": "Siguiente <i class='material-icons'>chevron_right</i>",
//                "sPrevious": "<i class='material-icons'>chevron_left</i> Anterior"
//            },
//            "oAria": {
//                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
//                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
//            }
//        },
//        "rowCallback": function (row, data, index) {
//            valores.OnFilaCreada(row, data, index);

//            $('.material-tooltip').hide();
//            $(row).find('.tooltipped').tooltip({ delay: 50 });

//            $(row).off('contextmenu');
//            $(row).bind("contextmenu", function (e) {
//                // Avoid the real one
//                e.preventDefault();

//                $(this).MenuFlotanteTabla(e, idTabla, data, {
//                    e: e,
//                    PosicionX: "derecha",
//                    PosicionY: "abajo"
//                })
//            });
//        }
//    });


//    if ('InputBusqueda' in valores) {
//        $(valores.InputBusqueda).on('input', function () {
//            dt.search($(this).val()).draw(false);
//        });
//        $('#' + idTabla).parents('.dataTables_wrapper').find('.dataTables_filter').hide();
//    }

//    $('#' + idTabla + ' tbody').on('click', '.btnTabla', function (e) {
//        var index = $(this).attr('index');
//        var idRow = $(this).attr('idRow');

//        var data = buscarRowPorId(idTabla, idRow, valores.KeyId);
//        if (data == null) return;

//        var btn = botonesTablas[idTabla].botones[index];
//        if ('Validar' in btn) {
//            var result = btn.Validar(data);
//            if (result == undefined || result == false) {
//                return;
//            }
//        }

//        btn.OnClick(data, e);
//    });

//    $('#' + idTabla + ' tbody').on('click', '.btnMenu', function (e) {
//        var data = dt.row($(this).parents('tr')).data();
//        $(this).MenuFlotanteTabla(e, idTabla, data);
//    });

//    return dt;
//}

//function buscarRowPorId(idTabla, id, keyId) {
//    if (keyId == undefined) keyId = "Id";

//    if (id == undefined) return null;
//    if (!typeof idTabla == 'string') {
//        idTabla = $(idTabla).prop('id');
//    }
//    //Busco el indice de la persona a actualizar
//    var i = -1;
//    var info;
//    var dt = $('#' + idTabla).DataTable();
//    $.each(dt.data(), function (index, element) {
//        if (element[keyId] == id) {
//            i = index
//            info = element;
//        }
//    });

//    //Si no esta, corto
//    if (i == -1) {
//        return null;
//    }

//    return info;
//}

//function moverDatatableFooter(selectorTabla, selectorDestino) {
//    $(selectorDestino).empty();
//    $(selectorTabla + '_wrapper').find('.dataTables_info').detach().appendTo($(selectorDestino));
//    $(selectorTabla + '_wrapper').find('.dataTables_paginate').detach().appendTo($(selectorDestino));
//}

//$.fn.GetData = function () {
//    var data = [];
//    $(this).DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
//        data.push(this.data());
//    });
//    return data;
//}

//jQuery.fn.MenuFlotanteTabla = function (e, idTabla, data, valores) {

//    if (valores == undefined) valores = {};
//    if (!('e' in valores)) valores.e = undefined;

//    //Soluciono el offset
//    var iframe = $(this)[0].ownerDocument.defaultView.frameElement;
//    var xOffset = 0;
//    var yOffset = 0;
//    while (iframe != undefined) {
//        xOffset += $(iframe).offset().left;
//        yOffset += $(iframe).offset().top;
//        iframe = $(iframe)[0].ownerDocument.defaultView.frameElement;
//    }


//    var pl = parseInt($(this).css('padding-left').replace("px", ""));
//    var pr = parseInt($(this).css('padding-right').replace("px", ""));
//    var pb = parseInt($(this).css('padding-bottom').replace("px", ""));
//    var pt = parseInt($(this).css('padding-top').replace("px", ""));

//    var x, y;
//    if (valores.e == undefined) {
//        x = $(this).offset().left + xOffset;
//        y = $(this).offset().top + yOffset;
//    } else {
//        var e = valores.e;
//        x = e.originalEvent.clientX + xOffset;
//        y = e.originalEvent.clientY + yOffset;
//    }

//    var hItem = 48;
//    var hMax = 192;
//    var w = 200;

//    var botones = botonesTablas[idTabla].botones;
//    if (botones == undefined || botones.length == 0) return;

//    //Creo el menu
//    var menu = $('<div>', {
//        'class': 'card'
//    });
//    var id = new Date().getTime();
//    $(menu).prop('id', id);
//    $(menu).addClass('menu-flotante');

//    $(menu).css('width', w + 'px');
//    $(menu).css('max-height', hMax + 'px');

//    //Calculo los items
//    var hCalculado = 0;
//    var ul = $('<ul>');
//    $(ul).appendTo(menu);

//    $.each(botones, function (index, btn) {
//        if (valores.e == undefined) {
//            if (!btn.Oculto) return true;
//        }

//        var visible = true;
//        if ('Visible' in btn) {
//            if (typeof (btn.Visible) === 'boolean') {
//                visible = btn.Visible;
//            } else {
//                visible = btn.Visible(data);
//            }
//        }

//        if (!visible) {
//            return true;
//        }

//        var texto = '';
//        if ('Texto' in btn) {
//            texto = btn.Texto;
//        } else {
//            if ('Titulo' in btn) {
//                texto = btn.Titulo;
//            } else {
//                texto = 'Sin texto';
//            }
//        }
//        btn.Texto = texto;

//        if (!('OnClick' in btn)) {
//            btn.OnClick = function () { };
//        }

//        var hMenuItem = hItem;
//        var separador = false;
//        if ('Separador' in btn && btn.Separador == true) {
//            hMenuItem = 32;
//            separador = true;
//        }

//        hCalculado += hMenuItem;

//        if (!('Id' in btn)) {
//            btn.Id = new Date().getTime();
//        }

//        var li = $('<li>');
//        $(li).appendTo(ul);
//        $(li).addClass('menu-item waves-effect');
//        $(li).attr('id', btn.Id);
//        if (separador) {
//            $(li).addClass('separador');
//        }
//        $(li).attr('index', index);

//        if (separador) {
//            //Texto
//            var titulo = btn.Texto;
//            if (typeof titulo != 'string') {
//                titulo = btn.Texto(data);
//            }
//            var texto = $('<label>');
//            $(texto).addClass('texto');
//            $(texto).text(titulo);
//            $(texto).appendTo(li);
//            return true;
//        }

//        //Texto
//        if ('Icono' in btn) {
//            var icono = $('<i class="material-icons">' + btn.Icono + '</i>')
//            $(icono).css('margin-right', '0.5rem');
//            $(icono).appendTo(li);
//        }
//        var titulo = btn.Texto;
//        if (typeof titulo != 'string') {
//            titulo = btn.Texto(data);
//        }
//        var texto = $('<label>');
//        $(texto).addClass('texto');
//        $(texto).text(titulo);
//        $(texto).appendTo(li);
//    });

//    //Limito el alto
//    if (hCalculado < hMax) {
//        $(menu).css('height', (hCalculado) + 'px');
//    } else {
//        hCalculado = hMax;
//    }

//    //Obtengo la posicion
//    var posicion_x = "izquierda";
//    var posicion_y = "abajo";
//    if ('PosicionX' in valores) {
//        posicion_x = valores.PosicionX;
//    }
//    if ('PosicionY' in valores) {
//        posicion_y = valores.PosicionY;
//    }

//    //Calculo la posicion del MenuFlotante
//    var objeto = calcularClaseMenuFoltante({
//        Elemento: $(this),
//        e: valores.e,
//        PosicionX: posicion_x,
//        PosicionY: posicion_y,
//        X: x,
//        Y: y,
//        PaddingLeft: pl,
//        PaddingRight: pr,
//        PaddingTop: pt,
//        PaddingBottom: pb,
//        MenuFlotanteW: w,
//        MenuFlotanteH: hCalculado
//    });


//    $(menu).css('left', (objeto.X) + 'px');
//    $(menu).css('top', (objeto.Y) + 'px');
//    $(menu).addClass(objeto.Clase);

//    //Fondo
//    var fondo = $('<div>');
//    $(fondo).addClass('menu-flotante-fondo waves-effect');
//    $(fondo).append($('<div>'));
//    $(fondo).click(function () {
//        $(menu).removeClass('abierto');
//        $(fondo).removeClass('abierto');
//        $(menu).fadeOut(300, function () {
//            $(fondo).remove();
//            $(menu).remove();
//        });
//    });

//    //deshabilito click derecho en el fonmdo
//    $(fondo).bind("contextmenu", function (event) {
//        event.preventDefault();
//    });

//    top.$('body').append(fondo);
//    top.$('body').append(menu);
//    setTimeout(function () {
//        $(menu).addClass('abierto');
//        $(fondo).addClass('abierto');
//    }, 100);
//    top.$('#' + id).find('.menu-item').click(function () {
//        var index = $(this).attr('index');

//        var menuItem = botones[index];
//        if ('Separador' in menuItem && menuItem.Separador) {
//            //No hago nada
//        } else {
//            if ('Validar' in menuItem) {
//                var result = menuItem.Validar(data);
//                if (result == undefined || result == false) {
//                    return;
//                }
//            }

//            menuItem.OnClick(data);
//            top.$(fondo).trigger('click');
//        }
//    });
//}

//function calcularClaseMenuFoltante(valores) {
//    var element = valores.Elemento;
//    var e = valores.e;
//    var posicion_x = valores.PosicionX;
//    var posicion_y = valores.PosicionY;
//    var x = valores.X;
//    var y = valores.Y;
//    var pl = valores.PaddingLeft;
//    var pr = valores.PaddingRight;
//    var pt = valores.PaddingTop;
//    var pb = valores.PaddingBottom;
//    var w = valores.MenuFlotanteW;
//    var h = valores.MenuFlotanteH;

//    var clase;
//    if (posicion_x == "izquierda") {
//        if (posicion_y == "abajo") {
//            clase = "abajo-izquierda";
//        } else {
//            clase = "arriba-izquierda";
//        }
//    } else {
//        if (posicion_y == "abajo") {
//            clase = "abajo-derecha";
//        } else {
//            clase = "arriba-derecha";
//        }
//    }

//    var wElement = $(element).width();
//    var hElement = $(element).height();

//    switch (clase) {
//        case 'abajo-izquierda': {
//            //Como es para la izquierda, le resto a la posicion el ancho de la ventana
//            x -= w;

//            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
//            if (e == undefined) {
//                x += wElement + pl + pr;
//            }
//        } break;

//        case 'abajo-derecha': {
//            //No va nada aca porque es la posicion por defecto
//        } break;

//        case 'arriba-izquierda': {
//            //Como es para la izquierda, le resto a la posicion el ancho de la ventana
//            x -= w;

//            //Como es para arriba, le resto a la posicion el alto de la ventana
//            y -= h;

//            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
//            if (e == undefined) {
//                x += wElement + pl + pr;
//                y += hElement + pt + pb;
//            }
//        } break;

//        case 'arriba-derecha': {
//            //Como es para arriba, le resto a la posicion el alto de la ventana
//            y -= h;

//            //Si no mando el evento de click, quiere decir que solo me fijo en el elemento que inicia el popup, entonces arreglo la posicion segun su tamaño y padding
//            if (e == undefined) {
//                y += hElement + pt + pb;
//            }
//        } break;
//    }

//    if (x < 0) {
//        valores.PosicionX = "derecha";
//        return calcularClaseMenuFoltante(valores);
//    } else {
//        var maxX = top.$('body').width();
//        if (x + w > maxX) {
//            valores.PosicionX = "izquierda";
//            return calcularClaseMenuFoltante(valores);
//        }
//    }

//    if (y < 0) {
//        valores.PosicionY = "abajo";
//        return calcularClaseMenuFoltante(valores);
//    } else {
//        var maxY = top.$('body').height();

//        if (y + h > maxY) {
//            valores.PosicionY = "arriba";
//            return calcularClaseMenuFoltante(valores);
//        }
//    }

//    var objeto = {};
//    objeto.Clase = clase;
//    objeto.X = x;
//    objeto.Y = y;
//    return objeto;
//}


///* Estados */

//function ordenarEstados(estados) {

//}



//////Traduccion date picker
////$.extend($.fn.pickadate.defaults, {
////    monthsFull: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
////    monthsShort: ['ene', 'feb', 'mar', 'abr', 'may', 'jun', 'jul', 'ago', 'sep', 'oct', 'nov', 'dic'],
////    weekdaysFull: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
////    weekdaysShort: ['dom', 'lun', 'mar', 'mié', 'jue', 'vie', 'sáb'],
////    weekdaysLetter: ['D', 'L', 'M', 'M', 'J', 'V', 'S'],
////    labelMonthNext: 'Mes siguiente',
////    labelMonthPrev: 'Mes anterior',
////    labelMonthSelect: 'Selecionar mes',
////    labelYearSelect: 'Seleccionar año',
////    today: 'hoy',
////    clear: 'borrar',
////    close: 'cerrar',
////    firstDay: 1,
////    format: 'dddd d !de mmmm !de yyyy',
////    formatSubmit: 'dd/mm/yyyy'
////});

//////Forms
////$.validator.setDefaults({
////    ignore: [],
////    rules: {
////        dateField: {
////            fecha: true
////        },
////        nombre: {
////            lettersonly: true
////        },
////        apellido: {
////            lettersonly: true
////        },
////        numeroEntero: {
////            number: true
////        },
////        dni: {
////            number: true,
////            minlength: 7
////        },
////        cuil: {
////            cuil: true
////        },
////        email: {
////            email: true
////        },
////        telefono: {
////            number: true
////        },
////        repeatPassword: {
////            equalTo: "#input_Password"
////        },
////        selectRequerido: {
////            select_requerido: true
////        }
////    },
////    messages: {
////        repeatPassword: "Las contraseñas no coinciden",
////    },
////    errorClass: 'error',
////    validClass: "",
////    errorPlacement: function (error, element) {
////        var contenedorError = $(element).siblings(".input-error");
////        $(contenedorError).find('a').text(error.text());
////    },
////    submitHandler: function (form) {
////        console.log('form ok');
////    },
////});

//////Traduccion del validador
////jQuery.extend(jQuery.validator.messages, {
////    required: "Dato requerido.",
////    remote: "Verificar dato.",
////    email: "E-mail inválido.",
////    url: "URL inválida.",
////    date: "Fecha inválida.",
////    dateISO: "Fecha inválida.",
////    number: "Dato inválido.",
////    digits: "Solo se permiten digitos.",
////    creditcard: "Dato inválido.",
////    equalTo: "Los datos debes ser iguales.",
////    accept: "Extensión invalida.",
////    confirmarPassword: "Las contraseñas no coinciden",
////    maxlength: jQuery.validator.format("Máximo {0} caracteres."),
////    minlength: jQuery.validator.format("Al menos {0} caracteres."),
////    rangelength: jQuery.validator.format("Ingrese entre {0} y {1} caracteres."),
////    range: jQuery.validator.format("Ingrese un valor entre {0} y {1}."),
////    max: jQuery.validator.format("Ingrese un valor igual o menor a {0}."),
////    min: jQuery.validator.format("Ingrese un valor igual o mayor a {0}.")
////});

//////Validador de los selects
////jQuery.validator.addMethod("select_requerido", function (value, element) {
////    return this.optional(element) || (parseInt(value) != -1);
////}, "Dato requerido.");
////jQuery.validator.addMethod("lettersonly", function (value, element) {
////    return this.optional(element) || /^[a-z áãâäàéêëèíîïìóõôöòúûüùçñ]+$/i.test(value);
////}, "Solo letras permitidas");
////jQuery.validator.addMethod("fecha", function (value, element) {
////    return this.optional(element) || moment(value, "DD/MM/YYYY").isValid();
////}, "Fecha inválida");
////jQuery.validator.addMethod("cuil", function (value, element) {
////    var cuil = validaCuit(value);
////    return this.optional(element) || cuil;
////}, "Cuil invalido");

////jQuery.validator.addMethod("fede", function (value, element) {
////    var cuil = value == 'fede';
////    return this.optional(element) || cuil;
////}, "Solo fede permitido");

////function validaCuit(sCUIT) {
////    var aMult = '5432765432';
////    var aMult = aMult.split('');

////    if (sCUIT && sCUIT.length == 11) {
////        aCUIT = sCUIT.split('');
////        var iResult = 0;
////        for (i = 0; i <= 9; i++) {
////            iResult += aCUIT[i] * aMult[i];
////        }
////        iResult = (iResult % 11);
////        iResult = 11 - iResult;

////        if (iResult == 11) iResult = 0;
////        if (iResult == 10) iResult = 9;

////        if (iResult == aCUIT[10]) {
////            return true;
////        }
////    }
////    return false;
////}

////$(function () {
////    //evento que tomar el enter en toda la pagina y dispara el buscar...
////    //$("form").submit(function (e) {
////    //    return false;
////    //});

////    $('form input').keypress(function (e) {
////        if (e.which == 13) {
////            $(this).parents('form').submit();
////            return false;
////        }
////    });

////    //Ajax cancel
////    setupCancelarAjax();

////    //Forms
////    initForms();

////    jQuery.validator.addClassRules("select-requerido", {
////        select_requerido: true
////    });

////});

////function initForms() {
////    var inputs = $(".input-field > input, .input-field > textarea, .input-field > select");

////    $.each(inputs, function (index, input) {
////        var contenedorError = $(input).siblings(".input-error");
////        if ($(input).siblings(".input-error").length == 0) {
////            contenedorError = $('<div class="input-error">');
////            $(contenedorError).append('<a>');
////            $(input).parent().append(contenedorError);
////        }
////    });

////    try {
////        $('select').select2().on("change", function (e) {
////            try {
////                $(this).valid(); //jquery validation script validate on change                
////            }
////            catch (ex) {

////            }
////        });
////    } catch (e) {

////    }
////}

////var xhrPool = [];
////function setupCancelarAjax() {
////    $(document).ajaxSend(function (e, jqXHR, options) {
////        xhrPool.push(jqXHR);
////    });
////    $(document).ajaxComplete(function (e, jqXHR, options) {
////        xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR });
////    });

////    var oldbeforeunload = window.onbeforeunload;
////    window.onbeforeunload = function () {
////        var r = oldbeforeunload ? oldbeforeunload() : undefined;
////        if (r == undefined) {
////            // only cancel requests if there is no prompt to stay on the page
////            // if there is a prompt, it will likely give the requests enough time to finish
////            cancelarAjax();
////        }
////        return r;
////    }
////}

////function cancelarAjax() {
////    $.each(xhrPool, function (idx, jqXHR) {
////        jqXHR.abort();
////    });
////};

////function crearAjax(valores) {
////    $.ajax({
////        url: ResolveUrl(valores.Url),
////        data: JSON.stringify(valores.Data),
////        dataType: 'json',
////        contentType: 'application/json; charset=utf-8',
////        type: 'POST',
////        success: function (result) {
////            result = result.d;
////            valores.OnSuccess(result);
////        },
////        error: function (result) {
////            valores.OnError(result);
////            console.log(valores.Url);
////            console.log(result);
////            console.log(valores.Data);

////            if (result.readyState == 0) {
////                try {
////                    //top.SinConexion();
////                }
////                catch (e) {

////                }
////            }
////        }
////    });
////}

////function ordenarJSON(data, prop, asc) {
////    var datasort = data.sort(function (a, b) {
////        if (asc) {
////            return (a[prop] > b[prop]) ? 1 : ((a[prop] < b[prop]) ? -1 : 0);
////        } else {
////            return (b[prop] > a[prop]) ? 1 : ((b[prop] < a[prop]) ? -1 : 0);
////        }
////    });
////    return datasort;
////}

////jQuery.fn.CargarSelect = function (valores) {

////    if (!('Data' in valores) || valores.Data == null) {
////        alert('Debe enviar Data');
////        return;
////    }

////    if (!('Value' in valores) || valores.Value == null) {
////        alert('Debe enviar Key');
////        return;
////    }

////    if (!('Text' in valores) || valores.Text == null) {
////        alert('Debe enviar Text');
////        return;
////    }

////    var titleCase = true;
////    if ('TitleCase' in valores) {
////        titleCase = valores.TitleCase;
////    }

////    //Creo mis array de datos
////    var opciones = [];
////    $.each(valores.Data, function (index, val) {
////        var texto = val[valores.Text];
////        if (titleCase) {
////            texto = toTitleCase(texto);
////        }
////        var mihtml = null;
////        if ('html' in val) {
////            mihtml = val.html;
////        }
////        opciones[index] = { id: val[valores.Value], text: texto, html: mihtml };
////    });

////    //ordeno
////    if ('Sort' in valores && valores.Sort) {
////        var ascendente = true;
////        if ('Asc' in valores) {
////            ascendente = valores.Asc;
////        }
////        valores.Data = ordenarJSON(opciones, 'text', ascendente);
////    }

////    //Agrego por defecto
////    if ('Default' in valores && valores.Default != null) {
////        var opciones2 = [];
////        opciones2[0] = { id: -1, text: valores.Default };
////        $.each(opciones, function (index, val) {
////            opciones2[index + 1] = val;
////        });
////        opciones = opciones2;
////    }

////    ////Cargo en el select
////    //var output = "";
////    //$.each(opciones, function (index, val) {
////    //    var text = val.text;
////    //    if (val.html != null) {
////    //        text = val.html;
////    //    }
////    //    output += '<option value="' + val.value + '" data-foo="">' + text + '</option>';
////    //});
////    //$(this).html(output);

////    $(this).empty();

////    $(this).select2({
////        data: opciones,
////        templateResult: function (d) {
////            if (d.html) {
////                return $(d.html);
////            }
////            return d.text;
////        },
////        templateSelection: function (d) {
////            //if (d.html) {
////            //    return $(d.html);
////            //}
////            return d.text;
////        },
////        "language": {
////            "noResults": function () {
////                return "Sin resultados";
////            }
////        }
////    });
////}

////jQuery.fn.AgregarCheckbox = function (valores) {
////    $('<input />', { type: 'checkbox', value: valores.Value, id: 'cb' + valores.Value }).appendTo($(this));
////    $('<label />', { 'for': 'cb' + valores.Value, color: 'black', text: valores.Name, width: '180px', id: 'cblb' + valores.Value }).appendTo($(this));

////}

////jQuery.fn.AgregarIndicadorCargando = function (valores) {
////    if (valores == undefined) valores = {};
////    var opaco = false;
////    if ('Opaco' in valores) {
////        opaco = valores.Opaco;
////    }

////    var clases = opaco ? 'opaco' : '';

////    var spinner = ' <div class="preloader-wrapper big active"><div class="spinner-layer"><div class="circle-clipper left"><div class="circle"></div></div><div class="gap-patch"><div class="circle"></div></div><div class="circle-clipper right"><div class="circle"></div></div></div></div>';
////    var divCargando = "<div id='indicadorCargando' class='contenedor' style='position:absolute; top:0; left:0; margin:0; padding:0; width:100%; height:100%'><div class='cargando " + clases + "'>" + spinner + "</div></div>";
////    $(divCargando).appendTo($(this));
////}

////jQuery.fn.GetIndicadorCargando = function (valores) {
////    return $(this).find('#indicadorCargando');
////}

////jQuery.fn.MostrarIndicadorCargando = function (valores) {
////    $(this).AgregarIndicadorCargando(valores);
////    $(this).find('#indicadorCargando').fadeIn(300);
////}
////jQuery.fn.OcultarIndicadorCargando = function (valores) {
////    $(this).find('#indicadorCargando').fadeOut(300);
////}

////jQuery.fn.enterKey = function (fnc) {
////    return this.each(function () {
////        $(this).keypress(function (ev) {
////            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
////            if (keycode == '13') {
////                fnc.call(this, ev);
////            }
////        })
////    });
////}

////function ResolveUrl(url) {
////    if (url.indexOf("~/") == 0) {
////        var base = baseUrl;
////        if (base.substring(base.length - 1) != "/") {
////            base += "/";
////        }
////        url = base + url.substring(2);
////    }
////    return url;
////}

////function parse(json) {
////    if (json == null || json == undefined || json == "" || typeof json != 'string') {
////        return json;
////    }
////    return JSON.parse(json.replace(/\n/g, "\\n")
////        .replace(/\r/g, "\\r")
////        .replace(/\t/g, "\\t")
////        .replace(/\f/g, "\\f"));
////}

////function isMobile() {
////    var check = false;
////    (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
////    return check;
////};

////function isMobileOrTablet() {
////    var check = false;
////    (function (a) { if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino|android|ipad|playbook|silk/i.test(a) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0, 4))) check = true; })(navigator.userAgent || navigator.vendor || window.opera);
////    return check;
////};

////function toTitleCase(str) {
////    if (str == null) return "";
////    str = str.toLowerCase();
////    return str.replace(/(?:^|\s)\w/g, function (match) {
////        return match.toUpperCase();
////    });
////}

////function toDateString(fecha) {
////    return moment(fecha).format('DD/MM/YYYY');
////}

////function toDateTimeString(fecha) {
////    return moment(fecha).format('DD/MM/YYYY hh:mm:ss a');
////}

////function getUrlParameter(sParam) {
////    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
////        sURLVariables = sPageURL.split('&'),
////        sParameterName,
////        i;

////    for (i = 0; i < sURLVariables.length; i++) {
////        sParameterName = sURLVariables[i].split('=');

////        if (sParameterName[0] === sParam) {
////            return sParameterName[1] === undefined ? true : sParameterName[1];
////        }
////    }
////};