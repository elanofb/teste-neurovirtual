using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Telefones {

	//
	public class OperadoraTelefonia {

		public int id { get; set; }

		public string nome { get; set; }

		public bool? ativo { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public OperadoraTelefonia() {
		}
	}

	//
	internal sealed class OperadoraTelefoniaMapper : EntityTypeConfiguration<OperadoraTelefonia> {

		public OperadoraTelefoniaMapper() {

			this.ToTable("datatb_telefone_operadora");

            this.HasKey(o => o.id);
		}
	}
}