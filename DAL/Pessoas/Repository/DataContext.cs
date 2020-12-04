using System.Data.Entity;
using DAL.Pessoas;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Pessoa> Pessoa { get; set; }

		public DbSet<PessoaContato> PessoaContato { get; set; }

		public DbSet<PessoaEndereco> PessoaEndereco { get; set; }

		public DbSet<PessoaEmail> PessoaEmail { get; set; }

		public DbSet<PessoaTelefone> PessoaTelefone { get; set; }

		public DbSet<PessoaRelacionamento> PessoaRelacionamento { get; set; }

        public DbSet<PessoaVW> PessoaVW { get; set; }
		
		public DbSet<PessoaContaBancaria> PessoaContaBancaria { get; set; }

        //
        private void mapperModuloPessoas(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new PessoaMapper());

            modelBuilder.Configurations.Add(new PessoaContatoMapper());

            modelBuilder.Configurations.Add(new PessoaEnderecoMapper());

            modelBuilder.Configurations.Add(new PessoaEmailMapper());

            modelBuilder.Configurations.Add(new PessoaTelefoneMapper());

            modelBuilder.Configurations.Add(new PessoaRelacionamentoMapper());

            modelBuilder.Configurations.Add(new PessoaVWMapper());
	        
	        modelBuilder.Configurations.Add(new PessoaContaBancariaMapper());
		}
	}
}
