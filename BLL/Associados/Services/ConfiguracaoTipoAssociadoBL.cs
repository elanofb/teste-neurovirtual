using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.Associados {

	public class ConfiguracaoTipoAssociadoBL: DefaultBL, IConfiguracaoTipoAssociadoBL {

		//Constantes
		private static IConfiguracaoTipoAssociadoBL _instance;

        // Atributos
        private readonly string chaveCache = "configuracao_tipo_associado";

		//Propriedades
		public static IConfiguracaoTipoAssociadoBL getInstance => _instance = _instance ?? new ConfiguracaoTipoAssociadoBL();

	    //
		public ConfiguracaoTipoAssociadoBL(){

		}

		/// <summary>
        /// Carregar as configurações de Scripts da organização e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoTipoAssociado carregar(int idTipoAssociado, int idOrganizacaoParam = 0, bool flagCache = true) {

		    if (idOrganizacaoParam == 0){
		        idOrganizacaoParam = User.idOrganizacao();
		    }

		    var cacheData = CacheService.getInstance.carregar<ConfiguracaoTipoAssociado>(chaveCache+idTipoAssociado, idOrganizacaoParam);

		    if (cacheData != null && flagCache) {
		        return cacheData;
		    }

			var query = db.ConfiguracaoTipoAssociado
                            .Include(x => x.Organizacao).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null && x.idTipoAssociado == idTipoAssociado);           

		    query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null);

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? this.carregarPadrao();

            if (flagCache) {
                CacheService.getInstance.adicionar(chaveCache+idTipoAssociado, OConfiguracao, idOrganizacaoParam);
            }

		    return OConfiguracao;
		}

        // Carregamento Padrão
        private ConfiguracaoTipoAssociado carregarPadrao() {

            var OConfig = new ConfiguracaoTipoAssociado();

            OConfig.htmlCarteirinha = "";

            return OConfig;

        }
        
		//Configuracoes gerais
		public IQueryable<ConfiguracaoTipoAssociado> listar(int idTipoAssociado, int idOrganizacao) {

			var query = db.ConfiguracaoTipoAssociado
                            .Include(x => x.Organizacao).Include(x => x.Organizacao.Pessoa).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

            if (idTipoAssociado > 0)
            {
                query = query.Where(x => x.idTipoAssociado == idTipoAssociado);
            }

            if (idOrganizacao > 0)
            {
                query = query.Where(x => x.idOrganizacao == idOrganizacao);
            }

            return query;
		}

		/// <summary>
        /// Salvar configuracoes de Scripts e remover os registros anteriores.
        /// </summary>
		public bool salvar(ConfiguracaoTipoAssociado OConfiguracoes) {
			
			OConfiguracoes.setDefaultInsertValues();

			db.ConfiguracaoTipoAssociado.Add(OConfiguracoes);

			db.SaveChanges();

		    bool flagSucesso = OConfiguracoes.id > 0;

		    int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

		    if (flagSucesso) {

		        db.ConfiguracaoTipoAssociado
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacao && x.idTipoAssociado == OConfiguracoes.idTipoAssociado && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoTipoAssociado { dtExclusao = DateTime.Now });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
		    }

			return (OConfiguracoes.id > 0);

		}
	}
}