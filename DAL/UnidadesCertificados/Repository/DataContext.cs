using System.Data.Entity;
using DAL.UnidadesCertificados;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<UnidadeCertificado> UnidadeCertificado { get; set; }

        public DbSet<CertificadoDigitalVW> CertificadoDigitalVW { get; set; }

		//
		private void mapperModuloUnidadesCertificados(DbModelBuilder modelBuilder) {		
            	
			modelBuilder.Configurations.Add(new UnidadeCertificadoMapper());

            modelBuilder.Configurations.Add(new CertificadoDigitalVWMapper());
		}
	}
}