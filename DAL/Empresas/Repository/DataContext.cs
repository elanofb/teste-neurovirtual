using System.Data.Entity;
using DAL.Empresas;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<Empresa> Empresa { get; set; }

		public DbSet<EmpresaEndereco> EmpresaEndereco { get; set; }

        public DbSet<EmpresaPorte> EmpresaPorte { get; set; }

        //
        private void mapperModuloEmpresas(DbModelBuilder modelBuilder) {

			modelBuilder.Configurations.Add(new EmpresaMapper());

			modelBuilder.Configurations.Add(new EmpresaEnderecoMapper());

            modelBuilder.Configurations.Add(new EmpresaPorteMapper());
        }
	}
}