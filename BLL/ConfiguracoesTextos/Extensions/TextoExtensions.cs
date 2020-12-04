using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BLL.ConfiguracoesTextos.Extensions {

    public static class TextoExtensions {

        /// <summary>
        /// Captura de textos dinâmicos no sistema
        /// </summary>
        public static IHtmlString texto(this HtmlHelper helper, int idOrganizacao, string chave, string valorDefault = "") {

            var label = texto(idOrganizacao, chave, valorDefault);

            return helper.Raw(label);
        }

        /// <summary>
        /// Captura de textos dinâmicos no sistema
        /// </summary>
        public static string texto(this HttpContextBase context, int idOrganizacao, string chave, string valorDefault = "") {

            var label = texto(idOrganizacao, chave, valorDefault);

            return label;
        }
        

/*        public static string textoReplace(this string texto, int idOrganizacao, string chave, string hash) {

            string str = texto(idOrganizacao, chave, "");

            return texto.Replace(hash, str);
        }*/
        
        /// <summary>
        /// 
        /// </summary>
        public static string texto(int idOrganizacao, string chave, string defaultTexto = ""){

            if (idOrganizacao == 0 && !defaultTexto.isEmpty()) {
                
                return defaultTexto;
            }
            
            string chaveLower = chave.onlyAlphaNumber().removerAcentuacao().Replace(" ","").ToLower();

            var listaTextos = ConfiguracaoTextoBL.getInstance.listarFromCache(idOrganizacao);

            var OTexto = listaTextos.FirstOrDefault(x => x.idOrganizacao == idOrganizacao && x.key == chaveLower);

            if (OTexto != null) {

                return OTexto.texto;
                
            }

            if (idOrganizacao == 0) {
                return chave;
            }

            //Se nao houve sobreposicao com o id da associacao, usamos o padrao do sistema 
            OTexto = listaTextos.FirstOrDefault(x => x.idOrganizacao == 0 && x.key == chaveLower);

            if (OTexto != null) {

                return OTexto.texto;
                
            }

            //Se nao localizou da organizacao nem padrao do sistema, utiliza o padrao informado
            if (!defaultTexto.isEmpty()){
                
                return defaultTexto;
            }

            return chave;    
        }
    }
}
