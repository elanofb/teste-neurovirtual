using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosOperacoes;
using DAL.Associados.DTO;
using MvcFlashMessages;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoDesativacaoController : Controller {

		//Atributos
        private IAssociadoBL _IAssociadoBL;
		private IAssociadoDesativacaoBL _IAssociadoDesativacaoBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
		private IAssociadoDesativacaoBL OAssociadoDesativacaoBL => _IAssociadoDesativacaoBL = _IAssociadoDesativacaoBL ?? new AssociadoDesativacaoBL();
        
	    //Abertura do modal para configurar a Desativacao do associado
	    [HttpPost, ActionName("modal-desativar-associados")]
	    public ActionResult modalReativarAssociados(AssociadoFiltroVM DadosConsulta) {

	        var ViewModel = new AssociadoDesativacaoForm();
            
	        ViewModel.listaAssociados = DadosConsulta.montarQuery().Where(x => x.ativo == "S")
	                                                 .Select(x => new ItemListaAssociado {
	                                                     id = x.id, nome = x.nome,
	                                                     nroAssociado = x.nroAssociado,
	                                                     descricaoTipoAssociado = x.descricaoTipoAssociado,
	                                                     idPessoa = x.idPessoa
	                                                 }).OrderBy(x => x.nome).ToList();

            if (!ViewModel.listaAssociados.Any()) {

                return Json(new { error = true, message = "Nenhum associado ativo foi encontrado para realizar a desativação." }, JsonRequestBehavior.AllowGet);

            }

	        ViewModel.idsAssociados = ViewModel.listaAssociados.Select(x => x.id).ToList();

	        return PartialView(ViewModel);

	    }

	    //Desativacao do associado
	    [HttpPost, ActionName("desativar-associados")]
	    public ActionResult desativarAssociados(AssociadoDesativacaoForm ViewModel) {
            
	        if (!ModelState.IsValid) {

	            ViewModel.listaAssociados = this.OAssociadoBL.listar(0, "", "", "S")
	                                            .Where(x => ViewModel.idsAssociados.Contains(x.id))
	                                            .Select(x => new ItemListaAssociado {
	                                                id = x.id, nome = x.Pessoa.nome, nroAssociado = x.nroAssociado,
	                                                descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
	                                            }).OrderBy(x => x.nome).ToList();

	            return View("modal-desativar-associados", ViewModel);    

	        }

	        var ORetorno = this.OAssociadoDesativacaoBL.desativarAssociados(ViewModel.idsAssociados, ViewModel.idMotivoDesativacao.toInt(), ViewModel.motivoDesativacao);

	        if (!ORetorno.flagError) {

	            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", ORetorno.listaErros.FirstOrDefault()));

	            return Json(new { error = false }, JsonRequestBehavior.AllowGet);

	        }

	        return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

	    }
        
	}

}
