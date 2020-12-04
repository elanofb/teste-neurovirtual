using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web.Mvc;

namespace System {
    public static class StringExtensions {

        // Cpaturar a string e joga-la dentro de uma lista
        public static List<string> ToOneList(this string strValue) {
            List<string> lista = new List<string>();
            lista.Add(strValue);

            return lista;
        }

        // Cpaturar a string e joga-la dentro de uma lista
        public static List<string> ToEmailList(this string strValue, string separator = "") {

            List<string> lista = new List<string>();

            if (String.IsNullOrEmpty(strValue)) {
                return lista;
            }

            if (!String.IsNullOrEmpty(separator)) {
                lista = strValue.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();
            } else {
                lista.Add(strValue);
            }


            return lista;
        }

        //Remover tags de uma determinada string
        public static string removeTags(this string inputString) {
            inputString = (inputString ?? "");

            const string HTML_TAG_PATTERN = "<.*?>";

            return Regex.Replace(inputString, HTML_TAG_PATTERN, string.Empty);
        }

        //Extensao para facilitar filtro de numeros
        public static string onlyNumber(this string value) {

            return UtilString.onlyNumber(value);

        }

        //Extensao para facilitar filtro alphanumerico
        public static string onlyAlphaNumber(this string value) {

            return UtilString.onlyAlphaNumber(value);

        }

        //Extensao para facilitar a conversao de int nullable
        public static string removerAcentuacao(this string value) {

            return UtilString.cleanAccents(value);
        }

        //Remover caracteres especiais aceitando somente letras e números
        public static string removerCaracteresEspeciais(this string value) {

            return Regex.Replace(value, "[^0-9a-zA-Z]+", "");
        }

        //Remover caracteres especiais aceitando somente letras e números
        public static string onlyAlphaSpace(this string value) {

            return Regex.Replace(value, "[^a-zA-Z]+", "");
            ;
        }

        //Extensao para facilitar a conversao de int nullable
        public static string decodeString(this string value) {

            if (string.IsNullOrEmpty(value)) {
                return "";
            }

            value = value.Replace("&#39;", "'");

            return value;
        }

        //Devolve um vazio caso a string seja nula
        public static string stringOrEmpty(this object str) {
            str = str ?? "";
            return str.ToString().Trim();
        }

        //Devolve um vazio caso a string seja nula em uppercase
        public static string stringOrEmptyUpper(this object str) {
            str = str ?? "";
            return str.ToString().Trim().ToUpper();
        }

        //Devolve um vazio caso a string seja nula em lowercase
        public static string stringOrEmptyLower(this object str) {
            str = str ?? "";
            return str.ToString().Trim().ToLower();
        }

        /// <summary>
        /// Transformar o texto em Uppercase
        /// </summary>
        public static string toUppercaseWords(this string value) {

            if (value.isEmpty()) {
                return "";
            }

            string[] array = value.Split(' ');

            string retorno = string.Empty;

            string[] wordsByPass = new[] { "de", "da", "dos", "do", "e", "a", "ou" };

            for (int i = 0; i < array.Length; i++) {

                string word = array[i].stringOrEmptyLower();

                if (word.isEmpty()) {
                    continue;
                }

                if (wordsByPass.Contains(word)) {

                    retorno = string.Concat(retorno, " ", word);

                    continue;
                }

                retorno = string.Concat(retorno, " ", word.toUppercaseFirst());
            }

            return retorno;
        }

        /// <summary>
        /// Transformar a primeira letra em padrao CamelCse
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string toUppercaseFirst(this string s) {

            if (string.IsNullOrEmpty(s)) {
                return string.Empty;
            }

            char[] a = s.ToCharArray();

            a[0] = char.ToUpper(a[0]);

            return new string(a);
        }

        // Cpaturar a string e joga-la dentro de uma lista
        public static bool isEmpty(this object strValue) {
            return string.IsNullOrEmpty(strValue?.ToString().Trim());
        }

        // Cpaturar a string e joga-la dentro de uma lista
        public static string TrimNullable(this string strValue) {
            if (strValue == null) {
                return "";
            }

            return strValue.Trim();
        }

        // 
        public static int LengthNullable(this string strValue) {
            if (strValue == null) {
                return 0;
            }

            return strValue.Length;
        }

        // 
        public static string ToUpperNullable(this string strValue) {
            if (strValue == null) {
                return "";
            }

            return strValue.ToUpper();
        }

        // 1 - Abreviar string
        public static string abreviar(this string str, int qtdeCaracteres, string strSufixo = "") {
            
            str = str.TrimNullable();
            
            if (String.IsNullOrEmpty(str)) {
                return String.Empty;
            }
            
            if (str.Length > qtdeCaracteres) {
                str = String.Concat(str.Substring(0, qtdeCaracteres), strSufixo);
            }
            return str;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static string nl2br(this string input){
            
            return input.nl2br(true);
        }
        
        /// <summary>
        /// Converte quebras de linha em quebras HTML
        /// </summary>
        public static string nl2br(this string input, bool is_xhtml){
            
            return input.Replace("\r\n", is_xhtml ? "<br />\r\n" : "<br>\r\n");
        }        
        
        /// <summary>
        /// 
        /// </summary>
        public static string defaultIfEmpty(this string str, string strDefault = "...") {

            str = str.stringOrEmpty();

            if (str.isEmpty()) {
                return strDefault;
            }

            return str;
        }      
        
        /// <summary>
        /// 
        /// </summary>
        public static MvcHtmlString htmlIfEmpty(this string str, string[] args, string htmlDefault = "<em class='fs-11'>{0} não informado</em>") {

            str = str.stringOrEmpty();

            if (str.isEmpty()) {
                return new MvcHtmlString(string.Format(htmlDefault, args));
            }

            return new MvcHtmlString(str);
        }           
    }
}