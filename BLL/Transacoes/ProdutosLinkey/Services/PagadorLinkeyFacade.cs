using System;
using System.Collections.Generic;
using BLL.Transacoes.Movimentos;
using DAL.Pedidos;
using DAL.Transacoes;

namespace BLL.Transacoes.ProdutosLinkey {

    public class PagadorLinkeyFacade : IPagadorLinkeyFacade {
        
        //Atributos
        private IValidadorOperacao _Validador;
        private IGeradorMovimento _GeradorMovimento;
        private IAtualizadorSaldoBL _AtualizadorSaldoBL;
        
        //Servicos
        private IValidadorOperacao Validador => _Validador = _Validador ?? new ValidadorPagamento();
        private IGeradorMovimento GeradorMovimento => _GeradorMovimento = _GeradorMovimento ?? new GeradorMovimento();
        private IAtualizadorSaldoBL AtualizadorSaldoBL => _AtualizadorSaldoBL = _AtualizadorSaldoBL ?? new AtualizadorSaldoBL();

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno pagar(List<PedidoProdutoRendimento> listaProdutos) {

            /*var RetornoValidacao = this.Validador.validar(Transacao);

            if (RetornoValidacao.flagError) {
                
                return RetornoValidacao;
                
            }*/

            /*var ValidacaoSenha = this.Validador.validaSenha(Transacao);
            
            if (ValidacaoSenha.flagError) {

                return ValidacaoSenha;
            }*/

            var RetornoMovimento = GeradorMovimento.transferir(listaProdutos);

            if (RetornoMovimento.flagError) {
                
                return RetornoMovimento;
            }

            var listaMovimentos = RetornoMovimento.info as List<Movimento>;
            
            var RetornoAtualizador = this.AtualizadorSaldoBL.atualizar(listaMovimentos);
            
            return UtilRetorno.newInstance(false, "", listaMovimentos);
        }
    }

}
