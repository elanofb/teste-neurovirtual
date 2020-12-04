using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.Ajudas;
using DAL.Ajudas;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.Ajudas.ViewModels;

namespace WEB.Areas.Ajudas.Controllers {

    public class AjudaModuloController : Controller {

        //Atributos
        private IAjudaModuloBL _AjudaModuloBL;

        //Propriedades
        private IAjudaModuloBL OAjudaModuloBL => _AjudaModuloBL = _AjudaModuloBL ?? new AjudaModuloBL();

        //Construtor
        public AjudaModuloController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            int? idAjudaCategoria = UtilRequest.getInt32("idAjudaCategoria");

            int? idRecursoGrupo = UtilRequest.getInt32("idRecursoGrupo");

            var listaAjudaModulo = this.OAjudaModuloBL.listar(descricao, ativo);

            if (idAjudaCategoria > 0) {
                listaAjudaModulo = listaAjudaModulo.Where(x => x.idCategoriaAjuda == idAjudaCategoria);
            }

            if (idRecursoGrupo > 0) {
                listaAjudaModulo = listaAjudaModulo.Where(x => x.AjudaCategoria.idRecursoGrupo == idRecursoGrupo);
            }
            
            return View(listaAjudaModulo.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new AjudaModuloForm();

            ViewModel.AjudaModulo = this.OAjudaModuloBL.carregar(UtilNumber.toInt32(id)) ?? new AjudaModulo();

            if (ViewModel.AjudaModulo == null) {
                return RedirectToAction("listar");
            }

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(AjudaModuloForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }
            
            bool flagSucesso = this.OAjudaModuloBL.salvar(ViewModel.AjudaModulo);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.AjudaModulo.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.OAjudaModuloBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idAjudaModulo in id) {

                var RetornoItem = this.OAjudaModuloBL.excluir(idAjudaModulo);

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }

            }

            return Json(Retorno);

        }

    }

}