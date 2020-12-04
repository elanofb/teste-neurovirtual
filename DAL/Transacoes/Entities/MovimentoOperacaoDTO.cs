using DAL.Associados;

namespace DAL.Transacoes {

    public class MovimentoOperacaoDTO {

        public int nroContaOrigem { get; set; }
        
        public Associado MembroOrigem { get; set; }

        public int nroContaDestino { get; set; }
        
        public Associado MembroDestino { get; set; }
        
        public decimal valorOperacao { get; set; }

        public string tk { get; set; }

        public byte idOrigem { get; set; }
        
        public string so { get; set; }
        
        public string browser { get; set; }
        
        public string ip { get; set; }
        
        public bool flagIgnorarSaldo { get; set; }
        
        public bool flagIgnorarSenha { get; set; }
        
        public bool flagPagamentoComBitkink { get; set; }
        
        public int? idProduto { get; set; }
        
        public byte? idTipoTransacao { get; set; }
        
        public MovimentoOperacaoDTO(){
            this.MembroOrigem = new Associado();
            this.MembroDestino = new Associado();
        }
    }

}
