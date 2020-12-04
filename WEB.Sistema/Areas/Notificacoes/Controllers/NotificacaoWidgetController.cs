using System;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using BLL.Notificacoes;
using BLL.Notificacoes.Services;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.Notificacoes.ViewModels;

namespace WEB.Areas.Notificacoes.Controllers {

	public class NotificacaoWidgetController : BaseSistemaController {

        //Atributos
        private INotificacaoSistemaEnvioBL _INotificacaoSistemaEnvioBL;
        private INotificacaoLeituraBL _NotificacaoLeituraBL;

        //Propriedades
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => _INotificacaoSistemaEnvioBL = _INotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();
        private INotificacaoLeituraBL ONotificacaoLeituraBL => this._NotificacaoLeituraBL = this._NotificacaoLeituraBL ?? new NotificacaoLeituraBL();

        //Listagem de mensagens para o usuario
        [ActionName("partial-mensagens")]
        public PartialViewResult partialMensagens() {

            return PartialView();
        }

		//Listagem de avisos para o usuario
		[ActionName("partial-avisos")]
		public PartialViewResult partialAvisos(){

		    var listaNotificacoes = ONotificacaoSistemaEnvioBL.listar(User.id(), 0)
                                                                .Where(x => x.dtLeitura == null && x.flagExcluido == false)
                                                                .Select(x => new ItemNotificacaoWidget{
                                                                    id = x.id,
                                                                    titulo = x.NotificacaoSistema.titulo,
                                                                    notificacao = x.NotificacaoSistema.notificacao
                                                                }).ToList();

            return PartialView(listaNotificacoes);
        }

		[ActionName("registrar-leitura")]
		public ActionResult registrarLeitura(int id) {

			UtilRetorno Retorno = new UtilRetorno();

            Retorno.flagError = false;

			this.ONotificacaoLeituraBL.registrarLeitura(id);

		    var listaNotificacoes = ONotificacaoSistemaEnvioBL.listar(User.id(), 0)
                                                                .Where(x => x.dtLeitura == null && x.flagExcluido == false)
                                                                .Select(x => new ItemNotificacaoWidget{
                                                                    id = x.id,
                                                                    titulo = x.NotificacaoSistema.titulo,
                                                                    notificacao = x.NotificacaoSistema.notificacao
                                                                }).ToList();

            return PartialView("partial-avisos", listaNotificacoes);
		}

    }
}

