using System;
using System.Data.Entity;
using System.Linq;
using BLL.Caches;
using BLL.Services;
using DAL.ConfiguracoesRecibo;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.ConfiguracoesRecibo {

	public class ConfiguracaoReciboBL: DefaultBL, IConfiguracaoReciboBL {

		//Constantes
		private static IConfiguracaoReciboBL _instance;

        // Atributos
        private readonly string chaveCache = "configuracao_recibo";

		//Propriedades
		public static IConfiguracaoReciboBL getInstance => _instance = _instance ?? new ConfiguracaoReciboBL();

	    //
		public ConfiguracaoReciboBL(){

		}

		/// <summary>
        /// Carregar as configurações de Recibo da organização e cachear os dados se necessario
        /// </summary>
		public ConfiguracaoRecibo carregar(int idOrganizacaoParam = 0, bool flagCache = true) {

		    if (idOrganizacaoParam == 0){
		        idOrganizacaoParam = User.idOrganizacao();
		    }

		    var cacheData = CacheService.getInstance.carregar<ConfiguracaoRecibo>(chaveCache, idOrganizacaoParam);

		    if (cacheData != null && flagCache) {
		        return cacheData;
		    }

			var query = db.ConfiguracaoRecibo
                            .Include(x => x.Organizacao)
                            .Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null)
                            .AsNoTracking();

		    query = idOrganizacaoParam > 0 ? query.Where(x => x.idOrganizacao == idOrganizacaoParam) : query.Where(x => x.idOrganizacao == null) ;

		    var OConfiguracao = query.OrderByDescending(x => x.id).FirstOrDefault(x => !string.IsNullOrEmpty(x.htmlRecibo)) ?? this.carregarPadrao();

            if (flagCache) {
                CacheService.getInstance.adicionar(chaveCache, OConfiguracao, idOrganizacaoParam);
            }

		    return OConfiguracao;
		}

        // Carregamento Padrão
        private ConfiguracaoRecibo carregarPadrao() {

            var OConfig = new ConfiguracaoRecibo();

            OConfig.htmlRecibo = ""
                                +"<div id=\"container\">"
                                +"	<div class=\"col-md-12 header-recibo\">"
                                +"		<div class=\"col-md-2 logo-recibo\">"
                                +"			<img src=\"#LINK_LOGO#\" alt=\"#NOME_ORGANIZACAO#\" />"
                                +"		</div>"
                                +"		<div class=\"col-md-9 text-right\">"
                                +"			<h4 class=\"nome-associacao\">#NOME_ORGANIZACAO#</h4>"
                                +"		</div>"
                                +"		<div class=\"clearfix\"></div>"
                                +"		<br />"
                                +"		<div class=\"col-md-12\">"
                                +"			<p class=\"w500 pull-left\">#ENDERECO_CABECALHO#</p>"
                                +"			<p class=\"w200 pull-right text-right\">#TEL_1_CABECALHO#</p>"
                                +"				<div class=\"clearfix\"></div>"
                                +"		</div>"
                                +"				<div class=\"clearfix\"></div>"
                                +"		<div class=\"col-md-12\">"
                                +"			<p class=\"w500 pull-left\">#UF_CIDADE_CABECALHO#</p>"
                                +"			<p class=\"w200 pull-right text-right\">#TEL_2_CABECALHO#</p>"
                                +"				<div class=\"clearfix\"></div>"
                                +"		</div>"
                                +"				<div class=\"clearfix\"></div>"
                                +"	</div>"
                                +"	<div class=\"clearfix\"></div>"
                                +"	<br />"
                                +"	<div id=\"content\" class=\"corpo-recibo\">"

                                +"		<div class=\"col-md-5 pull-left\">"
                                +"			<label class=\"recibo-label-nro\">RECIBO Nº <span class=\"recibo-nro\">#NUMERO#</span></label>"
                                +"		</div>"
                                +"		<div class=\"col-md-5 pull-right\">"
                                +"			<div class=\"col-md-12\">"
                                +"				<div>"
                                +"					<p class=\"bg-destaque\" style=\"padding-left:5px;font-size:22px;\"> #VALOR#</p>"
                                +"				</div>"
                                +"			</div>"
                                +"		</div>"
                                +"		<div class=\"clearfix\"></div><br />"
                                +""
                                +"		<div class=\"col-md-12\">"
                                +"			<table class=\"dados-recibo\" style=\"width:100%\">"
                                +"				<tr>"
                                +"					<td class=\"text-right w125\">Recebemos de:</td>"
                                +"					<td class=\"text-left\"><p class=\"bg-destaque\">#NOME#</p></td>"
                                +"				</tr>"
                                +"				<tr>"
                                +"					<td class=\"text-right w125\">A quantia de:</td>"
                                +"					<td class=\"text-left\"><p class=\"bg-destaque\">#VALOR#</p></td>"
                                +"				</tr>"
                                +"				<tr>"
                                +"					<td class=\"text-right w125\">Referente à:</td>"
                                +"					<td class=\"text-left\">"
                                +"						<p class=\"bg-destaque\">"
                                +"							<strong>#DESCRICAO#</strong>"
                                +"						</p>"
                                +"					</td>"
                                +"				</tr>"
                                +"			</table>"
                                +"			<div class=\"clearfix\"></div>"
                                +"		</div>"
                                +"		<div class=\"clearfix\"></div>"
                                +"		<br />"
                                +""
                                +"		<table class=\"col-md-8 pull-right\">"
                                +"			<tr>"
                                +"				<td class=\"text-right\">"
                                +"					São Paulo, #DATA#."
                                +"				</td>"
                                +"			</tr>"
                                +"			<tr>"
                                +"				<td class=\"text-center\" style=\"border-bottom:solid 1px #000000; padding-top:20px;\">"
                                +"					<strong>#ASSINATURA#</strong>"
                                +"				</td>"
                                +"			</tr>"
                                +"			<tr>"
                                +"				<td class=\"text-center\">Identificador Eletrônico</td>"
                                +"			</tr>"
                                +"		</table>"
                                +"		<div class=\"clearfix\"></div>"
                                +"		<br />"
                                +""
                                +"	</div>"
                                +"</div>";

            return OConfig;

        }
        
		//Configuracoes gerais
		public IQueryable<ConfiguracaoRecibo> listar(int idOrganizacaoParam) {

		    if (User.idOrganizacao() > 0){
		        idOrganizacaoParam = User.idOrganizacao();
		    }

			var query = db.ConfiguracaoRecibo
                            .Include(x => x.Organizacao).Include(x => x.Organizacao.Pessoa).Include(x => x.UsuarioCadastro)
							.Where(x => x.dtExclusao == null).AsNoTracking();

    	    if (idOrganizacaoParam > 0) {
		        query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }

    	    return query;
		}

		/// <summary>
        /// Salvar configuracoes de Recibo e remover  os registros anteriores.
        /// </summary>
		public bool salvar(ConfiguracaoRecibo OConfiguracoes) {
			
			OConfiguracoes.setDefaultInsertValues();

			db.ConfiguracaoRecibo.Add(OConfiguracoes);

			db.SaveChanges();

		    bool flagSucesso = OConfiguracoes.id > 0;

		    int? idOrganizacaoParam = OConfiguracoes.idOrganizacao;

            if (User.idOrganizacao() > 0){
		        idOrganizacaoParam = User.idOrganizacao();
		    }

		    if (flagSucesso) {

		        db.ConfiguracaoRecibo
                    .Where(x => x.dtExclusao == null && x.idOrganizacao == idOrganizacaoParam && x.id != OConfiguracoes.id)
                    .Update(x => new ConfiguracaoRecibo { dtExclusao = DateTime.Now });

                CacheService.getInstance.remover("configuracao_recibo", idOrganizacaoParam.toInt());
		    }

			return (OConfiguracoes.id > 0);

		}
	}
}