<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="IUsuarioDetalle.aspx.cs" Inherits="UI.IFrame.IUsuarioDetalle" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/IUsuarioDetalle.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">


    <div style="display: block" class="scroll">

        <div id="contenedor_Encabezado">
            <div class="contenedor_Datos">
                <div id="foto"></div>
                <div class="datos">
                    <label id="texto_Nombre"></label>
                </div>
            </div>

            <div id="contenedor_Acciones" class="contenedor_Acciones">
                <div id="btn_Editar" class="accion"><i class="material-icons">edit</i><label>Editar perfil</label></div>
                <div id="btn_Password" class="accion"><i class="material-icons">vpn_key</i><label>Cambiar contraseña</label></div>
                <div id="btn_Username" class="accion"><i class="material-icons">person  </i><label>Cambiar nombre de usuario</label></div>
            </div>
        </div>


        <div id="contenedor_Detalle">

            <div id="contenedor_Usuario" class="seccion">
                <label class="titulo">Usuario</label>
                <label id="texto_Usuario"></label>
            </div>

            <div id="contenedor_Sexo" class="seccion">
                <label class="titulo">Sexo</label>
                <label id="texto_Sexo"></label>
            </div>

            <div id="contenedor_Dni" class="seccion">
                <label class="titulo">DNI</label>
                <label id="texto_Dni"></label>
            </div>


            <div id="contenedor_Cuil" class="seccion">
                <label class="titulo">N° de Cuil</label>
                <label id="texto_Cuil"></label>
            </div>

            <div id="contenedor_Email" class="seccion">
                <label class="titulo">E-Mail</label>
                <label id="texto_Email"></label>
            </div>

            <div id="contenedor_TelefonoFijo" class="seccion">
                <label class="titulo">N° de Telefono Fijo</label>
                <label id="texto_TelefonoFijo"></label>
            </div>

            <div id="contenedor_TelefonoCelular" class="seccion">
                <label class="titulo">N° de Telefono Celular</label>
                <label id="texto_TelefonoCelular"></label>
            </div>


            <div id="contenedor_Rol" class="seccion" style="display: none">
                <label class="titulo">Rol</label>
                <label id="texto_Rol"></label>
            </div>

            <div id="contenedor_AmbitoDeTrabajo" class="seccion">
                <label class="titulo">Ambito de Trabajo</label>
                <label id="texto_Ambito"></label>
            </div>

            <div id="contenedor_InfoAdicional" class="seccion" style="display: none">
                <label class="titulo">Información adicional</label>
                <label id="texto_InformacionAdicional"></label>
            </div>
        </div>

    </div>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/IUsuarioDetalle.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
