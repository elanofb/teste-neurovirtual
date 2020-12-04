using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System {

    public static class UtilString {

        //
        public static string onlyNumber(string str) {
            return (str.isEmpty()) ? "" : Regex.Replace(str, @"[^\d]", String.Empty);
        }

        //
        public static string onlyAlphaNumber(string str) {
            return (str.isEmpty()) ? "" : Regex.Replace(str, @"[^\w\s]", String.Empty);
        }
        
        //
        public static string onlyEmailChars(string str) {
            return (str.isEmpty())? "": Regex.Replace(str, @"[^a-zA-Z0-9@._]", String.Empty);
        }
        
        //
        public static string onlyUrlChars(string str) {

            if (str.isEmpty()){
                return str;
            }
            
            string strClean = UtilString.cleanAccents(str);
            
            return (strClean.isEmpty())? "": Regex.Replace(strClean, @"[^a-zA-Z0-9._-]", String.Empty);
        }

        //Devolve uma string sem acentos
        public static string cleanAccents(string str) {
            if (string.IsNullOrEmpty(str)) {
                return String.Empty;
            }
	        byte[] bytes = Encoding.GetEncoding("iso-8859-8").GetBytes(str);
	        return Encoding.UTF8.GetString(bytes);
        }

        //Devolve uma string sem acentos
        public static string cleanStringToURL(string str) {

            if (string.IsNullOrEmpty(str)) {
                return String.Empty;
            }
            str = UtilString.cleanAccents(str);
            str = Regex.Replace(str, "[^0-9a-zA-Z\\s]+", "");
            return str.Replace(" ", "-");
        }

        //Devolve um vazio caso a string seja nula
        public static string notNull(string str) {
            str = str ?? "";
            return str;
        }

        public static string notNull(object str) {
            str = str ?? "";
            return str.ToString();
        }

        //
        public static string toQueryString(NameValueCollection qs) {
            return string.Join("&", Array.ConvertAll(qs.AllKeys, key => string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(qs[key]))));
        }

        //
        public static string formatCPFCNPJ(string dado) {
            string info = onlyNumber(dado);
            if (info.Length > 11) {
                return toFormat(info, "##.###.###/####-##");
            }
	        return toFormat(info, "###.###.###-##");
        }

        //
        public static string formatPhone(string dado) {
            string info = onlyNumber(dado);
            if (info.Length == 10) {
                return toFormat(info, "## ####-####");
            }
	        return toFormat(info, "## #####-####");
        }

        //
        public static string formatCEP(string dado) {
            if (String.IsNullOrEmpty(dado)) return "";
            string info = onlyNumber(dado);
            return toFormat(info, "#####-###");
        }

        //
        private static string toFormat(string valor, string mascara) {
            StringBuilder dado = new StringBuilder();

            // remove caracteres nao numericos
            foreach (char c in valor) {
                if (Char.IsNumber(c)) dado.Append(c);
            }

            int indMascara = mascara.Length;
            int indCampo = dado.Length;

            for (;indCampo > 0 && indMascara > 0;) {
                if (mascara[--indMascara] == '#') indCampo--;
            }

            StringBuilder saida = new StringBuilder();
            for (;indMascara < mascara.Length;indMascara++) {
                saida.Append((mascara[indMascara] == '#') ? dado[indCampo++] : mascara[indMascara]);
            }
            return saida.ToString();
        }

        //
        public static string reverse(string s) {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        //
        public static string encodeURL(string s) {
			return HttpContext.Current.Server.UrlEncode(s);
        }

        //
        public static string decodeURL(string s) {
			return HttpContext.Current.Server.UrlDecode(s);
        }

        //
        public static string abreviarTextos(string texto){

            texto = texto.ToUpper();

            texto = texto.Replace("RUA", "R");

            texto = texto.Replace("AVENIDA", "AV");

            texto = texto.Replace("RODOVIA", "ROD");

            texto = texto.Replace("ALAMEDA", "AL");

            texto = texto.Replace("PRAÇA", "PC");

            texto = texto.Replace("ENGENHEIRO", "ENG");

            return texto;
        }

        //
        public static string randomString(int strLength) {
            Random Random = new Random();
            int seed = Random.Next(1, int.MaxValue);

            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            //const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            //const string specialCharacters = @"!#$%&'()*+,-./:;<=>?@[\]_";

            var chars = new char[strLength];
            var rd = new Random(seed);

            for (var i = 0;i < strLength;i++) {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        //
        public static string randomNumber(int strLength) {
            Random Random = new Random();
            int seed = Random.Next(1, int.MaxValue);

            const string allowedChars = "0123456789";

            var chars = new char[strLength];
            var rd = new Random(seed);

            for (var i = 0;i < strLength;i++) {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        
         public static string acertaCasas(string str, int tamanho, string valorAdicionar = "0"){

            if (str.Length < tamanho) {
                for (int x = str.Length; x < tamanho; x++) { 
                    str = valorAdicionar + str;
                }
            }

            return str;
        }

		//
	    public static string removeHtml(string input) {
            if (!input.isEmpty()) {
                return Regex.Replace(input, "<[^>]+>|&nbsp;", "").Trim();
            }
            return "";
	    }

        public static string limparParaCSV(string campo) {

            if (!String.IsNullOrEmpty(campo)) {
                return campo.Replace(System.Environment.NewLine, "")
                    .Replace("\t", "")
                    .Replace(";", "")
                    .Replace("<strong>", "")
                    .Replace("</strong>", "");
            }

            return String.Empty;
        }
    }
}

