using System.Web;
using BLL.Configuracoes;

namespace WEB.Areas.Configuracao.ViewModels{

	public class ConfiguracaoImagemForm{

        //Atributos

        //Propriedades

		public HttpPostedFileBase SistemaLogin { get; set; }

        public string urlSistemaLogin { get; set; }

		public HttpPostedFileBase SistemaTopo { get; set; }

        public string urlSistemaTopo { get; set; }

	    public HttpPostedFileBase SistemaRodape { get; set; }

	    public string urlSistemaRodape { get; set; }

        public HttpPostedFileBase SistemaEmail { get; set; }

        public string urlSistemaEmail { get; set; }

		public HttpPostedFileBase SistemaPrint { get; set; }

        public string urlSistemaPrint { get; set; }

		public HttpPostedFileBase AreaAssociadoLogin { get; set; }

        public string urlAreaAssociadoLogin { get; set; }

		public HttpPostedFileBase AreaAssociadoHeader { get; set; }

		public string urlAreaAssociadoHeader { get; set; }

		public HttpPostedFileBase BgLogin { get; set; }

        public string urlBgLogin { get; set; }

        public int idOrganizacao { get; set; }

		
		//Construtor
		public ConfiguracaoImagemForm() { 

		}

        //
	    public void preLoad(int idOrganizacao) {

	        this.urlSistemaLogin = ConfiguracaoImagemBL.linkImagemOrganizacao(idOrganizacao, ConfiguracaoImagemBL.IMAGEM_LOGIN_SISTEMA);

            this.urlSistemaTopo = ConfiguracaoImagemBL.linkImagemOrganizacao(idOrganizacao, ConfiguracaoImagemBL.IMAGEM_TOPO_SISTEMA);

            this.urlSistemaRodape = ConfiguracaoImagemBL.linkImagemOrganizacao(idOrganizacao, ConfiguracaoImagemBL.IMAGEM_RODAPE_SISTEMA);

            this.urlSistemaEmail = ConfiguracaoImagemBL.linkImagemOrganizacao(idOrganizacao, ConfiguracaoImagemBL.IMAGEM_EMAIL_SISTEMA);

            this.urlSistemaPrint = ConfiguracaoImagemBL.linkImagemOrganizacao(idOrganizacao, ConfiguracaoImagemBL.IMAGEM_PRINT_SISTEMA);

            this.urlBgLogin = ConfiguracaoImagemBL.linkImagemOrganizacao(idOrganizacao, ConfiguracaoImagemBL.IMAGEM_BG_LOGIN);

	        this.idOrganizacao = idOrganizacao;
	    }
	}
}