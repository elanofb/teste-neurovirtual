using System;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesAssociados.ViewModels;
using BLL.Caches;

namespace WEB.Areas.ConfiguracoesAssociados.Controllers {
    public class ConfiguracaoAssociadoCampoOpcaoController : Controller {

        //Atributos
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;
        private IConfiguracaoAssociadoCampoOpcaoBL _ConfiguracaoAssociadoCampoOpcaoBL;

        //Opcaos
        private IConfiguracaoAssociadoCampoBL OConfiguracaoAssociadoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();
        private IConfiguracaoAssociadoCampoOpcaoBL OConfiguracaoAssociadoCampoOpcaoBL => _ConfiguracaoAssociadoCampoOpcaoBL = _ConfiguracaoAssociadoCampoOpcaoBL ?? new ConfiguracaoAssociadoCampoOpcaoBL();

        [ActionName("modal-campo-opcao")]
        public ActionResult modalCampoOpcao(int idCampo) {

            var ViewModel = OConfiguracaoAssociadoCampoBL.carregar(idCampo);

            if (ViewModel == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Campo não localizado!");
            }

            return View(ViewModel);
        }

        //
        [ActionName("partial-form-opcao")]
        public PartialViewResult partialFormOpcao(int? id, int idCampo) {

            var ViewModel = new AssociadoCampoOpcaoForm();

            ViewModel.AssociadoCampoOpcao = this.OConfiguracaoAssociadoCampoOpcaoBL.carregar(id.toInt()) ?? new ConfiguracaoAssociadoCampoOpcao();

            ViewModel.AssociadoCampoOpcao.idConfiguracaoAssociadoCampo = idCampo;

            return PartialView(ViewModel);
        }

        [HttpPost, ActionName("salvar-form-opcao")]
        public ActionResult salvarFormOpcao(AssociadoCampoOpcaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView("partial-form-opcao", ViewModel);
            }

            bool flagSucesso = OConfiguracaoAssociadoCampoOpcaoBL.salvar(ViewModel.AssociadoCampoOpcao);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Registro salvo!", "Os dados foram enviados com sucesso."));

                CacheService.getInstance.remover("lista_campos_associado");

                CacheService.getInstance.remover("lista_grupos_campos_associado");

                return Json(new { error = false, message = "Os dados foram salvos com sucesso." });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "Não foi possível salvar o registro."));

            return PartialView("partial-form-opcao", ViewModel);
        }

        // GET: ConfiguracoesAssociados/ConfiguracaoAssociadoCampoOpcao
        [ActionName("partial-lista-opcao")]
        public PartialViewResult partialListaOpcao(int idCampo) {

            var query = this.OConfiguracaoAssociadoCampoOpcaoBL.listar(idCampo);

            var lista = query.OrderBy(x => x.id).ToList();

            return PartialView(lista);
        }

        // GET: ConfiguracoesAssociados/ConfiguracaoAssociadoCampoOpcao
        [ActionName("excluir")]
        public ActionResult excluir(int? id) {

            var Retorno = this.OConfiguracaoAssociadoCampoOpcaoBL.excluir(id.toInt());

            return Json(new {Retorno.flagError, Retorno.listaErros, idTipoCadastroCampo = Retorno.info.toInt() } , JsonRequestBehavior.AllowGet);
        }
    }
}