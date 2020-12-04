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

    public class AssociadoAbrangenciaController : BaseSistemaController{

		//Atributos
		private IAssociadoAbrangenciaBL _AssociadoAbrangenciaBL; 

		//Propriedades
        private IAssociadoAbrangenciaBL OAssociadoAbrangenciaBL { get { return (this._AssociadoAbrangenciaBL = this._AssociadoAbrangenciaBL ?? new AssociadoAbrangenciaBL()); } }

        //Construtor
        public AssociadoAbrangenciaController() {

        }

		//Bloco Partial para listagem de Abrangencias de um associado
		[HttpGet, ActionName("partial-listar-Abrangencia")]
        public PartialViewResult partialListarAbrangencias(int idAssociado){
			var listaAbrangencias = this.OAssociadoAbrangenciaBL.listar(idAssociado, "S").ToList();
            return PartialView(listaAbrangencias);
        }

		//Formulário Parcial para novo Abrangencia
		[HttpGet]
		public PartialViewResult editar(int? id, int? idAssociado){

			var OAssociadoAbrangencia = this.OAssociadoAbrangenciaBL.carregar(UtilNumber.toInt32(id)) ?? new AssociadoAbrangencia();
			OAssociadoAbrangencia.idAssociado = ( OAssociadoAbrangencia.idAssociado > 0? OAssociadoAbrangencia.idAssociado : UtilNumber.toInt32(idAssociado));

			AssociadoAbrangenciaForm ViewModel = new AssociadoAbrangenciaForm();
			ViewModel.AssociadoAbrangencia = OAssociadoAbrangencia;

			return PartialView(ViewModel);
		}


		//Formulario submetido para novo Abrangencia para o associado
		[HttpPost]
		public ActionResult editar(AssociadoAbrangenciaForm ViewModel){

			if (!ModelState.IsValid) {
				return PartialView(ViewModel);
			}

			bool flagSalvo = this.OAssociadoAbrangenciaBL.salvar(ViewModel.AssociadoAbrangencia);

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
				UtilRetorno RetornoExclusao = this.OAssociadoAbrangenciaBL.excluirPorId(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = false;
				}
			}

            return Json(Retorno);
        }
    }
}
