using System.Web.Mvc;

namespace WEB.Areas.Configuracao.Helpers {

    public class ConfiguracaoGeralHelper {


        //
        public static SelectList selectListTipoGeracaoContribuicao(bool? selected) {
            var list = new[] { 
                    new{value = true, text = "Geração Automática"},
                    new{value = false, text = "Geração Manual"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListTipoAtualizacaoInadimplencia(bool? selected) {
            var list = new[] {
                    new{value = true, text = "Atualização Automática"},
                    new{value = false, text = "Atualização Manual"}
            };
            return new SelectList(list, "value", "text", selected);
        }

        //
        public static SelectList selectListDiaVencimentoMensalidade(byte? selected) {
            var list = new[] { 
                    new{value = 5, text = "5"},
                    new{value = 10, text = "10"},
                    new{value = 15, text = "15"},
                    new{value = 20, text = "20"},
                    new{value = 25, text = "25"}
            };
            return new SelectList(list, "value", "text", selected);
        }
    }
}