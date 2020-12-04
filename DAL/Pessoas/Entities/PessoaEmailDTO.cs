using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Emails;

namespace DAL.Pessoas {

	//
	[Serializable]
	public class PessoaEmailDTO  {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

		public int idPessoa { get; set; }

		public virtual PessoaDTO Pessoa { get; set; }

		public byte? idTipoEmail { get; set; }

		public virtual TipoEmail TipoEmail { get; set; }

		public string email { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public string motivoExclusao { get; set; }
	}
}