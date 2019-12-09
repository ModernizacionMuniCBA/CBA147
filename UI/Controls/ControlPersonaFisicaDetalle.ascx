<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlPersonaFisicaDetalle.ascx.cs" Inherits="UI.Controls.ControlPersonaFisicaDetalle" %>

<div class="row">
    <div class="col s12">
        <div class="flex direction-row">
            <div class="flex-main flex direction-vertical">
                <label id="ControlPersonaFisicaDetalle_textoNombre" class="detalle"></label>
                <label id="ControlPersonaFisicaDetalle_textoNumeroDocumento" class="detalle"></label>
                <a class="btn waves-effect" id="ControlPersonaFisicaDetalle_btnVerMas" href="#" style="margin-top:8px; width:160px">
                    <i class="material-icons btn-icono">description</i>
                    Ver detalle
                </a>
            </div>
        </div>
    </div>
</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlPersonaFisicaDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
