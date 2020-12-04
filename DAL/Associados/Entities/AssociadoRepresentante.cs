using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Associados {

	[Serializable]
	public class AssociadoRepresentante : DefaultEntity {

		public int idAssociado { get; set; }

		public virtual Associado Associado { get; set; }

        public int idTipoAssociadoRepresentante { get; set; }

		public virtual TipoAssociadoRepresentante TipoAssociadoRepresentante { get; set; }

		public string nome { get; set; }

		public string cpf { get; set; }

		public string rg { get; set; }

		public string ddiTelPrincipal { get; set; }

		public string dddTelPrincipal { get; set; }

		public string nroTelPrincipal { get; set; }

		public string emailPrincipal { get; set; }

		public string flagRepresentantaAssociacao { get; set; }
	}

	//
	internal sealed class AssociadoRepresentanteMapper : EntityTypeConfiguration<AssociadoRepresentante> {

		public AssociadoRepresentanteMapper() {
			this.ToTable("tb_associado_representante");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Associado).WithMany().HasForeignKey(o => o.idAssociado);
			this.HasRequired(o => o.TipoAssociadoRepresentante).WithMany().HasForeignKey(o => o.idTipoAssociadoRepresentante);
		}
	}
}