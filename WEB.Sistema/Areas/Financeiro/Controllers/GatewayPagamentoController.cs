using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.App_Infrastructure;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class GatewayPagamentoController : BaseSistemaController {

        //Atributos
        private IGatewayPagamentoBL _GatewayPagamentoBL;

        // Propriedades
        private IGatewayPagamentoBL OGatewayPagamentoBL => _GatewayPagamentoBL = _GatewayPagamentoBL ?? new GatewayPagamentoBL();
        
        [ActionName("listar-json")]
        public ActionResult listarJson() {

            var query = this.OGatewayPagamentoBL.listar("", true);

            var lista = query.Select(x => new { value = x.id, text = x.descricao })
                             .Distinct().OrderBy(x => x.text).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        
    }
}