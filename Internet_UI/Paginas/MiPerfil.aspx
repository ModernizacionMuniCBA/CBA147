<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_Pagina.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="Internet_UI.Paginas.MiPerfil" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/MiPerfil.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/MiPerfil.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderBoton" runat="server">
    <a class="btn btn-quadrate btn-transparent btn-volver waves-effect"><i class="material-icons">arrow_back</i></a>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <label id="texto_Titulo">Mi perfil</label>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Foto --%>
    <div id="contenedor_FotoPersonal" class="card-detalle">
        <input type="file" accept="image/*" style="display: none" />
        <div id="fotoPersonal"></div>
    </div>


    <%-- Datos personales --%>
    <div id="contenedor_DatosPersonales" class="contenedor-detalle">
        <label>Datos personales</label>
        <div class="card card-detalle">
            <div class="content-body">
                <div class="item-detalle nombre">
                    <i class="mdi mdi-account-card-details"></i>
                    <div class="textos">
                        <label class="titulo">Nombre</label>
                        <label class="detalle"></label>
                    </div>
                </div>

                <div class="item-detalle dni">
                    <i class="mdi mdi-account-card-details"></i>
                    <div class="textos">
                        <label class="titulo">N° de documento</label>
                        <label class="detalle"></label>
                    </div>
                </div>

                <div class="item-detalle cuil">
                    <i class="mdi mdi-account-card-details"></i>
                    <div class="textos">
                        <label class="titulo">CUIL</label>
                        <label class="detalle"></label>
                    </div>
                </div>

                <div class="item-detalle fechaNacimiento">
                    <i class="mdi mdi-calendar"></i>
                    <div class="textos">
                        <label class="titulo">Fecha de nacimiento</label>
                        <label class="detalle"></label>
                    </div>
                </div>

                <div class="item-detalle sexo">
                    <i class="mdi mdi-gender-male-female"></i>
                    <div class="textos">
                        <label class="titulo">Sexo</label>
                        <label class="detalle"></label>
                    </div>
                </div>

                <div class="item-detalle domicilioLegal">
                    <i class="mdi mdi-map"></i>
                    <div class="textos">
                        <label class="titulo">Domicilio legal</label>
                        <label class="detalle"></label>
                    </div>
                </div>
            </div>
        </div>

        <div class="info abajo">
            <i class="material-icons">info_outline</i>
            <label>Como sus datos se encuentran validados por el Registro Nacional de Personas, estos no se pueden editar</label>
        </div>
    </div>


    <%-- Datos acceso --%>
    <div id="contenedor_DatosAcceso" class="contenedor-detalle">
        <label>Datos de acceso</label>
        <div class="card card-detalle">
            <div class="content-body">
                <div class="item-detalle username">
                    <i class="mdi mdi-account"></i>
                    <div class="textos">
                        <label class="titulo">Nombre de usuario</label>
                        <label class="detalle"></label>
                    </div>
                    <a class="link waves-effect">
                        <i class="mdi mdi-pencil"></i>
                        <label>Modificar</label>
                    </a>
                </div>

                <div class="item-detalle password">
                    <i class="mdi mdi-key"></i>
                    <div class="textos">
                        <label class="titulo">Contraseña</label>
                        <label class="detalle">••••••••••</label>
                    </div>
                    <a class="link waves-effect">
                        <i class="mdi mdi-pencil"></i>
                        <label>Modificar</label>
                    </a>
                </div>
            </div>
        </div>
    </div>


    <%-- Datos contacto --%>
    <div id="contenedor_DatosContacto" class="contenedor-detalle">
        <label>Datos de contacto</label>
        <form id="formContacto" autocomplete="off">
            <div class="card card-detalle">
                <div class="content-body">
                    <div id="error_DatosContacto" class="mensaje-error">
                        <i class="material-icons">error</i>
                        <label></label>
                    </div>

                    <div class="item-detalle">
                        <i class="mdi mdi-email"></i>
                        <div class="textos">
                            <label class="titulo" for="input_Email">E-Mail</label>
                            <input id="input_Email" type="email" required="required" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="item-detalle col s12 m6">
                            <i class="mdi mdi-phone"></i>
                            <div class="textos">
                                <label class="col s12 titulo no-padding">Celular</label>
                                <div>
                                    <label class="col s1 text-input">0</label>
                                    <input class="col s3" type="number" id="celA" placeholder="Prefijo" />
                                    <label class="col s2 text-input">15</label>
                                    <input class="col s6" type="number" id="celN" placeholder="Número" />
                                </div>
                            </div>
                        </div>

                        <div class="item-detalle col s12 m6">
                            <i class="mdi mdi-phone"></i>
                            <div class="textos">
                                <label class="col s12 titulo no-padding">Fijo</label>
                                <div>
                                    <label class="col s1 text-input">0</label>
                                    <input class="col s3" type="number" id="telA" placeholder="Área" />
                                    <label class="col s2 text-input"></label>
                                    <input class="col s6" type="number" id="telN" placeholder="Número" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="content-footer">
                    <a class="btn btn-guardar waves-effect">Modificar</a>
                </div>
            </div>
        </form>
    </div>


    <%-- Acciones --%>
    <div class="contenedor-botones">
        <div>
            <a class="btn btn-transparent btn-volver waves-effect">Volver</a>
        </div>
    </div>
</asp:Content>
