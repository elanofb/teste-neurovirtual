using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Relatorios {

    //
    public class AssociadoContribuicaoVW {
        public int id { get; set; }
        public string nomeAssociado { get; set; }
        public int idTipoAssociado { get; set; }
        public string descricaoTipoAssociado { get; set; }
        public string descricaoCategoriaTipoAssociado { get; set; }
        public byte idSegmento { get; set; }
        public string descricaoSegmento { get; set; }
        public string flagPago { get; set; }
        public DateTime? dtVencimento { get; set; }
        public decimal valor { get; set; }

        public AssociadoContribuicaoVW() {
        }
    }

    //
    internal sealed class AssociadoContribuicaoVWMapper : EntityTypeConfiguration<AssociadoContribuicaoVW> {

        public AssociadoContribuicaoVWMapper() {

            this.ToTable("vw_associado_contribuicao");
            this.HasKey(o => o.id);
        }
    }
}