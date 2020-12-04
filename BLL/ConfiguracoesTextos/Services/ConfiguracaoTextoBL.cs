using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using BLL.Services;
using System.Data.Entity;
using BLL.Caches;
using DAL.ConfiguracoesTextos;
using DAL.Permissao.Security.Extensions;

namespace BLL.ConfiguracoesTextos {

	public class ConfiguracaoTextoBL: DefaultBL, IConfiguracaoTextoBL {

		//Constantes
		private static ConfiguracaoTextoBL _instance;
		
		// Atributos
		private readonly string chaveCacheUnico = "textos_";
		private readonly string chaveCacheLista = "lista_textos";
		
		//Propriedades
		public static ConfiguracaoTextoBL getInstance => _instance = _instance ?? new ConfiguracaoTextoBL();
		
		public IQueryable<ConfiguracaoTexto> query(int? idOrganizacaoParam = null) {

			var query = from E in db.ConfiguracaoTexto
				where !E.dtExclusao.HasValue
				select E;

			if (idOrganizacaoParam == null) {
				idOrganizacaoParam = idOrganizacao;
			}

			if (idOrganizacaoParam > 0) {
				query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
			}

			return query;
		}
		
		/// <summary>
		/// Carregar registro pelo banco de dados sem cache
		/// </summary>
		public ConfiguracaoTexto carregar(int id, int idOrganizacaoParam = 0, bool flagCache = true) {

			if (idOrganizacaoParam == 0){
				idOrganizacaoParam = User.idOrganizacao();
			}

			var chaveCacheTexto = String.Concat(chaveCacheUnico, id);
			
			var cacheData = CacheService.getInstance.carregar<ConfiguracaoTexto>(chaveCacheTexto, idOrganizacaoParam);
			
			if (cacheData != null && flagCache) {
				return cacheData;
			}
			
			var query = db.ConfiguracaoTexto.Where(x => x.id == id);
			
			query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == 0);

			var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault();
			
			if (flagCache) {
				CacheService.getInstance.adicionar(chaveCacheTexto, OConfiguracao, idOrganizacaoParam);
			}

			return OConfiguracao;
		}
		
        //
        public IQueryable<ConfiguracaoTexto> listar(string valorBusca, int? idOrganizacaoParam = null) {

	        if (idOrganizacaoParam.toInt() == 0){
		        idOrganizacaoParam = User.idOrganizacao();
	        }

            var query = from Conf in db.ConfiguracaoTexto.Include(x => x.UsuarioSistema)
                        select Conf;

	        if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
	        }

            if (!string.IsNullOrEmpty(valorBusca)) {
	            query = query.Where(x => x.key.Equals(valorBusca) || x.texto.Contains(valorBusca));
            }

            return query;
		}
		
		//
		public List<ConfiguracaoTexto> listarFromCache(int idOrganizacaoParam = 0, bool flagCache = true) {

			if (idOrganizacaoParam == 0){
				idOrganizacaoParam = User.idOrganizacao();
			}
	        
			var cacheData = CacheService.getInstance.carregar<List<ConfiguracaoTexto>>(chaveCacheLista, idOrganizacaoParam);

			if (cacheData != null && flagCache) {
				return cacheData;
			}

			var listaTextos = this.listar("", idOrganizacaoParam).ToList(); 

			CacheService.getInstance.adicionar(chaveCacheLista, listaTextos, idOrganizacaoParam);

			return listaTextos;
		}

		//
		public bool salvar(ConfiguracaoTexto OConfiguracoes) {

			OConfiguracoes.UsuarioSistema = null;

			OConfiguracoes.key = OConfiguracoes.key.ToLower();

			if (OConfiguracoes.id > 0) {
				return this.atualizar(OConfiguracoes);
			}
			
			return this.inserir(OConfiguracoes);
            
		}

		private bool inserir(ConfiguracaoTexto OConfiguracao){

			OConfiguracao.setDefaultInsertValues();

			db.ConfiguracaoTexto.Add(OConfiguracao);

			db.SaveChanges();

			CacheService.getInstance.limparCacheOrganizacao(User.idOrganizacao(), chaveCacheLista);
		    
			return OConfiguracao.id > 0;
		    
		}
		
		//Persistir o objecto e atualizar informações
		private bool atualizar(ConfiguracaoTexto OConfiguracao) { 

			OConfiguracao.setDefaultUpdateValues();

			//Localizar existentes no banco
			ConfiguracaoTexto dbConfiguracaoTexto = this.carregar(OConfiguracao.id, User.idOrganizacao(), false);

			var ConfiguracaoTextoEntry = db.Entry(dbConfiguracaoTexto);
			
			ConfiguracaoTextoEntry.CurrentValues.SetValues(OConfiguracao);
			
			ConfiguracaoTextoEntry.ignoreFields();

			db.SaveChanges();

			return (OConfiguracao.id > 0);

		}

	}
	
}