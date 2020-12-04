using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web;
using DAL.Arquivos;

namespace BLL.Arquivos {

	public class ArquivoUploadFotoBL : ArquivoUploadPadraoBL, IArquivoUploadFotoBL {
        
		//Construtor
		public ArquivoUploadFotoBL() : base(ArquivoUploadTypes.FOTO) {

		}

	    // Carregar foto principal
	    public ArquivoUpload carregarPrincipal(int idReferencia, string entidade, int? idOrganizacaoParam = null) {
			
	        var query = this.listar(idReferencia, entidade, "S", idOrganizacaoParam);

	        return query.OrderByDescending(x => x.id).FirstOrDefault(x => x.flagPrincipal == "S") ?? query.OrderByDescending(x => x.id).FirstOrDefault();

	    }

        //
	    public override bool salvar(ArquivoUpload OArquivo, HttpPostedFileBase FileUpload = null, string pathUpload = "", List<ThumbDTO> listaThumb = null) {

	        if (OArquivo.ordem == 0) { 

	            var ordem = this.listar(OArquivo.idReferenciaEntidade, OArquivo.entidade, "S").Count() + 1;

	            OArquivo.ordem = ordem.toByte();

	        }

            return base.salvar(OArquivo, FileUpload, pathUpload, listaThumb);

        }

        // Registrar flagPrincipal
        public JsonMessageStatus registrarFotoPrincipal(int id) {

            var ORetorno = new JsonMessageStatus();

            var OArquivo = this.carregar(id);

            if (OArquivo == null) {

                ORetorno.error = true;
                ORetorno.message = "O arquivo informado não foi encontrado;";
                return ORetorno;

            }

            var listaArquivos = this.listar(OArquivo.idReferenciaEntidade, OArquivo.entidade, "")
                                    .Where(x => x.id != OArquivo.id).ToList();

            listaArquivos.ForEach(x => { x.flagPrincipal = "N"; });

            OArquivo.flagPrincipal = "S";

            db.SaveChanges();

            ORetorno.error = false;
            return ORetorno;

        }

        //
	    public void reordenarExibicao(int id, byte ordem) {

	        var OArquivo = this.carregar(id);

	        if (OArquivo == null) {
                return;
	        }

	        var listaArquivos = this.listar(OArquivo.idReferenciaEntidade, OArquivo.entidade, "")
	                                                .OrderBy(x => x.ordem).ToList();

	        byte cont = 1;

	        listaArquivos.ForEach(x => {

	            if (x.id == id) {

	                x.ordem = ordem;

	            } else {

	                if (cont == ordem) { 
	                    cont++;
	                }

	                x.ordem = cont;
	                cont++;

	            }

	        });

	        db.SaveChanges();

	    }

        //
	    public override JsonMessageStatus excluir(int id) {

	        var ORetorno = new JsonMessageStatus();

	        var OArquivo = this.carregar(id);

	        if (OArquivo == null) {

	            ORetorno.error = true;
	            ORetorno.message = "O arquivo informado não foi encontrado;";
	            return ORetorno;

	        }

            ORetorno = base.excluir(id);

            if (ORetorno.error) {
                return ORetorno;
            }

	        var listaArquivos = this.listar(OArquivo.idReferenciaEntidade, OArquivo.entidade, "")
                                    .Where(x => x.id != id).OrderBy(x => x.ordem).ToList();

	        byte cont = 1;

	        listaArquivos.ForEach(x => {
                    
	            x.ordem = cont;
	            cont++;

	        });

	        db.SaveChanges();

	        ORetorno.error = false;

            return ORetorno;

        }
        
	}
}