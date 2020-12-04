using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using DAL.Associados;
using WEB.App_Infrastructure;
using WEB.Areas.Associados.ViewModels;
using System.Json;
using MvcFlashMessages;

namespace WEB.Areas.Associados.Controllers{

    public class AssociadoCargoController : BaseSistemaController{

		//Atributos
		private IAssociadoCargoBL _AssociadoCargoBL; 

		//Propriedades
        private IAssociadoCargoBL OAssociadoCargoBL { get { return (this._AssociadoCargoBL = this._AssociadoCargoBL ?? new AssociadoCargoBL()); } }

        //Construtor
        public AssociadoCargoController() {

        }

		//Bloco Partial para listagem de cargos de um associado
		[HttpGet, ActionName("partial-listar-cargos")]
        public PartialViewResult partialListarCargos(int idAssociado){
			var listaCargos = this.OAssociadoCargoBL.listar(idAssociado, "S").ToList();
            return PartialView(listaCargos);
        }

		//Formulário Parcial para novo cargo
		[HttpGet]
		public PartialViewResult editar(int? id, int? idAssociado){

			var OAssociadoCargo = this.OAssociadoCargoBL.carregar(UtilNumber.toInt32(id)) ?? new AssociadoCargo();
			OAssociadoCargo.idAssociado = ( OAssociadoCargo.idAssociado > 0? OAssociadoCargo.idAssociado : UtilNumber.toInt32(idAssociado));

			AssociadoCargoForm ViewModel = new AssociadoCargoForm();
			ViewModel.AssociadoCargo = OAssociadoCargo;

			return PartialView(ViewModel);
		}


		//Formulario submetido para novo cargo para o associado
		[HttpPost]
		public ActionResult editar(AssociadoCargoForm ViewModel){

			if (!ModelState.IsValid) {
				return PartialView(ViewModel);
			}

			bool flagSalvo = this.OAssociadoCargoBL.salvar(ViewModel.AssociadoCargo);

            if (flagSalvo) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }

			return Json(new{ flagSucesso = flagSalvo });
		}

        //Excluir determinado registro
        [HttpPost]
        public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				UtilRetorno RetornoExclusao = this.OAssociadoCargoBL.excluir(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = false;
				}
			}

            return Json(Retorno);
        }
    }
}
