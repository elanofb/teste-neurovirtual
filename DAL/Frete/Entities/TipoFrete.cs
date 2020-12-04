using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Frete {

	public class TipoFrete {

		public int id { get; set; }

		public int idTransportador { get; set; }

		public virtual Transportador Transportador { get; set; }

		public string descricao { get; set; }

		public string descricaoCliente { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool ativo { get; set; }

		public bool flagExcluido { get; set; }
	}

	//
	internal sealed class TipoFreteMapper : EntityTypeConfiguration<TipoFrete> {

		public TipoFreteMapper() {

			this.ToTable("datatb_tipo_frete");

			this.HasKey(o => o.id);

			this.HasRequired(o => o.Transportador).WithMany().HasForeignKey(o => o.idTransportador);
		}
	}
}