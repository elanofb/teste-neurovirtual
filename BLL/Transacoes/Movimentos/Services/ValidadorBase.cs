using System;
using System.Linq;
using BLL.Associados;
using BLL.Services;
using DAL.Associados;
using DAL.Transacoes;
using DAL.Transacoes.Extensions;

namespace BLL.Transacoes.Movimentos {

    public abstract class ValidadorBase : IValidadorOperacao{
        
        //Atributos
        protected IMembroSaldoConsultaBL _SaldoConsultaBL;
        protected IAssociadoConsultaBL _MembroConsultaBL;
        
        //Servicos
        protected IMembroSaldoConsultaBL SaldoConsultaBL => _SaldoConsultaBL = _SaldoConsultaBL ?? new MembroSaldoConsultaBL();
        protected IAssociadoConsultaBL MembroConsultaBL => _MembroConsultaBL = _MembroConsultaBL ?? new AssociadoConsultaBL();

        /// <summary>
        /// 
        /// </summary>
        public abstract UtilRetorno validar(MovimentoOperacaoDTO Transacao);
        
        /// <summary>
        /// 
        /// </summary>
        public virtual UtilRetorno validarContas(MovimentoOperacaoDTO Transacao) {
            
            var Retorno = UtilRetorno.newInstance(false, "");
            
            Retorno = this.validarDestino(Transacao);

            if (Retorno.flagError) {
                return Retorno;
            }
            
            return this.validarOrigem(Transacao);

        }

        protected virtual UtilRetorno validarDestino(MovimentoOperacaoDTO Transacao) {
            
            if (Transacao.MembroDestino == null) {
            
                return UtilRetorno.newInstance(true, "Não foi possível identificar a conta de destino da transação.");
            }

            if (Transacao.valorOperacao <= 0) {
                
                return UtilRetorno.newInstance(true, "O valor informado para esta operação não é válido.");
            }
            
            return UtilRetorno.newInstance(false, "");
            
        }

        protected virtual UtilRetorno validarOrigem(MovimentoOperacaoDTO Transacao) {
            
            if (Transacao.MembroOrigem == null) {
            
                return UtilRetorno.newInstance(true, "Não foi possível identificar a conta de origem da transação.");
            }
            
            if (Transacao.MembroOrigem.id == Transacao.MembroDestino.id) {
            
                return UtilRetorno.newInstance(true, "Operação inválida! Não é possível realizar uma transferência para a própria conta.");
            }

            if (Transacao.MembroOrigem.id == 0 || Transacao.MembroDestino.id == 0) {
            
                return UtilRetorno.newInstance(true, "Operação inválida! Uma das contas não pôde ser identificada corretamente.");
            }
            
            if (Transacao.flagIgnorarSaldo == true) {
                
                return UtilRetorno.newInstance(false, "");
            }
            
            var SaldoOrigem = this.SaldoConsultaBL.query(Transacao.MembroOrigem.id, 1)
                                  .Select(x => new {x.id, x.saldoAtual})
                                  .FirstOrDefault()
                                  .ToJsonObject<MembroSaldo>() ?? new MembroSaldo();

            if (SaldoOrigem.saldoAtual < Transacao.valorOperacao) {

                return UtilRetorno.newInstance(true, "Não há saldo suficiente para realizar a operação.");
                
            }
            
            return UtilRetorno.newInstance(false, "");
            
        }

        protected virtual UtilRetorno validarSaldoOrigem(MovimentoOperacaoDTO Transacao) {
            
            var SaldoDesstino = this.SaldoConsultaBL.query(Transacao.MembroDestino.id, 1)
                                  .Select(x => new {x.id, x.saldoAtual})
                                  .FirstOrDefault()
                                  .ToJsonObject<MembroSaldo>() ?? new MembroSaldo();

            if (SaldoDesstino.saldoAtual < Transacao.valorOperacao) {

                return UtilRetorno.newInstance(true, "Não há saldo suficiente para realizar a operação.");
                
            }
            
            return UtilRetorno.newInstance(false, "");
            
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual UtilRetorno validaSenha(MovimentoOperacaoDTO Transacao) {
            
            var senhaTransacao = Transacao.MembroOrigem.senhaTransacao;

            if (senhaTransacao.isEmpty()) {
                
                return UtilRetorno.newInstance(true, "A conta informada não possui senha de transação configurada.");
            }
            
            if (Transacao.tk.isEmpty()) {
                
                return UtilRetorno.newInstance(true, "A senha de confirmação da transação não foi informada.");
            }

            string senhaEnviada = UtilCrypt.SHA512(Transacao.tk);

            if (!senhaEnviada.Equals(senhaTransacao)) {
                
                return UtilRetorno.newInstance(true, "A senha de transação informada é inválida.");
            }
            
            return UtilRetorno.newInstance(false, "");
        }
    }

}