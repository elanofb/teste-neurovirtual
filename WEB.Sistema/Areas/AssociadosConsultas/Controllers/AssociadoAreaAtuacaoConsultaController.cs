using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using WEB.App_Infrastructure;
using WEB.Areas.AssociadosConsultas.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.AssociadosConsultas.Controllers {
	
	[OrganizacaoFilter]
	public class AssociadoAreaAtuacaoConsultaController : BaseSistemaController {
		
		//Atributos
		private IAssociadoAreaAtuacaoVWBL _IAssociadoAreaAtuacaoVWBL;

		//Propriedades
		private IAssociadoAreaAtuacaoVWBL OAssociadoAreaAtuacaoVWBL => _IAssociadoAreaAtuacaoVWBL = _IAssociadoAreaAtuacaoVWBL ?? new AssociadoAreaAtuacaoVWBL();

	    //
		public ActionResult index() {
            
            var ViewModel = new AssociadoAreaAtuacaoConsultaForm();

			return View(ViewModel);

		}

        //
        [HttpGet]
        public ActionResult consultar() {

            var ViewModel = new AssociadoAreaAtuacaoConsultaForm();

            return View("index", ViewModel);
        }

        //
        public ActionResult consultar(AssociadoAreaAtuacaoConsultaForm ViewModel) {

            var query = this.OAssociadoAreaAtuacaoVWBL.listar(ViewModel.idsAreaAtuacao, ViewModel.flagSituacaoContribuicao, ViewModel.valorBusca, ViewModel.ativo);
			
            if (ViewModel.idsTipoAssociado != null) {
                var listaIdsAssociado = ViewModel.idsTipoAssociado.Select(x => (int?) x).ToList();
                query = query.Where(x => listaIdsAssociado.Contains(x.idTipoAssociado));
            }
			
            if (ViewModel.idTipoCadastro > 0) {
                query = query.Where(x => x.idTipoCadastro == ViewModel.idTipoCadastro);
            }
			
            query = query.OrderBy(x => x.nome);

            if(ViewModel.flagTipoSaida == TipoSaidaHelper.EXCEL) {
				
                var OAssociadoConsultaExportacao = new AssociadoAreaAtuacaoConsultaExportacao();
                OAssociadoConsultaExportacao.baixarExcel(query.ToList());
	            
            }
			
            ViewModel.listaResultados = query.ToList();
			
	        ViewModel.carregarResultados();
			
            return View("index", ViewModel);

        }

	}
}
