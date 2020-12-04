using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesAssociados.ViewModels;

namespace WEB.Areas.ConfiguracoesAssociados.Controllers {
    public class ConfiguracaoAssociadoCamposClonagemController : Controller {

        //Atributos
        private IConfiguracaoAssociadoCampoTipoAssociadoClonagemBL _ConfiguracaoAssociadoCampoTipoAssociadoClonagemBL;

        //Services
        private IConfiguracaoAssociadoCampoTipoAssociadoClonagemBL OConfiguracaoAssociadoCampoTipoAssociadoClonagemBL => this._ConfiguracaoAssociadoCampoTipoAssociadoClonagemBL = this._ConfiguracaoAssociadoCampoTipoAssociadoClonagemBL ?? new ConfiguracaoAssociadoCampoTipoAssociadoClonagemBL();

        /// <summary>
        /// Modal para selecionar os tipos de associados para clonar os campos
        /// </summary>
        [HttpGet, ActionName("modal-clonar-campos")]
        public PartialViewResult modalClonarCampos(short? tipoCadastro) {

            AssociadoCampoClonagemForm ViewModel = new AssociadoCampoClonagemForm();

            ViewModel.tipoCadastro = tipoCadastro;
            
            return PartialView(ViewModel);
        }

        /// <summary>
        /// Clonar os campos
        /// </summary>
        [HttpPost, ActionName("clonar-campos")]
        public ActionResult clonarCampos(AssociadoCampoClonagemForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView("modal-clonar-campos", ViewModel);
            }
            
            var idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            ViewModel.idsTiposAssociadoDestinos = ViewModel.idsTiposAssociadoDestinos.Where(x => x != ViewModel.idTipoAssociadoOrigem && x > 0).ToList();
            
            CacheService.getInstance.remover("lista_campos_associado");
            CacheService.getInstance.remover("lista_grupos_campos_associado");
            
            var flagSucesso = OConfiguracaoAssociadoCampoTipoAssociadoClonagemBL.clonarConfiguracaoCampos(idOrganizacao, ViewModel.idTipoAssociadoOrigem.toInt(), ViewModel.idsTiposAssociadoDestinos);

            if (flagSucesso) {
                return Json(new { error = false });
            }
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "Não foi possível salvar o registro."));
            return PartialView("modal-clonar-campos", ViewModel);
        }

    }
}