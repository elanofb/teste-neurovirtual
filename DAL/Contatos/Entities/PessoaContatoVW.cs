using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Contatos {

    public class PessoaContatoVW {

        public int idContato { get; set; }

        public string nomeContato { get; set; }

        public string emailContato { get; set; }

        public string telComercial { get; set; }

        public string telCelular { get; set; }

        public string nextel { get; set; }

        public int? idTipoContato { get; set; }

        public string descricaoTipoContato { get; set; }

        public int? idAssociado { get; set; }

        public int? nroAssociado { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public int? idTipoAssociado { get; set; }

        public string descricaoTipoAssociado { get; set; }

        public string nomeAssociado { get; set; }

        public string razaoSocial { get; set; }

        public string flagTipoPessoa { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public string ativo { get; set; }

        public DateTime? dtCadastroContato { get; set; }

        public DateTime? dtCadastroAssociado { get; set; }

        public string observacao { get; set; }
    }

	internal sealed class PessoaContatoVWMapper : EntityTypeConfiguration<PessoaContatoVW> {

		public PessoaContatoVWMapper() {
			this.ToTable("vw_pessoa_contato");
			this.HasKey(o => o.idContato);
		}
	}
}