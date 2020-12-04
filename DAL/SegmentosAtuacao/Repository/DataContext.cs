using System.Data.Entity;
using DAL.SegmentosAtuacao;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

        public DbSet<SegmentoAtuacao> SegmentoAtuacao { get; set; }

        //
        private void mapperModuloSegmentosAtuacao(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new SegmentoAtuacaoMapper());
		}
	}
}
