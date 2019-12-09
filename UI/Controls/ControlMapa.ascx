<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlMapa.ascx.cs" Inherits="UI.Controls.ControlMapa" %>
<link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/Controls/Styles/ControlMapa.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />

<div id="ControlMapa_Contenedor">
    <div id="ControlMapa_Loader">
        <div class="indeterminate"></div>
    </div>

    <div id="ControlMapa_Mapa"></div>

    <div id="ControlMapa_ContenedorBusqueda">
        <div>

            <div id="ControlMapa_ContenedorBuscar">
                <input id="ControlMapa_InputBuscar" type="text" placeholder="Calle y numero | Interseccion | Barrio | Lugar de interes" />
                <a id="ControlMapa_BotonBuscar" class="tooltipped" data-position="bottom" data-tooltip="Buscar"><i class="material-icons">search</i></a>
                <div id="ControlMapa_LoaderBuscar">
                    <div class="indeterminate"></div>
                </div>
            </div>


            <div id="ControlMapa_ContenedorSugerencias">
            </div>
        </div>
    </div>

    <div id="ControlMapa_Botones">
        <a id="ControlMapa_BtnCpcs" class="btn-mapa">CPC</a>     
    </div>

    <div id="ControlMapa_BotonesOpciones">
        <div id="ControlMapa_BtnFullscreen" class="tooltipped" style="display: none;" data-position="left" data-tooltip="Expandir">
            <div class="btn-mapa">
                <a>
                    <i class="material-icons">fullscreen</i>
                </a>
            </div>
        </div>
        <div id="ControlMapa_BtnLocalizacion" class="tooltipped" style="display: none;" data-position="left" data-tooltip="Localizar mi ubicación">
            <div class="btn-mapa">
                <a>
                    <i class="material-icons">my_location</i>

                    <div class="cargando">
                        <div class="preloader-wrapper big active">
                            <div class="spinner-layer spinner-green-only">
                                <div class="circle-clipper left">
                                    <div class="circle"></div>
                                </div>
                                <div class="gap-patch">
                                    <div class="circle"></div>
                                </div>
                                <div class="circle-clipper right">
                                    <div class="circle"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </a>
            </div>
        </div>
    </div>
</div>

<div id="ControlMapa_TemplateSugerencia" style="display: none">
    <div class="sugerencia clickable">
        <i class="material-icons"></i>
        <div class="textos">
            <label class="texto1"></label>
            <label class="texto2"></label>
        </div>
    </div>
</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlMapa.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
