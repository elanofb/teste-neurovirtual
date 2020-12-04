using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;
using DAL.Financeiro;
using PagedList;
using System.Json;
using MvcFlashMessages;

namespace WEB.Areas.Financeiro.Controllers {

    public class FormaPagamentoController:Controller {
        //Constantes

        //Atributos
        private IFormaPagamentoBL _FormaPagamentoBL;

        //Propriedades
        private IFormaPagamentoBL OFormaPagamentoBL => this._FormaPagamentoBL = this._FormaPagamentoBL ?? new FormaPagamentoBL();

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            string ativo = UtilRequest.getString("flagAtivo");
            var listaFormaPagamento = this.OFormaPagamentoBL.listar(descricao,ativo).OrderBy(x => x.descricao);

            return View(listaFormaPagamento.ToPagedList(UtilRequest.getNroPagina(),20));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            FormaPagamentoForm ViewModel = new FormaPagamentoForm();
            FormaPagamento OFormaPagamento = this.OFormaPagamentoBL.carregar(UtilNumber.toInt32(id)) ?? new FormaPagamento();

            ViewModel.FormaPagamento = OFormaPagamento;

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(FormaPagamentoForm ViewModel) {
            
            if(!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OFormaPagamentoBL.salvar(ViewModel.FormaPagamento);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

            if(flagSucesso) {
                return RedirectToAction("listar");
            }

            return View(ViewModel);
        }

        //Buscar as formas de pagamento e devolver retorno em JSON
        [HttpPost, ActionName("buscar-formas-pagamentos")]
        public ActionResult buscarFormasPagamentos() {

            int idMeioPagamento = UtilRequest.getInt32("idMeioPagamento");

            var query = this.OFormaPagamentoBL.listar("", "S");

            if (idMeioPagamento > 0) {

                query = query.Where(x => x.idMeioPagamento == idMeioPagamento);

            }

            var listaFormas = query.ToList()
                                    .Select( x => new {
                                        x.id,
                                        x.idMeioPagamento,
                                        x.descricao,
                                        x.ativo
                                    })
                                    .ToList();

            return Json(new { error = !listaFormas.Any(), listaResultados = listaFormas }, JsonRequestBehavior.AllowGet);
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            JsonMessage Retorno = new JsonMessage();
            Retorno.error = false;

            foreach(int idExclusao in id) {
                bool flagSucesso = this.OFormaPagamentoBL.excluir(idExclusao);

                if(!flagSucesso) {
                    Retorno.error = true;
                    Retorno.message = "Alguns registros não puderam ser excluídos.";
                }
            }

            return Json(Retorno);
        }
    }
}