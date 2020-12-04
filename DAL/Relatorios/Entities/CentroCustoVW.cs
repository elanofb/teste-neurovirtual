using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Relatorios {

    //
    public class CentroCustoVW {
        public Guid id { get; set; }
        public int idCentroCusto { get; set; }
        public int tipo { get; set; }
        public string descricao { get; set; }
        public string flagPago { get; set; }
        public string descricaoCentroCusto { get; set; }
        public string descricaoCategoria { get; set; }
        public string descricaoTipoCategoria { get; set; }
        public string descricaoDetalheCategoria { get; set; }
        public string flagFixa { get; set; }
        public DateTime? dtVencimento { get; set; }
        public DateTime? dtBaixa { get; set; }
        public decimal valor { get; set; }
    }

    //
    internal sealed class CentroCustoVWMapper : EntityTypeConfiguration<CentroCustoVW> {

        public CentroCustoVWMapper() {

            this.ToTable("vw_centro_custo");
            this.HasKey(o => o.id);
        }
    }
}