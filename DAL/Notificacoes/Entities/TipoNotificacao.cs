using DAL.Permissao;
using DAL.Pessoas;
using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Organizacoes;

namespace DAL.Notificacoes {

    public class TipoNotificacao {

		public byte id { get; set; }

		public string descricao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public bool ativo { get; set; }

		public bool flagExcluido { get; set; }

		public bool flagSistema { get; set; }

    }


    public class TipoNotificacaoMapper : EntityTypeConfiguration<TipoNotificacao> {

        public TipoNotificacaoMapper() {

            this.ToTable("datatb_tipo_notificacao");

            this.HasKey(x => x.id);

        }
    }
}
