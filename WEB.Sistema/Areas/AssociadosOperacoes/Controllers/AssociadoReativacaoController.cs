using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosOperacoes;
using DAL.Associados.DTO;
using MvcFlashMessages;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoReativacaoController : Controller {

		//Atributos
        private IAssociadoBL _IAssociadoBL;
		private IAssociadoReativacaoBL _IAssociadoReativacaoBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
		private IAssociadoReativacaoBL OAssociadoReativacaoBL => _IAssociadoReativacaoBL = _IAssociadoReativacaoBL ?? new AssociadoReativacaoBL();
        
	    //Abertura do modal para configurar a reativacao do associado
	    [HttpPost, ActionName("modal-reativar-associados")]
	    public ActionResult modalReativarAssociados(AssociadoFiltroVM DadosConsulta) {

	        var ViewModel = new AssociadoReativacaoForm();
            
	        ViewModel.listaAssociados = DadosConsulta.montarQuery().Where(x => x.ativo == "N")
	                                                 .Select(x => new ItemListaAssociado {
	                                                     id = x.id, nome = x.nome,
	                                                     nroAssociado = x.nroAssociado,
	                                                     descricaoTipoAssociado = x.descricaoTipoAssociado,
	                                                     idPessoa = x.idPessoa
	                                                 }).OrderBy(x => x.nome).ToList();

            if (!ViewModel.listaAssociados.Any()) {

                return Json(new { error = true, message = "Nenhum associado desativado foi encontrado para realizar a reativação." }, JsonRequestBehavior.AllowGet);

            }

	        ViewModel.idsAssociados = ViewModel.listaAssociados.Select(x => x.id).ToList();

	        return PartialView(ViewModel);

	    }

	    //reativacao do associado
	    [HttpPost, ActionName("reativar-associados")]
	    public ActionResult reativarAssociados(AssociadoReativacaoForm ViewModel) {
            
	        if (!ModelState.IsValid) {

	            ViewModel.listaAssociados = this.OAssociadoBL.listar(0, "", "", "N")
	                                            .Where(x => ViewModel.idsAssociados.Contains(x.id))
	                                            .Select(x => new ItemListaAssociado {
	                                                id = x.id, nome = x.Pessoa.nome, nroAssociado = x.nroAssociado,
	                                                descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
	                                            }).OrderBy(x => x.nome).ToList();

	            return View("modal-reativar-associados", ViewModel);    

	        }

	        var ORetorno = this.OAssociadoReativacaoBL.reativarAssociados(ViewModel.idsAssociados, ViewModel.motivoReativacao);

	        if (!ORetorno.flagError) {

	            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", ORetorno.listaErros.FirstOrDefault()));

	            return Json(new { error = false }, JsonRequestBehavior.AllowGet);

	        }

	        return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

	    }
        
	}

}
