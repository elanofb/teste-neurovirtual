using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Permissao.ViewModels;
using PagedList;
using BLL.Permissao;
using DAL.Permissao;
using MvcFlashMessages;

namespace WEB.Areas.Permissao.Controllers {

    public class ModulosController : Controller {

		//Constantes

		//Atributos
        private AcessoRecursoGrupoBL _AcessoRecursoGrupoBL;

		//Propriedades
		private AcessoRecursoGrupoBL ORecursoGrupoBL => (this._AcessoRecursoGrupoBL = this._AcessoRecursoGrupoBL ?? new AcessoRecursoGrupoBL());

        //Eventos

		//
        public ActionResult listar() {

            string ativo = UtilRequest.getString("flagAtivo");

            var listaPerfis = this.ORecursoGrupoBL.listar( ativo)
                                                    .OrderBy(x => x.ordem);

            return View(listaPerfis.ToPagedList(UtilRequest.getNroPagina(), 50));
        }

		[HttpGet]
		public ActionResult editar(int? id) {

			RecursoGrupoForm ViewModel = new RecursoGrupoForm();

			AcessoRecursoGrupo ORecursoGrupo = this.ORecursoGrupoBL.carregar(UtilNumber.toInt32(id)) ?? new AcessoRecursoGrupo();

			ViewModel.AcessoRecursoGrupo = ORecursoGrupo;

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(RecursoGrupoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			this.ORecursoGrupoBL.salvar(ViewModel.AcessoRecursoGrupo);

		    if (ViewModel.AcessoRecursoGrupo.id > 0) {
		        
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Dados atualizados!", "Os dados foram salvos com sucesso!") );

                return RedirectToAction("listar");
		    }
			

			return View(ViewModel);
		}
	}
}