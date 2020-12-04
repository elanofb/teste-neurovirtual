using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosOperacoes;
using DAL.Associados.DTO;
using MvcFlashMessages;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoAdmissaoAlteracaoController : Controller {

		//Atributos
        private IAssociadoBL _IAssociadoBL;
		private IAssociadoAdmissaoAlteracaoBL _IAssociadoAdmissaoAlteracaoBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
		private IAssociadoAdmissaoAlteracaoBL OAssociadoAdmissaoAlteracaoBL => _IAssociadoAdmissaoAlteracaoBL = _IAssociadoAdmissaoAlteracaoBL ?? new AssociadoAdmissaoAlteracaoBL();
        
	    //
	    [HttpPost, ActionName("modal-alterar-admissao")]
	    public ActionResult modalReativarAssociados(AssociadoFiltroVM DadosConsulta) {

	        var ViewModel = new AssociadoAdmissaoAlteracaoForm();
            
	        ViewModel.listaAssociados = DadosConsulta.montarQuery().Where(x => x.ativo == "S")
	                                                 .Select(x => new ItemListaAssociado {
	                                                     id = x.id, nome = x.nome,
	                                                     nroAssociado = x.nroAssociado,
	                                                     descricaoTipoAssociado = x.descricaoTipoAssociado,
	                                                     idPessoa = x.idPessoa
	                                                 }).OrderBy(x => x.nome).ToList();

            if (!ViewModel.listaAssociados.Any()) {

                return Json(new { error = true, message = "Nenhum associado ativo foi encontrado para alterar a admissão." }, JsonRequestBehavior.AllowGet);

            }

	        ViewModel.idsAssociados = ViewModel.listaAssociados.Select(x => x.id).ToList();

	        return PartialView(ViewModel);

	    }

	    //
	    [HttpPost, ActionName("alterar-admissao")]
	    public ActionResult desativarAssociados(AssociadoAdmissaoAlteracaoForm ViewModel) {
            
	        if (!ModelState.IsValid) {

	            ViewModel.listaAssociados = this.OAssociadoBL.listar(0, "", "", "S")
	                                            .Where(x => ViewModel.idsAssociados.Contains(x.id))
	                                            .Select(x => new ItemListaAssociado {
	                                                id = x.id, nome = x.Pessoa.nome, nroAssociado = x.nroAssociado,
	                                                descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
	                                            }).OrderBy(x => x.nome).ToList();

	            return View("modal-alterar-admissao", ViewModel);    

	        }

	        var ORetorno = this.OAssociadoAdmissaoAlteracaoBL.alterarAdmissao(ViewModel.idsAssociados, ViewModel.dtAdmissao.Value, ViewModel.motivoAlteracao);

	        if (!ORetorno.flagError) {

	            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", ORetorno.listaErros.FirstOrDefault()));

	            return Json(new { error = false }, JsonRequestBehavior.AllowGet);

	        }

	        return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

	    }
        
	}

}
