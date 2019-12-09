<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ControlSexoSelector.ascx.cs" Inherits="UI.Paginas.UserControls.ControlSexoSelector" %>

<div>
    <div class="row">
        <div class="col s12">
            <div class="input-field select" id="ControlSexoSelector_ContenedorSelect">
                <select id="ControlSexoSelector_select_Sexo" style="width: 100%"></select>
                <label for="ControlSexoSelector_select_Sexo">Sexo</label>
            </div>
            <div class="input-field" id="ControlSexoSelector_ContenedorInput" style="display: none">
                <div class="input-field">
                    <input id="ControlSexoSelector_input_SexoOtro" type="text" />
                    <label for="ControlSexoSelector_input_SexoOtro" class="no-select">Género</label>
                    <a id="ControlSexoSelector_BotonCancelar" class="btn-flat waves-effect btn-redondo chico boton-input"><i class="material-icons">clear</i></a>
                </div>
            </div>
        </div>
    </div>

        <script type="text/javascript" src="<%=ResolveUrl("~/Controls/Scripts/ControlSexoSelector.js?v=" + (ConfigurationManager.AppSettings["VERSION"] as string)) %>"></script>

</div>