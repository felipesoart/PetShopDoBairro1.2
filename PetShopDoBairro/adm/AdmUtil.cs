using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetShopDoBairro.adm
{
    public class AdmUtil
    {
        public static string removerAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }

            string comCaract = @"!@#$%&()+{}[];°ªº¹²³£¢¬§";

            for (int i = 0; i < comCaract.Length; i++)
            {
                texto = texto.Replace(comCaract[i].ToString(), "");
            }

            return texto;
        }
    }
}