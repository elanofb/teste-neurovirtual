using System.Linq;
using System.Web.Mvc;
using BLL.Organizacoes;
using BLL.Permissao;
using DAL.Organizacoes;
using WEB.Areas.Associacoes.ViewModels;

namespace WEB.Areas.Associacoes.Controllers {

    public class AssociacaoModulosContratadosController : Controller {

        //Atributos
        private IAcessoRecursoGrupoBL _IAcessoRecursoGrupoBL;
        private IAcessoRecursoGrupoOrganizacaoBL _IAcessoRecursoGrupoOrganizacaoBL;

        //Propriedades
        private IAcessoRecursoGrupoBL OAcessoRecursoGrupoBL => _IAcessoRecursoGrupoBL = _IAcessoRecursoGrupoBL ?? new AcessoRecursoGrupoBL();
        private IAcessoRecursoGrupoOrganizacaoBL OAcessoRecursoGrupoOrganizacaoBL => _IAcessoRecursoGrupoOrganizacaoBL = _IAcessoRecursoGrupoOrganizacaoBL ?? new AcessoRecursoGrupoOrganizacaoBL();


        //
        [ActionName("partial-lista-modulos")]
        public PartialViewResult partialListaModulos(int idOrganizacao) {

            var listaIdsModulosAtivados = this.OAcessoRecursoGrupoOrganizacaoBL.listar(idOrganizacao).Select(x => x.idRecursoGrupo).ToList();

            var ViewModel = new AssociacaoModulosContratadosVM();

            ViewModel.idOrganizacao = idOrganizacao;

            var listaModulosSistema = this.OAcessoRecursoGrupoBL.listar("S").ToList();
            
            ViewModel.listaModulosAtivos = listaModulosSistema.Where(x => listaIdsModulosAtivados.Contains(x.id)).OrderBy(x => x.descricao).ToList();

            ViewModel.listaModulosInativos = listaModulosSistema.Where(x => !listaIdsModulosAtivados.Contains(x.id)).OrderBy(x => x.descricao).ToList();

            return PartialView(ViewModel);

        }

        [HttpPost, ActionName("registrar-modulo")]
        public ActionResult registrarModulo(int idOrganizacao, int idRecursoGrupo, bool flagAtivar) {

            if (!flagAtivar) {
                this.OAcessoRecursoGrupoOrganizacaoBL.excluir(idOrganizacao, idRecursoGrupo);
            }

            if (flagAtivar) {
                var OAcessoRecursoGrupoOrganizacao = new AcessoRecursoGrupoOrganizacao();
                OAcessoRecursoGrupoOrganizacao.idOrganizacao = idOrganizacao;
                OAcessoRecursoGrupoOrganizacao.idRecursoGrupo = idRecursoGrupo;

                this.OAcessoRecursoGrupoOrganizacaoBL.salvar(OAcessoRecursoGrupoOrganizacao);
            }

            return null;

        }

    }
}