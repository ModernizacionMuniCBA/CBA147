<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="UI.Controls.Navigation.Footer" %>

<style type="text/css">
    .footer {
        display: flex;
        align-items: center;
        justify-content: flex-start;
        padding-left: 8px;
        padding-right: 8px;
        background-color: var(--colorFooter);
        height: 48px;
        max-height: 48px;
        min-height: 48px;
    }

        .footer .logo {
            width: auto;
            height: 38px;
            -webkit-filter: initial;
            filter: initial;
        }

        .footer > .nombreSistema {
            color: white;
            flex:1;
            text-align: right;
            margin-right:8px;
            font-size:1rem;
        }

    @media only screen and (max-width : 992px) {
        .footer {
            background-color: var(--colorFondo);
        }

            .footer .logo {
                -webkit-filter: invert(100%);
                filter: invert(100%);
            }
    }

    .invert {
    }
</style>
<div id="footer" class="footer hide">
    <img class="logo" src="<%=ResolveUrl("~/Resources/Imagenes/logo_muni.png") %>" />
    <label class="nombreSistema" id="textoNombre">Sistema Integral de Gestion Operativa</label>
</div>

<script type="text/javascript">

</script>
