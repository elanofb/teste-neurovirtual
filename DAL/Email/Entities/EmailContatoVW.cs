using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Entities {

	[Serializable]
	public class EmailContatoVW {

		public string nome { get; set; }

		public string email { get; set; }
	}

	//
	internal sealed class EmailContatoVWMapper : EntityTypeConfiguration<EmailContatoVW> {

		public EmailContatoVWMapper() {
			this.ToTable("vw_email_contato");
			this.HasKey(o => o.email);
		}
	}
}