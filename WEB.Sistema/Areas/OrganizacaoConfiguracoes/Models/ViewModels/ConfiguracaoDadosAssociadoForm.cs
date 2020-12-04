using DAL.OrganizacaoConfiguracoes;

namespace WEB.Areas.OrganizacaoConfiguracoes.ViewModels{

	public class ConfiguracaoDadosAssociadoForm{

        //Atributos

        //Servicos

        //Propriedades
		public OrganizacaoDadosAssociado OrganizacaoDadosAssociado { get; set; }

        //Construtor
        public ConfiguracaoDadosAssociadoForm() { 

			this.OrganizacaoDadosAssociado = new OrganizacaoDadosAssociado();
        }
    }

}