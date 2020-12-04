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

    public class AssociadoTituloController : BaseSistemaController{

		//Atributos
		private IAssociadoTituloBL _AssociadoTituloBL; 
		private ITipoTituloBL _TipoTituloBL;


		//Propriedades
        private IAssociadoTituloBL OAssociadoTituloBL { get { return (this._AssociadoTituloBL = this._AssociadoTituloBL ?? new AssociadoTituloBL()); } }
		private ITipoTituloBL OTipoTituloBL{ get{ return (this._TipoTituloBL = this._TipoTituloBL?? new TipoTituloBL() ); }}

        //Construtor
        public AssociadoTituloController() {

        }

		//Bloco Partial para listagem de titulos de um associado
		[HttpGet, ActionName("partial-listar-titulos")]
        public PartialViewResult partialListarTitulos(int idAssociado){
			var listaTitulos = this.OAssociadoTituloBL.listar(idAssociado, 0, "S").ToList();
            return PartialView(listaTitulos);
        }

		//Formulário Parcial para novo titulo do associado
		[HttpGet]
		public PartialViewResult editar(int? id, int? idAssociado){

			var OAssociadoTitulo = this.OAssociadoTituloBL.carregar(UtilNumber.toInt32(id)) ?? new AssociadoTitulo();
			OAssociadoTitulo.idAssociado = ( OAssociadoTitulo.idAssociado > 0? OAssociadoTitulo.idAssociado : UtilNumber.toInt32(idAssociado));

			AssociadoTituloForm ViewModel = new AssociadoTituloForm();
			ViewModel.AssociadoTitulo = OAssociadoTitulo;

			return PartialView(ViewModel);
		}


		//Formulario submetido para novo cargo para o associado
		[HttpPost]
		public ActionResult editar(AssociadoTituloForm ViewModel){

			if (!ModelState.IsValid) {
				return PartialView(ViewModel);
			}

			//Carregar o Id da Instituicao Atual do Titulo escolhido
			TipoTitulo OTipoTitulo = this.OTipoTituloBL.carregar(ViewModel.AssociadoTitulo.idTipoTitulo) ?? new TipoTitulo();
			ViewModel.AssociadoTitulo.idInstituicao = OTipoTitulo.idInstituicao;

			//Enviar cadastro para servico de persistencia
			bool flagSalvo = this.OAssociadoTituloBL.salvar(ViewModel.AssociadoTitulo);

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
				UtilRetorno RetornoExclusao = this.OAssociadoTituloBL.excluir(idExclusao);
				
				if (RetornoExclusao.flagError) { 
					Retorno.error = false;
				}
			}

            return Json(Retorno);
        }
    }
}
