<%@ Page Title="" Language="C#" MasterPageFile="~/Paginas/_Pagina.Master" AutoEventWireup="true" CodeBehind="ValidarDatos.aspx.cs" Inherits="Internet_UI.Paginas.ValidarDatos" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="<%=ResolveUrl("~/Paginas/Styles/ValidarDatos.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" rel="stylesheet" />
    <script type="text/javascript" src="<%=ResolveUrl("~/Paginas/Scripts/ValidarDatos.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
    <label id="texto_Titulo">Validar datos de Usuario</label>
</asp:Content>


<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="contenedor-detalle">
        <div class="info arriba card alerta">
            <i class="material-icons">info_outline</i>
            <label>A partir de ahora para utilizar #CBA147 su información debe estar validada formalmente a través del Registro Nacional de las Personas</label>
        </div>

        <label>Datos personales</label>
        <form id="form" autocomplete="off">
            <div class="card card-detalle">
                <div class="content-body">
                    <div id="error_ActualizarPerfil" class="mensaje-error">
                        <i class="material-icons">error</i>
                        <label></label>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_Nombre">Nombre</label>
                            <input id="input_Nombre" type="text" required="required" maxlength="200" />
                        </div>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_Apellido">Apellido</label>
                            <input id="input_Apellido" type="text" required="required" maxlength="200" />
                        </div>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_Dni">N° de Documento</label>
                            <input id="input_Dni" type="number" required="required" minlength="7" maxlength="8" />
                        </div>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_FechaNacimiento">Fecha de nacimiento</label>
                            <input id="input_FechaNacimiento" class="datepicker" type="text" required="required" placeholder="DD/MM/YYYY" />
                        </div>
                    </div>

                    <div class="item-detalle">
                        <div class="textos">
                            <label class="titulo" for="input_Dni">Sexo</label>
                            <p>
                                <input name="sexo" type="radio" id="radio_Masculino" class="with-gap" />
                                <label for="radio_Masculino">Masculino</label>
                            </p>
                            <p>
                                <input name="sexo" type="radio" id="radio_Femenino" class="with-gap" />
                                <label for="radio_Femenino">Femenino</label>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </form>

        <div class="contenedor-botones">
            <div>
                <a class="btn-guardar btn waves-effect">Validar</a>
            </div>
        </div>
    </div>


    <%-- Templantes --%>
    <div id="template_DialogoBienvenida" style="display: none">
        <label>Sus datos han sido validados exitosamente.</label>
        <label>Su información personal ha sido organizada en tres secciones:</label>
        <ul>
            <li>Datos personales (No puede ser modificados)</li>
            <li>Datos de acceso</li>
            <li>Datos de contacto</li>
        </ul>
        <label>En la próxima pantalla usted podrá revisar y actualizar el resto de la información para tener un perfil más completo.</label>
        <label>Si algunos de estos datos necesitan ser modificados, lo podrá hacer desde la opción "Mi Perfil"</label>
    </div>
</asp:Content>
