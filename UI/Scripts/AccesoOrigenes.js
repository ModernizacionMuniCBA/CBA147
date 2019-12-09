
function init(data) {
    console.log(data);

    $('#fondo')
       .css('background-image', 'url(' + ResolveUrl('~/Resources/Imagenes/fondo_login.jpg') + ')')
       .waitForImages(function () {
           $('#fondo').addClass('visible');
           setTimeout(function () {
               $('#card').addClass('visible');
           }, 200);
       }, $.noop, true);

    $.each(data.Origenes, function (index, element) {
        let html = $($('#template_Origen').html());
        $(html).find('.nombre').text(element.Nombre);
        $('#contenedor_Origenes').append(html);

        $(html).click(function () {

            $('#card').addClass('cargando');
            crearAjax({
                Url: ResolveUrl('~/Servicios/UsuarioService.asmx/SetOrigen'),
                Data: { id: element.Id },
                OnSuccess: function () {
                    $('#card').removeClass('cargando');
                    $('#card').removeClass('visible');
                    $('#fondo').removeClass('visible');
                    setTimeout(function () {
                        location.reload();
                    }, 500);
                },
                OnError: function () {
                    $('#card').removeClass('cargando');
                    console.log('Error');
                }
            })
        });
    });
}


//Utils

function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}

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
