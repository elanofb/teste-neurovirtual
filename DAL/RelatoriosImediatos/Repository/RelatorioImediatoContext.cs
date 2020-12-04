using System.Data.Entity;

namespace DAL.Repository.Base {

	public class RelatorioImediatoContext : DbContext {

		//Iniciar conexão com banco de dados
		public RelatorioImediatoContext(): base("STAdmConnection") {
		}

		//
		public int saveChanges() {
			return this.SaveChanges();
		}


		//Aqui faz o mapeamento das tabelas e a conexão com o banco de dados
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);
		}
	}
}
