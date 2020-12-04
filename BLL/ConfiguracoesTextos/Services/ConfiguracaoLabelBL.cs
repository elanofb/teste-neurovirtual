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

	public class ConfiguracaoLabelBL: DefaultBL, IConfiguracaoLabelBL {

		//Constantes
		private static ConfiguracaoLabelBL _instance;
		
		// Atributos
		private readonly string chaveCacheUnico = "labels_";
		private readonly string chaveCacheLista = "lista_labels";
		
		//Propriedades
		public static ConfiguracaoLabelBL getInstance => _instance = _instance ?? new ConfiguracaoLabelBL();
		
		public IQueryable<ConfiguracaoLabel> query(int? idOrganizacaoParam = null) {

			var query = from E in db.ConfiguracaoLabel
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
		public ConfiguracaoLabel carregar(int id, int idOrganizacaoParam = 0, bool flagCache = true) {

			if (idOrganizacaoParam == 0){
				idOrganizacaoParam = User.idOrganizacao();
			}

			var chaveCacheTexto = String.Concat(chaveCacheUnico, id);
			
			var cacheData = CacheService.getInstance.carregar<ConfiguracaoLabel>(chaveCacheTexto, idOrganizacaoParam);
			
			if (cacheData != null && flagCache) {
				return cacheData;
			}
			
			var query = this.query(idOrganizacaoParam).Where(x => x.id == id);
			
			query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == 0);

			var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault();
			
			if (flagCache) {
				CacheService.getInstance.adicionar(chaveCacheTexto, OConfiguracao, idOrganizacaoParam);
			}

			return OConfiguracao;
		}
		
        //
        public IQueryable<ConfiguracaoLabel> listar(string valorBusca, int? idOrganizacaoParam = null) {

	        if (idOrganizacaoParam.toInt() == 0){
		        idOrganizacaoParam = User.idOrganizacao();
	        }

            var query = from Conf in db.ConfiguracaoLabel.Include(x => x.UsuarioSistema)
						where !Conf.dtExclusao.HasValue
                        select Conf;

	        if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
	        }

            if (!string.IsNullOrEmpty(valorBusca)){
                query = query.Where(x => x.key.Equals(valorBusca) || x.label.Contains(valorBusca));
            }

            return query;
		}
		
		//
		public List<ConfiguracaoLabel> listarFromCache(int idOrganizacaoParam = 0, bool flagCache = true) {

			if (idOrganizacaoParam == 0){
				idOrganizacaoParam = User.idOrganizacao();
			}
	        
			var cacheData = CacheService.getInstance.carregar<List<ConfiguracaoLabel>>(chaveCacheLista, idOrganizacaoParam);

			if (cacheData != null && flagCache) {
				return cacheData;
			}

			var listaTextos = this.listar("", idOrganizacaoParam)
									.Select(x => new {
										x.id,
										x.idOrganizacao,
										x.key,
										x.idIdioma,
										x.label
									}).ToListJsonObject<ConfiguracaoLabel>(); 

			CacheService.getInstance.adicionar(chaveCacheLista, listaTextos, idOrganizacaoParam);

			return listaTextos;
		}

		//
		public bool salvar(ConfiguracaoLabel OConfiguracoes) {

		    OConfiguracoes.UsuarioSistema = null;

            OConfiguracoes.key = OConfiguracoes.key.ToLower();

			if (OConfiguracoes.id > 0) {
				return this.atualizar(OConfiguracoes);
			}
			
            return this.inserir(OConfiguracoes);
            
		}

	    private bool inserir(ConfiguracaoLabel OConfiguracao){

	        OConfiguracao.setDefaultInsertValues();

	        db.ConfiguracaoLabel.Add(OConfiguracao);

	        db.SaveChanges();

		    CacheService.getInstance.limparCacheOrganizacao(User.idOrganizacao(), chaveCacheLista);
		    
	        return OConfiguracao.id > 0;
		    
	    }
		
		//Persistir o objecto e atualizar informações
		private bool atualizar(ConfiguracaoLabel OConfiguracao) { 

			OConfiguracao.setDefaultUpdateValues();

			//Localizar existentes no banco
			ConfiguracaoLabel dbConfiguracaoLabel = this.carregar(OConfiguracao.id, User.idOrganizacao(), false);

			var ConfiguracaoLabelEntry = db.Entry(dbConfiguracaoLabel);
			
			ConfiguracaoLabelEntry.CurrentValues.SetValues(OConfiguracao);
			
			ConfiguracaoLabelEntry.ignoreFields();

			db.SaveChanges();

			return (OConfiguracao.id > 0);

		}

	}
	
}