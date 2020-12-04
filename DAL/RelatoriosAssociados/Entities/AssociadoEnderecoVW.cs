using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.RelatoriosAssociados {

    public class AssociadoEnderecoVW {

        public int id { get; set; }

        public int idOrganizacao { get; set; }

        public int idAssociado { get; set; }

        public int idPessoa { get; set; }

        public int? nroAssociado { get; set; }

        public string nomeAssociado { get; set; }

        public string nroDocumento { get; set; }

        public string cep { get; set; }

        public string logradouro { get; set; }

        public string numero { get; set; }

        public string complemento { get; set; }

        public string bairro { get; set; }

        public int? idCidade { get; set; }

        public string nomeCidade {get; set; }

        public int? idEstado { get; set; }

        public string siglaEstado { get; set; }

        public string zona { get; set; }

        public byte? idTipoEndereco { get; set; }

        public string descricaoTipoEndereco { get; set; }

        public DateTime? dtCadastroEndereco { get; set; }

        public int idTipoAssociado { get; set; }

        public string descricaoTipoAssociado { get; set; }

        public string ativo { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public DateTime? dtAdmissao { get; set; }

        public DateTime? dtCadastro { get; set; }

        public byte? idTipoCadastro { get; set; }

        public int? idAssociadoEstipulante { get; set; }
    }

    internal sealed class AssociadoEnderecoVWMapper : EntityTypeConfiguration<AssociadoEnderecoVW> {

        public AssociadoEnderecoVWMapper() {

            this.ToTable("vw_associado_endereco");

            this.HasKey(o => o.id);
            
            this.Ignore(x => x.flagSituacaoContribuicao);
        }
    }
}