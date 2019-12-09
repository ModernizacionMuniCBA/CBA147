using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model.Entities;
using Rules;
using Rules.Rules;
using System.Web;
using UI.Resources;
using Intranet_UI.Utils;
using Model.Resultados;
using Model.Consultas;

namespace UI.IFrame
{
    public partial class IRubroMotivoNuevo : _IFrame
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var resultado = new Dictionary<string, object>();

            var usuarioLogeado = SessionKey.getUsuarioLogueado(Session);

            try
            {
                var idGrupo = -1;
                if (Request.Params["IdGrupo"] != null)
                {
                    idGrupo = int.Parse(Request.Params["IdGrupo"]);
                }

                //Si estoy editando
                int? idCategoria = null;
                if (Request.Params["Id"] != null)
                {
                    idCategoria = int.Parse(Request.Params["Id"]);
                    var resultadoCategoria = new RubroMotivoRules(usuarioLogeado).GetById(idCategoria.Value);
                    if (!resultadoCategoria.Ok)
                    {
                        resultado.Add("Error", resultadoCategoria.Errores);
                        InitJs(resultado);
                        return;
                    }

                    if (resultadoCategoria.Return == null)
                    {
                        resultado.Add("Error", "El rubro de motivo no existe");
                        InitJs(resultado);
                        return;
                    }

                    idGrupo = resultadoCategoria.Return.Grupo.Id;
                    resultado.Add("RubroMotivo", new Resultado_CategoriaMotivo(resultadoCategoria.Return));
                }

                //Motivos en grupo
                var resultadoMotivosGrupo = new MotivoPorRubroMotivoRules(usuarioLogeado).GetByIdGrupo(idGrupo);
                if (!resultadoMotivosGrupo.Ok)
                {
                    resultado.Add("Error", "Error con el area");
                    InitJs(resultado);
                    return;
                }

                //Motivos
                var resultadoMotivos = new MotivoRules(usuarioLogeado).GetAll(false);
                if (!resultadoMotivos.Ok)
                {
                    resultado.Add("Error", "Error con el area");
                    InitJs(resultado);
                    return;
                }

                var a=resultadoMotivosGrupo.Return.Select(z => z.Motivo).ToList();

                resultado.Add("Motivos", ResultadoTabla_Motivo.ToList(resultadoMotivos.Return.Except(a).ToList()));
                resultado.Add("IdGrupo", idGrupo);

            }
            catch (Exception ex)
            {
                resultado.Add("Error", "Error");
            }


            //Devuelvo la info
            InitJs(resultado);
        }

    }
}