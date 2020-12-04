using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Emails {

	//
	public class MensagemEmail {

		public int id { get; set; }

		public int? idOrganizacao { get; set; }
		
		public string codigoIdentificacao { get; set; }
		
		public int? idReferencia { get; set; }
		
		public string titulo { get; set; }
		
		public string emailsCopia { get; set; }
		
		public string corpoEmail { get; set; }
		
		public DateTime? dtCadastro { get; set; }
		
		public int? idUsuarioCadastro { get; set; }
		
		public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioAlteracao { get; set; }
			
		public bool? ativo { get; set; }

		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }
		
		public MensagemEmail() {
		}
	}
	
	//
	internal sealed class MensagemEmailMapper : EntityTypeConfiguration<MensagemEmail> {

		public MensagemEmailMapper() {
				
			this.ToTable("tb_mensagem_email");

            this.HasKey(o => o.id);
		}
	}
}