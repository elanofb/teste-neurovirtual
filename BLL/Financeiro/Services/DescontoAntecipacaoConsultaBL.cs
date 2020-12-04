using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using DAL.Financeiro.Entities;

namespace BLL.Financeiro {

    public class DescontoAntecipacaoConsultaBL : DefaultBL, IDescontoAntecipacaoConsultaBL {

        //Atributos
        //Propriedades


        //Salvar os dados do cupom de desconto
        public IQueryable<TituloReceitaDescontoAntecipacao> listar(int idTituloReceita) {

            var query = from Item in db.TituloReceitaDescontoAntecipacao
                        where
                            Item.idTituloReceita == idTituloReceita &&
                            Item.dtExclusao == null
                        select Item;

            return query;
        }
        
        /// <summary>
        /// Carregar id e valor do descnto de antecipacao de acordo com os dado do pagamento
        /// </summary>
        public TituloReceitaPagamento carregarDescontoAntecipacao(TituloReceitaPagamento OTituloReceitaPagamento) {

            var listaDescontos = this.listar(OTituloReceitaPagamento.idTituloReceita)
                                     .Select(x => new {
                                                          x.id,
                                                          x.idTituloReceita,
                                                          x.dtLimiteDesconto,
                                                          x.valor,
                                                          x.dtExclusao
                                                      })
                                     .ToListJsonObject<TituloReceitaDescontoAntecipacao>();

					
            var ODescontoAntecipacao = listaDescontos.retornarDescontosAntecipacao(OTituloReceitaPagamento.dtPagamento.GetValueOrDefault())
                                                     .FirstOrDefault();

            if (ODescontoAntecipacao == null) {
                
                return OTituloReceitaPagamento;
                
            }

            OTituloReceitaPagamento.idDescontoAntecipacao = ODescontoAntecipacao.id;

            OTituloReceitaPagamento.valorDescontoAntecipacao = ODescontoAntecipacao.valor;
			
            return OTituloReceitaPagamento;
        }        
    }

}
