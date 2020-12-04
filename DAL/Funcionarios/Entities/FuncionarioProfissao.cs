using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Funcionarios {

	//
	public class FuncionarioProfissao : Geral {
	}

	//
	internal sealed class FuncionarioProfissaoMapper : EntityTypeConfiguration<FuncionarioProfissao> {

		public FuncionarioProfissaoMapper() {
			this.ToTable("tb_funcionario_profissao");
			this.HasKey(o => o.id);
		}
	}
}