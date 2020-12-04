using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Json;
using BLL.Publicacoes;
using DAL.Permissao.Security.Extensions;
using DAL.Publicacoes;
using MvcFlashMessages;
using WEB.Areas.Publicacoes.ViewModels;

namespace WEB.Areas.Publicacoes.Controllers {

    public class TipoParceiroController : Controller {

        //Atributos
        private ITipoParceiroBL _TipoParceiroBL;

        //Propriedades
        private ITipoParceiroBL OTipoParceiroBL => _TipoParceiroBL = _TipoParceiroBL ?? new TipoParceiroBL();

        public ActionResult listar() {

            var descricao = UtilRequest.getString("valorBusca");
            var ativo = UtilRequest.getBool("flagAtivo");

            var listaTipoParceiro = this.OTipoParceiroBL.listar(User.idOrganizacao(), descricao, ativo).OrderBy(x => x.descricao);

            return View(listaTipoParceiro.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new TipoParceiroForm();
            ViewModel.TipoParceiro = this.OTipoParceiroBL.carregar(UtilNumber.toInt32(id)) ?? new TipoParceiro();

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult editar(TipoParceiroForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OTipoParceiroBL.salvar(ViewModel.TipoParceiro);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.TipoParceiro.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult excluir(int[] id) {

            JsonMessage Retorno = new JsonMessage();

            foreach (int idExclusao in id) {

                UtilRetorno RetornoExclusao = this.OTipoParceiroBL.excluir(idExclusao);

                if (RetornoExclusao.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
                }
            }

            if (Retorno.error == false) {
                Retorno.error = false;
                Retorno.message = "Os registros foram excluídos com sucesso.";
            }

            return Json(Retorno);
        }

        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {
            return Json(this.OTipoParceiroBL.alterarStatus(id));
        }
    }
}