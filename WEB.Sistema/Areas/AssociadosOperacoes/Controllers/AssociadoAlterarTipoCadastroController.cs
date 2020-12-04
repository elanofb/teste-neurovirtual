using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosOperacoes;
using DAL.Associados;
using DAL.Associados.DTO;
using MvcFlashMessages;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoAlterarTipoCadastroController : Controller {

		//Atributos
        private IAssociadoConsultaBL _IAssociadoBL;
		private IAssociadoAlterarTipoCadastroBL _IAssociadoAlterarTipoCadastroBL;

		//Propriedades
        private IAssociadoConsultaBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoConsultaBL();
		private IAssociadoAlterarTipoCadastroBL OAssociadoAlterarTipoCadastroBL => _IAssociadoAlterarTipoCadastroBL = _IAssociadoAlterarTipoCadastroBL ?? new AssociadoAlterarTipoCadastroBL();
        
	    //Abertura do modal para configurar a exclusão dos associados
		[HttpPost, ActionName("modal-alterar-tipo-cadastro")]
		public ActionResult modalAlterarTipoCadastro(AssociadoFiltroVM DadosConsulta) {
            
			var ViewModel = new AssociadoAlterarTipoCadastroForm();
            
            ViewModel.listaAssociados = DadosConsulta.montarQuery()
                                                     .Select(x => new ItemListaAssociado {
                                                         id = x.id, nome = x.nome,
                                                         nroAssociado = x.nroAssociado,
                                                         descricaoTipoAssociado = x.descricaoTipoAssociado,
                                                         idPessoa = x.idPessoa, ativo = x.ativo,
														 idTipoCadastro = x.idTipoCadastro
                                                     }).OrderBy(x => x.nome).ToList();

		    if (!ViewModel.listaAssociados.Any()) {

		        return Json(new { error = true, message = "Nenhum associado foi encontrado para alterar o tipo de cadastro." }, JsonRequestBehavior.AllowGet);

		    }

		    ViewModel.idsAssociados = ViewModel.listaAssociados.Select(x => x.id).ToList();

			return PartialView(ViewModel);

		}

        //
        [HttpPost, ActionName("alterar-tipo-cadastro")]
        public ActionResult alterarTipoCadastro(AssociadoAlterarTipoCadastroForm ViewModel) {
			
            ViewModel.listaAssociados = this.OAssociadoBL.queryNoFilter(1)
                                            .Where(x => ViewModel.idsAssociados.Contains(x.id))
                                            .Select(x => new ItemListaAssociado {
												id = x.id, nome = x.Pessoa.nome,
												nroAssociado = x.nroAssociado,
												idTipoAssociado = x.idTipoAssociado,
												descricaoTipoAssociado = x.TipoAssociado.descricao,
												idTipoCadastro = x.idTipoCadastro,
												idPessoa = x.idPessoa, ativo = x.ativo
                                            }).OrderBy(x => x.nome).ToList();

            if (!ModelState.IsValid) {
				
                return View("modal-alterar-tipo-cadastro", ViewModel);

            }
	        
	        ViewModel.preencherTipoAssociado();
			
            var ORetorno = this.OAssociadoAlterarTipoCadastroBL.alterarTipoCadastro(ViewModel.listaAssociados, ViewModel.idTipoCadastro, ViewModel.idTipoAssociado);
			
            if (!ORetorno.flagError) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", ORetorno.listaErros.FirstOrDefault()));

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

        }

	}
}
