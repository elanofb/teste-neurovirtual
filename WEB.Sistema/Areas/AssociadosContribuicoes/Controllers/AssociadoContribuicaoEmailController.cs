using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;

namespace WEB.Areas.AssociadosContribuicoes.Controllers {

	public class AssociadoContribuicaoEmailController : Controller {

		//Constantes

		//Atributos
		private IAssociadoContribuicaoEmailBL _AssociadoContribuicaoEmailBL; 

		//Propriedades
	    private IAssociadoContribuicaoEmailBL OAssociadoContribuicaoEmailBL => (this._AssociadoContribuicaoEmailBL = this._AssociadoContribuicaoEmailBL ?? new AssociadoContribuicaoEmailBL() );

		// Post: AssociadosContribuicoes/AssociadoAnuidadeOperacao/enviar-cobranca
		[HttpPost, ActionName("enviar-email-cobranca-associado")]
		public ActionResult enviarEmailCobrancaAssociado(int idAssociadoContribuicao) {
			
			var Retorno = this.OAssociadoContribuicaoEmailBL.enviarEmailCobranca(idAssociadoContribuicao);

			return Json( new JsonMessage{ error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault() } );
		}
	}
}
