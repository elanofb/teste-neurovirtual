using System.Data.Entity;
using DAL.Contatos;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<TipoContatoPadrao> TipoContatoPadrao { get; set; }
		
		public DbSet<TipoContato> TipoContato { get; set; }
		
		public DbSet<TipoContatoVW> TipoContatoVW { get; set; }

		public DbSet<PessoaContatoVW> PessoaContatoVW { get; set; }

        //
        private void mapperModuloContato(DbModelBuilder modelBuilder) {
	        
			modelBuilder.Configurations.Add(new TipoContatoPadraoMapper());
	        
	        modelBuilder.Configurations.Add(new TipoContatoMapper());
	        
	        modelBuilder.Configurations.Add(new TipoContatoVWMapper());

			modelBuilder.Configurations.Add(new PessoaContatoVWMapper());
	        
        }
	}
}