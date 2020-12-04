using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using MoreLinq;
using PagedList;
using WEB.Areas.AssociadosConsultas.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.AssociadosConsultas.Controllers {

	public class AssociadoEmailConsultaController : Controller {

		//Atributos
		private IAssociadoEmailVWBL _IAssociadoEmailVWBL;

		//Propriedades
		private IAssociadoEmailVWBL OAssociadoEmailVWBL => _IAssociadoEmailVWBL = _IAssociadoEmailVWBL ?? new AssociadoEmailVWBL();

	    //
		public ActionResult index() {
            
            var ViewModel = new AssociadoEmailConsultaForm();

			return View(ViewModel);

		}

        //
        [HttpGet]
        public ActionResult consultar() {

            var ViewModel = new AssociadoEmailConsultaForm();

            return View("index", ViewModel);
        }

        //
        public ActionResult consultar(AssociadoEmailConsultaForm ViewModel) {

            var query = this.OAssociadoEmailVWBL.listar(ViewModel.idTipoEmail, ViewModel.flagSituacaoContribuicao, ViewModel.valorBusca, ViewModel.ativo);

            if (ViewModel.idsTipoAssociado != null) {
                var listaIdsAssociado = ViewModel.idsTipoAssociado.Select(x => (int?) x).ToList();
                query = query.Where(x => listaIdsAssociado.Contains(x.idTipoAssociado));
            }

            if (ViewModel.idTipoCadastro > 0) {
                query = query.Where(x => x.idTipoCadastro == ViewModel.idTipoCadastro);
            }

            query = query.OrderBy(x => x.nome);

            if(ViewModel.flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OAssociadoConsultaExportacao = new AssociadoEmailConsultaExportacao();
                OAssociadoConsultaExportacao.baixarExcel(query.DistinctBy(x => new { x.nome, x.email, x.idTipoEmail }).ToList());
            }

            ViewModel.listaEmails = query.DistinctBy(x => new { x.nome, x.email, x.idTipoEmail }).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View("index", ViewModel);

        }

	}
}
