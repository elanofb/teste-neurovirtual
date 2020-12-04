using System.Web.Mvc;
using DAL.Enderecos;

namespace WEB.Areas.Enderecos.Helpers {

    public class TipoEnderecoHelper {

        public static SelectList selectList(int? selected) {

            var list = new[] {
                new{value = TipoEnderecoConst.PRINCIPAL, text = "Mudar"},
                new{value = TipoEnderecoConst.COMERCIAL, text = "Mudar 2"}
            };

            return new SelectList(list, "value", "text", selected);
        }
    }
}