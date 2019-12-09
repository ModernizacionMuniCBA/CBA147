$(function () {
    window.addEventListener('message', function (e) {
        if (e == undefined || e.data == undefined) return;

        let data = e.data;
        setToken(data.Token);
    });
});

function setToken(token) {
    ajax_SetToken(token)
        .then(function (data) {
            $('iframe')[0].contentWindow.postMessage('login-completado', '*');
            setTimeout(function () {
                location.reload();
            }, 500);
        })
        .catch(function (error) {
            Materialize.toast(error);
            $('iframe')[0].contentWindow.postMessage('reintentar-login', '*');
        });
}

function ajax_SetToken(token) {
    return new Promise(function (callback, callbackError) {

        crearAjax({
            Url: ResolveUrl('~/Servicios/ServicioUsuario.asmx/SetToken'),
            Data: { token: token },
            OnSuccess: function (result) {
                if (!result.Ok) {
                    callbackError(result.Error);
                    return;
                }
                callback();
            },
            OnError: function (result) {
                callbackError('Error procesando la solicitud');
            }
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

    try {
        json = JSON.parse(json);
    } catch (e) {
        json = json.replace(/\\/g, "");
        json = JSON.parse(json);
    }
    return json;
}
