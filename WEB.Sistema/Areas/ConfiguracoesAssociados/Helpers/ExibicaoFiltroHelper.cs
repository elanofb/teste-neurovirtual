using System.Web.Mvc;

namespace WEB.Areas.ConfiguracoesAssociados.Helpers {

    public class ExibicaoFiltroHelper {

        //Atributos
        private static ExibicaoFiltroHelper _instance;

        //Propriedades
        public static ExibicaoFiltroHelper getInstance => _instance = _instance ?? new ExibicaoFiltroHelper();

        //Private Constructor
        private ExibicaoFiltroHelper() {

        }

        public static MultiSelectList selectList(string[] selected){
            var list = new[] {
                new{value = "area-associado", text = "Campos Área Associado"},
                new{value = "adm", text = "Campos Adm"},
                new {value = "hidden", text = "Campos Hidden"},
                new{value = "add", text = "Campos Cadastro"},
                new{value = "edit", text = "Campos Edição"},
                new {value = "disabled", text = "Campos Desativados"},
            };
            return new MultiSelectList(list, "value", "text", selected);
        }
    }
}