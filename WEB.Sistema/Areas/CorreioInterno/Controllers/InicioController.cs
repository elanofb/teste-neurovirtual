using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using BLL.Email;
using DAL.Entities;
using WEB.App_Infrastructure;
using PagedList;
using WEB.Areas.CorreioInterno.ViewModels;
using System.Threading.Tasks;
using BLL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.CorreioInterno.Controllers {

	//[RouteArea("CorreioInterno", AreaPrefix = "correio-interno")]
	public class InicioController : BaseSistemaController {

		//Atributos
		private IEmailBL _EmailBL { get; set;}

		//Propriedades

		private IEmailBL OEmailBL { 
			get{ return (this._EmailBL = this._EmailBL ?? new EmailBL() ); }
		}

		//Eventos
		//private EventAggregator _onEmailEnviado;



		//
		public ActionResult index() { 
			return View();
		}


		//
		[ActionName("caixa-de-entrada")]
		public async Task<ActionResult> caixaDeEntrada(bool? flagBuscarEmails = true, string valorBusca = "") {
			
			var Config = ConfiguracaoEmailBL.getInstance.carregar();

			try {

				if (flagBuscarEmails == true) {
					await this.OEmailBL.downloadAsync(Config.servidorPOPEmailSistema, UtilNumber.toInt32(Config.portaPOPEmailSistema), (Config.flagSSLPOPEmailSistema == "S"? true: false), Config.contaEmailSistema, Config.senhaEmailSistema, null);
				}

			}catch(Exception ex){
				UtilLog.saveError(ex, "Falha ao baixar e-mails.");
			}

			ViewBag.actionPaginacao = "caixa-de-entrada";
			var listaEmails = this.OEmailBL.listar("R", valorBusca, "N").OrderByDescending(x => x.dtEnvio);
			return View(listaEmails.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}


		/**
		 *
		 */
		public ActionResult lixeira() { 
			return View();
		}

		/**
		 *
		 */
		[ActionName("caixa-emails-lixeira")]
		public ActionResult caixaEmailsLixeira(string valorBusca = "") {
			var listaEmails = this.OEmailBL.listar("R", valorBusca, "S").OrderByDescending(x => x.dtEnvio);
			
			ViewBag.actionPaginacao = "caixa-emails-lixeira";
			return View(listaEmails.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		/**
		 *
		 */
		public ActionResult enviados() { 
			return View();
		}

		//
		[ActionName("caixa-emails-enviados")]
		public ActionResult caixaEmailsEnviados(string valorBusca = "") {
			var listaEmails = this.OEmailBL.listar("E", valorBusca, "N").OrderByDescending(x => x.dtEnvio);
			
			ViewBag.actionPaginacao = "caixa-emails-enviados";
			return View(listaEmails.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//Action para exibição de formulário de envio de e-mail (novo, resposta, encaminhamento)
		[ActionName("criar-novo-email"), HttpGet, ValidateInput(false)]
		public ActionResult criarNovoEmail(int? idEmailEncaminhamento, int? idEmailResposta) {

			var Config = ConfiguracaoEmailBL.getInstance.carregar();

			EmailNovoVM ViewModel = new EmailNovoVM();
			ViewModel.emailRemetente = Config.contaEmailSistema;

			//Se for uma resposta ou encaminhamento preencher campos padrão adicionando espaço para nova mensagem.
			int? idEmail = (idEmailEncaminhamento ?? idEmailResposta);
			var Email = this.OEmailBL.carregar(UtilNumber.toInt32(idEmail));
			if (Email != null) { 
				ViewModel.assunto = Email.assunto;
				ViewModel.corpoMensagem = String.Concat("<br /><br /><br />", "___________________________________________________________________________________",Email.corpoMensagem);
				
				//Se for uma resposta é necessário já selecionar o endereço de destino
				if(UtilNumber.toInt32(idEmailResposta) > 0){
					ViewModel.listaDestinos.Add(Email.emailRemetente);
				}
			}

			return View(ViewModel);
		}

		//
		[ActionName("enviar-email"), HttpPost, ValidateInput(false)]
		public ActionResult enviarEmail(EmailNovoVM ViewModel) {

			string nomeView = "criar-novo-email";

			if (!ModelState.IsValid) { 

				return View(nomeView, ViewModel);

			}

		    var ConfiguracaoSistema = ConfiguracaoSistemaBL.getInstance.carregar(HttpContextFactory.Current.User.idOrganizacao());

			LogEmail NovoEmail = new LogEmail();

			NovoEmail.flagFluxo = EmailBL.FLAG_EMAIL_ENVIADO;

            NovoEmail.emailRemetente = ViewModel.emailRemetente;

            NovoEmail.nomeRemetente = ConfiguracaoSistema.tituloSistema;

            NovoEmail.assunto = ViewModel.assunto;

            NovoEmail.corpoMensagem = ViewModel.corpoMensagem;

            NovoEmail.dtEnvio = DateTime.Now;

			//Buscar dados completos dos contatos (se houver)
			List<string> listaEmails = new List<string>();
			listaEmails.AddRange(ViewModel.listaDestinos);
			listaEmails.AddRange(ViewModel.listaCopias);
			listaEmails.AddRange(ViewModel.listaCopiasOcultas);
			var listaContatos = this.OEmailBL.listarContatos("").Where(x => listaEmails.Contains(x.email)).ToList();

			ViewModel.listaDestinos.ForEach(x => {
				var Contato = listaContatos.Where(c => c.email == x).FirstOrDefault();
				NovoEmail.listaEmailDestino.Add(new LogEmailDestino{ nomeDestino = (Contato == null? "": Contato.nome), emailDestino = (Contato == null? x: Contato.email), flagCopia = "N", flagCopiaOculta = "N"  });
			});

			ViewModel.listaCopias.ForEach(x => {
				var Contato = listaContatos.FirstOrDefault(c => c.email == x);
				NovoEmail.listaEmailDestino.Add(new LogEmailDestino{ nomeDestino = (Contato == null? "": Contato.nome), emailDestino = (Contato == null? x: Contato.email), flagCopia = "S", flagCopiaOculta = "N"  });
			});

			ViewModel.listaCopiasOcultas.ForEach(x => {
				var Contato = listaContatos.FirstOrDefault(c => c.email == x);
				NovoEmail.listaEmailDestino.Add(new LogEmailDestino{ nomeDestino = (Contato == null? "": Contato.nome), emailDestino = (Contato == null? x: Contato.email), flagCopia = "N", flagCopiaOculta = "S"  });
			});

			EnvioEmailAdapter EmailAdapter = EnvioCorreioInterno.factory(User.idOrganizacao(), ViewModel.listaDestinos, ViewModel.listaCopias, ViewModel.listaCopiasOcultas);
			
			var ORetorno = EmailAdapter.enviar(new Dictionary<string, object>{ {"mensagem", NovoEmail.corpoMensagem} }, NovoEmail.assunto);
			
			if (ORetorno.flagError) { 
	
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível enviar o e-mail!");

				return View(nomeView, ViewModel);
			}

			if (!this.OEmailBL.salvarEmail(NovoEmail)) { 

				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o e-mail enviado!");

				return View(nomeView, ViewModel);
			}
				
			return Content("OK");
		}

		//
		[ActionName("visualizar-email")]
		public ActionResult visualizarEmail(int id, bool flagConfiavel = false) {
			LogEmail OEmail = this.OEmailBL.carregar(id);
			ViewBag.flagConfiavel = flagConfiavel;

			return View(OEmail);
		}

		//
		[ActionName("registrar-visualizacao")]
		public ActionResult registrarVisualizacao(int id) {
			LogEmail OEmail = this.OEmailBL.carregar(id);
			if (OEmail == null) { 
				return Json(new { flagErro = true, message = "Registro não localizado"}, JsonRequestBehavior.AllowGet);
			}

			if (OEmail.dtPrimeiraAbertura.HasValue) { 
				return Json(new { flagErro = true, message = "Visualização já registrada."}, JsonRequestBehavior.AllowGet);
			}

			this.OEmailBL.registrarLeitura(id);

			return Json(new { flagErro = false, message = "Visualização registrada com sucesso!"}, JsonRequestBehavior.AllowGet);
		}

		//
		[ActionName("carregar-emails")]
		public ActionResult carregarEmails(string term) {
			
			var Config = ConfiguracaoEmailBL.getInstance.carregar();
			string emailOrigem = Config.contaEmailSistema;

			var lista = this.OEmailBL.listarContatos(emailOrigem)
									.Where(x => x.email.Contains(term) || x.nome.Contains(term))
									.OrderBy(x => x.email)
									.Take(50)
									.Select(x => new { 
													id = x.email, 
													label = x.email,
													text = (String.IsNullOrEmpty(x.nome) || x.email.Equals(x.nome) ? x.email: String.Concat(x.nome, " - ", x.email))
									}).ToList();

			return Json(new{ listaEmails = lista}, JsonRequestBehavior.AllowGet);
		}

		//
		[ActionName("carregar-totais")]
		public ActionResult carregarTotais() {

			var lista = this.OEmailBL.listar("", "", "");

			var Retorno = new { 
				qtdeNaoLidos = lista.Where(x => x.flagFluxo == "R" && x.dtPrimeiraAbertura == null && x.dtExclusao == null).Count(),
				qtdeLixeira = lista.Where(x => x.dtExclusao != null && x.dtPrimeiraAbertura == null).Count(),
				//qtdeEnviados = lista.Where(x => x.flagFluxo == "E" && x.dtExclusao == null).Count()
			}; 

			return Json(Retorno, JsonRequestBehavior.AllowGet);
		}

		//
		[ActionName("enviar-para-lixeira")]
		public ActionResult enviarParaLixeira(int id) {
			LogEmail OEmail = this.OEmailBL.carregar(id);

			if (OEmail == null) { 
				return Json(new { flagErro = true, message = "Registro não localizado"}, JsonRequestBehavior.AllowGet);
			}

			if (OEmail.dtExclusao.HasValue) { 
				return Json(new { flagErro = true, message = "Mensagem já na lixeira."}, JsonRequestBehavior.AllowGet);
			}

			this.OEmailBL.lixeira(id);

			return Json(new { flagErro = false, message = "Mensagem enviada para a lixeira com sucesso!"}, JsonRequestBehavior.AllowGet);
		}

		//
		[ActionName("restaurar-mensagem")]
		public ActionResult restaurarMensagem(int id) {
			LogEmail OEmail = this.OEmailBL.carregar(id);

			if (OEmail == null) { 
				return Json(new { flagErro = true, message = "Registro não localizado"}, JsonRequestBehavior.AllowGet);
			}

			if (!OEmail.dtExclusao.HasValue) { 
				return Json(new { flagErro = true, message = "Mensagem não está na lixeira no momento."}, JsonRequestBehavior.AllowGet);
			}

			this.OEmailBL.restaurar(id);

			return Json(new { flagErro = false, message = "Mensagem recuperada da lixeira com sucesso!"}, JsonRequestBehavior.AllowGet);
		}
	}
}
