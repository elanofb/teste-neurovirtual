using System.Json;
using System.Web.Mvc;
using BLL.DocumentosDigitais;

namespace WEB.Areas.DocumentosDigitais.Controllers {

    public class DocumentoDigitalOperacaoController : Controller {

        //Constantes

        //Atributos
        private IDocumentoDigitalBL _DocumentoDigitalBL;

        //Propriedades
        private IDocumentoDigitalBL ODocumentoDigitalBL => _DocumentoDigitalBL = _DocumentoDigitalBL ?? new DocumentoDigitalBL();
        
        //
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.ODocumentoDigitalBL.alterarStatus(id));
		}

        //POST
        [HttpPost]
        public ActionResult excluir(int[] id) {
            JsonMessage Retorno = new JsonMessage();

            foreach (int idExclusao in id) {
                bool flagExcluido = this.ODocumentoDigitalBL.excluir(idExclusao);

                if (!flagExcluido) {
                    Retorno.error = true;
                    Retorno.message = "Alguns registros não puderam ser excluídos.";
                }
            }

            return Json(Retorno);
        }
    }
}