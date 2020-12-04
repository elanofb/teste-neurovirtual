using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Fornecedores;
using BLL.Produtos;
using DAL.Fornecedores;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.Fornecedores.ViewModels;
using DAL.Pessoas;
using MvcFlashMessages;

namespace WEB.Areas.Fornecedores.Controllers {

	
	public class FornecedorCadastroController : Controller {

		//Constantes

		//Atributos
		private IFornecedorConsultaBL _FornecedorConsultaBL;
		private IFornecedorCadastroBL _FornecedorCadastroBL;
	    private IProdutoBL _IProdutoBL;

		//Propriedades
		private IFornecedorConsultaBL OFornecedorConsultaBL => _FornecedorConsultaBL = _FornecedorConsultaBL ?? new FornecedorConsultaBL();
		private IFornecedorCadastroBL OFornecedorCadastroBL => _FornecedorCadastroBL = _FornecedorCadastroBL ?? new FornecedorCadastroBL();
	    private IProdutoBL OProdutoBL => _IProdutoBL = _IProdutoBL ?? new ProdutoBL();

		//Events

		//GET: 
		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new FornecedorForm();

			ViewModel.Fornecedor = this.OFornecedorConsultaBL.carregar(UtilNumber.toInt32(id) ) ?? new Fornecedor();
			
			ViewModel.Fornecedor.Pessoa = ViewModel.Fornecedor.Pessoa ?? new Pessoa();
			ViewModel.Fornecedor.Pessoa.flagTipoPessoa = ViewModel.Fornecedor.Pessoa.flagTipoPessoa ?? "J";

			ViewModel.filtrarEnderecoPrincipal();

		    if (ViewModel.Fornecedor.id > 0) {

                ViewModel.listaProdutos = this.OProdutoBL.listar(0, "", true)
                                              .Where(x => x.idFornecedor == ViewModel.Fornecedor.id)
                                              .OrderByDescending(x => x.id).ToList();

		    }

			return View(ViewModel);
		}

		//POST: 
		[HttpPost]
		public ActionResult editar(FornecedorForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OFornecedorCadastroBL.salvar(ViewModel.Fornecedor);

			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados do fornecedor foram salvos com sucesso."));

                return RedirectToAction("editar", new { id = ViewModel.Fornecedor.id });
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

			return View(ViewModel);
		}

		[HttpGet, ActionName("modal-editar")]
		public ActionResult modalEditar(int? id) {
			
			var ViewModel = new FornecedorForm();
			
			ViewModel.Fornecedor = this.OFornecedorConsultaBL.carregar(UtilNumber.toInt32(id)) ?? new Fornecedor();
			ViewModel.Fornecedor.Pessoa.flagTipoPessoa = ViewModel.Fornecedor.Pessoa.flagTipoPessoa ?? "J";

			ViewModel.Fornecedor.Pessoa = ViewModel.Fornecedor.Pessoa ?? new Pessoa();

			ViewModel.filtrarEnderecoPrincipal();
            
			return PartialView(ViewModel);
		}

		[HttpPost, ActionName("salvar-modal-editar")]
		public ActionResult salvarModalEditar(FornecedorForm ViewModel) {

			if (!ModelState.IsValid) {
				
				ViewModel.Fornecedor.Pessoa = ViewModel.Fornecedor.Pessoa ?? new Pessoa();

				return PartialView("modal-editar", ViewModel);
			}

			ViewModel.Fornecedor.Pessoa.setDataUser(User.id());

			bool flagSucesso = this.OFornecedorCadastroBL.salvar(ViewModel.Fornecedor);

			return Json(new {error = false, flagSucesso, ViewModel.Fornecedor.id, descricao= ViewModel.Fornecedor.Pessoa.nome});
		}

        //
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OFornecedorCadastroBL.alterarStatus(id));
		}

	}
}
