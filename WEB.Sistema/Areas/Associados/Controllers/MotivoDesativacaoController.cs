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
    public class MotivoDesativacaoController : Controller
    {
        //Constantes

        //Atributos
        private IMotivoDesativacaoBL _MotivoDesativacaoBL;

        //Propriedades
        private IMotivoDesativacaoBL OMotivoDesativacaoBL => (this._MotivoDesativacaoBL = this._MotivoDesativacaoBL ?? new MotivoDesativacaoBL());

        //Construtor
        public MotivoDesativacaoController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaMotivoDesativacao = this.OMotivoDesativacaoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaMotivoDesativacao.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            MotivoDesativacaoForm ViewModel = new MotivoDesativacaoForm();

            var OMotivoDesativacao = this.OMotivoDesativacaoBL.carregar(UtilNumber.toInt32(id)) ?? new MotivoDesativacao();

            ViewModel.MotivoDesativacao = OMotivoDesativacao;

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(MotivoDesativacaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OMotivoDesativacaoBL.salvar(ViewModel.MotivoDesativacao);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados forma salvos com sucesso."));

                return RedirectToAction("editar", new { id = ViewModel.MotivoDesativacao.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.OMotivoDesativacaoBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idItem in id) {

                var RetornoItem = this.OMotivoDesativacaoBL.excluir(idItem, User.id());

                if (RetornoItem.flagError == true) {
                    Retorno.error = true;
                    Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
                }
            }

            return Json(Retorno);
        }
    }
}