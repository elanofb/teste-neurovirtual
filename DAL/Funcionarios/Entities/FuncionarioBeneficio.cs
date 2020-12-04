using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;
using DAL.Beneficios;

namespace DAL.Funcionarios {

	//
	public class FuncionarioBeneficio {
		public int id { get; set; }		
        public string duracao { get; set; }		
        public decimal participacao { get; set; }		
        public DateTime? dtAdesao { get; set; }
        public DateTime? dtTermino { get; set; }
		public int idFuncionario { get; set; }
		public virtual Funcionario Funcionario { get; set; }
		public int idBeneficio { get; set; }
		public virtual Beneficio Beneficio { get; set; }

		public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class FuncionarioBeneficioMapper : EntityTypeConfiguration<FuncionarioBeneficio> {

		public FuncionarioBeneficioMapper() {
			this.ToTable("tb_funcionario_beneficio");
			this.HasKey(o => o.id);

			//FKs
			this.HasRequired(x => x.Funcionario).WithMany().HasForeignKey(o => o.idFuncionario);
			this.HasRequired(x => x.Beneficio).WithMany().HasForeignKey(o => o.idBeneficio);

		}
	}
}