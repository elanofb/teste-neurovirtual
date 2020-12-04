using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Instituicoes;
using WEB.Areas.Instituicoes.ViewModels;
using DAL.Instituicoes;
using DAL.Permissao.Security.Extensions;
using PagedList;
using MvcFlashMessages;

namespace WEB.Areas.Instituicoes.Controllers {

	public class InstituicaoController : Controller {

        //Constantes

        //Atributos
        public IInstituicaoBL _InstituicaoBL { get; set; }
        
        //Propriedades
        private IInstituicaoBL OInstituicaoBL { get{ return (this._InstituicaoBL = this._InstituicaoBL ?? new InstituicaoBL() ); } }

		//Listagem para consulta de Instituicoes existentes
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaInstituicao = this.OInstituicaoBL.listar(User.idOrganizacao(), descricao, ativo).OrderBy(x => x.descricao);

			return View(listaInstituicao.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {

			InstituicaoForm ViewModel = new InstituicaoForm();

			ViewModel.Instituicao = this.OInstituicaoBL.carregar(UtilNumber.toInt32(id)) ?? new Instituicao();

			return View(ViewModel);
		}
		
		//
		[HttpPost]
		public ActionResult editar(InstituicaoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OInstituicaoBL.salvar(ViewModel.Instituicao);

		    if (flagSucesso) {

		        this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

			    return RedirectToAction("editar", new { id = ViewModel.Instituicao.id });

		    }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
		}

		//Excluir um ou mais registros
		[HttpPost]
		public ActionResult excluir(int[] id) {

			UtilRetorno Retorno = new UtilRetorno();

            Retorno.flagError = false;

			foreach (int idExclusao in id) { 

				var RetornoItem = this.OInstituicaoBL.excluir(idExclusao);
				
				if (RetornoItem.flagError) { 

					Retorno.flagError = true;

					Retorno.listaErros.Add("Algumas exclusões não puderam ser realizadas, tente novamente.");
				}
			}

            return Json(new { error = Retorno.flagError, message = (Retorno.flagError? string.Join("<br >", Retorno.listaErros): "Os regitros foram removidos com sucesso.") });
		}
	}
}