using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Organizacoes;
using DAL.Organizacoes;

namespace WEB.Areas.Associacoes.Helpers {

	public class StatusAssociacaoHelper{

		//Atributos
		private static StatusAssociacaoHelper _instance;
		private IStatusOrganizacaoBL _StatusOrganizacaoBL;
        
        //Propriedades
        public static StatusAssociacaoHelper getInstance => _instance = _instance ?? new StatusAssociacaoHelper();
        private IStatusOrganizacaoBL OStatusOrganizacaoBL => this._StatusOrganizacaoBL = this._StatusOrganizacaoBL ?? new StatusOrganizacaoBL();

        //Private Constructor
        private StatusAssociacaoHelper() {
			
		}
        
		/// <summary>
        /// Dropdown com opcao de unidades cadastradas no sistema
        /// </summary>
		public SelectList selectList(int? selected, bool flagCache = true) {

            if (!flagCache) {

                var query = this.OStatusOrganizacaoBL.listar("", true);

                var lista = query.ToList();

                var subList = lista.Select(x => new { value = x.id, text = x.descricao }).ToList();

                return new SelectList(subList, "value", "text", selected);
		    }


		    var OCacheService = CacheService.getInstance;

		    string keyCache = "dd_Status_Associacoes";
             
		    var listaCache = OCacheService.carregar<List<StatusOrganizacao>>(keyCache);

		    if (listaCache == null) {

                var queryUnidades = this.OStatusOrganizacaoBL.listar("", true);
                
                listaCache = queryUnidades.ToList();

                OCacheService.remover(keyCache);

		        OCacheService.adicionar(keyCache, listaCache);
		    }
            
			var list = listaCache.Select(x => new { value = x.id, text = x.descricao }).ToList();

			return new SelectList(list, "value", "text", selected);
		}
    }
}