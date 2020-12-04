using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.RamosAtividade {

	//
	public class RamoAtividade {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

		public string descricao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool ativo { get; set; }

		public bool flagExcluido { get; set; }

		public bool flagSistema { get; set; }
	}

	//
	internal sealed class RamoAtividadeMapper : EntityTypeConfiguration<RamoAtividade> {

		public RamoAtividadeMapper() {

			this.ToTable("tb_ramo_atividade");

			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}