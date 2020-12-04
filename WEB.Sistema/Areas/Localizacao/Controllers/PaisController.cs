using BLL.Localizacao;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;
using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Localizacao.ViewModels;

namespace WEB.Areas.Localizacao.Controllers {

    public class PaisController : BaseSistemaController {

        public IPaisBL _IPaisBL { get; set; }

        private IPaisBL OPaisBL => _IPaisBL = _IPaisBL ?? new PaisBL();

		public PaisController() { 
		} 

		//
        public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");
			string ativo = UtilRequest.getString("flagAtivo");

			var queryPaises = this.OPaisBL.listar(descricao, ativo).OrderBy(x => x.nome);

            return View(queryPaises.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));

        }
   

        //
		[HttpGet]
        public ActionResult editar(string id) {

			PaisVM ViewModel = new PaisVM();

			ViewModel.Pais = this.OPaisBL.carregar(id);
            
            if (ViewModel.Pais == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Nenhum País foi encontrado."));
                return RedirectToAction("listar");
            }

            return View(ViewModel);
        }

        //
		[HttpPost]
        public ActionResult editar(PaisVM ViewModel) {

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

			bool flagSucesso = this.OPaisBL.atualizar(ViewModel.Pais);

			if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do país foram salvos com sucesso."));
			    return RedirectToAction("listar");	
			}
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);

        }    
	
        //
		[HttpPost]
		[ActionName("alterar-status")]
		public ActionResult alterarStatus(string id) {
			return Json(this.OPaisBL.alterarStatus(id));
		}

        //
        [HttpPost]
        public ActionResult excluir(string[] id) {
			
            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idServico in id) {

                var RetornoItem = this.OPaisBL.excluir(idServico, User.id());

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }

            }

            return Json(Retorno);

        }

    }
}