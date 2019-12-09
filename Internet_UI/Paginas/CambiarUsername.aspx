<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_Pagina.Master" AutoEventWireup="true" CodeBehind="CambiarUsername.aspx.cs" Inherits="Internet_UI.Paginas.CambiarUsername" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/CambiarUsername.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/CambiarUsername.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderBoton" runat="server">
    <a class="btn btn-quadrate btn-transparent btn-volver waves-effect"><i class="material-icons">arrow_back</i></a>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <label id="texto_Titulo">Cambiar nombre de usuario</label>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contenedor-detalle">
        <div class="info arriba card alerta">
            <i class="material-icons">info_outline</i>
            <label>El nombre de usuario es un alias con el que usted puede acceder a su cuenta. Tenga en cuenta que todavía podrá acceder a través de su número de CUIL</label>
        </div>

        <form id="formCambiarUsername" autocomplete="off">
            <div class="card card-detalle">
                <div class="content-body">
                    <div id="error_CambiarUsername" class="mensaje-error">
                        <i class="material-icons">error</i>
                        <label></label>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_UsernameActual">Nombre de usuario actual</label>
                            <input id="input_UsernameActual" type="text" readonly="true" />
                        </div>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_Username">Nombre de usuario nuevo</label>
                            <input id="input_Username" type="text" required="required" maxlength="50" />
                        </div>
                    </div>
                </div>
            </div>
        </form>

        <div class="contenedor-botones">
            <div>
                <a class="btn-volver btn btn-transparent waves-effect">Volver</a>
                <a class="btn-guardar btn waves-effect">Modificar</a>
            </div>
        </div>
    </div>
</asp:Content>
