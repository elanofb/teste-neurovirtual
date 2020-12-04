using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Json;
using BLL.Bancos;
using DAL.Bancos;
using WEB.Areas.Bancos.ViewModels;
using MvcFlashMessages;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Bancos.Controllers {

    public class BancoController:Controller {
        //Constantes

        //Atributos
        private IBancoBL _BancoBL;

        //Propriedades
        private IBancoBL OBancoBL => _BancoBL = _BancoBL ?? new BancoBL();

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            string ativo = UtilRequest.getString("flagAtivo");
            var listaBanco = this.OBancoBL.listar(descricao,ativo).OrderBy(x => x.descricao);

            return View(listaBanco.ToPagedList(UtilRequest.getNroPagina(),20));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            BancoForm ViewModel = new BancoForm();
            
            ViewModel.Banco = this.OBancoBL.carregar(UtilNumber.toInt32(id)) ?? new Banco();

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(BancoForm ViewModel) {

            if(!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OBancoBL.salvar(ViewModel.Banco);

            if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do banco foram salvos com sucesso."));
			    return RedirectToAction("listar");	
			}
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);
        }

        //
		[HttpPost]
		[ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OBancoBL.alterarStatus(id));
		}

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idServico in id) {

                var RetornoItem = this.OBancoBL.excluir(idServico, User.id());

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }

            }

            return Json(Retorno);

        }
        
    }
}