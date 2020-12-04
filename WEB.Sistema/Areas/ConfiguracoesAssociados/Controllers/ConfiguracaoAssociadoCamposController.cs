using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.ConfiguracoesAssociados;
using BLL.Organizacoes;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesAssociados.Models.ViewModels;
using WEB.Areas.ConfiguracoesAssociados.ViewModels;

namespace WEB.Areas.ConfiguracoesAssociados.Controllers {
    public class ConfiguracaoAssociadoCamposController : Controller {

        //Atributos
        private IOrganizacaoBL _OrganizacaoBL;
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;
        private IConfiguracaoAssociadoCampoOpcaoBL _ConfiguracaoAssociadoCampoOpcaoBL;
        private IConfiguracaoAssociadoCampoClonagemBL _ConfiguracaoAssociadoCampoClonagemBL;
        private IConfiguracaoAssociadoCampoPropriedadeBL _ConfiguracaoAssociadoCampoPropriedadeBL;

        //Services
        private IOrganizacaoBL OOrganizacaoBL => this._OrganizacaoBL = this._OrganizacaoBL ?? new OrganizacaoBL();
        private IConfiguracaoAssociadoCampoBL OConfiguracaoAssociadoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();
        private IConfiguracaoAssociadoCampoOpcaoBL OConfiguracaoAssociadoCampoOpcaoBL => _ConfiguracaoAssociadoCampoOpcaoBL = _ConfiguracaoAssociadoCampoOpcaoBL ?? new ConfiguracaoAssociadoCampoOpcaoBL();
        private IConfiguracaoAssociadoCampoClonagemBL OConfiguracaoAssociadoCampoClonagemBL => _ConfiguracaoAssociadoCampoClonagemBL = _ConfiguracaoAssociadoCampoClonagemBL ?? new ConfiguracaoAssociadoCampoClonagemBL();
        private IConfiguracaoAssociadoCampoPropriedadeBL OConfiguracaoAssociadoCampoPropriedadeBL => _ConfiguracaoAssociadoCampoPropriedadeBL = _ConfiguracaoAssociadoCampoPropriedadeBL ?? new ConfiguracaoAssociadoCampoPropriedadeBL();

        //
        [HttpGet]
        public ActionResult listar() {

            if (User.idOrganizacao() > 0) {
                return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
            }

            var lista = this.OOrganizacaoBL.listar("", true).ToList();

            return View(lista);
        }


        [HttpGet, ActionName("editar")]
        public ActionResult editar() {

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {

                idOrganizacao = User.idOrganizacao();
            }

            var ViewModel = new ConfiguracaoCadastroVM();

            ViewModel.carregarDados(idOrganizacao);

            return View(ViewModel);
        }

        /// <summary>
        /// Listagem de campos e grupo já configurados
        /// </summary>
        [HttpGet, ActionName("partial-lista-campos")]
        public PartialViewResult partialListaCampos(int idTipoCampoCadastro) {

            var ViewModel = new ConfiguracaoCamposVM();

            ViewModel.idTipoCampoCadastro = idTipoCampoCadastro;

            ViewModel.idsTipoAssociado = UtilRequest.getListInt("idsTipoAssociado");

            ViewModel.idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                ViewModel.idOrganizacao = User.idOrganizacao();
            }

            ViewModel.carregarDados();

            return PartialView(ViewModel);
        }

        /// <summary>
        /// Lista com as opções de campos disponíveis para incluir no cadastro do associado
        /// </summary>
        [HttpGet, ActionName("modal-opcoes-campos-pf")]
        public PartialViewResult modalOpcoesCampoPF() {

            return PartialView();
        }
        /// <summary>
        /// Lista com as opções de campos disponíveis para incluir no cadastro do associado
        /// </summary>
        [HttpGet, ActionName("modal-opcoes-campos-pj")]
        public PartialViewResult modalOpcoesCampoPJ() {

            return PartialView();
        }

        /// <summary>
        /// Listagem de campos e grupo já configurados
        /// </summary>
        [HttpGet, ActionName("modal-form-campo")]
        public PartialViewResult modalFormCampo(int? id, int idTipoCampoCadastro) {

            var idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var ViewModel = new AssociadoCampoForm();

            ViewModel.AssociadoCampo = this.OConfiguracaoAssociadoCampoBL.carregar(id.toInt(), idOrganizacao) ?? new ConfiguracaoAssociadoCampo();

            ViewModel.AssociadoCampo.idOrganizacao = idOrganizacao;

            ViewModel.AssociadoCampo.idTipoCampoCadastro = idTipoCampoCadastro;

            ViewModel.AssociadoCampo.idsTipoAssociado = ViewModel.AssociadoCampo.listaTipoAssociado.Where(x => x.dtExclusao == null).Select(x => x.idTipoAssociado).ToList();

            if (UtilRequest.getInt32("idCampoClone") > 0) {
                ViewModel.AssociadoCampo.id = 0;
                ViewModel.idCampoClone = UtilRequest.getInt32("idCampoClone");
            }

            return PartialView(ViewModel);
        }

        /// <summary>
        /// Salvar os dados
        /// </summary>
        [HttpPost, ActionName("salvar-form-campo"), ValidateInput(false)]
        public ActionResult salvarFormCampo(AssociadoCampoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView("modal-form-campo", ViewModel);
            }
            
            bool flagSucesso = OConfiguracaoAssociadoCampoBL.salvar(ViewModel.AssociadoCampo);

            if (ViewModel.idCampoClone > 0 && flagSucesso) {

                var idCampo = UtilNumber.toInt32(ViewModel.idCampoClone);

                var flagExisteOCampoClone = this.OConfiguracaoAssociadoCampoBL.carregar(idCampo, ViewModel.AssociadoCampo.idOrganizacao);

                if (flagExisteOCampoClone == null) {
                    return Json(new { error = false, ViewModel.AssociadoCampo.idTipoCampoCadastro, message = "Os dados foram salvos com sucesso." });
                }

                this.OConfiguracaoAssociadoCampoOpcaoBL.clonarOpcoesCampo(idCampo, ViewModel.AssociadoCampo.id);

                this.OConfiguracaoAssociadoCampoPropriedadeBL.clonarPropriedadesCampo(idCampo, ViewModel.AssociadoCampo.id);

                return Json(new { error = false, ViewModel.AssociadoCampo.idTipoCampoCadastro, message = "Os dados foram salvos com sucesso." });
            }


            if (flagSucesso) {

                CacheService.getInstance.remover("lista_campos_associado");

                CacheService.getInstance.remover("lista_grupos_campos_associado");

                return Json(new { error = false, ViewModel.AssociadoCampo.idTipoCampoCadastro, message = "Os dados foram salvos com sucesso." });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "Não foi possível salvar o registro."));

            return PartialView("modal-form-campo", ViewModel);
        }

        ///
        [HttpPost, ActionName("clonar-padrao-sistema")]
        public ActionResult clonarPadraoSistema() {

            var idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var idTipoCampoCadastro = UtilRequest.getInt32("idTipoCampoCadastro");

            this.OConfiguracaoAssociadoCampoClonagemBL.clonarDefaultSistema(idOrganizacao, idTipoCampoCadastro);

            CacheService.getInstance.remover();

            return Json(new { flagError = false });
        }
    }
}