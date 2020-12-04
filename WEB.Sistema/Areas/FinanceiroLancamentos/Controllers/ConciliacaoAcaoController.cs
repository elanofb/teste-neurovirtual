using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using MvcFlashMessages;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers {

    public class ConciliacaoAcaoController : Controller {

	    // Atrbiutos Serviços
	    private IReceitasDespesasVWBL _ReceitasDespesasVWBL;
	    private IConciliacaoFinanceiraCadastroBL _ConciliacaoFinanceiraCadastroBL;
	    private IConciliacaoFinanceiraConsultaBL _ConciliacaoFinanceiraConsultaBL;
        
	    // Propriedades Serviços
	    private IReceitasDespesasVWBL OReceitasDespesasVWBL => _ReceitasDespesasVWBL = _ReceitasDespesasVWBL ?? new ReceitasDespesasVWBL();
	    private IConciliacaoFinanceiraCadastroBL OConciliacaoFinanceiraCadastroBL => _ConciliacaoFinanceiraCadastroBL = _ConciliacaoFinanceiraCadastroBL ?? new ConciliacaoFinanceiraCadastroBL();
	    private IConciliacaoFinanceiraConsultaBL OConciliacaoFinanceiraConsultaBL => _ConciliacaoFinanceiraConsultaBL = _ConciliacaoFinanceiraConsultaBL ?? new ConciliacaoFinanceiraConsultaBL();

        //
	    [HttpPost, ActionName("modal-conciliar")]
	    public ActionResult modalConciliar() {
		    
		    var viewModel = new ConciliacaoAcaoForm();
		    viewModel.idsLancamentos = UtilRequest.getListInt("idsLancamentos");
		    viewModel.tiposLancamentos = UtilRequest.getListString("tiposLancamentos");
		    
		    viewModel.listaLancamentos = OReceitasDespesasVWBL.listar().Where(x => viewModel.idsLancamentos.Contains(x.idPagamento)).ToList();

		    if (!viewModel.listaLancamentos.Any()) {

			    return Json(new { error = true, message = "Nenhum lançamento foi encontrado para ser conciliado." }, JsonRequestBehavior.AllowGet);

		    }

		    return PartialView(viewModel);

	    }
	    
	    [HttpPost, ActionName("modal-lista-detalhe")]
	    public ActionResult modalListaDetalhe(int id) {
		    
		    var ViewModel = new ConciliacaoDetalheVM();

		    ViewModel.idConciliacaoFinanceita = id;
		    
		    ViewModel.carregarInformacoes();
		    
		    if (ViewModel.OConciliacaoFinanceira == null) {
			    return Json(new { error = true, message = "Conciliação não encontrada." }, JsonRequestBehavior.AllowGet);
		    }

		    return PartialView(ViewModel);

	    }

	    //
	    [HttpPost, ActionName("realizar-conciliacao")]
	    public ActionResult realizarConciliacao(ConciliacaoAcaoForm ViewModel) {
            
		    if (!ModelState.IsValid) {

			    ViewModel.listaLancamentos = OReceitasDespesasVWBL.listar().Where(x => ViewModel.idsLancamentos.Contains(x.idPagamento)).ToList();

			    return View("modal-conciliar", ViewModel);    

		    }

		    var OAcaoVM = new ConciliacaoAcaoVM();
		    var listaConciliacao = new List<ConciliacaoFinanceira>();
		    
		    if (ViewModel.flagAgrupar)
		    {
			    listaConciliacao = OAcaoVM.gerarListaConciliacaoAgrupada(ViewModel);   
		    }else{
			    listaConciliacao = OAcaoVM.gerarListaConciliacao(ViewModel);   			    
		    }

		    foreach (var OConciliacao in listaConciliacao)
		    {
			    this.OConciliacaoFinanceiraCadastroBL.salvar(OConciliacao);			    
		    }


		    return Json(new { error = false, message = "Lançamentos conciliados com sucesso!"}, JsonRequestBehavior.AllowGet);		
	    }
	    
	    //
	    [HttpPost, ActionName("excluir-conciliacao")]
	    public ActionResult excluirConciliacao(List<int> idsConciliacoes) {
            
		    foreach (var idConciliacao in idsConciliacoes)
		    {
			    var OConciliacao = OConciliacaoFinanceiraConsultaBL.carregar(idConciliacao);
			    if(OConciliacao.id > 0) this.OConciliacaoFinanceiraCadastroBL.excluir(OConciliacao);			    
		    }

		    this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Exclusão realizada com sucesso!"));
		    return Json(new { error = false }, JsonRequestBehavior.AllowGet);		
	    }
    }
}