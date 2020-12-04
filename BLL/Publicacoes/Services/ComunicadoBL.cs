using System.Linq;
using System.Web;
using DAL.Publicacoes;


namespace BLL.Publicacoes {

	public class ComunicadoBL : PublicacaoBL, IPublicacaoBL {

        //Atributos

        //Propriedades

        //Construtor
		public ComunicadoBL() {
        }

		//Listagem dos comunicados
		public override IQueryable<Noticia> listar(string valorBusca, string ativo = "S", int? idPortal = 0) {
			
			var query = this.listar(TipoNoticiaConst.COMUNICADO, valorBusca, ativo, idPortal);
			
			return query;
		}

		//Salvar ou atualizar um registro
		public override bool salvar(Noticia OPublicacao, HttpPostedFileBase OArquivo) {
			
			OPublicacao.idTipoNoticia = TipoNoticiaConst.COMUNICADO;

			bool flagSucesso = this.salvar(OPublicacao);

			//Comunicados nao possuem fotos
            //if (OArquivo != null && OPublicacao.id > 0) {
            //    this.OArquivoUploadBL.salvarFoto(OPublicacao.id, EntityTypes.NOTICIA, OArquivo, listaThumb);
            //}
			return flagSucesso;
		}
	}
}