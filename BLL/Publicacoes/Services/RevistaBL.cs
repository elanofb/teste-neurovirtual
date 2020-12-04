using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Arquivos;
using DAL.Entities;
using DAL.Publicacoes;


namespace BLL.Publicacoes {

	public class RevistaBL : PublicacaoBL, IPublicacaoBL {

        //Atributos

        //Propriedades

        //Construtor
		public RevistaBL() {

        }

		//Listagem dos comunicados
		public override IQueryable<Noticia> listar(string valorBusca, string ativo = "S", int? idPortal = 0) {
			
			var query = this.listar(TipoNoticiaConst.REVISTA, valorBusca, ativo, idPortal);
			
			return query;
		}

		//Salvar ou atualizar um registro
		public override bool salvar(Noticia OPublicacao, HttpPostedFileBase OFoto) {
			
			OPublicacao.idTipoNoticia = TipoNoticiaConst.REVISTA;
			
			bool flagSucesso = this.salvar(OPublicacao);
			
            if (flagSucesso && OFoto != null) {

                var OArquivo = new ArquivoUpload();

                OArquivo.idReferenciaEntidade = OPublicacao.id;

                OArquivo.entidade = EntityTypes.NOTICIA;

				List<ThumbDTO> listaThumb = new List<ThumbDTO>();

				listaThumb.Add(new ThumbDTO{ folderName="sistema", height = 50, width = 0});

				listaThumb.Add(new ThumbDTO{ folderName="box", height = 210, width = 0});

                this.OArquivoUploadFotoBL.salvar(OArquivo, OFoto, "", listaThumb);

            }

			return flagSucesso;
		}
	}
}