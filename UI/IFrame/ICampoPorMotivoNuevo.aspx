<%@ Page Title="" Language="C#" MasterPageFile="~/IFrame/_MasterPageIFrame.Master" AutoEventWireup="true" CodeBehind="ICampoPorMotivoNuevo.aspx.cs" Inherits="UI.IFrame.ICampoPorMotivoNuevo" ClientIDMode="Static" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="<%=ResolveUrl("~/IFrame/Styles/ICampoPorMotivoNuevo.css?v=" + ConfigurationManager.AppSettings["VERSION"]) %>" media="screen,projection" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

    <!-- Card Alta -->
    <form id="cardFormulario">
        <div class="contenedor-main">
            <div class="row ">
                <!-- Tipo-->
                <div class="col s4">
                    <div class="mi-input-field">
                        <label class="no-select">Tipo</label>
                        <select id="select_Tipos" style="width: 100%"></select>
                    </div>
                </div>

                <!-- Nombre-->
                <div class="col s4">
                    <div class=" input-field fix-margin">
                        <input id="inputFormulario_Nombre" type="text" maxlength="50" length="50" class="contador" required="" aria-required="true" />
                        <label for="inputFormulario_Nombre" class=" no-select">Nombre</label>
                    </div>
                </div>
                <div class="col s2 ">
                    <!-- Orden-->
                    <div class=" input-field fix-margin">
                        <input id="inputFormulario_Orden" type="number" />
                        <label for="inputFormulario_Orden" class=" no-select">Orden</label>
                    </div>
                </div>

                <div class="col s2 " id="contenedor_checkObligatorio">
                    <!-- Obligatorio-->
                    <input class="with-gap" name="group1" type="checkbox" id="check_Obligatorio" />
                    <label for="check_Obligatorio">Obligatorio</label>
                </div>
            </div>
            <div class="row ">
                <div class="col s4 ">
                    <!-- Grupo -->
                    <div class="input-field fix-margin">
                        <input id="inputFormulario_Grupo" maxlength="50" class="contador" length="50" type="text" />
                        <label for="inputFormulario_Grupo" class=" no-select">Grupo</label>
                    </div>
                </div>
                

            </div>
            <div class="row ">
                <div class="col s12">
                    <!-- Descripcion -->
                    <div class="input-field fix-margin">
                        <textarea id="inputFormulario_Observaciones" class="materialize-textarea contador" length="500" maxlength="500"></textarea>
                        <label for="inputFormulario_Observaciones" class=" no-select">Descripcion</label>
                    </div>
                </div>
            </div>
            <div class="row" id="contenedor_opcionesSelector" style="display: none">
                <!-- Opciones Selector-->
                <div class="col s12">
                    <label class="titulo">Opciones</label>
                    <div style="display: flex;     align-items: center;">
                        <div class="input-field fix-margin">
                            <input id="input_Opcion" type="text"/>
                            <label for="input_Opcion" class=" no-select">Opción</label>
                        </div>
                        <a id="btnAgregarOpcionSelector" class="btn btn-cuadrado chico tooltipped no-select btnTabla waves-effect"  data-position="bottom" data-delay="50" data-tooltip="Agregar opción">
                            <i class="material-icons borrar">add</i></a>
                    </div>
                    <div class="contenido">
                        <div class="items"></div>
                    </div>
                </div>
            </div>

            <div id="template_OpcionSelector" style="display: none">
                <div class="opcion">
                    <div class="texto">
                        <label class="nombre"></label>
                        <a class="btn-flat btn-redondo">
                            <i class="material-icons borrar">delete</i></a>
                    </div>
                </div>
            </div>

            <%--            <div id="template_OpcionSelector" style="display: none">
                <div class="card flex opcionSelector" style="align-items: center; min-width: fit-content; height: fit-content; margin-right: 1rem">
                    <div class="row " style="flex: 1">
                        <div class="col s2">
                            <div class="input-field fix-margin">
                                <input class="inputClave" type="number" />
                                <label for="inputClave">Clave</label>
                            </div>
                        </div>
                        <div class="col s10">
                            <div class="input-field fix-margin">
                                <input class="inputValor" type="text" />
                                <label for="inputClave">Valor</label>
                            </div>
                        </div>
                    </div>

                    <a class="btn-flat waves-effect btn-redondo btn-circular"><i class="material-icons">delete</i></a>
                </div>
            </div>--%>
        </div>
    </form>

    <!-- Mi JS -->
    <script type="text/javascript" src="<%=ResolveUrl("~/IFrame/Scripts/ICampoPorMotivoNuevo.js?v=" + ConfigurationManager.AppSettings["VERSION"]) %>"></script>
</asp:Content>
