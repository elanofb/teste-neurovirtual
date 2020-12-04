using System.Data.Entity.ModelConfiguration;

namespace DAL.Transacoes {

    public class TotalMovimentoVW {

        public int    idMembro       { get; set; }
        public byte?  idTipoCadastro { get; set; }
        public int?   nroMembro      { get; set; }
        public decimal valorDebitos   { get; set; }
        public decimal valorCreditos  { get; set; }
    }

    //
    internal sealed class TotalMovimentoVWMapper : EntityTypeConfiguration<TotalMovimentoVW> {

        public TotalMovimentoVWMapper() {

            this.ToTable("vw_total_movimento");

            this.HasKey(o => o.idMembro);
            
            this.Property(x => x.valorDebitos).HasPrecision(18, 4);
            
            this.Property(x => x.valorCreditos).HasPrecision(18, 4);
        }
    }

}
