using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using WEB.Areas.Associados.ViewModels;
using DAL.Associados;
using System.Json;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Extensions;

namespace WEB.Areas.Associados.Controllers {

	public class CategoriaTipoAssociadoController : Controller {

		private ICategoriaTipoAssociadoConsultaBL _ICategoriaTipoAssociadoConsultaBL;
		private ICategoriaTipoAssociadoConsultaBL OCategoriaTipoAssociadoConsultaBL
			=> this._ICategoriaTipoAssociadoConsultaBL = this._ICategoriaTipoAssociadoConsultaBL
			?? new CategoriaTipoAssociadoConsultaBL();

		private ICategoriaTipoAssociadoCadastroBL _ICategoriaTipoAssociadoCadastroBL;
		private ICategoriaTipoAssociadoCadastroBL OCategoriaTipoAssociadoCadastroBL
			=> this._ICategoriaTipoAssociadoCadastroBL = this._ICategoriaTipoAssociadoCadastroBL
			?? new CategoriaTipoAssociadoCadastroBL();

		private ICategoriaTipoAssociadoExclusaoBL _ICategoriaTipoAssociadoExclusaoBL;
		private ICategoriaTipoAssociadoExclusaoBL OCategoriaTipoAssociadoExclusaoBL
			=> this._ICategoriaTipoAssociadoExclusaoBL = this._ICategoriaTipoAssociadoExclusaoBL
			?? new CategoriaTipoAssociadoExclusaoBL();

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            string ativo = UtilRequest.getString("flagAtivo");
            var listaCategoriaTipoAssociado = this.OCategoriaTipoAssociadoConsultaBL
	            .listar(descricao, ativo)
				.Select( x => new {
					x.id
					, x.ativo
					, x.descricao
					, x.dtCadastro
				}).OrderBy(x => x.descricao)
				  .ToPagedListJsonObject<CategoriaTipoAssociado>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());


			return View(listaCategoriaTipoAssociado);
		}


		//
		[HttpGet, OrganizacaoFilter]
		public ActionResult editar(int? id) {
            CategoriaTipoAssociadoForm ViewModel = new CategoriaTipoAssociadoForm();

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

			ViewModel.CategoriaTipoAssociado = this.OCategoriaTipoAssociadoConsultaBL.carregar(UtilNumber.toInt32(id), idOrganizacao) ?? new CategoriaTipoAssociado();

		    ViewModel.CategoriaTipoAssociado.idOrganizacao = idOrganizacao;

			return View(ViewModel);
		}

		//
		[HttpPost, OrganizacaoFilter]
		public ActionResult editar(CategoriaTipoAssociadoForm ViewModel) {

            if (!ModelState.IsValid) {
				return View(ViewModel);
			}

            bool flagSucesso = this.OCategoriaTipoAssociadoCadastroBL.salvar(ViewModel.CategoriaTipoAssociado);

		    if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");

                return RedirectToAction("listar");
		    }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");

			return View(ViewModel);
		}

		//
		[HttpPost, OrganizacaoFilter]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				var RetornoExclusao = this.OCategoriaTipoAssociadoExclusaoBL.excluir(idExclusao);

				if (RetornoExclusao.flagError) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

			return Json(Retorno);
		}
	}
}