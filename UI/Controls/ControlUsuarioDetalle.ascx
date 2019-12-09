<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlUsuarioDetalle.ascx.cs" Inherits="UI.Controls.ControlUsuarioDetalle" %>

<div class="row">
    <div class="col s12">
        <div class="flex direction-row">
            <div class="flex-main flex direction-vertical">
                <label id="ControlUsuarioDetalle_textoNombre" class="detalle"></label>
                <label id="ControlUsuarioDetalle_textoNumeroDocumento" class="detalle"></label>
                <a class="btn waves-effect" id="ControlUsuarioDetalle_btnVerMas" href="#" style="margin-top:8px; width:160px">
                    <i class="material-icons btn-icono">description</i>
                    Ver detalle
                </a>
            </div>
        </div>
    </div>
</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlUsuarioDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
