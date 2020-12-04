using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels {

    public class ConfiguracaoCadastroVM {

        //Atributos

        //Servicos

        //Propriedades
        public ConfiguracaoAssociadoPF ConfiguracaoAssociadoPF { get; set; }

        public ConfiguracaoAssociadoPJ ConfiguracaoAssociadoPJ { get; set; }

        //Construtor
        public ConfiguracaoCadastroVM() {

        }

        /// <summary>
        /// Carregar os dados de configuracao
        /// </summary>
        public void carregarDados(int idOrganizacao) {

            this.ConfiguracaoAssociadoPF = ConfiguracaoAssociadoPFBL.getInstance.carregar(idOrganizacao, false);

            this.ConfiguracaoAssociadoPJ = ConfiguracaoAssociadoPJBL.getInstance.carregar(idOrganizacao, false);

        }
    }
}