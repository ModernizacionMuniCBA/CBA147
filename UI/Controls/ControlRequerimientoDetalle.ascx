<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlRequerimientoDetalle.ascx.cs" Inherits="UI.Controls.ControlRequerimientoDetalle" %>

<style type="text/css">
    #ControlRequerimientoDetalle_textoNumero {
        font-weight: 600;
        font-size: 1.4rem;
    }
</style>

<div class="row">
    <div class="col s12">
        <div class="flex direction-row">
            <div class="flex-main flex direction-vertical">
                <label id="ControlRequerimientoDetalle_textoNumero" class="detalle"></label>
                <label id="ControlRequerimientoDetalle_textoFechaAlta" class="detalle"></label>
                <div id="ControlRequerimientoDetalle_estado" class="contenedor-estado">
                    <label id="indicadorEstado" class="indicador-estado"></label>
                    <label id="textoEstado" class="detalle"></label>
                </div>
                <a class="btn waves-effect" id="ControlRequerimientoDetalle_btnVerMas" href="#" style="margin-top: 8px; width: 160px">
                    <i class="material-icons btn-icono">description</i>
                    Ver detalle
                </a>
            </div>
        </div>
    </div>
</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlRequerimientoDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
