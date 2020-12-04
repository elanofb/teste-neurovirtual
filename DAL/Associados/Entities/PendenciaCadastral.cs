using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Associados {

	//
	[Serializable]
	public class PendenciaCadastralVW {

		public int id { get; set; }
        
        public int? nroAssociado { get; set; }

        public int idPessoa { get; set; }

        public int idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public int idTipoAssociado { get; set; }

        public string descricaoTipoAssociado { get; set; }

        public string nroDocumento { get; set; }

        public string nome { get; set; }

        public string razaoSocial { get; set; }

        public string flagTipoPessoa { get; set; }

        public int qtdTelefones { get; set; }

        public int qtdEnderecos { get; set; }

        public int qtdEmails { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public string ativo { get; set; }

        //Construtor
        public PendenciaCadastralVW() {

		}
	}

	//
	internal sealed class PendenciaCadastralVWMapper : EntityTypeConfiguration<PendenciaCadastralVW> {

		public PendenciaCadastralVWMapper() {

			this.ToTable("vw_pendencia_cadastral");

			this.HasKey(o => o.id);
        }
	}
}