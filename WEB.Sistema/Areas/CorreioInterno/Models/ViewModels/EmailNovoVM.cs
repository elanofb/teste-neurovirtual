using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using BLL.Email;
using FluentValidation;
using FluentValidation.Attributes;

namespace WEB.Areas.CorreioInterno.ViewModels {

	[Validator(typeof(EmailNovoValidator))]
	public class EmailNovoVM {

		//Fields
		private IEmailBL _EmailBL;
		
		//Propriedades
		public IEmailBL OEmailBL {
			get{ return (this._EmailBL = this._EmailBL ?? new EmailBL() ); }
		}
		
		public List<string> listaDestinos { get; set;}
		public List<string> listaCopias { get; set;}
		public List<string> listaCopiasOcultas { get; set;}

		[AllowHtml]
		public string corpoMensagem { get; set;}

		public string emailRemetente { get; set;}
		public string assunto { get; set;}
		public string flagEnviado { get; set;}


		//Construtor
		public EmailNovoVM() {
			this.listaDestinos = new List<string>();
			this.listaCopias = new List<string>();
			this.listaCopiasOcultas = new List<string>();
		}

		/**
		* Carrega os e-mails que ja foram incluídos para pre carregar o formulario
		* Isso evita que o usuário tenha que digitar os e-mails novamente caso a ViewModel nao seja validada
		*/
		public List<SelectListItem> listarSelecionados(List<string> listaEmails) {
			
			List<SelectListItem> listaContatos = this.OEmailBL.listarContatos(this.emailRemetente)
					.Where( x => listaEmails.Contains(x.email))
					.Select(x => new SelectListItem{
										Text = (String.IsNullOrEmpty(x.nome)? x.email: String.Concat(x.nome, " - ", x.email)),
										Value = x.email
					}).ToList();
			return listaContatos;
		}

		/**
		* Caso haja algum e-mail informado que ainda nao exista na base de dados, incluímos novamente na lista
		*/
		private void incluirNovosEnderecos(ref List<SelectListItem> listaContatos, List<string> listaEmails) {
			var listaAtual = listaContatos;
			var listaNovos = listaEmails.Where(x => !listaAtual.Where(l => l.Value == x).Any() ).ToList();

			if (listaNovos.Count > 0) {
				foreach (string novoEmail in listaNovos) { 
					listaContatos.Add(new SelectListItem{ Value = novoEmail, Text = novoEmail});
				}
			}
		}

		//
		public MultiSelectList comboDestinos() { 
			List<SelectListItem> listaJaPreenchidos = this.listarSelecionados(this.listaDestinos);
			this.incluirNovosEnderecos(ref listaJaPreenchidos, this.listaDestinos);

			return new MultiSelectList(listaJaPreenchidos, "Value", "Text", this.listaDestinos.ToArray());
		}

		//
		public MultiSelectList comboCopias() { 
			List<SelectListItem> listaJaPreenchidos = this.listarSelecionados(this.listaCopias);
			this.incluirNovosEnderecos(ref listaJaPreenchidos, this.listaCopias);

			return new MultiSelectList(listaJaPreenchidos, "Value", "Text", this.listaCopias.ToArray());
		}

		//
		public MultiSelectList comboCopiasOcultas() { 
			List<SelectListItem> listaJaPreenchidos = this.listarSelecionados(this.listaCopiasOcultas);
			this.incluirNovosEnderecos(ref listaJaPreenchidos, this.listaCopiasOcultas);

			return new MultiSelectList(listaJaPreenchidos, "Value", "Text", this.listaCopiasOcultas.ToArray());
		}
	}

	/**
	*
	*/
	internal class EmailNovoValidator : AbstractValidator<EmailNovoVM> {

		//
		public EmailNovoValidator() {

			RuleFor(x => x.emailRemetente).NotEmpty().WithMessage("O e-mail usado para envio deve ser informado.");
			RuleFor(x => x.assunto).NotEmpty().WithMessage("Informe o assunto para o e-mail.");
			RuleFor(x => x.corpoMensagem).NotEmpty().WithMessage("Informe o conteúdo de sua mensagem.");
			RuleFor(x => x.listaDestinos).Must((m, listaDestinos) => (listaDestinos.Count > 0)).WithMessage("Informe para quem o e-mail será enviado.");
		}
	}
}