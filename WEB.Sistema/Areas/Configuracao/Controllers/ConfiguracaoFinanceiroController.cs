using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Configuracao.ViewModels;
using DAL.Configuracoes;
using BLL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using BLL.Organizacoes;

namespace WEB.Areas.Configuracao.Controllers {

    public class ConfiguracaoFinanceiroController : Controller {

        //Atributos
        private IOrganizacaoBL _OrganizacaoBL;
        private IConfiguracaoFinanceiroBL _ConfiguracaoFinanceiroBL;

        //Propriedades
        private IOrganizacaoBL OOrganizacaoBL => this._OrganizacaoBL = this._OrganizacaoBL ?? new OrganizacaoBL();
        private IConfiguracaoFinanceiroBL OConfiguracaoFinanceiroBL => this._ConfiguracaoFinanceiroBL = this._ConfiguracaoFinanceiroBL ?? new ConfiguracaoFinanceiroBL();


        //
        [HttpGet]
        public ActionResult listar() {

            if (User.idOrganizacao() > 0) {
                return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
            }

            var lista = this.OOrganizacaoBL.listar("", true).ToList();

            return View(lista);
        }

        //
        [HttpGet]
        public ActionResult editar() {
            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            ConfiguracaoFinanceiroForm ViewModel = new ConfiguracaoFinanceiroForm() {
                ConfiguracaoFinanceiro = this.OConfiguracaoFinanceiroBL.carregar(idOrganizacao, false) ?? new ConfiguracaoFinanceiro(),
            };

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoFinanceiroForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (User.idOrganizacao() > 0) {
                ViewModel.ConfiguracaoFinanceiro.idOrganizacao = User.idOrganizacao();
            }

            this.OConfiguracaoFinanceiroBL.salvar(ViewModel.ConfiguracaoFinanceiro);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("editar", new { ViewModel.ConfiguracaoFinanceiro.idOrganizacao });
        }
    }
}