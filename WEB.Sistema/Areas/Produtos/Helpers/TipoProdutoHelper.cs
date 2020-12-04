using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;

namespace WEB.Helpers {

	public class TipoProdutoHelper {

        // Atributos
        public static TipoProdutoHelper _instance;
		private ITipoProdutoBL _ITipoProdutoBL;

        // Propriedades
	    public static TipoProdutoHelper getInstance => _instance = _instance ?? new TipoProdutoHelper();
		private ITipoProdutoBL OTipoProdutoBL => _ITipoProdutoBL = _ITipoProdutoBL ?? new TipoProdutoBL();

		//
		public SelectList selectList(int? selected) {

			var list = this.OTipoProdutoBL.listar("", true);

			return new SelectList(list, "id", "descricao", selected);

		}
		
		//
		public MultiSelectList multiSelectList(List<int> selected, List<int> idsExclude = null){
			
			var query = this.OTipoProdutoBL.listar("", true);
			
			if (idsExclude != null) {
				query = query.Where(x => !idsExclude.Contains(x.id));
			}
            
			var listaAreas = query.OrderBy(x => x.id).AsNoTracking().ToList();
            
			return new MultiSelectList(listaAreas, "id", "descricao", selected);

		}

	}
}