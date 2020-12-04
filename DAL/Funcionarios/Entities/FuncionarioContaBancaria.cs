using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Contatos;

namespace DAL.Funcionarios {

	//
	public class FuncionarioContaBancaria {

		public int id { get; set; }
		public string tipoConta { get; set; }
		public string nomeBanco { get; set; }
		public string codigoBanco { get; set; }
		public string nroConta { get; set; }
		public string nroAgencia { get; set; }
		public string observacao { get; set; }
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
	internal sealed class FuncionarioContaBancariaMapper : EntityTypeConfiguration<FuncionarioContaBancaria> {

		public FuncionarioContaBancariaMapper() {
			this.ToTable("tb_funcionario_contabancaria");
			this.HasKey(o => o.id);

			//FKs
			this.HasRequired(x => x.Funcionario).WithMany().HasForeignKey(o => o.idFuncionario);
		}
	}
}