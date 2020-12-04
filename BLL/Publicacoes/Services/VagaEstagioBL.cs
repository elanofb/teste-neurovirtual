using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Arquivos;
using DAL.Entities;
using DAL.Publicacoes;


namespace BLL.Publicacoes {

	public class VagaEstagioBL : PublicacaoBL, IPublicacaoBL {

        //Atributos

        //Propriedades

        //Construtor
		public VagaEstagioBL() {
        }

		//Listagem dos comunicados
		public override IQueryable<Noticia> listar(string valorBusca, string ativo = "S", int? idPortal = 0) {
			
			var query = this.listar(TipoNoticiaConst.VAGAESTAGIO, valorBusca, ativo, idPortal);
			
			return query;
		}

		//Salvar ou atualizar um registro
		public override bool salvar(Noticia OPublicacao, HttpPostedFileBase OArquivo) {
			
			OPublicacao.idTipoNoticia = TipoNoticiaConst.VAGAESTAGIO;
			
			bool flagSucesso = this.salvar(OPublicacao);

			
			//if (OArquivo != null && OPublicacao.id > 0) {
			//	List<ThumbDTO> listaThumb = new List<ThumbDTO>();
			//	listaThumb.Add(new ThumbDTO{ folderName="sistema", height = 50, width = 0});
			//	listaThumb.Add(new ThumbDTO{ folderName="box", height = 210, width = 0});
			//	this.OArquivoUploadBL.salvarFoto(OPublicacao.id, EntityTypes.NOTICIA, OArquivo, listaThumb);
			//}
			return flagSucesso;
		}
	}
}