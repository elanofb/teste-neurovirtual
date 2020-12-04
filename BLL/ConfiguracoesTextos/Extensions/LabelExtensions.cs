using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BLL.ConfiguracoesTextos.Extensions {

    public static class LabelExtensions {

        /// <summary>
        /// Captura de textos dinâmicos no sistema
        /// </summary>
        public static IHtmlString labelTexto(this HtmlHelper helper, int idOrganizacao, string chave, string valorDefault = "") {

            var label = labelTexto(idOrganizacao, chave, valorDefault);

            return helper.Raw(label);
        }

        /// <summary>
        /// Captura de textos dinâmicos no sistema
        /// </summary>
        public static string labelTexto(this HttpContextBase context, int idOrganizacao, string chave, string valorDefault = "") {

            var label = labelTexto(idOrganizacao, chave, valorDefault);

            return label;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static string labelTextoReplace(this string texto, int idOrganizacao, string chave, string hash) {

            return texto.Replace(hash, labelTexto(idOrganizacao, chave));
        }

        /// <summary>
        /// 
        /// </summary>
        public static string labelTexto(int idOrganizacao, string chave, string valorDefault = "") {

            if (idOrganizacao == 0 && !valorDefault.isEmpty()) {
                return valorDefault;
            }
            
            string chaveLower = chave.onlyAlphaNumber().removerAcentuacao().Replace(" ","").ToLower();

            var listaLabels = ConfiguracaoLabelBL.getInstance.listarFromCache(idOrganizacao);

            var OLabel = listaLabels.FirstOrDefault(x => x.idOrganizacao == idOrganizacao && x.key == chaveLower && x.idIdioma == null);

            if (OLabel != null) {

                return OLabel.label;
            }

            if (idOrganizacao == 0) {
                return chave;
            }

            //Se nao houve sobreposicao com o id da associacao, usamos o padrao do sistema 
            OLabel = listaLabels.FirstOrDefault(x => x.idOrganizacao == 0 && x.key == chaveLower);

            if (OLabel != null) {

                return OLabel.label;
            }

            //Se nao localizou da organizacao nem padrao do sistema, utiliza o padrao informado
            if (!valorDefault.isEmpty())
            {
                return valorDefault;
            }

            return chave;
        }
    }
}
