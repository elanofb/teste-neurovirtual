using System.Collections.Generic;
using FluentValidation.Attributes;
using DAL.Permissao;

namespace WEB.Areas.Permissao.ViewModels {


	[Validator(typeof(AcessoRecursoValidator))]
	public class AcessoRecursoForm {

		public int? id { get; set; }

		public int? idPerfilAcesso { get; set; }

		public string descricaoPerfil { get; set; }

		public int? idRecursoGrupo { get; set; }

		public int? idRecursoPai { get; set; }

		public string area { get; set; }

		public string controller { get; set; }

		public string nomeDisplay { get; set; }

		public string actionPadrao { get; set; }

		public string descricao { get; set; }

		public string flagAcessoLiberado { get; set; }

		public string flagSistema { get; set; }

		public bool? flagMenu { get; set; }

		public IList<AcessoRecursoAcao> listRecursoAcao { get; set; }

		public IList<RecursoPermissaoVW> listaPermissoes { get; set; }
		
		//Construtor
		public AcessoRecursoForm() {
			this.listRecursoAcao = new List<AcessoRecursoAcao>();
			this.listaPermissoes = new List<RecursoPermissaoVW>();
		}
	}

}