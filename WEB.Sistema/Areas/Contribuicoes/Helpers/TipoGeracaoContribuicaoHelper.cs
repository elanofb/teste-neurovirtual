using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BLL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.Helpers{

    public class TipoGeracaoContribuicaoHelper{

        //Constantes
        private static TipoGeracaoContribuicaoHelper _instance; 

        //Atributos
         private static ITipoGeracaoContribuicaoBL _TipoGeracaoContribuicaoBL;

        //Propriedades
        public static TipoGeracaoContribuicaoHelper getInstance => _instance = _instance ?? new TipoGeracaoContribuicaoHelper();
        private ITipoGeracaoContribuicaoBL OTipoGeracaoContribuicaoBL   => _TipoGeracaoContribuicaoBL = _TipoGeracaoContribuicaoBL ?? new TipoGeracaoContribuicaoBL();
       

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int? selected) {


            var listaItens = OTipoGeracaoContribuicaoBL.listar(true)
                                    .OrderBy(x => x.descricao)
                                    .Select(x => new {
                                        x.id,
                                        descricao = x.descricao
                                    }).ToList();

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }

    }
}