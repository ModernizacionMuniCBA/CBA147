<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_Pagina.Master" AutoEventWireup="true" CodeBehind="Ajustes.aspx.cs" Inherits="Internet_UI.Paginas.Ajustes" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/Ajustes.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/Ajustes.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderBoton" runat="server">
    <a class="btn btn-quadrate btn-transparent btn-volver waves-effect"><i class="material-icons">arrow_back</i></a>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <label id="texto_Titulo">Ajustes</label>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contenedor-detalle">
        <label>Acerca de</label>
        <div class="card card-detalle">
            <div class="content-body">
                <div class="cabecera">
                    <img class="_logo" />
                    <label class="_titulo"></label>
                    <label class="_secretaria"></label>
                    <label class="_direccion"></label>
                </div>
            </div>

            <div class="content-body not-padding">
                <div class="item-detalle clickable _domicilio">
                    <i class="mdi mdi-map"></i>
                    <div class="textos">
                        <label class="titulo">Domicilio</label>
                        <a class="detalle"></a>
                    </div>
                </div>

                <div class="item-detalle clickable _telefono">
                    <i class="mdi mdi-phone"></i>
                    <div class="textos">
                        <label class="titulo">Teléfono</label>
                        <a class="detalle"></a>
                    </div>
                </div>

                <div class="item-detalle clickable _email">
                    <i class="mdi mdi-email"></i>
                    <div class="textos">
                        <label class="titulo">E-Mail</label>
                        <a class="detalle"></a>
                    </div>
                </div>
            </div>
        </div>

        <div class="info abajo">
            <label class="_version"></label>
            <label class="_versionFecha"></label>
        </div>
    </div>

    <div class="contenedor-botones">
        <div>
            <a class="btn-volver btn btn-transparent waves-effect">Volver</a>
        </div>
    </div>
</asp:Content>
