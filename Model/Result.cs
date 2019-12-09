using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    [Serializable]
    public class Result<Entity>
    {
        public Entity Return { get; set; }
        public Errores Errores { get; set; }

        //public Dictionary<string, object> Data { get; set; }
        public string Error
        {
            get
            {
                return ToStringPublico();
            }
        }

        public bool Ok
        {
            get
            {
                return Errores.Ok;
            }
            set
            {
                Ok = value;
            }
        }

        public string Mensaje
        {
            get
            {
                return ToStringPublico();
            }
        }

       
        public Result()
        {
            //Data = new Dictionary<string, object>();
            Errores = new Errores();
        }

        public Result(string publico)
        {
            //Data = new Dictionary<string, object>();
            Errores = new Errores(publico);
        }

        public void Copy(Result<Entity> result)
        {
            SetErrorInterno(result.Errores.ErroresInternos);
            SetErrorPublico(result.Errores.ErroresPublicos);
        }

        public void Copy(Errores errores)
        {
            SetErrorInterno(errores.ErroresInternos);
            SetErrorPublico(errores.ErroresPublicos);
        }

        public void AddErrorInterno(string message)
        {
            if (message == null) return;
            Errores.AddErrorInterno(message);
        }

        public void AddErrorInterno(Exception e)
        {
            if (e == null) return;
            if (e.Message != null)
            {
                Errores.AddErrorInterno(e.Message);
            }
            if (e.InnerException != null)
            {
                Errores.AddErrorInterno(e.InnerException.Message);
            }
        }

        public void AddErrorInterno(List<string> messages)
        {
            if (messages == null) return;
            if (messages.Count == 0) return;
            foreach (string message in messages)
            {
                AddErrorInterno(message);
            }
        }

        public void SetErrorInterno(List<string> messages)
        {
            Errores.ErroresInternos.Clear();
            AddErrorInterno(messages);
        }

        public void AddErrorPublico(string message)
        {
            if (message == null) return;
            Errores.ErroresPublicos.Add(message);
        }

        public void AddErrorPublico(List<string> messages)
        {
            if (messages == null) return;
            if (messages.Count == 0) return;
            foreach (string message in messages)
            {
                AddErrorPublico(message);
            }
        }

        public void SetErrorPublico(List<string> messages)
        {
            Errores.ErroresPublicos.Clear();
            AddErrorPublico(messages);
        }

        public List<string> MessagesInternos
        {
            get
            {
                return Errores.ErroresInternos;
            }
            set
            {
                Errores.ErroresInternos = value;
            }
        }

        public List<string> MessagesPublicos
        {
            get
            {
                return Errores.ErroresPublicos;
            }
            set
            {
                Errores.ErroresPublicos = value;
            }
        }

        public Dictionary<string, object> ToDictionary()
        {
            var dic = new Dictionary<string, object>();
            dic.Add("Publico", ToStringPublico());
            dic.Add("Interno", ToStringInterno());
            return dic;
        }

        public override string ToString()
        {
            return ToStringPublico();
        }

        public String ToStringInterno()
        {
            if (Ok)
            {
                return "Ok";
            }
            return string.Join("<br/>", Errores.ErroresInternos);
        }

        public String ToStringPublico()
        {
            if (Ok)
            {
                return "Ok";
            }
            string result = string.Join("<br/>", Errores.ErroresPublicos);
            if (string.IsNullOrEmpty(result))
            {
                result = "Error procesando la solicitud";
            }
            return result;
        }
    }

    [Serializable]
    public class Errores
    {
        public bool Ok
        {
            get
            {
                try
                {
                    return ErroresInternos.Count == 0 && ErroresPublicos.Count == 0;
                }
                catch
                {
                    return true;
                }
            }
        }

        public List<string> ErroresInternos { get; set; }
        public List<string> ErroresPublicos { get; set; }
        public string Mensaje
        {
            get
            {
                return ToStringPublico();
            }
        }

        public Errores()
        {
            ErroresInternos = new List<string>();
            ErroresPublicos = new List<string>();
        }

        public Errores(string publico)
        {
            ErroresInternos = new List<string>();
            ErroresPublicos = new List<string>();
            AddErrorPublico(publico);
        }

        public void Copy(Errores result)
        {
            SetErrorInterno(result.ErroresInternos);
            SetErrorPublico(result.ErroresPublicos);

        }
        public void AddErrorInterno(string message)
        {
            if (message == null) return;
            ErroresInternos.Add(message);
        }

        public void AddErrorInterno(Exception e)
        {
            if (e == null) return;
            if (e.Message != null)
            {
                ErroresInternos.Add(e.Message);
            }
            if (e.InnerException != null)
            {
                ErroresInternos.Add(e.InnerException.Message);
            }
        }

        public void AddErrorInterno(List<string> messages)
        {
            if (messages == null) return;
            if (messages.Count == 0) return;
            foreach (string message in messages)
            {
                AddErrorInterno(message);
            }
        }

        public void SetErrorInterno(List<string> messages)
        {
            ErroresInternos.Clear();
            AddErrorInterno(messages);
        }

        public void AddErrorPublico(string message)
        {
            if (message == null) return;
            ErroresPublicos.Add(message);
        }

        public void AddErrorPublicoSiNoExiste(string message)
        {
            if (ErroresPublicos.Count != 0) return;
            AddErrorPublico(message);
        }

        public void AddErrorPublico(List<string> messages)
        {
            if (messages == null) return;
            if (messages.Count == 0) return;
            foreach (string message in messages)
            {
                AddErrorPublico(message);
            }
        }

        public void SetErrorPublico(List<string> messages)
        {
            ErroresPublicos.Clear();
            AddErrorPublico(messages);
        }

        public string ToStringPublico()
        {
            if (Ok) return "Ok";

            if (ErroresPublicos.Count == 0)
            {
                return "Error procesando la solicitud";
            }
            return string.Join("<br/>", ErroresPublicos);
        }

        public string ToStringInterno()
        {
            return string.Join("<br/>", ErroresInternos);
        }

        public string ToStringCompleto()
        {
            return "Error público: " + ToStringPublico() + " | Error interno: " + ToStringInterno();
        }
    }
}
