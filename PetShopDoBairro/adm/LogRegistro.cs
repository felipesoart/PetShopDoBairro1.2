using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PetShopDoBairro.adm
{
    public class LogRegistro
    {
        private static object _lock = new object();

        public static void registrarLog(string mensagem, string executavelOrigem, string caminhoLogErro, DateTime dataHoraErro)
        {
            try
            {
                //string caminhoLogErro = @"C:\LogRegistro\";
                if (!Directory.Exists(caminhoLogErro))
                {
                    Directory.CreateDirectory(caminhoLogErro);
                }

                lock (_lock)
                {
                    using (StreamWriter sw = new StreamWriter(caminhoLogErro + @"\" + executavelOrigem + "_" + dataHoraErro.ToString("yyyy-MM-dd-HH-mm-ss-fff") + "_Log.txt", true))
                    {
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " " + mensagem);
                        sw.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                Guid _guid = Guid.NewGuid();
                string pathArquivoERROLOG = caminhoLogErro + @"\"
                    + dataHoraErro.ToString("yyyy-MM-dd-HH-mm-ss-fff")
                    + "_" + _guid
                    + "_Log.txt";

                using (StreamWriter sw = new StreamWriter(pathArquivoERROLOG, true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + mensagem + Environment.NewLine +
                        ex.ToString());
                    sw.Close();
                }
            }
        }

    }
}