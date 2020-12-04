using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.OrganizacaoConfiguracoes;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.OrganizacaoConfiguracoes {

	public class OrganizacaoDadosAssociadoBL : DefaultBL, IOrganizacaoDadosAssociadoBL {

		//Constantes
		private static IOrganizacaoDadosAssociadoBL _instance;

        // Atributos
        private readonly string chaveCache = "organizacao_dados_associado";

		//Propriedades
		public static IOrganizacaoDadosAssociadoBL getInstance => _instance = _instance ?? new OrganizacaoDadosAssociadoBL();

	    //
		public OrganizacaoDadosAssociadoBL(){

		}

		//
		public OrganizacaoDadosAssociado carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

		    if (idOrganizacaoParam == 0){
			    
		        idOrganizacaoParam = User.idOrganizacao();
		    }

		    var cacheData = CacheService.getInstance.carregar<OrganizacaoDadosAssociado>(chaveCache, idOrganizacaoParam);

		    if (cacheData != null && flagCache) {
		        return cacheData;
		    }

			var query = db.OrganizacaoDadosAssociado.Include(x => x.UsuarioCadastro).Where(x => x.dtExclusao == null);

		    query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null);

            var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault() ?? this.carregarPadrao();

            if (flagCache) {
                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

		    return OConfiguracao;
		}

        // 
        private OrganizacaoDadosAssociado carregarPadrao() {

            var OConfig = new OrganizacaoDadosAssociado();

            OConfig.flagCadastroAssociado = true;

	        OConfig.flagSituacaoAssociado = true;
	        
	        OConfig.flagDadosEndereco = true;
	        
	        OConfig.flagDadosTelefone = true;
	        
	        OConfig.flagDadosEmail = true;
	        
	        OConfig.flagDadosProfissao = true;
	        
	        OConfig.flagDadosEmpresa = true;
	        
	        OConfig.flagDadosResponsavelEmpresa = true;
	        
	        OConfig.flagAreasAtuacao = true;
	        
	        OConfig.flagDadosFuncionario = true;
	        
            return OConfig;

        }
        
		//
		public IQueryable<OrganizacaoDadosAssociado> listar(int idOrganizacaoParam) {

			var query = db.OrganizacaoDadosAssociado.Where(x => x.dtExclusao == null).AsNoTracking();

    	    if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }

    	    return query;
		}

		//
		public bool salvar(OrganizacaoDadosAssociado OOrganizacaoDadosAssociado) {
			
			OOrganizacaoDadosAssociado.setDefaultInsertValues();

			db.OrganizacaoDadosAssociado.Add(OOrganizacaoDadosAssociado);

			db.SaveChanges();

		    bool flagSucesso = OOrganizacaoDadosAssociado.id > 0;

		    int? idOrganizacaoParam = OOrganizacaoDadosAssociado.idOrganizacao;

		    if (flagSucesso) {

		        db.OrganizacaoDadosAssociado
                  .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacaoParam && x.id != OOrganizacaoDadosAssociado.id)
                  .Update(x => new OrganizacaoDadosAssociado { dtExclusao = DateTime.Now });

                CacheService.getInstance.remover(chaveCache, idOrganizacaoParam.toInt());
		    }

			return OOrganizacaoDadosAssociado.id > 0;
		}
		
	}
	
}