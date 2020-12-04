using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;

namespace DAL.Funcionarios {

	//
	public class FuncionarioOcorrencia {
        public int id { get; set; }		
        public string observacao { get; set; }
        public DateTime? dtOcorrencia { get; set; }
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
	internal sealed class FuncionarioOcorrenciaMapper : EntityTypeConfiguration<FuncionarioOcorrencia> {

		public FuncionarioOcorrenciaMapper() {
			this.ToTable("tb_funcionario_Ocorrencia");
			this.HasKey(o => o.id);

            //FKs
			this.HasRequired(x => x.Funcionario).WithMany().HasForeignKey(o => o.idFuncionario);
		}
	}
}