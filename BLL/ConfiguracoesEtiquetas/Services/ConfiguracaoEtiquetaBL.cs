using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesEtiquetas;
using DAL.Permissao.Security.Extensions;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;

namespace BLL.ConfiguracoesEtiquetas {

	public class ConfiguracaoEtiquetaBL : DefaultBL, IConfiguracaoEtiquetaBL {

		//Constantes
		private static IConfiguracaoEtiquetaBL _instance;

        // Atributos
        public static string chaveCache = "configuracao_etiquetas";

		//Propriedades
		public static IConfiguracaoEtiquetaBL getInstance => _instance = _instance ?? new ConfiguracaoEtiquetaBL();
        


		/// <summary>
        /// Carregar as configurações de RedesSociais da organização e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoEtiqueta carregar(int id, int? idOrganizacaoParam = null) {
            
			var query = db.ConfiguracaoEtiqueta
                            .Include(x => x.Organizacao)
                            .Include(x => x.UsuarioCadastro)
                            .Include(x => x.UsuarioExclusao)
							.Where(x => x.dtExclusao == null && x.id == id);

		    if (idOrganizacaoParam.toInt() == 0) {
		        idOrganizacaoParam = idOrganizacao;
		    }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }
            
		    return query.FirstOrDefault();
		}
        
		//Configuracoes gerais
		public IQueryable<ConfiguracaoEtiqueta> listar(int? idOrganizacaoParam = null) {
            
			var query = db.ConfiguracaoEtiqueta
                            .Include(x => x.Organizacao)
                            .Include(x => x.Organizacao.Pessoa)
                            .Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

		    if (idOrganizacaoParam.toInt() == 0) {
		        idOrganizacaoParam = idOrganizacao;
		    }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }
            
    	    return query;
		}

        ///
        public List<ConfiguracaoEtiqueta> listarFromCache(int idOrganizacaoParam, bool flagCache = true) {

            object jsonData = CacheService.getInstance.carregar(chaveCache, idOrganizacaoParam);

            if (jsonData != null && flagCache) {

                var cacheData = JsonConvert.DeserializeObject<List<ConfiguracaoEtiqueta>>(jsonData.ToString());

                return cacheData;
            }

            var query = this.listar(idOrganizacaoParam);

            var listaDb = query.AsNoTracking();

            var listaCampos = listaDb.Select(cam => new {
                cam.id,
                cam.descricao,
                cam.cssCustomizado,
                cam.html,
                cam.height,
                cam.width,
                cam.margPagTop,
                cam.margPagLef,
                cam.margEtiquetaTop,
                cam.margEtiquetaBot,
                cam.margEtiquetaLef,
                cam.margEtiquetaRig,
                cam.qtdeEtiquetasLinha,
                cam.qtdeLinhasPagina,
                cam.flagImpressoraTermica
            }).ToListJsonObject<ConfiguracaoEtiqueta>();

            if (flagCache) {

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam);

                var json = JsonConvert.SerializeObject(listaCampos);

                CacheService.getInstance.adicionar(chaveCache, json, idOrganizacaoParam);
            }

            return listaCampos;
        }

		/// <summary>
        /// Salvar configuracoes de RedesSociais e remover os registros anteriores.
        /// </summary>
		public bool salvar(ConfiguracaoEtiqueta OConfiguracoes) {

		    CacheService.getInstance.remover(chaveCache);

            if (OConfiguracoes.id == 0) {
                return this.inserir(OConfiguracoes);
            }

            return this.atualizar(OConfiguracoes);

		}

        /// <summary>
        /// Inserir um novo registro
        /// </summary>
        private bool inserir(ConfiguracaoEtiqueta OConfiguracao) {
            
            OConfiguracao.setDefaultInsertValues();

            db.ConfiguracaoEtiqueta.Add(OConfiguracao);

            db.SaveChanges();

	        var flagSucesso = OConfiguracao.id > 0;

	        if (flagSucesso) {
		        
		        CacheService.getInstance.remover(chaveCache, OConfiguracao.idOrganizacao.toInt());
	        }

	        return flagSucesso;
	        
        }

        /// <summary>
        /// Atualizar os dados de um registro existente
        /// </summary>
        private bool atualizar(ConfiguracaoEtiqueta OConfiguracao) {

            ConfiguracaoEtiqueta dbRegistro = this.carregar(OConfiguracao.id);
            if (dbRegistro == null) {
                return false;
            }

            var TipoEntry = db.Entry(dbRegistro);

            OConfiguracao.setDefaultUpdateValues();

            TipoEntry.CurrentValues.SetValues(OConfiguracao);

            TipoEntry.State = EntityState.Modified;

            TipoEntry.ignoreFields();

            db.SaveChanges();

	        var flagSucesso = OConfiguracao.id > 0;
	        
	        if (flagSucesso) {
		        
		        CacheService.getInstance.remover(chaveCache, OConfiguracao.idOrganizacao.toInt());
	        }
	        
            return flagSucesso;
        }

        /// <summary>
        /// Excluir um registro
        /// </summary>
        public UtilRetorno excluir(int id) {

            var Registro = this.carregar(id);

            if (Registro == null) {
                return UtilRetorno.newInstance(true, "O registro informado não pode ser removido.");
            }

            Registro.dtExclusao = DateTime.Now;
            Registro.idUsuarioExclusao = User.id();

            this.db.SaveChanges();

            CacheService.getInstance.remover(chaveCache);

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }
	}
}