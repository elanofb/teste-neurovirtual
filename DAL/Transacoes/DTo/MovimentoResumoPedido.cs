using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Transacoes {

    public class MovimentoResumoPedido {
        
        public long     idMovimento               { get; set; }
        public long?    codigoMovimento           { get; set; }
        public byte     idTipoTransacao           { get; set; }
        public string   descricaoTipoTransacao    { get; set; }
        public bool     flagDebito { get; set; }
        public bool     flagCredito { get; set; }
        public bool? flagSaldoInicial { get; set;  }
        public int?      idMembroDestino           { get; set; }
        public int? idPessoaDestino { get; set; }
        public int?      nroMembroDestino          { get; set; }
        public string    nomeMembroDestino         { get; set; }
        public string    razaoSocialMembroDestino  { get; set; }
        public string    nroDocumentoMembroDestino { get; set; }
        public int?       idMembroOrigem            { get; set; }
        public decimal?    valorOperacao             { get; set; }
        public DateTime? dtIntegracaoSaldo         { get; set; }
        public long? idMovimentoPrincipal { get; set; }
        public string observacao { get; set; }
        public DateTime? dtCadastro                { get; set; }
        public byte?     idOrigem                  { get; set; }        
        
        public DateTime? dtPedidoQuitacao { get; set; }
        public int? idPedidoProduto { get; set; }
        public int? idPedido { get; set; }
        public string nomeProduto { get; set; }
        
        //
        public string  valorOperacaoFormatado { get; set; }
        public string dtCadastroFormatado    { get; set; }
        
    }
  

}
