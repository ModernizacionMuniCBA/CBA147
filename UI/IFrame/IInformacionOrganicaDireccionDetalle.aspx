<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IInformacionOrganicaDireccionDetalle.aspx.cs" Inherits="UI.IFrame.IInformacionOrganicaDireccionDetalle" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IInformacionOrganicaDireccionDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <div style="display: block" class="scroll">

        <div id="contenedor_Encabezado">
            <div class="datos">
                <label class="titulo">Secretaría</label>
                <label id="texto_Secretaria" class="link"></label>
            </div>

            <div class="seccion ">
                <div class="encabezado">
                    <label class="titulo">Nombre</label>
                    <a id="btn_CambiarNombre" class="btn-flat btn-redondo chico waves-effect btnEdit"><i class="material-icons">edit</i></a>
                </div>
                <label id="texto_Nombre"></label>
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

            <div class="seccion icono">
                <i class="material-icons">person</i>
                <div class="textos">
                    <div class="encabezado">
                        <label class="titulo">Responsable</label>
                        <a id="btn_CambiarResponsable" class="btn-flat btn-redondo chico waves-effect btnEdit"><i class="material-icons">edit</i></a>
                    </div>
                    <label id="texto_Responsable"></label>
                </div>

            </div>

            <div class="seccion icono">
                <i class="material-icons">map</i>
                <div class="textos">
                    <div class="encabezado">
                        <label class="titulo">Domicilio</label>
                        <a id="btn_CambiarDomicilio" class="btn-flat btn-redondo chico waves-effect btnEdit"><i class="material-icons">edit</i></a>
                    </div>
                    <label id="texto_Domicilio"></label>
                </div>
            </div>

            <div class="seccion icono">
                <i class="material-icons">email</i>
                <div class="textos">
                    <div class="encabezado">
                        <label class="titulo">E-Mail</label>
                        <a id="btn_CambiarEmail" class="btn-flat btn-redondo chico waves-effect btnEdit"><i class="material-icons">edit</i></a>
                    </div>
                    <label id="texto_Email"></label>
                </div>
            </div>

            <div class="seccion icono">
                <i class="material-icons">phone</i>
                <div class="textos">
                    <div class="encabezado">
                        <label class="titulo">Teléfono</label>
                        <a id="btn_CambiarTelefono" class="btn-flat btn-redondo chico waves-effect btnEdit"><i class="material-icons">edit</i></a>
                    </div>
                    <label id="texto_Telefono"></label>
                </div>
            </div>


        </div>
    </div>
    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IInformacionOrganicaDireccionDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
