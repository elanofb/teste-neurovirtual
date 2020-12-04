using System.Linq;
using System.Web.Mvc;
using BLL.Pessoas;
using WEB.App_Infrastructure;

namespace WEB.Areas.Pessoas.Controllers{

    public class PessoaFinanceiroController : BaseSistemaController{

		//Atributos
		private IPessoaVWBL _PessoaBL; 

		//Propriedades
        private IPessoaVWBL OPessoaBL => this._PessoaBL = this._PessoaBL ?? new PessoaVWBL();

        [ActionName("listar-ajax")]
        public ActionResult listarAjax() {

            var lista = this.OPessoaBL.listar("")
                .Select(x => new { value = (x.flagCategoriaPessoa + "#" + x.idPessoa), text = (x.descricaoCategoriaPessoa.ToUpper() + " - " + x.nome.ToUpper())})
                .OrderBy(x => x.text).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}
