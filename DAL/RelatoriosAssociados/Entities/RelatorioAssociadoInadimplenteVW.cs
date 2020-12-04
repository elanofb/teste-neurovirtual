using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.RelatoriosAssociados {

    public class RelatorioAssociadoInadimplenteVW {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int idTipoAssociado { get; set; }

        public int? nroAssociado { get; set; }

        public string nome { get; set; }

        public string nroDocumento { get; set; }

        public int idPessoa { get; set; }
        
        public string descricaoTipoAssociado { get; set; }

        public string status { get; set; }

        public DateTime dtCadastro { get; set; }

        //IGNORE
        public string contribuicoesPendentes { get; set; }
        public int qtdCobrancaAberto { get; set; }
        public decimal valorTotalAberto { get; set; }

    }

    internal sealed class RelatorioAssociadoInadimplenteVWMapper : EntityTypeConfiguration<RelatorioAssociadoInadimplenteVW> {

        public RelatorioAssociadoInadimplenteVWMapper() {

            this.ToTable("vw_relatorio_associado_inadimplente");
            this.HasKey(o => o.id);

            this.Ignore(x => x.contribuicoesPendentes);
            this.Ignore(x => x.qtdCobrancaAberto);
            this.Ignore(x => x.valorTotalAberto);
        }
    }
}