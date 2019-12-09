var grupos;
var camposDinamicos = [];

//campos dinamicos
var tipoCampo_TextoCorto = 1;
var tipoCampo_TextoLargo = 2;
var tipoCampo_Numero = 3;
var tipoCampo_Fecha = 4;
var tipoCampo_SiNo = 5;
var tipoCampo_Leyenda = 6;
var tipoCampo_Selector = 7;

//------------------------
//Creación de HTML
//------------------------
function cargarCamposDinamicos(campos) {
    return new Promise(function(callback) {
        $.each(campos, function(i, c) {
            if (c.Grupo == null) {
                c.Grupo = "Otra información";
            }
        });

        grupos = _.groupBy(campos, "Grupo");
        var ordenGrupos = [];

        for (var keyGrupo in grupos) {
            if (grupos.hasOwnProperty(keyGrupo)) {
                var grupo = grupos[keyGrupo];
                console.log("grupo no ordenado");
                console.log(grupo);

                console.log("orden grupos");
                var primerOrden = grupo[0].Orden;
                ordenGrupos.push({ Grupo: keyGrupo, Orden: primerOrden });
                console.log(ordenGrupos);
            }
        }

        ordenGrupos = _.sortBy(ordenGrupos, "Orden");

        var html = "";
        $.each(ordenGrupos, function(index) {
            var g = grupos[ordenGrupos[index].Grupo];
            g = _.sortBy(g, "Orden");
            html += crearGrupo(g, ordenGrupos[index].Grupo);
            if (ordenGrupos.length > index + 1)
                html += '    <div class="form-separador">    </div>';
        });

        callback(html);
    });
}

function crearGrupo(grupo, nombreGrupo) {
    var div = $(getHtmlGrupo());
    $(div)
      .find(".titulo")
      .text(nombreGrupo);

    var contenido = $(div).find(".contenido");
    var descripciones = "";
    for (var i in grupo) {
        var divCampo = crearCampoDinamico(grupo[i]);
        $(contenido).append(divCampo);
        initCampoDinamico(divCampo, grupo[i]);
    }

    var htmlGrupo = "";
    $.each($(div), function(i, html) {
        if (html.outerHTML != undefined) htmlGrupo += html.outerHTML;
    });

    return htmlGrupo;
}

function crearCampoDinamico(campo) {
    let div;
    switch (campo.KeyValueTipoCampo) {
        case tipoCampo_TextoCorto:
            div = $(getHtmlTextoCorto());
            break;
        case tipoCampo_TextoLargo:
            div = $(getHtmlTextoLargo());
            break;
        case tipoCampo_Numero:
            div = $(getHtmlNumero());
            break;
        case tipoCampo_Fecha:
            div = $(getHtmlFecha(campo.Id));
            initCampoDinamicoFecha(div, campo.Id);
            break;
        case tipoCampo_SiNo:
            div = $(getHtmlSiNo());
            break;
        case tipoCampo_Leyenda:
            div = $(getHtmlLeyenda());
            break;
        case tipoCampo_Selector:
            div = $(getHtmlSelector());
            break;
    }

    $(div)
      .find(".campo")
      .attr("id", campo.Id);
    $(div)
      .find(".texto_Nombre")
      .text(campo.Nombre);

    if (campo.Observaciones === "") {
        $(div)
          .find(".ayuda")
          .hide();
    } else {
        $(div)
          .find(".texto_observaciones")
          .html(campo.Observaciones);
    }

    if (campo.KeyValueTipoCampo == tipoCampo_Leyenda) {
        return div;
    }

    $(div)
      .find(".texto_Nombre")
      .attr("for", campo.Id);
    return div;
}

function initCampoDinamico(div, campo) {
    setTimeout(function() {
        switch (campo.KeyValueTipoCampo) {
            case tipoCampo_Fecha:
                initCampoDinamicoFecha(div, campo);
                break;
            case tipoCampo_Selector:
                initCampoDinamicoSelector(div, campo);
                break;
        }
    }, 500);
}

function initCampoDinamicoFecha(div, campo) {
    let idCampo = campo.Id;
    //Evento datepicker fecha de TUV
    $("#campoFecha" + idCampo)
      .find(".botonFecha")
      .click(function(e) {
          if ($(this).prop("disabled") == true) {
              return;
          }
          $("#campoFecha" + idCampo)
            .find("input.datepicker")
            .click();
          e.stopPropagation();
      });

    //inicializar datepickeres
    $("#campoFecha" + idCampo)
      .find("input.datepicker")
      .pickadate({
          // Date limits
          min: new Date(1900, 1, 1),
          container: top.$(top.document).find("body"),
          selectMonths: true,
          selectYears: 60,
          onSet: function(value) {
              if ("select" in value) {
                  var s = this.get("select", "dd/mm/yyyy");
                  $("#campoFecha" + idCampo)
                    .find("input.date")
                    .val(s);
                  this.close();
              }

              if ("clear" in value) {
                  $("#campoFecha" + idCampo)
                    .find("input.date")
                    .val("");
              }

              console.log(value);

              Materialize.updateTextFields();
          }
      });
}

function initCampoDinamicoSelector(div, campo) {
    let opciones = _.map(campo.Opciones, function(valor, i) {
        return { Id: i, Nombre: valor };
    });
    $("#" + campo.Id).CargarSelect({
        Data: opciones,
        Value: "Id",
        Text: "Nombre",
        Default: "Seleccione...",
        Sort: true
    });
}

function init_Campos() {
    Materialize.updateTextFields();

    $(".date").mask("00/00/0000");

    $(".tooltipAyuda").each(function() {
        // Notice the .each() loop, discussed below
        $(this).qtip({
            content: {
                text: $(this).next("div") // Use the "div" element next to this for the content
            },
            position: {
                my: "bottom center",
                at: "top center"
            },
            style: {
                classes: "qtip-shadow qtip-rounded qtip-tipsy"
            }
        });
    });
}

//HTML
function getHtmlGrupo() {
    return `
    <div class="row">
            <div class="col s12 m2 no-padding no-margin">
                <div class="row flex">
                    <div class="col">
                    <label class="titulo no-select" style="flex: 1;"></label>
                 </div></div>
            </div>

            <div class="col s12 m10">
                <div class="row contenido">
                </div>
            </div>
        </div>
   `;
}

function getHtmlTextoCorto() {
    return `
    <div class="col s6" style="position: relative">
      <div class="contenedor_Campo input-field fix-margin">
          <input type="text" maxlength="50" length="50" class="campo contador" />
          <label class="texto_Nombre"></label>
          <a class="control-observacion colorTextoError no-select"></a>
      </div>

           <div class="ayuda" style="
    margin-top: 12px;
    position: absolute;
    right: 0;
    top: 0;
    ">
                  <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                  <div class="texto_observaciones" style="display: none">
                  </div>
              </div>
</div>
</div>
    `;
}

function getHtmlTextoLargo() {
    return ` 
    <div class="col s12" style="position: relative">
        <div class="contenedor_Campo input-field fix-margin">
            <textarea class="campo contador materialize-textarea" maxlength="500" length="500"></textarea>
            <label class="texto_Nombre"></label>
            <a class="control-observacion colorTextoError no-select"></a>
        </div>

                <div class="ayuda" style="
    margin-top: 12px;
    position: absolute;
    right: 0;
    top: 0;
    ">
                <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                <div class="texto_observaciones" style="display: none">
                </div>
            </div>
</div>
    `;
}

function getHtmlNumero() {
    return ` <div class="col s6" style="position: relative">
            <div class="contenedor_Campo input-field fix-margin">
                <input type="number" class="campo" />
                <label class="texto_Nombre"></label>
                <a class="control-observacion colorTextoError no-select"></a>
            </div>

                    <div class="ayuda" style="
    margin-top: 12px;
    position: absolute;
    right: 0;
    top: 0;
    ">
              <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
              <div class="texto_observaciones" style="display: none">
              </div>
          </div>
</div>
    `;
}

function getHtmlFecha(idCampo) {
    let idC = "campoFecha" + idCampo + "";
    return (
      `
      <div id=` +
      idC +
      ` class="col s6" style="position: relative">
          <div class="contenedor_Campo input-field fix-margin">
              <input type="text" class="date campo" name="date" maxlength="10" autocomplete="off" />
              <label class="texto_Nombre no-select"></label>
              <input type="date" class="datepicker" style="display: none;" />
              <a  class="botonFecha btn-flat waves-effect boton-input" style="    margin-right: 16px;">
                  <i class="material-icons">today</i>
              </a>
              <a class="control-observacion colorTextoError no-select"></a>
          </div>    
              <div class="ayuda" style="
    margin-top: 12px;
    position: absolute;
    right: 0;
    top: 0;
    ">
                <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                <div class="texto_observaciones" style="display: none">
                </div>
            </div>
</div>
</div>
  `
  );
}

function getHtmlSiNo() {
    return ` 
    <div class="col s6 mi-input-field " style="margin-top: 16px; position: relative">
          <div class="contenedor_Campo ">
              <input class="campo" type="checkbox" />
              <label  class="texto_Nombre"></label>
          </div>

          <div class="ayuda" style="
    position: absolute;
    right: 0;
    top: -6px;
    ">
              <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
              <div class="texto_observaciones" style="display: none">
              </div>
          </div>
</div>
        `;
}

function getHtmlLeyenda() {
    return ` 
    <div class="col s12" style="position: relative; margin-top: 1rem !important;margin-bottom: 1rem !important;">
        <div class="contenedor_Campo flex" style="
    flex-direction: column;
    ">
            <label class="texto_Nombre campo motivo-titulo"></label>
            <label  class="texto_observaciones"></label>
            <a class="control-observacion colorTextoError no-select"></a>
        </div>          
    </div>
    `;
}

//<div class="contenedor_Campo flex mi-input-field" style="
//flex-direction: column;
//">
//                                   <label class="no-select "></label>

//MEDIOOOOO

//<a class="control-observacion colorTextoError no-select"></a>
//    </div>

function getHtmlSelector() {
    return ` 
    <div class="col s6" style="position: relative">
    <div class="mi-input-field">
            <label class="texto_Nombre no-select"></label>
            <select class="campo" style="width: 100%"></select>
                               <a class="control-observacion colorTextoError no-select"></a>
            </div>   
             <div class="ayuda" style="
    margin-top: 12px;
    margin-right: 24px!important;
    position: absolute;
    right: 0;
    top: 0;
    ">
                  <a class="btn-flat btn-redondo tooltipAyuda waves-effect"><i class="material-icons colorAyuda">help</i></a>
                  <div class="texto_observaciones" style="display: none">
                   
                  </div>
              </div>
            
    </div>


    `;
}

//GET Y SET
function setCampoPorRequerimiento(camposPorRequerimiento) {
    $.each(camposPorRequerimiento, function(i, c) {
        if (c.IdTipoCampoPorMotivo == tipoCampo_SiNo) {
            if (c.Valor == "true") {
                $("#" + c.IdCampoPorMotivo).attr("checked", true);
                return;
            }

            $("#" + c.IdCampoPorMotivo).attr("checked", false);
            return;
        }

        $("#" + c.IdCampoPorMotivo).val(c.Valor);
    });

    Materialize.updateTextFields();
}

function getCamposDinamicosPorRequerimiento(camposPorRequerimiento) {
    var campos = [];
    $.each(camposPorRequerimiento, function(i, c) {
        let valor;

        switch (c.IdTipoCampoPorMotivo) {
            case tipoCampo_SiNo:
                valor = $("#" + c.Id).is(":checked") + "";
                break;
            case tipoCampo_Selector:
                valor = $("#" + c.Id + " option:selected").text();
                break;
            default:
                valor = $("#" + c.Id).val();
                break;
        }

        campos.push({ Id: c.Id, Valor: valor });
    });

    return campos;
}

//Validaciones
function validarCamposDinamicos(campos) {
    var resultado = true;
    $.each(campos, function(i, campo) {
        if (campo.Obligatorio) {
            var valorCampo = $("#" + campo.Id).val();

            if(campo.IdTipoCampoPorMotivo==tipoCampo_Selector && valorCampo=="-1"){
                $("#" + campo.Id)
           .siblings(".control-observacion")
           .text("Campo obligatorio");
                resultado = false;
            }
            if (campo.IdTipoCampoPorMotivo!=tipoCampo_Selector && valorCampo == "") {
                $("#" + campo.Id)
                  .siblings(".control-observacion")
                  .text("Campo obligatorio");
                resultado = false;
            }
        }
    });

    return resultado;
}
