using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Empresas;
using DAL.Empresas;
using WEB.Areas.Empresas.ViewModels;
using WEB.App_Infrastructure;
using PagedList;
using System.Json;
using MvcFlashMessages;
using WEB.Helpers;

namespace WEB.Areas.Empresas.Controllers{

    public class DefaultController : BaseSistemaController{

		//Constantes

		//Atributos
        private IEmpresaBL _EmpresaBL;

		//Propriedades
		private IEmpresaBL OEmpresaBL => _EmpresaBL = _EmpresaBL ?? new EmpresaBL();

        //
        public DefaultController() {
        }

		
		//Listagem para consulta de empresas existentes
		[HttpGet]
        public ActionResult listar(){
            string ativo = UtilRequest.getString("flagAtivo");
            string valorBusca = UtilRequest.getString("valorBusca");
            string flagTipoSaida = UtilRequest.getString("flagTipoSaida");

            var listaEmpresas = this.OEmpresaBL.listar(valorBusca, ativo).OrderBy(x => x.Pessoa.nome);

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OEmpresaExportacao = new EmpresaExportacao();
                OEmpresaExportacao.baixarExcel(listaEmpresas.ToList());

                return null;
            }

            return View(listaEmpresas.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        /**
		*
		*/
		[HttpGet]
        public ActionResult editar(int? id){
            
			EmpresaForm ViewModel = new EmpresaForm();
			ViewModel.Empresa = this.OEmpresaBL.carregar(UtilNumber.toInt32(id) ) ?? new Empresa();

			ViewModel.filtrarEnderecoPrincipal();

			return View(ViewModel);
        }


        /**
		*
		*/
        [HttpPost]
        public ActionResult editar(EmpresaForm ViewModel){

            if (!ModelState.IsValid) {
				return View(ViewModel);
            }          

			bool flagSucesso = this.OEmpresaBL.salvar(ViewModel.Empresa);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }
			return View(ViewModel);
        }
        
        //Excluir um ou mais registros
        [HttpPost]
        public ActionResult excluir(int[] id) {
			
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 
				bool flagSucesso = this.OEmpresaBL.excluir(idExclusao);
				
				if (!flagSucesso) { 
					Retorno.error = true;
					Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
				}
			}

            return Json(Retorno);
        }
    }
}
