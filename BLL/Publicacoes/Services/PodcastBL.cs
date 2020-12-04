using System;
using System.Linq;
using System.Web;
using BLL.Arquivos;
using DAL.Entities;
using DAL.Publicacoes;


namespace BLL.Publicacoes {

	public class PodcastBL : PublicacaoBL, IPublicacaoBL {

        //Atributos
        private IArquivoUploadBL _IArquivoUploadBL;

        //Propriedades
	    private IArquivoUploadBL OArquivoUploadBL => _IArquivoUploadBL = _IArquivoUploadBL ?? new ArquivoUploadBL();

        //Construtor
		public PodcastBL() {
        }

		//Listagem dos comunicados
		public override IQueryable<Noticia> listar(string valorBusca, string ativo = "S", int? idPortal = 0) {
			
			var query = this.listar(TipoNoticiaConst.PODCAST, valorBusca, ativo, idPortal);
			
            query = query.condicoesSeguranca();

			return query;
		}

		//Salvar ou atualizar um registro
		public override bool salvar(Noticia OPublicacao, HttpPostedFileBase OFoto) {
			
			OPublicacao.idTipoNoticia = TipoNoticiaConst.PODCAST;
			
			bool flagSucesso = this.salvar(OPublicacao);

            if (flagSucesso && OFoto != null) {
                this.OArquivoUploadBL.salvarAudio(OPublicacao.id, EntityTypes.PODCAST, "", OFoto);
            }

			return flagSucesso;
		}
	}
}