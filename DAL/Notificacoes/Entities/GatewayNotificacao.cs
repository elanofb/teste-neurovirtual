using DAL.Permissao;
using DAL.Pessoas;
using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.Notificacoes {

    public class GatewayNotificacao {

		public byte id { get; set; }

		public string descricao { get; set; }
	    
	    public bool ativo { get; set; }
	    
		public DateTime dtCadastro { get; set; }

		public int idUsuarioCadastro { get; set; }

	    public DateTime? dtExclusao { get; set; }
	    
		public int idUsuarioExclusao { get; set; }

    }


    public class GatewayNotificacaoMapper : EntityTypeConfiguration<GatewayNotificacao> {

        public GatewayNotificacaoMapper() {

            this.ToTable("datatb_gateway_notificacao");

            this.HasKey(x => x.id);

        }
    }
}
