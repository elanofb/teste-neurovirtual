using System.Collections.Generic;

namespace WEB.Extensions {

    public static class StringExtensions {

        // Cpaturar a string e joga-la dentro de uma lista
        public static List<string> ToOneList(this string strValue) {
            List<string> lista = new List<string>();
            lista.Add(strValue);

            return lista;
        }


    }
}