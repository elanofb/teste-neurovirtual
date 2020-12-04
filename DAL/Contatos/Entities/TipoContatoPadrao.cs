using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;

namespace DAL.Contatos {

    public class TipoContatoPadrao {
	    
        public int id { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }
    }


	internal sealed class TipoContatoPadraoMapper : EntityTypeConfiguration<TipoContatoPadrao> {

		public TipoContatoPadraoMapper() {
			
			this.ToTable("datatb_tipo_contato");
			
			this.HasKey(o => o.id);
			
		}
	}
}