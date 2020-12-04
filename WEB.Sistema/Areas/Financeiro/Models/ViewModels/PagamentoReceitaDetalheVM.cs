using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class PagamentoReceitaDetalheVM {

        //Atributos
        private ITituloReceitaPagamentoBL _ContasAPagarPagamentoBL;

        //Propriedades
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _ContasAPagarPagamentoBL = _ContasAPagarPagamentoBL ?? new TituloReceitaPagamentoBL();
        
        //Atributos
        public TituloReceitaPagamento OPagamentoReceita { get; set; }
        public int idTituloReceitaPagamento { get; set; }
        public bool flagEdicao { get; set; }

        public PagamentoReceitaDetalheVM() {
            this.OPagamentoReceita = new TituloReceitaPagamento();
        }

        public void carregar() {

            var flagExcluidos = this.flagEdicao ? false : (bool?) null;
            
            var query = OTituloReceitaPagamentoBL.listar(0, flagExcluidos)
                .Where(x => x.id == this.idTituloReceitaPagamento);
            
            this.OPagamentoReceita = query.Select(x => new {
                    x.id,
                    x.descricaoParcela,
                    x.nroDocumento,
                    x.dtFinalizacaoCheckout,
                    x.tokenTransacao,
                    x.dtCadastro,
                    x.dtExclusao,
                    x.motivoExclusao,
                    x.dtPagamento,
                    x.nroParcela,
                    x.boletoCodigoBarras,
                    
                    x.valorRecebido,
                    x.valorDescontoCupom,
                    x.valorDesconto,
                    x.valorJuros,
                    x.valorDescontoAntecipacao,
                    x.valorOutrasTarifas,
                    x.valorTarifasBancarias,
                    x.valorTarifasTransacao,
                    x.valorOriginal,
                    
                    x.bairroRecibo,
                    x.cepRecibo,
                    x.complementoRecibo,
                    x.documentoRecibo,
                    x.logradouroRecibo,
                    x.nomeCidadeRecibo,
                    x.nomeRecibo,
                    x.numeroRecibo,
                    
                    x.dtVencimento,
                    x.dtPrevisaoPagamento,
                    x.dtCredito,
                    x.dtBaixa,
                    x.dtCompetencia,
                    
                    x.idContaBancaria,
                    ContaBancaria = new {
                        x.ContaBancaria.descricao,
                        x.ContaBancaria.digitoConta,
                        x.ContaBancaria.nroConta
                    },
                    
                    idEstadoRecibo = x.idEstadoRecibo > 0 ? x.idEstadoRecibo : x.CidadeRecibo.idEstado,
                    EstadoRecibo = new {
                        sigla = x.EstadoRecibo == null ? x.CidadeRecibo.Estado.sigla : x.EstadoRecibo.sigla,
                        nome = x.EstadoRecibo == null ? x.CidadeRecibo.Estado.nome : x.EstadoRecibo.nome
                    },
                    
                    x.idCidadeRecibo,
                    CidadeRecibo = new {
                        idEstado = x.CidadeRecibo != null ? x.CidadeRecibo.idEstado : 0,
                        Estado = new {
                            x.CidadeRecibo.Estado.sigla,
                            x.CidadeRecibo.Estado.nome
                        },
                        x.CidadeRecibo.nome
                    },
                    
                    TituloReceita = new {
                        x.TituloReceita.idPessoa,
                        x.TituloReceita.idReceita,
                        x.TituloReceita.idTipoReceita,
                        x.TituloReceita.documentoPessoa,
                        x.TituloReceita.nomePessoa,
                        x.TituloReceita.nroTelPrincipal,
                        TipoReceita = new {
                            x.TituloReceita.TipoReceita.descricao
                        }
                    },
                    
                    x.idMeioPagamento,
                    MeioPagamento = new {
                        x.MeioPagamento.descricao
                    },
                    
                    x.idFormaPagamento,
                    FormaPagamento = new {
                        x.FormaPagamento.descricao
                    },
                    
                    x.idCentroCusto,
                    CentroCusto = new {
                        x.CentroCusto.descricao
                    },
                    
                    x.idMacroConta,
                    MacroConta = new {
                        x.MacroConta.descricao
                    },
                    
                    x.idCategoria,
                    Categoria = new {
                        x.Categoria.descricao
                    },
                    
                    x.idUsuarioCadastro,
                    UsuarioCadastro = new {
                        x.UsuarioCadastro.nome
                    },
                    
                    x.idUsuarioExclusao,
                    UsuarioExclusao = new {
                        x.UsuarioExclusao.nome
                    }
                }).FirstOrDefault().ToJsonObject<TituloReceitaPagamento>() ?? new TituloReceitaPagamento();

        }
        
    }
}