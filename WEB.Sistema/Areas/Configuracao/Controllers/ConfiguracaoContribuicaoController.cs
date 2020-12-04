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

    public class ConfiguracaoContribuicaoController : Controller {

        //Atributos
        private IOrganizacaoBL _OrganizacaoBL;
        private IConfiguracaoContribuicaoBL _ConfiguracaoContribuicaoBL;

        //Propriedades
        private IOrganizacaoBL OOrganizacaoBL => this._OrganizacaoBL = this._OrganizacaoBL ?? new OrganizacaoBL();
        private IConfiguracaoContribuicaoBL OConfiguracaoContribuicaoBL => this._ConfiguracaoContribuicaoBL = this._ConfiguracaoContribuicaoBL ?? new ConfiguracaoContribuicaoBL();


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

            ConfiguracaoContribuicaoForm ViewModel = new ConfiguracaoContribuicaoForm() {
                ConfiguracaoContribuicao = this.OConfiguracaoContribuicaoBL.carregar(idOrganizacao, false) ?? new ConfiguracaoContribuicao(),
            };

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoContribuicaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (User.idOrganizacao() > 0) {
                ViewModel.ConfiguracaoContribuicao.idOrganizacao = User.idOrganizacao();
            }

            this.OConfiguracaoContribuicaoBL.salvar(ViewModel.ConfiguracaoContribuicao);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("editar", new { ViewModel.ConfiguracaoContribuicao.idOrganizacao });
        }
    }
}