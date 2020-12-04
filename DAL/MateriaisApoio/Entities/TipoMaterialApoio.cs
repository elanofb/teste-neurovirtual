using DAL.Entities;
using DAL.Organizacoes;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.MateriaisApoio {

	//
	public class TipoMaterialApoio {
		public byte id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public string descricao { get; set; }

		public DateTime? dtCadastro { get; set; }
        
		public int idUsuarioCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }

		public string flagSistema { get; set; }
	}

	//
	internal sealed class TipoMaterialApoioMapper : EntityTypeConfiguration<TipoMaterialApoio> {

		public TipoMaterialApoioMapper() {
			this.ToTable("tb_tipo_material_apoio");
			this.HasKey(o => o.id);
            this.Property(c => c.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
        }
	}
}