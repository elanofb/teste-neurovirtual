namespace DAL.Transacoes.Extensions {

    public static class MovimentoVWExtensions {

        /// <summary>
        /// 1) Pessoa destino passa a ser a pessoa que receberá o debito, sendo que a origem do debito é a pessoa que sera creditada
        /// </summary>
        public static Movimento captarDadosOrigem(this Movimento Movimento, MovimentoResumoVW MovimentoResumo) {

            Movimento.idMembroDestino = MovimentoResumo.idMembroOrigem;
            
            Movimento.idPessoaDestino = MovimentoResumo.idPessoaOrigem;
            
            Movimento.idMembroOrigem = MovimentoResumo.idMembroDestino;
            
            Movimento.idPessoaOrigem  = MovimentoResumo.idPessoaDestino;
            
            Movimento.valor = MovimentoResumo.valorOperacao;

            if (MovimentoResumo.idOrigem > 0) {

                Movimento.idOrigem = MovimentoResumo.idOrigem ;
                
            }
            
            Movimento.flagDebito = true;
            
            Movimento.flagCredito = false;
            
            return Movimento;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static Movimento captarDadosDestino(this Movimento Movimento, MovimentoResumoVW MovimentoResumo) {

            Movimento.idMembroOrigem = MovimentoResumo.idMembroOrigem;
            
            Movimento.idPessoaOrigem = MovimentoResumo.idPessoaOrigem;
            
            Movimento.idMembroDestino = MovimentoResumo.idMembroDestino;
            
            Movimento.idPessoaDestino = MovimentoResumo.idPessoaDestino;
            
            Movimento.valor = MovimentoResumo.valorOperacao;
            
            if (MovimentoResumo.idOrigem > 0) {

                Movimento.idOrigem = MovimentoResumo.idOrigem ;
                
            }
            
            Movimento.flagDebito = false;
            
            Movimento.flagCredito = true;
            
            return Movimento;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static Movimento captarDadosDestinoDebito(this Movimento Movimento, MovimentoResumoVW MovimentoResumo) {

            Movimento.idMembroOrigem = null;
            
            Movimento.idPessoaOrigem = null;
            
            Movimento.idMembroDestino = MovimentoResumo.idMembroDestino;
            
            Movimento.idPessoaDestino = MovimentoResumo.idPessoaDestino;
            
            Movimento.valor = MovimentoResumo.valorOperacao;
            
            Movimento.flagDebito = true;
            
            Movimento.flagCredito = false;
            
            return Movimento;
        }
    }

}
