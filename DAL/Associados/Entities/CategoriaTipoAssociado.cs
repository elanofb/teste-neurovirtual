using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Associados {

	[Serializable]
	public class CategoriaTipoAssociado : Geral {

        public int? idOrganizacao { get; set; }

        public string observacao { get; set; }

		public string flagSistema { get; set; }
	}

	internal sealed class CategoriaTipoAssociadoMapper : EntityTypeConfiguration<CategoriaTipoAssociado> {

		public CategoriaTipoAssociadoMapper() {
			this.ToTable("tb_categoria_tipo_associado");
			this.HasKey(o => o.id);
		}
	}
}