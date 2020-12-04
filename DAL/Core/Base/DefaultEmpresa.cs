using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities {

	public class DefaultEmpresa {

		[Key]
		public virtual int id { get; set; }

		public virtual string razaoSocial { get; set; }

		public virtual string nomeFantasia { get; set; }

		public virtual string cnpj { get; set; }

		public virtual string inscricaoEstadual { get; set; }

		public virtual string inscricaoMunicipal { get; set; }

		public virtual string telComercial { get; set; }

		public virtual string email { get; set; }

		public virtual DateTime dtCadastro { get; set; }

		public virtual DateTime dtAlteracao { get; set; }

		public virtual int? idUsuarioCadastro { get; set; }

		public virtual int? idUsuarioAlteracao { get; set; }

		public virtual string ativo { get; set; }

		public virtual string flagSistema { get; set; }

		public virtual string flagExcluido { get; set; }
	}
}