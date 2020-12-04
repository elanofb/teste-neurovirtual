using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Organizacoes {

	//
	public class StatusOrganizacao{

        public byte id { get; set; }
        
        public string descricao { get; set; }

        public string classColor { get; set; }

        public DateTime dtCadastro { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }
        
        public StatusOrganizacao() {
		}
	}

	//
	internal sealed class StatusOrganizacaoMapper : EntityTypeConfiguration<StatusOrganizacao> {

		public StatusOrganizacaoMapper() {

			this.ToTable("datatb_status_organizacao");

		    this.HasKey(x => x.id);
		}
	}
}