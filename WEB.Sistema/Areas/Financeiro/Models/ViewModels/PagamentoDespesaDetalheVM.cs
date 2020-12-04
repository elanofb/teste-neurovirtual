using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class PagamentoDespesaDetalheVM {

        //Atributos
        private ITituloDespesaPagamentoBL _ContasAPagarPagamentoBL;

        //Propriedades
        private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _ContasAPagarPagamentoBL = _ContasAPagarPagamentoBL ?? new TituloDespesaPagamentoBL();
        
        //Atributos
        public TituloDespesaPagamento OPagamentoDespesa { get; set; }
        public int idTituloDespesaPagamento { get; set; }
        public bool flagEdicao { get; set; }

        public PagamentoDespesaDetalheVM() {
            this.OPagamentoDespesa = new TituloDespesaPagamento();
        }

        public void carregar() {

            var flagExcluidos = this.flagEdicao ? false : (bool?) null;
            
            var query = OTituloDespesaPagamentoBL.listar(0, flagExcluidos)
                .Where(x => x.id == this.idTituloDespesaPagamento);
            
            this.OPagamentoDespesa = query.Select(x => new {
                    x.id,
                    x.descParcela,
                    x.nroNotaFiscal,
                    x.nroContrato,
                    x.nroDocumento,
                    x.idContaBancariaFavorecida,
                    x.dtCadastro,
                    x.dtExclusao,
                    x.motivoExclusao,
                    x.dtPagamento,
                    
                    x.valorPago,
                    x.valorMulta,
                    x.valorDesconto,
                    x.valorJuros,
                    x.valorOriginal,
                    
                    x.dtVencimento,
                    x.dtPrevisaoPagamento,
                    x.dtDebito,
                    x.dtBaixa,
                    x.dtCompetencia,
                    
                    ContaBancariaFavorecida = new {
                        Banco = new {
                            x.ContaBancariaFavorecida.Banco.nome
                        },
                        x.ContaBancariaFavorecida.digitoConta,
                        x.ContaBancariaFavorecida.nroConta
                    },
                    
                    TituloDespesa = new {
                        x.TituloDespesa.idPessoa,
                        x.TituloDespesa.idDespesa,
                        x.TituloDespesa.idTipoDespesa,
                        x.TituloDespesa.documentoPessoaCredor,
                        x.TituloDespesa.nomePessoaCredor,
                        x.TituloDespesa.nroTelPrincipalCredor,
                        TipoDespesa = new {
                            x.TituloDespesa.TipoDespesa.descricao
                        }
                    },
                    x.idModoPagamento,
                    ModoPagamento = new {
                        x.ModoPagamento.descricao,
                        x.ModoPagamento.flagContaBancaria
                    },
                    x.idContaBancaria,
                    ContaBancaria = new {
                        x.ContaBancaria.descricao
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
                }).FirstOrDefault().ToJsonObject<TituloDespesaPagamento>() ?? new TituloDespesaPagamento();

        }
        
    }
}