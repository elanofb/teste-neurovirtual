using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Associados {

    public class MembroSaldoVW {
        
        public int       idMembro           { get; set; }
        
        public int       idTipoMembro       { get; set; }
        
        public byte?     idTipoCadastro     { get; set; }
        
        public int?      nroMembro          { get; set; }
        
        public string    nomeMembro         { get; set; }
        
        public string    nroDocumentoMembro { get; set; }
        
        public decimal?    valorSaldoAtual    { get; set; }
        
        public string valorSaldoAtualFormatado { get; set; }
        
        public DateTime? dtAtualizacaoSaldo { get; set; }        
    }

    internal sealed class MembroSaldoVWMapper : EntityTypeConfiguration<MembroSaldoVW> {

        public MembroSaldoVWMapper() {

            this.ToTable("vw_membro_saldo");

            this.HasKey(o => o.idMembro);

            this.Ignore(x => x.valorSaldoAtualFormatado);
        }
    }    
}
