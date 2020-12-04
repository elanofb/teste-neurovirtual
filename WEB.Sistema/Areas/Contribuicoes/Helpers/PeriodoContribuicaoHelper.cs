using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BLL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.Helpers{

    public class PeriodoContribuicaoHelper{

        //Constantes
        private static PeriodoContribuicaoHelper _instance; 

        //Atributos
         private static IPeriodoContribuicaoBL _PeriodoContribuicaoBL;

        //Propriedades
        public static PeriodoContribuicaoHelper getInstance => _instance = _instance ?? new PeriodoContribuicaoHelper();
        private IPeriodoContribuicaoBL OPeriodoContribuicaoBL   => _PeriodoContribuicaoBL = _PeriodoContribuicaoBL ?? new PeriodoContribuicaoBL();
       

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int? selected) {


            var listaItens = OPeriodoContribuicaoBL.listar(true)
                                    .Select(x => new {
                                        x.id,
                                        descricao = x.descricao
                                    }).ToList();

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }

    }
}