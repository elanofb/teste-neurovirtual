using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Configuracao.ViewModels;
using DAL.Configuracoes;
using BLL.Configuracoes;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Configuracao.Controllers {

    public class ConfiguracaoEmailController : Controller {

        //Atributos
        private IConfiguracaoEmailBL _ConfiguracaoEmailBL;

        //Propriedades
        private IConfiguracaoEmailBL OConfiguracaoEmailBL => this._ConfiguracaoEmailBL = this._ConfiguracaoEmailBL ?? new ConfiguracaoEmailBL();


        //
        [HttpGet]
        public ActionResult listar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                return RedirectToAction("editar", new {idOrganizacao = User.idOrganizacao()});
            }

            var lista = this.OConfiguracaoEmailBL.listar(idOrganizacao)
                            .Select(x => new {
                                                 x.id,
                                                 x.idOrganizacao,
                                                 Organizacao = new {
                                                                       id = x.Organizacao == null ? 0 : x.Organizacao.id,
                                                                       Pessoa = new {
                                                                                        id = x.Organizacao == null? 0 : x.Organizacao.Pessoa.id,
                                                                                        x.Organizacao.Pessoa.nome
                                                                                    }
                                                                   },
                                                 x.idUsuarioCadastro,
                                                 UsuarioCadastro = new {
                                                                           x.UsuarioCadastro.id,
                                                                           x.UsuarioCadastro.nome
                                                                       },    
                                                 x.dtCadastro
                                             })
                            .ToListJsonObject<ConfiguracaoEmail>();

            return View(lista);
        }

        //
        [HttpGet]
        public ActionResult editar() {
            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            ConfiguracaoEmailForm ViewModel = new ConfiguracaoEmailForm() {ConfiguracaoEmail = this.OConfiguracaoEmailBL.carregar(idOrganizacao, false) ?? new ConfiguracaoEmail(),};

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoEmailForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (User.idOrganizacao() > 0) {
                ViewModel.ConfiguracaoEmail.idOrganizacao = User.idOrganizacao();
            }

            this.OConfiguracaoEmailBL.salvar(ViewModel.ConfiguracaoEmail);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("editar", new {ViewModel.ConfiguracaoEmail.idOrganizacao});
        }
    }

}
