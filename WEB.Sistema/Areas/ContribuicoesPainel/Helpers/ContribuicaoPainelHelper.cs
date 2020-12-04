using System.Collections.Generic;
using System.Web.Mvc;
using UTIL.UtilClasses;

namespace WEB.Areas.Contribuicoes.Helpers{

    public class ContribuicaoPainelHelper{

        //Constantes
        private static ContribuicaoPainelHelper _instance; 

        //Atributos

        //Propriedades
        public static ContribuicaoPainelHelper getInstance => _instance = _instance ?? new ContribuicaoPainelHelper();


        //Carregar combo de selecao dos tipos de associados
        public SelectList  selectListFiltroAssociados(string selected) {

            var listaOpcoes = new List<OptionSelect>();

            listaOpcoes.Add( new OptionSelect { value = "", text = "Somente associados já cobrados"});

            listaOpcoes.Add( new OptionSelect { value = "todos", text = "Todos os associados"});

 
            return new SelectList(listaOpcoes, "value", "text", selected);
        }

    }
}