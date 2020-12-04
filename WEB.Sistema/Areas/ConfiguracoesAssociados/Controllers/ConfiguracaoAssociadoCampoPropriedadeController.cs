using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesAssociados.ViewModels;

namespace WEB.Areas.ConfiguracoesAssociados.Controllers {
    public class ConfiguracaoAssociadoCampoPropriedadeController : Controller {

        //Atributos
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;
        private IConfiguracaoAssociadoCampoPropriedadeBL _ConfiguracaoAssociadoCampoPropriedadeBL;

        //Propriedades
        private IConfiguracaoAssociadoCampoBL OConfiguracaoAssociadoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();
        private IConfiguracaoAssociadoCampoPropriedadeBL OConfiguracaoAssociadoCampoPropriedadeBL => _ConfiguracaoAssociadoCampoPropriedadeBL = _ConfiguracaoAssociadoCampoPropriedadeBL ?? new ConfiguracaoAssociadoCampoPropriedadeBL();

        [ActionName("modal-campo-propriedade")]
        public ActionResult modalCampoPropriedade(int idCampo) {

            var ViewModel = OConfiguracaoAssociadoCampoBL.carregar(idCampo);

            if (ViewModel == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Campo não localizado!");
            }

            return View(ViewModel);
        }

        //
        [ActionName("partial-form-propriedade")]
        public PartialViewResult partialFormPropriedade(int? id, int idCampo) {

            var ViewModel = new AssociadoCampoPropriedadeForm();

            ViewModel.AssociadoCampoPropriedade = this.OConfiguracaoAssociadoCampoPropriedadeBL.carregar(id.toInt()) ?? new ConfiguracaoAssociadoCampoPropriedade();

            ViewModel.AssociadoCampoPropriedade.idConfiguracaoAssociadoCampo = idCampo;

            return PartialView(ViewModel);
        }

        [HttpPost, ActionName("salvar-form-propriedade")]
        public ActionResult salvarFormPropriedade(AssociadoCampoPropriedadeForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView("partial-form-propriedade", ViewModel);
            }

            bool flagSucesso = OConfiguracaoAssociadoCampoPropriedadeBL.salvar(ViewModel.AssociadoCampoPropriedade);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Registro salvo!", "Os dados foram enviados com sucesso."));

                CacheService.getInstance.remover("lista_campos_associado");

                CacheService.getInstance.remover("lista_grupos_campos_associado");

                return Json(new { error = false, message = "Os dados foram salvos com sucesso." });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "Não foi possível salvar o registro."));

            return PartialView("partial-form-propriedade", ViewModel);
        }

        // GET: ConfiguracoesAssociados/ConfiguracaoAssociadoCampoPropriedade
        [ActionName("partial-lista-propriedade")]
        public PartialViewResult partialListaPropriedade(int idCampo) {

            var query = this.OConfiguracaoAssociadoCampoPropriedadeBL.listar(idCampo);

            var lista = query.OrderBy(x => x.id).ToList();

            return PartialView(lista);
        }

        // GET: ConfiguracoesAssociados/ConfiguracaoAssociadoCampoPropriedade
        [ActionName("excluir")]
        public ActionResult excluir(int? id) {

            var Retorno = this.OConfiguracaoAssociadoCampoPropriedadeBL.excluir(id.toInt());

            return Json(new { Retorno.flagError, Retorno.listaErros, idTipoCadastroCampo = Retorno.info.toInt() }, JsonRequestBehavior.AllowGet);
        }
    }
}