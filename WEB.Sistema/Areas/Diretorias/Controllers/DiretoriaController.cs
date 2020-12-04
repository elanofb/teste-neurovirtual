using BLL.Diretorias;
using DAL.Diretorias;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;
using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Diretorias.ViewModels;

namespace WEB.Areas.Diretorias.Controllers {
    public class DiretoriaController : Controller {
        //Constantes

        //Atributos
        private IDiretoriaBL _DiretoriaBL;
        private IDiretoriaVWBL _DiretoriaVWBL;

        //Propriedades
        private IDiretoriaBL ODiretoriaBL => (this._DiretoriaBL = this._DiretoriaBL ?? new DiretoriaBL());
        private IDiretoriaVWBL ODiretoriaVWBL => (this._DiretoriaVWBL = this._DiretoriaVWBL ?? new DiretoriaVWBL());

        //Construtor
        public DiretoriaController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaDiretoria = this.ODiretoriaVWBL.listar(descricao, ativo).OrderBy(x => x.anoInicioGestao);

            return View(listaDiretoria.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            DiretoriaForm ViewModel = new DiretoriaForm();

            var ODiretoria = this.ODiretoriaBL.carregar(UtilNumber.toInt32(id)) ?? new Diretoria();

            ViewModel.Diretoria = ODiretoria;

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(DiretoriaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.ODiretoriaBL.salvar(ViewModel.Diretoria);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { id = ViewModel.Diretoria.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.ODiretoriaBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idItem in id) {

                var RetornoItem = this.ODiretoriaBL.excluir(idItem, User.id());

                if (RetornoItem.flagError == true) {
                    Retorno.error = true;
                    Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
                }
            }

            return Json(Retorno);
        }
    }
}