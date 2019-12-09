
function init(data) {


    data = parse(data);
    if ('Error' in data) {
        var errror = data.Errror;
        return;
    }


    $('#iframeMapa').attr('src', ResolveUrl(data.UrlMapa.Return));
    $('#iframeMapa').on('load', function () {
        $('.cargando').stop(true, true).fadeOut(300);
    });
}


