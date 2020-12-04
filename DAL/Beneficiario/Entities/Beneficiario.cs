using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Empresas;
using DAL.Entities;
using DAL.Pessoas;

namespace DAL.Beneficiarios {

	//
	[Serializable]
	public class Beneficiario : DefaultEntity {

		public int? idEmpresa { get; set; }

		public virtual Empresa Empresa { get; set; }

		public int? idResponsavel { get; set; }

		public virtual Beneficiario Responsavel { get; set; }

		public int idPessoa { get; set; }

		public virtual Pessoa Pessoa { get; set; }

		public string apelido { get; set; }

		public string flagInformativosOnline { get; set; }

		public string observacoes { get; set; }

		public Beneficiario() {
		}
	}

	//
	internal sealed class BeneficiarioMapper : EntityTypeConfiguration<Beneficiario> {

		public BeneficiarioMapper() {
			this.ToTable("tb_beneficiario");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.Pessoa).WithMany().HasForeignKey(o => o.idPessoa);
			this.HasOptional(o => o.Empresa).WithMany().HasForeignKey(o => o.idEmpresa);
			this.HasOptional(o => o.Responsavel).WithMany().HasForeignKey(o => o.idResponsavel);
		}
	}
}