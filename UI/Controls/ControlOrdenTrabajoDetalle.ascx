<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlOrdenTrabajoDetalle.ascx.cs" Inherits="UI.Controls.ControlOrdenTrabajoDetalle" %>

<div class="row">
    <div class="col s12">
        <div class="flex direction-row">
            <div class="flex-main flex direction-vertical">
                <label id="ControlOrdenTrabajoDetalle_textoNumero" class="detalle"></label>
                <label id="ControlOrdenTrabajoDetalle_textoFechaAlta" class="detalle"></label>
                <div id="ControlOrdenTrabajoDetalle_estado" class="contenedor-estado">
                    <label id="indicadorEstado" class="indicador-estado"></label>
                    <label id="textoEstado" class="detalle"></label>
                </div>
                <a class="btn waves-effect" id="ControlOrdenTrabajoDetalle_btnVerMas" href="#" style="margin-top:8px; width:160px">
                    <i class="material-icons btn-icono">description</i>
                    Ver detalle
                </a>
            </div>
        </div>
    </div>
</div>

<!-- Mi JS -->
<script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlOrdenTrabajoDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
