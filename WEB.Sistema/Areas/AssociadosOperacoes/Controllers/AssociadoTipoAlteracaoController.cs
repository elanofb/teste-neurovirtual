using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosOperacoes;
using DAL.Associados.DTO;
using MvcFlashMessages;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoTipoAlteracaoController : Controller {

		//Atributos
        private IAssociadoBL _IAssociadoBL;
		private IAssociadoTipoAlteracaoBL _IAssociadoTipoAlteracaoBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
		private IAssociadoTipoAlteracaoBL OAssociadoTipoAlteracaoBL => _IAssociadoTipoAlteracaoBL = _IAssociadoTipoAlteracaoBL ?? new AssociadoTipoAlteracaoBL();
        
	    //
	    [HttpPost, ActionName("modal-alterar-tipo")]
	    public ActionResult modalAlterarTipoAssociados(AssociadoFiltroVM DadosConsulta) {

	        var ViewModel = new AssociadoTipoAlteracaoForm();
            
	        ViewModel.listaAssociados = DadosConsulta.montarQuery().Where(x => x.ativo == "S")
	                                                 .Select(x => new ItemListaAssociado {
	                                                     id = x.id, nome = x.nome,
	                                                     nroAssociado = x.nroAssociado,
	                                                     descricaoTipoAssociado = x.descricaoTipoAssociado,
	                                                     idPessoa = x.idPessoa
	                                                 }).OrderBy(x => x.nome).ToList();

            if (!ViewModel.listaAssociados.Any()) {

                return Json(new { error = true, message = "Nenhum associado ativo foi encontrado para alterar o tipo de associado." }, JsonRequestBehavior.AllowGet);

            }

	        ViewModel.idsAssociados = ViewModel.listaAssociados.Select(x => x.id).ToList();

	        return PartialView(ViewModel);

	    }

	    //
	    [HttpPost, ActionName("alterar-tipo")]
	    public ActionResult alterarTipoAssociados(AssociadoTipoAlteracaoForm ViewModel) {
            
	        if (!ModelState.IsValid) {
				
	            ViewModel.listaAssociados = this.OAssociadoBL.listar(0, "", "", "S")
	                                            .Where(x => ViewModel.idsAssociados.Contains(x.id))
	                                            .Select(x => new ItemListaAssociado {
	                                                id = x.id, nome = x.Pessoa.nome, nroAssociado = x.nroAssociado,
	                                                descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
	                                            }).OrderBy(x => x.nome).ToList();
				
	            return View("modal-alterar-tipo", ViewModel);    

	        }
		    		    
			
	        var ORetorno = this.OAssociadoTipoAlteracaoBL.alterarTipo(ViewModel.idsAssociados, ViewModel.idTipoAssociado, ViewModel.motivoAlteracao);
			
	        if (!ORetorno.flagError) {
				
	            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", ORetorno.listaErros.FirstOrDefault()));
					
	            return Json(new { error = false }, JsonRequestBehavior.AllowGet);

	        }

	        return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

	    }
        
	}

}
