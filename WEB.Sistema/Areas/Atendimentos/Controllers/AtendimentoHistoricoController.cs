using System.Web.Mvc;
using System.Linq;
using BLL.Atendimentos;

namespace WEB.Areas.Atendimentos.Controllers {

	public class AtendimentoHistoricoController : Controller {

		//Atributos
		private IAtendimentoHistoricoBL _IAtendimentoHistoricoBL;

		//Propriedades
		private IAtendimentoHistoricoBL OAtendimentoHistoricoBL => _IAtendimentoHistoricoBL = _IAtendimentoHistoricoBL ?? new AtendimentoHistoricoBL();

        [ActionName("partial-historico-atendimento")]
        public PartialViewResult partialHistoricoAtendimento(int idAtendimento) {

            var listaHistorico = this.OAtendimentoHistoricoBL.listar(idAtendimento).OrderByDescending(x => x.id).ToList();

            return PartialView(listaHistorico);

        }

    }
}