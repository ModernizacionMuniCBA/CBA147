var quill;

function init(data) {
    //data = JSON.parse(data);

    if ('Error' in data) return;

    quill = new Quill('#editor', {
        theme: 'snow',
        "modules": {
            "toolbar": false
        }
    });

    quill.disable();
    $(".ql-editor").html(data.NotificacionParaUsuario.Contenido);
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