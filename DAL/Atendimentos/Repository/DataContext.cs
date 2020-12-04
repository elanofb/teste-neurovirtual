using System.Data.Entity;
using DAL.Atendimentos;

namespace DAL.Repository.Base {

	public partial class DataContext {

	    public DbSet<Atendimento> Atendimento { get; set; }

        public DbSet<AtendimentoTipo> AtendimentoTipo { get; set; }

	    public DbSet<AtendimentoStatus> AtendimentoStatus { get; set; }

	    public DbSet<AtendimentoArea> AtendimentoArea { get; set; }

	    public DbSet<AtendimentoHistorico> AtendimentoHistorico { get; set; }

        //
        private void mapperModuloAtendimentos(DbModelBuilder modelBuilder) {
            
            modelBuilder.Configurations.Add(new AtendimentoMapper());

            modelBuilder.Configurations.Add(new AtendimentoTipoMapper());

            modelBuilder.Configurations.Add(new AtendimentoStatusMapper());

            modelBuilder.Configurations.Add(new AtendimentoAreaMapper());

            modelBuilder.Configurations.Add(new AtendimentoHistoricoMapper());

        }
	}
}