using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Una.Cripto.Helpers;
using System.IO;
using System.Text;


namespace Una.Cripto.Cliente.Controllers
{
    public class HomeController : Controller
    {
        private string ConfigPath = Path.Combine(Environment.CurrentDirectory, "CriptoConfig.txt");

        //
        // GET: /Index/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Execute(Criptografia cripto)
        {
            try
            {
                HttpPostedFileBase postedFile = Request.Files[0];
                object[] parameters = { postedFile.InputStream };
                MethodInfo mi = typeof(Criptografia).GetMethod(Request["Metodo"], new[] { typeof(Stream) });

                mi.Invoke(cripto, parameters);
                SaveConfig();
            }
            catch
            { }
            return View("Index");
        }

        private void SaveConfig()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string fileName = Request.Files[0].FileName;
                sb.AppendLine(string.Format("Arquivo: {0}", fileName));
                sb.AppendLine(string.Format("Algoritmo: {0}", Request["Algoritmo"]));
                sb.AppendLine(string.Format("Modo de Operação: {0}", Request["Mode"]));
                sb.AppendLine(string.Format("Modo de Padding: {0}", Request["Padding"]));
                sb.AppendLine(string.Format("Chave: {0}", Request["Key"]));
                sb.AppendLine(string.Format("Vetor de Inicialização: {0}", Request["IV"]));
                sb.AppendLine(string.Format("Ação: {0}", Request["Metodo"]));
                sb.AppendLine("----------------------------------");

                using (FileStream fs = System.IO.File.Open(ConfigPath, FileMode.Append))
                {
                    byte[] arr = Encoding.UTF8.GetBytes(sb.ToString());
                    fs.Write(arr, 0, arr.Length);
                }
            }
            catch
            { }
        }
    }
}
