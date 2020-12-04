using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.Associados {

	//
	public class ConfiguracaoMembro {

		public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public Organizacao Organizacao { get; set; }

		public int? idMembro { get; set; }
		
		public byte? idChaveBinaria { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime? dtExclusao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int? idUsuarioExclusao { get; set; }

		public bool? ativo { get; set; }
	}

	//
	internal sealed class ConfiguracaoMembroMapper : EntityTypeConfiguration<ConfiguracaoMembro> {

		public ConfiguracaoMembroMapper() {

			this.ToTable("tb_configuracao_membro");
            
			this.HasKey(o => o.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

		}
	}
}