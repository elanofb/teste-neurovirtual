using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.ConfiguracoesAssociados.ViewModels;

namespace WEB.Areas.ConfiguracoesAssociados.Controllers {
    public class ConfiguracaoAssociadoCampoGrupoController : Controller {

        //Atributos
        private IConfiguracaoAssociadoCampoGrupoBL _ConfiguracaoAssociadoCampoGrupoBL;

        //Propriedades
        private IConfiguracaoAssociadoCampoGrupoBL OConfiguracaoAssociadoCampoGrupoBL => _ConfiguracaoAssociadoCampoGrupoBL = _ConfiguracaoAssociadoCampoGrupoBL ?? new ConfiguracaoAssociadoCampoGrupoBL();

        // GET: ConfiguracoesAssociados/ConfiguracaoAssociadoCampoGrupo
        [ActionName("partial-form-grupo")]
        public PartialViewResult partialFormGrupo(int? id, int idTipoCampoCadastro) {

            var idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var ViewModel = new AssociadoCampoGrupoForm();

            ViewModel.AssociadoCampoGrupo = this.OConfiguracaoAssociadoCampoGrupoBL.carregar(id.toInt(), idOrganizacao) ?? new ConfiguracaoAssociadoCampoGrupo();

            ViewModel.AssociadoCampoGrupo.idOrganizacao = idOrganizacao;

            ViewModel.AssociadoCampoGrupo.idTipoCampoCadastro = idTipoCampoCadastro;

            return PartialView(ViewModel);
        }

        [HttpPost, ActionName("salvar-form-grupo"), ValidateInput(false)]
        public ActionResult salvarFormGrupo(AssociadoCampoGrupoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView("partial-form-grupo", ViewModel);
            }

            bool flagSucesso = OConfiguracaoAssociadoCampoGrupoBL.salvar(ViewModel.AssociadoCampoGrupo);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Registro salvo!", "Os dados foram enviados com sucesso."));

                CacheService.getInstance.remover("lista_campos_associado");

                CacheService.getInstance.remover("lista_grupos_campos_associado");

                return Json(new { error = false, ViewModel.AssociadoCampoGrupo.idTipoCampoCadastro, message = "Os dados foram salvos com sucesso." });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "Não foi possível salvar o registro."));

            return PartialView("partial-form-grupo", ViewModel);
        }

        // GET: ConfiguracoesAssociados/ConfiguracaoAssociadoCampoGrupo
        [ActionName("partial-lista-grupo")]
        public PartialViewResult partialListaGrupo(int idTipoCampoCadastro) {

            var idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var query = this.OConfiguracaoAssociadoCampoGrupoBL.listar(UtilNumber.toInt32(idOrganizacao));

            query = query.Where(x => x.idTipoCampoCadastro == idTipoCampoCadastro);


            var lista = query.ToList().OrderBy(x => x.ordemExibicao ?? 10000).ToList();

            return PartialView(lista);
        }

        // GET: ConfiguracoesAssociados/ConfiguracaoAssociadoCampoGrupo
        [HttpPost, ActionName("excluir")]
        public ActionResult excluir(int? id) {

            var Retorno = this.OConfiguracaoAssociadoCampoGrupoBL.excluir(id.toInt());

            CacheService.getInstance.remover("lista_campos_associado");

            CacheService.getInstance.remover("lista_grupos_campos_associado");

            return Json(Retorno);
        }

        /// <summary>
        /// Alterar a ordem de exibição dos grupos
        /// </summary>
        [ActionName("alterar-ordem")]
        public ActionResult alterarOrdem(int id, byte pos, int idTipoCampoCadastro) {

            this.OConfiguracaoAssociadoCampoGrupoBL.reordenarExibicao(id, pos, idTipoCampoCadastro);

            return null;
        }
    }
}