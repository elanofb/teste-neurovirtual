using System.Web.Mvc;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.Helpers {

    public class BannerHelper {


        //Select lista para selecao de items
        public static SelectList selectListPosicao(string selected) {
            var list = new[] { 
                    new{value = PosicaoBannerConst.PRINCIPAL, text = "Principal"}
            };
            return new SelectList(list, "value", "text", selected);
        }

    }
}