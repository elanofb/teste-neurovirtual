using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Notificacoes;
using BLL.Notificacoes.Services;
using DAL.Permissao.Security.Extensions;
using PagedList;

namespace WEB.Areas.AvisosNotificacoes.Controllers {

    public class AvisoSistemaListaController : Controller {

        // Atributos
        private INotificacaoLeituraBL _NotificacaoLeituraBL;
        private INotificacaoSistemaEnvioBL _INotificacaoSistemaEnvioBL;

        //Propriedades
        private INotificacaoLeituraBL ONotificacaoLeituraBL => this._NotificacaoLeituraBL = this._NotificacaoLeituraBL ?? new NotificacaoLeituraBL();
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => _INotificacaoSistemaEnvioBL = _INotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();
        

		public ActionResult index(){

		    var listaNotificacoes = ONotificacaoSistemaEnvioBL.listar(User.id(), 0)
                                                        .Where(x => x.dtLeitura == null && x.flagExcluido == false)
                                                        .OrderByDescending(x => x.dtCadastro).ToList();

            return View(listaNotificacoes);
        }

        [ActionName("avisos-lidos")]
        public ActionResult avisosLidos() {

            var listaNotificacoes = ONotificacaoSistemaEnvioBL.listar(User.id(), 0)
                .Where(x => x.dtLeitura.HasValue && x.flagExcluido == false).OrderByDescending(x => x.dtCadastro)
                .ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(listaNotificacoes);
        }

        [HttpPost, ActionName("registrar-leitura")]
		public ActionResult registrarLeitura() {
            
            var ids = UtilRequest.getListInt("id");

            foreach (var id in ids) {
                this.ONotificacaoLeituraBL.registrarLeitura(id);
            }

            return Json(new {error = false});
        }
    }
}