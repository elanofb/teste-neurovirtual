using System.Linq;
using System.Web.Mvc;
using BLL.Arquivos;
using WEB.Areas.Associados.ViewModels;
using BLL.Associados;

namespace WEB.Areas.Associados.Controllers {

	public class AssociadoWidgetController : Controller {

		//Atributos
		private IAssociadoBL _AssociadoBL;
		private IMembroSaldoConsultaBL _SaldoConsultaBL;
        private IArquivoUploadFotoBL _IArquivoUploadFotoBL;

		//Propriedades
		private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
		private IMembroSaldoConsultaBL OSaldoConsultaBL => _SaldoConsultaBL = _SaldoConsultaBL ?? new MembroSaldoConsultaBL();
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _IArquivoUploadFotoBL = _IArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

		//Listagem dos associados do sistema
		[ActionName("widget-saldos")]
		public PartialViewResult widgetSaldos() {

			var ViewModel = new ResumoSaldoWidget();

			ViewModel.saldoLinkey = OSaldoConsultaBL.query(1, 1).Select(x => x.saldoAtual).DefaultIfEmpty(0).Sum();
			
			ViewModel.saldoTotal = OSaldoConsultaBL.query(0, 1).Select(x => x.saldoAtual).DefaultIfEmpty(0).Sum();
			
			ViewModel.saldoConsumidores = OSaldoConsultaBL.query(0, 1).Where(x => x.Membro.idTipoCadastro == 1).Select(x => x.saldoAtual).DefaultIfEmpty(0).Sum();
			
			ViewModel.saldoEstabelecimentos = OSaldoConsultaBL.query(0, 1).Where(x => x.Membro.idTipoCadastro == 2).Select(x => x.saldoAtual).DefaultIfEmpty(0).Sum();

			return PartialView(ViewModel);
		}

		//
		[ActionName("widget-resumo-associados")]
		public PartialViewResult widgetResumoAssociados() {
            
			var ViewModel = new WidgetResumoAssociado();

			ViewModel.carregarDados();

			return PartialView("widget-resumo-associados-conteudo", ViewModel);
		}

        //
		[ActionName("widget-resumo-associados-por-periodo")]
		public PartialViewResult widgetResumoAssociadosPorPeriodo() {
            
			var ViewModel = new WidgetResumoAssociadosPorPeriodo();

			ViewModel.carregarDados();
            
			return PartialView("widget-resumo-associados-por-periodo-conteudo", ViewModel);
		}
        
	}
}