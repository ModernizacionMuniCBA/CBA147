var data;
var gallery;

function descargar() {
    var info;
    var nombre;

    if (data instanceof Array) {
        var index = gallery.getCurrentIndex();
        info = data[index].src;
        nombre = data[index].Nombre;
    } else {
        info = data.Data;
        nombre = data.Nombre;
    }

    var archivo = {
        Nombre: nombre,
        Data: info
    }

    descargarArchivo(archivo);
}

//--------------------------
// Imagenes
//--------------------------

function openGallery(index) {
    var pswpElement = document.querySelectorAll('.pswp')[0];

    if (index == undefined) {
        index = 0;
    }
    // define options (if needed)
    var options = {
        // optionName: 'option value'
        // for example:
        index: index,
        closeElClasses: [],
        closeOnVerticalDrag: false,
        escKey: true,
        pinchToClose: false,
        closeOnScroll: false,
        shareEl: false,
        closeEl: false,
        tapToClose: false,
        tapToToggleControls: false,
        clickToCloseNonZoomable: false
    };

    var ui = PhotoSwipeUI_Default;
    ui.shareEl = false;

    // Initializes and opens PhotoSwipe
    gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, data, options);
    gallery.init();
}

function setImagen(archivo) {
    archivo = parse(archivo);

    $('#indicadorError').hide();
    $('#contenedorDocumento').hide();
    $('#contenedorImagenes').show();

    data = [];

    var imagenes = [];
    imagenes.push(archivo);

    calcularTamañoImagenes(imagenes, function () {
        $.each(imagenes, function (index, img) {
            data.push({
                src: img.Data,
                w: img.W,
                h: img.H,
                Nombre: img.Nombre
            });
        });
        openGallery(0);
    });
}

function setImagenes(archivo, index) {
    archivo = parse(archivo);

    $('#indicadorError').hide();
    $('#contenedorDocumento').hide();
    $('#contenedorImagenes').show();

    data = [];

    var imagenes = archivo;

    calcularTamañoImagenes(imagenes, function () {
        $.each(imagenes, function (index, img) {
            data.push({
                src: img.Data,
                w: img.W,
                h: img.H,
                Nombre: img.Nombre
            });
        });

        openGallery(index);
    });
}

function setUrlImagenes(archivos, index) {
    $('#indicadorError').hide();
    $('#contenedorDocumento').hide();
    $('#contenedorImagenes').show();

    data = [];


    calcularTamañoImagenes(archivos, function () {
        $.each(imagenes, function (index, img) {
            data.push({
                src: img.Data,
                w: img.W,
                h: img.H,
                Nombre: img.Nombre
            });
        });

        openGallery(index);
    });
}

function calcularTamañoImagenes(imagenes, callback) {
    var imagen = imagenes[0];

    var img = new Image();
    img.onload = function () {
        imagen.W = this.width;
        imagen.H = this.height;

        if (imagenes.length != 1) {
            var listaNueva = [];
            $.each(imagenes, function (index, img) {
                if (index != 0) {
                    listaNueva.push(img);
                }
            });
            imagenes = listaNueva;
            calcularTamañoImagenes(imagenes, callback);
        } else {
            callback();
        }
    }
    img.src = imagen.Data;
}

//-------------------------
// PDF
//-------------------------

function setPDF(archivo) {
    archivo = parse(archivo);
    data = archivo;

    $('#indicadorError').hide();
    $('#contenedorDocumento').empty();
    $('#contenedorDocumento').show();
    $('#contenedorImagenes').hide();

    var obj = $('<object data="' + archivo.Data + '" type="application/pdf" width="100%" height="100%">');
    $('#contenedorDocumento').append(obj);
}

//-------------------------
// Documento General
//-------------------------

function setDocumento(archivo) {
    archivo = parse(archivo);
    data = archivo;

    $('#indicadorError').show();
    $('#contenedorDocumento').hide();
    $('#contenedorImagenes').hide();
}

function informarCerrar() {
    if (listenerCerrar == undefined) return;
    listenerCerrar();
}

function setOnCerrarListener(listener) {
    listenerCerrar = listener;
}