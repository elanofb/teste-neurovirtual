using System.Web.Mvc;
using BoletoNet;

namespace WEB.Areas.Configuracao.Helpers {

    public class ConfiguracaoFinanceiroHelper {

        //
        public static SelectList selectListTipoArquivoRemessa(byte? selected) {
            var list = new[] { 
                    new{value = (int)TipoArquivo.CNAB240, text = "CNAB240"},
                    new{value = (int)TipoArquivo.CNAB400, text = "CNAB400"},
                    new{value = (int)TipoArquivo.CBR643, text = "CBR643"}
            };
            return new SelectList(list, "value", "text", selected);
        }

    }
}