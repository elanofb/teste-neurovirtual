using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using BLL.Configuracoes;
using BLL.Services;
using DAL.Transacoes;
using DAL.Transacoes.Extensions;
using MoreLinq;
using System.Data.Entity;
using System.Linq;
using DAL.Configuracoes;

namespace BLL.Transacoes.Pagamentos {

    public class GeradorMovimentoPagamento: DefaultBL, IGeradorMovimentoPagamento {
        
        //Atributos
        private IConfiguracaoOperacaoCompraBL _ConfiguracaoCompra;
        
        //Servicos
        private IConfiguracaoOperacaoCompraBL ConfiguracaoCompraBL => _ConfiguracaoCompra = _ConfiguracaoCompra ?? new ConfiguracaoOperacaoCompraBL();

        
        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno pagar(MovimentoResumoVW MovimentoResumo, MovimentoOperacaoDTO Transacao) {


            var MovimentoOrigem = new Movimento();
            MovimentoOrigem.captarDadosOrigem(MovimentoResumo);
            MovimentoOrigem.idTipoTransacao = (byte) TipoTransacaoEnum.PAGAMENTO;
            MovimentoOrigem.setDefaultInsertValues();
            db.Movimento.Add(MovimentoOrigem);
            db.SaveChanges();

            var listaItens = new List<Movimento>();
            var ValoresTransacao = this.carregarValores(Transacao);

            var MovimentoDestino = new Movimento();
            MovimentoDestino.captarDadosDestino(MovimentoResumo);
            MovimentoDestino.valor = ValoresTransacao.valorEstabelecimento;
            MovimentoDestino.observacao = $"{ValoresTransacao.valorPagamento:N2} com {ValoresTransacao.percentualDescontoEstabelecimento:N2} de desconto.";
            MovimentoDestino.idTipoTransacao = (byte) TipoTransacaoEnum.PAGAMENTO;
            MovimentoDestino.setDefaultInsertValues();
            
            listaItens.Add(MovimentoDestino);

            
            if (ValoresTransacao.valorCashback > 0) {
                
                var MovimentoCashback = new Movimento();
                MovimentoCashback.captarDadosOrigem(MovimentoResumo);
                MovimentoCashback.valor = ValoresTransacao.valorCashback;
                MovimentoCashback.idTipoTransacao = (byte) TipoTransacaoEnum.CASHBACK;
                MovimentoCashback.setDefaultInsertValues();
                
                listaItens.Add(MovimentoCashback);
            }

            
            if (ValoresTransacao.valorComissaoCorretor > 0) {
                
                var MovimentoCorretor = new Movimento();
                MovimentoCorretor.idMembroOrigem = MovimentoDestino.idMembroDestino;
                MovimentoCorretor.idPessoaOrigem = MovimentoDestino.idPessoaDestino;
                MovimentoCorretor.idMembroDestino = Transacao.MembroDestino.idIndicador ?? 1;
                MovimentoCorretor.idPessoaDestino = null;
                MovimentoCorretor.valor = ValoresTransacao.valorComissaoCorretor;
                MovimentoCorretor.idTipoTransacao = (byte) TipoTransacaoEnum.COMISSAO_COMPRA_ESTABELECIMENTO;
                MovimentoCorretor.setDefaultInsertValues();
                
                listaItens.Add(MovimentoCorretor);
            }
            
            
            if (ValoresTransacao.valorLinkey > 0) {
                
                var MovimentoLinkey = new Movimento();
                MovimentoLinkey.idMembroOrigem = MovimentoOrigem.idMembroDestino;
                MovimentoLinkey.idPessoaOrigem = MovimentoOrigem.idPessoaDestino;
                MovimentoLinkey.idMembroDestino = 1;
                MovimentoLinkey.idPessoaDestino = null;
                MovimentoLinkey.valor = ValoresTransacao.valorLinkey;
                MovimentoLinkey.observacao = "Remuneração da SINCTEC";                
                MovimentoLinkey.idTipoTransacao = (byte) TipoTransacaoEnum.PAGAMENTO;
                MovimentoLinkey.setDefaultInsertValues();
                
                listaItens.Add(MovimentoLinkey);
            }

            
            if (ValoresTransacao.valorComissaoIndicador1 > 0) {
                
                var MovimentoIndicador1 = new Movimento();
                MovimentoIndicador1.idMembroOrigem = MovimentoOrigem.idMembroDestino;
                MovimentoIndicador1.idPessoaOrigem = MovimentoOrigem.idPessoaDestino;
                MovimentoIndicador1.idMembroDestino = Transacao.MembroOrigem.idIndicador ?? 1;
                MovimentoIndicador1.idPessoaDestino = null;
                MovimentoIndicador1.valor = ValoresTransacao.valorComissaoIndicador1;
                MovimentoIndicador1.idTipoTransacao = (byte) TipoTransacaoEnum.COMISSAO_COMPRA_USUARIO;
                MovimentoIndicador1.observacao = "Indicação comprador nível 1";                
                MovimentoIndicador1.setDefaultInsertValues();
                
                listaItens.Add(MovimentoIndicador1);
                
            }
            
            if (ValoresTransacao.valorComissaoIndicador2 > 0) {
                
                var MovimentoIndicador2 = new Movimento();
                MovimentoIndicador2.idMembroOrigem = MovimentoOrigem.idMembroDestino;
                MovimentoIndicador2.idPessoaOrigem = MovimentoOrigem.idPessoaDestino;
                MovimentoIndicador2.idMembroDestino = Transacao.MembroOrigem.idIndicadorSegundoNivel ?? 1;
                MovimentoIndicador2.idPessoaDestino = null;
                MovimentoIndicador2.valor = ValoresTransacao.valorComissaoIndicador2;
                MovimentoIndicador2.idTipoTransacao = (byte) TipoTransacaoEnum.COMISSAO_COMPRA_USUARIO;
                MovimentoIndicador2.observacao = "Indicação comprador nível 2";                
                MovimentoIndicador2.setDefaultInsertValues();
                
                listaItens.Add(MovimentoIndicador2);
                
            }
            
            if (ValoresTransacao.valorComissaoIndicador3 > 0) {
                
                var MovimentoIndicador3 = new Movimento();
                MovimentoIndicador3.idMembroOrigem = MovimentoOrigem.idMembroDestino;
                MovimentoIndicador3.idPessoaOrigem = MovimentoOrigem.idPessoaDestino;
                MovimentoIndicador3.idMembroDestino = Transacao.MembroOrigem.idIndicadorTerceiroNivel ?? 1;
                MovimentoIndicador3.idPessoaDestino = null;
                MovimentoIndicador3.valor = ValoresTransacao.valorComissaoIndicador3;
                MovimentoIndicador3.idTipoTransacao = (byte) TipoTransacaoEnum.COMISSAO_COMPRA_USUARIO;
                MovimentoIndicador3.observacao = "Indicação comprador nível 3";                
                MovimentoIndicador3.setDefaultInsertValues();
                
                listaItens.Add(MovimentoIndicador3);
                
            }

            listaItens.ForEach(item => {
                                   item.flagDebito = false;
                                   item.flagCredito = true;
                                   item.idOrigem = MovimentoOrigem.idOrigem;
                                   item.idMovimentoPrincipal = MovimentoOrigem.id;
                                   item.valorMovimentoPrincipal = MovimentoOrigem.valor;
                                   item.observacao = item.observacao.abreviar(100);
                               });

            db.Movimento.AddRange(listaItens);
            
            var Retorno = db.validateAndSave();

            if (Retorno.flagError) {
                
                return Retorno;
            }

            //Adicionaro primeiro movimento para os próximos processos
            listaItens.Add(MovimentoOrigem);
            
            return UtilRetorno.newInstance(false, "", listaItens);
        }

        /// <summary>
        /// 
        /// </summary>
        private TransacaoPagamentoDTO carregarValores(MovimentoOperacaoDTO Transacao) {

            var ConfiguracaoCompra = ConfiguracaoCompraBL.listar(1)
                                                        .Select(x => new { x.id, 
                                                                             x.percentualLucro,
                                                                             x.percentualCashback,
                                                                             x.percentualComissao,
                                                                             x.percentualIndicacaoNivel1,
                                                                             x.percentualIndicacaoNivel2,
                                                                             x.percentualIndicacaoNivel3
                                                                         })
                                                        .FirstOrDefault()
                                                        .ToJsonObject<ConfiguracaoOperacaoCompra>();

            var Dados = new TransacaoPagamentoDTO();

            Dados.valorPagamento = Transacao.valorOperacao.toDecimal();
            
            Dados.percentualDescontoEstabelecimento = Transacao.MembroDestino.percentualDesconto.toDecimal();

            Dados.valorDesconto = Transacao.valorOperacao.valorPercentual(Dados.percentualDescontoEstabelecimento);

            Dados.valorEstabelecimento = decimal.Subtract(Transacao.valorOperacao.toDecimal(), Dados.valorDesconto);

            Dados.valorLinkey = Dados.valorDesconto.valorPercentual(ConfiguracaoCompra.percentualLucro.toDecimal());
            
            Dados.valorCashback = Dados.valorDesconto.valorPercentual(ConfiguracaoCompra.percentualCashback.toDecimal());
            
            Dados.valorComissaoCorretor = Dados.valorDesconto.valorPercentual(ConfiguracaoCompra.percentualComissao.toDecimal());
            
            Dados.valorComissaoIndicador1 = Dados.valorDesconto.valorPercentual(ConfiguracaoCompra.percentualIndicacaoNivel1.toDecimal());
            
            Dados.valorComissaoIndicador2 = Dados.valorDesconto.valorPercentual(ConfiguracaoCompra.percentualIndicacaoNivel2.toDecimal());
            
            Dados.valorComissaoIndicador3 = Dados.valorDesconto.valorPercentual(ConfiguracaoCompra.percentualIndicacaoNivel3.toDecimal());

            return Dados;
        }
    }

}
