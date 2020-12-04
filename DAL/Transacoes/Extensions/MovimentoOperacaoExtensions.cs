namespace DAL.Transacoes.Extensions {

    public static class MovimentoResumoVWExtensions {

        /// <summary>
        /// 
        /// </summary>
        public static MovimentoResumoVW captarDados(this MovimentoResumoVW MovimentoResumo, MovimentoOperacaoDTO Transacao) {
            
            MovimentoResumo.valorOperacao = Transacao.valorOperacao;
            
            MovimentoResumo.valorOperacaoFormatado = Transacao.valorOperacao.ToString("F2");

            MovimentoResumo.idMembroOrigem = Transacao.MembroOrigem.id;
            
            MovimentoResumo.idPessoaOrigem = Transacao.MembroOrigem.idPessoa;
            
            MovimentoResumo.nroMembroOrigem = Transacao.MembroOrigem.nroAssociado;
            
            MovimentoResumo.nomeMembroOrigem = (Transacao.MembroOrigem?.Pessoa?.nome);
            
            MovimentoResumo.nroDocumentoMembroOrigem = (Transacao.MembroOrigem?.Pessoa?.nroDocumento);
            
            MovimentoResumo.idMembroDestino = Transacao.MembroDestino.id;
            
            MovimentoResumo.idPessoaDestino = Transacao.MembroDestino.idPessoa;
            
            MovimentoResumo.nroMembroDestino = Transacao.MembroDestino.nroAssociado;

            MovimentoResumo.nomeMembroDestino = Transacao.MembroDestino.Pessoa.nome;
            
            MovimentoResumo.nroDocumentoMembroDestino = Transacao.MembroDestino.Pessoa.nroDocumento;
            
            MovimentoResumo.idOrigem = Transacao.idOrigem;
            
            return MovimentoResumo;
        }
    }

}
