using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using BLL.Core.Events;
using BLL.Services;
using DAL.Entities;
using DAL.Permissao.Security.Extensions;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using MimeKit;

namespace BLL.Email {

	public class EmailBL : DefaultBL, IEmailBL {

		//Constantes
		public static string FLAG_EMAIL_ENVIADO = "E";

		public static string FLAG_EMAIL_RECEBIDO = "R";

	    //Eventos
		private readonly EventAggregator onEmailRecebido = OnEmailRecebido.getInstance;

		private readonly EventAggregator onEmailEnviado = OnEmailEnviado.getInstance;

		/**
		* Construtor
		*/

		public EmailBL() {
			//Registrar assinante do evento de recebimento de e-mails
			this.onEmailRecebido.subscribe(new EmailRecebidoHandler());

			//Registrar assinante do evento de envio de e-mails
			this.onEmailEnviado.subscribe(new EmailEnviadoHandler());
		}

		/**
		*
		*/
		public LogEmail carregar(int id) {
			var query = from Em in db.LogEmail
						where Em.id == id
						select Em;

			return query.FirstOrDefault();
		}

		/**
		*
		*/
		public IQueryable<LogEmail> listar(string flagFluxo, string valorBusca, string flagLixeira) {

			var query = from Em in db.LogEmail.Include(x => x.listaEmailDestino)
						select Em;

			if (!String.IsNullOrEmpty(flagFluxo)) {
				query = query.Where(x => x.flagFluxo == flagFluxo);
			}

			if (flagLixeira.Equals("S")) {
				query = query.Where(x => x.dtExclusao.HasValue);
			}

			if (flagLixeira.Equals("N")) {
				query = query.Where(x => !x.dtExclusao.HasValue);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.emailRemetente.Contains(valorBusca) || x.nomeRemetente.Contains(valorBusca) || x.corpoMensagem.Contains(valorBusca) || x.assunto.Contains(valorBusca));
			}

			return query;
		}

		/**
		*
		*/
		public IQueryable<EmailContatoVW> listarContatos(string emailOrigem) {

			var query = from Em in db.EmailContatoVW.AsNoTracking()
						where Em.email != emailOrigem
						select Em;
			return query;
		}

		/**
		 * Conectar no servidor para fazer o download dos e-mails
		*/

		public async Task<List<LogEmail>> downloadAsync(string host, int nroPorta, bool flagSSL, string usuario, string senha, List<string> emailJaBaixados) {
			//Criar lista para armazenar as novas mensagens.
			List<LogEmail> listaNovasMensagens = new List<LogEmail>();

			Task<List<LogEmail>> OTask = new Task<List<LogEmail>>(() => {
				using (MailKit.Net.Pop3.Pop3Client client = new MailKit.Net.Pop3.Pop3Client()) {
					client.Connect(host, nroPorta, flagSSL);
					client.Authenticate(usuario, senha);
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					int limit = client.Count;
					int lastcount;

					for (int i = 0; i < limit; i++) {
						lastcount = (limit - 1) - i;
						var NovaMensagem = client.GetMessage(lastcount);

						LogEmail NovoEmail = this.mapper(NovaMensagem, lastcount);

						bool flagSucesso = this.salvarEmail(NovoEmail);

						if (flagSucesso) {
							client.DeleteMessage(lastcount);
						}
					}

					client.Disconnect(true);
				}

				return listaNovasMensagens;
			});

			OTask.Start();

			try {
				listaNovasMensagens = await OTask;
			} catch (Exception ex) {
				UtilLog.saveError(ex.GetBaseException(), String.Concat(ex.Message, "\n", ex.StackTrace));
			}

			return listaNovasMensagens;
		}

		/**
		*
		*/

		private LogEmail mapper(MimeMessage NovaMensagem, int nroEmail) {
			LogEmail NovoEmail = new LogEmail();
			NovoEmail.idEmailServidor = NovaMensagem.MessageId;
			NovoEmail.assunto = NovaMensagem.Subject;
			NovoEmail.nomeRemetente = (NovaMensagem.Sender != null ? NovaMensagem.Sender.Name : NovaMensagem.From.FirstOrDefault().Name);
			NovoEmail.emailRemetente = (NovaMensagem.Sender != null ? NovaMensagem.Sender.Address : NovaMensagem.From.Mailboxes.FirstOrDefault().Address);
			NovoEmail.corpoMensagem = ((NovaMensagem.HtmlBody ?? NovaMensagem.TextBody) ?? "");
			NovoEmail.nroEmailServidor = nroEmail;
			NovoEmail.dtEnvio = NovaMensagem.Date.DateTime;

			NovoEmail.flagFluxo = "R"; //Fluxo de Recebimento

			if (NovaMensagem.ReplyTo.Count > 0) {
				NovoEmail.nomeResposta = NovaMensagem.ReplyTo.FirstOrDefault().Name;
				NovoEmail.emailResposta = NovaMensagem.ReplyTo.Mailboxes.FirstOrDefault().Address;
			}

			var ReturnPath = NovaMensagem.Headers.FirstOrDefault(x => x.Id == MimeKit.HeaderId.ReturnPath);
			if (ReturnPath != null) {
				NovoEmail.returnPath = UtilString.onlyEmailChars(ReturnPath.Value);
			}

			if (NovaMensagem.To != null) {
				foreach (var EmailTo in NovaMensagem.To.Mailboxes) {
					LogEmailDestino ODestino = new LogEmailDestino();
					ODestino.emailDestino = EmailTo.Address;
					ODestino.nomeDestino = EmailTo.Name;
					ODestino.flagCopia = "N";
					ODestino.flagCopiaOculta = "N";
					NovoEmail.listaEmailDestino.Add(ODestino);
				}
			}

			if (NovaMensagem.Cc != null) {
				foreach (var EmailTo in NovaMensagem.Cc.Mailboxes) {
					LogEmailDestino ODestino = new LogEmailDestino();
					ODestino.emailDestino = EmailTo.Address;
					ODestino.nomeDestino = EmailTo.Name;
					ODestino.flagCopia = "S";
					ODestino.flagCopiaOculta = "N";
					NovoEmail.listaEmailDestino.Add(ODestino);
				}
			}

			if (NovaMensagem.Bcc != null) {
				foreach (var EmailTo in NovaMensagem.Bcc.Mailboxes) {
					LogEmailDestino ODestino = new LogEmailDestino();
					ODestino.emailDestino = EmailTo.Address;
					ODestino.nomeDestino = EmailTo.Name;
					ODestino.flagCopia = "S";
					ODestino.flagCopiaOculta = "N";
					NovoEmail.listaEmailDestino.Add(ODestino);
				}
			}

			return NovoEmail;
		}

		/**
		* Salvar o e-mail e os destinatários
		*/

		public bool salvarEmail(LogEmail NovoEmail) {
			NovoEmail.setDefaultInsertValues<LogEmail>();
			NovoEmail.listaEmailDestino.ForEach(x => { x.setDefaultInsertValues<LogEmailDestino>(); });

			this.db.LogEmail.Add(NovoEmail);
			this.db.SaveChanges();

			bool flagEnviado = (NovoEmail.id > 0);

			//Disparar o Evento do envio de e-mail
			if (flagEnviado) {
				this.onEmailEnviado.publish(NovoEmail);
			}

			return flagEnviado;
		}

		/**
		* Salvar usuário e data da primeira visualizacao
		*/

		public void registrarLeitura(int id) {

		    var idUsuario = User.id();

			this.db.LogEmail
					.Where(x => x.id == id)
					.Update(x => new LogEmail { dtPrimeiraAbertura = DateTime.Now, idUsuarioPrimeiraAbertura = idUsuario });
		}

		/**
		* Enviar uma mensagem para a lixeira
		*/

		public void lixeira(int id) {

            var idUsuario = User.id();

            this.db.LogEmail
					.Where(x => x.id == id)
					.Update(x => new LogEmail { dtExclusao = DateTime.Now, idUsuarioExclusao = idUsuario });
		}

		/**
		* Retirar uma mensagem da lixeira
		*/

		public void restaurar(int id) {
			this.db.LogEmail
					.Where(x => x.id == id)
					.Update(x => new LogEmail { dtExclusao = null });
		}

		/**
		* Exclusao em definitivo de uma mensagem
		*/

		public bool excluir(int id) {
			this.db.LogEmailDestino.Where(x => x.idEmail == id).Delete();
			this.db.LogEmail.Where(x => x.id == id).Delete();

			int qtde = this.db.LogEmail.Count(x => x.id == id);
			return (qtde == 0);
		}
	}
}