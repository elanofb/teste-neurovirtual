using System.Data.Entity;
using DAL.ProcessoAdmissoes;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {
        

        public DbSet<TipoFaseAdmissao> TipoFaseAdmissao { get; set; }

	    public DbSet<TipoAssociadoFaseAdmissao> TipoAssociadoFaseAdmissao { get; set; }

	    public DbSet<TipoAssociadoDocumentoAdmissao> TipoAssociadoDocumentoAdmissao { get; set; }

	    public DbSet<AssociadoProcessoAdmissao> AssociadoProcessoAdmissao { get; set; }

        //  
        private void mapperModuloProcessoAdmissoes(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new TipoFaseAdmissaoMapper());

			modelBuilder.Configurations.Add(new TipoAssociadoFaseAdmissaoMapper());

            modelBuilder.Configurations.Add(new TipoAssociadoDocumentoAdmissaoMapper());

            modelBuilder.Configurations.Add(new AssociadoProcessoAdmissaoMapper());

        }
	}
}