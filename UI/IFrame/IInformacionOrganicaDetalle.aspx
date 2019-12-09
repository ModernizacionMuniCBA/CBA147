<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IInformacionOrganicaDetalle.aspx.cs" Inherits="UI.IFrame.IInformacionOrganicaDetalle" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IInformacionOrganicaDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div style="display: block" class="scroll">

        <div id="contenedor_Encabezado">
            <div class="datos">
                <label class="titulo">Área</label>
                <label id="texto_Area"></label>
            </div>

            <div id="contenedor_Secretaria">
                <div>
                    <div class="seccion">
                        <label class="titulo">Secretaria</label>
                        <label id="texto_Secretaria" class="link"></label>
                    </div>
                </div>
                <div>
                    <div class="seccion">
                        <label class="titulo">Dirección</label>
                        <label id="texto_Direccion" class="link"></label>
                    </div>
                </div>
            </div>

            <div id="contenedor_Acciones">
                <label id="btn_DarDeBaja" class="link">Quitar información orgánica del area</label>
            </div>
        </div>

        <div id="contenedor_Detalle">



            <div class="seccion icono">
                <i class="material-icons">person</i>
                <div class="textos">
                    <label class="titulo">Responsable</label>
                    <label id="texto_Responsable"></label>
                </div>

            </div>

            <div class="seccion icono">
                <i class="material-icons">map</i>
                <div class="textos">
                    <label class="titulo">Domicilio</label>
                    <label id="texto_Domicilio"></label>
                </div>
            </div>

            <div class="seccion icono">
                <i class="material-icons">email</i>
                <div class="textos">
                    <label class="titulo">E-Mail</label>
                    <label id="texto_Email"></label>
                </div>
            </div>

            <div class="seccion icono">
                <i class="material-icons">phone</i>
                <div class="textos">
                    <label class="titulo">Teléfono</label>
                    <label id="texto_Telefono"></label>
                </div>
            </div>
        </div>
    </div>

    <div id="contenedor_Error" class="panel-mensaje">
        <i class="material-icons">error_outline</i>
        <label></label>
        <a class="btn waves-effect colorExito">Agregar información orgánica</a>
    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IInformacionOrganicaDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
