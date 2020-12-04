using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Organizacoes;
using BLL.Services;
using DAL.Organizacoes;
using DAL.Permissao;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Associacoes.Helpers {

	public class AssociacaoHelper {

		//Atributos
		private static AssociacaoHelper _instance;
		private IOrganizacaoBL _OrganizacaoBL;
        
        //Propriedades
        public static AssociacaoHelper getInstance => _instance = _instance ?? new AssociacaoHelper();
        private IOrganizacaoBL OOrganizacaoBL => this._OrganizacaoBL = this._OrganizacaoBL ?? new OrganizacaoBL();

        //Private Constructor
        private AssociacaoHelper() {
			
		}
        
		/// <summary>
        /// Dropdown com opcao de unidades cadastradas no sistema
        /// </summary>
		public SelectList selectList(int? selected, bool flagCache = true, bool flagTodasOrganizacaos = false) {

            var idsOrganizacoes = HttpContext.Current.User.idsOrganizacoes();

            if (!flagCache) {

                var query = this.OOrganizacaoBL.listar("", true, flagTodasOrganizacaos);

                if (HttpContext.Current.User.flagMultiOrganizacao() && HttpContext.Current.User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR) {
	                
	                query = query.Where(x => idsOrganizacoes.Contains(x.id));
                }

                var subList = query.Select(x => new { value = x.id, text = x.Pessoa.nome }).OrderBy(x => x.text).ToList();

                return new SelectList(subList, "value", "text", selected);
		    }


		    var OCacheService = CacheService.getInstance;

		    string keyCache = "dd_Associacoes";
             
		    var listaCache = OCacheService.carregar<List<Organizacao>>(keyCache);

		    if (listaCache == null) {

                var queryUnidades = this.OOrganizacaoBL.listar("", true, flagTodasOrganizacaos);
                
                listaCache = queryUnidades.Select(x => new { x.id, Pessoa = new { x.Pessoa.nome } }).ToListJsonObject<Organizacao>();

                OCacheService.remover(keyCache);

		        OCacheService.adicionar(keyCache, listaCache);
		    }

			if (HttpContext.Current.User.flagMultiOrganizacao()) {
		        listaCache = listaCache.Where(x => idsOrganizacoes.Contains(x.id)).ToList();
		    }			

			var list = listaCache.Select(x => new { value = x.id, text = x.Pessoa.nome }).OrderBy(x => x.text).ToList();

			return new SelectList(list, "value", "text", selected);
		}

        /// <summary>
        /// Dropdown com opção de organizações gestoras
        /// </summary>
        public SelectList selectListOrganizacaoGestora(int? selected, bool? ativo, int idOrganizacaoIgnore) {

            var query = this.OOrganizacaoBL.listar("", ativo).Where(x => x.idOrganizacaoGestora == null && x.id != idOrganizacaoIgnore);

            var lista = query.ToList();

            var subList = lista.Select(x => new { value = x.id, text = x.Pessoa.nome }).OrderBy(x => x.text).ToList();

            return new SelectList(subList, "value", "text", selected);
        }
    }
}