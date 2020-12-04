
using System.Collections.Specialized;

namespace System {

    public static class CollectionExtensions {

        //Exibicao das datas
        public static string getIfExists(this NameValueCollection dados, string key, string valorPadrao = "") {

            if (dados == null) {
                return "";
            }

            if (!dados[key].isEmpty()) {
                return dados[key];
            }

            if (!valorPadrao.isEmpty()) {
                return valorPadrao;
            }

            return string.Empty;

        }


    }
}