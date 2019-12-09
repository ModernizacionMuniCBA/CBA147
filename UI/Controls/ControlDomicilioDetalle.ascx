<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlDomicilioDetalle.ascx.cs" Inherits="UI.Controls.ControlDomicilioDetalle" %>
<style type="text/css">
    .botones {
        display: flex;
        margin-top: 0.5rem;
    }

    .botones a:not(:last-child){
        margin-right:0.5rem;
    }
</style>

<div class="row">
    <div class="col s12" style="padding: 0rem;">
        <div class="flex direction-row">
            <div class="flex-main flex direction-vertical">
                <label id="ControlDomicilioDetalle_textoTitulo" class="detalle"></label>
                <label id="ControlDomicilioDetalle_textoBarrio" class="detalle"></label>
                <label id="ControlDomicilioDetalle_textoCPC" class="detalle"></label>

                <div class="botones">
                    <a class="btn waves-effect" id="ControlDomicilioDetalle_btnVerMas">
                        <i class="material-icons btn-icono">description</i>
                        Ver detalle
                    </a>
                    <a class="btn btn-cuadrado waves-effect tooltipped verde" data-position="bottom" data-delay="50" data-tooltip="Ver mapa" id="ControlDomicilioDetalle_btnMapa" href="#">
                        <i class="material-icons btn-icono">room</i>
                    </a>
                </div>

            </div>
        </div>
    </div>
</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlDomicilioDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
