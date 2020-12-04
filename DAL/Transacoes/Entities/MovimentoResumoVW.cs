using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Transacoes {

    public class MovimentoResumoVW {
        
        public long     idMovimento               { get; set; }
        public long?    codigoMovimento           { get; set; }
        public byte     idTipoTransacao           { get; set; }
        public string   descricaoTipoTransacao    { get; set; }
        public bool     flagDebito { get; set; }
        public bool     flagCredito { get; set; }
        public bool? flagSaldoInicial { get; set;  }
        public int? idImportacao { get; set; }
        public int      idMembroDestino           { get; set; }
        public int? idPessoaDestino { get; set; }
        public int?      nroMembroDestino          { get; set; }
        public string    nomeMembroDestino         { get; set; }
        public string    razaoSocialMembroDestino  { get; set; }
        public string    nroDocumentoMembroDestino { get; set; }
        public int?       idMembroOrigem            { get; set; }
        public int? idPessoaOrigem { get; set; }
        public int?      nroMembroOrigem           { get; set; }
        public string    nomeMembroOrigem         { get; set; }
        public string    razaoSocialMembroOrigem   { get; set; }
        public string    nroDocumentoMembroOrigem  { get; set; }
        public decimal?    valorOperacao             { get; set; }
        public DateTime? dtIntegracaoSaldo         { get; set; }
        public long? idMovimentoPrincipal { get; set; }
        public string observacao { get; set; }
        public DateTime? dtCadastro                { get; set; }
        public byte?     idOrigem                  { get; set; }        
        
        //
        public string  valorOperacaoFormatado { get; set; }
        public string dtCadastroFormatado    { get; set; }
        
    }
    
    /// <summary>
    /// 
    /// </summary>
    internal sealed class MovimentoResumoVWMapper : EntityTypeConfiguration<MovimentoResumoVW> {

        public MovimentoResumoVWMapper() {

            this.ToTable("vw_movimento_resumo");

            this.HasKey(o => o.idMovimento);

            this.Ignore(x => x.valorOperacaoFormatado);
            
            this.Ignore(x => x.dtCadastroFormatado);
        }
    }    

}
