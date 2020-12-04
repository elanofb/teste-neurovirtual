using BLL.Localizacao;
using DAL.Localizacao;
using MvcFlashMessages;
using PagedList;
using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.Localizacao.ViewModels;

namespace WEB.Areas.Localizacao.Controllers {

    public class CidadeController : BaseSistemaController {

        public ICidadeBL _ICidadeBL { get; set; }

        private ICidadeBL OCidadeBL => _ICidadeBL = _ICidadeBL ?? new CidadeBL();

		public CidadeController() { 

		} 

		//
        public ActionResult listar() {

			string ativo = UtilRequest.getString("flagStatus");
			string valorBusca = UtilRequest.getString("valorBusca");
			string flagIBGE = UtilRequest.getString("flagIBGE");
			int idEstado = UtilRequest.getInt32("idEstado");

			var queryCidades = this.OCidadeBL.listar(idEstado, valorBusca, ativo);

			if (flagIBGE == "S") {
				queryCidades = queryCidades.Where(x => x.idMunicipioIBGE != null);
			}

			if (flagIBGE == "N") {
				queryCidades = queryCidades.Where(x => x.idMunicipioIBGE == null);
			}

			queryCidades = queryCidades.OrderBy(x => x.nome);
			return View(queryCidades.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));

        }
   

        //
		[HttpGet]
        public ActionResult editar(int? id) {

			var ViewModel = new CidadeVM();
			ViewModel.Cidade = this.OCidadeBL.carregar(UtilNumber.toInt32(id)) ?? new Cidade();
            
            return View(ViewModel);
        }

        //
		[HttpPost]
        public ActionResult editar(CidadeVM ViewModel) {

			if(!ModelState.IsValid){
				return View(ViewModel);
			}

            ViewModel.Cidade.nomeMunicipio = ViewModel.Cidade.nome;
			bool flagSucesso = this.OCidadeBL.salvar(ViewModel.Cidade);

			if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados da cidade foram salvos com sucesso."));
			    return RedirectToAction("listar");	
			}
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);

        }    
	
        //
		[HttpPost]
		[ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OCidadeBL.alterarStatus(id));
		}

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
			
            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idCidade in id) {

                var RetornoItem = this.OCidadeBL.excluir(idCidade);

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }

            }

            return Json(Retorno);

        }

    }
}