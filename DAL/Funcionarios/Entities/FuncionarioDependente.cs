using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Contatos;

namespace DAL.Funcionarios {

	//
	public class FuncionarioDependente {

		public int id { get; set; }
		public int idFuncionario { get; set; }
		public virtual Funcionario Funcionario { get; set; }
		public string nroDocumento { get; set; } 
		public string nome { get; set; } 
		public string rg { get; set; } 
		public DateTime? dtNascimento { get; set; }
		public string flagSexo { get; set; }
		public string observacao { get; set; }
		public DateTime? dtCadastro { get; set; }
		public DateTime? dtAlteracao { get; set; }
		public int? idUsuarioCadastro { get; set; }
		public int? idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class FuncionarioDependenteMapper : EntityTypeConfiguration<FuncionarioDependente> {

		public FuncionarioDependenteMapper() {
			this.ToTable("tb_Funcionario_dependente");
			this.HasKey(o => o.id);

			//FKs
			this.HasRequired(x => x.Funcionario).WithMany().HasForeignKey(o => o.idFuncionario);
		}
	}
}