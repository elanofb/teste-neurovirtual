using DAL.Organizacoes;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Empresas {

	//
	public class EmpresaPorte {

		public int id { get; set; }

		public string sigla { get; set; }

		public string descricao { get; set; }

        public DateTime? dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public EmpresaPorte() {
		}
	}

	//
	internal sealed class EmpresaPorteMapper : EntityTypeConfiguration<EmpresaPorte> {

		public EmpresaPorteMapper() {

			this.ToTable("tb_empresa_porte");

            this.HasKey(o => o.id);

			this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasOptional(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);

        }
	}
}