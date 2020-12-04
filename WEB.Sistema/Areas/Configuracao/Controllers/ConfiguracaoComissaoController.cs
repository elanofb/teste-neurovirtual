using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Configuracao.ViewModels;
using DAL.Configuracoes;
using BLL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using BLL.Caches;
using BLL.Organizacoes;

namespace WEB.Areas.Configuracao.Controllers {

    public class ConfiguracaoComissaoController : Controller {

        //Atributos
        private IOrganizacaoBL _OrganizacaoBL;
        private IConfiguracaoComissaoBL _ConfiguracaoComissaoBL;
        private IConfiguracaoPerfilComissionavelBL _ConfiguracaoPerfilComissionavelBL;

        //Propriedades
        private IOrganizacaoBL OOrganizacaoBL => this._OrganizacaoBL = this._OrganizacaoBL ?? new OrganizacaoBL();
        private IConfiguracaoComissaoBL OConfiguracaoComissaoBL => this._ConfiguracaoComissaoBL = this._ConfiguracaoComissaoBL ?? new ConfiguracaoComissaoBL();
        private IConfiguracaoPerfilComissionavelBL OConfiguracaoPerfilComissionavelBL => this._ConfiguracaoPerfilComissionavelBL = this._ConfiguracaoPerfilComissionavelBL ?? new ConfiguracaoPerfilComissionavelBL();


        //
        [HttpGet]
        public ActionResult listar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

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

            ConfiguracaoComissaoForm ViewModel = new ConfiguracaoComissaoForm() {
                ConfiguracaoComissao = this.OConfiguracaoComissaoBL.carregar(idOrganizacao, false) ?? new ConfiguracaoComissao(),
            };

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoComissaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (User.idOrganizacao() > 0) {
                ViewModel.ConfiguracaoComissao.idOrganizacao = User.idOrganizacao();
            }

            this.OConfiguracaoComissaoBL.salvar(ViewModel.ConfiguracaoComissao);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("editar", new { ViewModel.ConfiguracaoComissao.idOrganizacao });
        }

        // 
        [ActionName("partial-form-vincular-perfil-comissionavel")]
        public PartialViewResult partialFormVincularPerfilComissionavel(int? id) {

            var listaPerfisComissionaveis = this.OConfiguracaoPerfilComissionavelBL.listar(UtilNumber.toInt32(id), 0).ToList();

            ViewBag.id = id;

            return PartialView(listaPerfisComissionaveis);
        }

        [HttpPost, ActionName("vincular-perfil-comissionavel")]
        public ActionResult vincularPerfilComissionavel(int idPerfilAcesso, int idConfiguracaoComissao) {

            ConfiguracaoPerfilComissionavel OConfiguracaoPerfilComissionavel = OConfiguracaoPerfilComissionavelBL.listar(idConfiguracaoComissao, idPerfilAcesso).FirstOrDefault() ?? new ConfiguracaoPerfilComissionavel();

            if (OConfiguracaoPerfilComissionavel.id > 0) {
                return RedirectToAction("partial-form-vincular-perfil-comissionavel", new { idConfiguracaoComissao });
            }

            OConfiguracaoPerfilComissionavel.idPerfilAcesso = idPerfilAcesso;
            OConfiguracaoPerfilComissionavel.idConfiguracaoComissao = idConfiguracaoComissao;

            this.OConfiguracaoPerfilComissionavelBL.salvar(OConfiguracaoPerfilComissionavel);

            CacheService.getInstance.remover("configuracao_comissao");

            return RedirectToAction("partial-form-vincular-perfil-comissionavel", new { idConfiguracaoComissao });
        }

        //
        [HttpPost, ActionName("excluir-perfil-comissionavel")]
        public ActionResult excluirPerfilComissionavel(int[] id) {

            var Retorno = new UtilRetorno();

            Retorno.flagError = false;

            foreach (var idUsuario in id) {

                var RetornoItem = this.OConfiguracaoPerfilComissionavelBL.excluir(idUsuario);

                if (RetornoItem.flagError) {
                    Retorno.flagError = true;
                    Retorno.listaErros.Add("Não foi possível remover todos os registros.");
                }
            }

            if (!Retorno.flagError) {
                Retorno.listaErros.Add("Os registros informados foram removidos com sucesso.");
            }

            CacheService.getInstance.remover("configuracao_comissao");

            return Json(Retorno);
        }
    }
}