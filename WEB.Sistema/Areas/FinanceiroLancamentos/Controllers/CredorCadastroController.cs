using System;
using System.Web.Mvc;
using BLL.Caches;
using BLL.FinanceiroLancamentos;
using WEB.App_Infrastructure;
using DAL.FinanceiroLancamentos;
using DAL.Pessoas;
using MvcFlashMessages;
using WEB.Areas.FinanceiroLancamentos.ViewModels;

namespace WEB.Areas.FinanceiroLancamentos.Controllers{

    public class CredorCadastroController : BaseSistemaController{

        //Atributos
        private ICredorBL _CredorBL;

        //Propriedades
        private ICredorBL OCredorBL => _CredorBL = _CredorBL ?? new CredorBL();
        

        //
        public ActionResult index(int? id){
            
            var ViewModel = new CredorCadastroForm();

            ViewModel.Credor = this.OCredorBL.carregar(id.toInt()) ?? new Credor();
            
            ViewModel.Credor.Pessoa = ViewModel.Credor.Pessoa ?? new Pessoa();

            return View(ViewModel);

        }
        
        //
        [HttpPost, ActionName("salvar-credor")]
        public ActionResult salvarCredor(CredorCadastroForm ViewModel) {

            if (!ModelState.IsValid) {

                ViewModel.Credor.Pessoa = ViewModel.Credor.Pessoa ?? new Pessoa();

                return View("index", ViewModel);

            }

            var flagSucesso = this.OCredorBL.salvar(ViewModel.Credor);

            if (flagSucesso) {

                CacheService.getInstance.remover(CredorVWBL.keyCache);

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados do credor foram salvos com sucesso"));

                return RedirectToAction("index", new { ViewModel.Credor.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve algum problema ao salvar os dados do credor."));

            return View("index", ViewModel);

        }

        [ActionName("modal-cadastrar-credor"), HttpGet]
        public ActionResult modalCadastrarCredor(int? id){

            var ViewModel = new CredorCadastroForm();

            ViewModel.Credor = this.OCredorBL.carregar(UtilNumber.toInt32(id)) ?? new Credor();

            ViewModel.Credor.Pessoa = ViewModel.Credor.Pessoa ?? new Pessoa();

            ViewModel.group = UtilRequest.getString("group");

            return View(ViewModel);
        }

        [ActionName("salvar-modal-credor"), HttpPost]
        public ActionResult salvarModalCredor(CredorCadastroForm ViewModel) {

            if (!ModelState.IsValid) {

                ViewModel.Credor.Pessoa = ViewModel.Credor.Pessoa ?? new Pessoa();

                return PartialView("modal-cadastrar-credor", ViewModel);
            }

            bool flagSucesso = this.OCredorBL.salvar(ViewModel.Credor);

            if (!flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve algum problema ao salvar os dados do credor."));
                return PartialView("modal-cadastrar-credor", ViewModel);
            }

            CacheService.getInstance.remover(CredorVWBL.keyCache);

            var nroDocumento = UtilString.formatCPFCNPJ(ViewModel.Credor.Pessoa.nroDocumento);

            return Json(new {
                error = false,
                flagSucesso = flagSucesso,
                idPessoa = ViewModel.Credor.Pessoa.id,
                id = "CR#" + ViewModel.Credor.Pessoa.id,
                descricao = "Credor - " + ViewModel.Credor.Pessoa.nome + (!nroDocumento.isEmpty() ? " (" + nroDocumento + ")" : ""),
                nroDocumento = ViewModel.Credor.Pessoa.nroDocumento,
                nroTelefone = ViewModel.Credor.Pessoa.nroTelPrincipal,
                group = ViewModel.group
            });
        }
    }
}