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

    public class AssociadoRepresentanteController : BaseSistemaController{

		//Atributos
		private IAssociadoRepresentanteBL _AssociadoRepresentanteBL; 

		//Propriedades
        private IAssociadoRepresentanteBL OAssociadoRepresentanteBL { get { return (this._AssociadoRepresentanteBL = this._AssociadoRepresentanteBL ?? new AssociadoRepresentanteBL()); } }

        //Construtor
        public AssociadoRepresentanteController() {

        }

		//Bloco Partial para listagem de Representantes de um associado
		[HttpGet, ActionName("partial-listar-representantes")]
        public PartialViewResult partialListarRepresentantes(int idAssociado){
			var listaRepresentantes = this.OAssociadoRepresentanteBL.listar(idAssociado, "S").ToList();
            return PartialView(listaRepresentantes);
        }

		//Formulário Parcial para novo Representante
		[HttpGet]
		public PartialViewResult editar(int? id, int? idAssociado){

			var OAssociadoRepresentante = this.OAssociadoRepresentanteBL.carregar(UtilNumber.toInt32(id)) ?? new AssociadoRepresentante();
			OAssociadoRepresentante.idAssociado = ( OAssociadoRepresentante.idAssociado > 0? OAssociadoRepresentante.idAssociado : UtilNumber.toInt32(idAssociado));

			AssociadoRepresentanteForm ViewModel = new AssociadoRepresentanteForm();
			ViewModel.AssociadoRepresentante = OAssociadoRepresentante;

			return PartialView(ViewModel);
		}


		//Formulario submetido para novo Representante para o associado
		[HttpPost]
		public ActionResult editar(AssociadoRepresentanteForm ViewModel){

			if (!ModelState.IsValid) {
				return PartialView(ViewModel);
			}

			bool flagSalvo = this.OAssociadoRepresentanteBL.salvar(ViewModel.AssociadoRepresentante);

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
				UtilRetorno RetornoExclusao = this.OAssociadoRepresentanteBL.excluirPorId(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = false;
				}
			}

            return Json(Retorno);
        }
    }
}
