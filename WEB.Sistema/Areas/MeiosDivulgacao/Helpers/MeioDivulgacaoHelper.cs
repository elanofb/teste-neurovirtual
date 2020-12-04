using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.MeiosDivulgacao;

namespace WEB.Areas.MeiosDivulgacao.Helpers{

    public class MeioDivulgacaoHelper{

        //Constantes
        private static MeioDivulgacaoHelper _instance; 

        //Atributos
         private static IMeioDivulgacaoBL _MeioDivulgacaoBL;

        //Propriedades
        public static MeioDivulgacaoHelper getInstance => _instance = _instance ?? new MeioDivulgacaoHelper();
        private IMeioDivulgacaoBL OMeioDivulgacaoBL   => _MeioDivulgacaoBL = _MeioDivulgacaoBL ?? new MeioDivulgacaoBL();
       

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int? selected) {

            var listaItens = CacheService.getInstance.carregar(CacheService.MEIO_DIVULGACAO_DD_SIMPLES);

            if (listaItens != null) {

                return new SelectList((IEnumerable)listaItens, "id", "descricao", selected);
            }

            listaItens = OMeioDivulgacaoBL.listar(0, "", true)
                                .OrderBy(x => x.descricao)
                                    .Select(x => new {
                                        x.id,
                                        descricao = x.descricao
                                    }).ToList();

            CacheService.getInstance.adicionar(CacheService.MEIO_DIVULGACAO_DD_SIMPLES, listaItens);

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }

    }
}