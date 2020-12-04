using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.ContasBancarias;
using BLL.Financeiro;
using PagedList;
using DAL.ContasBancarias;
using WEB.App_Infrastructure;
using WEB.Areas.ContasBancarias.ViewModels;
using DAL.Localizacao;
using MvcFlashMessages;

namespace WEB.Areas.ContasBancarias.Controllers {

    [OrganizacaoFilter]
    public class ContaBancariaController : BaseSistemaController {
        //Constantes

        //Atributos
        private IContaBancariaBL _ContaBancariaBL;
        private ISpContaBancariaSaldoAtualBL _SpContaBancariaSaldoAtualBL;

        //Propriedades
        private IContaBancariaBL OContaBancariaBL => this._ContaBancariaBL = this._ContaBancariaBL ?? new ContaBancariaBL();
        private ISpContaBancariaSaldoAtualBL OSpContaBancariaSaldoAtualBL => this._SpContaBancariaSaldoAtualBL = this._SpContaBancariaSaldoAtualBL ?? new SpContaBancariaSaldoAtualBL();

        //
        public ActionResult listar() {

            var ViewModel = new ContaBancariaVM();

            string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            ViewModel.listaContaBancaria = this.OContaBancariaBL.listar(descricao, ativo).OrderBy(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            ViewModel.listaSaldos = OSpContaBancariaSaldoAtualBL.listar(DateTime.Now.AddDays(1));

            return View(ViewModel);
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            ContaBancariaForm ViewModel = new ContaBancariaForm();
            
            ViewModel.ContaBancaria = this.OContaBancariaBL.carregar(UtilNumber.toInt32(id)) ?? new ContaBancaria();
            
            if (ViewModel.ContaBancaria.idCidade > 0 && ViewModel.ContaBancaria.idEstado.toInt() == 0){
                ViewModel.ContaBancaria.idEstado = ViewModel.ContaBancaria.Cidade.idEstado;
            }
            
            ViewModel.ContaBancaria.Cidade = ViewModel.ContaBancaria.Cidade ?? new Cidade();

            return View(ViewModel);
            
        }

        //
        [HttpPost]
        public ActionResult editar(ContaBancariaForm ViewModel) {

            if (!ModelState.IsValid) {

                return View(ViewModel);

            }

            bool flagSucesso = this.OContaBancariaBL.salvar(ViewModel.ContaBancaria);

            if (flagSucesso) {

                CacheService.getInstance.remover(ContaBancariaBL.keyCache);

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }
            if (ViewModel.ContaBancaria.id > 0) {
                return RedirectToAction("editar", new { ViewModel.ContaBancaria.id });
            }

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            JsonMessage Retorno = new JsonMessage();
            Retorno.error = false;

            foreach (int idExclusao in id) {
                bool flagSucesso = this.OContaBancariaBL.excluir(idExclusao);

                if (!flagSucesso) {
                    Retorno.error = true;
                    Retorno.message = "Alguns registros não puderam ser excluídos.";
                }
            }

            CacheService.getInstance.remover(ContaBancariaBL.keyCache);

            return Json(Retorno);
        }

        /// <summary>
        /// Listar ajax das contas bancárias
        /// </summary>
        [ActionName("listar-ajax")]
        public ActionResult listarAjax() {
            var query = this.OContaBancariaBL.listar("", true)
                 .ToList()
                 .Select(x => new {
                    value = x.id
                    , text = x.descricao
                 })
                 .OrderBy(x => x.text);

            var lista = query.ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}