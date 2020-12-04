using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using DAL.Associados.DTO;
using MvcFlashMessages;
using WEB.Areas.Associados.ViewModels;

namespace WEB.Areas.Associados.Controllers {

	public class AssociadoAdmissaoAcaoController : Controller {

		//Atributos
        private IAssociadoBL _IAssociadoBL;
		private IAssociadoAdmissaoBL _IAssociadoAdmissaoBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
		private IAssociadoAdmissaoBL OAssociadoAdmissaoBL => _IAssociadoAdmissaoBL = _IAssociadoAdmissaoBL ?? new AssociadoAdmissaoBL();
        
	    //Abertura do modal para configurar a admissao dos associados
		[HttpPost, ActionName("modal-admitir-associados")]
		public PartialViewResult modalAdmitirAssociados(List<int> idsAssociados) {
            
			var ViewModel = new AssociadoAdmissaoForm();

            ViewModel.idsAssociados = idsAssociados;

            ViewModel.listaAssociados = this.OAssociadoBL.listar(0, "", "", "E")
                                            .Where(x => idsAssociados.Contains(x.id))
                                            .Select(x => new ItemListaAssociado {
                                                id = x.id, nome = x.Pessoa.nome, nroAssociado = x.nroAssociado,
                                                descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
                                            }).OrderBy(x => x.nome).ToList();

			return PartialView(ViewModel);

		}

        [HttpPost, ActionName("admitir-associados")]
        public ActionResult admitirAssociados(AssociadoAdmissaoForm ViewModel) {

            if (!ModelState.IsValid) {

                ViewModel.listaAssociados = this.OAssociadoBL.listar(0, "", "", "E")
                                            .Where(x => ViewModel.idsAssociados.Contains(x.id))
                                            .Select(x => new ItemListaAssociado {
                                                id = x.id, nome = x.Pessoa.nome, nroAssociado = x.nroAssociado,
                                                descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
                                            }).OrderBy(x => x.nome).ToList();

                return View("modal-admitir-associados", ViewModel);    
            }

            var ORetorno = this.OAssociadoAdmissaoBL.admitirAssociados(ViewModel.idsAssociados, ViewModel.dtAdmissao, ViewModel.observacoes);

            if (!ORetorno.flagError) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", ORetorno.listaErros.FirstOrDefault()));
            }

            return Json(new { error = ORetorno.flagError, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

        }

        
	}
}
