using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Arquivos;
using BLL.Services;
using DAL.Arquivos;

namespace WEB.Areas.Arquivos.Controllers {

	public class ArquivoLogotipoController : Controller {
        
		//Atributos
		private ArquivoUploadPadraoBL _IArquivoUploadBL; 

		//Propriedades
		protected ArquivoUploadPadraoBL OArquivoUploadBL => _IArquivoUploadBL = _IArquivoUploadBL ?? new ArquivoUploadLogotipoBL();
		
		//Listagem de arquivos em formato imagem 
		[ActionName("partial-lista-logotipo")]
		public PartialViewResult listarLogotipo(int idReferencia, string entidade, string tipoExibicao = "tabela") {

            var viewName = "partial-lista-logotipo-" + tipoExibicao;

            if(idReferencia == 0) {
	            
                return PartialView(viewName, new List<ArquivoUpload>());
	            
            }

            var listaArquivos = this.OArquivoUploadBL.listar(idReferencia, entidade, "")
	            										.Select( x => new {
															x.id, 
															x.idOrganizacao,
															x.ordem,
															x.categoria,
															x.entidade,
															x.contentType,
															x.legenda,
															x.extensao,
															x.nomeArquivo,
															x.idReferenciaEntidade,
															x.titulo,
															x.path,
															x.pathThumb,
															x.dtCadastro,
		            										x.ativo
		            
														}).ToListJsonObject<ArquivoUpload>()
														.OrderBy(x => x.ordem)
														.ThenByDescending(x => x.id)
														.ToList();

			return PartialView(viewName, listaArquivos);
		}

		//
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OArquivoUploadBL.alterarStatus(id), JsonRequestBehavior.AllowGet);
		}
        
		//
		[HttpPost]
		public ActionResult excluir(int id) {
			return Json(this.OArquivoUploadBL.excluir(id), JsonRequestBehavior.AllowGet);
		}
	}

}
