using System;
using System.Collections.Generic;
using BLL.Services;
using DAL.Transacoes;
using DAL.Transacoes.Extensions;
using System.Linq;
using BLL.Produtos;
using BLL.RedeAfiliados.Services;
using DAL.Produtos;
using DAL.Repository.Base;

namespace BLL.Transacoes.Compras {

    public class GeradorMovimentoCompra: DefaultBL, IGeradorMovimentoCompra {
        
        //Atributos
        private IProdutoRedeConfiguracaoConsultaBL _ProdutoConfiguracao;
        private IRedeLinearConsultaBL _RedeConsulta;
        
        //Servicos
        private IProdutoRedeConfiguracaoConsultaBL ProdutoConfiguracao => _ProdutoConfiguracao = _ProdutoConfiguracao ?? new ProdutoRedeConfiguracaoConsultaBL();
        private IRedeLinearConsultaBL RedeConsulta => _RedeConsulta = _RedeConsulta ?? new RedeLinearConsultaBL();


        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno pagar(MovimentoResumoVW MovimentoResumo, MovimentoOperacaoDTO Transacao) {

            var listaMovimentos = this.criarMovimentos(MovimentoResumo, Transacao);

            var MovimentoOrigem = listaMovimentos.FirstOrDefault(x => x.flagMovimentoOrigem);
            
            var MovimentoDestino = listaMovimentos.FirstOrDefault(x => x.flagMovimentoDestino);
            
            var listaMovimentoRede = listaMovimentos.Where(x => !x.flagMovimentoDestino && !x.flagMovimentoOrigem).ToList();
            
            if (MovimentoOrigem == null) {
                
                return UtilRetorno.newInstance(true, "O movimento de origem não pôde ser carregado.");
            }

            if (MovimentoDestino == null) {
                
                return UtilRetorno.newInstance(true, "O movimento de destino não pôde ser carregado.");
                
            }
            
            using (var context = new DataContext()){
                
                using (var transaction = context.Database.BeginTransaction()){
                    
                    try{
                        
                        context.Movimento.Add(MovimentoOrigem);
                        context.SaveChanges();

                        MovimentoDestino.idMovimentoPrincipal = MovimentoOrigem.id;
                        MovimentoDestino.valorMovimentoPrincipal = MovimentoOrigem.valor;
                        context.Movimento.Add(MovimentoDestino);

                        if (listaMovimentoRede.Any()) {

                            listaMovimentoRede.ForEach(item => {
                                                           item.idMovimentoPrincipal = MovimentoOrigem.id;
                                                           item.idOrigem = MovimentoOrigem.idOrigem;
                                                           item.idMovimentoPrincipal = MovimentoOrigem.id;
                                                           item.valorMovimentoPrincipal = MovimentoOrigem.valor;
                                                       });

                            context.Movimento.AddRange(listaMovimentoRede);
                            
                        }

                        context.SaveChanges();
                        
                        transaction.Commit();
                        
                        listaMovimentos.Clear();
                        
                        listaMovimentos.Add(MovimentoOrigem);
                        
                        listaMovimentos.Add(MovimentoDestino);
                        
                        listaMovimentos.AddRange(listaMovimentoRede);
                        
                    }catch (Exception){
                        
                        transaction.Rollback();
                        
                        return UtilRetorno.newInstance(true, "Houve problemas ao tentar realizar a transação para salvar os movimentos.");
                    }
                }
            }
            
            return UtilRetorno.newInstance(false, "", listaMovimentos);
            
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Movimento> criarMovimentos(MovimentoResumoVW MovimentoResumo, MovimentoOperacaoDTO Transacao) {

            var TransacaoProdutoInterno = this.carregarDistribuicaoRede(Transacao);

            var MovimentoOrigem = gerarMovimentoOrigem(MovimentoResumo, Transacao);
            
            var MovimentoDestino = gerarMovimentoDestino(MovimentoResumo, Transacao, TransacaoProdutoInterno);

            var MovimentoRede = this.gerarMovimentosRede(MovimentoOrigem, TransacaoProdutoInterno);

            MovimentoDestino.percentualMovimentoPrincipal = decimal.Subtract(new decimal(100), MovimentoRede.carregarPercentualDistribuido());
            
            MovimentoDestino.valor = decimal.Subtract(Transacao.valorOperacao, MovimentoRede.carregarValorDistribuido());

            MovimentoDestino.observacao = $"Recebimento de {MovimentoDestino.percentualMovimentoPrincipal:N2} de {Transacao.valorOperacao:N2} produto SINCTEC";
 
            if (!Transacao.flagPagamentoComBitkink) {

                MovimentoOrigem.valor = new decimal(0);

                MovimentoOrigem.observacao = $"Compra de Produto SINCTEC sem usar BTK";
                
                MovimentoDestino.valor = new decimal(0);
                
                MovimentoDestino.observacao = $"Pagamento de Produto SINCTEC sem receber BTK";
            }

            MovimentoRede.listaMovimentoRede = MovimentoRede.listaMovimentoRede.Where(x => x.idMembroDestino > 0 && x.valor > 0)
                                                            .ToList();

            var listaItens = new List<Movimento>();

            listaItens.Add(MovimentoOrigem);
            
            listaItens.Add(MovimentoDestino);
            
            listaItens.AddRange(MovimentoRede.listaMovimentoRede);

            foreach (var item in listaItens) {

                if (item.flagMovimentoOrigem == true) {
                    
                    continue;
                }
                                   
                item.flagDebito = false;
                                   
                item.flagCredito = true;
                                   
                item.observacao = item.observacao.abreviar(100);

                item.setDefaultInsertValues();
            }

            return listaItens;
            
        }

        /// <summary>
        /// 
        /// </summary>
        private Movimento gerarMovimentoOrigem(MovimentoResumoVW MovimentoResumo, MovimentoOperacaoDTO Transacao) {

            var MovimentoOrigem = new Movimento();
            
            MovimentoOrigem.captarDadosOrigem(MovimentoResumo);
            
            MovimentoOrigem.idTipoTransacao = (byte) TipoTransacaoEnum.PRODUTOS_LINKEY;

            MovimentoOrigem.setDefaultInsertValues();

            MovimentoOrigem.flagMovimentoOrigem = true;
            
            return MovimentoOrigem;
        }

        /// <summary>
        /// 
        /// </summary>
        private Movimento gerarMovimentoDestino(MovimentoResumoVW MovimentoResumo, MovimentoOperacaoDTO Transacao, TransacaoProdutoInterno TransacaoProdutoInterno) {

            var MovimentoDestino = new Movimento();
            MovimentoDestino.captarDadosDestino(MovimentoResumo);
            MovimentoDestino.valor = TransacaoProdutoInterno.valorInterno;
            MovimentoDestino.observacao = $"{TransacaoProdutoInterno.percentualTotalInterno:N4}% de {Transacao.valorOperacao:N4}";
            MovimentoDestino.idTipoTransacao = (byte) TipoTransacaoEnum.PRODUTOS_LINKEY;
            MovimentoDestino.setDefaultInsertValues();
            MovimentoDestino.flagMovimentoDestino = true;

            return MovimentoDestino;
        }

        /// <summary>
        /// 
        /// </summary>
        private MovimentoRedeDTO gerarMovimentosRede(Movimento MovimentoOrigem, TransacaoProdutoInterno TransacaoProdutoInterno) {

            var Retorno = new MovimentoRedeDTO();

            Retorno.listaMovimentoRede = new List<Movimento>();

            if (TransacaoProdutoInterno.percentualTotalDistribuicao <= 0) {
                
                return Retorno;
            }

            var RedeLinearMembro = this.RedeConsulta.query(MovimentoOrigem.idMembroDestino.toInt()).FirstOrDefault();

            if (RedeLinearMembro == null) {
                
                return Retorno;
            }

            
            for (byte i = 1; i <= 15; i++) {

                var Comissao = TransacaoProdutoInterno.listaComissoes.FirstOrDefault(x => x.nivel == i);

                if (Comissao == null || Comissao.percentualComissao.toDecimal() <= 0) {
                    continue;
                }

                decimal valorComissao = TransacaoProdutoInterno.valorPagamento.valorPercentual(Comissao.percentualComissao);
                
                var idIndicadorObj = RedeLinearMembro.getValueByString($"idIndicador{i.ToString().PadLeft(2, '0')}");

                int idIndicador = idIndicadorObj.toInt();

                var MovimentoRede = new Movimento();
                MovimentoRede.idMembroOrigem = MovimentoOrigem.idMembroDestino;
                MovimentoRede.idPessoaOrigem = MovimentoOrigem.idPessoaDestino;
                MovimentoRede.idPessoaDestino = null;
                MovimentoRede.valor = valorComissao;
                MovimentoRede.observacao = $"Comissão de {Comissao.percentualComissao:N2}% em compra de produto SINCTEC de {TransacaoProdutoInterno.valorPagamento:N2}  no nível {i}";
                MovimentoRede.idTipoTransacao = (byte) TipoTransacaoEnum.PRODUTOS_LINKEY;
                MovimentoRede.percentualMovimentoPrincipal = Comissao.percentualComissao;
                MovimentoRede.setDefaultInsertValues();

                if (idIndicador > 0) {

                    MovimentoRede.idMembroDestino = idIndicador;
                    
                }
            
                Retorno.listaMovimentoRede.Add(MovimentoRede);
            }
            

            return Retorno;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private TransacaoProdutoInterno carregarDistribuicaoRede(MovimentoOperacaoDTO Transacao) {

            var Retorno = new TransacaoProdutoInterno();

            Retorno.valorDistribuicao = new decimal(0);
            
            Retorno.valorPagamento = Transacao.valorOperacao;
            
            Retorno.valorInterno = Transacao.valorOperacao;
            
            Retorno.percentualTotalDistribuicao = new decimal(0);

            Retorno.percentualTotalInterno = new decimal(100);
            
            if (Transacao.idProduto.toInt() == 0) {

                return Retorno;
            }

            var listaConfiguracoes = ProdutoConfiguracao.listar(Transacao.idProduto.toInt())
                                                        .Where(x => x.percentualComissao > 0 && x.idProduto > 0)
                                                        .Select(x => new { x.id, 
                                                                             x.idProduto,
                                                                             x.idUsuarioCadastro,
                                                                             x.nivel,
                                                                             x.percentualComissao
                                                                         })
                                                        .ToListJsonObject<ProdutoRedeConfiguracao>();

            Retorno.percentualTotalDistribuicao = listaConfiguracoes.Select(x => x.percentualComissao).DefaultIfEmpty(0).Sum();

            Retorno.percentualTotalInterno = decimal.Subtract(Retorno.percentualTotalInterno, Retorno.percentualTotalDistribuicao);

            Retorno.valorInterno = Retorno.valorInterno.valorPercentual(Retorno.percentualTotalInterno);

            Retorno.valorDistribuicao = decimal.Subtract(Transacao.valorOperacao, Retorno.valorInterno);

            Retorno.listaComissoes = listaConfiguracoes;
            

            return Retorno;
        }
    }

}

