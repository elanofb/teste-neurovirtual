using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.Ajudas;
using DAL.Ajudas;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.Ajudas.ViewModels;

namespace WEB.Areas.Ajudas.Controllers
{
    public class AjudaCategoriaController : Controller
    {

        //Atributos        
        private IAjudaCategoriaBL _AjudaCategoriaBL;

        //Propriedades
        private IAjudaCategoriaBL OAjudaCategoriaBL => this._AjudaCategoriaBL = this._AjudaCategoriaBL ?? new AjudaCategoriaBL();

        //
        public ActionResult listar() {

            var descricao = UtilRequest.getString("valorBusca");
            var ativo = UtilRequest.getBool("flagAtivo");

            var listaAjudaCategoria = this.OAjudaCategoriaBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaAjudaCategoria.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet, ActionName("modal-cadastrar")]
        public ActionResult modalCadastrar(int id = 0) {

            var ViewModel = new AjudaCategoriaForm();

            ViewModel.AjudaCategoria = OAjudaCategoriaBL.carregar(id) ?? new AjudaCategoria();

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("salvar")]
        public ActionResult salvarAjudaCategoria(AjudaCategoriaForm ViewModel) {
            
            if (!ModelState.IsValid) {
                return View("modal-cadastrar", ViewModel);
            }

            bool flagSucesso = this.OAjudaCategoriaBL.salvar(ViewModel.AjudaCategoria);

            if (flagSucesso) {
                
                return Json(new { error = false, message = "" });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View("modal-cadastrar", ViewModel);
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idServico in id) {

                var RetornoItem = this.OAjudaCategoriaBL.excluir(idServico);

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }

            }

            return Json(Retorno);

        }
    }
}