using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules.Rules;
using UI.Resources;
using System.Web;
using Intranet_UI.Utils;
using Model;
using Model.Resultados;

namespace UI.IFrame
{
    public partial class IMovilCambiarEstado : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var resultado = new Dictionary<string, object>();
            if (Request.QueryString["Id"] == null)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            int id = 0;
            if (!int.TryParse("" + Request.QueryString["Id"], out id) || id <= 0)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            if (Request.QueryString["IdEstadoAnterior"] == null)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            if (Request.QueryString["Modo"] == null)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            int idEstadoAnterior = 0;
            if (!int.TryParse("" + Request.QueryString["IdEstadoAnterior"], out idEstadoAnterior) || idEstadoAnterior <= 0)
            {
                resultado.Add("Error", "Solucitud invalida");
                InitJs(resultado);
                return;
            }

            var usuarioLogeado = SessionKey.getUsuarioLogueado(HttpContext.Current.Session);

            var modo = Request.QueryString["Modo"];
            resultado.Add("Modo", modo);

            switch (modo)
            {
                case "Movil":
                    var consultaEstados = new EstadoMovilRules(usuarioLogeado).GetAllParaCambiarEstado();
                    if (!consultaEstados.Ok)
                    {
                        resultado.Add("Error", consultaEstados.Error);
                        InitJs(resultado);
                        return;
                    }

                    var estadoAnterior = consultaEstados.Return.Where(x => x.Id == idEstadoAnterior).FirstOrDefault();
                    if(estadoAnterior==null)
                    {
                        var consultaEstadoAnterior= new EstadoMovilRules(usuarioLogeado).GetById(idEstadoAnterior);
                        if (!consultaEstadoAnterior.Ok)
                        {
                            resultado.Add("Error", consultaEstadoAnterior.Error);
                            InitJs(resultado);
                            return;
                        }

                        estadoAnterior =new Resultado_EstadoMovil( consultaEstadoAnterior.Return);
                    }

                    consultaEstados.Return.Remove(estadoAnterior);

                    resultado.Add("Estados", consultaEstados.Return);
                    resultado.Add("NombreEstadoAnterior", estadoAnterior.Nombre);
                    break;

                case "Flota":
                    var consultaEstadosFlota = new EstadoFlotaRules(usuarioLogeado).GetAll(false);
                    if (!consultaEstadosFlota.Ok)
                    {
                        resultado.Add("Error", consultaEstadosFlota.Error);
                        InitJs(resultado);
                        return;
                    }

                    var estadoAnteriorFlota = consultaEstadosFlota.Return.Where(x => x.Id == idEstadoAnterior).FirstOrDefault();
                    consultaEstadosFlota.Return.Remove(estadoAnteriorFlota);
                    resultado.Add("Estados", Resultado_EstadoFlota.ToList(consultaEstadosFlota.Return));
                    resultado.Add("NombreEstadoAnterior", estadoAnteriorFlota.Nombre);  
                    break;
            }


            resultado.Add("Id", id);
            resultado.Add("IdEstadoAnterior", idEstadoAnterior);
            InitJs(resultado);
        }
    }
}