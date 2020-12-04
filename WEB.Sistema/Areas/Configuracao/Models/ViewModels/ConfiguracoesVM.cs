using DAL.Configuracoes;
using DAL.ConfiguracoesEcommerce;
using DAL.ConfiguracoesRedesSociais;
using FluentValidation.Attributes;

namespace WEB.Areas.Configuracao.ViewModels{

	[Validator(typeof(ConfiguracoesVMValidator))]
	public class ConfiguracoesVM{

		public ConfiguracaoEmail ConfiguracaoEmail { get; set; }

		public ConfiguracaoNotificacao ConfiguracaoNotificacao { get; set; }

		public ConfiguracaoRedesSociais ConfiguracaoRedeSocial { get; set; }

		public ConfiguracaoFinanceiro ConfiguracaoFinanceiro { get; set; }

		public ConfiguracaoContribuicao ConfiguracaoContribuicao { get; set; }

		public ConfiguracaoPortal ConfiguracaoPortal { get; set; }

		public ConfiguracaoEcommerce ConfiguracaoEcommerce { get; set; }
		
		//Construtor
		public ConfiguracoesVM() { 

			this.ConfiguracaoEmail = new ConfiguracaoEmail();

			this.ConfiguracaoNotificacao = new ConfiguracaoNotificacao();

			this.ConfiguracaoRedeSocial = new ConfiguracaoRedesSociais();

			this.ConfiguracaoFinanceiro = new ConfiguracaoFinanceiro();

			this.ConfiguracaoContribuicao = new ConfiguracaoContribuicao();

			this.ConfiguracaoPortal = new ConfiguracaoPortal();

			this.ConfiguracaoEcommerce = new ConfiguracaoEcommerce();
		}
	}

}