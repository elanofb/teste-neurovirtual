using System.Collections;
using System.Web.Mvc;
using System.Linq;
using BLL.AreasAtuacao;
using BLL.Caches;

namespace WEB.Areas.AreasAtuacao.Helpers {
	public class AreaAtuacaoHelper {

	    private static AreaAtuacaoHelper _instance;
	    private IAreaAtuacaoBL _AreaAtuacaoBL;

	    public static AreaAtuacaoHelper getInstance => _instance = _instance ?? new AreaAtuacaoHelper();
	    private IAreaAtuacaoBL OAreaAtuacaoBL => _AreaAtuacaoBL = _AreaAtuacaoBL ?? new AreaAtuacaoBL();

        public SelectList selectList(int? selected, bool flagCache = true, int[] excludeItens = null) {

            var listaItens = CacheService.getInstance.carregar(CacheService.AREA_ATUACAO_DD_SIMPLES);

            if (listaItens != null && flagCache) {
                return new SelectList((IEnumerable)listaItens, "id", "descricao", selected);
            }

            var query = OAreaAtuacaoBL.listar("", "S");

            if (excludeItens != null && excludeItens.Length > 0) {
                query = query.Where(x => !excludeItens.Contains(x.id));
            }
            
            listaItens = query.OrderBy(x => x.descricao).ToList()
                            .Select(x => new { x.id, x.descricao}).ToList();

            if (flagCache) {
                CacheService.getInstance.adicionar(CacheService.AREA_ATUACAO_DD_SIMPLES, listaItens);
            }

            return new SelectList((IEnumerable)listaItens, "id", "descricao", selected);
		}

        public MultiSelectList multiSelectList(int[] selected, bool flagCache = true, int[] excludeItens = null) {

            var listaItens = CacheService.getInstance.carregar(CacheService.AREA_ATUACAO_DD_SIMPLES);

            if (listaItens != null && flagCache) {
                return new MultiSelectList((IEnumerable)listaItens, "id", "descricao", selected);
            }

            var query = OAreaAtuacaoBL.listar("", "S");

            if (excludeItens != null && excludeItens.Length > 0) {
                query = query.Where(x => !excludeItens.Contains(x.id));
            }
            
            listaItens = query.OrderBy(x => x.descricao).ToList()
                            .Select(x => new { x.id, x.descricao}).ToList();

            if (flagCache) {
                CacheService.getInstance.adicionar(CacheService.AREA_ATUACAO_DD_SIMPLES, listaItens);
            }

            return new MultiSelectList((IEnumerable)listaItens, "id", "descricao", selected);
		}

	}
}