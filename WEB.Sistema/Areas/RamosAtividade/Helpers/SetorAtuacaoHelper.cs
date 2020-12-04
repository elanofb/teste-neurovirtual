using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.RamosAtividade;

namespace WEB.Areas.RamosAtividade.Helpers{

    public class SetorAtuacaoHelper{

        //Constantes
        private static SetorAtuacaoHelper _instance; 

        //Atributos
         private static ISetorAtuacaoBL _SetorAtuacaoBL;

        //Propriedades
        public static SetorAtuacaoHelper getInstance => _instance = _instance ?? new SetorAtuacaoHelper();
        private ISetorAtuacaoBL OSetorAtuacaoBL   => _SetorAtuacaoBL = _SetorAtuacaoBL ?? new SetorAtuacaoBL();
       

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int idRamoAtividade, int? selected) {

            var listaItens = CacheService.getInstance.carregar(CacheService.SETOR_ATUACAO_DD_SIMPLES);

            if (listaItens != null) {

                return new SelectList((IEnumerable)listaItens, "id", "descricao", selected);
            }

            listaItens = OSetorAtuacaoBL.listar(idRamoAtividade, "", true).OrderBy(x => x.RamoAtividade.descricao).ThenBy(x => x.descricao)
                                    .Select(x => new {
                                        x.id,
                                        descricao = x.RamoAtividade.descricao +"/"+ x.descricao
                                    }).ToList();

            CacheService.getInstance.adicionar(CacheService.SETOR_ATUACAO_DD_SIMPLES, listaItens);

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }
        
        //Carregar combo de selecao dos tipos de associados
        public SelectList selectListAtividadeOpcional(int? selected, bool flagMonstrarRamo = true) {

            var listaItens = CacheService.getInstance.carregar(CacheService.SETOR_ATUACAO_DD_SIMPLES);

            if (listaItens != null) {

                return new SelectList((IEnumerable)listaItens, "id", "descricao", selected);
            }

            listaItens = OSetorAtuacaoBL.listar(0, "", true).OrderBy(x => x.RamoAtividade.descricao).ThenBy(x => x.descricao)
                .Select(x => new {
                    x.id,
                    descricao = x.RamoAtividade.descricao +"/"+ x.descricao
                }).ToList();

            CacheService.getInstance.adicionar(CacheService.SETOR_ATUACAO_DD_SIMPLES, listaItens);

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }        

    }
}