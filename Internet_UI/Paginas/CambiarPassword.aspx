<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_Pagina.Master" AutoEventWireup="true" CodeBehind="CambiarPassword.aspx.cs" Inherits="Internet_UI.Paginas.CambiarPassword" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/CambiarPassword.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/CambiarPassword.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderBoton" runat="server">
    <a class="btn btn-quadrate btn-transparent btn-volver waves-effect"><i class="material-icons">arrow_back</i></a>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <label id="texto_Titulo">Cambiar contraseña</label>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contenedor-detalle">
        <form id="formCambiarPassword" autocomplete="off">
            <div class="card card-detalle">
                <div class="content-body">
                    <div id="error_CambiarPassword" class="mensaje-error">
                        <i class="material-icons">error</i>
                        <label></label>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_PasswordAnterior">Contraseña actual</label>
                            <input id="input_PasswordAnterior" type="password" required="required" maxlength="50" />
                        </div>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_PasswordNuevo">Contraseña nueva</label>
                            <input id="input_PasswordNuevo" type="password" required="required" maxlength="50" />
                        </div>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_PasswordNuevo2">Reingrese la contraseña nueva</label>
                            <input id="input_PasswordNuevo2" type="password" required="required" maxlength="50" />
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
