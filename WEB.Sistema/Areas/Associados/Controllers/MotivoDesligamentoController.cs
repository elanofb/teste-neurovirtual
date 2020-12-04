using BLL.Associados;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.Associados.ViewModels;
using MvcFlashMessages;
using PagedList;
using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;

namespace WEB.Areas.Associados.Controllers
{
    public class MotivoDesligamentoController : Controller
    {
        //Constantes

        //Atributos
        private IMotivoDesligamentoBL _MotivoDesligamentoBL;

        //Propriedades
        private IMotivoDesligamentoBL OMotivoDesligamentoBL => (this._MotivoDesligamentoBL = this._MotivoDesligamentoBL ?? new MotivoDesligamentoBL());

        //Construtor
        public MotivoDesligamentoController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaMotivoDesligamento = this.OMotivoDesligamentoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaMotivoDesligamento.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            MotivoDesligamentoForm ViewModel = new MotivoDesligamentoForm();

            var OMotivoDesligamento = this.OMotivoDesligamentoBL.carregar(UtilNumber.toInt32(id)) ?? new MotivoDesligamento();

            ViewModel.MotivoDesligamento = OMotivoDesligamento;

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(MotivoDesligamentoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OMotivoDesligamentoBL.salvar(ViewModel.MotivoDesligamento);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados forma salvos com sucesso."));

                return RedirectToAction("editar", new { id = ViewModel.MotivoDesligamento.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.OMotivoDesligamentoBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idItem in id) {

                var RetornoItem = this.OMotivoDesligamentoBL.excluir(idItem, User.id());

                if (RetornoItem.flagError == true) {
                    Retorno.error = true;
                    Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
                }
            }

            return Json(Retorno);
        }
    }
}