using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Pessoas;

namespace DAL.Notificacoes {

	//
	public class PessoaDevice {

		public int id { get; set; }

		public int? idOrganizacao { get; set; }

		public int? idPessoa { get; set; }

		public Pessoa Pessoa { get; set; }

		public string idDevice { get; set; }
		
		public string token { get; set; }

		public bool? flagAndroid { get; set; }

		public bool? flagIOS { get; set; }

		public string versao { get; set; }
        
        public bool? ativo { get; set; }

		public DateTime? dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }
		
		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

	}

	//
	internal sealed class PessoaDeviceMapper : EntityTypeConfiguration<PessoaDevice> {

		public PessoaDeviceMapper() {
			
			this.ToTable("tb_pessoa_device");
			
			this.HasKey(x => x.id);

			//FKs
			this.HasOptional(x => x.Pessoa).WithMany().HasForeignKey(x => x.idPessoa);
			
		}
		
	}
	
}