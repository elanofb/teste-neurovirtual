using System.Data.Entity;
using DAL.Emails;
using DAL.Entities;

namespace DAL.Repository.Base {

	public partial class DataContext : DbContext {

		public DbSet<LogEmail> LogEmail { get; set; }

		public DbSet<LogEmailDestino> LogEmailDestino { get; set; }

		public DbSet<EmailContatoVW> EmailContatoVW { get; set; }

        public DbSet<TipoEmail> TipoEmail { get; set; }
		
		public DbSet<MensagemEmail> MensagemEmail { get; set; }

        
		private void mapperModuloEmail(DbModelBuilder modelBuilder) {

            modelBuilder.Configurations.Add(new LogEmailMapper());

            modelBuilder.Configurations.Add(new LogEmailDestinoMapper());

            modelBuilder.Configurations.Add(new EmailContatoVWMapper());

            modelBuilder.Configurations.Add(new TipoEmailMapper());
			
			modelBuilder.Configurations.Add(new MensagemEmailMapper());
		}
	}
}