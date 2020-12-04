using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Configuracao.ViewModels;
using BLL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Configuracao.Controllers {

    public class ConfiguracaoNotificacaoController : Controller {

        //Atributos
        private ConfiguracaoNotificacaoBL _ConfiguracaoNotificacaoBL;

        //Propriedades
        private IConfiguracaoNotificacaoBL OConfiguracaoNotificacaoBL => this._ConfiguracaoNotificacaoBL = this._ConfiguracaoNotificacaoBL ?? new ConfiguracaoNotificacaoBL();


        //
        [HttpGet]
        public ActionResult listar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
            }

            var lista = this.OConfiguracaoNotificacaoBL.listar(idOrganizacao).ToList();

            return View(lista);
        }

        //
        [HttpGet]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }
            
            ConfiguracaoNotificacaoForm ViewModel = new ConfiguracaoNotificacaoForm();

            ViewModel.ConfiguracaoNotificacao = this.OConfiguracaoNotificacaoBL.carregar(idOrganizacao, false);
            
            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoNotificacaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (User.idOrganizacao() > 0) {
                ViewModel.ConfiguracaoNotificacao.idOrganizacao = User.idOrganizacao();
            }

            this.OConfiguracaoNotificacaoBL.salvar(ViewModel.ConfiguracaoNotificacao);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("editar", new { ViewModel.ConfiguracaoNotificacao.idOrganizacao });
        }
    }
}