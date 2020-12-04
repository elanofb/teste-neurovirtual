using System.Data.Entity.ModelConfiguration;

namespace DAL.Transacoes {

    public class ConferenciaSaldoVW {

        public int    idMembro            { get; set; }
        public int?   nroMembro           { get; set; }
        public byte?  idTipoCadastro      { get; set; }
        public decimal valorSaldoAtual     { get; set; }
        public decimal valorDebitos        { get; set; }
        public decimal valorCreditos       { get; set; }
        public decimal valorSaldoMovimento { get; set; }
    }

    //
    internal sealed class ConferenciaSaldoVWMapper : EntityTypeConfiguration<ConferenciaSaldoVW> {

        public ConferenciaSaldoVWMapper() {

            this.ToTable("vw_conferencia_saldo");

            this.HasKey(o => o.idMembro);
            
            this.Property(x => x.valorSaldoAtual).HasPrecision(18, 4);
            
            this.Property(x => x.valorDebitos).HasPrecision(18, 4);
            
            this.Property(x => x.valorCreditos).HasPrecision(18, 4);
            
            this.Property(x => x.valorSaldoMovimento).HasPrecision(18, 4);
        }
    }

}
