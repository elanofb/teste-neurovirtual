using System.Data.Entity;
using DAL.Funcionarios;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<FuncionarioFerias> FuncionarioFerias { get; set; }
        public DbSet<FuncionarioProfissao> FuncionarioProfissao { get; set; }
        public DbSet<FuncionarioBeneficio> FuncionarioBeneficio { get; set; }
        public DbSet<FuncionarioOcorrencia> FuncionarioOcorrencia { get; set; }
        public DbSet<FuncionarioDependente> FuncionarioDependente { get; set; }
        public DbSet<FuncionarioContaBancaria> FuncionarioContaBancaria { get; set; }

		//
		private void mapperModuloFuncionarios(DbModelBuilder modelBuilder) {
			modelBuilder.Configurations.Add(new FuncionarioMapper());
            modelBuilder.Configurations.Add(new FuncionarioFeriasMapper());
            modelBuilder.Configurations.Add(new FuncionarioProfissaoMapper());
            modelBuilder.Configurations.Add(new FuncionarioOcorrenciaMapper());
            modelBuilder.Configurations.Add(new FuncionarioBeneficioMapper());
            modelBuilder.Configurations.Add(new FuncionarioDependenteMapper());
            modelBuilder.Configurations.Add(new FuncionarioContaBancariaMapper());
		}
	}
}