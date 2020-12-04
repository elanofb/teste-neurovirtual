using System.Web.Mvc;
using DAL.Associados;

namespace WEB.Helpers {

    public class SituacaoContribuicaoHelper {

        public static SelectList getSituacao(string selected) {
            var list = new[] { 
                    new{value = SituacaoContribuicaoConst.ADIMPLENTE, text = "Adimplente"},
                    new{value = SituacaoContribuicaoConst.INADIMPLENTE, text = "Inadimplente"}
            };

            return new SelectList(list, "value", "text", selected);
        }

    }
}