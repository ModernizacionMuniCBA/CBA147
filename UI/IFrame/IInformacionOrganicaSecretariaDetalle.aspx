<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IInformacionOrganicaSecretariaDetalle.aspx.cs" Inherits="UI.IFrame.IInformacionOrganicaSecretariaDetalle" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IInformacionOrganicaSecretariaDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div style="display: block" class="scroll">

        <div id="contenedor_Encabezado">
            <div class="datos">
                <label id="texto_Nombre"></label>
                <a id="btn_EditarNombre" class="btn-flat btn-redondo chico waves-effect btnEdit"><i class="material-icons">edit</i></a>
            </div>

            <div class="margin-top">
                <label id="texto_Estado" class="texto-entidad-dada-de-baja">Entidad dada de baja</label>
                <div id="contenedor_Acciones">
                    <label id="btn_DarDeBaja" class="accion link">Dar de baja</label>
                    <label id="btn_DarDeAlta" class="accion link">Dar de alta</label>
                </div>
            </div>
        </div>

        <div id="contenedor_Detalle">

            <div class="seccion">
                <div class="encabezado">
                    <label class="titulo">Direcciones</label>
                    <label id="btn_AgregarDireccion" class="link">Agregar</label>
                </div>
                <div id="contenedor_Direcciones"></div>
            </div>

        </div>

    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IInformacionOrganicaSecretariaDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
