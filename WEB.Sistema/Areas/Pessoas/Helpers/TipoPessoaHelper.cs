using System.Web.Mvc;

namespace WEB.Areas.Pessoas.Helpers{

    public class TipoPessoaHelper{

        // Atributos 
        private static TipoPessoaHelper _helper;

        // Propriedades
        public static TipoPessoaHelper getInstance => _helper = _helper ?? new TipoPessoaHelper();

        // Constants
		private static readonly string PESSOA_FISICA = "Física";
		private static readonly string PESSOA_JURIDICA = "Jurídica";
        private static readonly string TODOS = "Todos";

        //
        public static SelectList selectList(string selected){

            var list = new[] { 
                    new{value = "F", text = PESSOA_FISICA},
                    new{value = "J", text = PESSOA_JURIDICA}
            };
            return new SelectList(list, "value", "text", selected);
        }

        public SelectList selectListTodos(string selected) {
            var list = new[] {
                    new{value = "T", text = TODOS},
                    new{value = "F", text = PESSOA_FISICA},
                    new{value = "J", text = PESSOA_JURIDICA}
            };
            return new SelectList(list, "value", "text", selected);
        }

        public string getTipoPessoa(string tipoPessoa) {
            switch (tipoPessoa) {
                case "F" : return PESSOA_FISICA;
                case "J" : return PESSOA_JURIDICA;
                default  : return TODOS;
            }
        }

    }
}