using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Contatos;

namespace DAL.Funcionarios {

	//
	public class FuncionarioFerias {

		public int id { get; set; }		
        public string observacao { get; set; }		
        public DateTime? dtInicioPeriodoAquisitivo { get; set; }
        public DateTime? dtFimPeriodoAquisitivo { get; set; }
        public DateTime? dtInicioFerias { get; set; }
        public DateTime? dtFimFerias { get; set; }
        public string flagAbono10Dias { get; set; }		
        public string flagAntecipa13Salario { get; set; }		
		public int idFuncionario { get; set; }
		public virtual Funcionario Funcionario { get; set; }
		public DateTime? dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class FuncionarioFeriasMapper : EntityTypeConfiguration<FuncionarioFerias> {

		public FuncionarioFeriasMapper() {
			this.ToTable("tb_funcionario_ferias");
			this.HasKey(o => o.id);

			//FKs
			this.HasRequired(x => x.Funcionario).WithMany().HasForeignKey(o => o.idFuncionario);
		}
	}
}